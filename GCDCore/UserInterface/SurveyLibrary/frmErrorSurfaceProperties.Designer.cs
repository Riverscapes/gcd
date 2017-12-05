namespace GCDCore.UserInterface.SurveyLibrary
{
    partial class frmErrorSurfaceProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorSurfaceProperties));
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.cboFIS = new System.Windows.Forms.ComboBox();
            this.rdoFIS = new System.Windows.Forms.RadioButton();
            this.valUniform = new System.Windows.Forms.NumericUpDown();
            this.rdoUniform = new System.Windows.Forms.RadioButton();
            this.cboAssociated = new System.Windows.Forms.ComboBox();
            this.rdoAssociated = new System.Windows.Forms.RadioButton();
            this.grdFISInputs = new System.Windows.Forms.DataGridView();
            this.FISInput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssociatedSurface = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.grdErrorProperties = new System.Windows.Forms.DataGridView();
            this.Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtRasterPath = new System.Windows.Forms.TextBox();
            this.chkIsDefault = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.valUniform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFISInputs)).BeginInit();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdErrorProperties)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboFIS
            // 
            this.cboFIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFIS.FormattingEnabled = true;
            this.cboFIS.Location = new System.Drawing.Point(161, 84);
            this.cboFIS.Name = "cboFIS";
            this.cboFIS.Size = new System.Drawing.Size(282, 21);
            this.cboFIS.TabIndex = 5;
            this.cboFIS.SelectedIndexChanged += new System.EventHandler(this.cboFIS_SelectedIndexChanged);
            // 
            // rdoFIS
            // 
            this.rdoFIS.AutoSize = true;
            this.rdoFIS.Location = new System.Drawing.Point(16, 86);
            this.rdoFIS.Name = "rdoFIS";
            this.rdoFIS.Size = new System.Drawing.Size(96, 17);
            this.rdoFIS.TabIndex = 4;
            this.rdoFIS.TabStop = true;
            this.rdoFIS.Text = "FIS error model";
            this.rdoFIS.UseVisualStyleBackColor = true;
            this.rdoFIS.CheckedChanged += new System.EventHandler(this.cboFIS_SelectedIndexChanged);
            // 
            // valUniform
            // 
            this.valUniform.DecimalPlaces = 2;
            this.valUniform.Location = new System.Drawing.Point(161, 25);
            this.valUniform.Name = "valUniform";
            this.valUniform.Size = new System.Drawing.Size(85, 20);
            this.valUniform.TabIndex = 1;
            this.valUniform.ValueChanged += new System.EventHandler(this.valUniform_ValueChanged);
            // 
            // rdoUniform
            // 
            this.rdoUniform.AutoSize = true;
            this.rdoUniform.Checked = true;
            this.rdoUniform.Location = new System.Drawing.Point(16, 27);
            this.rdoUniform.Name = "rdoUniform";
            this.rdoUniform.Size = new System.Drawing.Size(114, 17);
            this.rdoUniform.TabIndex = 0;
            this.rdoUniform.TabStop = true;
            this.rdoUniform.Text = "Uniform error value";
            this.rdoUniform.UseVisualStyleBackColor = true;
            this.rdoUniform.CheckedChanged += new System.EventHandler(this.rdoUniform_CheckedChanged);
            // 
            // cboAssociated
            // 
            this.cboAssociated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssociated.FormattingEnabled = true;
            this.cboAssociated.Location = new System.Drawing.Point(161, 54);
            this.cboAssociated.Name = "cboAssociated";
            this.cboAssociated.Size = new System.Drawing.Size(282, 21);
            this.cboAssociated.TabIndex = 3;
            this.cboAssociated.SelectedIndexChanged += new System.EventHandler(this.cboAssociated_SelectedIndexChanged);
            // 
            // rdoAssociated
            // 
            this.rdoAssociated.AutoSize = true;
            this.rdoAssociated.Location = new System.Drawing.Point(16, 56);
            this.rdoAssociated.Name = "rdoAssociated";
            this.rdoAssociated.Size = new System.Drawing.Size(139, 17);
            this.rdoAssociated.TabIndex = 2;
            this.rdoAssociated.TabStop = true;
            this.rdoAssociated.Text = "Associated error surface";
            this.rdoAssociated.UseVisualStyleBackColor = true;
            this.rdoAssociated.CheckedChanged += new System.EventHandler(this.rdoUniform_CheckedChanged);
            // 
            // grdFISInputs
            // 
            this.grdFISInputs.AllowUserToAddRows = false;
            this.grdFISInputs.AllowUserToDeleteRows = false;
            this.grdFISInputs.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdFISInputs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdFISInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FISInput,
            this.AssociatedSurface});
            this.grdFISInputs.Location = new System.Drawing.Point(34, 116);
            this.grdFISInputs.Name = "grdFISInputs";
            this.grdFISInputs.RowHeadersVisible = false;
            this.grdFISInputs.Size = new System.Drawing.Size(409, 177);
            this.grdFISInputs.TabIndex = 6;
            this.grdFISInputs.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdFISInputs_DataError);
            // 
            // FISInput
            // 
            this.FISInput.DataPropertyName = "FISInputName";
            this.FISInput.HeaderText = "FIS Input";
            this.FISInput.Name = "FISInput";
            this.FISInput.ReadOnly = true;
            this.FISInput.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FISInput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FISInput.Width = 200;
            // 
            // AssociatedSurface
            // 
            this.AssociatedSurface.DataPropertyName = "AssociatedSurface";
            this.AssociatedSurface.HeaderText = "Associated Surface";
            this.AssociatedSurface.Name = "AssociatedSurface";
            this.AssociatedSurface.Width = 200;
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.grdErrorProperties);
            this.GroupBox2.Location = new System.Drawing.Point(17, 106);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(372, 208);
            this.GroupBox2.TabIndex = 5;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Error Calculation Methods";
            // 
            // grdErrorProperties
            // 
            this.grdErrorProperties.AllowUserToAddRows = false;
            this.grdErrorProperties.AllowUserToDeleteRows = false;
            this.grdErrorProperties.AllowUserToResizeRows = false;
            this.grdErrorProperties.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdErrorProperties.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdErrorProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdErrorProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Method,
            this.ErrorType});
            this.grdErrorProperties.Location = new System.Drawing.Point(6, 19);
            this.grdErrorProperties.Name = "grdErrorProperties";
            this.grdErrorProperties.RowHeadersVisible = false;
            this.grdErrorProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdErrorProperties.Size = new System.Drawing.Size(355, 180);
            this.grdErrorProperties.TabIndex = 0;
            this.grdErrorProperties.SelectionChanged += new System.EventHandler(this.grdErrorProperties_SelectionChanged);
            // 
            // Method
            // 
            this.Method.DataPropertyName = "Name";
            this.Method.HeaderText = "Survey Method";
            this.Method.Name = "Method";
            this.Method.ReadOnly = true;
            this.Method.Width = 175;
            // 
            // ErrorType
            // 
            this.ErrorType.DataPropertyName = "ErrorType";
            this.ErrorType.HeaderText = "Error Type";
            this.ErrorType.Name = "ErrorType";
            this.ErrorType.ReadOnly = true;
            this.ErrorType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ErrorType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ErrorType.Width = 175;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.cboAssociated);
            this.GroupBox1.Controls.Add(this.rdoAssociated);
            this.GroupBox1.Controls.Add(this.grdFISInputs);
            this.GroupBox1.Controls.Add(this.cboFIS);
            this.GroupBox1.Controls.Add(this.rdoFIS);
            this.GroupBox1.Controls.Add(this.valUniform);
            this.GroupBox1.Controls.Add(this.rdoUniform);
            this.GroupBox1.Location = new System.Drawing.Point(406, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(453, 302);
            this.GroupBox1.TabIndex = 6;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Error Calculation Definition For Selected Survey Method";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(784, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(659, 328);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(119, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "Derive Error Surface";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Enabled = false;
            this.btnHelp.Location = new System.Drawing.Point(17, 328);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 9;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(19, 41);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(69, 13);
            this.Label4.TabIndex = 2;
            this.Label4.Text = "Project raster";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(92, 12);
            this.txtName.MaxLength = 255;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(297, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(53, 15);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Name";
            // 
            // txtRasterPath
            // 
            this.txtRasterPath.Location = new System.Drawing.Point(92, 38);
            this.txtRasterPath.MaxLength = 255;
            this.txtRasterPath.Name = "txtRasterPath";
            this.txtRasterPath.ReadOnly = true;
            this.txtRasterPath.Size = new System.Drawing.Size(297, 20);
            this.txtRasterPath.TabIndex = 3;
            // 
            // chkIsDefault
            // 
            this.chkIsDefault.AutoSize = true;
            this.chkIsDefault.Location = new System.Drawing.Point(92, 69);
            this.chkIsDefault.Name = "chkIsDefault";
            this.chkIsDefault.Size = new System.Drawing.Size(183, 17);
            this.chkIsDefault.TabIndex = 4;
            this.chkIsDefault.Text = "Default error surface for this DEM";
            this.chkIsDefault.UseVisualStyleBackColor = true;
            // 
            // frmErrorSurfaceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 363);
            this.Controls.Add(this.chkIsDefault);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtRasterPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmErrorSurfaceProperties";
            this.Text = "Error Surface Properties";
            this.Load += new System.EventHandler(this.frmErrorSurfaceProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valUniform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFISInputs)).EndInit();
            this.GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdErrorProperties)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolTip tTip;
        internal System.Windows.Forms.ComboBox cboFIS;
        internal System.Windows.Forms.RadioButton rdoFIS;
        internal System.Windows.Forms.NumericUpDown valUniform;
        internal System.Windows.Forms.RadioButton rdoUniform;
        internal System.Windows.Forms.ComboBox cboAssociated;
        internal System.Windows.Forms.RadioButton rdoAssociated;
        internal System.Windows.Forms.DataGridView grdFISInputs;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.DataGridView grdErrorProperties;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnHelp;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtRasterPath;
        private System.Windows.Forms.CheckBox chkIsDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn Method;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FISInput;
        private System.Windows.Forms.DataGridViewComboBoxColumn AssociatedSurface;
    }
}