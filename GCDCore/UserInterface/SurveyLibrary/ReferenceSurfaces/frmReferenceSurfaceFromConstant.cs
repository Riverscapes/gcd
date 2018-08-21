using System;
using System.IO;
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
    public partial class frmReferenceSurfaceFromConstant : Form, IProjectItemForm
    {
        public naru.ui.SortableBindingList<DEMSurvey> DEMSurveys;
        public Surface ReferenceSurface { get; internal set; }

        public GCDProjectItem GCDProjectItem { get { return ReferenceSurface; } }

        public frmReferenceSurfaceFromConstant()
        {
            InitializeComponent();
        }

        private void frmReferenceSurfaceFromConstant_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.CreateButtonText;

            // Add all the project DEM surveys to the list and then bind to checked listbox
            DEMSurveys = new naru.ui.SortableBindingList<DEMSurvey>(ProjectManager.Project.DEMSurveys);
            cboDEMSurvey.DataSource = DEMSurveys;
            cboDEMSurvey.SelectedIndex = 0;

            string sUnits = UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit);
            lblSingle.Text = lblSingle.Text.Replace(")", sUnits + ")");
            lblLower.Text = lblLower.Text.Replace(")", sUnits + ")");
            lblUpper.Text = lblUpper.Text.Replace(")", sUnits + ")");
            lblIncrement.Text = lblIncrement.Text.Replace(")", sUnits + ")");
            lblError.Text = lblError.Text.Replace(")", sUnits + ")");

            UpdateControls(null, null);

            tTip.SetToolTip(txtName, "The name used to identify this reference surface within this GCD project. It cannot be empty and it must be unique among all reference surfaces within this project.");
            tTip.SetToolTip(txtPath, "The relative file path within this GCD project where the reference surface(s) will be produced.");
            tTip.SetToolTip(cboDEMSurvey, "The DEM survey used to provide the spatial reference and extent of the output reference surface(s).");
            tTip.SetToolTip(rdoSingle, "A single reference surface with a unfirom, constant elevation.");
            tTip.SetToolTip(valSingle, "The floating point value that will be stored in the single reference surface raster that will be produced.");
            tTip.SetToolTip(rdoMultiple, "Multiple reference surfaces will be generated.");
            tTip.SetToolTip(valUpper, "The maximum elevation value used to generate a refereance surface raster. A raster with this value will only be generated if the difference between this value and the lower elevation is evenly divisble by the specified increment.");
            tTip.SetToolTip(valLower, "The minimum elevation value used to generate a reference surface raster. A value with this value will always be generated.");
            tTip.SetToolTip(valIncrement, "The elevation increment that is added to the specified lower elevation to produce the set of elevations at which reference surfaces will be generated.");
            tTip.SetToolTip(valError, "The single, uniform error value used to generate an error raster for each reference surface produced.");
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

            // Ensure that output path is updated to reflect the new mode
            txtName_TextChanged(sender, e);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (rdoSingle.Checked)
            {
                txtPath.Font = new Font(txtPath.Font, FontStyle.Regular);

                if (string.IsNullOrEmpty(txtName.Text))
                    txtPath.Text = string.Empty;
                else
                    txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.ReferenceSurfacePath(txtName.Text));
            }
            else
            {
                txtPath.Text = "Multiple rasters produced";
                txtPath.Font = new Font(txtPath.Font, FontStyle.Italic);
            }
        }

        private DialogResult ValidateForm()
        {
            // Sanity check to avoid empty names
            txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the reference surface.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return DialogResult.None;
            }

            if (rdoSingle.Checked)
            {
                if (!GCDCore.Project.ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, null))
                {
                    MessageBox.Show("The GCD project already contains a reference surface with this name. Please choose a unique name for the reference surface.", "Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Select();
                    return DialogResult.None;
                }
            }

            if (rdoMultiple.Checked)
            {
                if (valUpper.Value <= valLower.Value)
                {
                    MessageBox.Show("The upper elevation must be greater than the lower elevation.", "Invalid Elevations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return DialogResult.None;
                }

                long count = Convert.ToInt64((valUpper.Value - valLower.Value) / valIncrement.Value);
                if (count > 20)
                {
                    switch (MessageBox.Show(string.Format("This process is about to generate a large number ({0:n0}) of rasters in this GCD project. Are you sure you want to proceed with this operation?", count),
                         Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        case DialogResult.No:
                            valUpper.Select();
                            return DialogResult.None;

                        case DialogResult.Cancel:
                            return DialogResult.Cancel;
                    }
                }
            }

            return DialogResult.OK;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = ValidateForm();
            if (DialogResult != DialogResult.OK)
            {
                return;
            }


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
                for (decimal aVal = valLower.Value; aVal <= valUpper.Value; aVal += valIncrement.Value)
                {
                    Values.Add((float)aVal);
                }
                successMsg = string.Format("{0} reference surfaces generated successfully.", Values.Count);
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (float value in Values)
                {
                    string name = GetUniqueName(value);

                    // Get a unique name and ensure directory exists
                    FileInfo fiOutput = ProjectManager.Project.ReferenceSurfacePath(name);
                    fiOutput.Directory.Create();

                    // Generate reference surface
                    GCDConsoleLib.Raster rOut = GCDConsoleLib.RasterOperators.Uniform<float>(template, fiOutput, value, ProjectManager.OnProgressChange);
                    ReferenceSurface = new Surface(name, rOut, null);
                    ProjectManager.Project.ReferenceSurfaces.Add(ReferenceSurface);

                    // Error surface
                    string errName = string.Format("Uniform Error at {0:0.000}", valError.Value);
                    FileInfo fiError = Surface.ErrorSurfaceRasterPath(fiOutput.Directory, errName);
                    fiError.Directory.Create();

                    GCDConsoleLib.RasterOperators.Uniform<float>(template, fiError, (float)valError.Value, ProjectManager.OnProgressChange);
                    ErrorSurface errSurf = new ErrorSurface(errName, fiError, ReferenceSurface);
                    ReferenceSurface.ErrorSurfaces.Add(errSurf);
                }

                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
                MessageBox.Show(successMsg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error generating reference surface.");
                this.DialogResult = DialogResult.None;
            }
        }

        private string GetUniqueName(float value)
        {
            string name = txtName.Text;
            int index = 0;
            do
            {
                if (rdoMultiple.Checked)
                {
                    name = string.Format("{0}_{1}", txtName.Text, value.ToString().Replace(".", "").Replace(",", ""));
                }

                if (index > 0)
                {
                    name += string.Format("({0})", index);
                }

                index++;

            } while (!GCDCore.Project.ProjectManager.Project.IsReferenceSurfaceNameUnique(name, null));

            return name;
        }

        private void numericUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
