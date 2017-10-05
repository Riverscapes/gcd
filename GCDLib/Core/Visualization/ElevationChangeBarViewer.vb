Imports naru.math
Imports System.Windows.Forms.DataVisualization.Charting

Namespace Core.Visualization

    Public Class ElevationChangeBarViewer

        Public Enum BarTypes
            Area
            Volume
            Vertical
        End Enum

        Private m_chtControl As Chart
        'Private _ThresholdedHist As Dictionary(Of Double, Double)
        'Private _RawHist As Dictionary(Of Double, Double)
        Private m_sUnits As String

        Public Sub New(ByRef chtControl As Chart, ByVal sUnits As String)
            m_chtControl = chtControl
            m_sUnits = sUnits
        End Sub

        Public Sub Refresh(fErosion As Double, fDeposition As Double, eUnits As naru.math.LinearUnitClass, eType As BarTypes, bAbsolute As Boolean)

            Refresh(fErosion, fDeposition, 0, 0, 0, 0, eUnits, False, False, eType, bAbsolute)
        End Sub

        Public Sub Refresh(fErosion As Double, fDeposition As Double, fNet As Double, fErosionError As Double, fDepositionError As Double, fNetError As Double, eUnits As naru.math.LinearUnitClass, eType As BarTypes, bAbsolute As Boolean)

            Refresh(fErosion, fDeposition, fNet, fErosionError, fDepositionError, fNetError, eUnits, True, True, eType, bAbsolute)
        End Sub

        Private Sub Refresh(fErosion As Double, fDeposition As Double, fNet As Double, fErosionError As Double, fDepositionError As Double, fNetError As Double, eUnits As naru.math.LinearUnitClass, bShowErrorBars As Boolean, bShowNet As Boolean, eType As BarTypes, bAbsolute As Boolean)

            'If Not TypeOf _ZedGraph Is ZedGraph.ZedGraphControl Then
            '    Exit Sub
            'End If

            ''setup pane
            'Dim Pane1 As ZedGraph.GraphPane = _ZedGraph.GraphPane
            'Pane1.Title.IsVisible = False
            'Pane1.Legend.IsVisible = False
            'Pane1.Border.IsVisible = False
            'Pane1.CurveList.Clear()
            'Pane1.BarSettings.Type = ZedGraph.BarType.Overlay
            'Pane1.BarSettings.MinClusterGap = 0.5

            ''Set up gridlines to the plot
            'Pane1.XAxis.MajorGrid.IsVisible = False
            'Pane1.YAxis.MajorGrid.IsVisible = True
            'Pane1.YAxis.MajorGrid.Color = System.Drawing.Color.Gray
            'Pane1.XAxis.Title.IsVisible = False

            ''Stop zedgraph from detecting magnitude
            'Pane1.YAxis.Scale.MagAuto = False

            'Dim fDisplayErosion As Double = fErosion
            'Dim fDisplayNet As Double = fNet
            'If bAbsolute Then
            '    ' Bars should have their correct sign. Erosion should be negative
            '    ' but the number stored in the project is always positive.
            '    fDisplayErosion = -1 * fDisplayErosion
            'Else
            '    fDisplayNet = Math.Abs(fNet)
            'End If

            'If bShowErrorBars Then
            '    Dim dErosionError As New ZedGraph.PointPairList
            '    dErosionError.Add(1, fDisplayErosion + fErosionError, fDisplayErosion - fErosionError)
            '    dErosionError.Add(2, fDeposition + fDepositionError, fDeposition - fDepositionError)
            '    Debug.Write(fDeposition - fDepositionError)

            '    If bShowNet Then
            '        dErosionError.Add(3, fDisplayNet + fNetError, fDisplayNet - fNetError)
            '    End If

            '    Dim barError As ZedGraph.ErrorBarItem = Pane1.AddErrorBar("error", dErosionError, System.Drawing.Color.Black)
            '    barError.Bar.PenWidth = 2.0F
            '    barError.Bar.Symbol.Size = 10
            'End If

            ''Prepare thresholded data
            'Dim fErosionData As New List(Of Double)
            'Dim fErosionData2 As New ZedGraph.PointPairList
            'fErosionData2.Add(0, fDisplayErosion)
            'fErosionData2.Add(1, 0)


            'fErosionData.Add(fDisplayErosion)
            ''fErosionData.Add(0)
            'If bShowNet Then
            '    ' fErosionData.Add(0)
            '    fErosionData2.Add(2, 0)
            'End If

            'Dim fDepositionData2 As New ZedGraph.PointPairList
            'fDepositionData2.Add(0, 0)
            'fDepositionData2.Add(1, fDeposition)

            'If bShowNet Then
            '    fDepositionData2.Add(2, 0)
            'End If

            '' Create  erosion bars (red)
            'Dim barErosion As ZedGraph.BarItem
            'barErosion = Pane1.AddBar("Erosion", {0}, fErosionData.ToArray, GCD.GCDProject.ProjectManager.ColourErosion)
            'barErosion.Bar.Fill.Type = ZedGraph.FillType.Solid
            'barErosion.Bar.Border.IsVisible = False

            '' Create deposition bars (blue)
            'Dim barDeposition As ZedGraph.BarItem
            'barDeposition = Pane1.AddBar("Deposition", fDepositionData2, GCD.GCDProject.ProjectManager.ColourDeposition)
            'barDeposition.Bar.Fill.Type = ZedGraph.FillType.Solid
            'barDeposition.Bar.Border.IsVisible = False

            'If bShowNet Then
            '    'Create net bars (blue when depositional and red when erosional)
            '    Dim fNetData2 As New ZedGraph.PointPairList
            '    fNetData2.Add(0, 0)
            '    fNetData2.Add(1, 0)
            '    fNetData2.Add(2, fDisplayNet)

            '    Dim cNetcolour As Drawing.Color = Drawing.Color.DarkGreen
            '    If fDeposition > fErosion Then
            '        cNetcolour = GCD.GCDProject.ProjectManager.ColourDeposition
            '    Else
            '        cNetcolour = GCD.GCDProject.ProjectManager.ColourErosion
            '    End If

            '    Dim barNet As ZedGraph.BarItem = Pane1.AddBar("Net", fNetData2, cNetcolour)
            '    barNet.Bar.Fill = New ZedGraph.Fill(cNetcolour)
            '    barNet.Bar.Border.IsVisible = False
            'End If


            'Dim sLabels As String() = {}
            'If bShowNet Then
            '    Select Case eType
            '        Case BarTypes.Area
            '            Throw New Exception("Should not be possible")
            '        Case BarTypes.Volume
            '            sLabels = {"Total" & vbNewLine & "Volume of" & vbNewLine & "Erosion", "Total" & vbNewLine & "Volume of" & vbNewLine & "Deposition", "Total" & vbNewLine & "Net Volume" & vbNewLine & "Difference"}
            '            Pane1.YAxis.Title.Text = "Volume " & NumberFormatting.GetUnitsAsString(eUnits, True, 3)
            '            Pane1.YAxis.Scale.Format = "#,#"

            '        Case BarTypes.Vertical
            '            'sLabels = {"Average", "Next", "Next"}
            '            sLabels = {"Average" & vbNewLine & "Depth Of" & vbNewLine & "Erosion", "Average" & vbNewLine & "Depth Of" & vbNewLine & "Deposition", "Avg. Total" & vbNewLine & "Thickness" & vbNewLine & "Difference"}
            '            Pane1.YAxis.Title.Text = "Elevation " & NumberFormatting.GetUnitsAsString(eUnits, True, 1)
            '            Pane1.YAxis.Scale.Format = "#,##0.00"

            '    End Select
            'Else
            '    Select Case eType
            '        Case BarTypes.Area
            '            sLabels = {"Total" & vbNewLine & "Area of" & vbNewLine & "Erosion", "Total" & vbNewLine & "Area of" & vbNewLine & "Deposition"}
            '            Pane1.YAxis.Title.Text = "Area " & NumberFormatting.GetUnitsAsString(eUnits, True, 2)
            '            Pane1.YAxis.Scale.Format = "#,#"

            '        Case Else
            '            Throw New Exception("Should not be possible")
            '    End Select

            'End If
            'Pane1.XAxis.Type = ZedGraph.AxisType.Text
            'Pane1.XAxis.Scale.TextLabels = sLabels
            'Pane1.XAxis.MajorTic.IsBetweenLabels = True

            '_ZedGraph.AxisChange()
            '_ZedGraph.Refresh()
        End Sub

        Public Sub Save(sFilePath As String, nChartWidth As Integer, nChartHeight As Integer, nDPI As Integer)
            m_chtControl.SaveImage(sFilePath, ChartImageFormat.Png)
        End Sub

    End Class

End Namespace
