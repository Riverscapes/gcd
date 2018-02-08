using System;
using System.IO;
using GCDCore.Project;

namespace GCDCore
{
    public class OutputManager
    {
        public const string RasterExtension = "tif";
        private const string m_sInputsFolder = "inputs";
        private const string m_sAssociatedSurfacesFolder = "AssociatedSurfaces";
        private const string m_sBudgetSegregationFolder = "BS";
        private const string m_sAnalysesFolder = "Analyses";
        private const string m_sChangeDetectionFolder = "CD";
        private const string m_sDoDFolderPrefix = "GCD";
        private const string m_sErrorCalculationsFolder = "ErrorSurfaces";
        private const string m_sAOIFolder = "AOIs";
        private const string m_sDEMSurveyHillshadeSuffix = "_HS";
        private const string m_sDEMSurveyMethodMasks = "Masks";
        private const string m_sErrorSurfaceMethodsFolder = "Methods";
        private const string m_sErrorSurfaceMethodMask = "_Mask";
        private const string m_sInterComparison = "InterCompare";

        private const string m_sFiguresSubfolder = "Figs";
        private string m_sOutputDriver = "GTiff";

        private int m_nNoData = -9999;
        #region "Properties"

        public string OutputDriver
        {
            get { return m_sOutputDriver; }
        }

        public int NoData
        {
            get { return m_nNoData; }
        }
        #endregion


        public OutputManager()
        {
        }

        #region "Folder Paths"

        public string GCDProjectFolder()
        {
            return Path.GetDirectoryName(ProjectManager.Project.ProjectFile.FullName);
        }

        public string DEMSurveyFolder(string sSurveyName)
        {
            sSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            return Path.Combine(Path.Combine(GCDProjectFolder(), m_sInputsFolder), sSurveyName);
        }

        public string AssociatedSurfaceFolder(string sSurveyName, string sAssociatedSurfaceName)
        {
            string sRasterPath = DEMSurveyFolder(sSurveyName);
            sRasterPath = Path.Combine(sRasterPath, m_sAssociatedSurfacesFolder);
            sAssociatedSurfaceName = naru.os.File.RemoveDangerousCharacters(sAssociatedSurfaceName);
            sRasterPath = Path.Combine(sRasterPath, sAssociatedSurfaceName);
            return sRasterPath;
        }

        public string ErrorSurfaceFolder(string sSurveyName, string sErrorSurfaceName)
        {
            string sRasterPath = DEMSurveyFolder(sSurveyName);
            sRasterPath = Path.Combine(sRasterPath, m_sErrorCalculationsFolder);
            sErrorSurfaceName = naru.os.File.RemoveDangerousCharacters(sErrorSurfaceName);
            sRasterPath = Path.Combine(sRasterPath, sErrorSurfaceName);
            return sRasterPath;
        }

        public string ErrorSurfaceMethodFolder(string sSurveyName, string sErrorSurfaceName, string sMethodName)
        {
            string sRasterPath = ErrorSurfaceFolder(sSurveyName, sErrorSurfaceName);
            sRasterPath = Path.Combine(sRasterPath, m_sErrorSurfaceMethodsFolder);
            sRasterPath = Path.Combine(sRasterPath, naru.os.File.RemoveDangerousCharacters(sMethodName).Replace(" ", ""));
            return sRasterPath;
        }

        public string DEMSurveyMethodMaskFolder(string sSurveyName)
        {
            string sMaskPath = DEMSurveyFolder(sSurveyName);
            sMaskPath = Path.Combine(sMaskPath, m_sDEMSurveyMethodMasks);
            return sMaskPath;
        }

        public string AOIFolder(string sAOIName, bool bCreateIfMissing = false)
        {
            sAOIName = naru.os.File.RemoveDangerousCharacters(sAOIName);
            string sFolder = Path.Combine(GCDProjectFolder(), m_sAOIFolder);
            if (bCreateIfMissing)
            {
                Directory.CreateDirectory(sFolder);
            }
            sFolder = Path.Combine(sFolder, sAOIName);
            if (bCreateIfMissing)
            {
                Directory.CreateDirectory(sFolder);
            }
            return sFolder;
        }

        #endregion

        #region "Raster Paths"

        public FileInfo DEMSurveyRasterPath(string sSurveyName)
        {
            return naru.os.File.GetNewSafeName(DEMSurveyFolder(sSurveyName), sSurveyName, RasterExtension);
        }

