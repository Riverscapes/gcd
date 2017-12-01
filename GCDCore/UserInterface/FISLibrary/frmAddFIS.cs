using System.Windows.Forms;
using System;
using System.Diagnostics;
using GCDCore.ErrorCalculation.FIS;
using System.Linq;

namespace GCDCore.UserInterface.FISLibrary
{
    public partial class frmAddFIS
    {
        public FISLibraryItem FISItem { get; internal set; }

        public frmAddFIS()
        {
            InitializeComponent();
        }


        private void AddFISForm_Load(object sender, System.EventArgs e)
        {
        }

        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                FISItem = new FISLibraryItem(txtName.Text, new System.IO.FileInfo(txtFISFile.Text));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToLower().Contains("name"))
                {
                    MessageBox.Show("Please select a unique name for the selected FIS file.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.Message.ToString().ToLower().Contains("path"))
                {
                    MessageBox.Show("A FIS file with the same name is already present in the FIS library.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("An error occured while trying to save the information, " + Environment.NewLine + ex.Message);
                }
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            // Santiy check to avoid empty string names
            txtName.Text = txtName.Text.Trim();

            if (txtName.TextLength < 1)
            {
                MessageBox.Show("Please enter a name for the FIS file.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (txtFISFile.TextLength < 1)
            {
                MessageBox.Show("Please select a FIS file.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (!System.IO.File.Exists(txtFISFile.Text))
            {
                MessageBox.Show("The FIS file does not exist.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (GCDCore.Project.ProjectManager.FISLibrary.Count<FISLibraryItem>(x => string.Compare(x.Name, txtName.Text,true)==0) > 0)
            {
                MessageBox.Show("A FIS library item already exists with this name. Each FIS library item must have a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }
            
            if (GCDCore.Project.ProjectManager.FISLibrary.Count<FISLibraryItem>(x => string.Compare(txtFISFile.Text, x.FilePathString,true)==0) > 0)
            {
                MessageBox.Show("A FIS library item already refers to this FIS rule file. Each FIS library item must refere to a different file.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            try
            {
                ErrorCalculation.FIS.FISRuleFile theFile = new ErrorCalculation.FIS.FISRuleFile(new System.IO.FileInfo(txtFISFile.Text));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("The FIS file is invalid and/or badly formatted. Check that the formatting of the file contents match the MatLab fully inference toolbox specifications and try again.", GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void btnBrowseFIS_Click(System.Object sender, System.EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "Select a FIS File";
            fileDialog.Filter = "GCD FIS Files (*.fis) | *.fis";
            fileDialog.RestoreDirectory = false;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFISFile.Text = fileDialog.FileName;

                if (string.IsNullOrEmpty(txtName.Text))
                {
                    txtName.Text = System.IO.Path.GetFileNameWithoutExtension(fileDialog.FileName);
                    txtName.SelectAll();
                    txtName.Focus();
                }
            }
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
        }
    }
}
