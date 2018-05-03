using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.Options
{
	partial class frmOptions : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.grdSurveyTypes = new System.Windows.Forms.DataGridView();
            this.colSurveyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUncertainty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.chkComparativeSymbology = new System.Windows.Forms.CheckBox();
            this.chkAutoApplyTransparency = new System.Windows.Forms.CheckBox();
            this.grbTransparencyLayer = new System.Windows.Forms.GroupBox();
            this.chkErrorSurfacesTransparency = new System.Windows.Forms.CheckBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.chkAnalysesTransparency = new System.Windows.Forms.CheckBox();
            this.chkAssociatedSurfacesTransparency = new System.Windows.Forms.CheckBox();
            this.nudTransparency = new System.Windows.Forms.NumericUpDown();
            this.grbComparitiveLayers = new System.Windows.Forms.GroupBox();
            this.chkPointDensityComparative = new System.Windows.Forms.CheckBox();
            this.chkDoDComparative = new System.Windows.Forms.CheckBox();
            this.chkFISErrorComparative = new System.Windows.Forms.CheckBox();
            this.chkInterpolationErrorComparative = new System.Windows.Forms.CheckBox();
            this.chk3DPointQualityComparative = new System.Windows.Forms.CheckBox();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmdFont = new System.Windows.Forms.Button();
            this.txtFont = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picDeposition = new System.Windows.Forms.PictureBox();
            this.picErosion = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lblWidth = new System.Windows.Forms.Label();
            this.numChartHeight = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.numChartWidth = new System.Windows.Forms.NumericUpDown();
            this.TabPage5 = new System.Windows.Forms.TabPage();
            this.lstPyramids = new System.Windows.Forms.CheckedListBox();
            this.lnkPyramidsHelp = new System.Windows.Forms.LinkLabel();
            this.SurveyTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ttpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.frmColourPicker = new System.Windows.Forms.ColorDialog();
            this.frmFont = new System.Windows.Forms.FontDialog();
            this.cmdResetColours = new System.Windows.Forms.Button();
            this.TabControl1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSurveyTypes)).BeginInit();
            this.TabPage3.SuspendLayout();
            this.grbTransparencyLayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).BeginInit();
            this.grbComparitiveLayers.SuspendLayout();
            this.TabPage4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeposition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picErosion)).BeginInit();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChartHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartWidth)).BeginInit();
            this.TabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SurveyTypesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(13, 263);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click_1);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(446, 263);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage3);
            this.TabControl1.Controls.Add(this.TabPage4);
            this.TabControl1.Controls.Add(this.TabPage5);
            this.TabControl1.Location = new System.Drawing.Point(13, 13);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(508, 244);
            this.TabControl1.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.grdSurveyTypes);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(500, 218);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Survey Types";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // grdSurveyTypes
            // 
            this.grdSurveyTypes.AllowUserToResizeRows = false;
            this.grdSurveyTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSurveyTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSurveyType,
            this.colUncertainty});
            this.grdSurveyTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSurveyTypes.Location = new System.Drawing.Point(3, 3);
            this.grdSurveyTypes.Name = "grdSurveyTypes";
            this.grdSurveyTypes.Size = new System.Drawing.Size(494, 212);
            this.grdSurveyTypes.TabIndex = 0;
            // 
            // colSurveyType
            // 
            this.colSurveyType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSurveyType.DataPropertyName = "Name";
            this.colSurveyType.HeaderText = "Survey Type";
            this.colSurveyType.Name = "colSurveyType";
            // 
            // colUncertainty
            // 
            this.colUncertainty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colUncertainty.DataPropertyName = "ErrorValue";
            this.colUncertainty.HeaderText = "Default Uncertainty";
            this.colUncertainty.Name = "colUncertainty";
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.chkComparativeSymbology);
            this.TabPage3.Controls.Add(this.chkAutoApplyTransparency);
            this.TabPage3.Controls.Add(this.grbTransparencyLayer);
            this.TabPage3.Controls.Add(this.grbComparitiveLayers);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(500, 218);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Symbology";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // chkComparativeSymbology
            // 
            this.chkComparativeSymbology.AutoSize = true;
            this.chkComparativeSymbology.Enabled = false;
            this.chkComparativeSymbology.Location = new System.Drawing.Point(214, 12);
            this.chkComparativeSymbology.Margin = new System.Windows.Forms.Padding(2);
            this.chkComparativeSymbology.Name = "chkComparativeSymbology";
            this.chkComparativeSymbology.Size = new System.Drawing.Size(15, 14);
            this.chkComparativeSymbology.TabIndex = 6;
            this.chkComparativeSymbology.UseVisualStyleBackColor = true;
            this.chkComparativeSymbology.CheckedChanged += new System.EventHandler(this.chkComparativeSymbology_CheckedChanged);
            // 
            // chkAutoApplyTransparency
            // 
            this.chkAutoApplyTransparency.AutoSize = true;
            this.chkAutoApplyTransparency.Location = new System.Drawing.Point(27, 12);
            this.chkAutoApplyTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoApplyTransparency.Name = "chkAutoApplyTransparency";
            this.chkAutoApplyTransparency.Size = new System.Drawing.Size(15, 14);
            this.chkAutoApplyTransparency.TabIndex = 8;
            this.chkAutoApplyTransparency.UseVisualStyleBackColor = true;
            this.chkAutoApplyTransparency.CheckedChanged += new System.EventHandler(this.chkAutoApplyTransparency_CheckedChanged);
            // 
            // grbTransparencyLayer
            // 
            this.grbTransparencyLayer.Controls.Add(this.chkErrorSurfacesTransparency);
            this.grbTransparencyLayer.Controls.Add(this.Label7);
            this.grbTransparencyLayer.Controls.Add(this.chkAnalysesTransparency);
            this.grbTransparencyLayer.Controls.Add(this.chkAssociatedSurfacesTransparency);
            this.grbTransparencyLayer.Controls.Add(this.nudTransparency);
            this.grbTransparencyLayer.Location = new System.Drawing.Point(15, 12);
            this.grbTransparencyLayer.Margin = new System.Windows.Forms.Padding(2);
            this.grbTransparencyLayer.Name = "grbTransparencyLayer";
            this.grbTransparencyLayer.Padding = new System.Windows.Forms.Padding(2);
            this.grbTransparencyLayer.Size = new System.Drawing.Size(176, 131);
            this.grbTransparencyLayer.TabIndex = 9;
            this.grbTransparencyLayer.TabStop = false;
            this.grbTransparencyLayer.Text = "       Auto Apply Transparency";
            // 
            // chkErrorSurfacesTransparency
            // 
            this.chkErrorSurfacesTransparency.AutoSize = true;
            this.chkErrorSurfacesTransparency.Location = new System.Drawing.Point(19, 55);
            this.chkErrorSurfacesTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.chkErrorSurfacesTransparency.Name = "chkErrorSurfacesTransparency";
            this.chkErrorSurfacesTransparency.Size = new System.Drawing.Size(93, 17);
            this.chkErrorSurfacesTransparency.TabIndex = 4;
            this.chkErrorSurfacesTransparency.Text = "Error Surfaces";
            this.chkErrorSurfacesTransparency.UseVisualStyleBackColor = true;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(18, 103);
            this.Label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(72, 13);
            this.Label7.TabIndex = 3;
            this.Label7.Text = "Transparency";
            // 
            // chkAnalysesTransparency
            // 
            this.chkAnalysesTransparency.AutoSize = true;
            this.chkAnalysesTransparency.Location = new System.Drawing.Point(19, 78);
            this.chkAnalysesTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.chkAnalysesTransparency.Name = "chkAnalysesTransparency";
            this.chkAnalysesTransparency.Size = new System.Drawing.Size(113, 17);
            this.chkAnalysesTransparency.TabIndex = 1;
            this.chkAnalysesTransparency.Text = "Analyses Surfaces";
            this.chkAnalysesTransparency.UseVisualStyleBackColor = true;
            // 
            // chkAssociatedSurfacesTransparency
            // 
            this.chkAssociatedSurfacesTransparency.AutoSize = true;
            this.chkAssociatedSurfacesTransparency.Location = new System.Drawing.Point(19, 32);
            this.chkAssociatedSurfacesTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.chkAssociatedSurfacesTransparency.Name = "chkAssociatedSurfacesTransparency";
            this.chkAssociatedSurfacesTransparency.Size = new System.Drawing.Size(123, 17);
            this.chkAssociatedSurfacesTransparency.TabIndex = 0;
            this.chkAssociatedSurfacesTransparency.Text = "Associated Surfaces";
            this.chkAssociatedSurfacesTransparency.UseVisualStyleBackColor = true;
            // 
            // nudTransparency
            // 
            this.nudTransparency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudTransparency.Location = new System.Drawing.Point(100, 99);
            this.nudTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.nudTransparency.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudTransparency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudTransparency.Name = "nudTransparency";
            this.nudTransparency.Size = new System.Drawing.Size(44, 20);
            this.nudTransparency.TabIndex = 2;
            this.nudTransparency.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // grbComparitiveLayers
            // 
            this.grbComparitiveLayers.Controls.Add(this.chkPointDensityComparative);
            this.grbComparitiveLayers.Controls.Add(this.chkDoDComparative);
            this.grbComparitiveLayers.Controls.Add(this.chkFISErrorComparative);
            this.grbComparitiveLayers.Controls.Add(this.chkInterpolationErrorComparative);
            this.grbComparitiveLayers.Controls.Add(this.chk3DPointQualityComparative);
            this.grbComparitiveLayers.Enabled = false;
            this.grbComparitiveLayers.Location = new System.Drawing.Point(203, 12);
            this.grbComparitiveLayers.Margin = new System.Windows.Forms.Padding(2);
            this.grbComparitiveLayers.Name = "grbComparitiveLayers";
            this.grbComparitiveLayers.Padding = new System.Windows.Forms.Padding(2);
            this.grbComparitiveLayers.Size = new System.Drawing.Size(280, 131);
            this.grbComparitiveLayers.TabIndex = 7;
            this.grbComparitiveLayers.TabStop = false;
            this.grbComparitiveLayers.Text = "       Use Comparitive Symbology";
            // 
            // chkPointDensityComparative
            // 
            this.chkPointDensityComparative.AutoSize = true;
            this.chkPointDensityComparative.Location = new System.Drawing.Point(19, 78);
            this.chkPointDensityComparative.Margin = new System.Windows.Forms.Padding(2);
            this.chkPointDensityComparative.Name = "chkPointDensityComparative";
            this.chkPointDensityComparative.Size = new System.Drawing.Size(88, 17);
            this.chkPointDensityComparative.TabIndex = 4;
            this.chkPointDensityComparative.Text = "Point Density";
            this.chkPointDensityComparative.UseVisualStyleBackColor = true;
            // 
            // chkDoDComparative
            // 
            this.chkDoDComparative.AutoSize = true;
            this.chkDoDComparative.Location = new System.Drawing.Point(180, 32);
            this.chkDoDComparative.Margin = new System.Windows.Forms.Padding(2);
            this.chkDoDComparative.Name = "chkDoDComparative";
            this.chkDoDComparative.Size = new System.Drawing.Size(48, 17);
            this.chkDoDComparative.TabIndex = 3;
            this.chkDoDComparative.Text = "DoD";
            this.chkDoDComparative.UseVisualStyleBackColor = true;
            // 
            // chkFISErrorComparative
            // 
            this.chkFISErrorComparative.AutoSize = true;
            this.chkFISErrorComparative.Location = new System.Drawing.Point(180, 55);
            this.chkFISErrorComparative.Margin = new System.Windows.Forms.Padding(2);
            this.chkFISErrorComparative.Name = "chkFISErrorComparative";
            this.chkFISErrorComparative.Size = new System.Drawing.Size(67, 17);
            this.chkFISErrorComparative.TabIndex = 2;
            this.chkFISErrorComparative.Text = "FIS Error";
            this.chkFISErrorComparative.UseVisualStyleBackColor = true;
            // 
            // chkInterpolationErrorComparative
            // 
            this.chkInterpolationErrorComparative.AutoSize = true;
            this.chkInterpolationErrorComparative.Location = new System.Drawing.Point(19, 55);
            this.chkInterpolationErrorComparative.Margin = new System.Windows.Forms.Padding(2);
            this.chkInterpolationErrorComparative.Name = "chkInterpolationErrorComparative";
            this.chkInterpolationErrorComparative.Size = new System.Drawing.Size(109, 17);
            this.chkInterpolationErrorComparative.TabIndex = 1;
            this.chkInterpolationErrorComparative.Text = "Interpolation Error";
            this.chkInterpolationErrorComparative.UseVisualStyleBackColor = true;
            // 
            // chk3DPointQualityComparative
            // 
            this.chk3DPointQualityComparative.AutoSize = true;
            this.chk3DPointQualityComparative.Location = new System.Drawing.Point(19, 32);
            this.chk3DPointQualityComparative.Margin = new System.Windows.Forms.Padding(2);
            this.chk3DPointQualityComparative.Name = "chk3DPointQualityComparative";
            this.chk3DPointQualityComparative.Size = new System.Drawing.Size(102, 17);
            this.chk3DPointQualityComparative.TabIndex = 0;
            this.chk3DPointQualityComparative.Text = "3D Point Quality";
            this.chk3DPointQualityComparative.UseVisualStyleBackColor = true;
            // 
            // TabPage4
            // 
            this.TabPage4.Controls.Add(this.groupBox3);
            this.TabPage4.Controls.Add(this.groupBox2);
            this.TabPage4.Controls.Add(this.GroupBox1);
            this.TabPage4.Location = new System.Drawing.Point(4, 22);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage4.Size = new System.Drawing.Size(500, 218);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Graphs";
            this.TabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmdFont);
            this.groupBox3.Controls.Add(this.txtFont);
            this.groupBox3.Location = new System.Drawing.Point(6, 154);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(479, 51);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Font";
            // 
            // cmdFont
            // 
            this.cmdFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFont.Image = global::GCDCore.Properties.Resources.Options;
            this.cmdFont.Location = new System.Drawing.Point(448, 19);
            this.cmdFont.Name = "cmdFont";
            this.cmdFont.Size = new System.Drawing.Size(23, 23);
            this.cmdFont.TabIndex = 1;
            this.cmdFont.UseVisualStyleBackColor = true;
            this.cmdFont.Click += new System.EventHandler(this.cmdFont_Click);
            // 
            // txtFont
            // 
            this.txtFont.Location = new System.Drawing.Point(7, 20);
            this.txtFont.Name = "txtFont";
            this.txtFont.ReadOnly = true;
            this.txtFont.Size = new System.Drawing.Size(435, 20);
            this.txtFont.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdResetColours);
            this.groupBox2.Controls.Add(this.picDeposition);
            this.groupBox2.Controls.Add(this.picErosion);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(479, 58);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Colors";
            // 
            // picDeposition
            // 
            this.picDeposition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDeposition.Location = new System.Drawing.Point(296, 23);
            this.picDeposition.Name = "picDeposition";
            this.picDeposition.Size = new System.Drawing.Size(146, 23);
            this.picDeposition.TabIndex = 3;
            this.picDeposition.TabStop = false;
            this.picDeposition.Click += new System.EventHandler(this.picBox_Click);
            // 
            // picErosion
            // 
            this.picErosion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picErosion.Location = new System.Drawing.Point(96, 23);
            this.picErosion.Name = "picErosion";
            this.picErosion.Size = new System.Drawing.Size(146, 23);
            this.picErosion.TabIndex = 2;
            this.picErosion.TabStop = false;
            this.picErosion.Click += new System.EventHandler(this.picBox_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Raising";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lowering";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.lblWidth);
            this.GroupBox1.Controls.Add(this.numChartHeight);
            this.GroupBox1.Controls.Add(this.lblHeight);
            this.GroupBox1.Controls.Add(this.numChartWidth);
            this.GroupBox1.Location = new System.Drawing.Point(6, 6);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(479, 80);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Default Settings for Automatically Saved Graphs";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(21, 28);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(70, 13);
            this.lblWidth.TabIndex = 0;
            this.lblWidth.Text = "Width (pixels)";
            // 
            // numChartHeight
            // 
            this.numChartHeight.Location = new System.Drawing.Point(96, 50);
            this.numChartHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numChartHeight.Name = "numChartHeight";
            this.numChartHeight.Size = new System.Drawing.Size(70, 20);
            this.numChartHeight.TabIndex = 3;
            this.numChartHeight.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(18, 54);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(73, 13);
            this.lblHeight.TabIndex = 1;
            this.lblHeight.Text = "Height (pixels)";
            // 
            // numChartWidth
            // 
            this.numChartWidth.Location = new System.Drawing.Point(96, 24);
            this.numChartWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numChartWidth.Name = "numChartWidth";
            this.numChartWidth.Size = new System.Drawing.Size(70, 20);
            this.numChartWidth.TabIndex = 2;
            this.numChartWidth.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // TabPage5
            // 
            this.TabPage5.Controls.Add(this.lstPyramids);
            this.TabPage5.Controls.Add(this.lnkPyramidsHelp);
            this.TabPage5.Location = new System.Drawing.Point(4, 22);
            this.TabPage5.Name = "TabPage5";
            this.TabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage5.Size = new System.Drawing.Size(500, 218);
            this.TabPage5.TabIndex = 4;
            this.TabPage5.Text = "Pyramids";
            this.TabPage5.UseVisualStyleBackColor = true;
            // 
            // lstPyramids
            // 
            this.lstPyramids.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPyramids.CheckOnClick = true;
            this.lstPyramids.FormattingEnabled = true;
            this.lstPyramids.Location = new System.Drawing.Point(15, 38);
            this.lstPyramids.Name = "lstPyramids";
            this.lstPyramids.Size = new System.Drawing.Size(467, 169);
            this.lstPyramids.TabIndex = 1;
            // 
            // lnkPyramidsHelp
            // 
            this.lnkPyramidsHelp.AutoSize = true;
            this.lnkPyramidsHelp.LinkArea = new System.Windows.Forms.LinkArea(44, 8);
            this.lnkPyramidsHelp.Location = new System.Drawing.Point(15, 13);
            this.lnkPyramidsHelp.Name = "lnkPyramidsHelp";
            this.lnkPyramidsHelp.Size = new System.Drawing.Size(414, 17);
            this.lnkPyramidsHelp.TabIndex = 0;
            this.lnkPyramidsHelp.TabStop = true;
            this.lnkPyramidsHelp.Text = "Choose which GCD rasters automatically have pyramids built as they are created.";
            this.lnkPyramidsHelp.UseCompatibleTextRendering = true;
            this.lnkPyramidsHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPyramidsHelp_LinkClicked);
            // 
            // SurveyTypesBindingSource
            // 
            this.SurveyTypesBindingSource.AllowNew = true;
            this.SurveyTypesBindingSource.DataMember = "SurveyTypes";
            // 
            // cmdResetColours
            // 
            this.cmdResetColours.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdResetColours.Image = global::GCDCore.Properties.Resources.refresh;
            this.cmdResetColours.Location = new System.Drawing.Point(448, 23);
            this.cmdResetColours.Name = "cmdResetColours";
            this.cmdResetColours.Size = new System.Drawing.Size(23, 23);
            this.cmdResetColours.TabIndex = 2;
            this.cmdResetColours.UseVisualStyleBackColor = true;
            this.cmdResetColours.Click += new System.EventHandler(this.cmdResetColours_Click);
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 298);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnHelp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 299);
            this.Name = "frmOptions";
            this.Text = "GCD Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.TabControl1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSurveyTypes)).EndInit();
            this.TabPage3.ResumeLayout(false);
            this.TabPage3.PerformLayout();
            this.grbTransparencyLayer.ResumeLayout(false);
            this.grbTransparencyLayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).EndInit();
            this.grbComparitiveLayers.ResumeLayout(false);
            this.grbComparitiveLayers.PerformLayout();
            this.TabPage4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeposition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picErosion)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChartHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartWidth)).EndInit();
            this.TabPage5.ResumeLayout(false);
            this.TabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SurveyTypesBindingSource)).EndInit();
            this.ResumeLayout(false);

		}
        internal System.Windows.Forms.Button btnHelp;
        internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.TabControl TabControl1;
		internal System.Windows.Forms.TabPage TabPage2;
		internal System.Windows.Forms.TabPage TabPage3;
		internal System.Windows.Forms.ToolTip ttpTooltip;
		internal System.Windows.Forms.BindingSource SurveyTypesBindingSource;
		internal System.Windows.Forms.TabPage TabPage4;
		internal System.Windows.Forms.Label lblHeight;
		internal System.Windows.Forms.Label lblWidth;
		internal System.Windows.Forms.NumericUpDown numChartHeight;
		internal System.Windows.Forms.NumericUpDown numChartWidth;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.GroupBox grbComparitiveLayers;
		internal System.Windows.Forms.CheckBox chkPointDensityComparative;
		internal System.Windows.Forms.CheckBox chkDoDComparative;
		internal System.Windows.Forms.CheckBox chkFISErrorComparative;
		internal System.Windows.Forms.CheckBox chkInterpolationErrorComparative;
		internal System.Windows.Forms.CheckBox chk3DPointQualityComparative;
        internal System.Windows.Forms.CheckBox chkComparativeSymbology;
		internal System.Windows.Forms.GroupBox grbTransparencyLayer;
		internal System.Windows.Forms.CheckBox chkAssociatedSurfacesTransparency;
        internal System.Windows.Forms.CheckBox chkAutoApplyTransparency;
		internal System.Windows.Forms.CheckBox chkAnalysesTransparency;
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.NumericUpDown nudTransparency;
		internal System.Windows.Forms.CheckBox chkErrorSurfacesTransparency;
		internal System.Windows.Forms.TabPage TabPage5;
        internal System.Windows.Forms.LinkLabel lnkPyramidsHelp;
		internal System.Windows.Forms.CheckedListBox lstPyramids;
        private System.Windows.Forms.DataGridView grdSurveyTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSurveyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUncertainty;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox picDeposition;
        private System.Windows.Forms.PictureBox picErosion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog frmColourPicker;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button cmdFont;
        private System.Windows.Forms.TextBox txtFont;
        private System.Windows.Forms.FontDialog frmFont;
        private System.Windows.Forms.Button cmdResetColours;
    }
}
