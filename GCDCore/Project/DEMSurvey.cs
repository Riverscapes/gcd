using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class DEMSurvey : GCDProjectRasterItem
    {
        public readonly naru.ui.SortableBindingList<AssocSurface> AssocSurfaces;
        public readonly naru.ui.SortableBindingList<ErrorSurface> ErrorSurfaces;

        public string SurveyMethod { get; set; } // Single survey methods
        public bool IsSingleSurveyMethod { get { return MethodMask == null; } }
        public SurveyDateTime SurveyDate { get; set; }
        public FileInfo MethodMask { get; set; } // Multi-method polygon ShapeFile
        public string MethodMaskField { get; set; } // Multi-method field in ShapeFile

        public ErrorSurface DefaultErrorSurface
        {
            get
            {
                if (ErrorSurfaces.Count(x => x.IsDefault) > 0)
                    return ErrorSurfaces.First(x => x.IsDefault);
                else
                    return null;
            }
        }

        public DEMSurvey(string name, SurveyDateTime surveyDate, FileInfo rasterPath)
            : base(name, rasterPath)
        {
            SurveyDate = surveyDate;

            AssocSurfaces = new naru.ui.SortableBindingList<AssocSurface>();
            ErrorSurfaces = new naru.ui.SortableBindingList<ErrorSurface>();
        }

        public bool IsErrorNameUnique(string name, ErrorSurface ignore)
        {
            return ErrorSurfaces.Count<ErrorSurface>(x => x != ignore && string.Compare(name, x.Name, true) == 0) == 0;
        }

        public bool IsAssocNameUnique(string name, AssocSurface ignore)
        {
            return AssocSurfaces.Count<AssocSurface>(x => x != ignore && string.Compare(name, x.Name, true) == 0) == 0;
        }

        new public void Delete()
        {
            try
            {
                foreach (AssocSurface assoc in AssocSurfaces)
                {
                    assoc.Delete();
                }
            }
            finally
            {
                AssocSurfaces.Clear();
            }

            try
            {
                foreach (ErrorSurface errSurf in ErrorSurfaces)
                {
                    errSurf.Delete();
                }
            }
            finally
            {
                ErrorSurfaces.Clear();
            }

            // Delete the vector mask if it exists
            if (MethodMask is FileInfo)
            {
                ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(MethodMask));
                Vector mask = new Vector(MethodMask);
                mask.Delete();
            }
        }

        public void DeleteAssociatedSurface(AssocSurface assoc)
        {
            try
            {
                assoc.Delete();
            }
            finally
            {
                AssocSurfaces.Remove(assoc);
            }
        }

        public void DeleteErrorSurface(ErrorSurface err)
        {
            try
            {
                err.Delete();
            }
            finally
            {
                ErrorSurfaces.Remove(err);
            }
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDEM = nodParent.AppendChild(xmlDoc.CreateElement("DEM"));
            nodDEM.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodDEM.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.GISFileInfo);

            if (!string.IsNullOrEmpty(SurveyMethod))
            {
                nodDEM.AppendChild(xmlDoc.CreateElement("SurveyMethod")).InnerText = SurveyMethod;
            }

            if (SurveyDate != null)
            {
                XmlNode nodSurveyDate = nodDEM.AppendChild(xmlDoc.CreateElement("SurveyDate"));
                naru.xml.XMLHelpers.AddNode(xmlDoc, nodSurveyDate, "Year", SurveyDate.Year > 0 ? SurveyDate.Year.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(xmlDoc, nodSurveyDate, "Month", SurveyDate.Month > 0 ? SurveyDate.Month.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(xmlDoc, nodSurveyDate, "Day", SurveyDate.Day > 0 ? SurveyDate.Day.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(xmlDoc, nodSurveyDate, "Hour", SurveyDate.Hour > -1 ? SurveyDate.Hour.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(xmlDoc, nodSurveyDate, "Minute", SurveyDate.Minute > -1 ? SurveyDate.Minute.ToString() : string.Empty);
            }

            if (MethodMask != null)
            {
                XmlNode nodMethodMask = nodDEM.AppendChild(xmlDoc.CreateElement("MethodMask"));
                nodMethodMask.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(MethodMask);
                nodMethodMask.AppendChild(xmlDoc.CreateElement("Field")).InnerText = MethodMaskField;
            }

            if (AssocSurfaces.Count > 0)
            {
                XmlNode nodAssoc = nodDEM.AppendChild(xmlDoc.CreateElement("AssociatedSurfaces"));
                foreach (AssocSurface assoc in AssocSurfaces)
                    assoc.Serialize(xmlDoc, nodAssoc);
            }

            if (ErrorSurfaces.Count > 0)
            {
                XmlNode nodError = nodDEM.AppendChild(xmlDoc.CreateElement("ErrorSurfaces"));
                foreach (ErrorSurface error in ErrorSurfaces)
                    error.Serialize(xmlDoc, nodError);
            }
        }

        public static DEMSurvey Deserialize(XmlNode nodDEM)
        {
            string name = nodDEM.SelectSingleNode("Name").InnerText;
            FileInfo path = ProjectManager.Project.GetAbsolutePath(nodDEM.SelectSingleNode("Path").InnerText);

            SurveyDateTime surveyDT = null;
            XmlNode nodSurveyDate = nodDEM.SelectSingleNode("SurveyDate");
            if (nodSurveyDate is XmlNode)
            {
                surveyDT = new SurveyDateTime();
                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText))
                    surveyDT.Year = ushort.Parse(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText))
                    surveyDT.Month = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText))
                    surveyDT.Day = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText))
                    surveyDT.Hour = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText))
                    surveyDT.Minute = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText);
            }

            DEMSurvey dem = new DEMSurvey(name, surveyDT, path);

            // Single survey method DEM surveys
            XmlNode nodSurveyMethod = nodDEM.SelectSingleNode("SurveyMethod");
            if (nodSurveyMethod is XmlNode)
                dem.SurveyMethod = nodSurveyMethod.InnerText;

            // Multi-method DEM surveys
            XmlNode nodMethodMask = nodDEM.SelectSingleNode("MethodMask");
            if (nodMethodMask is XmlNode)
            {
                dem.MethodMask = ProjectManager.Project.GetAbsolutePath(nodMethodMask.SelectSingleNode("Path").InnerText);
                dem.MethodMaskField = nodMethodMask.SelectSingleNode("Field").InnerText;
            }

            foreach (XmlNode nodAssoc in nodDEM.SelectNodes("AssociatedSurfaces/AssociatedSurface"))
            {
                AssocSurface assoc = AssocSurface.Deserialize(nodAssoc, dem);
                dem.AssocSurfaces.Add(assoc);
            }

            foreach (XmlNode nodError in nodDEM.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = ErrorSurface.Deserialize(nodError, dem);
                dem.ErrorSurfaces.Add(error);
            }

            return dem;
        }

    }
}
