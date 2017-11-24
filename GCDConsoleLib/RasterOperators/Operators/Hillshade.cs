using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// We do Hillshade as a float on purpose since we need it to be fast and accuracy is less important
    /// </summary>
    class Hillshade : WindowOverlapOperator<float>
    {
        private double azimuth, zFactor, altDeg, zenDeg, zenRad, azimuthMath, azimuthRad, aspectRad;

        /// <summary>
        /// Pass-through constructor for Hillshade
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        public Hillshade(Raster rInput, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, 1, rOutputRaster)
        { }

        /// <summary>
        /// Give us a sensible default for shadow direction etc.
        /// </summary>
        private void SetDefaultVars()
        {
            //setup default and zenith variables
            azimuth = 315;
            zFactor = 1;
            altDeg = 45;
            zenDeg = 90 - altDeg;
            zenRad = zenDeg * Math.PI / 180;
            azimuthMath = 360 - azimuth + 90;

            if (azimuthMath >= 360)
                azimuthMath = azimuthMath - 360;

            azimuthRad = azimuthMath * Math.PI / 180;
        }

        /// <summary>
        /// The actual window operation
        /// </summary>
        /// <param name="wd"></param>
        /// <returns></returns>
        protected override float WindowOp(List<float[]> wd)
        {
            // Don't calculate if we have nodatas in the mix
            for (int k = 0; k < BufferCellNum; k++)
                if (wd[0][k].Equals(OpNodataVal))
                    return OpNodataVal;

            float dzdx = ((wd[0][2] + (2 * wd[0][5]) + wd[0][8]) - (wd[0][0] + (2 * wd[0][3]) + wd[0][6])) / (8 * (float)Math.Abs(WindowExtent.CellHeight));
            float dzdy = ((wd[0][6] + (2 * wd[0][7]) + wd[0][8]) - (wd[0][0] + (2 * wd[0][1]) + wd[0][2])) / (8 * (float)Math.Abs(WindowExtent.CellHeight));
            double slopeRad = Math.Atan(zFactor * Math.Sqrt(Math.Pow(dzdx, 2.0) + Math.Pow(dzdy, 2.0)));

            if (dzdx != 0)
            {
                aspectRad = Math.Atan2(dzdy, (dzdx * (-1)));
                if (aspectRad < 0.0)
                    aspectRad = 2.0 * Math.PI + aspectRad;
            }
            else
            {
                if (dzdy > 0.0)
                    aspectRad = Math.PI / 2.0;
                else if (dzdy < 0.0)
                    aspectRad = 2.0 * Math.PI - Math.PI / 2.0;
                //else
                //{
                //    //aspectRad = aspectRad;
                //}
            }

            return (float)Math.Round(254 * ((Math.Cos(zenRad) * Math.Cos(slopeRad)) + (Math.Sin(zenRad) * Math.Sin(slopeRad) * Math.Cos(azimuthRad - aspectRad)))) + 1;
        }
    }
}
