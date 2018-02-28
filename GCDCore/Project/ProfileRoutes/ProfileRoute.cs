using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib;

namespace GCDCore.Project.ProfileRoutes
{
    public class ProfileRoute : GCDProjectItem
    {
        public readonly Vector FeatureClass;
        public readonly string DistanceField;
        public readonly string LabelField;
        public override string Noun { get { return "Profile Route"; } }

        public ProfileRoute(string name, FileInfo shpPath, string distanceField, string labelField)
            : base(name)
        {
            FeatureClass = new Vector(shpPath);
            DistanceField = distanceField;
            LabelField = labelField;
        }

        public ProfileRoute(XmlNode nodItem)
            : base(nodItem.SelectSingleNode("Name").InnerText)
        {
            FeatureClass = new Vector(ProjectManager.Project.GetAbsolutePath(nodItem.SelectSingleNode("Path").InnerText));
            DistanceField = nodItem.SelectSingleNode("DistanceField").InnerText;

            XmlNode nodLabel = nodItem.SelectSingleNode("LabelField");
            if (nodLabel is XmlNode)
            {
                LabelField = nodLabel.InnerText;
            }
        }

        public override bool IsItemInUse
        {
            get
            {
                // Check if any linear extractions use this profile route
                return ProjectManager.Project.LinearExtractions.Values.Any(x => x.Equals(this));
            }
        }

        public override void Delete()
        {
            try
            {
                // Raise the event to say that a GIS layer is about to be deleted.
                // This should bubble to ArcGIS so that the layer is removed from the ArcMap ToC
                ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(FeatureClass.GISFileInfo));

                // Delete the actual raster
                FeatureClass.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error attempting to delete mask ShapeFile " + FeatureClass.GISFileInfo.FullName);
                Console.WriteLine(ex.Message);
            }

            // Remove the profile route from the project
            ProjectManager.Project.ProfileRoutes.Remove(Name);

            // Delete the individual routes folder
            try
            {
                FeatureClass.GISFileInfo.Directory.Delete();
            }
            catch (Exception ex)
            {
                Console.Write(string.Format("Failed to delete the mask directory {0}\n\n{1}", FeatureClass.GISFileInfo.Directory.FullName, ex.Message));
            }

            // If no more profile routes then delete the project routes folder
            if (ProjectManager.Project.Masks.Count < 1 && !Directory.EnumerateFileSystemEntries(FeatureClass.GISFileInfo.Directory.Parent.FullName).Any())
            {
                try
                {
                    FeatureClass.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write(string.Format("Failed to delete empty mask directory {0}\n\n{1}", FeatureClass.GISFileInfo.Directory.Parent.FullName, ex.Message));
                }
            }

            ProjectManager.Project.Save();
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodItem = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("ProfileRoute"));
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(FeatureClass.GISFileInfo);
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("DistanceField")).InnerText = DistanceField;

            if (!string.IsNullOrEmpty(LabelField))
            {
                nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("LabelField")).InnerText = LabelField;
            }
        }
    }
}
