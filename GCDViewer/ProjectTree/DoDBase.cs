using ArcGIS.Core.Internal.CIM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class DoDBase : BaseDataset
    {
        public readonly DirectoryInfo Folder;
        public readonly Surface NewSurface;
        public readonly Surface OldSurface;
        public readonly Masks.AOIMask AOIMask;

        public DoDRaster RawDoD { get; internal set; }
        public DoDRaster ThrDoD { get; internal set; }
        public DoDRaster ThrErr { get; internal set; }

        //public HistogramPair Histograms { get; set; }
        public FileInfo SummaryXML { get; set; }
        //public readonly DoDStats Statistics;

        //public List<BudgetSegregation> BudgetSegregations { get; internal set; }
        //public readonly List<LinearExtraction.LinearExtraction> LinearExtractions;

        public override string ContextMenu => "GroupLayer";

        public override bool Exists { get { return true; } }

        public override string Noun { get { return "Change Detection"; } }

        //public abstract string UncertaintyAnalysisLabel
        //{
        //    get;
        //}
        public DoDBase(GCDProject project, XmlNode nodDoD)
        : base(nodDoD.SelectSingleNode("Name").InnerText, "Delta.png", "Delta.png", nodDoD.SelectSingleNode("ID")?.InnerText)
        {
            Folder = project.GetAbsoluteDir(nodDoD.SelectSingleNode("Folder").InnerText);
            NewSurface = DeserializeSurface(project, nodDoD, "NewSurface");
            OldSurface = DeserializeSurface(project, nodDoD, "OldSurface");

            XmlNode nodAOI = nodDoD.SelectSingleNode("AOI");
            if (nodAOI is XmlNode)
            {
                AOIMask = project.Masks.First(x => string.Compare(x.Name, nodAOI.InnerText, true) == 0) as Masks.AOIMask;
            }

            RawDoD = new DoDRaster(project, string.Format(/*Name +*/ "DoD Raw"), project.GetAbsolutePath(nodDoD.SelectSingleNode("RawDoD").InnerText));
            ThrDoD = new DoDRaster(project, string.Format(/*Name +*/ "DoD Thresholded"), project.GetAbsolutePath(nodDoD.SelectSingleNode("ThrDoD").InnerText));
            ThrErr = new DoDRaster(project, string.Format(/*Name +*/ "DoD Thresholded Error"), project.GetAbsolutePath(nodDoD.SelectSingleNode("ThrErr").InnerText));
            //Histograms = new HistogramPair(ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("RawHistogram").InnerText),
            //SummaryXML = ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("SummaryXML").InnerText);
            //Statistics = DeserializeStatistics(nodDoD.SelectSingleNode("Statistics"), ProjectManager.Project.CellArea, ProjectManager.Project.Units);

            //BudgetSegregations = new List<BudgetSegregation>();
            //XmlNode nodBSes = nodDoD.SelectSingleNode("BudgetSegregations");
            //if (nodBSes is XmlNode)
            //{
            //    foreach (XmlNode nodBS in nodBSes.SelectNodes("BudgetSegregation"))
            //    {
            //        BudgetSegregation bs = new BudgetSegregation(nodBS, this);
            //        BudgetSegregations.Add(bs);
            //    }
            //}

            //LinearExtractions = new List<LinearExtraction.LinearExtraction>();
            //foreach (XmlNode nodLE in nodDoD.SelectNodes("LinearExtractions/LinearExtraction"))
            //{
            //    LinearExtraction.LinearExtraction le = new LinearExtraction.LinearExtractionFromDoD(nodLE, this);
            //    LinearExtractions.Add(le);
            //}
        }

        private Surface DeserializeSurface(GCDProject project, XmlNode nodDoD, string nodDEMName)
        {
            string surfaceName = nodDoD.SelectSingleNode(nodDEMName).InnerText;
            string surfaceType = nodDoD.SelectSingleNode(nodDEMName).Attributes["type"].InnerText;

            if (string.Compare(surfaceType, "dem", true) == 0)
            {
                return project.DEMSurveys.First(x => string.Compare(x.Name, surfaceName, true) == 0);
            }
            else
            {
                return project.ReferenceSurfaces.First(x => string.Compare(x.Name, surfaceName, true) == 0);
            }
        }
    }
}
