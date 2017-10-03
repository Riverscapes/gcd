Namespace GISCode.GCD.Concurrency

    Public Class RasterExtentAdjuster

        Private m_AdjustedExtent As GISCode.GISDataStructures.ExtentRectangle
        Private m_fBuffer As Double
        Private m_fOrthogonalityFactor As Double

        ''' <summary>
        ''' When combining extents, this tolerance is used to test if the extents are meaningful
        ''' </summary>
        ''' <remarks>Early testing of CHaMP data highlighted that sometimes, unprojected data are
        ''' sometimes combined with projected data. When the extents are unioned this creates a
        ''' massive rectangle which is then used for the TIN to Raster process. It hangs ArcGIS
        ''' and creates some massive files. 
        ''' 
        ''' This tolerance is used to throw an error if the raster extents are more than this fraction
        ''' apart from the original. i.e. 0.1 is 10% of the original units.</remarks>
        Private Const m_fUnionTolerance As Double = 0.1

#Region "Properties"

        Public ReadOnly Property Left As Double
            Get
                Dim fResult As Double = m_AdjustedExtent.Left
                fResult = fResult / OrthogonalityFactor
                fResult = Math.Floor(fResult)
                fResult = fResult * OrthogonalityFactor
                Return fResult - m_fBuffer
            End Get
        End Property

        Public ReadOnly Property Right As Double
            Get
                Dim fResult As Double = m_AdjustedExtent.Right
                fResult = fResult / OrthogonalityFactor
                fResult = Math.Ceiling(fResult)
                fResult = fResult * OrthogonalityFactor
                Return fResult + m_fBuffer
            End Get
        End Property

        Public ReadOnly Property Top As Double
            Get
                Dim fResult As Double = m_AdjustedExtent.Top
                fResult = fResult / OrthogonalityFactor
                fResult = Math.Ceiling(fResult)
                fResult = fResult * OrthogonalityFactor
                Return fResult + m_fBuffer
            End Get
        End Property

        Public ReadOnly Property Bottom As Double
            Get
                Dim fResult As Double = m_AdjustedExtent.Bottom
                fResult = fResult / OrthogonalityFactor
                fResult = Math.Floor(fResult)
                fResult = fResult * OrthogonalityFactor
                Return fResult - m_fBuffer
            End Get
        End Property

        Private ReadOnly Property OrthogonalityFactor As Double
            Get
                Return m_fOrthogonalityFactor
            End Get
        End Property

        Public ReadOnly Property AdjustedExtent As GISDataStructures.ExtentRectangle
            Get
                Return New GISDataStructures.ExtentRectangle(Top, Left, Right, Bottom)
            End Get
        End Property

        Public ReadOnly Property AdjustedExtentAsString As String
            Get
                Return Left & " " & Bottom & " " & Right & " " & Top
            End Get
        End Property

        Public Property Buffer As Double
            Get
                Return m_fBuffer
            End Get
            Set(value As Double)
                If value > 0 Then
                    m_fBuffer = value
                End If
            End Set
        End Property

#End Region

        Public Sub New(ByVal demInfo As GISDataStructures.RasterDirect, Optional fBuffer As Double = 0)

            If Not TypeOf demInfo Is GISDataStructures.Raster Then
                Throw New ArgumentNullException("demInfo", "Invalid input raster to extent adjuster.")
            End If

            m_AdjustedExtent = demInfo.Extent
            Debug.WriteLine("Initial rectangle: " & m_AdjustedExtent.Rectangle.ToString)
            m_AdjustedExtent.MakeDivisible(fBuffer, 0)
            Debug.WriteLine("Divisible rectangle: " & m_AdjustedExtent.Rectangle.ToString)

            m_fOrthogonalityFactor = demInfo.CellSize

            If fBuffer >= 0 Then
                m_fBuffer = fBuffer
            Else
                m_fBuffer = 0
            End If

        End Sub

        Public Sub New(sRasterPath As String, nPrecision As Integer, fPrecision As Double, Optional fBuffer As Double = 0)
            Me.New(New GISDataStructures.RasterDirect(sRasterPath), fBuffer)
        End Sub

        Public Function Adjusted(fPrecision As Double, fBuffer As Double) As GISDataStructures.ExtentRectangle

            m_fBuffer = fBuffer
            Debug.WriteLine("Adjusted (buffer = " & fBuffer.ToString & "): " & AdjustedExtent.ToString)

            Return New GISDataStructures.ExtentRectangle(Top, Left, Right, Bottom)

        End Function

        Public Sub UnionExtent(ByVal extent As GISDataStructures.ExtentRectangle)

            Debug.WriteLine("Adding rectangle: " & extent.ToString)

            Try
                If Math.Abs(m_AdjustedExtent.Left - extent.Left) / m_AdjustedExtent.Left > m_fUnionTolerance OrElse _
                    Math.Abs(m_AdjustedExtent.Right - extent.Right) / m_AdjustedExtent.Right > m_fUnionTolerance OrElse _
                    Math.Abs(m_AdjustedExtent.Top - extent.Top) / m_AdjustedExtent.Top > m_fUnionTolerance OrElse _
                    Math.Abs(m_AdjustedExtent.Bottom - extent.Bottom) / m_AdjustedExtent.Bottom > m_fUnionTolerance Then

                    Throw New Exception("The original extent is more than " & m_fUnionTolerance * 100 & "% different than the original. Projected/unprojected data suspected.")
                End If

                m_AdjustedExtent.Union(extent)
            Catch ex As Exception
                ex.Data("Existing extent") = m_AdjustedExtent.ToString
                ex.Data("New extent") = extent.ToString
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Tests whether the argument extent requires shrinking to fit on the adjusted extent
        ''' </summary>
        ''' <param name="extent"></param>
        ''' <returns>True when the argument extent extends outside the adjusted extent</returns>
        ''' <remarks></remarks>
        Public Function RequiresShrinking(extent As GISDataStructures.ExtentRectangle) As Boolean

            Dim bResult As Boolean = True

            If Me.Left <= extent.Left Then
                If Me.Top >= extent.Top Then
                    If Me.Right >= extent.Right Then
                        If Me.Bottom <= extent.Bottom Then
                            bResult = False
                        End If
                    End If
                End If
            End If

            Return bResult

        End Function

        Public Function Rows(fCellSize As Single) As Integer

            If Convert.ToSingle((Top - Bottom) / fCellSize) Mod 1 <> 0 OrElse Convert.ToSingle((Right - Left) / fCellSize) Mod 1 <> 0 Then
                Dim ex As New Exception("The extent is not evenly divisible by the cell size.")
                ex.Data("Extent") = AdjustedExtentAsString
                ex.Data("Cell size") = fCellSize.ToString
                Throw ex
            End If

            Return (Top - Bottom) / fCellSize
        End Function

        Public Function Columns(fCellSize As Single) As Integer

            If Convert.ToSingle((Top - Bottom) / fCellSize) Mod 1 <> 0 OrElse Convert.ToSingle((Right - Left) / fCellSize) Mod 1 <> 0 Then
                Dim ex As New Exception("The extent is not evenly divisible by the cell size.")
                ex.Data("Extent") = AdjustedExtentAsString
                ex.Data("Cell size") = fCellSize.ToString
                Throw ex
            End If

            Return (Right - Left) / fCellSize
        End Function

    End Class

End Namespace
