using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree.Masks
{
    public class AOIMask : Mask
    {
        public override string Noun { get { return "Area of Interest Mask"; } }

        public AOIMask(GCDProject project, XmlNode nodParent) : base(project, nodParent, "mask.png", "mask.png")
        {

        }
    }
}
