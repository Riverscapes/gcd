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
        protected override void CellOp(List<T[]> data, List<T[]> outputs, int id)
        {
            outputs[0][id] = outNodataVals[0];

            for (int did = 0; did < data.Count; did++)
                if (!data[did][id].Equals(inNodataVals[did]))
                {
                    outputs[0][id] = data[did][id];
                    continue;
                }

        }

    }
}
