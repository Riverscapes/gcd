using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetChangeStats : CellByCellOperator<float>
    {
        public DoDStats Stats;
        private bool bSeg;
        protected List<float> _nodata;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public GetChangeStats(Raster rInput, DoDStats theStats) :
            base(new List<Raster> { rInput })
        {
            Stats = theStats;
            bSeg = false;
            _nodata = _rasternodatavals;
        }

        /// <summary>
        /// This is the propError constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rPropError"></param>
        /// <param name="theStats"></param>
        public GetChangeStats(Raster rInput, Raster rPropError, DoDStats theStats) :
            base(new List<Raster> { rInput, rPropError })
        {
            Stats = theStats;
            bSeg = false;
        }


        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(List<float[]> data, int id)
        {
            CellChangeCalc(data, id, Stats);
            // We need to return something. Doesn't matter what
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
            float fRVal, fMask;
            if (data.Count == 1 && data[0][id] != _rasternodatavals[0])
            {
                fRVal = data[0][id];
                // Deposition
                if (fRVal > 0)
                    stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                // Erosion
                else if (fRVal< 0)
                    stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal* -1);
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
                        stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                    // Erosion
                    else if (fMask< 0)
                        stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal);
                }
            }
        }

    }
}
