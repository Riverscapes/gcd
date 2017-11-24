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
namespace GCDUserInterface.UI.BudgetSegregation
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmBudgetSegProperties : System.Windows.Forms.Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetSegProperties));
			this.Label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.txtUncertaintyAnalysis = new System.Windows.Forms.TextBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.txtOldDEM = new System.Windows.Forms.TextBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.txtNewDEM = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.cboDoD = new System.Windows.Forms.ComboBox();
			this.Label6 = new System.Windows.Forms.Label();
			this.ucPolygon = new GCDCore.UserInterface.UtilityForms.ucVectorInput();
			this.cmdHelp = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.Label7 = new System.Windows.Forms.Label();
			this.GroupBox2 = new System.Windows.Forms.GroupBox();
			this.cboField = new System.Windows.Forms.ComboBox();
			this.Label8 = new System.Windows.Forms.Label();
			this.txtOutputFolder = new System.Windows.Forms.TextBox();
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.SuspendLayout();
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(13, 13);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(38, 13);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "Name:";
			//
			//txtName
			//
			this.txtName.Location = new System.Drawing.Point(95, 9);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(411, 20);
			this.txtName.TabIndex = 1;
			//
			//GroupBox1
			//
			this.GroupBox1.Controls.Add(this.txtUncertaintyAnalysis);
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.txtOldDEM);
			this.GroupBox1.Controls.Add(this.Label4);
			this.GroupBox1.Controls.Add(this.txtNewDEM);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.cboDoD);
			this.GroupBox1.Location = new System.Drawing.Point(13, 41);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(490, 150);
			this.GroupBox1.TabIndex = 2;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Change Detection Analysis";
			//
			//txtUncertaintyAnalysis
			//
			this.txtUncertaintyAnalysis.Location = new System.Drawing.Point(120, 118);
			this.txtUncertaintyAnalysis.Name = "txtUncertaintyAnalysis";
			this.txtUncertaintyAnalysis.ReadOnly = true;
			this.txtUncertaintyAnalysis.Size = new System.Drawing.Size(355, 20);
			this.txtUncertaintyAnalysis.TabIndex = 7;
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Location = new System.Drawing.Point(10, 122);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(104, 13);
			this.Label5.TabIndex = 6;
			this.Label5.Text = "Uncertainty analysis:";
			//
			//txtOldDEM
			//
			this.txtOldDEM.Location = new System.Drawing.Point(120, 88);
			this.txtOldDEM.Name = "txtOldDEM";
			this.txtOldDEM.ReadOnly = true;
			this.txtOldDEM.Size = new System.Drawing.Size(355, 20);
			this.txtOldDEM.TabIndex = 5;
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Location = new System.Drawing.Point(10, 92);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(53, 13);
			this.Label4.TabIndex = 4;
			this.Label4.Text = "Old DEM:";
			//
			//txtNewDEM
			//
			this.txtNewDEM.Location = new System.Drawing.Point(120, 58);
			this.txtNewDEM.Name = "txtNewDEM";
			this.txtNewDEM.ReadOnly = true;
			this.txtNewDEM.Size = new System.Drawing.Size(355, 20);
			this.txtNewDEM.TabIndex = 3;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(10, 62);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(59, 13);
			this.Label3.TabIndex = 2;
			this.Label3.Text = "New DEM:";
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(10, 31);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(38, 13);
			this.Label2.TabIndex = 0;
			this.Label2.Text = "Name:";
			//
			//cboDoD
			//
			this.cboDoD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDoD.FormattingEnabled = true;
			this.cboDoD.Location = new System.Drawing.Point(120, 27);
			this.cboDoD.Name = "cboDoD";
			this.cboDoD.Size = new System.Drawing.Size(355, 21);
			this.cboDoD.TabIndex = 1;
			//
			//Label6
			//
			this.Label6.AutoSize = true;
			this.Label6.Location = new System.Drawing.Point(13, 27);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(74, 13);
			this.Label6.TabIndex = 3;
			this.Label6.Text = "Feature Class:";
			//
			//ucPolygon
			//
			this.ucPolygon.Location = new System.Drawing.Point(120, 21);
			this.ucPolygon.Name = "ucPolygon";
			this.ucPolygon.Size = new System.Drawing.Size(355, 25);
			this.ucPolygon.TabIndex = 0;
			//
			//cmdHelp
			//
			this.cmdHelp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.cmdHelp.Location = new System.Drawing.Point(12, 340);
			this.cmdHelp.Name = "cmdHelp";
			this.cmdHelp.Size = new System.Drawing.Size(75, 23);
			this.cmdHelp.TabIndex = 0;
			this.cmdHelp.Text = "Help";
			this.cmdHelp.UseVisualStyleBackColor = true;
			//
			//cmdCancel
			//
			this.cmdCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(430, 340);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 8;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			//
			//cmdOK
			//
			this.cmdOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(349, 340);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 7;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			//
			//Label7
			//
			this.Label7.AutoSize = true;
			this.Label7.Location = new System.Drawing.Point(13, 57);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(32, 13);
			this.Label7.TabIndex = 1;
			this.Label7.Text = "Field:";
			//
			//GroupBox2
			//
			this.GroupBox2.Controls.Add(this.cboField);
			this.GroupBox2.Controls.Add(this.Label7);
			this.GroupBox2.Controls.Add(this.Label6);
			this.GroupBox2.Controls.Add(this.ucPolygon);
			this.GroupBox2.Location = new System.Drawing.Point(13, 200);
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.Size = new System.Drawing.Size(493, 88);
			this.GroupBox2.TabIndex = 4;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "Polygon Mask";
			//
			//cboField
			//
			this.cboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboField.FormattingEnabled = true;
			this.cboField.Location = new System.Drawing.Point(120, 53);
			this.cboField.Name = "cboField";
			this.cboField.Size = new System.Drawing.Size(355, 21);
			this.cboField.TabIndex = 2;
			//
			//Label8
			//
			this.Label8.AutoSize = true;
			this.Label8.Location = new System.Drawing.Point(13, 304);
			this.Label8.Name = "Label8";
			this.Label8.Size = new System.Drawing.Size(71, 13);
			this.Label8.TabIndex = 5;
			this.Label8.Text = "Output folder:";
			//
			//txtOutputFolder
			//
			this.txtOutputFolder.Location = new System.Drawing.Point(95, 300);
			this.txtOutputFolder.Name = "txtOutputFolder";
			this.txtOutputFolder.ReadOnly = true;
			this.txtOutputFolder.Size = new System.Drawing.Size(411, 20);
			this.txtOutputFolder.TabIndex = 6;
			//
			//BudgetSegPropertiesForm
			//
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(517, 375);
			this.Controls.Add(this.txtOutputFolder);
			this.Controls.Add(this.Label8);
			this.Controls.Add(this.GroupBox2);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdHelp);
			this.Controls.Add(this.GroupBox1);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.Label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BudgetSegPropertiesForm";
			this.Text = "Budget Segregation Properties";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox txtName;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.TextBox txtUncertaintyAnalysis;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.TextBox txtOldDEM;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.TextBox txtNewDEM;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Label Label2;
		private System.Windows.Forms.ComboBox withEventsField_cboDoD;
		internal System.Windows.Forms.ComboBox cboDoD {
			get { return withEventsField_cboDoD; }
			set {
				if (withEventsField_cboDoD != null) {
					withEventsField_cboDoD.SelectedIndexChanged -= cboDoD_SelectedIndexChanged;
				}
				withEventsField_cboDoD = value;
				if (withEventsField_cboDoD != null) {
					withEventsField_cboDoD.SelectedIndexChanged += cboDoD_SelectedIndexChanged;
				}
			}
		}
		internal System.Windows.Forms.Label Label6;
		private GCDCore.UserInterface.UtilityForms.ucVectorInput withEventsField_ucPolygon;
		internal GCDCore.UserInterface.UtilityForms.ucVectorInput ucPolygon {
			get { return withEventsField_ucPolygon; }
			set {
				if (withEventsField_ucPolygon != null) {
					withEventsField_ucPolygon.PathChanged -= PolygonChanged;
				}
				withEventsField_ucPolygon = value;
				if (withEventsField_ucPolygon != null) {
					withEventsField_ucPolygon.PathChanged += PolygonChanged;
				}
			}
		}
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
		internal System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button withEventsField_cmdOK;
		internal System.Windows.Forms.Button cmdOK {
			get { return withEventsField_cmdOK; }
			set {
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click -= cmdOK_Click;
				}
				withEventsField_cmdOK = value;
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click += cmdOK_Click;
				}
			}
		}
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.ComboBox cboField;
		internal System.Windows.Forms.Label Label8;
		internal System.Windows.Forms.TextBox txtOutputFolder;
	}
}
