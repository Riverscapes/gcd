namespace GCDCore.UserInterface.Project
{
    partial class frmProjectProperties
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProjectProperties));
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdMetaData = new System.Windows.Forms.DataGridView();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.cboVerticalUnits = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.grpRasterUnits = new System.Windows.Forms.GroupBox();
            this.cboHorizontalUnits = new System.Windows.Forms.ComboBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.cboVolumeUnits = new System.Windows.Forms.ComboBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.cboAreaUnits = new System.Windows.Forms.ComboBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.groGCDUnits = new System.Windows.Forms.GroupBox();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.txtGCDPath = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.ttpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdMetaData)).BeginInit();
            this.TabPage2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.grpRasterUnits.SuspendLayout();
            this.groGCDUnits.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.DataPropertyName = "Value";
            this.colValue.HeaderText = "Value";
            this.colValue.MaxInputLength = 255;
            this.colValue.Name = "colValue";
            // 
            // grdMetaData
            // 
            this.grdMetaData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMetaData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKey,
            this.colValue});
            this.grdMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMetaData.Location = new System.Drawing.Point(3, 3);
            this.grdMetaData.Name = "grdMetaData";
            this.grdMetaData.Size = new System.Drawing.Size(466, 166);
            this.grdMetaData.TabIndex = 0;
            // 
            // colKey
            // 
            this.colKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colKey.DataPropertyName = "Key";
            this.colKey.HeaderText = "Key";
            this.colKey.MaxInputLength = 255;
            this.colKey.Name = "colKey";
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.txtDescription);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(472, 172);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Description";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.AcceptsReturn = true;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 3);
            this.txtDescription.MaxLength = 1000;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(466, 166);
            this.txtDescription.TabIndex = 8;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.grdMetaData);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(472, 172);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "MetaData";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // cboVerticalUnits
            // 
            this.cboVerticalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVerticalUnits.FormattingEnabled = true;
            this.cboVerticalUnits.Location = new System.Drawing.Point(65, 46);
            this.cboVerticalUnits.Name = "cboVerticalUnits";
            this.cboVerticalUnits.Size = new System.Drawing.Size(372, 21);
            this.cboVerticalUnits.TabIndex = 3;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(18, 50);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(42, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Vertical";
            // 
            // grpRasterUnits
            // 
            this.grpRasterUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRasterUnits.Controls.Add(this.cboVerticalUnits);
            this.grpRasterUnits.Controls.Add(this.Label2);
            this.grpRasterUnits.Controls.Add(this.cboHorizontalUnits);
            this.grpRasterUnits.Controls.Add(this.Label6);
            this.grpRasterUnits.Location = new System.Drawing.Point(6, 6);
            this.grpRasterUnits.Name = "grpRasterUnits";
            this.grpRasterUnits.Size = new System.Drawing.Size(460, 76);
            this.grpRasterUnits.TabIndex = 0;
            this.grpRasterUnits.TabStop = false;
            this.grpRasterUnits.Text = "Raster Units";
            // 
            // cboHorizontalUnits
            // 
            this.cboHorizontalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHorizontalUnits.FormattingEnabled = true;
            this.cboHorizontalUnits.Location = new System.Drawing.Point(65, 19);
            this.cboHorizontalUnits.Name = "cboHorizontalUnits";
            this.cboHorizontalUnits.Size = new System.Drawing.Size(372, 21);
            this.cboHorizontalUnits.TabIndex = 1;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(6, 23);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(54, 13);
            this.Label6.TabIndex = 0;
            this.Label6.Text = "Horizontal";
            // 
            // cboVolumeUnits
            // 
            this.cboVolumeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVolumeUnits.FormattingEnabled = true;
            this.cboVolumeUnits.Location = new System.Drawing.Point(65, 46);
            this.cboVolumeUnits.Name = "cboVolumeUnits";
            this.cboVolumeUnits.Size = new System.Drawing.Size(372, 21);
            this.cboVolumeUnits.TabIndex = 3;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(18, 50);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(42, 13);
            this.Label7.TabIndex = 2;
            this.Label7.Text = "Volume";
            // 
            // cboAreaUnits
            // 
            this.cboAreaUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAreaUnits.FormattingEnabled = true;
            this.cboAreaUnits.Location = new System.Drawing.Point(65, 19);
            this.cboAreaUnits.Name = "cboAreaUnits";
            this.cboAreaUnits.Size = new System.Drawing.Size(372, 21);
            this.cboAreaUnits.TabIndex = 1;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(31, 23);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(29, 13);
            this.Label8.TabIndex = 0;
            this.Label8.Text = "Area";
            // 
            // groGCDUnits
            // 
            this.groGCDUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groGCDUnits.Controls.Add(this.cboVolumeUnits);
            this.groGCDUnits.Controls.Add(this.Label7);
            this.groGCDUnits.Controls.Add(this.cboAreaUnits);
            this.groGCDUnits.Controls.Add(this.Label8);
            this.groGCDUnits.Location = new System.Drawing.Point(6, 88);
            this.groGCDUnits.Name = "groGCDUnits";
            this.groGCDUnits.Size = new System.Drawing.Size(460, 76);
            this.groGCDUnits.TabIndex = 1;
            this.groGCDUnits.TabStop = false;
            this.groGCDUnits.Text = "GCD Display Units";
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.groGCDUnits);
            this.TabPage1.Controls.Add(this.grpRasterUnits);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(472, 172);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Units";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage3);
            this.TabControl1.Location = new System.Drawing.Point(11, 110);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(480, 198);
            this.TabControl1.TabIndex = 18;
            // 
            // txtGCDPath
            // 
            this.txtGCDPath.Location = new System.Drawing.Point(97, 74);
            this.txtGCDPath.Name = "txtGCDPath";
            this.txtGCDPath.ReadOnly = true;
            this.txtGCDPath.Size = new System.Drawing.Size(394, 20);
            this.txtGCDPath.TabIndex = 17;
            this.txtGCDPath.TabStop = false;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(11, 78);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(81, 13);
            this.Label4.TabIndex = 16;
            this.Label4.Text = "GCD project file";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(11, 322);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 21;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseOutput.Image")));
            this.btnBrowseOutput.Location = new System.Drawing.Point(468, 41);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(23, 23);
            this.btnBrowseOutput.TabIndex = 15;
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(337, 322);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(416, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(97, 42);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(365, 20);
            this.txtDirectory.TabIndex = 14;
            this.txtDirectory.TabStop = false;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(11, 46);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(81, 13);
            this.Label3.TabIndex = 13;
            this.Label3.Text = "Parent directory";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(97, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(394, 20);
            this.txtName.TabIndex = 12;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(57, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 11;
            this.Label1.Text = "Name";
            // 
            // frmProjectProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(503, 357);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.txtGCDPath);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProjectProperties";
            this.Text = "frmProjectProperties";
            this.Load += new System.EventHandler(this.frmProjectProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMetaData)).EndInit();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.TabPage3.ResumeLayout(false);
            this.grpRasterUnits.ResumeLayout(false);
            this.grpRasterUnits.PerformLayout();
            this.groGCDUnits.ResumeLayout(false);
            this.groGCDUnits.PerformLayout();
            this.TabPage1.ResumeLayout(false);
            this.TabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        internal System.Windows.Forms.DataGridView grdMetaData;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TextBox txtDescription;
        internal System.Windows.Forms.TabPage TabPage3;
        internal System.Windows.Forms.ComboBox cboVerticalUnits;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.GroupBox grpRasterUnits;
        internal System.Windows.Forms.ComboBox cboHorizontalUnits;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ComboBox cboVolumeUnits;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.ComboBox cboAreaUnits;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.GroupBox groGCDUnits;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TextBox txtGCDPath;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.ToolTip ttpTooltip;
        internal System.Windows.Forms.Button btnHelp;
        internal System.Windows.Forms.Button btnBrowseOutput;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox txtDirectory;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.Label Label1;
    }
}