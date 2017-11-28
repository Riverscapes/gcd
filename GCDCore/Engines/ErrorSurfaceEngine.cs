using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib;
using GCDConsoleLib.GCD;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public struct ErrorSurfaceEngine
    {
        public Raster Calculate(ErrorSurface errSurf)
        {
            return null;
        }
        //public ErrorSurface CalculateSingleMethod(string name, DEMSurvey DEM, ErrorSurfaceProperty errProps)
        //{
        //    FileInfo errSurfaceRasterPath = ProjectManagerBase.OutputManager.ErrorSurfaceRasterPath(DEM.Name, name, true);

        //    ErrorRasterProperties errRasProps = new ErrorRasterProperties()

        //    Raster errRaster = RasterOperators.CreateErrorRaster(DEM.Raster.Raster, errProps, errSurfaceRasterPath);

        //    ErrorSurfaceProperty errSurfProps = new ErrorSurfaceProperty("FullExtent", )
        //    Dictionary<string, ErrorSurfaceProperty> errSurfaceProps = new Dictionary<string, ErrorSurfaceProperty>();
        //    errProps["Full Extent"] = new ErrorSurfaceProperty("Full E")

        //    ErrorSurface e = new ErrorSurface(name, errSurfaceRasterPath, DEM, errProps);
        //    return e ;
        //}

        //public ErrorSurface CalculateMultiMethod(string name, DEMSurvey DEM, Dictionary<string, ErrorSurfaceProperty> errProps)
        //{

        //}
    }
}
