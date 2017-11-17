using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class AssocSurface : GCDProjectItem
    {
        public readonly System.IO.FileInfo RasterPath;
        public string AssocSurfaceType { get; set; }
        public readonly DEMSurvey DEM;

        public AssocSurface(string name, System.IO.FileInfo rasterPath, string sType, DEMSurvey dem)
            : base(name)
        {
            RasterPath = rasterPath;
            AssocSurfaceType = sType;
            DEM = dem;
        }
    }
}
