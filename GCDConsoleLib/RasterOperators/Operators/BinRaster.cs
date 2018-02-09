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
        private string _fieldname;
        private int _segNumBins;

        // When we use rasterized polygons we use this as the field vals
        private Dictionary<long, string> _rasterVectorFieldVals;

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
        public BinRaster(Raster rInput, int numBins, Raster rPolymask, Vector vPolygonMask,
            string FieldName) :
            base(new List<Raster> { rInput }, rPolymask)
        {
            SegHistograms = new Dictionary<string, Histogram>();
            _fieldname = FieldName;
            _segNumBins = numBins;

            // Pull just the field values out for later retrieval
            _rasterVectorFieldVals = vPolygonMask.Features
                .ToDictionary(d => d.Key, d => d.Value.Feat.GetFieldAsString(FieldName));

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
            if (data[_inputRasters.Count - 1][id] != inNodataVals[_inputRasters.Count - 1])
            {
                string fldVal = _rasterVectorFieldVals[(long)data[_inputRasters.Count - 1][id]];
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

            if (!_hasVectorPolymask)
                theHistogram.AddBinVal(data[0][id]);
            else
            {
                // Budget seggregation can be one of two types
                if (!_hasRasteriszedPolymask)
                    VectorBudgetSegCellOp(data, id);
                else
                    RasterBudgetSegCellOp(data, id);
            }                    
        }

    }
}
