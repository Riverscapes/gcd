using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{
    class Slope : WindowOverlapOperator<float>
    {
        float dzdx, dzdy, dzxy, riseRun, cellWidth, cellHeight, retval, theSlope;
        float[] _buff;
        public enum SlopeType: byte { Percent, Degrees};
        private SlopeType _slopetype;

        public Slope(Raster rInput, Raster rOutputRaster, SlopeType theType) :
            base(new List<Raster> { rInput }, 1, rOutputRaster)
        {
            _slopetype = theType;
            cellWidth = (float)Math.Abs(OpExtent.CellWidth);
            cellHeight = (float)Math.Abs(OpExtent.CellHeight);
        }

        public float CalculateSlope(float[] fElev)
        {
            dzdx = ((fElev[2] - fElev[0]) + ((2 * fElev[5]) - (2 * fElev[3])) + (fElev[8] - fElev[6])) / (8 * cellWidth);
            dzdy = ((fElev[0] - fElev[6]) + ((2 * fElev[1]) - (2 * fElev[7])) + (fElev[2] - fElev[8])) / (8 * cellHeight);
            dzxy = (float)Math.Pow(dzdx, 2.0) + (float)Math.Pow(dzdy, 2.0);
            riseRun = (float)Math.Pow(dzxy, 0.5);

            return riseRun;
        }

        protected override float WindowOp(List<float[]> wd)
        {
            _buff = wd[0];
            // If anything is nodataval just return that and skip everything else
            for (int k = 0; k < BufferCellNum; k++)
                if (wd[0][k].Equals(OpNodataVal))
                    return OpNodataVal;

            theSlope = CalculateSlope(_buff);

            switch (_slopetype)
            {
                case SlopeType.Degrees:
                    retval = (float)Math.Atan(riseRun) * (180 / (float)Math.PI);
                    break;
                case SlopeType.Percent:
                    retval = riseRun * 100;
                    break;
            }

            return 0;
        }

    }
}
