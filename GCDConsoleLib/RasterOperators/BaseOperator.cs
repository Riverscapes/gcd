using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;

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
        protected T OpNodataVal;

        protected Raster _outputRaster;

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
            _init(rRasters, rOutputRaster);
            // Now that we have our rasters tested and a unioned extent we can set the operation extent
            SetOpExtent(InExtent);
        }

        /// <summary>
        /// Just a simple init function to put all the pieces we want in place
        /// </summary>
        /// <param name="rOutRaster"></param>
        /// <param name="newExt"></param>
        private void _init(List<Raster> rRasters, Raster rOutRaster)
        {
            foreach (Raster rRa in rRasters)
            {
                _rasters.Add(rRa);
                _rasternodatavals.Add(rRa.NodataValue<T>());
            }

            OpDone = false;
            Raster r0 = _rasters[0];
            InExtent = r0.Extent;

            if (typeof(T) == typeof(float))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<float>(), typeof(T));
            else if (typeof(T) == typeof(double))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<double>(), typeof(T));
            else if (typeof(T) == typeof(int))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<int>(), typeof(T));
            else if (typeof(T) == typeof(byte))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<byte>(), typeof(T));

            // Validate our each raster, Add each raster to the union extent window and open it for business
            foreach (Raster rN in _rasters)
            {
                Raster rR = rN;
                Raster.ValidateSameMeta(r0, rR);
                InExtent = InExtent.Union(rN.Extent);
                rN.Open();
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
            int chunkXsize = OpExtent.cols;
            int chunkYsize = 10;

            if (OpExtent.rows < chunkYsize) chunkYsize = OpExtent.rows;
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
                ChunkExtent.Top = ChunkExtent.Top + (ChunkExtent.rows * ChunkExtent.CellHeight);

                // If we've fallen off the bottom of the intended extent then we need to shorten the chunk
                if ((ChunkExtent.Bottom * ChunkExtent.CellHeight) > (OpExtent.Bottom * ChunkExtent.CellHeight))
                {
                    ChunkExtent.rows = (int)((ChunkExtent.Top - OpExtent.Bottom) / ChunkExtent.CellHeight);
                    if (ChunkExtent.rows <= 0)
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
            for (int idx = 0; idx < _rasters.Count; idx++)
            {
                Raster rRa = _rasters[idx];

                // Reset everything first so we don't get any data bleed
                // NOTE: if this is slow we can revisit
                data[idx].Fill(OpNodataVal);

                ExtentRectangle _interSectRect = rRa.Extent.Intersect(ChunkExtent);

                // Make sure there's some data to read, otherwise return the filled nodata values from above
                if (_interSectRect.rows > 0 && _interSectRect.cols > 0)
                {
                    T[] _buffer = new T[_interSectRect.rows * _interSectRect.cols];

                    // Find the offset between the intersection and rRa
                    Tuple<int, int> offrRa = _interSectRect.GetTopCornerTranslationRowCol(rRa.Extent);
                    _rasters[idx].Read(offrRa.Item2, offrRa.Item1, _interSectRect.cols, _interSectRect.rows, _buffer);

                    // Find the offset between the intersection and the chunkwindow
                    Tuple<int, int> offChunk = _interSectRect.GetTopCornerTranslationRowCol(ChunkExtent);
                    data[idx].Plunk(_buffer,
                        ChunkExtent.rows, ChunkExtent.cols,
                        _interSectRect.rows, _interSectRect.cols,
                        offChunk.Item1, offChunk.Item2);
                }
            }
        }

        /// <summary>
        /// Run an operation over every cell individually
        /// </summary>
        public void Run(int vOffset = 0)
        {
            List<T[]> data = new List<T[]>(_rasters.Count);

            // Set up an array with nodatavals to be populated (or not)
            for (int idx = 0; idx < _rasters.Count; idx++)
                data.Add(new T[ChunkExtent.cols * ChunkExtent.rows]);

            T[] outBuffer = new T[ChunkExtent.cols * ChunkExtent.rows];
            while (!OpDone)
            {
                GetChunk(data);
                ChunkOp(data, outBuffer);

                if (_outputRaster != null)
                {             
                    // Get the (col,row) offsets
                    Tuple<int, int> offset = ChunkExtent.GetTopCornerTranslationRowCol(OpExtent);
                    // Write this window tot he file
                    // it goes (colNum, rowNum, COLS, ROWS, buffer);
                    _outputRaster.Write(offset.Item2, offset.Item1 + vOffset, ChunkExtent.cols, ChunkExtent.rows, outBuffer);
                }
                // We always increment to the next one
                nextChunk();
            }
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
                if (!rRa.IsOpen)
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
