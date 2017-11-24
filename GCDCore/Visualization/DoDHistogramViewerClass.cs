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
        private Dictionary<double, ElevationChangeDataPoint> m_Raw;
        private Dictionary<double, ElevationChangeDataPoint> m_Thresholded;

        private readonly GCDConsoleLib.GCD.UnitGroup DataUnits;
        private GCDConsoleLib.GCD.UnitGroup DisplayUnits { get; set; }

        private int m_nHistogramBins;

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
                {
                    EROSION, Properties.Settings.Default.Erosion
                },
                {
                    DEPOSITION, Properties.Settings.Default.Deposition
                },
                {
                    RAW, Color.LightGray
                }
            };
          

            foreach (KeyValuePair<string, System.Drawing.Color> aDef in seriesDefs)
            {
                Series series = m_Chart.Series.Add(aDef.Key);
                series.ChartType = SeriesChartType.StackedColumn;
                series.Color = aDef.Value;
                series.ChartArea = m_Chart.ChartAreas.First().Name;
            }

            var _with1 = m_Chart.ChartAreas[0].AxisX;
            _with1.MajorGrid.LineColor = System.Drawing.Color.LightSlateGray;
            _with1.MinorTickMark.Enabled = true;

            var _with2 = m_Chart.ChartAreas[0].AxisY;
            _with2.MajorGrid.LineColor = Color.LightSlateGray;
            _with2.MinorTickMark.Enabled = true;

            m_Raw = new Dictionary<double, ElevationChangeDataPoint>();
            m_Thresholded = new Dictionary<double, ElevationChangeDataPoint>();

            UpdateDisplay(true, DataUnits);
        }

        public void SetChartType(bool bArea)
        {
            UpdateDisplay(bArea, DisplayUnits);
        }

        public void UpdateDisplay(bool bArea, GCDConsoleLib.GCD.UnitGroup displayUnits)
        {
            // Store the display units so that the user can switch between area and volume easily
            DisplayUnits = displayUnits;

            List<HistogramDisplayData> histoData = GetDisplayValues(bArea);

            m_Chart.Series.FindByName(EROSION).Points.DataBindXY(histoData, "elevation", histoData, "erosion");
            m_Chart.Series.FindByName(DEPOSITION).Points.DataBindXY(histoData, "elevation", histoData, "deposition");
            m_Chart.Series.FindByName(RAW).Points.DataBindXY(histoData, "elevation", histoData, "raw");

            var _with3 = m_Chart.ChartAreas[0];
            _with3.AxisX.Title = string.Format("Elevation Change ({0})", DisplayUnits.VertUnit);

            if (bArea)
            {
                _with3.AxisY.Title = string.Format("Area ({0}²)", UnitsNet.Area.GetAbbreviation(DisplayUnits.ArUnit));
            }
            else
            {
                _with3.AxisY.Title = string.Format("Volume ({0}³)", UnitsNet.Volume.GetAbbreviation(DisplayUnits.VolUnit));
            }
        }

        private List<HistogramDisplayData> GetDisplayValues(bool bArea)
        {
            // Note that the key to this dictionary is the histogram elevation values in their ORIGINAL units
            // while the elevation properties of the HistogramDisplayDataPoint should be in the display units
            Dictionary<double, HistogramDisplayData> lDisplayData = new Dictionary<double, HistogramDisplayData>();

            // There must always be a thresholded histogram that is displayed red/blue
            foreach (ElevationChangeDataPoint dataPoint in m_Thresholded.Values)
            {
                lDisplayData[dataPoint.Elevation] = new HistogramDisplayData(dataPoint.GetElevation(DataUnits.VertUnit, DisplayUnits.VertUnit));

                if (bArea)
                {
                    lDisplayData[dataPoint.Elevation].Erosion = dataPoint.AreaErosion(DataUnits.ArUnit, DisplayUnits.ArUnit);
                    lDisplayData[dataPoint.Elevation].Deposition = dataPoint.AreaDeposition(DataUnits.ArUnit, DisplayUnits.ArUnit);
                }
                else
                {
                    lDisplayData[dataPoint.Elevation].Erosion = dataPoint.VolumeErosion(DataUnits.VolUnit, DisplayUnits.VolUnit);
                    lDisplayData[dataPoint.Elevation].Deposition = dataPoint.VolumeDeposition(DataUnits.VolUnit, DisplayUnits.VolUnit);
                }
            }

            // If there's a raw histogram then load the values. These will be displayed as a grey column
            if ((m_Raw != null))
            {
                foreach (KeyValuePair<double, ElevationChangeDataPoint> item in m_Raw)
                {
                    if (!lDisplayData.ContainsKey(item.Key))
                    {
                        lDisplayData[item.Key] = new HistogramDisplayData(UnitsNet.Length.From(item.Value.Elevation, DataUnits.VertUnit).As(DisplayUnits.VertUnit));
                    }

                    if (item.Key < 0)
                    {
                        if (bArea)
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.AreaChange(DataUnits.ArUnit, DisplayUnits.ArUnit) - lDisplayData[item.Key].Erosion);
                        }
                        else
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.VolumeChange(DataUnits.VolUnit, DisplayUnits.VolUnit) - lDisplayData[item.Key].Erosion);
                        }
                    }
                    else
                    {
                        if (bArea)
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.AreaChange(DataUnits.ArUnit, DisplayUnits.ArUnit) - lDisplayData[item.Key].Deposition);
                        }
                        else
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.VolumeChange(DataUnits.VolUnit, DisplayUnits.VolUnit) - lDisplayData[item.Key].Deposition);
                        }
                    }
                }
            }

            return lDisplayData.Values.ToList();
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

            UpdateDisplay(true, DataUnits);
            m_Chart.SaveImage(AreaGraphPath, ChartImageFormat.Png);

            UpdateDisplay(false, DataUnits);
            m_Chart.SaveImage(VolumeGraphPath, ChartImageFormat.Png);
        }
    }
}