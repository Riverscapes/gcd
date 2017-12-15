namespace GCDCore.UserInterface.ChangeDetection.Intercomparison
{
    partial class frmInterComp
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdMoveUp = new System.Windows.Forms.Button();
            this.cmdMoveDown = new System.Windows.Forms.Button();
            this.grdDEMs = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdEpochs = new System.Windows.Forms.DataGridView();
            this.colNewDEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldDEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkPrevious = new System.Windows.Forms.CheckBox();
            this.chkEarliest = new System.Windows.Forms.CheckBox();
            this.chkNewest = new System.Windows.Forms.CheckBox();
            this.colActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDEMSurvey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrorSurface = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ucThresholding1 = new GCDCore.UserInterface.ChangeDetection.ucThresholding();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDEMs)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEpochs)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(415, 558);
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
            this.cmdOK.Location = new System.Drawing.Point(334, 558);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "Calculate";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 558);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdMoveUp);
            this.groupBox1.Controls.Add(this.cmdMoveDown);
            this.groupBox1.Controls.Add(this.grdDEMs);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 169);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DEM Surveys in Chronological Order";
            // 
            // cmdMoveUp
            // 
            this.cmdMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdMoveUp.Image = global::GCDCore.Properties.Resources.up;
            this.cmdMoveUp.Location = new System.Drawing.Point(449, 19);
            this.cmdMoveUp.Name = "cmdMoveUp";
            this.cmdMoveUp.Size = new System.Drawing.Size(23, 23);
            this.cmdMoveUp.TabIndex = 2;
            this.cmdMoveUp.UseVisualStyleBackColor = true;
            this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
            // 
            // cmdMoveDown
            // 
            this.cmdMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdMoveDown.Image = global::GCDCore.Properties.Resources.down;
            this.cmdMoveDown.Location = new System.Drawing.Point(449, 45);
            this.cmdMoveDown.Name = "cmdMoveDown";
            this.cmdMoveDown.Size = new System.Drawing.Size(23, 23);
            this.cmdMoveDown.TabIndex = 1;
            this.cmdMoveDown.UseVisualStyleBackColor = true;
            this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
            // 
            // grdDEMs
            // 
            this.grdDEMs.AllowUserToAddRows = false;
            this.grdDEMs.AllowUserToDeleteRows = false;
            this.grdDEMs.AllowUserToResizeRows = false;
            this.grdDEMs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDEMs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDEMs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colActive,
            this.colDEMSurvey,
            this.colErrorSurface});
            this.grdDEMs.Location = new System.Drawing.Point(6, 19);
            this.grdDEMs.MultiSelect = false;
            this.grdDEMs.Name = "grdDEMs";
            this.grdDEMs.RowHeadersVisible = false;
            this.grdDEMs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDEMs.Size = new System.Drawing.Size(437, 144);
            this.grdDEMs.TabIndex = 0;
            this.grdDEMs.SelectionChanged += new System.EventHandler(this.grdDEMs_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.grdEpochs);
            this.groupBox2.Controls.Add(this.chkPrevious);
            this.groupBox2.Controls.Add(this.chkEarliest);
            this.groupBox2.Controls.Add(this.chkNewest);
            this.groupBox2.Location = new System.Drawing.Point(12, 359);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 193);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DEM Survey Epochs";
            // 
            // grdEpochs
            // 
            this.grdEpochs.AllowUserToAddRows = false;
            this.grdEpochs.AllowUserToDeleteRows = false;
            this.grdEpochs.AllowUserToResizeRows = false;
            this.grdEpochs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdEpochs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEpochs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNewDEM,
            this.colOldDEM});
            this.grdEpochs.Location = new System.Drawing.Point(14, 83);
            this.grdEpochs.MultiSelect = false;
            this.grdEpochs.Name = "grdEpochs";
            this.grdEpochs.RowHeadersVisible = false;
            this.grdEpochs.Size = new System.Drawing.Size(458, 104);
            this.grdEpochs.TabIndex = 3;
            // 
            // colNewDEM
            // 
            this.colNewDEM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNewDEM.DataPropertyName = "NewDEM";
            this.colNewDEM.HeaderText = "New DEM Survey";
            this.colNewDEM.Name = "colNewDEM";
            this.colNewDEM.ReadOnly = true;
            // 
            // colOldDEM
            // 
            this.colOldDEM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOldDEM.DataPropertyName = "OldDEM";
            this.colOldDEM.HeaderText = "Old DEM Survey";
            this.colOldDEM.Name = "colOldDEM";
            this.colOldDEM.ReadOnly = true;
            // 
            // chkPrevious
            // 
            this.chkPrevious.AutoSize = true;
            this.chkPrevious.Checked = true;
            this.chkPrevious.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrevious.Location = new System.Drawing.Point(14, 60);
            this.chkPrevious.Name = "chkPrevious";
            this.chkPrevious.Size = new System.Drawing.Size(187, 17);
            this.chkPrevious.TabIndex = 2;
            this.chkPrevious.Text = "All DEMs minus the previous DEM";
            this.chkPrevious.UseVisualStyleBackColor = true;
            // 
            // chkEarliest
            // 
            this.chkEarliest.AutoSize = true;
            this.chkEarliest.Checked = true;
            this.chkEarliest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEarliest.Location = new System.Drawing.Point(14, 39);
            this.chkEarliest.Name = "chkEarliest";
            this.chkEarliest.Size = new System.Drawing.Size(180, 17);
            this.chkEarliest.TabIndex = 1;
            this.chkEarliest.Text = "All DEMs minus the earliest DEM";
            this.chkEarliest.UseVisualStyleBackColor = true;
            // 
            // chkNewest
            // 
            this.chkNewest.AutoSize = true;
            this.chkNewest.Checked = true;
            this.chkNewest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNewest.Location = new System.Drawing.Point(14, 20);
            this.chkNewest.Name = "chkNewest";
            this.chkNewest.Size = new System.Drawing.Size(191, 17);
            this.chkNewest.TabIndex = 0;
            this.chkNewest.Text = "Newest DEM minus all other DEMs";
            this.chkNewest.UseVisualStyleBackColor = true;
            // 
            // colActive
            // 
            this.colActive.DataPropertyName = "IsActive";
            this.colActive.HeaderText = "";
            this.colActive.Name = "colActive";
            this.colActive.Width = 30;
            // 
            // colDEMSurvey
            // 
            this.colDEMSurvey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDEMSurvey.DataPropertyName = "DEMName";
            this.colDEMSurvey.HeaderText = "DEM Survey";
            this.colDEMSurvey.Name = "colDEMSurvey";
            this.colDEMSurvey.ReadOnly = true;
            this.colDEMSurvey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDEMSurvey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colErrorSurface
            // 
            this.colErrorSurface.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colErrorSurface.DataPropertyName = "ErrorName";
            this.colErrorSurface.HeaderText = "Error Surface";
            this.colErrorSurface.Name = "colErrorSurface";
            this.colErrorSurface.ReadOnly = true;
            // 
            // ucThresholding1
            // 
            this.ucThresholding1.Location = new System.Drawing.Point(12, 184);
            this.ucThresholding1.Name = "ucThresholding1";
            this.ucThresholding1.Size = new System.Drawing.Size(478, 169);
            this.ucThresholding1.TabIndex = 4;
            // 
            // frmInterComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 593);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ucThresholding1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Name = "frmInterComp";
            this.Text = "DEM Survey Intercomparison";
            this.Load += new System.EventHandler(this.frmInterComp_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDEMs)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEpochs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdMoveUp;
        private System.Windows.Forms.Button cmdMoveDown;
        private System.Windows.Forms.DataGridView grdDEMs;
        private ucThresholding ucThresholding1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdEpochs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewDEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldDEM;
        private System.Windows.Forms.CheckBox chkPrevious;
        private System.Windows.Forms.CheckBox chkEarliest;
        private System.Windows.Forms.CheckBox chkNewest;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDEMSurvey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorSurface;
    }
}