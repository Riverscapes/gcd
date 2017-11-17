using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class ErrorSurface : GCDProjectItem
    {
        public readonly string ErrorSurfaceType;
        public readonly System.IO.FileInfo RasterPath;
        public readonly DEMSurvey DEM;

        public Dictionary<string, ErrorFISProperties> FISProperties { get; internal set; }
        public Dictionary<string, ErrorUniformProperties> UniformProperties { get; internal set; }

        public ErrorSurface(string name, string sType, System.IO.FileInfo rasterPath, DEMSurvey dem)
            : base(name)
        {
            ErrorSurfaceType = sType;
            RasterPath = rasterPath;
            DEM = dem;

            FISProperties = new Dictionary<string, ErrorFISProperties>();
            UniformProperties = new Dictionary<string, ErrorUniformProperties>();
        }
    }
}
