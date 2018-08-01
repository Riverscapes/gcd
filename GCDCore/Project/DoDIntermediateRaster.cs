using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class DoDIntermediateRaster : GCDProjectRasterItem
    {
        private readonly string _Noun;
        public override string Noun { get { return _Noun; } }

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

        public DoDIntermediateRaster(string noun, GCDConsoleLib.Raster raster)
            : base(noun, raster)
        {
            _Noun = noun;
        }
    }
}
