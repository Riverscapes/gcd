using GCDCore.Project;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using GCDCore.ErrorCalculation;

namespace GCDUserInterface.FISLibrary
{

	public partial class frmAddFIS
	{


		private void AddFISForm_Load(object sender, System.EventArgs e)
		{
		}


		private void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			try {
				if (txtName.TextLength < 1) {
					Interaction.MsgBox("Please enter a name for the FIS file.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}

				if (txtFISFile.TextLength < 1) {
					Interaction.MsgBox("Please select a FIS file.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}

				if (!System.IO.File.Exists(txtFISFile.Text)) {
					System.Windows.Forms.MessageBox.Show("The FIS file does not exist.", GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}

				try {
					FIS.FISRuleFile theFile = new FIS.FISRuleFile(new System.IO.FileInfo(txtFISFile.Text));

				} catch (Exception ex) {
					System.Windows.Forms.MessageBox.Show("The FIS file is invalid and/or badly formatted. Check that the formatting of the file contents match the MatLab fully inference toolbox specifications and try again.", GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}

				// old binding source code removed here
				throw new NotImplementedException();

			} catch (Exception ex) {
				if (ex.Message.ToString().ToLower().Contains("name")) {
					Interaction.MsgBox("Please select a unique name for the selected FIS file.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
				} else if (ex.Message.ToString().ToLower().Contains("path")) {
					Interaction.MsgBox("A FIS file with the same name is already present in the FIS library.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong);
				} else {
					Interaction.MsgBox("An error occured while trying to save the information, " + Constants.vbNewLine + ex.Message);
				}
				DialogResult = System.Windows.Forms.DialogResult.None;
			}

		}


		private void btnBrowseFIS_Click(System.Object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
			fileDialog.Title = "Select a FIS File";
			fileDialog.Filter = "GCD FIS Files (*.fis) | *.fis";
			fileDialog.InitialDirectory = fileDialog.RestoreDirectory == false;

			if (fileDialog.ShowDialog() == DialogResult.OK) {
				txtFISFile.Text = fileDialog.FileName;

				if (string.IsNullOrEmpty(txtName.Text)) {
					txtName.Text = System.IO.Path.GetFileNameWithoutExtension(fileDialog.FileName);
					txtName.SelectAll();
					txtName.Focus();
				}
			}

		}

		private void btnHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
		}
	}

}
