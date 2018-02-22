using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project.LinearExtraction
{
    public class LinearExtractionFromDEM : LinearExtractionFromSurface
    {
        public LinearExtractionFromDEM(string name, ProfileRoutes.ProfileRoute route, FileInfo db, DEMSurvey dem, ErrorSurface err)
            : base(name, route, db, dem, err)
        {

        }

        public LinearExtractionFromDEM(XmlNode nodItem)
            : base(nodItem)
        {

        }
    }
}
