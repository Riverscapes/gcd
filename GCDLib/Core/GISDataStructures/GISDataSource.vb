Namespace Core.GISDataStructures

#Region "Enumerations"

    ''' <summary>
    ''' Basic GIS data geometry types
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum GeometryTypes
        Point
        Line
        Polygon
    End Enum

    Public Enum BasicGISTypes
        Point
        Line
        Polygon
        Raster
    End Enum

    Public Enum GISDataStorageTypes
        ShapeFile
        FileGeodatase
        RasterFile
        PersonalGeodatabase
        CAD
        TIN
    End Enum

    Public Enum BrowseGISTypes
        Point
        Line
        Polygon
        Raster
        TIN
        Any
    End Enum

    Public Enum BrowseVectorTypes
        Point
        Line
        Polygon
        CrossSections
    End Enum

    Public Enum ValidationStates
        Untested
        Pass
        Fail
    End Enum

    Public Enum RasterTypes
        ASCII
        ESRIGrid
        IMG
        FileGeodatabase
        TIFF
    End Enum

#End Region

    Public Class GISDataSource

        Private m_FilePath As System.IO.FileInfo
        Private m_SpatialReferenceWKS As String ' GDAL Spatial Ref well known string

        Public ReadOnly Property FilePath As System.IO.FileInfo
            Get
                Return m_FilePath
            End Get
        End Property

        Public ReadOnly Property FullPath As String
            Get
                Return m_FilePath.FullName
            End Get
        End Property

        Public Property SpatialReferenceWKS As String
            Protected Set(value As String)
                m_SpatialReferenceWKS = value
            End Set
            Get
                Return m_SpatialReferenceWKS
            End Get
        End Property

        Public Sub New(sFilePath As String, sSpatialReferenceWKS As String)
            m_FilePath = New IO.FileInfo(sFilePath)
            m_SpatialReferenceWKS = sSpatialReferenceWKS
        End Sub

        Public Sub New(sFilePath As String)
            m_FilePath = New IO.FileInfo(sFilePath)
            m_SpatialReferenceWKS = "Not Set"
        End Sub

        Public Function CheckSpatialReferenceMatches(sOtherSpatialReference As String) As Boolean
            Return String.Compare(sOtherSpatialReference, True) = 0
        End Function

        Public Function CheckSpatialReferenceMatches(otherDataSource As GISDataSource) As Boolean
            Return CheckSpatialReferenceMatches(otherDataSource.SpatialReferenceWKS)
        End Function

        Public Shared Function Exists(sFullPath As String) As Boolean
            Return (Not String.IsNullOrEmpty(sFullPath) AndAlso System.IO.File.Exists(sFullPath))
        End Function

        Public Shared Function GetWorkspacePath(sFullPath As String) As String
            Return System.IO.Path.GetDirectoryName(sFullPath)
        End Function
    End Class

End Namespace