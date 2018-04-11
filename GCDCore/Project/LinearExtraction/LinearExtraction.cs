using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project.ProfileRoutes;
using GCDCore.Project;

namespace GCDCore.Project.LinearExtraction
{
    public abstract class LinearExtraction : GCDCore.Project.GCDProjectItem
    {
        public readonly decimal SampleDistance;
        public readonly ProfileRoute ProfileRoute;
        public readonly FileInfo Database;
        public abstract GCDProjectItem GCDProjectItem { get; }

        public override string Noun { get { return "Linear Extraction"; } }

        // Linear extractions are not used by another other project item
        public override bool IsItemInUse { get { return false; } }

        public LinearExtraction(string name, ProfileRoute route, FileInfo db, decimal sampleDistance)
            : base(name)
        {
            ProfileRoute = route;
            Database = db;
            SampleDistance = sampleDistance;
        }

        public LinearExtraction(XmlNode nodItem)
            : base(nodItem)
        {
            ProfileRoute = ProjectManager.Project.ProfileRoutes[nodItem.SelectSingleNode("ProfileRoute").InnerText];
            Database = ProjectManager.Project.GetAbsolutePath(nodItem.SelectSingleNode("Database").InnerText);
            SampleDistance = decimal.Parse(nodItem.SelectSingleNode("SampleDistance").InnerText);
        }

        public virtual XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodLE = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("LinearExtraction"));
            nodLE.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodLE.AppendChild(nodParent.OwnerDocument.CreateElement("SampleDistance")).InnerText = SampleDistance.ToString();
            nodLE.AppendChild(nodParent.OwnerDocument.CreateElement("ProfileRoute")).InnerText = ProfileRoute.Name;
            nodLE.AppendChild(nodParent.OwnerDocument.CreateElement("Database")).InnerText = ProjectManager.Project.GetRelativePath(Database);
            return nodLE;
        }

        public override void Delete()
        {
            if (GCDConsoleLib.Utility.FileHelpers.IsFileLocked(Database.FullName, FileAccess.ReadWrite))
            {
                Exception ex = new IOException("File locked and in use.");
                ex.Data["Locks"] = Database.FullName;
                throw ex;
            }

            Database.Delete();
            Database.Refresh();

            if (!Directory.EnumerateFileSystemEntries(Database.DirectoryName).Any())
            {
                Database.Directory.Delete();
            }

            if (!Directory.EnumerateFileSystemEntries(Database.Directory.Parent.FullName).Any())
            {
                Database.Directory.Parent.Delete();
            }

            if (GCDProjectItem is DoDBase)
            {
                ((DoDBase)GCDProjectItem).LinearExtractions.Remove(Name);
            }
            else
            {
                ((Surface)GCDProjectItem).LinearExtractions.Remove(Name);
            }
            ProjectManager.Project.Save();
        }
    }
}
