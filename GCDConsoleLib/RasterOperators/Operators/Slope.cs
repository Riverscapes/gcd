using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// Slope is a window-overlap function with a window size of (1) == 3x3 = 9 cells
    /// </summary>
    class Slope : WindowOverlapOperator<double>
    {
        double dzdx, dzdy, dzxy, riseRun, cellWidth, cellHeight, retval, theSlope;
        double[] _buff;
        public enum SlopeType : byte { Percent, Degrees };
        private SlopeType _slopetype;

        /// <summary>
        /// Pass-through constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="theType"></param>
        public Slope(Raster rInput, Raster rOutputRaster, SlopeType theType) :
            base(new List<Raster> { rInput }, 1, rOutputRaster)
        {
            _slopetype = theType;
            cellWidth = (double)Math.Abs(OpExtent.CellWidth);
            cellHeight = (double)Math.Abs(OpExtent.CellHeight);
        }

        /// <summary>
        /// Handy convenience function for slope calculations
        /// </summary>
        /// <param name="fElev"></param>
        /// <returns></returns>
        public double CalculateSlope(double[] fElev)
        {
            dzdx = ((fElev[2] - fElev[0]) + ((2 * fElev[5]) - (2 * fElev[3])) + (fElev[8] - fElev[6])) / (8 * cellWidth);
            dzdy = ((fElev[0] - fElev[6]) + ((2 * fElev[1]) - (2 * fElev[7])) + (fElev[2] - fElev[8])) / (8 * cellHeight);

            dzxy = Math.Pow(dzdx, 2.0) + Math.Pow(dzdy, 2.0);
            riseRun = Math.Sqrt(dzxy);

            return riseRun;
        }

        /// <summary>
        /// Here's where the actual operation happens
        /// </summary>
        /// <param name="wd">Window Data. A list of double arrays</param>
        /// <returns></returns>
        protected override double WindowOp(List<double[]> wd)
        {
            _buff = wd[0];

            if (wd[0][BufferCenterID].Equals(OpNodataVal))
                return OpNodataVal;

            // If anything is nodataval just return that and skip everything else
            for (int k = 0; k < BufferCellNum; k++)
                if (wd[0][k].Equals(OpNodataVal))
                    wd[0][k] = wd[0][BufferCenterID];

            theSlope = CalculateSlope(_buff);

            switch (_slopetype)
            {
                case SlopeType.Degrees:
                    retval = Math.Atan(riseRun) * (180 / Math.PI);
                    break;
                case SlopeType.Percent:
                    retval = riseRun * 100;
                    break;
            }

            // We are required to return something
            return retval;
        }

    }
}
