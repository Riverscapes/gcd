using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection.Intercomparison
{
    public class DEMSurveyItem
    {
        public bool IsActive { get; set; }
        public readonly DEMSurvey DEM;
        public readonly ErrorSurface ErrorSurf;

        public string DEMName { get { return DEM.Name; } }
        public string ErrorName { get { return ErrorSurf.Name; } }

        public DEMSurveyItem(DEMSurvey dem, ErrorSurface err)
        {
            DEM = dem;
            ErrorSurf = err;
            IsActive = false;
        }
    }
}
