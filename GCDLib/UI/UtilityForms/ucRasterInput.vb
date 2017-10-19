
Namespace UI.UtilityForms

    Public Class ucRasterInput
        Inherits naru.ui.ucInput

        Private m_sNoun As String

        Public ReadOnly Property Noun As String
            Get
                Return m_sNoun
            End Get
        End Property

        Public Event BrowseRaster(txtPath As System.Windows.Forms.TextBox, e As naru.ui.PathEventArgs)
        Public Event SelectRasterFromArcMap(txtPath As System.Windows.Forms.TextBox, e As naru.ui.PathEventArgs)

        Public Shadows Sub Initialize(sNoun As String, fiPath As System.IO.FileInfo, bRequiredInput As Boolean)
            m_sNoun = sNoun
        End Sub

        'Public Event RasterSelected(e As naru)

        Public ReadOnly Property SelectedItem As RasterWranglerLib.Raster
            Get
                If TypeOf Path Is System.IO.FileInfo Then
                    Return New RasterWranglerLib.Raster(Path.FullName)
                Else
                    Return Nothing
                End If
            End Get

        End Property

        Public Sub cmdBrowseRaster_Click(sender As Object, e As naru.ui.PathEventArgs) Handles MyBase.BrowseFile

            Try
                RaiseEvent BrowseRaster(sender, e)
                'naru.ui.Textbox.BrowseOpenRaster(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"))

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

        '    If Not TypeOf SelectedItem Is Core.RasterWranglerLib.Raster Then
        '        System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " to continue."), My.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        '        Return False
        '    End If

        '    Return True

        'End Function

    End Class

End Namespace

