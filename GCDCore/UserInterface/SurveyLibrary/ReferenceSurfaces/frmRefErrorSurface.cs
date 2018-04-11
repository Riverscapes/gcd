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
    public partial class frmRefErrorSurface : Form, IProjectItemForm
    {
        public readonly Surface ReferenceSurface;
        public ErrorSurface ErrorSurface { get; internal set; }

        public GCDProjectItem GCDProjectItem { get { return ErrorSurface; } }

        public frmRefErrorSurface(Surface parent)
        {
            InitializeComponent();
            ReferenceSurface = parent;
        }

        private void frmRefErrorSurface_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.CreateButtonText;

            string sUnits = UnitsNet.Length.GetAbbreviation(GCDCore.Project.ProjectManager.Project.Units.VertUnit);
            lblSingle.Text = lblSingle.Text.Replace(")", sUnits + ")");
            lblLower.Text = lblLower.Text.Replace(")", sUnits + ")");
            lblUpper.Text = lblUpper.Text.Replace(")", sUnits + ")");
            lblIncrement.Text = lblIncrement.Text.Replace(")", sUnits + ")");

            UpdateControls(sender, e);
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            valSingle.Enabled = rdoSingle.Checked;
            lblSingle.Enabled = rdoSingle.Checked;

            valLower.Enabled = rdoMultiple.Checked;
            lblLower.Enabled = rdoMultiple.Checked;
            valUpper.Enabled = rdoMultiple.Checked;
            lblUpper.Enabled = rdoMultiple.Checked;
            valIncrement.Enabled = rdoMultiple.Checked;
            lblIncrement.Enabled = rdoMultiple.Checked;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (rdoSingle.Checked)
            {
                txtPath.Font = new Font(txtPath.Font, FontStyle.Regular);

                if (string.IsNullOrEmpty(txtName.Text))
                    txtPath.Text = string.Empty;
                else
                    txtPath.Text = ProjectManager.Project.GetRelativePath(ReferenceSurface.ErrorSurfacePath(txtName.Text));
            }
            else
            {
                txtPath.Text = "Multiple rasters produced";
                txtPath.Font = new Font(txtPath.Font, FontStyle.Italic);
            }
        }

        private void numericUpDown_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult eResult = ValidateForm();
            if (eResult != DialogResult.OK)
            {
                DialogResult= eResult;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string successMsg = string.Empty;
            List<float> errVals = new List<float>();
            if (rdoSingle.Checked)
            {
                successMsg = "Reference Error Surface generated successfully.";
                errVals.Add((float)valSingle.Value);
            }
            else
            {
                for (decimal errVal = valLower.Value; errVal <= valUpper.Value; errVal += valIncrement.Value)
                    errVals.Add((float)errVal);

                successMsg = string.Format("{0} reference error surfaces generated successfully.", errVals.Count);
            }

            try
            {
                foreach (float errVal in errVals)
                {
                    string name = GetUniqueName(errVal);
                    System.IO.FileInfo fiOutput = ReferenceSurface.ErrorSurfacePath(name);
                    fiOutput.Directory.Create();

                    GCDConsoleLib.RasterOperators.Uniform<float>(ReferenceSurface.Raster, fiOutput, errVal);
                    ErrorSurface = new ErrorSurface(name, fiOutput, ReferenceSurface);
                    ReferenceSurface.ErrorSurfaces.Add(ErrorSurface);
                }

                ProjectManager.Project.Save();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(successMsg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                DialogResult = DialogResult.None;
                GCDException.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private DialogResult ValidateForm()
        {
            // Sanity check to avoid empty names
            txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the reference error surface.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return DialogResult.None;
            }

            if (rdoSingle.Checked)
            {
                if (!ReferenceSurface.IsErrorNameUnique(txtName.Text, null))
                {
                    MessageBox.Show("The parent reference surface already contains an error surface with this name. Please choose a unique name.", "Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Select();
                    return DialogResult.None;
                }
            }

            if (rdoMultiple.Checked)
            {
                if (valUpper.Value <= valLower.Value)
                {
                    MessageBox.Show("The upper error value must be greater than the lower value.", "Invalid Error Range", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            } while (!ReferenceSurface.IsErrorNameUnique(name, null));

            return name;
        }
    }
}
