Imports System.IO

Namespace Core.GCDProject

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>New base class for the GCD project manager. This only has some
    ''' of the members from the old GCD Project Manager class - just the ones
    ''' needed by the RBT Console. The GCD still uses the inherited
    ''' GCDProjectManager class that has more members that are specific to
    ''' the desktop software.</remarks>
    Public Class ProjectManagerBase

        Private Shared m_ProjectDS As ProjectDS = Nothing
        Private Shared m_GCDProjectXMLFilePath As String = Nothing

        Protected Shared m_OutputManager As OutputManager
        Protected Const m_sExcelTemplatesFolder As String = "ExcelTemplates"

        ' This is the folder that contains the Excel Templates
        Protected Shared m_dExcelTemplatesFolder As IO.DirectoryInfo

        Private Shared m_cColourErosion As System.Drawing.Color
        Private Shared m_cColourDeposition As System.Drawing.Color

        Private Const m_sReportResourcesFolder As String = "ReportFiles"
        ' This is the folder that contains the symbology layer files
        Private Shared m_dResourcesFolder As IO.DirectoryInfo

        Protected Shared m_GCDNARCError As External.NARCError

        Protected Shared m_eDefaultRasterType As GISDataStructures.RasterTypes

        Public Shared ReadOnly Property GCDNARCError As External.NARCError
            Get
                Return m_GCDNARCError
            End Get
        End Property

        Public Shared Property ColourErosion As System.Drawing.Color
            Get
                Return m_cColourErosion
            End Get
            Set(ByVal value As System.Drawing.Color)
                m_cColourErosion = value
            End Set
        End Property

        Public Shared Property ColourDeposition As System.Drawing.Color
            Get
                Return m_cColourDeposition
            End Get
            Set(ByVal value As System.Drawing.Color)
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
                If m_GCDProjectXMLFilePath Is Nothing Then
                    '
                    ' PGB 8 May. If the user has deleted the last used project, then the software
                    ' throws an error.
                    '
                    'If IO.File.Exists(_filepath) Then
                    '    _filepath = m_sGCDProjectXMLFilePath
                    'End If
                End If
                Return m_GCDProjectXMLFilePath
            End Get
            Set(ByVal value As String)
                m_GCDProjectXMLFilePath = value
                If m_ProjectDS Is Nothing Then
                    m_ProjectDS = New ProjectDS
                End If
                m_ProjectDS.Clear()
                If File.Exists(m_GCDProjectXMLFilePath) Then
                    Try
                        m_ProjectDS.ReadXml(m_GCDProjectXMLFilePath)

                    Catch ex As Exception
                        '
                        ' PGB 8 July 2011 - Need to handled incomplete or corrupted XML files. 
                        '
                        m_GCDProjectXMLFilePath = String.Empty
                        m_ProjectDS = Nothing
                        Throw New Exception("ProjectManager._FilePath(): Error reading GCD Project XML file.", ex)
                    End Try
                End If
            End Set
        End Property

        Public Shared ReadOnly Property OutputManager As OutputManager
            Get
                Return m_OutputManager
            End Get
        End Property

        Public Shared ReadOnly Property ResourcesFolder As IO.DirectoryInfo
            Get
                Return m_dResourcesFolder
            End Get
        End Property

        Public Shared ReadOnly Property ReportResourcesFolder As IO.DirectoryInfo
            Get
                Dim dResourcesFolder As New IO.DirectoryInfo(IO.Path.Combine(m_dResourcesFolder.FullName, m_sReportResourcesFolder))
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

        Public Shared ReadOnly Property DefaultRasterType As GISDataStructures.RasterTypes
            Get
                Return m_eDefaultRasterType
            End Get
        End Property


        Public Shared Sub save()
            m_ProjectDS.WriteXml(FilePath)
        End Sub

        Public Shared ReadOnly Property ExcelTemplatesFolder As IO.DirectoryInfo
            Get
                Return m_dExcelTemplatesFolder
            End Get
        End Property

        Public Shared ReadOnly Property DisplayUnits As naru.math.NumberFormatting.LinearUnits
            Get
                If Not IsDBNull(GCDProject.ProjectManager.CurrentProject.DisplayUnits) Then
                    Return naru.math.NumberFormatting.GetLinearUnitsFromString(GCDProject.ProjectManager.CurrentProject.DisplayUnits)
                End If
                Return Nothing
            End Get
        End Property

        Public Sub New(ByVal eDefaultRasterType As GISDataStructures.RasterTypes,
                ByVal sResourcesFolder As String,
                ByVal sExcelTempateFolder As String,
                ByVal ColourErosion As System.Drawing.Color,
                ByVal ColourDeposition As System.Drawing.Color)

            m_OutputManager = New OutputManager()
            m_cColourErosion = ColourErosion
            m_cColourDeposition = ColourDeposition
            m_eDefaultRasterType = eDefaultRasterType
            m_GCDNARCError = New External.NARCError()

            If IO.Directory.Exists(sResourcesFolder) Then
                m_dResourcesFolder = New IO.DirectoryInfo(sResourcesFolder)
            Else
                'Debug.Assert(False, "The Resource Folder is missing.")
            End If

            If IO.Directory.Exists(sExcelTempateFolder) Then
                m_dExcelTemplatesFolder = New IO.DirectoryInfo(sExcelTempateFolder) ' IO.Path.Combine(sResourcesFolder, "ExcelTemplates"))
            Else
                Dim ex As New Exception("The GCD Excel template folder path does not exist.")
                ex.Data("GCD Excel Template Path") = sExcelTempateFolder
                Throw ex
            End If
        End Sub

        Public Shared Function GetRelativePath(ByVal sFullPath As String) As String

            If String.IsNullOrEmpty(m_GCDProjectXMLFilePath) Then ' OrElse Not IO.File.Exists(sProjectXMLPath) Then
                ' PGB 14 Aug 2014. In the RBT Console the GCD project path is known but not created already.
                Throw New Exception("The project XML file path must be provided.") ' and the file must already exist.")
            End If

            Dim sProjectFolder As String = IO.Path.GetDirectoryName(m_GCDProjectXMLFilePath)
            Dim nIndex As Integer = sFullPath.ToLower.IndexOf(sProjectFolder.ToLower)
            If nIndex >= 0 Then
                sFullPath = sFullPath.Substring(sProjectFolder.Length, sFullPath.Length - sProjectFolder.Length)
                sFullPath = sFullPath.TrimStart(IO.Path.DirectorySeparatorChar)
            End If
            Return sFullPath
        End Function

        Public Shared Function GetAbsolutePath(ByVal sRelativePath As String) As String

            If String.IsNullOrEmpty(m_GCDProjectXMLFilePath) Then ' OrElse Not IO.File.Exists(sProjectXMLPath) Then
                ' PGB 14 Aug 2014. In the RBT Console the GCD project path is known but not created already.
                Throw New Exception("The project XML file path must be provided.") ' and the file must already exist.")
            End If

            Dim sProjectFolder As String = IO.Path.GetDirectoryName(m_GCDProjectXMLFilePath)
            Dim sResult As String = IO.Path.Combine(sProjectFolder, sRelativePath)
            Return sResult

        End Function

        Public Shared Sub CloseCurrentProject()

            m_ProjectDS = Nothing
            m_GCDProjectXMLFilePath = String.Empty
            GC.Collect()

        End Sub

        Public Enum ProjectTypes
            AddIn
            Standalone
            Commandline
            CHaMPRBT
            Custom
        End Enum

    End Class

End Namespace