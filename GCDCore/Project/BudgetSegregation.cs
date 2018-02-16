using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace GCDCore.Project
{
    public class BudgetSegregation : GCDProjectItem
    {
        public readonly DoDBase DoD;
        public readonly DirectoryInfo Folder;
        public readonly Masks.Mask Mask;
        public readonly FileInfo ClassLegend;
        public readonly FileInfo SummaryXML;
        public readonly string MaskField;
        public readonly Dictionary<string, BudgetSegregationClass> Classes;

        public readonly Dictionary<string, Morphological.MorphologicalAnalysis> MorphologicalAnalyses;

        public bool IsMaskDirectional { get { return Mask is GCDCore.Project.Masks.DirectionalMask; } }

        public BindingList<BudgetSegregationClass> FilteredClasses
        {
            get
            {
                BindingList<BudgetSegregationClass> result = new BindingList<BudgetSegregationClass>();

                // Loop over all distinct field values that are flagged to be included
                foreach (KeyValuePair<string, string> kvp in Mask.ActiveFieldValues)
                {
                    if (Classes.ContainsKey(kvp.Value))
                    {
                        BudgetSegregationClass existingClass = Classes[kvp.Key];
                        result.Add(new BudgetSegregationClass(kvp.Value, existingClass.Statistics, existingClass.Histograms, existingClass.SummaryXML));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Engine Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <param name="maskField"></param>
        /// <param name="dod"></param>
        public BudgetSegregation(string name, DirectoryInfo folder, Masks.Mask mask, DoDBase dod)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            Mask = mask;
            ClassLegend = new FileInfo(Path.Combine(Folder.FullName, "ClassLegend.csv"));
            SummaryXML = new FileInfo(Path.Combine(Folder.FullName, "Summary.xml"));
            Classes = new Dictionary<string, BudgetSegregationClass>();
            MorphologicalAnalyses = new Dictionary<string, Morphological.MorphologicalAnalysis>();
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
        public BudgetSegregation(string name, DirectoryInfo folder, Masks.Mask mask, DoDBase dod, FileInfo summaryXML, FileInfo classLegend)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            Mask = mask;
            ClassLegend = classLegend;
            SummaryXML = summaryXML;
            Classes = new Dictionary<string, BudgetSegregationClass>();
            MorphologicalAnalyses = new Dictionary<string, Morphological.MorphologicalAnalysis>();
        }

        public BudgetSegregation(XmlNode nodBS, DoDBase dod)
            : base(nodBS.SelectSingleNode("Name").InnerText)
        {
            DoD = dod;
            Folder = ProjectManager.Project.GetAbsoluteDir(nodBS.SelectSingleNode("Folder").InnerText);
            Mask = ProjectManager.Project.Masks[nodBS.SelectSingleNode("Mask").InnerText];
            SummaryXML = ProjectManager.Project.GetAbsolutePath(nodBS.SelectSingleNode("SummaryXML").InnerText);
            ClassLegend = ProjectManager.Project.GetAbsolutePath(nodBS.SelectSingleNode("ClassLegend").InnerText);

            Classes = new Dictionary<string, BudgetSegregationClass>();
            foreach (XmlNode nodClass in nodBS.SelectNodes("Classes/Class"))
            {
                BudgetSegregationClass bsClass = new BudgetSegregationClass(nodClass);
                Classes[bsClass.Name] = bsClass;
            }

            MorphologicalAnalyses = new Dictionary<string, Morphological.MorphologicalAnalysis>();
            foreach (XmlNode nodMA in nodBS.SelectNodes("MorphologicalAnalyses/MorphologicalAnalysis"))
            {
                Morphological.MorphologicalAnalysis ma = new Morphological.MorphologicalAnalysis(nodMA, this);
                MorphologicalAnalyses[ma.Name] = ma;
            }
        }
        
        public bool IsMorphologicalAnalysisNameUnique(string name, Morphological.MorphologicalAnalysis ignore)
        {
            return MorphologicalAnalyses.ContainsKey(name) ? MorphologicalAnalyses[name] == ignore : true;
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodBS = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("BudgetSegregation"));
            nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("Folder")).InnerText = ProjectManager.Project.GetRelativePath(Folder.FullName);
            nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("Mask")).InnerText = Mask.Name;
            nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("SummaryXML")).InnerText = ProjectManager.Project.GetRelativePath(SummaryXML);
            nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("ClassLegend")).InnerText = ProjectManager.Project.GetRelativePath(ClassLegend);
            nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("MaskField")).InnerText = MaskField;

            XmlNode nodClasses = nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("Classes"));
            foreach (BudgetSegregationClass segClass in Classes.Values)
                segClass.Serialize(nodClasses);

            if (MorphologicalAnalyses.Count > 0)
            {
                XmlNode nodMA = nodBS.AppendChild(nodParent.OwnerDocument.CreateElement("MorphologicalAnalyses"));
                foreach (Morphological.MorphologicalAnalysis ma in MorphologicalAnalyses.Values)
                {
                    ma.Serialize(nodMA);
                }
            }
        }

        public void Delete()
        {
            try
            {
                Folder.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to delete budget segregation folder", Folder.FullName);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
