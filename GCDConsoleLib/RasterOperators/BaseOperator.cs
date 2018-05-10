using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;
using System.Diagnostics;

namespace GCDConsoleLib.Internal
{
    /// <summary>
    /// 
    /// This is the very base class for raster operation.IN GENERAL YOU SHOULD NOT USE THIS DIRECTLY
    /// 
    /// If you want to implement a new operator you should inherit a subclass of this one.
    /// 
    /// See:
    ///   * CellByCellOperator.cs
    ///   * WindowOverlapOperator.cs
    ///   
    /// </summary>
    public abstract class BaseOperator<T>
    {
        public ExtentRectangle ChunkExtent;
        public Boolean OpDone;

        protected readonly List<Raster> _inputRasters;
        protected readonly List<Raster> _outputRasters;

        public readonly List<T> inNodataVals;
        public readonly List<T> outNodataVals;

        public ExtentRectangle OpExtent;
        public ExtentRectangle InExtent;
        protected int chunkRows;

        protected int _vOffset;

        public event EventHandler<int> ProgressEvent;
        private int lastReportedProgress;

        /// <summary>
        /// Report back an integer between 0 and 100
        /// </summary>
        public int Progress
        {
            get
            {
                return (int)(100 * (ChunkExtent.Bottom - OpExtent.Top) / (OpExtent.Bottom - OpExtent.Top));
            }
        }

        /// <summary>
        /// Initialize a bunch of rasters
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="newRect"></param>
        protected BaseOperator(List<Raster> rRasters, Raster rOutputRaster = null)
        {
            _inputRasters = new List<Raster>();
            _outputRasters = new List<Raster>();
            inNodataVals = new List<T>();
            outNodataVals = new List<T>();
            lastReportedProgress = -1;

            List<Raster> tempOut = new List<Raster>();
            if (rOutputRaster != null)
                tempOut.Add(rOutputRaster);

            _init(rRasters, tempOut);

            // Now that we have our rasters tested and a unioned extent we can set the operation extent
            SetOpExtent(InExtent);
        }


        /// <summary>
        /// Initialize a bunch of rasters with multiple outputs
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="newRect"></param>
        protected BaseOperator(List<Raster> rRasters, List<Raster> rOutputRasters = null)
        {
            _inputRasters = new List<Raster>();
            _outputRasters = new List<Raster>();
            inNodataVals = new List<T>();
            outNodataVals = new List<T>();
            lastReportedProgress = -1;


            if (rOutputRasters == null)
                rOutputRasters = new List<Raster>();

            _init(rRasters, rOutputRasters);

            // Now that we have our rasters tested and a unioned extent we can set the operation extent
            SetOpExtent(InExtent);
        }

        /// <summary>
        /// Sometimes we want to add an input raster after the constructor has been called
        /// </summary>
        /// <param name="rInput"></param>
        protected void AddInputRaster(Raster rInput)
        {
            if (_inputRasters.Count > 1)
                Raster.ValidateSameMeta(_inputRasters[0], rInput, true);

            _inputRasters.Add(rInput);
            inNodataVals.Add(rInput.NodataValue<T>());

            InExtent = InExtent.Union(rInput.Extent);
            rInput.Open();
        }

        /// <summary>
        /// Sometimes we want to add an output raster after the constructor has been called
        /// </summary>
        /// <param name="rOutput"></param>
        protected void AddOutputRaster(Raster rOutput)
        {
            if (_outputRasters.Count > 1)
                Raster.ValidateSameMeta(_outputRasters[0], rOutput);

            _outputRasters.Add(rOutput);
            outNodataVals.Add(rOutput.NodataValue<T>());

            InExtent = InExtent.Union(rOutput.Extent);
        }

        /// <summary>
        /// Just a simple init function to put all the pieces we want in place
        /// </summary>
        /// <param name="rOutRaster"></param>
        /// <param name="newExt"></param>
        private void _init(List<Raster> rRasters, List<Raster> rOutputRasters)
        {
            chunkRows = 10;
            _vOffset = 0;

            OpDone = false;

            if (rRasters.Count > 0)
            {
                InExtent = rRasters[0].Extent;

                // Do a union on the inputextent
                foreach (Raster rRa in rRasters)
                    AddInputRaster(rRa);
            }
            else if (rOutputRasters.Count > 0)
                InExtent = rOutputRasters[0].Extent;

            if (rOutputRasters.Count > 0)
                // Do a union on the inputextent
                foreach (Raster rRout in rOutputRasters)
                    AddOutputRaster(rRout);
            else
                // Sometimes we need an output nodataval even when we don't have an output raster
                // (think FIS for error rasters
                outNodataVals.Add(rRasters[0].NodataValue<T>());

            // Last thing we do is set the nodata value (which is suprisingly hard to do)
            SetNodataValue();

        }

