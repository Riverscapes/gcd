using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.Globalization;

namespace GCDCore.Project
{
    public class DoDMinLoD : DoDBase
    {
        public readonly decimal Threshold;

        public override string UncertaintyAnalysisLabel
        {
            get
            {
                return string.Format("{0:0.00}{1} Minimum level of detection", Threshold, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            }
        }

        /// <summary>
        /// Constructor for change detection engine
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <param name="newSurface"></param>
        /// <param name="oldSurface"></param>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoD"></param>
        /// <param name="rawHist"></param>
        /// <param name="thrHist"></param>
        /// <param name="threshold"></param>
        /// <param name="stats"></param>
        public DoDMinLoD(string name, DirectoryInfo folder, Surface newSurface, Surface oldSurface, Project.Masks.AOIMask aoi, Raster rawDoD, Raster thrDoD, Raster thrErr, HistogramPair histograms, FileInfo summaryXML, decimal threshold, DoDStats stats)
            : base(name, folder, newSurface, oldSurface, aoi, rawDoD, thrDoD, thrErr, histograms, summaryXML, stats)
        {
            Threshold = threshold;
        }

        public DoDMinLoD(XmlNode nodDoD)
            : base(nodDoD)
        {
            Threshold = decimal.Parse(nodDoD.SelectSingleNode("Threshold").InnerText, CultureInfo.InvariantCulture);
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodDoD = base.Serialize(nodParent);
            nodDoD.InsertBefore(nodParent.OwnerDocument.CreateElement("Threshold"), nodDoD.SelectSingleNode("Statistics")).InnerText = Threshold.ToString(CultureInfo.InvariantCulture);
            return nodDoD;
        }
    }
}
