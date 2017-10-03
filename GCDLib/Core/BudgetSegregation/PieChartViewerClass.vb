
Namespace GISCode.GCD.BudgetSegregation

    Public Class PieChartViewerClass

        Private _MaskStats As Dictionary(Of String, ChangeDetection.StatsDataClass)
        Private m_PieChart As Windows.Forms.DataVisualization.Charting.Chart
        Private _MaskName As String = ""
        Private _Statistics As String = ""
        Private _Legend As Boolean = False

        Public Property MaskName As String
            Get
                Return _MaskName
            End Get
            Set(value As String)
                _MaskName = value
            End Set
        End Property

        Public Property Statistics As String
            Get
                Return _Statistics
            End Get
            Set(value As String)
                _Statistics = value
            End Set
        End Property

        Public Property Legend As Boolean
            Get
                Return _Legend
            End Get
            Set(value As Boolean)
                _Legend = value
            End Set
        End Property

        Public Sub New(ByRef theChart As Windows.Forms.DataVisualization.Charting.Chart, MaskStats As Dictionary(Of String, ChangeDetection.StatsDataClass))
            _MaskStats = MaskStats
            '_ZedGraph = ZedGraphPieChart
            m_PieChart = theChart
            refresh()
        End Sub

        Public Sub refresh()
            '_ZedGraph.GraphPane.CurveList.Clear()
            m_PieChart.Series.Clear()

            'setup pie chart
            'based on example from http://www.resolverhacks.net/zedgraph_basic.html
            Dim color_num As Integer = 0
            'Dim pane As ZedGraph.GraphPane = _ZedGraph.GraphPane

            For Each kvp As KeyValuePair(Of String, ChangeDetection.StatsDataClass) In _MaskStats
                Dim MaskName As String = kvp.Key
                Dim MaskStat As ChangeDetection.StatsDataClass = kvp.Value
                Dim value As Double

                'get value based on statistics chosen
                Select Case Statistics
                    Case "Total Area"
                        value = MaskStat.AreaDeposition_Thresholded + MaskStat.AreaErosion_Thresholded
                    Case "Total Volume Change"
                        value = MaskStat.VolumeOfDifference_Thresholded
                    Case "Total Erosion Area"
                        value = MaskStat.AreaErosion_Thresholded
                    Case "Total Deposition Area"
                        value = MaskStat.AreaDeposition_Thresholded
                    Case "Total Erosion Volume"
                        value = MaskStat.VolumeErosion_Thresholded
                    Case "Total Deposition Volume"
                        value = MaskStat.VolumeDeposition_Thresholded
                    Case Else
                        value = MaskStat.VolumeOfDifference_Thresholded
                End Select

                Dim displacement As Double = 0

                'Dim pieitem As ZedGraph.PieItem = pane.AddPieSlice(value, GetRandomColour, 0, MaskName)
                If MaskName = _MaskName Then
                    'displace pie chart to highlight
                    'pieitem.Displacement = 0.5
                    'pieitem.LabelType = ZedGraph.PieLabelType.Percent
                    'pieitem.LabelType = ZedGraph.PieLabelType.None
                Else
                    'pieitem.LabelType = ZedGraph.PieLabelType.None

                End If
            Next

            'pane.YAxis.IsVisible = False
            'pane.XAxis.IsVisible = False
            'pane.Title.IsVisible = False
            'pane.Legend.Position = ZedGraph.LegendPos.Right
            'pane.Legend.IsVisible = _Legend
            'pane.Legend.FontSpec.Size = 20
            'pane.Title.Text = _Statistics
            '_ZedGraph.Refresh()
            
        End Sub

        ''' <summary>
        ''' Return a random colour
        ''' </summary>
        ''' <returns>Random system colour</returns>
        ''' <remarks>Frank's old code only worked with up to 16 colours. Joe was using classes with
        ''' 25 or so classes and it was breaking. So we decided to use random colours instead.</remarks>
        Private Function GetRandomColour() As Drawing.Color

            Dim MyAlpha As Integer
            Dim MyRed As Integer
            Dim MyGreen As Integer
            Dim MyBlue As Integer
            Dim result As Drawing.Color = Drawing.Color.Black

            Try
                Randomize()
                MyAlpha = CInt(Int((254 * Rnd()) + 0))

                Randomize()
                MyRed = CInt(Int((254 * Rnd()) + 0))

                Randomize()
                MyGreen = CInt(Int((254 * Rnd()) + 0))

                Randomize()
                MyBlue = CInt(Int((254 * Rnd()) + 0))

                result = Drawing.Color.FromArgb(MyAlpha, MyRed, MyGreen, MyBlue)

            Catch ex As Exception
                Debug.Write("Error generating colour. Continuing with black.")
                result = Drawing.Color.Black
            End Try

            Return result

        End Function

        Public Sub ExportCharts(ByVal sOutputFilePath As String)
            m_PieChart.SaveImage(sOutputFilePath, System.Drawing.Imaging.ImageFormat.Png)
        End Sub

    End Class

End Namespace