using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{

    public class UniformRaster<T> : CellByCellOperator<T>
    {
        private T _val;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="val"></param>
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
