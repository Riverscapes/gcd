using System;
using System.Collections.Generic;
using System.Xml;

namespace GCDCore.ErrorCalculation.FIS
{
    public class FISLibraryItem
    {
        public string Name { get; internal set; }
        public readonly System.IO.FileInfo FilePath;

        public string FilePathString { get { return FilePath.FullName; } }

        public override string ToString()
        {
            return Name;
        }

        public FISLibraryItem(string sName, System.IO.FileInfo fiPath)
        {
            Name = sName;
            FilePath = fiPath;
        }

        public static List<FISLibraryItem> Load(System.IO.FileInfo filePath)
        {
            List<FISLibraryItem> dItems = new List<FISLibraryItem>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath.FullName);

            foreach (XmlNode nodType in xmlDoc.SelectNodes("FISLibrary/Item"))
            {
                string sName = nodType.SelectSingleNode("Name").InnerText;
                string fisPath = nodType.SelectSingleNode("FilePath").InnerText;
                if (!string.IsNullOrEmpty(fisPath) && System.IO.File.Exists(fisPath))
                {
                    dItems.Add(new FISLibraryItem(sName, new System.IO.FileInfo(fisPath)));
                }
            }

            return dItems;
        }

        public static void Save(System.IO.FileInfo filePath, List<FISLibraryItem> dFISLibraryItems)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodRoot = xmlDoc.AppendChild(xmlDoc.CreateElement("FISLibrary"));
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", null, null), nodRoot);

            foreach (FISLibraryItem item in dFISLibraryItems)
            {
                XmlElement nodType = xmlDoc.CreateElement("Item");
                nodRoot.AppendChild(nodType);

                XmlElement nodName = xmlDoc.CreateElement("Name");
                nodName.InnerText = item.Name;
                nodType.AppendChild(nodName);

                XmlElement nodFilePath = xmlDoc.CreateElement("FilePath");
                nodFilePath.InnerText = item.FilePath.FullName;
                nodType.AppendChild(nodFilePath);
            }

            xmlDoc.Save(filePath.FullName);
        }
    }
}