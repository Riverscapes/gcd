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
			this.DataGridView1 = new System.Windows.Forms.DataGridView();
			this.NameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FISTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.ttpTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.btnFISRepo = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)this.DataGridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.FISTableBindingSource).BeginInit();
			this.SuspendLayout();
			//
			//btnAddFIS
			//
			this.btnAddFIS.Image = Properties.Resources.Add;
			this.btnAddFIS.Location = new System.Drawing.Point(12, 12);
			this.btnAddFIS.Name = "btnAddFIS";
			this.btnAddFIS.Size = new System.Drawing.Size(29, 23);
			this.btnAddFIS.TabIndex = 1;
			this.btnAddFIS.UseVisualStyleBackColor = true;
			//
			//btnEditFIS
			//
			this.btnEditFIS.Image = Properties.Resources.Settings;
			this.btnEditFIS.Location = new System.Drawing.Point(47, 12);
			this.btnEditFIS.Name = "btnEditFIS";
			this.btnEditFIS.Size = new System.Drawing.Size(29, 23);
			this.btnEditFIS.TabIndex = 2;
			this.btnEditFIS.UseVisualStyleBackColor = true;
			//
			//btnDeleteFIS
			//
			this.btnDeleteFIS.Image = Properties.Resources.Delete;
			this.btnDeleteFIS.Location = new System.Drawing.Point(82, 12);
			this.btnDeleteFIS.Name = "btnDeleteFIS";
			this.btnDeleteFIS.Size = new System.Drawing.Size(29, 23);
			this.btnDeleteFIS.TabIndex = 3;
			this.btnDeleteFIS.UseVisualStyleBackColor = true;
			//
			//btnHelp
			//
			this.btnHelp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnHelp.Location = new System.Drawing.Point(12, 227);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 23);
			this.btnHelp.TabIndex = 7;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			//
			//btnClose
			//
			this.btnClose.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(647, 227);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 8;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			//
			//DataGridView1
			//
			this.DataGridView1.AllowUserToAddRows = false;
			this.DataGridView1.AllowUserToDeleteRows = false;
			this.DataGridView1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.DataGridView1.AutoGenerateColumns = false;
			this.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
			this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
				this.NameDataGridViewTextBoxColumn,
				this.PathDataGridViewTextBoxColumn
			});
			this.DataGridView1.DataSource = this.FISTableBindingSource;
			this.DataGridView1.Location = new System.Drawing.Point(12, 41);
			this.DataGridView1.MultiSelect = false;
			this.DataGridView1.Name = "DataGridView1";
			this.DataGridView1.ReadOnly = true;
			this.DataGridView1.RowHeadersVisible = false;
			this.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DataGridView1.Size = new System.Drawing.Size(710, 180);
			this.DataGridView1.TabIndex = 9;
			//
			//NameDataGridViewTextBoxColumn
			//
			this.NameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.NameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.NameDataGridViewTextBoxColumn.FillWeight = 30f;
			this.NameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn";
			this.NameDataGridViewTextBoxColumn.ReadOnly = true;
			//
			//PathDataGridViewTextBoxColumn
			//
			this.PathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.PathDataGridViewTextBoxColumn.DataPropertyName = "Path";
			this.PathDataGridViewTextBoxColumn.FillWeight = 70f;
			this.PathDataGridViewTextBoxColumn.HeaderText = "Path";
			this.PathDataGridViewTextBoxColumn.Name = "PathDataGridViewTextBoxColumn";
			this.PathDataGridViewTextBoxColumn.ReadOnly = true;
			//
			//btnFISRepo
			//
			this.btnFISRepo.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnFISRepo.Location = new System.Drawing.Point(572, 11);
			this.btnFISRepo.Name = "btnFISRepo";
			this.btnFISRepo.Size = new System.Drawing.Size(150, 23);
			this.btnFISRepo.TabIndex = 10;
			this.btnFISRepo.Text = "Visit ET-AL FIS Repository";
			this.btnFISRepo.UseVisualStyleBackColor = true;
			//
			//FISLibraryForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(734, 262);
			this.Controls.Add(this.btnFISRepo);
			this.Controls.Add(this.DataGridView1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnDeleteFIS);
			this.Controls.Add(this.btnEditFIS);
			this.Controls.Add(this.btnAddFIS);
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MinimumSize = new System.Drawing.Size(200, 199);
			this.Name = "FISLibraryForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Fuzzy Inference System Library";
			((System.ComponentModel.ISupportInitialize)this.DataGridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.FISTableBindingSource).EndInit();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button withEventsField_btnAddFIS;
		internal System.Windows.Forms.Button btnAddFIS {
			get { return withEventsField_btnAddFIS; }
			set {
				if (withEventsField_btnAddFIS != null) {
					withEventsField_btnAddFIS.Click -= btnAddFIS_Click;
				}
				withEventsField_btnAddFIS = value;
				if (withEventsField_btnAddFIS != null) {
					withEventsField_btnAddFIS.Click += btnAddFIS_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnEditFIS;
		internal System.Windows.Forms.Button btnEditFIS {
			get { return withEventsField_btnEditFIS; }
			set {
				if (withEventsField_btnEditFIS != null) {
					withEventsField_btnEditFIS.Click -= btnEditFIS_Click;
				}
				withEventsField_btnEditFIS = value;
				if (withEventsField_btnEditFIS != null) {
					withEventsField_btnEditFIS.Click += btnEditFIS_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnDeleteFIS;
		internal System.Windows.Forms.Button btnDeleteFIS {
			get { return withEventsField_btnDeleteFIS; }
			set {
				if (withEventsField_btnDeleteFIS != null) {
					withEventsField_btnDeleteFIS.Click -= btnDeleteFIS_Click;
				}
				withEventsField_btnDeleteFIS = value;
				if (withEventsField_btnDeleteFIS != null) {
					withEventsField_btnDeleteFIS.Click += btnDeleteFIS_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnHelp;
		internal System.Windows.Forms.Button btnHelp {
			get { return withEventsField_btnHelp; }
			set {
				if (withEventsField_btnHelp != null) {
					withEventsField_btnHelp.Click -= btnHelp_Click;
				}
				withEventsField_btnHelp = value;
				if (withEventsField_btnHelp != null) {
					withEventsField_btnHelp.Click += btnHelp_Click;
				}
			}
		}
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.DataGridView DataGridView1;
		internal System.Windows.Forms.BindingSource FISTableBindingSource;
		internal System.Windows.Forms.ToolTip ttpTooltip;
		internal System.Windows.Forms.DataGridViewTextBoxColumn NameDataGridViewTextBoxColumn;
		internal System.Windows.Forms.DataGridViewTextBoxColumn PathDataGridViewTextBoxColumn;
		private System.Windows.Forms.Button withEventsField_btnFISRepo;
		internal System.Windows.Forms.Button btnFISRepo {
			get { return withEventsField_btnFISRepo; }
			set {
				if (withEventsField_btnFISRepo != null) {
					withEventsField_btnFISRepo.Click -= btnFISRepo_Click;
				}
				withEventsField_btnFISRepo = value;
				if (withEventsField_btnFISRepo != null) {
					withEventsField_btnFISRepo.Click += btnFISRepo_Click;
				}
			}
		}
	}
}
