Imports System.Windows.Forms.DataVisualization

Namespace Core.Visualization

    Public Class ElevationChangeBarViewer

        Public Enum BarTypes
            Area
            Volume
            Vertical
        End Enum

        Private Enum SeriesType
            Erosion
            Depositon
            Net
        End Enum

        Private m_chtControl As Charting.Chart
        'Private _ThresholdedHist As Dictionary(Of Double, Double)
        'Private _RawHist As Dictionary(Of Double, Double)
        Private m_eLinearUnits As UnitsNet.Units.LengthUnit

        Public Sub New(ByRef chtControl As Charting.Chart, eLinearUnits As UnitsNet.Units.LengthUnit)
            m_chtControl = chtControl
            m_eLinearUnits = eLinearUnits

            chtControl.ChartAreas.Clear()
            chtControl.Series.Clear()

            chtControl.ChartAreas.Add("ElevationChangeBars")
            chtControl.Legends.Clear()

            With chtControl.ChartAreas(0).AxisX
                .MajorGrid.Enabled = False
                .MajorTickMark.Enabled = False
            End With

            With chtControl.ChartAreas(0).AxisY
                .MinorTickMark.Enabled = True
                .MajorGrid.LineColor = Drawing.Color.LightSlateGray
                .MinorGrid.Enabled = True
                .MinorGrid.LineColor = Drawing.Color.LightGray
            End With

        End Sub

        Public Sub Refresh(fErosion As Double, fDeposition As Double, eUnits As UnitsNet.Units.LengthUnit, eType As BarTypes, bAbsolute As Boolean)

            Refresh(fErosion, fDeposition, 0, 0, 0, 0, eUnits, False, False, eType, bAbsolute)
        End Sub

        Public Sub Refresh(fErosion As Double, fDeposition As Double, fNet As Double, fErosionError As Double, fDepositionError As Double, fNetError As Double, eUnits As UnitsNet.Units.LengthUnit, eType As BarTypes, bAbsolute As Boolean)

            Refresh(fErosion, fDeposition, fNet, fErosionError, fDepositionError, fNetError, eUnits, True, True, eType, bAbsolute)
        End Sub

        Private Sub Refresh(fErosion As Double, fDeposition As Double, fNet As Double, fErosionError As Double, fDepositionError As Double, fNetError As Double, eUnits As UnitsNet.Units.LengthUnit, bShowErrorBars As Boolean, bShowNet As Boolean, eType As BarTypes, bAbsolute As Boolean)

            m_chtControl.Series.Clear()

            If bAbsolute Then
                ' Bars should have their correct sign. Erosion should be negative
                ' but the number stored in the project is always positive.
                fErosion = -1 * fErosion
            Else
                fNet = Math.Abs(fNet)
            End If

            Dim sYAxisLabel As String = String.Empty
            Select Case eType
                Case BarTypes.Area
                    sYAxisLabel = String.Format("Area ({0}²)", UnitsNet.Length.GetAbbreviation(eUnits))
                Case BarTypes.Volume
                    sYAxisLabel = String.Format("Volume ({0}³)", UnitsNet.Length.GetAbbreviation(eUnits))
                Case BarTypes.Vertical
                    sYAxisLabel = String.Format("Elevation ({0})", UnitsNet.Length.GetAbbreviation(eUnits))
            End Select
            m_chtControl.ChartAreas(0).AxisY.Title = sYAxisLabel

            Dim dSeries As New Dictionary(Of String, Drawing.Color) From {{"Erosion", My.Settings.Erosion}, {"Depsotion", My.Settings.Depsoition}}
            If bShowNet Then
                dSeries.Add("Net", Drawing.Color.Black)
            End If

            Dim errSeries As Charting.Series = m_chtControl.Series.Add("erosion")
            errSeries.Color = Drawing.Color.Red
            errSeries.ChartArea = m_chtControl.ChartAreas.First.Name
            errSeries.ChartType = Charting.SeriesChartType.StackedColumn
            errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), fErosion)
            errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), 0)

            Dim depSeries As Charting.Series = m_chtControl.Series.Add("deposition")
            depSeries.Color = Drawing.Color.Blue
            depSeries.ChartArea = m_chtControl.ChartAreas.First.Name
            depSeries.ChartType = Charting.SeriesChartType.StackedColumn
            depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), 0)
            depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), fDeposition)

            If bShowNet Then
                Dim netSeries As Charting.Series = m_chtControl.Series.Add("net")
                netSeries.Color = IIf(fNet >= 0, Drawing.Color.Blue, Drawing.Color.Red)
                netSeries.ChartArea = m_chtControl.ChartAreas.First.Name
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), 0)
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), 0)
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), fNet)

                errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), 0)
                depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), 0)
            End If

            m_chtControl.ChartAreas(0).RecalculateAxesScale()
            m_chtControl.AlignDataPointsByAxisLabel()

        End Sub

        Private Function GetXAxisLabel(eBarType As BarTypes, eSeriesType As SeriesType)

            Dim sBarType As String = String.Empty
            Select Case eBarType
                Case BarTypes.Area
                    sBarType = "Total\nArea"
                Case BarTypes.Volume
                    sBarType = "Total\nVolume"
                Case BarTypes.Vertical
                    sBarType = "Average\nDepth"
            End Select

            Dim sSeriesType As String = String.Empty
            Select Case eSeriesType
                Case SeriesType.Erosion
                    sSeriesType = "Erosion"
                Case SeriesType.Depositon
                    sSeriesType = "Deposition"
                Case SeriesType.Net
                    If eBarType = BarTypes.Volume Then
                        Return String.Format("Total{0}Net Volume{0}Difference", Environment.NewLine)
                    ElseIf eBarType = BarTypes.Vertical Then
                        Return String.Format("Avg. Total{0}Thickness{0}Difference", Environment.NewLine)
                    End If
            End Select

            Return String.Format("{1} of{0}{2}", Environment.NewLine, sBarType, sSeriesType)

        End Function

        Public Sub Save(sFilePath As String, nChartWidth As Integer, nChartHeight As Integer, nDPI As Integer)
            m_chtControl.SaveImage(sFilePath, Charting.ChartImageFormat.Png)
        End Sub

    End Class

End Namespace
