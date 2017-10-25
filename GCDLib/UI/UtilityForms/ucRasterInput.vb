
Namespace UI.UtilityForms

    Public Class ucRasterInput
        Inherits naru.ui.ucInput

        Private m_sNoun As String

        Public Property Noun As String
            Get
                Return m_sNoun
            End Get
            Set(value As String)
                m_sNoun = value
            End Set
        End Property

        Public Event BrowseRaster(txtPath As System.Windows.Forms.TextBox, e As naru.ui.PathEventArgs)
        Public Event SelectRasterFromArcMap(txtPath As System.Windows.Forms.TextBox, e As naru.ui.PathEventArgs)

        Public Shadows Sub Initialize(sNoun As String, fiPath As IO.FileInfo, bRequiredInput As Boolean)
            MyBase.Initialize(fiPath, bRequiredInput)
            m_sNoun = sNoun
        End Sub

        'Public Event RasterSelected(e As naru)

        Public ReadOnly Property SelectedItem As GCDConsoleLib.Raster
            Get
                If TypeOf Path Is System.IO.FileInfo Then
                    Return New GCDConsoleLib.Raster(Path.FullName)
                Else
                    Return Nothing
                End If
            End Get

        End Property

        Public Sub cmdBrowseRaster_Click(sender As Object, e As naru.ui.PathEventArgs) Handles MyBase.BrowseFile

            Try
                If Core.GCDProject.ProjectManagerUI.IsArcMap Then
                    RaiseEvent BrowseRaster(sender, e)
                Else
                    naru.ui.Textbox.BrowseOpenRaster(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"))
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex, "Error browsing to raster")
            End Try

        End Sub

        Public Sub cmdSelectRaster_Click(sender As Object, e As naru.ui.PathEventArgs) Handles MyBase.SelectLayer

            Try
                RaiseEvent SelectRasterFromArcMap(sender, e)

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex, "Error selecting raster from ArcMap")
            End Try

        End Sub

        'Public Overrides Function Validate() As Boolean

        '    If Not TypeOf SelectedItem Is GCDConsoleLib.Raster Then
        '        System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " to continue."), My.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        '        Return False
        '    End If

        '    Return True

        'End Function

    End Class

End Namespace

