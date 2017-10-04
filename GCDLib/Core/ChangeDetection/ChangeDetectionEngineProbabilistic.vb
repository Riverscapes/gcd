
Namespace Core.ChangeDetection

    Public Class ChangeDetectionEngineProbabilistic
        Inherits ChangeDetectionEnginePropProb

        Private m_fThreshold As Double
        Private m_SpatialCoherence As CoherenceProperties

        Public ReadOnly Property Threshold As Double
            Get
                Return m_fThreshold
            End Get
        End Property

        Public ReadOnly Property SpatialCoherenceProperties As CoherenceProperties
            Get
                Return m_SpatialCoherence
            End Get
        End Property

        Public Sub New(ByVal sName As String, ByVal sFolder As String,
                       ByVal gNewDEM As GISDataStructures.Raster, ByVal gOldDEM As GISDataStructures.Raster,
                       ByVal gNewError As GISDataStructures.Raster, ByVal gOldError As GISDataStructures.Raster,
                       ByVal gAOI As GISDataStructures.Vector,
                       ByVal fThreshold As Double, ByVal fChartHeight As Integer, ByVal fChartWidth As Integer, Optional ByVal spatCoherence As CoherenceProperties = Nothing)

            ' Call the base class constructor to instantiate common members.
            MyBase.New(sName, sFolder, gNewDEM, gOldDEM, gNewError, gOldError, gAOI, fChartHeight, fChartWidth)

            If fThreshold < 0 OrElse fThreshold > 1 Then
                Throw New ArgumentOutOfRangeException("fThreshold", fThreshold, "The threshold for probabilistic change detection engine must be between zero and one.")
            End If
            m_fThreshold = fThreshold

            m_SpatialCoherence = spatCoherence

        End Sub

        Public Overrides Function Calculate(ByRef sRawDoDPath As String, ByRef sThreshDodPath As String, ByRef sRawHistPath As String, ByRef sThreshHistPath As String, ByRef sSummaryXMLPath As String) As DoDResultSet

            GenerateAnalysisRasters()

            ' Calculate the raw DoD
            CalculateRawDoD(sRawDoDPath, sRawHistPath)
            sThreshDodPath = GCD.GCDProject.ProjectManager.OutputManager.GetDoDThresholdPath(Name, IO.Path.GetDirectoryName(sRawDoDPath))
            sThreshHistPath = GCD.GCDProject.ProjectManager.OutputManager.GetCsvThresholdPath(Name, IO.Path.GetDirectoryName(sRawDoDPath))

            Dim sPosteriorRaster As String = ""
            Dim sConditionalRaster As String = ""
            Dim priorProbFn As String = ""
            Dim sPriorProbRaster As String = ""
            Dim sSpatialCoDepositionRaster As String = ""
            Dim sSpatialCoErosionRaster As String = ""

            ' Create the prior probability raster
            sPriorProbRaster = GISDataStructures.Raster.GetNewSafeName(Folder.FullName & IO.Path.DirectorySeparatorChar, "priorProb")
            If Not GCDCore.CreatePriorProbabilityRaster(sRawDoDPath, AnalysisNewError.FullPath, AnalysisOldError.FullPath, sPriorProbRaster,
                                                    GCD.GCDProject.ProjectManager.OutputManager.OutputDriver,
                                                    GCD.GCDProject.ProjectManager.OutputManager.NoData,
                                                   GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString) = GCDCoreOutputCodes.PROCESS_OK Then
                Throw New Exception(GCD.GCDProject.ProjectManager.GCDNARCError.ToString)

            End If

            If TypeOf SpatialCoherenceProperties Is CoherenceProperties Then

                sPosteriorRaster = GISDataStructures.Raster.GetNewSafeName(Folder.FullName & IO.Path.DirectorySeparatorChar, "postProb")
                sConditionalRaster = GISDataStructures.Raster.GetNewSafeName(Folder.FullName & IO.Path.DirectorySeparatorChar, "condProb")
                sSpatialCoErosionRaster = GISDataStructures.Raster.GetNewSafeName(Folder.FullName & IO.Path.DirectorySeparatorChar, "nbrErosion")
                sSpatialCoDepositionRaster = GISDataStructures.Raster.GetNewSafeName(Folder.FullName & IO.Path.DirectorySeparatorChar, "nbrDeposition")

                If Not GCDCore.ThresholdDoDProbWithSpatialCoherence(sRawDoDPath, sThreshDodPath, AnalysisNewError.FullPath, AnalysisOldError.FullPath,
                                                            sPriorProbRaster, sPosteriorRaster, sConditionalRaster, sSpatialCoErosionRaster, sSpatialCoDepositionRaster,
                                                             GCD.GCDProject.ProjectManager.OutputManager.OutputDriver, GCD.GCDProject.ProjectManager.OutputManager.NoData,
                                                             SpatialCoherenceProperties.MovingWindowWidth, SpatialCoherenceProperties.MovingWindowHeight, Threshold,
                                                             GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString) = GCDCoreOutputCodes.PROCESS_OK Then
                    Throw New Exception(GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                End If

                If Not GCDCore.CalculateAndWriteDoDHistogram(sThreshDodPath, sThreshHistPath, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString) = GCDCoreOutputCodes.PROCESS_OK Then
                    Throw New Exception(GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                End If

            Else
                If Not GCDCore.ThresholdDoDProbability(sRawDoDPath, sThreshDodPath, AnalysisNewError.FullPath, AnalysisOldError.FullPath, sPriorProbRaster,
                                                    GCD.GCDProject.ProjectManager.OutputManager.OutputDriver, GCD.GCDProject.ProjectManager.OutputManager.NoData,
                                                    Threshold, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString) = GCDCoreOutputCodes.PROCESS_OK Then
                    Throw New Exception(GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                End If

                Try
                    If Not GCDCore.CalculateAndWriteDoDHistogramWithSpecifiedBins(sThreshDodPath, sThreshHistPath, m_nNumBins, m_nMinimumBin, m_fBinSize,
                                                                                     m_fBinIncrement, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString) = GCDCoreOutputCodes.PROCESS_OK Then
                        Throw New Exception(GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                    End If
                Catch ex As Exception
                    Debug.WriteLine("Warning thresholded histogram failed to write to: " & sThreshHistPath)
                End Try
            End If

            Dim gDoDRaw As New GISDataStructures.RasterDirect(sRawDoDPath)

            Dim sPropErrRaster As String = GeneratePropagatedErrorRaster()
            Dim dodProp As New ChangeDetectionPropertiesProbabilistic(sRawDoDPath, sThreshDodPath, sPropErrRaster, sPriorProbRaster, sSpatialCoErosionRaster, sSpatialCoDepositionRaster, sConditionalRaster, sPosteriorRaster, Threshold, -1, False, gDoDRaw.CellSize, NumberFormatting.GetLinearUnitsFromESRI(gDoDRaw.LinearUnits))
            Dim theChangeStats As New ChangeStatsCalculator(dodProp)
            sSummaryXMLPath = GenerateSummaryXML(theChangeStats)

            Dim theHistograms As New DoDResultHistograms(sRawHistPath, sThreshHistPath)

            theChangeStats.GenerateChangeBarGraphicFiles(GCD.GCDProject.ProjectManager.OutputManager.GetChangeDetectionFiguresFolder(Folder.FullName, True), dodProp.Units.LinearUnit, ChartWidth, ChartHeight)
            GenerateHistogramGraphicFiles(theHistograms, dodProp.Units.LinearUnit)

            Dim dodResults As New DoDResultSet(theChangeStats, theHistograms, dodProp)

            Return dodResults

        End Function

    End Class

End Namespace