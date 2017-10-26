
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
                       ByVal gNewDEM As GCDConsoleLib.Raster, ByVal gOldDEM As GCDConsoleLib.Raster,
                       ByVal gNewError As GCDConsoleLib.Raster, ByVal gOldError As GCDConsoleLib.Raster,
                       ByVal gAOI As GCDConsoleLib.Vector,
                       ByVal fThreshold As Double, ByVal fChartHeight As Integer, ByVal fChartWidth As Integer, Optional ByVal spatCoherence As CoherenceProperties = Nothing)

            ' Call the base class constructor to instantiate common members.
            MyBase.New(sName, sFolder, gNewDEM, gOldDEM, gNewError, gOldError, gAOI, fChartHeight, fChartWidth)

            If fThreshold < 0 OrElse fThreshold > 1 Then
                Throw New ArgumentOutOfRangeException("fThreshold", fThreshold, "The threshold for probabilistic change detection engine must be between zero and one.")
            End If
            m_fThreshold = fThreshold

            m_SpatialCoherence = spatCoherence

        End Sub

        Protected Overrides Function ThresholdRawDoD(rawDoDPath As String, rawHistPath As String) As DoDResult

            Dim propErrorRaster As String = GeneratePropagatedErrorRaster()

            Dim thrDodPath As String = GCDProject.ProjectManagerBase.OutputManager.GetDoDThresholdPath(Name, IO.Path.GetDirectoryName(rawDoDPath))
            Dim thrHistPath As String = GCDProject.ProjectManagerBase.OutputManager.GetCsvThresholdPath(Name, IO.Path.GetDirectoryName(rawDoDPath))

            Dim sPosteriorRaster As String = ""
            Dim sConditionalRaster As String = ""
            Dim priorProbFn As String = ""
            Dim sPriorProbRaster As String = ""
            Dim sSpatialCoDepositionRaster As String = ""
            Dim sSpatialCoErosionRaster As String = ""

            ' Create the prior probability raster
            sPriorProbRaster = naru.os.File.GetNewSafeName(Folder.FullName, "priorprob", GCDProject.ProjectManagerBase.RasterExtension).FullName
            External.CreatePriorProbabilityRaster(rawDoDPath, AnalysisNewError.FilePath, AnalysisOldError.FilePath, sPriorProbRaster,
                                                    GCDProject.ProjectManagerBase.OutputManager.OutputDriver,
                                                    GCDProject.ProjectManagerBase.OutputManager.NoData,
                                                   GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)



            If TypeOf SpatialCoherenceProperties Is CoherenceProperties Then

                sPosteriorRaster = naru.os.File.GetNewSafeName(Folder.FullName, "postProb", GCDProject.ProjectManagerBase.RasterExtension).FullName
                sConditionalRaster = naru.os.File.GetNewSafeName(Folder.FullName, "condProb", GCDProject.ProjectManagerBase.RasterExtension).FullName
                sSpatialCoErosionRaster = naru.os.File.GetNewSafeName(Folder.FullName, "nbrErosion", GCDProject.ProjectManagerBase.RasterExtension).FullName
                sSpatialCoDepositionRaster = naru.os.File.GetNewSafeName(Folder.FullName, "nbrDeposition", GCDProject.ProjectManagerBase.RasterExtension).FullName

                External.ThresholdDoDProbWithSpatialCoherence(rawDoDPath, thrDodPath, AnalysisNewError.FilePath, AnalysisOldError.FilePath,
                                                            sPriorProbRaster, sPosteriorRaster, sConditionalRaster, sSpatialCoErosionRaster, sSpatialCoDepositionRaster,
                                                             GCDProject.ProjectManagerBase.OutputManager.OutputDriver, GCDProject.ProjectManagerBase.OutputManager.NoData,
                                                             SpatialCoherenceProperties.MovingWindowWidth, SpatialCoherenceProperties.MovingWindowHeight, Threshold,
                                                             GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)


                External.CalculateAndWriteDoDHistogram(thrDodPath, thrHistPath, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            Else
                External.ThresholdDoDProbability(rawDoDPath, thrHistPath, AnalysisNewError.FilePath, AnalysisOldError.FilePath, sPriorProbRaster,
                                                    GCDProject.ProjectManagerBase.OutputManager.OutputDriver, GCDProject.ProjectManagerBase.OutputManager.NoData,
                                                    Threshold, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

                External.CalculateAndWriteDoDHistogramWithSpecifiedBins(thrDodPath, thrHistPath, m_nNumBins, m_nMinimumBin, m_fBinSize,
                                                                                     m_fBinIncrement, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)
            End If

            Return New DoDResultPropagated(rawDoDPath, rawHistPath, thrDodPath, thrHistPath, propErrorRaster, CellSize, LinearUnits)

        End Function
    End Class

End Namespace