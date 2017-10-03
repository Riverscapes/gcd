Namespace Core.ChangeDetection

    Public Class ChangeDetectionEngineMinLOD
        Inherits ChangeDetectionEngineBase

        Private m_fThreshold As Double

        Public ReadOnly Property Threshold As Double
            Get
                Return m_fThreshold
            End Get
        End Property

        Public Sub New(ByVal sName As String, ByVal sFolder As String, ByVal gNewDEM As GISDataStructures.Raster, ByVal gOldDEM As GISDataStructures.Raster,
                       ByVal gAOI As GISDataStructures.PolygonDataSource, ByVal fThreshold As Double, ByVal fChartWidth As Integer, ByVal fChartHeight As Integer)
            MyBase.New(sName, sFolder, gNewDEM, gOldDEM, gAOI, fChartHeight, fChartWidth)

            m_fThreshold = fThreshold
        End Sub

        Public Overrides Function Calculate(ByRef sRawDoDPath As String, ByRef sThreshDodPath As String, ByRef sRawHistPath As String, ByRef sThreshHistPath As String, ByRef sSummaryXMLPath As String) As DoDResultSet

            GenerateAnalysisRasters()

            CalculateRawDoD(sRawDoDPath, sRawHistPath)
            sThreshDodPath = GCD.GCDProject.ProjectManager.OutputManager.GetDoDThresholdPath(Name, Folder.FullName)

            Dim eResult As GCDCore.GCDCoreOutputCodes
            eResult = GCDCore.ThresholdDoDMinLoD(sRawDoDPath, sThreshDodPath, m_fThreshold, GCD.GCDProject.ProjectManager.OutputManager.OutputDriver,
                                                                              GCD.GCDProject.ProjectManager.OutputManager.NoData, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString)

            If eResult <> GCDCoreOutputCodes.PROCESS_OK Then
                Dim ex As New Exception(GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                Throw New Exception("Error calculating the raw DEM of difference raster.", ex)
            End If

            ' Check that the raster exists
            If Not GISDataStructures.Raster.Exists(sThreshDodPath) Then
                Throw New Exception("The thresholded DoD raster file does noth exist.")
            End If

            sThreshHistPath = GCD.GCDProject.ProjectManager.OutputManager.GetCsvThresholdPath(Name, Folder.FullName)
            eResult = GCDCore.CalculateAndWriteDoDHistogram(sThreshDodPath, sThreshHistPath, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString)
            If eResult <> GCDCoreOutputCodes.PROCESS_OK Then
                Dim ex As New Exception(GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                Throw New Exception("Error calculating and writing the raw DEM histogram.", ex)
            End If

            Dim gRawDoD As New GISDataStructures.RasterDirect(sRawDoDPath)
            Dim dodProps As New ChangeDetectionPropertiesMinLoD(sRawDoDPath, sThreshDodPath, Threshold, gRawDoD.CellSize, NumberFormatting.GetLinearUnitsFromESRI(gRawDoD.LinearUnits))
            Dim theChangeStats As New ChangeStatsCalculator(dodProps)
            sSummaryXMLPath = GenerateSummaryXML(theChangeStats)

            Dim dodHistos As New DoDResultHistograms(sRawHistPath, sThreshHistPath)
            Dim dodResultSet As New DoDResultSet(theChangeStats, dodHistos, dodProps)

            theChangeStats.GenerateChangeBarGraphicFiles(GCD.GCDProject.ProjectManager.OutputManager.GetChangeDetectionFiguresFolder(Folder.FullName, True), dodProps.Units.LinearUnit, ChartHeight, ChartWidth)
            GenerateHistogramGraphicFiles(dodHistos, dodProps.Units.LinearUnit)

            Return dodResultSet

        End Function

    End Class


End Namespace