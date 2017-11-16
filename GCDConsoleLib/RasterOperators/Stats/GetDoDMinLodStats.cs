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
            fDoDValue = data[0][id];
            if (fDoDValue != _rasternodatavals[0])
            {
                // Deposition
                if (fDoDValue > 0)
                {
                    // Raw Deposition
                    Stats.DepositionRaw.AddToSumAndIncrementCounter(fDoDValue);

                    // Thresholded Deposition
                    if (fDoDValue > _thresh)
                    {
                        Stats.DepositionThr.AddToSumAndIncrementCounter(fDoDValue);
                        Stats.DepositionErr.AddToSumAndIncrementCounter(fDoDValue * _thresh);
                    }
                }

                // Erosion
                if (fDoDValue < 0)
                {
                    // Raw Erosion
                    Stats.ErosionRaw.AddToSumAndIncrementCounter(fDoDValue * -1);

                    // Thresholded Erosion
                    if (fDoDValue < (_thresh * -1))
                    {
                        Stats.ErosionThr.AddToSumAndIncrementCounter(fDoDValue * -1);
                        Stats.ErosionErr.AddToSumAndIncrementCounter(fDoDValue * _thresh);
                    }
                }
            }
            // We need to return something
            return 0;
        }

    }
}
