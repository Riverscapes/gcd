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
            if (!ProjectManager.IsArcMap)
            {
                cmdAddToMap2.Visible = false;
                txtDoDName.Width = txtDoDName.Width + (cmdAddToMap2.Right - cmdProperties.Right);
                cmdBrowse2.Left = cmdProperties.Left;
                cmdProperties.Left = cmdAddToMap2.Left;
            }

            txtDoDName.Text = DoD.Name;

            ucBars.ChangeStats = DoD.Statistics;
            ucBars.ChartContextMenuStrip = cmsChart;
            ucHistogram.LoadHistograms(DoD.Histograms.Raw.Data, DoD.Histograms.Thr.Data);
            ucHistogram.ChartContextMenuStrip = cmsChart;
            ucSummary.RefreshDisplay(DoD.Statistics, m_Options);
        }

        private void cmdAddToMap_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                ProjectManager.OnAddToMap(DoD.ThrDoD);
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

        #region Chart Control Context Menu Strip

        private Chart GetChartControl(object sender)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenu that contains this MenuItem
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;

                // Get the control that is displaying this context menu
                Control sourceControl = menu.SourceControl;
                if (sourceControl is Chart)
                {
                    return sourceControl as Chart;
                }
            }

            return null;
        }

        private void copyChartImageToClipboardToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Chart cht = GetChartControl(sender);
                    cht.SaveImage(ms, ChartImageFormat.Bmp);
                    Bitmap bm = new Bitmap(ms);
                    Clipboard.SetImage(bm);
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "An error occurred copying the chart image to the clipboard");
            }
        }

        private void saveChartImageToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Chart cht = GetChartControl(sender);

                List<string> lFormats = new List<string>();
                lFormats.Add("Bitmap (*.bmp)|*.bmp");
                lFormats.Add("GIF (*.gif)|*.gif");
                lFormats.Add("JPEG (*.jpg)|*.jpg");
                lFormats.Add("PNG (*.png)|*.png");
                lFormats.Add("TIFF (*.tiff)|*.tif");
                lFormats.Add("WMF (*.wmf)|*.wmf");
                SaveFileDialog dlgSave = new SaveFileDialog();
                dlgSave.Title = "Save Chart Image";
                dlgSave.AddExtension = true;
                foreach (string sFormat in lFormats)
                {
                    if (string.IsNullOrEmpty(dlgSave.Filter))
                    {
                        dlgSave.Filter = sFormat;
                    }
                    else
                    {
                        dlgSave.Filter = dlgSave.Filter + "|" + sFormat;
                    }
                }

                dlgSave.FilterIndex = 3;
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = dlgSave.FileName;
                    System.Drawing.Imaging.ImageFormat imgFormat;
                    switch (dlgSave.FilterIndex)
                    {
                        case 1: imgFormat = System.Drawing.Imaging.ImageFormat.Bmp; break;
                        case 2: imgFormat = System.Drawing.Imaging.ImageFormat.Gif; break;
                        case 3: imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                        case 4: imgFormat = System.Drawing.Imaging.ImageFormat.Png; break;
                        case 5: imgFormat = System.Drawing.Imaging.ImageFormat.Tiff; break;
                        case 6: imgFormat = System.Drawing.Imaging.ImageFormat.Wmf; break;
                        default:
                            Exception ex = new Exception("Unhandled image format.");
                            ex.Data["Filter Index"] = dlgSave.FilterIndex;
                            ex.Data["Filter"] = dlgSave.Filter[dlgSave.FilterIndex];
                            ex.Data["File Path"] = sFilePath;
                            throw ex;
                    }

                    cht.SaveImage(sFilePath, imgFormat);
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "An error occurred copying the chart image to the clipboard");
            }
        }

        #endregion
    }
}
