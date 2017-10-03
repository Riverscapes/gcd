#Region "Code Comments"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'       Author: Philip Bailey, Nick Ochoski, & Frank Poulsen
'               ESSA Software Ltd.
'               1765 W 8th Avenue
'               Vancouver, BC, Canada V6J 5C6
'     
'     Copyright: (C) 2011 by ESSA technologies Ltd.
'                This software is subject to copyright protection under the       
'                laws of Canada and other countries.
'
'  Date Created: 14 January 2011
'
'   Description: 
'
#End Region

#Region "Imports"
'Imports OSGeo.GDAL
Imports System.IO

#End Region

Namespace GISCode.GCD

    Public Class UncertaintyAnalysisClass

#Region "Members"

        Public m_sDoDRasterThresholded As String
        Public m_sDoDRasterRaw As String
        Public oldSurvey As String
        Public oldError As String
        Public newSurvey As String
        Public newError As String
        Public m_fProbability As Double
        Public m_bBaysian As Boolean
        Public RawHist As String
        Public ProbabilityHist As String
        Public RawSource As String
        Public filterSize As Integer = 5
        Public minNbrPercent As Integer = 60
        Public maxNbrPercent As Integer = 100

#End Region

#Region "Properties"

        Public Property Probability As Double
            Get
                Return m_fProbability
            End Get
            Set(value As Double)
                m_fProbability = value
            End Set
        End Property

