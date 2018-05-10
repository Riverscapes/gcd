using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// We do Hillshade as a float on purpose since we need it to be fast and accuracy is less important
    /// </summary>
    class Hillshade : WindowOverlapOperator<float>
    {
        private double azimuth, zFactor, altDeg, zenDeg, zenRad, azimuthMath, azimuthRad, aspectRad;

        private float fcellHeight;

        /// <summary>
        /// Pass-through constructor for Hillshade
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        public Hillshade(Raster rInput, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, 1, new List<Raster>() { rOutputRaster })
        {
            SetDefaultVars();
        }


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

            fcellHeight = (float)Math.Abs(WindowExtent.CellHeight);
        }

        /// <summary>
        /// The actual window operation
        /// </summary>
        /// <param name="wd"></param>
        /// <returns></returns>
        protected override void WindowOp(List<float[]> wd, List<float[]> outbuffers, int id, bool containsNodata)
        {
            // Don't calculate if we have nodatas in the mix
            if (containsNodata)
            {
                outbuffers[0][id] = outNodataVals[0];
                return;
            }

            float dzdx = ((wd[0][2] + (2 * wd[0][5]) + wd[0][8]) - (wd[0][0] + (2 * wd[0][3]) + wd[0][6])) / (8 * fcellHeight);
            float dzdy = ((wd[0][6] + (2 * wd[0][7]) + wd[0][8]) - (wd[0][0] + (2 * wd[0][1]) + wd[0][2])) / (8 * fcellHeight);
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

            float val = (float)Math.Round(254 * ((Math.Cos(zenRad) * Math.Cos(slopeRad)) + (Math.Sin(zenRad) * Math.Sin(slopeRad) * Math.Cos(azimuthRad - aspectRad)))) + 1;
            // WEird edge effects at the bottom of some rasters. We want to enforce 0-255

            if (val < 0) val = 0;
            outbuffers[0][id] = val;
        }
    }
}
