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
        protected readonly List<Raster> _rasters;
        protected ExtentRectangle _opExtent;
        protected ExtentRectangle _chunkWindow;
        protected T OpNodataVal;
        protected Boolean bDone;

        private Raster _outputRaster;
        private List<RasterInternals<T>> _rasterguts;
        private string _outputrasterpath;
        private RasterInternals<T> _outputRasterGuts;

        /// <summary>
        /// Initialize a bunch of rasters
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="newRect"></param>
        protected BaseOperator(List<Raster> rRasters, Raster rOutputRaster)
        {
            _rasters = new List<Raster>(rRasters.Count);
            _rasterguts = new List<RasterInternals<T>>(rRasters.Count);
            foreach (Raster rRa in rRasters)
            {
                _rasters.Add(rRa);
                _rasterguts.Add(rRa.RasterGuts as RasterInternals<T>);
            }
            _init(rOutputRaster);

        }

        /// <summary>
        /// Just a simple init function to put all the pieces we want in place
        /// </summary>
        /// <param name="rOutRaster"></param>
        /// <param name="newExt"></param>
        private void _init(Raster rOutRaster)
        {
            bDone = false;
            Raster r0 = _rasters[0];
            ExtentRectangle tmpRect = r0.Extent;
            OpNodataVal = _rasterguts[0].NodataVal;

            // Validate our each raster, Add each raster to the union extent window and open it for business
            foreach (Raster rN in _rasters)
            {
                Raster rR = rN;
                Raster.ValidateSameMeta(ref r0, ref rR);
                tmpRect = r0.Extent.Union(ref rN.Extent);
                rN.Open();
            }

            // Now that we have our rasters tested and a unioned extent we can set the operation extent
            SetOpExtent(tmpRect);

            // Finally, set up our output raster and make sure it's open for writing
            _outputRaster = rOutRaster;
            _outputRaster.Extent = _opExtent;
            _outputRasterGuts = _outputRaster.RasterGuts as RasterInternals<T>;
            // Open our output for writing
            _outputRaster.Open(true);

        }

        /// <summary>
        /// Sometimes we want to enforce the extent even though it calculates automatically.
        /// </summary>
        /// <param name="newRect"></param>
        protected void SetOpExtent(ExtentRectangle newRect)
        {
            _opExtent = newRect;

            // Now initialize our window rectangle
            int chunkXsize = _opExtent.cols;
            int chunkYsize = 10;

            if (_opExtent.rows < chunkYsize) chunkYsize = _opExtent.rows;
            _chunkWindow = new ExtentRectangle(_opExtent.Top, _opExtent.Left, _opExtent.CellHeight, _opExtent.CellWidth, chunkYsize, chunkXsize);

        }

        /// <summary>
        /// Advance the chunk rectangle to the next chunk
        /// </summary>
        /// <returns></returns>
        public void nextChunk()
        {
            if (_chunkWindow.Top < _opExtent.Bottom)
            {
                // Advance the chunk
                _chunkWindow.Top = _chunkWindow.Top + (_chunkWindow.rows * _chunkWindow.CellHeight);

                // If we've fallen off the bottom of the intended extent then we need to shorten the chunk
                if (_chunkWindow.Bottom < _opExtent.Bottom)
                {
                    _chunkWindow.rows = (int)((_chunkWindow.Top - _opExtent.Bottom) / _chunkWindow.CellHeight);
                }
                bDone = false;
            }
            else
                bDone = true;
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

                // Set up an array with nodatavals to be populated (or not)
                T[] inputchunk = new T[_chunkWindow.cols * _chunkWindow.rows];
                inputchunk.Fill(OpNodataVal);

                // Make sure there's some data to read, otherwise return the filled nodata values from above
                ExtentRectangle _interrect = _chunkWindow.Intersect(ref rRa.Extent);
                if (_interrect.rows > 0 && _interrect.cols > 0)
                {
                    T[] readChunk = new T[_chunkWindow.rows * _chunkWindow.cols];

                    // Get the (col,row) offsets
                    Tuple<int, int> offset = _interrect.GetTopCornerTranslation(ref _chunkWindow);
                    _rasterguts[idx].Read(offset.Item2, offset.Item1, _interrect.cols, _interrect.rows, ref readChunk);
                    inputchunk.Plunk(ref readChunk, _chunkWindow.cols, _chunkWindow.rows, _interrect.cols, _interrect.rows, offset.Item2, offset.Item1);
                }

                data.Add(inputchunk);
            }
            // We always increment to the next one
            nextChunk();
        }

        /// <summary>
        /// Run an operation over every cell individually
        /// </summary>
        public Raster Run()
        {
            List<T[]> data = new List<T[]>(_rasters.Count);
            while (!bDone)
            {
                GetChunk(ref data);
                T[] outChunk = new T[_chunkWindow.rows * _chunkWindow.cols];
                ChunkOp(ref data, ref outChunk);
                // Get the (col,row) offsets
                Tuple<int, int> offset = _chunkWindow.GetTopCornerTranslation(ref _opExtent);
                // Write this window tot he file
                _outputRasterGuts.Write(offset.Item2, offset.Item1, _chunkWindow.cols, _chunkWindow.rows, ref outChunk);
            }
            Cleanup();
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
            _outputRaster.ComputeStatistics();
            _outputRaster.Dispose();
        }
    }



}
