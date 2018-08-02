using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace GCDCore.Project.Masks
{
    public class RegularMask : AttributeFieldMask
    {
        public readonly List<MaskItem> _Items;

        public override string Noun { get { return "Regular Mask"; } }

        public RegularMask(string name, FileInfo shapeFile, string field, List<MaskItem> items)
            : base(name, shapeFile, field)
        {
            _Items = items;
        }

        public override List<MaskItem> ActiveFieldValues
        {
            get
            {
                List<MaskItem> result = new List<MaskItem>();
                foreach (MaskItem item in _Items.Where(x => x.Include))
                {
                    if (!result.Any(x => x.FieldValue == item.FieldValue))
                        result.Add(new MaskItem(true, item.FieldValue, string.IsNullOrEmpty(item.Label) ? item.FieldValue : item.Label));
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
