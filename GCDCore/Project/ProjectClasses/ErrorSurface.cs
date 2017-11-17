using System.IO;
using System.Collections.Generic;

namespace GCDCore.Project
{
    public class ErrorSurface : GCDProjectItem
    {
        public readonly string ErrorSurfaceType;
        public readonly ProjectRaster Raster;
        public readonly DEMSurvey DEM;

        public Dictionary<string, ErrorFISProperties> FISProperties { get; internal set; }
        public Dictionary<string, ErrorUniformProperties> UniformProperties { get; internal set; }

        public ErrorSurface(string name, string sType, FileInfo rasterPath, DEMSurvey dem)
            : base(name)
        {
            ErrorSurfaceType = sType;
            Raster = new ProjectRaster(rasterPath);
            DEM = dem;

            FISProperties = new Dictionary<string, ErrorFISProperties>();
            UniformProperties = new Dictionary<string, ErrorUniformProperties>();
        }
    }
}