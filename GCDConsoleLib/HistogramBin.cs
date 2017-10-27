using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib
{
    public class HistogramBin
    {
        public readonly double BinLower;
        public readonly double BinUpper;
        public readonly double BinCentre;
        public readonly double Area;
        public readonly double Volume;
        public readonly long CellCount;

        public HistogramBin(double fBinLower, double fBinUpper, double fBinCentre, double fArea, double fVolume, long fCellCount)
        {
            BinLower = fBinLower;
            BinUpper = fBinUpper;
            BinCentre = fBinCentre;
            Area = fArea;
            Volume = fVolume;
            CellCount = fCellCount;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", BinLower, BinUpper, BinCentre, Area, Volume, CellCount);
        }

        public static void WriteHistogram(ref Dictionary<double, HistogramBin> histogramData, System.IO.FileInfo outputPath)
        {
            using (System.IO.StreamWriter stream = new System.IO.StreamWriter(outputPath.FullName))
            {
                stream.WriteLine("Bin Lower,Bin Upper,Bin Centre,Area,Volume,Cell Count");
                foreach (HistogramBin bin in histogramData.Values)
                    stream.WriteLine(bin.ToString());
            }
        }
    }
}
