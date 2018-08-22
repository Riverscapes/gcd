using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Xml.Schema;
using System.Diagnostics;

namespace GCDCore.ErrorCalculation.FIS
{
    public class FISLibrary
    {
        public enum FISLibraryItemTypes
        {
            System,
            User,
            Project
        };

        public readonly naru.ui.SortableBindingList<FISLibraryItem> FISItems;

        /// <summary>
        /// Customizable FIS library that stores user FIS
        /// </summary>
        public readonly FileInfo CustomFISLibrary;

        /// <summary>
        /// The FIS library that ships with the GCD software
        /// </summary>
        public FileInfo SystemFISLibrary
        {
            get
            {
                return new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Deploy", "FISLibrary", "FISLibrary.xml"));
            }
        }

        public FISLibrary(DirectoryInfo gcdAppDataFolder)
        {
            CustomFISLibrary = new FileInfo(Path.Combine(gcdAppDataFolder.FullName, "FISLibrary.xml"));
            FISItems = new naru.ui.SortableBindingList<FISLibraryItem>();
        }

        public void Load()
        {
            FISItems.Clear();
            LoadFISLibrary(SystemFISLibrary, FISLibraryItemTypes.System);
            LoadFISLibrary(CustomFISLibrary, FISLibraryItemTypes.User);
            LoadUnreferenceSystemFIS();
        }

        public void Save()
        {
            XmlNode parentNode = CreateFISLibraryXML();
            FISItems.Where(x => x.FISType == FISLibraryItemTypes.User).ToList().ForEach(x => x.Serialize(parentNode));

            CustomFISLibrary.Directory.Create();
            parentNode.OwnerDocument.Save(CustomFISLibrary.FullName);
        }

        public static XmlNode CreateFISLibraryXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodRoot = xmlDoc.AppendChild(xmlDoc.CreateElement("FISLibrary", "http://www.w3.org/2001/XMLSchema-instance"));
            nodRoot.Attributes.Append(xmlDoc.CreateAttribute("noNamespaceSchemaLocation")).InnerText = "https://raw.githubusercontent.com/Riverscapes/fis-dem-error/master/FISLibrary.xsd";
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null), nodRoot);
            return nodRoot;
        }

        private void LoadFISLibrary(FileInfo filePath, FISLibraryItemTypes eType)
        {
            if (!filePath.Exists)
            {
                Console.WriteLine("FIS library XML file not present " + filePath.FullName);
                return;
            }

            // System FIS Library contains relative paths and the directory storing the manifest XML is needed
            DirectoryInfo rootDir = null;
            if (eType == FISLibraryItemTypes.System)
                rootDir = filePath.Directory;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath.FullName);

                foreach (XmlNode nodItem in xmlDoc.DocumentElement.SelectNodes("FISLibraryItem"))
                {
                    try
                    {
                        FISLibraryItem item = new FISLibraryItem(nodItem, eType, rootDir);
                        FISItems.Add(item);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Console.WriteLine(string.Format("Error reading {0} FIS library item from file {1}", eType.ToString(), filePath));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error loading {0} FIS Library XML file: {1}\n{2}", eType.ToString(), filePath.FullName, ex.Message));
            }
        }

        /// <summary>
        /// Find all the FIS files next to the system FIS library manifest
        /// that are not defined in the manifest and add them to the library
        /// without any metadata
        /// </summary>
        private void LoadUnreferenceSystemFIS()
        {
            foreach (FileInfo fis in SystemFISLibrary.Directory.GetFiles("*.fis", SearchOption.AllDirectories))
            {
                try
                {
                    if (!FISItems.Any(x => string.Compare(x.FilePath.FullName, fis.FullName, true) == 0))
                    {
                        // This FIS file on disk is not currently listed in the manifest XML
                        string name = Path.GetFileNameWithoutExtension(fis.FullName);
                        FISItems.Add(new FISLibraryItem(name, fis, FISLibraryItemTypes.System));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Console.WriteLine("Error loading unreferenced system FIS file " + fis.FullName);
                }
            }
        }
    }
}
