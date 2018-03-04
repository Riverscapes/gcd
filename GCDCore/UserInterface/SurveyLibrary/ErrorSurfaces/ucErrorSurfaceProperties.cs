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

        private BindingList<AssocSurface> AssociatedSurfaces;

        public ucErrorSurfaceProperties()
        {
            InitializeComponent();
        }

        public void InitializeNew(string regionName, List<AssocSurface> assocs)
        {
            AssociatedSurfaces = new BindingList<AssocSurface>(assocs);
            ErrSurfProperty = new ErrorSurfaceProperty(regionName);
        }

        public void InitializeExisting(ErrorSurfaceProperty errProp, List<AssocSurface> assocs)
        {
            AssociatedSurfaces = new BindingList<AssocSurface>(assocs);
            ErrSurfProperty = errProp;
        }

        private void ucErrorSurfaceProperties_Load(object sender, EventArgs e)
        {
            grdFISInputs.AutoGenerateColumns = false;

            // required by Visual Studio designer
            if (ProjectManager.Project == null)
                return;

            cboAssociated.DataSource = AssociatedSurfaces;
            cboFIS.DataSource = ProjectManager.FISLibrary;

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
            else
            {
                // Assoc requires at least one DEM associated surface
                rdoAssociated.Enabled = AssociatedSurfaces.Count() > 0;
            }

            if (ErrSurfProperty.FISRuleFile is System.IO.FileInfo)
            {
                rdoFIS.Checked = true;
                cboFIS.SelectedItem = ErrSurfProperty.FISRuleFile;
            }
            else
            {
                // FIS requires 2 or more DEM associated surfaces
                rdoFIS.Enabled = AssociatedSurfaces.Count() > 1;
            }

            // Finally call the update to put the controls in the correct state
            ErrorSurfaceTypeChanged(sender, e);
        }

        private void ErrorSurfaceTypeChanged(object sender, EventArgs e)
        {
            valUniform.Enabled = rdoUniform.Checked;
            cboAssociated.Enabled = rdoAssociated.Checked;
            cboFIS.Enabled = rdoFIS.Checked;
            grdFISInputs.Enabled = rdoFIS.Checked;

            if (!rdoAssociated.Checked)
                cboAssociated.SelectedIndex = -1;

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
            if (ErrSurfProperty.FISRuleFile is System.IO.FileInfo && string.Compare(ErrSurfProperty.FISRuleFile.FullName, selectedFIS.RuleFilePath.FullName, true) == 0)
                return;

            // Load the inputs for the newly selected FIS rule file into the error properties
            ErrSurfProperty.FISRuleFile = selectedFIS.RuleFilePath;
            ErrSurfProperty.FISInputs.Clear();
            foreach (string input in selectedFIS.FISInputs)
                ErrSurfProperty.FISInputs.Add(new FISInput(input));

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

            return true;
        }

        private void valUniform_Enter(object sender, EventArgs e)
        {
            valUniform.Select(0, valUniform.Text.Length);
        }
    }
}