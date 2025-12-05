using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class Raster : GISDataset
    {

        public Raster(GCDProject project, string name, FileSystemInfo fsInfo, string image_exists, string image_missing)
            : base(project, name, fsInfo, image_exists, image_missing)
        {

        }

        public Raster(GCDProject project, XmlNode nodItem, string image_exists, string image_missing)
             : base(project, nodItem, image_exists, image_missing)
        {

        }

        public override Uri GISUri => new Uri(this.GISPath);

    }
}
