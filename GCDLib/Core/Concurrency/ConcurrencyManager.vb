#Region "Imports"

#End Region

Namespace GISCode.GCD.Concurrency

    Public Class ConcurrencyManager

        ' Moved to GISDataStructures.RasterGDal
        'Public Shared Function CheckConcurrrency(ByVal Raster1 As GISDataStructures.RasterGDAL, ByVal Raster2 As GISDataStructures.RasterGDAL, Optional ByVal precision As Integer = -1) As Boolean
        '    Dim RasterInfo1 As New Concurrency.DEMInfoClass(Raster1, precision)
        '    Return RasterInfo1.isConcurrent(Raster2, precision)


        'End Function

        'Public Shared Function CheckOrthogonality(ByVal gRaster As GISDataStructures.RasterGDAL, ByRef IsCellHeightAndWidthOK As Boolean, ByRef IsCellResolutionOK As Boolean, ByRef IsExtentOK As Boolean, Optional ByVal precision As Integer = -1) As Boolean

        '    'Dim RasterInfo As New Concurrency.DEMInfoClass(RasterPath, precision)
        '    IsCellHeightAndWidthOK = False
        '    IsCellResolutionOK = False
        '    IsExtentOK = False

        '    'check width and height is identical
        '    If Not Math.Round(Math.Abs(gRaster.CellHeight), 3) = Math.Round(Math.Abs(gRaster.CellWidth), 3) Then
        '        Return False
        '    Else
        '        IsCellHeightAndWidthOK = True
        '    End If

        '    'check that resolition is rounded to 3 decimals
        '    Dim CellResolution As Double = Math.Abs(gRaster.CellWidth)
        '    Dim RoundedCellResolution As Double = Math.Round(CellResolution, 3)
        '    If Not CellResolution = RoundedCellResolution Then
        '        Return False
        '    Else
        '        IsCellResolutionOK = True
        '    End If

        '    'check that extents are divisible by the cell resolution (without a remainder)
        '    Dim Left As Double = gRaster.Left
        '    Dim LeftDivided As Double = Left / CellResolution
        '    If precision > -1 Then
        '        LeftDivided = Math.Round(LeftDivided, precision)
        '    End If
        '    Dim RoundedLeftDivided As Double = Math.Round(LeftDivided)
        '    If Not LeftDivided = RoundedLeftDivided Then
        '        Return False
        '    End If

        '    Dim Right As Double = gRaster.Right
        '    Dim RightDivided As Double = Right / CellResolution
        '    If precision > -1 Then
        '        RightDivided = Math.Round(RightDivided, precision)
        '    End If
        '    Dim RoundedRightDivided As Double = Math.Round(RightDivided)
        '    If Not RightDivided = RoundedRightDivided Then
        '        Return False
        '    End If

        '    Dim Top As Double = gRaster.Top
        '    Dim TopDivided As Double = Top / CellResolution
        '    If precision > -1 Then
        '        TopDivided = Math.Round(TopDivided, precision)
        '    End If
        '    Dim RoundedTopDivided As Double = Math.Round(TopDivided)
        '    If Not TopDivided = RoundedTopDivided Then
        '        Return False
        '    End If

        '    Dim Bottom As Double = gRaster.Bottom
        '    Dim BottomDivided As Double = Bottom / CellResolution
        '    If precision > -1 Then
        '        BottomDivided = Math.Round(BottomDivided, precision)
        '    End If
        '    Dim RoundedBottomDivided As Double = Math.Round(BottomDivided)
        '    If Not BottomDivided = RoundedBottomDivided Then
        '        Return False
        '    End If

        '    IsExtentOK = True
        '    Return True

        'End Function

        'Public Shared Function CreateOrthogonality(ByVal RasterPath As String, ByVal OutputRasterPath As String) As Boolean
        '    Dim RasterInfo As New Concurrency.DEMInfoClass(RasterPath)

        '    'check width and height is identical
        '    If Not Math.Round(Math.Abs(RasterInfo.CellHeight), 3) = Math.Round(Math.Abs(RasterInfo.CellWidth), 3) Then
        '        Dim ex As New Exception("Cell height and with is not identical")
        '        ex.Data("cellheight") = RasterInfo.CellHeight
        '        ex.Data("cellWidth") = RasterInfo.CellWidth
        '        Throw ex
        '    End If

        '    'check that resolition is rounded to 3 decimals
        '    Dim CellResolution As Decimal = Math.Abs(RasterInfo.CellWidth)
        '    Dim RoundedCellResolution As Decimal = Math.Round(CellResolution, 3)

        '    'check that extents are divisible by the cell resolution (without a remainder)
        '    Dim Left As Double = RasterInfo.Left
        '    Dim LeftDivided As Double = Left / CellResolution
        '    Dim RoundedLeftDivided As Double = Math.Floor(LeftDivided)
        '    Dim RoundedLeft As Double = RoundedLeftDivided * RoundedCellResolution


        '    Dim Right As Double = RasterInfo.Right
        '    Dim RightDivided As Double = Right / CellResolution
        '    Dim RoundedRightDivided As Double = Math.Ceiling(RightDivided)
        '    Dim RoundedRight As Double = RoundedRightDivided * RoundedCellResolution

        '    Dim Top As Double = RasterInfo.Top
        '    Dim TopDivided As Double = Top / CellResolution
        '    Dim RoundedTopDivided As Double = Math.Ceiling(TopDivided)
        '    Dim RoundedTop As Double = RoundedTopDivided * RoundedCellResolution

        '    Dim Bottom As Double = RasterInfo.Bottom
        '    Dim BottomDivided As Double = Bottom / CellResolution
        '    Dim RoundedBottomDivided As Double = Math.Floor(BottomDivided)
        '    Dim RoundedBottom As Double = RoundedBottomDivided * RoundedCellResolution

        '    Dim expression As String = """" & RasterPath & """"
        '    Dim ExtentRectangle As String = RoundedLeft & " " & RoundedBottom & " " & RoundedRight & " " & RoundedTop

        '    GISCode.GP.SpatialAnalyst.Raster_Calculator(expression, OutputRasterPath, ExtentRectangle, RoundedCellResolution)

        'End Function


        Public Shared Sub CreateOrthogonalityCellResolutionAndExtent( _
                                                ByRef CellResolution As Double, _
                                                ByRef ExtentLeft As Double, _
                                                ByRef ExtentBottom As Double, _
                                                ByRef ExtentRight As Double, _
                                                ByRef ExtentTop As Double)

            Dim RoundedCellResolution As Double = Math.Round(CellResolution, 3)

            'check that extents are divisible by the cell resolution (without a remainder)
            Dim LeftDivided As Double = ExtentLeft / CellResolution
            Dim RoundedLeftDivided As Double = Math.Floor(LeftDivided)
            Dim RoundedLeft As Double = RoundedLeftDivided * RoundedCellResolution


            Dim RightDivided As Double = ExtentRight / CellResolution
            Dim RoundedRightDivided As Double = Math.Ceiling(RightDivided)
            Dim RoundedRight As Double = RoundedRightDivided * RoundedCellResolution

            Dim TopDivided As Double = ExtentTop / CellResolution
            Dim RoundedTopDivided As Double = Math.Ceiling(TopDivided)
            Dim RoundedTop As Double = RoundedTopDivided * RoundedCellResolution

            Dim BottomDivided As Double = ExtentBottom / CellResolution
            Dim RoundedBottomDivided As Double = Math.Floor(BottomDivided)
            Dim RoundedBottom As Double = RoundedBottomDivided * RoundedCellResolution

            CellResolution = RoundedCellResolution
            ExtentLeft = RoundedLeft
            ExtentBottom = RoundedBottom
            ExtentRight = RoundedRight
            ExtentTop = RoundedTop

        End Sub

        Public Shared Sub CreateConcurrentAndOrthogonalRasters(ByVal gRaster1 As GISDataStructures.CHaMPRaster, ByVal gRaster2 As GISDataStructures.CHaMPRaster, ByVal outRaster1 As String, ByVal outRaster2 As String, ByVal nPrecision As Integer)

            ' Make original rasters concurrent
            Dim theUnionExtent As GISDataStructures.ExtentRectangle = gRaster1.Extent
            theUnionExtent.Union(gRaster2.Extent)

            Dim nCols As Integer = (theUnionExtent.Right - theUnionExtent.Left) / gRaster1.CellSize
            Dim nRows As Integer = (theUnionExtent.Top - theUnionExtent.Bottom) / gRaster1.CellSize
            Dim fCellSize As Double = Math.Round(gRaster1.CellSize, nPrecision)

            ProcessRaster(gRaster1, theUnionExtent, nCols, nRows, fCellSize, nPrecision, outRaster1)
            ProcessRaster(gRaster2, theUnionExtent, nCols, nRows, fCellSize, nPrecision, outRaster2)


        End Sub

        Private Shared Sub ProcessRaster(ByRef gRaster As GISDataStructures.CHaMPRaster, ByRef theExtent As GISDataStructures.ExtentRectangle, nCols As Integer, nRows As Integer, fCellSize As Double, nPrecision As Integer, sOutputRaster As String)

            If GISDataStructures.Raster.Exists(sOutputRaster) Then

                ' The raster already exists in the desired location. Validate that it is concurrent with the desired extent
                Dim theRaster As New GISDataStructures.RasterDirect(sOutputRaster)
                If Not theExtent.IsConcurrent(theRaster.Extent) Then
                    Dim ex As New Exception("Error creating concurrent and orthogonal rasters. The output raster path already exists.")
                    ex.Data("Original Raster Path") = gRaster.FullPath
                    ex.Data("Desired Output Path") = sOutputRaster
                    ex.Data("Desired extent") = theExtent.ToString
                    Throw ex
                End If
            Else
                ' The raster doesn't already exist. Copy the raw raster to the desired path, making it concurrent with the desired extent
                If Not RasterManager.Copy(gRaster.FullPath, sOutputRaster, fCellSize, Math.Round(theExtent.Left, nPrecision), Math.Round(theExtent.Top, nPrecision), _
                                      nRows, nCols, GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString) = RasterManagerOutputCodes.PROCESS_OK Then
                    Throw New Exception(GCD.GCDProject.ProjectManager.GCDNARCError.ErrorString.ToString)
                End If
            End If
        End Sub

    End Class

End Namespace