using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDCore.Project.Masks
{
    public abstract class AttributeFieldMask : Mask
    {
        public readonly string _Field;
 
        /// <summary>
        /// Returns a dictionary of dictinct field values keyed to the label
        /// </summary>
        /// <remarks>Regular masks return a filtered list by whether each
        /// field value is set to be "Included". Directional masks simply
        /// return all distinct values from the field.
        /// 
        /// Field values are keyed to labels to use in the user interface.
        /// There is always a label. It might be the same as the field value.</remarks>
        public abstract Dictionary<string, string> ActiveFieldValues { get; }
        
        public AttributeFieldMask(string name, FileInfo shapeFile, string field)
            : base(name, shapeFile)
        {
            _Field = field;
        }

        /// <summary>
        /// Deserialize from XML
        /// </summary>
        /// <param name="nodParent"></param>
        public AttributeFieldMask(XmlNode nodParent)
            : base(nodParent)
        {
            _Field = nodParent.SelectSingleNode("Field").InnerText;
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = base.Serialize(nodParent);
            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Field")).InnerText = _Field;
            return nodMask;
        }
    }
}
