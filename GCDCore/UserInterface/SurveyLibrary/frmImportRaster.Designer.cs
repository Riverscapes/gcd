namespace GCDCore.UserInterface.SurveyLibrary
{
    partial class frmImportRaster : System.Windows.Forms.Form
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.lblRasterPath = new System.Windows.Forms.Label();
            this.txtRasterPath = new System.Windows.Forms.TextBox();
            this.grpOriginalRaster = new System.Windows.Forms.GroupBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.txtOrigCellSize = new System.Windows.Forms.TextBox();
            this.txtOrigRows = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtBottom = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtOrigCols = new System.Windows.Forms.TextBox();
            this.txtLeft = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.txtOrigWidth = new System.Windows.Forms.TextBox();
            this.txtRight = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.txtOrigHeight = new System.Windows.Forms.TextBox();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.ucRaster = new GCDCore.UserInterface.UtilityForms.ucRasterInput();
            this.grpProjectRaaster = new System.Windows.Forms.GroupBox();
            this.cmdHelpPrecision = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.valPrecision = new System.Windows.Forms.NumericUpDown();
            this.valCellSize = new System.Windows.Forms.NumericUpDown();
            this.valBottom = new System.Windows.Forms.NumericUpDown();
            this.valRight = new System.Windows.Forms.NumericUpDown();
            this.valLeft = new System.Windows.Forms.NumericUpDown();
            this.valTop = new System.Windows.Forms.NumericUpDown();
            this.lblPrecision = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.lblCellResolution = new System.Windows.Forms.Label();
            this.Label19 = new System.Windows.Forms.Label();
            this.Label20 = new System.Windows.Forms.Label();
            this.txtProjRows = new System.Windows.Forms.TextBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.txtProjCols = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.txtProjHeight = new System.Windows.Forms.TextBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.txtProjWidth = new System.Windows.Forms.TextBox();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.grpOriginalRaster.SuspendLayout();
            this.grpProjectRaaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valCellSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valTop)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(71, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(113, 9);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(491, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.UpdateRasterPath);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(24, 25);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(71, 13);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Original raster";
            // 
            // lblRasterPath
            // 
            this.lblRasterPath.AutoSize = true;
            this.lblRasterPath.Location = new System.Drawing.Point(8, 25);
            this.lblRasterPath.Name = "lblRasterPath";
            this.lblRasterPath.Size = new System.Drawing.Size(93, 13);
            this.lblRasterPath.TabIndex = 0;
            this.lblRasterPath.Text = "Project raster path";
            // 
            // txtRasterPath
            // 
            this.txtRasterPath.Location = new System.Drawing.Point(107, 21);
            this.txtRasterPath.Name = "txtRasterPath";
            this.txtRasterPath.ReadOnly = true;
            this.txtRasterPath.Size = new System.Drawing.Size(456, 20);
            this.txtRasterPath.TabIndex = 1;
            this.txtRasterPath.TabStop = false;
            // 
            // grpOriginalRaster
            // 
            this.grpOriginalRaster.Controls.Add(this.Label12);
            this.grpOriginalRaster.Controls.Add(this.txtOrigCellSize);
            this.grpOriginalRaster.Controls.Add(this.txtOrigRows);
            this.grpOriginalRaster.Controls.Add(this.Label4);
            this.grpOriginalRaster.Controls.Add(this.txtBottom);
            this.grpOriginalRaster.Controls.Add(this.Label5);
            this.grpOriginalRaster.Controls.Add(this.Label8);
            this.grpOriginalRaster.Controls.Add(this.txtOrigCols);
            this.grpOriginalRaster.Controls.Add(this.txtLeft);
            this.grpOriginalRaster.Controls.Add(this.Label7);
            this.grpOriginalRaster.Controls.Add(this.Label9);
            this.grpOriginalRaster.Controls.Add(this.txtOrigWidth);
            this.grpOriginalRaster.Controls.Add(this.txtRight);
            this.grpOriginalRaster.Controls.Add(this.Label6);
            this.grpOriginalRaster.Controls.Add(this.Label10);
            this.grpOriginalRaster.Controls.Add(this.txtOrigHeight);
            this.grpOriginalRaster.Controls.Add(this.txtTop);
            this.grpOriginalRaster.Controls.Add(this.Label11);
            this.grpOriginalRaster.Controls.Add(this.Label2);
            this.grpOriginalRaster.Controls.Add(this.ucRaster);
            this.grpOriginalRaster.Location = new System.Drawing.Point(12, 37);
            this.grpOriginalRaster.Name = "grpOriginalRaster";
            this.grpOriginalRaster.Size = new System.Drawing.Size(606, 156);
            this.grpOriginalRaster.TabIndex = 2;
            this.grpOriginalRaster.TabStop = false;
            this.grpOriginalRaster.Text = "Original Raster";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(434, 113);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(72, 13);
            this.Label12.TabIndex = 18;
            this.Label12.Text = "Cell resolution";
            // 
            // txtOrigCellSize
            // 
            this.txtOrigCellSize.Location = new System.Drawing.Point(512, 109);
            this.txtOrigCellSize.Name = "txtOrigCellSize";
            this.txtOrigCellSize.ReadOnly = true;
            this.txtOrigCellSize.Size = new System.Drawing.Size(80, 20);
            this.txtOrigCellSize.TabIndex = 19;
            // 
            // txtOrigRows
            // 
            this.txtOrigRows.Location = new System.Drawing.Point(372, 56);
            this.txtOrigRows.Name = "txtOrigRows";
            this.txtOrigRows.ReadOnly = true;
            this.txtOrigRows.Size = new System.Drawing.Size(80, 20);
            this.txtOrigRows.TabIndex = 11;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(332, 60);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(34, 13);
            this.Label4.TabIndex = 10;
            this.Label4.Text = "Rows";
            // 
            // txtBottom
            // 
            this.txtBottom.Location = new System.Drawing.Point(107, 126);
            this.txtBottom.Name = "txtBottom";
            this.txtBottom.Size = new System.Drawing.Size(100, 20);
            this.txtBottom.TabIndex = 9;
            this.txtBottom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OriginalExtentTextBoxes_KeyPress);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(459, 60);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(47, 13);
            this.Label5.TabIndex = 14;
            this.Label5.Text = "Columns";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(61, 130);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(40, 13);
            this.Label8.TabIndex = 8;
            this.Label8.Text = "Bottom";
            // 
            // txtOrigCols
            // 
            this.txtOrigCols.Location = new System.Drawing.Point(512, 56);
            this.txtOrigCols.Name = "txtOrigCols";
            this.txtOrigCols.ReadOnly = true;
            this.txtOrigCols.Size = new System.Drawing.Size(80, 20);
            this.txtOrigCols.TabIndex = 15;
            // 
            // txtLeft
            // 
            this.txtLeft.Location = new System.Drawing.Point(39, 91);
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(100, 20);
            this.txtLeft.TabIndex = 3;
            this.txtLeft.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OriginalExtentTextBoxes_KeyPress);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(331, 87);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(35, 13);
            this.Label7.TabIndex = 12;
            this.Label7.Text = "Width";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(75, 60);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(26, 13);
            this.Label9.TabIndex = 4;
            this.Label9.Text = "Top";
            // 
            // txtOrigWidth
            // 
            this.txtOrigWidth.Location = new System.Drawing.Point(372, 83);
            this.txtOrigWidth.Name = "txtOrigWidth";
            this.txtOrigWidth.ReadOnly = true;
            this.txtOrigWidth.Size = new System.Drawing.Size(80, 20);
            this.txtOrigWidth.TabIndex = 13;
            // 
            // txtRight
            // 
            this.txtRight.Location = new System.Drawing.Point(200, 91);
            this.txtRight.Name = "txtRight";
            this.txtRight.Size = new System.Drawing.Size(100, 20);
            this.txtRight.TabIndex = 7;
            this.txtRight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OriginalExtentTextBoxes_KeyPress);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(468, 87);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(38, 13);
            this.Label6.TabIndex = 16;
            this.Label6.Text = "Height";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(163, 95);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(32, 13);
            this.Label10.TabIndex = 6;
            this.Label10.Text = "Right";
            // 
            // txtOrigHeight
            // 
            this.txtOrigHeight.Location = new System.Drawing.Point(512, 83);
            this.txtOrigHeight.Name = "txtOrigHeight";
            this.txtOrigHeight.ReadOnly = true;
            this.txtOrigHeight.Size = new System.Drawing.Size(80, 20);
            this.txtOrigHeight.TabIndex = 17;
            // 
            // txtTop
            // 
            this.txtTop.Location = new System.Drawing.Point(107, 56);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(100, 20);
            this.txtTop.TabIndex = 5;
            this.txtTop.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OriginalExtentTextBoxes_KeyPress);
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(8, 95);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(25, 13);
            this.Label11.TabIndex = 2;
            this.Label11.Text = "Left";
            // 
            // ucRaster
            // 
            this.ucRaster.Location = new System.Drawing.Point(101, 19);
            this.ucRaster.Name = "ucRaster";
            this.ucRaster.Size = new System.Drawing.Size(491, 25);
            this.ucRaster.TabIndex = 1;
            // 
            // grpProjectRaaster
            // 
            this.grpProjectRaaster.Controls.Add(this.cmdHelpPrecision);
            this.grpProjectRaaster.Controls.Add(this.Label1);
            this.grpProjectRaaster.Controls.Add(this.cboMethod);
            this.grpProjectRaaster.Controls.Add(this.cmdSave);
            this.grpProjectRaaster.Controls.Add(this.valPrecision);
            this.grpProjectRaaster.Controls.Add(this.valCellSize);
            this.grpProjectRaaster.Controls.Add(this.valBottom);
            this.grpProjectRaaster.Controls.Add(this.valRight);
            this.grpProjectRaaster.Controls.Add(this.valLeft);
            this.grpProjectRaaster.Controls.Add(this.valTop);
            this.grpProjectRaaster.Controls.Add(this.lblPrecision);
            this.grpProjectRaaster.Controls.Add(this.Label17);
            this.grpProjectRaaster.Controls.Add(this.Label18);
            this.grpProjectRaaster.Controls.Add(this.lblCellResolution);
            this.grpProjectRaaster.Controls.Add(this.txtRasterPath);
            this.grpProjectRaaster.Controls.Add(this.Label19);
            this.grpProjectRaaster.Controls.Add(this.lblRasterPath);
            this.grpProjectRaaster.Controls.Add(this.Label20);
            this.grpProjectRaaster.Controls.Add(this.txtProjRows);
            this.grpProjectRaaster.Controls.Add(this.Label21);
            this.grpProjectRaaster.Controls.Add(this.txtProjCols);
            this.grpProjectRaaster.Controls.Add(this.Label14);
            this.grpProjectRaaster.Controls.Add(this.txtProjHeight);
            this.grpProjectRaaster.Controls.Add(this.Label16);
            this.grpProjectRaaster.Controls.Add(this.Label15);
            this.grpProjectRaaster.Controls.Add(this.txtProjWidth);
            this.grpProjectRaaster.Location = new System.Drawing.Point(12, 199);
            this.grpProjectRaaster.Name = "grpProjectRaaster";
            this.grpProjectRaaster.Size = new System.Drawing.Size(606, 196);
            this.grpProjectRaaster.TabIndex = 3;
            this.grpProjectRaaster.TabStop = false;
            this.grpProjectRaaster.Text = "Project Raster";
            // 
            // cmdHelpPrecision
            // 
            this.cmdHelpPrecision.FlatAppearance.BorderSize = 0;
            this.cmdHelpPrecision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdHelpPrecision.Image = global::GCDCore.Properties.Resources.Help;
            this.cmdHelpPrecision.Location = new System.Drawing.Point(343, 129);
            this.cmdHelpPrecision.Name = "cmdHelpPrecision";
            this.cmdHelpPrecision.Size = new System.Drawing.Size(23, 23);
            this.cmdHelpPrecision.TabIndex = 26;
            this.cmdHelpPrecision.UseVisualStyleBackColor = true;
            this.cmdHelpPrecision.Click += new System.EventHandler(this.cmdHelpPrecision_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(260, 161);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(103, 13);
            this.Label1.TabIndex = 25;
            this.Label1.Text = "Interpolation method";
            // 
            // cboMethod
            // 
            this.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Location = new System.Drawing.Point(372, 157);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(220, 21);
            this.cboMethod.TabIndex = 24;
            // 
            // cmdSave
            // 
            this.cmdSave.Image = global::GCDCore.Properties.Resources.SaveGIS;
            this.cmdSave.Location = new System.Drawing.Point(569, 20);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(23, 23);
            this.cmdSave.TabIndex = 23;
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // valPrecision
            // 
            this.valPrecision.Location = new System.Drawing.Point(512, 130);
            this.valPrecision.Name = "valPrecision";
            this.valPrecision.Size = new System.Drawing.Size(80, 20);
            this.valPrecision.TabIndex = 22;
            // 
            // valCellSize
            // 
            this.valCellSize.Location = new System.Drawing.Point(512, 104);
            this.valCellSize.Name = "valCellSize";
            this.valCellSize.Size = new System.Drawing.Size(80, 20);
            this.valCellSize.TabIndex = 19;
            // 
            // valBottom
            // 
            this.valBottom.Location = new System.Drawing.Point(107, 121);
            this.valBottom.Name = "valBottom";
            this.valBottom.Size = new System.Drawing.Size(100, 20);
            this.valBottom.TabIndex = 9;
            // 
            // valRight
            // 
            this.valRight.Location = new System.Drawing.Point(200, 86);
            this.valRight.Name = "valRight";
            this.valRight.Size = new System.Drawing.Size(100, 20);
            this.valRight.TabIndex = 7;
            // 
            // valLeft
            // 
            this.valLeft.Location = new System.Drawing.Point(39, 86);
            this.valLeft.Name = "valLeft";
            this.valLeft.Size = new System.Drawing.Size(100, 20);
            this.valLeft.TabIndex = 3;
            // 
            // valTop
            // 
            this.valTop.Location = new System.Drawing.Point(107, 51);
            this.valTop.Name = "valTop";
            this.valTop.Size = new System.Drawing.Size(100, 20);
            this.valTop.TabIndex = 5;
            // 
            // lblPrecision
            // 
            this.lblPrecision.AutoSize = true;
            this.lblPrecision.Location = new System.Drawing.Point(368, 134);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(138, 13);
            this.lblPrecision.TabIndex = 20;
            this.lblPrecision.Text = "Horizontal decimal precision";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(459, 55);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(47, 13);
            this.Label17.TabIndex = 14;
            this.Label17.Text = "Columns";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(61, 125);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(40, 13);
            this.Label18.TabIndex = 8;
            this.Label18.Text = "Bottom";
            // 
            // lblCellResolution
            // 
            this.lblCellResolution.AutoSize = true;
            this.lblCellResolution.Location = new System.Drawing.Point(434, 108);
            this.lblCellResolution.Name = "lblCellResolution";
            this.lblCellResolution.Size = new System.Drawing.Size(72, 13);
            this.lblCellResolution.TabIndex = 18;
            this.lblCellResolution.Text = "Cell resolution";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(75, 55);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(26, 13);
            this.Label19.TabIndex = 4;
            this.Label19.Text = "Top";
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Location = new System.Drawing.Point(163, 90);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(32, 13);
            this.Label20.TabIndex = 6;
            this.Label20.Text = "Right";
            // 
            // txtProjRows
            // 
            this.txtProjRows.Location = new System.Drawing.Point(372, 51);
            this.txtProjRows.Name = "txtProjRows";
            this.txtProjRows.ReadOnly = true;
            this.txtProjRows.Size = new System.Drawing.Size(80, 20);
            this.txtProjRows.TabIndex = 11;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(8, 90);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(25, 13);
            this.Label21.TabIndex = 2;
            this.Label21.Text = "Left";
            // 
            // txtProjCols
            // 
            this.txtProjCols.Location = new System.Drawing.Point(512, 51);
            this.txtProjCols.Name = "txtProjCols";
            this.txtProjCols.ReadOnly = true;
            this.txtProjCols.Size = new System.Drawing.Size(80, 20);
            this.txtProjCols.TabIndex = 15;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(332, 55);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(34, 13);
            this.Label14.TabIndex = 10;
            this.Label14.Text = "Rows";
            // 
            // txtProjHeight
            // 
            this.txtProjHeight.Location = new System.Drawing.Point(512, 78);
            this.txtProjHeight.Name = "txtProjHeight";
            this.txtProjHeight.ReadOnly = true;
            this.txtProjHeight.Size = new System.Drawing.Size(80, 20);
            this.txtProjHeight.TabIndex = 17;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(468, 82);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(38, 13);
            this.Label16.TabIndex = 16;
            this.Label16.Text = "Height";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(331, 82);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(35, 13);
            this.Label15.TabIndex = 12;
            this.Label15.Text = "Width";
            // 
            // txtProjWidth
            // 
            this.txtProjWidth.Location = new System.Drawing.Point(372, 78);
            this.txtProjWidth.Name = "txtProjWidth";
            this.txtProjWidth.ReadOnly = true;
            this.txtProjWidth.Size = new System.Drawing.Size(80, 20);
            this.txtProjWidth.TabIndex = 13;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(11, 408);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 6;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(419, 408);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(119, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "Import";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(544, 408);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // frmImportRaster
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(631, 443);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.grpProjectRaaster);
            this.Controls.Add(this.grpOriginalRaster);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportRaster";
            this.ShowIcon = false;
            this.Text = "Import Raster";
            this.Load += new System.EventHandler(this.ImportRasterForm_Load);
            this.grpOriginalRaster.ResumeLayout(false);
            this.grpOriginalRaster.PerformLayout();
            this.grpProjectRaaster.ResumeLayout(false);
            this.grpProjectRaaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valCellSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label lblRasterPath;
        internal System.Windows.Forms.TextBox txtRasterPath;
        internal System.Windows.Forms.GroupBox grpOriginalRaster;
        internal System.Windows.Forms.GroupBox grpProjectRaaster;
        internal System.Windows.Forms.Button cmdHelp;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.TextBox txtOrigCellSize;
        internal System.Windows.Forms.TextBox txtOrigRows;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtBottom;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox txtOrigCols;
        internal System.Windows.Forms.TextBox txtLeft;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.TextBox txtOrigWidth;
        internal System.Windows.Forms.TextBox txtRight;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.TextBox txtOrigHeight;
        internal System.Windows.Forms.TextBox txtTop;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.NumericUpDown valCellSize;
        internal System.Windows.Forms.NumericUpDown valBottom;
        internal System.Windows.Forms.NumericUpDown valRight;
        internal System.Windows.Forms.NumericUpDown valLeft;
        internal System.Windows.Forms.NumericUpDown valTop;
        internal System.Windows.Forms.Label lblPrecision;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.Label lblCellResolution;
        internal System.Windows.Forms.Label Label19;
        internal System.Windows.Forms.Label Label20;
        internal System.Windows.Forms.TextBox txtProjRows;
        internal System.Windows.Forms.Label Label21;
        internal System.Windows.Forms.TextBox txtProjCols;
        internal System.Windows.Forms.Label Label14;
        internal System.Windows.Forms.TextBox txtProjHeight;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.Label Label15;
        internal System.Windows.Forms.TextBox txtProjWidth;
        internal System.Windows.Forms.NumericUpDown valPrecision;
        internal System.Windows.Forms.Button cmdSave;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox cboMethod;
        internal System.Windows.Forms.Button cmdHelpPrecision;
        public UtilityForms.ucRasterInput ucRaster;
    }
}