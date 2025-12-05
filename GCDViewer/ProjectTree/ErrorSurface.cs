using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class ErrorSurface : Raster
    {
        public readonly Surface Surf;
        //public readonly Dictionary<string, ErrorSurfaceProperty> ErrorProperties;
        public readonly Masks.RegularMask Mask;

        private bool _IsDefault;

        public ErrorSurface(GCDProject project, XmlNode nodError, Surface surf)
       : base(project, nodError, "sigma.png", "signma.png")
        {
            _IsDefault = bool.Parse(nodError.SelectSingleNode("IsDefault").InnerText);
            Surf = surf;

            XmlNode nodMask = nodError.SelectSingleNode("Mask");
            if (nodMask is XmlNode)
            {
                // Must be a regular mask with the same name
                if (project.Masks.Any(x => x is Masks.RegularMask && string.Compare(x.Name, nodMask.InnerText, true) == 0))
                {
                    Mask = project.Masks.First(x => string.Compare(x.Name, nodMask.InnerText, true) == 0) as Masks.RegularMask;
                }
            }
        }
    }
}
