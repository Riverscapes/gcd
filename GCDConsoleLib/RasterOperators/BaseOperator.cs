﻿using System;
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

        // The op description gets used in the status message to tell us what
        // we're doing here. Example: "Creating Error Raster"
        private string _opDesc;
        public string OpDescription
        {
            get { return _opDesc; }
            set
            {
                _opDesc = value;
                MsgChange(_opDesc);
            }
        }

        public event EventHandler<OpStatus> ProgressEvent;

        protected readonly List<Raster> _inputRasters;
        protected readonly List<Raster> _outputRasters;

#if DEBUG
        protected Stopwatch tmr_overall;
        protected Stopwatch tmr_rasterread;
        protected Stopwatch tmr_rasterwrite;
        protected Stopwatch tmr_calculations;
        protected int num_reads;
        protected int num_writes;
        protected int num_calcs;
#endif

        public readonly List<T> inNodataVals;
        public readonly List<T> outNodataVals;

        public ExtentRectangle OpExtent;
        public ExtentRectangle InExtent;
        protected int chunkRows;

        protected int _vOffset;

        protected OpStatus _opStatus;
        protected Stopwatch _lastStatusInvoke;

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
            _opStatus = new OpStatus();

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
            _opStatus = new OpStatus();


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

            _lastStatusInvoke = Stopwatch.StartNew();

#if DEBUG
            tmr_overall = Stopwatch.StartNew();
            tmr_rasterread = new Stopwatch();
            tmr_rasterwrite = new Stopwatch();
            tmr_calculations = new Stopwatch();
            num_reads = 0;
            num_writes = 0;
            num_calcs = 0;
#endif

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

            StateChange(OpStatus.States.Initialized);
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

            foreach (Raster outraster in _outputRasters)
                outraster.Extent = OpExtent;
        }

        /// <summary>
        /// Advance the chunk rectangle to the next chunk
        /// </summary>
        /// <returns></returns>
        public void NextChunk()
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
#if DEBUG
                    tmr_rasterread.Start();
                    num_reads++;
#endif
                    _inputRasters[idx].Read(offrRa[1], offrRa[0], _interSectRect.Cols, _interSectRect.Rows, _buffer);
#if DEBUG
                    tmr_rasterread.Stop();
#endif

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
        protected void ProgressChange(int newProgress)
        {
            // Send status if it changes or if it's been more than a second since the last one.
            if (newProgress != _opStatus.Progress)
            {
                _opStatus.Progress = newProgress;
                EventInvoke();
            }
        }
        protected void StateChange(OpStatus.States newState)
        {
            if (newState != _opStatus.State)
            {
                _opStatus.State = newState;
                EventInvoke(true);
            }
        }
        protected void MsgChange(string newMsg)
        {
            if (newMsg != _opStatus.Message)
            {
                _opStatus.Message = newMsg;
                EventInvoke(true);
            }
        }

        public void AddProgressEvent(EventHandler<OpStatus> progressHandler)
        {
            if (progressHandler != null)
                ProgressEvent += progressHandler;
        }

        private void EventInvoke(bool force = false)
        {
            // Keep the noise down by capping the number of events we can get per second
            if (force || _lastStatusInvoke.ElapsedMilliseconds >= 300)
            {
                ProgressEvent?.Invoke(this, _opStatus);
                Debug.WriteLine(string.Format("BaseOperator OpStatus Event:: [{0}][{1}][{2}%]",
                    _opStatus.Message,
                    Enum.GetName(typeof(OpStatus.States), _opStatus.State),
                    _opStatus.Progress
                    ));
                _lastStatusInvoke.Restart();
            }
        }

        /// <summary>
        /// Run an operation over every cell individually
        /// </summary>
        public void Run()
        {
            List<T[]> data = new List<T[]>(_inputRasters.Count);
            StateChange(OpStatus.States.Started);

            // Set up an array with nodatavals to be populated (or not)
            for (int idx = 0; idx < _inputRasters.Count; idx++)
                data.Add(new T[ChunkExtent.Cols * ChunkExtent.Rows]);

            List<T[]> outBuffer = new List<T[]>();
            foreach (Raster outraster in _outputRasters)
                outBuffer.Add(new T[ChunkExtent.Cols * ChunkExtent.Rows]);

            while (!OpDone)
            {
                GetChunk(data);
                ProgressChange(Progress);
#if DEBUG
                tmr_calculations.Start();
                num_writes++;
#endif
                ChunkOp(data, outBuffer);
#if DEBUG
                tmr_calculations.Stop();
#endif

                for (int idx = 0; idx < _outputRasters.Count; idx++)
                {
                    // Get the (col,row) offsets
                    int[] offset = ChunkExtent.GetTopCornerTranslationRowCol(OpExtent);
                    // Write this window tot he file
                    // it goes (colNum, rowNum, COLS, ROWS, buffer);
#if DEBUG
                    tmr_rasterwrite.Start();
#endif
                    _outputRasters[idx].Write(offset[1], offset[0] + _vOffset, ChunkExtent.Cols, ChunkExtent.Rows, outBuffer[idx]);
#if DEBUG
                    tmr_rasterwrite.Stop();
#endif              
                }
                // We always increment to the next one
                NextChunk();
            }
            ProgressChange(100);
            StateChange(OpStatus.States.Complete);

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
#if DEBUG
            tmr_overall.Stop();
            Debug.WriteLine("---------------------------OP TIMER---------------------------");
            Debug.WriteLine(String.Format("Total Reads: {0}", num_reads));
            Debug.WriteLine(String.Format("Total Writes: {0}", num_writes));
            Debug.WriteLine(String.Format("Total Calculations: {0}", num_calcs));
            Debug.WriteLine("");
            Debug.WriteLine(String.Format("Raster Reading: {0}:{1}:{2}", tmr_rasterread.Elapsed.Minutes, tmr_rasterread.Elapsed.Seconds, tmr_rasterread.Elapsed.Milliseconds));
            Debug.WriteLine(String.Format("Raster Writing: {0}:{1}:{2}", tmr_rasterwrite.Elapsed.Minutes, tmr_rasterwrite.Elapsed.Seconds, tmr_rasterwrite.Elapsed.Milliseconds));
            Debug.WriteLine(String.Format("Calculations: {0}:{1}:{2}", tmr_calculations.Elapsed.Minutes, tmr_calculations.Elapsed.Seconds, tmr_calculations.Elapsed.Milliseconds));
            Debug.WriteLine(String.Format("Total Time: {0}:{1}:{2}", tmr_overall.Elapsed.Minutes, tmr_overall.Elapsed.Seconds, tmr_overall.Elapsed.Milliseconds));
            Debug.WriteLine("--------------------------------------------------------------");

#endif
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
