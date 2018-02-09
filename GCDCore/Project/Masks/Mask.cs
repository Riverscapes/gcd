using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project.Masks
{
    public class Mask : GCDProjectItem
    {
        public readonly string _Field;
        public readonly System.IO.FileInfo _ShapeFile;

        public Mask(string name, System.IO.FileInfo shapeFile, string field)
            : base(name)
        {
            _ShapeFile = shapeFile;
            _Field = field;
        }

        /// <summary>
        /// Deserialize from XML
        /// </summary>
        /// <param name="nodParent"></param>
        public Mask(XmlNode nodParent)
            :base(nodParent.SelectSingleNode("Name").InnerText)
        {
            _ShapeFile = ProjectManager.Project.GetAbsolutePath(nodParent.SelectSingleNode("ShapeFile").InnerText);
            _Field = nodParent.SelectSingleNode("Field").InnerText;
        }

        public XmlNode  Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Mask"));
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("ShapeFile")).InnerText = ProjectManager.Project.GetRelativePath(_ShapeFile);
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Field")).InnerText = _Field;
            return nodMask;
        }
    }
}
