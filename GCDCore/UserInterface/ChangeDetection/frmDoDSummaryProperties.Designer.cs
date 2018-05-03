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
            this.cmdReset = new System.Windows.Forms.Button();
            this.valPrecision = new System.Windows.Forms.NumericUpDown();
            this.Label5 = new System.Windows.Forms.Label();
            this.cboVolume = new System.Windows.Forms.ComboBox();
            this.cboArea = new System.Windows.Forms.ComboBox();
            this.cboLinear = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtFont = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.picDeposition = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picErosion = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoManualYScale = new System.Windows.Forms.RadioButton();
            this.rdoAutomatedYScale = new System.Windows.Forms.RadioButton();
            this.lblIntervalYScale = new System.Windows.Forms.Label();
            this.valYInterval = new System.Windows.Forms.NumericUpDown();
            this.valYMinimum = new System.Windows.Forms.NumericUpDown();
            this.lblMinYScale = new System.Windows.Forms.Label();
            this.valYMaximum = new System.Windows.Forms.NumericUpDown();
            this.lblMaxYScale = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.valInterval = new System.Windows.Forms.NumericUpDown();
            this.valMinimum = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.valMaximum = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.frmColourPicker = new System.Windows.Forms.ColorDialog();
            this.frmFont = new System.Windows.Forms.FontDialog();
            this.cmdResetColours = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeposition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picErosion)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valYInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valYMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valYMaximum)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMaximum)).BeginInit();
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
            this.GroupBox1.Location = new System.Drawing.Point(6, 6);
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
            this.GroupBox2.Location = new System.Drawing.Point(193, 6);
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
            this.cmdOK.Location = new System.Drawing.Point(349, 249);
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
            this.cmdCancel.Location = new System.Drawing.Point(430, 249);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 249);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 5;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click_1);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(37, 17);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(94, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Display linear units";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(39, 48);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(92, 13);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "Display areal units";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(14, 79);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(117, 13);
            this.Label4.TabIndex = 6;
            this.Label4.Text = "Display volumetric units";
            // 
            // cmdReset
            // 
            this.cmdReset.Image = global::GCDCore.Properties.Resources.refresh;
            this.cmdReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdReset.Location = new System.Drawing.Point(279, 105);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(75, 23);
            this.cmdReset.TabIndex = 10;
            this.cmdReset.Text = "    Reset";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // valPrecision
            // 
            this.valPrecision.Location = new System.Drawing.Point(137, 106);
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
            this.Label5.Location = new System.Drawing.Point(81, 110);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(50, 13);
            this.Label5.TabIndex = 8;
            this.Label5.Text = "Precision";
            // 
            // cboVolume
            // 
            this.cboVolume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVolume.FormattingEnabled = true;
            this.cboVolume.Location = new System.Drawing.Point(137, 75);
            this.cboVolume.Name = "cboVolume";
            this.cboVolume.Size = new System.Drawing.Size(217, 21);
            this.cboVolume.TabIndex = 7;
            // 
            // cboArea
            // 
            this.cboArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArea.FormattingEnabled = true;
            this.cboArea.Location = new System.Drawing.Point(137, 44);
            this.cboArea.Name = "cboArea";
            this.cboArea.Size = new System.Drawing.Size(217, 21);
            this.cboArea.TabIndex = 5;
            // 
            // cboLinear
            // 
            this.cboLinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLinear.FormattingEnabled = true;
            this.cboLinear.Location = new System.Drawing.Point(137, 13);
            this.cboLinear.Name = "cboLinear";
            this.cboLinear.Size = new System.Drawing.Size(217, 21);
            this.cboLinear.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(493, 231);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cmdReset);
            this.tabPage1.Controls.Add(this.cboVolume);
            this.tabPage1.Controls.Add(this.valPrecision);
            this.tabPage1.Controls.Add(this.Label2);
            this.tabPage1.Controls.Add(this.Label5);
            this.tabPage1.Controls.Add(this.Label3);
            this.tabPage1.Controls.Add(this.Label4);
            this.tabPage1.Controls.Add(this.cboArea);
            this.tabPage1.Controls.Add(this.cboLinear);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(485, 205);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Units";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.GroupBox1);
            this.tabPage2.Controls.Add(this.GroupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(485, 205);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Table";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(485, 205);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Charts";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtFont);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Location = new System.Drawing.Point(218, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(256, 53);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Fonts";
            // 
            // txtFont
            // 
            this.txtFont.Location = new System.Drawing.Point(11, 20);
            this.txtFont.Name = "txtFont";
            this.txtFont.ReadOnly = true;
            this.txtFont.Size = new System.Drawing.Size(207, 20);
            this.txtFont.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Image = global::GCDCore.Properties.Resources.Options;
            this.button1.Location = new System.Drawing.Point(224, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 6;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmdResetColours);
            this.groupBox5.Controls.Add(this.picDeposition);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.picErosion);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 53);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Colors";
            // 
            // picDeposition
            // 
            this.picDeposition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDeposition.Location = new System.Drawing.Point(140, 19);
            this.picDeposition.Name = "picDeposition";
            this.picDeposition.Size = new System.Drawing.Size(23, 23);
            this.picDeposition.TabIndex = 3;
            this.picDeposition.TabStop = false;
            this.picDeposition.Click += new System.EventHandler(this.OnColorBoxClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(95, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Raising";
            // 
            // picErosion
            // 
            this.picErosion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picErosion.Location = new System.Drawing.Point(67, 19);
            this.picErosion.Name = "picErosion";
            this.picErosion.Size = new System.Drawing.Size(23, 23);
            this.picErosion.TabIndex = 1;
            this.picErosion.TabStop = false;
            this.picErosion.Click += new System.EventHandler(this.OnColorBoxClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lowering";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdoManualYScale);
            this.groupBox4.Controls.Add(this.rdoAutomatedYScale);
            this.groupBox4.Controls.Add(this.lblIntervalYScale);
            this.groupBox4.Controls.Add(this.valYInterval);
            this.groupBox4.Controls.Add(this.valYMinimum);
            this.groupBox4.Controls.Add(this.lblMinYScale);
            this.groupBox4.Controls.Add(this.valYMaximum);
            this.groupBox4.Controls.Add(this.lblMaxYScale);
            this.groupBox4.Location = new System.Drawing.Point(218, 63);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(256, 136);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Histogram Y Axis";
            // 
            // rdoManualYScale
            // 
            this.rdoManualYScale.AutoSize = true;
            this.rdoManualYScale.Location = new System.Drawing.Point(16, 39);
            this.rdoManualYScale.Name = "rdoManualYScale";
            this.rdoManualYScale.Size = new System.Drawing.Size(78, 17);
            this.rdoManualYScale.TabIndex = 13;
            this.rdoManualYScale.Text = "Fixed scale";
            this.rdoManualYScale.UseVisualStyleBackColor = true;
            this.rdoManualYScale.CheckedChanged += new System.EventHandler(this.YAxisMethod_CheckedChanged);
            // 
            // rdoAutomatedYScale
            // 
            this.rdoAutomatedYScale.AutoSize = true;
            this.rdoAutomatedYScale.Checked = true;
            this.rdoAutomatedYScale.Location = new System.Drawing.Point(16, 18);
            this.rdoAutomatedYScale.Name = "rdoAutomatedYScale";
            this.rdoAutomatedYScale.Size = new System.Drawing.Size(104, 17);
            this.rdoAutomatedYScale.TabIndex = 12;
            this.rdoAutomatedYScale.TabStop = true;
            this.rdoAutomatedYScale.Text = "Automated scale";
            this.rdoAutomatedYScale.UseVisualStyleBackColor = true;
            this.rdoAutomatedYScale.CheckedChanged += new System.EventHandler(this.YAxisMethod_CheckedChanged);
            // 
            // lblIntervalYScale
            // 
            this.lblIntervalYScale.AutoSize = true;
            this.lblIntervalYScale.Location = new System.Drawing.Point(33, 115);
            this.lblIntervalYScale.Name = "lblIntervalYScale";
            this.lblIntervalYScale.Size = new System.Drawing.Size(70, 13);
            this.lblIntervalYScale.TabIndex = 11;
            this.lblIntervalYScale.Text = "Label interval";
            // 
            // valYInterval
            // 
            this.valYInterval.DecimalPlaces = 2;
            this.valYInterval.Location = new System.Drawing.Point(107, 111);
            this.valYInterval.Name = "valYInterval";
            this.valYInterval.Size = new System.Drawing.Size(77, 20);
            this.valYInterval.TabIndex = 10;
            // 
            // valYMinimum
            // 
            this.valYMinimum.DecimalPlaces = 2;
            this.valYMinimum.Location = new System.Drawing.Point(107, 85);
            this.valYMinimum.Name = "valYMinimum";
            this.valYMinimum.Size = new System.Drawing.Size(77, 20);
            this.valYMinimum.TabIndex = 9;
            // 
            // lblMinYScale
            // 
            this.lblMinYScale.AutoSize = true;
            this.lblMinYScale.Location = new System.Drawing.Point(55, 89);
            this.lblMinYScale.Name = "lblMinYScale";
            this.lblMinYScale.Size = new System.Drawing.Size(48, 13);
            this.lblMinYScale.TabIndex = 8;
            this.lblMinYScale.Text = "Minimum";
            // 
            // valYMaximum
            // 
            this.valYMaximum.DecimalPlaces = 2;
            this.valYMaximum.Location = new System.Drawing.Point(107, 59);
            this.valYMaximum.Name = "valYMaximum";
            this.valYMaximum.Size = new System.Drawing.Size(77, 20);
            this.valYMaximum.TabIndex = 7;
            // 
            // lblMaxYScale
            // 
            this.lblMaxYScale.AutoSize = true;
            this.lblMaxYScale.Location = new System.Drawing.Point(52, 63);
            this.lblMaxYScale.Name = "lblMaxYScale";
            this.lblMaxYScale.Size = new System.Drawing.Size(51, 13);
            this.lblMaxYScale.TabIndex = 6;
            this.lblMaxYScale.Text = "Maximum";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.valInterval);
            this.groupBox3.Controls.Add(this.valMinimum);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.valMaximum);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(6, 63);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 136);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Histogram X Axis";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Label interval";
            // 
            // valInterval
            // 
            this.valInterval.DecimalPlaces = 2;
            this.valInterval.Location = new System.Drawing.Point(80, 70);
            this.valInterval.Name = "valInterval";
            this.valInterval.Size = new System.Drawing.Size(77, 20);
            this.valInterval.TabIndex = 4;
            // 
            // valMinimum
            // 
            this.valMinimum.DecimalPlaces = 2;
            this.valMinimum.Location = new System.Drawing.Point(80, 44);
            this.valMinimum.Name = "valMinimum";
            this.valMinimum.Size = new System.Drawing.Size(77, 20);
            this.valMinimum.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Minimum";
            // 
            // valMaximum
            // 
            this.valMaximum.DecimalPlaces = 2;
            this.valMaximum.Location = new System.Drawing.Point(80, 18);
            this.valMaximum.Name = "valMaximum";
            this.valMaximum.Size = new System.Drawing.Size(77, 20);
            this.valMaximum.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Maximum";
            // 
            // cmdResetColours
            // 
            this.cmdResetColours.Image = global::GCDCore.Properties.Resources.refresh;
            this.cmdResetColours.Location = new System.Drawing.Point(171, 19);
            this.cmdResetColours.Name = "cmdResetColours";
            this.cmdResetColours.Size = new System.Drawing.Size(23, 23);
            this.cmdResetColours.TabIndex = 8;
            this.cmdResetColours.UseVisualStyleBackColor = true;
            this.cmdResetColours.Click += new System.EventHandler(this.cmdResetColours_Click);
            // 
            // frmDoDSummaryProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(517, 284);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
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
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeposition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picErosion)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valYInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valYMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valYMaximum)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMaximum)).EndInit();
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
        internal System.Windows.Forms.NumericUpDown valPrecision;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.ComboBox cboVolume;
        internal System.Windows.Forms.ComboBox cboArea;
        internal System.Windows.Forms.ComboBox cboLinear;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox picDeposition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picErosion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown valInterval;
        private System.Windows.Forms.NumericUpDown valMinimum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown valMaximum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ColorDialog frmColourPicker;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FontDialog frmFont;
        private System.Windows.Forms.TextBox txtFont;
        private System.Windows.Forms.Label lblIntervalYScale;
        private System.Windows.Forms.NumericUpDown valYInterval;
        private System.Windows.Forms.NumericUpDown valYMinimum;
        private System.Windows.Forms.Label lblMinYScale;
        private System.Windows.Forms.NumericUpDown valYMaximum;
        private System.Windows.Forms.Label lblMaxYScale;
        private System.Windows.Forms.RadioButton rdoManualYScale;
        private System.Windows.Forms.RadioButton rdoAutomatedYScale;
        private System.Windows.Forms.Button cmdResetColours;
    }
}
