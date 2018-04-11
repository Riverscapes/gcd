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

            cmsChart = new UtilityForms.ChartContextMenu(DoD.Folder, "change_detection");

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

            cmdOK.Text = GCDCore.Properties.Resources.UpdateButtonText;
        }

        private void cmdAddToMap_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                ProjectManager.OnAddRasterToMap(DoD.ThrDoD);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
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

                frm.XAxisMinimum = ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisX.Minimum;
                frm.XAxisMaximum = ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisX.Maximum;
                frm.XAxisInterval = ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisX.Interval;
                ucHistogram.HistogramViewer.Chart.ChartAreas[0].RecalculateAxesScale();

                frm.YAxisMinimum = ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.Minimum;
                frm.YAxisMaximum = ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.Maximum;
                frm.YAxisInterval = ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.Interval;

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisX.Minimum = frm.XAxisMinimum;
                    //ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisX.Maximum = frm.XAxisMaximum;
                    //ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisX.Interval = frm.XAxisInterval;

                    ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.Minimum = frm.YAxisMinimum;
                    ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.Maximum = frm.YAxisMaximum;
                    ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.Interval = frm.YAxisInterval;
                    ucHistogram.HistogramViewer.Chart.ChartAreas[0].AxisY.IntervalType = DateTimeIntervalType.Number;

                    ucHistogram.HistogramViewer.SetFont(m_Options.Font);
                    ucHistogram.HistogramViewer.Chart.Series[Visualization.ViewerBase.EROSION].Color = m_Options.Erosion;
                    ucHistogram.HistogramViewer.Chart.Series[Visualization.ViewerBase.DEPOSITION].Color = m_Options.Deposition;
                    ucHistogram.HistogramViewer.Chart.DataBind();

                    ucSummary.RefreshDisplay(DoD.Statistics, m_Options);
                    ucHistogram.SetHistogramUnits(m_Options.Units);
                    ucBars.DisplayUnits = new GCDConsoleLib.GCD.UnitGroup(m_Options.VolumeUnits, m_Options.AreaUnits, m_Options.LinearUnits, m_Options.LinearUnits);

                    ucBars.Viewer.SetFont(m_Options.Font);
                    ucBars.chtControl.Series[Visualization.ViewerBase.EROSION].Color = m_Options.Erosion;
                    ucBars.chtControl.Series[Visualization.ViewerBase.DEPOSITION].Color = m_Options.Deposition;

                    Series serNet = ucBars.chtControl.Series.FindByName(Visualization.ViewerBase.NET);
                    if (serNet is Series)
                        serNet.Color = serNet.Points[0].YValues[0] > 0 ? m_Options.Deposition : m_Options.Deposition;
                }

            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
        }

        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // Sanity check to avoid blank names
            txtDoDName.Text = txtDoDName.Text.Trim();

            if (string.IsNullOrEmpty(txtDoDName.Text))
            {
                MessageBox.Show("The change detection name cannot be empty.", "Empty Change Detection Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDoDName.Select();
                DialogResult = DialogResult.None;
                return;
            }

            if (!ProjectManager.Project.IsDoDNameUnique(txtDoDName.Text, DoD))
            {
                MessageBox.Show("This GCD project already contains a change detection with this name. Please choose a unique name for this change detection.", "Change Detection Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDoDName.Select();
                DialogResult = DialogResult.None;
                return;
            }

            if (string.Compare(DoD.Name, txtDoDName.Text, false) != 0)
            {
                ProjectManager.Project.DoDs.Remove(DoD.Name);
                DoD.Name = txtDoDName.Text;
                ProjectManager.Project.DoDs[DoD.Name] = DoD;
                ProjectManager.Project.Save();
            }
        }
    }
}
