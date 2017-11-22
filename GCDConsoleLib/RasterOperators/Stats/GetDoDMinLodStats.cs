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

        // If we do budget seg we need the following
        private bool isBudgSeg;
        public Dictionary<string, DoDStats> SegStats;
        private Vector _polymask;
        private string _fieldname;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public GetDodMinLodStats(Raster rawDoD, Raster thrDoD, float thresh, DoDStats theStats) :
            base(new List<Raster> { rawDoD, thrDoD })
        {
            Stats = theStats;
            _thresh = thresh;
            isBudgSeg = false;
        }


        /// <summary>
        /// Budget Seggregation constructor
        /// </summary>
        public GetDodMinLodStats(Raster rawDoD, Raster thrDoD,
            float thresh, DoDStats theStats, Vector PolygonMask, string FieldName) :
           base(new List<Raster> { rawDoD, thrDoD })
        {
            Stats = theStats;
            _thresh = thresh;
            SegStats = new Dictionary<string, DoDStats>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
            isBudgSeg = true;
        }


        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(List<float[]> data, int id)
        {
            // Speed things up by ignoring nodatas
            if (data[0][id] == _rasternodatavals[0])
                return 0;

            if (isBudgSeg)
                BudgetSegCellOp(data, id);
            else
                CellChangeCalc(data, id, Stats);
            // We need to return something
            return 0;
        }

        /// <summary>
        /// The budget seggregator looks to see if a cell is inside any of the features
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private void BudgetSegCellOp(List<float[]> data, int id)
        {
            Tuple<decimal, decimal> ptcoords = ChunkExtent.Id2XY(id);
            List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords.Item1, (double)ptcoords.Item2, _fieldname);
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
