using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    public class Threshold : CellByCellOperator<double>
    {
        private RasterOperators.ThresholdOps _botOp;
        private double _botNum;
        private RasterOperators.ThresholdOps _topOp;
        private double _topNum;
        private bool bTwoOps;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="tOp"></param>
        /// <param name="fThresh"></param>
        /// <param name="rOutputRaster"></param>
        public Threshold(Raster rInput, RasterOperators.ThresholdOps tOp,
            decimal fThresh, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            _botOp = tOp;
            _botNum = (double)fThresh;
            bTwoOps = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="tOp"></param>
        /// <param name="rThresh"></param>
        /// <param name="rOutputRaster"></param>
        public Threshold(Raster rInput, RasterOperators.ThresholdOps tOp,
            Raster rThresh, Raster rOutputRaster) :
            base(new List<Raster> { rInput, rThresh }, rOutputRaster)
        {
            _botOp = tOp;
            bTwoOps = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="tBottomOp"></param>
        /// <param name="fBottomThresh"></param>
        /// <param name="tTopOp"></param>
        /// <param name="fTopThresh"></param>
        /// <param name="rOutputRaster"></param>
        public Threshold(Raster rInput,
            RasterOperators.ThresholdOps tBottomOp, decimal fBottomThresh,
            RasterOperators.ThresholdOps tTopOp, decimal fTopThresh, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            if (tBottomOp == RasterOperators.ThresholdOps.LessThan ||
                tBottomOp == RasterOperators.ThresholdOps.LessThanOrEqual ||
                tTopOp == RasterOperators.ThresholdOps.GreaterThan ||
                tTopOp == RasterOperators.ThresholdOps.GreaterThanOrEqual)
                throw new ArgumentOutOfRangeException("Invalid Operators chosen for thresholding");

            _botOp = tBottomOp;
            _botNum = (double)fBottomThresh;
            _topOp = tTopOp;
            _topNum = (double)fTopThresh;
            bTwoOps = true;
        }

        /// <summary>
        /// Here's where the actual thrreshold Operation happens
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override double CellOp(List<double[]> data, int id)
        {
            // Get out quick if we can
            if (data[0][id] == _rasternodatavals[0])
                return _rasternodatavals[0];

            double val = data[0][id];
            // TwoOps means we're doing Greater than something AND less than something else
            if (bTwoOps)
            {
                if (_rasters.Count == 1)
                {
                    if (_botOp == RasterOperators.ThresholdOps.GreaterThan && val <= _botNum ||
                        _botOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && val < _botNum ||
                        _topOp == RasterOperators.ThresholdOps.LessThan && val >= _topNum ||
                        _topOp == RasterOperators.ThresholdOps.LessThanOrEqual && val > _topNum)
                        val = _rasternodatavals[0];
                }
                else
                {
                    double botNum = data[1][id];
                    double topNum = data[2][id];
                    if (_botOp == RasterOperators.ThresholdOps.GreaterThan && val <= _botNum ||
                        _botOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && val < _botNum ||
                        _topOp == RasterOperators.ThresholdOps.LessThan && val >= _topNum ||
                        _topOp == RasterOperators.ThresholdOps.LessThanOrEqual && val < _topNum)
                        val = _rasternodatavals[0];
                }
            }
            // One operation only means we're greater OR less than some value
            else
            {
                // Compare the raster value to a constant
                if (_rasters.Count == 1)
                {
                    if (_botOp == RasterOperators.ThresholdOps.GreaterThan && data[0][id] <= _botNum ||
                        _botOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && data[0][id] < _botNum ||
                        _botOp == RasterOperators.ThresholdOps.LessThan && data[0][id] >= _botNum ||
                        _botOp == RasterOperators.ThresholdOps.LessThanOrEqual && data[0][id] > _botNum)
                        val = _rasternodatavals[0];
                }
                // This is a raster operation. Compare the first raster value to a second raster value
                else
                {
                    double rBotNum = data[1][id];
                    if (_botOp == RasterOperators.ThresholdOps.GreaterThan && data[0][id] <= rBotNum ||
                        _botOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && data[0][id] < rBotNum ||
                        _botOp == RasterOperators.ThresholdOps.LessThan && data[0][id] >= rBotNum ||
                        _botOp == RasterOperators.ThresholdOps.LessThanOrEqual && data[0][id] > rBotNum)
                        val = _rasternodatavals[0];
                }

            }
            return val;
        }

    }
}
