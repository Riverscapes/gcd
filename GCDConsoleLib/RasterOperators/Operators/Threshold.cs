using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

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
        /// Pass-through constructure
        /// </summary>
        public Threshold(ref Raster rInput, RasterOperators.ThresholdOps tOp,
            float fThresh) :
            base(new List<Raster> { rInput })
        {
            _botOp = tOp;
            _botNum = fThresh;
            bTwoOps = false;
        }
        public Threshold(ref Raster rInput,
            RasterOperators.ThresholdOps tBottomOp, float fBottomThresh,
            RasterOperators.ThresholdOps tTopOp, float fTopThresh) :
            base(new List<Raster> { rInput })
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

        protected override float CellOp(ref List<float[]> data, int id)
        {
            // Get out quick if we can
            if (data[0][id] == _rasternodatavals[0])
                return _rasternodatavals[0];

            float val = data[0][id];
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