        public FileInfo DEMSurveyHillShadeRasterPath(string sSurveyName)
        {
            string path = naru.os.File.RemoveDangerousCharacters(sSurveyName + m_sDEMSurveyHillshadeSuffix);
            path = Path.ChangeExtension(Path.Combine(DEMSurveyFolder(sSurveyName), path), RasterExtension);
            return new System.IO.FileInfo(path);
        }

        public FileInfo AssociatedSurfaceRasterPath(string sSurveyName, string sAssociatedSurface)
        {
            return naru.os.File.GetNewSafeName(AssociatedSurfaceFolder(sSurveyName, sAssociatedSurface), sAssociatedSurface, RasterExtension);
        }

        public FileInfo ErrorSurfaceRasterPath(string sSurveyName, string sErrorSurfaceName, bool bCreateWorkspace = true)
        {
            string sSafeName = naru.os.File.RemoveDangerousCharacters(sErrorSurfaceName);
            string sWorkspace = ErrorSurfaceFolder(sSurveyName, sSafeName);

            if (!Directory.Exists(sWorkspace) && bCreateWorkspace)
            {
                Directory.CreateDirectory(sWorkspace);
            }

            return naru.os.File.GetNewSafeName(sWorkspace, sSafeName, RasterExtension);
        }

        public FileInfo DEMSurveyMethodMaskPath(string sSurveyName)
        {
            string sMaskPath = DEMSurveyMethodMaskFolder(sSurveyName);
            string sFeatureClassName = naru.os.File.RemoveDangerousCharacters(sSurveyName) + m_sErrorSurfaceMethodMask;
            return naru.os.File.GetNewSafeName(sMaskPath, sFeatureClassName, "shp");
        }

        public string AOIFeatureClassPath(string sAOIName)
        {
            string sAOISafe = naru.os.File.RemoveDangerousCharacters(sAOIName).Replace(" ", "");
            sAOISafe = Path.Combine(AOIFolder(sAOISafe), sAOISafe);
            sAOISafe = Path.ChangeExtension(sAOISafe, "shp");
            return sAOISafe;
        }

        private FileInfo BuildFixedRasterPath(DirectoryInfo folder, string RasterFileName)
        {
            string sPath = Path.Combine(folder.FullName, RasterFileName);
            sPath = Path.ChangeExtension(sPath, RasterExtension);
            return new FileInfo(sPath);
        }

        #endregion

        //Public Function GetErrorRasterPath(sFolder As String, ByVal sSurveyName As String, ByVal sErrorName As String) As String
        //    'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/RasterName

        //    'input directory
        //    Dim inputsDirectoryPath As String = Path.Combine(sFolder, "Inputs")
        //    If Not Directory.Exists(inputsDirectoryPath) Then
        //        Directory.CreateDirectory(inputsDirectoryPath)
        //    End If

        //    'Survey directory
        //    Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
        //    Dim surveyDirectoryPath As String = Path.Combine(inputsDirectoryPath, sSafeSurveyName)
        //    If Not Directory.Exists(surveyDirectoryPath) Then
        //        Directory.CreateDirectory(surveyDirectoryPath)
        //    End If

        //    'Error Surfaces directory
        //    Dim ErrorSurfacesDirectoryPath As String = Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
        //    If Not Directory.Exists(ErrorSurfacesDirectoryPath) Then
        //        Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
        //    End If

        //    'ErrorSurfaceName directory
        //    Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
        //    Dim ErrorNameDirectoryPath As String = Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
        //    If Not Directory.Exists(ErrorNameDirectoryPath) Then
        //        Directory.CreateDirectory(ErrorNameDirectoryPath)
        //    End If

        //    ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
        //    'errorCalcDirectoryPath &= Path.DirectorySeparatorChar
        //    'generate error name
        //    Dim errorCalcPath As String = GISCode.GCDConsoleLib.Raster.GetNewSafeName(ErrorNameDirectoryPath, RasterType, sErrorName, 12)
        //    Return errorCalcPath
        //End Function

        //Public Function GetMethodErrorSurfarcePath(sFolder As String, ByVal sSurveyName As String, ByVal sErrorName As String, ByVal sMethod As String) As String
        //    'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/Method/RasterName

        //    'input directory
        //    Dim inputsDirectoryPath As String = Path.Combine(sFolder, "Inputs")
        //    If Not Directory.Exists(inputsDirectoryPath) Then
        //        Directory.CreateDirectory(inputsDirectoryPath)
        //    End If

