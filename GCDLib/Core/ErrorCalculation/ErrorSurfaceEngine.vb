Imports ESRI.ArcGIS.Geodatabase

Namespace Core.ErrorCalculation

    Public Class ErrorSurfaceEngine

        Private m_ErrorSurfaceRow As ProjectDS.ErrorSurfaceRow
        ' Private m_sFISLibraryFilePath As String

        Private Const GDAL_NoDataValue As Integer = -9999
        Private Const GDAL_OutputDriver As String = "GTiff"

        Public Const UniformErrorString As String = "Uniform Error"
        Public Const FISErrorType As String = "FIS"
        Public Const MultipleErrorType As String = "Multiple"
        Public Const AssociatedsurfaceErrorType As String = "Associated Surface"

        Private ReadOnly Property DEMRaster As GCDConsoleLib.Raster
            Get
                Dim gDEMRaster As New GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.Source))
                Return gDEMRaster
            End Get
        End Property

        Public Sub New(errorSurfaceRow As ProjectDS.ErrorSurfaceRow)
            m_ErrorSurfaceRow = errorSurfaceRow
        End Sub

        Public Function CreateErrorSurfaceRaster() As GCDConsoleLib.Raster

            ' Create the name for the final error surface raster
            Dim sErrorSurfaceRasterPath As String = Project.ProjectManagerBase.OutputManager.ErrorSurfaceRasterPath(m_ErrorSurfaceRow.DEMSurveyRow.Name, m_ErrorSurfaceRow.Name, True)
            Dim gErrorSurface As GCDConsoleLib.Raster = Nothing

            Select Case m_ErrorSurfaceRow.Type
                Case UniformErrorString
                    gErrorSurface = CreateUniformErrorSurface(m_ErrorSurfaceRow.GetMultiErrorPropertiesRows.First._Error, DEMRaster, sErrorSurfaceRasterPath)

                Case AssociatedsurfaceErrorType
                    ' Find the associated surface on which the error raster should be based.
                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_ErrorSurfaceRow.DEMSurveyRow.GetAssociatedSurfaceRows
                        If rAssoc.AssociatedSurfaceID = m_ErrorSurfaceRow.GetMultiErrorPropertiesRows.First.AssociatedSurfaceID Then
                            Dim sAssocPath As String = Project.ProjectManagerBase.GetAbsolutePath(rAssoc.Source)
                            gErrorSurface = CreateAssociatedErrorSurface(sAssocPath, DEMRaster, sErrorSurfaceRasterPath)
                            Exit For
                        End If
                    Next

                Case FISErrorType
                    CreateFISErrorSurface(m_ErrorSurfaceRow.GetFISInputsRows.First.FIS, DEMRaster, sErrorSurfaceRasterPath, False)

                Case MultipleErrorType
                    CreateMultiMethodErrorSurface(sErrorSurfaceRasterPath)

                Case Else
                    Dim ex As New Exception("Unhandled error surface type")
                    ex.Data("Type") = m_ErrorSurfaceRow.Type
                    Throw ex
            End Select

            gErrorSurface = New GCDConsoleLib.Raster(sErrorSurfaceRasterPath)
            m_ErrorSurfaceRow.Source = Project.ProjectManagerBase.GetRelativePath(sErrorSurfaceRasterPath)
            Project.ProjectManagerBase.save()

            ' Build raster pyramids
            If RasterPyramidManager.AutomaticallyBuildPyramids(RasterPyramidManager.PyramidRasterTypes.ErrorSurfaces) Then
                RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ErrorSurfaces, sErrorSurfaceRasterPath)
            End If

            Return gErrorSurface

        End Function

        Private Sub CreateMultiMethodErrorSurface(sOutputRasterPath As String)

            ' String to hold a concatenation of each of the survey method error rasters
            Dim sAllMethodRasters As String = String.Empty

            For Each aMethod As ProjectDS.MultiErrorPropertiesRow In m_ErrorSurfaceRow.GetMultiErrorPropertiesRows
                Dim sMethodRaster As String = Project.ProjectManagerBase.OutputManager.ErrorSurfarceMethodRasterPath(m_ErrorSurfaceRow.DEMSurveyRow.Name, m_ErrorSurfaceRow.Name, aMethod.Method, True)

                Dim sMethodMask As String = Project.ProjectManagerBase.OutputManager.ErrorSurfarceMethodRasterMaskPath(m_ErrorSurfaceRow.DEMSurveyRow.Name, m_ErrorSurfaceRow.Name, aMethod.Method, True)
                Dim gMaskRaster As GCDConsoleLib.Raster = CreateRasterMask(aMethod.Method, sMethodMask)

                Select Case aMethod.ErrorType

                    Case UniformErrorString
                        CreateUniformErrorSurface(aMethod._Error, gMaskRaster, sMethodRaster)

                    Case AssociatedsurfaceErrorType

                        ' Find the associated surface on which the error raster should be based.
                        For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_ErrorSurfaceRow.DEMSurveyRow.GetAssociatedSurfaceRows
                            If rAssoc.AssociatedSurfaceID = m_ErrorSurfaceRow.GetMultiErrorPropertiesRows.First.AssociatedSurfaceID Then
                                Dim sAssocPath As String = Project.ProjectManagerBase.GetAbsolutePath(rAssoc.Source)
                                CreateAssociatedErrorSurface(sAssocPath, gMaskRaster, sMethodRaster)
                                Exit For
                            End If
                        Next

                    Case Else
                        ' FIS. the error type field contains the name of the FIS rule file to be used.
                        CreateFISErrorSurface(aMethod.ErrorType, gMaskRaster, sMethodRaster, True)

                End Select
                sAllMethodRasters &= sMethodRaster & ";"
            Next

            If Not String.IsNullOrEmpty(sAllMethodRasters) Then
                Dim gDEM As New GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.Source))

                Dim sMosaicWithoutMask As String = WorkspaceManager.GetTempRaster("Mosaic")
                ' Call the Raster Manager mosaic function to blend the rasters together.
                Debug.Print(sAllMethodRasters)
                Dim eResult As External.RasterManagerOutputCodes = External.RasterManager.Mosaic(sAllMethodRasters, sMosaicWithoutMask, Project.ProjectManagerBase.GCDNARCError.ErrorString)
                If eResult <> External.RasterManagerOutputCodes.PROCESS_OK Then
                    Dim exInner As New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                    Dim ex As New Exception("Error mosaicing the raster.", exInner)
                    ex.Data("Input rasters") = sAllMethodRasters
                    ex.Data("Output raster") = sOutputRasterPath
                    ex.Data("Error Message") = Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString
                    Throw ex
                End If

                Try
                    eResult = External.Mask(sMosaicWithoutMask, gDEM.FilePath, sOutputRasterPath, Project.ProjectManagerBase.GCDNARCError.ErrorString)
                    If eResult <> External.RasterManagerOutputCodes.PROCESS_OK Then
                        Dim exInner As New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                        Throw New Exception("Error masking mosaic output", exInner)
                    End If
                Catch ex As Exception
                    ex.Data("Input raster") = sMosaicWithoutMask
                    ex.Data("Mask raster") = gDEM.FilePath
                    ex.Data("Output raster") = sOutputRasterPath
                    Throw
                End Try

                ' Update the GCD project with the path to the output raster
                m_ErrorSurfaceRow.Source = Project.ProjectManagerBase.GetRelativePath(sOutputRasterPath)
                m_ErrorSurfaceRow.Type = MultipleErrorType
            End If

        End Sub

        Private Function CreateUniformErrorSurface(fErrorValue As Double, gRasterMask As GCDConsoleLib.Raster, sOutputRasterPath As String) As GCDConsoleLib.Raster

            Dim gErrorSurface As GCDConsoleLib.Raster = Nothing

            Try
                ' Do the conditional geoprocessing.
                Dim eResult As External.GCDCoreOutputCodes = External.UniformError(gRasterMask.FilePath, sOutputRasterPath, fErrorValue, Project.ProjectManagerBase.GCDNARCError.ErrorString)
                If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                    Dim exInner As New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                    Dim ex As New Exception("Error producing uniform error surface.", exInner)
                    ex.Data("GCD Core return code") = eResult.ToString
                    Throw ex
                End If

                gErrorSurface = New GCDConsoleLib.Raster(sOutputRasterPath)

            Catch ex As Exception
                Dim ex2 As New Exception("Error producing the error surface raster.", ex)
                ex2.Data("Error Surface Name") = m_ErrorSurfaceRow.Name
                ex2.Data("DEM Raster") = m_ErrorSurfaceRow.DEMSurveyRow.Source
                ex2.Data("Error Surface Raster Path") = sOutputRasterPath
                ex2.Data("Error Surface Value") = fErrorValue.ToString
                Throw ex2
            End Try

            Return gErrorSurface

        End Function

        ''' <summary>
        ''' Create an error surface by copying an associated surface raster
        ''' </summary>
        ''' <param name="sAssociatedSurfacePath"></param>
        ''' <param name="gRasterMask"></param>
        ''' <param name="sOutputRasterPath"></param>
        ''' <returns></returns>
        ''' <remarks>Simply copy the associated surface to the desired output location for the error surface.
        ''' Note that the associated surface should already be concurrent with the DEM
        '''</remarks>
        Private Function CreateAssociatedErrorSurface(sAssociatedSurfacePath As String, gRasterMask As GCDConsoleLib.Raster, sOutputRasterPath As String) As GCDConsoleLib.Raster

            Dim gErrorSurface As GCDConsoleLib.Raster = Nothing

            Try
                Dim eResult As External.RasterManagerOutputCodes = External.Mask(sAssociatedSurfacePath, gRasterMask.FilePath, sOutputRasterPath, Project.ProjectManagerBase.GCDNARCError.ErrorString)
                If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                    Dim ex As New Exception("Error producing associated error surface.")
                    ex.Data("raster manager return code") = eResult.ToString
                    Throw ex
                End If

                gErrorSurface = New GCDConsoleLib.Raster(sOutputRasterPath)

            Catch ex As Exception
                Dim ex2 As New Exception("Error producing the error surface raster.", ex)
                ex2.Data("Error Surface Name") = m_ErrorSurfaceRow.Name
                ex2.Data("DEM Raster") = m_ErrorSurfaceRow.DEMSurveyRow.Source
                ex2.Data("Error Surface Raster Path") = sOutputRasterPath
                ex2.Data("Associated surface raster path") = sAssociatedSurfacePath
                Throw ex2
            End Try

            Return gErrorSurface

        End Function

        Private Sub CreateFISErrorSurface(sFISRuleDefinitionFileName As String, gReferenceRaster As GCDConsoleLib.Raster, sOutputRasterPath As String, bClipToMask As Boolean)

            ' Find the local path of the FIS rule file based on the library on this machine. Note
            ' could be imported project from another machine.
            Dim sFISFuleFilePath As String = String.Empty
            For Each fis As GCDLib.Core.ErrorCalculation.FIS.FISLibraryItem In Project.ProjectManagerUI.FISLibrary
                If String.Compare(fis.Name, sFISRuleDefinitionFileName, True) = 0 Then
                    sFISFuleFilePath = fis.FilePath
                    Exit For
                End If
            Next

            If String.IsNullOrEmpty(sFISFuleFilePath) Then
                Throw New Exception("The FIS rule file specified in the error surface calculation does not exist in the FIS Library.")
            End If

            If Not IO.File.Exists(sFISFuleFilePath) Then
                Dim ex As New Exception("The FIS rule file specified in the FIS Library does not exist on this computer.")
                ex.Data("FIS Rule Path") = sFISFuleFilePath
                Throw ex
            End If

            ' Setup FIS inputs. One for each associated surface input
            Dim sInputs As String = ""
            For Each FISInput As ProjectDS.FISInputsRow In m_ErrorSurfaceRow.GetFISInputsRows

                ' New muti-method FIS check. Make sure that the FIS input is for this FIS file
                If String.Compare(FISInput.FIS, sFISFuleFilePath, True) = 0 OrElse String.Compare(FISInput.FIS, IO.Path.GetFileNameWithoutExtension(sFISFuleFilePath)) = 0 Then

                    Dim sSQL As String = Project.ProjectManagerBase.ds.AssociatedSurface.DEMSurveyIDColumn.ColumnName & "=" & m_ErrorSurfaceRow.DEMSurveyRow.DEMSurveyID
                    sSQL &= " AND " & Project.ProjectManagerBase.ds.AssociatedSurface.NameColumn.ColumnName & "='" & FISInput.AssociatedSurface & "'"

                    Dim AssociatedSurface As ProjectDS.AssociatedSurfaceRow = Project.ProjectManagerBase.ds.AssociatedSurface.Select(sSQL).First

                    sInputs &= FISInput.FISInput & ";" & Project.ProjectManagerBase.GetAbsolutePath(AssociatedSurface.Source) & ";"
                End If
            Next
            If sInputs.Length > 0 Then
                sInputs = sInputs.Substring(0, sInputs.Length - 1)
            Else
                Throw New Exception("No FIS inputs to process.")
            End If

            Try
                ' When this method is being used for a multi-method error surface the reference
                ' raster is a mask (with cell value 1). Otherwise the reference raster is the full
                ' DEM
                Dim sFullFISRaster As String
                If bClipToMask Then
                    sFullFISRaster = WorkspaceManager.GetTempRaster("FIS")
                Else
                    sFullFISRaster = sOutputRasterPath
                End If

                Dim eResult As External.GCDCoreOutputCodes
                eResult = External.CreateFISError(DEMRaster.FilePath, sFISFuleFilePath, sInputs, sFullFISRaster,
                                                    Project.ProjectManagerBase.OutputManager.OutputDriver,
                                                    Project.ProjectManagerBase.OutputManager.NoData, Project.ProjectManagerBase.GCDNARCError.ErrorString)

                If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                    Dim exInner As New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                    Dim ex As New Exception("Error generating FIS error raster.", exInner)
                    ex.Data("Error Code") = eResult.ToString
                    Throw ex
                End If

                If bClipToMask Then
                    Throw New NotImplementedException
                    'GP.SpatialAnalyst.Raster_Calculator("""" & sFullFISRaster & """ * """ & gReferenceRaster.FullPath & """", sOutputRasterPath, gReferenceRaster)
                End If
            Catch ex As Exception
                Dim ex2 As New Exception("Error generating FIS error surface raster", ex)
                ex2.Data.Add("OutputPath", sOutputRasterPath)
                ex2.Data.Add("Reference Mask", gReferenceRaster.FilePath)
                Throw ex2
            End Try

        End Sub

        ''' <summary>
        ''' Create a raster for the features that represent just a single method type
        ''' </summary>
        ''' <param name="sMethodName">Name of the method for which a raster is needed</param>
        ''' <param name="sOutputRasterMaskPath">Output raster path</param>
        ''' <remarks>For some reason the rasters created do not have spatial reference defined.
        ''' So you need to project the output raster using the spatial reference from the DEM.
        ''' Also, later in this process this mask will be multiplied by the FIS raster to mask
        ''' it. So the value of the mask needs to be 1, not zero.</remarks>
        Private Function CreateRasterMask(ByVal sMethodName As String, ByVal sOutputRasterMaskPath As String) As GCDConsoleLib.Raster

            ' Copy features for just this method name (e.g. "total station")
            Dim OutShapefile As String = WorkspaceManager.GetTempShapeFile("Mask")
            Dim WhereClause As String = """" & m_ErrorSurfaceRow.DEMSurveyRow.MethodMaskField & """ = '" & sMethodName & "'"
            Dim gMaskFeatures As GCDConsoleLib.Vector = CopyFeatures(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.MethodMask), OutShapefile, WhereClause)

            Dim gDEM As New GCDConsoleLib.Raster(Project.ProjectManagerBase.GetAbsolutePath(m_ErrorSurfaceRow.DEMSurveyRow.Source))
            Dim gResult As GCDConsoleLib.Raster = Nothing
            Try
                Dim sTempRaster As String = WorkspaceManager.GetTempRaster("Mask")
                Throw New NotImplementedException
                '  GP.Conversion.PolygonToRaster_conversion(gMaskFeatures, "FID", sTempRaster, gDEM)

                ' The conversion does not assign a projection. do so now.
                Throw New NotImplementedException
                ' GP.DataManagement.DefineProjection(sTempRaster, gDEM.SpatialReference)

                ' The output value of the raster is the FID. Because there's typically just one feature this has
                ' a value of zero. We need a value of 1.
                Throw New NotImplementedException
                'GP.SpatialAnalyst.Raster_Calculator("""" & sTempRaster & """ >= 0", sOutputRasterMaskPath, gDEM)
                gResult = New GCDConsoleLib.Raster(sOutputRasterMaskPath)
            Catch ex As Exception
                Dim ex2 As New Exception("Error creating raster mask for survey method", ex)
                ex2.Data.Add("Survey Method", sMethodName)
                Throw ex2
            End Try

            Return gResult

        End Function

        ''' <summary>
        ''' Copy only the features for a specific survey method to a new feature class
        ''' </summary>
        ''' <param name="sMaskFeatureClass">Input polygon method mask feature class</param>
        ''' <param name="sOutputFeatureClass">Output polygon feature class</param>
        ''' <param name="sWhereClause">SQL Where clause to select just the feautres needed</param>
        ''' <remarks></remarks>
        Private Function CopyFeatures(sMaskFeatureClass As String, sOutputFeatureClass As String, Optional sWhereClause As String = "") As GCDConsoleLib.Vector

            ' TODO implement
            Throw New Exception("Not implemented")

            'Dim gPolygonMask As New GCDConsoleLib.Vector(sMaskFeatureClass)
            'Dim gOutput As GCDConsoleLib.Vector = GCDConsoleLib.Vector.CreateFeatureClass(sOutputFeatureClass, GISDataStructures.BasicGISTypes.Polygon, False, gPolygonMask.SpatialReference)

            'Dim pFBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = gOutput.FeatureClass.CreateFeatureBuffer
            'Dim pFOutputCursor As IFeatureCursor = gOutput.FeatureClass.Insert(True)
            'Dim pNewFeature As IFeature = pFBuffer

            ''filter featureclass

            'Dim pInputQueryFilter As IQueryFilter = New QueryFilter()
            'pInputQueryFilter.WhereClause = sWhereClause

            'Dim pFInputCursor As IFeatureCursor = gPolygonMask.FeatureClass.Search(pInputQueryFilter, True)
            'Dim pInputFeature As IFeature = pFInputCursor.NextFeature
            'Dim pShape As ESRI.ArcGIS.Geometry.IGeometry
            'Dim pZAware As ESRI.ArcGIS.Geometry.IZAware
            'Dim pMAware As ESRI.ArcGIS.Geometry.IMAware
            'While TypeOf pInputFeature Is IFeature
            '    pShape = pInputFeature.ShapeCopy
            '    pZAware = pShape
            '    If pZAware.ZAware Then
            '        pZAware.DropZs()
            '        pZAware.ZAware = False
            '    End If

            '    pMAware = pShape
            '    If pMAware.MAware Then
            '        pMAware.DropMs()
            '        pMAware.MAware = False
            '    End If

            '    pNewFeature.Shape = pShape
            '    pFOutputCursor.InsertFeature(pFBuffer)
            '    pInputFeature = pFInputCursor.NextFeature
            'End While
            'pFOutputCursor.Flush()
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(pNewFeature)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(pFBuffer)
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(pFOutputCursor)

            'Dim gResult As New GCDConsoleLib.Vector(sOutputFeatureClass)
            'Return gResult

        End Function

    End Class

End Namespace