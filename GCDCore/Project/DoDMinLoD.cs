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
        public readonly double Threshold;

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
        public DoDMinLoD(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, Raster rawDoD, Raster thrDoD, HistogramPair histograms, double threshold, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, rawDoD, thrDoD, histograms, stats)
        {
            Threshold = threshold;
        }

        /// <summary>
        /// Constructor for XML deserialization
        /// </summary>
        /// <param name="dod"></param>
        /// <param name="threshold"></param>
        public DoDMinLoD(DoDBase dod, double threshold)
            : base(dod)
        {
            Threshold = threshold;
        }

        new public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDoD = base.Serialize(xmlDoc, nodParent);
            nodDoD.InsertBefore(xmlDoc.CreateElement("Threshold"), nodDoD.SelectSingleNode("Statistics")).InnerText = Threshold.ToString();
        }

        new public static DoDMinLoD Deserialize(XmlNode nodDoD, Dictionary<string, DEMSurvey> dems)
        {
            DoDBase partialDoD = DoDBase.Deserialize(nodDoD, dems);
            double threshold = double.Parse(nodDoD.SelectSingleNode("Threshold").InnerText);
            return new DoDMinLoD(partialDoD, threshold);
        }
    }
}
