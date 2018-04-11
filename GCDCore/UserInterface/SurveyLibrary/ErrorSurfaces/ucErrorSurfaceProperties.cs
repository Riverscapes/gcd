using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using GCDCore.ErrorCalculation.FIS;

namespace GCDCore.UserInterface.SurveyLibrary.ErrorSurfaces
{
    public partial class ucErrorSurfaceProperties : UserControl
    {
        public ErrorSurfaceProperty ErrSurfProperty { get; internal set; }
        public bool Editable { get; internal set; }

        private BindingList<AssocSurface> AssociatedSurfaces;

        public ucErrorSurfaceProperties()
        {
            InitializeComponent();
        }

        public void InitializeNew(string regionName, List<AssocSurface> assocs)
        {
            AssociatedSurfaces = new BindingList<AssocSurface>(assocs);
            ErrSurfProperty = new ErrorSurfaceProperty(regionName);
            Editable = true;
        }

        public void InitializeExisting(ErrorSurfaceProperty errProp, List<AssocSurface> assocs, bool editable = true)
        {
            AssociatedSurfaces = new BindingList<AssocSurface>(assocs);
            ErrSurfProperty = errProp;
            Editable = editable;
        }

        private void ucErrorSurfaceProperties_Load(object sender, EventArgs e)
        {
            // Needed by visual studio designer
            if (ProjectManager.Project == null)
                return;

            rdoUniform.Text = rdoUniform.Text.Replace("(", string.Format("({0}", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit)));
            grdFISInputs.AutoGenerateColumns = false;

            // required by Visual Studio designer
            if (ProjectManager.Project == null)
                return;


            // Load all the associated surfaces in the survey library to the grid combo box
            DataGridViewComboBoxColumn colCombo = (DataGridViewComboBoxColumn)grdFISInputs.Columns[1];
            colCombo.DataSource = new BindingList<AssocSurface>(AssociatedSurfaces.ToList());
            colCombo.DisplayMember = "Name";
            colCombo.ValueMember = "This"; // needed to support binding column to complex object

            cboAssociated.DataSource = AssociatedSurfaces;
            cboFIS.DataSource = ProjectManager.FISLibrary.FISItems;

            cboFIS.SelectedIndexChanged += cboFIS_SelectedIndexChanged;
            rdoAssociated.CheckedChanged += ErrorSurfaceTypeChanged;
            rdoFIS.CheckedChanged += ErrorSurfaceTypeChanged;

            if (ErrSurfProperty.UniformValue.HasValue)
            {
                rdoUniform.Checked = true;
                valUniform.Value = ErrSurfProperty.UniformValue.Value;
            }

            if (ErrSurfProperty.AssociatedSurface is AssocSurface)
            {
                rdoAssociated.Checked = true;
                cboAssociated.SelectedItem = ErrSurfProperty.AssociatedSurface;
            }

            if (ErrSurfProperty.FISRuleFile is System.IO.FileInfo)
            {
                rdoFIS.Checked = true;
                // Make sure that the correct FIS rule file is selected. Bit clumsy but works!
                for (int i = 0; i < cboFIS.Items.Count; i++)
                {
                    if (string.Compare(((FISLibraryItem)cboFIS.Items[i]).FilePath.FullName, ErrSurfProperty.FISRuleFile.FullName, true) == 0)
                    {
                        cboFIS.SelectedIndex = i;
                        break;
                    }
                }
            }

            // Finally call the update to put the controls in the correct state
            ErrorSurfaceTypeChanged(sender, e);
        }

        private void ErrorSurfaceTypeChanged(object sender, EventArgs e)
        {
            rdoUniform.Enabled = rdoUniform.Checked ? true : Editable;
            valUniform.Enabled = Editable && rdoUniform.Checked;

            rdoAssociated.Enabled = rdoAssociated.Checked ? true : Editable && AssociatedSurfaces.Count() > 0;
            cboAssociated.Enabled = Editable && rdoAssociated.Checked;
            if (!rdoAssociated.Checked)
                cboAssociated.SelectedIndex = -1;

            rdoFIS.Enabled = rdoFIS.Checked ? true : Editable && AssociatedSurfaces.Count() > 1;
            cboFIS.Enabled = rdoFIS.Checked && Editable;
            grdFISInputs.Enabled = cboFIS.Enabled;
            if (!rdoFIS.Checked)
                cboFIS.SelectedIndex = -1;
        }

        private void cboFIS_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (cboFIS.SelectedIndex < 0)
            {
                ErrSurfProperty.FISRuleFile = null;
                ErrSurfProperty.FISInputs.Clear();
                grdFISInputs.DataSource = null;
                return;
            }

            FISRuleFile selectedFIS = new FISRuleFile(((FISLibraryItem)cboFIS.SelectedItem).FilePath);

            // Detect if this is already the identified FIS
            if (!(ErrSurfProperty.FISRuleFile is System.IO.FileInfo && string.Compare(ErrSurfProperty.FISRuleFile.FullName, selectedFIS.RuleFilePath.FullName, true) == 0))
            {
                // Load the inputs for the newly selected FIS rule file into the error properties
                ErrSurfProperty.FISRuleFile = selectedFIS.RuleFilePath;
                ErrSurfProperty.FISInputs.Clear();
                foreach (string input in selectedFIS.FISInputs)
                    ErrSurfProperty.FISInputs.Add(new FISInput(input));
            }

            grdFISInputs.DataSource = ErrSurfProperty.FISInputs;
        }

        public bool ValidateForm()
        {
            if (rdoAssociated.Checked && cboAssociated.SelectedIndex < 0)
            {
                MessageBox.Show("You must pick an existing associated surface or select a different error surface type.", "Invalid Associated Surface", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (rdoFIS.Checked)
            {
                if (cboFIS.SelectedIndex < 0)
                {
                    MessageBox.Show("You must pick a FIS rule file from the FIS Library or select a different error surface type.", "Invalid FIS Specifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (ErrSurfProperty.FISInputs.Count<FISInput>(x => x.AssociatedSurface == null) > 0)
                {
                    MessageBox.Show("One or more FIS inputs have not been assigned to an associated surface.", "Unassigned FIS Inputs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grdFISInputs.Select();
                    return false;
                }
            }

            // Update the member error surface property now we know everything is OK
            // This is so that parent forms can use the updated properties after calling validate
            if (rdoUniform.Checked)
                ErrSurfProperty.UniformValue = valUniform.Value;
            else if (rdoAssociated.Checked)
            {
                ErrSurfProperty.AssociatedSurface = cboAssociated.SelectedItem as AssocSurface;
            }
            else
            {
                // The FIS file and inputs should already be updated
            }

            return true;
        }

        private void valUniform_Enter(object sender, EventArgs e)
        {
            valUniform.Select(0, valUniform.Text.Length);
        }
    }
}