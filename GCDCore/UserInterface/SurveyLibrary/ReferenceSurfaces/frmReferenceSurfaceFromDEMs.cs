using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    public partial class frmReferenceSurfaceFromDEMs : Form
    {
        public naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey> DEMSurveys;

        public frmReferenceSurfaceFromDEMs()
        {
            InitializeComponent();
        }

        private void frmReferenceSurfaceFromDEMs_Load(object sender, EventArgs e)
        {
            // Add all the project DEM surveys to the list and then bind to checked listbox
            DEMSurveys = new naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey>(GCDCore.Project.ProjectManager.Project.DEMSurveys.Values.ToList<GCDCore.Project.DEMSurvey>());
            lstDEMs.DataSource = DEMSurveys;

            foreach (GCDConsoleLib.RasterOperators.MultiMathOpType val in Enum.GetValues(typeof(GCDConsoleLib.RasterOperators.MultiMathOpType)))
            {
                if (val.ToString().ToLower().Contains("standarddeviation"))
                    cboMethod.Items.Add(new naru.db.NamedObject((long)val, "Standard Deviation"));
                else
                    cboMethod.Items.Add(new naru.db.NamedObject((long)val, val.ToString()));
            }
            cboMethod.SelectedIndex = 0;

            // ensure all DEMs are checked by default
            for (int i = 0; i < lstDEMs.Items.Count; i++)
                lstDEMs.SetItemChecked(i, true);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            System.IO.FileInfo fiOutput = GCDCore.Project.ProjectManager.Project.GetAbsolutePath(txtPath.Text); 

            List<GCDConsoleLib.Raster> rInputs = new List<GCDConsoleLib.Raster>();
            foreach(GCDCore.Project.DEMSurvey dem in lstDEMs.CheckedItems)
            {
                rInputs.Add(dem.Raster);
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                switch (((GCDConsoleLib.RasterOperators.MultiMathOpType)((naru.db.NamedObject)cboMethod.SelectedItem).ID))
                {
                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Addition:
                        GCDConsoleLib.RasterOperators.MultiAdd(rInputs, fiOutput);
                        break;

                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Maximum:
                        GCDConsoleLib.RasterOperators.Maximum(rInputs, fiOutput);
                        break;

                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Mean:
                        GCDConsoleLib.RasterOperators.Mean(rInputs, fiOutput);
                        break;

                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Minimum:
                        GCDConsoleLib.RasterOperators.Minimum(rInputs, fiOutput);
                        break;

                    case GCDConsoleLib.RasterOperators.MultiMathOpType.StandardDeviation:
                        GCDConsoleLib.RasterOperators.StandardDeviation(rInputs, fiOutput);
                        break;

                    default:
                        throw new Exception("Unhandled math operation type " + cboMethod.Text);
                }

                Cursor = Cursors.Default;
                MessageBox.Show("Reference surface generated successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error generating reference surface from DEM surveys.");
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

            if (!GCDCore.Project.ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, null))
            {
                MessageBox.Show("The GCD project already contains a reference surface with this name. Please choose a unique name for the reference surface.", "Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (lstDEMs.CheckedItems.Count < 2)
            {
                MessageBox.Show("You must select at least two DEM surveys to generate a reference surface.", "Insufficient DEM Surveys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstDEMs.Select();
                return false;
            }

            return true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {


        }
    }
}
