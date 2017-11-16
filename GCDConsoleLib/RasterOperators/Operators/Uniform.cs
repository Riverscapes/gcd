using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{

    public class UniformRaster<T> : CellByCellOperator<T>
    {
        private T _val;
        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public UniformRaster(Raster rInput, Raster rOutputRaster, T val) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
           _val = val;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override T CellOp(List<T[]> data, int id)
        {
            return _val;
        }

    }
}
