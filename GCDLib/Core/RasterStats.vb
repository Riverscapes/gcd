#Region "Imports"
Imports System.IO
#End Region

Namespace GISCode.GCD

    Public Class RasterStats
        ''' <summary>
        ''' Loads a histogram from a csv
        ''' </summary>
        ''' <param name="CsvFilepath">Path to csv fiel</param>
        ''' <param name="areaHist">return variable for area histogram</param>
        ''' <param name="volumeHist">return variable for volumehistogram</param>
        ''' <remarks></remarks>
        Public Shared Sub LoadHistogram(ByVal CsvFilepath As String, ByRef areaHist As Dictionary(Of Double, Double), ByRef volumeHist As Dictionary(Of Double, Double))
            'Implemented reading csv file
            'c#/SWIG/GDAL passes the histogram arrays as double pointers
            'To avoid reprogramming in VB, opted to just read and parse csv
            'Frank - Mar 11, 2011

            If Not File.Exists(CsvFilepath) Then
                Throw New Exception("Csv file """ & CsvFilepath & """ does not exist")
            End If

            Dim line As String
            Dim csvdata As String()

            Dim readFile As System.IO.TextReader
            Try
                readFile = New StreamReader(CsvFilepath)
            Catch ex As IOException
                'need to limit the length of the file to display properly
                Dim TrimmedFilename As String = GISCode.FileSystem.TrimFilename(CsvFilepath, 80)

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

        Public Shared Function ThresholdHistogram(ByVal Raw As Dictionary(Of Double, Double), ByVal Threshold As Double) As Dictionary(Of Double, Double)
            Dim ThresholdedHist As New Dictionary(Of Double, Double)
            Dim elevation As Double
            Dim area As Double
            For Each kvp As KeyValuePair(Of Double, Double) In Raw
                elevation = kvp.Key
                area = kvp.Value
                If elevation <= -Threshold Or elevation >= Threshold Then
                    ThresholdedHist.Add(elevation, area)
                End If

            Next
            Return ThresholdedHist
        End Function

    End Class

End Namespace