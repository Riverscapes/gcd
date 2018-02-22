using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using GCDCore.Project.ProfileRoutes;

namespace GCDCore.Project.LinearExtraction
{
    public class LinearExtractionFromSurface : LinearExtraction
    {
        public readonly Surface Surface;
        public readonly ErrorSurface ErrorSurface;

        public override GCDProjectItem GCDProjectItem { get { return Surface; } }

        public LinearExtractionFromSurface(string name, ProfileRoute route, FileInfo db, Surface surf, ErrorSurface err)
            : base(name, route, db)
        {
            Surface = surf;
            ErrorSurface = err;
        }

        public LinearExtractionFromSurface(XmlNode nodItem)
            : base(nodItem)
        {
            XmlNode nodDEM = nodItem.SelectSingleNode("DEM");
            if (nodDEM == null)
                Surface = ProjectManager.Project.ReferenceSurfaces[nodItem.SelectSingleNode("Surface").InnerText];
            else
                Surface = ProjectManager.Project.DEMSurveys[nodDEM.InnerText];

            if (nodItem.SelectSingleNode("ErrorSurface") is XmlNode)
                ErrorSurface = Surface.ErrorSurfaces.First(x => string.Compare(x.Name, nodItem.SelectSingleNode("ErrorSurface").InnerText, true) == 0);
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodItem = base.Serialize(nodParent);

            string nodName = this is LinearExtractionFromDEM ? "DEM" : "Surface";
            nodItem.AppendChild(nodParent.OwnerDocument.CreateElement(nodName)).InnerText = GCDProjectItem.Name;

            return nodItem;
        }
    }
}
