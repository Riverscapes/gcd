using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace GCDCore.Project
{
    public class ProjectManagerUI : ProjectManagerBase
    {
        protected static RasterPyramidManager m_PyramidManager;

        private static FileInfo m_FISLibrary;
        public static bool IsArcMap
        {
            get { return System.Reflection.Assembly.GetEntryAssembly().FullName.ToLower().Contains("arcmap"); }
        }

        public static RasterPyramidManager PyramidManager { get; }

        public static List<ErrorCalculation.FIS.FISLibraryItem> FISLibrary
        {
            get
            {
                if (m_FISLibrary.Exists)
                {
                    return ErrorCalculation.FIS.FISLibraryItem.Load(m_FISLibrary);
                }
                else
                {
                    return new List<ErrorCalculation.FIS.FISLibraryItem>();
                }
            }
            set { ErrorCalculation.FIS.FISLibraryItem.Save(m_FISLibrary, value); }
        }

        private static string ApplicationFolder
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GCD"); }
        }

        public ProjectManagerUI(GCDConsoleLib.Raster.RasterDriver eDefaultRasterType, string sAutomaticPyramids) : base(ApplicationFolder, Properties.Settings.Default.Erosion, Properties.Settings.Default.Deposition, eDefaultRasterType)
        {
            m_PyramidManager = new RasterPyramidManager(sAutomaticPyramids);
            m_FISLibrary = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FISLibrary.xml"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bRemoveMissingrasters"></param>
        /// <returns>True if the GCD project has been altered during the validation process</returns>
        public static bool Validate(bool bRemoveMissingrasters = false)
        {
            if (!Properties.Settings.Default.ValidateProjectOnLoad)
            {
                return false;
            }

            bool bEditedGCDFile = false;
            StringBuilder sOutputMessages = new StringBuilder();

            ProjectDS.ProjectRow rProject = CurrentProject;
            if (!(rProject is ProjectDS.ProjectRow))
            {
                return bEditedGCDFile;
            }

            List<ProjectDS.DEMSurveyRow> lItemsToRemove = new List<ProjectDS.DEMSurveyRow>();
            foreach (ProjectDS.DEMSurveyRow rDEM in rProject.GetDEMSurveyRows())
            {
                FileInfo sPath = GetAbsolutePath(rDEM.Source);
                if (!sPath.Exists)
                {
                    lItemsToRemove.Add(rDEM);
                }
            }

            //Pass bEditedGCDFile as a reference argument between these 3 methods 
            bEditedGCDFile = ValidateAssociatedSurfaces(ref sOutputMessages, ref bEditedGCDFile, bRemoveMissingrasters);
            bEditedGCDFile = ValidateErrorSurfaces(ref sOutputMessages, ref bEditedGCDFile, bRemoveMissingrasters);
            bEditedGCDFile = ValidateDoDAnalysisSurfaces(ref rProject, ref sOutputMessages, ref bEditedGCDFile, bRemoveMissingrasters);

            if (bRemoveMissingrasters)
            {
                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count);

                foreach (ProjectDS.DEMSurveyRow rDEM in lItemsToRemove)
                {
                    sOutputMessages.Append("Removing DEM Survey '" + rDEM.Name + "' because the source raster is missing or cannot be found.").AppendLine();
                    ds.DEMSurvey.RemoveDEMSurveyRow(rDEM);
                    Debug.Write(sOutputMessages.ToString());
                    bEditedGCDFile = true;
                }
            }

            try
            {
                save();
            }
            catch (Exception ex)
            {
                ex.Data["Removing datasets"] = sOutputMessages;
                throw;
            }

            return bEditedGCDFile;

            // This code cleans the project of empty folders which were not fully deleted due to inability to remove
            // ESRI locks even after the file has successfully been deleted And removed from GCD xml
            CleanProjectFolderStructure(true);

            return bEditedGCDFile;
        }

        private static bool ValidateAssociatedSurfaces(ref StringBuilder sOutputMessages, ref bool bEditedGCDFile, bool bRemoveMissingRasters = false)
        {
            List<ProjectDS.AssociatedSurfaceRow> lItemsToRemove = new List<ProjectDS.AssociatedSurfaceRow>();
            foreach (ProjectDS.AssociatedSurfaceRow rAssoc in ds.AssociatedSurface.Rows)
            {
                dynamic dbTest = (DataRow)rAssoc;
                if (DBNull.Value == dbTest("OriginalSource"))
                {
                    rAssoc.OriginalSource = rAssoc.Source;
                    string sAssociatedSurfaceName = System.IO.Path.GetFileNameWithoutExtension(rAssoc.OriginalSource);
                    rAssoc.Source = GetRelativePath(OutputManager.AssociatedSurfaceRasterPath(rAssoc.DEMSurveyRow.Name, sAssociatedSurfaceName));
                    CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, 1);
                    bEditedGCDFile = true;
                    save();
                }

                FileInfo sPath = GetAbsolutePath(rAssoc.Source);
                if (!sPath.Exists)
                {
                    lItemsToRemove.Add(rAssoc);
                }
            }

            if (bRemoveMissingRasters)
            {
                //Check to see if a copy of the .gcd project is necessary
                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count);

                foreach (ProjectDS.AssociatedSurfaceRow rAssoc in lItemsToRemove)
                {
                    sOutputMessages.Append("Removing Associated Surface '" + rAssoc.Name + "' because the source raster is missing or cannot be found.").AppendLine();
                    ds.AssociatedSurface.RemoveAssociatedSurfaceRow(rAssoc);
                    Debug.Write(sOutputMessages.ToString());
                    bEditedGCDFile = true;
                }
            }

            return bEditedGCDFile;
        }

        private static bool ValidateErrorSurfaces(ref StringBuilder sOutputMessages, ref bool bEditedGCDFile, bool bRemoveMissingRasters = false)
        {
            List<ProjectDS.ErrorSurfaceRow> lItemsToRemove = new List<ProjectDS.ErrorSurfaceRow>();
            foreach (ProjectDS.ErrorSurfaceRow rError in ds.ErrorSurface.Rows)
            {
                if (rError.IsSourceNull())
                {
                    lItemsToRemove.Add(rError);
                }
                else
                {
                    FileInfo sPath = GetAbsolutePath(rError.Source);
                    if (!sPath.Exists)
                    {
                        lItemsToRemove.Add(rError);
                    }
                }
            }

            if (bRemoveMissingRasters)
            {
                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count);

                foreach (ProjectDS.ErrorSurfaceRow rError in lItemsToRemove)
                {
                    string sMessage = "Removing Error Surface '" + rError.Name + "' because the source raster is missing or cannot be found.";
                    ds.ErrorSurface.RemoveErrorSurfaceRow(rError);
                    sOutputMessages.Append(sMessage).AppendLine();
                    Debug.Write(sOutputMessages.ToString());
                    bEditedGCDFile = true;
                }
            }

            return bEditedGCDFile;
        }

        private static bool ValidateDoDAnalysisSurfaces(ref ProjectDS.ProjectRow rProjectRow, ref StringBuilder sOutputMessages, ref bool bEditedGCDFile, bool bRemoveMissingRasters = false)
        {
            List<ProjectDS.DoDsRow> lItemsToRemove = new List<ProjectDS.DoDsRow>();
            foreach (ProjectDS.DoDsRow rDoD in ds.DoDs.Rows)
            {
                FileInfo sPathDoDRaw = GetAbsolutePath(rDoD.RawDoDPath);
                FileInfo sPathDoDThresh = GetAbsolutePath(rDoD.ThreshDoDPath);
                if (!sPathDoDRaw.Exists | !sPathDoDThresh.Exists)
                {
                    lItemsToRemove.Add(rDoD);
                }
            }

            if (bRemoveMissingRasters)
            {
                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count);

                foreach (ProjectDS.DoDsRow rDoD in lItemsToRemove)
                {
                    sOutputMessages.Append("Removing DoD Surface '" + rDoD.Name + "' because the source raster is missing or cannot be found.").AppendLine();
                    ds.DoDs.RemoveDoDsRow(rDoD);
                    Debug.Write(sOutputMessages.ToString());
                    bEditedGCDFile = true;
                }
            }

            return bEditedGCDFile;
        }

        private static void CleanProjectFolderStructure(bool bRemoveEmptyFolders = false)
        {
            if (FilePath == null)
            {
                return;
            }

            string sPath = Path.GetDirectoryName(FilePath);
            try
            {
                if (bRemoveEmptyFolders)
                {
                    DeleteEmptyFolders(sPath);
                }
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        private static string CheckCopyStatusOfGCDProject(string sGCDProjectFilePath, bool bEditedGCDFile, int iItemsToRemove)
        {
            if (string.IsNullOrEmpty(sGCDProjectFilePath))
            {
                return null;
            }

            //If gcd file has not been edited in previous steps
            if (bEditedGCDFile == false)
            {
                //If there are elements of GCD file currently marked for removal

                if (iItemsToRemove > 0)
                {
                    //Have established that gcd file has not been edited, i.e. don't want to make a backup of something that has already changed
                    //and established that there items from this set to be removed from the gcd file therefore make a copy of the .gcd file

                    //Copy GCD file, give backup file timestamp of when copy is made
                    string sGCDProjectCopyName = System.IO.Path.GetFileNameWithoutExtension(sGCDProjectFilePath);
                    DateTime sTimeStamp = DateTime.Now;
                    string format = "yyyydMHHmmss";
                    // format is one integer of year day hours minutes seconds
                    sGCDProjectCopyName = sGCDProjectCopyName + sTimeStamp.ToString(format) + ".gcd";
                    string sBackUpPath = Path.Combine(System.IO.Path.GetDirectoryName(sGCDProjectFilePath), "Backup");
                    sGCDProjectCopyName = System.IO.Path.Combine(sBackUpPath, sGCDProjectCopyName);

                    if (System.IO.Directory.Exists(sBackUpPath))
                    {
                        System.IO.File.Copy(sGCDProjectFilePath, sGCDProjectCopyName);
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(sBackUpPath);
                        System.IO.File.Copy(sGCDProjectFilePath, sGCDProjectCopyName);
                    }

                    return sGCDProjectCopyName;
                }
            }

            return null;
        }

        private static void DeleteEmptyFolders(string sPath)
        {
            string[] SubDirectories = Directory.GetDirectories(sPath);
            foreach (string strDirectory in SubDirectories)
            {
                DeleteEmptyFolders(strDirectory);
            }

            if (Directory.GetFiles(sPath).Length + Directory.GetDirectories(sPath).Length == 0)
            {
                Directory.Delete(sPath);
                string sMessage = "Deleting folder '" + sPath + "' from project file system because the folder is empty." + Environment.NewLine;
                Debug.Write(sMessage.ToString());
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
                    Debug.WriteLine("Copying AddIn Folder \"" + sOriginalFolder + "\" to \"" + sDestinationFolder + "\"");
                    CopyDirectory(sOriginalFolder, sDestinationFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(sDestinationFolder);
                string sOriginalFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Deploy");
                //sDestinationFolder = Path.Combine(sDestinationFolder, Path.GetFileNameWithoutExtension(sFolderName))
                Debug.WriteLine("Copying AddIn Folder \"" + sOriginalFolder + "\" to \"" + sDestinationFolder + "\"");
                CopyDirectory(sOriginalFolder, ApplicationFolder);
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
    }
}