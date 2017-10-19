using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;

namespace GCDConsoleLib
{
    public abstract class BaseOperator
    {
        protected readonly List<Raster> _rasters;
        protected ExtentRectangle _ExtentIterWindow;
        protected ExtentRectangle _chunkWindow;
        private Raster _outputRaster;

        protected double _nodataval;
        private string _outputrasterpath;

        protected abstract double CellOp(ref List<double[]> data, int id);
        protected abstract double[] ChunkOp(ref List<double[]> data);

        /// <summary>
        /// Simple case for initializing a single raster
        /// </summary>
        /// <param name="rRaster"></param>
        protected BaseOperator(ref Raster rRaster, ref Raster rOutputRaster, ExtentRectangle newRect = null)
        {
            _rasters = new List<Raster> { rRaster };
            _init(rOutputRaster, newRect);
        }

        /// <summary>
        /// Initialize two rasters (A and B)
        /// </summary>
        /// <param name="rRasterA"></param>
        /// <param name="rRasterB"></param>
        protected BaseOperator(ref Raster rRasterA, ref Raster rRasterB, ref Raster rOutputRaster, ExtentRectangle newRect = null)
        {
            _rasters = new List<Raster> { rRasterA, rRasterB };
            _init(rOutputRaster, newRect);
        }

        /// <summary>
        /// Initialize a bunch of rasters
        /// </summary>
        /// <param name="rRasters"></param>
        protected BaseOperator(List<Raster> rRasters, ref Raster rOutputRaster, ExtentRectangle newRect = null)
        {
            _rasters = new List<Raster>(rRasters.Count);
            foreach (Raster rRa in rRasters)
            {
                _rasters.Add(rRa);
            }
            _init(rOutputRaster, newRect);
        }

        /// <summary>
        /// Here's where we choose our chunk size. We'll keep it simple for now.
        /// </summary>
        private void _init(Raster rOutRaster, ExtentRectangle newExt = null)
        {
            _nodataval = _rasters[0].NodataVal;

            Raster r = _rasters[0];

            if (newExt != null)
            {
                _ExtentIterWindow = newExt;
            }
            else
            {
                _ExtentIterWindow = r.Extent;
                // Add each raster to the union extent window
                foreach (Raster rRa in _rasters)
                {
                    _ExtentIterWindow = r.Extent.Union(ref rRa.Extent);
                }
            }

            // Now make sure our rasters are open for business
            foreach (Raster rRa in _rasters)
            {
                rRa.Open();
            }

            // Now initialize our window rectangle
            int chunkXsize = 1000;
            int chunkYsize = 10;
            if (_ExtentIterWindow.cols < chunkXsize) chunkXsize = _ExtentIterWindow.cols;
            if (_ExtentIterWindow.rows < chunkYsize) chunkYsize = _ExtentIterWindow.rows;
            _chunkWindow = new ExtentRectangle(_ExtentIterWindow.Top, _ExtentIterWindow.Left, _ExtentIterWindow.CellHeight, _ExtentIterWindow.CellWidth, chunkYsize, chunkXsize);

            _validateInputs();
            // Finally set us up soemthing to write to
            Raster firstRaster = _rasters[0];
            // Now that we have our rasters tested and a unioned extent

            _outputRaster = rOutRaster;
            _outputRaster.Extent = _ExtentIterWindow;
            // Open our output for writing
            _outputRaster.Open(true);
        }


        /// <summary>
        /// We're just going to scream if any of our inputs are in the wrong format
        /// </summary>
        private void _validateInputs()
        {
            Raster rRef = _rasters[0];
            foreach (Raster rTest in _rasters)
            {
                if (_ExtentIterWindow.CellHeight != rTest.Extent.CellHeight)
                {
                    throw new NotSupportedException("Cellheights do not match");
                }
                if (_ExtentIterWindow.CellWidth != rTest.Extent.CellWidth)
                {
                    throw new NotSupportedException("Cellwidths do not match");
                }
                if (!_ExtentIterWindow.IsDivisible() || !rTest.Extent.IsDivisible())
                {
                    throw new NotSupportedException("Both raster extents must be divisible");
                }
                if (!rRef.Proj.IsSame(ref rTest.Proj))
                {
                    throw new NotSupportedException("Raster Projections do not match match");
                }
                if (rRef.VerticalUnits != rTest.VerticalUnits)
                {
                    throw new NotSupportedException(String.Format("Both rasters must have the same vertical units: `{0}` vs. `{1}`", rRef.VerticalUnits, rTest.VerticalUnits));
                }
            }
        }

        /// <summary>
        /// Advance the chunk rectangle to the next chunk
        /// </summary>
        /// <returns></returns>
        protected bool nextChunk()
        {
            bool bDone = false;
            if (_chunkWindow.Top < _ExtentIterWindow.Bottom)
            {
                // Advance the chunk
                _chunkWindow.Top = _chunkWindow.Top + (_chunkWindow.rows * _chunkWindow.CellHeight);

                // If we've fallen off the bottom of the intended extent then we need to shorten the chunk
                if (_chunkWindow.Bottom < _ExtentIterWindow.Bottom)
                {
                    _chunkWindow.rows = (int)((_chunkWindow.Top - _ExtentIterWindow.Bottom) / _chunkWindow.CellHeight);
                }
                bDone = false;
            }
            else
            {
                bDone = true;
            }
            return bDone;
        }


