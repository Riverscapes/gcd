using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class ProjectRaster
    {
        public readonly System.IO.FileInfo RasterPath;
        private GCDConsoleLib.Raster _Raster;

        public GCDConsoleLib.Raster Raster
        {
            get
            {
                if (_Raster == null)
                    _Raster = new GCDConsoleLib.Raster(RasterPath);

                return _Raster;
            }
        }

        public ProjectRaster(System.IO.FileInfo rasterPath)
        {
            RasterPath = rasterPath;
            _Raster = null;
        }

        public ProjectRaster(GCDConsoleLib.Raster raster)
        {
            RasterPath = raster.GISFileInfo;
            _Raster = raster;
        }
    }
}
