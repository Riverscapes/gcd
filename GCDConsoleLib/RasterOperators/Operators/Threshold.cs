using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    public class Threshold : CellByCellOperator<float>
    {
        private RasterOperators.ThresholdOps _botOp;
        private float _botNum;
        private RasterOperators.ThresholdOps _topOp;
        private float _topNum;
        private bool bTwoOps;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="tOp"></param>
        /// <param name="fThresh"></param>
        /// <param name="rOutputRaster"></param>
        public Threshold(Raster rInput, RasterOperators.ThresholdOps tOp,
            float fThresh, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            _botOp = tOp;
            _botNum = fThresh;
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
            RasterOperators.ThresholdOps tBottomOp, float fBottomThresh,
            RasterOperators.ThresholdOps tTopOp, float fTopThresh, Raster rOutputRaster) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            if (tBottomOp == RasterOperators.ThresholdOps.LessThan ||
                tBottomOp == RasterOperators.ThresholdOps.LessThanOrEqual ||
                tTopOp == RasterOperators.ThresholdOps.GreaterThan ||
                tTopOp == RasterOperators.ThresholdOps.GreaterThanOrEqual)
                throw new ArgumentOutOfRangeException("Invalid Operators chosen for thresholding");

            _botOp = tBottomOp;
            _botNum = fBottomThresh;
            _topOp = tTopOp;
            _topNum = fTopThresh;
            bTwoOps = true;
        }

        /// <summary>
        /// Here's where the actual thrreshold Operation happens
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override float CellOp(List<float[]> data, int id)
        {
            // Get out quick if we can
            if (data[0][id] == _rasternodatavals[0])
                return _rasternodatavals[0];

            float val = data[0][id];
            // TwoOps means we're doing Greater than something AND less than something else
            if (bTwoOps)
            {
                if (
                _botOp == RasterOperators.ThresholdOps.GreaterThan && val > _botNum ||
                _botOp == RasterOperators.ThresholdOps.GreaterThan && val >= _botNum ||
                _topOp == RasterOperators.ThresholdOps.LessThan && val < _topNum ||
                _botOp == RasterOperators.ThresholdOps.LessThanOrEqual && val <= _botNum
                )
                    val = _rasternodatavals[0];
            }
            else
            {
                if (
                _topOp == RasterOperators.ThresholdOps.GreaterThan && data[0][id] > _botNum ||
                _topOp == RasterOperators.ThresholdOps.GreaterThanOrEqual && data[0][id] >= _botNum ||
                _topOp == RasterOperators.ThresholdOps.LessThan && data[0][id] < _botNum ||
                _topOp == RasterOperators.ThresholdOps.LessThanOrEqual && data[0][id] <= _botNum
                )
                    val = _rasternodatavals[0];
            }
            return val;
        }

    }
}