        //    'Survey directory
        //    Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
        //    Dim surveyDirectoryPath As String = Path.Combine(inputsDirectoryPath, sSafeSurveyName)
        //    If Not Directory.Exists(surveyDirectoryPath) Then
        //        Directory.CreateDirectory(surveyDirectoryPath)
        //    End If

        //    'Error Surfaces directory
        //    Dim ErrorSurfacesDirectoryPath As String = Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
        //    If Not Directory.Exists(ErrorSurfacesDirectoryPath) Then
        //        Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
        //    End If

        //    'ErrorSurfaceName directory
        //    Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
        //    Dim ErrorNameDirectoryPath As String = Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
        //    If Not Directory.Exists(ErrorNameDirectoryPath) Then
        //        Directory.CreateDirectory(ErrorNameDirectoryPath)
        //    End If

        //    'MethodName directory
        //    Dim sSafeMethodName As String = naru.os.File.RemoveDangerousCharacters(sMethod)
        //    Dim sMethodNameDirectory As String = Path.Combine(ErrorNameDirectoryPath, sSafeMethodName)
        //    If Not Directory.Exists(sMethodNameDirectory) Then
        //        Directory.CreateDirectory(sMethodNameDirectory)
        //    End If

        //    ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
        //    'errorCalcDirectoryPath &= Path.DirectorySeparatorChar
        //    'generate error name
        //    Dim sMethodErrorSurfacePath As String = GCDConsoleLib.Raster.GetNewSafeName(sMethodNameDirectory, RasterType, sMethod, 12)
        //    Return sMethodErrorSurfacePath
        //End Function

        //Public Function GetDoDThresholdPath(sFolder As String, ByVal sDoDName As String) As String
        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    'get DoD output path
        //    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        //    'generate error name
        //    Dim dodThresholdPath As String = GCDConsoleLib.Raster.GetNewSafeName(dodDirectoryPath, RasterType, safeDoDName, 12)
        //    Return dodThresholdPath
        //End Function

        //Public Function GetCsvThresholdPath(sFolder As String, ByVal sDoDName As String) As String
        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    'get DoD output path
        //    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        //    'generate error name
        //    Dim csvThresholdPath As String = FileSystem.GetNewSafeFileName(dodDirectoryPath, safeDoDName, "csv")
        //    Return csvThresholdPath
        //End Function

        //Public Function GetPropagatedErrorPath(sFolder As String, sDoDName As String) As String
        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    'get DoD output path
        //    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        //    'generate error name
        //    Dim dodThresholdPath As String = GCDConsoleLib.Raster.GetNewSafeName(dodDirectoryPath, RasterType, safeDoDName, 12)
        //    Return dodThresholdPath
        //End Function

        //Public Function GetCsvRawPath(sFolder As String, ByVal sDoDName As String) As String
        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    'get DoD output path
        //    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        //    'generate error name
        //    Dim csvThresholdPath As String = FileSystem.GetNewSafeFileName(dodDirectoryPath, "raw", "csv")
        //    Return csvThresholdPath
        //End Function

        //Public Function GetDoDRawPath(sFolder As String, ByVal sDoDName As String) As String
        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    'get DoD output path
        //    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        //    'generate error name
        //    Dim csvThresholdPath As String = GCDConsoleLib.Raster.GetNewSafeName(dodDirectoryPath, RasterType, "dod", 12)
        //    Return csvThresholdPath
        //End Function

        //Public Function GetDoDOutputPath(sFolder As String, ByVal sDoDName As String) As String

        //    'setup folder for analysis
        //    Dim analysisDirectoryPath As String = Path.Combine(sFolder, m_sAnalysesFolder)
        //    If Not Directory.Exists(analysisDirectoryPath) Then
        //        Directory.CreateDirectory(analysisDirectoryPath)
        //    End If

        //    'setup folder for dods
        //    Dim dodsDirectoryPath As String = Path.Combine(analysisDirectoryPath, m_sChangeDetectionFolder)
        //    If Not Directory.Exists(dodsDirectoryPath) Then
        //        Directory.CreateDirectory(dodsDirectoryPath)
        //    End If

