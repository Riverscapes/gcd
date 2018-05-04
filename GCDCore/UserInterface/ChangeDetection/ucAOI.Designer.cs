namespace GCDCore.UserInterface.ChangeDetection
{
    partial class ucAOI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboAOI = new System.Windows.Forms.ComboBox();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboAOI);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(768, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Area of Interest";
            // 
            // cboAOI
            // 
            this.cboAOI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAOI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAOI.FormattingEnabled = true;
            this.cboAOI.Location = new System.Drawing.Point(6, 19);
            this.cboAOI.Name = "cboAOI";
            this.cboAOI.Size = new System.Drawing.Size(756, 21);
            this.cboAOI.TabIndex = 0;
            this.cboAOI.SelectedIndexChanged += new System.EventHandler(this.cboAOI_SelectedIndexChanged);
            // 
            // ucAOI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ucAOI";
            this.Size = new System.Drawing.Size(768, 50);
            this.Load += new System.EventHandler(this.ucAOI_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboAOI;
        private System.Windows.Forms.ToolTip tTip;
    }
}
