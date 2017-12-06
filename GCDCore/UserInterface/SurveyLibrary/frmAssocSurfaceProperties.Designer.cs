using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.SurveyLibrary
{
	partial class frmAssocSurfaceProperties : System.Windows.Forms.Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAssocSurfaceProperties));
            this.Label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ttpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.txtOriginalRaster = new System.Windows.Forms.TextBox();
            this.btnSlopePercent = new System.Windows.Forms.Button();
            this.btnDensity = new System.Windows.Forms.Button();
            this.txtProjectRaster = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.btnRoughness = new System.Windows.Forms.Button();
            this.btnSlopeDegree = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(73, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(115, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(483, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(77, 102);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(31, 13);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "Type";
            // 
            // cboType
            // 
            this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "[Undefined]",
            "3D Point Quality",
            "Grain Size Statistic",
            "Interpolation Error",
            "Point Density",
            "Roughness",
            "Slope Percent Rise",
            "Slope Degrees"});
            this.cboType.Location = new System.Drawing.Point(115, 98);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(483, 21);
            this.cboType.TabIndex = 12;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(458, 39);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 22);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(31, 44);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(77, 13);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Original source";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(15, 130);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(80, 22);
            this.btnHelp.TabIndex = 15;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(432, 130);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 22);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(518, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 22);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtOriginalRaster
            // 
            this.txtOriginalRaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOriginalRaster.Location = new System.Drawing.Point(115, 40);
            this.txtOriginalRaster.Name = "txtOriginalRaster";
            this.txtOriginalRaster.ReadOnly = true;
            this.txtOriginalRaster.Size = new System.Drawing.Size(338, 20);
            this.txtOriginalRaster.TabIndex = 3;
            this.txtOriginalRaster.TabStop = false;
            // 
            // btnSlopePercent
            // 
            this.btnSlopePercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSlopePercent.Image = global::GCDCore.Properties.Resources.SlopePercent;
            this.btnSlopePercent.Location = new System.Drawing.Point(515, 39);
            this.btnSlopePercent.Name = "btnSlopePercent";
            this.btnSlopePercent.Size = new System.Drawing.Size(25, 22);
            this.btnSlopePercent.TabIndex = 6;
            this.btnSlopePercent.UseVisualStyleBackColor = true;
            this.btnSlopePercent.Click += new System.EventHandler(this.btnSlope_Click);
            // 
            // btnDensity
            // 
            this.btnDensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDensity.Image = global::GCDCore.Properties.Resources.PointDensity;
            this.btnDensity.Location = new System.Drawing.Point(544, 39);
            this.btnDensity.Name = "btnDensity";
            this.btnDensity.Size = new System.Drawing.Size(25, 22);
            this.btnDensity.TabIndex = 7;
            this.btnDensity.UseVisualStyleBackColor = true;
            this.btnDensity.Click += new System.EventHandler(this.btnDensity_Click);
            // 
            // txtProjectRaster
            // 
            this.txtProjectRaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjectRaster.Location = new System.Drawing.Point(115, 69);
            this.txtProjectRaster.Name = "txtProjectRaster";
            this.txtProjectRaster.ReadOnly = true;
            this.txtProjectRaster.Size = new System.Drawing.Size(483, 20);
            this.txtProjectRaster.TabIndex = 10;
            this.txtProjectRaster.TabStop = false;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(15, 73);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(93, 13);
            this.Label4.TabIndex = 9;
            this.Label4.Text = "Project raster path";
            // 
            // btnRoughness
            // 
            this.btnRoughness.Image = global::GCDCore.Properties.Resources.Roughness;
            this.btnRoughness.Location = new System.Drawing.Point(573, 39);
            this.btnRoughness.Name = "btnRoughness";
            this.btnRoughness.Size = new System.Drawing.Size(25, 22);
            this.btnRoughness.TabIndex = 8;
            this.btnRoughness.UseVisualStyleBackColor = true;
            // 
            // btnSlopeDegree
            // 
            this.btnSlopeDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSlopeDegree.Image = global::GCDCore.Properties.Resources.SlopeDegree;
            this.btnSlopeDegree.Location = new System.Drawing.Point(486, 39);
            this.btnSlopeDegree.Name = "btnSlopeDegree";
            this.btnSlopeDegree.Size = new System.Drawing.Size(25, 22);
            this.btnSlopeDegree.TabIndex = 5;
            this.btnSlopeDegree.UseVisualStyleBackColor = true;
            this.btnSlopeDegree.Click += new System.EventHandler(this.btnSlopeDegree_Click);
            // 
            // frmAssocSurfaceProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(607, 162);
            this.ControlBox = false;
            this.Controls.Add(this.btnSlopeDegree);
            this.Controls.Add(this.btnRoughness);
            this.Controls.Add(this.txtProjectRaster);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.btnDensity);
            this.Controls.Add(this.btnSlopePercent);
            this.Controls.Add(this.txtOriginalRaster);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAssocSurfaceProperties";
            this.Text = "Associated Surface Properties";
            this.Load += new System.EventHandler(this.SurfacePropertiesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtName;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.ComboBox cboType;
		internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Button btnHelp;
        internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnBrowse;
		internal System.Windows.Forms.ToolTip ttpTooltip;
		internal System.Windows.Forms.TextBox txtOriginalRaster;
        internal System.Windows.Forms.Button btnSlopePercent;
        internal System.Windows.Forms.Button btnDensity;
		internal System.Windows.Forms.TextBox txtProjectRaster;
		internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button btnRoughness;
        internal System.Windows.Forms.Button btnSlopeDegree;
	}

}
