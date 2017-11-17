using System.IO;

namespace GCDCore.Project
{
    public class AssocSurface : GCDProjectItem
    {
        public readonly ProjectRaster Raster;
        public string AssocSurfaceType { get; set; }
        public readonly DEMSurvey DEM;

        public AssocSurface(string name, FileInfo rasterPath, string sType, DEMSurvey dem)
            : base(name)
        {
            Raster = new ProjectRaster(rasterPath);
            AssocSurfaceType = sType;
            DEM = dem;
        }
    }
}
