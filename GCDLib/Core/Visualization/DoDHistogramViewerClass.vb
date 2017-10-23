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


        Private Class HistogramData
            Private m_elevation As Double
            Private m_erosion As Double
            Private m_deposition As Double
            Private m_raw As Double

            Public Property Elevation As Double
                Get
                    Return m_elevation
                End Get
                Set(value As Double)
                    m_elevation = value
                End Set
            End Property

            Public Property Deposition As Double
                Get
                    Return m_deposition
                End Get
                Set(value As Double)
                    m_deposition = value
                End Set
            End Property

            Public Property Erosion As Double
                Get
                    Return m_erosion
                End Get
                Set(value As Double)
                    m_erosion = value
                End Set
            End Property

            Public Property Raw As Double
                Get
                    Return m_raw
                End Get
                Set(value As Double)
                    m_raw = value
                End Set
            End Property

            Public Sub New(fElevation As Double)
                m_elevation = fElevation
                m_erosion = 0
                m_deposition = 0
            End Sub
        End Class


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
                series.ChartType = Charting.SeriesChartType.StackedColumn
                series.Color = aDef.Value
                series.ChartArea = m_Chart.ChartAreas.First().Name

                'If series.Name = RAW Then
                '    series.YAxisType = Charting.AxisType.Secondary
                'End If

            Next

            With m_Chart.ChartAreas(0).AxisX
                .Title = String.Format("Elevation Change ({0})", eUnits)
                .MajorGrid.LineColor = Drawing.Color.LightSlateGray
                .MinorTickMark.Enabled = True
            End With

            With m_Chart.ChartAreas(0).AxisY
                .MajorGrid.LineColor = Drawing.Color.LightSlateGray
                .MinorTickMark.Enabled = True
            End With

        End Sub

        Public Sub refresh(ByVal dRawHistogram As Dictionary(Of Double, Double),
                           ByVal dThresholdedHistogram As Dictionary(Of Double, Double),
                           ByVal bArea As Boolean,
                           ByVal eUnits As UnitsNet.Units.LengthUnit)

            Dim histoData As New SortedDictionary(Of Double, HistogramData)

            For Each item As KeyValuePair(Of Double, Double) In dThresholdedHistogram
                If Not histoData.ContainsKey(item.Key) Then
                    histoData(item.Key) = New HistogramData(item.Key)
                End If

                If item.Key < 0 Then
                    histoData(item.Key).Erosion = item.Value
                Else
                    histoData(item.Key).Deposition = item.Value
                End If
            Next

            For Each item As KeyValuePair(Of Double, Double) In dRawHistogram
                If Not histoData.ContainsKey(item.Key) Then
                    histoData(item.Key) = New HistogramData(item.Key)
                End If

                If item.Key < 0 Then
                    If item.Value > histoData(item.Key).Erosion Then
                        histoData(item.Key).Raw = item.Value - histoData(item.Key).Erosion
                    End If
                Else
                    If item.Value > histoData(item.Key).Deposition Then
                        histoData(item.Key).Raw = item.Value - histoData(item.Key).Deposition
                    End If
                End If
            Next

            Dim histoList As New List(Of HistogramData)(histoData.Values)

            m_Chart.Series.FindByName(EROSION).Points.DataBindXY(histoList, "elevation", histoList, "erosion")
            m_Chart.Series.FindByName(DEPOSITION).Points.DataBindXY(histoList, "elevation", histoList, "deposition")
            m_Chart.Series.FindByName(RAW).Points.DataBindXY(histoList, "elevation", histoList, "raw")

            If bArea Then
                m_Chart.ChartAreas(0).AxisY.Title = String.Format("Area ({0}²)", UnitsNet.Length.GetAbbreviation(eUnits))
            Else
                m_Chart.ChartAreas(0).AxisY.Title = String.Format("Volume ({0}³)", UnitsNet.Length.GetAbbreviation(eUnits))
            End If

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