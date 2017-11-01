using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib
{
    /// <summary>
    /// Produces a Zero-centered Unitless histogram.
    /// </summary>
    public class Histogram
    {
        public List<int> BinCounts;
        public List<decimal> BinLefts;
        public List<decimal> BinSums;

        public int Count { get { return BinCounts.Count; } }
        public decimal BinWidth { get; set; }

        /// <summary>
        /// When we know the # of bins and the width of the bin
        /// </summary>
        /// <param name="bins"></param>
        /// <param name="width"></param>
        /// <param name="cellHeight"></param>
        /// <param name="cellWidth"></param>
        /// <param name="vUnit"></param>
        /// <param name="hUnit"></param>
        public Histogram(int bins, decimal width)
        {
            _init(bins, width);
        }

        /// <summary>
        /// Constructing this object from a raster and a number of bins
        /// </summary>
        /// <param name="bins"></param>
        /// <param name="rRa"></param>
        public Histogram(int bins, ref Raster rRa)
        {
            rRa.ComputeStatistics();
            Dictionary<string, decimal> stats = rRa.GetStatistics();

            Tuple<int, decimal> newDims = GetCleanBins(bins, stats["max"], stats["min"]);
            _init(newDims.Item1, newDims.Item2);
        }

        public static Tuple<int, decimal> GetCleanBins(int origBins, decimal max, decimal min)
        {
            decimal oneSideDataWidth = Math.Max(Math.Abs(max), Math.Abs(min));
            decimal startwidth = (oneSideDataWidth * 2) / origBins;

            // First clean the width to the nearest 5 or 10 power
            decimal newWidth = GetNearestFiveOrderWidth(startwidth);
            // Now re-adjust the bins to match
            int newBins = (int)Math.Ceiling((oneSideDataWidth * 2) / newWidth);

            return new Tuple<int, decimal>(newBins, newWidth);
        }

        /// <summary>
        /// Choose a clean division that is a muliple of 5 or 10
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal GetNearestFiveOrderWidth(decimal val)
        {
            int order = (int)Math.Round(Math.Log10((double)val));
            decimal tener = (decimal)Math.Pow(10, order);

            Dictionary<decimal, decimal> compares = new Dictionary<decimal, decimal>()
            {
                {tener, (decimal)Math.Abs(tener - val) },
                {(tener/2), (decimal) Math.Abs((tener/2) - val) },
                {(tener * 5),  (decimal)Math.Abs((tener * 5) - val) },
            };
            return compares.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
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
        public Histogram(decimal width, decimal max, decimal min)
        {
            BinWidth = width;
            int bins = (int)Math.Ceiling(Math.Max(Math.Abs(max), Math.Abs(min))) * 2;
            _init(bins, width);
        }

        /// <summary>
        /// This constructor loads a Histogram from a file.
        /// </summary>
        /// <param name="outputPath"></param>
        //public Histogram(FileInfo histPath)
        //{
        //    if (!histPath.Exists)
        //        throw new FileNotFoundException("Histogram file could not be found", histPath.FullName);

        //    List<string[]> sLines = new List<string[]>();
        //    using (var reader = new StreamReader(histPath.FullName))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            string line = reader.ReadLine();
        //            sLines.Add(line.Split(','));
        //        }
        //    }
        //    if (sLines.Count < 4)
        //        throw new FileLoadException("Histogram file did not have the correct number of lines", histPath.FullName);

        //    decimal width, cellHeight, cellWidth;
        //    LengthUnit vUnit, hUnit;
        //    try
        //    {
        //        width = Convert.ToDecimal(sLines[3][1]) - Convert.ToDecimal(sLines[3][0]);
        //        cellHeight = Convert.ToDecimal(sLines[1][0]);
        //        cellWidth = Convert.ToDecimal(sLines[1][1]);
        //        vUnit = Length.ParseUnit(sLines[1][2]);
        //        hUnit = Length.ParseUnit(sLines[1][3]);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new FileLoadException("Error loading histogram metadata", e);
        //    }

        //    _init(sLines.Count - 3, width, cellHeight, cellWidth, vUnit, hUnit);

        //    // We start on line 4 with the histogram
        //    for (int lid = 3; lid < sLines.Count; lid++)
        //    {
        //        _dlcounts[lid] = Convert.ToInt32(sLines[lid][5]);
        //        _dlbinlefts[lid] = Convert.ToDecimal(sLines[lid][0]);
        //        _dlbinVols[lid] = Convert.ToDecimal(sLines[lid][4]);
        //    }            
        //}

        private void _init(int bins, decimal width)
        {
            BinWidth = width;

            // Must be a multiple of 2. Add a bin
            if (bins % 2 == 1) bins++;

            BinCounts = new List<int>();
            BinLefts = new List<decimal>();
            BinSums = new List<decimal>();

            for (int bid = 0; bid < bins; bid++)
            {
                BinCounts.Add(0);
                BinLefts.Add(width * (bid - (bins / 2)));
                BinSums.Add(0);
            }
        }

        /// <summary>
        /// Get a bin ID for a given value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int BinId(double val)
        {
            int bid;
            decimal decVal = (decimal)val;
            if (decVal < BinLefts[0] || decVal > BinLefts[BinLefts.Count - 1] + BinWidth)
                bid = -1;
            // Top value is an exception and goes in the topmost bin
            else if (decVal == BinLefts[BinLefts.Count - 1] + BinWidth)
                bid = BinCounts.Count - 1;
            else
                for (bid = 0; decVal >= BinLefts[bid] + BinWidth && bid < BinCounts.Count - 1; bid++) ;
            return bid;
        }

        /// <summary>
        /// Add a value to the bin
        /// </summary>
        /// <param name="val"></param>
        public void AddBinVal(double val)
        {
            int bid = BinId(val);

            // Out of bounds is not allowed
            if (bid < 0)
                throw new ArgumentOutOfRangeException("Trying to bin a value outside the histogram range");

            BinCounts[bid]++;
            BinSums[bid] += (decimal)val;
        }



        /// <summary>
        /// These are helpful functions to figure stuff out.
        /// </summary>
        public int FirstBinId { get { return 0; } }
        public int LastBinId { get { return BinLefts.Count - 1; } }

        public decimal BinLower(decimal val) { return BinLower(BinId((double)val)); }
        public decimal BinLower(int id) { return BinLefts[id]; }

        public decimal BinUpper(decimal val) { return BinUpper(BinId((double)val)); }
        public decimal BinUpper(int id) { return BinLefts[id] + BinWidth; }

        public decimal BinCentre(decimal val) { return BinCentre(BinId((double)val)); }
        public decimal BinCentre(int id) { return BinLefts[id] + BinWidth / 2; }

        public decimal HistogramLower { get { return BinLefts[0]; } }
        public decimal HistogramUpper { get { return BinLefts.Last() + BinWidth; } }

        /// <summary>
        /// Get the bin Area in Area units
        /// TODO: MOVE THIS OUT OF THIS PROJECT. IT NEEDS UNITS
        /// </summary>
        //public Area BinArea
        //{
        //    get
        //    {
        //        return Area.From((double)(Math.Abs(_cellHeight) * Math.Abs(_cellWidth)), Conversion.LengthUnit2AreaUnit(HorizontalUnit));
        //    }
        //}

        /// <summary>
        /// Return the sum of values (used to make the volume) in length units
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        private decimal BinSum(int bid)
        {
            return BinSums[bid];
        }

        /// <summary>
        /// Get the Volume of one bin in volumetric units
        /// TODO: MOVE THIS OUT OF THIS PROJECT. IT NEEDs UNITS
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        //public Volume BinVolume(int bid)
        //{
        //    return Volume.FromCubicMeters(BinArea.SquareMeters * BinSum(bid).Meters);
        //}

        /// <summary>
        /// Return a count at a given bin
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public int BinCount(int bid)
        {
            return BinCounts[bid];
        }

        /// <summary>
        /// Write the Histogram to a file
        /// </summary>
        /// <param name="outputPath"></param>
        /// This is no longer here MOVE IT TO THE PRESENTATION LAYER
        //public void WriteFile(string outputPath)
        //{
        //    using (System.IO.StreamWriter stream = new System.IO.StreamWriter(outputPath))
        //    {
        //        stream.WriteLine(String.Format("Cell Height, Cell Width, Vertical Unit, Horizontal", VerticalUnit.ToString(), HorizontalUnit.ToString()));
        //        stream.WriteLine(String.Format("{0},{1},{2},{3}", _cellHeight, _cellWidth, VerticalUnit.ToString(), HorizontalUnit.ToString()));
        //        stream.WriteLine("Bin Lower,Bin Upper,Bin Centre,Area,Volume,Cell Count");
        //        for (int bid = 0; bid < _dlcounts.Count; bid++)
        //        {
        //            string binstr = string.Format("{0},{1},{2},{3},{4},{5}", BinLower(bid), BinUpper(bid), BinCentre(bid), BinArea, BinVolume(bid), _dlcounts[bid]);
        //            stream.WriteLine(binstr);
        //        }
        //    }
        //}

    }

}
