using System;
using System.Collections.Generic;
using UnitsNet.Units;

namespace GCDConsoleLib.RasterOperators
{
    public class RasterCopy : BaseOperator
    {
        private ExtentRectangle _newRect;
        private Projection _newProj;
        private LengthUnit _newVUnit;

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster)
        {
            Raster rOutputRaster = new Raster(sOutputRaster);
            RasterCopy myCopy = new RasterCopy(ref rInput,ref rOutputRaster, rInput.Extent, rInput.Proj, rInput.VerticalUnits);
            return myCopy.RunCellByCellOp();
        }

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect)
        {
            Raster rOutputRaster = new Raster(sOutputRaster);
            RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect, rInput.Proj, rInput.VerticalUnits);
            return myCopy.RunCellByCellOp();
        }

        public static Raster ExtendedCopy(ref Raster rInput, string sOutputRaster, ExtentRectangle newRect, Projection newProj, LengthUnit newVUnit)
        {
            Raster rOutputRaster = new Raster(sOutputRaster);
            RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect,  newProj,  newVUnit);
            return myCopy.RunCellByCellOp();
        }

        /// This one's mainly for testing purposes
        public static Raster ExtendedCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect, Projection newProj, LengthUnit newVUnit)
        {
            RasterCopy myCopy = new RasterCopy(ref rInput, ref rOutputRaster, newRect, newProj, newVUnit);
            return myCopy.RunCellByCellOp();
        }

        /// <summary>
        /// We protect our constructor because we don't really want people using it
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="sOutputRaster"></param>
        /// <param name="newRect"></param>
        /// <param name="newProj"></param>
        /// <param name="newVUnit"></param>
        protected RasterCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect, Projection newProj, LengthUnit newVUnit) : base(ref rInput, ref rOutputRaster)
        {
            __opinit(newRect, newProj, newVUnit);
        }

        private void __opinit(ExtentRectangle newRect, Projection newProj, LengthUnit newVUnit)
        {
            Raster _oldRaster = _rasters[0];
            _newRect = newRect;
            _newProj = newProj;
            _newVUnit = newVUnit;
        }

        protected override double CellOp(ref List<double[]> data, int id)
        {
            double val = 0;

            if (data[0][id] == _nodataval)
            {
                val = _nodataval;
            }
            else
            {
                val = data[0][id];
            }
            return val;
        }

        protected override double[] ChunkOp(ref List<double[]> data)
        {
            //We don't use this for math. Everything is cell-wise
            throw new NotImplementedException();
        }

    }
}
