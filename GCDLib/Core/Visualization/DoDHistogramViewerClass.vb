Imports System.Windows.Forms.DataVisualization

Namespace Core.Visualization

    Public Class DoDHistogramViewerClass

        Private Const EROSION As String = "Erosion"
        Private Const DEPOSITION As String = "Deposition"
        Private Const RAW As String = "Raw"

        Private m_Chart As Charting.Chart
        'Private _ThresholdedHist As Dictionary(Of Double, Double)
        'Private _RawHist As Dictionary(Of Double, Double)
        Private m_eUnits As UnitsNet.Units.LengthUnit

        'Public Property Hist1 As Dictionary(Of Double, Double)
        '    Get
        '        Return _ThresholdedHist
        '    End Get
        '    Set(ByVal value As Dictionary(Of Double, Double))
        '        _ThresholdedHist = value
        '    End Set
        'End Property


        'Public Property Hist2 As Dictionary(Of Double, Double)
        '    Get
        '        Return _RawHist
        '    End Get
        '    Set(ByVal value As Dictionary(Of Double, Double))
        '        _RawHist = value
        '    End Set
        'End Property

        'Public Property yLabel As String
        '    Get
        '        Dim Pane1 As ZedGraph.GraphPane = _ZedGraph.GraphPane
        '        Return Pane1.YAxis.Title.Text
        '    End Get
        '    Set(ByVal value As String)
        '        Dim Pane1 As ZedGraph.GraphPane = _ZedGraph.GraphPane
        '        Pane1.YAxis.Title.Text = value
        '    End Set
        'End Property


        Public Sub New(ByRef cht As Charting.Chart, ByVal eUnits As UnitsNet.Units.LengthUnit)
            m_Chart = cht
            m_eUnits = eUnits

            m_Chart.ChartAreas.Clear()
            m_Chart.ChartAreas.Add(New Charting.ChartArea())

            m_Chart.Series.Clear()
            m_Chart.Palette = Nothing
            m_Chart.Legends.Clear()

            Dim seriesDefs = New Dictionary(Of String, System.Drawing.Color) From {{EROSION, Drawing.Color.Red}, {DEPOSITION, Drawing.Color.Blue}, {RAW, Drawing.Color.LightGray}}
            For Each aDef As KeyValuePair(Of String, Drawing.Color) In seriesDefs
                Dim series As Charting.Series = m_Chart.Series.Add(aDef.Key)
                series.ChartType = Charting.SeriesChartType.Column
                series.Color = aDef.Value
                series.ChartArea = m_Chart.ChartAreas.First().Name
            Next

        End Sub

        Public Sub refresh(ByVal dRawHistogram As Dictionary(Of Double, Double),
                           ByVal dThresholdedHistogram As Dictionary(Of Double, Double),
                           ByVal bArea As Boolean,
                           ByVal eUnits As UnitsNet.Units.LengthUnit)

            m_Chart.Series.FindByName(EROSION).Points.DataBindY(dRawHistogram.Values.Where(Function(s As Double) s >= 0).ToList())

            m_Chart.Series.FindByName(DEPOSITION).Points.DataBindY(dRawHistogram.Values.Where(Function(s As Double) s < 0).ToList())
            m_Chart.Series.FindByName(RAW).Points.DataBindY(dThresholdedHistogram.Values)


            'setup pane
            'Dim Pane1 As Charting.ChartArea = _ZedGraph.ChartAreas(0)
            'Pane1.Title.IsVisible = False
            'Pane1.Legend.IsVisible = False
            'Pane1.Border.IsVisible = False
            'Pane1.CurveList.Clear()

            ''Set up gridlines to the plot
            'Pane1.XAxis.MajorGrid.IsVisible = True
            'Pane1.XAxis.MajorGrid.Color = Drawing.Color.Gray
            'Pane1.YAxis.MajorGrid.IsVisible = True
            'Pane1.YAxis.MajorGrid.Color = Drawing.Color.Gray

            'Pane1.XAxis.Scale.MaxGrace = 0
            'Pane1.XAxis.Scale.MinGrace = 0

            ''Stop zedgraph from detecting magnitude
            'Pane1.YAxis.Scale.MagAuto = False
            'Pane1.YAxis.Scale.Format = "#,#"

            ''TODO change eUnits to what the user has selected in the summary options panel
            'Pane1.XAxis.Title.Text = "Elevation Change" & naru.math.NumberFormatting.GetUnitsAsString(eUnits, True)

            ''Get proper label and subscripted units for y-axis
            'If bArea Then
            '    Pane1.YAxis.Title.Text = "Area " & naru.math.NumberFormatting.GetUnitsAsString(eUnits, True, 2)
            'Else
            '    Pane1.YAxis.Title.Text = "Volume " & naru.math.NumberFormatting.GetUnitsAsString(eUnits, True, 3)
            'End If

            'Pane1.BarSettings.MinClusterGap = 0
            'Pane1.BarSettings.Type = ZedGraph.BarType.Overlay

            ''Prepare thresholded data
            'Dim ErosionThresholdedData As New ZedGraph.PointPairList
            'Dim DepositionThresholdedData As New ZedGraph.PointPairList
            'For Each kv As KeyValuePair(Of Double, Double) In dThresholdedHistogram
            '    If kv.Key <0 Then
            '        If kv.Value > 0 Then
            '            ErosionThresholdedData.Add(kv.Key, kv.Value)
            '        End If
            '    Else
            '        If kv.Value > 0 Then
            '            DepositionThresholdedData.Add(kv.Key, kv.Value)
            '        End If
            '    End If
            'Next

            ''Create  erosion thresholded bars (red)
            'Dim ErosionThresholdedBars As ZedGraph.BarItem
            'ErosionThresholdedBars = Pane1.AddBar("ErosionThresholded", ErosionThresholdedData, Drawing.Color.DarkBlue)
            ''ErosionThresholdedBars.Bar.Fill = New ZedGraph.Fill(Drawing.Color.FromArgb(&HFFFF2222), Drawing.Color.FromArgb(&HFF885555), 270)
            'ErosionThresholdedBars.Bar.Fill = New ZedGraph.Fill(GCD.GCDProject.ProjectManager.ColourErosion)
            'ErosionThresholdedBars.Bar.Border.IsVisible = True
            'ErosionThresholdedBars.Bar.Border.Color = Drawing.Color.White

            ''Create deposition thresholded bars (blue)
            'Dim DepositionThresholdedBars As ZedGraph.BarItem
            'DepositionThresholdedBars = Pane1.AddBar("DepositionThresholded", DepositionThresholdedData, Drawing.Color.DarkRed)
            ''DepositionThresholdedBars.Bar.Fill = New ZedGraph.Fill(Drawing.Color.FromArgb(&HFF2222FF), Drawing.Color.FromArgb(&HFF555588), 270)
            'DepositionThresholdedBars.Bar.Fill = New ZedGraph.Fill(GCD.GCDProject.ProjectManager.ColourDeposition)
            'DepositionThresholdedBars.Bar.Border.IsVisible = True
            'DepositionThresholdedBars.Bar.Border.Color = Drawing.Color.White

            ''Prepare raw data
            'Dim RawData As New ZedGraph.PointPairList
            'For Each kv As KeyValuePair(Of Double, Double) In dRawHistogram
            '    If kv.Value > 0 Then
            '        RawData.Add(kv.Key, kv.Value)
            '    End If
            'Next

            ''Create raw bars
            'Dim RawBArs As ZedGraph.BarItem
            'RawBArs = Pane1.AddBar("Histogram 2", RawData, Drawing.Color.Azure)
            'RawBArs.Bar.Fill = New ZedGraph.Fill(Drawing.Color.FromArgb(&HFFAAAAAA))
            'RawBArs.Bar.Border.IsVisible = True

            '_ZedGraph.AxisChange()
            '_ZedGraph.Refresh()
        End Sub

        Public Sub ExportCharts(ByVal changeHisto As Core.ChangeDetection.DoDResultHistograms, ByVal theUnits As UnitsNet.Units.LengthUnit, ByVal AreaGraphPath As String, ByVal VolumeGraphPath As String, ByVal ChartWidth As Integer, ByVal ChartHeight As Integer)

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(AreaGraphPath)) Then
                Dim ex As New Exception("The output folder for the GCD area graph does not exist.")
                ex.Data("Area Graph Path") = AreaGraphPath
            End If

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(VolumeGraphPath)) Then
                Dim ex As New Exception("The output folder for the GCD volume graph does not exist.")
                ex.Data("volume Graph Path") = VolumeGraphPath
            End If

            'Save histograms
            Dim myZedGraphControl As New Charting.Chart
            Dim myHistogramViewer As New DoDHistogramViewerClass(myZedGraphControl, theUnits)
            myHistogramViewer.refresh(changeHisto.m_RawAreaHist, changeHisto.m_ThresholdedAreaHist, True, theUnits)
            myHistogramViewer.Save(AreaGraphPath, ChartWidth, ChartHeight)

            'myHistogramViewer.Hist2 = StatsData.m_RawVolumeHist
            'myHistogramViewer.Hist1 = StatsData.m_ThresholdedVolumeHist
            'myHistogramViewer.yLabel = "Volume " & GISDataStructures.GetLinearUnitsAsString(eLinearUnits, True, 3)
            myHistogramViewer.refresh(changeHisto.m_RawVolumeHist, changeHisto.m_ThresholdedVolumeHist, False, theUnits)
            myHistogramViewer.Save(VolumeGraphPath, ChartWidth, ChartHeight)

        End Sub

        Public Sub Save(ByVal GraphPath As String, ByVal ChartWidth As Integer, ByVal ChartHeight As Integer)
            m_Chart.SaveImage(GraphPath, Charting.ChartImageFormat.Png)
        End Sub
    End Class

End Namespace