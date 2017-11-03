using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{

    public class RootSumSquare : CellByCellOperator<float>
    {

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public RootSumSquare(ref Raster rInput1, ref Raster rInput2, Raster rOutputRaster) :
            base(new List<Raster> { rInput1, rInput2 }, rOutputRaster)
        { }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(ref List<float[]> data, int id)
        {
            if (data[0][id] == _rasternodatavals[0] ||
                 data[1][id] == _rasternodatavals[1])
                return OpNodataVal;
            else
                return (float)Math.Sqrt(Math.Pow(data[0][id], 2) + Math.Pow(data[1][id], 2));
        }

}
}
