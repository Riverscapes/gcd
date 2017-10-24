
Namespace Core.GCDProject

    Public Class SurveyType

        Private m_sName As String
        Private m_fError As Single

#Region "Properties"

        Public Property Name As String
            Get
                Return m_sName
            End Get
            Set(value As String)
                m_sName = value
            End Set
        End Property

        Public Property ErrorValue As Single
            Get
                Return m_fError
            End Get
            Set(value As Single)

                If value < 0 Then
                    Throw New ArgumentOutOfRangeException("value", "The error value must be greater than or equal to zero")
                End If

                m_fError = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Name
        End Function

#End Region

        Public Sub New(sName As String, fError As Single)
            m_sName = sName
            m_fError = fError
        End Sub

        Public Shared Function Load(filePath As IO.FileInfo) As Dictionary(Of String, SurveyType)

            Dim dSurveyTypes As New Dictionary(Of String, SurveyType)

            Dim xmlDoc As New Xml.XmlDocument()
            xmlDoc.LoadXml(filePath.FullName)

            For Each nodType As Xml.XmlNode In xmlDoc.SelectNodes("SurveyTypes/SurveyType")
                Dim sName As String = nodType.SelectSingleNode("Name").InnerText
                Dim fError As Single = Single.Parse(nodType.SelectSingleNode("Error").InnerText)
                dSurveyTypes(sName) = New SurveyType(sName, fError)
            Next

            Return dSurveyTypes

        End Function

        Public Shared Sub Save(filePath As IO.FileInfo, dSurveyTypes As Dictionary(Of String, SurveyType))

            Dim xmlDoc As New Xml.XmlDocument()
            Dim nodRoot As Xml.XmlNode = xmlDoc.CreateElement("SurveyTypes")
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", Nothing, Nothing), nodRoot)

            For Each sType As SurveyType In dSurveyTypes.Values
                Dim nodType As Xml.XmlElement = xmlDoc.CreateElement("SurveyType")
                nodRoot.AppendChild(nodType)

                Dim nodName As Xml.XmlElement = xmlDoc.CreateElement("Name")
                nodName.InnerText = sType.Name
                nodType.AppendChild(nodName)

                Dim nodError As Xml.XmlElement = xmlDoc.CreateElement("Error")
                nodError.InnerText = sType.ErrorValue.ToString()
                nodType.AppendChild(nodError)
            Next

            xmlDoc.Save(filePath.FullName)

        End Sub


    End Class

End Namespace
