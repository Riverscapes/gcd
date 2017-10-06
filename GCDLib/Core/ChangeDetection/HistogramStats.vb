Imports System.IO

Namespace Core.ChangeDetection

    Public Class HistogramStats
        ''' <summary>
        ''' Loads a histogram from a CSV
        ''' </summary>
        ''' <param name="CsvFilepath">Input path to CSV file</param>
        ''' <param name="areaHist">Return variable for area histogram</param>
        ''' <param name="volumeHist">Return variable for volumehistogram</param>
        ''' <remarks></remarks>
        Public Shared Sub LoadHistogram(ByVal CsvFilePath As String, ByRef areaHist As Dictionary(Of Double, Double), ByRef volumeHist As Dictionary(Of Double, Double))
            'Implemented reading csv file
            'c#/SWIG/GDAL passes the histogram arrays as double pointers
            'To avoid reprogramming in VB, opted to just read and parse csv
            'Frank - Mar 11, 2011

            If Not File.Exists(CsvFilePath) Then
                Throw New Exception("Csv file """ & CsvFilePath & """ does not exist")
            End If

            Dim line As String
            Dim csvdata As String()

            Dim readFile As System.IO.TextReader
            Try
                readFile = New StreamReader(CsvFilePath)
            Catch ex As IOException
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

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Raw">Raw histogram</param>
        ''' <param name="fThreshold">Threshold value</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ThresholdHistogram(ByVal Raw As Dictionary(Of Double, Double), ByVal fThreshold As Double) As Dictionary(Of Double, Double)
            Dim ThresholdedHist As New Dictionary(Of Double, Double)
            Dim elevation As Double
            Dim area As Double
            For Each kvp As KeyValuePair(Of Double, Double) In Raw
                elevation = kvp.Key
                area = kvp.Value
                If elevation <= -fThreshold Or elevation >= fThreshold Then
                    ThresholdedHist.Add(elevation, area)
                End If
            Next
            Return ThresholdedHist
        End Function

    End Class

End Namespace