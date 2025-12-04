/*
 * Hieerarchy of dataset classes
 * 
 * BaseDataset
 *    FileSystemDataset
 *       GISDataset (IGISLayer)
 *          Raster
 *          TIN (IGISLayer)
 *          Vector (IGISLayer)
 *     WMSLayer
 * 
 *  ProjectDataset
 *     GISLayer
 * 
 * 
 */

using System.Collections.Generic;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public abstract class BaseDataset : ITreeItem
    {
        public string Name { get; private set; }
        public string Id { get; private set; }

        // No folder and no file extension
        private readonly string ImageFileNameExists;
        private readonly string ImageFileNameMissing;

        public string ImagePath => System.IO.Path.Combine("Layers", ImageFileName);

        public abstract bool Exists { get; }

        public string ImageFileName { get { return Exists ? ImageFileNameExists : ImageFileNameMissing; } }

        public BaseDataset(string name, string imageFileNameExists, string imageFileNameMissing, string id)
        {
            Name = name;
            ImageFileNameExists = imageFileNameExists;
            ImageFileNameMissing = imageFileNameExists;
            Id = id;
        }
        public static Dictionary<string, string> LoadMetadata(XmlNode nodeParent)
        {
            // Load the layer metadata
            Dictionary<string, string> metadata = null;
            XmlNode nodMetadata = nodeParent.SelectSingleNode("MetaData");
            if (nodMetadata is XmlNode && nodMetadata.HasChildNodes)
            {
                metadata = new Dictionary<string, string>();
                foreach (XmlNode nodMeta in nodMetadata.SelectNodes("Meta"))
                {
                    XmlAttribute attName = nodMeta.Attributes["name"];
                    if (attName is XmlAttribute && !string.IsNullOrEmpty(attName.InnerText))
                    {
                        if (!string.IsNullOrEmpty(nodMeta.InnerText))
                        {
                            metadata.Add(attName.InnerText, nodMeta.InnerText);
                        }
                    }
                }
            }

            return metadata;
        }
    }

}
