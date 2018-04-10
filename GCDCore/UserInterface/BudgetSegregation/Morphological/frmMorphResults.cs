using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using UnitsNet;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;

namespace GCDCore.UserInterface.BudgetSegregation.Morphological
{
    public partial class frmMorphResults : Form, IProjectItemForm
    {
        private const string EROSION_CHART_SERIES = "Erosion";
        private const string DEPOSIT_CHART_SERIES = "Deposition";
        private const string VOLOUT__CHART_SERIES = "VolOut";
        private const string ERROR___CHART_SERIES = "Error";

        private Color colDoD = Color.FromArgb(255, 0, 255);
        private Color colVIn = Color.FromArgb(180, 95, 6);
        private Color colVOu = Color.FromArgb(56, 118, 29);
        private Color colDep = Color.FromArgb(0, 0, 255);
        private Color colErr = Color.FromArgb(255, 0, 0);

        UserInterface.UtilityForms.ChartContextMenu cmsChart;

        public readonly GCDCore.Project.Morphological.MorphologicalAnalysis Analysis;

        public GCDProjectItem GCDProjectItem { get { return Analysis; } }

 
        public frmMorphResults(GCDCore.Project.Morphological.MorphologicalAnalysis ma)
        {
            InitializeComponent();
            Analysis = ma;
 

            cmsChart = new UtilityForms.ChartContextMenu(Analysis.OutputFolder, "morphological");
            chtData.ContextMenuStrip = cmsChart.CMS;

        }

        private void frmMorphResults_Load(object sender, EventArgs e)
        {
            InitializeChart();

            cmdOK.Text = Properties.Resources.UpdateButtonText;

            ucDoDPropertiesGrid1.Initialize(Analysis.BS.DoD);
            ucDoDPropertiesGrid1.AddDoDProperty("Budget Segregation", Analysis.BS.Name);
            ucDoDPropertiesGrid1.AddDoDProperty("Directional Mask", Analysis.BS.Mask.Name);
            ucDoDPropertiesGrid1.AddDoDProperty("Directional Mask Field", Analysis.BS.Mask._Field);
            ucDoDPropertiesGrid1.AddDoDProperty("Mask Type", Analysis.BS.Mask.Noun);

            cboBoundaryType.Items.Add(GCDCore.Project.Morphological.MorphologicalAnalysis.FluxDirection.Input);
            cboBoundaryType.Items.Add(GCDCore.Project.Morphological.MorphologicalAnalysis.FluxDirection.Output);
            cboBoundaryType.SelectedIndex = 1;

            foreach (UnitsNet.Units.DurationUnit val in Enum.GetValues(typeof(UnitsNet.Units.DurationUnit)))
                cboDuration.Items.Add(val);

            cboDuration.SelectedItem = Analysis.DurationDataUnits;
            valDuration.Value = (decimal)Analysis.Duration.As(Analysis.DurationDataUnits);
            valPorosity.Value = Analysis.Porosity;
            valDensity.Value = Analysis.Density;
            valBoundaryFlux.Value = (decimal)Analysis.BoundaryFlux.As(ProjectManager.Project.Units.VolUnit);

            txtName.Text = Analysis.Name;
            txtPath.Text = ProjectManager.Project.GetRelativePath(Analysis.OutputFolder.FullName);

            grdData.AutoGenerateColumns = false;
            grdData.DataSource = Analysis.Units;

            cboBoundaryUnits.SelectedIndexChanged += UnitsChanged;
            foreach (UnitsNet.Units.VolumeUnit val in Enum.GetValues(typeof(UnitsNet.Units.VolumeUnit)))
            {
                int i = cboBoundaryUnits.Items.Add(new FormattedVolumeUnit(val));
                if (val == ProjectManager.Project.Units.VolUnit)
                    cboBoundaryUnits.SelectedIndex = i;
            }

            cboDuration.SelectedIndexChanged += cboDuration_SelectedIndexChanged;
            valDuration.ValueChanged += valDuration_ValueChanged;
            UpdateCriticalDuration();

            cboBoundaryUnit.DataSource = new BindingList<GCDCore.Project.Morphological.MorphologicalUnit>(Analysis.Units.Where(x => !x.IsTotal).ToList());
            cboBoundaryUnit.SelectedItem = Analysis.BoundaryFluxUnit;

            valPorosity.ValueChanged += PorosityChanged;

            // Make the grid the default control
            grdData.Select();

            valBoundaryFlux.ValueChanged += MinFlux_Changed;
            cboBoundaryUnit.SelectedIndexChanged += MinFlux_Changed;
            cboBoundaryType.SelectedIndexChanged += MinFlux_Changed;
            UpdateFormulae();

            grdData.Rows[grdData.Rows.Count - 1].Cells["volChange"].Style.ForeColor = colDoD;
            grdData.Rows[grdData.Rows.Count - 1].Cells["colVolDeposition"].Style.ForeColor = colDep;
            grdData.Rows[grdData.Rows.Count - 1].Cells["colVolErosion"].Style.ForeColor = colErr;
            grdData.Rows[grdData.Rows.Count - 1].Cells["colVolumeIn"].Style.ForeColor = colVIn;
            grdData.Rows[grdData.Rows.Count - 1].Cells["colVolumeOut"].Style.ForeColor = colVOu;
        }

