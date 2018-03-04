namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    partial class frmMultiMethodError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiMethodError));
            this.ucName = new GCDCore.UserInterface.ucProjectItemName();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboMask = new System.Windows.Forms.ComboBox();
            this.cmdAddMaskToMap = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdRegions = new System.Windows.Forms.DataGridView();
            this.colMaskValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkDefault = new System.Windows.Forms.CheckBox();
            this.cmsEditGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editErrorPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRegions)).BeginInit();
            this.cmsEditGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucName
            // 
            this.ucName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucName.Location = new System.Drawing.Point(12, 12);
            this.ucName.Name = "ucName";
            this.ucName.Size = new System.Drawing.Size(474, 45);
            this.ucName.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(411, 399);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(330, 399);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "Create";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 399);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 8;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mask";
            // 
            // cboMask
            // 
            this.cboMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMask.FormattingEnabled = true;
            this.cboMask.Location = new System.Drawing.Point(79, 65);
            this.cboMask.Name = "cboMask";
            this.cboMask.Size = new System.Drawing.Size(378, 21);
            this.cboMask.TabIndex = 2;
            // 
            // cmdAddMaskToMap
            // 
            this.cmdAddMaskToMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAddMaskToMap.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.cmdAddMaskToMap.Location = new System.Drawing.Point(463, 64);
            this.cmdAddMaskToMap.Name = "cmdAddMaskToMap";
            this.cmdAddMaskToMap.Size = new System.Drawing.Size(23, 23);
            this.cmdAddMaskToMap.TabIndex = 3;
            this.cmdAddMaskToMap.UseVisualStyleBackColor = true;
            this.cmdAddMaskToMap.Click += new System.EventHandler(this.cmdAddMaskToMap_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdRegions);
            this.groupBox1.Location = new System.Drawing.Point(12, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 272);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mas Regions";
            // 
            // grdRegions
            // 
            this.grdRegions.AllowUserToAddRows = false;
            this.grdRegions.AllowUserToDeleteRows = false;
            this.grdRegions.AllowUserToOrderColumns = true;
            this.grdRegions.AllowUserToResizeColumns = false;
            this.grdRegions.AllowUserToResizeRows = false;
            this.grdRegions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRegions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRegions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaskValue,
            this.colErrProperty});
            this.grdRegions.ContextMenuStrip = this.cmsEditGrid;
            this.grdRegions.Location = new System.Drawing.Point(6, 19);
            this.grdRegions.MultiSelect = false;
            this.grdRegions.Name = "grdRegions";
            this.grdRegions.RowHeadersVisible = false;
            this.grdRegions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdRegions.Size = new System.Drawing.Size(462, 247);
            this.grdRegions.TabIndex = 0;
            this.grdRegions.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdRegions_CellMouseDoubleClick);
            this.grdRegions.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdRegions_CellMouseDown);
            // 
            // colMaskValue
            // 
            this.colMaskValue.DataPropertyName = "Name";
            this.colMaskValue.HeaderText = "Mask Value";
            this.colMaskValue.Name = "colMaskValue";
            this.colMaskValue.ReadOnly = true;
            this.colMaskValue.Width = 150;
            // 
            // colErrProperty
            // 
            this.colErrProperty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colErrProperty.DataPropertyName = "ErrorPropertyString";
            this.colErrProperty.HeaderText = "Error Property";
            this.colErrProperty.Name = "colErrProperty";
            this.colErrProperty.ReadOnly = true;
            // 
            // chkDefault
            // 
            this.chkDefault.AutoSize = true;
            this.chkDefault.Location = new System.Drawing.Point(79, 98);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Size = new System.Drawing.Size(125, 17);
            this.chkDefault.TabIndex = 4;
            this.chkDefault.Text = "Default Error Surface";
            this.chkDefault.UseVisualStyleBackColor = true;
            // 
            // cmsEditGrid
            // 
            this.cmsEditGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editErrorPropertyToolStripMenuItem});
            this.cmsEditGrid.Name = "cmsEditGrid";
            this.cmsEditGrid.Size = new System.Drawing.Size(171, 26);
            // 
            // editErrorPropertyToolStripMenuItem
            // 
            this.editErrorPropertyToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Options;
            this.editErrorPropertyToolStripMenuItem.Name = "editErrorPropertyToolStripMenuItem";
            this.editErrorPropertyToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.editErrorPropertyToolStripMenuItem.Text = "Edit Error Property";
            this.editErrorPropertyToolStripMenuItem.Click += new System.EventHandler(this.EditErrorProperty);
            // 
            // frmMultiMethodError
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(498, 434);
            this.Controls.Add(this.chkDefault);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdAddMaskToMap);
            this.Controls.Add(this.cboMask);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.ucName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "frmMultiMethodError";
            this.Text = "Create Error Surface With Mask";
            this.Load += new System.EventHandler(this.frmMultiMethodError_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRegions)).EndInit();
            this.cmsEditGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucProjectItemName ucName;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboMask;
        private System.Windows.Forms.Button cmdAddMaskToMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdRegions;
        private System.Windows.Forms.CheckBox chkDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaskValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrProperty;
        private System.Windows.Forms.ContextMenuStrip cmsEditGrid;
        private System.Windows.Forms.ToolStripMenuItem editErrorPropertyToolStripMenuItem;
    }
}