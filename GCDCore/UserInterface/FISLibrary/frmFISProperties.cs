using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GCDCore.ErrorCalculation.FIS;

namespace GCDCore.UserInterface.FISLibrary
{
    public partial class frmFISProperties : Form
    {
        public readonly FISLibraryItem FISLibraryItem;
        private readonly bool IsEditMode;

        public frmFISProperties(FISLibraryItem item)
        {
            InitializeComponent();
            FISLibraryItem = item;
            IsEditMode = true;

            if (item.FISType == ErrorCalculation.FIS.FISLibrary.FISLibraryItemTypes.System)
            {
                cmdOK.Visible = false;
                cmdCancel.Text = "Close";
                txtName.ReadOnly = true;
                txtOutputName.ReadOnly = true;
                txtOutputUnits.ReadOnly = true;
                txtOutputDescription.ReadOnly = true;
                cmdEditFISFile.Enabled = false;
                grdPublications.ReadOnly = true;
                grdInputs.ReadOnly = true;
                grdMetaData.ReadOnly = true;
                grdDatasets.ReadOnly = true;
                txtDescription.ReadOnly = true;
            }
            else
            {
                cmdOK.Text = Properties.Resources.UpdateButtonText;
            }
        }

        public frmFISProperties(string fisFilePath)
        {
            InitializeComponent();
            FISLibraryItem = new FISLibraryItem(fisFilePath, ErrorCalculation.FIS.FISLibrary.FISLibraryItemTypes.User);
            cmdOK.Text = Properties.Resources.CreateButtonText;
            IsEditMode = false;
        }

        private void frmFISProperties_Load(object sender, EventArgs e)
        {
            InitializeToolTips();

            grdDatasets.AutoGenerateColumns = false;
            grdPublications.AutoGenerateColumns = false;
            grdInputs.AutoGenerateColumns = false;
            grdMetaData.AutoGenerateColumns = false;

            txtName.Text = FISLibraryItem.Name;
            txtFilePath.Text = FISLibraryItem.FilePath.FullName;
            grdInputs.DataSource = FISLibraryItem.Inputs;
            txtOutputName.Text = FISLibraryItem.OutputName;
            txtOutputUnits.Text = FISLibraryItem.OutputUnits;
            txtOutputDescription.Text = FISLibraryItem.OutputDescription;
            grdPublications.DataSource = FISLibraryItem.Publications;
            grdDatasets.DataSource = FISLibraryItem.ExampleDatasets;
            txtDescription.Text = FISLibraryItem.Description;
            grdMetaData.DataSource = FISLibraryItem.Metadata;

            try
            {
                string[] lines = File.ReadAllLines(FISLibraryItem.FilePath.FullName);
                txtFISFile.Text = String.Join("\r\n", lines);
                txtFISFile.Select(0, 0);
            }
            catch (Exception ex)
            {
                txtFISFile.Text = string.Format("Error Reading FIS File:\n{0}", ex.Message);
            }

            txtFISFile.ReadOnly = true;
            cmdSaveFISFile.Enabled = false;
        }

        private void InitializeToolTips()
        {
            tTip.SetToolTip(txtName, "The name used to represent this FIS rule file in the GCD user interface.");
            tTip.SetToolTip(txtFilePath, "The file path where the FIS rule file resides.");
            tTip.SetToolTip(grdInputs, "Listing of the FIS rule file inputs, their units, description and source (where the input data come from).");
            tTip.SetToolTip(txtOutputName, "The name of the one and only FIS rule output dataset.");
            tTip.SetToolTip(txtOutputUnits, "The units for the FIS rule file output.");
            tTip.SetToolTip(txtDescription, "Brief description of the FIS rule file output.");
            tTip.SetToolTip(cmdEditFISFile, "Edit the FIS rule file contents text. Only user FIS rule files are editable.");
            tTip.SetToolTip(cmdSaveFISFile, "Save edits to the FIS rule file back to the original FIS rule file path.");
            tTip.SetToolTip(txtFISFile, "FIS rule file contents. Click edit to edit user FIS rule files only.");
            tTip.SetToolTip(grdPublications, "List of citations and corresponding URLs if the FIS was part of an official publication.");
            tTip.SetToolTip(grdDatasets, "List of publicly available datasets and the corresponding URLs that demonstrat the FIS rule file in use.");
            tTip.SetToolTip(grdMetaData, "List of key value pairs of metadata attached to the FIS rule file.");
            tTip.SetToolTip(txtDescription, "General remarks describing the FIS rule file.");
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the FIS library entry. This can be different than the name specified in the FIS rule file itself.", "FIS Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (IsEditMode && !txtFISFile.ReadOnly)
            {
                MessageBox.Show("You must save your changes to the FIS rule file before you can continue.", "Editing FIS File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControl1.SelectedTab = tabFISFile;
                cmdSaveFISFile.Select();
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            FISLibraryItem.Name = txtName.Text;
            FISLibraryItem.OutputName = txtOutputName.Text;
            FISLibraryItem.OutputUnits = txtOutputUnits.Text;
            FISLibraryItem.OutputDescription = txtOutputDescription.Text;
            FISLibraryItem.Description = txtDescription.Text;

            if (!IsEditMode)
            {
                GCDCore.Project.ProjectManager.FISLibrary.FISItems.Add(FISLibraryItem);
            }
            GCDCore.Project.ProjectManager.FISLibrary.Save();
        }

        private void cmdEditFISFile_Click(object sender, EventArgs e)
        {
            txtFISFile.ReadOnly = false;
            cmdEditFISFile.Enabled = false;
            cmdSaveFISFile.Enabled = true;
        }

        private void cmdSaveFISFile_Click(object sender, EventArgs e)
        {
            cmdSaveFISFile.Enabled = false;
            cmdEditFISFile.Enabled = true;
            txtFISFile.ReadOnly = true;

            try
            {
                if (FISLibraryItem.FilePath.Exists)
                {
                    File.WriteAllText(FISLibraryItem.FilePath.FullName, txtFISFile.Text);
                }

                MessageBox.Show("FIS file saved successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Writing FIS Rule File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
