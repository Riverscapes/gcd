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

        protected readonly List<Raster> _rasters;
        protected readonly List<T> _rasternodatavals;
        public ExtentRectangle OpExtent;
        public ExtentRectangle InExtent;
        protected int _oprows;
        protected T OpNodataVal;

        protected Raster _outputRaster;
        protected int _vOffset;

        public event EventHandler<int> ProgressEvent;

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
            _rasters = new List<Raster>(rRasters.Count);
            _rasternodatavals = new List<T>(rRasters.Count);
            _oprows = 10;
            _init(rRasters, rOutputRaster);
            _vOffset = 0;

            // Now that we have our rasters tested and a unioned extent we can set the operation extent
            SetOpExtent(InExtent);

        }

        protected void AddInputRaster(Raster rInput)
        {
            if (_rasters.Count > 1)
                Raster.ValidateSameMeta(_rasters[0], rInput);

            _rasters.Add(rInput);
            _rasternodatavals.Add(rInput.NodataValue<T>());

            InExtent = InExtent.Union(rInput.Extent);
            rInput.Open();
        }

        /// <summary>
        /// Just a simple init function to put all the pieces we want in place
        /// </summary>
        /// <param name="rOutRaster"></param>
        /// <param name="newExt"></param>
        private void _init(List<Raster> rRasters, Raster rOutRaster)
        {
            OpDone = false;

            // Use the first input for the nodataval
            if (rRasters.Count > 0)
            {
                InExtent = rRasters[0].Extent;
                // Do a union on the inputextent
                foreach (Raster rRa in rRasters)
                    AddInputRaster(rRa);

                if (typeof(T) == typeof(float))
                    OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<float>(), typeof(T));
                else if (typeof(T) == typeof(double))
                    OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<double>(), typeof(T));
                else if (typeof(T) == typeof(int))
                    OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<int>(), typeof(T));
                else if (typeof(T) == typeof(byte))
                    OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<byte>(), typeof(T));
            }
            // No inputs? then get the nodataval from the output
            else if (rOutRaster != null)
            {
                InExtent = rOutRaster.Extent;
                if (typeof(T) == typeof(float))
                    OpNodataVal = (T)Convert.ChangeType(rOutRaster.NodataValue<float>(), typeof(T));
                else if (typeof(T) == typeof(double))
                    OpNodataVal = (T)Convert.ChangeType(rOutRaster.NodataValue<double>(), typeof(T));
                else if (typeof(T) == typeof(int))
                    OpNodataVal = (T)Convert.ChangeType(rOutRaster.NodataValue<int>(), typeof(T));
                else if (typeof(T) == typeof(byte))
                    OpNodataVal = (T)Convert.ChangeType(rOutRaster.NodataValue<byte>(), typeof(T));
            }
            // No inputs or outputs? (is this possible?) Just use the min value
            else
            {
                OpNodataVal = Utility.Conversion.minValue<T>();
            }

            // Finally, set up our output raster and make sure it's open for writing
            if (rOutRaster != null)
                _outputRaster = rOutRaster;
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
            int chunkYsize = _oprows;

            if (OpExtent.Rows < chunkYsize) chunkYsize = OpExtent.Rows;
            ChunkExtent = new ExtentRectangle(OpExtent.Top, OpExtent.Left, OpExtent.CellHeight, OpExtent.CellWidth, chunkYsize, chunkXsize);

            if (_outputRaster != null)
                _outputRaster.Extent = OpExtent;
        }

        /// <summary>
        /// Advance the chunk rectangle to the next chunk
        /// </summary>
        /// <returns></returns>
        public void nextChunk()
        {
            // If the top of the chunk is lower than the bottom of the operational extent then we're done
            if ((ChunkExtent.Top * ChunkExtent.CellHeight) < (OpExtent.Bottom * ChunkExtent.CellHeight))
            {
                // Advance the chunk
                ChunkExtent.Top = ChunkExtent.Top + (ChunkExtent.Rows * ChunkExtent.CellHeight);

                // If we've fallen off the bottom of the intended extent then we need to shorten the chunk
                if ((ChunkExtent.Bottom * ChunkExtent.CellHeight) > (OpExtent.Bottom * ChunkExtent.CellHeight))
                {
                    ChunkExtent.Rows = (int)((ChunkExtent.Top - OpExtent.Bottom) / ChunkExtent.CellHeight);
                    if (ChunkExtent.Rows <= 0)
                        OpDone = true;
                }
            }
            else
                OpDone = true;

            Debug.WriteLine(string.Format("Operation: {0}%", Progress));
        }

        /// <summary>
        /// Get a number of chunks from the actual rasters
        /// NOTE: for now a chunk goes across the whole extent to make the math easier
        /// </summary>
        /// <returns></returns>
        public void GetChunk(List<T[]> data)
        {
            for (int idx = 0; idx < _rasters.Count; idx++)
            {
                Raster rRa = _rasters[idx];

                // Reset everything first so we don't get any data bleed
                // NOTE: if this is slow we can revisit
                data[idx].Fill(OpNodataVal);

                ExtentRectangle _interSectRect = rRa.Extent.Intersect(ChunkExtent);

                // Make sure there's some data to read, otherwise return the filled nodata values from above
                if (_interSectRect.Rows > 0 && _interSectRect.Cols > 0)
                {
                    T[] _buffer = new T[_interSectRect.Rows * _interSectRect.Cols];

                    // Find the offset between the intersection and rRa
                    int[] offrRa = _interSectRect.GetTopCornerTranslationRowCol(rRa.Extent);
                    _rasters[idx].Read(offrRa[1], offrRa[0], _interSectRect.Cols, _interSectRect.Rows, _buffer);

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
        /// Run an operation over every cell individually
        /// </summary>
        public void Run()
        {
            List<T[]> data = new List<T[]>(_rasters.Count);

            ProgressEvent?.Invoke(this, 0);

            // Set up an array with nodatavals to be populated (or not)
            for (int idx = 0; idx < _rasters.Count; idx++)
                data.Add(new T[ChunkExtent.Cols * ChunkExtent.Rows]);

            T[] outBuffer = new T[ChunkExtent.Cols * ChunkExtent.Rows];
            while (!OpDone)
            {
                GetChunk(data);
                ProgressEvent?.Invoke(this, Progress);
                ChunkOp(data, outBuffer);

                if (_outputRaster != null)
                {
                    // Get the (col,row) offsets
                    int[] offset = ChunkExtent.GetTopCornerTranslationRowCol(OpExtent);
                    // Write this window tot he file
                    // it goes (colNum, rowNum, COLS, ROWS, buffer);
                    _outputRaster.Write(offset[1], offset[0] + _vOffset, ChunkExtent.Cols, ChunkExtent.Rows, outBuffer);
                }
                // We always increment to the next one
                nextChunk();
            }
            ProgressEvent?.Invoke(this, 100);

            Cleanup();

        }

        public Raster RunWithOutput()
        {
            Run();
            return _outputRaster;
        }

        /// <summary>
        /// The three types of operations need to implement this
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outChunk"></param>
        protected abstract void ChunkOp(List<T[]> data, T[] outChunk);

        /// <summary>
        /// Make sure this class leaves nothing behind and builds statistics before disappearing forever
        /// </summary>
        protected void Cleanup()
        {
            foreach (Raster rRa in _rasters)
            {
                if (rRa.IsOpen)
                    rRa.Dispose();
            }
            if (_outputRaster != null)
            {
                _outputRaster.ComputeStatistics();
                _outputRaster.Dispose();
            }
        }
    }



}
