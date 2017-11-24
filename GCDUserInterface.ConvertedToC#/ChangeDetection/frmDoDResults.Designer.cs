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
namespace GCDUserInterface.ChangeDetection
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmDoDResults : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDoDResults));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdHelp = new System.Windows.Forms.Button();
			this.txtDoDName = new System.Windows.Forms.TextBox();
			this.Label6 = new System.Windows.Forms.Label();
			this.cmdAddToMap = new System.Windows.Forms.Button();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.TabPage3 = new System.Windows.Forms.TabPage();
			this.ucProperties = new ChangeDetection.ucDoDProperties();
			this.tbpElevationChangeDistribution = new System.Windows.Forms.TabPage();
			this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ucHistogram = new ChangeDetection.ucDoDHistogram();
			this.ucBars = new ChangeDetection.ucChangeBars();
			this.TabPage1 = new System.Windows.Forms.TabPage();
			this.ucSummary = new ChangeDetection.ucDoDSummary();
			this.tabProperties = new System.Windows.Forms.TabControl();
			this.cmdSettings = new System.Windows.Forms.Button();
			this.TabPage3.SuspendLayout();
			this.tbpElevationChangeDistribution.SuspendLayout();
			this.TableLayoutPanel1.SuspendLayout();
			this.TabPage1.SuspendLayout();
			this.tabProperties.SuspendLayout();
			this.SuspendLayout();
			//
			//cmdOK
			//
			this.cmdOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(608, 495);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 6;
			this.cmdOK.Text = "Close";
			this.cmdOK.UseVisualStyleBackColor = true;
			//
			//cmdHelp
			//
			this.cmdHelp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.cmdHelp.Location = new System.Drawing.Point(11, 495);
			this.cmdHelp.Name = "cmdHelp";
			this.cmdHelp.Size = new System.Drawing.Size(75, 23);
			this.cmdHelp.TabIndex = 7;
			this.cmdHelp.Text = "Help";
			this.cmdHelp.UseVisualStyleBackColor = true;
			//
			//txtDoDName
			//
			this.txtDoDName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtDoDName.Location = new System.Drawing.Point(48, 12);
			this.txtDoDName.Name = "txtDoDName";
			this.txtDoDName.ReadOnly = true;
			this.txtDoDName.Size = new System.Drawing.Size(556, 20);
			this.txtDoDName.TabIndex = 1;
			//
			//Label6
			//
			this.Label6.AutoSize = true;
			this.Label6.Location = new System.Drawing.Point(7, 16);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(35, 13);
			this.Label6.TabIndex = 0;
			this.Label6.Text = "Name";
			//
			//cmdAddToMap
			//
			this.cmdAddToMap.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdAddToMap.Image = global::GCDUserInterface.My.Resources.Resources.AddToMap;
			this.cmdAddToMap.Location = new System.Drawing.Point(610, 11);
			this.cmdAddToMap.Name = "cmdAddToMap";
			this.cmdAddToMap.Size = new System.Drawing.Size(23, 23);
			this.cmdAddToMap.TabIndex = 2;
			this.cmdAddToMap.UseVisualStyleBackColor = true;
			//
			//cmdBrowse
			//
			this.cmdBrowse.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdBrowse.Image = global::GCDUserInterface.My.Resources.Resources.BrowseFolder;
			this.cmdBrowse.Location = new System.Drawing.Point(665, 11);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size(23, 23);
			this.cmdBrowse.TabIndex = 4;
			this.cmdBrowse.UseVisualStyleBackColor = true;
			//
			//TabPage3
			//
			this.TabPage3.Controls.Add(this.ucProperties);
			this.TabPage3.Location = new System.Drawing.Point(4, 22);
			this.TabPage3.Name = "TabPage3";
			this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.TabPage3.Size = new System.Drawing.Size(673, 411);
			this.TabPage3.TabIndex = 2;
			this.TabPage3.Text = "Analysis Inputs";
			this.TabPage3.UseVisualStyleBackColor = true;
			//
			//ucProperties
			//
			this.ucProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucProperties.Location = new System.Drawing.Point(3, 3);
			this.ucProperties.Name = "ucProperties";
			this.ucProperties.Size = new System.Drawing.Size(667, 405);
			this.ucProperties.TabIndex = 0;
			//
			//tbpElevationChangeDistribution
			//
			this.tbpElevationChangeDistribution.Controls.Add(this.TableLayoutPanel1);
			this.tbpElevationChangeDistribution.Location = new System.Drawing.Point(4, 22);
			this.tbpElevationChangeDistribution.Name = "tbpElevationChangeDistribution";
			this.tbpElevationChangeDistribution.Padding = new System.Windows.Forms.Padding(3);
			this.tbpElevationChangeDistribution.Size = new System.Drawing.Size(673, 411);
			this.tbpElevationChangeDistribution.TabIndex = 1;
			this.tbpElevationChangeDistribution.Text = "Graphical Results";
			this.tbpElevationChangeDistribution.UseVisualStyleBackColor = true;
			//
			//TableLayoutPanel1
			//
			this.TableLayoutPanel1.ColumnCount = 2;
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.51274f));
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.48726f));
			this.TableLayoutPanel1.Controls.Add(this.ucHistogram, 0, 0);
			this.TableLayoutPanel1.Controls.Add(this.ucBars, 1, 0);
			this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.TableLayoutPanel1.Name = "TableLayoutPanel1";
			this.TableLayoutPanel1.RowCount = 1;
			this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.TableLayoutPanel1.Size = new System.Drawing.Size(667, 405);
			this.TableLayoutPanel1.TabIndex = 1;
			//
			//ucHistogram
			//
			this.ucHistogram.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ucHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucHistogram.Location = new System.Drawing.Point(3, 3);
			this.ucHistogram.Name = "ucHistogram";
			this.ucHistogram.Size = new System.Drawing.Size(490, 399);
			this.ucHistogram.TabIndex = 0;
			//
			//ucBars
			//
			this.ucBars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucBars.Location = new System.Drawing.Point(499, 3);
			this.ucBars.Name = "ucBars";
			this.ucBars.Size = new System.Drawing.Size(165, 399);
			this.ucBars.TabIndex = 1;
			//
			//TabPage1
			//
			this.TabPage1.Controls.Add(this.ucSummary);
			this.TabPage1.Location = new System.Drawing.Point(4, 22);
			this.TabPage1.Name = "TabPage1";
			this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.TabPage1.Size = new System.Drawing.Size(673, 411);
			this.TabPage1.TabIndex = 0;
			this.TabPage1.Text = "Tabular Results";
			this.TabPage1.UseVisualStyleBackColor = true;
			//
			//ucSummary
			//
			this.ucSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucSummary.Location = new System.Drawing.Point(3, 3);
			this.ucSummary.Name = "ucSummary";
			this.ucSummary.Size = new System.Drawing.Size(667, 405);
			this.ucSummary.TabIndex = 0;
			//
			//tabProperties
			//
			this.tabProperties.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.tabProperties.Controls.Add(this.TabPage1);
			this.tabProperties.Controls.Add(this.tbpElevationChangeDistribution);
			this.tabProperties.Controls.Add(this.TabPage3);
			this.tabProperties.Location = new System.Drawing.Point(7, 48);
			this.tabProperties.Name = "tabProperties";
			this.tabProperties.SelectedIndex = 0;
			this.tabProperties.Size = new System.Drawing.Size(681, 437);
			this.tabProperties.TabIndex = 5;
			//
			//cmdSettings
			//
			this.cmdSettings.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdSettings.Image = global::GCDUserInterface.My.Resources.Resources.Settings;
			this.cmdSettings.Location = new System.Drawing.Point(637, 11);
			this.cmdSettings.Name = "cmdSettings";
			this.cmdSettings.Size = new System.Drawing.Size(23, 23);
			this.cmdSettings.TabIndex = 3;
			this.cmdSettings.UseVisualStyleBackColor = true;
			//
			//frmDoDResults
			//
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdOK;
			this.ClientSize = new System.Drawing.Size(695, 530);
			this.Controls.Add(this.cmdSettings);
			this.Controls.Add(this.cmdBrowse);
			this.Controls.Add(this.cmdAddToMap);
			this.Controls.Add(this.txtDoDName);
			this.Controls.Add(this.Label6);
			this.Controls.Add(this.tabProperties);
			this.Controls.Add(this.cmdHelp);
			this.Controls.Add(this.cmdOK);
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MinimumSize = new System.Drawing.Size(300, 200);
			this.Name = "frmDoDResults";
			this.Text = "Change Detection Results";
			this.TabPage3.ResumeLayout(false);
			this.tbpElevationChangeDistribution.ResumeLayout(false);
			this.TableLayoutPanel1.ResumeLayout(false);
			this.TabPage1.ResumeLayout(false);
			this.tabProperties.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button withEventsField_cmdHelp;
		internal System.Windows.Forms.Button cmdHelp {
			get { return withEventsField_cmdHelp; }
			set {
				if (withEventsField_cmdHelp != null) {
					withEventsField_cmdHelp.Click -= cmdHelp_Click;
				}
				withEventsField_cmdHelp = value;
				if (withEventsField_cmdHelp != null) {
					withEventsField_cmdHelp.Click += cmdHelp_Click;
				}
			}
		}
		internal System.Windows.Forms.TextBox txtDoDName;
		internal System.Windows.Forms.Label Label6;
		private System.Windows.Forms.Button withEventsField_cmdAddToMap;
		internal System.Windows.Forms.Button cmdAddToMap {
			get { return withEventsField_cmdAddToMap; }
			set {
				if (withEventsField_cmdAddToMap != null) {
					withEventsField_cmdAddToMap.Click -= cmdAddToMap_Click;
				}
				withEventsField_cmdAddToMap = value;
				if (withEventsField_cmdAddToMap != null) {
					withEventsField_cmdAddToMap.Click += cmdAddToMap_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdBrowse;
		internal System.Windows.Forms.Button cmdBrowse {
			get { return withEventsField_cmdBrowse; }
			set {
				if (withEventsField_cmdBrowse != null) {
					withEventsField_cmdBrowse.Click -= cmdBrowse_Click;
				}
				withEventsField_cmdBrowse = value;
				if (withEventsField_cmdBrowse != null) {
					withEventsField_cmdBrowse.Click += cmdBrowse_Click;
				}
			}
		}
		internal System.Windows.Forms.TabPage TabPage3;
		internal ucDoDProperties ucProperties;
		internal System.Windows.Forms.TabPage tbpElevationChangeDistribution;
		internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
		internal ucDoDHistogram ucHistogram;
		internal ucChangeBars ucBars;
		internal System.Windows.Forms.TabPage TabPage1;
		internal ucDoDSummary ucSummary;
		internal System.Windows.Forms.TabControl tabProperties;
		private System.Windows.Forms.Button withEventsField_cmdSettings;
		internal System.Windows.Forms.Button cmdSettings {
			get { return withEventsField_cmdSettings; }
			set {
				if (withEventsField_cmdSettings != null) {
					withEventsField_cmdSettings.Click -= cmdSettings_Click;
				}
				withEventsField_cmdSettings = value;
				if (withEventsField_cmdSettings != null) {
					withEventsField_cmdSettings.Click += cmdSettings_Click;
				}
			}
		}
	}
}
