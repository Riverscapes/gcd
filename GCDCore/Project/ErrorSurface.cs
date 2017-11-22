using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace GCDCore.Project
{
    public class ErrorSurface : GCDProjectItem
    {
        public readonly ProjectRaster Raster;
        public readonly DEMSurvey DEM;

        public readonly Dictionary<string, ErrorSurfaceProperty> ErrorProperties;

        /// <summary>
        /// Constructor for specifying error surface raster. i.e. has not error properties dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rasterPath"></param>
        /// <param name="dem"></param>
        public ErrorSurface(string name, FileInfo rasterPath, DEMSurvey dem)
            : base(name)
        {
            Raster = new ProjectRaster(rasterPath);
            DEM = dem;

            // Empty dictionary of properties
            ErrorProperties = new Dictionary<string, ErrorSurfaceProperty>();
        }

        public ErrorSurface(string name, FileInfo rasterPath, DEMSurvey dem, Dictionary<string, ErrorSurfaceProperty> errProperties)
            : base(name)
        {
            Raster = new ProjectRaster(rasterPath);
            DEM = dem;

            ErrorProperties = errProperties;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodError = nodParent.AppendChild(xmlDoc.CreateElement("ErrorSurface"));
            nodError.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodError.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.RasterPath);

            if (ErrorProperties != null)
            {
                XmlNode nodProperties = nodError.AppendChild(xmlDoc.CreateElement("ErrorSurfaceProperties"));
                foreach (ErrorSurfaceProperty props in ErrorProperties.Values)
                {
                    XmlNode nodProperty = nodProperties.AppendChild(xmlDoc.CreateElement("ErrorSurfaceProperty"));
                    props.Serialize(xmlDoc, nodProperty);
                }
            }
        }

        public static ErrorSurface Deserialize(XmlNode nodError, DEMSurvey dem)
        {
            string name = nodError.SelectSingleNode("Name").InnerText;
            FileInfo path = ProjectManager.Project.GetAbsolutePath(nodError.SelectSingleNode("Path").InnerText);

            // There might not be any error surface properties if the error raster was "specified" raster than calculated
            Dictionary<string, ErrorSurfaceProperty> properties = new Dictionary<string, ErrorSurfaceProperty>();
            foreach (XmlNode nodProperty in nodError.SelectNodes("ErrorSurfaceProperties/ErrorSurfaceProperty"))
            {
                ErrorSurfaceProperty prop = ErrorSurfaceProperty.Deserialize(nodProperty, dem);
                properties[prop.Name] = prop;
            }

            return new ErrorSurface(name, path, dem, properties); ;
        }
    }
}