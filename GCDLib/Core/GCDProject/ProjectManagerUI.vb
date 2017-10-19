Imports System.IO
Imports System.Windows.Forms

Namespace Core.GCDProject

    Public Class ProjectManagerUI
        Inherits ProjectManager

        Protected Shared m_PyramidManager As RasterPyramidManager
        Protected Shared m_GCDArcMapManager As GCDArcMapManager

        Public Shared ReadOnly Property ArcMapManager As GCDArcMapManager
            Get
                Return m_GCDArcMapManager
            End Get
        End Property

        Public Shared ReadOnly Property PyramidManager As RasterPyramidManager
            Get
                Return m_PyramidManager
            End Get
        End Property

        Public Sub New(ByVal sFISDatasetPath As String, ByVal sSurveyTypesDatasetPath As String,
                     ByVal eDefaultRasterType As GCDConsoleLib.Raster.RasterDriver,
                     ByVal sResourcesFolder As String,
                     ByVal sExcelTemplateFolder As String,
                     ByVal ColourErosion As System.Drawing.Color,
                     ByVal ColourDeposition As System.Drawing.Color,
                     ByVal sAutomaticPyramids As String)

            MyBase.New(sFISDatasetPath, sResourcesFolder, sExcelTemplateFolder, sSurveyTypesDatasetPath,
                                    eDefaultRasterType,
                                    ColourErosion,
                                    ColourDeposition)

            'm_GCDArcMapManager = New GCDArcMapManager(pArcMap)
            m_PyramidManager = New RasterPyramidManager(sAutomaticPyramids)

        End Sub

        Public Shared Function Validate(Optional bRemoveMissingrasters As Boolean = False) As Boolean
            If My.Settings.ValidateProjectOnLoad Then
                Dim bEditedGCDFile As Boolean = ValidateProjectRastersExist(True)
                'This code cleans the project of empty folders which were not fully deleted due to inability to remove esri locks even after the file has successfully been deleted and removed from GCD xml
                CleanProjectFolderStructure(True)
            End If
        End Function

        Private Shared Function ValidateProjectRastersExist(Optional ByVal bRemoveMissingRasters As Boolean = False) As Boolean

            Dim bEditedGCDFile As Boolean = False
            Dim sOutputMessages As New Text.StringBuilder

            Dim rProject As ProjectDS.ProjectRow = CurrentProject
            If Not TypeOf rProject Is ProjectDS.ProjectRow Then
                Return bEditedGCDFile
            End If

            Dim lItemsToRemove As New List(Of ProjectDS.DEMSurveyRow)
            For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows
                Dim sPath As String = GetAbsolutePath(rDEM.Source)
                If Not GCDConsoleLib.GISDataset.GISDatasetExists(sPath) Then
                    lItemsToRemove.Add(rDEM)
                End If
            Next

            'Pass bEditedGCDFile as a reference argument between these 3 methods 
            bEditedGCDFile = ValidateAssociatedSurfaces(sOutputMessages, bEditedGCDFile, bRemoveMissingRasters)
            bEditedGCDFile = ValidateErrorSurfaces(sOutputMessages, bEditedGCDFile, bRemoveMissingRasters)
            bEditedGCDFile = ValidateDoDAnalysisSurfaces(rProject, sOutputMessages, bEditedGCDFile, bRemoveMissingRasters)

            If bRemoveMissingRasters Then

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
                If Not GCDConsoleLib.GISDataset.GISDatasetExists(sPath) Then
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
                    If Not GCDConsoleLib.GISDataset.GISDatasetExists(sPath) Then
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
                If Not GCDConsoleLib.GISDataset.GISDatasetExists(sPathDoDRaw) Or Not GCDConsoleLib.GISDataset.GISDatasetExists(sPathDoDThresh) Then
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
    End Class

End Namespace