        //    'setup folder for dods
        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    Dim dodDirectoryPath As String = Path.Combine(dodsDirectoryPath, safeDoDName)
        //    If Not Directory.Exists(dodDirectoryPath) Then
        //        Directory.CreateDirectory(dodDirectoryPath)
        //    End If

        //    dodDirectoryPath &= Path.DirectorySeparatorChar
        //    Return dodDirectoryPath
        //End Function




        //Public Function GetChangeDetectionDirectoryPath(ByVal NewSurveyName As String, OldSurveyName As String, ByVal sDoDName As String) As String
        //    'structure: Analyses/ChangeDetection/Dods_NewSurveyName-OldSurveyName/DoDName

        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        //    Dim sDoDFolder As String = GetDoDOutputFolder(safeDoDName)

        //    'setup folder for dods directory
        //    Dim DoDsDirectoryName As String = "DoDs_" & NewSurveyName & "-" & OldSurveyName
        //    Dim SafeDoDsDirectoryName As String = naru.os.File.RemoveDangerousCharacters(DoDsDirectoryName)
        //    Dim DoDsDirectoryPath As String = Path.Combine(ChangeDetectionDirectoryPath, SafeDoDsDirectoryName)
        //    If Not Directory.Exists(DoDsDirectoryPath) Then
        //        Directory.CreateDirectory(DoDsDirectoryPath)
        //    End If

        //    'setup folder for dod
        //    Dim DoDDirectoryPath As String = Path.Combine(DoDsDirectoryPath, safeDoDName)
        //    If Not Directory.Exists(DoDDirectoryPath) Then
        //        Directory.CreateDirectory(DoDDirectoryPath)
        //    End If

        //    Return DoDDirectoryPath
        //End Function

        //Public Function GetGeomorphOutputPath(sFolder As String, ByVal sDoDName As String) As String

        //    'get DoD folder 
        //    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)

        //    'setup folder for Geomorph (based on GCD 4 naming convention
        //    Dim GeomorphDirectoryPath As String = Path.Combine(dodDirectoryPath, "Geomorph")
        //    If Not Directory.Exists(GeomorphDirectoryPath) Then
        //        Directory.CreateDirectory(GeomorphDirectoryPath)
        //    End If

        //    GeomorphDirectoryPath &= Path.DirectorySeparatorChar
        //    Return GeomorphDirectoryPath
        //End Function



        public object GetMethodMaskCopyPath(string sFolder, string sSurveyName, string OrigMethodMaskName)
        {
            //structure: Inputs/Surveyname/MethodMasks/MethodMaskName

            //input directory
            string inputsDirectoryPath = Path.Combine(sFolder, "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                Directory.CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                Directory.CreateDirectory(surveyDirectoryPath);
            }

            //MethodMask directory
            string MethodMaskDirectoryPath = Path.Combine(surveyDirectoryPath, "MethodMasks");
            if (!Directory.Exists(MethodMaskDirectoryPath))
            {
                Directory.CreateDirectory(MethodMaskDirectoryPath);
            }

            //Method Mask path
            string MethodMaskCopyPath = Path.Combine(MethodMaskDirectoryPath, OrigMethodMaskName);
            return MethodMaskCopyPath;

        }

        public object GetMethodMaskCopyPath(string sSurveyName, string OrigMethodMaskName)
        {
            //structure: Inputs/Surveyname/MethodMasks/MethodMaskName

            //input directory
            string inputsDirectoryPath = Path.Combine(GCDProjectFolder(), "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                Directory.CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                Directory.CreateDirectory(surveyDirectoryPath);
            }

            //MethodMask directory
            string MethodMaskDirectoryPath = Path.Combine(surveyDirectoryPath, "MethodMasks");
            if (!Directory.Exists(MethodMaskDirectoryPath))
            {
                Directory.CreateDirectory(MethodMaskDirectoryPath);
            }

            //Method Mask path
            string MethodMaskCopyPath = Path.Combine(MethodMaskDirectoryPath, OrigMethodMaskName);
            return MethodMaskCopyPath;

        }

        public object GetInputDEMCopyPath(string sSurveyName, string OrigRasterName)
        {
            //structure: Inputs/Surveyname/rastername

            //input directory
            string inputsDirectoryPath = Path.Combine(GCDProjectFolder(), "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                Directory.CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                Directory.CreateDirectory(surveyDirectoryPath);
            }

            //Raster path
            string RasterCopyPath = Path.Combine(surveyDirectoryPath, OrigRasterName);
            return RasterCopyPath;

        }

