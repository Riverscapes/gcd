Namespace Core.GISDataStructures

    ''' <summary>
    ''' Represents rectangular extents
    ''' </summary>
    ''' <remarks>PGB - Aug 2012 - This class represents rectangle extents. It's mainly intended for
    ''' use with DEM raster extents and orthogonality. Note: this class does not perform any orthogonality
    ''' calculations itself. It is intended to capture just the coordinates. This class is used by the
    ''' RasterExtentAdjuster for actually performing orthogonality changes.</remarks>
    Public Class ExtentRectangle

#Region "Members"
        Private m_fTop As Double
        Private m_fLeft As Double
        Private m_fRight As Double
        Private m_fBottom As Double
#End Region

#Region "Properties"
        Public ReadOnly Property Top As Double
            Get
                Return m_fTop
            End Get
        End Property

        Public ReadOnly Property Left As Double
            Get
                Return m_fLeft
            End Get
        End Property

        Public ReadOnly Property Right As Double
            Get
                Return m_fRight
            End Get
        End Property

        Public ReadOnly Property Bottom As Double
            Get
                Return m_fBottom
            End Get
        End Property

        Public ReadOnly Property Height As Double
            Get
                Return m_fTop - m_fBottom
            End Get
        End Property

        Public ReadOnly Property Width As Double
            Get
                Return m_fRight - m_fLeft
            End Get
        End Property

        ''' <summary>
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>The sequence of these argument is important. They must match the sequence that ESRI
        ''' expects when geoprocessing.</remarks>
        Public ReadOnly Property Rectangle As String
            Get
                Return String.Format("{0} {1} {2} {3}", Left, Bottom, Right, Top)
            End Get
        End Property

#End Region

#Region "Methods"

        Public Sub New(fTop As Double, fLeft As Double, fRight As Double, fBottom As Double)

            If fTop <= fBottom Then
                Throw New ArgumentOutOfRangeException("fTop", fTop, "The top coordinate cannot be less than or equal to the bottom")
            End If

            If fLeft >= fRight Then
                Throw New ArgumentOutOfRangeException("fLeft", fLeft, "The left coordinate cannot be less than or equal to the right")
            End If

            m_fLeft = fLeft
            m_fTop = fTop
            m_fRight = fRight
            m_fBottom = fBottom

        End Sub

        Public Sub Union(aRectangle As ExtentRectangle)

            Debug.WriteLine("Unioning rectangle")
            Debug.WriteLine(vbTab & "Before  : " & Rectangle)
            Debug.WriteLine(vbTab & "Argument: " & aRectangle.Rectangle)

            m_fLeft = Math.Min(m_fLeft, aRectangle.Left)
            m_fRight = Math.Max(m_fRight, aRectangle.Right)
            m_fBottom = Math.Min(m_fBottom, aRectangle.Bottom)
            m_fTop = Math.Max(m_fTop, aRectangle.Top)

            Debug.WriteLine(vbTab & "After   : " & Rectangle)
        End Sub

        Public Sub Buffer(fDistance As Double)

            If fDistance <= 0 Then
                Throw New ArgumentOutOfRangeException("Distance", fDistance, "The buffer distance must be greater than zero")
            ElseIf fDistance > 0 Then

                Debug.WriteLine("Buffering rectangle")
                Debug.WriteLine(vbTab & "Before  : " & Rectangle)

                m_fLeft -= fDistance
                m_fRight += fDistance
                m_fBottom -= fDistance
                m_fTop += fDistance

                Debug.WriteLine(vbTab & "After   : " & Rectangle)
            End If
        End Sub

        Public Shadows Function ToString()
            Return Rectangle
        End Function

        Public Function IsConcurrent(otherExtent As ExtentRectangle) As Boolean

            Dim bIsConcurrent As Boolean = False

            If Left = otherExtent.Left Then
                If Right = otherExtent.Right Then
                    If Top = otherExtent.Top Then
                        If Bottom = otherExtent.Bottom Then
                            bIsConcurrent = True
                        End If
                    End If
                End If
            End If
            Return bIsConcurrent
        End Function

        ''' <summary>
        '''  Returns true if the DEM raster is orthogonal to the specified precision
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Only true if the corner coordinates can be wholy divisible by the precision.
        ''' ie. if a value of 2 is supplied then all four corners must be able to be divided by 2 with
        ''' no fractions returned.</remarks>
        Public Function IsDivisible(fCellSize As Double) As Boolean
            Dim Test As Double = (Top / Math.Abs(fCellSize)) Mod 1
            Dim bResult As Boolean = False
            If (Top / Math.Abs(fCellSize)) Mod 1 = 0 Then
                If (Left / fCellSize) Mod 1 = 0 Then
                    If (Right / fCellSize) Mod 1 = 0 Then
                        If (Bottom / Math.Abs(fCellSize)) Mod 1 = 0 Then
                            bResult = True
                        End If
                    End If
                End If
            End If
            Return bResult

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="otherExtent"></param>
        ''' <param name="fCellSize"></param>
        ''' <returns></returns>
        ''' <remarks>PGB 16 Oct 2015. The CellHeight returned by the property is negative and so
        ''' needs to be made positive for comparison. also, the member properties return type
        ''' double which does not equate to true when compared to the same value as a single.</remarks>
        Public Function IsOrthogonal(otherExtent As ExtentRectangle, fCellSize As Double) As Boolean

            Dim bResult As Boolean = False

            If IsDivisible(fCellSize) Then
                If otherExtent.IsDivisible(fCellSize) Then
                    bResult = True
                End If
            End If

            Return bResult

        End Function

        Public Sub MakeDivisible()
            m_fLeft = Math.Floor(m_fLeft)
            m_fRight = Math.Ceiling(m_fRight)
            m_fTop = Math.Ceiling(m_fTop)
            m_fBottom = Math.Floor(m_fBottom)
        End Sub


        ''' <summary>
        ''' Overloaded method to allow the input of cell size to determine an orthogonal extent from an extent object
        ''' </summary>
        ''' <param name="fCellSize"></param>
        ''' <param name="nBuffer"></param>
        ''' <remarks></remarks>
        Public Sub MakeDivisible(ByVal fCellSize As Double, Optional ByVal nBuffer As Integer = 2)

            Dim fOrthogonalityFactor As Double

            If (fCellSize = 0) Then
                Throw New ArgumentOutOfRangeException("fCellSize", fCellSize, "Cell Size cannot be 0")
            End If

            ' We do the precision calculation just so we will get a warning if the 
            ' fCellsize is some weird value (or converted badly)
            Dim fPrecision = naru.math.Statistics.GetPrecision(fCellSize)
            ' NB: fPrecision < 0 means anything less than 1.0
            If (fPrecision < 0) Then
                fOrthogonalityFactor = 1
            Else
                fOrthogonalityFactor = fCellSize
            End If

            If m_fLeft <> 0 Then
                m_fLeft = m_fLeft / fOrthogonalityFactor
                m_fLeft = Math.Floor(m_fLeft)
                m_fLeft = m_fLeft * fOrthogonalityFactor
            End If
            m_fLeft -= nBuffer

            If m_fRight <> 0 Then
                m_fRight = m_fRight / fOrthogonalityFactor
                m_fRight = Math.Ceiling(m_fRight)
                m_fRight = m_fRight * fOrthogonalityFactor
            End If
            m_fRight += nBuffer

            If m_fTop <> 0 Then
                m_fTop = m_fTop / fOrthogonalityFactor
                m_fTop = Math.Ceiling(m_fTop)
                m_fTop = m_fTop * fOrthogonalityFactor
            End If
            m_fTop += nBuffer

            If m_fBottom <> 0 Then
                m_fBottom = m_fBottom / fOrthogonalityFactor
                m_fBottom = Math.Floor(m_fBottom)
                m_fBottom = m_fBottom * fOrthogonalityFactor
            End If
            m_fBottom -= nBuffer

        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="otherExtent"></param>
        ''' <returns>True if the two rectangle extents have any overlap</returns>
        ''' <remarks>Adapted from posts on 
        ''' http://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other</remarks>
        Public Function HasOverlap(ByVal otherExtent As ExtentRectangle) As Boolean

            Dim ulx As Double = Math.Max(Me.Left, otherExtent.Left)
            Dim uly As Double = Math.Max(Me.Bottom, otherExtent.Bottom)
            Dim lrx As Double = Math.Min(Me.Right, otherExtent.Right)
            Dim lry As Double = Math.Min(Me.Top, otherExtent.Top)

            Return ulx <= lrx AndAlso uly <= lry

        End Function

#End Region

    End Class

End Namespace