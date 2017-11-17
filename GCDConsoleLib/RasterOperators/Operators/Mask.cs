using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    public class Mask<T> : CellByCellOperator<T>
    {

        /// <summary>
        /// Pass-through constructor
        /// </summary>
        public Mask(Raster rUnMasked, Raster rMask, Raster rOutputRaster) :
            base(new List<Raster> { rUnMasked, rMask }, rOutputRaster)
        { }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override T CellOp(List<T[]> data, int id)
        {
            if (data[1][id].Equals(_rasternodatavals[1]))
                return OpNodataVal;
            else
                return data[0][id];
        }
    }
}
