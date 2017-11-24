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

namespace GCDUserInterface.UI.BudgetSegregation
{

	public partial class frmBudgetSegProperties
	{

		private GCDCore.Project.BudgetSegregation m_BudgetSeg;

		private DoDBase InitialDoD;
		public GCDCore.Project.BudgetSegregation BudgetSeg {
			get { return m_BudgetSeg; }
		}


		public frmBudgetSegProperties(DoDBase parentDoD)
		{
			Load += BudgetSegPropertiesForm_Load;
			// This call is required by the designer.
			InitializeComponent();
			InitialDoD = parentDoD;

			ucPolygon.Initialize("Budget Segregation Polygon Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon);
		}


		private void BudgetSegPropertiesForm_Load(object sender, System.EventArgs e)
		{
			cboDoD.DataSource = ProjectManager.Project.DoDs.Values;
			cboDoD.SelectedItem = InitialDoD;

		}


		private void cmdOK_Click(System.Object sender, System.EventArgs e)
		{
			if (!ValidateForm()) {
				this.DialogResult = DialogResult.None;
				return;
			}

			try {
				Cursor.Current = Cursors.WaitCursor;

				DoDBase dod = (DoDBase)cboDoD.SelectedItem;
				System.IO.DirectoryInfo bsFolder = ProjectManager.OutputManager.GetBudgetSegreationDirectoryPath(dod.Folder, true);

				GCDCore.Engines.BudgetSegregationEngine bsEngine = new GCDCore.Engines.BudgetSegregationEngine(txtName.Text, bsFolder);
				m_BudgetSeg = bsEngine.Calculate(dod, ucPolygon.SelectedItem, cboField.Text);

				ProjectManager.Project.Save();

			} catch (Exception ex) {
				naru.error.ExceptionUI.HandleException(ex);
			} finally {
				Cursor.Current = Cursors.Default;
			}

		}

		private bool ValidateForm()
		{

			// Sanity check to avoid names with only empty spaces
			txtName.Text = txtName.Text.Trim();

			if (!cboDoD.SelectedItem is DoDBase) {
				MessageBox.Show("Please choose a change detection analysis on which you want to base this budget segregation.", "Missing Change Detection", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}


			if (string.IsNullOrEmpty(txtName.Text)) {
				Interaction.MsgBox("Please enter a name for the budget segregation analysis.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
				return null;
			} else {
				if (!((DoDBase)cboDoD.SelectedItem).IsBudgetSegNameUnique(txtName.Text, null)) {
					Interaction.MsgBox("Another budget segregation already uses the name '" + txtName.Text + "'. Please choose a unique name.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
					return false;
				}
			}


			if (ucPolygon.SelectedItem is GCDConsoleLib.Vector) {
				if (ucPolygon.SelectedItem.Features.Count() < 1) {
					Interaction.MsgBox("The polygon mask feature class is empty and contains no features. You must choose a polygon feature class with at least one feature.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
					return false;
				}

				DoDBase dod = (DoDBase)cboDoD.SelectedItem;

				if (ucPolygon.SelectedItem.Proj.PrettyWkt.ToLower.Contains("unknown")) {
					Interaction.MsgBox("The selected feature class appears to be missing a spatial reference. All GCD inputs must possess a spatial reference and it must be the same spatial reference for all datasets in a GCD project." + " If the selected feature class exists in the same coordinate system, " + dod.RawDoD.Raster.Proj.PrettyWkt + ", but the coordinate system has not yet been defined for the feature class." + " Use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected datasets by defining the coordinate system as:" + Constants.vbCrLf + Constants.vbCrLf + dod.RawDoD.Raster.Proj.PrettyWkt + Constants.vbCrLf + Constants.vbCrLf + "Then try using it with the GCD again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
					return false;
				} else {
					if (!dod.RawDoD.Raster.Proj.IsSame(ucPolygon.SelectedItem.Proj)) {
						Interaction.MsgBox("The coordinate system of the selected feature class:" + Constants.vbCrLf + Constants.vbCrLf + ucPolygon.SelectedItem.Proj.PrettyWkt + Constants.vbCrLf + Constants.vbCrLf + "does not match that of the GCD project:" + Constants.vbCrLf + Constants.vbCrLf + dod.RawDoD.Raster.Proj.PrettyWkt + Constants.vbCrLf + Constants.vbCrLf + "All datasets within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " + "If you believe that the selected dataset does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " + "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected dataset by defining the coordinate system as:" + Constants.vbCrLf + Constants.vbCrLf + dod.RawDoD.Raster.Proj.PrettyWkt + Constants.vbCrLf + Constants.vbCrLf + "Then try importing it into the GCD again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
						return false;
					}
				}
			} else {
				Interaction.MsgBox("Please choose a polygon mask on which you wish to base this budget segregation.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
				return false;
			}

			if (string.IsNullOrEmpty(cboField.Text)) {
				Interaction.MsgBox("Please choose a polygon mask field. Or add a \"string\" field to the feature class if one does not exist.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
				return false;
			}

			return true;

		}


		private void cboDoD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (!cboDoD.SelectedItem is DoDBase) {
				return;
			}

			DoDBase dod = (DoDBase)cboDoD.SelectedItem;

			txtNewDEM.Text = dod.NewDEM.Name;
			txtOldDEM.Text = dod.OldDEM.Name;

			if (dod is DoDMinLoD) {
				txtUncertaintyAnalysis.Text = string.Format("Minimum Level of Detection at {0:#.00}{1}", ((DoDMinLoD)dod).Threshold, ProjectManager.Project.Units.VertUnit);
			} else if (dod is DoDPropagated) {
				txtUncertaintyAnalysis.Text = "Propagated Error";
			} else {
				txtUncertaintyAnalysis.Text = string.Format("Probabilistic at {0}% confidence level", ((DoDProbabilistic)dod).ConfidenceLevel * 100);
			}

			txtOutputFolder.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetBudgetSegreationDirectoryPath(dod.Folder, false).FullName);

		}


		private void PolygonChanged(object sender, naru.ui.PathEventArgs e)
		{
			cboField.Items.Clear();

			if (!ucPolygon.SelectedItem is GCDConsoleLib.Vector) {
				return;
			}

			cboField.DataSource = ucPolygon.SelectedItem.Fields.Values.Where(p => p.Type.Equals(GCDConsoleLib.GDalFieldType.StringField));

			if (cboField.Items.Count > 0) {
				cboField.SelectedIndex = 0;
			}

		}

		private void cmdHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/v-add-budget-segregation");
		}
	}

}
