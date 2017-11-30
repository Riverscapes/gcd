using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace GCDCore.UserInterface.FISLibrary
{
    public partial class frmEditFIS
    {
        private FileInfo FISRuleFile;

        public frmEditFIS(FileInfo filePath)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            FISRuleFile = filePath;
        }

        private void EditFISForm_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (FISRuleFile.Exists)
                {
                    //txtEditor.Text = File.ReadAllText(FISRuleFile.FullName);
                    string[] lines = File.ReadAllLines(FISRuleFile.FullName);
                    txtEditor.Text = String.Join("\r\n", lines);
                    txtEditor.Select(0, 0);
                }
                else
                {
                    MessageBox.Show("The FIS Rule File does not exist." + Environment.NewLine + FISRuleFile.FullName, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                txtEditor.ReadOnly = true;
            }
            catch (Exception ex)
            {
                ex.Data["FIS Rule File"] = FISRuleFile.FullName;
                naru.error.ExceptionUI.HandleException(ex, "Error loading FIS file into text editor.");
            }
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

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                myStream = fileDialog.OpenFile();

                if (!(txtFIS.Length < 1))
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        sw.Write(txtEditor.Text);
                        sw.Close();
                        txtEditor.ReadOnly = true;
                        MessageBox.Show("FIS file saved successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("The edited FIS is empty.");
                }
            }
        }

        private void btnSave_Click(System.Object sender, System.EventArgs e)
        {
            if (File.Exists(FISRuleFile.FullName))
            {
                File.WriteAllText(FISRuleFile.FullName, txtEditor.Text);
            }

            MessageBox.Show("FIS file saved successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
        }
    }
}
