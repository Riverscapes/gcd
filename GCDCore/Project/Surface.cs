using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
