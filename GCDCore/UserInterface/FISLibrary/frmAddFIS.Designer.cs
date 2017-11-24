using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace GCDCore.UserInterface.FISLibrary
{
	partial class frmAddFIS : System.Windows.Forms.Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddFIS));
			this.txtFISFile = new System.Windows.Forms.TextBox();
			this.FISTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.txtName = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.Label1 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this.btnBrowseFIS = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)this.FISTableBindingSource).BeginInit();
			this.SuspendLayout();
			//
			//txtFISFile
			//
			this.txtFISFile.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtFISFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FISTableBindingSource, "Path", true));
			this.txtFISFile.Location = new System.Drawing.Point(60, 12);
			this.txtFISFile.Name = "txtFISFile";
			this.txtFISFile.ReadOnly = true;
			this.txtFISFile.Size = new System.Drawing.Size(441, 20);
			this.txtFISFile.TabIndex = 1;
			this.txtFISFile.TabStop = false;
			//
			//FISTableBindingSource
			//
			this.FISTableBindingSource.DataMember = "FISTable";
			//
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.FISTableBindingSource, "Name", true));
			this.txtName.Location = new System.Drawing.Point(60, 48);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(441, 20);
			this.txtName.TabIndex = 4;
			//
			//btnOK
			//
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(377, 79);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//btnHelp
			//
			this.btnHelp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnHelp.Location = new System.Drawing.Point(12, 83);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 23);
			this.btnHelp.TabIndex = 5;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(458, 79);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(12, 17);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(42, 13);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "FIS file:";
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(12, 51);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(38, 13);
			this.Label2.TabIndex = 3;
			this.Label2.Text = "Name:";
			//
			//btnBrowseFIS
			//
			this.btnBrowseFIS.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnBrowseFIS.Image = (System.Drawing.Image)resources.GetObject("btnBrowseFIS.Image");
			this.btnBrowseFIS.Location = new System.Drawing.Point(506, 10);
			this.btnBrowseFIS.Name = "btnBrowseFIS";
			this.btnBrowseFIS.Size = new System.Drawing.Size(29, 23);
			this.btnBrowseFIS.TabIndex = 2;
			this.btnBrowseFIS.UseVisualStyleBackColor = true;
			//
			//AddFISForm
			//
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(545, 114);
			this.Controls.Add(this.btnBrowseFIS);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.txtFISFile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddFISForm";
			this.Text = "Add FIS Library File";
			((System.ComponentModel.ISupportInitialize)this.FISTableBindingSource).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.TextBox txtFISFile;
		internal System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Button withEventsField_btnOK;
		internal System.Windows.Forms.Button btnOK {
			get { return withEventsField_btnOK; }
			set {
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click -= btnOK_Click;
				}
				withEventsField_btnOK = value;
				if (withEventsField_btnOK != null) {
					withEventsField_btnOK.Click += btnOK_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnHelp;
		internal System.Windows.Forms.Button btnHelp {
			get { return withEventsField_btnHelp; }
			set {
				if (withEventsField_btnHelp != null) {
					withEventsField_btnHelp.Click -= btnHelp_Click;
				}
				withEventsField_btnHelp = value;
				if (withEventsField_btnHelp != null) {
					withEventsField_btnHelp.Click += btnHelp_Click;
				}
			}
		}
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label2;
		private System.Windows.Forms.Button withEventsField_btnBrowseFIS;
		internal System.Windows.Forms.Button btnBrowseFIS {
			get { return withEventsField_btnBrowseFIS; }
			set {
				if (withEventsField_btnBrowseFIS != null) {
					withEventsField_btnBrowseFIS.Click -= btnBrowseFIS_Click;
				}
				withEventsField_btnBrowseFIS = value;
				if (withEventsField_btnBrowseFIS != null) {
					withEventsField_btnBrowseFIS.Click += btnBrowseFIS_Click;
				}
			}
		}
		internal System.Windows.Forms.BindingSource FISTableBindingSource;
		public frmAddFIS()
		{
			Load += AddFISForm_Load;
			InitializeComponent();
		}
	}
}
