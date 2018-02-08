using System;
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
        ///  Budget Seggregation constructor
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
        /// The budget seggregator looks to see if a cell is inside any of the features
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private void BudgetSegCellOp(List<double[]> data, int id)
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
        /// The actual op on the cell
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<double[]> data, List<double[]> outputs, int id)
        {
            if (!data[0][id].Equals(inNodataVals[0]))
                if (has)
                    BudgetSegCellOp(data, id);
                else
                    theHistogram.AddBinVal(data[0][id]);
        }

    }
}
