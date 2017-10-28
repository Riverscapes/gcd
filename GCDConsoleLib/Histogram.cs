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
        private List<int> _dlcounts;
        private List<double> _dlbinlefts;
        private List<double> _dlbinSums;

        // Private since these are unitless and we don't want anyone using them
        private double _cellHeight;
        private double _cellWidth;
        private double _binWidth;

        public LengthUnit VerticalUnit;
        public LengthUnit HorizontalUnit;

        public int Count { get { return _dlcounts.Count; } }
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
            _binWidth = width;
            VerticalUnit = vUnit;
            HorizontalUnit = hUnit;

            // Must be a multiple of 2. Add a bin
            if (bins % 2 == 1) bins++;

            _dlcounts = new List<int>();
            _dlbinlefts = new List<double>();
            _dlbinSums = new List<double>();

            for (int bid = 0; bid < bins; bid++)
            {
                _dlcounts.Add(0);
                _dlbinlefts.Add(width * (bid - (bins / 2)));
                _dlbinSums.Add(0);
            }
        }

        /// <summary>
        /// Get a bin ID for a given value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int BinId(Length val)
        {
            double dVal = val.As(VerticalUnit);
            int bid = -1;
            if (dVal >= _dlbinlefts[0])
            {  
                for (bid = 0; bid < _dlcounts.Count && _dlbinlefts[bid] + _binWidth < dVal; bid++) ;
                // Error condition
                if (bid >= _dlcounts.Count)
                    bid = -1;
            }
            return bid;
        }

        /// <summary>
        /// Add a value to the bin
        /// </summary>
        /// <param name="val"></param>
        public void AddBinVal(double val)
        {
            int bid;
            for (bid = 0; _dlbinlefts[bid]+_binWidth < val; bid++) ;
            _dlcounts[bid]++;
            _dlbinSums[bid] += val;
        }

        /// <summary>
        /// These are helpful functions to figure stuff out.
        /// </summary>
        public int FirstBinId { get { return 0; } }
        public int LastBinId { get { return _dlbinlefts.Count - 1; } }

        public Length BinLower(Length val) { return BinLower(BinId(val)); }
        public Length BinLower(int id) { return Length.From(_dlbinlefts[id], VerticalUnit); }

        public Length BinUpper(Length val) { return BinUpper(BinId(val)); }
        public Length BinUpper(int id) { return Length.From(_dlbinlefts[id] + _binWidth, VerticalUnit); }

        public Length BinCentre(Length val) { return BinCentre(BinId(val)); }
        public Length BinCentre(int id) { return Length.From(_dlbinlefts[id] + _binWidth / 2, VerticalUnit); }

        public Length HistogramLower { get { return Length.From(_dlbinlefts[0], VerticalUnit); } }
        public Length HistogramUpper { get { return Length.From(_dlbinlefts.Last() + _binWidth, VerticalUnit); } }

        /// <summary>
        /// Get the bin Area in Area units
        /// </summary>
        public Area BinArea
        {
            get
            {
                return Area.From((Math.Abs(_cellHeight) * Math.Abs(_cellWidth)), Conversion.LengthUnit2AreaUnit(HorizontalUnit));
            }
        }

        /// <summary>
        /// Return the sum of values (used to make the volume) in length units
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        private Length BinSum(int bid){
            return Length.From(_dlbinSums[bid], VerticalUnit);
        }

        /// <summary>
        /// Get the Volume of one bin in volumetric units
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public Volume BinVolume(int bid)
        {
            return Volume.FromCubicMeters(BinArea.SquareMeters * BinSum(bid).Meters);
        }

        /// <summary>
        /// Return a count at a given bin
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public int BinCount(int bid)
        {
            return _dlcounts[bid];
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
                for (int bid = 0; bid < _dlcounts.Count; bid++)
                {
                    string binstr = string.Format("{0},{1},{2},{3},{4},{5}", BinLower(bid), BinUpper(bid), BinCentre(bid), BinArea, BinVolume(bid), _dlcounts[bid]);
                    stream.WriteLine(binstr);
                }
            }
        }

    }

}
