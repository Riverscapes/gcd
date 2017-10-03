Imports System.IO
Imports System.Windows.Forms

Namespace Core.FileSystem

    Public Module FileSystem

        Public Function BrowseToFolder(ByVal sFormTitle As String, Optional ByVal sDefaultFolder As String = "") As String

            Dim sFolder As String = String.Empty
            '
            ' If a default folder is passed, then check it exists and
            ' then use it as the initial folder.
            '
            If Not String.IsNullOrEmpty(sDefaultFolder) Then
                Dim dir As New DirectoryInfo(sDefaultFolder)
                If dir.Exists Then
                    sFolder = sDefaultFolder
                End If
            End If

            Dim FolderBrowserDialog As New FolderBrowserDialog
            ' Change the .SelectedPath property to the default location
            With FolderBrowserDialog
                .RootFolder = Environment.SpecialFolder.Desktop
                If Not String.IsNullOrEmpty(sFolder) Then
                    .SelectedPath = sFolder
                End If
                .Description = sFormTitle

                If .ShowDialog = DialogResult.OK Then
                    sFolder = .SelectedPath
                    '
                    'Saving this setting is now the responsibility of the calling function
                    ' My.Settings.LastBrowsedFolder = sFolder
                    'My.Settings.Save()
                Else
                    sFolder = String.Empty
                End If
            End With

            Return sFolder

        End Function

        Public Function GetNewSafeFileName(sFolder As String, sRootName As String, sExtension As String) As String

            If String.IsNullOrEmpty(sFolder) Then
                Throw New ArgumentNullException("Folder", "Null or empty folder")
            End If

            If Not IO.Directory.Exists(sFolder) Then
                Dim ex As New Exception("The Folder must already exist")
                ex.Data("Folder") = sFolder
                Throw ex
            End If

            If String.IsNullOrEmpty(sExtension) Then
                Dim ex As New ArgumentNullException("Extension", "Null or empty file extension")
                ex.Data("Folder") = sFolder
                ex.Data("sRootName") = sRootName
                Throw ex
            End If

            sRootName = RemoveDangerousCharacters(sRootName)
            Dim sNewName As String
            Dim nCount As Integer = 0
            Dim sResult As String
            Do
                If String.IsNullOrEmpty(sRootName) Then
                    sNewName = "gis"
                Else
                    sNewName = sRootName
                End If

                If nCount > 0 Then
                    sNewName &= nCount.ToString
                End If

                sResult = IO.Path.Combine(sFolder, IO.Path.ChangeExtension(sNewName, sExtension))
                nCount += 1
            Loop While IO.File.Exists(sResult) AndAlso nCount < 9999

            Return sResult

        End Function

        Public Function GetNewSafeDirectoryName(ByVal sParent As String, ByVal sRootName As String) As String

            If String.IsNullOrEmpty(sParent) Then
                Throw New ArgumentNullException("The parent folder cannot be an empty string.")
            Else
                If Not IO.Directory.Exists(sParent) Then
                    Dim ex As New ArgumentException("The parent folder must already exist.")
                    ex.Data("Parent folder") = sParent
                    Throw ex
                End If
            End If

            If String.IsNullOrEmpty(sRootName) Then
                Throw New ArgumentNullException("The root name cannot be an empty string.")
            End If

            Dim sNewName As String
            Dim nCount As Integer = 0
            Dim sResult As String

            Do
                sNewName = sRootName
                If nCount > 0 Then
                    sNewName &= nCount.ToString
                End If
                sResult = IO.Path.Combine(sParent, sNewName)
                nCount += 1
            Loop While IO.Directory.Exists(sResult) AndAlso nCount < 9999

            Return sResult

        End Function

        ''' <summary>
        ''' Gets a file name that doesn't already exist, based on a suggested file path.
        ''' </summary>
        ''' <param name="sSuggestedPath"></param>
        ''' <returns></returns>
        ''' <remarks>Use this method to get a file path that is guaranteed to be unique based on a suggested path and file name
        ''' that might already exist</remarks>
        Public Function GetNewSafeFileName(sSuggestedPath As String) As String

            If String.IsNullOrEmpty(sSuggestedPath) Then
                Return String.Empty
            End If

            Dim sSafeFilePath As String
            Dim i As Integer = 0
            Do
                ' 
                ' Get the initially suggested file name without an extension
                '
                sSafeFilePath = IO.Path.GetFileNameWithoutExtension(sSuggestedPath)
                If i > 0 Then
                    sSafeFilePath += i.ToString
                End If
                '
                ' Add back any extension
                '
                If IO.Path.HasExtension(sSuggestedPath) Then
                    sSafeFilePath = IO.Path.ChangeExtension(sSafeFilePath, IO.Path.GetExtension(sSuggestedPath))
                End If
                '
                ' Build the final path
                sSafeFilePath = IO.Path.Combine(IO.Path.GetDirectoryName(sSuggestedPath), sSafeFilePath)
                i += 1
            Loop While IO.File.Exists(sSafeFilePath) AndAlso i < 1000

            Return sSafeFilePath

        End Function

        ''' <summary>
        ''' Removes any characters that are not allowed in file names
        ''' </summary>
        ''' <param name="sInput">String represting a file name (without the extension!)</param>
        ''' <returns>cleaned string without dangerous characters</returns>
        ''' <remarks>Do not include the file extension when passing in a string</remarks>
        Public Function RemoveDangerousCharacters(sInput As String) As String

            Dim sResult As String = sInput
            If Not String.IsNullOrEmpty(sInput) Then
                For Each c As Char In IO.Path.GetInvalidFileNameChars
                    sResult = sResult.Replace(c, "")
                Next
            End If

            sResult = sResult.Replace("-", "")
            sResult = sResult.Replace("_", "")
            sResult = sResult.Replace(" ", "")
            sResult = sResult.Replace(".", "")
            sResult = sResult.Replace("!", "")
            sResult = sResult.Replace("@", "")
            sResult = sResult.Replace("#", "")
            sResult = sResult.Replace("$", "")
            sResult = sResult.Replace("%", "")
            sResult = sResult.Replace("^", "")
            sResult = sResult.Replace("&", "")
            sResult = sResult.Replace("*", "")
            sResult = sResult.Replace("(", "")
            sResult = sResult.Replace(")", "")
            sResult = sResult.Replace("+", "")
            sResult = sResult.Replace("=", "")
            sResult = sResult.Replace("'", "")
            sResult = sResult.Replace("~", "")
            sResult = sResult.Replace("`", "")
            sResult = sResult.Replace("{", "")
            sResult = sResult.Replace("}", "")
            sResult = sResult.Replace("[", "")
            sResult = sResult.Replace("]", "")
            sResult = sResult.Replace(";", "")
            sResult = sResult.Replace(",", "")

            Return sResult

        End Function

        Public Function TrimFilename(filename As String, trimmedlength As Integer)
            Dim trimmedfilename As String = ""
            If filename.Length > trimmedlength Then

                Dim PathSegments As String() = filename.Split(IO.Path.AltDirectorySeparatorChar)
                trimmedfilename = PathSegments(PathSegments.Length - 1)
                Dim PathIndex As Integer = PathSegments.Length - 2
                While PathIndex >= 0 AndAlso trimmedfilename.Length + PathSegments(PathIndex).Length < 80
                    trimmedfilename = IO.Path.Combine(PathSegments(PathIndex), trimmedfilename)
                    PathIndex = PathIndex - 1
                End While
                If PathIndex > -1 Then
                    trimmedfilename = "...\" & trimmedfilename
                End If
            Else
                trimmedfilename = filename
            End If
            Return trimmedfilename
        End Function

        ''' <summary>
        ''' Deletes a file and does exception handling
        ''' 
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <remarks>
        ''' see http://msdn.microsoft.com/en-us/library/system.io.file.delete.aspx for a list of exceptions
        ''' IOException: The specified file is in use.
        ''' UnauthorizedAccessException: The caller does not have the required permission. or path is a directory. or path specified a read-only file.
        ''' 
        ''' </remarks>
        Public Sub DeleteFile(ByVal filename As String)

            Dim trimmedfilename As String = TrimFilename(filename, 80)

            If Not File.Exists(filename) Then
                Exit Sub
            End If

            'check for readonly
            Dim fInfo As New FileInfo(filename)
            If fInfo.IsReadOnly Then
                Dim ex As New Exception("'" & filename & "' is readonly")
                ex.Data("UIMessage") = "Could not delete '" & trimmedfilename & "'. File is readonly."
                Throw ex
            End If

            Try
                File.Delete(filename)
            Catch ex As Exception
                If TypeOf ex Is IOException Then
                    'check for The specified file is in use.
                    ex.Data("UIMessage") = "Could not delete '" & trimmedfilename & "'. File is in use."
                    Throw
                End If
                ex.Data("UIMessage") = "Could not delete '" & trimmedfilename & "'."
                Throw
            End Try
        End Sub

    End Module
End Namespace
