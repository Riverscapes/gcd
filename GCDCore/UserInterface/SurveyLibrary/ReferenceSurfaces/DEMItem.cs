using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    public class DEMItem
    {
        public bool Include { get; set; }
        public string DEMName { get { return _DEM.Name; } internal set { } }

        public readonly DEMSurvey _DEM;

        /// <summary>
        /// Default constructor needed for binding
        /// </summary>
        public DEMItem()
        {

        }

        public DEMItem(DEMSurvey dem)
        {
            _DEM = dem;
            Include = true;
        }
    }
}
