using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDCore.Project
{
    public class DEMSurvey : GCDProjectItem
    {
        public SurveyDateTime SurveyDate { get; set; }
        public readonly System.IO.FileInfo RasterPath;

        public System.IO.FileInfo MethodMask { get; set; }
        public string MethodMaskField { get; set; }

        public Dictionary<string, AssocSurface> AssocSurfaces { get; internal set; }
        public Dictionary<string, ErrorSurface> ErrorSurfaces { get; internal set; }

        public DEMSurvey(string name, SurveyDateTime surveyDate, System.IO.FileInfo rasterPath)
            : base(name)
        {
            SurveyDate = surveyDate;
            RasterPath = rasterPath;

            AssocSurfaces = new Dictionary<string, AssocSurface>();
            ErrorSurfaces = new Dictionary<string, ErrorSurface>();
        }
    }
}
