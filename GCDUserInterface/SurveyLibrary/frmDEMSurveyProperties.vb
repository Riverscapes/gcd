Imports System.Windows.Forms

Namespace SurveyLibrary

    Public Class frmDEMSurveyProperties

        Private DEM As DEMSurvey
        Private m_ImportRasterform As frmImportRaster

#Region "Survey Property Routines"

        Public Sub New(editDEM As DEMSurvey)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            DEM = editDEM
        End Sub

        Private Sub SurveyPropertiesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            InitControls()
            cboSingle.Items.AddRange(ProjectManager.SurveyTypes.Values.ToArray())

            txtName.Text = DEM.Name
            txtRasterPath.Text = DEM.Raster.RelativePath
            txtMask.Text = DEM.MethodMaskField
            txtFolder.Text = DEM.Raster.RasterPath.DirectoryName
            rdoSingle.Checked = DEM.IsSingleSurveyMethod

            If DEM.IsSingleSurveyMethod Then
                ' Select the single survey method
                If Not String.IsNullOrEmpty(DEM.SurveyMethod) Then
                    cboSingle.SelectedItem = DEM.SurveyMethod
                End If
            Else
                ' Load the fields from the Polygon mask
                If TypeOf DEM.MethodMask Is IO.FileInfo AndAlso DEM.MethodMask.Exists Then
                    ' This should fire the reloading of the mask field dropdown
                    txtMask.Text = DEM.MethodMask.FullName
                End If
            End If

            ' Turn on handling survey method event handling (after setting control from project)
            AddHandler rdoSingle.CheckedChanged, AddressOf rdoSingle_CheckedChanged

            If DEM.SurveyDate Is Nothing Then
                lblDatetime.Text = SurveyDateTime.NotSetString
            Else
                lblDatetime.Text = DEM.SurveyDate.ToString
            End If

            UpdateControls()
            LoadRasterProperties()

        End Sub

        Private Sub InitControls()

            cmdAddDEMToMap.Visible = ProjectManager.IsArcMap
            cmdAddAssocToMap.Visible = ProjectManager.IsArcMap
            cmdAddErrorToMap.Visible = ProjectManager.IsArcMap

            'General Tooltips
            'ttpTooltip.SetToolTip(btnCancel, My.Resources.ttpCancel)
            'ttpTooltip.SetToolTip(btnHelp, My.Resources.ttpHelp)

            ''DEM Survey Tooltips
            'ttpTooltip.SetToolTip(txtName, My.Resources.ttpBudgetFormBtnCalculate)

            'ttpTooltip.SetToolTip(txtName, My.Resources.ttpSurveyPropertiesDEMSurveyName)
            'ttpTooltip.SetToolTip(txtDate, My.Resources.ttpSurveyPropertiesDEMSurveyDate)
            ''ttpTooltip.SetToolTip(cboLayer, My.Resources.ttpDEMLayer)
            'ttpTooltip.SetToolTip(cboSingle, My.Resources.ttpSurveyPropertiesSingleCombo)
            ''ttpTooltip.SetToolTip(cboPolyMask, My.Resources.ttpPolyMask)
            'ttpTooltip.SetToolTip(cboIdentify, My.Resources.ttpSurveyPropertiesIdentifierField)
            'ttpTooltip.SetToolTip(cmdAddToMap, My.Resources.ttpSurveyPropertiesBrowseFile)
            'ttpTooltip.SetToolTip(btnBrowseMask, My.Resources.ttpSurveyPropertiesBrowseMask)
            'ttpTooltip.SetToolTip(txtRasterPath, My.Resources.ttpSurveyPropertiesTxtFolder)
            'ttpTooltip.SetToolTip(txtMask, My.Resources.ttpSurveyPropertiesTxtMask)
            'ttpTooltip.SetToolTip(rdoSingle, My.Resources.ttpSurveyPropertiesSingleMethod)
            'ttpTooltip.SetToolTip(rdoMulti, My.Resources.ttpSurveyPropertiesMultiMethod)

            ''Associated Surfaces Tooltips
            'ttpTooltip.SetToolTip(btnAddAssociatedSurface, My.Resources.ttpAddAS)
            'ttpTooltip.SetToolTip(btnSettingsAssociatedSurface, My.Resources.ttpSettingsAS)
            'ttpTooltip.SetToolTip(btnDeleteAssociatedSurface, My.Resources.ttpDeleteAS)
            'ttpTooltip.SetToolTip(btnAddToMap, My.Resources.ttpAddAStoMap)

            ''Error Calculation Tooltips
            'ttpTooltip.SetToolTip(btnAddError, "Associate an existing raster as an error surface")
            'ttpTooltip.SetToolTip(btnCalculateError, "Calculate a new error raster surface")
            'ttpTooltip.SetToolTip(btnErrorSettings, My.Resources.ttpSettingsError)
            'ttpTooltip.SetToolTip(btnErrorDelete, My.Resources.ttpDeleteError)

        End Sub

        Private Function ValidateForm() As Boolean

            txtName.Text = txtName.Text.Trim

            If Not ProjectManager.Project.IsDEMNameUnique(txtName.Text, DEM) Then
                MessageBox.Show(String.Format("Ther is already another DEM survey in this project with the name '{0}'. Each DEM Survey must have a unique name.", txtName.Text))
                txtName.Select()
                Return False
            End If

            If rdoSingle.Checked Then
                If String.IsNullOrEmpty(cboSingle.Text) Then
                    MsgBox("You must select a survey method type.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    cboSingle.Select()
                    Return False
                End If
            Else
                If String.IsNullOrEmpty(txtMask.Text) Then
                    MsgBox("You must select a survey mask feature class.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    btnBrowseMask.Select()
                    Return False
                End If

                If String.IsNullOrEmpty(cboIdentify.Text) Then
                    MsgBox("You must select a survey method identifier field.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    cboIdentify.Select()
                    Return False
                End If
            End If

            Return True

        End Function

        Private Sub txtMask_TextChanged(sender As Object, e As System.EventArgs) Handles txtMask.TextChanged

            cboIdentify.Items.Clear()
            If String.IsNullOrEmpty(txtMask.Text) Then
                Return
            End If

            Dim gMask As New GCDConsoleLib.Vector(DEM.MethodMask)
            cboIdentify.Items.AddRange(gMask.Fields.Values.Where(Function(s As GCDConsoleLib.VectorField) s.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)))
            If Not String.IsNullOrEmpty(DEM.MethodMaskField) AndAlso cboIdentify.Items.Contains(DEM.MethodMaskField) Then
                cboIdentify.Text = DEM.MethodMaskField
            End If

        End Sub

        Private Sub txtFolder_DoubleClick(sender As Object, e As System.EventArgs) Handles txtFolder.DoubleClick

            If Not String.IsNullOrEmpty(txtFolder.Text) Then
                If IO.Directory.Exists(txtFolder.Text) Then
                    Process.Start("explorer.exe", txtFolder.Text)
                End If
            End If

        End Sub

        Private Sub UpdateControls()

            cboSingle.Enabled = rdoSingle.Checked
            txtMask.Enabled = rdoMulti.Checked
            cboIdentify.Enabled = rdoMulti.Checked
            btnBrowseMask.Enabled = rdoMulti.Checked

        End Sub

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

            If ValidateForm() Then
                SaveDEMSurvey()
                Me.DialogResult = DialogResult.OK
            Else
                Me.DialogResult = DialogResult.None
            End If

        End Sub

        Private Sub SaveDEMSurvey()

            DEM.Name = txtName.Text

            If rdoSingle.Checked Then
                DEM.SurveyMethod = cboSingle.Text
                DEM.MethodMask = Nothing
                DEM.MethodMaskField = String.Empty
            Else
                DEM.SurveyMethod = String.Empty
                DEM.MethodMask = New IO.FileInfo(txtMask.Text)
                DEM.MethodMaskField = cboIdentify.Text
            End If

            ProjectManager.Project.Save()

        End Sub

        Private Sub btnBrowseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddDEMToMap.Click

            'TODO ArcMap manager in addin
            Throw New Exception("not implemented")

        End Sub

        Private Sub btnBrowseMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMask.Click

            rdoMulti.Checked = True

            Try
                Throw New NotImplementedException("browse for polygon mask. then set mask text. This should trigger field combo refresh.")

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub rdoSingle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            If rdoSingle.Checked Then
                Throw New NotImplementedException("Need to delete any existing polygon mask")
            End If
            UpdateControls()
        End Sub

        Private Sub btnHlp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHlp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/i-edit-dem-survey-properties")
        End Sub

        Private Sub LoadRasterProperties()

            Dim sRasterProperties As String = "-- GCD Raster Properties --"

            With DEM.Raster.Raster.Extent
                sRasterProperties &= vbNewLine & "Left: " & .Left.ToString("#,##0.#")
                sRasterProperties &= vbNewLine & "Top: " & .Top.ToString("#,##0.#")
                sRasterProperties &= vbNewLine & "Right: " & .Right.ToString("#,##0.#")
                sRasterProperties &= vbNewLine & "Bottom: " & .Bottom.ToString("#,##0.#")
                sRasterProperties &= vbNewLine
                sRasterProperties &= vbNewLine & "Cell size: " & .CellWidth
                sRasterProperties &= vbNewLine
                sRasterProperties &= vbNewLine & "Width: " & (.Right - .Left).ToString("#,##0.#")
                sRasterProperties &= vbNewLine & "Height: " & (.Top - .Bottom).ToString("#,##0.#")
                sRasterProperties &= vbNewLine
                sRasterProperties &= vbNewLine & "Rows: " & .rows.ToString("#,##0")
                sRasterProperties &= vbNewLine & "Columns: " & .cols.ToString("#,##0")
            End With

            sRasterProperties &= vbNewLine & vbNewLine
            'sRasterProperties &= "-- Original Raster Properties --"
            'sRasterProperties &= vbNewLine & "Left: " & demRow.OriginalExtentLeft.ToString
            'sRasterProperties &= vbNewLine & "Top: " & demRow.OriginalExtentTop.ToString
            'sRasterProperties &= vbNewLine & "Right: " & demRow.OriginalExtentRight.ToString
            'sRasterProperties &= vbNewLine & "Bottom: " & demRow.OriginalExtentBottom.ToString
            'sRasterProperties &= vbNewLine
            'sRasterProperties &= vbNewLine & "Cell size: " & demRow.OriginalCellSize.ToString
            'sRasterProperties &= vbNewLine
            'sRasterProperties &= vbNewLine & "Path: " & demRow.OriginalSource
            'sRasterProperties &= vbNewLine & "Computer: " & demRow.OriginalComputer
            'sRasterProperties &= vbNewLine

            Try
                ' File Size
                sRasterProperties &= vbNewLine & "Raster file size: " & naru.os.File.GetFormattedFileSize(DEM.Raster.RasterPath)
            Catch ex As Exception
                ' Do nothing. 
            End Try

            txtProperties.Text = sRasterProperties

        End Sub

#End Region

#Region "Associated Surface Events"

        Private Sub ViewAssociatedSurface(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnSettingsAssociatedSurface.Click, grdAssocSurface.CellContentDoubleClick

            ' Save DEM so that latest survey method is stored ready for associated use by surface 
            SaveDEMSurvey()

            Dim assoc As AssocSurface = DirectCast(grdAssocSurface.SelectedRows().Item(0).DataBoundItem, AssocSurface)
            Dim frm As New frmAssocSurfaceProperties(DEM, assoc)
            If frm.ShowDialog() = DialogResult.OK Then
                Throw New NotImplementedException("Need to refresh data grid view")
            End If

        End Sub

        Private Sub btnAddToMap_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddAssocToMap.Click

            Throw New NotImplementedException("add selected associated surface to map")

        End Sub

        Private Sub btnAddAssociatedSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAssociatedSurface.Click

            If ValidateForm() Then

                ' Save DEM so that latest survey method is stored ready for associated use by surface 
                SaveDEMSurvey()

                Dim SurfaceForm As New frmAssocSurfaceProperties(DEM, Nothing)
                If SurfaceForm.ShowDialog() = DialogResult.OK Then
                    Throw New NotImplementedException("Need to refresh data grid view")
                End If
            End If

        End Sub

        Private Sub Associated_CellContentEnter(ByVal sender As System.Object, ByVal e As DataGridViewCellEventArgs) Handles grdAssocSurface.CellEnter
            cmdAddAssocToMap.Enabled = True
            btnDeleteAssociatedSurface.Enabled = True
            btnSettingsAssociatedSurface.Enabled = True
        End Sub

        Private Sub Associated_CellContentLeave(ByVal sender As System.Object, ByVal e As DataGridViewCellEventArgs) Handles grdAssocSurface.CellLeave
            cmdAddAssocToMap.Enabled = False
            btnDeleteAssociatedSurface.Enabled = False
            btnSettingsAssociatedSurface.Enabled = False
        End Sub

        Private Sub btnDeleteAssociatedSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAssociatedSurface.Click

            If MessageBox.Show("Are you sure you want to remove the selected associated surface from the GCD Project?" &
                      " This will also delete the raster associated with this surface.", "Deleted associated surface?",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then

                DEM.DeleteAssociatedSurface(DirectCast(grdAssocSurface.SelectedRows().Item(0).DataBoundItem, AssocSurface))
            End If

        End Sub

#End Region

#Region "Error Calculation Events"

        Private Sub btn_AddErrorSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddError.Click

            If Not ValidateForm() Then
                Exit Sub
            End If

            Try
                ' save the DEM survey information
                SaveDEMSurvey()
                SpecifyErrorSurface(DEM)
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Public Shared Function SpecifyErrorSurface(dem As DEMSurvey) As ErrorSurface

            Throw New NotImplementedException("specify error surface")

            Dim errSurface As ErrorSurface = Nothing
            'Dim gDEM As New GCDConsoleLib.Raster(ProjectManager.GetAbsolutePath(rDEM.Source))

            'Dim frm As New frmImportRaster(dem.Raster.Raster, rDEM, frmImportRaster.ImportRasterPurposes.ErrorCalculation, "Error Surface")

            'If frm.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            '    Dim gRaster As GCDConsoleLib.Raster = Nothing
            '    Try
            '        gRaster = frm.ProcessRaster
            '    Catch ex As Exception
            '        Try
            '            IO.Directory.Delete(IO.Path.GetDirectoryName(frm.txtRasterPath.Text))
            '        Catch ex2 As Exception
            '            ' do nothing
            '        End Try

            '        naru.error.ExceptionUI.HandleException(ex, "An error occurred attempting to import the error surface into the GCD project. No information has been saved to the GCD project file but you should check the GCD project folder to determine if any remains of the raster remain.")
            '    End Try

            '    If TypeOf gRaster Is GCDConsoleLib.Raster Then
            '        Try
            '            Dim errSurface As ErrorSurface = New ErrorSurface(frm.txtName.Text, gRaster.GISFileInfo, dem)
            '            dem.ErrorSurfaces.Add(errSurface.Name, errSurface)
            '            ProjectManager.Project.Save()

            '            'If My.Settings.AddOutputLayersToMap Then
            '            'TODO not implemented
            '            Throw New Exception("Not implemented")
            '            'GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            '            'End If

            '        Catch ex As Exception
            '            Dim bRasterExists As Boolean = True
            '            Try
            '                GCDConsoleLib.Raster.Delete(New IO.FileInfo(frm.txtRasterPath.Text))
            '                bRasterExists = False
            '            Catch ex2 As Exception
            '                bRasterExists = True
            '            End Try

            '            Dim sMsg As String = "Failed to save the error surface information to the GCD project file."
            '            If bRasterExists Then
            '                sMsg &= " The GCD project error surface raster still exists And should be deleted by hand."
            '            Else
            '                sMsg &= "The GCD project error surface raster was deleted."
            '            End If

            '            naru.error.ExceptionUI.HandleException(ex, sMsg)
            '        End Try
            '    End If
            'End If

            Return errSurface

        End Function

        Private Sub btnCalculateError_Click(sender As System.Object, e As System.EventArgs) Handles btnCalculateError.Click
            '
            ' Only open the Error calculation form if the survey properties save successfully.
            '
            Throw New NotImplementedException("todo")
            'If ValidateForm() Then
            '    SaveDEMSurvey()
            '    Dim dr As DataRowView = DEMSurveyBindingSource.Current
            '    Dim frm As New ErrorCalculation.frmErrorCalculation(DirectCast(dr.Row, ProjectDS.DEMSurveyRow))
            '    frm.ShowDialog()
            'End If

        End Sub

        Private Sub Error_CellContentEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdErrorSurfaces.CellEnter
            cmdAddErrorToMap.Enabled = True
            btnErrorDelete.Enabled = True
            btnErrorSettings.Enabled = True
        End Sub

        Private Sub Error_CellContentLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdErrorSurfaces.CellLeave
            cmdAddErrorToMap.Enabled = False
            btnErrorDelete.Enabled = False
            btnErrorSettings.Enabled = False
        End Sub

        Private Sub Error_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdErrorSurfaces.CellContentDoubleClick
            ViewErrorSettings()
        End Sub

        Private Sub btnErrorSurfaceSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErrorSettings.Click
            ViewErrorSettings()
        End Sub

        Private Sub btnDeleteErrorSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErrorDelete.Click

            If MessageBox.Show("Are you sure you want to delete the selected error surface from the GCD project?" &
                    " This will also delete the raster associated with this error surface.",
                    "Delete Error Surface", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then

                DEM.DeleteErrorSurface(DirectCast(grdErrorSurfaces.SelectedRows().Item(0).DataBoundItem, ErrorSurface))

            End If


        End Sub

        Private Sub btnAddErrorToMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddErrorToMap.Click

            Throw New NotImplementedException("add error surface to map")

        End Sub

        Private Sub ViewErrorSettings()

            Throw New NotImplementedException("edit error surface")

            'Dim CurrentRow As DataRowView = Me.ErrorTableBindingSource.Current
            'If Not CurrentRow Is Nothing AndAlso TypeOf CurrentRow.Row Is ProjectDS.ErrorSurfaceRow Then
            '    Dim frm As New ErrorCalculation.frmErrorCalculation(DirectCast(CurrentRow.Row, ProjectDS.ErrorSurfaceRow))
            '    frm.ShowDialog()
            'End If

        End Sub

#End Region

        Private Sub cmdDateTime_Click(sender As System.Object, e As System.EventArgs) Handles cmdDateTime.Click

            Dim frm As New frmSurveyDateTime(DEM.SurveyDate)
            If frm.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                DEM.SurveyDate = frm.SurveyDateTime
                lblDatetime.Text = DEM.SurveyDate.ToString()
            End If
        End Sub
    End Class

End Namespace