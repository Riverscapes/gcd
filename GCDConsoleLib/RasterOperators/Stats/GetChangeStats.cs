using System;
using System.Collections.Generic;
using System.Linq;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class GetChangeStats : CellByCellOperator<double>
    {
        public DoDStats Stats;
        private List<double> _nodata;
        private string _fieldname;

        // If we do budget seg we need the following
        public Dictionary<string, DoDStats> SegStats;

        // When we use rasterized polygons we use this as the field vals
        private Dictionary<int, string> _rasterVectorFieldVals;

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="theStats"></param>
        public GetChangeStats(Raster rInput, DoDStats theStats) :
            base(new List<Raster> { rInput })
        {
            Stats = theStats;
            _nodata = inNodataVals;
        }

        /// <summary>
        /// This is the propError constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rMask"></param>
        /// <param name="theStats"></param>
        public GetChangeStats(Raster rInput, Raster rMask, DoDStats theStats) :
            base(new List<Raster> { rInput, rMask })
        {
            Stats = theStats;
        }

        /// <summary>
        /// Budget Seggregation constructor using a VECTOR mask only
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetChangeStats(Raster rInput, DoDStats theStats, Vector PolygonMask, string FieldName) :
            base(new List<Raster> { rInput }, PolygonMask)
        {
            // Note hwo the polymask gets passed to the base. This is so we can filter to shapes that
            // overlap the chunks to speed things up.
            Stats = theStats;
            SegStats = new Dictionary<string, DoDStats>();
            _fieldname = FieldName;
        }

        /// <summary>
        /// This is the Budget Seggregation with propError Constructor using a VECTOR mask only
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rPropError"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetChangeStats(Raster rInput, Raster rPropError, DoDStats theStats, Vector PolygonMask, string FieldName) :
           base(new List<Raster> { rInput, rPropError }, PolygonMask)
        {
            // Note hwo the polymask gets passed to the base. This is so we can filter to shapes that
            // overlap the chunks to speed things up.
            Stats = theStats;
            SegStats = new Dictionary<string, DoDStats>();
            _fieldname = FieldName;
        }

        /// <summary>
        /// Budget Seggregation constructor using a raster mask
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetChangeStats(Raster rInput, DoDStats theStats, VectorRaster rPolygonMask, 
            string FieldName) :
            base(new List<Raster> { rInput }, rPolygonMask)
        {
            // Note how we don't pass the vector into the base. We're not going to do anything
            // with the geometry of the shapefile. 
            Stats = theStats;
            SegStats = new Dictionary<string, DoDStats>();

            _rasterVectorFieldVals = rPolygonMask.FieldValues;
        }

        /// <summary>
        /// This is the Budget Seggregation with propError Constructor using a raster mask
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rPropError"></param>
        /// <param name="theStats"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public GetChangeStats(Raster rInput, Raster rPropError, DoDStats theStats,
            VectorRaster rPolygonMask, string FieldName) :
           base(new List<Raster> { rInput, rPropError }, rPolygonMask)
        {
            Stats = theStats;
            SegStats = new Dictionary<string, DoDStats>();

            _rasterVectorFieldVals = rPolygonMask.FieldValues;

        }

        #endregion

        #region Functions

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
        /// When you want to use a rasterized vector instead
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
            double fRVal, fMask;

            int rasterCount = data.Count;
            // We need to discount the rastermask
            if (_hasRasterizedPolymask) rasterCount--;

            // If we don't have a mask to use then do it this way
            if (rasterCount == 1 && data[0][id] != inNodataVals[0])
            {
                fRVal = data[0][id];
                // Deposition
                if (fRVal > 0)
                    stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                // Erosion
                else if (fRVal < 0)
                    stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal * -1);
            }

            // If we have an error mask then use it.
            else if (rasterCount == 2 && data[1][id] != inNodataVals[1])
            {
                fRVal = data[0][id];
                fMask = data[1][id];
                if (fRVal > 0)
                {
                    if (fMask != inNodataVals[1])
                    {
                        if (fMask > 0) // Deposition
                            stats.DepositionRaw.AddToSumAndIncrementCounter(fRVal);
                        else if (fMask < 0) // Erosion
                            stats.ErosionRaw.AddToSumAndIncrementCounter(fRVal);
                    }
                }
            }
        }
        #endregion
    }
}
