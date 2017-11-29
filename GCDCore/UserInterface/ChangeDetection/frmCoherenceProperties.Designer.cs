namespace GCDCore.UserInterface.ChangeDetection
{
    partial class frmCoherenceProperties : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
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
            System.Windows.Forms.Label xDimensionsLabel;
            System.Windows.Forms.Label xLowPercentSuffixLabel;
            System.Windows.Forms.Label xLowPercentPrefixLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCoherenceProperties));
            this.fisPictureBox = new System.Windows.Forms.PictureBox();
            this.xHighPercentSuffixLabel = new System.Windows.Forms.Label();
            this.xHighPercentPrefixLabel = new System.Windows.Forms.Label();
            this.dimensionsLabel = new System.Windows.Forms.Label();
            this.cboFilterSize = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.okButton = new System.Windows.Forms.Button();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.numGreater = new System.Windows.Forms.NumericUpDown();
            this.numLess = new System.Windows.Forms.NumericUpDown();
            this.btnHelp = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            xDimensionsLabel = new System.Windows.Forms.Label();
            xLowPercentSuffixLabel = new System.Windows.Forms.Label();
            xLowPercentPrefixLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fisPictureBox)).BeginInit();
            this.grpInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGreater)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLess)).BeginInit();
            this.SuspendLayout();
            // 
            // xDimensionsLabel
            // 
            xDimensionsLabel.AutoSize = true;
            xDimensionsLabel.Location = new System.Drawing.Point(6, 30);
            xDimensionsLabel.Name = "xDimensionsLabel";
            xDimensionsLabel.Size = new System.Drawing.Size(139, 13);
            xDimensionsLabel.TabIndex = 12;
            xDimensionsLabel.Text = "Moving window dimensions:";
            // 
            // xLowPercentSuffixLabel
            // 
            xLowPercentSuffixLabel.AutoSize = true;
            xLowPercentSuffixLabel.Location = new System.Drawing.Point(188, 69);
            xLowPercentSuffixLabel.Name = "xLowPercentSuffixLabel";
            xLowPercentSuffixLabel.Size = new System.Drawing.Size(110, 13);
            xLowPercentSuffixLabel.TabIndex = 17;
            xLowPercentSuffixLabel.Text = "%, then probability = 0";
            // 
            // xLowPercentPrefixLabel
            // 
            xLowPercentPrefixLabel.AutoSize = true;
            xLowPercentPrefixLabel.Location = new System.Drawing.Point(6, 69);
            xLowPercentPrefixLabel.Name = "xLowPercentPrefixLabel";
            xLowPercentPrefixLabel.Size = new System.Drawing.Size(126, 13);
            xLowPercentPrefixLabel.TabIndex = 15;
            xLowPercentPrefixLabel.Text = "A) If percent of cells is <=";
            // 
            // fisPictureBox
            // 
            this.fisPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fisPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("fisPictureBox.Image")));
            this.fisPictureBox.Location = new System.Drawing.Point(9, 9);
            this.fisPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.fisPictureBox.Name = "fisPictureBox";
            this.fisPictureBox.Size = new System.Drawing.Size(298, 193);
            this.fisPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.fisPictureBox.TabIndex = 23;
            this.fisPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.fisPictureBox, resources.GetString("fisPictureBox.ToolTip"));
            // 
            // xHighPercentSuffixLabel
            // 
            this.xHighPercentSuffixLabel.AutoSize = true;
            this.xHighPercentSuffixLabel.Location = new System.Drawing.Point(188, 95);
            this.xHighPercentSuffixLabel.Name = "xHighPercentSuffixLabel";
            this.xHighPercentSuffixLabel.Size = new System.Drawing.Size(110, 13);
            this.xHighPercentSuffixLabel.TabIndex = 20;
            this.xHighPercentSuffixLabel.Text = "%, then probability = 1";
            // 
            // xHighPercentPrefixLabel
            // 
            this.xHighPercentPrefixLabel.AutoSize = true;
            this.xHighPercentPrefixLabel.Location = new System.Drawing.Point(6, 95);
            this.xHighPercentPrefixLabel.Name = "xHighPercentPrefixLabel";
            this.xHighPercentPrefixLabel.Size = new System.Drawing.Size(126, 13);
            this.xHighPercentPrefixLabel.TabIndex = 18;
            this.xHighPercentPrefixLabel.Text = "B) If percent of cells is >=";
            // 
            // dimensionsLabel
            // 
            this.dimensionsLabel.AutoSize = true;
            this.dimensionsLabel.Location = new System.Drawing.Point(228, 30);
            this.dimensionsLabel.Name = "dimensionsLabel";
            this.dimensionsLabel.Size = new System.Drawing.Size(28, 13);
            this.dimensionsLabel.TabIndex = 14;
            this.dimensionsLabel.Text = "cells";
            // 
            // cboFilterSize
            // 
            this.cboFilterSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboFilterSize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFilterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterSize.FormattingEnabled = true;
            this.cboFilterSize.Items.AddRange(new object[] {
            "3 x 3",
            "5 x 5",
            "7 x 7",
            "9 x 9",
            "11 x 11",
            "13 x 13",
            "15 x 15"});
            this.cboFilterSize.Location = new System.Drawing.Point(151, 27);
            this.cboFilterSize.Name = "cboFilterSize";
            this.cboFilterSize.Size = new System.Drawing.Size(71, 21);
            this.cboFilterSize.TabIndex = 13;
            this.toolTip1.SetToolTip(this.cboFilterSize, "Choose the size of your moving window.");
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(484, 208);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 21;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.numGreater);
            this.grpInput.Controls.Add(this.numLess);
            this.grpInput.Controls.Add(xDimensionsLabel);
            this.grpInput.Controls.Add(xLowPercentPrefixLabel);
            this.grpInput.Controls.Add(this.xHighPercentSuffixLabel);
            this.grpInput.Controls.Add(this.cboFilterSize);
            this.grpInput.Controls.Add(xLowPercentSuffixLabel);
            this.grpInput.Controls.Add(this.xHighPercentPrefixLabel);
            this.grpInput.Controls.Add(this.dimensionsLabel);
            this.grpInput.Location = new System.Drawing.Point(321, 9);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(319, 193);
            this.grpInput.TabIndex = 24;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Inputs";
            // 
            // numGreater
            // 
            this.numGreater.Location = new System.Drawing.Point(138, 93);
            this.numGreater.Name = "numGreater";
            this.numGreater.Size = new System.Drawing.Size(44, 20);
            this.numGreater.TabIndex = 22;
            this.numGreater.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numLess
            // 
            this.numLess.Location = new System.Drawing.Point(138, 67);
            this.numLess.Name = "numLess";
            this.numLess.Size = new System.Drawing.Size(44, 20);
            this.numLess.TabIndex = 21;
            this.numLess.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // btnHelp
            // 
            this.btnHelp.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnHelp.Enabled = false;
            this.btnHelp.Location = new System.Drawing.Point(9, 208);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 25;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(565, 208);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 26;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // frmCoherenceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(657, 239);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.fisPictureBox);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCoherenceProperties";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Transform Function Parameter Editor";
            ((System.ComponentModel.ISupportInitialize)(this.fisPictureBox)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGreater)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLess)).EndInit();
            this.ResumeLayout(false);

        }

        internal System.Windows.Forms.PictureBox fisPictureBox;
        internal System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.Label xHighPercentSuffixLabel;
        internal System.Windows.Forms.Label xHighPercentPrefixLabel;
        internal System.Windows.Forms.Label dimensionsLabel;
        internal System.Windows.Forms.ComboBox cboFilterSize;
        internal System.Windows.Forms.Button okButton;
        internal System.Windows.Forms.GroupBox grpInput;
        internal System.Windows.Forms.Button btnHelp;
        internal System.Windows.Forms.NumericUpDown numGreater;
        internal System.Windows.Forms.NumericUpDown numLess;
        internal System.Windows.Forms.Button cmdCancel;
    }
}
