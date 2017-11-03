using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetChangeStats : CellByCellOperator<float>
    {

        public DoDStats Stats;
        private bool bHasErrRaster;
        float fRVal, fMask;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public GetChangeStats(ref Raster rInput, DoDStats theStats) :
            base(new List<Raster> { rInput })
        {
            Stats = theStats;
            bHasErrRaster = false;
        }

        public GetChangeStats(ref Raster rInput, ref Raster rPropError, DoDStats theStats) :
            base(new List<Raster> { rInput, rPropError })
        {
            Stats = theStats;
            bHasErrRaster = true;
        }


        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(ref List<float[]> data, int id)
        {
            if (data.Count == 1 && data[0][id] != _rasternodatavals[0])
            {
                fRVal = data[0][id];
                // Deposition
                if (fRVal > 0)
                    Stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                // Erosion
                else if (fRVal < 0)
                    Stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal * -1);
            }
            // If we have a mask then use it.
            else if (data.Count == 2 && data[1][id] != _rasternodatavals[1])
            {
                fRVal = data[0][id];
                fMask = data[1][id];
                if (fRVal > 0)
                {
                    // Deposition
                    if (fMask != _rasternodatavals[1])
                        Stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                    // Erosion
                    else if (fMask < 0)
                        Stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal);
                }
            }

            // We need to return something. Doesn't matter what
            return 0;
        }

    }
}
