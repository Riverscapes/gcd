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
        private List<float> _nodata;

        // If we do budget seg we need the following
        private bool isBudgSeg;
        private Dictionary<string, DoDStats> SegStats;
        private Vector _polymask;
        private string _fieldname;

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
        /// Pass-through constructor
        /// </summary>
        public GetChangeStats(Raster rInput, DoDStats theStats, Vector PolygonMask, string FieldName) :
            base(new List<Raster> { rInput })
        {
            SegStats = new Dictionary<string, DoDStats>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(List<float[]> data, int id)
        {
            if (bSeg)
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
            Tuple<decimal, decimal> ptcoords = ChunkExtent.Id2YX(id);
            List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords.Item2, (double)ptcoords.Item1, _fieldname);
            if (shapes.Count > 0)
            {
                foreach (string shp in shapes)
                {
                    if (!SegStats.ContainsKey(shp))
                        SegStats[shp] = new DoDStats(Stats);

                    DoDStats myStats = SegStats[shp];

                    CellChangeCalc(data, id, myStats);
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
