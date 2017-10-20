using System;
using System.Collections.Generic;
using GCDConsoleLib.Operators.Base;

namespace GCDConsoleLib.RasterOperators
{
    public class RasterCopy : CellByCellOperator
    {

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster)
        {
            Raster rOutputRaster = new Raster(sOutputRaster);
            RasterCopy myCopy = new RasterCopy(ref rInput,ref rOutputRaster, rInput.Extent);
            return myCopy.Run();
        }

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect)
        {
            Raster rOutputRaster = new Raster(sOutputRaster);
            RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect);
            myCopy.SetOpExtent(newRect);
            return myCopy.Run();
        }

        /// This one's mainly for testing purposes
        public static Raster ExtendedCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect)
        {
            RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect);
            myCopy.SetOpExtent(newRect);
            return myCopy.Run();
        }

        /// <summary>
        /// We protect our constructor because we don't really want people using it
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <param name="newRect"></param>
        /// <param name="newProj"></param>
        /// <param name="newVUnit"></param>
        protected RasterCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect) :
            base(new List<Raster> { rInput }, ref rOutputRaster)
        { }

        protected override double CellOp(ref List<double[]> data, int id)
        {
            double val = 0;

            if (data[0][id] == OpNoDataVal)
                val = OpNoDataVal;
            else
                val = data[0][id];

            return val;
        }

    }
}
