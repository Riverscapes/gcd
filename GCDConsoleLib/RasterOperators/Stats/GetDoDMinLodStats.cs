using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetDodMinLodStats : CellByCellOperator<double>
    {
        public DoDStats Stats;
        private double fDoDValue;
        private double _thresh;

        // If we do budget seg we need the following
        private bool isBudgSeg;
        public Dictionary<string, DoDStats> SegStats;
        private Vector _polymask;
        private string _fieldname;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoD"></param>
        /// <param name="thresh"></param>
        /// <param name="theStats"></param>
        public GetDodMinLodStats(Raster rawDoD, Raster thrDoD, decimal thresh, DoDStats theStats) :
            base(new List<Raster> { rawDoD, thrDoD })
        {
            Stats = theStats;
            _thresh = (double)thresh;
            isBudgSeg = false;
        }


        /// <summary>
        /// Budget Seggregation constructor
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoD"></param>
        /// <param name="thresh"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetDodMinLodStats(Raster rawDoD, Raster thrDoD,
            decimal thresh, DoDStats theStats, Vector PolygonMask, string FieldName) :
           base(new List<Raster> { rawDoD, thrDoD })
        {
            Stats = theStats;
            _thresh = (double)thresh;
            SegStats = new Dictionary<string, DoDStats>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
            isBudgSeg = true;
        }


        /// <summary>
        ///  This is the actual implementation of the cell-by-cell logic
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override double CellOp(List<double[]> data, int id)
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
        private void BudgetSegCellOp(List<double[]> data, int id)
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
        public void CellChangeCalc(List<double[]> data, int id, DoDStats stats)
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
