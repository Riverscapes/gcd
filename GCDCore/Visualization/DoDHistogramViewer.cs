using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Drawing;
using GCDConsoleLib;

namespace GCDCore.Visualization
{
    public class DoDHistogramViewer : ViewerBase
    {
        // Raw histogram data
        Histogram _rawHist;
        Histogram _thrHist;

        private Dictionary<decimal, HistogramDisplayData> histoData;

        private readonly GCDConsoleLib.GCD.UnitGroup DataUnits;
        private GCDConsoleLib.GCD.UnitGroup DisplayUnits { get; set; }

        /// <summary>
        /// NOTE: The decimals in here must already be in their display unit
        /// </summary>
        public class HistogramDisplayData
        {
            public decimal Threshold { get; set; }
            public decimal Raw { get; set; }
            public decimal Elevation { get; private set; }

            public decimal Deposition { get { return Elevation > 0 ? Threshold : 0; } }
            public decimal Erosion { get { return Elevation < 0 ? Threshold : 0; } }

            public HistogramDisplayData(decimal elev)
            {
                Elevation = elev;
                Threshold = 0;
            }
        }

        /// <summary>
        /// Call this constructor from non-UI code that simply wants to generate histogram plot image files
        /// </summary>
        public DoDHistogramViewer(Histogram rawHisto, Histogram thrHisto, GCDConsoleLib.GCD.UnitGroup dataUnits, Chart chtControl = null)
            : base(chtControl)
        {
            DataUnits = dataUnits;
            DisplayUnits = dataUnits;

            Dictionary<string, Color> seriesDefs = new Dictionary<string, Color> {
                {  EROSION, Properties.Settings.Default.Erosion },
                {  DEPOSITION, Properties.Settings.Default.Deposition  },
                { RAW, Color.LightGray }
            };

            foreach (KeyValuePair<string, Color> aDef in seriesDefs)
            {
                Series series = Chart.Series.Add(aDef.Key);
                series.ChartType = SeriesChartType.StackedColumn;
                series.Color = aDef.Value;
                series.ChartArea = Chart.ChartAreas.First().Name;
            }

            Axis x = Chart.ChartAreas[0].AxisX;
            x.MajorGrid.LineColor = Color.LightSlateGray;
            x.MinorTickMark.Enabled = false;
            x.ScaleView.Zoomable = true;
            
            Axis y = Chart.ChartAreas[0].AxisY;
            y.MajorGrid.LineColor = Color.LightSlateGray;
            y.MinorTickMark.Enabled = true;
            y.ScaleView.Zoomable = true;

            // Allow the user to swipe across the X axis to zoom
            Chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            UpdateHistograms(rawHisto, thrHisto, true);
        }

        public void UpdateHistograms(Histogram rawHisto, Histogram thrHisto, bool bArea)
        {
            _rawHist = rawHisto;
            _thrHist = thrHisto;

            UpdateDisplay(bArea);
        }

        public void SetChartType(bool bSetArea)
        {
            UpdateDisplay(bSetArea);
        }

        public void UpdateDisplay(bool bArea, GCDConsoleLib.GCD.UnitGroup displayUnits = null)
        {
            // Store the display units so that the user can switch between area and volume easily
            if (displayUnits != null)
                DisplayUnits = displayUnits;

            // Go recalc our values
            GetDisplayValues(bArea);

            Chart.Series.FindByName(EROSION).Points.DataBindXY(histoData.Values, "Elevation", histoData.Values, "Erosion");
            Chart.Series.FindByName(DEPOSITION).Points.DataBindXY(histoData.Values, "Elevation", histoData.Values, "Deposition");
            Chart.Series.FindByName(RAW).Points.DataBindXY(histoData.Values, "Elevation", histoData.Values, "Raw");

            double binWidth = _thrHist.BinWidth(Project.ProjectManager.Project.Units).As(DisplayUnits.VertUnit);

            Axis axisX = Chart.ChartAreas[0].AxisX;
            axisX.Title = string.Format("Elevation Change ({0})", UnitsNet.Length.GetAbbreviation(DisplayUnits.VertUnit));
            axisX.Minimum = _thrHist.BinLower(_thrHist.FirstBinId, Project.ProjectManager.Project.Units).As(DisplayUnits.VertUnit);
            axisX.Maximum = _thrHist.BinLower(_thrHist.LastBinId, Project.ProjectManager.Project.Units).As(DisplayUnits.VertUnit) + binWidth;
            axisX.MajorGrid.Interval = 10 * binWidth;
            axisX.MajorGrid.IntervalOffset = binWidth;
            axisX.Interval = 10 * binWidth;
            axisX.IntervalOffset = binWidth;
            axisX.MinorGrid.Interval = binWidth;

            if (bArea)
                Chart.ChartAreas[0].AxisY.Title = string.Format("Area ({0})", UnitsNet.Area.GetAbbreviation(DisplayUnits.ArUnit));
            else
                Chart.ChartAreas[0].AxisY.Title = string.Format("Volume ({0})", UnitsNet.Volume.GetAbbreviation(DisplayUnits.VolUnit));

            Chart.ChartAreas[0].RecalculateAxesScale();
            Chart.ChartAreas[0].AxisY.RoundAxisValues();
        }

        private void GetDisplayValues(bool bArea)
        {
            // Note that the key to this dictionary is the histogram elevation values in their ORIGINAL units
            // while the elevation properties of the HistogramDisplayDataPoint should be in the display units
            histoData = new Dictionary<decimal, HistogramDisplayData>();

            for (int bid = 0; bid < _thrHist.Count; bid++)
            {
                // Make a dictionary entry if we don't already have one
                decimal bincentre = (decimal)_thrHist.BinCentre(bid, DataUnits).As(DisplayUnits.VertUnit);
                if (!histoData.ContainsKey(bincentre)) histoData[bincentre] = new HistogramDisplayData(bincentre);

                if (bArea)
                    histoData[bincentre].Threshold = (decimal)(_thrHist.BinArea(bid, Project.ProjectManager.Project.CellArea).As(DisplayUnits.ArUnit));
                else
                    histoData[bincentre].Threshold = Math.Abs((decimal)_thrHist.BinVolume(bid, Project.ProjectManager.Project.CellArea, DataUnits).As(DisplayUnits.VolUnit));
            }

            if ((_rawHist != null))
            {
                for (int bid = 0; bid < _rawHist.Count; bid++)
                {
                    // Make a dictionary entry if we don't already have one
                    decimal bincentre = (decimal)_rawHist.BinCentre(bid, DataUnits).As(DisplayUnits.VertUnit);
                    if (!histoData.ContainsKey(bincentre)) histoData[bincentre] = new HistogramDisplayData(bincentre);

                    if (bArea)
                        histoData[bincentre].Raw = (decimal)(_rawHist.BinArea(bid, Project.ProjectManager.Project.CellArea).As(DisplayUnits.ArUnit)) - histoData[bincentre].Threshold;
                    else
                        histoData[bincentre].Raw = Math.Abs((decimal)_rawHist.BinVolume(bid, Project.ProjectManager.Project.CellArea, DataUnits).As(DisplayUnits.VolUnit)) - histoData[bincentre].Threshold;
                }
            }
        }

        public void ExportCharts(FileInfo AreaGraphPath, FileInfo VolumeGraphPath, int ChartWidth, int ChartHeight)
        {
            Chart.Width = ChartWidth;
            Chart.Height = ChartHeight;

            UpdateDisplay(true, DataUnits);
            SaveImage(AreaGraphPath);

            UpdateDisplay(false, DataUnits);
            SaveImage(VolumeGraphPath);
        }
    }
}