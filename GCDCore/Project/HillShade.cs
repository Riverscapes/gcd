using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class HillShade : GCDProjectRasterItem
    {

        /// <summary>
        /// Hillshades are never considered in use
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return false;
            }
        }

        public HillShade(FileInfo rasterPath)
            : base("DEM Hillshade", rasterPath)
        {

        }        
    }
}
