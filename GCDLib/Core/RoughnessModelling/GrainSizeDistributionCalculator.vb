
Namespace GISCode.GCD

    Public Module GrainSizeDistributionCalculator

#Region "Private Variables"

        'Private _sPolygonFilePath As String
        Private pColumnIndex As List(Of Integer)
        Private pColumnNames As New List(Of String)({"Fines", "Sand", "FineGravel", "CoarseGravel", "Cobbles", "Boulders", "Bedrock"})
        Private pChannelUnitColumnNames As New List(Of String)({"SiteID", "Tier1", "Tier2", "ChannelUnitID"})
        Private pBounds As New Dictionary(Of String, Double) From {{"Fines", 0.06}, {"Sand", 2}, {"FineGravel", 16}, {"CoarseGravel", 64}, {"Cobbles", 256}, {"Boulders", 4000}}

#End Region

#Region "Enumerations"

        Public Enum GrainSizeClasses
            Fines
            Sand
            FineGravel
            CoarseGravel
            Cobbles
            Boulders
            Bedrock
        End Enum

#End Region

#Region "Private Functions/Sub Routines"

        ''' <summary>
        ''' takes string name of grain size class and returns corresponding GrainSizeClasses enumeration value
        ''' </summary>
        ''' <param name="sGrainSizeString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetGrainSizeClassFromString(ByVal sGrainSizeString As String) As GrainSizeClasses

            If String.IsNullOrEmpty(sGrainSizeString) Then
                Return Nothing
            End If

            Select Case sGrainSizeString

                Case "Fines"
                    Return GrainSizeClasses.Fines

                Case "Sand"
                    Return GrainSizeClasses.Sand

                Case "FineGravel"
                    Return GrainSizeClasses.FineGravel

                Case "CoarseGravel"
                    Return GrainSizeClasses.CoarseGravel

                Case "Cobbles"
                    Return GrainSizeClasses.Cobbles

                Case "Boulders"
                    Return GrainSizeClasses.Boulders

                Case "Bedrock"
                    Return GrainSizeClasses.Bedrock

                Case Else
                    Return Nothing

            End Select

        End Function

        ''' <summary>
        ''' simple structure to ease in understanding within code of lower bounds and uppper bounds
        ''' </summary>
        ''' <remarks></remarks>
        Private Structure BoundsPair

            Public LowerBound As Double
            Public UpperBound As Double

        End Structure

        ''' <summary>
        ''' crucial function to calculate grain d
        ''' </summary>
        ''' <param name="x1"></param>
        ''' <param name="x2"></param>
        ''' <param name="y1"></param>
        ''' <param name="y2"></param>
        ''' <param name="grainSize"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Di(ByVal x1 As Double,
                            ByVal x2 As Double,
                            ByVal y1 As Double,
                            ByVal y2 As Double,
                            ByVal grainSize As UInt16) As Double

            Dim pResult As Double = Nothing
            pResult = Math.Pow(10, (Math.Log10(x2) - Math.Log10(x1)) * ((grainSize - y1) / (y2 - y1)) + Math.Log10(x1))
            Return pResult

        End Function

        Private Function GetHeaderColumnIndex(ByVal sHeaderLine As String, ByVal pColumnNameList As List(Of String), ByVal separator As String) As List(Of Integer)

            Dim pColumnIndex As New List(Of Integer)
            Dim headerList As New List(Of String)

            For i As Integer = 0 To sHeaderLine.Split(separator).Count - 1
                headerList.Add(sHeaderLine.Split(separator)(i))
            Next

            For i As Integer = 0 To pColumnNameList.Count - 1
                Dim columnNameLike As String = pColumnNameList(i)
                For j As Integer = 0 To headerList.Count - 1
                    If headerList(j).Replace(" ", "").Contains(columnNameLike) Then
                        pColumnIndex.Add(j)
                    End If
                Next
            Next

            Return pColumnIndex

        End Function

        Private Function GetAuxillaryInfo(ByVal sInputFilePath As String, ByVal separator As String) As AuxillarySiteInfo

            If String.IsNullOrEmpty(sInputFilePath) Then
                Return Nothing
            End If

            Dim pColumnNamesSiteInfo As New List(Of String)({"SiteID", "SampleDate", "StreamName"})

            Using grainSizeFileReader As New IO.StreamReader(sInputFilePath)

                Dim headerLine As String = grainSizeFileReader.ReadLine()
                Dim headerList As New List(Of String)

                Dim pColumnIndex As List(Of Integer) = GetHeaderColumnIndex(headerLine, pColumnNamesSiteInfo, ",")

                Do While (grainSizeFileReader.Peek() > -1)
                    Dim currentLine = grainSizeFileReader.ReadLine

                    Dim pAuxillaryInfo As New AuxillarySiteInfo()
                    If pColumnIndex.Count = 3 Then

                        pAuxillaryInfo.SiteID = currentLine.Split(separator)(pColumnIndex(0))
                        pAuxillaryInfo.SampleDate = currentLine.Split(separator)(pColumnIndex(1))
                        pAuxillaryInfo.StreamName = currentLine.Split(separator)(pColumnIndex(2))

                    Else
                        Return Nothing
                    End If

                    If Not String.IsNullOrEmpty(pAuxillaryInfo.SiteID) And Not String.IsNullOrEmpty(pAuxillaryInfo.SampleDate) And Not String.IsNullOrEmpty(pAuxillaryInfo.StreamName) Then
                        Return pAuxillaryInfo
                    End If

                Loop


            End Using

            Return Nothing


        End Function

        Private Function GetUpperBoundsColumns(ByVal pGrainSizeClass As GrainSizeClasses) As BoundsPair

            Dim boundsPair As BoundsPair = New BoundsPair

            Select Case pGrainSizeClass

                Case GrainSizeClasses.Fines
                    boundsPair.LowerBound = pBounds("Fines")
                    boundsPair.UpperBound = pBounds("Sand")
                    Return boundsPair

                Case GrainSizeClasses.Sand
                    boundsPair.LowerBound = pBounds("Sand")
                    boundsPair.UpperBound = pBounds("FineGravel")
                    Return boundsPair

                Case GrainSizeClasses.FineGravel
                    boundsPair.LowerBound = pBounds("FineGravel")
                    boundsPair.UpperBound = pBounds("CoarseGravel")
                    Return boundsPair

                Case GrainSizeClasses.CoarseGravel
                    boundsPair.LowerBound = pBounds("CoarseGravel")
                    boundsPair.UpperBound = pBounds("Cobbles")
                    Return boundsPair

                Case GrainSizeClasses.Cobbles
                    boundsPair.LowerBound = pBounds("Cobbles")
                    boundsPair.UpperBound = pBounds("Boulders")
                    Return boundsPair

                Case GrainSizeClasses.Boulders
                    Return Nothing

                Case GrainSizeClasses.Bedrock
                    Return Nothing

                Case Else
                    Return Nothing

            End Select


        End Function

        Private Function GetColumnIndex(ByVal columnNameLikeList As List(Of String), ByVal headerLine As String, ByVal separator As String) As Dictionary(Of String, Integer)

            If String.IsNullOrEmpty(headerLine) Then
                Return Nothing
            End If

            Dim pDictionaryColumnNamesAndIndex As New Dictionary(Of String, Integer)
            Dim headerList As New List(Of String)

            For i As Integer = 0 To headerLine.Split(separator).Count - 1
                headerList.Add(headerLine.Split(separator)(i))
            Next

            For i As Integer = 0 To columnNameLikeList.Count - 1
                Dim columnNameLike As String = columnNameLikeList(i)
                For j As Integer = 0 To headerList.Count - 1
                    If headerList(j).Replace(" ", "").Contains(columnNameLike) Then
                        pDictionaryColumnNamesAndIndex.Add(columnNameLike, j)
                    End If
                Next
            Next

            Return pDictionaryColumnNamesAndIndex

        End Function

        Private Function CalculateCumulativeSum(ByVal pGrainSizeSampleDictionary As Dictionary(Of GrainSizeClasses, UInteger?)) As Dictionary(Of GrainSizeClasses, UInteger)

            Dim pCumulativeSumGrainSizeSampleDictionary As New Dictionary(Of GrainSizeClasses, UInteger)

            Dim pKeys As System.Collections.ICollection = pGrainSizeSampleDictionary.Keys
            Dim previousKey As String = String.Empty

            'Check here for no values in pGrainSizeSampleDictionary

            For Each k In pKeys
                If k = GrainSizeClasses.Fines Then
                    pCumulativeSumGrainSizeSampleDictionary.Add(k, pGrainSizeSampleDictionary(k))
                    previousKey = k
                Else
                    pCumulativeSumGrainSizeSampleDictionary.Add(k, pCumulativeSumGrainSizeSampleDictionary(previousKey) + pGrainSizeSampleDictionary(k))
                    previousKey = k
                End If
            Next

            Return pCumulativeSumGrainSizeSampleDictionary

        End Function

        Private Function CreateGrainSizeSampleDictionary(ByVal pColumnDictionary As Dictionary(Of String, Integer), ByVal sSampleRecord As String, ByVal sFileSeparator As String) As Dictionary(Of GrainSizeClasses, UInteger?)

            Dim grainSizeSampleList As New Dictionary(Of GrainSizeClasses, UInteger?)

            Dim pKeys As System.Collections.ICollection = pColumnDictionary.Keys

            For Each k In pKeys
                Dim grainSizeSample As String = sSampleRecord.Split(sFileSeparator)(pColumnDictionary(k)).Replace("""", "")
                If Integer.TryParse(grainSizeSample, grainSizeSample) Then
                    grainSizeSampleList.Add(GetGrainSizeClassFromString(k), grainSizeSample)
                Else
                    Return Nothing
                End If
            Next

            Return grainSizeSampleList

        End Function

        Private Sub AssessFinesColumn(ByVal pCumulativeSumDictionary As Dictionary(Of String, UInteger),
                                             ByRef D16 As Double,
                                             ByRef D50 As Double,
                                             ByRef D84 As Double,
                                             ByRef D90 As Double)

            If Not D16 = Nothing And Not D50 = Nothing And Not D84 = Nothing And Not D90 = Nothing Then
                Exit Sub
            End If

            Select Case pCumulativeSumDictionary(GrainSizeClasses.Fines)

                Case Is >= 90
                    D16 = 0.06
                    D50 = 0.06
                    D84 = 0.06
                    D90 = 0.06
                    Exit Sub

                Case 84 To 90
                    D16 = 0.06
                    D50 = 0.06
                    D84 = 0.06

                    'Check to see if 90 is in a contained in pCumulativeSum(ct) and pCumulativeSum(ct + 1) + 1

                    'If it is then calculate D90 and Exit For
                    'Else ct +=1 Continue For

                Case 50 To 84
                    D16 = 0.06
                    D50 = 0.06

                    'Same check as above but for 84 and 90
                    'if true calculate D84 and D90 Exit For
                    'ElseIf check 84
                    'if true calculate D84, ct+=1 continue
                    'Else ct+=1 continue For

                Case 16 To 50
                    D16 = 0.06
                    'same checks as above (see python code)

                Case Is < 16
                    Exit Sub

            End Select



        End Sub

        Private Function ConvertToMillimeters(ByVal cellValue As Double)

            Return cellValue / 0.0032808

        End Function

        Private Function ConvertToFeet(ByVal cellValue As Double)

            Return cellValue * 0.0032808

        End Function

        Private Function InitializeGrainSizeDictionary() As Dictionary(Of GrainSizeClasses, UInteger)

            Dim pGrainSizeDictionary As New Dictionary(Of GrainSizeClasses, UInteger) From {{GrainSizeClasses.Fines, 0},
                                                                                            {GrainSizeClasses.Sand, 0},
                                                                                            {GrainSizeClasses.FineGravel, 0},
                                                                                            {GrainSizeClasses.CoarseGravel, 0},
                                                                                            {GrainSizeClasses.Cobbles, 0},
                                                                                            {GrainSizeClasses.Boulders, 0},
                                                                                            {GrainSizeClasses.Bedrock, 0}}
            Return pGrainSizeDictionary

        End Function

        Private Sub AddValueToRasterDictionary(ByRef pCumulativeSumDictionary As Dictionary(Of GrainSizeClasses, UInteger), ByVal pCell As Double, ByVal eLinearUnit As NumberFormatting.LinearUnits)


            If pCell = Double.MinValue Or pCell = Single.MinValue Then
                Exit Sub
            End If

            'Convert to millimeters
            pCell = NumberFormatting.Convert(eLinearUnit, NumberFormatting.LinearUnits.mm, pCell)

            Select Case pCell

                Case Is < 0
                    Exit Sub

                Case 0 To 0.06
                    pCumulativeSumDictionary(GrainSizeClasses.Fines) += 1

                Case 0.06 To 2
                    pCumulativeSumDictionary(GrainSizeClasses.Sand) += 1

                Case 2 To 16
                    pCumulativeSumDictionary(GrainSizeClasses.FineGravel) += 1

                Case 16 To 64
                    pCumulativeSumDictionary(GrainSizeClasses.CoarseGravel) += 1

                Case 64 To 256
                    pCumulativeSumDictionary(GrainSizeClasses.Cobbles) += 1

                Case 256 To 4000
                    pCumulativeSumDictionary(GrainSizeClasses.Boulders) += 1

                Case Is > 4000
                    pCumulativeSumDictionary(GrainSizeClasses.Bedrock) += 1

            End Select

        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pRasterDS"></param>
        ''' <param name="pCumulativeSumDictionary"></param>
        ''' <param name="eLinearUnit"></param>
        ''' <remarks>http://resources.arcgis.com/en/help/arcobjects-net/conceptualhelp/index.html#/How_to_access_pixel_data_using_a_raster_cursor/00010000000t000000/</remarks>
        Private Sub ReadRasterValuesIntoGrainSizeRasterDictionary(ByVal pRasterDS As ESRI.ArcGIS.Geodatabase.IRasterDataset2,
                                                                         ByRef pCumulativeSumDictionary As Dictionary(Of GrainSizeClasses, UInteger),
                                                                         ByVal eLinearUnit As NumberFormatting.LinearUnits)

            Try

                'Create a raster from the raster dataset
                Dim pRaster2 As ESRI.ArcGIS.DataSourcesRaster.IRaster2 = Nothing
                Dim pRasterCursor As ESRI.ArcGIS.Geodatabase.IRasterCursor = Nothing
                pRaster2 = pRasterDS.CreateFullRaster

                'Create a raster cursor using a default pixel block size
                pRasterCursor = pRaster2.CreateCursorEx(Nothing)

                'Loop through each pixel block in the raster
                Dim pRasterBands As ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection = Nothing
                Dim pPixelBlock As ESRI.ArcGIS.DataSourcesRaster.IPixelBlock3 = Nothing
                Dim pTlc As ESRI.ArcGIS.Geodatabase.IPnt = Nothing
                Dim k As Long = Nothing, i As Long = Nothing, j As Long = Nothing
                Dim pPixelArray As System.Array = Nothing
                Dim pValue As Double = Nothing 'Assume the input raster has double value cells
                Dim pPixelBlockWidth As Long = Nothing, pPixelBlockHeight As Long = Nothing
                pRasterBands = pRasterDS

                Do
                    'Get the pixel block and size
                    pPixelBlock = pRasterCursor.PixelBlock
                    pPixelBlockWidth = pPixelBlock.Width
                    pPixelBlockHeight = pPixelBlock.Height
                    pPixelBlock.Mask(255) ' Set no data mask

                    For k = 0 To pRasterBands.Count - 1

                        'Get the pixel array
                        pPixelArray = CType(pPixelBlock.PixelData(k), System.Array)

                        'Loop through each pixel
                        For i = 0 To pPixelBlockWidth - 1
                            For j = 0 To pPixelBlockHeight - 1
                                'Get the pixel value
                                pValue = pPixelArray.GetValue(i, j)

                                'Perform my dictionary stuff here
                                AddValueToRasterDictionary(pCumulativeSumDictionary, pValue, eLinearUnit)

                            Next
                        Next

                        'Set the pixel array to the pixel block
                        pPixelBlock.PixelData(k) = pPixelArray

                    Next

                Loop While pRasterCursor.Next

            Catch ex As Exception
                Dim ex2 As New Exception("Error reading raster values into grain size dictionary.", ex)
                ex2.Data("Raster Dataset") = pRasterDS.CompleteName
            End Try

        End Sub

        Private Sub GrainSizeDistributionLogic(ByVal pCumulativeSumDictionary As Dictionary(Of GrainSizeClasses, UInteger),
                                       ByVal bin As GrainSizeClasses,
                                       ByRef pGrainSizeDistributionResults As GrainSizeDistributionResults)


            Select Case bin

                Case GrainSizeClasses.Boulders

                Case Else

                    Dim cumulativeSum As UInteger = pCumulativeSumDictionary(bin)
                    Dim pBoundsPair As BoundsPair = GetUpperBoundsColumns(bin)
                    Dim pEnumerableRange As System.Collections.Generic.IEnumerable(Of Integer) = Enumerable.Range(pCumulativeSumDictionary(bin), (pCumulativeSumDictionary(bin + 1) - pCumulativeSumDictionary(bin)) + 1)

                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 90 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        pGrainSizeDistributionResults.D50 = 0.06
                        pGrainSizeDistributionResults.D84 = 0.06
                        pGrainSizeDistributionResults.D90 = 0.06
                        'Continue Do
                    End If

                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 84 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        pGrainSizeDistributionResults.D50 = 0.06
                        pGrainSizeDistributionResults.D84 = 0.06

                        If pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        Else
                            'Continue For
                        End If

                    End If

                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 50 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        pGrainSizeDistributionResults.D50 = 0.06

                        If pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        ElseIf pEnumerableRange.Contains(84) Then
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            'Continue For
                        Else
                            'Continue For
                        End If

                    End If

                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 16 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        If pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        ElseIf pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            'Continue For

                        ElseIf pEnumerableRange.Contains(50) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            'Continue For

                        Else
                            'Continue For
                        End If

                    End If

                    If cumulativeSum < 16 And pCumulativeSumDictionary(bin + 1) < 16 Then
                        Exit Sub

                    ElseIf pEnumerableRange.Contains(16) And pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) Then
                        pGrainSizeDistributionResults.D16 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 16)
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(90) And Not pGrainSizeDistributionResults.D16 Is Nothing And Not pGrainSizeDistributionResults.D50 Is Nothing And Not pGrainSizeDistributionResults.D84 Is Nothing Then
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) And Not pGrainSizeDistributionResults.D16 Is Nothing And Not pGrainSizeDistributionResults.D50 Is Nothing Then
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) And Not pGrainSizeDistributionResults.D16 Is Nothing Then
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(16) And pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) Then
                        pGrainSizeDistributionResults.D16 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 16)
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        'Continue For

                    ElseIf pEnumerableRange.Contains(16) Then
                        pGrainSizeDistributionResults.D16 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 16)

                        If pEnumerableRange.Contains(50) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            'Continue For

                        Else
                            ' Continue For

                        End If

                    ElseIf pEnumerableRange.Contains(50) And Not pGrainSizeDistributionResults.D16 Is Nothing Then
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)

                        If pEnumerableRange.Contains(84) Then
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            'Continue For

                        Else
                            'Continue For
                        End If

                    ElseIf pEnumerableRange.Contains(84) Then
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)

                        If pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        Else
                            'Continue For
                        End If

                    ElseIf pEnumerableRange.Contains(90) Then
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    End If

            End Select


        End Sub

        Private Sub GrainSizeDistributionLogic(ByVal pCumulativeSumDictionary As Dictionary(Of GrainSizeClasses, UInteger),
                                                      ByVal bin As GrainSizeClasses,
                                                      ByRef pGrainSizeDistributionResults As GrainSizeDistributionResults,
                                                      ByRef pResultsList As List(Of GrainSizeDistributionResults))


            Select Case bin

                Case GrainSizeClasses.Boulders



                Case Else


                    Dim cumulativeSum As UInteger = pCumulativeSumDictionary(bin)
                    Dim pBoundsPair As BoundsPair = GetUpperBoundsColumns(bin)
                    Dim pEnumerableRange As System.Collections.Generic.IEnumerable(Of Integer) = Enumerable.Range(pCumulativeSumDictionary(bin), (pCumulativeSumDictionary(bin + 1) - pCumulativeSumDictionary(bin)) + 1)



                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 90 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        pGrainSizeDistributionResults.D50 = 0.06
                        pGrainSizeDistributionResults.D84 = 0.06
                        pGrainSizeDistributionResults.D90 = 0.06
                        pResultsList.Add(pGrainSizeDistributionResults)
                        'Continue Do
                    End If

                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 84 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        pGrainSizeDistributionResults.D50 = 0.06
                        pGrainSizeDistributionResults.D84 = 0.06

                        If pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        Else
                            'Continue For
                        End If

                    End If




                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 50 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        pGrainSizeDistributionResults.D50 = 0.06

                        If pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        ElseIf pEnumerableRange.Contains(84) Then
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            'Continue For
                        Else
                            'Continue For
                        End If

                    End If




                    If bin = GrainSizeClasses.Fines And cumulativeSum >= 16 Then
                        pGrainSizeDistributionResults.D16 = 0.06
                        If pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        ElseIf pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            'Continue For

                        ElseIf pEnumerableRange.Contains(50) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            'Continue For

                        Else
                            'Continue For
                        End If

                    End If

                    If cumulativeSum < 16 And pCumulativeSumDictionary(bin + 1) < 16 Then
                        Exit Sub

                    ElseIf pEnumerableRange.Contains(16) And pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) Then
                        pGrainSizeDistributionResults.D16 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 16)
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(90) And Not pGrainSizeDistributionResults.D16 Is Nothing And Not pGrainSizeDistributionResults.D50 Is Nothing And Not pGrainSizeDistributionResults.D84 Is Nothing Then
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) And Not pGrainSizeDistributionResults.D16 Is Nothing And Not pGrainSizeDistributionResults.D50 Is Nothing Then
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) And pEnumerableRange.Contains(90) And Not pGrainSizeDistributionResults.D16 Is Nothing Then
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    ElseIf pEnumerableRange.Contains(16) And pEnumerableRange.Contains(50) And pEnumerableRange.Contains(84) Then
                        pGrainSizeDistributionResults.D16 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 16)
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                        'Continue For

                    ElseIf pEnumerableRange.Contains(16) Then
                        pGrainSizeDistributionResults.D16 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 16)

                        If pEnumerableRange.Contains(50) Then
                            pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)
                            'Continue For

                        Else
                            ' Continue For

                        End If

                    ElseIf pEnumerableRange.Contains(50) And Not pGrainSizeDistributionResults.D16 Is Nothing Then
                        pGrainSizeDistributionResults.D50 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 50)

                        If pEnumerableRange.Contains(84) Then
                            pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)
                            'Continue For

                        Else
                            'Continue For
                        End If

                    ElseIf pEnumerableRange.Contains(84) Then
                        pGrainSizeDistributionResults.D84 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 84)

                        If pEnumerableRange.Contains(90) Then
                            pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                            'Continue Do

                        Else
                            'Continue For
                        End If

                    ElseIf pEnumerableRange.Contains(90) Then
                        pGrainSizeDistributionResults.D90 = Di(pBoundsPair.LowerBound, pBoundsPair.UpperBound, pCumulativeSumDictionary(bin), pCumulativeSumDictionary(bin + 1), 90)
                        'Continue Do

                    End If

            End Select


        End Sub

#End Region

#Region "Public Functions/Sub Routines"

        Public Function ProcessRBT_ChannelUnits(ByVal pChannelUnitsGrainSize As GISCode.CHaMP.ChannelUnit) As GrainSizeDistributionResults

            Dim pGrainSizeDistributionResults As New GrainSizeDistributionResults
            'check to make sure that the values add up to 100, if they do not it is a sign that values are missing and grain size metrics cannot be calculated
            Dim dCheckSum As Double = pChannelUnitsGrainSize.GrainSampleDictionary.Values.Sum(Function(dValue) dValue)
            pGrainSizeDistributionResults.CheckSum = dCheckSum

            If dCheckSum <> 0 Then

                pChannelUnitsGrainSize.CumulativeSum = CalculateCumulativeSum(pChannelUnitsGrainSize.GrainSampleDictionary)

                'check if grain size is bedrock change this to calculate cumulative sum 
                If pChannelUnitsGrainSize.CumulativeSum(GrainSizeClasses.Boulders) = 0 Then
                    pGrainSizeDistributionResults.D16 = 4000
                    pGrainSizeDistributionResults.D50 = 4000
                    pGrainSizeDistributionResults.D84 = 4000
                    pGrainSizeDistributionResults.D90 = 4000
                    Return pGrainSizeDistributionResults
                End If

                pChannelUnitsGrainSize.CumulativeSum.Remove(GrainSizeClasses.Bedrock)

                For Each bin In pChannelUnitsGrainSize.CumulativeSum.Keys

                    GrainSizeDistributionLogic(pChannelUnitsGrainSize.CumulativeSum,
                                               bin,
                                               pGrainSizeDistributionResults)


                Next
                pGrainSizeDistributionResults.CumulativeSum = pChannelUnitsGrainSize.CumulativeSum
            End If

            Return pGrainSizeDistributionResults

        End Function

        Public Function ProcessChannelFile(ByVal sChannelFilePath As String, ByVal sFileSeparator As String, ByVal sInputPolygonPath As String,
                                                  ByVal sOutputRasterPath As String, Optional ByVal bCreateCDF As Boolean = False, Optional ByVal sOutputCDFFolder As String = Nothing) As List(Of GrainSizeDistributionResults)

            If String.IsNullOrEmpty(sChannelFilePath) Then
                Return Nothing
            End If

            'Create a temporary copy of the polygon feature class
            Dim sTempPolygonPath As String = GISCode.WorkspaceManager.GetTempShapeFile(sInputPolygonPath)
            Dim gOriginalPolygonFC As GISCode.GISDataStructures.PolygonDataSource = New GISCode.GISDataStructures.PolygonDataSource(sInputPolygonPath)
            GISCode.GP.DataManagement.CopyFeatures(gOriginalPolygonFC.FeatureClass, sTempPolygonPath)
            Dim gTempPolygonFC As GISCode.GISDataStructures.PolygonDataSource
            If System.IO.File.Exists(sTempPolygonPath) Then
                gTempPolygonFC = New GISCode.GISDataStructures.PolygonDataSource(sTempPolygonPath)
                gTempPolygonFC.AddField("D50", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
            Else
                Throw New Exception(String.Format("Error accessing polygon feature class {0}.", sInputPolygonPath))
                Exit Function
            End If

            Dim pResultsList As New List(Of GrainSizeDistributionResults)

            Using pChannelFileReader As New IO.StreamReader(sChannelFilePath)

                'Get header information
                Dim currentLine As String = pChannelFileReader.ReadLine
                Dim pColumnDictionary As Dictionary(Of String, Integer) = GetColumnIndex(pColumnNames, currentLine, sFileSeparator)

                'Get channel units column Indices
                Dim pChannelUnitIndex As Dictionary(Of String, Integer) = GetColumnIndex(pChannelUnitColumnNames, currentLine, ",")

                'Perform a check to make sure ColumnDictionary is populated


                Do While (pChannelFileReader.Peek() > -1)
                    currentLine = pChannelFileReader.ReadLine

                    'Put relavant columns int a list or similar structure
                    Dim grainSizeSampleDictionary As Dictionary(Of GrainSizeClasses, UInteger?) = CreateGrainSizeSampleDictionary(pColumnDictionary, currentLine, ",")
                    If grainSizeSampleDictionary Is Nothing Then
                        Continue Do
                    End If

                    'calculate cumulative sum of all columns up to bedrock
                    Dim pGrainSizeDistributionResults As New GrainSizeDistributionResults

                    pGrainSizeDistributionResults.CumulativeSum = CalculateCumulativeSum(grainSizeSampleDictionary)


                    pGrainSizeDistributionResults.GetChannelUnitInfo(currentLine, pChannelUnitIndex, ",")


                    'check if grain size is bedrock change this to calculate cumulative sum 
                    If pGrainSizeDistributionResults.CumulativeSum(GrainSizeClasses.Boulders) = 0 Then
                        pGrainSizeDistributionResults.D16 = 4000
                        pGrainSizeDistributionResults.D50 = 4000
                        pGrainSizeDistributionResults.D84 = 4000
                        pGrainSizeDistributionResults.D90 = 4000
                        pResultsList.Add(pGrainSizeDistributionResults)
                        Continue Do
                    End If


                    'Get cumulative sum without bedrock
                    pGrainSizeDistributionResults.CumulativeSum.Remove(GrainSizeClasses.Bedrock)

                    'Handle each bin individually and write results

                    'AssessFinesColumn(pCumulativeSumDictionary("Fines"), D16, D50, D84, D90)

                    For Each bin In pGrainSizeDistributionResults.CumulativeSum.Keys

                        'Check to make sure within index of pCumulative sum 
                        'If ct + 1 > pCumulativeSum.Count Then
                        '    Exit For
                        'End If

                        GrainSizeDistributionLogic(pGrainSizeDistributionResults.CumulativeSum,
                                                   bin,
                                                   pGrainSizeDistributionResults,
                                                   pResultsList)

                    Next

                    'Update the feature class
                    Dim fcBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = gTempPolygonFC.FeatureClass.CreateFeatureBuffer()
                    Dim queryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilterClass()
                    Dim iChannelUnitFieldIndexCSV As Integer = pChannelUnitIndex("ChannelUnitID")
                    Dim iChannelUnitValue As Integer = Integer.Parse(currentLine.Split(sFileSeparator)(iChannelUnitFieldIndexCSV).Replace("""", ""))
                    queryFilter.WhereClause = "Unit_Numbe = " & iChannelUnitValue
                    Dim iD50FieldIndex As Integer = gTempPolygonFC.FindField("D50")
                    Dim updateCursor As ESRI.ArcGIS.Geodatabase.FeatureCursor = gTempPolygonFC.FeatureClass.Update(queryFilter, False)
                    Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = updateCursor.NextRow()

                    While Not pRow Is Nothing
                        pRow.Value(iD50FieldIndex) = pGrainSizeDistributionResults.D50
                        updateCursor.UpdateRow(pRow)
                        pRow = updateCursor.NextRow()
                    End While

                    Dim comReferencesLeft As Integer
                    Do
                        comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor) _
                                            + System.Runtime.InteropServices.Marshal.ReleaseComObject(fcBuffer)
                    Loop While (comReferencesLeft > 0)

                    Debug.WriteLine("D16: " & pGrainSizeDistributionResults.D16)
                    Debug.WriteLine("D50: " & pGrainSizeDistributionResults.D50)
                    Debug.WriteLine("D84: " & pGrainSizeDistributionResults.D84)
                    Debug.WriteLine("D90: " & pGrainSizeDistributionResults.D90)
                    Debug.WriteLine(vbCrLf & vbCrLf)


                    If bCreateCDF Then
                        Dim sCDFFolderName As String = System.IO.Path.GetFileNameWithoutExtension(sInputPolygonPath)
                        Dim sCDFFolderPath As String = System.IO.Path.Combine(sOutputCDFFolder, sCDFFolderName & "_CDFs")
                        If Not System.IO.Directory.Exists(sCDFFolderPath) Then
                            System.IO.Directory.CreateDirectory(sCDFFolderPath)
                        End If

                        Dim sCDFFilePath As String = System.IO.Path.Combine(sCDFFolderPath, "CU_" & iChannelUnitValue & ".png")
                        CreateCDFPlot(pGrainSizeDistributionResults, sCDFFilePath)
                    End If


                    pResultsList.Add(pGrainSizeDistributionResults)

                Loop

            End Using

            GISCode.GP.Conversion.PolygonToRaster_conversion(gTempPolygonFC, "D50", sOutputRasterPath)

            Return pResultsList

        End Function

        Public Sub CreateCDFPlot(ByVal pGrainSizeResults As GrainSizeDistributionResults, ByVal sOutputCDFPath As String)

            Dim zControl As ZedGraph.ZedGraphControl = New ZedGraph.ZedGraphControl()
            zControl.Size = New System.Drawing.Size(800, 800)
            Dim zPane As ZedGraph.GraphPane = zControl.GraphPane
            zPane.Title.Text = "Grain Size Distribution Estimate"


            Dim lPoints As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            lPoints.Add(0, 0)
            If Not pGrainSizeResults.CumulativeSum Is Nothing Then
                For Each sGrainSize As GrainSizeClasses In pGrainSizeResults.CumulativeSum.Keys
                    If sGrainSize <> GrainSizeClasses.Bedrock Then
                        'Debug.Print(sGrainSize.ToString)
                        Dim x As Double = pBounds(sGrainSize.ToString())
                        Dim y As Integer = pGrainSizeResults.CumulativeSum.Item(sGrainSize)
                        lPoints.Add(x, y)
                        'Debug.Print(String.Format("X: {0}, Y: {1}", x, y))
                        Dim sLabel As ZedGraph.TextObj = New ZedGraph.TextObj(sGrainSize.ToString, x, y, ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center)
                        sLabel.ZOrder = ZedGraph.ZOrder.A_InFront
                        sLabel.FontSpec.Border.IsVisible = False
                        sLabel.FontSpec.Fill.IsVisible = False
                        sLabel.FontSpec.Angle = 90
                        zPane.GraphObjList.Add(sLabel)
                    End If
                Next
            End If

            Dim sMetricText As String = String.Format("D16: {0}", Math.Round(pGrainSizeResults.D16.GetValueOrDefault(), 1))
            Dim sMetricLabel As ZedGraph.TextObj = New ZedGraph.TextObj(sMetricText, pGrainSizeResults.D16, 16, ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center)
            zPane.GraphObjList.Add(sMetricLabel)

            sMetricText = String.Format("D50: {0}", Math.Round(pGrainSizeResults.D50.GetValueOrDefault(), 1))
            sMetricLabel = New ZedGraph.TextObj(sMetricText, pGrainSizeResults.D50, 50, ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center)
            zPane.GraphObjList.Add(sMetricLabel)

            sMetricText = String.Format("D84: {0}", Math.Round(pGrainSizeResults.D84.GetValueOrDefault(), 1))
            sMetricLabel = New ZedGraph.TextObj(sMetricText, pGrainSizeResults.D84, 84, ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center)
            zPane.GraphObjList.Add(sMetricLabel)

            sMetricText = String.Format("D90: {0}", Math.Round(pGrainSizeResults.D90.GetValueOrDefault(), 1))
            sMetricLabel = New ZedGraph.TextObj(sMetricText, pGrainSizeResults.D90, 90, ZedGraph.CoordType.AxisXYScale, ZedGraph.AlignH.Left, ZedGraph.AlignV.Center)
            zPane.GraphObjList.Add(sMetricLabel)

            zPane.Legend.IsVisible = False
            Dim zLine As ZedGraph.LineItem = zPane.AddCurve("CDF", lPoints, Drawing.Color.Red, ZedGraph.SymbolType.Circle)
            zLine.Line.Width = 2.0F
            zLine.Line.IsAntiAlias = False
            zLine.Symbol.Fill = New ZedGraph.Fill(Drawing.Color.White)
            zLine.Symbol.Size = 7

            zPane.XAxis.Scale.Min = -5
            If pGrainSizeResults.D90 < 256 Then
                zPane.XAxis.Scale.Max = 256
            Else
                zPane.XAxis.Scale.Max = pGrainSizeResults.D90
            End If

            zPane.XAxis.Title.Text = "Grain Size (mm)"
            zPane.YAxis.Scale.Min = -5
            zPane.YAxis.Scale.Max = 100
            zPane.YAxis.Title.Text = "Cumulative Sum"
            zControl.AxisChange()
            zControl.MasterPane.GetImage().Save(sOutputCDFPath)

        End Sub

        Public Function ProcessRaster(ByVal sRasterPath As String,
                                      ByVal sInputPolygonPath As String,
                                      ByVal sChannelUnitFieldName As String,
                                      ByVal eLinearUnits As NumberFormatting.LinearUnits,
                                      Optional ByVal bAppendGrainSizeValues As Boolean = False,
                                      Optional ByVal bAppendNRasterCells As Boolean = False,
                                      Optional ByVal bCreateCDF As Boolean = False,
                                      Optional ByVal sOutputCDFFolder As String = Nothing) As GISCode.GISDataStructures.PolygonDataSource

            If String.IsNullOrEmpty(sRasterPath) Then
                Return Nothing
            End If


            Dim pResultsList As New List(Of GrainSizeDistributionResults)
            Dim pGrainSizeDistributionResults As New GrainSizeDistributionResults
            Dim pGrainSizeDictionary As Dictionary(Of GrainSizeClasses, UInteger)

            'Create a temporary copy of the polygon feature class if append values is false
            Dim sTempPolygonPath As String
            Dim gPolygonFC As GISCode.GISDataStructures.PolygonDataSource = Nothing
            If bAppendGrainSizeValues = False Then

                sTempPolygonPath = GISCode.WorkspaceManager.GetTempShapeFile(sInputPolygonPath)
                gPolygonFC = New GISCode.GISDataStructures.PolygonDataSource(sInputPolygonPath)
                GISCode.GP.DataManagement.CopyFeatures(gPolygonFC.FeatureClass, sTempPolygonPath)
                If System.IO.File.Exists(sTempPolygonPath) Then
                    gPolygonFC = New GISCode.GISDataStructures.PolygonDataSource(sTempPolygonPath)
                    'gPolygonFC.AddField("D50", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                Else
                    Throw New Exception(String.Format("Error accessing polygon feature class {0}.", sInputPolygonPath))
                    Exit Function
                End If

            ElseIf bAppendGrainSizeValues = True Then

                gPolygonFC = New GISCode.GISDataStructures.PolygonDataSource(sInputPolygonPath)
                gPolygonFC.AddField("D16", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                gPolygonFC.AddField("D50", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                gPolygonFC.AddField("D84", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                gPolygonFC.AddField("D90", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)

                If bAppendNRasterCells = True And bAppendGrainSizeValues = True Then
                    gPolygonFC.AddField("nCells", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger)
                End If

            End If


            'Define the variables
            Dim D16 As Double = Nothing
            Dim D50 As Double = Nothing
            Dim D84 As Double = Nothing
            Dim D90 As Double = Nothing

            'Update the feature class
            Dim fcBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = gPolygonFC.FeatureClass.CreateFeatureBuffer()
            Dim queryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilterClass()

            'Find channel unit/distinguishing field and shapefield
            Dim iChannelUnitFieldIndex As Integer = gPolygonFC.FindField(sChannelUnitFieldName)
            Dim iD16Field As Integer = gPolygonFC.FindField("D16")
            Dim iD50Field As Integer = gPolygonFC.FindField("D50")
            Dim iD84Field As Integer = gPolygonFC.FindField("D84")
            Dim iD90Field As Integer = gPolygonFC.FindField("D90")
            Dim iNRasterCellsField As Integer = gPolygonFC.FindField("nCells")
            Dim pShapeFieldName As String = gPolygonFC.FeatureClass.ShapeFieldName
            Dim pShapeFieldIndex As Integer = gPolygonFC.FeatureClass.FindField(pShapeFieldName)

            Dim updateCursor As ESRI.ArcGIS.Geodatabase.FeatureCursor = gPolygonFC.FeatureClass.Update(queryFilter, False)
            Dim pUpdateRow As ESRI.ArcGIS.Geodatabase.IRow = updateCursor.NextRow()

            While Not pUpdateRow Is Nothing

                Try

                    Dim pGPEngine As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor

                    Dim sTempRasterPath As String = GISCode.WorkspaceManager.GetTempRaster(sRasterPath)
                    Dim sChannelUnitValue As String = pUpdateRow.Value(iChannelUnitFieldIndex).ToString()

                    Dim lParams As New List(Of String)
                    lParams.Add(sRasterPath) 'Input Raster 
                    lParams.Add(sInputPolygonPath) 'Input Channel Unit FC
                    lParams.Add(sChannelUnitFieldName) ' Unique Channel Field Value
                    lParams.Add(sChannelUnitValue) 'Unique Channel Field Value 
                    lParams.Add(sTempRasterPath) 'Temporary Output Raster
                    Dim sToolboxPath As String = IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "GISCode\GCD\Python\GCD_Python.pyt")

                    GISCode.Toolbox.ToolBox.RunTool(sToolboxPath, "ExtractRasterByMask", lParams)

                    Dim gRaster As GISCode.GISDataStructures.Raster
                    If System.IO.File.Exists(sTempRasterPath) Then
                        Debug.Print("{0} was created", sTempRasterPath)
                        gRaster = New GISCode.GISDataStructures.Raster(sTempRasterPath)
                        pGrainSizeDictionary = InitializeGrainSizeDictionary()
                        ReadRasterValuesIntoGrainSizeRasterDictionary(gRaster.RasterDataset, pGrainSizeDictionary, eLinearUnits)
                    Else
                        Debug.Write("Raster was not created.")
                        pUpdateRow = updateCursor.NextRow()
                        Continue While
                    End If


                    'Grain size logic
                    Dim pTotalValues As Integer = Nothing

                    For Each bin In pGrainSizeDictionary.Keys

                        pTotalValues += pGrainSizeDictionary(bin)

                    Next

                    Debug.Print("Total Raster Cells: {0}", pTotalValues)
                    If pTotalValues <= 0 Then
                        Debug.Print("No values in raster.")
                        pUpdateRow = updateCursor.NextRow()
                        Continue While
                    End If

                    Dim pPercentDictionary As New Dictionary(Of GrainSizeClasses, UInteger?)

                    For Each bin In pGrainSizeDictionary.Keys
                        Dim pPercentTotal = UInteger.Parse(Math.Round(pGrainSizeDictionary(bin) / pTotalValues, 2) * 100)

                        pPercentDictionary.Add(bin, pPercentTotal)

                    Next

                    pGrainSizeDistributionResults = New GrainSizeDistributionResults
                    pGrainSizeDistributionResults.CumulativeSum = CalculateCumulativeSum(pPercentDictionary)
                    pGrainSizeDistributionResults.CumulativeSum.Remove(GrainSizeClasses.Bedrock)

                    For Each bin In pGrainSizeDistributionResults.CumulativeSum.Keys

                        GrainSizeDistributionLogic(pGrainSizeDistributionResults.CumulativeSum,
                                                   bin,
                                                   pGrainSizeDistributionResults,
                                                   pResultsList)
                    Next

                    If bAppendGrainSizeValues = True Then
                        pUpdateRow.Value(iD16Field) = pGrainSizeDistributionResults.D16
                        pUpdateRow.Value(iD50Field) = pGrainSizeDistributionResults.D50
                        pUpdateRow.Value(iD84Field) = pGrainSizeDistributionResults.D84
                        pUpdateRow.Value(iD90Field) = pGrainSizeDistributionResults.D90


                        If bAppendNRasterCells = True Then
                            pUpdateRow.Value(iNRasterCellsField) = pTotalValues
                        End If

                        updateCursor.UpdateRow(pUpdateRow)

                    End If

                    If bCreateCDF Then
                        Dim sCDFFolderPath As String = System.IO.Path.Combine(sOutputCDFFolder, "CDF_Figures")
                        If Not System.IO.Directory.Exists(sCDFFolderPath) Then
                            System.IO.Directory.CreateDirectory(sCDFFolderPath)
                        End If

                        Debug.Print("D16: {0}", pGrainSizeDistributionResults.D16)
                        Debug.Print("D50: {0}", pGrainSizeDistributionResults.D50)
                        Debug.Print("D84: {0}", pGrainSizeDistributionResults.D84)
                        Debug.Print("D90: {0}", pGrainSizeDistributionResults.D90)

                        Dim sCDFFilePath As String = System.IO.Path.Combine(sCDFFolderPath, sChannelUnitFieldName & "_" & sChannelUnitValue & ".png")
                        CreateCDFPlot(pGrainSizeDistributionResults, sCDFFilePath)
                    End If

                    pUpdateRow = updateCursor.NextRow()

                Catch ex As Exception
                    pUpdateRow = updateCursor.NextRow()
                    'Do nothing
                End Try
            End While




            pResultsList.Add(pGrainSizeDistributionResults)

            Return gPolygonFC

        End Function

        Public Sub SubsetRasters()


            Dim outFeatureClass As String = String.Empty


            Dim sWorkspacePath As String = IO.Path.GetDirectoryName(outFeatureClass)

            Dim aType As Type = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory")
            Dim obj As System.Object = Activator.CreateInstance(aType)
            Dim pWSFact As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = obj
            Dim pWork As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = pWSFact.OpenFromFile(sWorkspacePath, 0)
            Dim pFC As ESRI.ArcGIS.Geodatabase.IFeatureClass = pWork.OpenFeatureClass(IO.Path.GetFileNameWithoutExtension(outFeatureClass))
            Dim polygonGeometry As ESRI.ArcGIS.Geometry.IPolygon = New ESRI.ArcGIS.Geometry.Polygon
            Dim pCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = pFC.Search(Nothing, False)

            Dim pShapeFieldName As String = pFC.ShapeFieldName
            Dim pShapeFieldIndex As Integer = pFC.FindField(pShapeFieldName)

            Dim pGPEngine As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor
            Dim pMakeFeatureLayer As ESRI.ArcGIS.DataManagementTools.MakeFeatureLayer = New ESRI.ArcGIS.DataManagementTools.MakeFeatureLayer
            Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = pCursor.NextFeature
            While Not pRow Is Nothing

                Dim pGeom = pRow.Fields.Field(pShapeFieldIndex)
                polygonGeometry = TryCast(pGeom, ESRI.ArcGIS.Geometry.IPolygon)

                Dim pClipTool As ESRI.ArcGIS.AnalysisTools.Clip = New ESRI.ArcGIS.AnalysisTools.Clip()
                pClipTool.clip_features = polygonGeometry
                pClipTool.in_features = "TempRaster"
                pClipTool.out_feature_class = "Temp"

                pGPEngine.Execute(pClipTool, Nothing)

            End While


        End Sub

#End Region

        ''' <summary>
        ''' used to better organize and store the results of a grain size distribution analysis
        ''' </summary>
        ''' <remarks></remarks>
        Public Class GrainSizeDistributionResults

            Private _D16 As Double?
            Private _D50 As Double?
            Private _D84 As Double?
            Private _D90 As Double?
            Private _CumulativeSum As Dictionary(Of GrainSizeClasses, UInteger)
            Private _dCheckSum As UInteger

            Private _SiteID As String
            Private _Tier1ChannelUnit As String
            Private _Tier2ChannelUnit As String
            Private _ChannelUnitID As String

            Public Sub New()

                _D16 = Nothing
                _D50 = Nothing
                _D84 = Nothing
                _D90 = Nothing

            End Sub

            Public Property D16 As Double?
                Get
                    Return _D16
                End Get
                Set(ByVal value As Double?)
                    _D16 = value
                End Set
            End Property

            Public Property D50 As Double?
                Get
                    Return _D50
                End Get
                Set(ByVal value As Double?)
                    _D50 = value
                End Set
            End Property

            Public Property D84 As Double?
                Get
                    Return _D84
                End Get
                Set(ByVal value As Double?)
                    _D84 = value
                End Set
            End Property

            Public Property D90 As Double?
                Get
                    Return _D90
                End Get
                Set(ByVal value As Double?)
                    _D90 = value
                End Set
            End Property

            Public Property CumulativeSum As Dictionary(Of GrainSizeClasses, UInteger)
                Get
                    Return _CumulativeSum
                End Get
                Set(value As Dictionary(Of GrainSizeClasses, UInteger))
                    _CumulativeSum = value
                End Set
            End Property

            Public Property CheckSum As UInteger
                Get
                    Return _dCheckSum
                End Get
                Set(value As UInteger)
                    _dCheckSum = value
                End Set
            End Property


            Public Property SiteID
                Get
                    Return _SiteID
                End Get
                Set(ByVal value)
                    _SiteID = value
                End Set
            End Property

            Public Property Tier1ChannelUnit
                Get
                    Return _Tier1ChannelUnit
                End Get
                Set(ByVal value)
                    _Tier1ChannelUnit = value
                End Set
            End Property

            Public Property Tier2ChannelUnit
                Get
                    Return _Tier2ChannelUnit
                End Get
                Set(ByVal value)
                    _Tier2ChannelUnit = value
                End Set
            End Property

            Public Property ChannelUnitID
                Get
                    Return _ChannelUnitID
                End Get
                Set(ByVal value)
                    _ChannelUnitID = value
                End Set
            End Property

            Public Sub GetChannelUnitInfo(ByVal sCurrentLine As String, ByVal pColumnIndex As Dictionary(Of String, Integer), ByVal separator As String)

                If pColumnIndex.ContainsKey("SiteID") Then
                    Me.SiteID = sCurrentLine.Split(separator)(pColumnIndex("SiteID"))
                End If

                If pColumnIndex.ContainsKey("Tier1") Then
                    Me.Tier1ChannelUnit = sCurrentLine.Split(separator)(pColumnIndex("Tier1"))
                End If

                If pColumnIndex.ContainsKey("Tier2") Then
                    Me.Tier2ChannelUnit = sCurrentLine.Split(separator)(pColumnIndex("Tier2"))
                End If

                If pColumnIndex.ContainsKey("ChannelUnitID") Then
                    Me.ChannelUnitID = sCurrentLine.Split(separator)(pColumnIndex("ChannelUnitID"))
                End If

            End Sub

        End Class

        Public Class AuxillarySiteInfo

            Private m_SiteID As String
            Private m_SampleDate As String
            Private m_StreamName As String

            Sub New(ByVal siteID As String, ByVal sampleDate As String, ByVal streamName As String)

                m_SiteID = siteID
                m_SampleDate = sampleDate
                m_StreamName = streamName

            End Sub

            Sub New()


            End Sub

            Public Property SiteID
                Get
                    Return m_SiteID
                End Get
                Set(ByVal value)
                    m_SiteID = value
                End Set
            End Property

            Public Property SampleDate
                Get
                    Return m_SampleDate
                End Get
                Set(ByVal value)
                    m_SampleDate = value
                End Set
            End Property

            Public Property StreamName
                Get
                    Return m_StreamName
                End Get
                Set(ByVal value)
                    m_StreamName = value
                End Set
            End Property

        End Class

    End Module

End Namespace