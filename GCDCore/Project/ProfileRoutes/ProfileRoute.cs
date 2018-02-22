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
                throw new NotImplementedException();
            }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
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
