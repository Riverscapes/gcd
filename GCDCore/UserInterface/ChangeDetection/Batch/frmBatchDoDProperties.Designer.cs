namespace GCDCore.UserInterface.ChangeDetection.Batch
{
    partial class frmBatchDoDProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatchDoDProperties));
            this.rdoSimpleMinLoD = new System.Windows.Forms.RadioButton();
            this.lblSingleMinLoD = new System.Windows.Forms.Label();
            this.valSingleMinLoD = new System.Windows.Forms.NumericUpDown();
            this.valMMinLoDMin = new System.Windows.Forms.NumericUpDown();
            this.lblMMinLoDMin = new System.Windows.Forms.Label();
            this.rdoMultipleMinLoD = new System.Windows.Forms.RadioButton();
            this.valMMinLoDMax = new System.Windows.Forms.NumericUpDown();
            this.lblMMinLoDMax = new System.Windows.Forms.Label();
            this.valMMinLoDInterval = new System.Windows.Forms.NumericUpDown();
            this.lblMMinLoDInterval = new System.Windows.Forms.Label();
            this.rdoPropagated = new System.Windows.Forms.RadioButton();
            this.rdoSingleProbabilistic = new System.Windows.Forms.RadioButton();
            this.valSProb = new System.Windows.Forms.NumericUpDown();
            this.lblSProb = new System.Windows.Forms.Label();
            this.chkSProb = new System.Windows.Forms.CheckBox();
            this.cmdSProb = new System.Windows.Forms.Button();
            this.valMProbInterval = new System.Windows.Forms.NumericUpDown();
            this.lblMProbInterval = new System.Windows.Forms.Label();
            this.valMProbMax = new System.Windows.Forms.NumericUpDown();
            this.lblMProbMax = new System.Windows.Forms.Label();
            this.valMProbMin = new System.Windows.Forms.NumericUpDown();
            this.lblMProbMin = new System.Windows.Forms.Label();
            this.rdoMultipleProbabilistic = new System.Windows.Forms.RadioButton();
            this.cmdMProb = new System.Windows.Forms.Button();
            this.chkMProb = new System.Windows.Forms.CheckBox();
            this.rdoPrescribed = new System.Windows.Forms.RadioButton();
            this.lblPProb = new System.Windows.Forms.Label();
            this.cmdPProb = new System.Windows.Forms.Button();
            this.chkPProb = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.valSingleMinLoD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMMinLoDMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMMinLoDMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMMinLoDInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valSProb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMProbInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMProbMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMProbMin)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoSimpleMinLoD
            // 
            this.rdoSimpleMinLoD.AutoSize = true;
            this.rdoSimpleMinLoD.Location = new System.Drawing.Point(13, 13);
            this.rdoSimpleMinLoD.Name = "rdoSimpleMinLoD";
            this.rdoSimpleMinLoD.Size = new System.Drawing.Size(190, 17);
            this.rdoSimpleMinLoD.TabIndex = 0;
            this.rdoSimpleMinLoD.TabStop = true;
            this.rdoSimpleMinLoD.Text = "Simple minimum Level of detetction";
            this.rdoSimpleMinLoD.UseVisualStyleBackColor = true;
            this.rdoSimpleMinLoD.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // lblSingleMinLoD
            // 
            this.lblSingleMinLoD.AutoSize = true;
            this.lblSingleMinLoD.Location = new System.Drawing.Point(146, 41);
            this.lblSingleMinLoD.Name = "lblSingleMinLoD";
            this.lblSingleMinLoD.Size = new System.Drawing.Size(54, 13);
            this.lblSingleMinLoD.TabIndex = 1;
            this.lblSingleMinLoD.Text = "Threshold";
            // 
            // valSingleMinLoD
            // 
            this.valSingleMinLoD.DecimalPlaces = 2;
            this.valSingleMinLoD.Location = new System.Drawing.Point(202, 37);
            this.valSingleMinLoD.Name = "valSingleMinLoD";
            this.valSingleMinLoD.Size = new System.Drawing.Size(68, 20);
            this.valSingleMinLoD.TabIndex = 2;
            this.valSingleMinLoD.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // valMMinLoDMin
            // 
            this.valMMinLoDMin.DecimalPlaces = 2;
            this.valMMinLoDMin.Location = new System.Drawing.Point(202, 104);
            this.valMMinLoDMin.Name = "valMMinLoDMin";
            this.valMMinLoDMin.Size = new System.Drawing.Size(68, 20);
            this.valMMinLoDMin.TabIndex = 5;
            this.valMMinLoDMin.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMMinLoDMin
            // 
            this.lblMMinLoDMin.AutoSize = true;
            this.lblMMinLoDMin.Location = new System.Drawing.Point(106, 108);
            this.lblMMinLoDMin.Name = "lblMMinLoDMin";
            this.lblMMinLoDMin.Size = new System.Drawing.Size(94, 13);
            this.lblMMinLoDMin.TabIndex = 4;
            this.lblMMinLoDMin.Text = "Minimum threshold";
            // 
            // rdoMultipleMinLoD
            // 
            this.rdoMultipleMinLoD.AutoSize = true;
            this.rdoMultipleMinLoD.Location = new System.Drawing.Point(13, 80);
            this.rdoMultipleMinLoD.Name = "rdoMultipleMinLoD";
            this.rdoMultipleMinLoD.Size = new System.Drawing.Size(195, 17);
            this.rdoMultipleMinLoD.TabIndex = 3;
            this.rdoMultipleMinLoD.TabStop = true;
            this.rdoMultipleMinLoD.Text = "Multiple minimum Level of detetction";
            this.rdoMultipleMinLoD.UseVisualStyleBackColor = true;
            this.rdoMultipleMinLoD.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // valMMinLoDMax
            // 
            this.valMMinLoDMax.DecimalPlaces = 2;
            this.valMMinLoDMax.Location = new System.Drawing.Point(202, 130);
            this.valMMinLoDMax.Name = "valMMinLoDMax";
            this.valMMinLoDMax.Size = new System.Drawing.Size(68, 20);
            this.valMMinLoDMax.TabIndex = 7;
            this.valMMinLoDMax.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMMinLoDMax
            // 
            this.lblMMinLoDMax.AutoSize = true;
            this.lblMMinLoDMax.Location = new System.Drawing.Point(103, 134);
            this.lblMMinLoDMax.Name = "lblMMinLoDMax";
            this.lblMMinLoDMax.Size = new System.Drawing.Size(97, 13);
            this.lblMMinLoDMax.TabIndex = 6;
            this.lblMMinLoDMax.Text = "Maximum threshold";
            // 
            // valMMinLoDInterval
            // 
            this.valMMinLoDInterval.DecimalPlaces = 2;
            this.valMMinLoDInterval.Location = new System.Drawing.Point(202, 156);
            this.valMMinLoDInterval.Name = "valMMinLoDInterval";
            this.valMMinLoDInterval.Size = new System.Drawing.Size(68, 20);
            this.valMMinLoDInterval.TabIndex = 9;
            this.valMMinLoDInterval.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMMinLoDInterval
            // 
            this.lblMMinLoDInterval.AutoSize = true;
            this.lblMMinLoDInterval.Location = new System.Drawing.Point(158, 160);
            this.lblMMinLoDInterval.Name = "lblMMinLoDInterval";
            this.lblMMinLoDInterval.Size = new System.Drawing.Size(42, 13);
            this.lblMMinLoDInterval.TabIndex = 8;
            this.lblMMinLoDInterval.Text = "Interval";
            // 
            // rdoPropagated
            // 
            this.rdoPropagated.AutoSize = true;
            this.rdoPropagated.Location = new System.Drawing.Point(12, 186);
            this.rdoPropagated.Name = "rdoPropagated";
            this.rdoPropagated.Size = new System.Drawing.Size(109, 17);
            this.rdoPropagated.TabIndex = 10;
            this.rdoPropagated.TabStop = true;
            this.rdoPropagated.Text = "Propagated errors";
            this.rdoPropagated.UseVisualStyleBackColor = true;
            this.rdoPropagated.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // rdoSingleProbabilistic
            // 
            this.rdoSingleProbabilistic.AutoSize = true;
            this.rdoSingleProbabilistic.Location = new System.Drawing.Point(11, 214);
            this.rdoSingleProbabilistic.Name = "rdoSingleProbabilistic";
            this.rdoSingleProbabilistic.Size = new System.Drawing.Size(141, 17);
            this.rdoSingleProbabilistic.TabIndex = 11;
            this.rdoSingleProbabilistic.TabStop = true;
            this.rdoSingleProbabilistic.Text = "Probabilistic thresholding";
            this.rdoSingleProbabilistic.UseVisualStyleBackColor = true;
            this.rdoSingleProbabilistic.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // valSProb
            // 
            this.valSProb.DecimalPlaces = 2;
            this.valSProb.Location = new System.Drawing.Point(202, 237);
            this.valSProb.Name = "valSProb";
            this.valSProb.Size = new System.Drawing.Size(68, 20);
            this.valSProb.TabIndex = 13;
            this.valSProb.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblSProb
            // 
            this.lblSProb.AutoSize = true;
            this.lblSProb.Location = new System.Drawing.Point(90, 240);
            this.lblSProb.Name = "lblSProb";
            this.lblSProb.Size = new System.Drawing.Size(110, 13);
            this.lblSProb.TabIndex = 12;
            this.lblSProb.Text = "Confidence level (0-1)";
            // 
            // chkSProb
            // 
            this.chkSProb.AutoSize = true;
            this.chkSProb.Location = new System.Drawing.Point(64, 268);
            this.chkSProb.Name = "chkSProb";
            this.chkSProb.Size = new System.Drawing.Size(135, 17);
            this.chkSProb.TabIndex = 14;
            this.chkSProb.Text = "Use Bayesian updating";
            this.chkSProb.UseVisualStyleBackColor = true;
            this.chkSProb.CheckedChanged += new System.EventHandler(this.chkSProb_CheckedChanged);
            // 
            // cmdSProb
            // 
            this.cmdSProb.Image = global::GCDCore.Properties.Resources.Settings;
            this.cmdSProb.Location = new System.Drawing.Point(202, 265);
            this.cmdSProb.Name = "cmdSProb";
            this.cmdSProb.Size = new System.Drawing.Size(23, 23);
            this.cmdSProb.TabIndex = 15;
            this.cmdSProb.UseVisualStyleBackColor = true;
            this.cmdSProb.Click += new System.EventHandler(this.cmdSProb_Click);
            // 
            // valMProbInterval
            // 
            this.valMProbInterval.DecimalPlaces = 2;
            this.valMProbInterval.Location = new System.Drawing.Point(202, 371);
            this.valMProbInterval.Name = "valMProbInterval";
            this.valMProbInterval.Size = new System.Drawing.Size(68, 20);
            this.valMProbInterval.TabIndex = 22;
            this.valMProbInterval.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMProbInterval
            // 
            this.lblMProbInterval.AutoSize = true;
            this.lblMProbInterval.Location = new System.Drawing.Point(158, 374);
            this.lblMProbInterval.Name = "lblMProbInterval";
            this.lblMProbInterval.Size = new System.Drawing.Size(42, 13);
            this.lblMProbInterval.TabIndex = 21;
            this.lblMProbInterval.Text = "Interval";
            // 
            // valMProbMax
            // 
            this.valMProbMax.DecimalPlaces = 2;
            this.valMProbMax.Location = new System.Drawing.Point(202, 345);
            this.valMProbMax.Name = "valMProbMax";
            this.valMProbMax.Size = new System.Drawing.Size(68, 20);
            this.valMProbMax.TabIndex = 20;
            this.valMProbMax.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMProbMax
            // 
            this.lblMProbMax.AutoSize = true;
            this.lblMProbMax.Location = new System.Drawing.Point(44, 348);
            this.lblMProbMax.Name = "lblMProbMax";
            this.lblMProbMax.Size = new System.Drawing.Size(156, 13);
            this.lblMProbMax.TabIndex = 19;
            this.lblMProbMax.Text = "Maximum confidence level (0-1)";
            // 
            // valMProbMin
            // 
            this.valMProbMin.DecimalPlaces = 2;
            this.valMProbMin.Location = new System.Drawing.Point(202, 319);
            this.valMProbMin.Name = "valMProbMin";
            this.valMProbMin.Size = new System.Drawing.Size(68, 20);
            this.valMProbMin.TabIndex = 18;
            this.valMProbMin.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMProbMin
            // 
            this.lblMProbMin.AutoSize = true;
            this.lblMProbMin.Location = new System.Drawing.Point(47, 322);
            this.lblMProbMin.Name = "lblMProbMin";
            this.lblMProbMin.Size = new System.Drawing.Size(153, 13);
            this.lblMProbMin.TabIndex = 17;
            this.lblMProbMin.Text = "Minimum confidence level (0-1)";
            // 
            // rdoMultipleProbabilistic
            // 
            this.rdoMultipleProbabilistic.AutoSize = true;
            this.rdoMultipleProbabilistic.Location = new System.Drawing.Point(13, 295);
            this.rdoMultipleProbabilistic.Name = "rdoMultipleProbabilistic";
            this.rdoMultipleProbabilistic.Size = new System.Drawing.Size(179, 17);
            this.rdoMultipleProbabilistic.TabIndex = 16;
            this.rdoMultipleProbabilistic.TabStop = true;
            this.rdoMultipleProbabilistic.Text = "Multiple probabilistic thresholding";
            this.rdoMultipleProbabilistic.UseVisualStyleBackColor = true;
            this.rdoMultipleProbabilistic.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // cmdMProb
            // 
            this.cmdMProb.Image = global::GCDCore.Properties.Resources.Settings;
            this.cmdMProb.Location = new System.Drawing.Point(202, 397);
            this.cmdMProb.Name = "cmdMProb";
            this.cmdMProb.Size = new System.Drawing.Size(23, 23);
            this.cmdMProb.TabIndex = 24;
            this.cmdMProb.UseVisualStyleBackColor = true;
            this.cmdMProb.Click += new System.EventHandler(this.cmdSProb_Click);
            // 
            // chkMProb
            // 
            this.chkMProb.AutoSize = true;
            this.chkMProb.Location = new System.Drawing.Point(64, 400);
            this.chkMProb.Name = "chkMProb";
            this.chkMProb.Size = new System.Drawing.Size(135, 17);
            this.chkMProb.TabIndex = 23;
            this.chkMProb.Text = "Use Bayesian updating";
            this.chkMProb.UseVisualStyleBackColor = true;
            this.chkMProb.CheckedChanged += new System.EventHandler(this.chkSProb_CheckedChanged);
            // 
            // rdoPrescribed
            // 
            this.rdoPrescribed.AutoSize = true;
            this.rdoPrescribed.Location = new System.Drawing.Point(13, 429);
            this.rdoPrescribed.Name = "rdoPrescribed";
            this.rdoPrescribed.Size = new System.Drawing.Size(184, 17);
            this.rdoPrescribed.TabIndex = 25;
            this.rdoPrescribed.TabStop = true;
            this.rdoPrescribed.Text = "Prescribed probabilistic thresholds";
            this.rdoPrescribed.UseVisualStyleBackColor = true;
            this.rdoPrescribed.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // lblPProb
            // 
            this.lblPProb.AutoSize = true;
            this.lblPProb.Location = new System.Drawing.Point(49, 453);
            this.lblPProb.Name = "lblPProb";
            this.lblPProb.Size = new System.Drawing.Size(184, 13);
            this.lblPProb.TabIndex = 26;
            this.lblPProb.Text = "Thresholds at 50, 66, 80, 95 and 99%";
            // 
            // cmdPProb
            // 
            this.cmdPProb.Image = global::GCDCore.Properties.Resources.Settings;
            this.cmdPProb.Location = new System.Drawing.Point(202, 477);
            this.cmdPProb.Name = "cmdPProb";
            this.cmdPProb.Size = new System.Drawing.Size(23, 23);
            this.cmdPProb.TabIndex = 28;
            this.cmdPProb.UseVisualStyleBackColor = true;
            this.cmdPProb.Click += new System.EventHandler(this.cmdSProb_Click);
            // 
            // chkPProb
            // 
            this.chkPProb.AutoSize = true;
            this.chkPProb.Location = new System.Drawing.Point(64, 480);
            this.chkPProb.Name = "chkPProb";
            this.chkPProb.Size = new System.Drawing.Size(135, 17);
            this.chkPProb.TabIndex = 27;
            this.chkPProb.Text = "Use Bayesian updating";
            this.chkPProb.UseVisualStyleBackColor = true;
            this.chkPProb.CheckedChanged += new System.EventHandler(this.chkSProb_CheckedChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(201, 510);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 30;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(120, 510);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 29;
            this.cmdOK.Text = "Save";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Location = new System.Drawing.Point(11, 510);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 31;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // frmBatchDoDProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(288, 545);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdPProb);
            this.Controls.Add(this.chkPProb);
            this.Controls.Add(this.lblPProb);
            this.Controls.Add(this.rdoPrescribed);
            this.Controls.Add(this.cmdMProb);
            this.Controls.Add(this.chkMProb);
            this.Controls.Add(this.valMProbInterval);
            this.Controls.Add(this.lblMProbInterval);
            this.Controls.Add(this.valMProbMax);
            this.Controls.Add(this.lblMProbMax);
            this.Controls.Add(this.valMProbMin);
            this.Controls.Add(this.lblMProbMin);
            this.Controls.Add(this.rdoMultipleProbabilistic);
            this.Controls.Add(this.cmdSProb);
            this.Controls.Add(this.chkSProb);
            this.Controls.Add(this.valSProb);
            this.Controls.Add(this.lblSProb);
            this.Controls.Add(this.rdoSingleProbabilistic);
            this.Controls.Add(this.rdoPropagated);
            this.Controls.Add(this.valMMinLoDInterval);
            this.Controls.Add(this.lblMMinLoDInterval);
            this.Controls.Add(this.valMMinLoDMax);
            this.Controls.Add(this.lblMMinLoDMax);
            this.Controls.Add(this.valMMinLoDMin);
            this.Controls.Add(this.lblMMinLoDMin);
            this.Controls.Add(this.rdoMultipleMinLoD);
            this.Controls.Add(this.valSingleMinLoD);
            this.Controls.Add(this.lblSingleMinLoD);
            this.Controls.Add(this.rdoSimpleMinLoD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchDoDProperties";
            this.Text = "Batch DoD Uncertainty Methods";
            this.Load += new System.EventHandler(this.frmBatchDoDProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valSingleMinLoD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMMinLoDMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMMinLoDMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMMinLoDInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valSProb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMProbInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMProbMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMProbMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoSimpleMinLoD;
        private System.Windows.Forms.Label lblSingleMinLoD;
        private System.Windows.Forms.NumericUpDown valSingleMinLoD;
        private System.Windows.Forms.NumericUpDown valMMinLoDMin;
        private System.Windows.Forms.Label lblMMinLoDMin;
        private System.Windows.Forms.RadioButton rdoMultipleMinLoD;
        private System.Windows.Forms.NumericUpDown valMMinLoDMax;
        private System.Windows.Forms.Label lblMMinLoDMax;
        private System.Windows.Forms.NumericUpDown valMMinLoDInterval;
        private System.Windows.Forms.Label lblMMinLoDInterval;
        private System.Windows.Forms.RadioButton rdoPropagated;
        private System.Windows.Forms.RadioButton rdoSingleProbabilistic;
        private System.Windows.Forms.NumericUpDown valSProb;
        private System.Windows.Forms.Label lblSProb;
        private System.Windows.Forms.CheckBox chkSProb;
        private System.Windows.Forms.Button cmdSProb;
        private System.Windows.Forms.NumericUpDown valMProbInterval;
        private System.Windows.Forms.Label lblMProbInterval;
        private System.Windows.Forms.NumericUpDown valMProbMax;
        private System.Windows.Forms.Label lblMProbMax;
        private System.Windows.Forms.NumericUpDown valMProbMin;
        private System.Windows.Forms.Label lblMProbMin;
        private System.Windows.Forms.RadioButton rdoMultipleProbabilistic;
        private System.Windows.Forms.Button cmdMProb;
        private System.Windows.Forms.CheckBox chkMProb;
        private System.Windows.Forms.RadioButton rdoPrescribed;
        private System.Windows.Forms.Label lblPProb;
        private System.Windows.Forms.Button cmdPProb;
        private System.Windows.Forms.CheckBox chkPProb;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
    }
}