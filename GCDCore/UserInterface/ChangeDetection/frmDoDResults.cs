using GCDCore.Project;
using System;
using System.Diagnostics;

namespace GCDCore.UserInterface.ChangeDetection
{
	public partial class frmDoDResults
	{
		/// <summary>
		/// Stores the status of what columns, rows and units to use in the child user controls
		/// </summary>
		/// <remarks>This is passed to the pop-up form </remarks>
		private DoDSummaryDisplayOptions m_Options;
		private DoDBase DoD;

		public frmDoDResults(DoDBase dodItem)
		{
			InitializeComponent();

			DoD = dodItem;
			m_Options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);
			ucProperties.Initialize(DoD);

			// Select the tab control to make it easy for user to quickly pan results
			tabProperties.Select();
		}

		private void DoDResultsForm_Load(object sender, System.EventArgs e)
		{
			if (!ProjectManager.IsArcMap) {
				//cmdAddToMap.Visible = false;
				//txtDoDName.Width = cmdAddToMap.Right - txtDoDName.Left;
			}

            txtDoDName.Text = DoD.Name;

            ucBars.ChangeStats = DoD.Statistics;
			ucHistogram.LoadHistograms(DoD.Histograms.Raw.Data, DoD.Histograms.Thr.Data);
			ucSummary.RefreshDisplay(DoD.Statistics, m_Options);
		}

		private void cmdAddToMap_Click(System.Object sender, System.EventArgs e)
		{
			throw new Exception("Add to map not implemented");

		}

		private void cmdBrowse_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start("explorer.exe", DoD.RawDoD.GISFileInfo.Directory.FullName);
		}

		private void cmdSettings_Click(object sender, EventArgs e)
		{
			try {
				frmDoDSummaryProperties frm = new frmDoDSummaryProperties(m_Options);
				if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
					ucSummary.RefreshDisplay(DoD.Statistics, m_Options);
					ucHistogram.SetHistogramUnits(m_Options.Units);
                    ucBars.DisplayUnits = new GCDConsoleLib.GCD.UnitGroup(m_Options.VolumeUnits, m_Options.AreaUnits, m_Options.LinearUnits, m_Options.LinearUnits);
				}

			} catch (Exception ex) {
				naru.error.ExceptionUI.HandleException(ex);
			}
		}

		private void cmdHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/i-view-change-detection-results");
		}

	}

}
