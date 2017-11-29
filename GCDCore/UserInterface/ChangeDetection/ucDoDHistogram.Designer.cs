using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace GCDCore.UserInterface.ChangeDetection
{
	partial class ucDoDHistogram : System.Windows.Forms.UserControl
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
			this.rdoArea = new System.Windows.Forms.RadioButton();
			this.rdoVolume = new System.Windows.Forms.RadioButton();
			this.chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)this.chtData).BeginInit();
			this.SuspendLayout();
			//
			//rdoArea
			//
			this.rdoArea.AutoSize = true;
			this.rdoArea.Checked = true;
			this.rdoArea.Location = new System.Drawing.Point(4, 4);
			this.rdoArea.Name = "rdoArea";
			this.rdoArea.Size = new System.Drawing.Size(47, 17);
			this.rdoArea.TabIndex = 0;
			this.rdoArea.TabStop = true;
			this.rdoArea.Text = "Area";
			this.rdoArea.UseVisualStyleBackColor = true;
			//
			//rdoVolume
			//
			this.rdoVolume.AutoSize = true;
			this.rdoVolume.Location = new System.Drawing.Point(57, 4);
			this.rdoVolume.Name = "rdoVolume";
			this.rdoVolume.Size = new System.Drawing.Size(60, 17);
			this.rdoVolume.TabIndex = 1;
			this.rdoVolume.Text = "Volume";
			this.rdoVolume.UseVisualStyleBackColor = true;
            this.rdoVolume.CheckedChanged += rdoVolume_CheckedChanged;
            //
            //chtData
            //
            this.chtData.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.chtData.Location = new System.Drawing.Point(0, 32);
			this.chtData.Name = "chtData";
			this.chtData.Size = new System.Drawing.Size(500, 365);
			this.chtData.TabIndex = 3;
			//
			//ucDoDHistogram
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.chtData);
			this.Controls.Add(this.rdoVolume);
			this.Controls.Add(this.rdoArea);
			this.Name = "ucDoDHistogram";
			this.Size = new System.Drawing.Size(503, 400);
			((System.ComponentModel.ISupportInitialize)this.chtData).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.RadioButton rdoArea;
        internal System.Windows.Forms.RadioButton rdoVolume;
		internal System.Windows.Forms.DataVisualization.Charting.Chart chtData;
	}
}
