using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;

namespace GCDConsoleLib
{
    public abstract class BaseOperator
    {
        protected readonly List<Raster> _rasters;
        protected ExtentRectangle _opExtent;
        protected ExtentRectangle _chunkWindow;
        private Raster _outputRaster;

        protected double OpNoDataVal { get { return _rasters[0].NodataVal; } }
        private string _outputrasterpath;
        protected Boolean bDone;

        /// <summary>
        /// Initialize a bunch of rasters
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="newRect"></param>
        protected BaseOperator(List<Raster> rRasters, ref Raster rOutputRaster)
        {
            _rasters = new List<Raster>(rRasters.Count);
            foreach (Raster rRa in rRasters)
            {
                _rasters.Add(rRa);
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
            Raster r = _rasters[0];
            ExtentRectangle tmpRect = r.Extent;
            // Add each raster to the union extent window
            foreach (Raster rRa in _rasters) { tmpRect = r.Extent.Union(ref rRa.Extent); }

            // Now make sure our rasters are open for business
            foreach (Raster rRa in _rasters) { rRa.Open(); }

            // Make sure all our rasters follow the rules
            _validateInputs();

            // Now that we have our rasters tested and a unioned extent
            SetOpExtent(tmpRect);

            _outputRaster = rOutRaster;
            _outputRaster.Extent = _opExtent;
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
        /// We're just going to scream if any of our inputs are in the wrong format
        /// </summary>
        private void _validateInputs()
        {
            Raster rRef = _rasters[0];
            foreach (Raster rTest in _rasters)
            {
                if (_opExtent.CellHeight != rTest.Extent.CellHeight)
                    throw new NotSupportedException("Cellheights do not match");
                if (_opExtent.CellWidth != rTest.Extent.CellWidth)
                    throw new NotSupportedException("Cellwidths do not match");
                if (!_opExtent.IsDivisible() || !rTest.Extent.IsDivisible())
                    throw new NotSupportedException("Both raster extents must be divisible");
                if (!rRef.Proj.IsSame(ref rTest.Proj))
                    throw new NotSupportedException("Raster Projections do not match match");
                if (rRef.VerticalUnits != rTest.VerticalUnits)
                    throw new NotSupportedException(String.Format("Both rasters must have the same vertical units: `{0}` vs. `{1}`", rRef.VerticalUnits, rTest.VerticalUnits));
            }
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
        public void GetChunk(ref List<double[]> data)
        {
            for (int idx = 0; idx < _rasters.Count; idx++)
            {
                Raster rRa = _rasters[idx];

                // Set up an array with nodatavals to be populated (or not)
                double[] inputchunk = new double[_chunkWindow.cols * _chunkWindow.rows];
                inputchunk.Fill(OpNoDataVal);

                // Make sure there's some data to read, otherwise return the filled nodata values from above
                ExtentRectangle _interrect = _chunkWindow.Intersect(ref rRa.Extent);
                if (_interrect.rows > 0 && _interrect.cols > 0)
                {
                    double[] readChunk = new double[_chunkWindow.rows * _chunkWindow.cols];

                    // Get the (col,row) offsets
                    Tuple<int, int> offset = _interrect.GetTopCornerTranslation(ref _chunkWindow);
                    rRa.Read(offset.Item2, offset.Item1, _interrect.cols, _interrect.rows, ref readChunk);
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
        protected Raster Run()
        {
            List<double[]> data = new List<double[]>(_rasters.Count);
            while (!bDone)
            {
                GetChunk(ref data);
                double[] outChunk = new double[_chunkWindow.rows * _chunkWindow.cols];
                ChunkOp(ref data, ref outChunk);
                // Get the (col,row) offsets
                Tuple<int, int> offset = _chunkWindow.GetTopCornerTranslation(ref _opExtent);
                // Write this window tot he file
                _outputRaster.Write(offset.Item2, offset.Item1, _chunkWindow.cols, _chunkWindow.rows, ref outChunk);
            }
            Cleanup();
            return _outputRaster;
        }

        /// <summary>
        /// The three types of operations need to implement this
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outChunk"></param>
        protected abstract void ChunkOp(ref List<double[]> data, ref double[] outChunk);

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

    public abstract class CellByCellOperator : BaseOperator
    {
        public CellByCellOperator(List<Raster> rRasters, ref Raster rOutputRaster) :
            base(rRasters, ref rOutputRaster)
        {
        }

        protected abstract double CellOp(ref List<double[]> data, int id);

        protected override void ChunkOp(ref List<double[]> data, ref double[] outChunk)
        {
            for (int id = 0; id < data[0].Length; id++)
            {
                outChunk[id] = CellOp(ref data, id);
            }
        }
    }

    /// <summary>
    /// The overlap operator works by operating over a moving window
    /// </summary>
    public abstract class WindowOverlapOperator : BaseOperator
    {
        private int _bufferCells;
        private int BufferLength { get { return 1 + (2 * _bufferCells); } }
        private int BufferCellNum { get { return (int)Math.Pow(BufferLength, 2); } }
        private List<List<double[]>> _chunkCache;
        private int _cacheIdx;
        public WindowOverlapOperator(List<Raster> rRasters, ref Raster rOutputRaster, int bufferCells) :
            base(rRasters, ref rOutputRaster)
        {
            _bufferCells = bufferCells;
            _chunkCache = new List<List<double[]>>(BufferLength);
        }

        protected abstract double CellOp(ref List<double[]> data, int id);

        /// <summary>
        /// First time through we need to setup the buffer properly
        /// </summary>
        /// <param name="data"></param>
        private void firstTime(ref List<double[]> data)
        {   
            // Fill the rows above the extent with nodatavals
            double[] inputchunk = new double[_chunkWindow.cols * _chunkWindow.rows];
            inputchunk.Fill(OpNoDataVal);
            for (int idx = 0; idx < _bufferCells; idx++)
                _chunkCache.Add(null);

            // starting cache id is always in the middle
            _cacheIdx = _bufferCells;
            _chunkCache.Add(data);
            // Now try to fill the end of the window
            while (_chunkCache.Count < BufferLength)
            {
                GetChunk(ref data);
                _chunkCache.Add(data);
            }
            // Put our index right in the middle
            _cacheIdx = _bufferCells;
        }

        private void IdxIncrement() {_cacheIdx = (_cacheIdx + 1) % (BufferLength); }

        protected override void ChunkOp(ref List<double[]> data, ref double[] outChunk)
        {
            /**
             *  0  1  2
             *  3  4  5
             *  6  7  8
             */
            if (_chunkCache.Count == 0)
                firstTime(ref data);
            else
            {
                IdxIncrement();
                _chunkCache[_cacheIdx] = data;
            }
            double[] dWindow = new double[BufferCellNum];

            for (int id = 0; id < data[0].Length; id++)
            {
                for (int wId = 0; wId < BufferCellNum; wId++)
                {
                    int col = wId % BufferLength;
                    int row = (wId - (wId % BufferLength)) / BufferLength;
                    if (id < col || (id)
                }
                outChunk[id] = CellOp(ref data, id);
            }
        }
    }

}
