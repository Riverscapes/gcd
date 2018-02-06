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

        public Surface(string name, System.IO.FileInfo rasterPath)
            : base(name, rasterPath)
        {
            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
        }

        public bool IsErrorNameUnique(string name, ErrorSurface ignore)
        {
            return ErrorSurfaces.Count<ErrorSurface>(x => x != ignore && string.Compare(name, x.Name, true) == 0) == 0;
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
                    error.Serialize(nodItem.OwnerDocument, nodError);
            }
        }

        public static Surface Deserialize(XmlNode nodItem)
        {
            string name = nodItem.SelectSingleNode("Name").InnerText;
            FileInfo path = ProjectManager.Project.GetAbsolutePath(nodItem.SelectSingleNode("Path").InnerText);

            Surface surf = new Surface(name, path);

            foreach (XmlNode nodError in nodItem.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = ErrorSurface.Deserialize(nodError, surf);
                surf.ErrorSurfaces.Add(error);
            }

            return surf;
        }
    }
}
