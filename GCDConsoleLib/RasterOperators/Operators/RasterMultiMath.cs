using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    public class RasterMultiMath : CellByCellOperator<double>
    {
        private RasterOperators.MultiMathOpType _type;


        /// <summary>
        /// Pass-through constructor for Raster Math with a scalar operand
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        public RasterMultiMath(RasterOperators.MultiMathOpType otType, List<Raster> rasters,
            Raster rOutputRaster) : base(rasters, new List<Raster> { rOutputRaster })
        {
            _type = otType;

            if (rasters.Count < 2)
                throw new ArgumentException(String.Format("Must pass in at least 2 rasters ({0} found)", rasters.Count));
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
                case RasterOperators.MultiMathOpType.Addition:
                    val = Addition(data, id, inNodataVals, outNodataVals[0]);
                    break;
                case RasterOperators.MultiMathOpType.Maximum:
                    val = Maximum(data, id, inNodataVals, outNodataVals[0]);
                    break;
                case RasterOperators.MultiMathOpType.Minimum:
                    val = Minimum(data, id, inNodataVals, outNodataVals[0]);
                    break;
                case RasterOperators.MultiMathOpType.Mean:
                    val = Mean(data, id, inNodataVals, outNodataVals[0]);
                    break;
                case RasterOperators.MultiMathOpType.StandardDeviation:
                    val = StandardDeviation(data, id, inNodataVals, outNodataVals[0]);
                    break;
            }

            outputs[0][id] = val;
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double Addition(List<double[]> data, int id, List<double> inNodata, double outnodata)
        {
            double retVal = 0.0;
            bool bfound = false;
            for (int did = 0; did < data.Count; did++)
            {
                if (data[did][id] != inNodata[did])
                {
                    bfound = true;
                    retVal += data[did][id];
                }
            }
            if (!bfound) retVal = outnodata;
            return retVal;
        }

        /// <summary>
        /// Minimum
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double Minimum(List<double[]> data, int id, List<double> inNodata, double outnodata)
        {
            double retVal = double.MaxValue;
            bool bfound = false;
            for (int did = 0; did < data.Count; did++)
            {
                if (data[did][id] != inNodata[did] && data[did][id] < retVal)
                {
                    bfound = true;
                    retVal = data[did][id];
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
        public static double Maximum(List<double[]> data, int id, List<double> inNodata, double outnodata)
        {
            double retVal = double.MinValue;
            bool bfound = false;
            for (int did = 0; did < data.Count; did++)
            {
                if (data[did][id] != inNodata[did] && data[did][id] > retVal)
                {
                    bfound = true;
                    retVal = data[did][id];
                }
            }
            if (!bfound) retVal = outnodata;
            return retVal;
        }

        /// <summary>
        /// Mean
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double Mean(List<double[]> data, int id, List<double> inNodata, double outnodata)
        {
            double retVal = 0;

            int count = 0;
            for (int did = 0; did < data.Count; did++)
            {
                if (data[did][id] != inNodata[did])
                {
                    count++;
                    retVal += data[did][id];
                }
            }
            if (count==0) retVal = outnodata;
            else retVal = retVal / count;
            return retVal;
        }


        /// <summary>
        /// Standard Deviation
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double StandardDeviation(List<double[]> data, int id, List<double> inNodata, double outnodata)
        {
            List <double> vals = new List<double> { };
            double sum = 0;
            int count = 0;

            for (int did = 0; did < data.Count; did++)
            {
                if (data[did][id] != inNodata[did])
                {
                    vals.Add(data[did][id]);
                    count++;
                    sum += data[did][id];
                }
            }
            
            if (count == 0)
                return outnodata;

            double mean = sum / count;

            double newsum = 0;
            foreach(double val in vals)
            {
                newsum += Math.Pow(val - mean, 2);
            }

            return Math.Sqrt(newsum / (count - 1));
        }
    }


}
