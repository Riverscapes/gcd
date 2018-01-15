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
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.chkTempWorkspaceWarning = new System.Windows.Forms.CheckBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdDefault = new System.Windows.Forms.Button();
            this.txtWorkspace = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnExploreWorkspace = new System.Windows.Forms.Button();
            this.btnClearWorkspace = new System.Windows.Forms.Button();
            this.chkAutoLoadEtalFIS = new System.Windows.Forms.CheckBox();
            this.chkBoxValidateProjectOnLoad = new System.Windows.Forms.CheckBox();
            this.chkWarnAboutLongPaths = new System.Windows.Forms.CheckBox();
            this.chkAddOutputLayersToMap = new System.Windows.Forms.CheckBox();
            this.chkAddInputLayersToMap = new System.Windows.Forms.CheckBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.chkClearWorkspaceOnStartup = new System.Windows.Forms.CheckBox();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.SurveyTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.grbTransparencyLayer = new System.Windows.Forms.GroupBox();
            this.chkErrorSurfacesTransparency = new System.Windows.Forms.CheckBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.nudTransparency = new System.Windows.Forms.NumericUpDown();
            this.chkAnalysesTransparency = new System.Windows.Forms.CheckBox();
            this.chkAssociatedSurfacesTransparency = new System.Windows.Forms.CheckBox();
            this.chkAutoApplyTransparency = new System.Windows.Forms.CheckBox();
            this.grbComparitiveLayers = new System.Windows.Forms.GroupBox();
            this.chkPointDensityComparative = new System.Windows.Forms.CheckBox();
            this.chkDoDComparative = new System.Windows.Forms.CheckBox();
            this.chkFISErrorComparative = new System.Windows.Forms.CheckBox();
            this.chkInterpolationErrorComparative = new System.Windows.Forms.CheckBox();
            this.chk3DPointQualityComparative = new System.Windows.Forms.CheckBox();
            this.chkComparativeSymbology = new System.Windows.Forms.CheckBox();
            this.cboLayer = new System.Windows.Forms.ComboBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnSymReset = new System.Windows.Forms.Button();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lblWidth = new System.Windows.Forms.Label();
            this.numChartHeight = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.numChartWidth = new System.Windows.Forms.NumericUpDown();
            this.TabPage5 = new System.Windows.Forms.TabPage();
            this.lstPyramids = new System.Windows.Forms.CheckedListBox();
            this.lnkPyramidsHelp = new System.Windows.Forms.LinkLabel();
            this.ttpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.grdSurveyTypes = new System.Windows.Forms.DataGridView();
            this.colSurveyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUncertainty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.TabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SurveyTypesBindingSource)).BeginInit();
            this.TabPage3.SuspendLayout();
            this.grbTransparencyLayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).BeginInit();
            this.grbComparitiveLayers.SuspendLayout();
            this.TabPage4.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChartHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartWidth)).BeginInit();
            this.TabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSurveyTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(13, 328);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(446, 328);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage3);
            this.TabControl1.Controls.Add(this.TabPage4);
            this.TabControl1.Controls.Add(this.TabPage5);
            this.TabControl1.Location = new System.Drawing.Point(13, 13);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(508, 309);
            this.TabControl1.TabIndex = 0;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.chkTempWorkspaceWarning);
            this.TabPage1.Controls.Add(this.GroupBox2);
            this.TabPage1.Controls.Add(this.chkAutoLoadEtalFIS);
            this.TabPage1.Controls.Add(this.chkBoxValidateProjectOnLoad);
            this.TabPage1.Controls.Add(this.chkWarnAboutLongPaths);
            this.TabPage1.Controls.Add(this.chkAddOutputLayersToMap);
            this.TabPage1.Controls.Add(this.chkAddInputLayersToMap);
            this.TabPage1.Controls.Add(this.Label5);
            this.TabPage1.Controls.Add(this.cboFormat);
            this.TabPage1.Controls.Add(this.chkClearWorkspaceOnStartup);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(500, 283);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Workspace";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // chkTempWorkspaceWarning
            // 
            this.chkTempWorkspaceWarning.AutoSize = true;
            this.chkTempWorkspaceWarning.Location = new System.Drawing.Point(265, 126);
            this.chkTempWorkspaceWarning.Name = "chkTempWorkspaceWarning";
            this.chkTempWorkspaceWarning.Size = new System.Drawing.Size(226, 17);
            this.chkTempWorkspaceWarning.TabIndex = 9;
            this.chkTempWorkspaceWarning.Text = "Invalid temp workspace character warning";
            this.chkTempWorkspaceWarning.UseVisualStyleBackColor = true;
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.cmdDefault);
            this.GroupBox2.Controls.Add(this.txtWorkspace);
            this.GroupBox2.Controls.Add(this.btnBrowse);
            this.GroupBox2.Controls.Add(this.btnExploreWorkspace);
            this.GroupBox2.Controls.Add(this.btnClearWorkspace);
            this.GroupBox2.Location = new System.Drawing.Point(11, 9);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(483, 80);
            this.GroupBox2.TabIndex = 0;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Temporary Workspace Folder";
            // 
            // cmdDefault
            // 
            this.cmdDefault.Location = new System.Drawing.Point(328, 45);
            this.cmdDefault.Name = "cmdDefault";
            this.cmdDefault.Size = new System.Drawing.Size(148, 23);
            this.cmdDefault.TabIndex = 4;
            this.cmdDefault.Text = "Use Default Workspace";
            this.cmdDefault.UseVisualStyleBackColor = true;
            // 
            // txtWorkspace
            // 
            this.txtWorkspace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkspace.Location = new System.Drawing.Point(6, 19);
            this.txtWorkspace.Name = "txtWorkspace";
            this.txtWorkspace.ReadOnly = true;
            this.txtWorkspace.Size = new System.Drawing.Size(436, 20);
            this.txtWorkspace.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(448, 18);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnExploreWorkspace
            // 
            this.btnExploreWorkspace.Location = new System.Drawing.Point(6, 45);
            this.btnExploreWorkspace.Name = "btnExploreWorkspace";
            this.btnExploreWorkspace.Size = new System.Drawing.Size(148, 23);
            this.btnExploreWorkspace.TabIndex = 2;
            this.btnExploreWorkspace.Text = "Open In Explorer";
            this.btnExploreWorkspace.UseVisualStyleBackColor = true;
            // 
            // btnClearWorkspace
            // 
            this.btnClearWorkspace.Location = new System.Drawing.Point(167, 45);
            this.btnClearWorkspace.Name = "btnClearWorkspace";
            this.btnClearWorkspace.Size = new System.Drawing.Size(148, 23);
            this.btnClearWorkspace.TabIndex = 3;
            this.btnClearWorkspace.Text = "Clear Workspace";
            this.btnClearWorkspace.UseVisualStyleBackColor = true;
            // 
            // chkAutoLoadEtalFIS
            // 
            this.chkAutoLoadEtalFIS.AutoSize = true;
            this.chkAutoLoadEtalFIS.Location = new System.Drawing.Point(11, 213);
            this.chkAutoLoadEtalFIS.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoLoadEtalFIS.Name = "chkAutoLoadEtalFIS";
            this.chkAutoLoadEtalFIS.Size = new System.Drawing.Size(194, 17);
            this.chkAutoLoadEtalFIS.TabIndex = 5;
            this.chkAutoLoadEtalFIS.Text = "Autoload ET-AL provided FIS library";
            this.chkAutoLoadEtalFIS.UseVisualStyleBackColor = true;
            // 
            // chkBoxValidateProjectOnLoad
            // 
            this.chkBoxValidateProjectOnLoad.AutoSize = true;
            this.chkBoxValidateProjectOnLoad.Location = new System.Drawing.Point(11, 184);
            this.chkBoxValidateProjectOnLoad.Name = "chkBoxValidateProjectOnLoad";
            this.chkBoxValidateProjectOnLoad.Size = new System.Drawing.Size(163, 17);
            this.chkBoxValidateProjectOnLoad.TabIndex = 4;
            this.chkBoxValidateProjectOnLoad.Text = "Validate GCD project on load";
            this.chkBoxValidateProjectOnLoad.UseVisualStyleBackColor = true;
            // 
            // chkWarnAboutLongPaths
            // 
            this.chkWarnAboutLongPaths.AutoSize = true;
            this.chkWarnAboutLongPaths.Location = new System.Drawing.Point(265, 97);
            this.chkWarnAboutLongPaths.Name = "chkWarnAboutLongPaths";
            this.chkWarnAboutLongPaths.Size = new System.Drawing.Size(193, 17);
            this.chkWarnAboutLongPaths.TabIndex = 8;
            this.chkWarnAboutLongPaths.Text = "Warn about potential long file paths";
            this.chkWarnAboutLongPaths.UseVisualStyleBackColor = true;
            // 
            // chkAddOutputLayersToMap
            // 
            this.chkAddOutputLayersToMap.AutoSize = true;
            this.chkAddOutputLayersToMap.Location = new System.Drawing.Point(11, 155);
            this.chkAddOutputLayersToMap.Name = "chkAddOutputLayersToMap";
            this.chkAddOutputLayersToMap.Size = new System.Drawing.Size(143, 17);
            this.chkAddOutputLayersToMap.TabIndex = 3;
            this.chkAddOutputLayersToMap.Text = "Add output layers to map";
            this.chkAddOutputLayersToMap.UseVisualStyleBackColor = true;
            // 
            // chkAddInputLayersToMap
            // 
            this.chkAddInputLayersToMap.AutoSize = true;
            this.chkAddInputLayersToMap.Checked = true;
            this.chkAddInputLayersToMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddInputLayersToMap.Location = new System.Drawing.Point(11, 126);
            this.chkAddInputLayersToMap.Name = "chkAddInputLayersToMap";
            this.chkAddInputLayersToMap.Size = new System.Drawing.Size(136, 17);
            this.chkAddInputLayersToMap.TabIndex = 2;
            this.chkAddInputLayersToMap.Text = "Add input layers to map";
            this.chkAddInputLayersToMap.UseVisualStyleBackColor = true;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(11, 246);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(138, 13);
            this.Label5.TabIndex = 6;
            this.Label5.Text = "Default output raster format:";
            // 
            // cboFormat
            // 
            this.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Items.AddRange(new object[] {
            "ESRI GeoTIFF"});
            this.cboFormat.Location = new System.Drawing.Point(166, 242);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(170, 21);
            this.cboFormat.TabIndex = 7;
            // 
            // chkClearWorkspaceOnStartup
            // 
            this.chkClearWorkspaceOnStartup.AutoSize = true;
            this.chkClearWorkspaceOnStartup.Checked = true;
            this.chkClearWorkspaceOnStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearWorkspaceOnStartup.Location = new System.Drawing.Point(11, 97);
            this.chkClearWorkspaceOnStartup.Name = "chkClearWorkspaceOnStartup";
            this.chkClearWorkspaceOnStartup.Size = new System.Drawing.Size(155, 17);
            this.chkClearWorkspaceOnStartup.TabIndex = 1;
            this.chkClearWorkspaceOnStartup.Text = "Clear workspace on startup";
            this.chkClearWorkspaceOnStartup.UseVisualStyleBackColor = true;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.grdSurveyTypes);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(500, 283);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Survey Types";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // SurveyTypesBindingSource
            // 
            this.SurveyTypesBindingSource.AllowNew = true;
            this.SurveyTypesBindingSource.DataMember = "SurveyTypes";
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.grbTransparencyLayer);
            this.TabPage3.Controls.Add(this.chkAutoApplyTransparency);
            this.TabPage3.Controls.Add(this.grbComparitiveLayers);
            this.TabPage3.Controls.Add(this.chkComparativeSymbology);
            this.TabPage3.Controls.Add(this.cboLayer);
            this.TabPage3.Controls.Add(this.cboType);
            this.TabPage3.Controls.Add(this.Label2);
            this.TabPage3.Controls.Add(this.Label1);
            this.TabPage3.Controls.Add(this.btnSet);
            this.TabPage3.Controls.Add(this.btnSymReset);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(500, 283);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Symbology";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // grbTransparencyLayer
            // 
            this.grbTransparencyLayer.Controls.Add(this.chkErrorSurfacesTransparency);
            this.grbTransparencyLayer.Controls.Add(this.Label7);
            this.grbTransparencyLayer.Controls.Add(this.nudTransparency);
            this.grbTransparencyLayer.Controls.Add(this.chkAnalysesTransparency);
            this.grbTransparencyLayer.Controls.Add(this.chkAssociatedSurfacesTransparency);
            this.grbTransparencyLayer.Location = new System.Drawing.Point(48, 133);
            this.grbTransparencyLayer.Margin = new System.Windows.Forms.Padding(2);
            this.grbTransparencyLayer.Name = "grbTransparencyLayer";
            this.grbTransparencyLayer.Padding = new System.Windows.Forms.Padding(2);
            this.grbTransparencyLayer.Size = new System.Drawing.Size(176, 119);
            this.grbTransparencyLayer.TabIndex = 9;
            this.grbTransparencyLayer.TabStop = false;
            this.grbTransparencyLayer.Text = "Transparency Layer";
            // 
            // chkErrorSurfacesTransparency
            // 
            this.chkErrorSurfacesTransparency.AutoSize = true;
            this.chkErrorSurfacesTransparency.Location = new System.Drawing.Point(19, 65);
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
            this.Label7.Location = new System.Drawing.Point(21, 23);
            this.Label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(72, 13);
            this.Label7.TabIndex = 3;
            this.Label7.Text = "Transparency";
            // 
            // nudTransparency
            // 
            this.nudTransparency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudTransparency.Location = new System.Drawing.Point(103, 23);
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
            // chkAnalysesTransparency
            // 
            this.chkAnalysesTransparency.AutoSize = true;
            this.chkAnalysesTransparency.Location = new System.Drawing.Point(19, 85);
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
            this.chkAssociatedSurfacesTransparency.Location = new System.Drawing.Point(19, 46);
            this.chkAssociatedSurfacesTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.chkAssociatedSurfacesTransparency.Name = "chkAssociatedSurfacesTransparency";
            this.chkAssociatedSurfacesTransparency.Size = new System.Drawing.Size(123, 17);
            this.chkAssociatedSurfacesTransparency.TabIndex = 0;
            this.chkAssociatedSurfacesTransparency.Text = "Associated Surfaces";
            this.chkAssociatedSurfacesTransparency.UseVisualStyleBackColor = true;
            // 
            // chkAutoApplyTransparency
            // 
            this.chkAutoApplyTransparency.AutoSize = true;
            this.chkAutoApplyTransparency.Location = new System.Drawing.Point(48, 111);
            this.chkAutoApplyTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoApplyTransparency.Name = "chkAutoApplyTransparency";
            this.chkAutoApplyTransparency.Size = new System.Drawing.Size(140, 17);
            this.chkAutoApplyTransparency.TabIndex = 8;
            this.chkAutoApplyTransparency.Text = "Auto apply transparency";
            this.chkAutoApplyTransparency.UseVisualStyleBackColor = true;
            // 
            // grbComparitiveLayers
            // 
            this.grbComparitiveLayers.Controls.Add(this.chkPointDensityComparative);
            this.grbComparitiveLayers.Controls.Add(this.chkDoDComparative);
            this.grbComparitiveLayers.Controls.Add(this.chkFISErrorComparative);
            this.grbComparitiveLayers.Controls.Add(this.chkInterpolationErrorComparative);
            this.grbComparitiveLayers.Controls.Add(this.chk3DPointQualityComparative);
            this.grbComparitiveLayers.Enabled = false;
            this.grbComparitiveLayers.Location = new System.Drawing.Point(236, 133);
            this.grbComparitiveLayers.Margin = new System.Windows.Forms.Padding(2);
            this.grbComparitiveLayers.Name = "grbComparitiveLayers";
            this.grbComparitiveLayers.Padding = new System.Windows.Forms.Padding(2);
            this.grbComparitiveLayers.Size = new System.Drawing.Size(256, 119);
            this.grbComparitiveLayers.TabIndex = 7;
            this.grbComparitiveLayers.TabStop = false;
            this.grbComparitiveLayers.Text = "Comparitive Layers";
            // 
            // chkPointDensityComparative
            // 
            this.chkPointDensityComparative.AutoSize = true;
            this.chkPointDensityComparative.Location = new System.Drawing.Point(19, 85);
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
            this.chkDoDComparative.Location = new System.Drawing.Point(180, 17);
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
            this.chkFISErrorComparative.Location = new System.Drawing.Point(180, 53);
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
            this.chkInterpolationErrorComparative.Location = new System.Drawing.Point(19, 53);
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
            this.chk3DPointQualityComparative.Location = new System.Drawing.Point(19, 20);
            this.chk3DPointQualityComparative.Margin = new System.Windows.Forms.Padding(2);
            this.chk3DPointQualityComparative.Name = "chk3DPointQualityComparative";
            this.chk3DPointQualityComparative.Size = new System.Drawing.Size(102, 17);
            this.chk3DPointQualityComparative.TabIndex = 0;
            this.chk3DPointQualityComparative.Text = "3D Point Quality";
            this.chk3DPointQualityComparative.UseVisualStyleBackColor = true;
            // 
            // chkComparativeSymbology
            // 
            this.chkComparativeSymbology.AutoSize = true;
            this.chkComparativeSymbology.Enabled = false;
            this.chkComparativeSymbology.Location = new System.Drawing.Point(236, 111);
            this.chkComparativeSymbology.Margin = new System.Windows.Forms.Padding(2);
            this.chkComparativeSymbology.Name = "chkComparativeSymbology";
            this.chkComparativeSymbology.Size = new System.Drawing.Size(165, 17);
            this.chkComparativeSymbology.TabIndex = 6;
            this.chkComparativeSymbology.Text = "Apply comparative symbology";
            this.chkComparativeSymbology.UseVisualStyleBackColor = true;
            // 
            // cboLayer
            // 
            this.cboLayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLayer.Enabled = false;
            this.cboLayer.FormattingEnabled = true;
            this.cboLayer.Location = new System.Drawing.Point(48, 49);
            this.cboLayer.Name = "cboLayer";
            this.cboLayer.Size = new System.Drawing.Size(446, 21);
            this.cboLayer.TabIndex = 3;
            // 
            // cboType
            // 
            this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboType.Enabled = false;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(48, 15);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(446, 21);
            this.cboType.TabIndex = 1;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Enabled = false;
            this.Label2.Location = new System.Drawing.Point(6, 52);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(36, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Layer:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Enabled = false;
            this.Label1.Location = new System.Drawing.Point(6, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(34, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Type:";
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSet.Enabled = false;
            this.btnSet.Location = new System.Drawing.Point(416, 76);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(78, 23);
            this.btnSet.TabIndex = 5;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // btnSymReset
            // 
            this.btnSymReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSymReset.Enabled = false;
            this.btnSymReset.Location = new System.Drawing.Point(335, 76);
            this.btnSymReset.Name = "btnSymReset";
            this.btnSymReset.Size = new System.Drawing.Size(75, 23);
            this.btnSymReset.TabIndex = 4;
            this.btnSymReset.Text = "Reset";
            this.btnSymReset.UseVisualStyleBackColor = true;
            // 
            // TabPage4
            // 
            this.TabPage4.Controls.Add(this.GroupBox1);
            this.TabPage4.Location = new System.Drawing.Point(4, 22);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage4.Size = new System.Drawing.Size(500, 283);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Graphs";
            this.TabPage4.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.lblWidth);
            this.GroupBox1.Controls.Add(this.numChartHeight);
            this.GroupBox1.Controls.Add(this.lblHeight);
            this.GroupBox1.Controls.Add(this.numChartWidth);
            this.GroupBox1.Location = new System.Drawing.Point(6, 6);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(479, 117);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Default Settings for Automatically Saved Graphs:";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(15, 26);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(73, 13);
            this.lblWidth.TabIndex = 0;
            this.lblWidth.Text = "Width (pixels):";
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
            this.numChartHeight.Size = new System.Drawing.Size(120, 20);
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
            this.lblHeight.Location = new System.Drawing.Point(16, 52);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(76, 13);
            this.lblHeight.TabIndex = 1;
            this.lblHeight.Text = "Height (pixels):";
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
            this.numChartWidth.Size = new System.Drawing.Size(120, 20);
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
            this.TabPage5.Size = new System.Drawing.Size(500, 283);
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
            this.lstPyramids.Size = new System.Drawing.Size(467, 229);
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
            this.grdSurveyTypes.Size = new System.Drawing.Size(494, 277);
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
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 363);
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
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SurveyTypesBindingSource)).EndInit();
            this.TabPage3.ResumeLayout(false);
            this.TabPage3.PerformLayout();
            this.grbTransparencyLayer.ResumeLayout(false);
            this.grbTransparencyLayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).EndInit();
            this.grbComparitiveLayers.ResumeLayout(false);
            this.grbComparitiveLayers.PerformLayout();
            this.TabPage4.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChartHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChartWidth)).EndInit();
            this.TabPage5.ResumeLayout(false);
            this.TabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSurveyTypes)).EndInit();
            this.ResumeLayout(false);

		}
        internal System.Windows.Forms.Button btnHelp;
        internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.TabControl TabControl1;
		internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TextBox txtWorkspace;
		internal System.Windows.Forms.CheckBox chkClearWorkspaceOnStartup;
        internal System.Windows.Forms.Button btnClearWorkspace;
        internal System.Windows.Forms.Button btnBrowse;
		internal System.Windows.Forms.TabPage TabPage2;
		internal System.Windows.Forms.TabPage TabPage3;
		internal System.Windows.Forms.Button btnSet;
		internal System.Windows.Forms.Button btnSymReset;
		internal System.Windows.Forms.ComboBox cboLayer;
		internal System.Windows.Forms.ComboBox cboType;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.ToolTip ttpTooltip;
		internal System.Windows.Forms.BindingSource SurveyTypesBindingSource;
		internal System.Windows.Forms.DataGridViewTextBoxColumn NameDataGridViewTextBoxColumn;
		internal System.Windows.Forms.DataGridViewTextBoxColumn ErrorDataGridViewTextBoxColumn;
		internal System.Windows.Forms.DataGridViewTextBoxColumn TypeIDDataGridViewTextBoxColumn;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.ComboBox cboFormat;
		internal System.Windows.Forms.CheckBox chkAddOutputLayersToMap;
		internal System.Windows.Forms.CheckBox chkAddInputLayersToMap;
		internal System.Windows.Forms.TabPage TabPage4;
		internal System.Windows.Forms.Label lblHeight;
		internal System.Windows.Forms.Label lblWidth;
		internal System.Windows.Forms.NumericUpDown numChartHeight;
		internal System.Windows.Forms.NumericUpDown numChartWidth;
        internal System.Windows.Forms.Button btnExploreWorkspace;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.CheckBox chkWarnAboutLongPaths;
		internal System.Windows.Forms.CheckBox chkBoxValidateProjectOnLoad;
		internal System.Windows.Forms.CheckBox chkAutoLoadEtalFIS;
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
		internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Button cmdDefault;
		internal System.Windows.Forms.TabPage TabPage5;
        internal System.Windows.Forms.LinkLabel lnkPyramidsHelp;
		internal System.Windows.Forms.CheckedListBox lstPyramids;
		internal System.Windows.Forms.CheckBox chkTempWorkspaceWarning;
        private System.Windows.Forms.DataGridView grdSurveyTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSurveyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUncertainty;
    }
}
