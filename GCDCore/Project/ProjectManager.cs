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
        public const string RasterExtension = "tif";

        public static GCDProject Project { get; internal set; }
        public static DirectoryInfo ExcelTemplatesFolder { get; internal set; }
        public static DirectoryInfo ReportsFolder { get; internal set; }
        public static RasterPyramidManager PyramidManager { get; internal set; }
        private static FileInfo SurveyTypesPath { get; set; }
        public static ErrorCalculation.FIS.FISLibrary FISLibrary { get; internal set; }

        // Used for display but not saved to project XML
        public static System.Drawing.Color ColorDeposition { get; set; }
        public static System.Drawing.Color ColorErosion { get; set; }

        public static bool IsArcMap { get { return System.Reflection.Assembly.GetEntryAssembly() == null; } }

        // Simplifies the path combinations above
        public static DirectoryInfo CombinePaths(DirectoryInfo parentDir, string subDir) { return new DirectoryInfo(Path.Combine(parentDir.FullName, subDir)); }

        // Public event for ArcGIS to listen to when a GIS layer is about to be deleted
        // ArcGIS should search for this path in the map and then remove the layer
        public delegate void GISLayerDeletingEvent(GISLayerEventArgs e);
        public static event GISLayerDeletingEvent GISLayerDeletingEventHandler;

        public delegate void GISLayerBrowsing(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e);
        public static event GISLayerBrowsing GISLayerBrowsingEventHandler;

        public delegate void GISBrowseVector(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e, GCDConsoleLib.GDalGeometryType.SimpleTypes geometryType);
        public static event GISBrowseVector GISBrowseVectorEventHandler;

        public delegate void GISSelectingVector(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e, GCDConsoleLib.GDalGeometryType.SimpleTypes geometryType);
        public static event GISSelectingVector GISSelectingVectorEventHandler;

        public delegate void GISAddRasterToMapEvent(GCDProjectRasterItem raster);
        public static event GISAddRasterToMapEvent GISAddRasterToMapEventHandler;

        public delegate void GISAddVectorToMapEvent(GCDProjectVectorItem vector);
        public static event GISAddVectorToMapEvent GISAddVectorToMapEventHandler;

        public static EventHandler<GCDConsoleLib.OpStatus> OnProgressChange;
        public static EventHandler<GCDConsoleLib.OpStatus> OnProgressChangeAsync;

        public static Dictionary<string, SurveyType> SurveyTypes
        {
            get { return SurveyType.Load(SurveyTypesPath); }
            set { SurveyType.Save(SurveyTypesPath, value); }
        }

        private static DirectoryInfo ApplicationFolder
        {
            get { return new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GCD")); }
        }

        public static void Init(string sAutomaticPyramids)
        {
            PyramidManager = new RasterPyramidManager(sAutomaticPyramids);

            SurveyTypesPath = new FileInfo(Path.Combine(ApplicationFolder.FullName, "SurveyTypes.xml"));

            // The FIS library doesn't need to exist
            FISLibrary = new ErrorCalculation.FIS.FISLibrary(ApplicationFolder);
            FISLibrary.Load();

            ExcelTemplatesFolder = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Deploy", "ExcelTemplates"));
            if (!ExcelTemplatesFolder.Exists)
            {
                Exception ex = new Exception("The GCD Excel template folder path does not exist.");
                ex.Data["GCD Excel Template Path"] = ExcelTemplatesFolder.FullName;
                throw ex;
            }

            ColorErosion = Properties.Settings.Default.Erosion;
            ColorDeposition = Properties.Settings.Default.Deposition;
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

        public static void RefreshProject()
        {
            Project = GCDProject.Load(Project.ProjectFile);
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

        public static GCDConsoleLib.Raster BrowseRaster(string formTitle, IntPtr parentWindow)
        {
            FileInfo path = null;
            if (IsArcMap)
            {
                if (GISLayerBrowsingEventHandler != null)
                {
                    naru.ui.PathEventArgs e = new naru.ui.PathEventArgs(path, formTitle, parentWindow);
                    ProjectManager.GISLayerBrowsingEventHandler(null, e);
                }
            }
            else
            {
                path = naru.os.File.BrowseOpenFile(formTitle, "Raster Files (*.tif, *.tiff, *.img)|*.tif;*.tiff;*.img");
            }

            GCDConsoleLib.Raster raster = null;
            if (path is FileInfo)
                raster = new GCDConsoleLib.Raster(path);

            return raster;
        }

        public static void OnBrowseRaster(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e)
        {
            if (GISLayerBrowsingEventHandler != null)
                ProjectManager.GISLayerBrowsingEventHandler(txt, e);
        }

        public static void OnBrowseVector(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e, GCDConsoleLib.GDalGeometryType.SimpleTypes geometryType)
        {
            if (GISBrowseVectorEventHandler != null)
                ProjectManager.GISBrowseVectorEventHandler(txt, e, geometryType);
        }

        public static void OnSelectVector(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e, GCDConsoleLib.GDalGeometryType.SimpleTypes geometryType)
        {
            if (GISSelectingVectorEventHandler != null)
                ProjectManager.GISSelectingVectorEventHandler(txt, e, geometryType);
        }

        public static void OnAddRasterToMap(GCDCore.Project.GCDProjectRasterItem raster)
        {
            if (GISAddRasterToMapEventHandler != null)
                ProjectManager.GISAddRasterToMapEventHandler(raster);
        }

        public static void OnAddVectorToMap(GCDProjectVectorItem vector)
        {
            if (GISAddVectorToMapEventHandler != null)
                ProjectManager.GISAddVectorToMapEventHandler(vector);
        }

        public static void AddNewProjectItemToMap(GCDProjectItem item)
        {
            if (IsArcMap && Properties.Settings.Default.AddOutputLayersToMap)
            {
                try
                {
                    if (item is GCDProjectRasterItem)
                    {
                        OnAddRasterToMap(item as GCDProjectRasterItem);
                    }
                    else if (item is GCDProjectVectorItem)
                    {
                        OnAddVectorToMap(item as GCDProjectVectorItem);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error adding new project item to the map " + item.Name + ex.Message);
                }
            }
        }

        public class GISLayerEventArgs : EventArgs
        {
            public readonly FileInfo RasterPath;

            public GISLayerEventArgs(FileInfo rasterPath)
            {
                RasterPath = rasterPath;
            }
        }

        public static FileInfo GetProjectItemPath(DirectoryInfo parentFolder, string groupFolderPrefix, string name, string fileExtension)
        {
            DirectoryInfo itemDir = GetIndexedSubDirectory(parentFolder, groupFolderPrefix);
            string path = Path.Combine(itemDir.FullName, naru.os.File.RemoveDangerousCharacters(name));
            path = Path.ChangeExtension(path, fileExtension);
            return new FileInfo(path);
        }

        public static DirectoryInfo GetIndexedSubDirectory(DirectoryInfo parentFolder, string prefix)
        {
            // Find unique folder on disk
            int existingIndex = 0;

            if (parentFolder.Exists)
            {
                foreach (DirectoryInfo existingFolder in parentFolder.GetDirectories(string.Format("{0}*", prefix), SearchOption.TopDirectoryOnly))
                {
                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(existingFolder.FullName, "([0-9]*)$");
                    if (match.Groups.Count > 1)
                    {
                        int folderSuffix = int.Parse(match.Groups[1].Value);
                        if (folderSuffix > existingIndex)
                            existingIndex = folderSuffix;
                    }
                }
            }

            DirectoryInfo finalFolder = new DirectoryInfo(Path.Combine(parentFolder.FullName, string.Format("{0}{1:0000}", prefix, existingIndex + 1)));
            return finalFolder;
        }
    }
}