        private void PorosityChanged(object sender, EventArgs e)
        {
            Analysis.Porosity = valPorosity.Value;
        }

        private void DurationChanged(object sender, EventArgs e)
        {
            Analysis.Duration = UnitsNet.Duration.From((double)valDuration.Value, Analysis.DurationDataUnits);
        }

        private void valDensity_ValueChanged(object sender, EventArgs e)
        {
            Analysis.Density = valDensity.Value;
        }

        private void cboDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the existing duration units from the analysis
            UnitsNet.Units.DurationUnit oldUnits = Analysis.DurationDataUnits;

            // Must set this before duration, because setting the duration triggers work calculation that needs correct units
            Analysis.DurationDataUnits = (UnitsNet.Units.DurationUnit)cboDuration.SelectedItem;

            // Changing the duration value on the analysis will trigger work calculation. Make sure duration units are already set.
            decimal newValue = (decimal)UnitsNet.Duration.From((double)valDuration.Value, oldUnits).As((UnitsNet.Units.DurationUnit)cboDuration.SelectedItem);
            valDuration.Maximum = Math.Max(valDuration.Maximum, newValue);
            valDuration.Value = newValue;

            UnitsChanged(sender, e);
            UpdateCriticalDuration();
        }

        private void valDuration_ValueChanged(object sender, EventArgs e)
        {
            Analysis.Duration = UnitsNet.Duration.From((double)valDuration.Value, (UnitsNet.Units.DurationUnit)cboDuration.SelectedItem);
            UpdateCriticalDuration();
        }

        private void PercentCritical_ValueChanged(object sender, EventArgs e)
        {
            Analysis.Competency = valPercentCompetent.Value;
            UpdateCriticalDuration();
        }

        private void UpdateCriticalDuration()
        {
            decimal value = 0;
            if (valPercentCompetent.Value > 0)
                value = valDuration.Value * valPercentCompetent.Value;

            txtCriticalDuration.Text = string.Format("{0:F} {1}{2}", value, cboDuration.SelectedItem.ToString(), value != 1m ? "s" : "");
            UpdateChart();
        }

