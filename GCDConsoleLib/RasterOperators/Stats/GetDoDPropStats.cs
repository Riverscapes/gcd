using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD.Stats;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetDoDPropStats : CellByCellOperator<float>
    {
        public DoDPropStats Stats;
        public float fDoDValue, fPropErr;
        private float _thresh;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public GetDoDPropStats(ref Raster rDod, ref Raster rErr, float thresh, DoDPropStats theStats) :
            base(new List<Raster> { rDod, rErr })
        {
            _thresh = thresh;
            Stats = theStats;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(ref List<float[]> data, int id)
        {
            fDoDValue = data[0][id];
            if (fDoDValue != _rasternodatavals[0])
            {
                // Deposition
                if (fDoDValue > 0)
                {
                    // Raw Deposition
                    Stats.DepositionRaw.AddToSumAndIncrementCounter(fDoDValue);

                    if (fDoDValue > fPropErr)
                    {
                        // Thresholded Deposition
                        Stats.DepositionThr.AddToSumAndIncrementCounter(fDoDValue);
                        Stats.DepositionErr.AddToSumAndIncrementCounter(fPropErr);
                    }
                }
                // Erosion
                if (fDoDValue < 0)
                {
                    // Raw Erosion
                    Stats.ErosionRaw.AddToSumAndIncrementCounter(fDoDValue - 1);

                    if (fDoDValue < (fPropErr  -1))
                    {
                        // Thresholded Erosion
                        Stats.ErosionThr.AddToSumAndIncrementCounter(fDoDValue * - 1);
                        Stats.ErosionErr.AddToSumAndIncrementCounter(fPropErr);
                    }
                }
            }

            // We need to return something
            return 0;
        }

    }
}
