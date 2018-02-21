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
        public readonly string LabelField;
        public readonly string DirectionField;
        public readonly string DistanceField;

        public override string Noun { get { return "Directional Mask"; } }

        public override Dictionary<string, string> ActiveFieldValues
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                bool bUseLabel = !string.IsNullOrEmpty(LabelField);

                GCDConsoleLib.Vector polygons = new GCDConsoleLib.Vector(_ShapeFile);
                foreach (GCDConsoleLib.VectorFeature feat in polygons.Features.Values)
                {
                    if (!feat.IsNull(_Field))
                    {
                        string value = feat.GetFieldAsString(_Field);
                        if (!result.ContainsKey(value))
                        {
                            if (bUseLabel)
                                result[value] = feat.IsNull(LabelField) ? value : feat.GetFieldAsString(LabelField);
                            else
                                result[value] = value;
                        }
                    }
                }

                return result;
            }
        }

        public SortedList<int, Tuple<string, string>> SortedFieldValues
        {
            get
            {
                SortedList<int, Tuple<string, string>> result = new SortedList<int, Tuple<string, string>>();

                bool bUseLabel = !string.IsNullOrEmpty(LabelField);

                GCDConsoleLib.Vector polygons = new GCDConsoleLib.Vector(_ShapeFile);
                foreach (GCDConsoleLib.VectorFeature feat in polygons.Features.Values)
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

        public DirectionalMask(string name, FileInfo shapefile, string field, string labelField, string directionField, string distanceField)
                : base(name, shapefile, field)
        {
            LabelField = labelField;
            DirectionField = directionField;
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
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodMask = base.Serialize(nodParent);

            nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("DirectionField")).InnerText = DirectionField;

            if (!string.IsNullOrEmpty(LabelField))
                nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("LabelField")).InnerText = LabelField;

            if (!string.IsNullOrEmpty(DistanceField))
                nodMask.AppendChild(nodParent.OwnerDocument.CreateElement("DistanceField")).InnerText = LabelField;

            return nodMask;
        }
    }
}
