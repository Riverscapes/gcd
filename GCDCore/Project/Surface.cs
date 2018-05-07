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
        public readonly HillShade Hillshade;

        public readonly naru.ui.SortableBindingList<ErrorSurface> ErrorSurfaces;
        public readonly List<LinearExtraction.LinearExtraction> LinearExtractions;

        public override string Noun { get { return "Reference Surface"; } }

        public DirectoryInfo ErrorSurfacesFolder { get { return ProjectManager.CombinePaths(Raster.GISFileInfo.Directory, "ErrorSurfaces"); } }

        public FileInfo ErrorSurfacePath(string name)
        {
            return ProjectManager.GetProjectItemPath(ErrorSurfacesFolder, "Err", name, ProjectManager.RasterExtension);
        }

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
                return ProjectManager.Project.DoDs.Any(x => x.NewSurface == this || x.OldSurface == this);
            }
        }

        public static FileInfo HillShadeRasterPath(FileInfo surfaceRasterPath)
        {
            string path2 = Path.Combine(surfaceRasterPath.DirectoryName, Path.GetFileNameWithoutExtension(surfaceRasterPath.FullName) + "_HS");
            path2 = Path.ChangeExtension(path2, surfaceRasterPath.Extension);
            return new System.IO.FileInfo(path2);
        }

        public static FileInfo ErrorSurfaceRasterPath(DirectoryInfo surfaceDir, string name)
        {
            DirectoryInfo parentDir = new DirectoryInfo(Path.Combine(surfaceDir.FullName, "ErrorSurfaces"));
            return ProjectManager.GetProjectItemPath(parentDir, "Err", name, ProjectManager.RasterExtension);
        }

        public Surface(string name, FileInfo rasterPath, FileInfo hillshadePath)
            : base(name, rasterPath)
        {
            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
            LinearExtractions = new List<LinearExtraction.LinearExtraction>();

            if (hillshadePath is FileInfo && hillshadePath.Exists)
            {
                Hillshade = new HillShade(string.Format("{0} Hillshade", Noun), hillshadePath);
            }
        }

        public Surface(string name, GCDConsoleLib.Raster raster, GCDConsoleLib.Raster rHillShade)
         : base(name, raster)
        {
            if (rHillShade != null)
                Hillshade = new HillShade(string.Format("{0} Hillshade", Noun), rHillShade);

            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
            LinearExtractions = new List<LinearExtraction.LinearExtraction>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodSurface"></param>
        /// <param name="bLoadErrorSurfaces">See remarks</param>
        /// <remarks>
        /// DEM Survey error surfaces can be defined by assoc surface or FIS that uses
        /// assoc surface. But When this base constructor for Surfaces get's called the associated
        /// surface dictionary has not been loaded yet. DEM Surveys therefore override the following call
        /// </remarks>
        public Surface(XmlNode nodSurface, bool bLoadErrorSurfaces, bool bLoadLinearExtractions)
            : base(nodSurface)
        {
            XmlNode nodHillshade = nodSurface.SelectSingleNode("Hillshade");
            if (nodHillshade is XmlNode)
                Hillshade = new HillShade(string.Format("{0} Hillshade", Noun), ProjectManager.Project.GetAbsolutePath(nodHillshade.InnerText));

            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
            if (bLoadErrorSurfaces)
                LoadErrorSurfaces(nodSurface);

            LinearExtractions = new List<LinearExtraction.LinearExtraction>();
            if (bLoadLinearExtractions)
                LoadLinearExtractions(nodSurface);
        }

        protected void LoadErrorSurfaces(XmlNode nodSurface)
        {
            foreach (XmlNode nodError in nodSurface.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = new ErrorSurface(nodError, this);
                ErrorSurfaces.Add(error);
            }
        }

        protected void LoadLinearExtractions(XmlNode nodItem)
        {
            foreach (XmlNode nodLE in nodItem.SelectNodes("LinearExtractions/LinearExtraction"))
            {
                LinearExtraction.LinearExtraction le;
                if (nodLE.SelectSingleNode("DEM") is XmlNode)
                    le = new LinearExtraction.LinearExtractionFromDEM(nodLE, this as DEMSurvey);
                else
                    le = new LinearExtraction.LinearExtractionFromSurface(nodLE, this);

                LinearExtractions.Add(le);
            }
        }

        public bool IsErrorNameUnique(string name, ErrorSurface ignore)
        {
            return ErrorSurfaces.Count<ErrorSurface>(x => x != ignore && string.Compare(name, x.Name, true) == 0) == 0;
        }

        public override void Delete()
        {
            try
            {
                // This is the safest way to delete from a list while iterating through it.
                for (int i = ErrorSurfaces.Count - 1; i >= 0; i--)
                    ErrorSurfaces[i].Delete();
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

            if (!(this is DEMSurvey))
            {
                // Remove the DEM from the project
                ProjectManager.Project.ReferenceSurfaces.Remove(this);
                // If no more inputs then delete the folder
                if (ProjectManager.Project.ReferenceSurfaces.Count < 1 && !Directory.EnumerateFileSystemEntries(Raster.GISFileInfo.Directory.Parent.FullName).Any())
                {
                    try
                    {
                        Raster.GISFileInfo.Directory.Parent.Delete();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(string.Format("Failed to delete empty reference surface directory {0}\n\n{1}", Raster.GISFileInfo.Directory.Parent.FullName, ex.Message));
                    }
                }
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

            if (Hillshade != null)
                nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("Hillshade")).InnerText = ProjectManager.Project.GetRelativePath(Hillshade.Raster.GISFileInfo);

            if (ErrorSurfaces.Count > 0)
            {
                XmlNode nodError = nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("ErrorSurfaces"));
                foreach (ErrorSurface error in ErrorSurfaces)
                    error.Serialize(nodError);
            }

            if (LinearExtractions.Count > 0)
            {
                XmlNode nodLE = nodItem.AppendChild(nodItem.OwnerDocument.CreateElement("LinearExtractions"));
                LinearExtractions.ForEach(x => x.Serialize(nodLE));
            }
        }
    }
}
