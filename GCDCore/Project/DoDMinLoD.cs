using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;

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
        /// <param name="newDEM"></param>
        /// <param name="oldDEM"></param>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoD"></param>
        /// <param name="rawHist"></param>
        /// <param name="thrHist"></param>
        /// <param name="threshold"></param>
        /// <param name="stats"></param>
        public DoDMinLoD(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, Raster rawDoD, Raster thrDoD, HistogramPair histograms, FileInfo summaryXML, decimal threshold, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, rawDoD, thrDoD, histograms, summaryXML, stats)
        {
            Threshold = threshold;
        }

        public DoDMinLoD(XmlNode nodDoD, Dictionary<string, DEMSurvey> dems)
            : base(nodDoD, dems)
        {
           Threshold = decimal.Parse(nodDoD.SelectSingleNode("Threshold").InnerText);
        }

        public override XmlNode Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDoD = base.Serialize(xmlDoc, nodParent);
            nodDoD.InsertBefore(xmlDoc.CreateElement("Threshold"), nodDoD.SelectSingleNode("Statistics")).InnerText = Threshold.ToString();
            return nodDoD;
        }
    }
}
