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

        Public ReadOnly Property OIDFieldName As String
            Get
                ' TODO: implmenet this function
                Throw New Exception("Not implemented yet")
            End Get
        End Property

        Public ReadOnly Property Fields As Dictionary(Of Integer, String)
            Get
                ' TODO: implmenet this function
                Throw New Exception("Not implemented yet")
                Return New Dictionary(Of Integer, String)
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

        Public Function AddField(sFieldName As String, eFieldType As GISDataStructures.FieldTypes, Optional nFieldLength As Integer = 0) As Integer

            'TODO not implemented
            Throw New Exception("not implemented")

        End Function

        Public Function FieldType(i As Integer) As FieldTypes

            'TODO not implemented
            Throw New Exception("not implemented")

        End Function

        Public Shared Function CopyShapeFile(sOriginal As String, sOutput As String) As Vector

            'TODO not implemented
            Throw New Exception("not implemented")

            Return Nothing

        End Function

        Public Function CopyShapeFile(sOutput As String) As Vector
            Return CopyShapeFile(FullPath, sOutput)
        End Function

        Public Sub FillComboWithFields(ByRef cbo As System.Windows.Forms.ComboBox, sDefaultFieldToSelect As String, eFieldType As FieldTypes, Optional ByVal bClearCbomItemsFirst As Boolean = True)

            'TODO not implemented
            Throw New Exception("not implemented")

        End Sub



    End Class

End Namespace
