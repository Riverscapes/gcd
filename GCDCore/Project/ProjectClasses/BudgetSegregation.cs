using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace GCDCore.Project
{
    public class BudgetSegregation : GCDProjectItem
    {
        public readonly DoD DoD;
        public readonly DirectoryInfo Folder;

        public Dictionary<string, BudgetSegregationClass> Classes { get; internal set; }

        public BudgetSegregation(string name, DirectoryInfo folder, DoD dod)
            : base(name)
        {
            DoD = dod;
            Folder = folder;
            Classes = new Dictionary<string, BudgetSegregationClass>();
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodBS = nodParent.AppendChild(xmlDoc.CreateElement("BudgetSegregation"));
            nodBS.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodBS.AppendChild(xmlDoc.CreateElement("Folder")).InnerText = Folder.FullName;

            XmlNode nodClasses = nodParent.AppendChild(xmlDoc.CreateElement("Classes"));
            foreach (BudgetSegregationClass segClass in Classes.Values)
                segClass.Serialize(xmlDoc, nodBS);
        }
    }
}
