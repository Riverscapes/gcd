namespace GCDCore.UserInterface.BudgetSegregation
{
    partial class frmBudgetSegProperties
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetSegProperties));
            this.Label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cboRegularMasks = new System.Windows.Forms.ComboBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoRegular = new System.Windows.Forms.RadioButton();
            this.rdoDirectional = new System.Windows.Forms.RadioButton();
            this.cboDirMasks = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(56, 13);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(95, 9);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(408, 20);
            this.txtName.TabIndex = 1;
            // 
            // cboRegularMasks
            // 
            this.cboRegularMasks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRegularMasks.FormattingEnabled = true;
            this.cboRegularMasks.Location = new System.Drawing.Point(124, 19);
            this.cboRegularMasks.Name = "cboRegularMasks";
            this.cboRegularMasks.Size = new System.Drawing.Size(361, 21);
            this.cboRegularMasks.TabIndex = 1;
            this.cboRegularMasks.SelectedIndexChanged += new System.EventHandler(this.cboMasks_SelectedIndexChanged);
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(23, 43);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(68, 13);
            this.Label8.TabIndex = 2;
            this.Label8.Text = "Output folder";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(95, 39);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.ReadOnly = true;
            this.txtOutputFolder.Size = new System.Drawing.Size(408, 20);
            this.txtOutputFolder.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(428, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::GCDCore.Properties.Resources.Save;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(347, 163);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 163);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 7;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoDirectional);
            this.groupBox1.Controls.Add(this.cboDirMasks);
            this.groupBox1.Controls.Add(this.rdoRegular);
            this.groupBox1.Controls.Add(this.cboRegularMasks);
            this.groupBox1.Location = new System.Drawing.Point(12, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 82);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mask";
            // 
            // rdoRegular
            // 
            this.rdoRegular.AutoSize = true;
            this.rdoRegular.Checked = true;
            this.rdoRegular.Location = new System.Drawing.Point(10, 21);
            this.rdoRegular.Name = "rdoRegular";
            this.rdoRegular.Size = new System.Drawing.Size(95, 17);
            this.rdoRegular.TabIndex = 0;
            this.rdoRegular.Text = "Regular masks";
            this.rdoRegular.UseVisualStyleBackColor = true;
            this.rdoRegular.CheckedChanged += new System.EventHandler(this.MaskTypeChanged);
            // 
            // rdoDirectional
            // 
            this.rdoDirectional.AutoSize = true;
            this.rdoDirectional.Location = new System.Drawing.Point(10, 51);
            this.rdoDirectional.Name = "rdoDirectional";
            this.rdoDirectional.Size = new System.Drawing.Size(108, 17);
            this.rdoDirectional.TabIndex = 2;
            this.rdoDirectional.Text = "Directional masks";
            this.rdoDirectional.UseVisualStyleBackColor = true;
            this.rdoDirectional.CheckedChanged += new System.EventHandler(this.MaskTypeChanged);
            // 
            // cboDirMasks
            // 
            this.cboDirMasks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDirMasks.FormattingEnabled = true;
            this.cboDirMasks.Location = new System.Drawing.Point(124, 49);
            this.cboDirMasks.Name = "cboDirMasks";
            this.cboDirMasks.Size = new System.Drawing.Size(361, 21);
            this.cboDirMasks.TabIndex = 3;
            // 
            // frmBudgetSegProperties
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(517, 198);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBudgetSegProperties";
            this.Text = "Budget Segregation Properties";
            this.Load += new System.EventHandler(this.frmBudgetSegProperties_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdHelp;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.ComboBox cboRegularMasks;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoDirectional;
        private System.Windows.Forms.ComboBox cboDirMasks;
        private System.Windows.Forms.RadioButton rdoRegular;
    }
}
