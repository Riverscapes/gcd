#Region "Code Comments"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'       Author: Philip Bailey, Nick Ochoski, & Frank Poulsen
'               ESSA Software Ltd.
'               1765 W 8th Avenue
'               Vancouver, BC, Canada V6J 5C6
'     
'     Copyright: (C) 2011 by ESSA technologies Ltd.
'                This software is subject to copyright protection under the       
'                laws of Canada and other countries.
'
'  Date Created: 14 January 2011
'
'   Description: 
'
#End Region

#Region "Imports"
Imports System.IO
#End Region

Namespace Core.GCDProject

    Public Class ProjectManager
        Inherits ProjectManagerBase

        Private Shared m_FISLibraryDS As FISLibrary = Nothing
        Private Shared m_SurveyTypesDS As SurveyTypes = Nothing

        Private Shared m_FISLibraryXMLFilePath As String = Nothing
        Private Shared m_SurveyTypesLibraryXMLFilePath As String = Nothing

        Public Shared Property fispath As String
            Get
                Return m_FISLibraryXMLFilePath
            End Get
            Set(ByVal value As String)
                m_FISLibraryXMLFilePath = value
                If m_FISLibraryDS Is Nothing Then
                    m_FISLibraryDS = New FISLibrary
                End If
                m_FISLibraryDS.Clear()
                If File.Exists(m_FISLibraryXMLFilePath) Then
                    m_FISLibraryDS.ReadXml(m_FISLibraryXMLFilePath)
                End If
            End Set
        End Property

        Public Shared Property surveypath As String
            Get
                'If _surveypath Is Nothing Then
                ' PGB July 2013. 
                ' The following code should be moved into the GCD because it is 
                ' relies on the deployed software and won't work in the console
                '
                '#If DEBUG Then
                '                _surveypath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                '                If Not _surveypath.EndsWith(IO.Path.DirectorySeparatorChar) Then
                '                    _surveypath &= IO.Path.DirectorySeparatorChar
                '                End If
                '#Else
                '                    _surveypath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                '                    If Not _surveypath.EndsWith(IO.Path.DirectorySeparatorChar) Then
                '                        _surveypath &= IO.Path.DirectorySeparatorChar
                '                    End If
                '                    _surveypath &= "GCD\"
                '#End If
                '                    _surveypath &= "Options\SurveyTypes.xml"
                '                End If
                'End If
                Return m_SurveyTypesLibraryXMLFilePath

            End Get
            Set(ByVal value As String)
                m_SurveyTypesLibraryXMLFilePath = value
                If m_SurveyTypesDS Is Nothing Then
                    m_SurveyTypesDS = New SurveyTypes
                End If
                m_SurveyTypesDS.Clear()
                If File.Exists(m_SurveyTypesLibraryXMLFilePath) Then
                    m_SurveyTypesDS.ReadXml(m_SurveyTypesLibraryXMLFilePath)
                End If
            End Set
        End Property
        Public Shared ReadOnly Property fisds As FISLibrary
            Get
                If m_FISLibraryDS Is Nothing Then
                    m_FISLibraryDS = New FISLibrary
                    ' End If
                    '_fislib.Clear()
                    If File.Exists(fispath) Then
                        m_FISLibraryDS.ReadXml(fispath)
                    End If
                End If
                Return m_FISLibraryDS
            End Get
        End Property

        Public Shared Sub saveFIS()

            m_FISLibraryDS.WriteXml(fispath)


            'Saves added fis to the add-in folder so it can be permentaley stored
            'Dim sExecutingAssemblyFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            'If IO.Directory.Exists(sExecutingAssemblyFolder) Then
            '    'sExecutingAssemblyFolder = IO.Path.Combine(sExecutingAssemblyFolder, GISCode.FileSystem.RemoveDangerousCharacters(My.Resources.ApplicationNameShort))
            '    Dim fispath = IO.Path.Combine(sExecutingAssemblyFolder, "FISLibrary")
            '    Dim sFISLibraryPath = IO.Path.Combine(fispath, "FISLibraryXML.xml")
            '    m_FISLibraryDS.WriteXml(sFISLibraryPath)
            'End If

        End Sub

        Public Shared ReadOnly Property surveyds As SurveyTypes
            Get
                If m_SurveyTypesDS Is Nothing Then
                    m_SurveyTypesDS = New SurveyTypes
                    m_SurveyTypesDS.ReadXml(surveypath)
                End If
                '_surveytypes.Clear()
                '_surveytypes.ReadXml(surveypath)
                Return m_SurveyTypesDS
            End Get
        End Property

        Public Shared Sub saveSurveyTypes()
            m_SurveyTypesDS.WriteXml(surveypath)
        End Sub

        Public Sub New(ByVal sFISDatasetPath As String, ByVal sResourcesFolder As String, ByVal sExcelTemplateFolder As String,
                       ByVal sSurveyTypesDatasetPath As String,
                       ByVal eDefaultRasterType As GISDataStructures.RasterTypes,
                       ByVal ColourErosion As System.Drawing.Color,
                       ByVal ColourDeposition As System.Drawing.Color)

            MyBase.New(eDefaultRasterType, sResourcesFolder, sExcelTemplateFolder, ColourErosion, ColourDeposition)

            If String.IsNullOrEmpty(sFISDatasetPath) Then
                Throw New ArgumentNullException("sFISDatasetPath", "The FIS dataset path cannot be empty")
            Else
                If Not IO.File.Exists(sFISDatasetPath) Then
                    Dim ex As New Exception("The FIS dataset path does not exist")
                    ex.Data("FIS path") = sFISDatasetPath
                    Throw ex
                End If
            End If

            If String.IsNullOrEmpty(sSurveyTypesDatasetPath) Then
                Throw New ArgumentNullException("sSurveyTypesDatasetPath", "The survey types dataset path cannot be empty")
            Else
                If Not IO.File.Exists(sSurveyTypesDatasetPath) Then
                    Dim ex As New Exception("The survey types dataset path does not exist")
                    ex.Data("Survey types path") = sSurveyTypesDatasetPath
                    Throw ex
                End If
            End If

            m_FISLibraryXMLFilePath = sFISDatasetPath
            m_SurveyTypesLibraryXMLFilePath = sSurveyTypesDatasetPath

            m_GCDNARCError = New External.NARCError

        End Sub

    End Class

End Namespace