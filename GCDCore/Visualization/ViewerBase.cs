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
        public readonly Chart m_Chart;

        public ViewerBase(Chart cht)
        {
            if (cht == null)
                m_Chart = new Chart();
            else
                m_Chart = cht;

            m_Chart.ChartAreas.Clear();
            m_Chart.ChartAreas.Add(new ChartArea());

            m_Chart.Series.Clear();
            m_Chart.Palette = ChartColorPalette.None;
            m_Chart.Legends.Clear();
        }

        protected void SaveImage(FileInfo filePath)
        {
            m_Chart.SaveImage(filePath.FullName, ChartImageFormat.Png);
        }
    }
}
