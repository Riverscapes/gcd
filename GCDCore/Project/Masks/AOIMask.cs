using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDCore.Project.Masks
{
    public class AOIMask : Mask
    {
        public const string SurfaceDataExtentIntersection = "Intersection of new and old surface data extents";

        public override string MaskTypeLabel { get { return "Area of Interest Mask"; } }

        public AOIMask(string name, FileInfo shapeFile)
            : base(name, shapeFile)
        {

        }

        public AOIMask(XmlNode nodParent)
            : base(nodParent)
        {

        }
    }
}
