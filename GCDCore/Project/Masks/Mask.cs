using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project.Masks
{
    public abstract class Mask : GCDProjectItem
    {
        public readonly string _Field;
        public readonly System.IO.FileInfo _ShapeFile;

        public Mask(string name, System.IO.FileInfo shapeFile, string field)
            : base(name)
        {
            _ShapeFile = shapeFile;
            _Field = field;
        }

        /// <summary>
        /// Returns a dictionary of dictinct field values keyed to the label
        /// </summary>
        /// <remarks>Regular masks return a filtered list by whether each
        /// field value is set to be "Included". Directional masks simply
        /// return all distinct values from the field.
        /// 
        /// Field values are keyed to labels to use in the user interface.
        /// There is always a label. It might be the same as the field value.</remarks>
        public abstract Dictionary<string, string> ActiveFieldValues { get; }

        /// <summary>
        /// Returns a read only label indicating if the mask is "Regular Mask" or "Directional Mask"
        /// </summary>
        public abstract string MaskTypeLabel { get; }

        /// <summary>
        /// Deserialize from XML
        /// </summary>
        /// <param name="nodParent"></param>
        public Mask(XmlNode nodParent)
            : base(nodParent.SelectSingleNode("Name").InnerText)
        {
            _ShapeFile = ProjectManager.Project.GetAbsolutePath(nodParent.SelectSingleNode("ShapeFile").InnerText);
            _Field = nodParent.SelectSingleNode("Field").InnerText;
        }

        public virtual XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Mask"));
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("ShapeFile")).InnerText = ProjectManager.Project.GetRelativePath(_ShapeFile);
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Field")).InnerText = _Field;
            return nodMask;
        }

        public void Delete()
        {
            try
            {
                // Raise the event to say that a GIS layer is about to be deleted.
                // This should bubble to ArcGIS so that the layer is removed from the ArcMap ToC
                ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(_ShapeFile));

                // Delete the actual raster
                GCDConsoleLib.Vector vShapeFile = new GCDConsoleLib.Vector(_ShapeFile);
                vShapeFile.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error attempting to delete mask ShapeFile " + _ShapeFile.FullName);
                Console.WriteLine("ShapeFile Path: ", _ShapeFile.FullName);
                Console.WriteLine(ex.Message);
            }


            // Remove the mask from the project
            ProjectManager.Project.Masks.Remove(Name);

            // Delete the individual mask folder
            try
            {
                _ShapeFile.Directory.Delete();
            }
            catch (Exception ex)
            {
                Console.Write("Failed to delete the mask directory " + _ShapeFile.Directory.FullName);
            }

            // If no more masks then delete the project masks folder
            if (ProjectManager.Project.Masks.Count < 1 && !Directory.EnumerateFileSystemEntries(_ShapeFile.Directory.Parent.FullName).Any())
            {
                try
                {
                    _ShapeFile.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write("Failed to delete empty mask directory " + _ShapeFile.Directory.Parent.FullName);
                }
            }

            ProjectManager.Project.Save();
        }
    }
}
