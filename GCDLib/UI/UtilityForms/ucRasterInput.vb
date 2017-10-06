Imports GCD.GCDLib.UI.UtilityForms.InputUCSelectedItemChangedEventArgs

Namespace UI.UtilityForms

    Public Class ucRasterInput
        Inherits ucInputBase

        Public Event BrowseRaster(txtPath As System.Windows.Forms.TextBox, e As BrowseLayerEventArgs)

        Public Event RasterSelected(e As BrowseLayerEventArgs)


        Public Sub cmdBrowseRaster_Click(sender As Object, e As EventArgs) Handles cmdBrowse.Click

            Try
                RaiseEvent BrowseRaster(txtPath, New BrowseLayerEventArgs(naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"), Core.GISDataStructures.BrowseGISTypes.Raster, MyBase.txtPath.Text))
                'naru.ui.Textbox.BrowseOpenRaster(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"))

            Catch ex As Exception
                Core.ExceptionHelper.HandleException(ex, "Error browsing to raster.")
            End Try

        End Sub

        Public Overrides Function Validate() As Boolean

            If Not TypeOf SelectedItem Is Core.GISDataStructures.Raster Then
                System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " to continue."), My.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
                Return False
            End If

            Return True

        End Function



    End Class

End Namespace

