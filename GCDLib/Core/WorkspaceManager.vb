Imports System.IO
Imports System.Text

Namespace Core

    Public Class WorkspaceManager

        Private Shared m_diWorkspacePath As DirectoryInfo

        Public Shared ReadOnly Property WorkspacePath As String
            Get
                If TypeOf m_diWorkspacePath Is DirectoryInfo Then
                    Return m_diWorkspacePath.FullName
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        Public Shared Function GetTempRaster(sRootName As String) As String

            Dim sResult As String
            sResult = GISDataStructures.Raster.GetNewSafeName(m_diWorkspacePath.FullName, GISDataStructures.Raster.RasterTypes.TIFF, IO.Path.GetFileNameWithoutExtension(sRootName), 13)
            Return sResult

        End Function

        Public Shared Function GetTempShapeFile(sRootName As String) As String

            If String.IsNullOrEmpty(sRootName) Then
                Throw New ArgumentNullException("sRootName")
            End If
            sRootName = IO.Path.ChangeExtension(sRootName, ".shp")
            Dim sResult As String = GISDataStructures.VectorDataSource.GetNewSafeName(m_diWorkspacePath.FullName, sRootName)
            Return sResult

        End Function

        Public Shared Sub Initialize(diWorkspacePath As DirectoryInfo)

            If Not TypeOf diWorkspacePath Is IO.DirectoryInfo Then
                Throw New ArgumentNullException("dWorkspacePath")
            End If

            If Not diWorkspacePath.Exists Then
                Try
                    diWorkspacePath.Create()
                    m_diWorkspacePath = diWorkspacePath
                Catch ex As System.Exception
                    Dim ex2 As New Exception("Error in Workspace.Initialize() creating new workspace folder", ex)
                    ex2.Data.Add("Workspace path", diWorkspacePath.FullName)
                    Throw ex2
                End Try
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ClearWorkspace()

            '' Only proceed if the workspace exists.
            'If Not TypeOf m_diWorkspacePath Is System.IO.DirectoryInfo OrElse Not m_diWorkspacePath.Exists Then
            '    Exit Sub
            'End If

            ''use datasetname to avoid fail on dataset enumeration
            ''TODO: Develop method to delete .mxd files

            'Dim datasetnames As New ArrayList()

            'Dim pWkSp As IWorkspace
            ''Dim pWkSpFactory As IWorkspaceFactory

            'Dim pEDName As IEnumDatasetName
            'Dim pDSName As IDatasetName
            'Dim pName As ESRI.ArcGIS.esriSystem.IName
            'Dim pDataset As IDataset

            ''get shapefile dataserts in folder
            'Dim pWkSpFactory As IWorkspaceFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.ShapeFile)
            'pWkSp = pWkSpFactory.OpenFromFile(WorkspacePath, 0)
            'pEDName = pWkSp.DatasetNames(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass)
            'pDSName = pEDName.Next
            'Do Until pDSName Is Nothing
            '    datasetnames.Add(pDSName)
            '    pDSName = pEDName.Next
            'Loop
            'Runtime.InteropServices.Marshal.ReleaseComObject(pEDName)

            '' PGB - 2 June 2016 - TINs!
            ''get shapefile dataserts in folder
            'Dim pWkSpFactoryTIN As IWorkspaceFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.TIN)
            'pWkSp = pWkSpFactoryTIN.OpenFromFile(WorkspacePath, 0)
            'pEDName = pWkSp.DatasetNames(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTTin)
            'pDSName = pEDName.Next
            'Do Until pDSName Is Nothing
            '    datasetnames.Add(pDSName)
            '    pDSName = pEDName.Next
            'Loop
            'Runtime.InteropServices.Marshal.ReleaseComObject(pEDName)

            ''Get raster datasets in folder
            'pWkSpFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.RasterFile)
            'pWkSp = pWkSpFactory.OpenFromFile(WorkspacePath, 0)
            'pEDName = pWkSp.DatasetNames(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
            'pDSName = pEDName.Next
            'Do Until pDSName Is Nothing
            '    datasetnames.Add(pDSName)
            '    pDSName = pEDName.Next
            'Loop

            ''delete datasets
            'If datasetnames.Count > 0 Then
            '    For Each datasetname As IDatasetName In datasetnames
            '        If TypeOf datasetname Is ESRI.ArcGIS.esriSystem.IName Then
            '            pName = datasetname
            '            Try
            '                pDataset = pName.Open
            '                pDataset.Delete()
            '            Catch ex As Exception
            '                Debug.WriteLine("Could not delete dataset in temporary workspace: " & datasetname.Name)
            '            End Try
            '        End If
            '    Next
            'End If
            ''
            '' PGB 14 Mar 2012
            '' Starting to use more and more file geodatabases in the temporary workspace.
            '' delete these too.
            ''
            'Dim GP As New ESRI.ArcGIS.Geoprocessor.Geoprocessor
            'GP.SetEnvironmentValue("workspace", WorkspacePath)
            'Dim gplWorkspaces As IGpEnumList = GP.ListWorkspaces("", "")
            'Dim sWorkspace As String = gplWorkspaces.Next()

            'Dim DeleteTool As New ESRI.ArcGIS.DataManagementTools.Delete

            'Do While Not String.IsNullOrEmpty(sWorkspace)
            '    Try
            '        DeleteTool.in_data = sWorkspace
            '        GP.Execute(DeleteTool, Nothing)
            '    Catch ex As Exception
            '        Debug.WriteLine("Could not delete workspace" & sWorkspace)
            '    End Try
            '    sWorkspace = gplWorkspaces.Next()
            'Loop
        End Sub

        ''' <summary>
        ''' Get the default workspace
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Should resolve to the application folder, e.g. 
        ''' C:\Documents and Settings\[User]\Application Data\ESSA_Technologies_Ltd\RBT
        ''' Composed of Environment.SpecialFolder.ApplicationData, My.Resousource.idsManufacturer and My.resources.ApplicationNameShort
        ''' 
        ''' PGB 18 Jul 2011 - based on a conversation with Joe Wheaton on 11 July 2011. The 
        ''' default temp workspace will now be in the users application data folder and then:
        ''' appfolder\GCD\TempWorkspace
        '''
        ''' This should also work for the RBT. It is a good solution because the ESSA Technologies
        ''' compontent using the manufaturer was a bit odd anyways.</remarks>
        Public Shared Function GetDefaultWorkspace(sManufacturer As String, sApplicationShort As String) As String

            'Debug.Assert(Not String.IsNullOrEmpty(sManufacturer))
            Debug.Assert(Not String.IsNullOrEmpty(sApplicationShort))

            Dim sDefault As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            ' 
            '
            'sDefault = IO.Path.Combine(IO.Path.Combine(sDefault, sManufacturer), sApplicationShort)
            sDefault = IO.Path.Combine(sDefault, sApplicationShort)
            If Not IO.Directory.Exists(sDefault) Then
                IO.Directory.CreateDirectory(sDefault)
            End If

            sDefault = IO.Path.Combine(sDefault, "TempWorkspace")
            If Not IO.Directory.Exists(sDefault) Then
                IO.Directory.CreateDirectory(sDefault)
            End If

            Return sDefault

        End Function

        ''' <summary>
        ''' Set the temp workspace to a directory
        ''' </summary>
        ''' <param name="sPath"></param>
        ''' <returns></returns>
        ''' <remarks>PGB Apr 2015 - Throw exception rather than debug assert when the temp workspace does not exist.
        ''' Discovered on Steve Fortneys laptop.</remarks>
        Public Shared Function SetWorkspacePath(sPath As String) As String

            m_diWorkspacePath = New System.IO.DirectoryInfo(sPath)
            If String.IsNullOrEmpty(sPath) OrElse Not System.IO.Directory.Exists(sPath) Then
                Dim ex As New Exception("The specified temp workspace directory is null or does not exist. Go to GCD Options and set the temp workspace to a valid folder.")
                ex.Data("Path") = sPath
                Throw ex
            End If

            Return m_diWorkspacePath.FullName

        End Function

        Public Shared Function SetWorkspacePathDefault(sManufacturer As String, sApplicationShort As String) As String

            'Dim ApplicationDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim diPath As New DirectoryInfo(GetDefaultWorkspace(sManufacturer, sApplicationShort)) 'IO.Path.Combine(IO.Path.Combine(ApplicationDataFolder, sManufacturer), sApplicationShort))

            If Not diPath.Exists Then
                Directory.CreateDirectory(diPath.FullName)
            End If

            If diPath.Exists Then
                m_diWorkspacePath = diPath
            End If

            Return m_diWorkspacePath.FullName

        End Function

        Public Shared Function GetRandomString(Optional ByVal size As Integer = 8, Optional ByVal lowerCase As Boolean = True) As String
            Dim builder As New StringBuilder()
            Dim random As New Random()
            Dim ch As Char
            Dim i As Integer
            For i = 0 To size - 1
                ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)))
                builder.Append(ch)
            Next
            If lowerCase Then
                'For some reason a "[" is sometimes inserted - so this replaces it with a "N"
                'Need to look into this more
                If builder.ToString.Contains("[") Then
                    builder.Replace("[", "N")
                End If
                Return builder.ToString().ToLower()
            End If

            If builder.ToString.Contains("[") Then
                builder.Replace("[", "N")
            End If

            Return builder.ToString()

            'Delay the end of the function for a specified number of ms so that this string is not regenerated with
            'multiple runs of the GetRandomString function since the random seed value is time dependent
            Threading.Thread.Sleep(1000)
        End Function

        ''' <summary>
        ''' Gets a folder path that does not already exist
        ''' </summary>
        ''' <param name="sRootName">Seed name for the folder. Leave blank and the name "Temp" is used</param>
        ''' <returns>Full absolute path to the new folder that is guaranteed not to exist</returns>
        ''' <remarks></remarks>
        Public Shared Function GetSafeDirectoryPath(sRootName As String)

            If String.IsNullOrEmpty(sRootName) Then
                sRootName = "Temp"
            End If

            Dim i As Integer = 0

            Dim sSafeFolder As String = sRootName
            Dim sPath As String
            Do
                If i > 0 Then
                    sSafeFolder = sRootName & i.ToString
                End If
                sPath = IO.Path.Combine(WorkspacePath, sSafeFolder)
                i = i + 1
            Loop While IO.Directory.Exists(sPath)

            Return sPath

        End Function

    End Class

End Namespace