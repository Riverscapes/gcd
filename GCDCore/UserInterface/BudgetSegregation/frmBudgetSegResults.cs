using GCDCore.Project;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GCDCore.UserInterface.BudgetSegregation
{
	public partial class frmBudgetSegResults
	{
		private GCDCore.Project.BudgetSegregation BudgetSeg;

		private ChangeDetection.DoDSummaryDisplayOptions m_Options;

		public frmBudgetSegResults(GCDCore.Project.BudgetSegregation BS)
		{
			Load += BudgetSegResultsForm_Load;
			// This call is required by the designer.
			InitializeComponent();

			BudgetSeg = BS;
			ucProperties.Initialize(BudgetSeg.DoD);
		}

		private void BudgetSegResultsForm_Load(object sender, System.EventArgs e)
		{
			txtName.Text = BudgetSeg.Name;
			cboSummaryClass.DataSource = BudgetSeg.Classes.Keys;
			cboECDClass.DataSource = BudgetSeg.Classes.Keys;

			if (cboSummaryClass.Items.Count > 0) {
				cboSummaryClass.SelectedIndex = 0;
				cboECDClass.SelectedIndex = 0;
			}

			txtPolygonMask.Text = ProjectManager.Project.GetRelativePath(BudgetSeg.PolygonMask.FullName);
			txtField.Text = BudgetSeg.MaskField;

			//Hide Report tab for now
			tabMain.TabPages.Remove(TabPage4);
		}

		private void cboSummaryClass_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BudgetSegregationClass classResult = BudgetSeg.Classes[cboSummaryClass.SelectedItem.ToString()];
			ucSummary.RefreshDisplay(classResult.Statistics, ref m_Options);

			// syncronize the two dropdown lits
			cboECDClass.SelectedIndex = cboSummaryClass.SelectedIndex;
		}


		private void cboECDClass_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BudgetSegregationClass classResult = BudgetSeg.Classes[cboECDClass.SelectedItem.ToString()];
			ucHistogram.LoadHistograms(classResult.Histograms.Raw.Data, classResult.Histograms.Thr.Data);

			// Update the elevation change bar chart control
			ucBars.ChangeStats = classResult.Statistics;

			// syncronize the two dropdown lits
			cboSummaryClass.SelectedIndex = cboECDClass.SelectedIndex;
		}

		private void cmdBrowse_Click(System.Object sender, System.EventArgs e)
		{
			if (BudgetSeg.Folder.Exists) {
				Process.Start("explorer.exe", BudgetSeg.Folder.FullName);
			} else {
				MessageBox.Show("The budget segregation folder does not exist: " + BudgetSeg.Folder.FullName,Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void cmdHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results");
		}

		private void AddToMapToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
		{
			ToolStripMenuItem myItem = (ToolStripMenuItem)sender;
			ContextMenuStrip cms = (ContextMenuStrip)myItem.Owner;

			System.IO.FileInfo path = ProjectManager.Project.GetAbsolutePath(cms.SourceControl.Text);
			if (path.Exists) {
				try {
					GCDConsoleLib.Vector gPolygon = new GCDConsoleLib.Vector(path);
					throw new NotImplementedException("not implemented");
					//GCDProject.ProjectManagerUI.ArcMapManager.AddBSMaskVector(gPolygon, m_rBS)
				} catch (Exception ex) {
					//Pass
				}
			}
		}
	}
}
