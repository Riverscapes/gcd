using GCDCore.Project;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GCDCore.UserInterface.Options
{
    public partial class frmOptions
    {
        private readonly naru.ui.SortableBindingList<SurveyType> SurveyTypes;

        public frmOptions()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            SurveyTypes = new naru.ui.SortableBindingList<SurveyType>(ProjectManager.SurveyTypes.Values.ToList<SurveyType>());
            grdSurveyTypes.DataSource = SurveyTypes;

            frmColourPicker.SolidColorOnly = true;
            frmColourPicker.ShowHelp = false;
        }

        private void frmOptions_Load(System.Object sender, System.EventArgs e)
        {
            // New method to upgrade the GCD user configuration settings
            Properties.Settings.Default.Upgrade();

            frmFont.Font = Properties.Settings.Default.ChartFont;
            txtFont.Text = FontString(frmFont.Font);

            picErosion.BackColor = Properties.Settings.Default.Erosion;
            picDeposition.BackColor = Properties.Settings.Default.Deposition;

            tTip.SetToolTip(grdSurveyTypes, "List of survey data collection methods and the corresponding default uncertainty values.");
            tTip.SetToolTip(numChartWidth, "The default width, in pixels, of the charts saved as part of change detections.");
            tTip.SetToolTip(numChartHeight, "The default height, in pixels, of the charts saved as part of change detections.");
            tTip.SetToolTip(picErosion, "The default color used to display surface lowering in change detection charts.");
            tTip.SetToolTip(picDeposition, "The default color used to display surface raising in change detection charts.");
            tTip.SetToolTip(cmdResetColours, "Reset the chart colors to the defaults that are used when GCD is initially installed.");
            tTip.SetToolTip(txtFont, "The default font used for change detection chart axes, titles and legends.");
            tTip.SetToolTip(cmdFont, "Change the default font used for change detection chart axes, titles and legends.");
            tTip.SetToolTip(chkManualDoDRange, "Check this box to specify the range used to symbolize DoD rasters. Leave the box unchecked to use the symmetrical raster minimum and maximum values.");
            tTip.SetToolTip(chkManualDoDRange, "The range used to symbolize DoD rasters. This value only applies when the adjacent checkbox is checked.");

            //chkBoxValidateProjectOnLoad.Checked = Properties.Settings.Default.ValidateProjectOnLoad;
            chkComparativeSymbology.Checked = Properties.Settings.Default.ApplyComparativeSymbology;
            chkAutoApplyTransparency.Checked = Properties.Settings.Default.ApplyTransparencySymbology;

            if (chkComparativeSymbology.Checked)
            {
                //Settings based on chkComparativeSymbology (always turned off when chkComparativeSymbology is turned off)
                chk3DPointQualityComparative.Checked = Properties.Settings.Default.ComparativeSymbology3dPointQuality;
                chkInterpolationErrorComparative.Checked = Properties.Settings.Default.ComparativeSymbologyInterpolationError;
                chkPointDensityComparative.Checked = Properties.Settings.Default.ComparativeSymbologyPointDensity;
                chkDoDComparative.Checked = Properties.Settings.Default.ComparativeSymbologyDoD;
                chkDoDComparative.Checked = Properties.Settings.Default.ComparativeSymbologyFISError;
                grbComparitiveLayers.Enabled = true;
            }
            else
            {
                grbComparitiveLayers.Enabled = false;
            }

            if (chkAutoApplyTransparency.Checked)
            {
                chkAssociatedSurfacesTransparency.Checked = Properties.Settings.Default.TransparencyAssociatedLayers;
                chkAnalysesTransparency.Checked = Properties.Settings.Default.TransparencyAnalysesLayers;
                chkErrorSurfacesTransparency.Checked = Properties.Settings.Default.TransparencyErrorLayers;
                nudTransparency.Value = Properties.Settings.Default.AutoTransparencyValue;
                grbTransparencyLayer.Enabled = true;
                nudTransparency.Enabled = true;
            }
            else
            {
                grbTransparencyLayer.Enabled = false;
                nudTransparency.Enabled = false;
                nudTransparency.Value = -1;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////
            // Sizes for exporting charts to file
            numChartWidth.Value = Properties.Settings.Default.ChartWidth;
            numChartHeight.Value = Properties.Settings.Default.ChartHeight;

            //////////////////////////////////////////////////////////////////////////////////////////////////
            // Automatic Pyramid building features
            try
            {
                lstPyramids.Items.Clear();
                foreach (RasterPyramidManager.PyramidRasterTypes eType in Enum.GetValues(typeof(RasterPyramidManager.PyramidRasterTypes)))
                {
                    bool bPyramids = ProjectManager.PyramidManager.AutomaticallyBuildPyramids(eType);
                    lstPyramids.Items.Add(new PyramidRasterTypeDisplay(eType), bPyramids);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error building automated pymarids options list.\n\n{0}", ex.Message);
                lstPyramids.Items.Clear();
            }

            // DoD symbology range. Zero means the settings is disabled
            if (Properties.Settings.Default.DoDSymbologyRange == 0)
            {
                chkManualDoDRange.Checked = false;
            }
            else
            {
                chkManualDoDRange.Checked = true;
                valManualDoDRange.Value = (decimal)Properties.Settings.Default.DoDSymbologyRange;
            }
            chkManualDoDRange_CheckedChanged(sender, e);
        }

        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            Properties.Settings.Default.Erosion = picErosion.BackColor;
            Properties.Settings.Default.Deposition = picDeposition.BackColor;
            Properties.Settings.Default.ChartFont = frmFont.Font;

            //GCDCore.Properties.Settings.Default.AddInputLayersToMap = chkAddInputLayersToMap.Checked
            //GCDCore.Properties.Settings.Default.AddOutputLayersToMap = chkAddOutputLayersToMap.Checked
            //Properties.Settings.Default.ValidateProjectOnLoad = chkBoxValidateProjectOnLoad.Checked;
            Properties.Settings.Default.ApplyComparativeSymbology = chkComparativeSymbology.Checked;
            Properties.Settings.Default.ApplyTransparencySymbology = chkAutoApplyTransparency.Checked;

            //Settings based on chkComparativeSymbology (always turned off when chkComparativeSymbology is turned off)
            Properties.Settings.Default.ComparativeSymbology3dPointQuality = chk3DPointQualityComparative.Checked;
            Properties.Settings.Default.ComparativeSymbologyInterpolationError = chkInterpolationErrorComparative.Checked;
            Properties.Settings.Default.ComparativeSymbologyPointDensity = chkPointDensityComparative.Checked;
            Properties.Settings.Default.ComparativeSymbologyDoD = chkDoDComparative.Checked;
            Properties.Settings.Default.ComparativeSymbologyFISError = chkFISErrorComparative.Checked;

            //Settings based on chkAutoApplyTransparency (always turned off when chkAutoApplyTransparency is turned off)
            Properties.Settings.Default.TransparencyAssociatedLayers = chkAssociatedSurfacesTransparency.Checked;
            Properties.Settings.Default.TransparencyAnalysesLayers = chkAnalysesTransparency.Checked;
            Properties.Settings.Default.TransparencyErrorLayers = chkErrorSurfacesTransparency.Checked;
            Properties.Settings.Default.AutoTransparencyValue = (short)nudTransparency.Value;

            //chart setting for exporting charts
            Properties.Settings.Default.ChartWidth = (int)numChartWidth.Value;
            Properties.Settings.Default.ChartHeight = (int)numChartHeight.Value;

            for (int i = 0; i <= lstPyramids.Items.Count - 1; i++)
            {
                PyramidRasterTypeDisplay lItem = (PyramidRasterTypeDisplay)lstPyramids.Items[i];
                ProjectManager.PyramidManager.SetAutomaticPyramidsForRasterType(lItem.RasterType, lstPyramids.CheckedIndices.Contains(i));
            }

            Properties.Settings.Default.AutomaticPyramids = ProjectManager.PyramidManager.GetPyramidSettingString();

            ////////////////////////////////////////////////////////////////////
            // Survey Types
            Dictionary<string, SurveyType> d = new Dictionary<string, SurveyType>();
            foreach (SurveyType s in SurveyTypes)
                d.Add(s.Name, s);
            ProjectManager.SurveyTypes = d;

            // DoD symbology types (zero means that the setting is disable)
            Properties.Settings.Default.DoDSymbologyRange = chkManualDoDRange.Checked ? (double)valManualDoDRange.Value : 0;

            ////////////////////////////////////////////////////////////////////
            // Save the project settings
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void chkComparativeSymbology_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkComparativeSymbology.Checked)
            {
                grbComparitiveLayers.Enabled = true;
            }
            else
            {
                chk3DPointQualityComparative.Checked = false;
                chkInterpolationErrorComparative.Checked = false;
                chkPointDensityComparative.Checked = false;
                chkDoDComparative.Checked = false;
                chkFISErrorComparative.Checked = false;
                grbComparitiveLayers.Enabled = false;
            }
        }

        private void chkAutoApplyTransparency_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkAutoApplyTransparency.Checked)
            {
                grbTransparencyLayer.Enabled = true;
                nudTransparency.Enabled = true;
                nudTransparency.Value = 40;
            }
            else
            {
                chkAssociatedSurfacesTransparency.Checked = false;
                chkAnalysesTransparency.Checked = false;
                chkErrorSurfacesTransparency.Checked = false;
                grbTransparencyLayer.Enabled = false;
                nudTransparency.Value = -1;
                nudTransparency.Enabled = false;
            }
        }

        private void lnkPyramidsHelp_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://desktop.arcgis.com/en/arcmap/10.3/manage-data/raster-and-images/raster-pyramids.htm");
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "customize-menu/options.html");
        }

        private void picBox_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            frmColourPicker.Color = pic.BackColor;
            if (frmColourPicker.ShowDialog() == DialogResult.OK)
            {
                pic.BackColor = frmColourPicker.Color;
            }
        }

        private void cmdFont_Click(object sender, EventArgs e)
        {
            frmFont.Font = Properties.Settings.Default.ChartFont;
            if (frmFont.ShowDialog() == DialogResult.OK)
            {
                txtFont.Text = FontString(frmFont.Font);
            }
        }

        public static string FontString(Font font)
        {
            string fontstring = string.Format("{0}, {1}pts", font.Name, font.Size);
            if (font.Bold || font.Italic)
            {
                fontstring = string.Format("{0},{1}{2}", fontstring, font.Bold ? " bold" : "", font.Italic ? " italic" : "");
            }

            return fontstring;
        }

        private void btnHelp_Click_1(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        private void cmdResetColours_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to reset colors used for surface raising and lowering in GCD graphs to the default values?",
                "Reset Colors To Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                picErosion.BackColor = Color.FromArgb(192, 0, 0);
                picDeposition.BackColor = Color.FromArgb(27, 63, 139);
            }
        }

        private void chkManualDoDRange_CheckedChanged(object sender, EventArgs e)
        {
            valManualDoDRange.Enabled = chkManualDoDRange.Checked;
        }
    }
}
