using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDSummary
    {
        private int copyRow;
        private int copyCol;

        private const string colAtt = "colAttribute";
        private const string colRaw = "colRawDoD";
        private const string colThr = "colThresholded";
        private const string colErr = "colError";
        private const string colEPC = "colErrorPC";
        private const string colTot = "colPCTotal";

        private ContextMenuStrip cmsCopy;

        public ucDoDSummary()
        {
            InitializeComponent();
        }

        private void DoDSummaryUC_Load(object sender, System.EventArgs e)
        {
            grdData.AllowUserToAddRows = false;
            grdData.RowHeadersVisible = false;
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdData.MultiSelect = false;
            grdData.AllowUserToOrderColumns = false;
            grdData.AllowUserToResizeRows = false;

            // Numeric columns should be right aligned
            for (int i = 1; i < grdData.Columns.Count; i++)
                grdData.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Create a single copy to clipboard context menu that will be assigned to numeric cells only
            cmsCopy = new ContextMenuStrip();
            cmsCopy.Items.Add("Copy Value To Clipboard", Properties.Resources.Copy, OnCopyCellValue);
            cmsCopy.Items.Add("Copy Row To Clipboard", Properties.Resources.Copy, OnCopyRow);
        }

        /// <summary>
        /// Copy the raw, unformatted cell contents to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyCellValue(object sender, EventArgs e)
        {
            try
            {
                naru.ui.Clipboard.SetText(grdData.Rows[copyRow].Cells[copyCol].Value.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copying value to clipboard" + ex.Message);
            }
        }

        /// <summary>
        /// Copy the raw, unformatted cell contents to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyRow(object sender, EventArgs e)
        {
            try
            {
                List<string> values = new List<string>();
                for (int colIndex = 0; colIndex < grdData.Columns.Count; colIndex++)
                {
                    if (grdData.Rows[copyRow].Cells[colIndex].Value == null)
                        values.Add(string.Empty);
                    else
                        values.Add(grdData.Rows[copyRow].Cells[colIndex].Value.ToString());
                }

                naru.ui.Clipboard.SetText(string.Join(",", values.ToArray()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copying value to clipboard" + ex.Message);
            }
        }

        private string GetFormatString(DoDSummaryDisplayOptions options)
        {
            string sFormat = "#,##0";
            for (int i = 1; i <= options.m_nPrecision; i++)
            {
                if (i == 1)
                {
                    sFormat += ".";
                }
                sFormat += "0";
            }

            return sFormat;
        }

        public void RefreshDisplay(GCDConsoleLib.GCD.DoDStats dodStats, DoDSummaryDisplayOptions options)
        {
            // Hide the % total column in single DoD View
            grdData.Columns[colTot].Visible = false;
            grdData.Rows.Clear();
            DataGridViewRow aRow = null;

            // Build the string formatting based on the precision in the pop-up properties form
            string sFormat = GetFormatString(options);
            UnitsNet.Area ca = ProjectManager.Project.CellArea;
            UnitsNet.Units.LengthUnit vunit = ProjectManager.Project.Units.VertUnit;

            // Now we know the desired format precision we can assign it to all the numeric columns
            for (int i = 1; i < grdData.Columns.Count; i++)
            {
                grdData.Columns[i].DefaultCellStyle.Format = sFormat;

                if (grdData.Columns[i] == colErrorPC || grdData.Columns[i] == colPCTotal)
                {
                    grdData.Columns[i].DefaultCellStyle.Format += "%";
                }
                else if (grdData.Columns[i] == colError)
                {
                    grdData.Columns[i].DefaultCellStyle.Format = "±" + sFormat;
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Show/hide columns based on the properties pop-up
            colRawDoD.Visible = options.m_bColsRaw;
            colThresholded.Visible = options.m_bColsThresholded;
            colError.Visible = options.m_bColsPCError;
            colErrorPC.Visible = options.m_bColsPCError;

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Area of erosion header

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsAreal))
            {
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "AREAL";
                aRow.Cells[colAtt].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);

                // Area of erosion
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Area of Surface Lowering (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The amount of area experiencing a lowering of surface elevations";
                aRow.Cells[colRaw].Value = dodStats.ErosionRaw.GetArea(ca).As(options.AreaUnits);
                aRow.Cells[colThr].Value = dodStats.ErosionThr.GetArea(ca).As(options.AreaUnits);
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;

                // Area of deposition
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Area of Surface Raising (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The amount of area experiencing an increase of surface elevations";
                aRow.Cells[colRaw].Value = dodStats.DepositionRaw.GetArea(ca).As(options.AreaUnits);
                aRow.Cells[colThr].Value = dodStats.DepositionThr.GetArea(ca).As(options.AreaUnits);
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;

                // Area of detectable change
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Area of Detectable Change (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The sum of areas experiencing detectable lowering and raising";
                aRow.Cells[colRaw].Value = "NA";
                aRow.Cells[colThr].Value = dodStats.AreaDetectableChange_Thresholded.As(options.AreaUnits);
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;

                // Area of interest 
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Area of Interest (" + UnitsNet.Area.GetAbbreviation(options.AreaUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The total amount of area under analysis (including detectable and undetectable)";
                aRow.Cells[colRaw].Value = dodStats.AreaOfInterest_Raw.As(options.AreaUnits);
                aRow.Cells[colThr].Value = "NA";
                aRow.Cells[colThr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;

                // Percent of area of interest with detectable change
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Percent of Area of Interest with Detectable Change";
                aRow.Cells[colAtt].ToolTipText = "The percent of the total area of interest with detectable changes (i.e. either exceeding the minimum level of detection or with a proability greater then the confidence interval chosen by user)";
                aRow.Cells[colRaw].Value = "NA";
                aRow.Cells[colRaw].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colThr].Value = dodStats.AreaPercentAreaInterestWithDetectableChange.ToString(sFormat) + "%";
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // VOLUMETRIC

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVolumetric))
            {
                // Volume of erosion header
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "VOLUMETRIC";
                aRow.Cells[colAtt].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
            }

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVolumetric))
            {
                // Volume of erosion
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Volume of Surface Lowering (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "On a cell-by-cell basis, the DoD surface lowering depth (e.g. erosion, cut or deflation) multiplied by cell area and summed";
                aRow.Cells[colRaw].Value = dodStats.ErosionRaw.GetVolume(ca, vunit).As(options.VolumeUnits);
                aRow.Cells[colThr].Value = dodStats.ErosionThr.GetVolume(ca, vunit).As(options.VolumeUnits);
                aRow.Cells[colErr].Value = dodStats.ErosionErr.GetVolume(ca, vunit).As(options.VolumeUnits);
                aRow.Cells[colEPC].Value = dodStats.VolumeOfErosion_Percent;

                // Volume of deposition
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Volume of Surface Raising (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "On a cell-by-cell basis, the DoD surface raising (e.g. deposition, fill or inflation) depth multiplied by cell area and summed";
                aRow.Cells[colRaw].Value = dodStats.DepositionRaw.GetVolume(ca, vunit).As(options.VolumeUnits);
                aRow.Cells[colThr].Value = dodStats.DepositionThr.GetVolume(ca, vunit).As(options.VolumeUnits);
                aRow.Cells[colErr].Value = dodStats.DepositionErr.GetVolume(ca, vunit).As(options.VolumeUnits);
                aRow.Cells[colEPC].Value = dodStats.VolumeOfDeposition_Percent;

                // Total volume of difference
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Volume of Difference (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The sum of lowering and raising volumes (a measure of total turnover)";
                aRow.Cells[colRaw].Value = dodStats.VolumeOfDifference_Raw.As(options.VolumeUnits);
                aRow.Cells[colThr].Value = dodStats.VolumeOfDifference_Thresholded.As(options.VolumeUnits);
                aRow.Cells[colErr].Value = dodStats.VolumeOfDifference_Error.As(options.VolumeUnits);
                aRow.Cells[colEPC].Value = dodStats.VolumeOfDifference_Percent;
            }


            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVolumetric))
            {
                //Total NET volume difference
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Total Net Volume Difference (" + UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The net difference of erosion and depostion volumes (i.e. deposition minus erosion)";
                aRow.Cells[colRaw].Value = dodStats.NetVolumeOfDifference_Raw.As(options.VolumeUnits);
                aRow.Cells[colThr].Value = dodStats.NetVolumeOfDifference_Thresholded.As(options.VolumeUnits);
                aRow.Cells[colErr].Value = dodStats.NetVolumeOfDifference_Error.As(options.VolumeUnits);
                aRow.Cells[colEPC].Value = dodStats.NetVolumeOfDifference_Percent;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // VERTICAL AVERAGES

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages))
            {
                // Vertical Averages header
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "VERTICAL AVERAGES";
                aRow.Cells[colAtt].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
            }

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages)
            {
                // Average Depth of Erosion
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Average Depth of Surface Lowering (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The average depth of lowering (lowering volume divided by lowering area)";
                aRow.Cells[colRaw].Value = dodStats.AverageDepthErosion_Raw.As(options.LinearUnits);
                aRow.Cells[colThr].Value = dodStats.AverageDepthErosion_Thresholded.As(options.LinearUnits);
                aRow.Cells[colErr].Value = dodStats.AverageDepthErosion_Error.As(options.LinearUnits);
                aRow.Cells[colEPC].Value = dodStats.AverageDepthErosion_Percent;

                // Average Depth of Deposition
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Average Depth of Surface Raising (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ")";
                aRow.Cells[colAtt].ToolTipText = "The average depth of raising (raising volume divided by raising area)";
                aRow.Cells[colRaw].Value = dodStats.AverageDepthDeposition_Raw.As(options.LinearUnits);
                aRow.Cells[colThr].Value = dodStats.AverageDepthDeposition_Thresholded.As(options.LinearUnits);
                aRow.Cells[colErr].Value = dodStats.AverageDepthDeposition_Error.As(options.LinearUnits);
                aRow.Cells[colEPC].Value = dodStats.AverageDepthDeposition_Percent;

                // Average Total Thickness of Difference for AOI
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Average Total Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area of Interest";
                aRow.Cells[colAtt].ToolTipText = "The total volume of difference divided by the area of interest (a measure of total turnover thickness in the analysis area)";
                aRow.Cells[colRaw].Value = dodStats.AverageThicknessOfDifferenceAOI_Raw.As(options.LinearUnits);
                aRow.Cells[colThr].Value = dodStats.AverageThicknessOfDifferenceAOI_Thresholded.As(options.LinearUnits);
                aRow.Cells[colErr].Value = dodStats.AverageThicknessOfDifferenceAOI_Error.As(options.LinearUnits);
                aRow.Cells[colEPC].Value = dodStats.AverageThicknessOfDifferenceAOI_Percent;
            }

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages))
            {
                // Average **NET** Thickness of Difference AOI
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Average Net Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area of Interest";
                aRow.Cells[colAtt].ToolTipText = "The total net volume of difference divided by the area of interest (a measure of resulting net change within the analysis area)";
                aRow.Cells[colRaw].Value = dodStats.AverageNetThicknessofDifferenceAOI_Raw.As(options.LinearUnits);
                aRow.Cells[colThr].Value = dodStats.AverageNetThicknessOfDifferenceAOI_Thresholded.As(options.LinearUnits);
                aRow.Cells[colErr].Value = dodStats.AverageNetThicknessOfDifferenceAOI_Error.As(options.LinearUnits);
                aRow.Cells[colEPC].Value = dodStats.AverageNetThicknessOfDifferenceAOI_Percent;
            }

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages)
            {
                // Average Thickness of Difference ADC
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Average Total Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area with Detectable Change";
                aRow.Cells[colAtt].ToolTipText = "The total volume of difference divided by the total area of detectable change (a measure of total turnover thickness where there was detectable change)";
                aRow.Cells[colRaw].Value = "NA";
                aRow.Cells[colRaw].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colThr].Value = dodStats.AverageThicknessOfDifferenceADC_Thresholded.As(options.LinearUnits);
                aRow.Cells[colErr].Value = dodStats.AverageThicknessOfDifferenceADC_Error.As(options.LinearUnits);
                aRow.Cells[colEPC].Value = dodStats.AverageThicknessOfDifferenceADC_Percent;
            }

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.Normalized || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsVerticalAverages))
            {
                // Average **NET** Thickness of Difference ADC
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Average Net Thickness of Difference (" + UnitsNet.Length.GetAbbreviation(options.LinearUnits) + ") for Area with Detectable Change";
                aRow.Cells[colAtt].ToolTipText = "The total net volume of difference divided by the total area of detectable change (a measure of resulting net change where the was detectable change)";
                aRow.Cells[colRaw].Value = "NA";
                aRow.Cells[colRaw].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colThr].Value = dodStats.AverageNetThicknessOfDifferenceADC_Thresholded.As(options.LinearUnits);
                aRow.Cells[colErr].Value = dodStats.AverageNetThicknessOfDifferenceADC_Error.As(options.LinearUnits);
                aRow.Cells[colEPC].Value = dodStats.AverageNetThicknessOfDifferenceADC_Percent;
            }

            if (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.ShowAll || (options.m_eRowGroups == DoDSummaryDisplayOptions.RowGroups.SpecificGroups && options.m_bRowsPercentages))
            {
                ///'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                // Percentages by Volume
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "PERCENTAGES (BY VOLUME)";
                aRow.Cells[colAtt].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);

                // Percent Erosion
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Percent Elevation Lowering";
                aRow.Cells[colAtt].ToolTipText = "Percent of Total Volume of Difference that is surface lowering";
                aRow.Cells[colRaw].Value = dodStats.PercentErosion_Raw;
                aRow.Cells[colThr].Value = dodStats.PercentErosion_Thresholded;
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colTot].Style.BackColor = System.Drawing.Color.LightGray;

                // Percent Deposition
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Percent Elevation Raising";
                aRow.Cells[colAtt].ToolTipText = "Percent of Total Volume of Difference that is surface raising";
                aRow.Cells[colRaw].Value = dodStats.PercentDeposition_Raw;
                aRow.Cells[colThr].Value = dodStats.PercentDeposition_Thresholded;
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colTot].Style.BackColor = System.Drawing.Color.LightGray;

                // Percent Imbalance
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Percent Imbalance (departure from equilibium)";
                aRow.Cells[colAtt].ToolTipText = "The percent depature from a 50%-50% equilibirum lowering/raising (i.e. erosion/deposition) balance (a normalized indication of the magnitude of the net difference)";
                aRow.Cells[colRaw].Value = dodStats.PercentImbalance_Raw;
                aRow.Cells[colThr].Value = dodStats.PercentImbalance_Thresholded;
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colTot].Style.BackColor = System.Drawing.Color.LightGray;

                // Net to Total Volume Ratio
                aRow = grdData.Rows[grdData.Rows.Add()];
                aRow.Cells[colAtt].Value = "Net to Total Volume Ratio";
                aRow.Cells[colAtt].ToolTipText = "The ratio of net volumetric change divided by total volume of change (a measure of how much the net trend explains of the total turnover)";
                aRow.Cells[colRaw].Value = dodStats.NetToTotalVolumeRatio_Raw;
                aRow.Cells[colThr].Value = dodStats.NetToTotalVolumeRatio_Thresholded;
                aRow.Cells[colErr].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colEPC].Style.BackColor = System.Drawing.Color.LightGray;
                aRow.Cells[colTot].Style.BackColor = System.Drawing.Color.LightGray;
            }

            for (int rowIndex = 1; rowIndex < grdData.Rows.Count; rowIndex++)
            {
                for (int colIndex = 1; colIndex < grdData.Columns.Count; colIndex++)
                {
                    //if (rowIndex == 0)
                    //{
                    //    grdData.Rows[rowIndex].Cells[colIndex].Style.Font = new System.Drawing.Font(grdData.Font, System.Drawing.FontStyle.Bold);
                    //    grdData.Rows[rowIndex].Cells[colIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //}
                    //else
                    //{
                    if (grdData.Rows[rowIndex].Cells[colIndex].Value != null)
                    {
                        // Assign the copy to clipboard context menu to non-null cells
                        grdData.Rows[rowIndex].Cells[colIndex].ContextMenuStrip = cmsCopy;
                    }
                    //}
                }
            }
        }

        public void RefreshDisplay(GCDConsoleLib.GCD.DoDStats activeStats, List<GCDConsoleLib.GCD.DoDStats> allStats, UserInterface.BudgetSegregation.frmBudgetSegResults.RawRepresents raw, DoDSummaryDisplayOptions options)
        {
            // Load the base statistics for this class
            RefreshDisplay(activeStats, options);

            // Previous call will hide col 6. Make it visible for bs view
            grdData.Columns[colTot].Visible = true;

            // Build the string formatting based on the precision in the pop-up properties form
            string sFormat = GetFormatString(options);

            UnitsNet.Area ca = ProjectManager.Project.CellArea;
            UnitsNet.Units.LengthUnit lu = ProjectManager.Project.Units.VertUnit;

            // AREAL

            // Erosion
            double classErosionArea = activeStats.ErosionThr.GetArea(ca).As(options.AreaUnits);
            double totalErosionArea = classErosionArea;
            switch (raw)
            {
                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawDoDAreaOfInterest:
                    totalErosionArea = allStats.Sum(x => x.ErosionRaw.GetArea(ca).As(options.AreaUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.ThrDoDAreaDetectableChange:
                    totalErosionArea = allStats.Sum(x => x.ErosionThr.GetArea(ca).As(options.AreaUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawClassAreaOfInterest:
                    totalErosionArea = activeStats.ErosionRaw.GetArea(ca).As(options.AreaUnits);
                    break;
            }
            grdData.Rows[2].Cells[colTot].Value = SafePercent(classErosionArea, totalErosionArea).ToString(sFormat);

            // Deposition
            double classDepositionArea = activeStats.DepositionThr.GetArea(ca).As(options.AreaUnits);
            double totalDepositionArea = classDepositionArea;
            switch (raw)
            {
                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawDoDAreaOfInterest:
                    totalDepositionArea = allStats.Sum(x => x.DepositionRaw.GetArea(ca).As(options.AreaUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.ThrDoDAreaDetectableChange:
                    totalDepositionArea = allStats.Sum(x => x.DepositionThr.GetArea(ca).As(options.AreaUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawClassAreaOfInterest:
                    totalDepositionArea = activeStats.DepositionRaw.GetArea(ca).As(options.AreaUnits);
                    break;
            }
            grdData.Rows[3].Cells[colTot].Value = SafePercent(classDepositionArea, totalDepositionArea).ToString(sFormat);

            // VOLUME

            // Erosion
            double classErosionVol = activeStats.ErosionThr.GetVolume(ca, lu).As(options.VolumeUnits);
            double totalErosionVol = classErosionVol;
            switch (raw)
            {
                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawDoDAreaOfInterest:
                    totalErosionVol = allStats.Sum(x => x.ErosionRaw.GetVolume(ca, lu).As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.ThrDoDAreaDetectableChange:
                    totalErosionVol = allStats.Sum(x => x.ErosionThr.GetVolume(ca, lu).As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawClassAreaOfInterest:
                    totalErosionVol = activeStats.ErosionRaw.GetVolume(ca, lu).As(options.VolumeUnits);
                    break;
            }
            grdData.Rows[8].Cells[colTot].Value = SafePercent(classErosionVol, totalErosionVol).ToString(sFormat);

            // Deposition
            double classDepositionVol = activeStats.DepositionThr.GetArea(ca).As(options.AreaUnits);
            double totalDepositionVol = classDepositionVol;
            switch (raw)
            {
                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawDoDAreaOfInterest:
                    totalDepositionVol = allStats.Sum(x => x.DepositionRaw.GetVolume(ca, lu).As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.ThrDoDAreaDetectableChange:
                    totalDepositionVol = allStats.Sum(x => x.DepositionThr.GetVolume(ca, lu).As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawClassAreaOfInterest:
                    totalDepositionVol = activeStats.DepositionRaw.GetVolume(ca, lu).As(options.VolumeUnits);
                    break;
            }
            grdData.Rows[9].Cells[colTot].Value = SafePercent(classDepositionArea, totalDepositionVol).ToString(sFormat);

            // Vol of Difference
            double classVolDiff = activeStats.VolumeOfDifference_Thresholded.As(options.VolumeUnits);
            double totalVolDiff = classVolDiff;
            switch (raw)
            {
                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawDoDAreaOfInterest:
                    totalVolDiff = allStats.Sum(x => x.VolumeOfDifference_Raw.As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.ThrDoDAreaDetectableChange:
                    totalVolDiff = allStats.Sum(x => x.VolumeOfDifference_Thresholded.As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawClassAreaOfInterest:
                    totalVolDiff = activeStats.VolumeOfDifference_Raw.As(options.VolumeUnits);
                    break;
            }
            grdData.Rows[10].Cells[colTot].Value = SafePercent(classVolDiff, totalVolDiff).ToString(sFormat);

            // Net Vol of Difference
            double classNetVolDiff = activeStats.NetVolumeOfDifference_Thresholded.As(options.VolumeUnits);
            double totalNetVolDiff = classNetVolDiff;
            switch (raw)
            {
                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawDoDAreaOfInterest:
                    totalNetVolDiff = allStats.Sum(x => x.NetVolumeOfDifference_Thresholded.As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.ThrDoDAreaDetectableChange:
                    totalNetVolDiff = allStats.Sum(x => x.NetVolumeOfDifference_Thresholded.As(options.VolumeUnits));
                    break;

                case BudgetSegregation.frmBudgetSegResults.RawRepresents.RawClassAreaOfInterest:
                    totalNetVolDiff = activeStats.NetVolumeOfDifference_Thresholded.As(options.VolumeUnits);
                    break;
            }
            grdData.Rows[11].Cells[colTot].Value = SafePercent(classNetVolDiff, totalNetVolDiff).ToString(sFormat);
        }

        private double SafePercent(double numerator, double denomenator)
        {
            if (numerator != 0 && denomenator != 0)
            {
                return 100 * numerator / denomenator;
            }
            else
                return 0;
        }

        private void grdData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (grdData.Rows[e.RowIndex].Cells[e.ColumnIndex].ContextMenuStrip == cmsCopy)
                {
                    grdData.Rows[e.RowIndex].Selected = true;
                    copyRow = e.RowIndex;
                    copyCol = e.ColumnIndex;
                }
            }
        }
    }
}
