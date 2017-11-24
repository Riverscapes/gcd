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
using System.Windows.Forms;
using System.IO;

namespace GCDUserInterface.FISLibrary
{

	public partial class frmEditFIS
	{


		private string m_sPath;

		public frmEditFIS(string sPath)
		{
			Load += EditFISForm_Load;
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			if (string.IsNullOrEmpty(sPath)) {
				throw new Exception("Null or empty FIS path.");
			}

			m_sPath = sPath;

		}


		private void EditFISForm_Load(System.Object sender, System.EventArgs e)
		{
			try {
				if (System.IO.File.Exists(m_sPath)) {
					StreamReader s = new StreamReader(m_sPath);

					while (!s.EndOfStream) {
						txtEditor.Text += s.ReadLine() + Constants.vbNewLine;
					}
					txtEditor.Select(0, 0);
					s.Close();
				}

				txtEditor.ReadOnly = true;

			} catch (Exception ex) {
				Exception ex2 = new Exception("Error loading FIS file into text editor.", ex);
				ex2.Data.Add("FIS Path", m_sPath);
				throw ex2;
			}

			// TOOLTIPS
			//ttpTooltip.SetToolTip(btnEdit, My.Resources.ttpEditFISFormBtnEdit)
			//ttpTooltip.SetToolTip(btnOK, My.Resources.ttpEditFISFormBtnOK)
			//ttpTooltip.SetToolTip(btnSave, My.Resources.ttpEditFISFormBtnSave)
			//ttpTooltip.SetToolTip(btnSaveAs, My.Resources.ttpEditFISFormBtnSaveAs)
		}


		private void btnEdit_Click(System.Object sender, System.EventArgs e)
		{
			txtEditor.ReadOnly = false;

		}


		private void btnSaveAs_Click(System.Object sender, System.EventArgs e)
		{
			Stream myStream = null;
			string txtFIS = txtEditor.Text;

			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.Title = "Save FIS file";
			fileDialog.Filter = "GCD FIS Files (*.fis) | *.fis";
			fileDialog.RestoreDirectory = false;

			if (fileDialog.ShowDialog() == DialogResult.OK) {
				myStream = fileDialog.OpenFile();

				if (!(txtFIS.Length < 1)) {
					using (StreamWriter sw = new StreamWriter(myStream)) {
						sw.Write(txtEditor.Text);
						sw.Close();
						txtEditor.ReadOnly = true;
						Interaction.MsgBox("FIS file saved successfully.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);
					}
				} else {
					Interaction.MsgBox("The edited FIS is empty.");
				}
			}

		}


		private void btnSave_Click(System.Object sender, System.EventArgs e)
		{
			if (System.IO.File.Exists(m_sPath)) {
				StreamWriter s = new StreamWriter(m_sPath);
				s.Write(txtEditor.Text);
				s.Close();
			}

			Interaction.MsgBox("FIS file saved successfully.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong);

			//Else - if file does not exist - would you like to remove from library?

		}

		private void btnHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
		}

	}

}
