using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDViewer.ProjectTree
{
    class DoD : Raster
    {
        public DoD(GCDProject project, string name, FileSystemInfo path) : base(project, name, path, "GCD_16.png", "GCD_16.png")
        {
        }
    }
}