        public object GetInputDEMCopyPath(string sFolder, string sSurveyName, string OrigRasterName)
        {
            //structure: Inputs/Surveyname/rastername

            //input directory
            string inputsDirectoryPath = Path.Combine(sFolder, "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                Directory.CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                Directory.CreateDirectory(surveyDirectoryPath);
            }

            //Raster path
            string RasterCopyPath = Path.Combine(surveyDirectoryPath, OrigRasterName);
            return RasterCopyPath;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSurveyName"></param>
        /// <param name="sErrorName"></param>
        /// <param name="sMethod"></param>
        /// <param name="bCreateWorkspace"></param>
        /// <returns></returns>
        /// <remarks>Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/Method/RasterName</remarks>
        public FileInfo ErrorSurfarceMethodRasterPath(string sSurveyName, string sErrorName, string sMethod, bool bCreateWorkspace = true)
        {

            string sWorkspace = ErrorSurfaceMethodFolder(sSurveyName, sErrorName, sMethod);

            if (!Directory.Exists(sWorkspace) && bCreateWorkspace)
            {
                Directory.CreateDirectory(sWorkspace);
            }

            return naru.os.File.GetNewSafeName(sWorkspace, sMethod.Replace(" ", ""), RasterExtension);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sSurveyName"></param>
        /// <param name="sErrorName"></param>
        /// <param name="sMethod"></param>
        /// <param name="bCreateWorkspace"></param>
        /// <returns></returns>
        /// <remarks>Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/Method/RasterName</remarks>
        public FileInfo ErrorSurfarceMethodRasterMaskPath(string sSurveyName, string sErrorName, string sMethod, bool bCreateWorkspace = true)
        {

            string sWorkspace = ErrorSurfaceMethodFolder(sSurveyName, sErrorName, sMethod);
            if (!Directory.Exists(sWorkspace) && bCreateWorkspace)
            {
                Directory.CreateDirectory(sWorkspace);
            }

            return naru.os.File.GetNewSafeName(sWorkspace, sMethod.Replace(" ", "") + m_sErrorSurfaceMethodMask, RasterExtension);

        }

        public string GetAssociatedSurfaceCopyPath(string sFolder, string sSurveyName, string AssociatedSurfaceName, string OrigRasterName)
        {
            //structure: Inputs/Surveyname/AssociatedSurfaces/surfacename/rastername

            //input directory
            string inputsDirectoryPath = Path.Combine(sFolder, "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                CreateDirectory(surveyDirectoryPath);
            }

            //Associated Surfaces directory
            string AssociatedSurfacesDirectoryPath = Path.Combine(surveyDirectoryPath, "AssociatedSurfaces");
            if (!Directory.Exists(AssociatedSurfacesDirectoryPath))
            {
                CreateDirectory(AssociatedSurfacesDirectoryPath);
            }

            //Associated Surface directory
            string SafeAssociatedSurfaceName = naru.os.File.RemoveDangerousCharacters(AssociatedSurfaceName);
            string AssociatedSurfaceNameDirectoryPath = Path.Combine(AssociatedSurfacesDirectoryPath, SafeAssociatedSurfaceName);
            if (!Directory.Exists(AssociatedSurfaceNameDirectoryPath))
            {
                CreateDirectory(AssociatedSurfaceNameDirectoryPath);
            }

            //Raster path
            string AssociatedSurfaceCopyPath = Path.Combine(AssociatedSurfaceNameDirectoryPath, OrigRasterName);
            return AssociatedSurfaceCopyPath;


        }

        private void CreateDirectory(string DirectoryPath)
        {
            try
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Could not create directory '" + DirectoryPath + "'";
                throw new Exception(ErrorMessage);
            }
        }

        public DirectoryInfo GetBudgetSegreationDirectoryPath(DirectoryInfo dodFolder, bool bCreate)
        {
            DirectoryInfo bsGroupFolder = new DirectoryInfo(Path.Combine(dodFolder.FullName, m_sBudgetSegregationFolder));

            if (!bsGroupFolder.Exists && bCreate)
                bsGroupFolder.Create();

            // Find unique folder on disk
            int maxExisting = 0;

            if (bsGroupFolder.Exists)
            {
                foreach (DirectoryInfo existingFolder in bsGroupFolder.GetDirectories("BS*", SearchOption.TopDirectoryOnly))
                {
                    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(existingFolder.FullName, "([0-9]*)$");
                    if (match.Groups.Count > 1)
                    {
                        int folderSuffix = int.Parse(match.Groups[1].Value);
                        if (folderSuffix > maxExisting)
                            maxExisting = folderSuffix;
                    }
                }
            }

            DirectoryInfo bsFolder = new DirectoryInfo(Path.Combine(bsGroupFolder.FullName, string.Format("BS{0:0000}", maxExisting + 1)));

            if (bCreate)
                bsFolder.Create();

            return bsFolder;
        }

