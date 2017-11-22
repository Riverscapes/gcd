using System;
using System.Collections.Generic;
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
    public class ProjectManager
    {
        public static GCDProject Project { get; internal set; }
        public static DirectoryInfo ExcelTemplatesFolder { get; internal set; }
        public static DirectoryInfo ReportsFolder { get; internal set; }
        public static OutputManager OutputManager { get; internal set; }
        public static RasterPyramidManager PyramidManager { get; internal set; }
        private static FileInfo SurveyTypesPath { get; set; }
        private static FileInfo FISLibraryPath { get; set; }

        public static bool IsArcMap { get { return System.Reflection.Assembly.GetEntryAssembly().FullName.ToLower().Contains("arcmap"); } }

        public static Dictionary<string, SurveyType> SurveyTypes
        {
            get { return SurveyType.Load(SurveyTypesPath); }
            set { SurveyType.Save(SurveyTypesPath, value); }
        }

        public static void Init(string sResourcesFolder, string sAutomaticPyramids)
        {
            OutputManager = new OutputManager();
            PyramidManager = new RasterPyramidManager(sAutomaticPyramids);

            SurveyTypesPath = new FileInfo(Path.Combine(sResourcesFolder, "SurveyTypes.xml"));
            if (!SurveyTypesPath.Exists)
            {
                Exception ex = new Exception("The GCD Survey Types XML file does not exist.");
                ex.Data["Survey Types XML File"] = SurveyTypesPath.FullName;
                throw ex;
            }

            // The FIS library doesn't need to exist
            FISLibraryPath = new FileInfo(Path.Combine(sResourcesFolder, "FISLibrary.xml"));

            ExcelTemplatesFolder = new DirectoryInfo(System.IO.Path.Combine(sResourcesFolder, "ExcelTemplates"));
            if (!ExcelTemplatesFolder.Exists)
            {
                Exception ex = new Exception("The GCD Excel template folder path does not exist.");
                ex.Data["GCD Excel Template Path"] = ExcelTemplatesFolder.FullName;
                throw ex;
            }

            ReportsFolder = new DirectoryInfo(Path.Combine(sResourcesFolder, "ReportFiles"));
            if (!ReportsFolder.Exists)
            {
                Exception ex = new Exception("The GCD reports folder path does not exist.");
                ex.Data["GCD Reports Path"] = ReportsFolder.FullName;
                throw ex;
            }
        }

        public static List<ErrorCalculation.FIS.FISLibraryItem> FISLibrary
        {
            get
            {
                if (FISLibraryPath.Exists)
                {
                    return ErrorCalculation.FIS.FISLibraryItem.Load(FISLibraryPath);
                }
                else
                {
                    return new List<ErrorCalculation.FIS.FISLibraryItem>();
                }
            }
            set { ErrorCalculation.FIS.FISLibraryItem.Save(FISLibraryPath, value); }
        }

        public static void OpenProject(FileInfo projectFile)
        {
            Project = GCDProject.Load(projectFile);
        }

        public static void OpenProject(GCDProject project)
        {
            Project = project;
        }

        public static void CloseCurrentProject()
        {
            Project = null;
            GC.Collect();
        }
    }
}