using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    public class Mosaic<T> : CellByCellOperator<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rlInputs"></param>
        /// <param name="rOutputRaster"></param>
        public Mosaic(List<Raster> rlInputs, Raster rOutputRaster) :
            base(rlInputs, rOutputRaster)
        { }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override T CellOp(List<T[]> data, int id)
        {
            for (int did = 0; did < data.Count; did++)
                if (!data[did][id].Equals(_rasternodatavals[did]))
                    return data[did][id];

            return OpNodataVal;

        }

    }
}
