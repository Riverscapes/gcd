using GCDCore.Project;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

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
        }

        private void frmOptions_Load(System.Object sender, System.EventArgs e)
        {
            // New method to upgrade the GCD user configuration settings
            Properties.Settings.Default.Upgrade();

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
        }

        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {         
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
            Process.Start("http://blogs.esri.com/esri/arcgis/2012/11/14/should-i-build-pyramids-or-overviews");
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "customize-menu/options.html");
        }
    }
}
