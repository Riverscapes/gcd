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
    partial class frmDoDProperties : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDoDProperties));
            this.Label1 = new System.Windows.Forms.Label();
            this.cboNewDEM = new System.Windows.Forms.ComboBox();
            this.cboNewError = new System.Windows.Forms.ComboBox();
            this.lblNewError = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.valMinLodThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblMinLodThreshold = new System.Windows.Forms.Label();
            this.cmdBayesianProperties = new System.Windows.Forms.Button();
            this.chkBayesian = new System.Windows.Forms.CheckBox();
            this.valConfidence = new System.Windows.Forms.NumericUpDown();
            this.lblConfidence = new System.Windows.Forms.Label();
            this.rdoProbabilistic = new System.Windows.Forms.RadioButton();
            this.rdoPropagated = new System.Windows.Forms.RadioButton();
            this.rdoMinLOD = new System.Windows.Forms.RadioButton();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.cboOldDEM = new System.Windows.Forms.ComboBox();
            this.cboOldError = new System.Windows.Forms.ComboBox();
            this.lblOldError = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.GroupBox1.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valMinLodThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valConfidence)).BeginInit();
            this.GroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(14, 30);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(31, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "DEM";
            // 
            // cboNewDEM
            // 
            this.cboNewDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewDEM.FormattingEnabled = true;
            this.cboNewDEM.Location = new System.Drawing.Point(54, 30);
            this.cboNewDEM.Name = "cboNewDEM";
            this.cboNewDEM.Size = new System.Drawing.Size(201, 21);
            this.cboNewDEM.TabIndex = 1;
            // 
            // cboNewError
            // 
            this.cboNewError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewError.FormattingEnabled = true;
            this.cboNewError.Location = new System.Drawing.Point(54, 57);
            this.cboNewError.Name = "cboNewError";
            this.cboNewError.Size = new System.Drawing.Size(201, 21);
            this.cboNewError.TabIndex = 3;
            // 
            // lblNewError
            // 
            this.lblNewError.AutoSize = true;
            this.lblNewError.Location = new System.Drawing.Point(16, 57);
            this.lblNewError.Name = "lblNewError";
            this.lblNewError.Size = new System.Drawing.Size(29, 13);
            this.lblNewError.TabIndex = 2;
            this.lblNewError.Text = "Error";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.cboNewDEM);
            this.GroupBox1.Controls.Add(this.cboNewError);
            this.GroupBox1.Controls.Add(this.lblNewError);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Location = new System.Drawing.Point(12, 74);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(266, 93);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "New Survey";
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.valMinLodThreshold);
            this.GroupBox3.Controls.Add(this.lblMinLodThreshold);
            this.GroupBox3.Controls.Add(this.cmdBayesianProperties);
            this.GroupBox3.Controls.Add(this.chkBayesian);
            this.GroupBox3.Controls.Add(this.valConfidence);
            this.GroupBox3.Controls.Add(this.lblConfidence);
            this.GroupBox3.Controls.Add(this.rdoProbabilistic);
            this.GroupBox3.Controls.Add(this.rdoPropagated);
            this.GroupBox3.Controls.Add(this.rdoMinLOD);
            this.GroupBox3.Location = new System.Drawing.Point(12, 173);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(537, 177);
            this.GroupBox3.TabIndex = 7;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Uncertainty Analysis Method";
            // 
            // valMinLodThreshold
            // 
            this.valMinLodThreshold.DecimalPlaces = 2;
            this.valMinLodThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valMinLodThreshold.Location = new System.Drawing.Point(170, 43);
            this.valMinLodThreshold.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.valMinLodThreshold.Name = "valMinLodThreshold";
            this.valMinLodThreshold.Size = new System.Drawing.Size(66, 20);
            this.valMinLodThreshold.TabIndex = 2;
            this.valMinLodThreshold.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.valMinLodThreshold.ValueChanged += new System.EventHandler(this.Threshold_ValueChanged);
            // 
            // lblMinLodThreshold
            // 
            this.lblMinLodThreshold.AutoSize = true;
            this.lblMinLodThreshold.Location = new System.Drawing.Point(51, 47);
            this.lblMinLodThreshold.Name = "lblMinLodThreshold";
            this.lblMinLodThreshold.Size = new System.Drawing.Size(63, 13);
            this.lblMinLodThreshold.TabIndex = 1;
            this.lblMinLodThreshold.Text = "Threshold ()";
            // 
            // cmdBayesianProperties
            // 
            this.cmdBayesianProperties.Image = global::GCDCore.Properties.Resources.Settings;
            this.cmdBayesianProperties.Location = new System.Drawing.Point(189, 141);
            this.cmdBayesianProperties.Name = "cmdBayesianProperties";
            this.cmdBayesianProperties.Size = new System.Drawing.Size(23, 23);
            this.cmdBayesianProperties.TabIndex = 8;
            this.cmdBayesianProperties.UseVisualStyleBackColor = true;
            this.cmdBayesianProperties.Click += new System.EventHandler(this.cmdBayesianProperties_Click);
            // 
            // chkBayesian
            // 
            this.chkBayesian.AutoSize = true;
            this.chkBayesian.Location = new System.Drawing.Point(51, 144);
            this.chkBayesian.Name = "chkBayesian";
            this.chkBayesian.Size = new System.Drawing.Size(135, 17);
            this.chkBayesian.TabIndex = 7;
            this.chkBayesian.Text = "Use Bayesian updating";
            this.chkBayesian.UseVisualStyleBackColor = true;
            this.chkBayesian.CheckedChanged += new System.EventHandler(this.chkBayesian_CheckedChanged);
            // 
            // valConfidence
            // 
            this.valConfidence.DecimalPlaces = 2;
            this.valConfidence.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valConfidence.Location = new System.Drawing.Point(170, 112);
            this.valConfidence.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valConfidence.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valConfidence.Name = "valConfidence";
            this.valConfidence.Size = new System.Drawing.Size(66, 20);
            this.valConfidence.TabIndex = 6;
            this.valConfidence.Value = new decimal(new int[] {
            95,
            0,
            0,
            131072});
            this.valConfidence.ValueChanged += new System.EventHandler(this.Threshold_ValueChanged);
            // 
            // lblConfidence
            // 
            this.lblConfidence.AutoSize = true;
            this.lblConfidence.Location = new System.Drawing.Point(51, 116);
            this.lblConfidence.Name = "lblConfidence";
            this.lblConfidence.Size = new System.Drawing.Size(110, 13);
            this.lblConfidence.TabIndex = 5;
            this.lblConfidence.Text = "Confidence level (0-1)";
            // 
            // rdoProbabilistic
            // 
            this.rdoProbabilistic.AutoSize = true;
            this.rdoProbabilistic.Location = new System.Drawing.Point(17, 92);
            this.rdoProbabilistic.Name = "rdoProbabilistic";
            this.rdoProbabilistic.Size = new System.Drawing.Size(141, 17);
            this.rdoProbabilistic.TabIndex = 4;
            this.rdoProbabilistic.Text = "Probabilistic thresholding";
            this.rdoProbabilistic.UseVisualStyleBackColor = true;
            this.rdoProbabilistic.CheckedChanged += new System.EventHandler(this.rdoProbabilistic_CheckedChanged);
            // 
            // rdoPropagated
            // 
            this.rdoPropagated.AutoSize = true;
            this.rdoPropagated.Location = new System.Drawing.Point(17, 69);
            this.rdoPropagated.Name = "rdoPropagated";
            this.rdoPropagated.Size = new System.Drawing.Size(109, 17);
            this.rdoPropagated.TabIndex = 3;
            this.rdoPropagated.Text = "Propagated errors";
            this.rdoPropagated.UseVisualStyleBackColor = true;
            this.rdoPropagated.CheckedChanged += new System.EventHandler(this.rdoProbabilistic_CheckedChanged);
            // 
            // rdoMinLOD
            // 
            this.rdoMinLOD.AutoSize = true;
            this.rdoMinLOD.Checked = true;
            this.rdoMinLOD.Location = new System.Drawing.Point(17, 22);
            this.rdoMinLOD.Name = "rdoMinLOD";
            this.rdoMinLOD.Size = new System.Drawing.Size(183, 17);
            this.rdoMinLOD.TabIndex = 0;
            this.rdoMinLOD.TabStop = true;
            this.rdoMinLOD.Text = "Simple minimum level of detection";
            this.rdoMinLOD.UseVisualStyleBackColor = true;
            this.rdoMinLOD.CheckedChanged += new System.EventHandler(this.rdoProbabilistic_CheckedChanged);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(12, 22);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(74, 13);
            this.Label5.TabIndex = 0;
            this.Label5.Text = "Analysis name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(98, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(451, 20);
            this.txtName.TabIndex = 1;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.cboOldDEM);
            this.GroupBox4.Controls.Add(this.cboOldError);
            this.GroupBox4.Controls.Add(this.lblOldError);
            this.GroupBox4.Controls.Add(this.Label7);
            this.GroupBox4.Location = new System.Drawing.Point(283, 74);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(266, 93);
            this.GroupBox4.TabIndex = 5;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Old Survey";
            // 
            // cboOldDEM
            // 
            this.cboOldDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOldDEM.FormattingEnabled = true;
            this.cboOldDEM.Location = new System.Drawing.Point(52, 30);
            this.cboOldDEM.Name = "cboOldDEM";
            this.cboOldDEM.Size = new System.Drawing.Size(201, 21);
            this.cboOldDEM.TabIndex = 1;
            // 
            // cboOldError
            // 
            this.cboOldError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOldError.FormattingEnabled = true;
            this.cboOldError.Location = new System.Drawing.Point(52, 57);
            this.cboOldError.Name = "cboOldError";
            this.cboOldError.Size = new System.Drawing.Size(201, 21);
            this.cboOldError.TabIndex = 3;
            // 
            // lblOldError
            // 
            this.lblOldError.AutoSize = true;
            this.lblOldError.Location = new System.Drawing.Point(14, 57);
            this.lblOldError.Name = "lblOldError";
            this.lblOldError.Size = new System.Drawing.Size(29, 13);
            this.lblOldError.TabIndex = 2;
            this.lblOldError.Text = "Error";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(12, 30);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(31, 13);
            this.Label7.TabIndex = 0;
            this.Label7.Text = "DEM";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(474, 358);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(393, 358);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "Calculate";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 358);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 10;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(98, 46);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.ReadOnly = true;
            this.txtOutputFolder.Size = new System.Drawing.Size(451, 20);
            this.txtOutputFolder.TabIndex = 3;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 50);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(68, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Output folder";
            // 
            // frmDoDProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(562, 395);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDoDProperties";
            this.Text = "Change Detection Configuration";
            this.Load += new System.EventHandler(this.DoDPropertiesForm_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valMinLodThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valConfidence)).EndInit();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ComboBox withEventsField_cboNewDEM;
        internal System.Windows.Forms.ComboBox cboNewDEM;
        internal System.Windows.Forms.ComboBox cboNewError;
        internal System.Windows.Forms.Label lblNewError;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.GroupBox GroupBox3;
        private System.Windows.Forms.Button withEventsField_cmdBayesianProperties;
        internal System.Windows.Forms.Button cmdBayesianProperties;
        internal System.Windows.Forms.CheckBox chkBayesian;
        internal System.Windows.Forms.NumericUpDown valConfidence;
        internal System.Windows.Forms.Label lblConfidence;
        internal System.Windows.Forms.RadioButton rdoProbabilistic;
        internal System.Windows.Forms.RadioButton rdoPropagated;
        internal System.Windows.Forms.RadioButton rdoMinLOD;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.ComboBox cboOldDEM;
        internal System.Windows.Forms.ComboBox cboOldError;
        internal System.Windows.Forms.Label lblOldError;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdHelp;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.NumericUpDown valMinLodThreshold;
        internal System.Windows.Forms.Label lblMinLodThreshold;
    }
}
