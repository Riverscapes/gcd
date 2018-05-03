using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class frmDoDSummaryProperties
    {
        public DoDSummaryDisplayOptions Options { get; internal set; }

        public frmDoDSummaryProperties(DoDSummaryDisplayOptions theOptions)
        {
            // This call is required by the designer
            InitializeComponent();

            valMinimum.Minimum = decimal.MinValue;
            valMinimum.Maximum = decimal.MaxValue;
            valMaximum.Minimum = decimal.MinValue;
            valMaximum.Maximum = decimal.MaxValue;

            valYMinimum.Minimum = decimal.MinValue;
            valYMinimum.Maximum = decimal.MaxValue;
            valYMaximum.Minimum = decimal.MinValue;
            valYMaximum.Maximum = decimal.MaxValue;

            Options = theOptions;
        }

        private void frmDoDSummaryProperties_Load(object sender, System.EventArgs e)
        {
            frmFont.Font = Options.Font;
            frmFont.FontMustExist = true;
            frmFont.ShowHelp = false;
            frmFont.ShowColor = false;
            txtFont.Text = UserInterface.Options.frmOptions.FontString(Options.Font);

            frmColourPicker.SolidColorOnly = true;
            frmColourPicker.ShowHelp = false;

            cboLinear.DataSource = GCDUnits.GCDLinearUnits();
            cboArea.DataSource = GCDUnits.GCDAreaUnits();
            cboVolume.DataSource = GCDUnits.GCDVolumeUnits();

            GCDUnits.SelectUnit(cboLinear, Options.LinearUnits);
            GCDUnits.SelectUnit(cboArea, Options.AreaUnits);
            GCDUnits.SelectUnit(cboVolume, Options.VolumeUnits);

            picErosion.BackColor = Options.Erosion;
            picDeposition.BackColor = Options.Deposition;

            valPrecision.Value = Options.m_nPrecision;

            // Do the row check boxes first with the specifc box checked so that they are enabled.
            rdoRowsSpecific.Checked = true;
            chkRowsAreal.Checked = Options.m_bRowsAreal;
            chkVolumetric.Checked = Options.m_bRowsVolumetric;
            chkVertical.Checked = Options.m_bRowsVerticalAverages;
            chkPercentages.Checked = Options.m_bRowsPercentages;

            rdoRowsAll.Checked = Options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll;
            rdoRowsNormalized.Checked = Options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized;
            rdoRowsSpecific.Checked = Options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups;

            UpdateControls();

            chkColsRaw.Checked = Options.m_bColsRaw;
            chkColsThresholded.Checked = Options.m_bColsThresholded;
            chkColsError.Checked = Options.m_bColsPMError;
            chkColsPercentage.Checked = Options.m_bColsPCError;
        }

        private void OnColorBoxClicked(object sender, EventArgs e)
        {
            System.Windows.Forms.PictureBox pic = sender as System.Windows.Forms.PictureBox;
            frmColourPicker.Color = pic.BackColor;
            if (frmColourPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pic.BackColor = frmColourPicker.Color;
            }
        }

        private void rdoRows_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            chkRowsAreal.Enabled = rdoRowsSpecific.Checked;
            chkVolumetric.Enabled = rdoRowsSpecific.Checked;
            chkVertical.Enabled = rdoRowsSpecific.Checked;
            chkPercentages.Enabled = rdoRowsSpecific.Checked;

            if (!chkColsThresholded.Checked)
            {
                chkColsError.Checked = false;
                chkColsPercentage.Checked = false;
            }

            chkColsError.Enabled = chkColsThresholded.Checked;
            chkColsPercentage.Enabled = chkColsThresholded.Checked;

            // Y Axis scale controls
            bool bManualScale = rdoManualYScale.Checked;
            lblMaxYScale.Enabled = bManualScale;
            lblMinYScale.Enabled = bManualScale;
            lblIntervalYScale.Enabled = bManualScale;
            valYMinimum.Enabled = bManualScale;
            valYMaximum.Enabled = bManualScale;
            valYInterval.Enabled = bManualScale;
        }

        private void cmdOK_Click(object sender, System.EventArgs e)
        {
            Options.Font = frmFont.Font;
            Options.Erosion = picErosion.BackColor;
            Options.Deposition = picDeposition.BackColor;

            Options.LinearUnits = ((GCDUnits.FormattedUnit<UnitsNet.Units.LengthUnit>)cboLinear.SelectedItem).Unit;
            Options.AreaUnits = ((GCDUnits.FormattedUnit<UnitsNet.Units.AreaUnit>)cboArea.SelectedItem).Unit;
            Options.VolumeUnits = ((GCDUnits.FormattedUnit<UnitsNet.Units.VolumeUnit>)cboVolume.SelectedItem).Unit;

            Options.m_nPrecision = Convert.ToInt32(valPrecision.Value);

            if (rdoRowsAll.Checked)
            {
                Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll;
            }
            else if (rdoRowsNormalized.Checked)
            {
                Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized;
            }
            else
            {
                Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups;
            }

            Options.m_bRowsAreal = chkRowsAreal.Checked;
            Options.m_bRowsVolumetric = chkVolumetric.Checked;
            Options.m_bRowsVerticalAverages = chkVertical.Checked;
            Options.m_bRowsPercentages = chkPercentages.Checked;

            Options.m_bColsRaw = chkColsRaw.Checked;
            Options.m_bColsThresholded = chkColsThresholded.Checked;
            Options.m_bColsPMError = chkColsError.Checked;
            Options.m_bColsPCError = chkColsPercentage.Checked;

            Options.AutomatedYAxisScale = rdoAutomatedYScale.Checked;
        }

        private void chkColsThresholded_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            UpdateControls();
        }

        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL);
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            GCDUnits.SelectUnit(cboLinear, ProjectManager.Project.Units.HorizUnit);
            GCDUnits.SelectUnit(cboArea, ProjectManager.Project.Units.ArUnit);
            GCDUnits.SelectUnit(cboVolume, ProjectManager.Project.Units.VolUnit);

            valPrecision.Value = Options.m_nPrecision;
            rdoRowsAll.Checked = true;
            chkColsRaw.Checked = true;
            chkColsThresholded.Checked = true;
            chkColsError.Checked = true;
            chkColsPercentage.Checked = true;
        }

        public double XAxisMinimum
        {
            get { return (double)valMinimum.Value; }

            set
            {
                valMinimum.Value = (decimal)value;
            }
        }

        public double XAxisMaximum
        {
            get { return (double)valMaximum.Value; }

            set
            {
                valMaximum.Value = (decimal)value;
            }
        }

        public double XAxisInterval
        {
            get { return (double)valInterval.Value; }

            set
            {
                valInterval.Value = (decimal)value;
            }
        }

        public double YAxisMinimum
        {
            get { return (double)valYMinimum.Value; }

            set
            {
                valYMinimum.Value = (decimal)value;
            }
        }

        public double YAxisMaximum
        {
            get { return (double)valYMaximum.Value; }

            set
            {
                valYMaximum.Value = (decimal)value;
            }
        }

        public double YAxisInterval
        {
            get { return (double)valYInterval.Value; }

            set
            {
                valYInterval.Value = (decimal)value;
            }
        }  

        private void button1_Click(object sender, EventArgs e)
        {
            if (frmFont.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFont.Text = UserInterface.Options.frmOptions.FontString(frmFont.Font);
            }
        }

        private void cmdHelp_Click_1(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        private void YAxisMethod_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
    }
}