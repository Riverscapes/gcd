using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection.Intercomparison
{
    public partial class frmInterComparisonProperties : Form, IProjectItemForm
    {
        BindingList<GCDCore.Project.DoDBase> DoDs;

        public InterComparison InterComparison { get; internal set; }
        public GCDProjectItem GCDProjectItem { get { return InterComparison as GCDProjectItem; } }

        public frmInterComparisonProperties()
        {
            InitializeComponent();

            DoDs = new BindingList<DoDBase>(ProjectManager.Project.DoDs);
        }

        private void frmInterComparisonProperties_Load(object sender, EventArgs e)
        {
            lstDoDs.DataSource = DoDs;

            tTip.SetToolTip(txtName, "The name for this inter-comparison. The name cannot be empty and it must be unique among all inter-comparisons within the current GCD project.");
            tTip.SetToolTip(txtPath, "The relative output path where the inter-comparison will get created.");
            tTip.SetToolTip(lstDoDs, "Select which change detections should be included. Right click to quickly select all or none of the listed items.");
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.InterComparisonPath(txtName.Text));
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

            if (!ProjectManager.Project.IsInterComparisonNameUnique(txtName.Text, null))
            {
                MessageBox.Show("A change detection inter-comparison already exists with this name. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                List<Tuple<string, GCDConsoleLib.GCD.DoDStats>> dodStats = new List<Tuple<string, GCDConsoleLib.GCD.DoDStats>>();
                List<DoDBase> dods = new List<DoDBase>();
                foreach (DoDBase dod in lstDoDs.CheckedItems)
                {
                    dodStats.Add(new Tuple<string, GCDConsoleLib.GCD.DoDStats>(dod.Name, dod.Statistics));
                    dods.Add(dod);
                }

                System.IO.FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                if (!fiOutput.Directory.Exists)
                    fiOutput.Directory.Create();

                Engines.InterComparison.Generate(dodStats, fiOutput);

                InterComparison = new InterComparison(txtName.Text, fiOutput, dods);
                ProjectManager.Project.InterComparisons.Add(InterComparison);
                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
                MessageBox.Show("Change detection inter-comparison generated successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error performing change detection inter-comparison.");
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
                GCDException.HandleException(ex);
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        private void cmdMove(object sender, EventArgs e)
        {
            int original = lstDoDs.SelectedIndex;
            int moved = original + (string.Compare(((Control)sender).Name, "cmdMoveUp", true) == 0 ? -1 : 1);
            DoDBase dod = DoDs[original];
            DoDs.Remove(dod);
            DoDs.Insert(moved, dod);
            lstDoDs.SelectedIndex = moved;
        }

        private void lstDoDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdMoveUp.Enabled = lstDoDs.SelectedIndex > 0;
            cmdMoveDown.Enabled = lstDoDs.SelectedIndex < DoDs.Count - 1;
        }
    }
}
