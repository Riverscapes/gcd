Imports System.IO

Namespace Core.GCDProject

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>Base class for the GCD project manager. This should only contain
    ''' members needed by both console and UI applications. The GCD still uses the inherited
    ''' GCDProjectManager class that has more members that are specific to
    ''' the desktop software.</remarks>
    Public Class ProjectManagerBase

        Private Shared m_ProjectDS As ProjectDS = Nothing
        Private Shared m_GCDProjectXMLFilePath As String = Nothing

        Protected Shared m_OutputManager As OutputManager

        ' This is the folder that contains the Excel Templates
        Private Const EXCELTEMPLATESFOLDERNAME As String = "ExcelTemplates"
        Protected Shared m_dExcelTemplatesFolder As DirectoryInfo

        ' Colour for displaying erosion and deposition in charts
        Private Shared m_cColourErosion As System.Drawing.Color
        Private Shared m_cColourDeposition As System.Drawing.Color

        Private Const m_sReportsFolder As String = "ReportFiles"
        Private Shared m_dResourcesFolder As IO.DirectoryInfo

        Protected Shared m_eDefaultRasterType As GCDConsoleLib.Raster.RasterDriver

        Private Shared m_fiSurveyTypes As System.IO.FileInfo

        Private Shared m_GCDNARCError As External.NARCError
        Public Shared ReadOnly Property GCDNARCError As External.NARCError
            Get
                Return m_GCDNARCError
            End Get
        End Property

#Region "Properties"

        Public Shared ReadOnly Property RasterExtension As String
            Get
                Select Case m_eDefaultRasterType
                    Case GCDConsoleLib.Raster.RasterDriver.GTiff
                        Return "tif"
                    Case GCDConsoleLib.Raster.RasterDriver.HFA
                        Return "img"
                    Case Else
                        Throw New NotImplementedException(String.Format("Unhandled raster driver type {0}", m_eDefaultRasterType))
                End Select
            End Get
        End Property

        Public Shared Property ColourErosion As Drawing.Color
            Get
                Return m_cColourErosion
            End Get
            Set(ByVal value As Drawing.Color)
                m_cColourErosion = value
            End Set
        End Property

        Public Shared Property ColourDeposition As Drawing.Color
            Get
                Return m_cColourDeposition
            End Get
            Set(ByVal value As Drawing.Color)
                m_cColourErosion = value
            End Set
        End Property

        Public Shared ReadOnly Property CurrentProject As ProjectDS.ProjectRow
            Get
                Dim rResult As ProjectDS.ProjectRow = Nothing
                If TypeOf ds Is ProjectDS Then
                    If TypeOf ds.Project Is ProjectDS.ProjectDataTable Then
                        If ds.Project.Rows.Count > 0 Then
                            rResult = ds.Project.Rows(0)
                        End If
                    End If
                End If

                Return rResult
            End Get
        End Property

        Public Shared Property FilePath As String
            Get
                Return m_GCDProjectXMLFilePath
            End Get
            Set(ByVal value As String)

                ' Store the new GCD project file path
                m_GCDProjectXMLFilePath = value

                ' Create a new project dataset
                If m_ProjectDS Is Nothing Then
                    m_ProjectDS = New ProjectDS
                End If
                m_ProjectDS.Clear()

                ' Attempt to read the GCD project
                If File.Exists(m_GCDProjectXMLFilePath) Then
                    Try
                        m_ProjectDS.ReadXml(m_GCDProjectXMLFilePath)
                    Catch ex As Exception
                        m_GCDProjectXMLFilePath = String.Empty
                        m_ProjectDS = Nothing
                        Dim ex2 As New Exception("Error reading GCD project file.", ex)
                        ex2.Data("GCD Project File") = value
                        Throw ex2
                    End Try
                End If
            End Set
        End Property

        Public Shared ReadOnly Property OutputManager As OutputManager
            Get
                If m_OutputManager Is Nothing Then
                    m_OutputManager = New OutputManager()
                End If
                Return m_OutputManager
            End Get
        End Property

        Public Shared ReadOnly Property ReportResourcesFolder As IO.DirectoryInfo
            Get
                Dim dResourcesFolder As New IO.DirectoryInfo(IO.Path.Combine(m_dResourcesFolder.FullName, m_sReportsFolder))
                Return dResourcesFolder
            End Get
        End Property

        Public Shared ReadOnly Property ds As ProjectDS
            Get
                If m_ProjectDS Is Nothing Then
                    m_ProjectDS = New ProjectDS
                    If Not String.IsNullOrEmpty(FilePath) Then
                        m_ProjectDS.ReadXml(FilePath)
                    End If
                End If
                Return m_ProjectDS
            End Get
        End Property

        Public Shared ReadOnly Property DefaultRasterType As GCDConsoleLib.Raster.RasterDriver
            Get
                Return m_eDefaultRasterType
            End Get
        End Property

        Public Shared ReadOnly Property ExcelTemplatesFolder As IO.DirectoryInfo
            Get
                Return m_dExcelTemplatesFolder
            End Get
        End Property

        Public Shared ReadOnly Property DisplayUnits As UnitsNet.Units.LengthUnit
            Get
                If Not IsDBNull(CurrentProject.DisplayUnits) Then
                    Return DirectCast([Enum].Parse(GetType(UnitsNet.Units.LengthUnit), "Meter"), UnitsNet.Units.LengthUnit)
                End If
                Return Nothing
            End Get
        End Property

        Public Shared Property SurveyTypes As Dictionary(Of String, SurveyType)
            Get
                Return SurveyType.Load(m_fiSurveyTypes)
            End Get
            Set(value As Dictionary(Of String, SurveyType))
                SurveyType.Save(m_fiSurveyTypes, value)
            End Set

        End Property

#End Region

        Public Sub New(ByVal sResourcesFolder As String, ByVal ColourErosion As System.Drawing.Color, ByVal ColourDeposition As System.Drawing.Color, ByVal eDefaultRasterType As GCDConsoleLib.Raster.RasterDriver)

            m_OutputManager = New OutputManager()
            m_cColourErosion = ColourErosion
            m_cColourDeposition = ColourDeposition
            m_eDefaultRasterType = eDefaultRasterType

            m_fiSurveyTypes = New FileInfo(Path.Combine(sResourcesFolder, "SurveyTypes.xml"))
            If Not m_fiSurveyTypes.Exists Then
                Dim ex As New Exception("The GCD Survey Types XML file does not exist.")
                ex.Data("Survey Types XML File") = m_fiSurveyTypes.FullName
                Throw ex
            End If

            m_dExcelTemplatesFolder = New IO.DirectoryInfo(System.IO.Path.Combine(sResourcesFolder, EXCELTEMPLATESFOLDERNAME))
            If Not m_dExcelTemplatesFolder.Exists Then
                Dim ex As New Exception("The GCD Excel template folder path does not exist.")
                ex.Data("GCD Excel Template Path") = m_dExcelTemplatesFolder.FullName
                Throw ex
            End If

        End Sub

        Public Shared Function GetRelativePath(ByVal sFullPath As String) As String

            If String.IsNullOrEmpty(m_GCDProjectXMLFilePath) Then
                Throw New Exception("The project XML file path must be provided.")
            End If

            Dim sProjectFolder As String = Path.GetDirectoryName(m_GCDProjectXMLFilePath)
            Dim nIndex As Integer = sFullPath.ToLower.IndexOf(sProjectFolder.ToLower)

            If nIndex >= 0 Then
                sFullPath = sFullPath.Substring(sProjectFolder.Length, sFullPath.Length - sProjectFolder.Length)
                sFullPath = sFullPath.TrimStart(IO.Path.DirectorySeparatorChar)
            End If

            Return sFullPath

        End Function

        Public Shared Function GetAbsolutePath(ByVal sRelativePath As String) As String

            If String.IsNullOrEmpty(m_GCDProjectXMLFilePath) Then
                Throw New Exception("The project XML file path must be provided.")
            End If

            Dim sProjectFolder As String = Path.GetDirectoryName(m_GCDProjectXMLFilePath)
            Dim sResult As String = Path.Combine(sProjectFolder, sRelativePath)
            Return sResult

        End Function

        Public Shared Sub CloseCurrentProject()

            m_ProjectDS = Nothing
            m_GCDProjectXMLFilePath = String.Empty
            GC.Collect()

        End Sub

        Public Shared Sub save()
            m_ProjectDS.WriteXml(FilePath)
        End Sub

    End Class

End Namespace