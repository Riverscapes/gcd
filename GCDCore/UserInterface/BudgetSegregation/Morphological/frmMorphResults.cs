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

namespace GCDCore.UserInterface.BudgetSegregation.Morphological
{
    public partial class frmMorphResults : Form
    {
        public readonly GCDCore.Project.Morphological.MorphologicalAnalysis Analysis;

        public frmMorphResults(GCDCore.Project.Morphological.MorphologicalAnalysis ma)
        {
            InitializeComponent();

            Analysis = ma;
        }

        private void frmMorphResults_Load(object sender, EventArgs e)
        {
            foreach (UnitsNet.Units.DurationUnit val in Enum.GetValues(typeof(UnitsNet.Units.DurationUnit)))
                cboDuration.Items.Add(val);

            foreach (UnitsNet.Units.VolumeUnit val in Enum.GetValues(typeof(UnitsNet.Units.VolumeUnit)))
                cboVolumeUnits.Items.Add(val);
            cboVolumeUnits.SelectedItem = ProjectManager.Project.Units.VolUnit;

            cboDuration.SelectedItem = Analysis.DurationDisplayUnits;
            valDuration.Value = (decimal)Analysis.Duration.As(Analysis.DurationDisplayUnits);
            valPorosity.Value = Analysis.Porosity;
            valDensity.Value = Analysis.Density;
            valMinFlux.Value = (decimal)Analysis.MinimumFlux.As(ProjectManager.Project.Units.VolUnit);

            txtName.Text = Analysis.Name;
            txtPath.Text = ProjectManager.Project.GetRelativePath(Analysis.OutputFolder.FullName);

            grdData.AutoGenerateColumns = false;
            grdData.DataSource = Analysis.Units;


            cboMinFluxUnit.DataSource = new BindingList<GCDCore.Project.Morphological.MorphologicalUnit>(Analysis.Units);
            cboMinFluxUnit.SelectedItem = Analysis.MinimumFluxCell;

            // Bold the last row for the totals
            grdData.Rows[grdData.Rows.Count - 1].DefaultCellStyle.Font = new Font(grdData.Font, FontStyle.Bold);

            UpdateChart();
        }

        private void cboBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Analysis == null)
            //{
            //    GCDCore.Project.BudgetSegregation bs = cboBS.SelectedItem as GCDCore.Project.BudgetSegregation;
            //    txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetMorphologicalDirectory(bs.Folder, false).FullName);
            //}
            //else
            //{
            //    txtPath.Text = ProjectManager.Project.GetRelativePath(Analysis.OutputFolder);
            //}
        }

        private void cboDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal newValue = (decimal)UnitsNet.Duration.From((double)valDuration.Value, Analysis.DurationDisplayUnits).As((UnitsNet.Units.DurationUnit)cboDuration.SelectedItem);
            valDuration.Maximum = Math.Max(valDuration.Maximum, newValue);
            valDuration.Value = newValue;

            Analysis.DurationDisplayUnits = (UnitsNet.Units.DurationUnit)cboDuration.SelectedItem;

            UpdateCriticalDuration();
        }

        private void valDuration_ValueChanged(object sender, EventArgs e)
        {
            Analysis.Duration = UnitsNet.Duration.From((double)valDuration.Value, (UnitsNet.Units.DurationUnit)cboDuration.SelectedItem);
            UpdateCriticalDuration();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            UpdateCriticalDuration();
        }

        private void UpdateCriticalDuration()
        {
            decimal value = 0;
            if (valPercentCompetent.Value > 0)
                value = valDuration.Value * valPercentCompetent.Value;

            txtCriticalDuration.Text = string.Format("{0:F} {1}{2}", value, cboDuration.SelectedItem.ToString(), value > 0 ? "s" : "");
        }

        private void UpdateChart()
        {
            UnitsNet.Units.VolumeUnit volUnit = ProjectManager.Project.Units.VolUnit;

            chtData.Series.Clear();
            chtData.ChartAreas.Clear();

            ChartArea chtArea1 = chtData.ChartAreas.Add("Volume");
            chtArea1.AxisX.Title = "Sub Reach";
            chtArea1.AxisY.Title = string.Format("Volume ({0})", Volume.GetAbbreviation(volUnit));
            chtArea1.AxisX.MajorGrid.Enabled = false;
            chtArea1.AxisY.MajorGrid.LineColor = Color.LightSlateGray;
            chtArea1.AxisY.MinorGrid.Interval = chtArea1.AxisY.MajorGrid.Interval / 5;
            chtArea1.AxisY.MinorGrid.LineColor = Color.LightGray;
            chtArea1.AxisY.MinorGrid.Enabled = true;
            chtArea1.AxisY.MinorTickMark.Enabled = true;
            chtArea1.AxisY.MinorTickMark.LineColor = chtArea1.AxisY.MinorGrid.LineColor;

            ChartArea chtArea2 = chtData.ChartAreas.Add("Out");
            chtArea2.AxisX.Title = chtArea1.AxisX.Title;
            chtArea2.AxisY.Title = chtArea1.AxisY.Title;
            chtArea2.AxisX.MajorGrid.Enabled = false;
            chtArea1.AxisY.MajorGrid.LineColor = chtArea1.AxisY.MajorGrid.LineColor;


            Series serErosion = chtData.Series.Add("Erosion");
            serErosion.LegendText = "Surface Lowering";
            serErosion.ChartArea = chtArea1.Name;
            serErosion.Color = Properties.Settings.Default.Erosion;

            Series serErosionErr = chtData.Series.Add("Erosion Error");
            serErosionErr.IsVisibleInLegend = false;
            serErosionErr.ChartType = SeriesChartType.ErrorBar;
            serErosionErr.ChartArea = chtArea1.Name;

            Series serDeposit = chtData.Series.Add("Depsotion");
            serDeposit.LegendText = "Surface Raising";
            serDeposit.ChartArea = chtArea1.Name;
            serDeposit.Color = Properties.Settings.Default.Deposition;




            Series serVolOut = chtData.Series.Add("VolOut");
            serVolOut.LegendText = "Volume Out";
            serVolOut.ChartArea = chtArea2.Name;


            foreach (GCDCore.Project.Morphological.MorphologicalUnit unit in Analysis.Units.Where(x => !x.IsTotal))
            {
                serErosion.Points.AddXY(unit.Name, unit.VolErosion.As(volUnit));

                serErosionErr.Points.AddXY(unit.Name, new object[] { unit.VolErosion.As(volUnit), (unit.VolErosion - unit.VolErosionErr).As(volUnit), (unit.VolErosion + unit.VolErosionErr).As(volUnit) });

                serDeposit.Points.AddXY(unit.Name, unit.VolDeposition.As(volUnit));
                serVolOut.Points.AddXY(unit.Name, unit.VolOut.As(volUnit));
            }

        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = ProjectManager.Project.GetAbsoluteDir(txtPath.Text);
            if (dir.Exists)
                System.Diagnostics.Process.Start(dir.FullName);
        }
    }
}
