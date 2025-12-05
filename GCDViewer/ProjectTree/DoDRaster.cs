using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDViewer.ProjectTree
{
    public class DoDRaster : Raster
    {
        public override string Noun { get { return "DoD Raster"; } }

        public DoDRaster(GCDProject project, string name, FileInfo rasterPath)
            : base(project, name, rasterPath, "GCD_16.png", "GCD_16.png")
        {

        }
    }
}
