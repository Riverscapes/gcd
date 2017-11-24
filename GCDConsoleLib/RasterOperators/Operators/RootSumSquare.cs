using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{

    public class RootSumSquare : CellByCellOperator<double>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput1"></param>
        /// <param name="rInput2"></param>
        /// <param name="rOutputRaster"></param>
        public RootSumSquare(Raster rInput1, Raster rInput2, Raster rOutputRaster) :
            base(new List<Raster> { rInput1, rInput2 }, rOutputRaster)
        { }

        /// <summary>
        ///  This is the actual implementation of the cell-by-cell logic
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override double CellOp(List<double[]> data, int id)
        {
            if (data[0][id] == _rasternodatavals[0] ||
                 data[1][id] == _rasternodatavals[1])
                return OpNodataVal;
            else
                return Math.Sqrt(Math.Pow(data[0][id], 2) + Math.Pow(data[1][id], 2));
        }

    }
}
