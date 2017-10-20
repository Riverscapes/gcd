Namespace Core.ChangeDetection

    Public Class DoDResultHistograms

        'histograms data
        Public m_RawAreaHist = New Dictionary(Of Double, Double)
        Public m_RawVolumeHist = New Dictionary(Of Double, Double)
        Public m_ThresholdedAreaHist = New Dictionary(Of Double, Double)
        Public m_ThresholdedVolumeHist = New Dictionary(Of Double, Double)

        ''' <summary>
        ''' Load a single histogram into the class
        ''' </summary>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sThresholdedHist As String)
            LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)
        End Sub

        ''' <summary>
        ''' Load raw and thresholded histogram into the class
        ''' </summary>
        ''' <param name="sRawHist">Raw CSV Histogram</param>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sRawHist As String, ByVal sThresholdedHist As String)
            LoadHistogram(sRawHist, m_RawAreaHist, m_RawVolumeHist)
            LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)
        End Sub

        ''' <summary>
        ''' Loads a histogram from a CSV
        ''' </summary>
        ''' <param name="CsvFilepath">Input path to CSV file</param>
        ''' <param name="areaHist">Return variable for area histogram</param>
        ''' <param name="volumeHist">Return variable for volumehistogram</param>
        ''' <remarks></remarks>
        Private Sub LoadHistogram(ByVal CsvFilePath As String, ByRef areaHist As Dictionary(Of Double, Double), ByRef volumeHist As Dictionary(Of Double, Double))
            'Implemented reading csv file
            'c#/SWIG/GDAL passes the histogram arrays as double pointers
            'To avoid reprogramming in VB, opted to just read and parse csv
            'Frank - Mar 11, 2011

            If Not IO.File.Exists(CsvFilePath) Then
                Throw New Exception("Csv file """ & CsvFilePath & """ does not exist")
            End If

            Dim line As String
            Dim csvdata As String()

            Dim readFile As System.IO.TextReader
            Try
                readFile = New IO.StreamReader(CsvFilePath)
            Catch ex As IO.IOException
                'need to limit the length of the file to display properly
                Dim TrimmedFilename As String = naru.os.File.TrimFilename(CsvFilePath, 80)

                ex.Data("UIMessage") = "Could not access the file '" & TrimmedFilename & "' because it is being used by another program."
                Throw ex
            End Try

            'skip first line
            line = readFile.ReadLine()
            While True
                line = readFile.ReadLine()
                If line Is Nothing Then
                    Exit While
                Else
                    csvdata = Split(line, ",")
                    Dim elev As Double = csvdata(0)
                    Dim area As Double = csvdata(2)
                    Dim volume As Double = csvdata(3)
                    areaHist(elev) = area
                    volumeHist(elev) = volume
                End If
            End While
            readFile.Close()
        End Sub

    End Class

End Namespace