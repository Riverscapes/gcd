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

        public static bool IsArcMap { get { return System.Reflection.Assembly.GetEntryAssembly() == null; } }

        // Public event for ArcGIS to listen to when a GIS layer is about to be deleted
        // ArcGIS should search for this path in the map and then remove the layer
        public delegate void GISLayerDeletingEvent(GISLayerEventArgs e);
        public static event GISLayerDeletingEvent GISLayerDeletingEventHandler;

        public delegate void GISLayerBrowsing(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e);
        public static event GISLayerBrowsing GISLayerBrowsingEventHandler;

        public static Dictionary<string, SurveyType> SurveyTypes
        {
            get { return SurveyType.Load(SurveyTypesPath); }
            set { SurveyType.Save(SurveyTypesPath, value); }
        }

        private static string ApplicationFolder
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GCD"); }
        }

        public static void Init(string sAutomaticPyramids)
        {
            OutputManager = new OutputManager();
            PyramidManager = new RasterPyramidManager(sAutomaticPyramids);

            SurveyTypesPath = new FileInfo(Path.Combine(ApplicationFolder, "SurveyTypes.xml"));
            if (!SurveyTypesPath.Exists)
            {
                Exception ex = new Exception("The GCD Survey Types XML file does not exist.");
                ex.Data["Survey Types XML File"] = SurveyTypesPath.FullName;
                throw ex;
            }

            // The FIS library doesn't need to exist
            FISLibraryPath = new FileInfo(Path.Combine(ApplicationFolder, "FISLibrary.xml"));

            ExcelTemplatesFolder = new DirectoryInfo(System.IO.Path.Combine(ApplicationFolder, "ExcelTemplates"));
            if (!ExcelTemplatesFolder.Exists)
            {
                Exception ex = new Exception("The GCD Excel template folder path does not exist.");
                ex.Data["GCD Excel Template Path"] = ExcelTemplatesFolder.FullName;
                throw ex;
            }

            ReportsFolder = new DirectoryInfo(Path.Combine(ApplicationFolder, "ReportFiles"));
            if (!ReportsFolder.Exists)
            {
                Exception ex = new Exception("The GCD reports folder path does not exist.");
                ex.Data["GCD Reports Path"] = ReportsFolder.FullName;
                throw ex;
            }
        }

        public static void CopyDeployFolder()
        {
            string sDestinationFolder = ApplicationFolder;

            //New Code to test this may not be what you intend though
            string sFolderName = "Deploy";

            if (string.Compare(sFolderName, "Reources\\FIS", true) == 0)
            {
                sDestinationFolder = Path.Combine(sDestinationFolder, sFolderName);
                dynamic sExecutingAssemblyFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string sOriginalFolder = Path.Combine(sExecutingAssemblyFolder, sFolderName);

                if (Directory.Exists(Path.GetDirectoryName(sDestinationFolder)))
                {
                    CopyDirectory(sOriginalFolder, sDestinationFolder);
                }
                else if (!Directory.Exists(Path.GetDirectoryName(sDestinationFolder)))
                {
                    Directory.CreateDirectory(sDestinationFolder);
                    System.Diagnostics.Debug.WriteLine("Copying AddIn Folder \"" + sOriginalFolder + "\" to \"" + sDestinationFolder + "\"");
                    CopyDirectory(sOriginalFolder, sDestinationFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(sDestinationFolder);
                string sOriginalFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Deploy");
                //sDestinationFolder = Path.Combine(sDestinationFolder, Path.GetFileNameWithoutExtension(sFolderName))
                System.Diagnostics.Debug.WriteLine("Copying AddIn Folder \"" + sOriginalFolder + "\" to \"" + sDestinationFolder + "\"");
                CopyDirectory(sOriginalFolder, sDestinationFolder);
            }
        }

        private static void CopyDirectory(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                try
                {
                    DirectoryInfo dFolder = Directory.CreateDirectory(destPath);
                    if (!dFolder.Exists)
                    {
                        Exception ex = new Exception("Failed to create directory.");
                        ex.Data["Directory"] = destPath;
                        throw ex;
                    }
                }
                catch (DirectoryNotFoundException ex)
                {
                    Exception ex2 = new Exception("The specified path is invalid (for example, it is on an unmapped drive).", ex);
                    throw ex2;
                }
                catch (IOException ex)
                {
                    Exception ex2 = new Exception("The directory specified by path is a file or the network name is not known.", ex);
                    throw ex2;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Exception ex2 = new Exception("The caller does not have the required permission.", ex);
                    throw ex2;
                }
                catch (ArgumentNullException ex)
                {
                    Exception ex2 = new Exception("Path is Nothing.", ex);
                    throw ex2;
                }
                catch (ArgumentException ex)
                {
                    Exception ex2 = new Exception("The path is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the GetInvalidPathChars method or path is prefixed with, or contains, only a colon character (:).", ex);
                    throw ex2;
                }
                catch (PlatformNotSupportedException ex)
                {
                    Exception ex2 = new Exception("The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.", ex);
                    throw ex2;
                }
                catch (NotSupportedException ex)
                {
                    Exception ex2 = new Exception("path contains a colon character (:) that is not part of a drive label (\"C:\\\").", ex);
                    throw ex2;
                }
                catch (Exception ex)
                {
                    Exception ex2 = new Exception("Failed to create directory.");
                    ex2.Data["Directory"] = destPath;
                    throw ex2;
                }
            }

            foreach (string file1 in Directory.GetFiles(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file1));
                if (File.Exists(file1))
                {
                    if (string.Compare("FISLibraryXML.xml", Path.GetFileName(file1)) == 0)
                    {
                        if (!File.Exists(dest))
                        {
                            File.Copy(file1, dest, true);
                        }
                    }
                    else
                    {
                        File.Copy(file1, dest, true);
                        // Added True here to force the an overwrite 
                    }
                }
            }

            // Use directly the sourcePath passed in, not the parent of that path
            foreach (string dir1 in Directory.GetDirectories(sourcePath))
            {
                string destdir = Path.Combine(destPath, Path.GetFileName(dir1));
                CopyDirectory(dir1, destdir);
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
            set
            {
                ErrorCalculation.FIS.FISLibraryItem.Save(FISLibraryPath, value);
            }
        }

        public static void CreateProject(GCDProject project)
        {
            project.Save();
            Project = project;
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

        public static void OnGISLayerDelete(GISLayerEventArgs e)
        {
            if (GISLayerDeletingEventHandler != null)
                ProjectManager.GISLayerDeletingEventHandler(e);
        }

        public static void OnBrowseRaster(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e)
        {
            if (GISLayerBrowsingEventHandler != null)
                ProjectManager.GISLayerBrowsingEventHandler(txt, e);
        }

        public class GISLayerEventArgs : EventArgs
        {
            public readonly FileInfo RasterPath;

            public GISLayerEventArgs(FileInfo rasterPath)
            {
                RasterPath = rasterPath;
            }
        }
    }
}