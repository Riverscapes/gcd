namespace GCDCore.UserInterface.FISLibrary
{
	partial class frmFISLibrary : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFISLibrary));
            this.btnAddFIS = new System.Windows.Forms.Button();
            this.btnEditFIS = new System.Windows.Forms.Button();
            this.btnDeleteFIS = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdFIS = new System.Windows.Forms.DataGridView();
            this.FISTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ttpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.btnFISRepo = new System.Windows.Forms.Button();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdFIS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FISTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddFIS
            // 
            this.btnAddFIS.Image = global::GCDCore.Properties.Resources.Add;
            this.btnAddFIS.Location = new System.Drawing.Point(12, 12);
            this.btnAddFIS.Name = "btnAddFIS";
            this.btnAddFIS.Size = new System.Drawing.Size(29, 23);
            this.btnAddFIS.TabIndex = 0;
            this.btnAddFIS.UseVisualStyleBackColor = true;
            this.btnAddFIS.Click += new System.EventHandler(this.btnAddFIS_Click);
            // 
            // btnEditFIS
            // 
            this.btnEditFIS.Image = global::GCDCore.Properties.Resources.Settings;
            this.btnEditFIS.Location = new System.Drawing.Point(47, 12);
            this.btnEditFIS.Name = "btnEditFIS";
            this.btnEditFIS.Size = new System.Drawing.Size(29, 23);
            this.btnEditFIS.TabIndex = 1;
            this.btnEditFIS.UseVisualStyleBackColor = true;
            this.btnEditFIS.Click += new System.EventHandler(this.btnEditFIS_Click);
            // 
            // btnDeleteFIS
            // 
            this.btnDeleteFIS.Image = global::GCDCore.Properties.Resources.Delete;
            this.btnDeleteFIS.Location = new System.Drawing.Point(82, 12);
            this.btnDeleteFIS.Name = "btnDeleteFIS";
            this.btnDeleteFIS.Size = new System.Drawing.Size(29, 23);
            this.btnDeleteFIS.TabIndex = 2;
            this.btnDeleteFIS.UseVisualStyleBackColor = true;
            this.btnDeleteFIS.Click += new System.EventHandler(this.btnDeleteFIS_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(12, 258);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(545, 258);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // grdFIS
            // 
            this.grdFIS.AllowUserToAddRows = false;
            this.grdFIS.AllowUserToDeleteRows = false;
            this.grdFIS.AllowUserToResizeRows = false;
            this.grdFIS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFIS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdFIS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdFIS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFIS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colFilePath});
            this.grdFIS.Location = new System.Drawing.Point(12, 41);
            this.grdFIS.MultiSelect = false;
            this.grdFIS.Name = "grdFIS";
            this.grdFIS.ReadOnly = true;
            this.grdFIS.RowHeadersVisible = false;
            this.grdFIS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFIS.Size = new System.Drawing.Size(608, 211);
            this.grdFIS.TabIndex = 4;
            this.grdFIS.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFIS_CellContentDoubleClick);
            this.grdFIS.SelectionChanged += new System.EventHandler(this.UpdateControls);
            // 
            // btnFISRepo
            // 
            this.btnFISRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFISRepo.Location = new System.Drawing.Point(470, 11);
            this.btnFISRepo.Name = "btnFISRepo";
            this.btnFISRepo.Size = new System.Drawing.Size(150, 23);
            this.btnFISRepo.TabIndex = 3;
            this.btnFISRepo.Text = "Visit ET-AL FIS Repository";
            this.btnFISRepo.UseVisualStyleBackColor = true;
            this.btnFISRepo.Click += new System.EventHandler(this.btnFISRepo_Click);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colFilePath
            // 
            this.colFilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFilePath.DataPropertyName = "FilePathString";
            this.colFilePath.HeaderText = "File Path";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            // 
            // frmFISLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 293);
            this.Controls.Add(this.btnFISRepo);
            this.Controls.Add(this.grdFIS);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnDeleteFIS);
            this.Controls.Add(this.btnEditFIS);
            this.Controls.Add(this.btnAddFIS);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "frmFISLibrary";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Fuzzy Inference System Library";
            this.Load += new System.EventHandler(this.FISLibraryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdFIS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FISTableBindingSource)).EndInit();
            this.ResumeLayout(false);

		}
        internal System.Windows.Forms.Button btnAddFIS;
        internal System.Windows.Forms.Button btnEditFIS;
        internal System.Windows.Forms.Button btnDeleteFIS;
        internal System.Windows.Forms.Button btnHelp;
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.DataGridView grdFIS;
		internal System.Windows.Forms.BindingSource FISTableBindingSource;
		internal System.Windows.Forms.ToolTip ttpTooltip;
		internal System.Windows.Forms.DataGridViewTextBoxColumn NameDataGridViewTextBoxColumn;
		internal System.Windows.Forms.DataGridViewTextBoxColumn PathDataGridViewTextBoxColumn;
        internal System.Windows.Forms.Button btnFISRepo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilePath;
    }
}
