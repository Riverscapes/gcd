Imports System.Windows.Forms

Namespace Options

    Public Class frmOptions

        Private m_sArcMapDisplayUnits As String

#Region "Events"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sArcMapDisplayUnits">The current linear display units of the current ArcMap map data frame</param>
        Public Sub New(Optional sArcMapDisplayUnits = "")
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            m_sArcMapDisplayUnits = sArcMapDisplayUnits
        End Sub

        Private Sub OptionsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            ' New method to upgrade the GCD user configuration settings
            '
            GCDCore.Properties.Settings.Default.Upgrade()

            'TOOLTIPS
            'Workspace Tab
            'ttpTooltip.SetToolTip(rdoDefault, My.Resources.ttpOptionsFormRdoDefault)
            'ttpTooltip.SetToolTip(rdoUserDefined, My.Resources.ttpOptionsFormRdoUserDefined)
            'ttpTooltip.SetToolTip(btnClearWorkspace, My.Resources.ttpOptionsFormClearWorkspace)
            'ttpTooltip.SetToolTip(btnBrowseWorkspace, My.Resources.ttpOptionsFormBrowseWorkspace)
            'ttpTooltip.SetToolTip(chkClearWorkspaceOnStartup, My.Resources.ttpOptionsFormClearOnStart)
            'ttpTooltip.SetToolTip(chkAddInputLayersToMap, My.Resources.ttpOptionsFormAddSurveyTypeAddLayersToMap)
            'ttpTooltip.SetToolTip(chkAddInputLayersToMap, My.Resources.ttpOptionsFormAddSurveyTypeAddInLayersToMap)
            'ttpTooltip.SetToolTip(cboFormat, My.Resources.ttpOptionsFormCboFormat)

            ''Survey Types Tab
            'ttpTooltip.SetToolTip(btnAddSurveyType, My.Resources.ttpOptionsFormAddSurveyType)
            'ttpTooltip.SetToolTip(btnDeleteSurveyType, My.Resources.ttpOptionsFormDeleteSurveyType)
            'ttpTooltip.SetToolTip(btnSettingsSurveyType, My.Resources.ttpOptionsFormSettingSurveyType)
            'ttpTooltip.SetToolTip(txtSurveyType, My.Resources.ttpOptionsFormSurveyTypeText)
            ''Symbology Tab

            ''Graphs Tab

            ''Precision Tab
            ''ttpTooltip.SetToolTip(numPrecision, My.Resources.ttpOptionsFormNumPrecision)


            'ttpTooltip.SetToolTip(nbrError, My.Resources.ttpOptionsFormSurveyTypeError)

            txtWorkspace.Text = GCDCore.WorkspaceManager.WorkspacePath
            chkTempWorkspaceWarning.Checked = GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning

            chkClearWorkspaceOnStartup.Checked = GCDCore.Properties.Settings.Default.ClearWorkspaceOnStartup
            'chkAddInputLayersToMap.Checked = GCDCore.Properties.Settings.Default.AddInputLayersToMap
            'chkAddOutputLayersToMap.Checked = GCDCore.Properties.Settings.Default.AddOutputLayersToMap
            chkWarnAboutLongPaths.Checked = GCDCore.Properties.Settings.Default.WarnAboutLongPaths
            chkBoxValidateProjectOnLoad.Checked = GCDCore.Properties.Settings.Default.ValidateProjectOnLoad
            chkAutoLoadEtalFIS.Checked = GCDCore.Properties.Settings.Default.AutoLoadFISLibrary
            chkComparativeSymbology.Checked = GCDCore.Properties.Settings.Default.ApplyComparativeSymbology
            chkAutoApplyTransparency.Checked = GCDCore.Properties.Settings.Default.ApplyTransparencySymbology

            If chkComparativeSymbology.Checked Then

                'Settings based on chkComparativeSymbology (always turned off when chkComparativeSymbology is turned off)
                chk3DPointQualityComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbology3dPointQuality
                chkInterpolationErrorComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyInterpolationError
                chkPointDensityComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyPointDensity
                chkDoDComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyDoD
                chkDoDComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyFISError
                grbComparitiveLayers.Enabled = True

            ElseIf chkComparativeSymbology.Checked = False Then

                grbComparitiveLayers.Enabled = False

            End If

            If chkAutoApplyTransparency.Checked Then

                chkAssociatedSurfacesTransparency.Checked = GCDCore.Properties.Settings.Default.TransparencyAssociatedLayers
                chkAnalysesTransparency.Checked = GCDCore.Properties.Settings.Default.TransparencyAnalysesLayers
                chkErrorSurfacesTransparency.Checked = GCDCore.Properties.Settings.Default.TransparencyErrorLayers
                nudTransparency.Value = GCDCore.Properties.Settings.Default.AutoTransparencyValue
                grbTransparencyLayer.Enabled = True
                nudTransparency.Enabled = True

            ElseIf chkAutoApplyTransparency.Checked = False Then

                grbTransparencyLayer.Enabled = False
                nudTransparency.Enabled = False
                nudTransparency.Value = -1

            End If

            cboFormat.Text = GCDCore.Properties.Settings.Default.DefaultRasterFormat

            'chart setting for exporting charts
            numChartWidth.Value = GCDCore.Properties.Settings.Default.ChartWidth
            numChartHeight.Value = GCDCore.Properties.Settings.Default.ChartHeight

            'settings for accuracy, used for concurrency and orthogonality
            'numPrecision.Value = GCDCore.Properties.Settings.Default.Precision

            Label6.Text = m_sArcMapDisplayUnits

            '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' New Automatic Pyramid building features
            Try
                lstPyramids.Items.Clear()
                For Each eType As GCDCore.RasterPyramidManager.PyramidRasterTypes In [Enum].GetValues(GetType(GCDCore.RasterPyramidManager.PyramidRasterTypes))
                    Dim bPyramids As Boolean = ProjectManagerUI.PyramidManager.AutomaticallyBuildPyramids(eType)
                    lstPyramids.Items.Add(New GCDCore.PyramidRasterTypeDisplay(eType), bPyramids)
                Next

            Catch ex As Exception
                lstPyramids.Items.Clear()
            End Try
        End Sub

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click

            If GCDCore.WorkspaceManager.ValidateWorkspace(txtWorkspace.Text) Then
                GCDCore.WorkspaceManager.SetWorkspacePath(txtWorkspace.Text)
                GCDCore.Properties.Settings.Default.TempWorkspace = txtWorkspace.Text
            Else
                Me.DialogResult = System.Windows.Forms.DialogResult.None
                Exit Sub
            End If

            GCDCore.Properties.Settings.Default.ClearWorkspaceOnStartup = chkClearWorkspaceOnStartup.Checked
            GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning = chkTempWorkspaceWarning.Checked

            'GCDCore.Properties.Settings.Default.AddInputLayersToMap = chkAddInputLayersToMap.Checked
            'GCDCore.Properties.Settings.Default.AddOutputLayersToMap = chkAddOutputLayersToMap.Checked
            GCDCore.Properties.Settings.Default.WarnAboutLongPaths = chkWarnAboutLongPaths.Checked
            GCDCore.Properties.Settings.Default.ValidateProjectOnLoad = chkBoxValidateProjectOnLoad.Checked
            GCDCore.Properties.Settings.Default.AutoLoadFISLibrary = chkAutoLoadEtalFIS.Checked
            GCDCore.Properties.Settings.Default.ApplyComparativeSymbology = chkComparativeSymbology.Checked
            GCDCore.Properties.Settings.Default.ApplyTransparencySymbology = chkAutoApplyTransparency.Checked

            'Settings based on chkComparativeSymbology (always turned off when chkComparativeSymbology is turned off)
            GCDCore.Properties.Settings.Default.ComparativeSymbology3dPointQuality = chk3DPointQualityComparative.Checked
            GCDCore.Properties.Settings.Default.ComparativeSymbologyInterpolationError = chkInterpolationErrorComparative.Checked
            GCDCore.Properties.Settings.Default.ComparativeSymbologyPointDensity = chkPointDensityComparative.Checked
            GCDCore.Properties.Settings.Default.ComparativeSymbologyDoD = chkDoDComparative.Checked
            GCDCore.Properties.Settings.Default.ComparativeSymbologyFISError = chkFISErrorComparative.Checked

            'Settings based on chkAutoApplyTransparency (always turned off when chkAutoApplyTransparency is turned off)
            GCDCore.Properties.Settings.Default.TransparencyAssociatedLayers = chkAssociatedSurfacesTransparency.Checked
            GCDCore.Properties.Settings.Default.TransparencyAnalysesLayers = chkAnalysesTransparency.Checked
            GCDCore.Properties.Settings.Default.TransparencyErrorLayers = chkErrorSurfacesTransparency.Checked
            GCDCore.Properties.Settings.Default.AutoTransparencyValue = nudTransparency.Value

            GCDCore.Properties.Settings.Default.DefaultRasterFormat = cboFormat.Text

            'chart setting for exporting charts
            GCDCore.Properties.Settings.Default.ChartWidth = numChartWidth.Value
            GCDCore.Properties.Settings.Default.ChartHeight = numChartHeight.Value

            For i As Integer = 0 To lstPyramids.Items.Count - 1
                Dim lItem As GCDCore.PyramidRasterTypeDisplay = lstPyramids.Items(i)
                ProjectManagerUI.PyramidManager.SetAutomaticPyramidsForRasterType(lItem.RasterType, lstPyramids.CheckedIndices.Contains(i))
            Next

            GCDCore.Properties.Settings.Default.AutomaticPyramids = ProjectManagerUI.PyramidManager.GetPyramidSettingString

            'settings for accuracy, used for concurrency and orthogonality
            'GCDCore.Properties.Settings.Default.Precision = numPrecision.Value

            GCDCore.Properties.Settings.Default.Save()
            '
            ' PGB 2 Jun 2011 - Now need to update the workspace folder as it is managed by the Extension
            '
            ' TODO: is this necessary?
            Throw New Exception("not implemented")
            'Dim gcd As GCDExtension = GCDExtension.GetGCDExtension(My.ThisApplication)
            'If TypeOf gcd Is GCDExtension Then
            '    'gcd.SetWorkspacePath()
            'End If

            Me.Close()
        End Sub

        Private Sub btnBrowseChangeWorkspace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
            Dim pFolderBrowserDialog As New FolderBrowserDialog
            pFolderBrowserDialog.Description = String.Format("Select {0} Temporary Workspace", GCDCore.Properties.Resources.ApplicationNameShort)
            If pFolderBrowserDialog.ShowDialog = DialogResult.OK Then
                If GCDCore.WorkspaceManager.ValidateWorkspace(pFolderBrowserDialog.SelectedPath) Then
                    txtWorkspace.Text = pFolderBrowserDialog.SelectedPath

                    ' User may have updated whether they want temp workspace warnings
                    chkTempWorkspaceWarning.Checked = GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning
                End If
            End If
        End Sub

        Private Sub btnClearWorkspace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearWorkspace.Click

            If Not txtWorkspace.Text = GCDCore.WorkspaceManager.WorkspacePath Then
                MsgBox("Please save your settings before clearing workspace", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Exit Sub
            End If
            If MsgBox("Are you sure you want to clear the workspace?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong) = MsgBoxResult.No Then
                Exit Sub
            End If

            Try
                Cursor.Current = Cursors.WaitCursor
                GCDCore.WorkspaceManager.ClearWorkspace()
                Cursor.Current = Cursors.Default
                MsgBox("Workspace cleared successfully.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            Catch ex As Exception
                'GISCode.ExceptionHandling.HandleException(ex, "Could not clear workspace")
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End Sub

        Private Sub btnExploreWorkspace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExploreWorkspace.Click

            If String.IsNullOrEmpty(txtWorkspace.Text) Then
                MessageBox.Show("You must define a temporary workspace before you can open it in Windows Explorer.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If Not System.IO.Directory.Exists(txtWorkspace.Text) Then
                    MessageBox.Show("The temporary workspace is not a valid folder. Browse and set the temporary workspace to an existing folder on your computer. This should preferably be a folder without spaces or periods in the path.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            ' Should only get to here if the path is valid.
            Process.Start("explorer.exe", txtWorkspace.Text)

        End Sub

        Private Sub cmdDefault_Click(sender As System.Object, e As System.EventArgs) Handles cmdDefault.Click

            Dim sNewWorkspacePath As String = GCDCore.WorkspaceManager.GetDefaultWorkspace(GCDCore.Properties.Resources.ApplicationNameShort)
            If GCDCore.WorkspaceManager.ValidateWorkspace(sNewWorkspacePath) Then
                txtWorkspace.Text = sNewWorkspacePath

                ' User may have updated whether they want temp workspace warnings
                chkTempWorkspaceWarning.Checked = GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning
            End If

        End Sub

        Private Sub txtWorkspace_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtWorkspace.TextChanged
            btnBrowse.Enabled = (Not String.IsNullOrEmpty(txtWorkspace.Text)) AndAlso System.IO.Directory.Exists(txtWorkspace.Text)
            btnClearWorkspace.Enabled = btnBrowse.Enabled
        End Sub

#End Region

        Private Sub btnDeleteSurveyType_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteSurveyType.Click

            Throw New NotImplementedException

            'Dim CurrentRow As DataRowView = SurveyTypesBindingSource.Current
            'If TypeOf CurrentRow Is DataRowView Then
            '    If TypeOf CurrentRow.Row Is SurveyTypes.SurveyTypesRow Then
            '        Dim surveytype As SurveyTypes.SurveyTypesRow = CurrentRow.Row
            '        Dim sMessage As String = "Are you sure you want to remove the survey type '" & surveytype.Name & "' and its corresponding error value?"
            '        Dim response As MsgBoxResult = MsgBox(sMessage, MsgBoxStyle.YesNo Or MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong)
            '        If response = MsgBoxResult.Yes Then
            '            If Not CurrentRow Is Nothing Then
            '                'Delete the selected item from the dataset and write this new information to the XML file at the specified location
            '                SurveyTypesBindingSource.RemoveCurrent()
            '                Core.GCDProject.ProjectManagerBase.saveSurveyTypes()
            '            End If
            '        End If
            '    End If
            'End If

        End Sub

        Private Sub btnAddSurveyType_Click(sender As System.Object, e As System.EventArgs) Handles btnAddSurveyType.Click

            Try
                If txtSurveyType.TextLength < 1 Then
                    MsgBox("Please enter a name for the survey type.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                    Exit Sub
                End If

                If nbrError.Value < 0.01 OrElse nbrError.Value > 100 Then
                    MsgBox("Please enter a default error value in meters to be associated with the survey type. The value must be greater than 0.01 and less than 100.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                    Exit Sub
                End If

                SurveyTypesBindingSource.AddNew()

                Dim CurrentRow As DataRowView = SurveyTypesBindingSource.Current

                CurrentRow("Name") = txtSurveyType.Text
                CurrentRow("Error") = nbrError.Text

                Throw New NotImplementedException
                'SurveyTypesBindingSource.EndEdit()
                'Core.GCDProject.ProjectManagerBase.saveSurveyTypes()

                txtSurveyType.Text = ""
                'nbrError.Value = nbrError.Minimum

            Catch ex As Exception
                If ex.Message.ToString.ToLower.Contains("name") Then
                    MsgBox("Please select a unique name for the survey type being added.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                ElseIf ex.Message.ToString.ToLower.Contains("error") Then
                    MsgBox("The error value is invalid.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                Else
                    MsgBox("An error occured while trying to save the information. " & vbNewLine & ex.Message)
                End If

                SurveyTypesBindingSource.CancelEdit()

            End Try

        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/customize-menu/options")
        End Sub

        Private Sub chkComparativeSymbology_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkComparativeSymbology.CheckedChanged

            If chkComparativeSymbology.Checked = False Then

                chk3DPointQualityComparative.Checked = False
                chkInterpolationErrorComparative.Checked = False
                chkPointDensityComparative.Checked = False
                chkDoDComparative.Checked = False
                chkFISErrorComparative.Checked = False
                grbComparitiveLayers.Enabled = False

            ElseIf chkComparativeSymbology.Checked Then

                grbComparitiveLayers.Enabled = True

            End If

        End Sub

        Private Sub chkAutoApplyTransparency_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAutoApplyTransparency.CheckedChanged

            If chkAutoApplyTransparency.Checked = False Then

                chkAssociatedSurfacesTransparency.Checked = False
                chkAnalysesTransparency.Checked = False
                chkErrorSurfacesTransparency.Checked = False
                grbTransparencyLayer.Enabled = False
                nudTransparency.Value = -1
                nudTransparency.Enabled = False

            ElseIf chkAutoApplyTransparency.Checked Then

                grbTransparencyLayer.Enabled = True
                nudTransparency.Enabled = True
                nudTransparency.Value = 40

            End If

        End Sub

        Private Sub lnkPyramidsHelp_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkPyramidsHelp.LinkClicked
            Process.Start("http://blogs.esri.com/esri/arcgis/2012/11/14/should-i-build-pyramids-or-overviews")
        End Sub

    End Class

End Namespace