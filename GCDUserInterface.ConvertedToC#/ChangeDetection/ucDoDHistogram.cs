using GCDCore.Project;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using GCDCore.Visualization;

namespace GCDUserInterface.ChangeDetection
{

	public partial class ucDoDHistogram
	{


		private DoDHistogramViewerClass m_HistogramViewer;
		private void rdoVolume_CheckedChanged(object sender, System.EventArgs e)
		{
			m_HistogramViewer.SetChartType(rdoArea.Checked);
		}

		public void SetHistogramUnits(GCDConsoleLib.GCD.UnitGroup displayUnits)
		{
			m_HistogramViewer.UpdateDisplay(rdoArea.Checked, displayUnits);
		}

		public void LoadHistograms(GCDConsoleLib.Histogram rawHistogram, GCDConsoleLib.Histogram thrHistogram)
		{
			m_HistogramViewer = new DoDHistogramViewerClass(rawHistogram, thrHistogram, ProjectManager.Project.Units);
		}
		public ucDoDHistogram()
		{
			InitializeComponent();
		}

	}

}
