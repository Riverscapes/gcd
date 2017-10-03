Imports GCD.GCDLib.Core.External

Namespace Core.GISDataStructures

    Public Class Raster
        Inherits GISDataSource

        Private m_nRows As Integer
        Private m_nColumns As Integer
        Private m_bHasNoData As Boolean
        Private m_fCellWidth As Double
        Private m_fCellHeight As Double
        Private m_fNoData As Double
        Private m_fLeft As Double
        Private m_fTop As Double
        Private m_sUnits As String

#Region "Properties"

        Public ReadOnly Property CellSize As Double
            Get
                Return m_fCellWidth
            End Get
        End Property

        Public ReadOnly Property CellWidth As Double
            Get
                Return m_fCellWidth
            End Get
        End Property

        Public ReadOnly Property CellHeight As Double
            Get
                Return m_fCellHeight
            End Get
        End Property

        Public ReadOnly Property Rows As Double
            Get
                Return m_nRows
            End Get
        End Property

        Public ReadOnly Property Columns As Double
            Get
                Return m_nColumns
            End Get
        End Property

        Public Shadows ReadOnly Property Extent As ExtentRectangle
            Get
                Return New GISDataStructures.ExtentRectangle(m_fTop, m_fLeft, m_fLeft + (m_fCellWidth * m_nColumns), m_fTop + (m_fCellHeight * m_nRows))
            End Get
        End Property

        Public ReadOnly Property Units As String
            Get
                Return m_sUnits
            End Get
        End Property

        Public ReadOnly Property IsDivisible As Boolean
            Get
                Return Extent.IsDivisible(CellSize)
            End Get
        End Property

#End Region

        Public Sub New(sFullPath As String)
            MyBase.New(sFullPath)

            ' This check is here and not in the new DataSource base class because
            ' DataSources can also represent File Geodatabase layers that don't 
            ' necessarily represent files on disk.
            If Not IO.File.Exists(sFullPath) Then
                If Not IO.Directory.Exists(sFullPath) Then
                    Dim ex As New Exception("The raster path does not appear to be either an existing file or directory.")
                    ex.Data("File Path") = sFullPath
                    Throw ex
                End If
            End If

            Dim theError As New NARCError
            Dim nNoData, nDataType As Integer

            ' Placeholder variables for new C API properties.
            Dim sSpatialReference As New System.Text.StringBuilder(1024)
            Dim sUnits As New System.Text.StringBuilder(1024)

            Dim eResult As Integer = RasterManager.GetRasterProperties(sFullPath, m_fCellHeight, m_fCellWidth,
                                              m_fLeft, m_fTop, m_nRows, m_nColumns, m_fNoData, nNoData, nDataType,
                                              sUnits, sSpatialReference,
                                              theError.ErrorString)

            If Not eResult = RasterManagerOutputCodes.PROCESS_OK Then
                Throw New Exception(theError.ErrorString.ToString)
            End If

            SpatialReferenceWKS = sSpatialReference.ToString()
            m_sUnits = sUnits.ToString()

            m_bHasNoData = nNoData <> 0
        End Sub

        Public Function CheckSpatialReferenceMatches(sSpatialReferenceWKS As String) As Boolean
            Return String.Compare(SpatialReferenceWKS, sSpatialReferenceWKS, True) = 0
        End Function

        ''' <summary>
        ''' True when two rasters share cell resolution and also the horizontal and vertical offset is evenly divisible by cell size.
        ''' </summary>
        ''' <param name="gRaster2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsOrthogonal(ByRef gRaster2 As Raster) As Boolean
            Return Extent.IsOrthogonal(gRaster2.Extent, m_fCellWidth)
        End Function

        Public Function IsConcurrent(ByRef gRaster2 As Raster, nPrecision As Integer) As Boolean
            Return Extent.IsConcurrent(gRaster2.Extent)
        End Function

        Public Shared Sub DeleteRaster(sFullPath As String)

            If String.IsNullOrEmpty(sFullPath) Then
                Exit Sub
            End If

            If Not System.IO.File.Exists(sFullPath) Then
                Exit Sub
            End If
            Try
                Dim sDirectory As String = System.IO.Path.GetDirectoryName(sFullPath)
                Dim sFileName As String = System.IO.Path.GetFileName(sFullPath)
                Dim nFirstPeriod As Integer = sFileName.IndexOf(".")
                Dim sSearchPattern As String = System.IO.Path.GetFileNameWithoutExtension(sFullPath)
                If nFirstPeriod > 0 Then
                    sSearchPattern = String.Format("{0}.*", sFileName.Substring(0, nFirstPeriod))
                End If

                Dim sFiles As String() = System.IO.Directory.GetFiles(sDirectory, sSearchPattern)
                For Each aFile As String In sFiles
                    System.IO.File.Delete(aFile)
                Next

            Catch ex As Exception
#If DEBUG Then
                Dim ex2 As New Exception("Error attempting to delete raster file.", ex)
                ex2.Data("Raster Path") = sFullPath
                Throw ex2
#End If
            End Try

        End Sub

    End Class

End Namespace
