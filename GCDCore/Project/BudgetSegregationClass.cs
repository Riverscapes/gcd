using System.IO;
using GCDConsoleLib.GCD;
using System.Xml;
using UnitsNet;

namespace GCDCore.Project
{
    public class BudgetSegregationClass : Morphological.IBudgetGraphicalResults
    {
        public string Name { get; private set; }
        public readonly HistogramPair Histograms;
        public readonly FileInfo SummaryXML;
        public readonly DoDStats Statistics;

        public override string ToString()
        {
            return Name;
        }

        public BudgetSegregationClass(string name, DoDStats stats, HistogramPair histograms, FileInfo summaryXML)
        {
            Name = name;
            Statistics = stats;
            Histograms = histograms;
            SummaryXML = summaryXML;
        }

        public BudgetSegregationClass(XmlNode nodClass)
        {
            Name = nodClass.SelectSingleNode("Name").InnerText;
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

        public Volume VolErosion { get { return Statistics.ErosionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit); } }
        public Volume VolErosionErr { get { return Statistics.ErosionErr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit); } }

        public Volume VolDeposition { get { return Statistics.DepositionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit); } }
        public Volume VolDepositionErr { get { return Statistics.DepositionErr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit); } }
 
        public Volume SecondGraphValue { get; set; }
    }
}
