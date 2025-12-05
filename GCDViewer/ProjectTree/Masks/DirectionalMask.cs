using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree.Masks
{
    public class DirectionalMask : Mask
    { 
        public string LabelField { get; set; }
        public readonly string DirectionField;
        public string DistanceField { get; set; }
        public bool Ascending { get; set; }


        public override string Noun { get { return "Directional Mask"; } }

        public DirectionalMask(GCDProject project, string name, string path) : base(project, name, path)
        {

        }

        public DirectionalMask(GCDProject project, XmlNode nodParent)
                      : base(project, nodParent, "mask_dir.png", "mask_dir.png")
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
    }
}
