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
using naru.math;

namespace GCDUserInterface.ChangeDetection
{

	public partial class frmDoDSummaryProperties
	{


		private DoDSummaryDisplayOptions m_Options;
		public DoDSummaryDisplayOptions Options {
			get { return m_Options; }
		}


		public frmDoDSummaryProperties(DoDSummaryDisplayOptions theOptions)
		{
			Load += DoDSummaryPropertiesForm_Load;
			// This call is required by the designer
			InitializeComponent();

			m_Options = theOptions;

		}


		private void DoDSummaryPropertiesForm_Load(object sender, System.EventArgs e)
		{
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Millimeter);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Centimeter);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Meter);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Kilometer);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Inch);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Foot);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Yard);
			AddUnitsToCombo(UnitsNet.Units.LengthUnit.Mile);

			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareMillimeter);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareCentimeter);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareMeter);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareKilometer);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareInch);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareFoot);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareYard);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareMile);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.Hectare);
			AddUnitsToCombo(UnitsNet.Units.AreaUnit.Acre);

			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicMillimeter);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicCentimeter);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.MetricCup);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.Liter);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicMeter);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicInch);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicFoot);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.UsGallon);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicYard);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicMile);
			AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicKilometer);

			// TODO need Acre feet
			//AddUnitsToCombo(UnitsNet.Units.VolumeUnit.acrefeet)

			NumericUpDown1.Value = m_Options.m_nPrecision;

			// Do the row check boxes first with the specifc box checked so
			// that they are enabled.
			rdoRowsSpecific.Checked = true;
			chkRowsAreal.Checked = m_Options.m_bRowsAreal;
			chkVolumetric.Checked = m_Options.m_bRowsVolumetric;
			chkVertical.Checked = m_Options.m_bRowsVerticalAverages;
			chkPercentages.Checked = m_Options.m_bRowsPercentages;

			rdoRowsAll.Checked = m_Options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll;
			rdoRowsNormalized.Checked = m_Options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized;
			rdoRowsSpecific.Checked = m_Options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups;

			UpdateControls();

			chkColsRaw.Checked = m_Options.m_bColsRaw;
			chkColsThresholded.Checked = m_Options.m_bColsThresholded;
			chkColsError.Checked = m_Options.m_bColsPMError;
			chkColsPercentage.Checked = m_Options.m_bColsPCError;

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

		private void AddUnitsToCombo(UnitsNet.Units.LengthUnit eUnit)
		{
			int i = cboLinear.Items.Add(new LinearComboItem(eUnit.ToString(), eUnit));
			if (eUnit == m_Options.LinearUnits) {
				cboLinear.SelectedIndex = i;
			}
		}


		private void AddUnitsToCombo(UnitsNet.Units.AreaUnit eUnit)
		{
			int i = cboArea.Items.Add(new AreaComboItem(eUnit.ToString(), eUnit));
			if (eUnit == m_Options.AreaUnits) {
				cboArea.SelectedIndex = i;
			}
		}


		private void AddUnitsToCombo(UnitsNet.Units.VolumeUnit eUnit)
		{
			int i = cboVolume.Items.Add(new VolumeComboItem(eUnit.ToString(), eUnit));
			if (eUnit == m_Options.VolumeUnits) {
				cboVolume.SelectedIndex = i;
			}
		}

		/// <summary>
		/// ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		/// </summary>
		/// <remarks></remarks>
		private class UnitComboItem
		{

			private string m_sName;
			public override string ToString()
			{
				return m_sName;
			}

			public UnitComboItem(string sName)
			{
				m_sName = sName;
			}
		}

		/// <summary>
		/// ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		/// </summary>
		/// <remarks></remarks> 
		private class LinearComboItem : UnitComboItem
		{


			private UnitsNet.Units.LengthUnit m_eUnit;
			public UnitsNet.Units.LengthUnit Units {
				get { return m_eUnit; }
			}

			public LinearComboItem(string sName, UnitsNet.Units.LengthUnit eUnit) : base(sName)
			{

				m_eUnit = eUnit;
			}
		}

		/// <summary>
		/// ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		/// </summary>
		/// <remarks></remarks>
		private class AreaComboItem : UnitComboItem
		{


			private UnitsNet.Units.AreaUnit m_eUnit;
			public UnitsNet.Units.AreaUnit Units {
				get { return m_eUnit; }
			}

			public AreaComboItem(string sName, UnitsNet.Units.AreaUnit eUnit) : base(sName)
			{

				m_eUnit = eUnit;
			}
		}

		/// <summary>
		/// ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		/// </summary>
		/// <remarks></remarks>
		private class VolumeComboItem : UnitComboItem
		{


			private UnitsNet.Units.VolumeUnit m_eUnit;
			public UnitsNet.Units.VolumeUnit Units {
				get { return m_eUnit; }
			}

			public VolumeComboItem(string sName, UnitsNet.Units.VolumeUnit eUnit) : base(sName)
			{

				m_eUnit = eUnit;
			}
		}


		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			m_Options.LinearUnits = ((LinearComboItem)cboLinear.SelectedItem).Units;
			m_Options.AreaUnits = ((AreaComboItem)cboArea.SelectedItem).Units;
			m_Options.VolumeUnits = ((VolumeComboItem)cboVolume.SelectedItem).Units;

			m_Options.m_nPrecision = Convert.ToInt32(NumericUpDown1.Value);

			if (rdoRowsAll.Checked) {
				m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll;
			} else if (rdoRowsNormalized.Checked) {
				m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized;
			} else {
				m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups;
			}

			m_Options.m_bRowsAreal = chkRowsAreal.Checked;
			m_Options.m_bRowsVolumetric = chkVolumetric.Checked;
			m_Options.m_bRowsVerticalAverages = chkVertical.Checked;
			m_Options.m_bRowsPercentages = chkPercentages.Checked;

			m_Options.m_bColsRaw = chkColsRaw.Checked;
			m_Options.m_bColsThresholded = chkColsThresholded.Checked;
			m_Options.m_bColsPMError = chkColsError.Checked;
			m_Options.m_bColsPCError = chkColsPercentage.Checked;

		}

		private void chkColsThresholded_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			UpdateControls();
		}

		private void cmdHelp_Click(System.Object sender, System.EventArgs e)
		{
			//Process.Start(My.Resources.HelpBaseURL & "")
		}
	}

}
