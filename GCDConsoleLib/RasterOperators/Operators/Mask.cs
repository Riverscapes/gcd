using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    public class Mask<T> : CellByCellOperator<T>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rUnMasked"></param>
        /// <param name="rMask"></param>
        /// <param name="rOutputRaster"></param>
        public Mask(Raster rUnMasked, Raster rMask, Raster rOutputRaster) :
            base(new List<Raster> { rUnMasked, rMask }, rOutputRaster)
        { }

        /// <summary>
        /// The operation on each cell
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override T CellOp(List<T[]> data, int id)
        {
            if (data[1][id].Equals(_rasternodatavals[1]))
                return OpNodataVal;
            else
                return data[0][id];
        }
    }
}