#End Region


        'Public Sub Process(ByVal UncertaintyID As Integer)
        '    Dim bSuccess As Boolean

        '    'Init
        '    Gdal.AllRegister()

        '    'Setup inputs
        '    Dim UncertaintyProperties As New UncertaintyProperties(UncertaintyID)
        '    Dim oldDemFn As String = UncertaintyProperties.OldSurveySource
        '    Dim newDemFn As String = UncertaintyProperties.NewSurveySource
        '    Dim outputDriver As String = GCDOptions.outputDriver
        '    Dim noData As Integer = GCDOptions.NoData

        '    Dim UncertaintyPropertiesRow As ProjectDS.UncertaintyPropertiesRow = ProjectManager.ds.UncertaintyProperties.FindByUncertaintyID(UncertaintyID)
        '    Dim DoDID As Integer = UncertaintyPropertiesRow.DoDID
        '    Dim DoDRow As ProjectDS.DoDTableRow = ProjectManager.ds.DoDTable.FindByDoDID(DoDID)
        '    Dim DoDName As String = DoDRow.Name
        '    Dim UncertaintyName As String = UncertaintyPropertiesRow.Name

        '    'setup outputs
        '    Dim LayerSource As String
        '    Dim HistogramSource As String
        '    Dim ExcelSource As String

        '    Dim workspace As String = My.Settings.OutputDirectory
        '    If Not workspace.EndsWith(Path.DirectorySeparatorChar) Then
        '        workspace &= Path.DirectorySeparatorChar
        '    End If
        '    workspace = Path.Combine(workspace, DoDName)
        '    workspace = Path.Combine(workspace, UncertaintyName)

        '    If Not Directory.Exists(workspace) Then
        '        Directory.CreateDirectory(workspace)
        '    End If

        '    Dim outputExtension As String = ".tif"
        '    Dim dodFn As String = Path.Combine(workspace, "DoD" & outputExtension)
        '    Dim csvFn As String = Path.Combine(workspace, "DoDElevDist.csv")
        '    Dim dodPlotFn As String = Path.Combine(workspace, "DoD_Dist_Gross_AV.png")

        '    LayerSource = dodFn
        '    HistogramSource = dodPlotFn
        '    ExcelSource = csvFn

        '    'Create DoD
        '    Dim dod As DoDClass
        '    dod = New DoDClass(dodFn, outputDriver, noData, oldDemFn, newDemFn)

        '    'Create histogram
        '    bSuccess = dod.plotAreaVolume(dodPlotFn, "Gross Distribution")
        '    If Not bSuccess Then
        '        Throw New Exception(dod.getError())
        '    End If


        '    If Not UncertaintyPropertiesRow.IsTypeElevationNull AndAlso UncertaintyPropertiesRow.TypeElevation Then
        '        'Setup outputs
        '        Dim elevThreshold As Single = UncertaintyPropertiesRow.ElevationTheshold
        '        Dim elevThresholdFn As String = Path.Combine(workspace, String.Format("Uniform_DoD_{0}T{1}", elevThreshold, outputExtension))
        '        Dim elevThresholdPlotFn As String = Path.Combine(workspace, "DoD_Dist_Uniform_AV.png")

        '        'Calculate Thresholded DoD
        '        Dim elev As ElevationThresholdClass = New ElevationThresholdClass(elevThresholdFn, outputDriver, noData, dodFn, elevThreshold)

        '        'Compute histograms
        '        If Not dod.computeHistograms() Then
        '            Throw New Exception(dod.getError())
        '        End If
        '        Dim dodHist As HistogramsClass
        '        dodHist = dod.getHistograms()

        '        'Plot histogram
        '        bSuccess = elev.plotAreaVolume(elevThresholdPlotFn,
        '        "Uniform Threshold Classification")
        '        If Not bSuccess Then
        '            Throw New Exception(elev.getError())
        '        End If

        '        'setup outputs
        '        LayerSource = elevThresholdFn
        '        HistogramSource = elevThresholdPlotFn

        '        Dim thresholdHist As HistogramsClass = elev.getHistograms()

        '    End If

        '    'Create CSV
        '    bSuccess = dod.writeCSV(csvFn)
        '    If Not bSuccess Then
        '        Throw New Exception(dod.getError())
        '    End If


        '    'update project config file
        '    UncertaintyPropertiesRow.LayerSource = LayerSource
        '    UncertaintyPropertiesRow.HistogramSource = HistogramSource
        '    UncertaintyPropertiesRow.ExcelSource = ExcelSource
        '    ProjectManager.save()


        'End Sub

        Public Sub CalculateSimpleDoD(ByVal sNewSurvey As String, ByVal sOldSurvey As String, ByVal sSimpleDod As String, ByVal sOutputDoDHisto As String, ByVal elevThreshold As String, ByVal sRaw As String, ByVal sRawHistogramCsv As String) 'As String
            '
            ' Must register GDAL to use the base GCD code
            '
            'Gdal.AllRegister()

            'Create DoD
            Dim dod As DoDClass
            dod = New DoDClass(sRaw, GCDOptions.outputDriver, GCDOptions.NoData, sOldSurvey, sNewSurvey)

            Dim elev As ElevationThresholdClass = New ElevationThresholdClass(sSimpleDod, GCDOptions.outputDriver, GCDOptions.NoData, sRaw, elevThreshold)

            '
            ' Compute histograms
            '
            If Not dod.computeHistograms() Then
                Throw New Exception(dod.getError())
            End If

            Dim rawHist As HistogramsClass
            rawHist = dod.getHistograms()
            rawHist.writeCSV(sRawHistogramCsv)


            If Not elev.computeHistograms() Then
                Throw New Exception(elev.getError())
            End If

            Dim dodHist As HistogramsClass
            dodHist = elev.getHistograms()
            dodHist.writeCSV(sOutputDoDHisto)

            dodHist.Dispose()
            dodHist = Nothing

            dod.Dispose()
            dod = Nothing
            GC.Collect()

        End Sub

        Public Sub CalculateRawDoD(ByVal DoDID As Integer, ByVal sNewSurvey As String, ByVal sOldSurvey As String, ByVal sDod As String, ByVal sOutputDoDHisto As String) 'As String
            '
            ' Must register GDAL to use the base GCD code
            '
            'Gdal.AllRegister()

            'Dim DoDRow As ProjectDS.DoDTableRow = ProjectManager.ds.DoDTable.FindByDoDID(DoDID)
            'Dim DoDName As String = DoDRow.Name

            'Create DoD
            Dim dod As DoDClass
            dod = New DoDClass(sDod, GCDOptions.outputDriver, GCDOptions.NoData, sOldSurvey, sNewSurvey)
            '
            ' Compute histograms
            '
            If Not dod.computeHistograms() Then
                Throw New Exception(dod.getError())
            End If

            Dim dodHist As HistogramsClass
            dodHist = dod.getHistograms()
            dodHist.writeCSV(sOutputDoDHisto)

            dod.Dispose()
            dod = Nothing

            dodHist.Dispose()
            dodHist = Nothing

            GC.Collect()
            'Gdal.GDALDestroyDriverManager()
            'GC.Collect()

        End Sub

        ''' <summary>
        ''' Perform a Propogated Error DoD 
        ''' </summary>
        ''' <param name="sNewSurvey">New Survey raster file path</param>
        ''' <param name="sOldSurvey">Old Survey raster file path</param>
        ''' <param name="sNewError">New Error raster file path</param>
        ''' <param name="sOldError">Old Error raster file path</param>
        ''' <param name="sDoD">File path for the final output THRESHOLDED DoD</param>
        ''' <param name="sPropError">File path for the output propogated error</param>
        ''' <remarks>
        ''' IMPORTANT: The DoD was originally calculated by the GCD, but there is dicrepancies between rasters
        ''' created by ArcGIS and GDAL, that shows up in the histogram, and that we have not been able to explain.
        ''' </remarks>
        Public Sub CalculatePropogatedDoD(ByVal sNewSurvey As String, ByVal sOldSurvey As String, ByVal sNewError As String, ByVal sOldError As String, ByVal sDoD As String, ByVal sPropError As String, ByVal sRawHist As String, ByVal sPropagatedHist As String)
            '
            ' The raw DOD is only a temporary output for propogated error calculations. The final DOD
            ' output is a thresholded raster. Generate a temporary name for the raw DOD and then 
            ' calculate it.
            '
            Dim sTempDoD As String = WorkspaceManager.GetTempRaster("TempDoD") ', GetDefaultRasterExtension()).FullName

            'Calculate DoD using ArcGIS
            GISCode.GP.SpatialAnalyst.Minus(sNewSurvey, sOldSurvey, New FileInfo(sTempDoD))
            '
            ' Calculate the propograted error raster based on the two error surfaces. Then threshold the raw
            ' DoD removing any cells that have a value less than the propogated error.
            '
            'GISCode.GP.Helpers.Helpers.CalculatePropagatedError(sNewError, sOldError, sPropError)
            Dim expression As String
            expression = "SquareRoot(Square(""" & sNewError & """) + Square(""" & sOldError & """))"
            GP.SpatialAnalyst.Raster_Calculator(expression, sPropError)

            'GISCode.GP.Helpers.Helpers.ThresholdRaster(sTempDoD, sPropError, sDoD)
            expression = "Con(Abs(""" & sTempDoD & """) > """ & sPropError & """, """ & sTempDoD & """)"
            GP.SpatialAnalyst.Raster_Calculator(expression, sDoD)

            '
            ' Must register GDAL to use the base GCD code
            '
            'Gdal.AllRegister()
            '
            ' Compute histograms
            '
            Dim DoD As New DoDClass(sTempDoD)
            If Not DoD.computeHistograms() Then
                Throw New Exception(DoD.getError())
            End If

            Dim dodHist As HistogramsClass
            dodHist = DoD.getHistograms()
            dodHist.writeCSV(sRawHist)

            Dim PropagatedDoD As New DoDClass(sDoD)
            If Not PropagatedDoD.computeHistograms() Then
                Throw New Exception(PropagatedDoD.getError())
            End If

            Dim PropagatedHist As HistogramsClass
            PropagatedHist = PropagatedDoD.getHistograms()
            PropagatedHist.writeCSV(sPropagatedHist)

            DoD.Dispose()
            DoD = Nothing

            dodHist.Dispose()
            dodHist = Nothing

            PropagatedDoD.Dispose()
            PropagatedDoD = Nothing

            PropagatedHist.Dispose()
            PropagatedHist = Nothing

            GC.Collect()
            'Gdal.GDALDestroyDriverManager()
            'GC.Collect()

        End Sub

        Public Sub CalculateProbabilityDoD()

            If GISDataStructures.RasterGDAL.Exists(newSurvey) Then
                If GISDataStructures.IsFileGeodatabase(newSurvey) Then
                    Dim ex As New Exception("The new survey raster cannot be in a file GDB.")
                    ex.Data.Add("New Survey Raster", newSurvey)
                    Throw ex
                End If
            Else
                Dim ex As New Exception("The new survey raster is missing or does not exist.")
                ex.Data.Add("New Survey Raster", newSurvey)
                Throw ex
            End If

            If GISDataStructures.RasterGDAL.Exists(oldSurvey) Then
                If GISDataStructures.IsFileGeodatabase(oldSurvey) Then
                    Dim ex As New Exception("The old survey raster cannot be in a file GDB.")
                    ex.Data.Add("Old Survey Raster", oldSurvey)
                    Throw ex
                End If
            Else
                Dim ex As New Exception("The old survey raster is missing or does not exist.")
                ex.Data.Add("Old Survey Raster", oldSurvey)
                Throw ex
            End If

            If String.Compare(newSurvey, oldSurvey, True) = 0 Then
                Dim ex As New Exception("The new and old survey rasters cannot be the same.")
                ex.Data.Add("New Survey Raster", newSurvey)
                ex.Data.Add("Old Survey Raster", oldSurvey)
                Throw ex
            End If

            If GISDataStructures.RasterGDAL.Exists(m_sDoDRasterRaw) Then
                Dim ex As New Exception("The raw DoD raster already exists.")
                ex.Data.Add("Raw  DoD Raster", m_sDoDRasterRaw)
                Throw ex
            Else
                If GISDataStructures.IsFileGeodatabase(m_sDoDRasterRaw) Then
                    Dim ex As New Exception("The raw DoD raster cannot be in a file GDB.")
                    ex.Data.Add("Raw DoD Raster", m_sDoDRasterRaw)
                    Throw ex
                End If
            End If

            If GISDataStructures.RasterGDAL.Exists(m_sDoDRasterThresholded) Then
                Dim ex As New Exception("The thresholded DoD raster already exists.")
                ex.Data.Add("thresholded DoD Raster", m_sDoDRasterThresholded)
                Throw ex
            Else
                If GISDataStructures.IsFileGeodatabase(m_sDoDRasterThresholded) Then
                    Dim ex As New Exception("The thresholded DoD raster cannot be in a file GDB.")
                    ex.Data.Add("Thresholded DoD Raster", m_sDoDRasterThresholded)
                    Throw ex
                End If
            End If
            '
            ' Create Raw DoD
            '
            Dim dod As DoDClass
            Debug.WriteLine("Old Survey: " & oldSurvey)
            Debug.WriteLine("New Survey: " & newSurvey)
            Debug.WriteLine("Raw DOD: " & m_sDoDRasterRaw)

            dod = New DoDClass(m_sDoDRasterRaw, GCDOptions.outputDriver, GCDOptions.NoData, oldSurvey, newSurvey)
            '
            ' Save histogram
            If Not dod.computeHistograms() Then
                Throw New Exception(dod.getError())
            End If

            Dim dodHist As HistogramsClass = dod.getHistograms()
            dodHist.writeCSV(RawHist)

            If m_bBaysian Then
                'create baysian grid
                Dim priorProbFn As String = WorkspaceManager.GetTempRaster("PriorProb")

                Dim prior As PriorVariableClass = New PriorVariableClass(priorProbFn,
                                                                         GCDOptions.outputDriver,
                                                                         GCDOptions.NoData,
                                                                         m_sDoDRasterRaw,
                                                                         oldError,
                                                                         newError)

                Dim movingWinWidth As Integer = filterSize
                Dim movingWinHeight As Integer = filterSize

                Dim maxCells As Integer = movingWinWidth * movingWinHeight
                Dim xMin As Integer = Math.Floor(maxCells * (minNbrPercent / 100))
                Dim xMax As Integer = Math.Floor(maxCells * (maxNbrPercent / 100))

                Dim postProbFn As String = WorkspaceManager.GetTempRaster("postProb")
                Dim condProbFn As String = WorkspaceManager.GetTempRaster("condProb")
                Dim nbrErosionFn As String = WorkspaceManager.GetTempRaster("nbrErosion")
                Dim nbrDepositionFn As String = WorkspaceManager.GetTempRaster("nbrDeposition")

                Dim win As GCDWindowClass = New GCDWindowClass(nbrErosionFn,
                                                               nbrDepositionFn,
                                                               GCDOptions.outputDriver,
                                                               GCDOptions.NoData,
                                                               m_sDoDRasterRaw,
                                                               movingWinWidth,
                                                               movingWinHeight)

                Dim post As PostClass = New PostClass(postProbFn,
                                                      condProbFn,
                                                  GCDOptions.outputDriver,
                                                               GCDOptions.NoData,
                                                                   m_sDoDRasterRaw,
                                                      priorProbFn,
                                                      nbrErosionFn,
                                                      nbrDepositionFn,
                                                      xMin,
                                                      xMax)

                Dim postCi As CIThresholdProbabilityClass = New CIThresholdProbabilityClass(m_sDoDRasterThresholded,
                                                                                            GCDOptions.outputDriver,
                                                                                            GCDOptions.NoData,
                                                                                            m_sDoDRasterRaw,
                                                                                            postProbFn,
                                                                                            m_fProbability)

                'save histogram
                If Not postCi.computeHistograms() Then
                    Throw New Exception(postCi.getError())
                End If
                Dim gcdProbabilityHist As HistogramsClass
                gcdProbabilityHist = postCi.getHistograms()
                gcdProbabilityHist.writeCSV(ProbabilityHist)

                prior.Dispose()
                prior = Nothing

                win.Dispose()
                win = Nothing

                post.Dispose()
                post = Nothing

                postCi.Dispose()
                postCi = Nothing

            Else
                Dim ci As CIThresholdVariableClass = New CIThresholdVariableClass(m_sDoDRasterThresholded,
                                                                                  GCDOptions.outputDriver,
                                                                                  GCDOptions.NoData,
                                                                                  m_sDoDRasterRaw,
                                                                                  oldError,
                                                                                  newError,
                                                                                  m_fProbability)

                'save histogram
                'Change to use same bins as raw histogram - Frank
                Dim gcdProbabilityHist As New HistogramsClass(m_sDoDRasterThresholded, dodHist.getNumBins, dodHist.getMinimumBin, dodHist.getBinSize, dodHist.getBinIncrement)
                gcdProbabilityHist.writeCSV(ProbabilityHist)

                ci.Dispose()
                ci = Nothing

                gcdProbabilityHist.Dispose()
                gcdProbabilityHist = Nothing

            End If

            dod.Dispose()
            dod = Nothing

            GC.Collect()

        End Sub


        Public Sub CalculateMinimumDoD(sThresholdDoDPath As String, sPropagatedErrorPath As String, sMinDoDPath As String, sMinDoDCSVPath As String)

            'calculate min raster
            GP.SpatialAnalyst.Minus(sThresholdDoDPath, sPropagatedErrorPath, sMinDoDPath)

            'export csv
            Dim DoD As New DoDClass(sMinDoDPath)
            'If Not DoD.computeHistograms() Then
            '    Throw New Exception(DoD.getError())
            'End If

            Dim dodHist As HistogramsClass = DoD.getHistograms()
            dodHist.writeCSV(sMinDoDCSVPath)

            DoD.Dispose()
            DoD = Nothing
            GC.Collect()

        End Sub

        'Public Sub CalculateMinimumDoD(OutputParameters As ChangeDetectionOutput)

        '    'calculate min raster
        '    GP.SpatialAnalyst.Minus(OutputParameters.ThresholdedDoDPath, OutputParameters.PropagatedErrorPath, OutputParameters.MinDoDPath)

        '    'export csv
        '    Dim DoD As New DoDClass(OutputParameters.MinDoDPath)
        '    If Not DoD.computeHistograms() Then
        '        Throw New Exception(DoD.getError())
        '    End If

        '    Dim dodHist As HistogramsClass
        '    dodHist = DoD.getHistograms()
        '    dodHist.writeCSV(OutputParameters.MinDoDCsvPath)

        'End Sub

        Public Sub New()

        End Sub
    End Class

End Namespace