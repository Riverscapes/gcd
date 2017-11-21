using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GCDCore.Project;
using GCDConsoleLib;

namespace GCDCore.ErrorCalculation
{
    public class ErrorSurfaceEngine
    {
        private const int GDAL_NoDataValue = -9999;
        private const string GDAL_OutputDriver = "GTiff";
        public const string UniformErrorString = "Uniform Error";
        public const string FISErrorType = "FIS";
        public const string MultipleErrorType = "Multiple";

        public const string AssociatedsurfaceErrorType = "Associated Surface";

        public readonly DEMSurvey DEM;

        public ErrorSurfaceEngine(DEMSurvey dem)
        {
            DEM = dem;
        }

        public ErrorSurface CreateErrorSurfaceRaster(string name, Dictionary<string, ErrorSurfaceProperty> errProps)
        {
            // Create the name for the final error surface raster
            FileInfo errSurfaceRasterPath = ProjectManagerBase.OutputManager.ErrorSurfaceRasterPath(DEM.Name, name, true);
            Raster gErrorSurface = null;

            switch (m_ErrorSurfaceRow.Type)
            {
                case UniformErrorString:
                    double fError = double.Parse(m_ErrorSurfaceRow.GetMultiErrorPropertiesRows().First<Project.ProjectDS.MultiErrorPropertiesRow>()._Error);
                    gErrorSurface = GCDConsoleLib.RasterOperators.Uniform( DEMRaster, errSurfaceRasterPath, fError);
                    break;

                case AssociatedsurfaceErrorType:

                    // Find the associated surface on which the error raster should be based and copy it into location so that it only has values where DEM has values.
                    Project.ProjectDS.MultiErrorPropertiesRow errorAssoc = m_ErrorSurfaceRow.GetMultiErrorPropertiesRows().First<Project.ProjectDS.MultiErrorPropertiesRow>();
                    Project.ProjectDS.AssociatedSurfaceRow[] assocRows = m_ErrorSurfaceRow.DEMSurveyRow.GetAssociatedSurfaceRows();
                    Project.ProjectDS.AssociatedSurfaceRow rAssoc = assocRows.Where<Project.ProjectDS.AssociatedSurfaceRow>(x => x.AssociatedSurfaceID == errorAssoc.AssociatedSurfaceID).First();

                    GCDConsoleLib.Raster gAssoc = new GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(rAssoc.Source));
                    gErrorSurface = GCDConsoleLib.RasterOperators.Mask( gAssoc,  DEMRaster, errSurfaceRasterPath);
                    break;

                case FISErrorType:
                    CreateFISErrorSurface(m_ErrorSurfaceRow.GetFISInputsRows().First<Project.ProjectDS.FISInputsRow>().FIS, ref DEMRaster, errSurfaceRasterPath, false);
                    break;

                case MultipleErrorType:
                    CreateMultiMethodErrorSurface(errSurfaceRasterPath);
                    break;

                default:
                    Exception ex = new Exception("Unhandled error surface type");
                    ex.Data["Type"] = m_ErrorSurfaceRow.Type;
                    throw ex;
            }

            gErrorSurface = new GCDConsoleLib.Raster(errSurfaceRasterPath);
            m_ErrorSurfaceRow.Source = Project.ProjectManagerBase.GetRelativePath(errSurfaceRasterPath);
            Project.ProjectManagerBase.save();

            // Build raster pyramids
            if (Project.ProjectManagerUI.PyramidManager.AutomaticallyBuildPyramids(RasterPyramidManager.PyramidRasterTypes.ErrorSurfaces))
            {
                Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ErrorSurfaces, errSurfaceRasterPath);
            }

