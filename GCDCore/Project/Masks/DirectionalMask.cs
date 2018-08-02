using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDCore.Project.Masks
{
    public class DirectionalMask : AttributeFieldMask
    {
        public string LabelField { get; set; }
        public readonly string DirectionField;
        public string DistanceField { get; set; }
        public bool Ascending { get; set; }

        public override string Noun { get { return "Directional Mask"; } }

        /// <summary>
        /// Returns a dictionary of field values keyed to their display text (either the field value or label)
        /// </summary>
        /// <remarks>
        /// The dictionary will only contain fields that have a non-Null field value.
        /// </remarks>
        public override List<MaskItem> ActiveFieldValues
        {
            get
            {
                List<MaskItem> result = new List<MaskItem>();

                bool bUseLabel = !string.IsNullOrEmpty(LabelField);


                foreach (GCDConsoleLib.VectorFeature feat in Vector.Features.Values)
                {
                    if (!feat.IsNull(_Field))
                    {
                        string value = feat.GetFieldAsString(_Field);
                        int direction = feat.GetFieldAsInt(DirectionField);

                        if (!result.Any(x => x.FieldValue == value))
                        {
                            string label = bUseLabel ? feat.IsNull(LabelField) ? value : feat.GetFieldAsString(LabelField) : value;

                            result.Add(new DirectionMaskItem(true, value, label, direction));
                        }
                    }
                }

                // Sort the items by their direction value.
                result.Sort((a, b) => ((DirectionMaskItem)a).CompareTo((DirectionMaskItem)b));

                // Reverse the sorting if the values descend downstream
                if (!Ascending)
                    result.Reverse();

                return result;
            }
        }

        public SortedList<int, Tuple<string, string>> SortedFieldValues
        {
            get
            {
                SortedList<int, Tuple<string, string>> result = new SortedList<int, Tuple<string, string>>();

                bool bUseLabel = !string.IsNullOrEmpty(LabelField);

                foreach (GCDConsoleLib.VectorFeature feat in Vector.Features.Values)
                {
                    if (!feat.IsNull(_Field) && !feat.IsNull(DirectionField))
                    {
                        int directionValue = feat.GetFieldAsInt(DirectionField);
                        string fieldValue = feat.GetFieldAsString(_Field);
                        string labelValue = fieldValue;

                        if (bUseLabel && !feat.IsNull(LabelField))
                            labelValue = feat.GetFieldAsString(LabelField);

                        result.Add(directionValue, new Tuple<string, string>(fieldValue, labelValue));
                    }
                }

                return result;
            }
        }

        public DirectionalMask(string name, FileInfo shapefile, string field, string labelField, string directionField, bool ascending, string distanceField)
                : base(name, shapefile, field)
        {
            LabelField = labelField;
            DirectionField = directionField;
            Ascending = ascending;
            DistanceField = distanceField;
        }

        public DirectionalMask(XmlNode nodParent)
                        : base(nodParent)
        {
            DirectionField = nodParent.SelectSingleNode("DirectionField").InnerText;

            XmlNode nodLabel = nodParent.SelectSingleNode("LabelField");
            if (nodLabel is XmlNode)
            {
                LabelField = nodLabel.InnerText;
            }

            XmlNode nodDistance = nodParent.SelectSingleNode("DistanceField");
            if (nodDistance is XmlNode)
            {
                DistanceField = nodDistance.InnerText;
            }

            XmlNode nodAscending = nodParent.SelectSingleNode("Ascending");
            if (nodAscending is XmlNode)
            {
                Ascending = bool.Parse(nodAscending.InnerText);
            }
            else
            {
                Ascending = true;
            }
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = base.Serialize(nodParent);

            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("DirectionField")).InnerText = DirectionField;

            if (!string.IsNullOrEmpty(LabelField))
                nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("LabelField")).InnerText = LabelField;

            if (!string.IsNullOrEmpty(DistanceField))
                nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("DistanceField")).InnerText = LabelField;

            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("Ascending")).InnerText = Ascending.ToString();

            return nodMask;
        }
    }
}
