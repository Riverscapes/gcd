﻿using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;

namespace GCDConsoleLib.Internal.Operators
{

    public class BinRaster : CellByCellOperator<double>
    {
        public Histogram theHistogram;

        // If we do budget seg we need the following
        private bool isBudgSeg;
        public Dictionary<string, Histogram> SegHistograms;
        private Vector _polymask;
        private string _fieldname;
        private int _segNumBins;

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public BinRaster(Raster rInput, int numBins) :
            base(new List<Raster> { rInput })
        {
            theHistogram = new Histogram(numBins, rInput);
            isBudgSeg = false;
        }

        /// <summary>
        /// Budget Seggregation constructor
        /// </summary>
        public BinRaster(Raster rInput, int numBins, Vector PolygonMask,
            string FieldName) :
            base(new List<Raster> { rInput })
        {
            isBudgSeg = true;
            SegHistograms = new Dictionary<string, Histogram>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
            _segNumBins = numBins;
        }

        protected override double CellOp(List<double[]> data, int id)
        {
            if (!data[0][id].Equals(_rasternodatavals[0]))
                if (isBudgSeg)
                    BudgetSegCellOp(data, id);
                else
                    theHistogram.AddBinVal(data[0][id]);

            // We need to return something. Doesn't matter what
            return 0;
        }

        /// <summary>
        /// The budget seggregator looks to see if a cell is inside any of the features
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        private void BudgetSegCellOp(List<double[]> data, int id)
        {
            Tuple<decimal, decimal> ptcoords = ChunkExtent.Id2YX(id);
            List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords.Item2, (double)ptcoords.Item1, _fieldname);
            if (shapes.Count > 0)
            {
                foreach (string fldVal in shapes)
                {
                    if (!SegHistograms.ContainsKey(fldVal))
                        SegHistograms[fldVal] = new Histogram(_segNumBins, _rasters[0]);

                    SegHistograms[fldVal].AddBinVal(data[0][id]);
                }

            }
        }

    }
}
