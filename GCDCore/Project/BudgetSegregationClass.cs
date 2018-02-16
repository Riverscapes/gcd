using System.IO;
using GCDConsoleLib.GCD;
using System.Xml;

namespace GCDCore.Project
{
    public class BudgetSegregationClass : GCDProjectItem
    {
        public readonly HistogramPair Histograms;
        public readonly FileInfo SummaryXML;
        public readonly DoDStats Statistics;

        public BudgetSegregationClass(string name, DoDStats stats, HistogramPair histograms, FileInfo summaryXML)
            : base(name)
        {
            Statistics = stats;
            Histograms = histograms;
            SummaryXML = summaryXML;
        }

        public BudgetSegregationClass(XmlNode nodClass)
            : base(nodClass.SelectSingleNode("Name").InnerText)
        {
            FileInfo rawHist = ProjectManager.Project.GetAbsolutePath(nodClass.SelectSingleNode("RawHistogram").InnerText);
            FileInfo thrHist = ProjectManager.Project.GetAbsolutePath(nodClass.SelectSingleNode("ThrHistogram").InnerText);

            Histograms = new HistogramPair(rawHist, thrHist);
            SummaryXML = ProjectManager.Project.GetAbsolutePath(nodClass.SelectSingleNode("SummaryXML").InnerText);
            Statistics = DoDBase.DeserializeStatistics(nodClass.SelectSingleNode("Statistics"), ProjectManager.Project.CellArea, ProjectManager.Project.Units);
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodClass = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Class"));
            nodClass.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodClass.AppendChild(nodParent.OwnerDocument.CreateElement("RawHistogram")).InnerText = ProjectManager.Project.GetRelativePath(Histograms.Raw.Path);
            nodClass.AppendChild(nodParent.OwnerDocument.CreateElement("ThrHistogram")).InnerText = ProjectManager.Project.GetRelativePath(Histograms.Thr.Path);
            nodClass.AppendChild(nodParent.OwnerDocument.CreateElement("SummaryXML")).InnerText = ProjectManager.Project.GetRelativePath(SummaryXML);
            XmlNode nodStatistics = nodClass.AppendChild(nodParent.OwnerDocument.CreateElement("Statistics"));
            DoDBase.SerializeDoDStatistics(nodParent.OwnerDocument, nodStatistics, Statistics);
        }
    }
}
