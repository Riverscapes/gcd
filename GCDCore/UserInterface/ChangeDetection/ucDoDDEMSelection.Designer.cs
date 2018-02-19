namespace GCDCore.UserInterface.ChangeDetection
{
    partial class ucDoDDEMSelection
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
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.cboOldSurface = new System.Windows.Forms.ComboBox();
            this.cboOldError = new System.Windows.Forms.ComboBox();
            this.lblOldError = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.cboNewSurface = new System.Windows.Forms.ComboBox();
            this.cboNewError = new System.Windows.Forms.ComboBox();
            this.lblNewError = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupBox4.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.cboOldSurface);
            this.GroupBox4.Controls.Add(this.cboOldError);
            this.GroupBox4.Controls.Add(this.lblOldError);
            this.GroupBox4.Controls.Add(this.Label7);
            this.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox4.Location = new System.Drawing.Point(251, 3);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(242, 83);
            this.GroupBox4.TabIndex = 7;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Old DEM Survey or Reference Surface";
            // 
            // cboOldSurface
            // 
            this.cboOldSurface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOldSurface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOldSurface.FormattingEnabled = true;
            this.cboOldSurface.Location = new System.Drawing.Point(52, 21);
            this.cboOldSurface.Name = "cboOldSurface";
            this.cboOldSurface.Size = new System.Drawing.Size(184, 21);
            this.cboOldSurface.TabIndex = 1;
            this.cboOldSurface.SelectedIndexChanged += new System.EventHandler(this.SurfaceComboSelectedIndexChanged);
            // 
            // cboOldError
            // 
            this.cboOldError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOldError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOldError.FormattingEnabled = true;
            this.cboOldError.Location = new System.Drawing.Point(52, 48);
            this.cboOldError.Name = "cboOldError";
            this.cboOldError.Size = new System.Drawing.Size(184, 21);
            this.cboOldError.TabIndex = 3;
            // 
            // lblOldError
            // 
            this.lblOldError.AutoSize = true;
            this.lblOldError.Location = new System.Drawing.Point(20, 52);
            this.lblOldError.Name = "lblOldError";
            this.lblOldError.Size = new System.Drawing.Size(29, 13);
            this.lblOldError.TabIndex = 2;
            this.lblOldError.Text = "Error";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(5, 25);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(44, 13);
            this.Label7.TabIndex = 0;
            this.Label7.Text = "Surface";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.cboNewSurface);
            this.GroupBox1.Controls.Add(this.cboNewError);
            this.GroupBox1.Controls.Add(this.lblNewError);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox1.Location = new System.Drawing.Point(3, 3);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(242, 83);
            this.GroupBox1.TabIndex = 6;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "New or Current DEM Survey";
            // 
            // cboNewSurface
            // 
            this.cboNewSurface.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNewSurface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewSurface.FormattingEnabled = true;
            this.cboNewSurface.Location = new System.Drawing.Point(54, 21);
            this.cboNewSurface.Name = "cboNewSurface";
            this.cboNewSurface.Size = new System.Drawing.Size(182, 21);
            this.cboNewSurface.TabIndex = 1;
            this.cboNewSurface.SelectedIndexChanged += new System.EventHandler(this.SurfaceComboSelectedIndexChanged);
            // 
            // cboNewError
            // 
            this.cboNewError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNewError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewError.FormattingEnabled = true;
            this.cboNewError.Location = new System.Drawing.Point(54, 48);
            this.cboNewError.Name = "cboNewError";
            this.cboNewError.Size = new System.Drawing.Size(182, 21);
            this.cboNewError.TabIndex = 3;
            // 
            // lblNewError
            // 
            this.lblNewError.AutoSize = true;
            this.lblNewError.Location = new System.Drawing.Point(21, 52);
            this.lblNewError.Name = "lblNewError";
            this.lblNewError.Size = new System.Drawing.Size(29, 13);
            this.lblNewError.TabIndex = 2;
            this.lblNewError.Text = "Error";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 25);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(44, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Surface";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.GroupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.GroupBox4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(496, 89);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // ucDoDDEMSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ucDoDDEMSelection";
            this.Size = new System.Drawing.Size(496, 89);
            this.Load += new System.EventHandler(this.ucDoDDEMSelection_Load);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.ComboBox cboOldSurface;
        internal System.Windows.Forms.ComboBox cboOldError;
        internal System.Windows.Forms.Label lblOldError;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.ComboBox cboNewSurface;
        internal System.Windows.Forms.ComboBox cboNewError;
        internal System.Windows.Forms.Label lblNewError;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
