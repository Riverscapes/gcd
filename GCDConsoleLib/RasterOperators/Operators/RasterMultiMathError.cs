using System;
using System.Collections.Generic;
using System.Linq;

namespace GCDConsoleLib.Internal.Operators
{
    public class RasterMultiMathError : CellByCellOperator<double>
    {
        private readonly RasterOperators.MultiMathErrOpType _type;
        private readonly List<int> _inputids;
        private readonly List<int> _errids;

        /// <summary>
        /// Pass-through constructor for Raster Math with a scalar operand
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        public RasterMultiMathError(RasterOperators.MultiMathErrOpType otType, List<Raster> rasters, List<Raster> errrasters,
            Raster rOutputRaster) : base(rasters, new List<Raster> { rOutputRaster })
        {
            _type = otType;

            if (rasters.Count < 2)
                throw new ArgumentException(String.Format("Must pass in at least 2 rasters ({0} found)", rasters.Count));

            if (rasters.Count != errrasters.Count)
                throw new ArgumentException("number of Rasters and Error Rasters mut be equal");

            _inputids = Enumerable.Range(0, rasters.Count).ToList();
            _errids = Enumerable.Range(rasters.Count, errrasters.Count).ToList();

            // There could be a lot of rasters here so let's make our window size small-ish
            chunkRows = 10;

            // Add our errors onto the pile
            foreach (Raster errRaster in errrasters)
                AddInputRaster(errRaster);

        }

        /// <summary>
        /// The actual cell-by-cell operations
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<double[]> data, List<double[]> outputs, int id)
        {
            double val = outNodataVals[0];
            switch (_type)
            {
                // Choose your math operation
                case RasterOperators.MultiMathErrOpType.Maximum:
                    val = Maximum(data, id, inNodataVals, _inputids, _errids, outNodataVals[0]);
                    break;
                case RasterOperators.MultiMathErrOpType.Minimum:
                    val = Minimum(data, id, inNodataVals, _inputids, _errids, outNodataVals[0]);
                    break;
                default:
                    throw new ArgumentException("Only max and min are accepted for this operation");
            }

            outputs[0][id] = val;
        }


        /// <summary>
        /// Minimum
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double Minimum(List<double[]> data, int id, List<double> inNodata, List<int> _inputids, List<int> _errids, double outnodata)
        {
            double compareVal = double.MaxValue;
            double retVal = outnodata;
            bool bfound = false;
            
            foreach (int did in _inputids)
            {
                if (data[_inputids[did]][id] != inNodata[_inputids[did]] && 
                    data[_inputids[did]][id] < compareVal)
                {
                    bfound = true;
                    compareVal = data[_inputids[did]][id];
                    retVal = data[_errids[did]][id];
                }
            }
            if (!bfound) retVal = outnodata;
            return retVal;
        }

        /// <summary>
        /// Maximum
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double Maximum(List<double[]> data, int id, List<double> inNodata, List<int> _inputids, List<int> _errids, double outnodata)
        {
            double compareVal = double.MinValue;
            double retVal = outnodata;
            bool bfound = false;
            foreach (int did in _inputids)
            {
                if (data[_inputids[did]][id] != inNodata[_inputids[did]] && 
                    data[_inputids[did]][id] > compareVal)
                {
                    bfound = true;
                    compareVal = data[_inputids[did]][id];
                    retVal = data[_errids[did]][id];
                }
            }
            if (!bfound) retVal = outnodata;
            return retVal;
        }
    }
}
