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

namespace GCDCore.UserInterface.SurveyLibrary.ReferenceSurfaces
{
    public partial class frmReferenceSurfaceFromConstant : Form
    {
        public naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey> DEMSurveys;

        public frmReferenceSurfaceFromConstant()
        {
            InitializeComponent();
        }

        private void frmReferenceSurfaceFromConstant_Load(object sender, EventArgs e)
        {
            // Add all the project DEM surveys to the list and then bind to checked listbox
            DEMSurveys = new naru.ui.SortableBindingList<GCDCore.Project.DEMSurvey>(GCDCore.Project.ProjectManager.Project.DEMSurveys.Values.ToList<GCDCore.Project.DEMSurvey>());
            cboDEMSurvey.DataSource = DEMSurveys;
            cboDEMSurvey.SelectedIndex = 0;

            string sUnits = UnitsNet.Length.GetAbbreviation(GCDCore.Project.ProjectManager.Project.Units.VertUnit);
            lblSingle.Text = lblSingle.Text.Replace(")", sUnits + ")");
            lblLower.Text = lblLower.Text.Replace(")", sUnits + ")");
            lblUpper.Text = lblUpper.Text.Replace(")", sUnits + ")");
            lblIncrement.Text = lblIncrement.Text.Replace(")", sUnits + ")");

            UpdateControls(null, null);
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            lblSingle.Enabled = rdoSingle.Checked;
            valSingle.Enabled = rdoSingle.Checked;

            lblUpper.Enabled = rdoMultiple.Checked;
            valUpper.Enabled = rdoMultiple.Checked;
            lblLower.Enabled = rdoMultiple.Checked;
            valLower.Enabled = rdoMultiple.Checked;
            lblIncrement.Enabled = rdoMultiple.Checked;
            valIncrement.Enabled = rdoMultiple.Checked;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

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

            if (rdoSingle.Checked)
            {
                if (!GCDCore.Project.ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, null))
                {
                    MessageBox.Show("The GCD project already contains a reference surface with this name. Please choose a unique name for the reference surface.", "Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Select();
                    return false;
                }
            }

            if (rdoMultiple.Checked)
            {
                if (valUpper.Value <= valLower.Value)
                {
                    MessageBox.Show("The upper elevation must be greater than the lower elevation.", "Invalid Elevations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            return true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            System.IO.FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
            fiOutput.Directory.Create();

            GCDConsoleLib.Raster template = ((DEMSurvey)cboDEMSurvey.SelectedItem).Raster;

            List<float> Values = new List<float>();
            string successMsg = string.Empty;
            if (rdoSingle.Checked)
            {
                Values.Add((float)valSingle.Value);
                successMsg = "Reference surface generated successfully.";
            }
            else
            {
                successMsg = string.Format("{0} reference surfaces generated successfully.", Values.Count);

                for (decimal aVal = valLower.Value; aVal <= valUpper.Value; aVal += valIncrement.Value)
                {
                    Values.Add((float)aVal);
                }
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (float value in Values)
                {
                    string name = GetUniqueName(value);
                    GCDConsoleLib.Raster rOut = GCDConsoleLib.RasterOperators.Uniform<float>(template, fiOutput, value);
                    Surface surf = new Surface(name, rOut);
                    ProjectManager.Project.ReferenceSurfaces[surf.Name] = surf;
                }

                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
                MessageBox.Show(successMsg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error generating reference surface.");
                this.DialogResult = DialogResult.None;
            }
        }

        private string GetUniqueName(float value)
        {
            string name = string.Empty;
            int index = 0;
            do
            {
                name = string.Format("{0}_{1}", txtName.Text, value.ToString().Replace(".", "").Replace(",", ""));

                if (index > 0)
                {
                    name += string.Format("({0})", index);
                }

                index++;

            } while (!GCDCore.Project.ProjectManager.Project.IsReferenceSurfaceNameUnique(name, null));

            return Name;
        }

    }
}
