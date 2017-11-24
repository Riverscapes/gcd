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
namespace GCDUserInterface
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class BudgetClassPieChartUC : System.Windows.Forms.UserControl
	{

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea ChartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend Legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series Series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chtPie = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)this.chtPie).BeginInit();
			this.SuspendLayout();
			//
			//chtPie
			//
			ChartArea1.Name = "ChartArea1";
			this.chtPie.ChartAreas.Add(ChartArea1);
			Legend1.Name = "Legend1";
			this.chtPie.Legends.Add(Legend1);
			this.chtPie.Location = new System.Drawing.Point(73, 94);
			this.chtPie.Name = "chtPie";
			Series1.ChartArea = "ChartArea1";
			Series1.Legend = "Legend1";
			Series1.Name = "Series1";
			this.chtPie.Series.Add(Series1);
			this.chtPie.Size = new System.Drawing.Size(300, 300);
			this.chtPie.TabIndex = 0;
			this.chtPie.Text = "chtPie";
			//
			//BudgetClassPieChartUC
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chtPie);
			this.Name = "BudgetClassPieChartUC";
			this.Size = new System.Drawing.Size(538, 576);
			((System.ComponentModel.ISupportInitialize)this.chtPie).EndInit();
			this.ResumeLayout(false);

		}

		internal System.Windows.Forms.DataVisualization.Charting.Chart chtPie;
		public BudgetClassPieChartUC()
		{
			InitializeComponent();
		}
	}
}
