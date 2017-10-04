Namespace Core.GISDataStructures

    Public Class Vector
        Inherits GISDataStructures.GISDataSource

        Public Sub New(sFilePath As String, sSpatialReference As String)
            MyBase.New(sFilePath, sSpatialReference)

        End Sub

        ''' <summary>
        ''' Alters the file extension depending on file geodatabase or not.
        ''' </summary>
        ''' <param name="inFullPath">Full path to an existing or new feature class</param>
        ''' <remarks></remarks>
        Public Shared Sub ConfirmExtension(ByRef inFullPath As String)

            If String.IsNullOrEmpty(inFullPath) Then
                Throw New ArgumentNullException("inFullPath", "Null or empty feature class full path")
            End If

            inFullPath = IO.Path.ChangeExtension(inFullPath, "shp")

        End Sub

        Public Shared Function GetNewSafeName(sDirectory As String, sFileName As String) As String

            Return naru.os.File.GetNewSafeName(sDirectory, sFileName, "shp").FullName

        End Function

    End Class

End Namespace
