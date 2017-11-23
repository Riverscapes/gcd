using System.IO;
using System.Collections.Generic;
using System.Xml;
using System;

namespace GCDCore.Project
{
    public class DEMSurvey : GCDProjectItem
    {
        public readonly ProjectRaster Raster;
        public readonly Dictionary<string, AssocSurface> AssocSurfaces;
        public readonly Dictionary<string, ErrorSurface> ErrorSurfaces;

        public string SurveyMethod { get; set; } // Single survey methods
        public bool IsSingleSurveyMethod { get { return string.IsNullOrEmpty(SurveyMethod); } }
        public SurveyDateTime SurveyDate { get; set; }
        public FileInfo MethodMask { get; set; } // Multi-method polygon ShapeFile
        public string MethodMaskField { get; set; } // Multi-method field in ShapeFile


        public DEMSurvey(string name, SurveyDateTime surveyDate, FileInfo rasterPath)
            : base(name)
        {
            SurveyDate = surveyDate;
            Raster = new ProjectRaster(rasterPath);

            AssocSurfaces = new Dictionary<string, AssocSurface>();
            ErrorSurfaces = new Dictionary<string, ErrorSurface>();
        }

        public bool IsErrorNameUnique(string name, ErrorSurface ignore)
        {
            bool bUnique = true;
            if (ErrorSurfaces.ContainsKey(name))
            {
                bUnique = ErrorSurfaces[name] != ignore;
            }
            return bUnique;
        }

        public bool IsAssocNameUnique(string name, AssocSurface ignore)
        {
            bool bUnique = true;
            if (AssocSurfaces.ContainsKey(name))
            {
                bUnique = AssocSurfaces[name] != ignore;
            }
            return bUnique;
        }

        public void DeleteAssociatedSurface(AssocSurface assoc)
        {
            throw new NotImplementedException("delete associated surface not implemented");

            //Dim sPath As IO.FileInfo = ProjectManager.GetAbsolutePath(rAssoc.Source)
            //    Dim bContinue As Boolean = True

            //    Try
            //        'TODO
            //        Throw New Exception("Not implemented")
            //        ' RemoveAuxillaryLayersFromMap(sPath)
            //    Catch ex As Exception
            //        MsgBox("Error removing the associated surface from the ArcMap table of contents. Remove it manually And then try deleting the associated surface again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            //        bContinue = False
            //    End Try

            //    If bContinue Then
            //        If ProjectManager.GetAbsolutePath(rAssoc.Source).Exists Then
            //            Try
            //                GCDConsoleLib.Raster.Delete(sPath)
            //            Catch ex As Exception
            //                Dim ex2 As New Exception("Error deleting the associated surface raster file And directory.", ex)
            //                ex2.Data.Add("Raster Path", sPath)
            //                naru.error.ExceptionUI.HandleException(ex2)
            //                bContinue = False
            //            End Try
            //        End If
            //    End If

            //    Try
            //        GCDConsoleLib.Raster.Delete(ProjectManager.GetAbsolutePath(rAssoc.Source))
            //    Catch ex As Exception
            //        ' do nothing
            //    End Try

            //    If bContinue Then
            //        Try
            //            rAssoc.Delete()
            //            ProjectManager.save()
            //        Catch ex As Exception
            //            naru.error.ExceptionUI.HandleException(ex, "The raster file was deleted, but an error occurred removing the surface from the GCD project file. This will be fixed automatically by closing And opening ArcMap if Validate Project Is selected from the options menu")
            //        End Try
            //    End If
        }

        public void DeleteErrorSurface(ErrorSurface err)
        {
            throw new NotImplementedException("todo");

            //If Not TypeOf rError Is ProjectDS.ErrorSurfaceRow Then
            //    Exit Sub
            //End If

            //If response = MsgBoxResult.Yes Then
            //    Dim sPath As IO.FileInfo = ProjectManager.GetAbsolutePath(rError.Source.ToString)
            //    Dim bContinue As Boolean = True

            //    Try
            //        'TODO
            //        Throw New Exception("Not implemented")
            //        'RemoveAuxillaryLayersFromMap(sPath)
            //    Catch ex As Exception
            //        MsgBox("Error removing the error surface from the ArcMap table of contents. Remove it manually And then try deleting the error surface again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            //        bContinue = False
            //    End Try

            //    If bContinue Then
            //        If Not rError.IsSourceNull Then
            //            If ProjectManager.GetAbsolutePath(rError.Source).Exists Then
            //                Try
            //                    GCDConsoleLib.Raster.Delete(sPath)
            //                Catch ex As Exception
            //                    naru.error.ExceptionUI.HandleException(ex, "Failed to delete the error surface raster.")
            //                    bContinue = False
            //                End Try
            //            End If
            //        End If
            //    End If

            //    If bContinue Then
            //        Try
            //            ProjectManager.GetAbsolutePath(rError.Source).Directory.Delete()
            //        Catch ex As Exception
            //            ' do nothing
            //        End Try
            //    End If

            //    If bContinue Then
            //        Try
            //            rError.Delete()
            //            ProjectManager.save()
            //        Catch ex As Exception
            //            naru.error.ExceptionUI.HandleException(ex, "The raster file was deleted, but an error occurred removing the surface from the GCD project file. This will be fixed automatically by closing and opening ArcMap if Validate Project is selected from the options menu")
            //        End Try
            //    End If

            //End If

        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDEM = nodParent.AppendChild(xmlDoc.CreateElement("DEM"));
            nodDEM.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodDEM.AppendChild(xmlDoc.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(Raster.RasterPath);

            XmlNode nodSurveyMethod = nodDEM.AppendChild(xmlDoc.CreateElement("SurveyMethod"));
            if (!string.IsNullOrEmpty(SurveyMethod))
                nodSurveyMethod.InnerText = SurveyMethod;

            if (SurveyDate != null)
            {
                XmlNode nodSurveyDate = nodDEM.AppendChild(xmlDoc.CreateElement("SurveyDate"));
                nodSurveyDate.AppendChild(xmlDoc.CreateElement("Year")).InnerText = SurveyDate.Year > 0 ? SurveyDate.Year.ToString() : string.Empty;
                nodSurveyDate.AppendChild(xmlDoc.CreateElement("Month")).InnerText = SurveyDate.Month > 0 ? SurveyDate.Month.ToString() : string.Empty;
                nodSurveyDate.AppendChild(xmlDoc.CreateElement("Day")).InnerText = SurveyDate.Day > 0 ? SurveyDate.Day.ToString() : string.Empty;
                nodSurveyDate.AppendChild(xmlDoc.CreateElement("Hour")).InnerText = SurveyDate.Hour > -1 ? SurveyDate.Hour.ToString() : string.Empty;
                nodSurveyDate.AppendChild(xmlDoc.CreateElement("Minute")).InnerText = SurveyDate.Minute > -1 ? SurveyDate.Minute.ToString() : string.Empty;
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

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText))
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
                dem.AssocSurfaces[assoc.Name] = assoc;
            }

            foreach (XmlNode nodError in nodDEM.SelectNodes("ErrorSurfaces/ErrorSurface"))
            {
                ErrorSurface error = ErrorSurface.Deserialize(nodError, dem);
                dem.ErrorSurfaces[error.Name] = error;
            }

            return dem;
        }
    }
}
