Imports ESRI.ArcGIS.Framework

Namespace UI.UtilityForms

    Public Class ucRasterInput
        Inherits ucInputBase

#Region "Methods"

        Private Sub RasterInputUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            FillComboBox()
        End Sub

        Public Sub FillComboBox()

            GISCode.ArcMap.FillComboFromMapRaster(ArcMap, cboInput, True)
            '
            ' If there is a pre-select item specified. Then ensure it is in the combo
            ' and then select it.
            '
            If TypeOf m_gPreSelect Is GISDataStructures.Raster Then
                Dim nSelectIndex As Integer = -1
                For nIndex As Integer = 0 To cboInput.Items.Count - 1
                    If String.Compare(DirectCast(cboInput.Items(nIndex), GISDataStructures.GISDataSource).FullPath, m_gPreSelect.FullPath, True) = 0 Then
                        nSelectIndex = nIndex
                        Exit For
                    End If
                Next
                If nSelectIndex < 0 Then
                    nSelectIndex = cboInput.Items.Add(m_gPreSelect)
                End If
                cboInput.SelectedIndex = nSelectIndex
            End If
        End Sub

        Protected Overrides Sub Browse()

            Dim sTitle = GISCode.UserInterface.WrapMessageWithNoun("Browse and Select a", Noun, "Raster")
            Try
                Dim gRaster As GISDataStructures.GISDataSource = GISDataStructures.Raster.BrowseOpen(cboInput, sTitle)
                If TypeOf gRaster Is GISDataStructures.Raster Then
                    tTip.SetToolTip(cboInput, gRaster.FullPath)
                End If
            Catch ex As Exception
                naru.error.ExceptionHelper.HandleException(ex, "Error browsing to raster.")
            End Try

        End Sub

        Public Overrides Function Validate() As Boolean

            If Not TypeOf SelectedItem Is GISDataStructures.Raster Then
                MsgBox(GISCode.UserInterface.WrapMessageWithNoun("Please select a", Noun, " to continue."), MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            Return True

        End Function

#End Region

    End Class
End Namespace

