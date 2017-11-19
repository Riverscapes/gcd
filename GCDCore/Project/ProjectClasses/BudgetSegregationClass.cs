using System.IO;
using GCDConsoleLib.GCD;
using System.Xml;

namespace GCDCore.Project
{
    public class BudgetSegregationClass : GCDProjectItem
    {
        public readonly FileInfo RawHistogram;
        public readonly FileInfo ThrHistogram;
        public readonly FileInfo SummaryXML;
        public readonly DoDStats Statistics;

        public BudgetSegregationClass(string name, DoDStats stats, FileInfo rawHist, FileInfo thrHist, FileInfo summaryXML)
            : base(name)
        {
            Statistics = stats;
            RawHistogram = rawHist;
            ThrHistogram = thrHist;
            SummaryXML = summaryXML;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodClass = nodParent.AppendChild(xmlDoc.CreateElement("Class"));
            nodClass.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodClass.AppendChild(xmlDoc.CreateElement("RawHistogram")).InnerText = ProjectManagerBase.GetRelativePath(RawHistogram);
            nodClass.AppendChild(xmlDoc.CreateElement("ThrHistogram")).InnerText = ProjectManagerBase.GetRelativePath(ThrHistogram);
            nodClass.AppendChild(xmlDoc.CreateElement("SummaryXML")).InnerText = ProjectManagerBase.GetRelativePath(SummaryXML);
            DoD.SerializeDoDStatistics(xmlDoc, nodParent, Statistics);
        }
    }
}
