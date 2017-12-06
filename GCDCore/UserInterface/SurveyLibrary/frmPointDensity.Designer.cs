namespace GCDCore.UserInterface.SurveyLibrary
{
	partial class frmPointDensity : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.valSampleDistance = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.ttpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.cboNeighbourhood = new System.Windows.Forms.ComboBox();
            this.ucPointCloud = new GCDCore.UserInterface.UtilityForms.ucVectorInput();
            ((System.ComponentModel.ISupportInitialize)(this.valSampleDistance)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(44, 17);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(60, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Point cloud";
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new System.Drawing.Point(39, 56);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(49, 13);
            this.lblDistance.TabIndex = 2;
            this.lblDistance.Text = "Distance";
            // 
            // valSampleDistance
            // 
            this.valSampleDistance.DecimalPlaces = 1;
            this.valSampleDistance.Location = new System.Drawing.Point(95, 52);
            this.valSampleDistance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.valSampleDistance.Name = "valSampleDistance";
            this.valSampleDistance.Size = new System.Drawing.Size(108, 20);
            this.valSampleDistance.TabIndex = 3;
            this.valSampleDistance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(397, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(316, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            this.btnHelp.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnHelp.Enabled = false;
            this.btnHelp.Location = new System.Drawing.Point(12, 140);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 5;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.cboNeighbourhood);
            this.GroupBox1.Controls.Add(this.valSampleDistance);
            this.GroupBox1.Controls.Add(this.lblDistance);
            this.GroupBox1.Location = new System.Drawing.Point(15, 39);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(455, 90);
            this.GroupBox1.TabIndex = 2;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Neighborhood:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(57, 25);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(31, 13);
            this.Label3.TabIndex = 0;
            this.Label3.Text = "Type";
            // 
            // cboNeighbourhood
            // 
            this.cboNeighbourhood.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNeighbourhood.FormattingEnabled = true;
            this.cboNeighbourhood.Location = new System.Drawing.Point(95, 21);
            this.cboNeighbourhood.Name = "cboNeighbourhood";
            this.cboNeighbourhood.Size = new System.Drawing.Size(327, 21);
            this.cboNeighbourhood.TabIndex = 1;
            // 
            // ucPointCloud
            // 
            this.ucPointCloud.Location = new System.Drawing.Point(110, 11);
            this.ucPointCloud.Name = "ucPointCloud";
            this.ucPointCloud.Size = new System.Drawing.Size(360, 25);
            this.ucPointCloud.TabIndex = 1;
            // 
            // frmPointDensity
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(483, 175);
            this.Controls.Add(this.ucPointCloud);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPointDensity";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Generate Point Density Raster";
            this.Load += new System.EventHandler(this.frmPointDensity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valSampleDistance)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label lblDistance;
		internal System.Windows.Forms.NumericUpDown valSampleDistance;
		internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnHelp;
		internal System.Windows.Forms.ToolTip ttpToolTip;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.ComboBox cboNeighbourhood;
		internal GCDCore.UserInterface.UtilityForms.ucVectorInput ucPointCloud;
	}

}
