using System;
using System.Collections.Generic;
using GCDConsoleLib.Utility;
namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// We do Hillshade as a float on purpose since we need it to be fast and accuracy is less important
    /// </summary>
    class PriorProbRaster : CellByCellOperator<double>
    {
        private static int rawDod = 0;
        private static int propError = 1;

        /// <summary>
        /// Pass-through constructor for Creating Prior Probability Rasters
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        public PriorProbRaster(Raster rawDoD, Raster propError, Raster rOutputRaster) :
            base(new List<Raster> { rawDoD, propError }, rOutputRaster)
        {  }

        /// <summary>
        /// The Cell op for Prior Probability Rasters
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<double[]> data, List<double[]> outputs,int id)
        {
            // Reminder: rawDod ==> 0, newError ==> 1, oldError ==> 2
            double result = outNodataVals[0];

            // If Nothing is Nodata (as long as there is a nodata value) 
            if ((data[rawDod][id] != inNodataVals[rawDod] || !_inputRasters[rawDod].HasNodata) &&
                (data[propError][id] != inNodataVals[propError] || !_inputRasters[propError].HasNodata))
            {

                if (data[rawDod][id] < 0)
                    result = -(2 * Probability.normalDist(Math.Abs(data[rawDod][id]) / data[propError][id]) - 1);
                else
                    result = 2 * Probability.normalDist(Math.Abs(data[rawDod][id]) / data[propError][id]) - 1;

            }

            outputs[0][id] = result;
        }
    }
}
