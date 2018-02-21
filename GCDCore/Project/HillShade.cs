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
        public override string Noun { get { return "DEM Hillshade"; } }

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
