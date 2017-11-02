using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{

    public class BinRaster : CellByCellOperator<double>
    {
        public Histogram theHistogram;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public BinRaster(ref Raster rInput, int numBins) :
            base(new List<Raster> { rInput })
        {
            theHistogram = new Histogram(numBins, ref rInput);
        }

        protected override double CellOp(ref List<double[]> data, int id)
        {
            if (!data[0][id].Equals(_rasternodatavals[0]))
                theHistogram.AddBinVal(data[0][id]);
            return 0;
        }

    }
}
