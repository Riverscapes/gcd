using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using GCDCore.ErrorCalculation.FIS;
using GCDCore.Project;

namespace GCDCore.UserInterface.FISLibrary
{
    public partial class frmFISLibrary
    {
        private naru.ui.SortableBindingList<FISLibraryItem> FISList;

        public frmFISLibrary()
        {
            InitializeComponent();
        }

        private void btnAddFIS_Click(System.Object sender, System.EventArgs e)
        {
            OpenFileDialog frm = new OpenFileDialog();
            frm.Title = "Select a FIS File";
            frm.Filter = "GCD FIS Files (*.fis) | *.fis";

            if (ProjectManager.FISLibrary.FISItems.Count > 0)
            {
                FileInfo fisFile = ProjectManager.FISLibrary.FISItems.Last().FilePath;
                if (fisFile.Directory.Exists)
                {
                    frm.InitialDirectory = fisFile.DirectoryName;
                }
            }

            if (frm.ShowDialog() == DialogResult.OK)
            {
                frmFISProperties frmFIS = new frmFISProperties(frm.FileName);
                if (frmFIS.ShowDialog() == DialogResult.OK)
                {
                    ProjectManager.FISLibrary.FISItems.ResetBindings();
                    foreach (DataGridViewRow dgvr in grdFIS.Rows)
                    {
                        if (((FISLibraryItem)dgvr.DataBoundItem == frmFIS.FISLibraryItem))
                        {
                            dgvr.Selected = true;
                            grdFIS.FirstDisplayedScrollingRowIndex = dgvr.Index;
                            break;
                        }
                    }
                }
            }
        }

        private void btnDeleteFIS_Click(System.Object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected FIS file from the GCD Software? Note that this will not delete the associated *.fis file.",
            Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FISLibraryItem item = (FISLibraryItem)grdFIS.SelectedRows[0].DataBoundItem;
                ProjectManager.FISLibrary.FISItems.Remove(item);
                ProjectManager.FISLibrary.Save();
            }
        }

        private void FISLibraryForm_Load(System.Object sender, System.EventArgs e)
        {
            ttpTooltip.SetToolTip(btnAddFIS, "Add a FIS file to the GCD FIS Library.");
            ttpTooltip.SetToolTip(btnEditFIS, "Edit the selected FIS file.");
            ttpTooltip.SetToolTip(btnDeleteFIS, "Delete the selected FIS file.");

            grdFIS.AutoGenerateColumns = false;

            try
            {
                FISList = ProjectManager.FISLibrary.FISItems;
                grdFIS.DataSource = FISList;
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            UpdateControls(sender, e);
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            bool bSystemFISSelected = false;

            if (grdFIS.SelectedRows.Count > 0)
            {
                bSystemFISSelected = ((FISLibraryItem)grdFIS.SelectedRows[0].DataBoundItem).FISType == ErrorCalculation.FIS.FISLibrary.FISLibraryItemTypes.System;
            }

            btnDeleteFIS.Enabled = !bSystemFISSelected && grdFIS.SelectedRows.Count > 0;
            btnEditFIS.Enabled = grdFIS.SelectedRows.Count > 0;
        }

        private void btnEditFIS_Click(System.Object sender, System.EventArgs e)
        {
            FISLibraryItem item = (FISLibraryItem)grdFIS.SelectedRows[0].DataBoundItem;
            frmFISProperties frm = new frmFISProperties(item);
            if (frm.ShowDialog() == DialogResult.OK)
                ProjectManager.FISLibrary.FISItems.ResetBindings();
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
        }

        private void btnFISRepo_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.FISRepositoryWebsite);
        }

        private void grdFIS_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditFIS_Click(sender, e);
        }
    }
}
