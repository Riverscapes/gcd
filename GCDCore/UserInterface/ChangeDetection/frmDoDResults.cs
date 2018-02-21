using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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

        private UserInterface.UtilityForms.ChartContextMenu cmsChart;

        public frmDoDResults(DoDBase dodItem)
        {
            InitializeComponent();

            DoD = dodItem;
            m_Options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);
            ucProperties.Initialize(DoD);

            cmsChart = new UtilityForms.ChartContextMenu();
  
            // Select the tab control to make it easy for user to quickly pan results
            tabProperties.Select();
        }

        private void DoDResultsForm_Load(object sender, System.EventArgs e)
        {
            if (!ProjectManager.IsArcMap)
            {
                cmdAddToMap.Visible = false;
                txtDoDName.Width = txtDoDName.Width + (cmdAddToMap.Right - cmdProperties.Right);
                cmdBrowse.Left = cmdProperties.Left;
                cmdProperties.Left = cmdAddToMap.Left;
            }

            txtDoDName.Text = DoD.Name;

            ucBars.ChangeStats = DoD.Statistics;
            ucBars.ChartContextMenuStrip = cmsChart.CMS;
            ucHistogram.LoadHistograms(DoD.Histograms.Raw.Data, DoD.Histograms.Thr.Data);
            ucHistogram.ChartContextMenuStrip = cmsChart.CMS;
            ucSummary.RefreshDisplay(DoD.Statistics, m_Options);
        }

        private void cmdAddToMap_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                ProjectManager.OnAddRasterToMap(DoD.ThrDoD);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void cmdBrowse_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start("explorer.exe", DoD.RawDoD.Raster.GISFileInfo.Directory.FullName);
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            try
            {
                frmDoDSummaryProperties frm = new frmDoDSummaryProperties(m_Options);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ucSummary.RefreshDisplay(DoD.Statistics, m_Options);
                    ucHistogram.SetHistogramUnits(m_Options.Units);
                    ucBars.DisplayUnits = new GCDConsoleLib.GCD.UnitGroup(m_Options.VolumeUnits, m_Options.AreaUnits, m_Options.LinearUnits, m_Options.LinearUnits);
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/i-view-change-detection-results");
        }      
    }
}
