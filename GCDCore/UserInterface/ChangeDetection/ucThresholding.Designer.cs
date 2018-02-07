namespace GCDCore.UserInterface.ChangeDetection
{
    partial class ucThresholding
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
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.valMinLodThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblMinLodThreshold = new System.Windows.Forms.Label();
            this.cmdBayesianProperties = new System.Windows.Forms.Button();
            this.chkBayesian = new System.Windows.Forms.CheckBox();
            this.valConfidence = new System.Windows.Forms.NumericUpDown();
            this.lblConfidence = new System.Windows.Forms.Label();
            this.rdoProbabilistic = new System.Windows.Forms.RadioButton();
            this.rdoPropagated = new System.Windows.Forms.RadioButton();
            this.rdoMinLOD = new System.Windows.Forms.RadioButton();
            this.GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valMinLodThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valConfidence)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.valMinLodThreshold);
            this.GroupBox3.Controls.Add(this.lblMinLodThreshold);
            this.GroupBox3.Controls.Add(this.cmdBayesianProperties);
            this.GroupBox3.Controls.Add(this.chkBayesian);
            this.GroupBox3.Controls.Add(this.valConfidence);
            this.GroupBox3.Controls.Add(this.lblConfidence);
            this.GroupBox3.Controls.Add(this.rdoProbabilistic);
            this.GroupBox3.Controls.Add(this.rdoPropagated);
            this.GroupBox3.Controls.Add(this.rdoMinLOD);
            this.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox3.Location = new System.Drawing.Point(0, 0);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(321, 223);
            this.GroupBox3.TabIndex = 8;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Uncertainty Analysis Method";
            // 
            // valMinLodThreshold
            // 
            this.valMinLodThreshold.DecimalPlaces = 2;
            this.valMinLodThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valMinLodThreshold.Location = new System.Drawing.Point(170, 43);
            this.valMinLodThreshold.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.valMinLodThreshold.Name = "valMinLodThreshold";
            this.valMinLodThreshold.Size = new System.Drawing.Size(66, 20);
            this.valMinLodThreshold.TabIndex = 2;
            this.valMinLodThreshold.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.valMinLodThreshold.ValueChanged += new System.EventHandler(this.UpdateControls);
            // 
            // lblMinLodThreshold
            // 
            this.lblMinLodThreshold.AutoSize = true;
            this.lblMinLodThreshold.Location = new System.Drawing.Point(51, 47);
            this.lblMinLodThreshold.Name = "lblMinLodThreshold";
            this.lblMinLodThreshold.Size = new System.Drawing.Size(63, 13);
            this.lblMinLodThreshold.TabIndex = 1;
            this.lblMinLodThreshold.Text = "Threshold ()";
            // 
            // cmdBayesianProperties
            // 
            this.cmdBayesianProperties.Image = global::GCDCore.Properties.Resources.Settings;
            this.cmdBayesianProperties.Location = new System.Drawing.Point(189, 141);
            this.cmdBayesianProperties.Name = "cmdBayesianProperties";
            this.cmdBayesianProperties.Size = new System.Drawing.Size(23, 23);
            this.cmdBayesianProperties.TabIndex = 8;
            this.cmdBayesianProperties.UseVisualStyleBackColor = true;
            this.cmdBayesianProperties.Click += new System.EventHandler(this.cmdBayesianProperties_Click);
            // 
            // chkBayesian
            // 
            this.chkBayesian.AutoSize = true;
            this.chkBayesian.Location = new System.Drawing.Point(51, 144);
            this.chkBayesian.Name = "chkBayesian";
            this.chkBayesian.Size = new System.Drawing.Size(135, 17);
            this.chkBayesian.TabIndex = 7;
            this.chkBayesian.Text = "Use Bayesian updating";
            this.chkBayesian.UseVisualStyleBackColor = true;
            this.chkBayesian.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // valConfidence
            // 
            this.valConfidence.DecimalPlaces = 2;
            this.valConfidence.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valConfidence.Location = new System.Drawing.Point(170, 112);
            this.valConfidence.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valConfidence.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valConfidence.Name = "valConfidence";
            this.valConfidence.Size = new System.Drawing.Size(66, 20);
            this.valConfidence.TabIndex = 6;
            this.valConfidence.Value = new decimal(new int[] {
            80,
            0,
            0,
            131072});
            this.valConfidence.ValueChanged += new System.EventHandler(this.UpdateControls);
            // 
            // lblConfidence
            // 
            this.lblConfidence.AutoSize = true;
            this.lblConfidence.Location = new System.Drawing.Point(51, 116);
            this.lblConfidence.Name = "lblConfidence";
            this.lblConfidence.Size = new System.Drawing.Size(110, 13);
            this.lblConfidence.TabIndex = 5;
            this.lblConfidence.Text = "Confidence level (0-1)";
            // 
            // rdoProbabilistic
            // 
            this.rdoProbabilistic.AutoSize = true;
            this.rdoProbabilistic.Location = new System.Drawing.Point(17, 92);
            this.rdoProbabilistic.Name = "rdoProbabilistic";
            this.rdoProbabilistic.Size = new System.Drawing.Size(141, 17);
            this.rdoProbabilistic.TabIndex = 4;
            this.rdoProbabilistic.Text = "Probabilistic thresholding";
            this.rdoProbabilistic.UseVisualStyleBackColor = true;
            this.rdoProbabilistic.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // rdoPropagated
            // 
            this.rdoPropagated.AutoSize = true;
            this.rdoPropagated.Location = new System.Drawing.Point(17, 69);
            this.rdoPropagated.Name = "rdoPropagated";
            this.rdoPropagated.Size = new System.Drawing.Size(109, 17);
            this.rdoPropagated.TabIndex = 3;
            this.rdoPropagated.Text = "Propagated errors";
            this.rdoPropagated.UseVisualStyleBackColor = true;
            this.rdoPropagated.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // rdoMinLOD
            // 
            this.rdoMinLOD.AutoSize = true;
            this.rdoMinLOD.Checked = true;
            this.rdoMinLOD.Location = new System.Drawing.Point(17, 22);
            this.rdoMinLOD.Name = "rdoMinLOD";
            this.rdoMinLOD.Size = new System.Drawing.Size(183, 17);
            this.rdoMinLOD.TabIndex = 0;
            this.rdoMinLOD.TabStop = true;
            this.rdoMinLOD.Text = "Simple minimum level of detection";
            this.rdoMinLOD.UseVisualStyleBackColor = true;
            this.rdoMinLOD.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // ucThresholding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBox3);
            this.Name = "ucThresholding";
            this.Size = new System.Drawing.Size(321, 223);
            this.Load += new System.EventHandler(this.ucThresholding_Load);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valMinLodThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valConfidence)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.NumericUpDown valMinLodThreshold;
        internal System.Windows.Forms.Label lblMinLodThreshold;
        internal System.Windows.Forms.Button cmdBayesianProperties;
        internal System.Windows.Forms.CheckBox chkBayesian;
        internal System.Windows.Forms.NumericUpDown valConfidence;
        internal System.Windows.Forms.Label lblConfidence;
        internal System.Windows.Forms.RadioButton rdoProbabilistic;
        internal System.Windows.Forms.RadioButton rdoPropagated;
        internal System.Windows.Forms.RadioButton rdoMinLOD;
    }
}
