Imports System.Windows.Forms.DataVisualization
Imports UnitsNet

Namespace Core.Visualization

    Public Class DoDHistogramViewerClass

        ' Names for data series
        Private Const EROSION As String = "Erosion"
        Private Const DEPOSITION As String = "Deposition"
        Private Const RAW As String = "Raw"

        Private m_Chart As Charting.Chart

        ' Raw histogram data
        Private m_Raw As Dictionary(Of Double, ElevationChangeDataPoint)
        Private m_Thresholded As Dictionary(Of Double, ElevationChangeDataPoint)
        Private m_LinearDataUnits As Units.LengthUnit

        Private m_LinearDisplayUnits As Units.LengthUnit
        Private m_AreaDisplayUnits As Units.AreaUnit
        Private m_VolumeDisplayUnits As Units.VolumeUnit

        Private ReadOnly Property AreaDataUnits
            Get
                Return GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(m_LinearDataUnits)
            End Get
        End Property

        Private ReadOnly Property VolumeDataUnits
            Get
                Return GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(m_LinearDataUnits)
            End Get
        End Property

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
            LoadHistogram(sRawHistogram, m_Raw)
            LoadHistogram(sThreshHistogram, m_Thresholded)
            m_LinearDataUnits = linearDataUnits

            m_LinearDisplayUnits = m_LinearDataUnits
            m_AreaDisplayUnits = AreaDataUnits
            m_VolumeDisplayUnits = VolumeDataUnits

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

            RefreshDisplay(True, linearDataUnits, GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(linearDataUnits), GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(linearDataUnits))

        End Sub

        Public Sub SetChartType(bArea As Boolean)
            RefreshDisplay(bArea, m_LinearDisplayUnits, m_AreaDisplayUnits, m_VolumeDisplayUnits)
        End Sub

        Public Sub RefreshDisplay(ByVal bArea As Boolean, ByVal linearDisplayUnits As UnitsNet.Units.LengthUnit, areaDisplayUnits As UnitsNet.Units.AreaUnit, volumeDisplayUnits As UnitsNet.Units.VolumeUnit)

            ' Store the display units so that the user can switch between area and volume easily
            m_LinearDisplayUnits = linearDisplayUnits
            m_AreaDisplayUnits = areaDisplayUnits
            m_VolumeDisplayUnits = volumeDisplayUnits

            Dim histoData As List(Of HistogramDisplayDataPoint) = GetDisplayValues(bArea, linearDisplayUnits, areaDisplayUnits, volumeDisplayUnits)

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

        Private Function GetDisplayValues(bArea As Boolean, linearDisplayUnits As Units.LengthUnit, areaDisplayUnits As Units.AreaUnit, volDisplayUnits As Units.VolumeUnit) As List(Of HistogramDisplayDataPoint)

            ' Note that the key to this dictionary is the histogram elevation values in their ORIGINAL units
            ' while the elevation properties of the HistogramDisplayDataPoint should be in the display units
            Dim lDisplayData As New Dictionary(Of Double, HistogramDisplayDataPoint)

            ' There must always be a thresholded histogram that is displayed red/blue
            For Each dataPoint As ElevationChangeDataPoint In m_Thresholded.Values
                lDisplayData(dataPoint.Elevation) = New HistogramDisplayDataPoint(dataPoint.GetElevation(m_LinearDataUnits, linearDisplayUnits))

                If bArea Then
                    lDisplayData(dataPoint.Elevation).Erosion = dataPoint.AreaErosion(AreaDataUnits, areaDisplayUnits)
                    lDisplayData(dataPoint.Elevation).Deposition = dataPoint.AreaDeposition(AreaDataUnits, areaDisplayUnits)
                Else
                    lDisplayData(dataPoint.Elevation).Erosion = dataPoint.VolumeErosion(VolumeDataUnits, volDisplayUnits)
                    lDisplayData(dataPoint.Elevation).Deposition = dataPoint.VolumeDeposition(VolumeDataUnits, volDisplayUnits)
                End If
            Next

            ' If there's a raw histogram then load the values. These will be displayed as a grey column
            If Not m_Raw Is Nothing Then
                For Each item As KeyValuePair(Of Double, ElevationChangeDataPoint) In m_Raw
                    If Not lDisplayData.ContainsKey(item.Key) Then
                        lDisplayData(item.Key) = New HistogramDisplayDataPoint(Length.From(item.Value.Elevation, m_LinearDataUnits).As(linearDisplayUnits))
                    End If

                    If item.Key < 0 Then
                        If bArea Then
                            lDisplayData(item.Key).Raw = Math.Max(0, item.Value.AreaChange(AreaDataUnits, areaDisplayUnits) - lDisplayData(item.Key).Erosion)
                        Else
                            lDisplayData(item.Key).Raw = Math.Max(0, item.Value.VolumeChange(volDisplayUnits, volDisplayUnits) - lDisplayData(item.Key).Erosion)
                        End If
                    Else
                        If bArea Then
                            lDisplayData(item.Key).Raw = Math.Max(0, item.Value.AreaChange(AreaDataUnits, areaDisplayUnits) - lDisplayData(item.Key).Deposition)
                        Else
                            lDisplayData(item.Key).Raw = Math.Max(0, item.Value.VolumeChange(volDisplayUnits, volDisplayUnits) - lDisplayData(item.Key).Deposition)
                        End If
                    End If
                Next
            End If

            Return lDisplayData.Values.ToList()

        End Function

        Public Sub ExportCharts(ByVal AreaGraphPath As String, ByVal VolumeGraphPath As String, ByVal ChartWidth As Integer, ByVal ChartHeight As Integer)

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(AreaGraphPath)) Then
                Dim ex As New Exception("The output folder for the GCD area graph does not exist.")
                ex.Data("Area Graph Path") = AreaGraphPath
            End If

            If Not IO.Directory.Exists(IO.Path.GetDirectoryName(VolumeGraphPath)) Then
                Dim ex As New Exception("The output folder for the GCD volume graph does not exist.")
                ex.Data("volume Graph Path") = VolumeGraphPath
            End If

            RefreshDisplay(True, m_LinearDataUnits, AreaDataUnits, VolumeDataUnits)
            m_Chart.SaveImage(AreaGraphPath, Charting.ChartImageFormat.Png)

            RefreshDisplay(False, m_LinearDataUnits, AreaDataUnits, VolumeDataUnits)
            m_Chart.SaveImage(VolumeGraphPath, Charting.ChartImageFormat.Png)

        End Sub

        ''' <summary>
        ''' Loads a histogram from a CSV
        ''' </summary>
        ''' <param name="csvFilePath">Input path to CSV file</param>
        ''' <remarks></remarks>
        Private Sub LoadHistogram(ByVal csvFilePath As String, ByRef values As Dictionary(Of Double, ElevationChangeDataPoint))

            Try
                If Not IO.File.Exists(csvFilePath) Then
                    Throw New Exception("Elevation change histogram CSV file does not exist.")
                End If

                values = New Dictionary(Of Double, ElevationChangeDataPoint)

                Using readFile As New IO.StreamReader(csvFilePath)

                    ' skip first headers line
                    Dim line As String = readFile.ReadLine()
                    Dim csvdata As String()
                    While True
                        line = readFile.ReadLine()
                        If line Is Nothing Then
                            Exit While
                        Else
                            csvdata = Split(line, ",")
                            Dim fElevation As Double = Double.Parse(csvdata(0))
                            values(fElevation) = New ElevationChangeDataPoint(fElevation, Double.Parse(csvdata(2)), Double.Parse(csvdata(3)))
                        End If
                    End While
                End Using

            Catch ex As IO.IOException
                Dim ex2 As New Exception("Could not access elevation change histogram file because it is being used by another program.", ex)
                ex2.Data("File Path") = csvFilePath
                Throw ex2
            Catch ex As Exception

            End Try
        End Sub

    End Class

End Namespace