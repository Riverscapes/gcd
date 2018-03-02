using GCDCore.Project;
using GCDCore.Visualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucDoDHistogram
    {
        public DoDHistogramViewer HistogramViewer;
        private void rdoVolume_CheckedChanged(object sender, System.EventArgs e)
        {
            HistogramViewer.SetChartType(rdoArea.Checked);
        }

        public ContextMenuStrip ChartContextMenuStrip
        {
            get
            {
                return chtData.ContextMenuStrip;
            }

            set
            {
                chtData.ContextMenuStrip = value;
            }
        }

        public void SetHistogramUnits(GCDConsoleLib.GCD.UnitGroup displayUnits)
        {
            HistogramViewer.UpdateDisplay(rdoArea.Checked, displayUnits);
        }

        public void LoadHistograms(GCDConsoleLib.Histogram rawHistogram, GCDConsoleLib.Histogram thrHistogram)
        {
            HistogramViewer = new DoDHistogramViewer(rawHistogram, thrHistogram, ProjectManager.Project.Units, chtData);
            HistogramViewer.SetFont(Properties.Settings.Default.ChartFont);
        }
        public ucDoDHistogram()
        {
            InitializeComponent();

            rdoVolume.CheckedChanged += rdoVolume_CheckedChanged;
        }
    }
}
