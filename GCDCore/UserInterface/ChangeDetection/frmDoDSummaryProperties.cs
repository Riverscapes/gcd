using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class frmDoDSummaryProperties
    {
        public DoDSummaryDisplayOptions Options {get; internal set;}

		public frmDoDSummaryProperties(DoDSummaryDisplayOptions theOptions)
		{
			// This call is required by the designer
			InitializeComponent();

			Options = theOptions;
		}

		private void frmDoDSummaryProperties_Load(object sender, System.EventArgs e)
		{
            cboLinear.DataSource = GCDUnits.GCDLinearUnitsAsString();
            cboArea.DataSource = GCDUnits.GCDAreaUnitsAsString();
            cboVolume.DataSource = GCDUnits.GCDVolumeUnitsAsString();

            cboLinear.Text = Options.LinearUnits.ToString();
            cboArea.Text = Options.AreaUnits.ToString();
            cboVolume.Text = Options.VolumeUnits.ToString();

			// TODO need Acre feet
			//AddUnitsToCombo(UnitsNet.Units.VolumeUnit.acrefeet)

			NumericUpDown1.Value = Options.m_nPrecision;

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

			if (!chkColsThresholded.Checked) {
				chkColsError.Checked = false;
				chkColsPercentage.Checked = false;
			}

			chkColsError.Enabled = chkColsThresholded.Checked;
			chkColsPercentage.Enabled = chkColsThresholded.Checked;
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
            //Options.LinearUnits = (UnitsNet.Units.VolumeUnit)Enum.Parse(typeof(UnitsNet.Units.VolumeUnit), cboVolumeUnits.SelectedItem.ToString()),
            //             (UnitsNet.Units.AreaUnit)Enum.Parse(typeof(UnitsNet.Units.AreaUnit), cboAreaUnits.SelectedItem.ToString()),
            //             (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), cboVerticalUnits.SelectedItem.ToString()),

            Options.LinearUnits = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), cboLinear.Text);
            Options.AreaUnits = (UnitsNet.Units.AreaUnit)Enum.Parse(typeof(UnitsNet.Units.AreaUnit), cboArea.Text);
            Options.VolumeUnits= (UnitsNet.Units.VolumeUnit)Enum.Parse(typeof(UnitsNet.Units.VolumeUnit), cboVolume.Text);
                        
			Options.m_nPrecision = Convert.ToInt32(NumericUpDown1.Value);

			if (rdoRowsAll.Checked) {
				Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll;
			} else if (rdoRowsNormalized.Checked) {
				Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized;
			} else {
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
            cboLinear.Text = ProjectManager.Project.Units.VertUnit.ToString();
            cboArea.Text = ProjectManager.Project.Units.ArUnit.ToString();
            cboVolume.Text = ProjectManager.Project.Units.VolUnit.ToString();
        }
    }
}