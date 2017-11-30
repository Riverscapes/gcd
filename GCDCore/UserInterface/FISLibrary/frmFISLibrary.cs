using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using GCDCore.ErrorCalculation.FIS;

namespace GCDCore.UserInterface.FISLibrary
{
    public partial class frmFISLibrary
    {
        private naru.ui.SortableBindingList<GCDCore.ErrorCalculation.FIS.FISLibraryItem> FISList;

        public frmFISLibrary()
        {
            InitializeComponent();
        }

        private void btnAddFIS_Click(System.Object sender, System.EventArgs e)
        {
            frmAddFIS AddFISForm = new frmAddFIS();
            if (AddFISForm.ShowDialog() == DialogResult.OK)
            {
                FISList.Add(AddFISForm.FISItem);
                GCDCore.Project.ProjectManager.FISLibrary = FISList.ToList<FISLibraryItem>();
                grdFIS.Rows[grdFIS.Rows.Count - 1].Selected = true;
            }
        }

        private void btnDeleteFIS_Click(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
            if (MessageBox.Show("Are you sure you want to remove the selected FIS file from the GCD Software? Note that this will not delete the associated *.fis file.",
            Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            }
        }

        private void FISLibraryForm_Load(System.Object sender, System.EventArgs e)
        {
            ttpTooltip.SetToolTip(btnAddFIS, "Add a FIS file to the GCD FIS Library.");
            ttpTooltip.SetToolTip(btnEditFIS, "Edit the selected FIS file.");
            ttpTooltip.SetToolTip(btnDeleteFIS, "Delete the selected FIS file.");

            try
            {
                FISList = new naru.ui.SortableBindingList<FISLibraryItem>(GCDCore.Project.ProjectManager.FISLibrary);
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
            btnDeleteFIS.Enabled = grdFIS.SelectedRows.Count > 0;
            btnEditFIS.Enabled = grdFIS.SelectedRows.Count > 0;
        }

        private void btnEditFIS_Click(System.Object sender, System.EventArgs e)
        {
            FISLibraryItem item = (FISLibraryItem)grdFIS.SelectedRows[0].DataBoundItem;
            frmEditFIS frm = new frmEditFIS(item.FilePath);
            frm.ShowDialog();
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
        }

        private void btnFISRepo_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.FISRepositoryWebsite);
        }

        private void grdFIS_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FISLibraryItem item = (FISLibraryItem)grdFIS.SelectedRows[0].DataBoundItem;
            frmEditFIS frm = new frmEditFIS(item.FilePath);
            frm.ShowDialog();
        }
    }
}
