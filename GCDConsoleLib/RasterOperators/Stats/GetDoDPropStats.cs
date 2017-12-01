using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetDoDPropStats : CellByCellOperator<float>
    {
        public DoDStats Stats;
        public float fDoDValue, fPropErr;

        // If we do budget seg we need the following
        private bool isBudgSeg;
        public Dictionary<string, DoDStats> SegStats;
        private Vector _polymask;
        private string _fieldname;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rDod"></param>
        /// <param name="rErr"></param>
        /// <param name="theStats"></param>
        public GetDoDPropStats(Raster rDod, Raster rErr, DoDStats theStats) :
            base(new List<Raster> { rDod, rErr })
        {
            Stats = theStats;
            isBudgSeg = false;
        }

        /// <summary>
        /// Budget Seggregation Constructor
        /// </summary>
        /// <param name="rDod"></param>
        /// <param name="rErr"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetDoDPropStats(Raster rDod, Raster rErr, DoDStats theStats, Vector PolygonMask,
            string FieldName) :
            base(new List<Raster> { rDod, rErr })
        {
            Stats = theStats;
            isBudgSeg = true;
            SegStats = new Dictionary<string, DoDStats>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override float CellOp(List<float[]> data, int id)
        {
            // Speed things up by ignoring nodatas
            if (data[0][id] == _rasternodatavals[0])
                return 0;

            if (isBudgSeg)
                BudgetSegCellOp(data, id);
            else
                CellChangeCalc(data, id, Stats);

            // We need to return something. Doesn't matter what
            return 0;
        }

        /// <summary>
        /// The budget seggregator looks to see if a cell is inside any of the features
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private void BudgetSegCellOp(List<float[]> data, int id)
        {
            decimal[] ptcoords = ChunkExtent.Id2XY(id);
            List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _fieldname);
            if (shapes.Count > 0)
            {
                foreach (string fldVal in shapes)
                {
                    if (!SegStats.ContainsKey(fldVal))
                        SegStats[fldVal] = new DoDStats(Stats);

                    CellChangeCalc(data, id, SegStats[fldVal]);
                }
            }
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
            fPropErr = data[1][id];

            // Deposition
            if (fDoDValue > 0)
            {
                // Raw Deposition
                stats.DepositionRaw.AddToSumAndIncrementCounter(fDoDValue);

                if (fDoDValue > fPropErr)
                {
                    // Thresholded Deposition
                    stats.DepositionThr.AddToSumAndIncrementCounter(fDoDValue);
                    stats.DepositionErr.AddToSumAndIncrementCounter(fPropErr);
                }
            }
            // Erosion
            if (fDoDValue < 0)
            {
                // Raw Erosion
                stats.ErosionRaw.AddToSumAndIncrementCounter(fDoDValue - 1);

                if (fDoDValue < (fPropErr - 1))
                {
                    // Thresholded Erosion
                    stats.ErosionThr.AddToSumAndIncrementCounter(fDoDValue * -1);
                    stats.ErosionErr.AddToSumAndIncrementCounter(fPropErr);
                }
            }
        }
    }
}
