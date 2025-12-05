using ArcGIS.Desktop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree.Masks
{
    public class RegularMask : Mask
    {
        public readonly List<MaskItem> _Items;

        public override string Noun { get { return "Regular Mask"; } }

        public RegularMask(GCDProject project, string name, string path) : base(project, name, path)
        {
        }

        public RegularMask(GCDProject project, XmlNode nodParent)
          : base(project, nodParent, "mask.png", "mask.png")
        {
            _Items = new List<MaskItem>();
            foreach (XmlNode nodItem in nodParent.SelectNodes("Items/Item"))
            {
                bool bInclude = bool.Parse(nodItem.SelectSingleNode("Include").InnerText);
                string fieldValue = nodItem.SelectSingleNode("FieldValue").InnerText;

                string label = fieldValue;
                XmlNode nodLabel = nodItem.SelectSingleNode("Label");
                if (nodLabel != null)
                    label = nodItem.SelectSingleNode("Label").InnerText;

                _Items.Add(new MaskItem(bInclude, fieldValue, label));
            }
        }
    }
}
