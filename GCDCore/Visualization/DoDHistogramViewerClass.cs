using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using UnitsNet.Units;
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

        private LengthUnit LinearDataUnits { get; set; }
        private AreaUnit AreaDataUnits { get { return GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(LinearDataUnits); } }
        private VolumeUnit VolumeDataUnits { get { return GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(LinearDataUnits); } }

        private LengthUnit LinearDisplayUnits { get; set; }
        private AreaUnit AreaDisplayUnits { get; set; }
        private VolumeUnit VolumeDisplayUnits { get; set; }

        private int m_nHistogramBins;

        /// <summary>
        /// Call this constructor from non-UI code that simply wants to generate histogram plot image files
        /// </summary>
        public DoDHistogramViewerClass(ref Histogram rawHisto, ref Histogram thrHisto, UnitsNet.Units.LengthUnit linearDataUnits)
        {
            m_Chart = new Chart();
            Init(ref rawHisto, ref thrHisto, linearDataUnits);
        }

        /// <summary>
        /// Constructor for UI code to pass in a chart on a user interface form
        /// </summary>
        /// <param name="cht"></param>
        public DoDHistogramViewerClass(ref Chart cht, FileInfo rawDodPath, FileInfo thrDoDPath, LengthUnit linearDataUnits)
        {
            m_Chart = cht;

            GCDConsoleLib.Raster rawDoD = new GCDConsoleLib.Raster(rawDodPath);
            GCDConsoleLib.Raster thrDoD = new GCDConsoleLib.Raster(thrDoDPath);

            Histogram rawHisto = RasterOperators.BinRaster(ref rawDoD, m_nHistogramBins);
            Histogram thrHisto = RasterOperators.BinRaster(ref thrDoD, m_nHistogramBins);
            Init(ref rawHisto, ref thrHisto, linearDataUnits);
        }

        private void Init(ref Histogram rawHisto, ref Histogram thrHiso, LengthUnit linearDataUnits)
        {
            // Load the data for both the raw and thresholded histograms

            throw new NotImplementedException("see next 2 lines");
            //m_Raw = rawHisto;
            //m_Thresholded = thrHiso;
            LinearDataUnits = linearDataUnits;

            LinearDisplayUnits = LinearDataUnits;
            AreaDisplayUnits = AreaDataUnits;
            VolumeDisplayUnits = VolumeDataUnits;

            // Proceed and do the one-time chart preparation
            m_Chart.ChartAreas.Clear();
            m_Chart.ChartAreas.Add(new ChartArea());

            m_Chart.Series.Clear();
            m_Chart.Palette = ChartColorPalette.None;
            m_Chart.Legends.Clear();

            dynamic seriesDefs = new Dictionary<string, System.Drawing.Color> {
                {
                    EROSION,
               Properties.Settings.Default.Erosion
                },
                {
                    DEPOSITION,
                   Properties.Settings.Default.Deposition
                },
                {
                    RAW,
                    Color.LightGray
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

            RefreshDisplay(true, linearDataUnits, GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(linearDataUnits), GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(linearDataUnits));
        }

        public void SetChartType(bool bArea)
        {
            RefreshDisplay(bArea, LinearDisplayUnits, AreaDisplayUnits, VolumeDisplayUnits);
        }

        public void RefreshDisplay(bool bArea, UnitsNet.Units.LengthUnit linearDisplayUnits, AreaUnit areaDisplayUnits, VolumeUnit volumeDisplayUnits)
        {
            // Store the display units so that the user can switch between area and volume easily
            LinearDisplayUnits = linearDisplayUnits;
            AreaDisplayUnits = areaDisplayUnits;
            VolumeDisplayUnits = volumeDisplayUnits;

            List<HistogramDisplayData> histoData = GetDisplayValues(bArea, linearDisplayUnits, areaDisplayUnits, volumeDisplayUnits);

            m_Chart.Series.FindByName(EROSION).Points.DataBindXY(histoData, "elevation", histoData, "erosion");
            m_Chart.Series.FindByName(DEPOSITION).Points.DataBindXY(histoData, "elevation", histoData, "deposition");
            m_Chart.Series.FindByName(RAW).Points.DataBindXY(histoData, "elevation", histoData, "raw");

            var _with3 = m_Chart.ChartAreas[0];
            _with3.AxisX.Title = string.Format("Elevation Change ({0})", linearDisplayUnits);

            if (bArea)
            {
                _with3.AxisY.Title = string.Format("Area ({0}²)", UnitsNet.Area.GetAbbreviation(areaDisplayUnits));
            }
            else
            {
                _with3.AxisY.Title = string.Format("Volume ({0}³)", UnitsNet.Volume.GetAbbreviation(volumeDisplayUnits));
            }
        }

        private List<HistogramDisplayData> GetDisplayValues(bool bArea, LengthUnit linearDisplayUnits, AreaUnit areaDisplayUnits, VolumeUnit volDisplayUnits)
        {
            // Note that the key to this dictionary is the histogram elevation values in their ORIGINAL units
            // while the elevation properties of the HistogramDisplayDataPoint should be in the display units
            Dictionary<double, HistogramDisplayData> lDisplayData = new Dictionary<double, HistogramDisplayData>();

            // There must always be a thresholded histogram that is displayed red/blue
            foreach (ElevationChangeDataPoint dataPoint in m_Thresholded.Values)
            {
                lDisplayData[dataPoint.Elevation] = new HistogramDisplayData(dataPoint.GetElevation(LinearDataUnits, linearDisplayUnits));

                if (bArea)
                {
                    lDisplayData[dataPoint.Elevation].Erosion = dataPoint.AreaErosion(AreaDataUnits, areaDisplayUnits);
                    lDisplayData[dataPoint.Elevation].Deposition = dataPoint.AreaDeposition(AreaDataUnits, areaDisplayUnits);
                }
                else
                {
                    lDisplayData[dataPoint.Elevation].Erosion = dataPoint.VolumeErosion(VolumeDataUnits, volDisplayUnits);
                    lDisplayData[dataPoint.Elevation].Deposition = dataPoint.VolumeDeposition(VolumeDataUnits, volDisplayUnits);
                }
            }

            // If there's a raw histogram then load the values. These will be displayed as a grey column
            if ((m_Raw != null))
            {
                foreach (KeyValuePair<double, ElevationChangeDataPoint> item in m_Raw)
                {
                    if (!lDisplayData.ContainsKey(item.Key))
                    {
                        lDisplayData[item.Key] = new HistogramDisplayData(UnitsNet.Length.From(item.Value.Elevation, LinearDataUnits).As(linearDisplayUnits));
                    }

                    if (item.Key < 0)
                    {
                        if (bArea)
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.AreaChange(AreaDataUnits, areaDisplayUnits) - lDisplayData[item.Key].Erosion);
                        }
                        else
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.VolumeChange(volDisplayUnits, volDisplayUnits) - lDisplayData[item.Key].Erosion);
                        }
                    }
                    else
                    {
                        if (bArea)
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.AreaChange(AreaDataUnits, areaDisplayUnits) - lDisplayData[item.Key].Deposition);
                        }
                        else
                        {
                            lDisplayData[item.Key].Raw = Math.Max(0, item.Value.VolumeChange(volDisplayUnits, volDisplayUnits) - lDisplayData[item.Key].Deposition);
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

            RefreshDisplay(true, LinearDataUnits, AreaDataUnits, VolumeDataUnits);
            m_Chart.SaveImage(AreaGraphPath, ChartImageFormat.Png);

            RefreshDisplay(false, LinearDataUnits, AreaDataUnits, VolumeDataUnits);
            m_Chart.SaveImage(VolumeGraphPath, ChartImageFormat.Png);
        }

        /// <summary>
        /// Loads a histogram from a CSV
        /// </summary>
        /// <param name="csvFilePath">Input path to CSV file</param>
        /// <remarks></remarks>
        private void LoadHistogram(string csvFilePath, ref Dictionary<double, ElevationChangeDataPoint> values)
        {
            try
            {
                if (!File.Exists(csvFilePath))
                {
                    throw new Exception("Elevation change histogram CSV file does not exist.");
                }

                values = new Dictionary<double, ElevationChangeDataPoint>();

                using (StreamReader readFile = new StreamReader(csvFilePath))
                {
                    // skip first headers line
                    string line = readFile.ReadLine();
                    string[] csvdata = null;
                    while (true)
                    {
                        line = readFile.ReadLine();
                        if (line == null)
                        {
                            break; // TODO: might not be correct. Was : Exit While
                        }
                        else
                        {
                            csvdata = line.Split(',');
                            double fElevation = double.Parse(csvdata[0]);
                            values[fElevation] = new ElevationChangeDataPoint(fElevation, double.Parse(csvdata[2]), double.Parse(csvdata[3]));
                        }
                    }
                }
            }
            catch (System.IO.IOException ex)
            {
                Exception ex2 = new Exception("Could not access elevation change histogram file because it is being used by another program.", ex);
                ex2.Data["File Path"] = csvFilePath;
                throw ex2;
            }
            catch (Exception ex)
            {
            }
        }
    }
}