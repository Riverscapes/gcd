namespace GCDCore.UserInterface.ChangeDetection.Batch
{
    partial class frmBatchDoD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatchDoD));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdAdd = new naru.ui.MenuButton();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.grdMethods = new System.Windows.Forms.DataGridView();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.colNewSurface = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldSurface = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAOI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThresholding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdMethods)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(774, 233);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(693, 233);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "Run Batch";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 233);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdd.Image = global::GCDCore.Properties.Resources.Add;
            this.cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdAdd.Location = new System.Drawing.Point(778, 12);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(42, 23);
            this.cmdAdd.TabIndex = 3;
            this.cmdAdd.UseVisualStyleBackColor = true;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.Image = global::GCDCore.Properties.Resources.Delete;
            this.cmdDelete.Location = new System.Drawing.Point(826, 12);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(23, 23);
            this.cmdDelete.TabIndex = 1;
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // grdMethods
            // 
            this.grdMethods.AllowUserToAddRows = false;
            this.grdMethods.AllowUserToDeleteRows = false;
            this.grdMethods.AllowUserToResizeRows = false;
            this.grdMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdMethods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMethods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNewSurface,
            this.colNewError,
            this.colOldSurface,
            this.colOldError,
            this.colAOI,
            this.colThresholding});
            this.grdMethods.Location = new System.Drawing.Point(12, 41);
            this.grdMethods.Name = "grdMethods";
            this.grdMethods.ReadOnly = true;
            this.grdMethods.RowHeadersVisible = false;
            this.grdMethods.Size = new System.Drawing.Size(837, 186);
            this.grdMethods.TabIndex = 0;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // colNewSurface
            // 
            this.colNewSurface.DataPropertyName = "NewSurface";
            this.colNewSurface.HeaderText = "New Surface";
            this.colNewSurface.Name = "colNewSurface";
            this.colNewSurface.ReadOnly = true;
            // 
            // colNewError
            // 
            this.colNewError.DataPropertyName = "NewError";
            this.colNewError.HeaderText = "New Error";
            this.colNewError.Name = "colNewError";
            this.colNewError.ReadOnly = true;
            // 
            // colOldSurface
            // 
            this.colOldSurface.DataPropertyName = "OldSurface";
            this.colOldSurface.HeaderText = "Old Surface";
            this.colOldSurface.Name = "colOldSurface";
            this.colOldSurface.ReadOnly = true;
            // 
            // colOldError
            // 
            this.colOldError.DataPropertyName = "OldError";
            this.colOldError.HeaderText = "Old Error";
            this.colOldError.Name = "colOldError";
            this.colOldError.ReadOnly = true;
            // 
            // colAOI
            // 
            this.colAOI.DataPropertyName = "AOIMask";
            this.colAOI.HeaderText = "AOI";
            this.colAOI.Name = "colAOI";
            this.colAOI.ReadOnly = true;
            // 
            // colThresholding
            // 
            this.colThresholding.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colThresholding.DataPropertyName = "ThresholdProps";
            this.colThresholding.HeaderText = "Thresholding";
            this.colThresholding.Name = "colThresholding";
            this.colThresholding.ReadOnly = true;
            // 
            // frmBatchDoD
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(861, 268);
            this.Controls.Add(this.grdMethods);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "frmBatchDoD";
            this.Text = "Batch Change Detection";
            this.Load += new System.EventHandler(this.frmBatchDoD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMethods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.DataGridView grdMethods;
        private naru.ui.MenuButton cmdAdd;
        private System.Windows.Forms.Button cmdDelete;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewSurface;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewError;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldSurface;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldError;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAOI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThresholding;
    }
}