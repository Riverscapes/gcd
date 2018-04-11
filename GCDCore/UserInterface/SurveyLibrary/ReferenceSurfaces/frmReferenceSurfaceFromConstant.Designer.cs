namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    partial class frmReferenceSurfaceFromConstant
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReferenceSurfaceFromConstant));
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.rdoSingle = new System.Windows.Forms.RadioButton();
            this.lblSingle = new System.Windows.Forms.Label();
            this.valSingle = new System.Windows.Forms.NumericUpDown();
            this.valUpper = new System.Windows.Forms.NumericUpDown();
            this.lblUpper = new System.Windows.Forms.Label();
            this.rdoMultiple = new System.Windows.Forms.RadioButton();
            this.valLower = new System.Windows.Forms.NumericUpDown();
            this.lblLower = new System.Windows.Forms.Label();
            this.valIncrement = new System.Windows.Forms.NumericUpDown();
            this.lblIncrement = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboDEMSurvey = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.valError = new System.Windows.Forms.NumericUpDown();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.valSingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valIncrement)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valError)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(130, 41);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(241, 20);
            this.txtPath.TabIndex = 3;
            this.txtPath.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project path";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(130, 12);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(241, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 353);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 10;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(215, 353);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "Save";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(296, 353);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // rdoSingle
            // 
            this.rdoSingle.AutoSize = true;
            this.rdoSingle.Checked = true;
            this.rdoSingle.Location = new System.Drawing.Point(17, 19);
            this.rdoSingle.Name = "rdoSingle";
            this.rdoSingle.Size = new System.Drawing.Size(186, 17);
            this.rdoSingle.TabIndex = 4;
            this.rdoSingle.TabStop = true;
            this.rdoSingle.Text = "Single reference surface elevation";
            this.rdoSingle.UseVisualStyleBackColor = true;
            // 
            // lblSingle
            // 
            this.lblSingle.AutoSize = true;
            this.lblSingle.Location = new System.Drawing.Point(90, 47);
            this.lblSingle.Name = "lblSingle";
            this.lblSingle.Size = new System.Drawing.Size(60, 13);
            this.lblSingle.TabIndex = 5;
            this.lblSingle.Text = "Elevation ()";
            // 
            // valSingle
            // 
            this.valSingle.DecimalPlaces = 2;
            this.valSingle.Location = new System.Drawing.Point(164, 43);
            this.valSingle.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.valSingle.Name = "valSingle";
            this.valSingle.Size = new System.Drawing.Size(94, 20);
            this.valSingle.TabIndex = 6;
            this.valSingle.Enter += new System.EventHandler(this.numericUpDown_Enter);
            // 
            // valUpper
            // 
            this.valUpper.DecimalPlaces = 2;
            this.valUpper.Location = new System.Drawing.Point(164, 96);
            this.valUpper.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.valUpper.Name = "valUpper";
            this.valUpper.Size = new System.Drawing.Size(94, 20);
            this.valUpper.TabIndex = 9;
            this.valUpper.Enter += new System.EventHandler(this.numericUpDown_Enter);
            // 
            // lblUpper
            // 
            this.lblUpper.AutoSize = true;
            this.lblUpper.Location = new System.Drawing.Point(59, 100);
            this.lblUpper.Name = "lblUpper";
            this.lblUpper.Size = new System.Drawing.Size(91, 13);
            this.lblUpper.TabIndex = 8;
            this.lblUpper.Text = "Upper elevation ()";
            // 
            // rdoMultiple
            // 
            this.rdoMultiple.AutoSize = true;
            this.rdoMultiple.Location = new System.Drawing.Point(17, 72);
            this.rdoMultiple.Name = "rdoMultiple";
            this.rdoMultiple.Size = new System.Drawing.Size(193, 17);
            this.rdoMultiple.TabIndex = 7;
            this.rdoMultiple.Text = "Multiple reference surface elevation";
            this.rdoMultiple.UseVisualStyleBackColor = true;
            this.rdoMultiple.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // valLower
            // 
            this.valLower.DecimalPlaces = 2;
            this.valLower.Location = new System.Drawing.Point(164, 122);
            this.valLower.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.valLower.Name = "valLower";
            this.valLower.Size = new System.Drawing.Size(94, 20);
            this.valLower.TabIndex = 11;
            this.valLower.Enter += new System.EventHandler(this.numericUpDown_Enter);
            // 
            // lblLower
            // 
            this.lblLower.AutoSize = true;
            this.lblLower.Location = new System.Drawing.Point(59, 126);
            this.lblLower.Name = "lblLower";
            this.lblLower.Size = new System.Drawing.Size(91, 13);
            this.lblLower.TabIndex = 10;
            this.lblLower.Text = "Lower elevation ()";
            // 
            // valIncrement
            // 
            this.valIncrement.DecimalPlaces = 2;
            this.valIncrement.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valIncrement.Location = new System.Drawing.Point(164, 150);
            this.valIncrement.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.valIncrement.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valIncrement.Name = "valIncrement";
            this.valIncrement.Size = new System.Drawing.Size(94, 20);
            this.valIncrement.TabIndex = 13;
            this.valIncrement.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valIncrement.Enter += new System.EventHandler(this.numericUpDown_Enter);
            // 
            // lblIncrement
            // 
            this.lblIncrement.AutoSize = true;
            this.lblIncrement.Location = new System.Drawing.Point(87, 154);
            this.lblIncrement.Name = "lblIncrement";
            this.lblIncrement.Size = new System.Drawing.Size(63, 13);
            this.lblIncrement.TabIndex = 12;
            this.lblIncrement.Text = "Increment ()";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Reference DEM survey";
            // 
            // cboDEMSurvey
            // 
            this.cboDEMSurvey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDEMSurvey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDEMSurvey.FormattingEnabled = true;
            this.cboDEMSurvey.Location = new System.Drawing.Point(130, 70);
            this.cboDEMSurvey.Name = "cboDEMSurvey";
            this.cboDEMSurvey.Size = new System.Drawing.Size(241, 21);
            this.cboDEMSurvey.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoSingle);
            this.groupBox1.Controls.Add(this.lblSingle);
            this.groupBox1.Controls.Add(this.valSingle);
            this.groupBox1.Controls.Add(this.valIncrement);
            this.groupBox1.Controls.Add(this.rdoMultiple);
            this.groupBox1.Controls.Add(this.lblIncrement);
            this.groupBox1.Controls.Add(this.lblUpper);
            this.groupBox1.Controls.Add(this.valLower);
            this.groupBox1.Controls.Add(this.valUpper);
            this.groupBox1.Controls.Add(this.lblLower);
            this.groupBox1.Location = new System.Drawing.Point(12, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 181);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reference Surface Elevations";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.valError);
            this.groupBox2.Controls.Add(this.lblError);
            this.groupBox2.Location = new System.Drawing.Point(12, 289);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 53);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Error Surface(s)";
            // 
            // valError
            // 
            this.valError.DecimalPlaces = 3;
            this.valError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valError.Location = new System.Drawing.Point(164, 19);
            this.valError.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.valError.Name = "valError";
            this.valError.Size = new System.Drawing.Size(94, 20);
            this.valError.TabIndex = 14;
            this.valError.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valError.Enter += new System.EventHandler(this.numericUpDown_Enter);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(78, 23);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(76, 13);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "Uniform error ()";
            // 
            // frmReferenceSurfaceFromConstant
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(383, 388);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboDEMSurvey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(312, 317);
            this.Name = "frmReferenceSurfaceFromConstant";
            this.Text = "Create Constant Reference Surface(s)";
            this.Load += new System.EventHandler(this.frmReferenceSurfaceFromConstant_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valSingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valIncrement)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.RadioButton rdoSingle;
        private System.Windows.Forms.Label lblSingle;
        private System.Windows.Forms.NumericUpDown valSingle;
        private System.Windows.Forms.NumericUpDown valUpper;
        private System.Windows.Forms.Label lblUpper;
        private System.Windows.Forms.RadioButton rdoMultiple;
        private System.Windows.Forms.NumericUpDown valLower;
        private System.Windows.Forms.Label lblLower;
        private System.Windows.Forms.NumericUpDown valIncrement;
        private System.Windows.Forms.Label lblIncrement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboDEMSurvey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown valError;
        private System.Windows.Forms.Label lblError;
    }
}