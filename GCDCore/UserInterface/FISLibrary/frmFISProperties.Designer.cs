namespace GCDCore.UserInterface.FISLibrary
{
    partial class frmFISProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFISProperties));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOutputDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOutputUnits = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpInputs = new System.Windows.Forms.GroupBox();
            this.grdInputs = new System.Windows.Forms.DataGridView();
            this.colInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmdEditFISFile = new System.Windows.Forms.Button();
            this.cmdSaveFISFile = new System.Windows.Forms.Button();
            this.txtFISFile = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.grdPublications = new System.Windows.Forms.DataGridView();
            this.colCitation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.grdDatasets = new System.Windows.Forms.DataGridView();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.grdMetaData = new System.Windows.Forms.DataGridView();
            this.colProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInputs)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPublications)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDatasets)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMetaData)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(430, 545);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(349, 545);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "button2";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 545);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(493, 527);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.grpInputs);
            this.tabPage1.Controls.Add(this.txtFilePath);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(485, 501);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtOutputDescription);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtOutputUnits);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtOutputName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 395);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(473, 97);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // txtOutputDescription
            // 
            this.txtOutputDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputDescription.Location = new System.Drawing.Point(71, 68);
            this.txtOutputDescription.Name = "txtOutputDescription";
            this.txtOutputDescription.Size = new System.Drawing.Size(396, 20);
            this.txtOutputDescription.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Description";
            // 
            // txtOutputUnits
            // 
            this.txtOutputUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputUnits.Location = new System.Drawing.Point(71, 40);
            this.txtOutputUnits.Name = "txtOutputUnits";
            this.txtOutputUnits.Size = new System.Drawing.Size(396, 20);
            this.txtOutputUnits.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Units";
            // 
            // txtOutputName
            // 
            this.txtOutputName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputName.Location = new System.Drawing.Point(71, 12);
            this.txtOutputName.Name = "txtOutputName";
            this.txtOutputName.Size = new System.Drawing.Size(396, 20);
            this.txtOutputName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Name";
            // 
            // grpInputs
            // 
            this.grpInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInputs.Controls.Add(this.grdInputs);
            this.grpInputs.Location = new System.Drawing.Point(6, 75);
            this.grpInputs.Name = "grpInputs";
            this.grpInputs.Size = new System.Drawing.Size(473, 314);
            this.grpInputs.TabIndex = 4;
            this.grpInputs.TabStop = false;
            this.grpInputs.Text = "Inputs";
            // 
            // grdInputs
            // 
            this.grdInputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInputName,
            this.colInputUnits,
            this.colInputDescription,
            this.colInputSource});
            this.grdInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInputs.Location = new System.Drawing.Point(3, 16);
            this.grdInputs.Name = "grdInputs";
            this.grdInputs.Size = new System.Drawing.Size(467, 295);
            this.grdInputs.TabIndex = 0;
            // 
            // colInputName
            // 
            this.colInputName.DataPropertyName = "Title";
            this.colInputName.HeaderText = "Name";
            this.colInputName.Name = "colInputName";
            // 
            // colInputUnits
            // 
            this.colInputUnits.DataPropertyName = "Units";
            this.colInputUnits.HeaderText = "Units";
            this.colInputUnits.Name = "colInputUnits";
            // 
            // colInputDescription
            // 
            this.colInputDescription.DataPropertyName = "Description";
            this.colInputDescription.HeaderText = "Description";
            this.colInputDescription.Name = "colInputDescription";
            // 
            // colInputSource
            // 
            this.colInputSource.DataPropertyName = "Source";
            this.colInputSource.HeaderText = "Source";
            this.colInputSource.Name = "colInputSource";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.Location = new System.Drawing.Point(77, 47);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(396, 20);
            this.txtFilePath.TabIndex = 3;
            this.txtFilePath.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "File file";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(77, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(396, 20);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cmdEditFISFile);
            this.tabPage2.Controls.Add(this.cmdSaveFISFile);
            this.tabPage2.Controls.Add(this.txtFISFile);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(485, 501);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "FIS Definition";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmdEditFISFile
            // 
            this.cmdEditFISFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEditFISFile.Image = global::GCDCore.Properties.Resources.edit;
            this.cmdEditFISFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdEditFISFile.Location = new System.Drawing.Point(385, 6);
            this.cmdEditFISFile.Name = "cmdEditFISFile";
            this.cmdEditFISFile.Size = new System.Drawing.Size(23, 23);
            this.cmdEditFISFile.TabIndex = 2;
            this.cmdEditFISFile.UseVisualStyleBackColor = true;
            this.cmdEditFISFile.Click += new System.EventHandler(this.cmdEditFISFile_Click);
            // 
            // cmdSaveFISFile
            // 
            this.cmdSaveFISFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSaveFISFile.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdSaveFISFile.Location = new System.Drawing.Point(413, 6);
            this.cmdSaveFISFile.Name = "cmdSaveFISFile";
            this.cmdSaveFISFile.Size = new System.Drawing.Size(23, 23);
            this.cmdSaveFISFile.TabIndex = 1;
            this.cmdSaveFISFile.UseVisualStyleBackColor = true;
            this.cmdSaveFISFile.Click += new System.EventHandler(this.cmdSaveFISFile_Click);
            // 
            // txtFISFile
            // 
            this.txtFISFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFISFile.Location = new System.Drawing.Point(6, 35);
            this.txtFISFile.Multiline = true;
            this.txtFISFile.Name = "txtFISFile";
            this.txtFISFile.Size = new System.Drawing.Size(430, 253);
            this.txtFISFile.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.grdPublications);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(485, 501);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Publications";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // grdPublications
            // 
            this.grdPublications.AllowUserToResizeRows = false;
            this.grdPublications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPublications.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCitation,
            this.colPURL});
            this.grdPublications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPublications.Location = new System.Drawing.Point(3, 3);
            this.grdPublications.Name = "grdPublications";
            this.grdPublications.Size = new System.Drawing.Size(479, 495);
            this.grdPublications.TabIndex = 0;
            // 
            // colCitation
            // 
            this.colCitation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCitation.DataPropertyName = "Title";
            this.colCitation.HeaderText = "Citation";
            this.colCitation.Name = "colCitation";
            // 
            // colPURL
            // 
            this.colPURL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPURL.DataPropertyName = "URLString";
            this.colPURL.HeaderText = "URL";
            this.colPURL.Name = "colPURL";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.grdDatasets);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(485, 501);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Example Datasets";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // grdDatasets
            // 
            this.grdDatasets.AllowUserToResizeRows = false;
            this.grdDatasets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDatasets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTitle,
            this.colURL});
            this.grdDatasets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDatasets.Location = new System.Drawing.Point(3, 3);
            this.grdDatasets.Name = "grdDatasets";
            this.grdDatasets.Size = new System.Drawing.Size(479, 495);
            this.grdDatasets.TabIndex = 0;
            // 
            // colTitle
            // 
            this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            // 
            // colURL
            // 
            this.colURL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colURL.DataPropertyName = "URLString";
            this.colURL.HeaderText = "URL";
            this.colURL.Name = "colURL";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.grdMetaData);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(485, 501);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Metadata";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // grdMetaData
            // 
            this.grdMetaData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMetaData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProperty,
            this.colValue});
            this.grdMetaData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMetaData.Location = new System.Drawing.Point(3, 3);
            this.grdMetaData.Name = "grdMetaData";
            this.grdMetaData.Size = new System.Drawing.Size(479, 495);
            this.grdMetaData.TabIndex = 0;
            // 
            // colProperty
            // 
            this.colProperty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProperty.DataPropertyName = "Title";
            this.colProperty.HeaderText = "Property";
            this.colProperty.Name = "colProperty";
            this.colProperty.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.DataPropertyName = "Value";
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.txtDescription);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(485, 501);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Description";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 3);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(479, 495);
            this.txtDescription.TabIndex = 0;
            // 
            // frmFISProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(517, 580);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFISProperties";
            this.Text = "FIS Properties";
            this.Load += new System.EventHandler(this.frmFISProperties_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpInputs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInputs)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPublications)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDatasets)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMetaData)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView grdDatasets;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colURL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtOutputDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOutputUnits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOutputName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpInputs;
        private System.Windows.Forms.DataGridView grdInputs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputSource;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFISFile;
        private System.Windows.Forms.DataGridView grdPublications;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCitation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPURL;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataGridView grdMetaData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.Button cmdEditFISFile;
        private System.Windows.Forms.Button cmdSaveFISFile;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox txtDescription;
    }
}