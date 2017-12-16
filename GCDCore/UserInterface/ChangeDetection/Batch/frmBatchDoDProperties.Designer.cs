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
            this.valMin = new System.Windows.Forms.NumericUpDown();
            this.lblMin = new System.Windows.Forms.Label();
            this.valMax = new System.Windows.Forms.NumericUpDown();
            this.lblMax = new System.Windows.Forms.Label();
            this.valInterval = new System.Windows.Forms.NumericUpDown();
            this.lblInterval = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.chkBayesian = new System.Windows.Forms.CheckBox();
            this.cmdBayesian = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.valMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // valMin
            // 
            this.valMin.DecimalPlaces = 2;
            this.valMin.Location = new System.Drawing.Point(187, 12);
            this.valMin.Name = "valMin";
            this.valMin.Size = new System.Drawing.Size(68, 20);
            this.valMin.TabIndex = 5;
            this.valMin.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMin
            // 
            this.lblMin.Location = new System.Drawing.Point(12, 12);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(170, 20);
            this.lblMin.TabIndex = 4;
            this.lblMin.Text = "Minimum threshold";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // valMax
            // 
            this.valMax.DecimalPlaces = 2;
            this.valMax.Location = new System.Drawing.Point(187, 38);
            this.valMax.Name = "valMax";
            this.valMax.Size = new System.Drawing.Size(68, 20);
            this.valMax.TabIndex = 7;
            this.valMax.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblMax
            // 
            this.lblMax.Location = new System.Drawing.Point(12, 38);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(170, 20);
            this.lblMax.TabIndex = 6;
            this.lblMax.Text = "Maximum threshold";
            this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // valInterval
            // 
            this.valInterval.DecimalPlaces = 2;
            this.valInterval.Location = new System.Drawing.Point(187, 64);
            this.valInterval.Name = "valInterval";
            this.valInterval.Size = new System.Drawing.Size(68, 20);
            this.valInterval.TabIndex = 9;
            this.valInterval.Enter += new System.EventHandler(this.valSingleMinLoD_Enter);
            // 
            // lblInterval
            // 
            this.lblInterval.Location = new System.Drawing.Point(12, 64);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(170, 20);
            this.lblInterval.TabIndex = 8;
            this.lblInterval.Text = "Interval";
            this.lblInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(182, 128);
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
            this.cmdOK.Location = new System.Drawing.Point(85, 128);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(91, 23);
            this.cmdOK.TabIndex = 29;
            this.cmdOK.Text = "Add To Batch";
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
            // chkBayesian
            // 
            this.chkBayesian.AutoSize = true;
            this.chkBayesian.Location = new System.Drawing.Point(91, 94);
            this.chkBayesian.Name = "chkBayesian";
            this.chkBayesian.Size = new System.Drawing.Size(135, 17);
            this.chkBayesian.TabIndex = 27;
            this.chkBayesian.Text = "Use Bayesian updating";
            this.chkBayesian.UseVisualStyleBackColor = true;
            this.chkBayesian.CheckedChanged += new System.EventHandler(this.chkSProb_CheckedChanged);
            // 
            // cmdBayesian
            // 
            this.cmdBayesian.Image = global::GCDCore.Properties.Resources.Settings;
            this.cmdBayesian.Location = new System.Drawing.Point(232, 91);
            this.cmdBayesian.Name = "cmdBayesian";
            this.cmdBayesian.Size = new System.Drawing.Size(23, 23);
            this.cmdBayesian.TabIndex = 28;
            this.cmdBayesian.UseVisualStyleBackColor = true;
            this.cmdBayesian.Click += new System.EventHandler(this.cmdSProb_Click);
            // 
            // frmBatchDoDProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(269, 163);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdBayesian);
            this.Controls.Add(this.chkBayesian);
            this.Controls.Add(this.valInterval);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.valMax);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.valMin);
            this.Controls.Add(this.lblMin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchDoDProperties";
            this.Text = "frmTitle";
            this.Load += new System.EventHandler(this.frmBatchDoDProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown valMin;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.NumericUpDown valMax;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.NumericUpDown valInterval;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.CheckBox chkBayesian;
        private System.Windows.Forms.Button cmdBayesian;
    }
}