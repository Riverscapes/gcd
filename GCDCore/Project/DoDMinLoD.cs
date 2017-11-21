using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib.GCD;

namespace GCDCore.Project
{
    public class DoDMinLoD : DoDBase
    {
        public readonly double Threshold;

        public DoDMinLoD(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, double threshold, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, stats)
        {
            Threshold = threshold;
        }

        public DoDMinLoD(DoDBase dod, double threshold)
            : base(dod.Name, dod.Folder, dod.NewDEM, dod.OldDEM, dod.Statistics)
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
