using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System;
using GCDConsoleLib;

namespace GCDCore.Project
{
    public class DEMSurvey : Surface, IComparable<DEMSurvey>
    {
        public readonly naru.ui.SortableBindingList<AssocSurface> AssocSurfaces;
        public SurveyDateTime SurveyDate { get; set; }
        public int? ChronologicalOrder { get; set; } // Optional zero-based index of chronological order maintain by the Multi-Epoch GCD Analysis

        public override string Noun { get { return "DEM Survey"; } }

        public DirectoryInfo AssocSurfacesFolder { get { return ProjectManager.CombinePaths(Raster.GISFileInfo.Directory, "AssocSurfaces"); } }

        public FileInfo AssocSurfacePath(string name)
        {
            return ProjectManager.GetProjectItemPath(AssocSurfacesFolder, "Ass", name, "tif");
        }

        public DEMSurvey(string name, SurveyDateTime surveyDate, FileInfo rasterPath, FileInfo hillshadePath)
            : base(name, rasterPath, hillshadePath)
        {
            SurveyDate = surveyDate;

            AssocSurfaces = new naru.ui.SortableBindingList<AssocSurface>();
        }

        public DEMSurvey(XmlNode nodDEM)
            : base(nodDEM, false, false)
        {
            SurveyDateTime surveyDT = null;
            XmlNode nodSurveyDate = nodDEM.SelectSingleNode("SurveyDate");
            if (nodSurveyDate is XmlNode)
            {
                SurveyDate = new SurveyDateTime();
                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText))
                    SurveyDate.Year = ushort.Parse(nodDEM.SelectSingleNode("SurveyDate/Year").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText))
                    SurveyDate.Month = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Month").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText))
                    SurveyDate.Day = byte.Parse(nodDEM.SelectSingleNode("SurveyDate/Day").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText))
                    SurveyDate.Hour = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Hour").InnerText);

                if (!string.IsNullOrEmpty(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText))
                    SurveyDate.Minute = short.Parse(nodDEM.SelectSingleNode("SurveyDate/Minute").InnerText);
            }

            //read Chronological Order, if set
            XmlNode nodChronologicalOrder = nodDEM.SelectSingleNode("ChronologicalOrder");
            if (nodChronologicalOrder is XmlNode)
            {
                string sChronologicalOrder = nodChronologicalOrder.InnerText;
                int iChronologicalOrder;
                Boolean bParseSuccessful = int.TryParse(sChronologicalOrder, out iChronologicalOrder);
                if (bParseSuccessful)
                {
                    ChronologicalOrder = iChronologicalOrder;
                }
            }

            AssocSurfaces = new naru.ui.SortableBindingList<AssocSurface>();
            foreach (XmlNode nodAssoc in nodDEM.SelectNodes("AssociatedSurfaces/AssociatedSurface"))
            {
                AssocSurface assoc = new AssocSurface(nodAssoc, this);
                AssocSurfaces.Add(assoc);
            }

            // Load the error surfaces now that the associated surfaces have been loaded
            LoadErrorSurfaces(nodDEM);
            LoadLinearExtractions(nodDEM);
        }

        public bool IsAssocNameUnique(string name, AssocSurface ignore)
        {
            return AssocSurfaces.Count<AssocSurface>(x => x != ignore && string.Compare(name, x.Name, true) == 0) == 0;
        }


        /// <summary>
        /// Delete a DEM Survery
        /// </summary>
        /// <remarks>Note that the Surface base class is responsible for deleting child error surfaces</remarks>
        public override void Delete()
        {
            try
            {
                AssocSurfaces.ToList().ForEach(x => x.Delete());
            }
            finally
            {
                AssocSurfaces.Clear();
            }

            try
            {
                if (Hillshade != null)
                    Hillshade.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error attempting to delete hillshade at {0}\n\n{1}", this.Hillshade.Raster.GISFileInfo.FullName, ex.Message));
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


            // If no more inputs then delete the folder
            if (ProjectManager.Project.DEMSurveys.Count < 1 && !Directory.EnumerateFileSystemEntries(Raster.GISFileInfo.Directory.Parent.FullName).Any())
            {
                try
                {
                    Raster.GISFileInfo.Directory.Parent.Delete();
                }
                catch (Exception ex)
                {
                    Console.Write(string.Format("Failed to delete empty DEM Survey directory at {0}\n\n{1}", Raster.GISFileInfo.Directory.Parent.FullName, ex.Message));
                }
            }

            // Remove the DEM from the project
            ProjectManager.Project.DEMSurveys.Remove(Name);
            ProjectManager.Project.Save();
        }

        new public void Serialize(XmlNode nodDEM)
        {
            // Serialize the surface properties first.
            base.Serialize(nodDEM);

            if (SurveyDate != null)
            {
                XmlNode nodSurveyDate = nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("SurveyDate"));
                naru.xml.XMLHelpers.AddNode(nodSurveyDate, "Year", SurveyDate.Year > 0 ? SurveyDate.Year.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodSurveyDate, "Month", SurveyDate.Month > 0 ? SurveyDate.Month.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodSurveyDate, "Day", SurveyDate.Day > 0 ? SurveyDate.Day.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodSurveyDate, "Hour", SurveyDate.Hour > -1 ? SurveyDate.Hour.ToString() : string.Empty);
                naru.xml.XMLHelpers.AddNode(nodSurveyDate, "Minute", SurveyDate.Minute > -1 ? SurveyDate.Minute.ToString() : string.Empty);
            }

            if (ChronologicalOrder.HasValue)
            {
                nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("ChronologicalOrder")).InnerText = ChronologicalOrder.ToString();
            }

            if (AssocSurfaces.Count > 0)
            {
                XmlNode nodAssoc = nodDEM.AppendChild(nodDEM.OwnerDocument.CreateElement("AssociatedSurfaces"));
                foreach (AssocSurface assoc in AssocSurfaces)
                    assoc.Serialize(nodAssoc);
            }
        }

        public int CompareTo(DEMSurvey dem)
        {
            System.Diagnostics.Debug.WriteLine("Comparing '{0}' with {1} to '{2}' with {3}", Name, SurveyDate, dem.Name, dem.SurveyDate);

            if (SurveyDate == null || dem.SurveyDate == null)
            {
                return -1;
            }
            else
            {
                return SurveyDate.CompareTo(dem.SurveyDate);
            }
        }
    }
}
