namespace GCDCore.UserInterface.About
{
    partial class frmAbout
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
            this.grpSupport = new System.Windows.Forms.GroupBox();
            this.lnkIssues = new System.Windows.Forms.LinkLabel();
            this.lnkOnlineHelp = new System.Windows.Forms.LinkLabel();
            this.lnkWebSite = new System.Windows.Forms.LinkLabel();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpAcknowledgements = new System.Windows.Forms.GroupBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.grpSupport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.grpAcknowledgements.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSupport
            // 
            this.grpSupport.Controls.Add(this.lnkIssues);
            this.grpSupport.Controls.Add(this.lnkOnlineHelp);
            this.grpSupport.Controls.Add(this.lnkWebSite);
            this.grpSupport.Controls.Add(this.Label5);
            this.grpSupport.Controls.Add(this.Label4);
            this.grpSupport.Controls.Add(this.Label3);
            this.grpSupport.Location = new System.Drawing.Point(182, 63);
            this.grpSupport.Name = "grpSupport";
            this.grpSupport.Size = new System.Drawing.Size(337, 90);
            this.grpSupport.TabIndex = 10;
            this.grpSupport.TabStop = false;
            this.grpSupport.Text = "Support";
            // 
            // lnkIssues
            // 
            this.lnkIssues.AutoSize = true;
            this.lnkIssues.Location = new System.Drawing.Point(102, 66);
            this.lnkIssues.Name = "lnkIssues";
            this.lnkIssues.Size = new System.Drawing.Size(221, 13);
            this.lnkIssues.TabIndex = 6;
            this.lnkIssues.TabStop = true;
            this.lnkIssues.Text = "http://gcd.riverscapes.xyz/Help/known-bugs";
            this.lnkIssues.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIssues_LinkClicked);
            // 
            // lnkOnlineHelp
            // 
            this.lnkOnlineHelp.AutoSize = true;
            this.lnkOnlineHelp.LinkColor = System.Drawing.Color.Blue;
            this.lnkOnlineHelp.Location = new System.Drawing.Point(102, 43);
            this.lnkOnlineHelp.Name = "lnkOnlineHelp";
            this.lnkOnlineHelp.Size = new System.Drawing.Size(158, 13);
            this.lnkOnlineHelp.TabIndex = 5;
            this.lnkOnlineHelp.TabStop = true;
            this.lnkOnlineHelp.Text = "http://gcd.riverscapes.xyz/Help";
            this.lnkOnlineHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOnlineHelp_LinkClicked);
            // 
            // lnkWebSite
            // 
            this.lnkWebSite.AutoSize = true;
            this.lnkWebSite.Location = new System.Drawing.Point(102, 20);
            this.lnkWebSite.Name = "lnkWebSite";
            this.lnkWebSite.Size = new System.Drawing.Size(131, 13);
            this.lnkWebSite.TabIndex = 4;
            this.lnkWebSite.TabStop = true;
            this.lnkWebSite.Text = "http://gcd.riverscapes.xyz";
            this.lnkWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWebSite_LinkClicked);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(62, 66);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(37, 13);
            this.Label5.TabIndex = 3;
            this.Label5.Text = "Issues";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(39, 43);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(60, 13);
            this.Label4.TabIndex = 2;
            this.Label4.Text = "Online help";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(50, 20);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(49, 13);
            this.Label3.TabIndex = 1;
            this.Label3.Text = "Web site";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(284, 46);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(40, 13);
            this.lblVersion.TabIndex = 9;
            this.lblVersion.Text = "5.0.0.0";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(186, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(92, 16);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "GCD version";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppTitle.Location = new System.Drawing.Point(182, 12);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(298, 24);
            this.lblAppTitle.TabIndex = 7;
            this.lblAppTitle.Text = "Geomorphic Change Detection";
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::GCDCore.Properties.Resources.GCD_SplashLogo_200;
            this.PictureBox1.Location = new System.Drawing.Point(12, 12);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(159, 141);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(446, 377);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // grpAcknowledgements
            // 
            this.grpAcknowledgements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAcknowledgements.Controls.Add(this.webBrowser1);
            this.grpAcknowledgements.Location = new System.Drawing.Point(12, 159);
            this.grpAcknowledgements.Name = "grpAcknowledgements";
            this.grpAcknowledgements.Size = new System.Drawing.Size(507, 212);
            this.grpAcknowledgements.TabIndex = 11;
            this.grpAcknowledgements.TabStop = false;
            this.grpAcknowledgements.Text = "Acknowledgements";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 16);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(501, 193);
            this.webBrowser1.TabIndex = 0;
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(533, 412);
            this.Controls.Add(this.grpAcknowledgements);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.grpSupport);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblAppTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAbout";
            this.Text = "frmAbout";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.grpSupport.ResumeLayout(false);
            this.grpSupport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.grpAcknowledgements.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox grpSupport;
        internal System.Windows.Forms.LinkLabel lnkIssues;
        internal System.Windows.Forms.LinkLabel lnkOnlineHelp;
        internal System.Windows.Forms.LinkLabel lnkWebSite;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label lblVersion;
        internal System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.Label lblAppTitle;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grpAcknowledgements;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}