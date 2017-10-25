Imports System.IO
Imports System.Windows.Forms

Namespace Core.GCDProject

    Public Class ProjectManagerUI
        Inherits ProjectManagerBase

        Protected Shared m_PyramidManager As RasterPyramidManager
        Private Shared m_FISLibrary As FileInfo

        Public Shared ReadOnly Property IsArcMap As Boolean
            Get
                Return Reflection.Assembly.GetEntryAssembly().FullName.ToLower().Contains("arcmap")
            End Get
        End Property

        Public Shared ReadOnly Property PyramidManager As RasterPyramidManager
            Get
                Return m_PyramidManager
            End Get
        End Property

        Public Shared Property FISLibrary As List(Of ErrorCalculation.FIS.FISLibraryItem)
            Get
                If m_FISLibrary.Exists Then
                    Return ErrorCalculation.FIS.FISLibraryItem.Load(m_FISLibrary)
                Else
                    Return New List(Of ErrorCalculation.FIS.FISLibraryItem)
                End If
            End Get
            Set(value As List(Of ErrorCalculation.FIS.FISLibraryItem))
                ErrorCalculation.FIS.FISLibraryItem.Save(m_FISLibrary, value)
            End Set
        End Property

        Private Shared ReadOnly Property ApplicationFolder As String
            Get
                Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GCD")
            End Get
        End Property

        Public Sub New(ByVal eDefaultRasterType As GCDConsoleLib.Raster.RasterDriver, ByVal sAutomaticPyramids As String)
            MyBase.New(ApplicationFolder, My.Settings.Erosion, My.Settings.Depsoition, eDefaultRasterType)

            m_PyramidManager = New RasterPyramidManager(sAutomaticPyramids)
            m_FISLibrary = New FileInfo(Path.Combine(Environment.SpecialFolder.ApplicationData, "FISLibrary.xml"))

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="bRemoveMissingrasters"></param>
        ''' <returns>True if the GCD project has been altered during the validation process</returns>
        Public Shared Function Validate(Optional bRemoveMissingrasters As Boolean = False) As Boolean

            If Not My.Settings.ValidateProjectOnLoad Then
                Return False
            End If

            Dim bEditedGCDFile As Boolean = False
            Dim sOutputMessages As New Text.StringBuilder

            Dim rProject As ProjectDS.ProjectRow = CurrentProject
            If Not TypeOf rProject Is ProjectDS.ProjectRow Then
                Return bEditedGCDFile
            End If

            Dim lItemsToRemove As New List(Of ProjectDS.DEMSurveyRow)
            For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows
                Dim sPath As String = GetAbsolutePath(rDEM.Source)
                If Not GCDConsoleLib.GISDataset.FileExists(sPath) Then
                    lItemsToRemove.Add(rDEM)
                End If
            Next

            'Pass bEditedGCDFile as a reference argument between these 3 methods 
            bEditedGCDFile = ValidateAssociatedSurfaces(sOutputMessages, bEditedGCDFile, bRemoveMissingrasters)
            bEditedGCDFile = ValidateErrorSurfaces(sOutputMessages, bEditedGCDFile, bRemoveMissingrasters)
            bEditedGCDFile = ValidateDoDAnalysisSurfaces(rProject, sOutputMessages, bEditedGCDFile, bRemoveMissingrasters)

            If bRemoveMissingrasters Then

                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count)

                For Each rDEM As ProjectDS.DEMSurveyRow In lItemsToRemove
                    sOutputMessages.Append("Removing DEM Survey '" & rDEM.Name & "' because the source raster is missing or cannot be found.").AppendLine()
                    ds.DEMSurvey.RemoveDEMSurveyRow(rDEM)
                    Debug.Write(sOutputMessages.ToString)
                    bEditedGCDFile = True
                Next
            End If

            Try
                save()

            Catch ex As Exception
                ex.Data("Removing datasets") = sOutputMessages
                Throw
            End Try

            Return bEditedGCDFile

            ' This code cleans the project of empty folders which were not fully deleted due to inability to remove
            ' ESRI locks even after the file has successfully been deleted And removed from GCD xml
            CleanProjectFolderStructure(True)

            Return bEditedGCDFile

        End Function

        Private Shared Function ValidateAssociatedSurfaces(ByRef sOutputMessages As Text.StringBuilder, ByRef bEditedGCDFile As Boolean, Optional ByVal bRemoveMissingRasters As Boolean = False) As Boolean

            Dim lItemsToRemove As New List(Of ProjectDS.AssociatedSurfaceRow)
            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In ds.AssociatedSurface.Rows
                Dim dbTest = CType(rAssoc, DataRow)
                If IsDBNull(dbTest("OriginalSource")) Then
                    rAssoc.OriginalSource = rAssoc.Source
                    Dim sAssociatedSurfaceName As String = System.IO.Path.GetFileNameWithoutExtension(rAssoc.OriginalSource)
                    rAssoc.Source = GetRelativePath(m_OutputManager.AssociatedSurfaceRasterPath(rAssoc.DEMSurveyRow.Name, sAssociatedSurfaceName))
                    CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, 1)
                    bEditedGCDFile = True
                    save()
                End If
                Dim sPath As String = GetAbsolutePath(rAssoc.Source)
                If Not GCDConsoleLib.GISDataset.FileExists(sPath) Then
                    lItemsToRemove.Add(rAssoc)
                End If
            Next

            If bRemoveMissingRasters Then

                'Check to see if a copy of the .gcd project is necessary
                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count)

                For Each rAssoc As ProjectDS.AssociatedSurfaceRow In lItemsToRemove
                    sOutputMessages.Append("Removing Associated Surface '" & rAssoc.Name & "' because the source raster is missing or cannot be found.").AppendLine()
                    ds.AssociatedSurface.RemoveAssociatedSurfaceRow(rAssoc)
                    Debug.Write(sOutputMessages.ToString)
                    bEditedGCDFile = True
                Next
            End If

            Return bEditedGCDFile

        End Function

        Private Shared Function ValidateErrorSurfaces(ByRef sOutputMessages As Text.StringBuilder, ByRef bEditedGCDFile As Boolean, Optional ByVal bRemoveMissingRasters As Boolean = False) As Boolean

            Dim lItemsToRemove As New List(Of ProjectDS.ErrorSurfaceRow)
            For Each rError As ProjectDS.ErrorSurfaceRow In ds.ErrorSurface.Rows
                If rError.IsSourceNull Then
                    lItemsToRemove.Add(rError)
                Else
                    Dim sPath As String = GetAbsolutePath(rError.Source)
                    If Not GCDConsoleLib.GISDataset.FileExists(sPath) Then
                        lItemsToRemove.Add(rError)
                    End If
                End If
            Next

            If bRemoveMissingRasters Then

                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count)

                For Each rError As ProjectDS.ErrorSurfaceRow In lItemsToRemove
                    Dim sMessage As String = "Removing Error Surface '" & rError.Name & "' because the source raster is missing or cannot be found."
                    ds.ErrorSurface.RemoveErrorSurfaceRow(rError)
                    sOutputMessages.Append(sMessage).AppendLine()
                    Debug.Write(sOutputMessages.ToString)
                    bEditedGCDFile = True
                Next
            End If

            Return bEditedGCDFile

        End Function

        Private Shared Function ValidateDoDAnalysisSurfaces(ByRef rProjectRow As ProjectDS.ProjectRow, ByRef sOutputMessages As Text.StringBuilder, ByRef bEditedGCDFile As Boolean, Optional ByVal bRemoveMissingRasters As Boolean = False) As Boolean

            Dim lItemsToRemove As New List(Of ProjectDS.DoDsRow)
            For Each rDoD As ProjectDS.DoDsRow In ds.DoDs.Rows
                Dim sPathDoDRaw As String = GetAbsolutePath(rDoD.RawDoDPath)
                Dim sPathDoDThresh As String = GetAbsolutePath(rDoD.ThreshDoDPath)
                If Not GCDConsoleLib.GISDataset.FileExists(sPathDoDRaw) Or Not GCDConsoleLib.GISDataset.FileExists(sPathDoDThresh) Then
                    lItemsToRemove.Add(rDoD)
                End If
            Next

            If bRemoveMissingRasters Then

                CheckCopyStatusOfGCDProject(FilePath, bEditedGCDFile, lItemsToRemove.Count)

                For Each rDoD As ProjectDS.DoDsRow In lItemsToRemove
                    sOutputMessages.Append("Removing DoD Surface '" & rDoD.Name & "' because the source raster is missing or cannot be found.").AppendLine()
                    ds.DoDs.RemoveDoDsRow(rDoD)
                    Debug.Write(sOutputMessages.ToString)
                    bEditedGCDFile = True
                Next
            End If

            Return bEditedGCDFile

        End Function

        Private Shared Sub CleanProjectFolderStructure(Optional ByVal bRemoveEmptyFolders As Boolean = False)

            If FilePath Is Nothing Then
                Exit Sub
            End If

            Dim sPath As String = IO.Path.GetDirectoryName(FilePath)
            Try
                If bRemoveEmptyFolders Then
                    DeleteEmptyFolders(sPath)
                End If
            Catch ex As Exception
                'Do Nothing
            End Try

        End Sub

        Private Shared Function CheckCopyStatusOfGCDProject(ByVal sGCDProjectFilePath As String, ByVal bEditedGCDFile As Boolean, ByVal iItemsToRemove As Integer) As String

            If String.IsNullOrEmpty(sGCDProjectFilePath) Then
                Return Nothing
            End If

            'If gcd file has not been edited in previous steps
            If bEditedGCDFile = False Then
                'If there are elements of GCD file currently marked for removal
                If iItemsToRemove > 0 Then

                    'Have established that gcd file has not been edited, i.e. don't want to make a backup of something that has already changed
                    'and established that there items from this set to be removed from the gcd file therefore make a copy of the .gcd file

                    'Copy GCD file, give backup file timestamp of when copy is made
                    Dim sGCDProjectCopyName As String = System.IO.Path.GetFileNameWithoutExtension(sGCDProjectFilePath)
                    Dim sTimeStamp As DateTime = DateTime.Now
                    Dim format As String = "yyyydMHHmmss" ' format is one integer of year day hours minutes seconds
                    sGCDProjectCopyName = sGCDProjectCopyName & sTimeStamp.ToString(format) & ".gcd"
                    Dim sBackUpPath As String = IO.Path.Combine(System.IO.Path.GetDirectoryName(sGCDProjectFilePath), "Backup")
                    sGCDProjectCopyName = System.IO.Path.Combine(sBackUpPath, sGCDProjectCopyName)

                    If System.IO.Directory.Exists(sBackUpPath) Then
                        System.IO.File.Copy(sGCDProjectFilePath, sGCDProjectCopyName)
                    Else
                        System.IO.Directory.CreateDirectory(sBackUpPath)
                        System.IO.File.Copy(sGCDProjectFilePath, sGCDProjectCopyName)
                    End If

                    Return sGCDProjectCopyName

                End If
            End If

            Return Nothing

        End Function

        Private Shared Sub DeleteEmptyFolders(ByVal sPath As String)

            Dim SubDirectories() As String = Directory.GetDirectories(sPath)
            For Each strDirectory As String In SubDirectories
                DeleteEmptyFolders(strDirectory)
            Next

            If Directory.GetFiles(sPath).Length + Directory.GetDirectories(sPath).Length = 0 Then
                Directory.Delete(sPath)
                Dim sMessage As String = "Deleting folder '" & sPath & "' from project file system because the folder is empty." & vbCrLf
                Debug.Write(sMessage.ToString)
            End If
        End Sub

        Public Shared Sub CopyDeployFolder()

            Dim sDestinationFolder As String = ApplicationFolder

            'New Code to test this may not be what you intend though
            Dim sFolderName As String = "Deploy"
            If String.Compare(sFolderName, "Reources\FIS", True) = 0 Then

                sDestinationFolder = IO.Path.Combine(sDestinationFolder, sFolderName)
                Dim sExecutingAssemblyFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                Dim sOriginalFolder As String = IO.Path.Combine(sExecutingAssemblyFolder, sFolderName)

                If IO.Directory.Exists(IO.Path.GetDirectoryName(sDestinationFolder)) Then
                    CopyDirectory(sOriginalFolder, sDestinationFolder)
                ElseIf Not IO.Directory.Exists(IO.Path.GetDirectoryName(sDestinationFolder)) Then
                    IO.Directory.CreateDirectory(sDestinationFolder)
                    Debug.WriteLine("Copying AddIn Folder """ & sOriginalFolder & """ to """ & sDestinationFolder & """")
                    CopyDirectory(sOriginalFolder, sDestinationFolder)
                End If
            Else
                Directory.CreateDirectory(sDestinationFolder)
                Dim sOriginalFolder As String = IO.Path.Combine(Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location), "Deploy")
                'sDestinationFolder = Path.Combine(sDestinationFolder, Path.GetFileNameWithoutExtension(sFolderName))
                Debug.WriteLine("Copying AddIn Folder """ & sOriginalFolder & """ to """ & sDestinationFolder & """")
                CopyDirectory(sOriginalFolder, ApplicationFolder)
            End If

        End Sub

        Private Shared Sub CopyDirectory(ByVal sourcePath As String, ByVal destPath As String)

            If Not IO.Directory.Exists(destPath) Then
                Try
                    Dim dFolder As IO.DirectoryInfo = IO.Directory.CreateDirectory(destPath)
                    If Not dFolder.Exists Then
                        Dim ex As New Exception("Failed to create directory.")
                        ex.Data("Directory") = destPath
                        Throw ex
                    End If
                Catch ex As IO.DirectoryNotFoundException
                    Dim ex2 As New Exception("The specified path is invalid (for example, it is on an unmapped drive).", ex)
                    Throw ex2

                Catch ex As IO.IOException
                    Dim ex2 As New Exception("The directory specified by path is a file or the network name is not known.", ex)
                    Throw ex2

                Catch ex As UnauthorizedAccessException
                    Dim ex2 As New Exception("The caller does not have the required permission.", ex)
                    Throw ex2

                Catch ex As ArgumentNullException
                    Dim ex2 As New Exception("Path is Nothing.", ex)
                    Throw ex2

                Catch ex As ArgumentException
                    Dim ex2 As New Exception("The path is a zero-length string, contains only white space, or contains one or more invalid characters. You can query for invalid characters by using the GetInvalidPathChars method or path is prefixed with, or contains, only a colon character (:).", ex)
                    Throw ex2

                Catch ex As PlatformNotSupportedException
                    Dim ex2 As New Exception("The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.", ex)
                    Throw ex2

                Catch ex As NotSupportedException
                    Dim ex2 As New Exception("path contains a colon character (:) that is not part of a drive label (""C:\"").", ex)
                    Throw ex2

                Catch ex As Exception
                    Dim ex2 As New Exception("Failed to create directory.")
                    ex2.Data("Directory") = destPath
                    Throw ex2
                End Try
            End If

            For Each file1 As String In IO.Directory.GetFiles(sourcePath)
                Dim dest As String = IO.Path.Combine(destPath, IO.Path.GetFileName(file1))
                If IO.File.Exists(file1) Then
                    If String.Compare("FISLibraryXML.xml", IO.Path.GetFileName(file1)) = 0 Then
                        If Not IO.File.Exists(dest) Then
                            IO.File.Copy(file1, dest, True)
                        End If
                    Else
                        IO.File.Copy(file1, dest, True)  ' Added True here to force the an overwrite 
                    End If
                End If
            Next

            ' Use directly the sourcePath passed in, not the parent of that path
            For Each dir1 As String In IO.Directory.GetDirectories(sourcePath)
                Dim destdir As String = IO.Path.Combine(destPath, IO.Path.GetFileName(dir1))
                CopyDirectory(dir1, destdir)
            Next
        End Sub

    End Class

End Namespace