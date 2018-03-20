using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

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
        public readonly List<FISInputMeta> Inputs;
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

        public FISLibraryItem(string sName, System.IO.FileInfo fiPath, FISLibrary.FISLibraryItemTypes eType)
        {
            FISType = eType;
            Name = sName;
            FilePath = fiPath;
            Inputs = new List<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();
        }

        public FISLibraryItem(string fisfilePath)
        {
            FISType = FISLibrary.FISLibraryItemTypes.User;
            FilePath = new System.IO.FileInfo(fisfilePath);
            Inputs = new List<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();
        }

        public FISLibraryItem(XmlNode nodFISItem, FISLibrary.FISLibraryItemTypes eType)
        {
            Name = nodFISItem.SelectSingleNode("Name").InnerText;
            FISType = eType;
            FilePath = new System.IO.FileInfo(nodFISItem.SelectSingleNode("FilePath").InnerText);
            Description = GetSafeNodeValue(nodFISItem, "Description");

            Inputs = new List<FISInputMeta>();
            Publications = new naru.ui.SortableBindingList<FISMetaItem>();
            ExampleDatasets = new naru.ui.SortableBindingList<FISMetaItem>();
            Metadata = new naru.ui.SortableBindingList<FISMetaItem>();

            LoadMetaItemList(nodFISItem, "Metadata/Item", "key", "value", Metadata);
            LoadMetaItemList(nodFISItem, "Publications/Publication", "Citation", "URL", Publications);
            LoadMetaItemList(nodFISItem, "ExampleDatasets/ExampleDataset", "Title", "URL", ExampleDatasets);

            foreach (XmlNode nodInput in nodFISItem.SelectNodes("Inputs/Input"))
            {
                string name = GetSafeNodeValue(nodInput, "Name");
                if (!string.IsNullOrEmpty(name))
                {
                    FISInputMeta input = new FISInputMeta(name);
                    input.Description = GetSafeNodeValue(nodInput, "Description");
                    input.Units = GetSafeNodeValue(nodInput, "Units");
                    input.Source = GetSafeNodeValue(nodInput, "Source");
                    Inputs.Add(input);
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

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodItem = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Item"));
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("FilePath")).InnerText = FilePath.FullName;
            naru.xml.XMLHelpers.AddNode(nodItem, "Description", Description);

            SerializeMetaDataList(nodItem, Metadata, "Metadata", "Item", "Key", "Value");
            SerializeMetaDataList(nodItem, Publications, "Publications", "Publication", "Citation", "URL");
            SerializeMetaDataList(nodItem, ExampleDatasets, "ExampleDatasets", "ExampleDataset", "Title", "URL");

            XmlNode nodInputs = nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Inputs"));
            foreach (FISInputMeta input in Inputs)
            {
                XmlNode nodInput = nodInputs.AppendChild(nodParent.OwnerDocument.CreateElement("Input"));
                naru.xml.XMLHelpers.AddNode(nodInput, "Name", input.Name);
                naru.xml.XMLHelpers.AddNode(nodInput, "Description", input.Description);
                naru.xml.XMLHelpers.AddNode(nodInput, "Source", input.Source);
            }
        }

        private void SerializeMetaDataList(XmlNode nodParent, naru.ui.SortableBindingList<FISMetaItem> items, string groupNode, string ItemNode, string keyNode, string valueNode)
        {
            if (items.Count < 1)
                return;

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
    }
}