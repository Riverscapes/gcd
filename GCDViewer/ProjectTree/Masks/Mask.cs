using ArcGIS.Core.Internal.CIM;
using ArcGIS.Desktop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree.Masks
{
    public class Mask : Vector
    {
        public Mask(GCDProject project, string name, string path)
     : base(project, name, path, "mask.png", "mask.png", "")
        {

        }
        public Mask(GCDProject project, XmlNode nodParent, string image_Exists, string image_Missing)
               : base(project, nodParent, image_Exists, image_Missing, "")
        {
        }
    }
}
