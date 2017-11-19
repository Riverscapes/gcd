using System.IO;
using System.Collections.Generic;
using System.Xml;

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

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDEM = nodParent.AppendChild(xmlDoc.CreateElement("DEM"));
            nodDEM.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodDEM.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManagerBase.GetRelativePath(Raster.RasterPath);

            XmlNode nodSurveyDate = nodDEM.AppendChild(xmlDoc.CreateElement("SurveyDate"));
            nodSurveyDate.AppendChild(xmlDoc.CreateElement("Year")).InnerText = SurveyDate.Year > 0 ? SurveyDate.Year.ToString() : string.Empty;
            nodSurveyDate.AppendChild(xmlDoc.CreateElement("Month")).InnerText = SurveyDate.Month > 0 ? SurveyDate.Month.ToString() : string.Empty;
            nodSurveyDate.AppendChild(xmlDoc.CreateElement("Day")).InnerText = SurveyDate.Day > 0 ? SurveyDate.Day.ToString() : string.Empty;
            nodSurveyDate.AppendChild(xmlDoc.CreateElement("Hour")).InnerText = SurveyDate.Hour > -1 ? SurveyDate.Hour.ToString() : string.Empty;
            nodSurveyDate.AppendChild(xmlDoc.CreateElement("Minute")).InnerText = SurveyDate.Minute > -1 ? SurveyDate.Minute.ToString() : string.Empty;

            if (MethodMask != null)
            {
                XmlNode nodMethodMask = nodDEM.AppendChild(xmlDoc.CreateElement("MethodMask"));
                nodMethodMask.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManagerBase.GetRelativePath(MethodMask);
                nodMethodMask.AppendChild(xmlDoc.CreateElement("Field")).InnerText = MethodMaskField;
            }

            if (AssocSurfaces.Count > 0)
            {
                XmlNode nodAssoc = nodDEM.AppendChild(xmlDoc.CreateElement("AssociatedSurfaces"));
                foreach (AssocSurface assoc in AssocSurfaces.Values)
                    assoc.Serialize(xmlDoc, nodAssoc);
            }

            if (ErrorSurfaces.Count > 0)
            {
                XmlNode nodError = nodDEM.AppendChild(xmlDoc.CreateElement("ErrorSurfaces"));
                foreach (ErrorSurface error in ErrorSurfaces.Values)
                    error.Serialize(xmlDoc, nodError);
            }
        }

        public static DEMSurvey Deserialize(XmlNode nodDEM)
        {
            string name = nodDEM.SelectSingleNode("Name").InnerText;
            FileInfo path = ProjectManagerBase.GetAbsolutePath(nodDEM.SelectSingleNode("Path").InnerText);

            SurveyDateTime surveyDT = new SurveyDateTime();
            if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText))
                surveyDT.Year = ushort.Parse(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText);

            if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText))
                surveyDT.Month = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText);

            if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText))
                surveyDT.Day = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText);

            if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText))
                surveyDT.Hour = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText);

            if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText))
                surveyDT.Minute = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText);

            DEMSurvey dem = new DEMSurvey(name, surveyDT, path);

            XmlNode nodMethodMask = nodDEM.SelectSingleNode("MethodMask");
            if (nodMethodMask is XmlNode)
            {
                dem.MethodMask = ProjectManagerBase.GetAbsolutePath(nodMethodMask.SelectSingleNode("Path").InnerText);
                dem.MethodMaskField = nodMethodMask.SelectSingleNode("Field").InnerText;
            }

            foreach(XmlNode nodAssoc in nodDEM.SelectNodes("AssociatedSurfaces/AssociatedSurface"))
            {
                AssocSurface assoc = AssocSurface.Deserialize(nodAssoc, dem);
                dem.AssocSurfaces[assoc.Name] = assoc;
            }

            foreach(XmlNode nodError in nodDEM.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = ErrorSurface.Deserialize(nodError, dem);
                dem.ErrorSurfaces[error.Name] = error;
            }

            return dem;
        }
    }
}
