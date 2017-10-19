Namespace Core.ChangeDetection

    Public Class ChangeDetectionEngineMinLOD
        Inherits ChangeDetectionEngineBase

        Private m_fThreshold As Double

        Public ReadOnly Property Threshold As Double
            Get
                Return m_fThreshold
            End Get
        End Property

        Public Sub New(ByVal sName As String, ByVal sFolder As String, ByVal gNewDEM As GCDConsoleLib.Raster, ByVal gOldDEM As GCDConsoleLib.Raster,
                       ByVal gAOI As GCDConsoleLib.Vector, ByVal fThreshold As Double, ByVal fChartWidth As Integer, ByVal fChartHeight As Integer)
            MyBase.New(sName, sFolder, gNewDEM, gOldDEM, gAOI, fChartHeight, fChartWidth)

            m_fThreshold = fThreshold
        End Sub

        Public Overrides Function Calculate(ByRef sRawDoDPath As String, ByRef sThreshDodPath As String, ByRef sRawHistPath As String, ByRef sThreshHistPath As String, ByRef sSummaryXMLPath As String) As DoDResultSet

            GenerateAnalysisRasters()

            CalculateRawDoD(sRawDoDPath, sRawHistPath)
            sThreshDodPath = GCDProject.ProjectManagerBase.OutputManager.GetDoDThresholdPath(Name, Folder.FullName)

            Dim eResult As External.GCDCoreOutputCodes
            eResult = External.ThresholdDoDMinLoD(sRawDoDPath, sThreshDodPath, m_fThreshold, GCDProject.ProjectManagerBase.OutputManager.OutputDriver,
                                                                              GCDProject.ProjectManagerBase.OutputManager.NoData, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                Dim ex As New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                Throw New Exception("Error calculating the raw DEM of difference raster.", ex)
            End If

            ' Check that the raster exists
            If Not GISDataStructures.GISDataSource.Exists(sThreshDodPath) Then
                Throw New Exception("The thresholded DoD raster file does noth exist.")
            End If

            sThreshHistPath = GCDProject.ProjectManagerBase.OutputManager.GetCsvThresholdPath(Name, Folder.FullName)
            eResult = External.CalculateAndWriteDoDHistogram(sThreshDodPath, sThreshHistPath, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)
            If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                Dim ex As New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                Throw New Exception("Error calculating and writing the raw DEM histogram.", ex)
            End If

            Dim gRawDoD As New GCDConsoleLib.Raster(sRawDoDPath)
            Dim dodProps As New ChangeDetectionPropertiesMinLoD(sRawDoDPath, sThreshDodPath, Threshold, gRawDoD.CellSize, gRawDoD.LinearUnits)
            Dim theChangeStats As New ChangeStatsCalculator(dodProps)
            sSummaryXMLPath = GenerateSummaryXML(theChangeStats)

            Dim dodHistos As New DoDResultHistograms(sRawHistPath, sThreshHistPath)
            Dim dodResultSet As New DoDResultSet(theChangeStats, dodHistos, dodProps)

            theChangeStats.GenerateChangeBarGraphicFiles(GCDProject.ProjectManagerBase.OutputManager.GetChangeDetectionFiguresFolder(Folder.FullName, True), dodProps.Units, ChartHeight, ChartWidth)
            GenerateHistogramGraphicFiles(dodHistos, dodProps.Units)

            Return dodResultSet

        End Function

    End Class


End Namespace