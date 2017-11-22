using System.IO;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class ProjectRaster
    {
        public readonly FileInfo RasterPath;
        private Raster _Raster;

        public Raster Raster
        {
            get
            {
                if (_Raster == null)
                    _Raster = new Raster(RasterPath);

                return _Raster;
            }
        }

        public string RelativePath
        {
            get
            {
                return ProjectManager.Project.GetRelativePath(RasterPath);
            }
        }

        public ProjectRaster(FileInfo rasterPath)
        {
            RasterPath = rasterPath;
            _Raster = null;
        }

        public ProjectRaster(Raster raster)
        {
            RasterPath = raster.GISFileInfo;
            _Raster = raster;
        }
    }
}
