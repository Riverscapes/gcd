﻿using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{

    public class RasterThreshold : CellByCellOperator<Single>
    {
        private RasterOperators.ThresholdOps _botOp;
        private Single _botNum;
        private RasterOperators.ThresholdOps _topOp;
        private Single _topNum;
        private bool bTwoOps;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public RasterThreshold(ref Raster rInput, RasterOperators.ThresholdOps tOp,
            Single fThresh) :
            base(new List<Raster> { rInput })
        {
            _botOp = tOp;
            _botNum = fThresh;
            bTwoOps = false;
        }
        public RasterThreshold(ref Raster rInput,
            RasterOperators.ThresholdOps tBottomOp, Single fBottomThresh,
            RasterOperators.ThresholdOps tTopOp, Single fTopThresh) :
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

        protected override Single CellOp(ref List<Single[]> data, int id)
        {
            // Get out quick if we can
            if (data[0][id] == _rasternodatavals[0])
                return _rasternodatavals[0];

            Single val = data[0][id];
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
