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
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

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
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.valPrecision = new System.Windows.Forms.NumericUpDown();
            this.Label5 = new System.Windows.Forms.Label();
            this.cboVolume = new System.Windows.Forms.ComboBox();
            this.cboArea = new System.Windows.Forms.ComboBox();
            this.cboLinear = new System.Windows.Forms.ComboBox();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.chkPercentages);
            this.GroupBox1.Controls.Add(this.chkVertical);
            this.GroupBox1.Controls.Add(this.chkVolumetric);
            this.GroupBox1.Controls.Add(this.chkRowsAreal);
            this.GroupBox1.Controls.Add(this.rdoRowsSpecific);
            this.GroupBox1.Controls.Add(this.rdoRowsNormalized);
            this.GroupBox1.Controls.Add(this.rdoRowsAll);
            this.GroupBox1.Location = new System.Drawing.Point(10, 162);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(175, 190);
            this.GroupBox1.TabIndex = 1;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Row Groups";
            // 
            // chkPercentages
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
            // chkVertical
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
            // chkVolumetric
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
            // chkRowsAreal
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
            // rdoRowsSpecific
            // 
            this.rdoRowsSpecific.AutoSize = true;
            this.rdoRowsSpecific.Location = new System.Drawing.Point(17, 71);
            this.rdoRowsSpecific.Name = "rdoRowsSpecific";
            this.rdoRowsSpecific.Size = new System.Drawing.Size(100, 17);
            this.rdoRowsSpecific.TabIndex = 2;
            this.rdoRowsSpecific.Text = "Specific Groups";
            this.rdoRowsSpecific.UseVisualStyleBackColor = true;
            this.rdoRowsSpecific.CheckedChanged += new System.EventHandler(this.rdoRows_CheckedChanged);
            // 
            // rdoRowsNormalized
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
            this.rdoRowsNormalized.CheckedChanged += new System.EventHandler(this.rdoRows_CheckedChanged);
            // 
            // rdoRowsAll
            // 
            this.rdoRowsAll.AutoSize = true;
            this.rdoRowsAll.Location = new System.Drawing.Point(17, 23);
            this.rdoRowsAll.Name = "rdoRowsAll";
            this.rdoRowsAll.Size = new System.Drawing.Size(65, 17);
            this.rdoRowsAll.TabIndex = 0;
            this.rdoRowsAll.Text = "Show all";
            this.rdoRowsAll.UseVisualStyleBackColor = true;
            this.rdoRowsAll.CheckedChanged += new System.EventHandler(this.rdoRows_CheckedChanged);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.chkColsPercentage);
            this.GroupBox2.Controls.Add(this.chkColsError);
            this.GroupBox2.Controls.Add(this.chkColsThresholded);
            this.GroupBox2.Controls.Add(this.chkColsRaw);
            this.GroupBox2.Location = new System.Drawing.Point(197, 162);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(175, 190);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Column Groups";
            // 
            // chkColsPercentage
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
            // chkColsError
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
            // chkColsThresholded
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
            this.chkColsThresholded.CheckedChanged += new System.EventHandler(this.chkColsThresholded_CheckedChanged);
            // 
            // chkColsRaw
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
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(216, 361);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "Update";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(297, 361);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 361);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 5;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(39, 22);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(94, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Display linear units";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(41, 53);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(92, 13);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "Display areal units";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(16, 84);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(117, 13);
            this.Label4.TabIndex = 6;
            this.Label4.Text = "Display volumetric units";
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.cmdReset);
            this.GroupBox3.Controls.Add(this.valPrecision);
            this.GroupBox3.Controls.Add(this.Label5);
            this.GroupBox3.Controls.Add(this.cboVolume);
            this.GroupBox3.Controls.Add(this.cboArea);
            this.GroupBox3.Controls.Add(this.cboLinear);
            this.GroupBox3.Controls.Add(this.Label4);
            this.GroupBox3.Controls.Add(this.Label3);
            this.GroupBox3.Controls.Add(this.Label2);
            this.GroupBox3.Location = new System.Drawing.Point(10, 10);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(362, 144);
            this.GroupBox3.TabIndex = 0;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Units";
            // 
            // cmdReset
            // 
            this.cmdReset.Image = global::GCDCore.Properties.Resources.refresh;
            this.cmdReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdReset.Location = new System.Drawing.Point(281, 110);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(75, 23);
            this.cmdReset.TabIndex = 10;
            this.cmdReset.Text = "    Reset";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // valPrecision
            // 
            this.valPrecision.Location = new System.Drawing.Point(139, 111);
            this.valPrecision.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.valPrecision.Name = "valPrecision";
            this.valPrecision.Size = new System.Drawing.Size(53, 20);
            this.valPrecision.TabIndex = 9;
            this.valPrecision.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(83, 115);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(50, 13);
            this.Label5.TabIndex = 8;
            this.Label5.Text = "Precision";
            // 
            // cboVolume
            // 
            this.cboVolume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVolume.FormattingEnabled = true;
            this.cboVolume.Location = new System.Drawing.Point(139, 80);
            this.cboVolume.Name = "cboVolume";
            this.cboVolume.Size = new System.Drawing.Size(217, 21);
            this.cboVolume.TabIndex = 7;
            // 
            // cboArea
            // 
            this.cboArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArea.FormattingEnabled = true;
            this.cboArea.Location = new System.Drawing.Point(139, 49);
            this.cboArea.Name = "cboArea";
            this.cboArea.Size = new System.Drawing.Size(217, 21);
            this.cboArea.TabIndex = 5;
            // 
            // cboLinear
            // 
            this.cboLinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLinear.FormattingEnabled = true;
            this.cboLinear.Location = new System.Drawing.Point(139, 18);
            this.cboLinear.Name = "cboLinear";
            this.cboLinear.Size = new System.Drawing.Size(217, 21);
            this.cboLinear.TabIndex = 3;
            // 
            // frmDoDSummaryProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(384, 396);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDoDSummaryProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DoD Summary Properties";
            this.Load += new System.EventHandler(this.frmDoDSummaryProperties_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).EndInit();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.CheckBox chkPercentages;
        internal System.Windows.Forms.CheckBox chkVertical;
        internal System.Windows.Forms.CheckBox chkVolumetric;
        internal System.Windows.Forms.CheckBox chkRowsAreal;
        internal System.Windows.Forms.RadioButton rdoRowsSpecific;
        internal System.Windows.Forms.RadioButton rdoRowsNormalized;
        internal System.Windows.Forms.RadioButton rdoRowsAll;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.CheckBox chkColsPercentage;
        internal System.Windows.Forms.CheckBox chkColsError;
        internal System.Windows.Forms.CheckBox chkColsThresholded;
        internal System.Windows.Forms.CheckBox chkColsRaw;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Button cmdHelp;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.NumericUpDown valPrecision;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.ComboBox cboVolume;
        internal System.Windows.Forms.ComboBox cboArea;
        internal System.Windows.Forms.ComboBox cboLinear;
        private System.Windows.Forms.Button cmdReset;
    }
}
