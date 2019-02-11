using System;
using System.IO;
using System.Xml;
using GCDConsoleLib;
using System.ComponentModel;
using System.Linq;

namespace GCDCore.Project
{
    public class AssocSurface : GCDProjectRasterItem
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

        /// <summary>
        /// Needed to support putting these items in a DataGridView combo column
        /// </summary>
        public AssocSurface This { get { return this; } }

        public override string Noun { get { return "Associated Surface"; } }

        /// <summary>
        /// Associated surface are not used by any other items
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// GIS legend label for the associated surface
        /// </summary>
        /// <remarks>This isn't the ToC label, but instead it's the label
        /// that appears above the legend to describe the symbology</remarks>
        public string LayerHeader
        {
            get
            {
                string header = string.Empty;
                switch (AssocSurfaceType)
                {
                    case AssociatedSurfaceTypes.GrainSizeStatic:
                        header = "D50 Size Category (mm)";
                        break;

                    case AssociatedSurfaceTypes.InterpolationError:
                    case AssociatedSurfaceTypes.ElevationUncertainty:
                    case AssociatedSurfaceTypes.PointQuality3D:
                        header = string.Format("Uncertainty ({0})", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
                        break;

                    case AssociatedSurfaceTypes.PointDensity:
                        header = string.Format("pts/{0}²", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.HorizUnit));
                        break;

                    case AssociatedSurfaceTypes.Roughness:
                        header = UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit);
                        break;

                    case AssociatedSurfaceTypes.SlopeDegree:
                        header = "Slope (degrees)";
                        break;

                    case AssociatedSurfaceTypes.SlopePercent:
                        header = "Slope (pecent rise)";
                        break;
                }

                return header;
            }
        }

        public static BindingList<naru.db.NamedObject> GetAssocatedSurfaceTypes()
        {
            BindingList<naru.db.NamedObject> assocTypes = new BindingList<naru.db.NamedObject>
            {
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.PointDensity, "Point Density"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.SlopeDegree, "Slope (Degrees)"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.SlopePercent, "Slope (Percent)"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.Roughness, "Roughness"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.PointQuality3D, "3D Point Quality"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.InterpolationError, "Interpolation Error"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.ElevationUncertainty, "Elevation Uncertainty"),
                new naru.db.NamedObject((long)AssociatedSurfaceTypes.Other, "Unknown")
            };

            return assocTypes;
        }

        public AssocSurface(string name, FileInfo rasterPath, DEMSurvey dem, AssociatedSurfaceTypes eType)
                    : base(name, rasterPath)
        {
            AssocSurfaceType = eType;
            DEM = dem;
        }

        public AssocSurface(XmlNode nodAssoc, DEMSurvey dem)
            : base(nodAssoc)
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

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodAssoc = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("AssociatedSurface"));
            nodAssoc.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodAssoc.AppendChild(nodParent.OwnerDocument.CreateElement("Type")).InnerText = AssocSurfaceType.ToString();
            nodAssoc.AppendChild(nodParent.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.GISFileInfo);
        }

        public override void Delete()
        {
            base.Delete();

            // delete the associated surfaces group folder if this was the last associated surface
            if (Raster.GISFileInfo.Directory.Parent.Exists && !Directory.EnumerateFileSystemEntries(Raster.GISFileInfo.Directory.Parent.FullName).Any())
            {
                Raster.GISFileInfo.Directory.Parent.Delete();
            }

            DEM.AssocSurfaces.Remove(this);
            ProjectManager.Project.Save();
        }
    }
}
