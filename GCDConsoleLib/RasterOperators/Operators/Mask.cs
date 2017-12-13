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
        protected override void CellOp(List<T[]> data, List<T[]> outputs, int id)
        {
            if (data[1][id].Equals(inNodataVals[1]))
                outputs[0][id] = outNodataVals[0];
            else
                outputs[0][id] = data[0][id];
        }
    }
}
