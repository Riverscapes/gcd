using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDCore.Project
{
    public class Surface : GCDProjectRasterItem
    {
        public readonly naru.ui.SortableBindingList<ErrorSurface> ErrorSurfaces;

        public ErrorSurface DefaultErrorSurface
        {
            get
            {
                if (ErrorSurfaces.Count(x => x.IsDefault) > 0)
                    return ErrorSurfaces.First(x => x.IsDefault);
                else
                    return null;
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
                return string.Format("Elevation ({0})", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            }
        }

        /// <summary>
        /// A surface is in use if it is used as the new or old surface in a DoD
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return ProjectManager.Project.DoDs.Values.Any(x => x.NewSurface == this || x.OldSurface == this);
            }
        }

        public Surface(string name, FileInfo rasterPath)
            : base(name, rasterPath)
        {
            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
        }

        public Surface(string name, GCDConsoleLib.Raster raster)
         : base(name, raster)
        {
            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
        }

        public Surface(XmlNode nodSurface)
            : base(nodSurface)
        {
            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
            foreach (XmlNode nodError in nodSurface.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = new ErrorSurface(nodError, this);
                ErrorSurfaces.Add(error);
            }
        }

        public bool IsErrorNameUnique(string name, ErrorSurface ignore)
        {
            return ErrorSurfaces.Count<ErrorSurface>(x => x != ignore && string.Compare(name, x.Name, true) == 0) == 0;
        }

        new public void Delete()
        {
            try
            {
                foreach (ErrorSurface errSurf in ErrorSurfaces)
                {
                    errSurf.Delete();
                }
            }
            finally
            {
                ErrorSurfaces.Clear();
            }

            // Delete the raster
            try
            {
                base.Delete();
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error attempting to delete DEM Survey.", ex);
                ex2.Data["Name"] = Name;
                ex2.Data["File Path"] = Raster.GISFileInfo.FullName;
                throw ex2;
            }

            // Remove the DEM from the project
            ProjectManager.Project.ReferenceSurfaces.Remove(Name);

            // If no more inputs then delete the folder
            if (ProjectManager.Project.ReferenceSurfaces.Count < 1 && !Directory.EnumerateFileSystemEntries(Raster.GISFileInfo.Directory.Parent.FullName).Any())
            {
                try
                {
                    Raster.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write("Failed to delete empty reference surface directory " + Raster.GISFileInfo.Directory.Parent.FullName);
                }
            }

            ProjectManager.Project.Save();
        }

        public void DeleteErrorSurface(ErrorSurface err)
        {
            try
            {
                err.Delete();
            }
            finally
            {
                ErrorSurfaces.Remove(err);
            }
        }

        /// <summary>
        /// Serialize the surface
        /// </summary>
        /// <param name="nodItem">This is either the DEM or the ReferenceSurface XMLNode</param>
        /// <remarks>Note that because ReferenceSurface is inherited that this serialization
        /// works a little differently than other classes. The argument is the actual node
        /// into which the members of this class are serialized.</remarks>
        public void Serialize(XmlNode nodItem)
        {
            nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.GISFileInfo);

            if (ErrorSurfaces.Count > 0)
            {
                XmlNode nodError = nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("ErrorSurfaces"));
                foreach (ErrorSurface error in ErrorSurfaces)
                    error.Serialize(nodError);
            }
        }
    }
}
