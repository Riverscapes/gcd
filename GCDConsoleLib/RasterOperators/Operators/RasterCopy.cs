using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{

    public class RasterCopy<T> : CellByCellOperator<T>
    {
        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public RasterCopy(ref Raster rInput, ref Raster rOutputRaster, ExtentRectangle newRect) :
            base(new List<Raster> { rInput }, ref rOutputRaster)
        { }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override T CellOp(ref List<T[]> data, int id)
        {
            return data[0][id];
        }

    }
}
