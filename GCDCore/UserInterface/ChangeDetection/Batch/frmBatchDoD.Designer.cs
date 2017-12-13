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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.ucDoDDEMSelection1 = new GCDCore.UserInterface.ChangeDetection.ucDoDDEMSelection();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.grdMethods = new System.Windows.Forms.DataGridView();
            this.colMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThresholding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMethods)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(542, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(461, 356);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Calculate";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 356);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // ucDoDDEMSelection1
            // 
            this.ucDoDDEMSelection1.Location = new System.Drawing.Point(12, 12);
            this.ucDoDDEMSelection1.Name = "ucDoDDEMSelection1";
            this.ucDoDDEMSelection1.NewDEM = null;
            this.ucDoDDEMSelection1.OldDEM = null;
            this.ucDoDDEMSelection1.Size = new System.Drawing.Size(605, 89);
            this.ucDoDDEMSelection1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.cmdDelete);
            this.groupBox1.Controls.Add(this.grdMethods);
            this.groupBox1.Location = new System.Drawing.Point(12, 107);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(605, 243);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Uncertainty Analysis Method";
            // 
            // button3
            // 
            this.button3.Image = global::GCDCore.Properties.Resources.Add;
            this.button3.Location = new System.Drawing.Point(549, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(23, 23);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Image = global::GCDCore.Properties.Resources.Delete;
            this.cmdDelete.Location = new System.Drawing.Point(577, 12);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(23, 23);
            this.cmdDelete.TabIndex = 1;
            this.cmdDelete.UseVisualStyleBackColor = true;
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
            this.colMethod,
            this.colThresholding});
            this.grdMethods.Location = new System.Drawing.Point(7, 41);
            this.grdMethods.Name = "grdMethods";
            this.grdMethods.ReadOnly = true;
            this.grdMethods.RowHeadersVisible = false;
            this.grdMethods.Size = new System.Drawing.Size(593, 196);
            this.grdMethods.TabIndex = 0;
            // 
            // colMethod
            // 
            this.colMethod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMethod.DataPropertyName = "Method";
            this.colMethod.HeaderText = "Method";
            this.colMethod.Name = "colMethod";
            this.colMethod.ReadOnly = true;
            // 
            // colThresholding
            // 
            this.colThresholding.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colThresholding.DataPropertyName = "Thresholding";
            this.colThresholding.HeaderText = "Thresholding";
            this.colThresholding.Name = "colThresholding";
            this.colThresholding.ReadOnly = true;
            // 
            // frmBatchDoD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 391);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ucDoDDEMSelection1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBatchDoD";
            this.Text = "Batch Change Detection";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMethods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button cmdHelp;
        private ucDoDDEMSelection ucDoDDEMSelection1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdMethods;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThresholding;
    }
}