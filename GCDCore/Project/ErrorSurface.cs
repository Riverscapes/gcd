using System.IO;
using System.Collections.Generic;
using System.Xml;
using GCDConsoleLib;
using System;

namespace GCDCore.Project
{
    public class ErrorSurface : GCDProjectRasterItem
    {
        public readonly Surface Surf;
        public readonly Dictionary<string, ErrorSurfaceProperty> ErrorProperties;

        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set
            {
                if (value)
                {
                    foreach (ErrorSurface errSurf in Surf.ErrorSurfaces)
                        errSurf._IsDefault = false;
                }

                _IsDefault = value;
            }
        }

        // Certain user interface controls want to display which surface is the default
        public string NameWithDefault
        {
            get
            {
                if (IsDefault)
                    return string.Format("{0} (Default)", Name);
                else
                    return Name;
            }
        }


        /// <summary>
        /// Constructor for specifying error surface raster. i.e. has not error properties dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rasterPath"></param>
        /// <param name="dem"></param>
        public ErrorSurface(string name, FileInfo rasterPath, Surface surf)
            : base(name, rasterPath)
        {
            Surf = surf;

            // The first error surface for a DEM is always the default
            _IsDefault = surf.ErrorSurfaces.Count == 0;

            // Empty dictionary of properties
            ErrorProperties = new Dictionary<string, ErrorSurfaceProperty>();
        }

        public ErrorSurface(string name, FileInfo rasterPath, Surface surf, bool isDefault, Dictionary<string, ErrorSurfaceProperty> errProperties)
            : base(name, rasterPath)
        {
            Surf = surf;
            _IsDefault = isDefault;
            ErrorProperties = errProperties;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodError = nodParent.AppendChild(xmlDoc.CreateElement("ErrorSurface"));
            nodError.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodError.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.GISFileInfo);
            nodError.AppendChild(xmlDoc.CreateElement("IsDefault")).InnerText = _IsDefault.ToString();

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

        public static ErrorSurface Deserialize(XmlNode nodError, Surface surf)
        {
            string name = nodError.SelectSingleNode("Name").InnerText;
            FileInfo path = ProjectManager.Project.GetAbsolutePath(nodError.SelectSingleNode("Path").InnerText);
            bool bIsDefault = bool.Parse(nodError.SelectSingleNode("IsDefault").InnerText);

            // There might not be any error surface properties if the error raster was "specified" raster than calculated
            Dictionary<string, ErrorSurfaceProperty> properties = new Dictionary<string, ErrorSurfaceProperty>();
            foreach (XmlNode nodProperty in nodError.SelectNodes("ErrorSurfaceProperties/ErrorSurfaceProperty"))
            {
                ErrorSurfaceProperty prop = ErrorSurfaceProperty.Deserialize(nodProperty, surf);
                properties[prop.Name] = prop;
            }

            return new ErrorSurface(name, path, surf, bIsDefault, properties); ;
        }

    }
}