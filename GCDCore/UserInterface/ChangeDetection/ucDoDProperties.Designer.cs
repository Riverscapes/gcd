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
	partial class ucDoDProperties : System.Windows.Forms.UserControl
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
			this.components = new System.ComponentModel.Container();
			this.Label1 = new System.Windows.Forms.Label();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.txtNewError = new System.Windows.Forms.TextBox();
			this.cmsBasicRaster = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Label2 = new System.Windows.Forms.Label();
			this.txtNewDEM = new System.Windows.Forms.TextBox();
			this.GroupBox2 = new System.Windows.Forms.GroupBox();
			this.txtOldError = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.txtOldDEM = new System.Windows.Forms.TextBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.GroupBox3 = new System.Windows.Forms.GroupBox();
			this.txtThreshold = new System.Windows.Forms.TextBox();
			this.lblThreshold = new System.Windows.Forms.Label();
			this.txtType = new System.Windows.Forms.TextBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.grpProbabilistic = new System.Windows.Forms.GroupBox();
			this.txtDepositionSpatialCoherenceRaster = new System.Windows.Forms.TextBox();
			this.Label12 = new System.Windows.Forms.Label();
			this.txtPosteriorRaster = new System.Windows.Forms.TextBox();
			this.Label10 = new System.Windows.Forms.Label();
			this.txtConditionalRaster = new System.Windows.Forms.TextBox();
			this.Label9 = new System.Windows.Forms.Label();
			this.txtErosionalSpatialCoherenceRaster = new System.Windows.Forms.TextBox();
			this.Label8 = new System.Windows.Forms.Label();
			this.txtBayesian = new System.Windows.Forms.TextBox();
			this.txtConfidence = new System.Windows.Forms.TextBox();
			this.Label6 = new System.Windows.Forms.Label();
			this.lblConfidence = new System.Windows.Forms.Label();
			this.txtProbabilityRaster = new System.Windows.Forms.TextBox();
			this.Label7 = new System.Windows.Forms.Label();
			this.grpPropagated = new System.Windows.Forms.GroupBox();
			this.txtPropErr = new System.Windows.Forms.TextBox();
			this.Label11 = new System.Windows.Forms.Label();
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.GroupBox3.SuspendLayout();
			this.TableLayoutPanel1.SuspendLayout();
			this.grpProbabilistic.SuspendLayout();
			this.grpPropagated.SuspendLayout();
			this.SuspendLayout();
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(9, 24);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(34, 13);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "DEM:";
			//
			//GroupBox1
			//
			this.GroupBox1.Controls.Add(this.txtNewError);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.txtNewDEM);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GroupBox1.Location = new System.Drawing.Point(3, 3);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(244, 74);
			this.GroupBox1.TabIndex = 1;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "New Survey";
			//
			//txtNewError
			//
			this.txtNewError.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtNewError.Location = new System.Drawing.Point(71, 46);
			this.txtNewError.Name = "txtNewError";
			this.txtNewError.ReadOnly = true;
			this.txtNewError.Size = new System.Drawing.Size(163, 20);
			this.txtNewError.TabIndex = 3;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(9, 50);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(32, 13);
			this.Label2.TabIndex = 2;
			this.Label2.Text = "Error:";
			//
			//txtNewDEM
			//
			this.txtNewDEM.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtNewDEM.Location = new System.Drawing.Point(72, 20);
			this.txtNewDEM.Name = "txtNewDEM";
			this.txtNewDEM.ReadOnly = true;
			this.txtNewDEM.Size = new System.Drawing.Size(163, 20);
			this.txtNewDEM.TabIndex = 1;
			//
			//GroupBox2
			//
			this.GroupBox2.Controls.Add(this.txtOldError);
			this.GroupBox2.Controls.Add(this.Label3);
			this.GroupBox2.Controls.Add(this.txtOldDEM);
			this.GroupBox2.Controls.Add(this.Label4);
			this.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GroupBox2.Location = new System.Drawing.Point(253, 3);
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.Size = new System.Drawing.Size(244, 74);
			this.GroupBox2.TabIndex = 2;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "Old Survey";
			//
			//txtOldError
			//
			this.txtOldError.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtOldError.Location = new System.Drawing.Point(71, 46);
			this.txtOldError.Name = "txtOldError";
			this.txtOldError.ReadOnly = true;
			this.txtOldError.Size = new System.Drawing.Size(163, 20);
			this.txtOldError.TabIndex = 3;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(9, 50);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(32, 13);
			this.Label3.TabIndex = 2;
			this.Label3.Text = "Error:";
			//
			//txtOldDEM
			//
			this.txtOldDEM.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtOldDEM.Location = new System.Drawing.Point(71, 20);
			this.txtOldDEM.Name = "txtOldDEM";
			this.txtOldDEM.ReadOnly = true;
			this.txtOldDEM.Size = new System.Drawing.Size(163, 20);
			this.txtOldDEM.TabIndex = 1;
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Location = new System.Drawing.Point(9, 24);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(34, 13);
			this.Label4.TabIndex = 0;
			this.Label4.Text = "DEM:";
			//
			//GroupBox3
			//
			this.GroupBox3.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.GroupBox3.Controls.Add(this.txtThreshold);
			this.GroupBox3.Controls.Add(this.lblThreshold);
			this.GroupBox3.Controls.Add(this.txtType);
			this.GroupBox3.Controls.Add(this.Label5);
			this.GroupBox3.Location = new System.Drawing.Point(3, 83);
			this.GroupBox3.Name = "GroupBox3";
			this.GroupBox3.Size = new System.Drawing.Size(244, 74);
			this.GroupBox3.TabIndex = 3;
			this.GroupBox3.TabStop = false;
			this.GroupBox3.Text = "Uncertainty Analysis Method";
			//
			//txtThreshold
			//
			this.txtThreshold.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtThreshold.Location = new System.Drawing.Point(72, 46);
			this.txtThreshold.Name = "txtThreshold";
			this.txtThreshold.ReadOnly = true;
			this.txtThreshold.Size = new System.Drawing.Size(162, 20);
			this.txtThreshold.TabIndex = 3;
			//
			//lblThreshold
			//
			this.lblThreshold.AutoSize = true;
			this.lblThreshold.Location = new System.Drawing.Point(9, 50);
			this.lblThreshold.Name = "lblThreshold";
			this.lblThreshold.Size = new System.Drawing.Size(57, 13);
			this.lblThreshold.TabIndex = 2;
			this.lblThreshold.Text = "Threshold:";
			//
			//txtType
			//
			this.txtType.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtType.Location = new System.Drawing.Point(72, 20);
			this.txtType.Name = "txtType";
			this.txtType.ReadOnly = true;
			this.txtType.Size = new System.Drawing.Size(162, 20);
			this.txtType.TabIndex = 1;
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Location = new System.Drawing.Point(9, 24);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(34, 13);
			this.Label5.TabIndex = 0;
			this.Label5.Text = "Type:";
			//
			//TableLayoutPanel1
			//
			this.TableLayoutPanel1.ColumnCount = 2;
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.TableLayoutPanel1.Controls.Add(this.GroupBox1, 0, 0);
			this.TableLayoutPanel1.Controls.Add(this.GroupBox2, 1, 0);
			this.TableLayoutPanel1.Controls.Add(this.GroupBox3, 0, 1);
			this.TableLayoutPanel1.Controls.Add(this.grpProbabilistic, 0, 2);
			this.TableLayoutPanel1.Controls.Add(this.grpPropagated, 1, 1);
			this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.TableLayoutPanel1.Name = "TableLayoutPanel1";
			this.TableLayoutPanel1.RowCount = 3;
			this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80f));
			this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80f));
			this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TableLayoutPanel1.Size = new System.Drawing.Size(500, 472);
			this.TableLayoutPanel1.TabIndex = 5;
			//
			//grpProbabilistic
			//
			this.TableLayoutPanel1.SetColumnSpan(this.grpProbabilistic, 2);
			this.grpProbabilistic.Controls.Add(this.txtDepositionSpatialCoherenceRaster);
			this.grpProbabilistic.Controls.Add(this.Label12);
			this.grpProbabilistic.Controls.Add(this.txtPosteriorRaster);
			this.grpProbabilistic.Controls.Add(this.Label10);
			this.grpProbabilistic.Controls.Add(this.txtConditionalRaster);
			this.grpProbabilistic.Controls.Add(this.Label9);
			this.grpProbabilistic.Controls.Add(this.txtErosionalSpatialCoherenceRaster);
			this.grpProbabilistic.Controls.Add(this.Label8);
			this.grpProbabilistic.Controls.Add(this.txtBayesian);
			this.grpProbabilistic.Controls.Add(this.txtConfidence);
			this.grpProbabilistic.Controls.Add(this.Label6);
			this.grpProbabilistic.Controls.Add(this.lblConfidence);
			this.grpProbabilistic.Controls.Add(this.txtProbabilityRaster);
			this.grpProbabilistic.Controls.Add(this.Label7);
			this.grpProbabilistic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpProbabilistic.Location = new System.Drawing.Point(3, 163);
			this.grpProbabilistic.Name = "grpProbabilistic";
			this.grpProbabilistic.Size = new System.Drawing.Size(494, 306);
			this.grpProbabilistic.TabIndex = 4;
			this.grpProbabilistic.TabStop = false;
			this.grpProbabilistic.Text = "Probabilistic Uncertainty Properties";
			this.grpProbabilistic.Visible = false;
			//
			//txtDepositionSpatialCoherenceRaster
			//
			this.txtDepositionSpatialCoherenceRaster.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtDepositionSpatialCoherenceRaster.ContextMenuStrip = this.cmsBasicRaster;
			this.txtDepositionSpatialCoherenceRaster.Location = new System.Drawing.Point(106, 154);
			this.txtDepositionSpatialCoherenceRaster.Name = "txtDepositionSpatialCoherenceRaster";
			this.txtDepositionSpatialCoherenceRaster.ReadOnly = true;
			this.txtDepositionSpatialCoherenceRaster.Size = new System.Drawing.Size(375, 20);
			this.txtDepositionSpatialCoherenceRaster.TabIndex = 9;
			//
			//Label12
			//
			this.Label12.Location = new System.Drawing.Point(9, 147);
			this.Label12.Name = "Label12";
			this.Label12.Size = new System.Drawing.Size(94, 35);
			this.Label12.TabIndex = 8;
			this.Label12.Text = "Deposition spatial coherence raster:";
			//
			//txtPosteriorRaster
			//
			this.txtPosteriorRaster.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtPosteriorRaster.ContextMenuStrip = this.cmsBasicRaster;
			this.txtPosteriorRaster.Location = new System.Drawing.Point(106, 244);
			this.txtPosteriorRaster.Name = "txtPosteriorRaster";
			this.txtPosteriorRaster.ReadOnly = true;
			this.txtPosteriorRaster.Size = new System.Drawing.Size(375, 20);
			this.txtPosteriorRaster.TabIndex = 13;
			//
			//Label10
			//
			this.Label10.Location = new System.Drawing.Point(9, 237);
			this.Label10.Name = "Label10";
			this.Label10.Size = new System.Drawing.Size(94, 35);
			this.Label10.TabIndex = 12;
			this.Label10.Text = "Posterior probability raster:";
			//
			//txtConditionalRaster
			//
			this.txtConditionalRaster.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtConditionalRaster.ContextMenuStrip = this.cmsBasicRaster;
			this.txtConditionalRaster.Location = new System.Drawing.Point(106, 199);
			this.txtConditionalRaster.Name = "txtConditionalRaster";
			this.txtConditionalRaster.ReadOnly = true;
			this.txtConditionalRaster.Size = new System.Drawing.Size(375, 20);
			this.txtConditionalRaster.TabIndex = 11;
			//
			//Label9
			//
			this.Label9.Location = new System.Drawing.Point(9, 192);
			this.Label9.Name = "Label9";
			this.Label9.Size = new System.Drawing.Size(94, 35);
			this.Label9.TabIndex = 10;
			this.Label9.Text = "Conditional probability raster:";
			//
			//txtErosionalSpatialCoherenceRaster
			//
			this.txtErosionalSpatialCoherenceRaster.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtErosionalSpatialCoherenceRaster.ContextMenuStrip = this.cmsBasicRaster;
			this.txtErosionalSpatialCoherenceRaster.Location = new System.Drawing.Point(106, 109);
			this.txtErosionalSpatialCoherenceRaster.Name = "txtErosionalSpatialCoherenceRaster";
			this.txtErosionalSpatialCoherenceRaster.ReadOnly = true;
			this.txtErosionalSpatialCoherenceRaster.Size = new System.Drawing.Size(375, 20);
			this.txtErosionalSpatialCoherenceRaster.TabIndex = 7;
			//
			//Label8
			//
			this.Label8.Location = new System.Drawing.Point(9, 102);
			this.Label8.Name = "Label8";
			this.Label8.Size = new System.Drawing.Size(94, 35);
			this.Label8.TabIndex = 6;
			this.Label8.Text = "Erosion spatial coherence raster:";
			//
			//txtBayesian
			//
			this.txtBayesian.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtBayesian.Location = new System.Drawing.Point(106, 52);
			this.txtBayesian.Name = "txtBayesian";
			this.txtBayesian.ReadOnly = true;
			this.txtBayesian.Size = new System.Drawing.Size(376, 20);
			this.txtBayesian.TabIndex = 3;
			//
			//txtConfidence
			//
			this.txtConfidence.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtConfidence.Location = new System.Drawing.Point(106, 29);
			this.txtConfidence.Name = "txtConfidence";
			this.txtConfidence.ReadOnly = true;
			this.txtConfidence.Size = new System.Drawing.Size(376, 20);
			this.txtConfidence.TabIndex = 1;
			//
			//Label6
			//
			this.Label6.AutoSize = true;
			this.Label6.Location = new System.Drawing.Point(9, 56);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(53, 13);
			this.Label6.TabIndex = 2;
			this.Label6.Text = "Bayesian:";
			//
			//lblConfidence
			//
			this.lblConfidence.AutoSize = true;
			this.lblConfidence.Location = new System.Drawing.Point(9, 33);
			this.lblConfidence.Name = "lblConfidence";
			this.lblConfidence.Size = new System.Drawing.Size(89, 13);
			this.lblConfidence.TabIndex = 0;
			this.lblConfidence.Text = "Confidence level:";
			//
			//txtProbabilityRaster
			//
			this.txtProbabilityRaster.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtProbabilityRaster.ContextMenuStrip = this.cmsBasicRaster;
			this.txtProbabilityRaster.Location = new System.Drawing.Point(106, 75);
			this.txtProbabilityRaster.Name = "txtProbabilityRaster";
			this.txtProbabilityRaster.ReadOnly = true;
			this.txtProbabilityRaster.Size = new System.Drawing.Size(375, 20);
			this.txtProbabilityRaster.TabIndex = 5;
			//
			//Label7
			//
			this.Label7.AutoSize = true;
			this.Label7.Location = new System.Drawing.Point(9, 79);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(87, 13);
			this.Label7.TabIndex = 4;
			this.Label7.Text = "Probability raster:";
			//
			//grpPropagated
			//
			this.grpPropagated.Controls.Add(this.txtPropErr);
			this.grpPropagated.Controls.Add(this.Label11);
			this.grpPropagated.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpPropagated.Location = new System.Drawing.Point(253, 83);
			this.grpPropagated.Name = "grpPropagated";
			this.grpPropagated.Size = new System.Drawing.Size(244, 74);
			this.grpPropagated.TabIndex = 5;
			this.grpPropagated.TabStop = false;
			this.grpPropagated.Text = "Propagated Error";
			this.grpPropagated.Visible = false;
			//
			//txtPropErr
			//
			this.txtPropErr.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtPropErr.ContextMenuStrip = this.cmsBasicRaster;
			this.txtPropErr.Location = new System.Drawing.Point(82, 25);
			this.txtPropErr.Name = "txtPropErr";
			this.txtPropErr.ReadOnly = true;
			this.txtPropErr.Size = new System.Drawing.Size(152, 20);
			this.txtPropErr.TabIndex = 5;
			//
			//Label11
			//
			this.Label11.Location = new System.Drawing.Point(9, 24);
			this.Label11.Name = "Label11";
			this.Label11.Size = new System.Drawing.Size(72, 40);
			this.Label11.TabIndex = 4;
			this.Label11.Text = "Propagated error:";
			//
			//ucDoDProperties
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.TableLayoutPanel1);
			this.Name = "ucDoDProperties";
			this.Size = new System.Drawing.Size(500, 472);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox2.PerformLayout();
			this.GroupBox3.ResumeLayout(false);
			this.GroupBox3.PerformLayout();
			this.TableLayoutPanel1.ResumeLayout(false);
			this.grpProbabilistic.ResumeLayout(false);
			this.grpProbabilistic.PerformLayout();
			this.grpPropagated.ResumeLayout(false);
			this.grpPropagated.PerformLayout();
			this.ResumeLayout(false);

		}
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.TextBox txtNewError;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox txtNewDEM;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.TextBox txtOldError;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TextBox txtOldDEM;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.TextBox txtThreshold;
		internal System.Windows.Forms.Label lblThreshold;
		internal System.Windows.Forms.TextBox txtType;
		internal System.Windows.Forms.ContextMenuStrip cmsRaster;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddToMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem {
			get { return withEventsField_AddToMapToolStripMenuItem; }
			set {
				if (withEventsField_AddToMapToolStripMenuItem != null) {
					withEventsField_AddToMapToolStripMenuItem.Click -= AddToMapToolStripMenuItem_Click;
				}
				withEventsField_AddToMapToolStripMenuItem = value;
				if (withEventsField_AddToMapToolStripMenuItem != null) {
					withEventsField_AddToMapToolStripMenuItem.Click += AddToMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_PropertiesToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem PropertiesToolStripMenuItem {
			get { return withEventsField_PropertiesToolStripMenuItem; }
			set {
				if (withEventsField_PropertiesToolStripMenuItem != null) {
					withEventsField_PropertiesToolStripMenuItem.Click -= PropertiesToolStripMenuItem_Click;
				}
				withEventsField_PropertiesToolStripMenuItem = value;
				if (withEventsField_PropertiesToolStripMenuItem != null) {
					withEventsField_PropertiesToolStripMenuItem.Click += PropertiesToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.GroupBox grpProbabilistic;
		internal System.Windows.Forms.TextBox txtPosteriorRaster;
		internal System.Windows.Forms.Label Label10;
		internal System.Windows.Forms.TextBox txtConditionalRaster;
		internal System.Windows.Forms.Label Label9;
		internal System.Windows.Forms.TextBox txtErosionalSpatialCoherenceRaster;
		internal System.Windows.Forms.Label Label8;
		internal System.Windows.Forms.TextBox txtBayesian;
		internal System.Windows.Forms.TextBox txtConfidence;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.Label lblConfidence;
		internal System.Windows.Forms.TextBox txtProbabilityRaster;
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.GroupBox grpPropagated;
		internal System.Windows.Forms.TextBox txtPropErr;
		internal System.Windows.Forms.Label Label11;
		internal System.Windows.Forms.TextBox txtDepositionSpatialCoherenceRaster;
		internal System.Windows.Forms.Label Label12;
		internal System.Windows.Forms.ContextMenuStrip cmsBasicRaster;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddToMapToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem1 {
			get { return withEventsField_AddToMapToolStripMenuItem1; }
			set {
				if (withEventsField_AddToMapToolStripMenuItem1 != null) {
					withEventsField_AddToMapToolStripMenuItem1.Click -= AddToMapToolStripMenuItem_Click;
				}
				withEventsField_AddToMapToolStripMenuItem1 = value;
				if (withEventsField_AddToMapToolStripMenuItem1 != null) {
					withEventsField_AddToMapToolStripMenuItem1.Click += AddToMapToolStripMenuItem_Click;
				}
			}

		}
	}
}
