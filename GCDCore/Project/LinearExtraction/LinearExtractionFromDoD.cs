using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using GCDCore.Project.ProfileRoutes;

namespace GCDCore.Project.LinearExtraction
{
    public class LinearExtractionFromDoD : LinearExtraction
    {
        public DoDBase DoD;

        public override GCDProjectItem GCDProjectItem { get { return DoD; } }

        public LinearExtractionFromDoD(string name, ProfileRoute route, FileInfo db, decimal sampleDistance, DoDBase dod)
            : base(name, route, db, sampleDistance)
        {
            DoD = dod;
        }

        public LinearExtractionFromDoD(XmlNode nodItem)
            : base(nodItem)
        {
            DoD = ProjectManager.Project.DoDs[nodItem.SelectSingleNode("DoD").InnerText];
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodItem = base.Serialize(nodParent);
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement("DoD")).InnerText = GCDProjectItem.Name;
            return nodItem;
        }
    }
}
