using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace GCDCore.Visualization
{
    public class ElevationChangeBarViewer
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

        private Chart m_chtControl;
        //Private _ThresholdedHist As Dictionary(Of Double, Double)
        //Private _RawHist As Dictionary(Of Double, Double)

        public ElevationChangeBarViewer()
        {
            m_chtControl = new Chart();
            Init(ref m_chtControl);
        }

        public ElevationChangeBarViewer(ref Chart chtControl)
        {
            Init(ref chtControl);
        }

        private void Init(ref Chart chtControl)
        {
            m_chtControl = chtControl;

            chtControl.ChartAreas.Clear();
            chtControl.Series.Clear();

            chtControl.ChartAreas.Add("ElevationChangeBars");
            chtControl.Legends.Clear();

            var _with1 = chtControl.ChartAreas[0].AxisX;
            _with1.MajorGrid.Enabled = false;
            _with1.MajorTickMark.Enabled = false;

            var _with2 = chtControl.ChartAreas[0].AxisY;
            _with2.MinorTickMark.Enabled = true;
            _with2.MajorGrid.LineColor = Color.LightSlateGray;
            _with2.MinorGrid.Enabled = true;
            _with2.MinorGrid.LineColor = Color.LightGray;
        }

        public void Refresh(double fErosion, double fDeposition, string sDisplayUnitsAbbreviation, BarTypes eType, bool bAbsolute)
        {
            Refresh(fErosion, fDeposition, 0, 0, 0, 0, sDisplayUnitsAbbreviation, false, false, eType, bAbsolute);
        }

        public void Refresh(double fErosion, double fDeposition, double fNet, double fErosionError, double fDepositionError, double fNetError, string sDisplayUnitsAbbreviation, BarTypes eType, bool bAbsolute)
        {
            Refresh(fErosion, fDeposition, fNet, fErosionError, fDepositionError, fNetError, sDisplayUnitsAbbreviation, true, true, eType,
            bAbsolute);
        }

        private void Refresh(double fErosion, double fDeposition, double fNet, double fErosionError, double fDepositionError, double fNetError, 
            string sDisplayUnitsAbbreviation, bool bShowErrorBars, bool bShowNet, BarTypes eType,

        bool bAbsolute)
        {
            m_chtControl.Series.Clear();

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
            m_chtControl.ChartAreas[0].AxisY.Title = sYAxisLabel;

            Dictionary<string, Color> dSeries = new Dictionary<string, Color> {
                {
                    "Erosion",
                    Properties.Settings.Default.Erosion
                },
                {
                    "Depsotion",
                    Properties.Settings.Default.Deposition
                }
            };
            if (bShowNet)
            {
                dSeries.Add("Net", Color.Black);
            }

            Series errSeries = m_chtControl.Series.Add("erosion");
            errSeries.Color = Properties.Settings.Default.Erosion;
            errSeries.ChartArea = m_chtControl.ChartAreas.First().Name;
            errSeries.ChartType = SeriesChartType.StackedColumn;
            errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), fErosion);
            errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), 0);

            Series depSeries = m_chtControl.Series.Add("deposition");
            depSeries.Color = Properties.Settings.Default.Deposition;
            depSeries.ChartArea = m_chtControl.ChartAreas.First().Name;
            depSeries.ChartType = SeriesChartType.StackedColumn;
            depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), 0);
            depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), fDeposition);

            if (bShowNet)
            {
                Series netSeries = m_chtControl.Series.Add("net");
                netSeries.Color = (fNet >= 0 ? Properties.Settings.Default.Deposition : Properties.Settings.Default.Erosion);
                netSeries.ChartArea = m_chtControl.ChartAreas.First().Name;
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Erosion), 0);
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Depositon), 0);
                netSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), fNet);

                errSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), 0);
                depSeries.Points.AddXY(GetXAxisLabel(eType, SeriesType.Net), 0);
            }

            m_chtControl.ChartAreas[0].RecalculateAxesScale();
            m_chtControl.AlignDataPointsByAxisLabel();
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
                    sSeriesType = "Erosion";
                    break;
                case SeriesType.Depositon:
                    sSeriesType = "Deposition";
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

        public void Save(string sFilePath, int nChartWidth, int nChartHeight)
        {
            m_chtControl.SaveImage(sFilePath, ChartImageFormat.Png);
        }
    }
}