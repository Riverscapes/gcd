namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    partial class frmRefErrorSurface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRefErrorSurface));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.rdoSingle = new System.Windows.Forms.RadioButton();
            this.lblSingle = new System.Windows.Forms.Label();
            this.valSingle = new System.Windows.Forms.NumericUpDown();
            this.valIncrement = new System.Windows.Forms.NumericUpDown();
            this.rdoMultiple = new System.Windows.Forms.RadioButton();
            this.lblIncrement = new System.Windows.Forms.Label();
            this.lblUpper = new System.Windows.Forms.Label();
            this.valLower = new System.Windows.Forms.NumericUpDown();
            this.valUpper = new System.Windows.Forms.NumericUpDown();
            this.lblLower = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.valSingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valIncrement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUpper)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(409, 237);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 15;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(328, 237);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 14;
            this.cmdOK.Text = "Save";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 237);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 16;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(85, 12);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(396, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project path";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(85, 41);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(396, 20);
            this.txtPath.TabIndex = 3;
            // 
            // rdoSingle
            // 
            this.rdoSingle.AutoSize = true;
            this.rdoSingle.Checked = true;
            this.rdoSingle.Location = new System.Drawing.Point(17, 74);
            this.rdoSingle.Name = "rdoSingle";
            this.rdoSingle.Size = new System.Drawing.Size(162, 17);
            this.rdoSingle.TabIndex = 4;
            this.rdoSingle.TabStop = true;
            this.rdoSingle.Text = "Single error surface elevation";
            this.rdoSingle.UseVisualStyleBackColor = true;
            // 
            // lblSingle
            // 
            this.lblSingle.AutoSize = true;
            this.lblSingle.Location = new System.Drawing.Point(78, 102);
            this.lblSingle.Name = "lblSingle";
            this.lblSingle.Size = new System.Drawing.Size(76, 13);
            this.lblSingle.TabIndex = 5;
            this.lblSingle.Text = "Uniform error ()";
            // 
            // valSingle
            // 
            this.valSingle.DecimalPlaces = 2;
            this.valSingle.Location = new System.Drawing.Point(164, 98);
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
            // valIncrement
            // 
            this.valIncrement.DecimalPlaces = 2;
            this.valIncrement.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valIncrement.Location = new System.Drawing.Point(164, 205);
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
            // rdoMultiple
            // 
            this.rdoMultiple.AutoSize = true;
            this.rdoMultiple.Location = new System.Drawing.Point(17, 127);
            this.rdoMultiple.Name = "rdoMultiple";
            this.rdoMultiple.Size = new System.Drawing.Size(169, 17);
            this.rdoMultiple.TabIndex = 7;
            this.rdoMultiple.Text = "Multiple error surface elevation";
            this.rdoMultiple.UseVisualStyleBackColor = true;
            this.rdoMultiple.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // lblIncrement
            // 
            this.lblIncrement.AutoSize = true;
            this.lblIncrement.Location = new System.Drawing.Point(91, 209);
            this.lblIncrement.Name = "lblIncrement";
            this.lblIncrement.Size = new System.Drawing.Size(63, 13);
            this.lblIncrement.TabIndex = 12;
            this.lblIncrement.Text = "Increment ()";
            // 
            // lblUpper
            // 
            this.lblUpper.AutoSize = true;
            this.lblUpper.Location = new System.Drawing.Point(85, 155);
            this.lblUpper.Name = "lblUpper";
            this.lblUpper.Size = new System.Drawing.Size(69, 13);
            this.lblUpper.TabIndex = 8;
            this.lblUpper.Text = "Upper error ()";
            // 
            // valLower
            // 
            this.valLower.DecimalPlaces = 2;
            this.valLower.Location = new System.Drawing.Point(164, 177);
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
            // valUpper
            // 
            this.valUpper.DecimalPlaces = 2;
            this.valUpper.Location = new System.Drawing.Point(164, 151);
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
            // lblLower
            // 
            this.lblLower.AutoSize = true;
            this.lblLower.Location = new System.Drawing.Point(85, 181);
            this.lblLower.Name = "lblLower";
            this.lblLower.Size = new System.Drawing.Size(69, 13);
            this.lblLower.TabIndex = 10;
            this.lblLower.Text = "Lower error ()";
            // 
            // frmRefErrorSurface
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(496, 272);
            this.Controls.Add(this.rdoSingle);
            this.Controls.Add(this.lblSingle);
            this.Controls.Add(this.valSingle);
            this.Controls.Add(this.valIncrement);
            this.Controls.Add(this.rdoMultiple);
            this.Controls.Add(this.lblIncrement);
            this.Controls.Add(this.lblUpper);
            this.Controls.Add(this.valLower);
            this.Controls.Add(this.valUpper);
            this.Controls.Add(this.lblLower);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRefErrorSurface";
            this.Text = "Reference Error Surface";
            this.Load += new System.EventHandler(this.frmRefErrorSurface_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valSingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valIncrement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUpper)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.RadioButton rdoSingle;
        private System.Windows.Forms.Label lblSingle;
        private System.Windows.Forms.NumericUpDown valSingle;
        private System.Windows.Forms.NumericUpDown valIncrement;
        private System.Windows.Forms.RadioButton rdoMultiple;
        private System.Windows.Forms.Label lblIncrement;
        private System.Windows.Forms.Label lblUpper;
        private System.Windows.Forms.NumericUpDown valLower;
        private System.Windows.Forms.NumericUpDown valUpper;
        private System.Windows.Forms.Label lblLower;
    }
}