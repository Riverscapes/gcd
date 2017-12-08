using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Drawing;
using GCDConsoleLib;

namespace GCDCore.Visualization
{
    public class DoDHistogramViewerClass
    {
        // Names for data series
        private const string EROSION = "Erosion";
        private const string DEPOSITION = "Deposition";
        private const string RAW = "Raw";

        private Chart m_Chart;

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
        public DoDHistogramViewerClass(Histogram rawHisto, Histogram thrHisto, GCDConsoleLib.GCD.UnitGroup dataUnits, Chart chtControl = null)
        {
            DataUnits = dataUnits;
            DisplayUnits = dataUnits;

            // Proceed and do the one-time chart preparation
            if (chtControl == null)
                m_Chart = new Chart();
            else
                m_Chart = chtControl;

            m_Chart.ChartAreas.Clear();
            m_Chart.ChartAreas.Add(new ChartArea());

            m_Chart.Series.Clear();
            m_Chart.Palette = ChartColorPalette.None;
            m_Chart.Legends.Clear();

            Dictionary<string, Color> seriesDefs = new Dictionary<string, Color> {
                {  EROSION, Properties.Settings.Default.Erosion },
                {  DEPOSITION, Properties.Settings.Default.Deposition  },
                { RAW, Color.LightGray }
            };

            foreach (KeyValuePair<string, Color> aDef in seriesDefs)
            {
                Series series = m_Chart.Series.Add(aDef.Key);
                series.ChartType = SeriesChartType.StackedColumn;
                series.Color = aDef.Value;
                series.ChartArea = m_Chart.ChartAreas.First().Name;
            }

            Axis x = m_Chart.ChartAreas[0].AxisX;
            x.MajorGrid.LineColor = Color.LightSlateGray;
            x.MinorTickMark.Enabled = false;
 
            Axis y = m_Chart.ChartAreas[0].AxisY;
            y.MajorGrid.LineColor = Color.LightSlateGray;
            y.MinorTickMark.Enabled = true;

            _rawHist = rawHisto;
            _thrHist = thrHisto;

            // Strip line is used to emphasize deposition
            StripLine depositionArea = new StripLine();
            depositionArea.BackColor = Color.FromArgb(10, GCDCore.Properties.Settings.Default.Deposition);
            depositionArea.Interval = 0;
            depositionArea.IntervalOffset = 0;
            depositionArea.StripWidth = 10;
            m_Chart.ChartAreas[0].AxisX.StripLines.Add(depositionArea);

            // Strip line is used to emphasize deposition
            StripLine erosionArea = new StripLine();
            erosionArea.BackColor = Color.FromArgb(10, GCDCore.Properties.Settings.Default.Erosion);
            erosionArea.Interval = 0;
            erosionArea.IntervalOffset = 0;
            erosionArea.StripWidth = 10;
            m_Chart.ChartAreas[0].AxisX.StripLines.Add(erosionArea);
            
            UpdateDisplay(true);
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

            m_Chart.Series.FindByName(EROSION).Points.DataBindXY(histoData.Values, "Elevation", histoData.Values, "Erosion");
            m_Chart.Series.FindByName(DEPOSITION).Points.DataBindXY(histoData.Values, "Elevation", histoData.Values, "Deposition");
            m_Chart.Series.FindByName(RAW).Points.DataBindXY(histoData.Values, "Elevation", histoData.Values, "Raw");

            double binWidth = UnitsNet.Length.From((double)_thrHist.BinWidth, Project.ProjectManager.Project.Units.VertUnit).As(DisplayUnits.VertUnit);

            Axis axisX = m_Chart.ChartAreas[0].AxisX;
            axisX.Title = string.Format("Elevation Change ({0})", UnitsNet.Length.GetAbbreviation(DisplayUnits.VertUnit));
            axisX.Minimum = _thrHist.BinLower(_thrHist.FirstBinId, Project.ProjectManager.Project.Units).As(DisplayUnits.VertUnit);
            axisX.Maximum = _thrHist.BinLower(_thrHist.LastBinId, Project.ProjectManager.Project.Units).As(DisplayUnits.VertUnit) + binWidth;
            axisX.MajorGrid.Interval = 10 * binWidth;
            axisX.MajorGrid.IntervalOffset = binWidth;
            axisX.Interval = 10 * binWidth;
            axisX.IntervalOffset = binWidth;
            axisX.MinorGrid.Interval = binWidth;

            if (bArea)
                m_Chart.ChartAreas[0].AxisY.Title = string.Format("Area ({0})", UnitsNet.Area.GetAbbreviation(DisplayUnits.ArUnit));
            else
                m_Chart.ChartAreas[0].AxisY.Title = string.Format("Volume ({0})", UnitsNet.Volume.GetAbbreviation(DisplayUnits.VolUnit));

            axisX.StripLines[0].StripWidth = axisX.Maximum;
            axisX.StripLines[1].StripWidth = axisX.Maximum;
            axisX.StripLines[1].IntervalOffset = axisX.Minimum;
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

        public void ExportCharts(string AreaGraphPath, string VolumeGraphPath, int ChartWidth, int ChartHeight)
        {
            if (!Directory.Exists(Path.GetDirectoryName(AreaGraphPath)))
            {
                Exception ex = new Exception("The output folder for the GCD area graph does not exist.");
                ex.Data["Area Graph Path"] = AreaGraphPath;
            }

            if (!Directory.Exists(Path.GetDirectoryName(VolumeGraphPath)))
            {
                Exception ex = new Exception("The output folder for the GCD volume graph does not exist.");
                ex.Data["volume Graph Path"] = VolumeGraphPath;
            }

            m_Chart.Width = ChartWidth;
            m_Chart.Height = ChartHeight;

            UpdateDisplay(true, DataUnits);
            m_Chart.SaveImage(AreaGraphPath, ChartImageFormat.Png);

            UpdateDisplay(false, DataUnits);
            m_Chart.SaveImage(VolumeGraphPath, ChartImageFormat.Png);
        }
    }
}