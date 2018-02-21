using System;
using System.IO;
using GCDCore.Project;

namespace GCDCore
{
    public class OutputManager
    {
        public const string RasterExtension = "tif";
        private const string m_sInputsFolder = "Inputs";
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
        private const string m_sRefSurfaceFolder = "RefSurfaces";
        private const string m_sProjectMaskFolder = "Masks";
        private const string m_sMorphologicalFolder = "Morph";

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
                throw new Exception(ErrorMessage, ex);
            }
        }

        public DirectoryInfo GetBudgetSegreationDirectoryPath(DirectoryInfo dodFolder, bool bCreate)
        {
            return GetIndexedSubDirectory(dodFolder, m_sBudgetSegregationFolder, "BS", bCreate);
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
        /// <param name="bCreate">True will create the necessary folder structure. False will just return the path without creating anything.</param>
        /// <returns></returns>
        /// <remarks>Note that the user interface should call this with bCreateIfMissing as False.
        /// That way it can determine the output folder and show it to users without actually
        /// creating the folder. Then the change detection engine code can call this method with
        /// the value of True to ensure that the folder exists when it is time to create files in it.</remarks>
        public DirectoryInfo GetDoDOutputFolder(bool bCreate = false)
        {
            DirectoryInfo diParent = new DirectoryInfo(Path.Combine(ProjectManager.Project.ProjectFile.DirectoryName, m_sAnalysesFolder));

            if (!diParent.Exists && bCreate)
                diParent.Create();

            return GetIndexedSubDirectory(diParent, m_sChangeDetectionFolder, m_sDoDFolderPrefix, bCreate);
        }

        #endregion 

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

        public DirectoryInfo GetChangeDetectionFiguresFolder(System.IO.DirectoryInfo parentFolder, bool bCreate)
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
            string sTopFolder = Path.Combine(GCDProjectFolder(), m_sAnalysesFolder, m_sInterComparison);
            return naru.os.File.GetNewSafeName(sTopFolder, name, "xml");
        }

        private string GetReferenceSurfaceFolder(string surfaceName)
        {
            surfaceName = naru.os.File.RemoveDangerousCharacters(surfaceName);
            return Path.Combine(GCDProjectFolder(), m_sInputsFolder, m_sRefSurfaceFolder, surfaceName);
        }

        public FileInfo GetReferenceSurfaceRasterPath(string surfaceName)
        {
            return naru.os.File.GetNewSafeName(GetReferenceSurfaceFolder(surfaceName), surfaceName, RasterExtension);
        }

        public FileInfo GetReferenceErrorSurfaceRasterPath(string errorName, DirectoryInfo surfaceDir)
        {
            errorName = naru.os.File.RemoveDangerousCharacters(errorName);
            string subDir = System.IO.Path.Combine(surfaceDir.FullName, m_sErrorCalculationsFolder, errorName);
            return naru.os.File.GetNewSafeName(subDir, errorName, RasterExtension);
        }

        private DirectoryInfo GetProjectMaskFolder()
        {
            DirectoryInfo diParent = new DirectoryInfo(Path.Combine(ProjectManager.Project.ProjectFile.DirectoryName, m_sInputsFolder));

            return GetIndexedSubDirectory(diParent, m_sProjectMaskFolder, "Mask", false);
        }

        public FileInfo GetMaskShapeFilePath(string maskName)
        {
            DirectoryInfo diFolder = GetProjectMaskFolder();
            return naru.os.File.GetNewSafeName(diFolder.FullName, maskName, "shp");
        }

        public DirectoryInfo GetMorphologicalDirectory(DirectoryInfo bsFolder, bool bCreate)
        {
            return GetIndexedSubDirectory(bsFolder, m_sMorphologicalFolder, "MA", bCreate);
        }

        public DirectoryInfo GetIndexedSubDirectory(DirectoryInfo parentFolder, string groupFolder, string prefix, bool bCreate)
        {
            DirectoryInfo diGroupFolder = new DirectoryInfo(Path.Combine(parentFolder.FullName, groupFolder));

            if (!diGroupFolder.Exists && bCreate)
                diGroupFolder.Create();

            // Find unique folder on disk
            int existingIndex = 0;

            if (diGroupFolder.Exists)
            {
                foreach (DirectoryInfo existingFolder in diGroupFolder.GetDirectories(string.Format("{0}*", prefix), SearchOption.TopDirectoryOnly))
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

            DirectoryInfo finalFolder = new DirectoryInfo(Path.Combine(diGroupFolder.FullName, string.Format("{0}{1:0000}", prefix, existingIndex + 1)));

            if (bCreate)
                finalFolder.Create();

            return finalFolder;
        }
    }
}