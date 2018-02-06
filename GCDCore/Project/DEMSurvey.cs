using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class DEMSurvey : Surface
    {
        public readonly GCDProjectRasterItem Hillshade;
        public readonly naru.ui.SortableBindingList<AssocSurface> AssocSurfaces;

        public string SurveyMethod { get; set; } // Single survey methods
        public bool IsSingleSurveyMethod { get { return MethodMask == null; } }
        public SurveyDateTime SurveyDate { get; set; }
        public FileInfo MethodMask { get; set; } // Multi-method polygon ShapeFile
        public string MethodMaskField { get; set; } // Multi-method field in ShapeFile
        public int? ChronologicalOrder { get; set; } // Optional zero-based index of chronological order maintain by the Multi-Epoch GCD Analysis

        public DEMSurvey(string name, SurveyDateTime surveyDate, FileInfo rasterPath)
            : base(name, rasterPath)
        {
            SurveyDate = surveyDate;

            AssocSurfaces = new naru.ui.SortableBindingList<AssocSurface>();

            FileInfo hsPath = Project.ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(name);
            if (hsPath.Exists)
            {
                Hillshade = new GCDProjectRasterItem("DEM Hillshade", hsPath);
            }
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

            try
            {
                if (Hillshade != null)
                    Hillshade.Delete();
            }
            catch (Exception ex)
            {
                // Do nothing try and continue with the delete.
            }

            // Delete the DEM raster
            try
            {
                base.Delete();
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error attempting to delete DEM Survey.", ex);
                ex2.Data["Name"] = Name;
                ex2.Data["File Path"] = Raster.GISFileInfo.FullName;
                throw ex2;
            }

            // Remove the DEM from the project
            ProjectManager.Project.DEMSurveys.Remove(Name);

            // If no more inputs then delete the folder
            if (ProjectManager.Project.DEMSurveys.Count < 1 && !Directory.EnumerateFileSystemEntries(Raster.GISFileInfo.Directory.Parent.FullName).Any())
            {
                try
                {
                    Raster.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write("Failed to delete empty DEM Survey directory " + Raster.GISFileInfo.Directory.Parent.FullName);
                }
            }

            ProjectManager.Project.Save();
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

            if (AssocSurfaces.Count < 1)
            {
                try
                {
                    assoc.Raster.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to delete associate surface directory" + assoc.Raster.GISFileInfo.Directory.Parent);
                }
            }
        }

        new public void Serialize(XmlNode nodDEM)
        {
            // Serialize the surface properties first.
            base.Serialize(nodDEM);

            // Now serialize the DEM-specific member properties
            if (!string.IsNullOrEmpty(SurveyMethod))
            {
                nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("SurveyMethod")).InnerText = SurveyMethod;
            }

            if (SurveyDate != null)
            {
                XmlNode nodSurveyDate = nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("SurveyDate"));
                naru.xml.XMLHelpers.AddNode(nodDEM.OwnerDocument, nodSurveyDate, "Year", SurveyDate.Year > 0 ? SurveyDate.Year.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodDEM.OwnerDocument, nodSurveyDate, "Month", SurveyDate.Month > 0 ? SurveyDate.Month.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodDEM.OwnerDocument, nodSurveyDate, "Day", SurveyDate.Day > 0 ? SurveyDate.Day.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodDEM.OwnerDocument, nodSurveyDate, "Hour", SurveyDate.Hour > -1 ? SurveyDate.Hour.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodDEM.OwnerDocument, nodSurveyDate, "Minute", SurveyDate.Minute > -1 ? SurveyDate.Minute.ToString() : string.Empty);
            }

            if (ChronologicalOrder.HasValue)
            {
                nodDEM.AppendChild(xmlDoc.CreateElement("ChronologicalOrder")).InnerText = ChronologicalOrder.ToString();
            }


            if (MethodMask != null)
            {
                XmlNode nodMethodMask = nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("MethodMask"));
                nodMethodMask.AppendChild(nodDEM.OwnerDocument.CreateElement("Path")).InnerText = ProjectManager.Project.GetRelativePath(MethodMask);
                nodMethodMask.AppendChild(nodDEM.OwnerDocument.CreateElement("Field")).InnerText = MethodMaskField;
            }

            if (AssocSurfaces.Count > 0)
            {
                XmlNode nodAssoc = nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("AssociatedSurfaces"));
                foreach (AssocSurface assoc in AssocSurfaces)
                    assoc.Serialize(nodDEM.OwnerDocument, nodAssoc);
            }
        }

        new public static DEMSurvey Deserialize(XmlNode nodDEM)
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

            //read Chronological Order, if set
            XmlNode nodChronologicalOrder = nodDEM.SelectSingleNode("ChronologicalOrder");
            if (nodChronologicalOrder is XmlNode)
            {
                string sChronologicalOrder = nodChronologicalOrder.InnerText;
                int iChronologicalOrder;
                Boolean bParseSuccessful = int.TryParse(sChronologicalOrder, out iChronologicalOrder);
                if (bParseSuccessful)
                {
                    dem.ChronologicalOrder = iChronologicalOrder;
                }
            }


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
