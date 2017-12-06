using System;
using System.Collections.Generic;
namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// We do Hillshade as a float on purpose since we need it to be fast and accuracy is less important
    /// </summary>
    class ThreshDoDProbSptlCoherence : CellByCellOperator<double>
    {
        /// <summary>
        /// Pass-through constructor for Creating Prior Probability Rasters
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        public ThreshDoDProbSptlCoherence(Raster rawDoD, Raster rPriorProb, Raster rOutputRaster, decimal cutoff) :
            base(new List<Raster> { rawDoD, rPriorProb }, rOutputRaster)
        {}
        // Raster rawDoD, string thrHistPath, Raster newError, Raster oldError, FileInfo sPriorProbRaster, decimal fThreshold

        /// <summary>
        /// The Cell op for Prior Probability Rasters
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override double CellOp(List<double[]> data, int id)
        {
            return OpNodataVal;
        }
    }
}
