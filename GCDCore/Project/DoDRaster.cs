using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class DoDRaster : GCDProjectRasterItem
    {
        public override string Noun { get { return "DoD Raster"; } }

        /// <summary>
        /// DoD Rasters are never considered in use.
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return false;
            }
        }

        public DoDRaster(string name, FileInfo rasterPath)
            : base(name, rasterPath)
        {

        }

        public DoDRaster(string name, GCDConsoleLib.Raster raster)
            : base(name, raster)
        {

        }
    }
}
