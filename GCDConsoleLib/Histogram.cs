using System;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib
{
    /// <summary>
    /// Produces a Zero-centered Unitfull histogram.
    /// </summary>
    public class Histogram
    {
        private List<double> counts;
        private List<double> binlefts;
        private List<double> binSums;

        // Private since these are unitless and we don't want anyone using them
        private double _cellHeight;
        private double _cellWidth;
        private double _binWidth;

        public LengthUnit VerticalUnit;
        public LengthUnit HorizontalUnit;

        public int BinCount { get { return counts.Count; } }
        public Length BinWidth { get { return Length.From(_binWidth, VerticalUnit);  } }

        /// <summary>
        /// When we know the # of bins and the width of the bin
        /// </summary>
        /// <param name="bins"></param>
        /// <param name="width"></param>
        /// <param name="cellHeight"></param>
        /// <param name="cellWidth"></param>
        /// <param name="vUnit"></param>
        /// <param name="hUnit"></param>
        public Histogram(int bins, double width, double cellHeight, double cellWidth, LengthUnit vUnit, LengthUnit hUnit)
        {
            _init(bins, width, cellHeight, cellWidth, vUnit, hUnit);
        }

        /// <summary>
        /// When we know just the width of the bins, the max and the min
        /// </summary>
        /// <param name="width"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="cellWidth"></param>
        /// <param name="cellHeight"></param>
        /// <param name="vUnit"></param>
        /// <param name="hUnit"></param>
        public Histogram(double width, double max, double min, double cellWidth, double cellHeight, LengthUnit vUnit, LengthUnit hUnit)
        {
            _binWidth = width;
            int bins = (int)Math.Ceiling(Math.Max(Math.Abs(max), Math.Abs(min))) * 2;
            _init(bins, width, cellHeight, cellWidth, vUnit, hUnit);
        }

        /// <summary>
        /// This constructor loads a Histogram from a file.
        /// </summary>
        /// <param name="outputPath"></param>
        public Histogram(string outputPath)
        {
            /**
             * read each line into a dictionary
             * line 2 {CellHeight, CellWidth, Vertical Unit, Horizonal Unit}
             * 
             * Count the bin lines
             * 
             *  _init(lines.Count, lines[1].Left - lines[0].left, cellHeight, cellWidth, vUnit, hUnit);
             * 
             * foreach line in the file:
             *      count[id] = line[count]
             *      binSum[id] = line[count] / line[area]
             *      binLeft[is] = 
             * 
             **/
        }

        private void _init(int bins, double width, double cellHeight, double cellWidth, LengthUnit vUnit, LengthUnit hUnit)
        {
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            VerticalUnit = vUnit;
            HorizontalUnit = hUnit;

            // Must be a multiple of 2. Add a bin
            if (bins % 2 == 1) bins++;

            counts = new List<double>();
            binlefts = new List<double>();

            for (int i = 0; i < bins; i++)
            {
                counts.Add(0);
                binlefts.Add(width * (i - (bins / 2)));
            }
        }

        /// <summary>
        /// Get a bin ID for a given value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int BinId(double val)
        {
            int binind;
            for (binind = 0; binind < val; binind++) ;
            return binind;
        }

        /// <summary>
        /// Add a value to the bin
        /// </summary>
        /// <param name="val"></param>
        public void binVal(double val)
        {
            int idx;
            for (idx = 0; idx < val; idx++) ;
            counts[idx]++;
            binSums[idx] += val;
        }

        /// <summary>
        /// These are helpful functions to figure stuff out.
        /// </summary>
        public int FirstBinId { get { return 0; } }
        public int LastBinId { get { return binlefts.Count - 1; } }

        public double BinLower(double val) { return BinLower(BinId(val)); }
        public double BinLower(int id) { return binlefts[id]; }

        public double BinUpper(double val) { return BinUpper(BinId(val)); }
        public double BinUpper(int id) { return binlefts[id] + _binWidth; }

        public double BinCentre(double val) { return BinCentre(BinId(val)); }
        public double BinCentre(int id) { return binlefts[id] + _binWidth / 2; }

        public double HistogramLower { get { return binlefts[0]; } }
        public double HistogramUpper { get { return binlefts.Last() + _binWidth; } }

        /// <summary>
        /// Get the bin Area in Area units
        /// </summary>
        public Area BinArea
        {
            get
            {
                return Area.From((_cellHeight * _cellWidth), Conversion.LengthUnit2AreaUnit(HorizontalUnit));
            }
        }

        /// <summary>
        /// BinSum just returns the bi
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        private Length BinSum(int binId){
            return Length.From(binSums[binId], VerticalUnit);
        }

        /// <summary>
        /// Get the Volume in volumetric units
        /// </summary>
        /// <param name="binId"></param>
        /// <returns></returns>
        public Volume BinVolume(int binId)
        {
            return Volume.FromCubicMeters(BinArea.SquareMeters * BinSum(binId).Meters);
        }

        /// <summary>
        /// Write the Histogram to a file
        /// </summary>
        /// <param name="outputPath"></param>
        public void WriteFile(string outputPath)
        {
            using (System.IO.StreamWriter stream = new System.IO.StreamWriter(outputPath))
            {
                stream.WriteLine(String.Format("Cell Height, Cell Width, Vertical Unit, Horizontal", VerticalUnit.ToString(), HorizontalUnit.ToString()));
                stream.WriteLine(String.Format("{0},{1},{2},{3}", _cellHeight, _cellWidth, VerticalUnit.ToString(), HorizontalUnit.ToString()));
                stream.WriteLine("Bin Lower,Bin Upper,Bin Centre,Area,Volume,Cell Count");
                for (int bid = 0; bid < counts.Count; bid++)
                {
                    string binstr = string.Format("{0},{1},{2},{3},{4},{5}", BinLower(bid), BinUpper(bid), BinCentre(bid), BinArea, BinVolume(bid), counts[bid]);
                    stream.WriteLine(binstr);
                }
            }
        }

    }

}
