using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib;

namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    public partial class frmReferenceSurfaceFromDEMs : Form, IProjectItemForm
    {
        public naru.ui.SortableBindingList<DEMItem> DEMSurveys;
        public GCDCore.Project.Surface ReferenceSurface { get; internal set; }

        public GCDProjectItem GCDProjectItem { get { return ReferenceSurface; } }

        public frmReferenceSurfaceFromDEMs()
        {
            InitializeComponent();
        }

        private void frmReferenceSurfaceFromDEMs_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.CreateButtonText;

            // Add all the project DEM surveys to the list and then bind to checked listbox
            List<DEMItem> items = new List<DEMItem>(ProjectManager.Project.DEMSurveys.Values.Select(x => new DEMItem(x)));
            DEMSurveys = new naru.ui.SortableBindingList<DEMItem>(items);
            grdData.DataSource = DEMSurveys;

            //Setup error surfaces for DEM grid
            for (int i = 0; i < grdData.Rows.Count; i++)
            {
                DataGridViewComboBoxCell comboCell = grdData.Rows[i].Cells["colError"] as DataGridViewComboBoxCell;

                DEMSurvey dem = ((DEMItem)grdData.Rows[i].DataBoundItem)._DEM;

                comboCell.DataSource = new BindingSource(dem.ErrorSurfaces, null);
                comboCell.DisplayMember = "NameWithDefault";

                //select any error surfaces have default flat
                if (dem.ErrorSurfaces.Any(x => x.IsDefault))
                {
                    comboCell.Value = dem.ErrorSurfaces.First(x => x.IsDefault).NameWithDefault;
                }
                else
                {
                    // No default error surface. Simply select the first.
                    if (comboCell.Items.Count == 1)
                    {
                        comboCell.Value = dem.ErrorSurfaces.First().NameWithDefault;
                    }
                }
            }

            cboMethod.Items.Add(new naru.db.NamedObject((long)RasterOperators.MultiMathOpType.Maximum, "Maximum"));
            cboMethod.Items.Add(new naru.db.NamedObject((long)RasterOperators.MultiMathOpType.Mean, "Mean"));
            cboMethod.Items.Add(new naru.db.NamedObject((long)RasterOperators.MultiMathOpType.Minimum, "Minimum"));
            cboMethod.Items.Add(new naru.db.NamedObject((long)RasterOperators.MultiMathOpType.StandardDeviation, "Standard Deviation"));
            cboMethod.SelectedIndex = 0;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            RasterOperators.MultiMathOpType eMethod = (RasterOperators.MultiMathOpType)((naru.db.NamedObject)cboMethod.SelectedItem).ID;

            List<Tuple<DEMSurvey, ErrorSurface>> rInputs = new List<Tuple<DEMSurvey, ErrorSurface>>();
            for (int i = 0; i < grdData.Rows.Count; i++)
            {
                DEMItem dem = grdData.Rows[i].DataBoundItem as DEMItem;
                if (!dem.Include)
                    continue;

                DataGridViewComboBoxCell comboCell = grdData.Rows[i].Cells["colError"] as DataGridViewComboBoxCell;
                BindingSource bs = comboCell.DataSource as BindingSource;
                ErrorSurface err = bs.Current as ErrorSurface;

                rInputs.Add(new Tuple<DEMSurvey, ErrorSurface>(dem._DEM, err));
            }

            Engines.ReferenceSurfaceEngine eng = new Engines.ReferenceSurfaceEngine(txtName.Text, rInputs, eMethod);

            System.IO.FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
            System.IO.FileInfo fiError = Surface.ErrorSurfaceRasterPath(fiOutput.Directory, txtName.Text);

            try
            {
                Cursor = Cursors.WaitCursor;

                Surface surf = eng.Run(fiOutput, fiError);
                ProjectManager.AddNewProjectItemToMap(surf);
                Cursor = Cursors.Default;
                MessageBox.Show("Reference surface generated successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error generating reference surface from DEM surveys.");
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            // Sanity check to avoid empty names
            txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the reference surface.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            // Validate that every DEM has an error surface selected
            foreach (DataGridViewRow dgvr in grdData.Rows)
            {
                DEMItem dem = dgvr.DataBoundItem as DEMItem;
                if (dem.Include)
                {
                    DataGridViewComboBoxCell comboCell = dgvr.Cells["colError"] as DataGridViewComboBoxCell;
                    if (comboCell.Value == null)
                    {
                        grdData.Select();
                        comboCell.Selected = true;
                        MessageBox.Show("You must select an error surface for all DEM Surveys that are to be included in the operation.", "Missing Error Surface", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            if (!GCDCore.Project.ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, null))
            {
                MessageBox.Show("The GCD project already contains a reference surface with this name. Please choose a unique name for the reference surface.", "Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (DEMSurveys.Count(x => x.Include) < 2)
            {
                MessageBox.Show("You must select at least two DEM surveys to generate a reference surface.", "Insufficient DEM Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdData.Select();
                return false;
            }

            return true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.ReferenceSurfacePath(txtName.Text));
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DEMSurveys.ToList<DEMItem>().ForEach(x => x.Include = ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all"));
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
    }
}
