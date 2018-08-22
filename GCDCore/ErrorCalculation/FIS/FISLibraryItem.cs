using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace GCDCore.ErrorCalculation.FIS
{
    public class FISLibraryItem
    {
        public readonly FISLibrary.FISLibraryItemTypes FISType;
        public readonly System.IO.FileInfo FilePath;
        public string Name { get; set; }
        public string Description { get; set; }

        public string OutputName { get; set; }
        public string OutputUnits { get; set; }
        public string OutputDescription { get; set; }

        // Note FIS Inputs are not sortable
        public readonly BindingList<FISInputMeta> Inputs;
        public readonly naru.ui.SortableBindingList<FISMetaItem> Publications;
        public readonly naru.ui.SortableBindingList<FISMetaItem> ExampleDatasets;
        public readonly naru.ui.SortableBindingList<FISMetaItem> Metadata;

        // Needed for binding to controls
        public string FilePathString { get { return FilePath.FullName; } }
        public string FISTypeString { get { return FISType.ToString(); } }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Constructore for loading "unreferenced" system FIS.
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="fiPath"></param>
        /// <param name="eType"></param>
        /// <remarks>Unreferenced system FIS are fis files found in the system folder that
        /// do not have an entry in the system FIS library XML</remarks>
        public FISLibraryItem(string sName, System.IO.FileInfo fiPath, FISLibrary.FISLibraryItemTypes eType)
        {
            FISType = eType;
            Name = sName;
            FilePath = fiPath;
            Inputs = new BindingList<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();

            LoadInputsFromFIS();
        }

        /// <summary>
        /// Creating new custom, user FIS item from an existing FIS rule file or loading project fis
        /// </summary>
        /// <param name="fisfilePath"></param>
        public FISLibraryItem(string fisfilePath, FISLibrary.FISLibraryItemTypes eType)
        {
            FISType = eType;
            FilePath = new FileInfo(fisfilePath);
            Name = Path.GetFileNameWithoutExtension(fisfilePath);
            Inputs = new BindingList<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();

            // ensure that the metadata list contains the essential items
            AddMetadataItem("Contributor");
            AddMetadataItem("Contributor Email");
            AddMetadataItem("Organization");
            AddMetadataItem("Purpose");
            AddMetadataItem("URL");
            AddMetadataItem("Survey Type");

            FileInfo xmlMetaData = new FileInfo(Path.ChangeExtension(fisfilePath, "fis.xml"));
            if (eType == FISLibrary.FISLibraryItemTypes.Project && xmlMetaData.Exists)
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlMetaData.FullName);
                    LoadFISFromXML(xmlDoc.DocumentElement.SelectSingleNode("FISLibraryItem"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error loading FIS metadata file at {0}\n{1}", xmlMetaData.FullName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Ensure that the metadata item contains the essential fields
        /// </summary>
        /// <param name="key"></param>
        private void AddMetadataItem(string key)
        {
            if (!Metadata.Any(x => string.Compare(x.Title, key, true) == 0))
                Metadata.Add(new FISMetaItem(key, string.Empty));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodFISItem"></param>
        /// <param name="eType">User, system or project</param>
        /// <param name="libraryRootDir">The root directory of the FIS Library (i.e. folder containing the FIS library XML file)</param>
        public FISLibraryItem(XmlNode nodFISItem, FISLibrary.FISLibraryItemTypes eType, DirectoryInfo libraryRootDir)
        {
            FISType = eType;
            if (libraryRootDir is System.IO.DirectoryInfo)
            {
                // System FIS Library XML stores relative paths to the FIS Library XML manfiest
                FilePath = new System.IO.FileInfo(System.IO.Path.Combine(libraryRootDir.FullName, nodFISItem.SelectSingleNode("FilePath").InnerText));
            }
            else
            {
                // User FIS items store absolute file paths in the custom FIS Library XML
                FilePath = new System.IO.FileInfo(nodFISItem.SelectSingleNode("FilePath").InnerText);
            }
            System.Diagnostics.Debug.Assert(FilePath.Exists, "This constructor should only be called if the FIS file actually exists");

            Inputs = new BindingList<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();

            LoadFISFromXML(nodFISItem);
        }

        /// <summary>
        /// Constructor for project FIS items
        /// </summary>
        /// <param name="nodFISItem"></param>
        public FISLibraryItem(XmlNode nodFISItem, FileInfo projectFISPath)
        {
            FISType = FISLibrary.FISLibraryItemTypes.Project;
            FilePath = projectFISPath;
            System.Diagnostics.Debug.Assert(FilePath.Exists, "This constructor should only be called if the FIS file actually exists");

            Inputs = new BindingList<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();

            LoadFISFromXML(nodFISItem);
        }

        private void LoadInputsFromFIS()
        {
            try
            {
                string sRuleFileText = File.ReadAllText(FilePath.FullName);

                Regex theRegEx = new Regex("dd");
                Match theMatch = theRegEx.Match(sRuleFileText);

                // Match data between single quotes hesitantly.
                MatchCollection col = Regex.Matches(sRuleFileText, "\\[Input[0-9]\\]\\s*Name='([^']*)'");
                foreach (Match m in col)
                {
                    // Access first Group and its value.
                    Group g = m.Groups[1];
                    Inputs.Add(new FISInputMeta(g.Value));
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error parsing FIS rule file", ex);
                ex2.Data["FIS Rule File Path"] = FilePath;
                throw ex2;
            }
        }

        private void LoadFISFromXML(XmlNode nodFISItem)
        {
            Name = nodFISItem.SelectSingleNode("Name").InnerText;
            Description = GetSafeNodeValue(nodFISItem, "Description");

            LoadMetaItemList(nodFISItem, "Metadata/Item", "key", "value", Metadata);
            LoadMetaItemList(nodFISItem, "Publications/Publication", "Citation", "URL", Publications);
            LoadMetaItemList(nodFISItem, "ExampleDatasets/ExampleDataset", "Title", "URL", ExampleDatasets);

            // First, attempt to load the FIS Inputs directly from the FIS rule file.
            LoadInputsFromFIS();

            // Now match up any additional input properties from the metadata XML
            foreach (XmlNode nodInput in nodFISItem.SelectNodes("Inputs/Input"))
            {
                string name = GetSafeNodeValue(nodInput, "Name");
                if (!string.IsNullOrEmpty(name))
                {
                    if (Inputs.Any(x => string.Compare(x.Name, name, true) == 0))
                    {
                        FISInputMeta input = Inputs.First(x => string.Compare(x.Name, name, true) == 0);
                        input.Description = GetSafeNodeValue(nodInput, "Description");
                        input.Units = GetSafeNodeValue(nodInput, "Units");
                        input.Source = GetSafeNodeValue(nodInput, "Source");
                    }
                }
            }

            OutputName = GetSafeNodeValue(nodFISItem, "Output/Name");
            OutputDescription = GetSafeNodeValue(nodFISItem, "Output/Description");
            OutputUnits = GetSafeNodeValue(nodFISItem, "Output/Units");
        }

        private static void LoadMetaItemList(XmlNode nodParent, string xPath, string keyNode, string valueNode, naru.ui.SortableBindingList<FISMetaItem> items)
        {
            items.Clear();

            foreach (XmlNode nodItem in nodParent.SelectNodes(xPath))
            {
                string key = GetSafeNodeValue(nodItem, keyNode);
                string value = GetSafeNodeValue(nodItem, valueNode);
                items.Add(new FISMetaItem(key, value));
            }
        }

        /// <summary>
        /// Save the FIS Library item to XML
        /// </summary>
        /// <param name="nodParent"></param>
        /// <remarks>THE ORDER OF THE XML ELEMENTS IS IMPORTANT
        /// The XSD specifies the order of each element, so don't rearrange the items below</remarks>
        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodItem = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("FISLibraryItem"));
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("FilePath")).InnerText = FilePath.FullName;
            naru.xml.XMLHelpers.AddNode(nodItem, "Description", Description);

            SerializeMetaDataList(nodItem, Metadata, "Metadata", "Item", "Key", "Value");

            XmlNode nodInputs = nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Inputs"));
            foreach (FISInputMeta input in Inputs)
            {
                XmlNode nodInput = nodInputs.AppendChild(nodParent.OwnerDocument.CreateElement("Input"));
                naru.xml.XMLHelpers.AddNode(nodInput, "Name", input.Name);
                naru.xml.XMLHelpers.AddNode(nodInput, "Description", input.Description);
                naru.xml.XMLHelpers.AddNode(nodInput, "Source", input.Source);
            }

            XmlNode nodOutput = nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Output"));
            naru.xml.XMLHelpers.AddNode(nodOutput, "Name", OutputName);
            naru.xml.XMLHelpers.AddNode(nodOutput, "Units", OutputUnits);
            naru.xml.XMLHelpers.AddNode(nodOutput, "Description", OutputDescription);

            SerializeMetaDataList(nodItem, Publications, "Publications", "Publication", "Citation", "URL");
            SerializeMetaDataList(nodItem, ExampleDatasets, "ExampleDatasets", "ExampleDataset", "Title", "URL");
        }

        private void SerializeMetaDataList(XmlNode nodParent, naru.ui.SortableBindingList<FISMetaItem> items, string groupNode, string ItemNode, string keyNode, string valueNode)
        {
            XmlNode nodList = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement(groupNode));
            foreach (FISMetaItem ds in items)
            {
                XmlNode nodPub = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement(ItemNode));
                naru.xml.XMLHelpers.AddNode(nodPub, keyNode, ds.Title);
                naru.xml.XMLHelpers.AddNode(nodPub, valueNode, ds.Value);
            }
        }

        private static string GetSafeNodeValue(XmlNode nodItem, string ChildNode)
        {
            XmlNode nodChild = nodItem.SelectSingleNode(ChildNode);
            if (nodChild is XmlNode)
                return nodChild.InnerText;
            else
                return string.Empty;
        }

        public FISLibraryItem Copy(string fileName, DirectoryInfo destination)
        {
            // Should already exist because it was used for the error surface
            destination.Create();

            FileInfo fisPath = naru.os.File.GetNewSafeName(destination.FullName, fileName, FilePath.Extension);
            FileInfo xmlPath = naru.os.File.GetNewSafeName(destination.FullName, Path.GetFileNameWithoutExtension(fisPath.FullName), FilePath.Extension + ".xml");

            // Copy the FIS rule file
            FilePath.CopyTo(fisPath.FullName);

            // Generate an XML file adjacent to the FIS rule file.
            XmlNode nodParent = FISLibrary.CreateFISLibraryXML();
            Serialize(nodParent);
            nodParent.OwnerDocument.Save(xmlPath.FullName);

            // Reload the FIS library item (lazy way to clone) using the project file path
            XmlNode nodItem = nodParent.SelectSingleNode("FISLibraryItem");
            FISLibraryItem projectFIS = new FISLibraryItem(nodItem, fisPath);

            return projectFIS;
        }
    }
}