        #region "Change Detection"


        public FileInfo RawDoDPath(DirectoryInfo dodFolder)
        {
            return BuildFixedRasterPath(dodFolder, "raw");
        }

        public FileInfo ThrDoDPath(DirectoryInfo dodFolder)
        {
            return BuildFixedRasterPath(dodFolder, "thresh");
        }

        public FileInfo PropagatedErrorPath(DirectoryInfo dodFolder)
        {
            return BuildFixedRasterPath(dodFolder, "PropErr");
        }

        public FileInfo RawHistPath(DirectoryInfo dodFolder)
        {
            return new FileInfo(Path.Combine(dodFolder.FullName, "raw.csv"));
        }

        public FileInfo ThrHistPath(DirectoryInfo dodFolder)
        {
            return new FileInfo(Path.Combine(dodFolder.FullName, "thresh.csv"));
        }

        public FileInfo SummaryXMLPath(DirectoryInfo dodFilder)
        {
            return new FileInfo(Path.Combine(dodFilder.FullName, "summary.xml"));
        }

        /// <summary>
        /// Determine the output folder for a DoD Analysis
        /// </summary>
        /// <param name="sDoDName">Raw Name of the DoD. This method will ensure it is safe.</param>
        /// <param name="bCreateIfMissing">True will create the necessary folder structure. False will just return the path without creating anything.</param>
        /// <returns></returns>
        /// <remarks>Note that the user interface should call this with bCreateIfMissing as False.
        /// That way it can determine the output folder and show it to users without actually
        /// creating the folder. Then the change detection engine code can call this method with
        /// the value of True to ensure that the folder exists when it is time to create files in it.</remarks>
        public DirectoryInfo GetDoDOutputFolder(string sDoDName, bool bCreateIfMissing = false)
        {
            string sDoDFolder = string.Empty;
            int nFolderIndex = 0;

            string sAnalysesFolder = Path.Combine(GCDProjectFolder(), m_sAnalysesFolder);
            if (bCreateIfMissing && !Directory.Exists(sAnalysesFolder))
            {
                Directory.CreateDirectory(sAnalysesFolder);
            }

            string sChangeDetectionFolder = Path.Combine(sAnalysesFolder, m_sChangeDetectionFolder);
            if (bCreateIfMissing && !Directory.Exists(sChangeDetectionFolder))
            {
                Directory.CreateDirectory(sChangeDetectionFolder);
            }

            // By now nFolder index should hold an integer that represents either the max DoD ID or the 
            // max folder integer suffix. Create the new DoD folder with the next biggest integer
            do
            {
                nFolderIndex += 1;
                sDoDFolder = Path.Combine(sChangeDetectionFolder, m_sDoDFolderPrefix + (nFolderIndex).ToString("0000"));
            } while (Directory.Exists(sDoDFolder) && nFolderIndex < 9999);

            if (bCreateIfMissing)
            {
                Directory.CreateDirectory(sDoDFolder);
            }

            return new DirectoryInfo(sDoDFolder);
        }

        private string GetSafeDoDName(string sOriginalName)
        {
            return naru.os.File.RemoveDangerousCharacters(sOriginalName);
        }

        #endregion

        //Public Shared Function GetErrorRasterPath(ByVal sSurveyName As String, ByVal sErrorName As String) As String
        //    'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/RasterName

        //    'input directory
        //    Dim inputsDirectoryPath As String = Path.Combine(outputdirectory, "Inputs")
        //    If Not Directory.Exists(inputsDirectoryPath) Then
        //        Directory.CreateDirectory(inputsDirectoryPath)
        //    End If

        //    'Survey directory
        //    Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
        //    Dim surveyDirectoryPath As String = Path.Combine(inputsDirectoryPath, sSafeSurveyName)
        //    If Not Directory.Exists(surveyDirectoryPath) Then
        //        Directory.CreateDirectory(surveyDirectoryPath)
        //    End If

