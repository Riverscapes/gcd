using GCDCore.Project;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDUserInterface.Options
{

	public partial class frmOptions
	{


		private string m_sArcMapDisplayUnits;
		#region "Events"

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sArcMapDisplayUnits">The current linear display units of the current ArcMap map data frame</param>
		public frmOptions(sArcMapDisplayUnits = "")
		{
			Load += OptionsForm_Load;
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			m_sArcMapDisplayUnits = sArcMapDisplayUnits;
		}

		private void OptionsForm_Load(System.Object sender, System.EventArgs e)
		{
			//
			// New method to upgrade the GCD user configuration settings
			//
			GCDCore.Properties.Settings.Default.Upgrade();

			//TOOLTIPS
			//Workspace Tab
			//ttpTooltip.SetToolTip(rdoDefault, My.Resources.ttpOptionsFormRdoDefault)
			//ttpTooltip.SetToolTip(rdoUserDefined, My.Resources.ttpOptionsFormRdoUserDefined)
			//ttpTooltip.SetToolTip(btnClearWorkspace, My.Resources.ttpOptionsFormClearWorkspace)
			//ttpTooltip.SetToolTip(btnBrowseWorkspace, My.Resources.ttpOptionsFormBrowseWorkspace)
			//ttpTooltip.SetToolTip(chkClearWorkspaceOnStartup, My.Resources.ttpOptionsFormClearOnStart)
			//ttpTooltip.SetToolTip(chkAddInputLayersToMap, My.Resources.ttpOptionsFormAddSurveyTypeAddLayersToMap)
			//ttpTooltip.SetToolTip(chkAddInputLayersToMap, My.Resources.ttpOptionsFormAddSurveyTypeAddInLayersToMap)
			//ttpTooltip.SetToolTip(cboFormat, My.Resources.ttpOptionsFormCboFormat)

			//'Survey Types Tab
			//ttpTooltip.SetToolTip(btnAddSurveyType, My.Resources.ttpOptionsFormAddSurveyType)
			//ttpTooltip.SetToolTip(btnDeleteSurveyType, My.Resources.ttpOptionsFormDeleteSurveyType)
			//ttpTooltip.SetToolTip(btnSettingsSurveyType, My.Resources.ttpOptionsFormSettingSurveyType)
			//ttpTooltip.SetToolTip(txtSurveyType, My.Resources.ttpOptionsFormSurveyTypeText)
			//'Symbology Tab

			//'Graphs Tab

			//'Precision Tab
			//'ttpTooltip.SetToolTip(numPrecision, My.Resources.ttpOptionsFormNumPrecision)


			//ttpTooltip.SetToolTip(nbrError, My.Resources.ttpOptionsFormSurveyTypeError)

			txtWorkspace.Text = GCDCore.WorkspaceManager.WorkspacePath;
			chkTempWorkspaceWarning.Checked = GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning;

			chkClearWorkspaceOnStartup.Checked = GCDCore.Properties.Settings.Default.ClearWorkspaceOnStartup;
			//chkAddInputLayersToMap.Checked = GCDCore.Properties.Settings.Default.AddInputLayersToMap
			//chkAddOutputLayersToMap.Checked = GCDCore.Properties.Settings.Default.AddOutputLayersToMap
			chkWarnAboutLongPaths.Checked = GCDCore.Properties.Settings.Default.WarnAboutLongPaths;
			chkBoxValidateProjectOnLoad.Checked = GCDCore.Properties.Settings.Default.ValidateProjectOnLoad;
			chkAutoLoadEtalFIS.Checked = GCDCore.Properties.Settings.Default.AutoLoadFISLibrary;
			chkComparativeSymbology.Checked = GCDCore.Properties.Settings.Default.ApplyComparativeSymbology;
			chkAutoApplyTransparency.Checked = GCDCore.Properties.Settings.Default.ApplyTransparencySymbology;


			if (chkComparativeSymbology.Checked) {
				//Settings based on chkComparativeSymbology (always turned off when chkComparativeSymbology is turned off)
				chk3DPointQualityComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbology3dPointQuality;
				chkInterpolationErrorComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyInterpolationError;
				chkPointDensityComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyPointDensity;
				chkDoDComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyDoD;
				chkDoDComparative.Checked = GCDCore.Properties.Settings.Default.ComparativeSymbologyFISError;
				grbComparitiveLayers.Enabled = true;


			} else if (chkComparativeSymbology.Checked == false) {
				grbComparitiveLayers.Enabled = false;

			}


			if (chkAutoApplyTransparency.Checked) {
				chkAssociatedSurfacesTransparency.Checked = GCDCore.Properties.Settings.Default.TransparencyAssociatedLayers;
				chkAnalysesTransparency.Checked = GCDCore.Properties.Settings.Default.TransparencyAnalysesLayers;
				chkErrorSurfacesTransparency.Checked = GCDCore.Properties.Settings.Default.TransparencyErrorLayers;
				nudTransparency.Value = GCDCore.Properties.Settings.Default.AutoTransparencyValue;
				grbTransparencyLayer.Enabled = true;
				nudTransparency.Enabled = true;


			} else if (chkAutoApplyTransparency.Checked == false) {
				grbTransparencyLayer.Enabled = false;
				nudTransparency.Enabled = false;
				nudTransparency.Value = -1;

			}

			cboFormat.Text = GCDCore.Properties.Settings.Default.DefaultRasterFormat;

			//chart setting for exporting charts
			numChartWidth.Value = GCDCore.Properties.Settings.Default.ChartWidth;
			numChartHeight.Value = GCDCore.Properties.Settings.Default.ChartHeight;

			//settings for accuracy, used for concurrency and orthogonality
			//numPrecision.Value = GCDCore.Properties.Settings.Default.Precision

			Label6.Text = m_sArcMapDisplayUnits;

			//
			///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			// New Automatic Pyramid building features
			try {
				lstPyramids.Items.Clear();
				foreach (GCDCore.RasterPyramidManager.PyramidRasterTypes eType in Enum.GetValues(typeof(GCDCore.RasterPyramidManager.PyramidRasterTypes))) {
					bool bPyramids = ProjectManager.PyramidManager.AutomaticallyBuildPyramids(eType);
					lstPyramids.Items.Add(new GCDCore.PyramidRasterTypeDisplay(eType), bPyramids);
				}

			} catch (Exception ex) {
				lstPyramids.Items.Clear();
			}
		}


		private void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			if (GCDCore.WorkspaceManager.ValidateWorkspace(txtWorkspace.Text)) {
				GCDCore.WorkspaceManager.SetWorkspacePath(txtWorkspace.Text);
				GCDCore.Properties.Settings.Default.TempWorkspace = txtWorkspace.Text;
			} else {
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			GCDCore.Properties.Settings.Default.ClearWorkspaceOnStartup = chkClearWorkspaceOnStartup.Checked;
			GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning = chkTempWorkspaceWarning.Checked;

			//GCDCore.Properties.Settings.Default.AddInputLayersToMap = chkAddInputLayersToMap.Checked
			//GCDCore.Properties.Settings.Default.AddOutputLayersToMap = chkAddOutputLayersToMap.Checked
			GCDCore.Properties.Settings.Default.WarnAboutLongPaths = chkWarnAboutLongPaths.Checked;
			GCDCore.Properties.Settings.Default.ValidateProjectOnLoad = chkBoxValidateProjectOnLoad.Checked;
			GCDCore.Properties.Settings.Default.AutoLoadFISLibrary = chkAutoLoadEtalFIS.Checked;
			GCDCore.Properties.Settings.Default.ApplyComparativeSymbology = chkComparativeSymbology.Checked;
			GCDCore.Properties.Settings.Default.ApplyTransparencySymbology = chkAutoApplyTransparency.Checked;

			//Settings based on chkComparativeSymbology (always turned off when chkComparativeSymbology is turned off)
			GCDCore.Properties.Settings.Default.ComparativeSymbology3dPointQuality = chk3DPointQualityComparative.Checked;
			GCDCore.Properties.Settings.Default.ComparativeSymbologyInterpolationError = chkInterpolationErrorComparative.Checked;
			GCDCore.Properties.Settings.Default.ComparativeSymbologyPointDensity = chkPointDensityComparative.Checked;
			GCDCore.Properties.Settings.Default.ComparativeSymbologyDoD = chkDoDComparative.Checked;
			GCDCore.Properties.Settings.Default.ComparativeSymbologyFISError = chkFISErrorComparative.Checked;

			//Settings based on chkAutoApplyTransparency (always turned off when chkAutoApplyTransparency is turned off)
			GCDCore.Properties.Settings.Default.TransparencyAssociatedLayers = chkAssociatedSurfacesTransparency.Checked;
			GCDCore.Properties.Settings.Default.TransparencyAnalysesLayers = chkAnalysesTransparency.Checked;
			GCDCore.Properties.Settings.Default.TransparencyErrorLayers = chkErrorSurfacesTransparency.Checked;
			GCDCore.Properties.Settings.Default.AutoTransparencyValue = nudTransparency.Value;

			GCDCore.Properties.Settings.Default.DefaultRasterFormat = cboFormat.Text;

			//chart setting for exporting charts
			GCDCore.Properties.Settings.Default.ChartWidth = numChartWidth.Value;
			GCDCore.Properties.Settings.Default.ChartHeight = numChartHeight.Value;

			for (int i = 0; i <= lstPyramids.Items.Count - 1; i++) {
				GCDCore.PyramidRasterTypeDisplay lItem = lstPyramids.Items[i];
				ProjectManager.PyramidManager.SetAutomaticPyramidsForRasterType(lItem.RasterType, lstPyramids.CheckedIndices.Contains(i));
			}

			GCDCore.Properties.Settings.Default.AutomaticPyramids = ProjectManager.PyramidManager.GetPyramidSettingString;

			//settings for accuracy, used for concurrency and orthogonality
			//GCDCore.Properties.Settings.Default.Precision = numPrecision.Value

			GCDCore.Properties.Settings.Default.Save();
			//
			// PGB 2 Jun 2011 - Now need to update the workspace folder as it is managed by the Extension
			//
			// TODO: is this necessary?
			throw new Exception("not implemented");
			//Dim gcd As GCDExtension = GCDExtension.GetGCDExtension(My.ThisApplication)
			//If TypeOf gcd Is GCDExtension Then
			//    'gcd.SetWorkspacePath()
			//End If

			this.Close();
		}

		private void btnBrowseChangeWorkspace_Click(System.Object sender, System.EventArgs e)
		{
			FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
			pFolderBrowserDialog.Description = string.Format("Select {0} Temporary Workspace", GCDCore.Properties.Resources.ApplicationNameShort);
			if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK) {
				if (GCDCore.WorkspaceManager.ValidateWorkspace(pFolderBrowserDialog.SelectedPath)) {
					txtWorkspace.Text = pFolderBrowserDialog.SelectedPath;

					// User may have updated whether they want temp workspace warnings
					chkTempWorkspaceWarning.Checked = GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning;
				}
			}
		}


		private void btnClearWorkspace_Click(System.Object sender, System.EventArgs e)
		{
			if (!(txtWorkspace.Text == GCDCore.WorkspaceManager.WorkspacePath)) {
				Interaction.MsgBox("Please save your settings before clearing workspace", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
				return;
			}
			if (Interaction.MsgBox("Are you sure you want to clear the workspace?", MsgBoxStyle.YesNo | MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong) == MsgBoxResult.No) {
				return;
			}

			try {
				Cursor.Current = Cursors.WaitCursor;
				GCDCore.WorkspaceManager.ClearWorkspace();
				Cursor.Current = Cursors.Default;
				Interaction.MsgBox("Workspace cleared successfully.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
			} catch (Exception ex) {
				//GISCode.ExceptionHandling.HandleException(ex, "Could not clear workspace")
			} finally {
				Cursor.Current = Cursors.Default;
			}

		}


		private void btnExploreWorkspace_Click(System.Object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(txtWorkspace.Text)) {
				MessageBox.Show("You must define a temporary workspace before you can open it in Windows Explorer.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			} else {
				if (!System.IO.Directory.Exists(txtWorkspace.Text)) {
					MessageBox.Show("The temporary workspace is not a valid folder. Browse and set the temporary workspace to an existing folder on your computer. This should preferably be a folder without spaces or periods in the path.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
			}

			// Should only get to here if the path is valid.
			Process.Start("explorer.exe", txtWorkspace.Text);

		}


		private void cmdDefault_Click(System.Object sender, System.EventArgs e)
		{
			string sNewWorkspacePath = GCDCore.WorkspaceManager.GetDefaultWorkspace(GCDCore.Properties.Resources.ApplicationNameShort);
			if (GCDCore.WorkspaceManager.ValidateWorkspace(sNewWorkspacePath)) {
				txtWorkspace.Text = sNewWorkspacePath;

				// User may have updated whether they want temp workspace warnings
				chkTempWorkspaceWarning.Checked = GCDCore.Properties.Settings.Default.StartUpWorkspaceWarning;
			}

		}

		private void txtWorkspace_TextChanged(System.Object sender, System.EventArgs e)
		{
			btnBrowse.Enabled = (!string.IsNullOrEmpty(txtWorkspace.Text)) && System.IO.Directory.Exists(txtWorkspace.Text);
			btnClearWorkspace.Enabled = btnBrowse.Enabled;
		}

		#endregion


		private void btnDeleteSurveyType_Click(System.Object sender, System.EventArgs e)
		{
			throw new NotImplementedException();

			//Dim CurrentRow As DataRowView = SurveyTypesBindingSource.Current
			//If TypeOf CurrentRow Is DataRowView Then
			//    If TypeOf CurrentRow.Row Is SurveyTypes.SurveyTypesRow Then
			//        Dim surveytype As SurveyTypes.SurveyTypesRow = CurrentRow.Row
			//        Dim sMessage As String = "Are you sure you want to remove the survey type '" & surveytype.Name & "' and its corresponding error value?"
			//        Dim response As MsgBoxResult = MsgBox(sMessage, MsgBoxStyle.YesNo Or MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong)
			//        If response = MsgBoxResult.Yes Then
			//            If Not CurrentRow Is Nothing Then
			//                'Delete the selected item from the dataset and write this new information to the XML file at the specified location
			//                SurveyTypesBindingSource.RemoveCurrent()
			//                Core.GCDProject.ProjectManagerBase.saveSurveyTypes()
			//            End If
			//        End If
			//    End If
			//End If

		}


		private void btnAddSurveyType_Click(System.Object sender, System.EventArgs e)
		{
			try {
				if (txtSurveyType.TextLength < 1) {
					Interaction.MsgBox("Please enter a name for the survey type.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
					return;
				}

				if (nbrError.Value < 0.01 || nbrError.Value > 100) {
					Interaction.MsgBox("Please enter a default error value in meters to be associated with the survey type. The value must be greater than 0.01 and less than 100.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
					return;
				}

				SurveyTypesBindingSource.AddNew();

				DataRowView CurrentRow = SurveyTypesBindingSource.Current;

				CurrentRow["Name"] = txtSurveyType.Text;
				CurrentRow["Error"] = nbrError.Text;

				throw new NotImplementedException();
				//SurveyTypesBindingSource.EndEdit()
				//Core.GCDProject.ProjectManagerBase.saveSurveyTypes()

				txtSurveyType.Text = "";
				//nbrError.Value = nbrError.Minimum

			} catch (Exception ex) {
				if (ex.Message.ToString().ToLower().Contains("name")) {
					Interaction.MsgBox("Please select a unique name for the survey type being added.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
				} else if (ex.Message.ToString().ToLower().Contains("error")) {
					Interaction.MsgBox("The error value is invalid.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
				} else {
					Interaction.MsgBox("An error occured while trying to save the information. " + Constants.vbNewLine + ex.Message);
				}

				SurveyTypesBindingSource.CancelEdit();

			}

		}

		private void btnHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/options");
		}


		private void chkComparativeSymbology_CheckedChanged(System.Object sender, System.EventArgs e)
		{

			if (chkComparativeSymbology.Checked == false) {
				chk3DPointQualityComparative.Checked = false;
				chkInterpolationErrorComparative.Checked = false;
				chkPointDensityComparative.Checked = false;
				chkDoDComparative.Checked = false;
				chkFISErrorComparative.Checked = false;
				grbComparitiveLayers.Enabled = false;


			} else if (chkComparativeSymbology.Checked) {
				grbComparitiveLayers.Enabled = true;

			}

		}


		private void chkAutoApplyTransparency_CheckedChanged(System.Object sender, System.EventArgs e)
		{

			if (chkAutoApplyTransparency.Checked == false) {
				chkAssociatedSurfacesTransparency.Checked = false;
				chkAnalysesTransparency.Checked = false;
				chkErrorSurfacesTransparency.Checked = false;
				grbTransparencyLayer.Enabled = false;
				nudTransparency.Value = -1;
				nudTransparency.Enabled = false;


			} else if (chkAutoApplyTransparency.Checked) {
				grbTransparencyLayer.Enabled = true;
				nudTransparency.Enabled = true;
				nudTransparency.Value = 40;

			}

		}

		private void lnkPyramidsHelp_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://blogs.esri.com/esri/arcgis/2012/11/14/should-i-build-pyramids-or-overviews");
		}

	}

}
