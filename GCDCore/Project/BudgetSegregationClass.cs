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

        public BudgetSegregationClass(string name, DirectoryInfo Folder, string filePrefix, DoDStats stats, HistogramPair histograms, FileInfo summaryXML)
            : base(name)
        {
            Statistics = stats;
            Histograms = histograms;
            //RawHistogram = new HistogramPair(new FileInfo(Path.Combine(Folder.FullName, string.Format("{0}_raw.csv", filePrefix))), rawHistogram);
            //ThrHistogram = new HistogramPair(new FileInfo(Path.Combine(Folder.FullName, string.Format("{0}_thr.csv", filePrefix))), thrHistogram);
            SummaryXML = summaryXML;
        }

        public BudgetSegregationClass(string name, DoDStats stats, HistogramPair histograms, FileInfo summaryXML)
            : base(name)
        {
            Statistics = stats;
            Histograms = histograms;
            SummaryXML = summaryXML;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodClass = nodParent.AppendChild(xmlDoc.CreateElement("Class"));
            nodClass.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodClass.AppendChild(xmlDoc.CreateElement("RawHistogram")).InnerText = ProjectManager.Project.GetRelativePath(Histograms.Raw.Path);
            nodClass.AppendChild(xmlDoc.CreateElement("ThrHistogram")).InnerText = ProjectManager.Project.GetRelativePath(Histograms.Thr.Path);
            nodClass.AppendChild(xmlDoc.CreateElement("SummaryXML")).InnerText = ProjectManager.Project.GetRelativePath(SummaryXML);
            XmlNode nodStatistics = nodParent.AppendChild(xmlDoc.CreateElement("Statistics"));
            DoDBase.SerializeDoDStatistics(xmlDoc, nodStatistics, Statistics);
        }

        public static BudgetSegregationClass Deserialize(XmlNode nodClass)
        {
            string name = nodClass.SelectSingleNode("Name").InnerText;
            FileInfo rawHist = ProjectManager.Project.GetAbsolutePath(nodClass.SelectSingleNode("RawHistogram").InnerText);
            FileInfo thrHist = ProjectManager.Project.GetAbsolutePath(nodClass.SelectSingleNode("ThrHistogram").InnerText);
            FileInfo summary = ProjectManager.Project.GetAbsolutePath(nodClass.SelectSingleNode("SummaryXML").InnerText);

            DoDStats stats = DoDBase.DeserializeStatistics(nodClass.SelectSingleNode("Statistics"), ProjectManager.Project.CellArea, ProjectManager.Project.Units);

            return new BudgetSegregationClass(name, stats, new HistogramPair(rawHist, thrHist), summary);
        }
    }
}
