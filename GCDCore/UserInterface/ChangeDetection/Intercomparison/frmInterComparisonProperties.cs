using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection.Intercomparison
{
    public partial class frmInterComparisonProperties : Form
    {
        BindingList<GCDCore.Project.DoDBase> DoDs;

        public frmInterComparisonProperties()
        {
            InitializeComponent();

            DoDs = new BindingList<DoDBase>(ProjectManager.Project.DoDs.Values.ToList<DoDBase>());
        }

        private void frmInterComparisonProperties_Load(object sender, EventArgs e)
        {
            lstDoDs.DataSource = DoDs;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetInterComparisonPath(txtName.Text));
        }

        private bool ValidateForm()
        {
            // Sanity check to avoid empty names
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the change detection inter-comparison.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (lstDoDs.CheckedItems.Count < 2)
            {
                MessageBox.Show("You must select at least two change detections to perform an inter-comparison.", "Insufficient Change Detections", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstDoDs.Select();
                return false;
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                //validate form
                if (!ValidateForm())
                {
                    this.DialogResult = DialogResult.None;
                    return;
                }

                Cursor = Cursors.WaitCursor;

                Dictionary<string, GCDConsoleLib.GCD.DoDStats> dodStats = new Dictionary<string, GCDConsoleLib.GCD.DoDStats>();
                List<DoDBase> dods = new List<DoDBase>();
                foreach (DoDBase dod in lstDoDs.CheckedItems)
                {
                    dodStats[dod.Name] = dod.Statistics;
                    dods.Add(dod);
                }

                System.IO.FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                if (!fiOutput.Directory.Exists)
                    fiOutput.Directory.Create();

                Engines.InterComparison.Generate(dodStats, fiOutput);

                InterComparison inter = new InterComparison(txtName.Text, fiOutput, dods);
                ProjectManager.Project.InterComparisons[inter.Name] = inter;
                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
                MessageBox.Show("Change detection inter-comparison generated successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error performing change detection inter-comparison.");
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < lstDoDs.Items.Count; i++)
                    lstDoDs.SetItemChecked(i, ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}