            return gErrorSurface;
        }

        private void CreateMultiMethodErrorSurface(FileInfo outputRasterPath)
        {
            // List of individual method rasters
            List<FileInfo> methodRasters = new List<FileInfo>();

            foreach (Project.ProjectDS.MultiErrorPropertiesRow aMethod in m_ErrorSurfaceRow.GetMultiErrorPropertiesRows())
            {
                FileInfo sMethodRaster = Project.ProjectManagerBase.OutputManager.ErrorSurfarceMethodRasterPath(m_ErrorSurfaceRow.DEMSurveyRow.Name, m_ErrorSurfaceRow.Name, aMethod.Method, true);

                FileInfo sMethodMask = Project.ProjectManagerBase.OutputManager.ErrorSurfarceMethodRasterMaskPath(m_ErrorSurfaceRow.DEMSurveyRow.Name, m_ErrorSurfaceRow.Name, aMethod.Method, true);
                GCDConsoleLib.Raster gMaskRaster = CreateRasterMask(aMethod.Method, sMethodMask);

                switch (aMethod.ErrorType)
                {
                    case UniformErrorString:
                        GCDConsoleLib.RasterOperators.Uniform( gMaskRaster, sMethodRaster, double.Parse(aMethod._Error));
                        break;

                    case AssociatedsurfaceErrorType:

                        // Find the associated surface on which the error raster should be based and copy it into location to ensure values only where DEM has values
                        Project.ProjectDS.AssociatedSurfaceRow rAssoc = m_ErrorSurfaceRow.DEMSurveyRow.GetAssociatedSurfaceRows().First((Project.ProjectDS.AssociatedSurfaceRow s) => s.AssociatedSurfaceID == m_ErrorSurfaceRow.GetMultiErrorPropertiesRows().First<Project.ProjectDS.MultiErrorPropertiesRow>().AssociatedSurfaceID);
                        GCDConsoleLib.Raster gAssoc = new GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(rAssoc.Source));
                        GCDConsoleLib.RasterOperators.Mask( gAssoc,  gMaskRaster, sMethodRaster);
                        break;

                    default:
                        // FIS. the error type field contains the name of the FIS rule file to be used.
                        CreateFISErrorSurface(aMethod.ErrorType, ref gMaskRaster, sMethodRaster, true);
                        break;
                }

                methodRasters.Add(sMethodRaster);
            }

            if (methodRasters.Count < 1)
            {
                return;
            }

            // Call the Raster Manager mosaic function to blend the rasters together.
            string sMosaicWithoutMask = WorkspaceManager.GetTempRaster("Mosaic");
            GCDConsoleLib.Raster gUnmasked = GCDConsoleLib.RasterOperators.Mosaic( methodRasters, new FileInfo(sMosaicWithoutMask));

            // Mask the result so there are only values where the DEM has values
            GCDConsoleLib.Raster gDEM = new GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.Source));
            GCDConsoleLib.RasterOperators.Mask( gUnmasked,  gDEM, outputRasterPath);

            // Update the GCD project with the path to the output raster
            m_ErrorSurfaceRow.Source = Project.ProjectManagerBase.GetRelativePath(outputRasterPath);
            m_ErrorSurfaceRow.Type = MultipleErrorType;
        }

        private void CreateFISErrorSurface(string sFISRuleDefinitionFileName, ref GCDConsoleLib.Raster gReferenceRaster, FileInfo outputRasterPath, bool bClipToMask)
        {
            // Find the local path of the FIS rule file based on the library on this machine. Note
            // could be imported project from another machine.
            FileInfo fisRuleFilePath = null;
            foreach (FIS.FISLibraryItem fis in Project.ProjectManagerUI.FISLibrary)
            {
                if (string.Compare(fis.Name, sFISRuleDefinitionFileName, true) == 0)
                {
                    fisRuleFilePath = fis.FilePath;
                    break;
                }
            }

            if (fisRuleFilePath == null)
            {
                throw new Exception("The FIS rule file specified in the error surface calculation does not exist in the FIS Library.");
            }

            if (!fisRuleFilePath.Exists)
            {
                Exception ex = new Exception("The FIS rule file specified in the FIS Library does not exist on this computer.");
                ex.Data["FIS Rule Path"] = fisRuleFilePath.FullName;
                throw ex;
            }

            // Setup FIS inputs. One for each associated surface input
            Dictionary<string, FileInfo> fisInputs = new Dictionary<string, FileInfo>();

            foreach (Project.ProjectDS.FISInputsRow FISInput in m_ErrorSurfaceRow.GetFISInputsRows())
            {
                // New muti-method FIS check. Make sure that the FIS input is for this FIS file
                if (string.Compare(FISInput.FIS, fisRuleFilePath.FullName, true) == 0 || string.Compare(FISInput.FIS, Path.GetFileNameWithoutExtension(fisRuleFilePath.FullName)) == 0)
                {
                    string sSQL = Project.ProjectManagerBase.ds.AssociatedSurface.DEMSurveyIDColumn.ColumnName + "=" + m_ErrorSurfaceRow.DEMSurveyRow.DEMSurveyID;
                    sSQL += " AND " + Project.ProjectManagerBase.ds.AssociatedSurface.NameColumn.ColumnName + "='" + FISInput.AssociatedSurface + "'";

                    Project.ProjectDS.AssociatedSurfaceRow rAssoc = (Project.ProjectDS.AssociatedSurfaceRow)Project.ProjectManagerBase.ds.AssociatedSurface.Select(sSQL).First();
                    fisInputs[FISInput.FISInput] = Project.ProjectManagerBase.GetAbsolutePath(rAssoc.Source);
                }
            }

            try
            {
                // When this method is being used for a multi-method error surface the reference
                // raster is a mask (with cell value 1). Otherwise the reference raster is the full DEM
                string sFullFISRaster = bClipToMask ? WorkspaceManager.GetTempRaster("FIS") : outputRasterPath.FullName;
                GCDConsoleLib.Raster gFISRaster = GCDConsoleLib.RasterOperators.FISRaster(fisInputs, fisRuleFilePath,  DEMRaster, new FileInfo(sFullFISRaster));

                if (bClipToMask)
                {
                    GCDConsoleLib.RasterOperators.Multiply( gFISRaster,  gReferenceRaster, outputRasterPath);
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error generating FIS error surface raster", ex);
                ex2.Data.Add("OutputPath", outputRasterPath);
                ex2.Data.Add("Reference Mask", gReferenceRaster.GISFileInfo.FullName);
                throw ex2;
            }
        }

        /// <summary>
        /// Create a raster for the features that represent just a single method type
        /// </summary>
        /// <param name="sMethodName">Name of the method for which a raster is needed</param>
        /// <param name="outputRasterMaskPath">Output raster path</param>
        /// <remarks>For some reason the rasters created do not have spatial reference defined.
        /// So you need to project the output raster using the spatial reference from the DEM.
        /// Also, later in this process this mask will be multiplied by the FIS raster to mask
        /// it. So the value of the mask needs to be 1, not zero.</remarks>
        private GCDConsoleLib.Raster CreateRasterMask(string sMethodName, FileInfo outputRasterMaskPath)
        {
            // Copy features for just this method name (e.g. "total station")
            string OutShapefile = WorkspaceManager.GetTempShapeFile("Mask");
            string WhereClause = "\"" + m_ErrorSurfaceRow.DEMSurveyRow.MethodMaskField + "\" = '" + sMethodName + "'";
            GCDConsoleLib.Vector gMaskFeatures = CopyFeatures(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.MethodMask).FullName, OutShapefile, WhereClause);

            GCDConsoleLib.Raster gDEM = new GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.Source));
            GCDConsoleLib.Raster gResult = null;
            try
            {
                string sTempRaster = WorkspaceManager.GetTempRaster("Mask");
                throw new NotImplementedException();
                //  GP.Conversion.PolygonToRaster_conversion(gMaskFeatures, "FID", sTempRaster, gDEM)

                // The conversion does not assign a projection. do so now.
                throw new NotImplementedException();
                // GP.DataManagement.DefineProjection(sTempRaster, gDEM.SpatialReference)

                // The output value of the raster is the FID. Because there's typically just one feature this has
                // a value of zero. We need a value of 1.
                throw new NotImplementedException();
                //GP.SpatialAnalyst.Raster_Calculator("""" & sTempRaster & """ >= 0", sOutputRasterMaskPath, gDEM)
                gResult = new GCDConsoleLib.Raster(outputRasterMaskPath);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error creating raster mask for survey method", ex);
                ex2.Data.Add("Survey Method", sMethodName);
                throw ex2;
            }

            return gResult;
        }

        /// <summary>
        /// Copy only the features for a specific survey method to a new feature class
        /// </summary>
        /// <param name="sMaskFeatureClass">Input polygon method mask feature class</param>
        /// <param name="sOutputFeatureClass">Output polygon feature class</param>
        /// <param name="sWhereClause">SQL Where clause to select just the feautres needed</param>
        /// <remarks></remarks>
        private GCDConsoleLib.Vector CopyFeatures(string sMaskFeatureClass, string sOutputFeatureClass, string sWhereClause = "")
        {
            // TODO implement
            throw new Exception("Not implemented");

            //Dim gPolygonMask As New GCDConsoleLib.Vector(sMaskFeatureClass)
            //Dim gOutput As GCDConsoleLib.Vector = GCDConsoleLib.Vector.CreateFeatureClass(sOutputFeatureClass, GISDataStructures.BasicGISTypes.Polygon, False, gPolygonMask.SpatialReference)

            //Dim pFBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = gOutput.FeatureClass.CreateFeatureBuffer
            //Dim pFOutputCursor As IFeatureCursor = gOutput.FeatureClass.Insert(True)
            //Dim pNewFeature As IFeature = pFBuffer

            //'filter featureclass

            //Dim pInputQueryFilter As IQueryFilter = New QueryFilter()
            //pInputQueryFilter.WhereClause = sWhereClause

            //Dim pFInputCursor As IFeatureCursor = gPolygonMask.FeatureClass.Search(pInputQueryFilter, True)
            //Dim pInputFeature As IFeature = pFInputCursor.NextFeature
            //Dim pShape As ESRI.ArcGIS.Geometry.IGeometry
            //Dim pZAware As ESRI.ArcGIS.Geometry.IZAware
            //Dim pMAware As ESRI.ArcGIS.Geometry.IMAware
            //While TypeOf pInputFeature Is IFeature
            //    pShape = pInputFeature.ShapeCopy
            //    pZAware = pShape
            //    If pZAware.ZAware Then
            //        pZAware.DropZs()
            //        pZAware.ZAware = False
            //    End If

            //    pMAware = pShape
            //    If pMAware.MAware Then
            //        pMAware.DropMs()
            //        pMAware.MAware = False
            //    End If

            //    pNewFeature.Shape = pShape
            //    pFOutputCursor.InsertFeature(pFBuffer)
            //    pInputFeature = pFInputCursor.NextFeature
            //End While
            //pFOutputCursor.Flush()
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pNewFeature)
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFBuffer)
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFOutputCursor)

            //Dim gResult As New GCDConsoleLib.Vector(sOutputFeatureClass)
            //Return gResult
        }
    }
}