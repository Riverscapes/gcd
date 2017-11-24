using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib.Internal.Operators
{
    class BilinearResample<T> : BaseOperator<T>
    {
        decimal fy;
        decimal fx;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="newRect"></param>
        /// <param name="rOutputRaster"></param>
        public BilinearResample(Raster rInput, ExtentRectangle newRect, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            fy = rInput.Extent.CellHeight / newRect.CellHeight;
            fx = rInput.Extent.CellWidth / newRect.CellWidth;

            int newRows = Convert.ToInt32(Math.Ceiling(rInput.Extent.rows * fy));
            int newCols = Convert.ToInt32(Math.Ceiling(rInput.Extent.cols * fx));

            ExtentRectangle newOpRect = rInput.Extent;
            newOpRect.Left = newRect.Left;
            newOpRect.Top = newRect.Top;

            newOpRect.cols = Convert.ToInt32((newRect.Top - newRect.Bottom) / rInput.Extent.Width);
            newOpRect.rows = Convert.ToInt32((newRect.Left - newRect.Right) / rInput.Extent.CellHeight);

            SetOpExtent(newOpRect);
        }

        /// <summary>
        /// We need a way to go back and forth between new cooords and old ones
        /// </summary>
        /// <param name="num"></param>
        /// <param name="factor"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int translateCoord(int num, decimal factor, int size)
        {
            decimal f = num / factor;
            int i1 = (int)(Math.Floor(f));

            // Special case where point is on upper bounds
            if (f == size - 1) i1 -= 1;

            return i1;
        }

        /// <summary>
        ///  We need to override the Run() method because the input and output scales are different
        /// </summary>
        public new void Run()
        {
            int oldCols = _rasters[0].Extent.cols;
            int oldRows = _rasters[0].Extent.rows;

            T[] outBuffer = new T[OpExtent.cols];
            List<T[]> indata = new List<T[]>() { new T[_rasters[0].Extent.cols], new T[_rasters[0].Extent.cols] };

            // This is a two-line cycling buffer. Either array el 1 or array el 0 is the first row.
            int topBit = 0;

            int topBitRow = 0;

            for (int nrow = 0; nrow < OpExtent.rows; nrow++)
            {
                outBuffer.Fill(OpNodataVal);
                for (int ncol = 0; ncol < OpExtent.cols; ncol++)
                {

                    int ix1 = translateCoord(ncol, fy, OpExtent.cols);
                    int iy1 = translateCoord(nrow, fx, OpExtent.rows);

                    // Increment both by 1 to get 4 coords we need
                    int ix2 = ix1 + 1;
                    int iy2 = iy1 + 1;

                    // Advance the cache if we need to
                    if (ix2 > topBitRow)
                    {
                        topBit = (topBit + 1) % 2;
                        topBitRow += 1;
                        _rasters[0].Read(0, iy2, _rasters[0].Extent.cols, 1, indata[topBit]);
                    }

                    // Test if we're within the raster midpoints
                    if ((ix1 >= 0) && (iy1 >= 0) && (ix2 < oldCols) && (iy2 < oldRows))
                    {
                        // get the 4 values we need
                        T[] vals = new T[4] {
                            indata[topBit][ix1],
                            indata[(topBit + 1) % 2][ix1],
                            indata[topBit][ix2],
                            indata[(topBit + 1)%2 ][ix2]
                        };
                        // Bail if anything is NODATAVAL
                        if (!vals[0].Equals(_rasternodatavals[0])
                            && !vals[1].Equals(_rasternodatavals[0])
                            && !vals[2].Equals(_rasternodatavals[0])
                            && !vals[3].Equals(_rasternodatavals[0]))
                        {
                            // Finally, here's the resample:
                            // The conversion here is unfortunate but since we don't know what kind of thing we're
                            // dealing with before we load it, this needs to happen.
                            outBuffer[ncol] = (
                                DynamicMath.Multiply(vals[0], (T)Convert.ChangeType((ix2 - fx) * (iy2 - fy), typeof(T)))
                                + DynamicMath.Multiply(vals[1], (T)Convert.ChangeType((fx - ix1) * (iy2 - fy), typeof(T)))
                                + DynamicMath.Multiply(vals[2], (T)Convert.ChangeType((ix2 - fx) * (fy - iy1), typeof(T)))
                                + DynamicMath.Multiply(vals[3], (T)Convert.ChangeType((fx - ix1) * (fy - iy1), typeof(T)))
                                / ((ix2 - ix1) * (iy2 - iy1)));
                        }
                    }
                }
                _outputRaster.Write(0, nrow, ChunkExtent.cols, 1, outBuffer);
            }
            Cleanup();
        }

        /// <summary>
        /// This ensures we use the right Run()
        /// </summary>
        /// <returns></returns>
        public new Raster RunWithOutput()
        {
            Run();
            return _outputRaster;
        }

        /// <summary>
        /// We don't need this. Going to implement our own thing
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outChunk"></param>
        protected override void ChunkOp(List<T[]> data, T[] outChunk)
        {
            throw new NotImplementedException();
        }
    }
}