        private void SetNodataValue()
        {
            T OpNodataVal = Utility.Conversion.minValue<T>();

            // Use the first input for the nodataval
            if (_inputRasters.Count > 0)
            {
                try
                {
                    if (_inputRasters[0].HasNodata)
                    {
                        T val = (T)Convert.ChangeType(_inputRasters[0].origNodataVal, typeof(T));
                        // Double is the biggest value we can have so use that to see if these
                        // values are really the same
                        double dValConverted = (double)Convert.ChangeType(val, typeof(double));
                        if ((double)_inputRasters[0].origNodataVal != dValConverted)
                            throw new OverflowException("No good");
                        OpNodataVal = val;
                    }
                }
                catch (OverflowException) { }
            }

            // Now make sure our nodataval is compatible with the Output value
            for (int dId = 0; dId < _outputRasters.Count; dId++)
            {
                try
                {
                    var test = Convert.ChangeType(OpNodataVal, _outputRasters[dId].Datatype.CSType);
                    T throwaway = (T)Convert.ChangeType(test, typeof(T));
                    if (!throwaway.Equals(OpNodataVal))
                        throw new OverflowException("No good");
                }
                catch (OverflowException)
                {
                    Type outType = _outputRasters[dId].Datatype.CSType;
                    if (outType == typeof(int))
                        OpNodataVal = (T)Convert.ChangeType(Utility.Conversion.minValue<int>(), typeof(T));
                    else if (outType == typeof(double))
                        OpNodataVal = (T)Convert.ChangeType(Utility.Conversion.minValue<double>(), typeof(T));
                    else if (outType == typeof(float))
                        OpNodataVal = (T)Convert.ChangeType(Utility.Conversion.minValue<float>(), typeof(T));
                    else if (outType == typeof(byte))
                        OpNodataVal = (T)Convert.ChangeType(Utility.Conversion.minValue<byte>(), typeof(T));
                }
                // Finally set the value in the output raster so it will be written correctly 
                _outputRasters[dId].origNodataVal = (double?)Convert.ChangeType(OpNodataVal, typeof(double));
                outNodataVals[dId] = OpNodataVal;
            }
        }

        /// <summary>
        /// Sometimes we want to enforce the extent even though it calculates automatically.
        /// </summary>
        /// <param name="newRect"></param>
        protected void SetOpExtent(ExtentRectangle newRect)
        {
            OpExtent = newRect;

            // Now initialize our window rectangle
            int chunkXsize = OpExtent.Cols;
            int chunkYsize = chunkRows;

            if (OpExtent.Rows < chunkYsize) chunkYsize = OpExtent.Rows;
            ChunkExtent = new ExtentRectangle(OpExtent.Top, OpExtent.Left, OpExtent.CellHeight, OpExtent.CellWidth, chunkYsize, chunkXsize);

            foreach(Raster outraster in _outputRasters)
                outraster.Extent = OpExtent;
        }

        /// <summary>
        /// Advance the chunk rectangle to the next chunk
        /// </summary>
        /// <returns></returns>
        public void nextChunk()
        {
            // If the top of the chunk is lower than the bottom of the operational extent then we're done
            if ((ChunkExtent.Top / ChunkExtent.CellHeight) < (OpExtent.Bottom / ChunkExtent.CellHeight))
            {
                // Advance the chunk
                ChunkExtent.Top = ChunkExtent.Top + (ChunkExtent.Rows * ChunkExtent.CellHeight);

                // If we've fallen off the bottom of the intended extent then we need to shorten the chunk
                if ((ChunkExtent.Bottom / ChunkExtent.CellHeight) > (OpExtent.Bottom / ChunkExtent.CellHeight))
                {
                    ChunkExtent.Rows = (int)((OpExtent.Bottom - ChunkExtent.Top) / ChunkExtent.CellHeight);
                    if (ChunkExtent.Rows <= 0)
                        OpDone = true;
                }
            }
            else
                OpDone = true;
        }

