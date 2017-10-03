Namespace GISCode.GCD.ErrorCalculation

    Public Class ErrorCalculationCHaMP_Console
        Inherits GISCode.GCD.ErrorCalculation.ErrorCalculationCHaMP2

        Private m_rDEMSurvey As ProjectDS.DEMSurveyRow

        Public Sub New(rDEMSurvey As ProjectDS.DEMSurveyRow, ByRef gDEM As GISDataStructures.Raster, diFISRuleFileDirectory As System.IO.DirectoryInfo)
            MyBase.New(gDEM, True, diFISRuleFileDirectory)

            m_rDEMSurvey = rDEMSurvey

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aVisit">The visit for which the error raster will be generated</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 1. Check and create each associated surface
        ''' 2. Based on the presence of each associated surface, generate a different fis error raster
        '''</remarks>
        Public Shadows Function Run(ByRef aVisit As CHaMP.Console.VisitRBTConsole) As ProjectDS.ErrorSurfaceRow

            ' Try and retrieve an existing error surface for this DEM
            Dim sErrorName As String = "FIS_" & m_rDEMSurvey.Name
            Dim sErrorRasterPath As String = GCD.GCDProject.ProjectManager.OutputManager.GetErrorRasterPath(m_rDEMSurvey.Name, sErrorName)
            Dim gRaster As GISDataStructures.RasterDirect = Nothing
            For Each aRow As ProjectDS.ErrorSurfaceRow In m_rDEMSurvey.GetErrorSurfaceRows
                If String.Compare(aRow.Name, sErrorName, True) = 0 Then
                    Return aRow
                End If
            Next

            Dim theTopo As CHaMP.Console.TopoDataConsole = DirectCast(aVisit.TopoData, CHaMP.Console.TopoDataConsole)

            ' Retrieve the grain size dictionary from the visit.
            Dim dChannelUnits As Dictionary(Of UInteger, CHaMP.ChannelUnit) = Nothing
            theTopo.GetChannelUnitsWithGrainSize(dChannelUnits)

            ' This is a dictionary of FIS input layers. The keys are the names of FIS inputs - as they are defined in the
            ' *.fis file, and the values are the corresponding raster layers for each input.
            Dim dFISInputLayers As New Dictionary(Of String, String)

            ' Creating each of these associated surface rows will retrieve any existing associated surfaces first
            ' and only generate a new raster if needed.
            Dim gcdSlopeRow As ProjectDS.AssociatedSurfaceRow = CreateFISInputAssociatedSurface(CHaMPFISInputTypes.Slope, theTopo, dChannelUnits, dFISInputLayers)
            Dim gcdPointDensityRow As ProjectDS.AssociatedSurfaceRow = CreateFISInputAssociatedSurface(CHaMPFISInputTypes.PointDensity, theTopo, dChannelUnits, dFISInputLayers)
            Dim gcdInterpolationErrorRow As ProjectDS.AssociatedSurfaceRow = CreateFISInputAssociatedSurface(CHaMPFISInputTypes.InterpolationError, theTopo, dChannelUnits, dFISInputLayers)
            Dim gcdRoughnessRow As ProjectDS.AssociatedSurfaceRow = CreateFISInputAssociatedSurface(CHaMPFISInputTypes.Roughness, theTopo, dChannelUnits, dFISInputLayers)
            Dim gcd3DPointQualityRow As ProjectDS.AssociatedSurfaceRow = CreateFISInputAssociatedSurface(CHaMPFISInputTypes.PointQuality3D, theTopo, dChannelUnits, dFISInputLayers)

            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(sErrorRasterPath))


            If TypeOf theTopo.ErrorRaster Is GISDataStructures.CHaMPRaster Then
                ExceptionHelper.UpdateStatus(String.Format("Reusing error surface {0} from the survey GDB for DEM survey {1}", sErrorName, m_rDEMSurvey.Name))

                ' The user has provided an error raster in the survey GDB. Make it concurrent and use it.
                Dim sManualInput As String = theTopo.ErrorRaster.FullPath
                If theTopo.ErrorRaster.GISDataStorageType = GISDataStructures.GISDataStorageTypes.FileGeodatase Then
                    ' The manual input is in a FileGDB, save a safe copy
                    sManualInput = theTopo.ErrorRaster.GetSafeCopy(GISDataStructures.Raster.RasterTypes.TIFF, True)
                End If
                RasterManager.Copy(sManualInput, sErrorRasterPath, DEM.CellSize, DEM.Extent.Left, DEM.Extent.Top, DEM.Rows, DEM.Columns, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString)
                gRaster = New GISDataStructures.RasterDirect(sErrorRasterPath)
            Else
                ' No user provided error raster. Create one using standard CHaMP FIS process.
                ExceptionHelper.UpdateStatus("Creating error surface " + sErrorName)
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Default FIS uses 3 Inputs: Slope, Point Density and Interpolation Error
                ' If Channel Units possess occular grain size estimates then use 4 input FIS: Slope, Point Density, Interpolation Error & Roughness
                ' If QAQC Points exist then use 5 input FIS: Slope Point Density, Interpolation Error, Roughness and 3D Point Quality
                '
                Dim sFISFileName As String = "CHaMP_TS_ZError_PD_SLPdeg_IntErr.fis"
                If TypeOf gcd3DPointQualityRow Is ProjectDS.AssociatedSurfaceRow Then
                    sFISFileName = "CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr.fis"
                    If TypeOf gcdRoughnessRow Is ProjectDS.AssociatedSurfaceRow Then
                        sFISFileName = "CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr.fis"
                    End If
                End If

                sFISFileName = IO.Path.Combine(IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location), sFISFileName)
                Dim theFISRuleFile As New GISCode.GCD.ErrorCalculation.FISRuleFile(sFISFileName)
                ExceptionHelper.UpdateStatus("Generating FIS error surface with " & sFISFileName)
                MyBase.Run(sErrorRasterPath, "")
            End If

            Dim gcdErrorRow As ProjectDS.ErrorSurfaceRow = GCD.GCDProject.ProjectManager.ds.ErrorSurface.AddErrorSurfaceRow(sErrorName, "FIS Error Raster", GCD.GCDProject.ProjectManager.GetRelativePath(sErrorRasterPath), m_rDEMSurvey)

            ' Store the FIS properties in the GCD project XML. Note, for this to work, the associated surfaces must
            ' have exactly the same name as the FIS input names (i.e. no extra spaces or formatting)
            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                GCD.GCDProject.ProjectManager.ds.FISInputs.AddFISInputsRow(gcdErrorRow, rAssoc.Name, rAssoc.Name, "")
            Next

            Return gcdErrorRow

        End Function

        ''' <summary>
        ''' Creates the associated surface for a particular FIS input and adds the associated surface row to the GCD project
        ''' </summary>
        ''' <param name="eType">Enumeration of the FIS input type being created</param>
        ''' <param name="gTopoData">CHaMP visit topo data</param>
        ''' <param name="dFISInputLayers">Dictionary FIS inputs. The associated surface raster will be added if it is created successfully</param>
        ''' <returns>The GCD project associated row if successful. Nothing if something goes wrong</returns>
        ''' <remarks></remarks>
        Private Function CreateFISInputAssociatedSurface(eType As CHaMPFISInputTypes, ByRef gTopoData As GISCode.CHaMP.Console.TopoDataConsole, ByRef dChannelUnits As Dictionary(Of UInteger, CHaMP.ChannelUnit), ByRef dFISInputLayers As Dictionary(Of String, String)) As ProjectDS.AssociatedSurfaceRow

            Dim rResult As ProjectDS.AssociatedSurfaceRow = Nothing
            Dim sAssocSurfaceType As String = ""

            Dim sFISInputName As String = ""
            Select Case eType
                Case CHaMPFISInputTypes.PointDensity
                    sFISInputName = "PointDensity"
                    sAssocSurfaceType = "Point Density"

                Case CHaMPFISInputTypes.Slope
                    sFISInputName = "Slope"
                    sAssocSurfaceType = "Slope Degrees"

                Case CHaMPFISInputTypes.InterpolationError
                    sFISInputName = "InterpolationError"
                    sAssocSurfaceType = "Interpolation Error"

                Case CHaMPFISInputTypes.Roughness
                    sFISInputName = "Roughness"
                    sAssocSurfaceType = "Roughness"

                Case CHaMPFISInputTypes.PointQuality3D
                    sFISInputName = "PointQuality3D"
                    sAssocSurfaceType = "3D Point Quality"

                Case Else
                    sAssocSurfaceType = "[Undefined]"
                    Throw New Exception("Unhandled associated surface type " & eType.ToString)
            End Select

            ' Skip if the associated surface already exists.
            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                If String.Compare(rAssoc.Name, sFISInputName, True) = 0 Then
                    Return rAssoc
                End If
            Next

            ' Get the path for the associated surface raster and make sure the containing directory is created
            Dim sAssociatedSurfaceRasterPath As String = GCD.GCDProject.ProjectManager.OutputManager.AssociatedSurfaceRasterPath(m_rDEMSurvey.Name, sFISInputName)
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(sAssociatedSurfaceRasterPath))

            Try
                Dim rFISInputRaster As GCD.ErrorCalculation.FISInputRaster = Nothing

                Select Case eType

                    Case CHaMPFISInputTypes.PointDensity
                        rFISInputRaster = GeneratePointDensity(sAssociatedSurfaceRasterPath, gTopoData.TopoPoints, gTopoData.EdgeOfWaterPoints)

                    Case CHaMPFISInputTypes.Slope
                        rFISInputRaster = GenerateSlope(sAssociatedSurfaceRasterPath, False)

                    Case CHaMPFISInputTypes.InterpolationError
                        rFISInputRaster = GenerateInterpolationError(sAssociatedSurfaceRasterPath, gTopoData.TopoTIN, gTopoData.SurveyExtent, False)

                    Case CHaMPFISInputTypes.Roughness

                        rFISInputRaster = GenerateRoughness(sAssociatedSurfaceRasterPath, gTopoData.ChannelUnits, dChannelUnits, gTopoData.Bankfull.WaterExtent, gTopoData.TopoTIN, gTopoData.SurveyExtent, gTopoData.Wetted.WaterExtent)

                    Case CHaMPFISInputTypes.PointQuality3D

                        If gTopoData.TopoPoints.FindField(GISDataStructures.TopoPoints.m_sPointQualityField) > 0 Then
                            rFISInputRaster = Generate3DPointQuality(sAssociatedSurfaceRasterPath, gTopoData.TopoPoints, gTopoData.SurveyExtent)
                        End If

                End Select

                If GISDataStructures.Raster.Exists(sAssociatedSurfaceRasterPath) Then
                    rResult = GCD.GCDProject.ProjectManager.ds.AssociatedSurface.AddAssociatedSurfaceRow(m_rDEMSurvey, sAssocSurfaceType, GCD.GCDProject.ProjectManager.GetRelativePath(sAssociatedSurfaceRasterPath), sAssocSurfaceType, "")
                End If

            Catch ex As Exception
                Dim ex2 As New GISException(GISException.ErrorTypes.Warning, "Failed to generate " & eType.ToString & " associated surface raster.", ex)
                ExceptionHelper.HandleException(ex2, gTopoData.VisitID)
                ' Clean up by removing the directory
                IO.Directory.Delete(IO.Path.GetDirectoryName(sAssociatedSurfaceRasterPath), True)
                rResult = Nothing
            End Try

            ' Only add the input to the dictionary if the associated surface was generated successfully
            If TypeOf rResult Is ProjectDS.AssociatedSurfaceRow Then
                dFISInputLayers.Add(sFISInputName, sAssociatedSurfaceRasterPath)
            End If

            Return rResult

        End Function

    End Class

End Namespace
