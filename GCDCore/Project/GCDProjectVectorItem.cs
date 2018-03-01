using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public abstract class GCDProjectVectorItem : GCDProjectItem
    {
        public readonly Vector Vector;

        public GCDProjectVectorItem(string name, FileInfo shapeFilePath)
            : base(name)
        {
            Vector = new Vector(shapeFilePath);
        }

        public GCDProjectVectorItem(string name, Vector vector)
            : base(name)
        {
            Vector = vector;
        }

        public GCDProjectVectorItem(XmlNode nodItem)
            : base(nodItem)
        {
            Vector = new Vector(ProjectManager.Project.GetAbsolutePath(nodItem.SelectSingleNode("Path").InnerText));
        }

        public override void Delete()
        {
            // Get the folder
            DirectoryInfo dir = Vector.GISFileInfo.Directory;

            // Remove the layer from the ArcGIS map and then delete the dataset
            try
            {
                // Raise the event to say that a GIS layer is about to be deleted.
                // This should bubble to ArcGIS so that the layer is removed from the ArcMap ToC
                ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(Vector.GISFileInfo));

                // Delete the actual raster
                if (Vector.GISFileInfo.Exists)
                    Vector.Delete();
            }
            catch (Exception ex)
            {
                Console.Write("Error attempting to remove raster from ArcGIS " + Vector.GISFileInfo.FullName, ex);
            }

            try
            {
                // Delete empty directory 
                if (dir.Exists && !Directory.EnumerateFileSystemEntries(dir.FullName).Any())
                {
                    dir.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error attempting to delete associated surface directory " + dir.FullName, ex);
            }
        }
    }
}
