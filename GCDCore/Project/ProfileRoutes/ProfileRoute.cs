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
    public class ProfileRoute : GCDProjectVectorItem
    {
        public readonly string DistanceField;
        public readonly string LabelField;
        public override string Noun { get { return "Profile Route"; } }

        public ProfileRoute(string name, FileInfo shpPath, string distanceField, string labelField)
            : base(name, shpPath)
        {
            DistanceField = distanceField;
            LabelField = labelField;
        }

        public ProfileRoute(XmlNode nodItem)
            : base(nodItem)
        {
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
                foreach(DEMSurvey dem in ProjectManager.Project.DEMSurveys)
                {
                    if (dem.LinearExtractions.Any(x => x.Equals(this)))
                        return true; 
                }
                
                foreach (Surface surf in ProjectManager.Project.ReferenceSurfaces)
                {
                    if (surf.LinearExtractions.Any(x => x.Equals(this)))
                        return true;
                }
                foreach (DoDBase dod in ProjectManager.Project.DoDs)
                {
                    if (dod.LinearExtractions.Any(x => x.Equals(this)))
                        return true;
                }

                return false;
            }
        }

        public override void Delete()
        {
            try
            {
                base.Delete();
            }
            catch (Exception ex)
            {
                Console.Write(string.Format("Failed to shapefile associated with the {0} at {1}", Noun, Vector.GISFileInfo.Directory.FullName, ex.Message));
            }

            ProjectManager.Project.ProfileRoutes.Remove(this);

            // If no more profile routes then delete the project routes folder
            if (ProjectManager.Project.Masks.Count < 1 && !Directory.EnumerateFileSystemEntries(Vector.GISFileInfo.Directory.Parent.FullName).Any())
            {
                try
                {
                    Vector.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write(string.Format("Failed to delete empty {0} directory {1}\n\n{2}", Noun, Vector.GISFileInfo.Directory.Parent.FullName, ex.Message));
                }
            }

            ProjectManager.Project.Save();
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodItem = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("ProfileRoute"));
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Vector.GISFileInfo);
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("DistanceField")).InnerText = DistanceField;

            if (!string.IsNullOrEmpty(LabelField))
            {
                nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("LabelField")).InnerText = LabelField;
            }
        }
    }
}
