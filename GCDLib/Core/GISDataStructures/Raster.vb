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

        Public ReadOnly Property IsDivisible As Boolean
            Get
                Return Extent.IsDivisible(CellSize)
            End Get
        End Property

        Public ReadOnly Property Maximum As Double
            Get
                Throw New NotImplementedException
                Return 0
            End Get
        End Property

        Public ReadOnly Property Minimum As Double
            Get
                Throw New NotImplementedException
                Return 0
            End Get
        End Property

        Public ReadOnly Property HasNoDataValue As Boolean
            Get
                'TODO: needs implementation from GDAL
                Throw New Exception("not implemented")
                Return True
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

            SpatialReference = sSpatialReference.ToString()

            m_eLinearUnits = New naru.math.LinearUnitClass(naru.math.NumberFormatting.LinearUnits.yard)
            If (Not String.IsNullOrEmpty(sUnits.ToString())) Then
                m_eLinearUnits = New naru.math.LinearUnitClass(naru.math.NumberFormatting.GetLinearUnitsFromString(sUnits.ToString()))
            End If

            m_bHasNoData = nNoData <> 0
        End Sub

        Public Function CheckSpatialReferenceMatches(sSpatialReferenceWKS As String) As Boolean
            Return String.Compare(SpatialReference, sSpatialReferenceWKS, True) = 0
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

            If String.IsNullOrEmpty(sFullPath) OrElse Not System.IO.File.Exists(sFullPath) Then
                Exit Sub
            End If

            Try
                External.RasterManager.Delete(sFullPath, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            Catch ex As Exception
#If DEBUG Then
                Dim ex2 As New Exception("Error attempting to delete raster file.", ex)
                ex2.Data("Raster Path") = sFullPath
                Throw ex2
#End If
            End Try

        End Sub

        ''' <summary>
        ''' Alters the file extension depending on file geodatabase or not.
        ''' </summary>
        ''' <param name="inFullPath">Full path to an existing or new feature class</param>
        ''' <remarks></remarks>
        Public Shared Sub ConfirmExtension(ByRef inFullPath As String, eRasterType As GISDataStructures.RasterTypes)

            If String.IsNullOrEmpty(inFullPath) Then
                Throw New ArgumentNullException("inFullPath", "Null or empty feature class full path")
            End If

            Dim sExtension As String = String.Empty
            Select Case eRasterType
                Case RasterTypes.TIFF
                    sExtension = "tiff"
                Case RasterTypes.IMG
                    sExtension = "img"
                Case Else
                    Throw New Exception(String.Format("Unhandled raster type: {0}", eRasterType))
            End Select

            inFullPath = IO.Path.ChangeExtension(inFullPath, sExtension)

        End Sub

        Public Shared Function GetNewSafeName(sDirectory As String, sFileName As String) As String

            Return naru.os.File.GetNewSafeName(sDirectory, sFileName, "tif").FullName

        End Function

        Public Shared Function BuildRasterPath(sDirectory As String, sFileName As String) As System.IO.FileInfo

            If (String.IsNullOrEmpty(sDirectory) OrElse Not System.IO.Directory.Exists(sDirectory)) Then
                Throw New ArgumentNullException("sDirectory", "Invalid directory")
            End If

            If (String.IsNullOrEmpty(sFileName)) Then
                Throw New ArgumentNullException(sFileName, "Invalid file name")
            End If

            Dim sPath As String = naru.os.File.RemoveDangerousCharacters(sFileName)
            sPath = System.IO.Path.Combine(sDirectory, sPath)
            sPath = System.IO.Path.ChangeExtension(sPath, "tif")
            Return New System.IO.FileInfo(sPath)

        End Function


    End Class

End Namespace
