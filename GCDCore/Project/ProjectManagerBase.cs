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
    public class ProjectManagerBase
    {
        public GCDProject Project { get; internal set; }
        public readonly DirectoryInfo ExcelTemplatesFolder;
        public readonly DirectoryInfo ReportsFolder;
        public readonly OutputManager OutputManager;
        private readonly FileInfo SurveyTypesPath;

        public Dictionary<string, SurveyType> SurveyTypes
        {
            get { return SurveyType.Load(SurveyTypesPath); }
            set { SurveyType.Save(SurveyTypesPath, value); }
        }

        public ProjectManagerBase(string sResourcesFolder)
        {
            OutputManager = new OutputManager();
  
            SurveyTypesPath = new FileInfo(Path.Combine(sResourcesFolder, "SurveyTypes.xml"));
            if (!SurveyTypesPath.Exists)
            {
                Exception ex = new Exception("The GCD Survey Types XML file does not exist.");
                ex.Data["Survey Types XML File"] = SurveyTypesPath.FullName;
                throw ex;
            }

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

        public void OpenProject(FileInfo projectFile)
        {
            Project = GCDProject.Load(projectFile);
        }

        public void OpenProject(GCDProject project)
        {
            Project = project;
        }      

        public void CloseCurrentProject()
        {
            Project = null;
            GC.Collect();
        }
    }
}