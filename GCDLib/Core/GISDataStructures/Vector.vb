Namespace Core.GISDataStructures

    Public Class Vector
        Inherits GISDataStructures.GISDataSource

        Public Sub New(sFilePath As String)
            MyBase.New(sFilePath)

            ' TODO: Open the shapefile using OGR and set the spatial reference here

        End Sub

        Public Function FindField(sFieldName As String) As Int32

            If (String.IsNullOrEmpty(sFieldName)) Then
                Throw New ArgumentNullException(sFieldName)
            End If

            ' TODO: lookup field index in ShapeFile
            Throw New Exception("Not implemented yet")

            Return -1

        End Function

        Public ReadOnly Property FeatureCount As Integer
            Get
                ' TODO: implmenet this function
                Throw New Exception("Not implemented yet")
                Return 0
            End Get
        End Property



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
