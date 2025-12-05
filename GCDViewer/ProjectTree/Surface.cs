using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GCDViewer.ProjectTree
{
    public class Surface : Raster
    {
        public readonly Raster Hillshade;
        public override string Noun { get { return "Reference Surface"; } }

        public readonly List<ErrorSurface> ErrorSurfaces;
        //public readonly List<LinearExtraction.LinearExtraction> LinearExtractions;

        public Surface(GCDProject project, XmlNode nodSurface, string image_exists, string image_missing)
         : base(project, nodSurface, image_exists, image_missing)
        {
            bool bLoadLinearExtractions = true;
            bool bLoadErrorSurfaces = true;


            XmlNode nodHillshade = nodSurface.SelectSingleNode("Hillshade");
            if (nodHillshade is XmlNode)
                Hillshade = new Raster(project, string.Format("{0} Hillshade", Noun), project.GetAbsolutePath(nodHillshade.InnerText), "", "");

            ErrorSurfaces = new List<ErrorSurface>();
            if (bLoadErrorSurfaces)
                LoadErrorSurfaces(project, nodSurface);

            //LinearExtractions = new List<LinearExtraction.LinearExtraction>();
            //if (bLoadLinearExtractions)
            //    LoadLinearExtractions(nodSurface);
        }

        protected void LoadErrorSurfaces(GCDProject project, XmlNode nodSurface)
        {
            foreach (XmlNode nodError in nodSurface.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = new ErrorSurface(project, nodError, this);
                ErrorSurfaces.Add(error);
            }
        }

        //protected void LoadLinearExtractions(XmlNode nodItem)
        //{
        //    foreach (XmlNode nodLE in nodItem.SelectNodes("LinearExtractions/LinearExtraction"))
        //    {
        //        LinearExtraction.LinearExtraction le;
        //        if (nodLE.SelectSingleNode("DEM") is XmlNode)
        //            le = new LinearExtraction.LinearExtractionFromDEM(nodLE, this as DEMSurvey);
        //        else
        //            le = new LinearExtraction.LinearExtractionFromSurface(nodLE, this);

        //        LinearExtractions.Add(le);
        //    }
        //}
    }
}
