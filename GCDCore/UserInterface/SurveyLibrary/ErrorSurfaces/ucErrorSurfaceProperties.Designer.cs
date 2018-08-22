namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    partial class ucErrorSurfaceProperties
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cboAssociated = new System.Windows.Forms.ComboBox();
            this.rdoAssociated = new System.Windows.Forms.RadioButton();
            this.grdFISInputs = new System.Windows.Forms.DataGridView();
            this.FISInput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssociatedSurface = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cboFIS = new System.Windows.Forms.ComboBox();
            this.rdoFIS = new System.Windows.Forms.RadioButton();
            this.valUniform = new System.Windows.Forms.NumericUpDown();
            this.rdoUniform = new System.Windows.Forms.RadioButton();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmdFISProperties = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdFISInputs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUniform)).BeginInit();
            this.SuspendLayout();
            // 
            // cboAssociated
            // 
            this.cboAssociated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAssociated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssociated.FormattingEnabled = true;
            this.cboAssociated.Location = new System.Drawing.Point(155, 28);
            this.cboAssociated.Name = "cboAssociated";
            this.cboAssociated.Size = new System.Drawing.Size(291, 21);
            this.cboAssociated.TabIndex = 3;
            // 
            // rdoAssociated
            // 
            this.rdoAssociated.AutoSize = true;
            this.rdoAssociated.Location = new System.Drawing.Point(0, 30);
            this.rdoAssociated.Name = "rdoAssociated";
            this.rdoAssociated.Size = new System.Drawing.Size(139, 17);
            this.rdoAssociated.TabIndex = 2;
            this.rdoAssociated.TabStop = true;
            this.rdoAssociated.Text = "Associated error surface";
            this.rdoAssociated.UseVisualStyleBackColor = true;
            // 
            // grdFISInputs
            // 
            this.grdFISInputs.AllowUserToAddRows = false;
            this.grdFISInputs.AllowUserToDeleteRows = false;
            this.grdFISInputs.AllowUserToResizeRows = false;
            this.grdFISInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFISInputs.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdFISInputs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdFISInputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FISInput,
            this.AssociatedSurface});
            this.grdFISInputs.Location = new System.Drawing.Point(28, 90);
            this.grdFISInputs.MultiSelect = false;
            this.grdFISInputs.Name = "grdFISInputs";
            this.grdFISInputs.RowHeadersVisible = false;
            this.grdFISInputs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFISInputs.Size = new System.Drawing.Size(418, 100);
            this.grdFISInputs.TabIndex = 7;
            // 
            // FISInput
            // 
            this.FISInput.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FISInput.DataPropertyName = "FISInputName";
            this.FISInput.HeaderText = "FIS Input";
            this.FISInput.Name = "FISInput";
            this.FISInput.ReadOnly = true;
            this.FISInput.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FISInput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AssociatedSurface
            // 
            this.AssociatedSurface.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AssociatedSurface.DataPropertyName = "AssociatedSurface";
            this.AssociatedSurface.HeaderText = "Associated Surface";
            this.AssociatedSurface.Name = "AssociatedSurface";
            // 
            // cboFIS
            // 
            this.cboFIS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFIS.FormattingEnabled = true;
            this.cboFIS.Location = new System.Drawing.Point(155, 57);
            this.cboFIS.Name = "cboFIS";
            this.cboFIS.Size = new System.Drawing.Size(262, 21);
            this.cboFIS.TabIndex = 5;
            // 
            // rdoFIS
            // 
            this.rdoFIS.AutoSize = true;
            this.rdoFIS.Location = new System.Drawing.Point(0, 59);
            this.rdoFIS.Name = "rdoFIS";
            this.rdoFIS.Size = new System.Drawing.Size(96, 17);
            this.rdoFIS.TabIndex = 4;
            this.rdoFIS.TabStop = true;
            this.rdoFIS.Text = "FIS error model";
            this.rdoFIS.UseVisualStyleBackColor = true;
            // 
            // valUniform
            // 
            this.valUniform.DecimalPlaces = 3;
            this.valUniform.Location = new System.Drawing.Point(155, 0);
            this.valUniform.Name = "valUniform";
            this.valUniform.Size = new System.Drawing.Size(85, 20);
            this.valUniform.TabIndex = 1;
            this.valUniform.Enter += new System.EventHandler(this.valUniform_Enter);
            // 
            // rdoUniform
            // 
            this.rdoUniform.AutoSize = true;
            this.rdoUniform.Checked = true;
            this.rdoUniform.Location = new System.Drawing.Point(0, 0);
            this.rdoUniform.Name = "rdoUniform";
            this.rdoUniform.Size = new System.Drawing.Size(123, 17);
            this.rdoUniform.TabIndex = 0;
            this.rdoUniform.TabStop = true;
            this.rdoUniform.Text = "Uniform error value ()";
            this.rdoUniform.UseVisualStyleBackColor = true;
            // 
            // cmdFISProperties
            // 
            this.cmdFISProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFISProperties.Image = global::GCDCore.Properties.Resources.info;
            this.cmdFISProperties.Location = new System.Drawing.Point(423, 56);
            this.cmdFISProperties.Name = "cmdFISProperties";
            this.cmdFISProperties.Size = new System.Drawing.Size(23, 23);
            this.cmdFISProperties.TabIndex = 6;
            this.cmdFISProperties.UseVisualStyleBackColor = true;
            this.cmdFISProperties.Click += new System.EventHandler(this.cmdFISProperties_Click);
            // 
            // ucErrorSurfaceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdFISProperties);
            this.Controls.Add(this.cboAssociated);
            this.Controls.Add(this.rdoAssociated);
            this.Controls.Add(this.grdFISInputs);
            this.Controls.Add(this.cboFIS);
            this.Controls.Add(this.rdoFIS);
            this.Controls.Add(this.valUniform);
            this.Controls.Add(this.rdoUniform);
            this.Name = "ucErrorSurfaceProperties";
            this.Size = new System.Drawing.Size(446, 190);
            this.Load += new System.EventHandler(this.ucErrorSurfaceProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdFISInputs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUniform)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox cboAssociated;
        internal System.Windows.Forms.RadioButton rdoAssociated;
        internal System.Windows.Forms.DataGridView grdFISInputs;
        internal System.Windows.Forms.ComboBox cboFIS;
        internal System.Windows.Forms.RadioButton rdoFIS;
        internal System.Windows.Forms.NumericUpDown valUniform;
        internal System.Windows.Forms.RadioButton rdoUniform;
        private System.Windows.Forms.DataGridViewTextBoxColumn FISInput;
        private System.Windows.Forms.DataGridViewComboBoxColumn AssociatedSurface;
        private System.Windows.Forms.ToolTip tTip;
        private System.Windows.Forms.Button cmdFISProperties;
    }
}
