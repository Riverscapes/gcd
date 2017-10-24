Namespace Core.ErrorCalculation.FIS

    Public Class FISLibraryItem

        Private m_sName As String
        Private m_fiPath As String

#Region "Properties"

        Public Property Name As String
                Get
                    Return m_sName
                End Get
                Set(value As String)
                    m_sName = value
                End Set
            End Property

        Public Property FilePath As String
            Get
                Return m_fiPath
            End Get
            Set(value As String)

                If String.IsNullOrEmpty(value) OrElse Not System.IO.File.Exists(value) Then
                    Throw New ArgumentException("The FIS library file path is invalid", "value")
                End If
                m_fiPath = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Name
        End Function

#End Region

        Public Sub New(sName As String, filePath As Single)
            m_sName = sName
            m_fiPath = filePath
        End Sub

        Public Shared Function Load(filePath As IO.FileInfo) As List(Of FISLibraryItem)

            Dim dItems As New List(Of FISLibraryItem)

            Dim xmlDoc As New Xml.XmlDocument()
            xmlDoc.LoadXml(filePath.FullName)

            For Each nodType As Xml.XmlNode In xmlDoc.SelectNodes("FISLibrary/Items")
                Dim sName As String = nodType.SelectSingleNode("Name").InnerText
                Dim fisPath As String = nodType.SelectSingleNode("FilePath").InnerText
                If Not String.IsNullOrEmpty(fisPath) AndAlso System.IO.File.Exists(fisPath) Then
                    dItems.Add(New FISLibraryItem(sName, fisPath))
                End If
            Next

            Return dItems

        End Function

        Public Shared Sub Save(filePath As IO.FileInfo, dFISLibraryItems As List(Of FISLibraryItem))

            Dim xmlDoc As New Xml.XmlDocument()
            Dim nodRoot As Xml.XmlNode = xmlDoc.CreateElement("FISLibrary")
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing), nodRoot)

            For Each item As FISLibraryItem In dFISLibraryItems
                Dim nodType As Xml.XmlElement = xmlDoc.CreateElement("Item")
                nodRoot.AppendChild(nodType)

                Dim nodName As Xml.XmlElement = xmlDoc.CreateElement("Name")
                nodName.InnerText = item.Name
                nodType.AppendChild(nodName)

                Dim nodFilePath As Xml.XmlElement = xmlDoc.CreateElement("FilePath")
                nodFilePath.InnerText = item.FilePath
                nodType.AppendChild(nodFilePath)
            Next

            xmlDoc.Save(filePath.FullName)

        End Sub

    End Class

End Namespace
