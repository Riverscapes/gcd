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
        public ExtentRectangle ChunkWindow;
        public Boolean OpDone;

        protected readonly List<Raster> _rasters;
        protected readonly List<T> _rasternodatavals;
        public ExtentRectangle OpExtent;
        public ExtentRectangle InExtent;
        protected T OpNodataVal;

        private Raster _outputRaster;

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

            if (typeof(T) == typeof(Single))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<Single>(), typeof(T));
            else if (typeof(T) == typeof(Double))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<Double>(), typeof(T));
            else if (typeof(T) == typeof(int))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<int>(), typeof(T));
            else if (typeof(T) == typeof(Byte))
                OpNodataVal = (T)Convert.ChangeType(_rasters[0].NodataValue<Byte>(), typeof(T));

            // Validate our each raster, Add each raster to the union extent window and open it for business
            foreach (Raster rN in _rasters)
            {
                Raster rR = rN;
                Raster.ValidateSameMeta(ref r0, ref rR);
                InExtent = InExtent.Union(ref rN.Extent);
                rN.Open();
            }

            // Finally, set up our output raster and make sure it's open for writing
            if (rOutRaster == null)
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
            ChunkWindow = new ExtentRectangle(OpExtent.Top, OpExtent.Left, OpExtent.CellHeight, OpExtent.CellWidth, chunkYsize, chunkXsize);

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
            if ((ChunkWindow.Top * ChunkWindow.CellHeight) < (OpExtent.Bottom * ChunkWindow.CellHeight))
            {
                // Advance the chunk
                ChunkWindow.Top = ChunkWindow.Top + (ChunkWindow.rows * ChunkWindow.CellHeight);

                // If we've fallen off the bottom of the intended extent then we need to shorten the chunk
                if ((ChunkWindow.Bottom * ChunkWindow.CellHeight) > (OpExtent.Bottom * ChunkWindow.CellHeight))
                {
                    ChunkWindow.rows = (int)((ChunkWindow.Top - OpExtent.Bottom) / ChunkWindow.CellHeight);
                    if (ChunkWindow.rows <= 0)
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
        public void GetChunk(ref List<T[]> data)
        {
            for (int idx = 0; idx < _rasters.Count; idx++)
            {
                Raster rRa = _rasters[idx];

                // Reset everything first so we don't get any data bleed
                // NOTE: if this is slow we can revisit
                data[idx].Fill(OpNodataVal);

                ExtentRectangle _interSectRect = rRa.Extent.Intersect(ref ChunkWindow);

                // Make sure there's some data to read, otherwise return the filled nodata values from above
                if (_interSectRect.rows > 0 && _interSectRect.cols > 0)
                {
                    T[] _buffer = new T[_interSectRect.rows * _interSectRect.cols];

                    // Find the offset between the intersection and rRa
                    Tuple<int, int> offrRa = _interSectRect.GetTopCornerTranslation(ref rRa.Extent);
                    _rasters[idx].Read(offrRa.Item1, offrRa.Item2, _interSectRect.cols, _interSectRect.rows, ref _buffer);

                    // Find the offset between the intersection and the chunkwindow
                    Tuple<int, int> offChunk = _interSectRect.GetTopCornerTranslation(ref ChunkWindow);
                    data[idx].Plunk(ref _buffer,
                        ChunkWindow.rows, ChunkWindow.cols,
                        _interSectRect.rows, _interSectRect.cols,
                        offChunk.Item2, offChunk.Item1);
                }
            }
        }

        /// <summary>
        /// Run an operation over every cell individually
        /// </summary>
        public void Run()
        {
            List<T[]> data = new List<T[]>(_rasters.Count);

            // Set up an array with nodatavals to be populated (or not)
            for (int idx = 0; idx < _rasters.Count; idx++)
                data.Add(new T[ChunkWindow.cols * ChunkWindow.rows]);

            T[] _buffer = new T[ChunkWindow.cols * ChunkWindow.rows];
            while (!OpDone)
            {
                GetChunk(ref data);
                ChunkOp(ref data, ref _buffer);

                if (_outputRaster != null)
                {             
                    // Get the (col,row) offsets
                    Tuple<int, int> offset = ChunkWindow.GetTopCornerTranslation(ref OpExtent);
                    // Write this window tot he file
                    _outputRaster.Write(offset.Item1, offset.Item2, ChunkWindow.cols, ChunkWindow.rows, ref _buffer);
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
        protected abstract void ChunkOp(ref List<T[]> data, ref T[] outChunk);

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
