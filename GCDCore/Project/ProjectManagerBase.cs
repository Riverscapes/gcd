using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace GCDCore.Project
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Base class for the GCD project manager. This should only contain
    /// members needed by both console and UI applications. The GCD still uses the inherited
    /// GCDProjectManager class that has more members that are specific to
    /// the desktop software.</remarks>
    public class ProjectManagerBase
    {
        private static ProjectDS m_ProjectDS;

        private static string m_GCDProjectXMLFilePath;

        public static OutputManager OutputManager { get; internal set; }

        // This is the folder that contains the Excel Templates
        private const string EXCELTEMPLATESFOLDERNAME = "ExcelTemplates";

        protected static DirectoryInfo m_dExcelTemplatesFolder;
        // Colour for displaying erosion and deposition in charts
        private static System.Drawing.Color ColourErosion { get; set; }
        private static System.Drawing.Color ColourDeposition { get; set; }
        private const string ReportsFolder = "ReportFiles";

        private static DirectoryInfo m_dResourcesFolder;

        protected static GCDConsoleLib.Raster.RasterDriver m_eDefaultRasterType;

        private static FileInfo m_fiSurveyTypes;

        public static GCDConsoleLib.GCD.UnitGroup Units { get; internal set; }
        public static UnitsNet.Area CellArea { get; internal set; }
        
        #region "Properties"

        public static string RasterExtension
        {
            get
            {
                switch (m_eDefaultRasterType)
                {
                    case GCDConsoleLib.Raster.RasterDriver.GTiff:
                        return "tif";
                    case GCDConsoleLib.Raster.RasterDriver.HFA:
                        return "img";
                    default:
                        throw new NotImplementedException(string.Format("Unhandled raster driver type {0}", m_eDefaultRasterType));
                }
            }
        }

        public static ProjectDS.ProjectRow CurrentProject
        {
            get
            {
                ProjectDS.ProjectRow rResult = null;
                if (ds is ProjectDS)
                {
                    if (ds.Project is ProjectDS.ProjectDataTable)
                    {
                        if (ds.Project.Rows.Count > 0)
                        {
                            rResult = ds.Project.First<ProjectDS.ProjectRow>();
                        }
                    }
                }

                return rResult;
            }
        }

        public static string FilePath
        {
            get { return m_GCDProjectXMLFilePath; }

            set
            {
                // Store the new GCD project file path
                m_GCDProjectXMLFilePath = value;

                // Create a new project dataset
                if (m_ProjectDS == null)
                {
                    m_ProjectDS = new ProjectDS();
                }
                m_ProjectDS.Clear();

                // Attempt to read the GCD project
                if (File.Exists(m_GCDProjectXMLFilePath))
                {
                    try
                    {
                        m_ProjectDS.ReadXml(m_GCDProjectXMLFilePath);
                    }
                    catch (Exception ex)
                    {
                        m_GCDProjectXMLFilePath = string.Empty;
                        m_ProjectDS = null;
                        Exception ex2 = new Exception("Error reading GCD project file.", ex);
                        ex2.Data["GCD Project File"] = value;
                        throw ex2;
                    }
                }
            }
        }

        public static DirectoryInfo ReportResourcesFolder
        {
            get
            {
                DirectoryInfo dResourcesFolder = new DirectoryInfo(Path.Combine(m_dResourcesFolder.FullName, ReportsFolder));
                return dResourcesFolder;
            }
        }

        public static ProjectDS ds
        {
            get
            {
                if (m_ProjectDS == null)
                {
                    m_ProjectDS = new ProjectDS();
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        m_ProjectDS.ReadXml(FilePath);
                    }
                }
                return m_ProjectDS;
            }
        }

        public static GCDConsoleLib.Raster.RasterDriver DefaultRasterType
        {
            get { return m_eDefaultRasterType; }
        }

        public static DirectoryInfo ExcelTemplatesFolder
        {
            get { return m_dExcelTemplatesFolder; }
        }

        public static UnitsNet.Units.LengthUnit DisplayUnits
        {
            get
            {
                if (CurrentProject.DisplayUnits != null)
                {
                    return (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), "Meter");
                }
                return UnitsNet.Units.LengthUnit.Undefined;
            }
        }

        public static Dictionary<string, SurveyType> SurveyTypes
        {
            get { return SurveyType.Load(m_fiSurveyTypes); }
            set { SurveyType.Save(m_fiSurveyTypes, value); }
        }

        #endregion

        public ProjectManagerBase(string sResourcesFolder, System.Drawing.Color colErosion, System.Drawing.Color colDeposition, GCDConsoleLib.Raster.RasterDriver eDefaultRasterType)
        {
            OutputManager = new OutputManager();
            ColourErosion = colErosion;
            ColourDeposition = colDeposition;
            m_eDefaultRasterType = eDefaultRasterType;

            m_fiSurveyTypes = new FileInfo(Path.Combine(sResourcesFolder, "SurveyTypes.xml"));
            if (!m_fiSurveyTypes.Exists)
            {
                Exception ex = new Exception("The GCD Survey Types XML file does not exist.");
                ex.Data["Survey Types XML File"] = m_fiSurveyTypes.FullName;
                throw ex;
            }

            m_dExcelTemplatesFolder = new DirectoryInfo(System.IO.Path.Combine(sResourcesFolder, EXCELTEMPLATESFOLDERNAME));
            if (!m_dExcelTemplatesFolder.Exists)
            {
                Exception ex = new Exception("The GCD Excel template folder path does not exist.");
                ex.Data["GCD Excel Template Path"] = m_dExcelTemplatesFolder.FullName;
                throw ex;
            }

        }

        public static string GetRelativePath(FileInfo FullPath)
        {
            return GetRelativePath(FullPath.FullName);
        }

        public static string GetRelativePath(string sFullPath)
        {
            if (string.IsNullOrEmpty(m_GCDProjectXMLFilePath))
            {
                throw new Exception("The project XML file path must be provided.");
            }

            string sProjectFolder = Path.GetDirectoryName(m_GCDProjectXMLFilePath);
            int nIndex = sFullPath.ToLower().IndexOf(sProjectFolder.ToLower());

            if (nIndex >= 0)
            {
                sFullPath = sFullPath.Substring(sProjectFolder.Length, sFullPath.Length - sProjectFolder.Length);
                sFullPath = sFullPath.TrimStart(Path.DirectorySeparatorChar);
            }

            return sFullPath;
        }

        public static FileInfo GetAbsolutePath(string sRelativePath)
        {

            if (string.IsNullOrEmpty(m_GCDProjectXMLFilePath))
            {
                throw new Exception("The project XML file path must be provided.");
            }

            string sProjectFolder = Path.GetDirectoryName(m_GCDProjectXMLFilePath);
            string sResult = Path.Combine(sProjectFolder, sRelativePath);
            return new FileInfo(sResult);
        }

        public static void CloseCurrentProject()
        {
            m_ProjectDS = null;
            m_GCDProjectXMLFilePath = string.Empty;
            GC.Collect();
        }

        public static void save()
        {
            m_ProjectDS.WriteXml(FilePath);
        }
    }
}