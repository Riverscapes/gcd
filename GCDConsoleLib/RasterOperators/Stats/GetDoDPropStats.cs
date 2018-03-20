using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using System.Linq;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetDoDPropStats : CellByCellOperator<double>
    {
        public DoDStats Stats;
        public double fDoDValue, fPropErr;

        // When we use rasterized polygons we use this as the field vals
        private Dictionary<int, string> _rasterVectorFieldVals;

        // If we do budget seg we need the following
        public Dictionary<string, DoDStats> SegStats;
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
            base(new List<Raster> { rDod, rErr }, PolygonMask)
        {
            Stats = theStats;
            SegStats = new Dictionary<string, DoDStats>();
            _fieldname = FieldName;
        }

        /// <summary>
        /// Budget Seggregation Constructor with RASTERized vector mask
        /// </summary>
        /// <param name="rDod"></param>
        /// <param name="rErr"></param>
        /// <param name="theStats"></param>
        /// <param name="rPolymask"></param>
        /// <param name="vPolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetDoDPropStats(Raster rDod, Raster rErr, DoDStats theStats, VectorRaster rPolymask,
            string FieldName) :
            base(new List<Raster> { rDod, rErr }, rPolymask)
        {
            Stats = theStats;
            SegStats = new Dictionary<string, DoDStats>();
            _fieldname = FieldName;

            _rasterVectorFieldVals = rPolymask.FieldValues;

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
            if (data[0][id] == inNodataVals[0] || data[1][id] == inNodataVals[1])
                return;

            // Pure vector method
            if (_hasVectorPolymask)
                VectorBudgetSegCellOp(data, id);

            // Rasterized vector method
            else if (_hasRasterizedPolymask)
                RasterBudgetSegCellOp(data, id);

            // Non budget seg method
            else
                CellChangeCalc(data, id, Stats);
        }

        /// <summary>
        /// The budget seggregator looks to see if a cell is inside any of the features
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private void VectorBudgetSegCellOp(List<double[]> data, int id)
        {
            if (_shapemask.Count > 0)
            {
                decimal[] ptcoords = ChunkExtent.Id2XY(id);
                List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _fieldname, _shapemask);
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
        }

        /// <summary>
        /// Using a Rasterized vector to run a budget seggregation op
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private void RasterBudgetSegCellOp(List<double[]> data, int id)
        {
            double rPolymaskVal = data[_inputRasters.Count - 1][id];
            if (rPolymaskVal != inNodataVals[_inputRasters.Count - 1])
            {
                string fldVal = _rasterVectorFieldVals[(int)rPolymaskVal];
                // Create a new DoDStats object if we don't already have one
                if (!SegStats.ContainsKey(fldVal))
                    SegStats[fldVal] = new DoDStats(Stats);

                CellChangeCalc(data, id, SegStats[fldVal]);
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
                stats.ErosionRaw.AddToSumAndIncrementCounter(fDoDValue * -1);

                if (fDoDValue < (fPropErr * -1))
                {
                    // Thresholded Erosion
                    stats.ErosionThr.AddToSumAndIncrementCounter(fDoDValue * -1);
                    stats.ErosionErr.AddToSumAndIncrementCounter(fPropErr);
                }
            }
        }
    }
}
