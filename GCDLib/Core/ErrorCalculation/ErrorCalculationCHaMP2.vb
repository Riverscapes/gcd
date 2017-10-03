Namespace GISCode.GCD.ErrorCalculation

    Public Class ErrorCalculationCHaMP2

        Public Enum CHaMPFISInputTypes
            Slope
            PointDensity
            InterpolationError
            Roughness
            PointQuality3D
        End Enum

        Private m_rDEM As GISDataStructures.RasterDirect
        Private m_diOutputWorkspace As System.IO.DirectoryInfo

        Private m_bReUseExistingRasters As Boolean

        Private m_dFISInputRasters As Dictionary(Of AssociatedSurfaceTypes, FISInputRaster)
        Private m_dFISInputRuleFiles As Dictionary(Of Byte, System.IO.FileInfo)

        Private Const m_sFISRuleFile_3Inputs As String = "CHaMP_TS_ZError_PD_SLPdeg_IntErr"
        Private Const m_sFISRuleFile_4Inputs As String = "CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr"
        Private Const m_sFISRuleFile_5Inputs As String = "CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr"

        Private m_Error As NARCError

        Protected ReadOnly Property DEM As GISDataStructures.RasterDirect
            Get
                Return m_rDEM
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="gDEM"></param>
        ''' <param name="bReUseExistingRasters">Console passes True and copies existing GDB rasters to file system. Toolbar passes False which deletes existing rasters and recreates.</param>
        ''' <param name="diFISRuleFileDirectory"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef gDEM As GISDataStructures.Raster, bReUseExistingRasters As Boolean, diFISRuleFileDirectory As System.IO.DirectoryInfo)

            Dim sSafeRaster As String = gDEM.FullPath
            If gDEM.GISDataStorageType = GISDataStructures.GISDataStorageTypes.FileGeodatase Then
                sSafeRaster = WorkspaceManager.GetTempRaster("DEM")
                GP.DataManagement.CopyRaster(gDEM.FullPath, sSafeRaster)
            End If
            m_rDEM = New GISDataStructures.RasterDirect(sSafeRaster)


            m_bReUseExistingRasters = bReUseExistingRasters

            If Not diFISRuleFileDirectory.Exists Then
                Dim ex As New Exception("The directory containing the FIS rule files does not exist.")
                ex.Data("FIS rule file directory") = diFISRuleFileDirectory.FullName
                Throw ex
            End If

            ' Add paths to each of the FIS rule files
            AddFISFuleFile(diFISRuleFileDirectory, m_sFISRuleFile_3Inputs, 3)
            AddFISFuleFile(diFISRuleFileDirectory, m_sFISRuleFile_4Inputs, 4)
            AddFISFuleFile(diFISRuleFileDirectory, m_sFISRuleFile_5Inputs, 5)

            ' Prepare the dictionary of FIS input rasters
            m_dFISInputRasters = New Dictionary(Of AssociatedSurfaceTypes, FISInputRaster)

            ' Prepare the error object to retrieve error messages
            m_Error = New NARCError

        End Sub

        Private Sub AddFISFuleFile(sDirectory As System.IO.DirectoryInfo, sFileName As String, nNumInputs As Byte)

            If m_dFISInputRuleFiles Is Nothing Then
                m_dFISInputRuleFiles = New Dictionary(Of Byte, System.IO.FileInfo)
            End If

            Dim sFilePath = System.IO.Path.Combine(sDirectory.FullName, sFileName)
            sFilePath = System.IO.Path.ChangeExtension(sFilePath, "fis")

            If Not System.IO.File.Exists(sFilePath) Then
                Dim ex As New Exception("The FIS rule file path does not exist.")
                ex.Data("File Path") = sFilePath
                Throw ex
            End If

            m_dFISInputRuleFiles.Add(nNumInputs, New System.IO.FileInfo(sFilePath))
        End Sub

        ''' <summary>
        ''' Each FIS generate method should call this routine to check if the raster exists already and whether to use it.
        ''' </summary>
        ''' <param name="sRasterPath"></param>
        ''' <param name="eType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function RasterNeedsToBeGenerated(sRasterPath As String, eType As AssociatedSurfaceTypes)

            If GISDataStructures.Raster.Exists(sRasterPath) Then

                ' Toolbar should set ReUse to False to force delete and recreate.
                ' Console should set ReUse to True to leverage rasters in GDB
                If Not m_bReUseExistingRasters Then
                    GP.DataManagement.Delete(sRasterPath)
                End If

                ' The raster already exists. Remove the item from the dictionary
                ' and it will be re-added below to ensure the correct path.
                If m_dFISInputRasters.ContainsKey(eType) Then
                    m_dFISInputRasters.Remove(eType)
                End If
            End If

            Return Not GISDataStructures.Raster.Exists(sRasterPath)

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sRasterPath"></param>
        ''' <param name="bObtainExtension">Console applications cannot use the ExtensionManager that the ObtainExtension method uses.
        ''' Instead they use the AOInit.CheckoutExtension method to obtain spatial analyst. Therefore this method should only do the extension
        ''' check in UI products.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GenerateSlope(sRasterPath As String, bObtainExtension As Boolean) As FISInputRaster

            If RasterNeedsToBeGenerated(sRasterPath, AssociatedSurfaceTypes.Slope) Then
                GISCode.GP.SpatialAnalyst.Slope(m_rDEM, sRasterPath, "DEGREE", bObtainExtension)
            End If

            Dim rRaster As New FISInputRaster(sRasterPath, AssociatedSurfaceTypes.Slope)
            m_dFISInputRasters.Add(rRaster.AssociatedSurfaceType, rRaster)
            Return rRaster

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sRasterPath"></param>
        ''' <param name="gSurveyPoints"></param>
        ''' <param name="gEoW"></param>
        ''' <param name="fPointDensityKernel"></param>
        ''' <returns></returns>
        ''' <remarks>1 Jun 2016. Realized that this method only used to take Topo Points and that it should 
        ''' also take the edge of water points. New code to merge these 2 feature classes before passing in
        ''' the merge result as the input to point density.</remarks>
        Public Function GeneratePointDensity(sRasterPath As String, gSurveyPoints As GISDataStructures.TopoPoints, gEoW As GISDataStructures.EdgeOfWaterPoints, Optional fPointDensityKernel As Single = 5) As FISInputRaster

            If RasterNeedsToBeGenerated(sRasterPath, AssociatedSurfaceTypes.PointDensity) Then

                Dim sTopoPoints As String = gSurveyPoints.FullPath
                If sTopoPoints.Contains(" ") Then
                    sTopoPoints = String.Format("'{0}'", sTopoPoints)
                End If

                Dim sEoW As String = gEoW.FullPath
                If sEoW.Contains(" ") Then
                    sEoW = String.Format("'{1}'", sEoW)
                End If

                ' Merge the topo points with the edge of water points
                Dim sInputs As String = String.Format("{0};{1}", sTopoPoints, sEoW)
                Dim sTempPoints As String = WorkspaceManager.GetTempShapeFile("SurveyPoints")
                GP.DataManagement.Merge(sInputs, sTempPoints)
                Dim gTempPoints As New GISDataStructures.PointDataSource(sTempPoints)

                Dim sTempRaster As String = WorkspaceManager.GetTempRaster("PDensity")
                GISCode.GP.SpatialAnalyst.PointDensity(gTempPoints, m_rDEM, sTempRaster, String.Format("Circle {0} Map", fPointDensityKernel), "SQUARE_METERS")
                Dim sMaskedRaster As String = sRasterPath
                If GISDataStructures.IsFileGeodatabase(sRasterPath) Then
                    sMaskedRaster = WorkspaceManager.GetTempRaster("PDMasked")
                End If

                RasterManager.Mask(sTempRaster, m_rDEM.FullPath, sMaskedRaster, m_Error.ErrorString)

                If GISDataStructures.IsFileGeodatabase(sRasterPath) Then
                    GP.DataManagement.CopyRaster(sMaskedRaster, sRasterPath)
                End If
            End If

            Dim rRaster As New FISInputRaster(sRasterPath, AssociatedSurfaceTypes.PointDensity)
            m_dFISInputRasters.Add(rRaster.AssociatedSurfaceType, rRaster)
            Return rRaster

        End Function

        ''' <summary>
        ''' 2016 
        ''' in channel roughness is now calculated from d84 and is extended to fill the bankfull polygon
        ''' out of channel roughness is uniform and calculated by solving for d50 in the Manning's coefficient equation
        ''' </summary>
        ''' <param name="sRasterPath"></param>
        ''' <param name="gChannelUnits"></param>
        ''' <param name="dGrainSizeDictionary"></param>
        ''' <param name="gBankfullPolygon"></param>
        ''' <param name="gTopoTin"></param>
        ''' <param name="gSurveyExtent"></param>
        ''' <param name="gWaterExtent"></param>
        ''' <param name="lMessages"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GenerateRoughness(ByVal sRasterPath As String,
                                          ByRef gChannelUnits As GISDataStructures.PolygonDataSource,
                                          ByRef dGrainSizeDictionary As Dictionary(Of UInteger, CHaMP.ChannelUnit),
                                          ByVal gBankfullPolygon As GISDataStructures.WaterExtentPolygon,
                                          ByVal gTopoTin As GISDataStructures.TINDataSource,
                                          ByVal gSurveyExtent As GISDataStructures.PolygonDataSource,
                                          ByVal gWaterExtent As GISDataStructures.WaterExtentPolygon,
                                          Optional ByRef lMessages As List(Of String) = Nothing) As FISInputRaster

            'Initialize lMessages to avoid issues with using it is nothing
            If lMessages Is Nothing Then
                lMessages = New List(Of String)
            End If

            If RasterNeedsToBeGenerated(sRasterPath, AssociatedSurfaceTypes.Roughness) Then

                Dim gTempInChannel As GISDataStructures.Raster = GenerateInChannelRoughness(gChannelUnits,
                                                                                            dGrainSizeDictionary,
                                                                                            gBankfullPolygon,
                                                                                            lMessages)
                Dim pOC_Roughness As ErrorGenerator_OutChannelRoughness = New ErrorGenerator_OutChannelRoughness(gSurveyExtent, gBankfullPolygon, m_rDEM)
                Dim gTempOutOfChannel As GISDataStructures.Raster = pOC_Roughness.Execute()
                Dim sRasterList As String = gTempInChannel.FullPath & ";" & gTempOutOfChannel.FullPath

                Dim sMosaic As String = sRasterPath
                If GISDataStructures.IsFileGeodatabase(sRasterPath) Then
                    sMosaic = WorkspaceManager.GetTempRaster("Rough")
                End If

                'The pixel depth of the ICRoughness needs to be checked in case it is all integer values, 
                'ESRI converts these to 16 bit rasters which if it is fed as the first raster into the mosaic operation 
                'will force the mosaic operation to output a 16 bit raster

                'Create a list of pixel types to check against in CheckPixelType Function, we are checking to confirm that the raster is indeed a double or a float
                Dim lrstPixelType As New List(Of ESRI.ArcGIS.Geodatabase.rstPixelType)(New ESRI.ArcGIS.Geodatabase.rstPixelType() {ESRI.ArcGIS.Geodatabase.rstPixelType.PT_DOUBLE, ESRI.ArcGIS.Geodatabase.rstPixelType.PT_FLOAT})

                If Not gTempInChannel.CheckPixelType(lrstPixelType) Then
                    Dim sCorrectPixelRasterPath As String = WorkspaceManager.GetTempRaster("PixelFix")
                    Dim bCopiedRaster As GISDataStructures.Raster = gTempInChannel.CreateCopyWithGivenPixelType(sCorrectPixelRasterPath, ESRI.ArcGIS.Geodatabase.rstPixelType.PT_FLOAT)
                    If bCopiedRaster IsNot Nothing Then
                        If GISDataStructures.Raster.Exists(bCopiedRaster.FullPath) Then
                            'overwrite the sRasterList to now use the raster with the corret pixel type
                            sRasterList = sCorrectPixelRasterPath & ";" & gTempOutOfChannel.FullPath
                        End If
                    Else
                        Throw New Exception("An error occured while correcting the raster's pixel type.")
                    End If
                End If

                RasterManager.Mosaic(sRasterList, sMosaic, m_Error.ErrorString)

                Dim sTempSetNullRaster As String = WorkspaceManager.GetTempRaster("RoughNull")
                GP.SpatialAnalyst.SetNull(sMosaic, sMosaic, sTempSetNullRaster)
                GP.DataManagement.CopyRaster(sTempSetNullRaster, sRasterPath)

            End If

            Dim rRaster = New FISInputRaster(sRasterPath, AssociatedSurfaceTypes.Roughness)
            m_dFISInputRasters.Add(rRaster.AssociatedSurfaceType, rRaster)

            Return rRaster

        End Function

        ''' <summary>
        ''' In channel roughness used for 2016
        ''' A roughness height is now calculated from the D84 value and the extent of raster is extended to the bankfull polygon
        ''' </summary>
        ''' <param name="gChannelUnits"></param>
        ''' <param name="dGrainSizeDictionary"></param>
        ''' <param name="gBankfullPolygon"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GenerateInChannelRoughness(gChannelUnits As GISDataStructures.PolygonDataSource,
                                                    ByRef dGrainSizeDictionary As Dictionary(Of UInteger, CHaMP.ChannelUnit),
                                                    ByVal gBankfullPolygon As GISDataStructures.WaterExtentPolygon,
                                                    Optional ByRef lMessages As List(Of String) = Nothing) As GISDataStructures.Raster

            ' In channel roughness
            Dim sTempInChannel As String = WorkspaceManager.GetTempRaster("ICRoughness")
            Dim icRoughness As New ErrorGenerator_InChannelRoughness(gChannelUnits, dGrainSizeDictionary, gBankfullPolygon, m_rDEM)

            'Initialize lMessages to avoid issues with using it is nothing
            If lMessages Is Nothing Then
                lMessages = New List(Of String)
            End If
            Dim gResult As GISDataStructures.Raster = icRoughness.Execute(sTempInChannel,
                                                                          GISCode.GISDataStructures.ChannelUnits.ChannelUnitNumberField,
                                                                          lMessages)

            If GISDataStructures.Raster.Exists(gResult.FullPath) Then
                Return gResult
            Else
                Throw New Exception(String.Format("The in channel roughness raster {0} was not created.", sTempInChannel))
                Exit Function
            End If

        End Function

        Public Function Generate3DPointQuality(sRasterPath As String, gTopoPoints As GISDataStructures.TopoPoints, gSurveyExtent As GISDataStructures.PolygonDataSource) As FISInputRaster

            If RasterNeedsToBeGenerated(sRasterPath, AssociatedSurfaceTypes.PointQuality3D) Then
                Debug.Assert(gTopoPoints.HasPointQualityField, String.Format("The topo points feature class must have the field '{0}'.", GISDataStructures.TopoPoints.m_sPointQualityField))
                Dim pq As New SurfaceInterpolator(gTopoPoints, GISCode.GISDataStructures.TopoPoints.m_sPointQualityField, gSurveyExtent, m_rDEM)
                Dim sTempTIN As String = WorkspaceManager.GetSafeDirectoryPath("TempTopo")
                pq.ExecuteRBT(False, sTempTIN, sRasterPath)
            End If

            Dim rRaster = New FISInputRaster(sRasterPath, AssociatedSurfaceTypes.PointQuality3D)
            m_dFISInputRasters.Add(rRaster.AssociatedSurfaceType, rRaster)
            Return rRaster

        End Function

        Public Function GenerateInterpolationError(sRasterPath As String, gTopoTIN As GISDataStructures.TINDataSource, gSurveyExtent As GISDataStructures.PolygonDataSource, bObtainExtension As Boolean) As FISInputRaster

            If RasterNeedsToBeGenerated(sRasterPath, AssociatedSurfaceTypes.InerpolationError) Then

                Dim sTempInterp As String = WorkspaceManager.GetTempRaster("Interp")
                Dim interp As New ErrorGenerator_InterpolationError(gTopoTIN, gSurveyExtent, m_rDEM, bObtainExtension)
                interp.Execute(sTempInterp)
                GP.DataManagement.CopyRaster(sTempInterp, sRasterPath)
            End If

            Dim rRaster = New FISInputRaster(sRasterPath, AssociatedSurfaceTypes.InerpolationError)
            m_dFISInputRasters.Add(rRaster.AssociatedSurfaceType, rRaster)
            Return rRaster

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sRasterFilePath"></param>
        ''' <param name="sProcessingMessage">Output: a string describing the FIS file and rules used. For use in a log table or file</param>
        ''' <remarks></remarks>
        Public Sub Run(sRasterFilePath As String, ByRef sProcessingMessage As String)

            ' Ensure that the minimum three inputs required are present

            If Not TypeOf m_dFISInputRasters(AssociatedSurfaceTypes.Slope) Is FISInputRaster Then
                Throw New Exception("You must generate a slope input raster.")
            End If

            If Not TypeOf m_dFISInputRasters(AssociatedSurfaceTypes.PointDensity) Is FISInputRaster Then
                Throw New Exception("You must generate a point density input raster.")
            End If

            If Not TypeOf m_dFISInputRasters(AssociatedSurfaceTypes.InerpolationError) Is FISInputRaster Then
                Throw New Exception("You must generate an interpolation error input raster.")
            End If

            ' Determine the correct FIS rule file to use. Start off using the three input version
            Dim sFISRuleFilePath As System.IO.FileInfo = m_dFISInputRuleFiles(3)
            If m_dFISInputRasters.ContainsKey(AssociatedSurfaceTypes.PointQuality3D) Then
                ' Roughness exists, could potentially do a 4 input FIS
                sFISRuleFilePath = m_dFISInputRuleFiles(4)

                If m_dFISInputRasters.ContainsKey(AssociatedSurfaceTypes.Roughness) Then
                    ' 3D Point Quality raster exists, can do a 5 input FIS
                    sFISRuleFilePath = m_dFISInputRuleFiles(5)
                End If
            End If

            sProcessingMessage = String.Format("{0} input FIS error raster. Inputs", m_dFISInputRasters.Count)
            For Each eType As AssociatedSurfaceTypes In m_dFISInputRasters.Keys
                sProcessingMessage &= String.Format(", {0}", eType.ToString)
            Next
            ExceptionHelper.UpdateStatus(sProcessingMessage)

            ' Actually build the error raster here. Note, the raster could be in file GDB
            CreateFISError(sFISRuleFilePath, sRasterFilePath)

        End Sub

        ''' <summary>
        ''' Create an error grid using fuzzy inference system.
        ''' </summary>
        ''' <param name="fiFISRuleFilePath">FIS rule definition file path</param>
        ''' <param name="outErrorRaster">Desired name of the resultant error grid</param>
        ''' <remarks>All FIS rules must have a corresponding layer defined in the inInputLayerPaths dictionary or an error is thrown.</remarks>
        Private Function CreateFISError(fiFISRuleFilePath As System.IO.FileInfo, ByVal outErrorRaster As String) As GISDataStructures.Raster

            Dim theFISRuleFile As New FISRuleFile(fiFISRuleFilePath.FullName)
            '
            ' Loop through all the FIS rules defined in the FIS file. Each rule needs an input raster
            ' specified in the input layer dictionary. Any failure to load an input or if an input
            ' is not specified causes an error to be thrown.
            '
            Dim sInputNamesAndRasterPaths As String = ""
            For Each sInputName As String In theFISRuleFile.FISInputs
                Dim bInputExists As Boolean = False

                For Each aFISInputRaster As FISInputRaster In m_dFISInputRasters.Values
                    If String.Compare(aFISInputRaster.FISInputName, sInputName, False) = 0 Then

                        Dim sFISInputRasterFilePath As String = aFISInputRaster.RasterPath
                        If GISDataStructures.IsFileGeodatabase(sFISInputRasterFilePath) Then
                            ' The actual raster used must be file-based
                            sFISInputRasterFilePath = WorkspaceManager.GetTempRaster(System.IO.Path.GetFileNameWithoutExtension(sFISInputRasterFilePath))
                            GP.DataManagement.CopyRaster(aFISInputRaster.RasterPath, sFISInputRasterFilePath)
                        End If

                        bInputExists = True
                        sInputNamesAndRasterPaths &= String.Format("{0};{1};", sInputName, sFISInputRasterFilePath)
                        Exit For
                    End If
                Next

                If Not bInputExists Then
                    Dim ex As New Exception("Failed to add FIS input layer")
                    ex.Data("FIS Rule Name") = theFISRuleFile.RuleFilePath
                    ex.Data("FIS input name") = sInputName
                    Throw ex
                End If
            Next

            ' Remove trailing semi-colon from input and raster pair list
            If sInputNamesAndRasterPaths.Length > 0 Then
                sInputNamesAndRasterPaths = sInputNamesAndRasterPaths.Substring(0, sInputNamesAndRasterPaths.Length - 1)
            Else
                Throw New Exception("No FIS inputs to process.")
            End If
            '
            ' GDAL cannot handle file geodatabases. If the final output location is a file geodatabase then create a temporary copy 
            ' of the error grid in the TempWorkspace then copy it to the final location.
            '
            Dim sTempOutputRaster As String = outErrorRaster
            If GISDataStructures.IsFileGeodatabase(outErrorRaster) Then
                sTempOutputRaster = WorkspaceManager.GetTempRaster("error.tif")
            End If

            Try
                Debug.WriteLine("Executing CHaMP Error Surface FIS")
                Debug.WriteLine("FIS Rule File: " & theFISRuleFile.RuleFilePath)
                Debug.WriteLine("DEM: " & m_rDEM.FullPath)
                Debug.WriteLine("Inputs: " & sInputNamesAndRasterPaths)
                Debug.WriteLine("Output: " & sTempOutputRaster)
                Dim eResult As GCDCore.GCDCoreOutputCodes = GCDCore.CreateFISError(m_rDEM.FullPath, theFISRuleFile.RuleFilePath, sInputNamesAndRasterPaths, sTempOutputRaster, "GTiff", -9999, m_Error.ErrorString)

                If eResult <> GCDCoreOutputCodes.PROCESS_OK Then
                    Dim ex As New Exception("Error producing error surface.")
                    ex.Data("Return code") = eResult.ToString
                    ex.Data("DEM") = m_rDEM.FullPath
                    ex.Data("Rule File") = theFISRuleFile.RuleFilePath
                    ex.Data("Inputs and rasters") = sInputNamesAndRasterPaths
                    ex.Data("Output Raster") = sTempOutputRaster
                    Throw ex
                End If

            Catch ex As Exception
                For Each aFISRaster In m_dFISInputRasters
                    ex.Data("FIS Input") = aFISRaster.ToString
                Next
                ex.Data("outErrorRaster") = outErrorRaster
                Throw
            End Try
            '
            ' Copy the error grid to the desired final location.
            '
            If GISDataStructures.IsFileGeodatabase(outErrorRaster) Then
                GISCode.GP.DataManagement.CopyRaster(sTempOutputRaster, outErrorRaster)
            End If

            Dim gRaster As New GISDataStructures.Raster(outErrorRaster)
            Return gRaster

        End Function

        Public Function LoadExistingAssociatedSurfaceRasters(dFileGDB As System.IO.DirectoryInfo) As Integer

            m_dFISInputRasters = New Dictionary(Of AssociatedSurfaceTypes, FISInputRaster)

            Dim sDataSource = System.IO.Path.Combine(dFileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerSlopeRaster)
            If (GISDataStructures.Raster.Exists(sDataSource)) Then
                m_dFISInputRasters.Add(AssociatedSurfaceTypes.Slope, New GCD.ErrorCalculation.FISInputRaster(sDataSource, AssociatedSurfaceTypes.Slope))
            End If

            sDataSource = System.IO.Path.Combine(dFileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerPointDensity)
            If (GISDataStructures.Raster.Exists(sDataSource)) Then
                m_dFISInputRasters.Add(AssociatedSurfaceTypes.PointDensity, New GCD.ErrorCalculation.FISInputRaster(sDataSource, AssociatedSurfaceTypes.PointDensity))
            End If

            sDataSource = System.IO.Path.Combine(dFileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerInterpError)
            If (GISDataStructures.Raster.Exists(sDataSource)) Then
                m_dFISInputRasters.Add(AssociatedSurfaceTypes.InerpolationError, New GCD.ErrorCalculation.FISInputRaster(sDataSource, AssociatedSurfaceTypes.InerpolationError))
            End If

            sDataSource = System.IO.Path.Combine(dFileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerRoughness)
            If (GISDataStructures.Raster.Exists(sDataSource)) Then
                m_dFISInputRasters.Add(AssociatedSurfaceTypes.Roughness, New GCD.ErrorCalculation.FISInputRaster(sDataSource, AssociatedSurfaceTypes.Roughness))
            End If

            sDataSource = System.IO.Path.Combine(dFileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayer3DPointQuality)
            If (GISDataStructures.Raster.Exists(sDataSource)) Then
                m_dFISInputRasters.Add(AssociatedSurfaceTypes.PointQuality3D, New GCD.ErrorCalculation.FISInputRaster(sDataSource, AssociatedSurfaceTypes.PointQuality3D))
            End If

            Return m_dFISInputRasters.Count

        End Function

    End Class

End Namespace