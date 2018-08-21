using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;
using GCDConsoleLib.Utility;
using System.Diagnostics;

namespace GCDConsoleLib.Internal.Operators
{
    class BilinearResample<T> : BaseOperator<T>
    {
        decimal fy; // old cell height  / new cellheight
        decimal fx; // old cell width / new cellwidth

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

            SetOpExtent(newRect);
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
            StateChange(OpStatus.States.Started);

            int oldRows = _inputRasters[0].Extent.Rows;
            int oldCols = _inputRasters[0].Extent.Cols;
            double dInNodata = (double)Convert.ChangeType(inNodataVals[0], typeof(double));
            T[] outBuffer = new T[OpExtent.Cols];
            List<double[]> indata = new List<double[]>() { new double[_inputRasters[0].Extent.Cols], new double[_inputRasters[0].Extent.Cols] };

            // Pre-populate our input data with nodatavals
            foreach (double[] arr in indata)
                arr.Fill(dInNodata);

            // This is a two-line cycling buffer. Either array el 1 or array el 0 is the first row.
            int topBit = 0;
            int bottomBit = 0;
            int topBitRow = 0;

            double oldCW = (double)_inputRasters[0].Extent.CellWidth;
            double oldCH = (double)_inputRasters[0].Extent.CellHeight;

            // Now we loop over the output space
            for (int nrow = 0; nrow < OpExtent.Rows; nrow++)
            {
                int progress = (int)((double)nrow / OpExtent.Rows * 100);
                ProgressChange(progress);

                outBuffer.Fill(outNodataVals[0]);
                for (int ncol = 0; ncol < OpExtent.Cols; ncol++)
                {
                    int ix1 = translateCoord(ncol, fy, OpExtent.Cols); // This gives us the old COL
                    int iy1 = translateCoord(nrow, fx, OpExtent.Rows); // This gives us the old ROW

                    // Increment both by 1 to get 4 coords we need ix2, iy2 are in the old COL and ROW space
                    int ix2 = ix1 + 1;
                    int iy2 = iy1 + 1;

                    // Advance the cache if we need to
                    if (iy2 > topBitRow && iy2 < oldRows)
                    {
                        topBit = (topBit + 1) % 2;
                        bottomBit = (topBit + 1) % 2;
                        topBitRow += 1;
                        _inputRasters[0].Read(0, iy2, _inputRasters[0].Extent.Cols, 1, indata[topBit]);
                    }

                    // Test if we're within the raster midpoints
                    if ((ix1 >= 0) && (iy1 >= 0) && (ix2 < oldCols) && (iy2 < oldRows))
                    {
                        // get the 4 values we need (these correspond to Z01, Z11, Z00 and Z10 in the old system
                        double Z01 = indata[topBit][ix1];
                        double Z11 = indata[topBit][ix2];
                        double Z00 = indata[bottomBit][ix1];
                        double Z10 = indata[bottomBit][ix2];

                        // Bail if anything is NODATAVAL
                        if (Z01 != dInNodata  && Z11 != dInNodata  && Z00 != dInNodata && Z10 != dInNodata)
                        {
                            // Finally, here's the resample:
                            // The multi[le casts here is unfortunate but since we don't know what kind of thing we're
                            // dealing with before we load it, this needs to happen.
                            double Z1 = Z01 + (Z11 - Z01) * ((ncol - ix1) * oldCW);
                            double Z0 = Z00 + (Z10 - Z00) * ((ncol - ix1) * oldCW);

                            double Z = Z1 - (Z1 - Z0) * ((nrow - iy1) * oldCH);

                            outBuffer[ncol] = (T)Convert.ChangeType(Z, typeof(T));
 
                        }
                    }
                }
                _outputRasters[0].Write(0, nrow, ChunkExtent.Cols, 1, outBuffer);
            }
            Cleanup();
            StateChange(OpStatus.States.Complete);
        }

        /// <summary>
        /// This ensures we use the right Run()
        /// </summary>
        /// <returns></returns>
        public new Raster RunWithOutput()
        {
            Run();
            return _outputRasters[0];
        }

        /// <summary>
        /// We don't need this. Going to implement our own thing
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outChunk"></param>
        protected override void ChunkOp(List<T[]> data, List<T[]> outChunk)
        {
            throw new NotImplementedException();
        }
    }
}
