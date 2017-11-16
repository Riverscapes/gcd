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
        public GCDConsoleLib.GCD.DoDStats ChangeStatistics { get; internal set; }

        public readonly FileInfo SummaryXMLPath;
        public readonly FileInfo RawHistogramPath;
        public readonly FileInfo ThrHistogramPath;

        public string ClassFilePrefix {  get { return string.Format("{0:000}", ClassIndex); } }

        public BSResult(DirectoryInfo Folder, string name, int classIndex, GCDConsoleLib.GCD.DoDStats stats)
        {
            ClassName = name;
            ClassIndex = classIndex;
            ChangeStatistics = stats;

            SummaryXMLPath = GetOutputPath(Folder, classIndex, "summary.xml");
            RawHistogramPath = GetOutputPath(Folder, classIndex, "raw.csv");
            ThrHistogramPath = GetOutputPath(Folder, classIndex, "thresholded.csv");
        }

        private FileInfo GetOutputPath(DirectoryInfo folder, int classIndex, string FileName)
        {
            return new FileInfo(Path.Combine(folder.FullName, string.Format("{0}_{1}.xml", ClassFilePrefix, FileName)));
        }
    }
}
