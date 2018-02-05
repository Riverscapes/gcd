using System;
using System.Collections.Generic;
using System.Linq;

namespace GCDConsoleLib.Internal.Operators
{
    public class ThresholdAbs : Threshold
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="tOp"></param>
        /// <param name="rThresh"></param>
        /// <param name="rOutputRaster"></param>
        public ThresholdAbs(Raster rInput, RasterOperators.ThresholdOps tOp,
            Raster rThresh, Raster rOutputRaster) :
            base(rInput, tOp, rThresh, rOutputRaster)
        {
        }

        /// <summary>
        /// Here's where the actual thrreshold Operation happens
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<double[]> data, List<double[]> outputs, int id)
        {
            // Get out quick if we can
            if (data[0][id] == inNodataVals[0])
            {
                outputs[0][id] = outNodataVals[0];
                return;
            }

            double val = Math.Abs(data[0][id]);
            double result = data[0][id];
            // Compare the raster value to a constant
            if (_inputRasters.Count == 1)
            {
                if (_botOp == RasterOperators.ThresholdOps.GreaterThan && val <= _botNum ||
                    _botOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && val < _botNum ||
                    _botOp == RasterOperators.ThresholdOps.LessThan && val >= _botNum ||
                    _botOp == RasterOperators.ThresholdOps.LessThanOrEqual && val > _botNum)
                    result = inNodataVals[0];
            }
            // This is a raster operation. Compare the first raster value to a second raster value
            else
            {
                double rBotNum = data[1][id];
                if (_botOp == RasterOperators.ThresholdOps.GreaterThan && val <= rBotNum ||
                    _botOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && val < rBotNum ||
                    _botOp == RasterOperators.ThresholdOps.LessThan && val >= rBotNum ||
                    _botOp == RasterOperators.ThresholdOps.LessThanOrEqual && val > rBotNum)
                    result = inNodataVals[0];
            }

            outputs[0][id] = result;
        }
    }
}
