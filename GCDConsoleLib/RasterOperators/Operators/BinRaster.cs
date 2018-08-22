using System;
using System.Linq;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{

    public class BinRaster : CellByCellOperator<double>
    {
        public Histogram theHistogram;

        // If we do budget seg we need the following
        public Dictionary<string, Histogram> SegHistograms;
        private readonly string _fieldname;
        private readonly int _segNumBins;

        // When we use rasterized polygons we use this as the field vals
        private readonly Dictionary<int, string> _rasterVectorFieldVals;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="numBins"></param>
        public BinRaster(Raster rInput, int numBins) :
            base(new List<Raster> { rInput })
        {
            theHistogram = new Histogram(numBins, rInput);
        }

        /// <summary>
        ///  Budget Seggregation constructor using a vector
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="numBins"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="FieldName"></param>
        public BinRaster(Raster rInput, int numBins, Vector PolygonMask,
            string FieldName) :
            base(new List<Raster> { rInput }, PolygonMask)
        {
            SegHistograms = new Dictionary<string, Histogram>();
            _fieldname = FieldName;
            _segNumBins = numBins;
        }

        /// <summary>
        /// Budget Seggregation constructor using a RASTERIZED vector
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="numBins"></param>
        /// <param name="rPolymask"></param>
        /// <param name="vPolygonMask"></param>
        /// <param name="FieldName"></param>
        public BinRaster(Raster rInput, int numBins, VectorRaster rPolymask,
            string FieldName) :
            base(new List<Raster> { rInput }, rPolymask)
        {
            SegHistograms = new Dictionary<string, Histogram>();
            _fieldname = FieldName;
            _segNumBins = numBins;

            _rasterVectorFieldVals = rPolymask.FieldValues;

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
                        if (!SegHistograms.ContainsKey(fldVal))
                            SegHistograms[fldVal] = new Histogram(_segNumBins, _inputRasters[0]);

                        SegHistograms[fldVal].AddBinVal(data[0][id]);
                    }
                }
            }
        }

        /// <summary>
        /// The budget seggregator looks to see if a cell is inside any of the features
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
                if (!SegHistograms.ContainsKey(fldVal))
                    SegHistograms[fldVal] = new Histogram(_segNumBins, _inputRasters[0]);

                SegHistograms[fldVal].AddBinVal(data[0][id]);
            }            
        }

        /// <summary>
        /// The actual op on the cell
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
            else if(_hasRasterizedPolymask)
                RasterBudgetSegCellOp(data, id);

            // Non budget seg method
            else
                theHistogram.AddBinVal(data[0][id]);
        }

    }
}
