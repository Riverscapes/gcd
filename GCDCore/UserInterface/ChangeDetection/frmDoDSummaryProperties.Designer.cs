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
	partial class frmDoDSummaryProperties : System.Windows.Forms.Form
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
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.chkPercentages = new System.Windows.Forms.CheckBox();
			this.chkVertical = new System.Windows.Forms.CheckBox();
			this.chkVolumetric = new System.Windows.Forms.CheckBox();
			this.chkRowsAreal = new System.Windows.Forms.CheckBox();
			this.rdoRowsSpecific = new System.Windows.Forms.RadioButton();
			this.rdoRowsNormalized = new System.Windows.Forms.RadioButton();
			this.rdoRowsAll = new System.Windows.Forms.RadioButton();
			this.GroupBox2 = new System.Windows.Forms.GroupBox();
			this.chkColsPercentage = new System.Windows.Forms.CheckBox();
			this.chkColsError = new System.Windows.Forms.CheckBox();
			this.chkColsThresholded = new System.Windows.Forms.CheckBox();
			this.chkColsRaw = new System.Windows.Forms.CheckBox();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdHelp = new System.Windows.Forms.Button();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtUnitsOriginal = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.GroupBox3 = new System.Windows.Forms.GroupBox();
			this.NumericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.Label5 = new System.Windows.Forms.Label();
			this.cboVolume = new System.Windows.Forms.ComboBox();
			this.cboArea = new System.Windows.Forms.ComboBox();
			this.cboLinear = new System.Windows.Forms.ComboBox();
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.GroupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.NumericUpDown1).BeginInit();
			this.SuspendLayout();
			//
			//GroupBox1
			//
			this.GroupBox1.Controls.Add(this.chkPercentages);
			this.GroupBox1.Controls.Add(this.chkVertical);
			this.GroupBox1.Controls.Add(this.chkVolumetric);
			this.GroupBox1.Controls.Add(this.chkRowsAreal);
			this.GroupBox1.Controls.Add(this.rdoRowsSpecific);
			this.GroupBox1.Controls.Add(this.rdoRowsNormalized);
			this.GroupBox1.Controls.Add(this.rdoRowsAll);
			this.GroupBox1.Location = new System.Drawing.Point(10, 190);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(205, 190);
			this.GroupBox1.TabIndex = 1;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Row Groups";
			//
			//chkPercentages
			//
			this.chkPercentages.AutoSize = true;
			this.chkPercentages.Checked = true;
			this.chkPercentages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPercentages.Location = new System.Drawing.Point(53, 164);
			this.chkPercentages.Name = "chkPercentages";
			this.chkPercentages.Size = new System.Drawing.Size(86, 17);
			this.chkPercentages.TabIndex = 6;
			this.chkPercentages.Text = "Percentages";
			this.chkPercentages.UseVisualStyleBackColor = true;
			//
			//chkVertical
			//
			this.chkVertical.AutoSize = true;
			this.chkVertical.Checked = true;
			this.chkVertical.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVertical.Location = new System.Drawing.Point(53, 141);
			this.chkVertical.Name = "chkVertical";
			this.chkVertical.Size = new System.Drawing.Size(108, 17);
			this.chkVertical.TabIndex = 5;
			this.chkVertical.Text = "Vertical averages";
			this.chkVertical.UseVisualStyleBackColor = true;
			//
			//chkVolumetric
			//
			this.chkVolumetric.AutoSize = true;
			this.chkVolumetric.Checked = true;
			this.chkVolumetric.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVolumetric.Location = new System.Drawing.Point(53, 118);
			this.chkVolumetric.Name = "chkVolumetric";
			this.chkVolumetric.Size = new System.Drawing.Size(75, 17);
			this.chkVolumetric.TabIndex = 4;
			this.chkVolumetric.Text = "Volumetric";
			this.chkVolumetric.UseVisualStyleBackColor = true;
			//
			//chkRowsAreal
			//
			this.chkRowsAreal.AutoSize = true;
			this.chkRowsAreal.Checked = true;
			this.chkRowsAreal.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRowsAreal.Location = new System.Drawing.Point(53, 95);
			this.chkRowsAreal.Name = "chkRowsAreal";
			this.chkRowsAreal.Size = new System.Drawing.Size(50, 17);
			this.chkRowsAreal.TabIndex = 3;
			this.chkRowsAreal.Text = "Areal";
			this.chkRowsAreal.UseVisualStyleBackColor = true;
			//
			//rdoRowsSpecific
			//
			this.rdoRowsSpecific.AutoSize = true;
			this.rdoRowsSpecific.Location = new System.Drawing.Point(17, 71);
			this.rdoRowsSpecific.Name = "rdoRowsSpecific";
			this.rdoRowsSpecific.Size = new System.Drawing.Size(100, 17);
			this.rdoRowsSpecific.TabIndex = 2;
			this.rdoRowsSpecific.Text = "Specific Groups";
			this.rdoRowsSpecific.UseVisualStyleBackColor = true;
			//
			//rdoRowsNormalized
			//
			this.rdoRowsNormalized.AutoSize = true;
			this.rdoRowsNormalized.Checked = true;
			this.rdoRowsNormalized.Location = new System.Drawing.Point(17, 47);
			this.rdoRowsNormalized.Name = "rdoRowsNormalized";
			this.rdoRowsNormalized.Size = new System.Drawing.Size(99, 17);
			this.rdoRowsNormalized.TabIndex = 1;
			this.rdoRowsNormalized.TabStop = true;
			this.rdoRowsNormalized.Text = "Normalized only";
			this.rdoRowsNormalized.UseVisualStyleBackColor = true;
			//
			//rdoRowsAll
			//
			this.rdoRowsAll.AutoSize = true;
			this.rdoRowsAll.Location = new System.Drawing.Point(17, 23);
			this.rdoRowsAll.Name = "rdoRowsAll";
			this.rdoRowsAll.Size = new System.Drawing.Size(65, 17);
			this.rdoRowsAll.TabIndex = 0;
			this.rdoRowsAll.Text = "Show all";
			this.rdoRowsAll.UseVisualStyleBackColor = true;
			//
			//GroupBox2
			//
			this.GroupBox2.Controls.Add(this.chkColsPercentage);
			this.GroupBox2.Controls.Add(this.chkColsError);
			this.GroupBox2.Controls.Add(this.chkColsThresholded);
			this.GroupBox2.Controls.Add(this.chkColsRaw);
			this.GroupBox2.Location = new System.Drawing.Point(221, 190);
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.Size = new System.Drawing.Size(211, 190);
			this.GroupBox2.TabIndex = 2;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "Column Groups";
			//
			//chkColsPercentage
			//
			this.chkColsPercentage.AutoSize = true;
			this.chkColsPercentage.Checked = true;
			this.chkColsPercentage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkColsPercentage.Location = new System.Drawing.Point(38, 92);
			this.chkColsPercentage.Name = "chkColsPercentage";
			this.chkColsPercentage.Size = new System.Drawing.Size(59, 17);
			this.chkColsPercentage.TabIndex = 3;
			this.chkColsPercentage.Text = "% Error";
			this.chkColsPercentage.UseVisualStyleBackColor = true;
			//
			//chkColsError
			//
			this.chkColsError.AutoSize = true;
			this.chkColsError.Checked = true;
			this.chkColsError.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkColsError.Location = new System.Drawing.Point(38, 69);
			this.chkColsError.Name = "chkColsError";
			this.chkColsError.Size = new System.Drawing.Size(57, 17);
			this.chkColsError.TabIndex = 2;
			this.chkColsError.Text = "± Error";
			this.chkColsError.UseVisualStyleBackColor = true;
			//
			//chkColsThresholded
			//
			this.chkColsThresholded.AutoSize = true;
			this.chkColsThresholded.Checked = true;
			this.chkColsThresholded.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkColsThresholded.Location = new System.Drawing.Point(15, 46);
			this.chkColsThresholded.Name = "chkColsThresholded";
			this.chkColsThresholded.Size = new System.Drawing.Size(85, 17);
			this.chkColsThresholded.TabIndex = 1;
			this.chkColsThresholded.Text = "Thresholded";
			this.chkColsThresholded.UseVisualStyleBackColor = true;
			//
			//chkColsRaw
			//
			this.chkColsRaw.AutoSize = true;
			this.chkColsRaw.Checked = true;
			this.chkColsRaw.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkColsRaw.Location = new System.Drawing.Point(15, 23);
			this.chkColsRaw.Name = "chkColsRaw";
			this.chkColsRaw.Size = new System.Drawing.Size(48, 17);
			this.chkColsRaw.TabIndex = 0;
			this.chkColsRaw.Text = "Raw";
			this.chkColsRaw.UseVisualStyleBackColor = true;
			//
			//cmdOK
			//
			this.cmdOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(276, 394);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 3;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			//
			//cmdCancel
			//
			this.cmdCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(357, 394);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			//
			//cmdHelp
			//
			this.cmdHelp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.cmdHelp.Location = new System.Drawing.Point(12, 394);
			this.cmdHelp.Name = "cmdHelp";
			this.cmdHelp.Size = new System.Drawing.Size(75, 23);
			this.cmdHelp.TabIndex = 5;
			this.cmdHelp.Text = "Help";
			this.cmdHelp.UseVisualStyleBackColor = true;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(13, 19);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(98, 13);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "Original linear units:";
			//
			//txtUnitsOriginal
			//
			this.txtUnitsOriginal.Location = new System.Drawing.Point(139, 15);
			this.txtUnitsOriginal.Name = "txtUnitsOriginal";
			this.txtUnitsOriginal.ReadOnly = true;
			this.txtUnitsOriginal.Size = new System.Drawing.Size(268, 20);
			this.txtUnitsOriginal.TabIndex = 1;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(13, 49);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(97, 13);
			this.Label2.TabIndex = 2;
			this.Label2.Text = "Display linear units:";
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(13, 80);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(95, 13);
			this.Label3.TabIndex = 4;
			this.Label3.Text = "Display areal units:";
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Location = new System.Drawing.Point(13, 111);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(120, 13);
			this.Label4.TabIndex = 6;
			this.Label4.Text = "Display volumetric units:";
			//
			//GroupBox3
			//
			this.GroupBox3.Controls.Add(this.NumericUpDown1);
			this.GroupBox3.Controls.Add(this.Label5);
			this.GroupBox3.Controls.Add(this.cboVolume);
			this.GroupBox3.Controls.Add(this.cboArea);
			this.GroupBox3.Controls.Add(this.cboLinear);
			this.GroupBox3.Controls.Add(this.txtUnitsOriginal);
			this.GroupBox3.Controls.Add(this.Label4);
			this.GroupBox3.Controls.Add(this.Label1);
			this.GroupBox3.Controls.Add(this.Label3);
			this.GroupBox3.Controls.Add(this.Label2);
			this.GroupBox3.Location = new System.Drawing.Point(10, 10);
			this.GroupBox3.Name = "GroupBox3";
			this.GroupBox3.Size = new System.Drawing.Size(422, 172);
			this.GroupBox3.TabIndex = 0;
			this.GroupBox3.TabStop = false;
			this.GroupBox3.Text = "Units";
			//
			//NumericUpDown1
			//
			this.NumericUpDown1.Location = new System.Drawing.Point(139, 138);
			this.NumericUpDown1.Maximum = new decimal(new int[] {
				10,
				0,
				0,
				0
			});
			this.NumericUpDown1.Name = "NumericUpDown1";
			this.NumericUpDown1.Size = new System.Drawing.Size(53, 20);
			this.NumericUpDown1.TabIndex = 9;
			this.NumericUpDown1.Value = new decimal(new int[] {
				2,
				0,
				0,
				0
			});
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Location = new System.Drawing.Point(13, 142);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(53, 13);
			this.Label5.TabIndex = 8;
			this.Label5.Text = "Precision:";
			//
			//cboVolume
			//
			this.cboVolume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboVolume.FormattingEnabled = true;
			this.cboVolume.Location = new System.Drawing.Point(139, 107);
			this.cboVolume.Name = "cboVolume";
			this.cboVolume.Size = new System.Drawing.Size(268, 21);
			this.cboVolume.TabIndex = 7;
			//
			//cboArea
			//
			this.cboArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboArea.FormattingEnabled = true;
			this.cboArea.Location = new System.Drawing.Point(139, 76);
			this.cboArea.Name = "cboArea";
			this.cboArea.Size = new System.Drawing.Size(268, 21);
			this.cboArea.TabIndex = 5;
			//
			//cboLinear
			//
			this.cboLinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLinear.FormattingEnabled = true;
			this.cboLinear.Location = new System.Drawing.Point(139, 45);
			this.cboLinear.Name = "cboLinear";
			this.cboLinear.Size = new System.Drawing.Size(268, 21);
			this.cboLinear.TabIndex = 3;
			//
			//DoDSummaryPropertiesForm
			//
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(444, 429);
			this.Controls.Add(this.GroupBox3);
			this.Controls.Add(this.cmdHelp);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.GroupBox2);
			this.Controls.Add(this.GroupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DoDSummaryPropertiesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "DoD Summary Properties";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox2.PerformLayout();
			this.GroupBox3.ResumeLayout(false);
			this.GroupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.NumericUpDown1).EndInit();
			this.ResumeLayout(false);

		}
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.CheckBox chkPercentages;
		internal System.Windows.Forms.CheckBox chkVertical;
		internal System.Windows.Forms.CheckBox chkVolumetric;
		internal System.Windows.Forms.CheckBox chkRowsAreal;
		private System.Windows.Forms.RadioButton withEventsField_rdoRowsSpecific;
		internal System.Windows.Forms.RadioButton rdoRowsSpecific {
			get { return withEventsField_rdoRowsSpecific; }
			set {
				if (withEventsField_rdoRowsSpecific != null) {
					withEventsField_rdoRowsSpecific.CheckedChanged -= rdoRows_CheckedChanged;
				}
				withEventsField_rdoRowsSpecific = value;
				if (withEventsField_rdoRowsSpecific != null) {
					withEventsField_rdoRowsSpecific.CheckedChanged += rdoRows_CheckedChanged;
				}
			}
		}
		internal System.Windows.Forms.RadioButton rdoRowsNormalized;
		internal System.Windows.Forms.RadioButton rdoRowsAll;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.CheckBox chkColsPercentage;
		internal System.Windows.Forms.CheckBox chkColsError;
		private System.Windows.Forms.CheckBox withEventsField_chkColsThresholded;
		internal System.Windows.Forms.CheckBox chkColsThresholded {
			get { return withEventsField_chkColsThresholded; }
			set {
				if (withEventsField_chkColsThresholded != null) {
					withEventsField_chkColsThresholded.CheckedChanged -= chkColsThresholded_CheckedChanged;
				}
				withEventsField_chkColsThresholded = value;
				if (withEventsField_chkColsThresholded != null) {
					withEventsField_chkColsThresholded.CheckedChanged += chkColsThresholded_CheckedChanged;
				}
			}
		}
		internal System.Windows.Forms.CheckBox chkColsRaw;
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
		internal System.Windows.Forms.Button cmdCancel;
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
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox txtUnitsOriginal;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.NumericUpDown NumericUpDown1;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.ComboBox cboVolume;
		internal System.Windows.Forms.ComboBox cboArea;
		internal System.Windows.Forms.ComboBox cboLinear;
	}
}