        /// <summary>
        /// Get a number of chunks from the actual rasters
        /// NOTE: for now a chunk goes across the whole extent to make the math easier
        /// </summary>
        /// <returns></returns>
        public void GetChunk(List<T[]> data)
        {
            for (int idx = 0; idx < _inputRasters.Count; idx++)
            {
                Raster rRa = _inputRasters[idx];

                // Reset everything first so we don't get any data bleed
                // NOTE: if this is slow we can revisit
                data[idx].Fill(inNodataVals[idx]);

                ExtentRectangle _interSectRect = rRa.Extent.Intersect(ChunkExtent);

                // Make sure there's some data to read, otherwise return the filled nodata values from above
                if (_interSectRect.Rows > 0 && _interSectRect.Cols > 0)
                {
                    T[] _buffer = new T[_interSectRect.Rows * _interSectRect.Cols];

                    // Find the offset between the intersection and rRa
                    int[] offrRa = _interSectRect.GetTopCornerTranslationRowCol(rRa.Extent);
                    _inputRasters[idx].Read(offrRa[1], offrRa[0], _interSectRect.Cols, _interSectRect.Rows, _buffer);

                    // Find the offset between the intersection and the chunkwindow
                    int[] offChunk = _interSectRect.GetTopCornerTranslationRowCol(ChunkExtent);
                    data[idx].Plunk(_buffer,
                        ChunkExtent.Rows, ChunkExtent.Cols,
                        _interSectRect.Rows, _interSectRect.Cols,
                        offChunk[0], offChunk[1]);
                }
            }
        }

        /// <summary>
        /// We need a little shim to handle the ProgressEvent?.Invoke()
        /// </summary>
        /// <param name="prog"></param>
        protected void ProgressInvoke(int prog)
        {
            if (prog != lastReportedProgress)
            {
                Debug.WriteLine(string.Format("Operation: {0}%", prog));
                ProgressEvent?.Invoke(this, prog);
            }
            lastReportedProgress = prog;
        }

        /// <summary>
        /// Run an operation over every cell individually
        /// </summary>
        public void Run()
        {
            List<T[]> data = new List<T[]>(_inputRasters.Count);

            ProgressInvoke(0);

            // Set up an array with nodatavals to be populated (or not)
            for (int idx = 0; idx < _inputRasters.Count; idx++)
                data.Add(new T[ChunkExtent.Cols * ChunkExtent.Rows]);

            List<T[]> outBuffer = new List<T[]>();
            foreach (Raster outraster in _outputRasters)
                outBuffer.Add(new T[ChunkExtent.Cols * ChunkExtent.Rows]);

            while (!OpDone)
            {
                GetChunk(data);
                ProgressInvoke(Progress);
                ChunkOp(data, outBuffer);

                for (int idx = 0; idx < _outputRasters.Count; idx++)
                {
                    // Get the (col,row) offsets
                    int[] offset = ChunkExtent.GetTopCornerTranslationRowCol(OpExtent);
                    // Write this window tot he file
                    // it goes (colNum, rowNum, COLS, ROWS, buffer);
                    _outputRasters[idx].Write(offset[1], offset[0] + _vOffset, ChunkExtent.Cols, ChunkExtent.Rows, outBuffer[idx]);
                    
                }
                // We always increment to the next one
                nextChunk();
            }
            ProgressInvoke(100);

            Cleanup();

        }

        public Raster RunWithOutput()
        {
            Run();
            return _outputRasters[0];
        }

        public List<Raster> RunWithOutputs()
        {
            Run();
            return _outputRasters;
        }

        /// <summary>
        /// The three types of operations need to implement this
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outChunk"></param>
        protected abstract void ChunkOp(List<T[]> data, List<T[]> outBuffers);

        /// <summary>
        /// Make sure this class leaves nothing behind and builds statistics before disappearing forever
        /// </summary>
        protected void Cleanup()
        {
            foreach (Raster rRa in _inputRasters)
            {
                if (rRa.IsOpen)
                    rRa.UnloadDS();
            }
            foreach (Raster rRout in _outputRasters)
            {
                if (rRout.IsOpen)
                    rRout.UnloadDS();
            }
        }
    }



}
