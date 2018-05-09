using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace GCDCore.Visualization
{
    public class ElevationChangeBarViewer : ViewerBase
    {
        public enum BarTypes
        {
            Area,
            Volume,
            Vertical
        }

        private enum SeriesType
        {
            Erosion,
            Depositon,
            Net
        }

        public ElevationChangeBarViewer(Chart chtControl = null)
            : base(chtControl)
        {
            var _with1 = Chart.ChartAreas[0].AxisX;
            _with1.MajorGrid.Enabled = false;
            _with1.MajorTickMark.Enabled = false;

            var _with2 = Chart.ChartAreas[0].AxisY;
            _with2.MinorTickMark.Enabled = true;
            _with2.MajorGrid.LineColor = Color.LightSlateGray;
            _with2.MinorGrid.Enabled = true;
            _with2.MinorGrid.LineColor = Color.LightGray;

            Chart.Series.Clear();

            Series errSeries = Chart.Series.Add(ViewerBase.EROSION);
            errSeries.Color = Properties.Settings.Default.Erosion;
            errSeries.ChartArea = Chart.ChartAreas.First().Name;
            errSeries.ChartType = SeriesChartType.StackedColumn;

            Series depSeries = Chart.Series.Add(ViewerBase.DEPOSITION);
            depSeries.Color = Properties.Settings.Default.Deposition;
            depSeries.ChartArea = Chart.ChartAreas.First().Name;
            depSeries.ChartType = SeriesChartType.StackedColumn;
        }

        public void Refresh(double fErosion, double fDeposition, string sDisplayUnitsAbbreviation, BarTypes eType, bool bAbsolute)
        {
            Refresh(fErosion, fDeposition, 0, 0, 0, 0, sDisplayUnitsAbbreviation, false, false, eType, bAbsolute);
        }

        public void Refresh(double fErosion, double fDeposition, double fNet, double fErosionError, double fDepositionError, double fNetError, string sDisplayUnitsAbbreviation, BarTypes eType, bool bAbsolute)
        {
            Refresh(fErosion, fDeposition, fNet, fErosionError, fDepositionError, fNetError, sDisplayUnitsAbbreviation, true, true, eType, bAbsolute);
        }

        private void Refresh(double fErosionOrig, double fDeposition, double fNetOrig, double fErosionError, double fDepositionError, double fNetError,
            string sDisplayUnitsAbbreviation, bool bShowErrorBars, bool bShowNet, BarTypes eType, bool bAbsolute)
        {
            double max, min;

            // We don't want to mutate our parameters so
            double fNet = fNetOrig;
            double fErosion = fErosionOrig;

            if (bAbsolute)
            {
                // Bars should have their correct sign. Erosion should be negative
                // but the number stored in the project is always positive.
                fErosion = -1 * fErosion;
            }
            else
            {
                fNet = Math.Abs(fNet);
            }

            string sYAxisLabel = string.Empty;
            switch (eType)
            {
                case BarTypes.Area:
                    sYAxisLabel = string.Format("Area ({0})", sDisplayUnitsAbbreviation);
                    break;
                case BarTypes.Volume:
                    sYAxisLabel = string.Format("Volume ({0})", sDisplayUnitsAbbreviation);
                    break;
                case BarTypes.Vertical:
                    sYAxisLabel = string.Format("Elevation ({0})", sDisplayUnitsAbbreviation);
                    break;
            }
            Chart.ChartAreas[0].AxisY.Title = sYAxisLabel;

            Dictionary<string, Color> dSeries = new Dictionary<string, Color> {
                {
                    "Lowering",
                    Properties.Settings.Default.Erosion
                },
                {
                    "Raising",
                    Properties.Settings.Default.Deposition
                }
            };

            if (bShowNet)
            {
                dSeries.Add("Net", Color.Black);
            }

            Series errSeries = Chart.Series[ViewerBase.EROSION];
            errSeries.Points.Clear();
            errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), fErosion);
            errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), 0);

            Series depSeries = Chart.Series[ViewerBase.DEPOSITION];
            depSeries.Points.Clear();
            depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), 0);
            depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), fDeposition);

            max = Math.Max(fErosion, fDeposition);
            min = Math.Min(Math.Min(fErosion, fDeposition), 0);

            Series netSeries = Chart.Series.FindByName(ViewerBase.NET);
            if (bShowNet)
            {
                if (netSeries == null)
                    netSeries = Chart.Series.Add(ViewerBase.NET);

                netSeries.Points.Clear();

                netSeries.Color = (fNetOrig >= 0 ? depSeries.Color : errSeries.Color);
                netSeries.ChartArea = Chart.ChartAreas.First().Name;
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), 0);
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), 0);
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), fNet);

                errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), 0);
                depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), 0);

                max = Math.Max(max, fNet);
                min = Math.Min(min, fNet);
            }
            else
            {
                if (netSeries is Series)
                    Chart.Series.Remove(netSeries);
            }

            Series errorSeries = Chart.Series.FindByName(ViewerBase.ERROR);
            if (eType != BarTypes.Area)
            {
                if (errorSeries == null)
                {
                    errorSeries = Chart.Series.Add(ViewerBase.ERROR);
                    errorSeries.ChartType = SeriesChartType.ErrorBar;
                    errorSeries.Color = Color.Black;
                }

                errorSeries.Points.Clear();

                errorSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), fErosion, fErosion - fErosionError, fErosion + fErosionError);
                errorSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), fDeposition, fDeposition - fDepositionError, fDeposition + fDepositionError);

                max = Math.Max(fErosion + fErosionError, fDeposition + fDepositionError);
                min = Math.Min(0, Math.Min(fErosion - fErosionError, fDeposition - fDepositionError));

                if (netSeries is Series)
                    errorSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), fNet, fNet - fNetError, fNet + fNetError);

                max = Math.Max(max, fNet + fNetError);
                min = Math.Min(min, fNet - fNetError);
            }
            else
            {
                if (errorSeries is Series)
                    Chart.Series.Remove(errorSeries);
            }

            try
            {
                // Set an initial max and min so the autoscale can do its thing
                Chart.ChartAreas[0].AxisY.Maximum = max;
                Chart.ChartAreas[0].AxisY.Minimum = min;
                Chart.ChartAreas[0].RecalculateAxesScale();

                Tuple<decimal, decimal> maxmin = GCDConsoleLib.Utility.IntervalMath.GetRegularizedMaxMin((decimal)max, (decimal)min, 0.02m);

                Chart.ChartAreas[0].AxisY.Maximum = (double)maxmin.Item1;
                Chart.ChartAreas[0].AxisY.Minimum = (double)maxmin.Item2;

                Chart.ChartAreas[0].AxisY.Interval = (double)GCDConsoleLib.Utility.IntervalMath.GetSensibleChartInterval(
                    (decimal)Chart.ChartAreas[0].AxisY.Maximum,
                    (decimal)Chart.ChartAreas[0].AxisY.Minimum,
                    10);

                Chart.AlignDataPointsByAxisLabel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating change bar chart axes: " + ex.Message);
                //throw new Exception("Error refreshing elevation bar charts.", ex);
            }
        }


        private object GetXAxisLabel(BarTypes eBarType, SeriesType eSeriesType)
        {
            string sBarType = string.Empty;
            switch (eBarType)
            {
                case BarTypes.Area:
                    sBarType = "Total\\nArea";
                    break;
                case BarTypes.Volume:
                    sBarType = "Total\\nVolume";
                    break;
                case BarTypes.Vertical:
                    sBarType = "Average\\nDepth";
                    break;
            }

            string sSeriesType = string.Empty;
            switch (eSeriesType)
            {
                case SeriesType.Erosion:
                    sSeriesType = "Lowering";
                    break;
                case SeriesType.Depositon:
                    sSeriesType = "Raising";
                    break;
                case SeriesType.Net:
                    if (eBarType == BarTypes.Volume)
                    {
                        return string.Format("Total{0}Net Volume{0}Difference", Environment.NewLine);
                    }
                    else if (eBarType == BarTypes.Vertical)
                    {
                        return string.Format("Avg. Total{0}Thickness{0}Difference", Environment.NewLine);
                    }
                    break;
            }

            return string.Format("{1} of{0}{2}", Environment.NewLine, sBarType, sSeriesType);

        }

        public void Save(System.IO.FileInfo filePath, int nChartWidth, int nChartHeight)
        {
            try
            {
                SaveImage(filePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error saving elevation change bar chart: " + ex.Message);
            }
        }
    }
}