using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib.Internal.Operators
{
    class BilinearResample<T> : BaseOperator<T>
    {
        double fy;
        double fx;

        /// <summary>
        /// Pass-through constructor for Extended Copy
        /// </summary>
        public BilinearResample(Raster rInput, ExtentRectangle newRect, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            fy = (double)(rInput.Extent.CellHeight / newRect.CellHeight);
            fx = (double)(rInput.Extent.CellWidth / newRect.CellWidth);

            int newRows = Convert.ToInt32(Math.Ceiling((double)(rInput.Extent.rows * fy)));
            int newCols = Convert.ToInt32(Math.Ceiling((double)(rInput.Extent.cols * fx)));

            SetOpExtent(newRect);
        }

        /// <summary>
        /// We need a way to go back and forth between
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public static int translateCoord(int num, double factor, int size)
        {
            double f = num / factor;
            int i1 = (int)(Math.Floor(f));

            // Special case where point is on upper bounds
            if (f == size - 1) i1 -= 1;

            return i1;
        }

        /// <summary>
        ///  We need to override the Run method because the input and output scales are different
        ///  
        /// </summary>
        public new void Run(int vOffset = 0)
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
                            outBuffer[ncol] = (
                                Dynamics.Multiply(vals[0], (ix2 - fx)) * (iy2 - fy)
                                + Dynamics.Multiply(vals[1], (fx - ix1)) * (iy2 - fy)
                                + Dynamics.Multiply(vals[2], (ix2 - fx)) * (fy - iy1)
                                + Dynamics.Multiply(vals[3], (fx - ix1)) * (fy - iy1)) 
                                / ((ix2 - ix1) * (iy2 - iy1));
                        }
                    }
                }
                _outputRaster.Write(0, nrow, ChunkExtent.cols, 1, outBuffer);
            }
            Cleanup();
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
