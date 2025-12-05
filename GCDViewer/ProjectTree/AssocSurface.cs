using ArcGIS.Desktop.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class AssocSurface : Raster
    {
        public enum AssociatedSurfaceTypes
        {
            PointDensity,
            SlopePercent,
            SlopeDegree,
            Roughness,
            GrainSizeStatic,
            PointQuality3D,
            InterpolationError,
            ElevationUncertainty,
            Other
        }

        public AssociatedSurfaceTypes AssocSurfaceType { get; set; }
        public readonly DEMSurvey DEM;

        public AssocSurface(GCDProject project, XmlNode nodAssoc, DEMSurvey dem)
        : base(project, nodAssoc, "AssociatedSurfaces.png", "AssociatedSurfaces.png")
        {
            DEM = dem;
            AssocSurfaceType = AssociatedSurfaceTypes.Other;
            XmlNode nodType = nodAssoc.SelectSingleNode("Type");
            if (nodType is XmlNode && !string.IsNullOrEmpty(nodType.InnerText))
            {
                try
                {
                    AssocSurfaceType = (AssociatedSurfaceTypes)Enum.Parse(typeof(AssociatedSurfaceTypes), nodType.InnerText);
                }
                catch (Exception ex)
                {
                    AssocSurfaceType = AssociatedSurfaceTypes.Other;
                    Console.WriteLine(string.Format("Error reading associated surface type from project XML. Defaulting to {0}\n\n{1}", AssociatedSurfaceTypes.Other, ex.Message));
                }
            }
        }
    }
}