        private void UnitsChanged(object sender, EventArgs e)
        {
            string abbr = UnitsNet.Volume.GetAbbreviation(((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit);

            lblBoundaryVolume.Text = lblBoundaryVolume.Text.Replace(")", string.Format("{0})", abbr));
            txtMinFlux.Text = Analysis.ReachInputFlux.As(((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit).ToString("#,##0.00");

            // This will cause the analysis to recalculate the flux volume and flux mass
            Analysis.DataVolumeUnits = ((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit;

            foreach (DataGridViewColumn col in grdData.Columns)
            {
                if (string.Compare(col.Name, "colFuxVolume", true) == 0)
                {
                    col.HeaderText = string.Format("{0}({1}/{2})", col.HeaderText.Substring(0, col.HeaderText.IndexOf("(")), abbr,
                       UnitsNet.Duration.GetAbbreviation(((UnitsNet.Units.DurationUnit)cboDuration.SelectedItem)));
                }
                else if (string.Compare(col.Name, "colFluxMass", true) == 0)
                {
                    col.HeaderText = string.Format("{0}({1}/{2})", col.HeaderText.Substring(0, col.HeaderText.IndexOf("(")), UnitsNet.Mass.GetAbbreviation(UnitsNet.Units.MassUnit.Kilogram),
                               UnitsNet.Duration.GetAbbreviation(((UnitsNet.Units.DurationUnit)cboDuration.SelectedItem)));

                }
                else if (col.HeaderText.ToLower().Contains("vol"))
                {
                    col.HeaderText = string.Format("{0}({1})", col.HeaderText.Substring(0, col.HeaderText.IndexOf("(")), abbr);
                }
            }

            Analysis.Units.ResetBindings();
            UpdateChart();
            UpdateFormulae();
        }

        private void InitializeChart()
        {
            chtData.Series.Clear();
            chtData.ChartAreas.Clear();

            ChartArea chtArea1 = chtData.ChartAreas.Add("Volume");
            chtArea1.AxisX.Title = "Sub Reach";
            chtArea1.AxisX.MajorGrid.Enabled = false;
            chtArea1.AxisY.MajorGrid.LineColor = Color.LightSlateGray;
            chtArea1.AxisY.MinorGrid.Interval = chtArea1.AxisY.MajorGrid.Interval / 5;
            chtArea1.AxisY.MinorGrid.LineColor = Color.LightGray;
            chtArea1.AxisY.MinorGrid.Enabled = true;
            chtArea1.AxisY.MinorTickMark.Enabled = true;
            chtArea1.AxisY.MinorTickMark.LineColor = chtArea1.AxisY.MinorGrid.LineColor;
            chtArea1.AxisX.Interval = 3;

            ChartArea chtArea2 = chtData.ChartAreas.Add("Out");
            chtArea2.AxisX.Title = chtArea1.AxisX.Title;
            chtArea2.AxisX.MajorGrid.Enabled = false;
            chtArea1.AxisY.MajorGrid.LineColor = chtArea1.AxisY.MajorGrid.LineColor;
            chtArea2.AxisY.MinorGrid.Interval = chtArea2.AxisY.MajorGrid.Interval / 5;
            chtArea2.AxisY.MinorGrid.LineColor = Color.LightGray;
            chtArea2.AxisY.MinorGrid.Enabled = true;
            chtArea2.AxisY.MinorTickMark.Enabled = true;
            chtArea2.AxisY.MinorTickMark.LineColor = chtArea1.AxisY.MinorGrid.LineColor;

            // Add series in reverse order that they will appear in legend


            Series serVolOut = chtData.Series.Add(VOLOUT__CHART_SERIES);
            serVolOut.LegendText = "Volume Out";
            serVolOut.ChartArea = chtArea2.Name;

            Series serDeposit = chtData.Series.Add(DEPOSIT_CHART_SERIES);
            serDeposit.LegendText = "Deposition";
            serDeposit.ChartArea = chtArea1.Name;
            serDeposit.Color = Properties.Settings.Default.Deposition;
            serDeposit.ChartType = SeriesChartType.StackedColumn;

            Series serErosion = chtData.Series.Add(EROSION_CHART_SERIES);
            serErosion.LegendText = "Erosion";
            serErosion.ChartArea = chtArea1.Name;
            serErosion.Color = Properties.Settings.Default.Erosion;
            serErosion.ChartType = SeriesChartType.StackedColumn;

            // Error bars should always be last to ensure they draw on top
            Series serErosionErr = chtData.Series.Add(ERROR___CHART_SERIES);
            serErosionErr.IsVisibleInLegend = false;
            serErosionErr.ChartType = SeriesChartType.ErrorBar;
            serErosionErr.ChartArea = chtArea1.Name;
            serErosionErr.Color = Color.Black;
        }

        private void UpdateChart()
        {
            // Update the Y axis volume units
            UnitsNet.Units.VolumeUnit volUnit = ((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit;
            chtData.ChartAreas.ToList<ChartArea>().ForEach(x => x.AxisY.Title = string.Format("Volume ({0})", Volume.GetAbbreviation(volUnit)));

            // Clear all series data points
            chtData.Series.ToList<Series>().ForEach(x => x.Points.Clear());

            CustomLabelsCollection xAxisLabels = chtData.ChartAreas[0].AxisX.CustomLabels;
            double maxY = 0;

            foreach (GCDCore.Project.Morphological.MorphologicalUnit unit in Analysis.Units.Where(x => !x.IsTotal))
            {
                double labelStart = chtData.Series[EROSION_CHART_SERIES].Points.Count;
                double labelEnd = labelStart + 3;
                xAxisLabels.Add(new CustomLabel(labelStart, labelEnd, unit.Name, 0, LabelMarkStyle.Box));

                chtData.Series[EROSION_CHART_SERIES].Points.AddY(unit.VolErosion.As(volUnit));
                chtData.Series[EROSION_CHART_SERIES].Points.AddY(0);
                chtData.Series[EROSION_CHART_SERIES].Points.AddY(0);

                chtData.Series[DEPOSIT_CHART_SERIES].Points.AddY(0);
                chtData.Series[DEPOSIT_CHART_SERIES].Points.AddY(unit.VolDeposition.As(volUnit));
                chtData.Series[DEPOSIT_CHART_SERIES].Points.AddY(0);

                chtData.Series[ERROR___CHART_SERIES].Points.AddY(new object[] { unit.VolErosion.As(volUnit), (unit.VolErosion - unit.VolErosionErr).As(volUnit), (unit.VolErosion + unit.VolErosionErr).As(volUnit) });
                chtData.Series[ERROR___CHART_SERIES].Points.AddY(new object[] { unit.VolDeposition.As(volUnit), (unit.VolDeposition - unit.VolDepositionErr).As(volUnit), (unit.VolDeposition + unit.VolDepositionErr).As(volUnit) });
                chtData.Series[ERROR___CHART_SERIES].Points.AddY(new object[] { 0, 0, 0 });

                maxY = Math.Max(maxY, (unit.VolErosion + unit.VolErosionErr).As(volUnit));
                maxY = Math.Max(maxY, (unit.VolDeposition + unit.VolDepositionErr).As(volUnit));

                chtData.Series[VOLOUT__CHART_SERIES].Points.AddXY(unit.Name, unit.VolOut.As(volUnit));
            }

            chtData.ChartAreas[0].AxisY.Maximum = Math.Ceiling(maxY);
            chtData.ChartAreas[0].AxisY.RoundAxisValues();

            foreach (int i in new List<int> { 0, 1 })
            {
                chtData.ChartAreas[i].AxisX.TitleFont = Properties.Settings.Default.ChartFont;
                chtData.ChartAreas[i].AxisX.LabelStyle.Font = Properties.Settings.Default.ChartFont;
                chtData.ChartAreas[i].AxisY.TitleFont = Properties.Settings.Default.ChartFont;
                chtData.ChartAreas[i].AxisY.LabelStyle.Font = Properties.Settings.Default.ChartFont;
            }
            chtData.Legends[0].Font = Properties.Settings.Default.ChartFont;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = ProjectManager.Project.GetAbsoluteDir(txtPath.Text);
            if (dir.Exists)
                System.Diagnostics.Process.Start(dir.FullName);
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;

            string colName = grdData.Columns[e.ColumnIndex].HeaderText.ToLower();

            if (e.Value is UnitsNet.Volume)
            {
                e.Value = ((UnitsNet.Volume)e.Value).As(((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit);
            }

            if (grdData.Rows[e.RowIndex].DataBoundItem is GCDCore.Project.Morphological.MorphologicalUnit)
            {
                if (((GCDCore.Project.Morphological.MorphologicalUnit)grdData.Rows[e.RowIndex].DataBoundItem).IsTotal)
                {
                    grdData.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(grdData.Font, FontStyle.Bold);
                }
            }
        }

        private class FormattedVolumeUnit
        {
            public UnitsNet.Units.VolumeUnit VolumeUnit;

            public override string ToString()
            {
                return Regex.Replace(VolumeUnit.ToString(), "(\\B[A-Z])", " $1");
            }

            public FormattedVolumeUnit(UnitsNet.Units.VolumeUnit vol)
            {
                VolumeUnit = vol;
            }
        }

        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            // The name should be the only property that is not already synchronized with the analysis object
            Analysis.Name = txtName.Text;

            try
            {
                Analysis.SaveExcelSpreadsheet();
            }
            catch (Exception ex)
            {
                ex.Data["Path"] = Analysis.Spreadsheet.FullName;
                naru.error.ExceptionUI.HandleException(ex, "Error saving morphological spreadsheet");
                DialogResult = DialogResult.None;
                return;
            }

            if (!Analysis.BS.MorphologicalAnalyses.ContainsValue(Analysis))
                Analysis.BS.MorphologicalAnalyses[Analysis.Name] = Analysis;

            ProjectManager.Project.Save();
        }

        private bool ValidateForm()
        {
            // Sanity check to avoid empty names
            txtName.Text = txtName.Text.Trim();

            if (!frmMorpProperties.ValidateName(txtName, Analysis.BS, Analysis))
                return false;

            return true;
        }

        private void exportTablularDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog frm = new SaveFileDialog();
            frm.Title = "Save Tabular Data To File";
            frm.Filter = "Comma Separated Value (CSV) Files (*.csv)|*.csv";

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            try
            {

                // Header row
                List<string> values = new List<string>();
                foreach (DataGridViewColumn col in grdData.Columns)
                {
                    values.Add(col.HeaderText.ToString().Replace(",", ""));
                }
                sb.AppendLine(string.Join(",", values));

                // Data rows
                foreach (DataGridViewRow dgvr in grdData.Rows)
                {
                    values = new List<string>();
                    for (int i = 0; i < grdData.Columns.Count; i++)
                    {
                        values.Add(dgvr.Cells[i].FormattedValue.ToString().Replace(",", ""));
                    }

                    sb.AppendLine(string.Join(",", values));
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error serializing data table");
            }

            try
            {
                using (StreamWriter file = new System.IO.StreamWriter(frm.FileName))
                {
                    file.WriteLine(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error Writing Tabular Data To CSV File");
            }

            if (File.Exists(frm.FileName))
            {
                try
                {
                    System.Diagnostics.Process.Start(frm.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("The tabular data file was created at {0} but an error occured attempting to open the file.\n\n{1}", frm.FileName, ex.Message), Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void MinFlux_Changed(object sender, EventArgs e)
        {
            GCDCore.Project.Morphological.MorphologicalUnit unit = cboBoundaryUnit.SelectedItem as GCDCore.Project.Morphological.MorphologicalUnit;
            GCDCore.Project.Morphological.MorphologicalAnalysis.FluxDirection eDir = (GCDCore.Project.Morphological.MorphologicalAnalysis.FluxDirection)cboBoundaryType.SelectedItem;

            Analysis.ImposeBoundaryCondition(eDir, unit, Volume.From((double)valBoundaryFlux.Value, ((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit));
            txtMinFlux.Text = Analysis.ReachInputFlux.As(((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit).ToString("#,##0.00");
            UpdateFormulae();

            //Analysis.Units.ResetBindings();
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            // Disconnect event firing    
            cboBoundaryUnit.SelectedIndexChanged -= MinFlux_Changed;
            valBoundaryFlux.ValueChanged -= MinFlux_Changed;
            cboBoundaryType.SelectedIndexChanged -= MinFlux_Changed;

            Analysis.ImposeMinimumFlux();
            cboBoundaryUnit.SelectedIndex = Analysis.Units.IndexOf(Analysis.BoundaryFluxUnit);
            valBoundaryFlux.Value = (decimal)Analysis.BoundaryFlux.As((cboBoundaryUnits.SelectedItem as FormattedVolumeUnit).VolumeUnit);
            cboBoundaryType.SelectedIndex = 1;

            txtMinFlux.Text = Analysis.ReachInputFlux.As(((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit).ToString("#,##0.00");
            UpdateFormulae();

            // Re-attach event firing
            cboBoundaryUnit.SelectedIndexChanged += MinFlux_Changed;
            valBoundaryFlux.ValueChanged += MinFlux_Changed;
            cboBoundaryType.SelectedIndexChanged += MinFlux_Changed;
        }

        private void UpdateFormulae()
        {
            string sFormat = "#,##0.00";
            GCDCore.Project.Morphological.MorphologicalUnit muTotal = Analysis.Units.First(x => x.IsTotal);
            UnitsNet.Units.VolumeUnit volUnit = ((FormattedVolumeUnit)cboBoundaryUnits.SelectedItem).VolumeUnit;

            SetFormulaTextbox(txtVDoD, muTotal.VolChange.As(volUnit).ToString(sFormat), colDoD);
            SetFormulaTextbox(txtVIn, Analysis.Units[0].VolIn.As(volUnit).ToString(sFormat), colVIn);
            SetFormulaTextbox(txtVOut, muTotal.VolOut.As(volUnit).ToString(sFormat), colVOu);
            SetFormulaTextbox(txtVD, muTotal.VolDeposition.As(volUnit).ToString(sFormat), colDep);
            SetFormulaTextbox(txtVE, muTotal.VolErosion.As(volUnit).ToString(sFormat), colErr);
        }

        private void SetFormulaTextbox(TextBox txt, string text, Color col)
        {
            txt.Text = text;
            txt.BorderStyle = BorderStyle.None;
            txt.ForeColor = col;
            txt.Font = new Font(txtVDoD.Font, FontStyle.Bold);

            foreach(DataGridViewRow dgvr in grdData.Rows)
            {
                GCDCore.Project.Morphological.MorphologicalUnit mu = dgvr.DataBoundItem as GCDCore.Project.Morphological.MorphologicalUnit;
                if (mu.IsTotal)
                    continue;

                dgvr.Cells[colVolumeIn.Index].Style.ForeColor = mu.VolIn.As(UnitsNet.Units.VolumeUnit.CubicMeter) < 0 ? Color.Red : Color.Black;
                dgvr.Cells[colVolumeOut.Index].Style.ForeColor = mu.VolOut.As(UnitsNet.Units.VolumeUnit.CubicMeter) < 0 ? Color.Red : Color.Black;
            }

        }
    }
}
