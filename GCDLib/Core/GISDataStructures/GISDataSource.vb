Namespace Core.GISDataStructures

    Public Class GISDataSource

        Private m_FilePath As System.IO.FileInfo
        Private m_SpatialReferenceWKS As String ' GDAL Spatial Ref well known string

        Public ReadOnly Property Exists As Boolean
            Get
                Return m_FilePath.Exists
            End Get
        End Property

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

    End Class

End Namespace