using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{
    public class Mosaic<T> : CellByCellOperator<T>
    {
        /// <summary>
        /// Pass-through constructor
        /// </summary>
        public Mosaic(List<Raster> rlInputs, Raster rOutputRaster) :
            base(rlInputs, rOutputRaster)
        { }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override T CellOp(List<T[]> data, int id)
        {
            for (int did = 0; did < data.Count; did++)
                if (!data[did][id].Equals(_rasternodatavals[did]))
                    return data[did][id];

            return OpNodataVal;

        }

    }
}
