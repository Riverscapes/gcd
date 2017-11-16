using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.BudgetSegregation
{
    public class BSResult
    {
        public string ClassName { get; internal set; }
        public int ClassIndex { get; internal set; }
        public GCDConsoleLib.GCD.DoDStats ChangeStats { get; internal set; }
        public DirectoryInfo Folder { get; internal set; }

        public string ClassFilePrefix { get { return string.Format("{0:000}", ClassIndex); } }
        public FileInfo SummaryXMLPath { get { return GetOutputPath(Folder, ClassIndex, "summary.xml"); } }
        public FileInfo RawHistogramPath { get { return GetOutputPath(Folder, ClassIndex, "raw.csv"); } }
        public FileInfo ThrHistogramPath { get { return GetOutputPath(Folder, ClassIndex, "thresholded.csv"); } }

        // Private histograms. Can be passed in constructor or loaded on demand.
        private GCDConsoleLib.Histogram _RawHistogram { get; set; }
        private GCDConsoleLib.Histogram _ThrHistogram { get; set; }

        public GCDConsoleLib.Histogram RawHistogram
        {
            get
            {
                if (_RawHistogram == null)
                {
                    _RawHistogram = new GCDConsoleLib.Histogram(RawHistogramPath);
                }

                return _RawHistogram;
            }
        }

        public GCDConsoleLib.Histogram ThrHistogram
        {
            get
            {
                if (_ThrHistogram == null)
                {
                    _ThrHistogram = new GCDConsoleLib.Histogram(RawHistogramPath);
                }

                return _ThrHistogram;
            }
        }
 
        public BSResult(DirectoryInfo Folder, string name, int classIndex, GCDConsoleLib.GCD.DoDStats stats)
        {
            ClassName = name;
            ClassIndex = classIndex;
            ChangeStats = stats;
        }

        public BSResult(DirectoryInfo folder, string name, int classIndex, GCDConsoleLib.GCD.DoDStats stats, GCDConsoleLib.Histogram rawHisto, GCDConsoleLib.Histogram thrHisto)
        {
            Init(folder, name, classIndex, stats);

            _RawHistogram = rawHisto;
            _ThrHistogram = thrHisto;
        }

        private void Init(DirectoryInfo folder, string name, int classIndex, GCDConsoleLib.GCD.DoDStats stats)
        {
            ClassName = name;
            ClassIndex = classIndex;
            ChangeStats = stats;
            Folder = folder;
        }

        private FileInfo GetOutputPath(DirectoryInfo folder, int classIndex, string FileName)
        {
            return new FileInfo(Path.Combine(folder.FullName, string.Format("{0}_{1}.xml", ClassFilePrefix, FileName)));
        }
    }
}
