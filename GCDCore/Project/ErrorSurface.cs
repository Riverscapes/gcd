﻿using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GCDConsoleLib;
using System;

namespace GCDCore.Project
{
    public class ErrorSurface : GCDProjectRasterItem
    {
        public readonly Surface Surf;
        public readonly Dictionary<string, ErrorSurfaceProperty> ErrorProperties;

        public override string Noun { get { return "Error Surface"; } }

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
        /// An error surface is in use if it is used by a propagated or probabilistic DoD
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                foreach (DoDPropagated dod in ProjectManager.Project.DoDs.Values.Where(x => x is DoDPropagated))
                {
                    if (dod.NewError == this || dod.OldError == this)
                        return true;
                }

                return false;
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

        public ErrorSurface(string name, FileInfo rasterPath, Surface surf, bool isDefault, ErrorSurfaceProperty errProperty)
            : base(name, rasterPath)
        {
            Surf = surf;
            _IsDefault = isDefault;
            ErrorProperties = new Dictionary<string, ErrorSurfaceProperty>();
            ErrorProperties[errProperty.Name] = errProperty;
        }

        public ErrorSurface(XmlNode nodError, Surface surf)
            : base(nodError)
        {
            _IsDefault = bool.Parse(nodError.SelectSingleNode("IsDefault").InnerText);
            Surf = surf;

            // There might not be any error surface properties if the error raster was "specified" raster than calculated
            Dictionary<string, ErrorSurfaceProperty> properties = new Dictionary<string, ErrorSurfaceProperty>();
            foreach (XmlNode nodProperty in nodError.SelectNodes("ErrorSurfaceProperties/ErrorSurfaceProperty"))
            {
                ErrorSurfaceProperty prop = new ErrorSurfaceProperty(nodProperty, surf);
                properties[prop.Name] = prop;
            }
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodError = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("ErrorSurface"));
            nodError.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodError.AppendChild(nodParent.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.GISFileInfo);
            nodError.AppendChild(nodParent.OwnerDocument.CreateElement("IsDefault")).InnerText = _IsDefault.ToString();

            if (ErrorProperties != null)
            {
                XmlNode nodProperties = nodError.AppendChild(nodParent.OwnerDocument.CreateElement("ErrorSurfaceProperties"));
                foreach (ErrorSurfaceProperty props in ErrorProperties.Values)
                {
                    XmlNode nodProperty = nodProperties.AppendChild(nodParent.OwnerDocument.CreateElement("ErrorSurfaceProperty"));
                    props.Serialize(nodProperty);
                }
            }
        }

        public override void Delete()
        {
            base.Delete();

            // delete the associated surfaces group folder if this was the last associated surface
            if (!Directory.EnumerateFileSystemEntries(Raster.GISFileInfo.Directory.Parent.FullName).Any())
            {
                Raster.GISFileInfo.Directory.Parent.Delete();
            }

            Surf.ErrorSurfaces.Remove(this);
            ProjectManager.Project.Save();
        }
    }
}