        //    'Error Surfaces directory
        //    Dim ErrorSurfacesDirectoryPath As String = Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
        //    If Not Directory.Exists(ErrorSurfacesDirectoryPath) Then
        //        Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
        //    End If

        //    'ErrorSurfaceName directory
        //    Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
        //    Dim ErrorNameDirectoryPath As String = Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
        //    If Not Directory.Exists(ErrorNameDirectoryPath) Then
        //        Directory.CreateDirectory(ErrorNameDirectoryPath)
        //    End If

        //    ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
        //    'errorCalcDirectoryPath &= Path.DirectorySeparatorChar
        //    'generate error name
        //    Dim errorCalcPath As String = GetNewSafeNameRaster(ErrorNameDirectoryPath, GISCode.GCDConsoleLib.Raster.GetDefaultRasterType(), sErrorName, 12)
        //    Return errorCalcPath
        //End Function


        //Public Function GetChangeDetectionDirectoryPath(ByVal NewSurveyName As String, OldSurveyName As String, ByVal sDoDName As String) As String
        //    'structure: Analyses/ChangeDetection/Dods_NewSurveyName-OldSurveyName/DoDName

        //    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)

        //    'setup folder for analysis
        //    Dim analysisDirectoryPath As String = Path.Combine(GCDProjectFolder, "Analyses")
        //    If Not Directory.Exists(analysisDirectoryPath) Then
        //        Directory.CreateDirectory(analysisDirectoryPath)
        //    End If

        //    'setup folder for ChangeDetection
        //    Dim ChangeDetectionDirectoryPath As String = Path.Combine(analysisDirectoryPath, "ChangeDetection")
        //    If Not Directory.Exists(ChangeDetectionDirectoryPath) Then
        //        Directory.CreateDirectory(ChangeDetectionDirectoryPath)
        //    End If

        //    'setup folder for dods directory
        //    Dim DoDsDirectoryName As String = "DoDs_" & NewSurveyName & "-" & OldSurveyName
        //    Dim SafeDoDsDirectoryName As String = naru.os.File.RemoveDangerousCharacters(DoDsDirectoryName)
        //    Dim DoDsDirectoryPath As String = Path.Combine(ChangeDetectionDirectoryPath, SafeDoDsDirectoryName)
        //    If Not Directory.Exists(DoDsDirectoryPath) Then
        //        Directory.CreateDirectory(DoDsDirectoryPath)
        //    End If

        //    'setup folder for dod
        //    Dim DoDDirectoryPath As String = Path.Combine(DoDsDirectoryPath, safeDoDName)
        //    If Not Directory.Exists(DoDDirectoryPath) Then
        //        Directory.CreateDirectory(DoDDirectoryPath)
        //    End If

        //    Return DoDDirectoryPath
        //End Function

        public string GetErrorRasterPath(string sSurveyName, string sErrorName)
        {
            //structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/RasterName

            //input directory
            string inputsDirectoryPath = Path.Combine(GCDProjectFolder(), "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                Directory.CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                Directory.CreateDirectory(surveyDirectoryPath);
            }

            //Error Surfaces directory
            string ErrorSurfacesDirectoryPath = Path.Combine(surveyDirectoryPath, "ErrorSurfaces");
            if (!Directory.Exists(ErrorSurfacesDirectoryPath))
            {
                Directory.CreateDirectory(ErrorSurfacesDirectoryPath);
            }

            //ErrorSurfaceName directory
            string sSafeErrorName = naru.os.File.RemoveDangerousCharacters(sErrorName);
            string ErrorNameDirectoryPath = Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName);
            if (!Directory.Exists(ErrorNameDirectoryPath))
            {
                Directory.CreateDirectory(ErrorNameDirectoryPath);
            }

            // PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
            //errorCalcDirectoryPath &= Path.DirectorySeparatorChar
            //generate error name
            string errorCalcPath = naru.os.File.GetNewSafeName(ErrorNameDirectoryPath, sErrorName, RasterExtension).FullName;
            return errorCalcPath;
        }

