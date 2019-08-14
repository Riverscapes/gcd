using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using GCDCore.Project;

namespace GCDCore.Visualization
{
    public class BudgetSegPieChartViewer : ViewerBase
    {
        public readonly List<BudgetSegregationClass> BudgetSegClasses;
        private readonly naru.ui.ChartSeriesColourManager SymbolMan;

        public BudgetSegPieChartViewer(List<BudgetSegregationClass> budgetSegClasses, Chart cht = null)
            : base(cht)
        {
            BudgetSegClasses = budgetSegClasses;
            SymbolMan = new naru.ui.ChartSeriesColourManager();

            Chart.ChartAreas.Clear();
            Chart.ChartAreas.Add("EROSION_AREA");
            Chart.ChartAreas.Add("DEPOSIT_AREA");
            Chart.ChartAreas.Add("EROSION_VOL");
            Chart.ChartAreas.Add("DEPOSIT_VOL");

            // Add the title
            AddChartTitle("EROSION_AREA", "Area of Erosion");
            AddChartTitle("DEPOSIT_AREA", "Area of Deposition");
            AddChartTitle("EROSION_VOL", "Volume of Erosion");
            AddChartTitle("DEPOSIT_VOL", "Volume of Deposition");
        }

        private void AddChartTitle(string chartAreaName, string titleText)
        {
            Title tt = Chart.Titles.Add(chartAreaName);
            tt.Text = titleText;
            tt.DockedToChartArea = chartAreaName;
        }

        public void RefreshPieCharts(GCDConsoleLib.GCD.UnitGroup displayUnits)
        {
            Series seriesErArea = Chart.Series.Add("EROSION_AREA");
            seriesErArea.ChartType = SeriesChartType.Pie;
            seriesErArea.ChartArea = seriesErArea.Name;
            seriesErArea.IsVisibleInLegend = true;
            double sumErosionArea = BudgetSegClasses.Sum<BudgetSegregationClass>(x => x.Statistics.ErosionThr.GetArea(ProjectManager.Project.CellArea).As(displayUnits.ArUnit));
            seriesErArea.Points.DataBindXY(
                BudgetSegClasses.Select(x => x.Name).ToArray<string>(),
                BudgetSegClasses.Select(x => x.Statistics.ErosionThr.GetArea(ProjectManager.Project.CellArea).As(displayUnits.ArUnit) / sumErosionArea).ToArray<double>());

            Series seriesDepArea = Chart.Series.Add("DEPOSIT_AREA");
            seriesDepArea.ChartType = SeriesChartType.Pie;
            seriesDepArea.ChartArea = seriesDepArea.Name;
            double sumDepArea = BudgetSegClasses.Sum<BudgetSegregationClass>(x => x.Statistics.DepositionThr.GetArea(ProjectManager.Project.CellArea).As(displayUnits.ArUnit));
            seriesDepArea.Points.DataBindY(BudgetSegClasses.Select(x => x.Statistics.DepositionThr.GetArea(ProjectManager.Project.CellArea).As(displayUnits.ArUnit) / sumDepArea).ToArray<double>());

            Series seriesErVol = Chart.Series.Add("EROSION_VOL");
            seriesErVol.ChartType = SeriesChartType.Pie;
            seriesErVol.ChartArea = seriesErVol.Name;
            double sumErVol = BudgetSegClasses.Sum<BudgetSegregationClass>(x => x.Statistics.ErosionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units).As(displayUnits.VolUnit));
            seriesErVol.Points.DataBindY(BudgetSegClasses.Select(x => x.Statistics.ErosionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units).As(displayUnits.VolUnit) / sumErVol).ToArray<double>());

            Series seriesDepVol = Chart.Series.Add("DEPOSIT_VOL");
            seriesDepVol.ChartType = SeriesChartType.Pie;
            seriesDepVol.ChartArea = seriesDepVol.Name;
            double sumDepVol = BudgetSegClasses.Sum<BudgetSegregationClass>(x => x.Statistics.DepositionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units).As(displayUnits.VolUnit));
            seriesDepVol.Points.DataBindY(BudgetSegClasses.Select(x => x.Statistics.DepositionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units).As(displayUnits.VolUnit) / sumDepVol).ToArray<double>());
        }
    }
}
