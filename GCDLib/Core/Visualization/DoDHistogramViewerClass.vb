Imports System.Windows.Forms.DataVisualization

Namespace Core.Visualization

    Public Class DoDHistogramViewerClass

        Private Const EROSION As String = "Erosion"
        Private Const DEPOSITION As String = "Deposition"
        Private Const RAW As String = "Raw"

        Private m_Chart As Charting.Chart
        Private m_HistogramData As ChangeDetection.DoDResultHistograms

        ''' <summary>
        ''' Call this constructor from non-UI code that simply wants to generate histogram plot image files
        ''' </summary>
        Public Sub New(sRawHistogram As String, sThreshHistogram As String, linearDataUnits As UnitsNet.Units.LengthUnit)
            m_Chart = New Charting.Chart
            Init(sRawHistogram, sThreshHistogram, linearDataUnits)
        End Sub

        ''' <summary>
        ''' Constructor for UI code to pass in a chart on a user interface form
        ''' </summary>
        ''' <param name="cht"></param>
        Public Sub New(ByRef cht As Charting.Chart, sRawHistogram As String, sThreshHistogram As String, linearDataUnits As UnitsNet.Units.LengthUnit)
            m_Chart = cht
            Init(sRawHistogram, sThreshHistogram, linearDataUnits)
        End Sub

        Private Sub Init(sRawHistogram As String, sThreshHistogram As String, linearDataUnits As UnitsNet.Units.LengthUnit)

            ' Load the data for both the raw and thresholded histograms
            m_HistogramData = New ChangeDetection.DoDResultHistograms(sRawHistogram, sThreshHistogram, linearDataUnits)

            ' Proceed and do the one-time chart preparation
            m_Chart.ChartAreas.Clear()
            m_Chart.ChartAreas.Add(New Charting.ChartArea())

            m_Chart.Series.Clear()
            m_Chart.Palette = Nothing
            m_Chart.Legends.Clear()

            Dim seriesDefs = New Dictionary(Of String, System.Drawing.Color) From {{EROSION, My.Settings.Erosion}, {DEPOSITION, My.Settings.Depsoition}, {RAW, Drawing.Color.LightGray}}
            For Each aDef As KeyValuePair(Of String, Drawing.Color) In seriesDefs
                Dim series As Charting.Series = m_Chart.Series.Add(aDef.Key)
                series.ChartType = Charting.SeriesChartType.StackedColumn
                series.Color = aDef.Value
                series.ChartArea = m_Chart.ChartAreas.First().Name
            Next

            With m_Chart.ChartAreas(0).AxisX
                .MajorGrid.LineColor = Drawing.Color.LightSlateGray
                .MinorTickMark.Enabled = True
            End With

            With m_Chart.ChartAreas(0).AxisY
                .MajorGrid.LineColor = Drawing.Color.LightSlateGray
                .MinorTickMark.Enabled = True
            End With

            Refresh(True, linearDataUnits, GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(linearDataUnits), GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(linearDataUnits))

        End Sub

        Public Sub SetHistogramUnits(bArea As Boolean, linearDisplayUnits As UnitsNet.Units.LengthUnit, areaDisplayUnits As UnitsNet.Units.AreaUnit, volumesDisplayUnits As UnitsNet.Units.VolumeUnit)
            Refresh(bArea, linearDisplayUnits, areaDisplayUnits, volumesDisplayUnits)
        End Sub

        'Public Sub Refresh(bArea As Boolean)
        '    Refresh(bArea)
        'End Sub

        Private Sub Refresh(ByVal bArea As Boolean, ByVal linearDisplayUnits As UnitsNet.Units.LengthUnit, areaDisplayUnits As UnitsNet.Units.AreaUnit, volumeDisplayUnits As UnitsNet.Units.VolumeUnit)

            Dim histoData As List(Of HistogramDisplayDataPoint) = Nothing

            If bArea Then
                histoData = m_HistogramData.GetAreaDisplayValues(linearDisplayUnits, areaDisplayUnits)
            Else
                histoData = m_HistogramData.GetVolumeDisplayValues(linearDisplayUnits, volumeDisplayUnits)
            End If

            m_Chart.Series.FindByName(EROSION).Points.DataBindXY(histoData, "elevation", histoData, "erosion")
            m_Chart.Series.FindByName(DEPOSITION).Points.DataBindXY(histoData, "elevation", histoData, "deposition")
            m_Chart.Series.FindByName(RAW).Points.DataBindXY(histoData, "elevation", histoData, "raw")

            With m_Chart.ChartAreas(0)
                .AxisX.Title = String.Format("Elevation Change ({0})", linearDisplayUnits)

                If bArea Then
                    .AxisY.Title = String.Format("Area ({0}²)", UnitsNet.Area.GetAbbreviation(areaDisplayUnits))
                Else
                    .AxisY.Title = String.Format("Volume ({0}³)", UnitsNet.Volume.GetAbbreviation(volumeDisplayUnits))
                End If
            End With

        End Sub

        Public Sub ExportCharts(ByVal AreaGraphPath As String, ByVal VolumeGraphPath As String, ByVal ChartWidth As Integer, ByVal ChartHeight As Integer)

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(AreaGraphPath)) Then
                Dim ex As New Exception("The output folder for the GCD area graph does not exist.")
                ex.Data("Area Graph Path") = AreaGraphPath
            End If

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(VolumeGraphPath)) Then
                Dim ex As New Exception("The output folder for the GCD volume graph does not exist.")
                ex.Data("volume Graph Path") = VolumeGraphPath
            End If

            Refresh(True)
            m_Chart.SaveImage(AreaGraphPath, Charting.ChartImageFormat.Png)

            Refresh(False)
            m_Chart.SaveImage(VolumeGraphPath, Charting.ChartImageFormat.Png)

        End Sub

    End Class

End Namespace