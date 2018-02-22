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

        public override string Noun { get { return "Profile Route"; } }

        public ProfileRoute(string name, FileInfo shpPath)
            : base(name)
        {
            FeatureClass = new Vector(shpPath);
        }

        public ProfileRoute(XmlNode nodItem)
            : base(nodItem.SelectSingleNode("Name").InnerText)
        {
            FeatureClass = new Vector(ProjectManager.Project.GetAbsolutePath(nodItem.SelectSingleNode("Path").InnerText));
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
    }
}
