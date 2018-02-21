namespace GCDCore.UserInterface.ChangeDetection
{
	partial class ucChangeBars : System.Windows.Forms.UserControl
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

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.chtControl = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.rdoAbsolute = new System.Windows.Forms.RadioButton();
            this.rdoRelative = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.chtControl)).BeginInit();
            this.SuspendLayout();
            // 
            // chtControl
            // 
            this.chtControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chtControl.Location = new System.Drawing.Point(0, 57);
            this.chtControl.Name = "chtControl";
            this.chtControl.Size = new System.Drawing.Size(200, 320);
            this.chtControl.TabIndex = 4;
            // 
            // cboType
            // 
            this.cboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(3, 3);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(194, 21);
            this.cboType.TabIndex = 5;
            // 
            // rdoAbsolute
            // 
            this.rdoAbsolute.AutoSize = true;
            this.rdoAbsolute.Checked = true;
            this.rdoAbsolute.Location = new System.Drawing.Point(3, 32);
            this.rdoAbsolute.Name = "rdoAbsolute";
            this.rdoAbsolute.Size = new System.Drawing.Size(66, 17);
            this.rdoAbsolute.TabIndex = 6;
            this.rdoAbsolute.TabStop = true;
            this.rdoAbsolute.Text = "Absolute";
            this.rdoAbsolute.UseVisualStyleBackColor = true;
            // 
            // rdoRelative
            // 
            this.rdoRelative.AutoSize = true;
            this.rdoRelative.Location = new System.Drawing.Point(75, 32);
            this.rdoRelative.Name = "rdoRelative";
            this.rdoRelative.Size = new System.Drawing.Size(64, 17);
            this.rdoRelative.TabIndex = 7;
            this.rdoRelative.Text = "Relative";
            this.rdoRelative.UseVisualStyleBackColor = true;
            // 
            // ucChangeBars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoRelative);
            this.Controls.Add(this.rdoAbsolute);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.chtControl);
            this.Name = "ucChangeBars";
            this.Size = new System.Drawing.Size(200, 381);
            this.Load += new System.EventHandler(this.ChangeBarsUC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chtControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.DataVisualization.Charting.Chart chtControl;
		internal System.Windows.Forms.ComboBox cboType;
		internal System.Windows.Forms.RadioButton rdoAbsolute;

		internal System.Windows.Forms.RadioButton rdoRelative;
	}
}