        public string GetAssociatedSurfaceCopyPath(string sSurveyName, string AssociatedSurfaceName, string OrigRasterName)
        {
            //structure: Inputs/Surveyname/AssociatedSurfaces/surfacename/rastername

            //input directory
            string inputsDirectoryPath = Path.Combine(GCDProjectFolder(), "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                CreateDirectory(inputsDirectoryPath);
            }

            //Survey directory
            string sSafeSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName);
            string surveyDirectoryPath = Path.Combine(inputsDirectoryPath, sSafeSurveyName);
            if (!Directory.Exists(surveyDirectoryPath))
            {
                CreateDirectory(surveyDirectoryPath);
            }

            //Associated Surfaces directory
            string AssociatedSurfacesDirectoryPath = Path.Combine(surveyDirectoryPath, "AssociatedSurfaces");
            if (!Directory.Exists(AssociatedSurfacesDirectoryPath))
            {
                CreateDirectory(AssociatedSurfacesDirectoryPath);
            }

            //Associated Surface directory
            string SafeAssociatedSurfaceName = naru.os.File.RemoveDangerousCharacters(AssociatedSurfaceName);
            string AssociatedSurfaceNameDirectoryPath = Path.Combine(AssociatedSurfacesDirectoryPath, SafeAssociatedSurfaceName);
            if (!Directory.Exists(AssociatedSurfaceNameDirectoryPath))
            {
                CreateDirectory(AssociatedSurfaceNameDirectoryPath);
            }

            //Raster path
            string AssociatedSurfaceCopyPath = Path.Combine(AssociatedSurfaceNameDirectoryPath, OrigRasterName);
            return AssociatedSurfaceCopyPath;
        }

        public string GetWaterSurfaceRasterPath(string sWaterSurfaceName)
        {

            //structure: Inputs/WaterSurfaces/rastername

            //input directory
            string inputsDirectoryPath = Path.Combine(GCDProjectFolder(), "Inputs");
            if (!Directory.Exists(inputsDirectoryPath))
            {
                CreateDirectory(inputsDirectoryPath);
            }

            // Water Surfaces directory
            string sSurfacesDirectoryPath = Path.Combine(inputsDirectoryPath, "WaterSurfaces");
            if (!Directory.Exists(sSurfacesDirectoryPath))
            {
                CreateDirectory(sSurfacesDirectoryPath);
            }

            //Dim sExtension As String = GISCode.Raster.GetRasterType(My.Settings.DefaultRasterFormat)
            //Dim eType As GCDConsoleLib.Raster.RasterTypes = Me.GetDefaultRasterTyp
            string sRasterPath = naru.os.File.GetNewSafeName(sSurfacesDirectoryPath, sWaterSurfaceName, RasterExtension).FullName;
            //sRasterPath = Path.Combine(sSurfacesDirectoryPath, sRasterPath)
            //sRasterPath = Path.ChangeExtension(sRasterPath, GISCode.Raster.GetRasterExtension(sExtension))

            return sRasterPath;

        }

        public string GetReservoirPath(string sDEMName, string sWaterSurfaceName)
        {

            string sReservoirPath = null;
            string sTopfolder = GetReservoirParentFolder();

            sReservoirPath = Path.Combine(sTopfolder, naru.os.File.RemoveDangerousCharacters(sDEMName) + "_" + naru.os.File.RemoveDangerousCharacters(sWaterSurfaceName));
            if (!Directory.Exists(sReservoirPath))
            {
                CreateDirectory(sReservoirPath);
            }

            return sReservoirPath;
        }

        public string GetReservoirParentFolder()
        {

            string sTopFolder = Path.Combine(GCDProjectFolder(), "Reservoir");
            if (!Directory.Exists(sTopFolder))
            {
                CreateDirectory(sTopFolder);
            }

            return sTopFolder;
        }

        public System.IO.DirectoryInfo GetChangeDetectionFiguresFolder(System.IO.DirectoryInfo parentFolder, bool bCreate)
        {

            string sResult = null;
            if (parentFolder.Exists)
            {
                sResult = Path.Combine(parentFolder.FullName, m_sFiguresSubfolder);
                if (bCreate && !Directory.Exists(sResult))
                {
                    Directory.CreateDirectory(sResult);
                }
            }
            else
            {
                throw new ArgumentException("The parent folder must already exist.", "sParentFolder");
            }
            return new System.IO.DirectoryInfo(sResult);
        }

        public FileInfo GetInterComparisonPath(string name)
        {
            string sTopFolder = Path.Combine(GCDProjectFolder(), m_sInterComparison);
            return naru.os.File.GetNewSafeName(sTopFolder, "ic" + name, "xml");
        }
    }
}