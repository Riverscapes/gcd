Namespace Core.ChangeDetection

    Public Class DoDResultHistograms

        ' Raw histogram data
        Private m_Raw As Dictionary(Of Double, ElevationChangeDataPoint)
        Private m_Thresholded As Dictionary(Of Double, ElevationChangeDataPoint)
        Private m_LinearDataUnits As UnitsNet.Units.LengthUnit

        Public Function GetAreaDisplayValues(eLinearUnits As UnitsNet.Units.LengthUnit, eAreaUnits As UnitsNet.Units.AreaUnit)
            Return DisplayValues(True, eLinearUnits, eAreaUnits, Nothing)
        End Function

        Public Function GetVolumeDisplayValues(eLinearUnits As UnitsNet.Units.LengthUnit, eVolumeUnits As UnitsNet.Units.VolumeUnit)
            Return DisplayValues(False, eLinearUnits, Nothing, eVolumeUnits)
        End Function

        Private Function DisplayValues(bArea As Boolean, eLinearUnits As UnitsNet.Units.LengthUnit, eAreaUnits As UnitsNet.Units.AreaUnit, eVolumeUnits As UnitsNet.Units.VolumeUnit) As List(Of Visualization.HistogramDisplayDataPoint)

            ' Note that the key to this dictionary is the raw elevation values from the DoD histogram data
            ' while the elevation properties of the HistogramDisplayDataPoint should be in the display units
            Dim lDisplayData As New Dictionary(Of Double, Visualization.HistogramDisplayDataPoint)

            If Not m_Thresholded Is Nothing Then
                For Each item As KeyValuePair(Of Double, ElevationChangeDataPoint) In m_Thresholded
                    lDisplayData(item.Key) = New Visualization.HistogramDisplayDataPoint(item.Value.Elevation(eLinearUnits))
                    lDisplayData(item.Key).Erosion = IIf(bArea, item.Value.AreaErosion(eAreaUnits), item.Value.VolumeErosion(eAreaUnits))
                    lDisplayData(item.Key).Deposition = IIf(bArea, item.Value.AreaDeposition(eAreaUnits), item.Value.VolumeDepsition(eAreaUnits))
                Next
            End If

            If Not m_Raw Is Nothing Then
                For Each item As KeyValuePair(Of Double, ElevationChangeDataPoint) In m_Raw
                    If Not lDisplayData.ContainsKey(item.Key) Then
                        lDisplayData(item.Key) = New Visualization.HistogramDisplayDataPoint(item.Value.Elevation(eLinearUnits))
                    End If

                    If bArea Then
                        lDisplayData(item.Key).Raw = Math.Max(0, item.Value.AreaErosion(eAreaUnits) - lDisplayData(item.Key).Erosion)
                    Else
                        lDisplayData(item.Key).Raw = Math.Max(0, item.Value.VolumeErosion(eAreaUnits) - lDisplayData(item.Key).Erosion)
                    End If
                Next
            End If

            Return lDisplayData.Values.ToList()

        End Function

        ''' <summary>
        ''' Load a single histogram into the class
        ''' </summary>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sThresholdedHist As String, linearDataUnits As UnitsNet.Units.LengthUnit)
            m_LinearDataUnits = linearDataUnits
            LoadHistogram(sThresholdedHist, m_Thresholded, linearDataUnits)
        End Sub

        ''' <summary>
        ''' Load raw and thresholded histogram into the class
        ''' </summary>
        ''' <param name="sRawHist">Raw CSV Histogram</param>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sRawHist As String, ByVal sThresholdedHist As String, linearDataUnits As UnitsNet.Units.LengthUnit)
            m_LinearDataUnits = linearDataUnits
            LoadHistogram(sRawHist, m_Raw, linearDataUnits)
            LoadHistogram(sThresholdedHist, m_Thresholded, linearDataUnits)
        End Sub

        ''' <summary>
        ''' Loads a histogram from a CSV
        ''' </summary>
        ''' <param name="csvFilePath">Input path to CSV file</param>
        ''' <param name="areaHist">Return variable for area histogram</param>
        ''' <param name="volumeHist">Return variable for volumehistogram</param>
        ''' <remarks></remarks>
        Private Sub LoadHistogram(ByVal csvFilePath As String, ByRef values As Dictionary(Of Double, ElevationChangeDataPoint), linearDataUnits As UnitsNet.Units.LengthUnit)

            Dim areaDataUnits As UnitsNet.Units.AreaUnit = GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(linearDataUnits)
            Dim volDataUnits As UnitsNet.Units.VolumeUnit = GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(linearDataUnits)

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
                            values(fElevation) = New ElevationChangeDataPoint(fElevation, linearDataUnits)
                            values(fElevation).AreaChange = UnitsNet.Area.From(Double.Parse(csvdata(2)), areaDataUnits)
                            values(fElevation).VolumeChange = UnitsNet.Volume.From(Double.Parse(csvdata(3)), volDataUnits)
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