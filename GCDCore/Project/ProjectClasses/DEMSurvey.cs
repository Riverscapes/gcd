using System.IO;
using System.Collections.Generic;

namespace GCDCore.Project
{
    public class DEMSurvey : GCDProjectItem
    {
        public SurveyDateTime SurveyDate { get; set; }
        public readonly ProjectRaster Raster;

        public FileInfo MethodMask { get; set; }
        public string MethodMaskField { get; set; }

        public Dictionary<string, AssocSurface> AssocSurfaces { get; internal set; }
        public Dictionary<string, ErrorSurface> ErrorSurfaces { get; internal set; }

        public DEMSurvey(string name, SurveyDateTime surveyDate, FileInfo rasterPath)
            : base(name)
        {
            SurveyDate = surveyDate;
            Raster = new ProjectRaster(rasterPath);

            AssocSurfaces = new Dictionary<string, AssocSurface>();
            ErrorSurfaces = new Dictionary<string, ErrorSurface>();
        }
    }
}
