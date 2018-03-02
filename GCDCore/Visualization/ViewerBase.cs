using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace GCDCore.Visualization
{
    public class ViewerBase
    {
        // Names for data series.
        // They are public to allow inherited classes and
        // forms to use them to access the data series
        public const string EROSION = "Erosion";
        public const string DEPOSITION = "Deposition";
        protected const string RAW = "Raw";
        public const string NET = "Net";

        public readonly Chart Chart;

        public ViewerBase(Chart cht)
        {
            if (cht == null)
                Chart = new Chart();
            else
                Chart = cht;

            Chart.ChartAreas.Clear();
            Chart.ChartAreas.Add(new ChartArea());

            Chart.Series.Clear();
            Chart.Palette = ChartColorPalette.None;
            Chart.Legends.Clear();
        }

        public void SetFont(System.Drawing.Font font)
        {
            Chart.ChartAreas[0].AxisX.TitleFont = font;
            Chart.ChartAreas[0].AxisX.LabelStyle.Font = font;
            Chart.ChartAreas[0].AxisY.TitleFont = font;
            Chart.ChartAreas[0].AxisY.LabelStyle.Font = font;
        }

        protected void SaveImage(FileInfo filePath)
        {
            Chart.SaveImage(filePath.FullName, ChartImageFormat.Png);
        }
    }
}
