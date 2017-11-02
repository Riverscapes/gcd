Imports GCDLib.Core

Namespace UtilityForms

    Public Class ucRasterOutput
        Inherits ucOutputBase

        Private m_eDefaultRasterType As GCDConsoleLib.Raster.RasterDriver

        Protected Overrides Function Browse() As String

            Dim sTitle As String = naru.ui.UIHelpers.WrapMessageWithNoun("Select", Noun, "Output Location")
            Return naru.ui.Textbox.BrowseSaveRaster(txtOutput, sTitle, txtOutput.Text)

        End Function

        Public Overrides Function FullPath() As String
            Dim sFullPath As String = txtOutput.Text
            If String.IsNullOrEmpty(sFullPath) Then
                If String.IsNullOrEmpty(m_sInitialDatasetName) Then
                    Return String.Empty
                Else
                    sFullPath = IO.Path.Combine(m_sWorkspace, m_sInitialDatasetName)
                End If
            End If

            If Not IO.Path.HasExtension(sFullPath) Then
                sFullPath = IO.Path.ChangeExtension(sFullPath, "tif")
            End If

            Return sFullPath
        End Function

        Public Shadows Sub Initialize(ByVal sNoun As String, ByVal sInitialDatasetName As String, ByVal eType As GCDConsoleLib.Raster.RasterDriver)
            MyBase.Initialize(sNoun, sInitialDatasetName)
            m_eDefaultRasterType = eType
        End Sub


        Public Overrides Function Validate() As Boolean

            If String.IsNullOrEmpty(txtOutput.Text) Then
                MsgBox("You must specify an output " & Noun & " feature class", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If GCDConsoleLib.GISDataset.FileExists(txtOutput.Text) Then
                If MsgBox("The " & Noun & " raster already exists. Do you wish to overwrite it?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong) = MsgBoxResult.Yes Then
                    Try
                        Dim gRaster As New GCDConsoleLib.Raster(txtOutput.Text)
                        gRaster.Delete()
                    Catch ex As Exception
                        naru.error.ExceptionUI.HandleException(ex, "Error attempting to delete the existing raster")
                        Return False
                    End Try
                Else
                    Return False
                End If
            End If

            Throw New NotFiniteNumberException
            'If Not IsValidPath Then
            '    MsgBox("The output path is not a valid GIS path name.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            '    Return False
            'End If

            Return True

        End Function

        'Public Overrides ReadOnly Property IsValidPath() As Boolean
        '    Get
        '        Dim bValid As Boolean = False
        '        Dim sFullPath As String = FullPath()
        '        Dim sWorkspace As String = Core.GISDataStructures.GISDataSource.GetWorkspacePath(sFullPath)

        '        If String.IsNullOrEmpty(sWorkspace) Then
        '            bValid = False
        '        Else
        '            If GISDataStructures.IsFileGeodatabase(sFullPath) Then
        '                ' file geodatabase rasters have no extension.
        '                bValid = String.IsNullOrEmpty(IO.Path.GetExtension(sFullPath))
        '            Else
        '                If IO.Directory.Exists(sWorkspace) Then
        '                    If Not String.IsNullOrEmpty(IO.Path.GetFileNameWithoutExtension(sFullPath)) Then
        '                        Dim sExt As String = IO.Path.GetExtension(sFullPath)
        '                        If String.IsNullOrEmpty(sExt) Then
        '                            ' ESRI Grids = folders = have no file extentsion.
        '                            bValid = False
        '                        Else
        '                            ' file based rasters
        '                            If String.Compare(sExt, ".img", True) = 0 Then
        '                                bValid = True
        '                            ElseIf String.Compare(sExt, ".tif", True) = 0 Then
        '                                bValid = True
        '                            End If
        '                        End If
        '                    Else
        '                        bValid = False
        '                    End If
        '                Else
        '                    bValid = False
        '                End If
        '            End If
        '        End If

        '        Return bValid
        '    End Get
        'End Property
    End Class
End Namespace