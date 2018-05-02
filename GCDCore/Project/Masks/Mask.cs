using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project.Masks
{
    public abstract class Mask : GCDProjectVectorItem
    {
        public Mask(string name, FileInfo shapeFile)
            : base(name, shapeFile)
        {
        }

        /// <summary>
        /// Deserialize from XML
        /// </summary>
        /// <param name="nodParent"></param>
        public Mask(XmlNode nodParent)
            : base(nodParent)
        {
        }

        public virtual XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Mask"));
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Vector.GISFileInfo);
            return nodMask;
        }

        public override bool IsItemInUse
        {
            get
            {
                foreach (DoDBase dod in ProjectManager.Project.DoDs)
                {
                    // Check if any DoDs use this mask as an AOI
                    if (dod.AOIMask != null && dod.AOIMask.Equals(this))
                    {
                        return true;
                    }

                    // Check if any budget segregations use this mask
                    foreach (BudgetSegregation bs in dod.BudgetSegregations)
                    {
                        if (bs.Mask.Equals(this))
                        {
                            return true;
                        }
                    }
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

            // Remove the mask from the project
            ProjectManager.Project.Masks.Remove(this);

            // Delete the individual mask folder
            try
            {
                Vector.GISFileInfo.Directory.Delete();
            }
            catch (Exception ex)
            {
                Console.Write(string.Format("Failed to delete the {0} directory {1}\n\n{2}", Noun, Vector.GISFileInfo.Directory.FullName, ex.Message));
            }

            // If no more masks then delete the project masks folder
            if (ProjectManager.Project.Masks.Count < 1 && !Directory.EnumerateFileSystemEntries(Vector.GISFileInfo.Directory.Parent.FullName).Any())
            {
                try
                {
                    Vector.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write(string.Format("Failed to delete empty mask directory {0}\n\n{1}", Vector.GISFileInfo.Directory.Parent.FullName, ex.Message));
                }
            }

            ProjectManager.Project.Save();
        }
    }
}
