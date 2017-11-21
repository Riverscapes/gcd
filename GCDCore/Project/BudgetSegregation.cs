using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace GCDCore.Project
{
    public class BudgetSegregation : GCDProjectItem
    {
        public readonly DoDBase DoD;
        public readonly DirectoryInfo Folder;
        public readonly FileInfo PolygonMask;
        public readonly FileInfo ClassLegend;
        public readonly FileInfo SummaryXML;
        public readonly string MaskField;
        public readonly Dictionary<string, BudgetSegregationClass> Classes;

        /// <summary>
        /// Engine Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <param name="maskField"></param>
        /// <param name="dod"></param>
        public BudgetSegregation(string name, DirectoryInfo folder, string maskField, DoDBase dod)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            PolygonMask = new FileInfo(Path.Combine(folder.FullName));
            ClassLegend = new FileInfo(Path.Combine(Folder.FullName, "ClassLegend.csv"));
            SummaryXML = new FileInfo(Path.Combine(Folder.FullName, "Summary.xml"));
            MaskField = maskField;
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <param name="polygonMask"></param>
        /// <param name="maskField"></param>
        /// <param name="dod"></param>
        /// <param name="summaryXML"></param>
        /// <param name="classLegend"></param>
        public BudgetSegregation(string name, DirectoryInfo folder, FileInfo polygonMask, string maskField, DoDBase dod, FileInfo summaryXML, FileInfo classLegend)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            PolygonMask = polygonMask;
            ClassLegend = classLegend;
            SummaryXML = summaryXML;
            MaskField = maskField;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodBS = nodParent.AppendChild(xmlDoc.CreateElement("BudgetSegregation"));
            nodBS.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodBS.AppendChild(xmlDoc.CreateElement("Folder")).InnerText = ProjectManagerBase.GetRelativePath(Folder.FullName);
            nodBS.AppendChild(xmlDoc.CreateElement("PolygonMask")).InnerText = ProjectManagerBase.GetRelativePath(PolygonMask);
            nodBS.AppendChild(xmlDoc.CreateElement("SummaryXML")).InnerText = ProjectManagerBase.GetRelativePath(SummaryXML);
            nodBS.AppendChild(xmlDoc.CreateElement("ClassLegend")).InnerText = ProjectManagerBase.GetRelativePath(ClassLegend);
            nodBS.AppendChild(xmlDoc.CreateElement("MaskField")).InnerText = MaskField;

            XmlNode nodClasses = nodParent.AppendChild(xmlDoc.CreateElement("Classes"));
            foreach (BudgetSegregationClass segClass in Classes.Values)
                segClass.Serialize(xmlDoc, nodBS);
        }

        public static BudgetSegregation Deserialize(XmlNode nodBS, DoDBase dod)
        {
            string name = nodBS.SelectSingleNode("Name").InnerText;
            DirectoryInfo folder = ProjectManagerBase.GetAbsoluteDir(nodBS.SelectSingleNode("Folder").InnerText);
            FileInfo polygonMask = ProjectManagerBase.GetAbsolutePath(nodBS.SelectSingleNode("PolygonMask").InnerText);
            FileInfo summaryXML = ProjectManagerBase.GetAbsolutePath(nodBS.SelectSingleNode("SummaryXML").InnerText);
            FileInfo classLegend = ProjectManagerBase.GetAbsolutePath(nodBS.SelectSingleNode("ClassLegend").InnerText);
            string maskField = nodBS.SelectSingleNode("MaskField").InnerText;

            BudgetSegregation bs = new BudgetSegregation(name, folder, polygonMask, maskField, dod, summaryXML, classLegend);

            foreach (XmlNode nodClass in nodBS.SelectNodes("Classes/Class"))
            {
                BudgetSegregationClass bsClass = BudgetSegregationClass.Deserialize(nodClass);
                bs.Classes[bsClass.Name] = bsClass;
            }

            return bs;
        }
    }
}
