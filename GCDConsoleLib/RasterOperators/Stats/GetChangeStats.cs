using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetChangeStats : CellByCellOperator<double>
    {
        public DoDStats Stats;
        private List<double> _nodata;

        // If we do budget seg we need the following
        private bool isBudgSeg;
        public Dictionary<string, DoDStats> SegStats;
        private Vector _polymask;
        private string _fieldname;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="theStats"></param>
        public GetChangeStats(Raster rInput, DoDStats theStats) :
            base(new List<Raster> { rInput })
        {
            Stats = theStats;
            isBudgSeg = false;
            _nodata = inNodataVals;
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
            isBudgSeg = false;
        }

        /// <summary>
        /// Budget Seggregation constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetChangeStats(Raster rInput, DoDStats theStats, Vector PolygonMask, string FieldName) :
            base(new List<Raster> { rInput })
        {
            Stats = theStats;
            isBudgSeg = true;
            SegStats = new Dictionary<string, DoDStats>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
        }

        /// <summary>
        /// This is the Budget Seggregation with propError Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rPropError"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetChangeStats(Raster rInput, Raster rPropError, DoDStats theStats, Vector PolygonMask, string FieldName) :
           base(new List<Raster> { rInput, rPropError })
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
        protected override void CellOp(List<double[]> data, List<double[]> outputs, int id)
        {
            // Speed things up by ignoring nodatas
            if (data[0][id] == inNodataVals[0])
                return;

            if (isBudgSeg)
                BudgetSegCellOp(data, id);
            else
                CellChangeCalc(data, id, Stats);

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
            double fRVal, fMask;

            // If we don't have a mask to use then do it this way
            if (data.Count == 1 && data[0][id] != inNodataVals[0])
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
            else if (data.Count == 2 && data[1][id] != inNodataVals[1])
            {
                fRVal = data[0][id];
                fMask = data[1][id];
                if (fRVal > 0)
                {
                    // Deposition
                    if (fMask != inNodataVals[1])
                        stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                    // Erosion
                    else if (fMask< 0)
                        stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal);
                }
            }
        }
    }
}
