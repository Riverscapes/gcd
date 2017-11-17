using System.IO;
using GCDConsoleLib.GCD;

namespace GCDCore.Project
{
    public class BudgetSegregationClass : GCDProjectItem
    {
        public readonly DoDStats Statistics;
        public readonly FileInfo RawHistogram;
        public readonly FileInfo ThrHistogram;
        public readonly FileInfo SummaryXML;

        public BudgetSegregationClass(string name, DoDStats stats, FileInfo rawHist, FileInfo thrHist, FileInfo summaryXML)
            : base(name)
        {
            Statistics = stats;
            RawHistogram = rawHist;
            ThrHistogram = thrHist;
            SummaryXML = summaryXML;
        }
    }
}
