using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project.Masks
{
    public class RegularMask : Mask
    {
        public readonly List<MaskItem> _Items;

        public override string MaskTypeLabel { get { return "Regular Mask"; } }

        public RegularMask(string name, FileInfo shapeFile, string field, List<MaskItem> items)
            : base(name, shapeFile, field)
        {
            _Items = items;
        }

        public override Dictionary<string, string> ActiveFieldValues
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (MaskItem item in _Items.Where(x => x.Include))
                {
                    if (!result.ContainsKey(item.FieldValue))
                        result[item.FieldValue] = string.IsNullOrEmpty(item.Label) ? item.FieldValue : item.Label;
                }

                return result;
            }
        }

        public RegularMask(XmlNode nodParent)
            : base(nodParent)
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

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = base.Serialize(nodParent);

            XmlNode nodItems = nodMask.AppendChild(nodMask.OwnerDocument.CreateElement("Items"));
            foreach (MaskItem item in _Items)
            {
                XmlNode nodItem = nodItems.AppendChild(nodItems.OwnerDocument.CreateElement("Item"));
                nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("FieldValue")).InnerText = item.FieldValue;
                nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("Include")).InnerText = item.Include.ToString();
                if (string.Compare(item.FieldValue, item.Label, true) != 0)
                {
                    nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("Label")).InnerText = item.Label;
                }
            }

            return nodMask;
        }
    }
}
