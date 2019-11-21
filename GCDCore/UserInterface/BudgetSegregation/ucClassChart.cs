using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GCDCore.Project;
using UnitsNet;
using GCDCore.UserInterface.ChangeDetection;

namespace GCDCore.UserInterface.BudgetSegregation
{
    public partial class ucClassChart : UserControl
    {
        private const string EROSION_CHART_SERIES = "Erosion";
        private const string DEPOSIT_CHART_SERIES = "Deposition";
        private const string VOLOUT__CHART_SERIES = "VolOut";
        private const string ERROR___CHART_SERIES = "Error";

        UserInterface.UtilityForms.ChartContextMenu cmsChart;

        public ucClassChart()
        {
            InitializeComponent();

            // Initialize chart here. Remember that if this user control is on a tab in a tab control
            // then the load doesn't occur until the host tab is viewed, which might be after UpdateChart()
            InitializeChart();
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
            serDeposit.Color = ProjectManager.ColorDeposition;
            serDeposit.ChartType = SeriesChartType.StackedColumn;

            Series serErosion = chtData.Series.Add(EROSION_CHART_SERIES);
            serErosion.LegendText = "Erosion";
            serErosion.ChartArea = chtArea1.Name;
            serErosion.Color = ProjectManager.ColorErosion;
            serErosion.ChartType = SeriesChartType.StackedColumn;

            // Error bars should always be last to ensure they draw on top
            Series serErosionErr = chtData.Series.Add(ERROR___CHART_SERIES);
            serErosionErr.IsVisibleInLegend = false;
            serErosionErr.ChartType = SeriesChartType.ErrorBar;
            serErosionErr.ChartArea = chtArea1.Name;
            serErosionErr.Color = Color.Black;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="units"></param>
        /// <param name="volUnit"></param>
        /// <remarks>remember to filter out morphological totals</remarks>
        public void UpdateChart(System.IO.DirectoryInfo outputDir, IEnumerable<GCDCore.Project.Morphological.IBudgetGraphicalResults> units, UnitsNet.Units.VolumeUnit volUnit, bool bBudgetSeg, bool directional)
        {
            cmsChart = new UtilityForms.ChartContextMenu(outputDir, "morphological");
            chtData.ContextMenuStrip = cmsChart.CMS;

            // Update the Y axis volume units
            chtData.ChartAreas.ToList<ChartArea>().ForEach(x => x.AxisY.Title = string.Format("Volume ({0})", Volume.GetAbbreviation(volUnit)));

            // Clear all series data points
            chtData.Series.ToList<Series>().ForEach(x => x.Points.Clear());

            // Hide the second chart area if this is a non-directional budget seg
            chtData.ChartAreas[1].Visible = !bBudgetSeg || (bBudgetSeg && directional);

            CustomLabelsCollection xAxisLabels = chtData.ChartAreas[0].AxisX.CustomLabels;
            double maxY = 0;
            double cumVolume = 0;

            foreach (GCDCore.Project.Morphological.IBudgetGraphicalResults unit in units)
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

                cumVolume += unit.VolDeposition.As(volUnit) - unit.VolErosion.As(volUnit);

                if (!bBudgetSeg)
                {
                    chtData.Series[VOLOUT__CHART_SERIES].Points.AddXY(unit.Name, unit.SecondGraphValue.As(volUnit));
                }
                else
                {
                    cumVolume += unit.VolDeposition.As(volUnit) - unit.VolErosion.As(volUnit);
                    chtData.Series[VOLOUT__CHART_SERIES].Points.AddXY(unit.Name, cumVolume);
                }
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

            // The data displayed on the second chart depends if this is being used for Morphological or Bs
            chtData.Series[VOLOUT__CHART_SERIES].LegendText = !bBudgetSeg ? "Volume Out" : "Cumulative Volume Change";
            chtData.Series[VOLOUT__CHART_SERIES].IsVisibleInLegend = !bBudgetSeg || directional;
        }

        public void SetChartOptions(DoDSummaryDisplayOptions option)
        {
            chtData.Series[DEPOSIT_CHART_SERIES].Color = ProjectManager.ColorDeposition;
            chtData.Series[EROSION_CHART_SERIES].Color = ProjectManager.ColorErosion;

            chtData.Legends.ToList<Legend>().ForEach(x => x.Font = option.Font);

            foreach (ChartArea area in chtData.ChartAreas)
            {
                area.AxisX.TitleFont = option.Font;
                area.AxisX.LabelStyle.Font = option.Font;
                area.AxisY.TitleFont = option.Font;
                area.AxisY.LabelStyle.Font = option.Font;
            }
        }
    }
}
