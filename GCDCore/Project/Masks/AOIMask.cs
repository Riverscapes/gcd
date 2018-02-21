using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace GCDCore.Project.Masks
{
    public class AOIMask : Mask
    {
        public const string SurfaceDataExtentIntersection = "Intersection of new and old surface data extents";

        public override string MaskTypeLabel { get { return "Area of Interest Mask"; } }

        // An AOI is in use if it is referred to by any DoD
        public override bool IsItemInUse
        {
            get
            {
                return ProjectManager.Project.DoDs.Values.Any(x => x.AOIMask == this);
            }
        }

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
