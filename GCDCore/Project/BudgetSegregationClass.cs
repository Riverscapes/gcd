using System.IO;
using GCDConsoleLib.GCD;
using System.Xml;

namespace GCDCore.Project
{
    public class BudgetSegregationClass : GCDProjectItem
    {
        public readonly ProjectHistogram RawHistogram;
        public readonly ProjectHistogram ThrHistogram;
        public readonly FileInfo SummaryXML;
        public readonly DoDStats Statistics;

        public BudgetSegregationClass(string name, DirectoryInfo Folder, string filePrefix, DoDStats stats, GCDConsoleLib.Histogram rawHistogram, GCDConsoleLib.Histogram thrHistogram)
            : base(name)
        {
            Statistics = stats;
            RawHistogram = new ProjectHistogram(new FileInfo(Path.Combine(Folder.FullName, string.Format("{0}_raw.csv", filePrefix))), rawHistogram);
            ThrHistogram = new ProjectHistogram(new FileInfo(Path.Combine(Folder.FullName, string.Format("{0}_thr.csv", filePrefix))), thrHistogram);
            SummaryXML = new FileInfo(Path.Combine(Folder.FullName, string.Format("{0}_summary.xml", filePrefix)));
        }

        public BudgetSegregationClass(string name, DoDStats stats, FileInfo rawHist, FileInfo thrHist, FileInfo summaryXML)
            : base(name)
        {
            Statistics = stats;
            RawHistogram = new ProjectHistogram(rawHist);
            ThrHistogram = new ProjectHistogram(thrHist);
            SummaryXML = summaryXML;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodClass = nodParent.AppendChild(xmlDoc.CreateElement("Class"));
            nodClass.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodClass.AppendChild(xmlDoc.CreateElement("RawHistogram")).InnerText = ProjectManagerBase.GetRelativePath(RawHistogram.Path);
            nodClass.AppendChild(xmlDoc.CreateElement("ThrHistogram")).InnerText = ProjectManagerBase.GetRelativePath(ThrHistogram.Path);
            nodClass.AppendChild(xmlDoc.CreateElement("SummaryXML")).InnerText = ProjectManagerBase.GetRelativePath(SummaryXML);
            DoDBase.SerializeDoDStatistics(xmlDoc, nodParent, Statistics);
        }

        public static BudgetSegregationClass Deserialize(XmlNode nodClass)
        {
            string name = nodClass.SelectSingleNode("Name").InnerText;
            FileInfo rawHist = ProjectManagerBase.GetAbsolutePath(nodClass.SelectSingleNode("RawHistogram").InnerText);
            FileInfo thrHist = ProjectManagerBase.GetAbsolutePath(nodClass.SelectSingleNode("ThrHistogram").InnerText);
            FileInfo summary = ProjectManagerBase.GetAbsolutePath(nodClass.SelectSingleNode("SummaryXML").InnerText);

            DoDStats stats = DoDBase.DeserializeStatistics(nodClass.SelectSingleNode("Statistics"), ProjectManagerBase.CellArea, ProjectManagerBase.Units);

            return new BudgetSegregationClass(name, stats, rawHist, thrHist, summary);
        }
    }
}
