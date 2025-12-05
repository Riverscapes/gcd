using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree.ProfileRoutes
{
    public class ProfileRoute : Vector
    {
        public enum ProfileRouteTypes
        {
            Transect,
            Longitudinal
        };

        public readonly string DistanceField;
        public readonly string LabelField;
        public override string Noun { get { return "Profile Route"; } }
        public readonly ProfileRouteTypes ProfileRouteType;

        public ProfileRoute(GCDProject project, XmlNode nodItem)
            : base(project, nodItem, "longitudinal.png", "longitudinal.png", "")
        {
            DistanceField = nodItem.SelectSingleNode("DistanceField").InnerText;

            XmlNode nodLabel = nodItem.SelectSingleNode("LabelField");
            if (nodLabel is XmlNode)
            {
                LabelField = nodLabel.InnerText;
            }

            XmlNode nodType = nodItem.SelectSingleNode("Type");
            if (nodType is XmlNode)
            {
                ProfileRouteType = (ProfileRouteTypes)Enum.Parse(typeof(ProfileRouteTypes), nodType.InnerText);
            }
            else
            {
                // Needed for backward compatibility
                ProfileRouteType = ProfileRouteTypes.Transect;
            }
        }
    }
}
