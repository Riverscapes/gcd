using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib
{
    /// <summary>
    /// Produces a Zero-centered Unitless histogram.
    /// </summary>
    public class Histogram
    {
        public List<int> BinCounts;
        private List<decimal> _binLefts;
        private List<decimal> _binSums;

        public int Count { get { return BinCounts.Count; } }
        public decimal _binWidth;

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
        public Histogram(int bins, Raster rRa)
        {
            rRa.ComputeStatistics();
            Dictionary<string, decimal> stats = rRa.GetStatistics();
            Tuple<int, decimal> newDims;

            // No point calculating stats if there are no bins
            if (stats["max"] == stats["min"])
                if (stats["max"] == 0)
                    newDims = new Tuple<int, decimal>(1, 1);
                else
                    newDims = new Tuple<int, decimal>(1, Math.Abs(stats["max"]));
            else
                newDims = GetCleanBins(bins, stats["max"], stats["min"]);

            _init(newDims.Item1, newDims.Item2);
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
            _binWidth = width;
            int bins = (int)Math.Ceiling(Math.Max(Math.Abs(max), Math.Abs(min))) * 2;
            _init(bins, width);
        }

        /// <summary>
        /// This constructor loads a Histogram from a file.
        /// </summary>
        /// <param name="outputPath"></param>
        public Histogram(FileInfo histPath)
        {
            if (!histPath.Exists)
                throw new FileNotFoundException("Histogram file could not be found", histPath.FullName);

            List<string[]> sLines = new List<string[]>();
            using (var reader = new StreamReader(histPath.FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    sLines.Add(line.Split(','));
                }
            }
            if (sLines.Count < 2)
                throw new FileLoadException("Histogram file did not have the correct number of lines", histPath.FullName);

            decimal width;
            try
            {
                width = Convert.ToDecimal(sLines[1][1]) - Convert.ToDecimal(sLines[1][0]);
            }
            catch (Exception e)
            {
                throw new FileLoadException("Error loading histogram bin width", e);
            }

            // Initialize the histogram with the right number of bins
            _init(sLines.Count - 1, width);

            // We start on line 1 with the histogram and fill the values we need from that
            for (int lid = 0; lid < sLines.Count - 1; lid++)
            {
                BinCounts[lid] = Convert.ToInt32(sLines[lid + 1][5]);
                _binLefts[lid] = Convert.ToDecimal(sLines[lid + 1][0]);
                _binSums[lid] = Convert.ToDecimal(sLines[lid + 1][6]);
            }
        }

        /// <summary>
        /// Initialize this histogram with the right number of bins
        /// </summary>
        /// <param name="bins"></param>
        /// <param name="width"></param>
        private void _init(int bins, decimal width)
        {
            if (bins == 0)
                throw new ArgumentOutOfRangeException("Number of bins cannot be zero");

            _binWidth = width;

            // Must be a multiple of 2. Add a bin
            if (bins % 2 == 1) bins++;
            BinCounts = new List<int>();
            _binLefts = new List<decimal>();
            _binSums = new List<decimal>();

            for (int bid = 0; bid < bins; bid++)
            {
                BinCounts.Add(0);
                _binLefts.Add(width * (bid - (bins / 2)));
                _binSums.Add(0);
            }
        }

        /// <summary>
        /// Get a set of bins that closely approximates the number asked for while tryng to put
        /// the bin divisions in sensible places
        /// </summary>
        /// <param name="origBins"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static Tuple<int, decimal> GetCleanBins(int origBins, decimal max, decimal min)
        {
            if (origBins == 0)
                throw new ArgumentOutOfRangeException("Number of bins cannot be zero.");

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
        /// <param name="startWidth"></param>
        /// <returns></returns>
        public static decimal GetNearestFiveOrderWidth(decimal startWidth)
        {
            // Special case. Constant rasters will generate this.
            if (startWidth == 0) return 0;

            int order = (int)Math.Round(Math.Log10((double)startWidth));
            decimal tener = (decimal)Math.Pow(10, order);

            Dictionary<decimal, decimal> compares = new Dictionary<decimal, decimal>()
            {
                {tener, Math.Abs(tener - startWidth) },
                {(tener/2), Math.Abs((tener/2) - startWidth) },
                {(tener * 5),  Math.Abs((tener * 5) - startWidth) },
            };
            return compares.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
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
            if (decVal < _binLefts[0] || decVal > _binLefts[_binLefts.Count - 1] + _binWidth)
                bid = -1;
            // Top value is an exception and goes in the topmost bin
            else if (decVal == _binLefts[_binLefts.Count - 1] + _binWidth)
                bid = BinCounts.Count - 1;
            else
                for (bid = 0; decVal >= _binLefts[bid] + _binWidth && bid < BinCounts.Count - 1; bid++) ;
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
            _binSums[bid] += (decimal)val;
        }

        /// <summary>
        /// These are helpful functions to figure stuff out.
        /// </summary>
        public int FirstBinId { get { return 0; } }
        public int LastBinId { get { return _binLefts.Count - 1; } }

        public Length BinLower(decimal val, UnitGroup unitg)
        {
            return BinLower(BinId((double)val), unitg);
        }
        public Length BinLower(int id, UnitGroup unitg)
        {
            return Length.From((double)_binLefts[id], unitg.VertUnit);
        }

        public Length BinUpper(decimal val, UnitGroup unitg)
        {
            return BinUpper(BinId((double)val), unitg);
        }
        public Length BinUpper(int id, UnitGroup unitg)
        {
            return Length.From((double)(_binLefts[id] + _binWidth), unitg.VertUnit);
        }

        public Length BinCentre(decimal val, UnitGroup unitg)
        {
            return BinCentre(BinId((double)val), unitg);
        }
        public Length BinCentre(int id, UnitGroup unitg)
        {
            return Length.From((double)(_binLefts[id] + _binWidth / 2), unitg.VertUnit);
        }

        public Length HistogramLower(UnitGroup unitg)
        {
            return Length.From((double)_binLefts[0], unitg.VertUnit);
        }
        public Length HistogramUpper(UnitGroup unitg)
        {
            return Length.From((double)(_binLefts.Last() + _binWidth), unitg.VertUnit);
        }

        public Length BinWidth(UnitGroup unitg) {
            return Length.From((double)(_binWidth), unitg.VertUnit);
        }

            
        /// <summary>
        /// Return the sum of values (used to make the volume) in length units
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        private decimal BinSum(int bid)
        {
            return _binSums[bid];
        }

        /// <summary>
        /// Get the Volume of one bin in volumetric units
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public Volume BinVolume(int bid, Area cellArea, UnitGroup unitg)
        {
            return Volume.FromCubicMeters(cellArea.SquareMeters * Length.From((double)BinSum(bid), unitg.VertUnit).Meters);
        }

        /// <summary>
        /// Simple area calculation
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="cellArea"></param>
        /// <returns></returns>
        public Area BinArea(int bid, Area cellArea)
        {
            return Area.FromSquareMeters(BinCounts[bid] * cellArea.SquareMeters);
        }
        
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
        public void WriteFile(FileInfo outputFile, Area cellArea, UnitGroup theUnits)
        {
            using (StreamWriter stream = new StreamWriter(outputFile.FullName))
            {
                stream.WriteLine(string.Format("Bin Lower,Bin Upper,Bin Centre,Area({0}),Volume({1}),Cell Count,Cell Sum({2})", theUnits.ArUnit, theUnits.VolUnit, theUnits.VertUnit));
                for (int bid = 0; bid < BinCounts.Count; bid++)
                {
                    // This is unforunate but there's a casting problem between double and decimals at the highest decimal values.
                    double bLower = BinLower(bid, theUnits).As(theUnits.VertUnit);
                    double bUpper = BinUpper(bid, theUnits).As(theUnits.VertUnit);
                    double bCenter = BinCentre(bid, theUnits).As(theUnits.VertUnit);

                    decimal bLowerDec, bUpperDec, bCenterDec;
                    try { bLowerDec = (decimal)bLower; }
                    catch (OverflowException) { bLowerDec = bLower > 0 ? decimal.MaxValue : decimal.MinValue; }

                    try { bUpperDec = (decimal)bUpper; }
                    catch (OverflowException) { bUpperDec = bUpper > 0 ? decimal.MaxValue : decimal.MinValue; }

                    try { bCenterDec = (decimal)bCenter; }
                    catch (OverflowException) { bCenterDec = bCenter > 0 ? decimal.MaxValue : decimal.MinValue; }

                    double vol = BinVolume(bid, cellArea, theUnits).As(theUnits.VolUnit);
                    string binstr = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                        bLowerDec,
                        bUpper,
                        bCenter,
                        cellArea, vol, BinCounts[bid], _binSums[bid]);
                    stream.WriteLine(binstr);
                }
            }
        }

    }

}
