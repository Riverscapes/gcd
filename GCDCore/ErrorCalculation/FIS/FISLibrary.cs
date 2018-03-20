using System;
using System.Xml;
using System.IO;
using System.Linq;

namespace GCDCore.ErrorCalculation.FIS
{
    public class FISLibrary
    {
        public enum FISLibraryItemTypes
        {
            System,
            User
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
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodRoot = xmlDoc.AppendChild(xmlDoc.CreateElement("FISLibrary"));
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", null, null), nodRoot);

            FISItems.Where(x => x.FISType == FISLibraryItemTypes.User).ToList().ForEach(x => x.Serialize(nodRoot));

            CustomFISLibrary.Directory.Create();
            xmlDoc.Save(CustomFISLibrary.FullName);
        }

        private void LoadFISLibrary(FileInfo filePath, FISLibraryItemTypes eType)
        {
            if (!filePath.Exists)
            {
                Console.WriteLine("FIS library XML file not present " + filePath.FullName);
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(filePath.FullName);

                foreach (XmlNode nodItem in xmlDoc.DocumentElement.SelectNodes("Item"))
                {
                    try
                    {
                        FISLibraryItem item = new FISLibraryItem(nodItem, eType);
                        FISItems.Add(item);
                    }
                    catch (Exception ex)
                    {
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
                catch(Exception ex)
                {
                    Console.WriteLine("Error loading unreferenced system FIS file " + fis.FullName);
                }
            }
        }
    }
}