        /// <summary>
        /// Get a number of chunks from the actual rasters
        /// NOTE: for now a chunk goes across the whole extent to make the math easier
        /// </summary>
        /// <returns></returns>
        protected List<double[]> GetChunk()
        {
            double nodata = _rasters[0].NodataVal;
            List<double[]> data = new List<double[]>(_rasters.Count);
            for (int idx = 0; idx < _rasters.Count; idx++)
            {
                Raster rRa = _rasters[idx];

                // Set up an array with nodatavals to be populated (or not)
                double[] inputchunk = new double[_chunkWindow.cols * _chunkWindow.rows];
                inputchunk.Fill(nodata);

                // Make sure there's some data to read
                ExtentRectangle _interrect = _chunkWindow.Intersect(ref rRa.Extent);
                if (_interrect.rows > 0 && _interrect.cols > 0)
                {
                    double[] readChunk = new double[_chunkWindow.rows * _chunkWindow.cols];

                    // Get the (col,row) offsets
                    Tuple<int, int> offset = GetRowColTranslation(ref _interrect, ref _chunkWindow);

                    rRa.Read(offset.Item2, offset.Item1, _interrect.cols, _interrect.rows, ref readChunk);

                    inputchunk.Plunk(ref readChunk, _chunkWindow.cols, _chunkWindow.rows, _interrect.cols, _interrect.rows, offset.Item2, offset.Item1);
                }

                data.Add(inputchunk);
            }
            return data;
        }

        /// <summary>
        /// Run an operation over every cell individually
        /// </summary>
        protected Raster RunCellByCellOp()
        {
            List<double[]> data;
            bool bDone = false;
            while (!bDone)
            {
                data = GetChunk();
                double[] outChunk = new double[_chunkWindow.rows * _chunkWindow.cols];
                for (int id = 0; id < data[0].Length; id++)
                {
                    outChunk[id] = CellOp(ref data, id);
                }

                // Get the (col,row) offsets
                Tuple<int, int> offset = GetRowColTranslation(ref _chunkWindow, ref _ExtentIterWindow);

                // Write this window tot he file
                _outputRaster.Write(offset.Item2, offset.Item1, _chunkWindow.cols, _chunkWindow.rows, ref outChunk);
                bDone = nextChunk();
            }
            _outputRaster.ComputeStatistics();
            Cleanup();
            return _outputRaster;
        }

        /// <summary>
        /// This is the operation to run a whole chunk into a function
        /// </summary>
        /// <returns></returns>
        protected Raster RunChunkOp()
        {
            List<double[]> data;
            bool bDone = false;
            while (!bDone)
            {
                data = GetChunk();
                double[] outChunk = new double[_chunkWindow.cols * _chunkWindow.rows];
                outChunk = ChunkOp(ref data);

                // Get the (col,row) offsets
                Tuple<int, int> offset = GetRowColTranslation(ref _chunkWindow, ref _ExtentIterWindow);

                // Write this window tot he file
                _outputRaster.Write(offset.Item2, offset.Item1, _chunkWindow.cols, _chunkWindow.rows, ref outChunk);
                bDone = nextChunk();
            }
            _outputRaster.ComputeStatistics();
            Cleanup();
            return _outputRaster;
        }

        /// <summary>
        /// This is the operation to run a whole chunk into a function
        /// </summary>
        /// <returns></returns>
        protected Raster RunOverlapWindowOp(int window = 1)
        {
            List<double[]> data;
            bool bDone = false;
            while (!bDone)
            {
                data = GetChunk();
                double[] outChunk = new double[_chunkWindow.cols * _chunkWindow.rows];
                outChunk = ChunkOp(ref data);

                // Get the (col,row) offsets
                Tuple<int, int> offset = GetRowColTranslation(ref _chunkWindow, ref _ExtentIterWindow);

                // Write this window tot he file
                _outputRaster.Write(offset.Item2, offset.Item1, _chunkWindow.cols, _chunkWindow.rows, ref outChunk);
                bDone = nextChunk();
            }
            _outputRaster.ComputeStatistics();
            Cleanup();
            return _outputRaster;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rExtent1"></param>
        /// <param name="rExtent2"></param>
        /// <returns>(colT, rowT)</returns>
        protected static Tuple<int, int> GetRowColTranslation(ref ExtentRectangle rExtent1, ref ExtentRectangle rExtent2)
        {
            int colT = (int)((rExtent1.Left - rExtent2.Left) / rExtent1.CellWidth);
            int rowT = (int)((rExtent1.Top - rExtent2.Top) / rExtent1.CellHeight);
            return new Tuple<int, int>(colT, rowT);
        }

        /// <summary>
        /// Make sure this class leaves nothing behind
        /// </summary>
        private void Cleanup()
        {
            foreach (Raster rRa in _rasters)
            {
                if (!rRa.IsOpen)
                {
                    rRa.Dispose();
                }
            }
            _outputRaster.Dispose();
        }
    }
}
