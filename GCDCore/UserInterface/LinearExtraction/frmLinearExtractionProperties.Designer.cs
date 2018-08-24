namespace GCDCore.UserInterface.LinearExtraction
{
    partial class frmLinearExtractionProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLinearExtractionProperties));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboRoute = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblErrorSurface = new System.Windows.Forms.Label();
            this.cboError = new System.Windows.Forms.ComboBox();
            this.txtElevationSurface = new System.Windows.Forms.TextBox();
            this.lblSampleDistance = new System.Windows.Forms.Label();
            this.valSampleDistance = new System.Windows.Forms.NumericUpDown();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.valSampleDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(426, 193);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 13;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(345, 193);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 12;
            this.cmdOK.Text = "   Save";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 193);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 14;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(121, 13);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(380, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project path";
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(121, 42);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(380, 20);
            this.txtPath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Profile route";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboRoute
            // 
            this.cboRoute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRoute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoute.FormattingEnabled = true;
            this.cboRoute.Location = new System.Drawing.Point(121, 71);
            this.cboRoute.Name = "cboRoute";
            this.cboRoute.Size = new System.Drawing.Size(380, 21);
            this.cboRoute.TabIndex = 5;
            this.cboRoute.SelectedIndexChanged += new System.EventHandler(this.cboRoute_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Elevation surface";
            // 
            // lblErrorSurface
            // 
            this.lblErrorSurface.AutoSize = true;
            this.lblErrorSurface.Location = new System.Drawing.Point(48, 165);
            this.lblErrorSurface.Name = "lblErrorSurface";
            this.lblErrorSurface.Size = new System.Drawing.Size(67, 13);
            this.lblErrorSurface.TabIndex = 10;
            this.lblErrorSurface.Text = "Error surface";
            // 
            // cboError
            // 
            this.cboError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboError.FormattingEnabled = true;
            this.cboError.Location = new System.Drawing.Point(121, 159);
            this.cboError.Name = "cboError";
            this.cboError.Size = new System.Drawing.Size(380, 21);
            this.cboError.TabIndex = 11;
            // 
            // txtElevationSurface
            // 
            this.txtElevationSurface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtElevationSurface.Location = new System.Drawing.Point(121, 130);
            this.txtElevationSurface.Name = "txtElevationSurface";
            this.txtElevationSurface.ReadOnly = true;
            this.txtElevationSurface.Size = new System.Drawing.Size(380, 20);
            this.txtElevationSurface.TabIndex = 9;
            // 
            // lblSampleDistance
            // 
            this.lblSampleDistance.Location = new System.Drawing.Point(-2, 105);
            this.lblSampleDistance.Name = "lblSampleDistance";
            this.lblSampleDistance.Size = new System.Drawing.Size(117, 13);
            this.lblSampleDistance.TabIndex = 6;
            this.lblSampleDistance.Text = "Sample distance ()";
            this.lblSampleDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // valSampleDistance
            // 
            this.valSampleDistance.DecimalPlaces = 2;
            this.valSampleDistance.Location = new System.Drawing.Point(121, 101);
            this.valSampleDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.valSampleDistance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.valSampleDistance.Name = "valSampleDistance";
            this.valSampleDistance.Size = new System.Drawing.Size(87, 20);
            this.valSampleDistance.TabIndex = 7;
            this.valSampleDistance.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valSampleDistance.Enter += new System.EventHandler(this.valSampleDistance_Enter);
            // 
            // frmLinearExtractionProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(513, 228);
            this.Controls.Add(this.valSampleDistance);
            this.Controls.Add(this.lblSampleDistance);
            this.Controls.Add(this.txtElevationSurface);
            this.Controls.Add(this.lblErrorSurface);
            this.Controls.Add(this.cboError);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboRoute);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(278, 238);
            this.Name = "frmLinearExtractionProperties";
            this.Text = "Derive Linear Extraction";
            this.Load += new System.EventHandler(this.frmLinearExtractionProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valSampleDistance)).EndInit();
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboRoute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblErrorSurface;
        private System.Windows.Forms.ComboBox cboError;
        private System.Windows.Forms.TextBox txtElevationSurface;
        private System.Windows.Forms.Label lblSampleDistance;
        private System.Windows.Forms.NumericUpDown valSampleDistance;
        private System.Windows.Forms.ToolTip tTip;
    }
}