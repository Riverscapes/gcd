using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection
{
	public partial class ucDoDSummary
	{
		private void DoDSummaryUC_Load(object sender, System.EventArgs e)
		{
			grdData.AllowUserToAddRows = false;
			grdData.RowHeadersVisible = false;
			grdData.ColumnHeadersVisible = false;
			grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grdData.AllowUserToOrderColumns = false;
			grdData.AllowUserToResizeRows = false;

			grdData.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			grdData.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			grdData.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			grdData.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			grdData.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
		}

		public void RefreshDisplay(GCDConsoleLib.GCD.DoDStats dodStats, ref DoDSummaryDisplayOptions options)
		{
			// Build the string formatting based on the precision in the pop-up properties form
			string sFormat = "#,##0";
			for (int i = 1; i <= options.m_nPrecision; i++) {
				if (i == 1) {
					sFormat += ".";
				}
				sFormat += "0";
			}

			grdData.Rows.Clear();

			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			// Show/hide columns based on the properties pop-up
			grdData.Columns[1].Visible = options.m_bColsRaw;
			grdData.Columns[2].Visible = options.m_bColsThresholded;
			grdData.Columns[3].Visible = options.m_bColsPMError;
			grdData.Columns[4].Visible = options.m_bColsPCError;
			grdData.Columns[5].Visible = options.m_bColsPCError;

			DataGridViewRow aRow = null;
			int nIndex = 0;
			DataGridViewCell cell = null;

			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			// Title Row
			nIndex = grdData.Rows.Add(1);
			aRow = grdData.Rows[nIndex];

			aRow.Cells[0].Value = "Attribute";
			aRow.Cells[0].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);

			aRow.Cells[1].Value = "Raw";
			aRow.Cells[1].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
			aRow.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			aRow.Cells[2].Value = "Thresholded";
			aRow.Cells[2].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
			aRow.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			aRow.Cells[4].Value = "± Error Volume";
			aRow.Cells[4].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
			aRow.Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			aRow.Cells[5].Value = "% Error";
			aRow.Cells[5].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
			aRow.Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

			UnitsNet.Area ca = ProjectManager.Project.CellArea;
			UnitsNet.Units.LengthUnit vunit = ProjectManager.Project.Units.VertUnit;

			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			//Area of erosion header

			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsAreal)) {
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "AREAL:";
				cell = aRow.Cells[0];
				cell.Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);

				//Area of erosion
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Area of Erosion (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
				//the superscript 2 is ALT+0178
				aRow.Cells[0].ToolTipText = "The amount of area experiencing erosion";
				aRow.Cells[1].Value = dodStats.ErosionRaw.GetArea(ca).As(options.AreaUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.ErosionThr.GetArea(ca).As(options.AreaUnits).ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				//Area of deposition
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Area of Deposition (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
				aRow.Cells[0].ToolTipText = "The amount of area experiencing deposition";
				aRow.Cells[1].Value = dodStats.DepositionRaw.GetArea(ca).As(options.AreaUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.DepositionThr.GetArea(ca).As(options.AreaUnits).ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				//Area of detectable change
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Area of Detectable Change (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
				aRow.Cells[0].ToolTipText = "The sum of areas experiencing detectable erosion and deposition";
				aRow.Cells[1].Value = "NA";
				aRow.Cells[2].Value = dodStats.AreaDetectableChange_Thresholded.As(options.AreaUnits).ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				//Area of interest 
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Area of Interest (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
				aRow.Cells[0].ToolTipText = "The total amount of area under analysis (including detectable and undetectable)";
				aRow.Cells[1].Value = dodStats.AreaOfInterest_Raw.As(options.AreaUnits).ToString(sFormat);
				aRow.Cells[2].Value = "NA";
				aRow.Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				// Percent of area of interest with detectable change
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Percent of Area of Interest with Detectable Change";
				aRow.Cells[0].ToolTipText = "The percent of the total area of interest with detectable changes (i.e. either exceeding the minimum level of detection or with a proability greater then the confidence interval chosen by user)";
				aRow.Cells[1].Value = "NA";
				aRow.Cells[1].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[2].Value = dodStats.AreaPercentAreaInterestWithDetectableChange.ToString(sFormat) + "%";
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;
			}
			//
			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			// VOLUMETRIC
			//

			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVolumetric)) {
				// Volume of erosion header
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "VOLUMETRIC:";
				cell = aRow.Cells[0];
				cell.Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
			}


			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVolumetric)) {
				//Volume of erosion
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Volume of Erosion (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
				//the superscript 3 is ALT+0179
				aRow.Cells[0].ToolTipText = "On a cell-by-cell basis, the DoD erosion depth multiplied by cell area and summed";
				aRow.Cells[1].Value = dodStats.ErosionRaw.GetVolume(ca, vunit).As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.ErosionThr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.ErosionErr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.VolumeOfErosion_Percent.ToString(sFormat);

				//Volume of deposition
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Volume of Deposition (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
				aRow.Cells[0].ToolTipText = "On a cell-by-cell basis, the DoD deposition depth multiplied by cell area and summed";
				aRow.Cells[1].Value = dodStats.DepositionRaw.GetVolume(ca, vunit).As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.DepositionThr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.DepositionErr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.VolumeOfDeposition_Percent.ToString(sFormat);

				//Total volume of difference
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Volume of Difference (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
				aRow.Cells[0].ToolTipText = "The sum of erosion and deposition volumes (a measure of total turnover)";
				aRow.Cells[1].Value = dodStats.VolumeOfDifference_Raw.As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.VolumeOfDifference_Thresholded.As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.VolumeOfDifference_Error.As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.VolumeOfDifference_Percent.ToString(sFormat);
			}


			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVolumetric)) {
				//Total NET volume difference
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Total Net Volume Difference (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
				aRow.Cells[0].ToolTipText = "The net difference of erosion and depostion volumes (i.e. deposition minus erosion)";
				aRow.Cells[1].Value = dodStats.NetVolumeOfDifference_Raw.As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.NetVolumeOfDifference_Thresholded.As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.NetVolumeOfDifference_Error.As(options.VolumeUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.NetVolumeOfDifference_Percent.ToString(sFormat);
			}
			//
			///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			// VERTICAL AVERAGES
			//

			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages)) {
				// Vertical Averages header
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "VERTICAL AVERAGES:";
				cell = aRow.Cells[0];
				cell.Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
			}


			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages) {
				// Average Depth of Erosion
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Average Depth of Erosion (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ")";
				aRow.Cells[0].ToolTipText = "The average depth of erosion (erosion volume dividied by erosion area)";
				aRow.Cells[1].Value = dodStats.AverageDepthErosion_Raw.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.AverageDepthErosion_Thresholded.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.AverageDepthErosion_Error.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.AverageDepthErosion_Percent.ToString(sFormat);

				// Average Depth of Deposition
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Average Depth of Deposition (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ")";
				aRow.Cells[0].ToolTipText = "The average depth of deposition (deposition volume dividied by deposition area)";
				aRow.Cells[1].Value = dodStats.AverageDepthDeposition_Raw.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.AverageDepthDeposition_Thresholded.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.AverageDepthDeposition_Error.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.AverageDepthDeposition_Percent.ToString(sFormat);

				// Average Total Thickness of Difference for AOI
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Average Total Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area of Interest";
				aRow.Cells[0].ToolTipText = "The total volume of difference divided by the area of interest (a measure of total turnover thickness in the analysis area)";
				aRow.Cells[1].Value = dodStats.AverageThicknessOfDifferenceAOI_Raw.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.AverageThicknessOfDifferenceAOI_Thresholded.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.AverageThicknessOfDifferenceAOI_Error.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.AverageThicknessOfDifferenceAOI_Percent.ToString(sFormat);
			}


			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages)) {
				// Average **NET** Thickness of Difference AOI
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Average Net Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area of Interest";
				aRow.Cells[0].ToolTipText = "The total net volume of difference dividied by the area of interest (a measure of resulting net change within the analysis area)";
				aRow.Cells[1].Value = dodStats.AverageNetThicknessofDifferenceAOI_Raw.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[2].Value = dodStats.AverageNetThicknessOfDifferenceAOI_Thresholded.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.AverageNetThicknessOfDifferenceAOI_Error.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.AverageNetThicknessOfDifferenceAOI_Percent.ToString(sFormat);
			}


			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages) {
				// Average Thickness of Difference ADC
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Average Total Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area with Detectable Change";
				aRow.Cells[0].ToolTipText = "The total volume of difference divided by the total area of detectable change (a measure of total turnover thickness where there was detectable change)";
				aRow.Cells[1].Value = "NA";
				aRow.Cells[1].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[2].Value = dodStats.AverageThicknessOfDifferenceADC_Thresholded.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.AverageThicknessOfDifferenceADC_Error.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.AverageThicknessOfDifferenceADC_Percent.ToString(sFormat);
			}


			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages)) {
				// Average **NET** Thickness of Difference ADC
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Average Net Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area with Detectable Change";
				aRow.Cells[0].ToolTipText = "The total net volume of difference dividied by the total area of detectable change (a measure of resulting net change where the was detectable change)";
				aRow.Cells[1].Value = "NA";
				aRow.Cells[1].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[2].Value = dodStats.AverageNetThicknessOfDifferenceADC_Thresholded.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[3].Value = "±";
				aRow.Cells[4].Value = dodStats.AverageNetThicknessOfDifferenceADC_Error.As(options.LinearUnits).ToString(sFormat);
				aRow.Cells[5].Value = dodStats.AverageNetThicknessOfDifferenceADC_Percent.ToString(sFormat);
			}

			if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsPercentages)) {
				///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
				// Percentages by Volume
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "PERCENTAGES (BY VOLUME):";
				cell = aRow.Cells[0];
				cell.Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);

				// Percent Erosion
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Percent Erosion";
				aRow.Cells[0].ToolTipText = "Percent of Total Volume of Difference that is erosional";
				aRow.Cells[1].Value = dodStats.PercentErosion_Raw.ToString(sFormat);
				aRow.Cells[2].Value = dodStats.PercentErosion_Thresholded.ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				// Percent Deposition
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Percent Deposition";
				aRow.Cells[0].ToolTipText = "Percent of Total Volume of Difference that is depositional";
				aRow.Cells[1].Value = dodStats.PercentDeposition_Raw.ToString(sFormat);
				aRow.Cells[2].Value = dodStats.PercentDeposition_Thresholded.ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				// Percent Imbalance
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Percent Imbalance (departure from equilibium)";
				aRow.Cells[0].ToolTipText = "The percent depature from a 50%-50% equilibirum erosion/deposition balance (an normalized indication of the magnitude of the net difference)";
				aRow.Cells[1].Value = dodStats.PercentImbalance_Raw.ToString(sFormat);
				aRow.Cells[2].Value = dodStats.PercentImbalance_Thresholded.ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;

				// Net to Total Volume Ratio
				nIndex = grdData.Rows.Add(1);
				aRow = grdData.Rows[nIndex];
				aRow.Cells[0].Value = "Net to Total Volume Ratio";
				aRow.Cells[0].ToolTipText = "The ratio of net volumetric change divided by total volume of change (a measure of how much the net trend explains of the total turnover)";
				aRow.Cells[1].Value = dodStats.NetToTotalVolumeRatio_Raw.ToString(sFormat);
				aRow.Cells[2].Value = dodStats.NetToTotalVolumeRatio_Thresholded.ToString(sFormat);
				aRow.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
				aRow.Cells[5].Style.BackColor = System.Drawing.Color.LightGray;
			}

		}
		public ucDoDSummary()
		{
			Load += DoDSummaryUC_Load;
			InitializeComponent();
		}

	}

}
