using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetDodMinLodStats : CellByCellOperator<float>
    {
        public DoDStats Stats;
        private float fDoDValue;
        private float _thresh;
        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public GetDodMinLodStats(Raster rawDoD, Raster thrDoD, 
            float thresh, DoDStats theStats) :
            base(new List<Raster> { rawDoD, thrDoD })
        {
            Stats = theStats;
            _thresh = thresh;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(List<float[]> data, int id)
        {
            CellChangeCalc(data, id, Stats);
            // We need to return something
            return 0;
        }

        /// <summary>
        /// We separate out the calc op so we can call it from elsewhere (like the seggregation function)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <param name="stats"></param>
        /// <param name="nodata"></param>
        public void CellChangeCalc(List<float[]> data, int id, DoDStats stats)
        {
            fDoDValue = data[0][id];
            if (fDoDValue != _rasternodatavals[0])
            {
                // Deposition
                if (fDoDValue > 0)
                {
                    // Raw Deposition
                    stats.DepositionRaw.AddToSumAndIncrementCounter(fDoDValue);

                    // Thresholded Deposition
                    if (fDoDValue > _thresh)
                    {
                        stats.DepositionThr.AddToSumAndIncrementCounter(fDoDValue);
                        stats.DepositionErr.AddToSumAndIncrementCounter(fDoDValue * _thresh);
                    }
                }

                // Erosion
                if (fDoDValue < 0)
                {
                    // Raw Erosion
                    stats.ErosionRaw.AddToSumAndIncrementCounter(fDoDValue * -1);

                    // Thresholded Erosion
                    if (fDoDValue < (_thresh * -1))
                    {
                        stats.ErosionThr.AddToSumAndIncrementCounter(fDoDValue * -1);
                        stats.ErosionErr.AddToSumAndIncrementCounter(fDoDValue * _thresh);
                    }
                }
            }
        }

    }
}
