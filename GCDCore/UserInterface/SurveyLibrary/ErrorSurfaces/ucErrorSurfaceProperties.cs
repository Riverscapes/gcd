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
        /// <summary>
        /// This list contains the FIS available for existing or new FIS error properties.
        /// The list is pre-populated with all the system and user FIS items. 
        /// However, when viewing the properties of an existing FIS, it might not be
        /// present in the FIS library, in which case a temporary FISLibraryItem is created
        /// for this FIS rule file, selected in the combo and locked so the user can't edit it.
        /// </summary>
        private readonly BindingList<FISLibraryItem> FISList;
        
        public ErrorSurfaceProperty ErrSurfProperty { get; internal set; }
        public bool Editable { get; internal set; }

        /// <summary>
        /// The selected FIS library item or NULL if none selected.
        /// </summary>
        public FISLibraryItem SelectedFIS { get { return rdoFIS.Checked ? cboFIS.SelectedItem as FISLibraryItem : null; } }

        private BindingList<AssocSurface> AssociatedSurfaces;

        public ucErrorSurfaceProperties()
        {
            InitializeComponent();

            // List of FIS that the user can choose from
            FISList = new BindingList<FISLibraryItem>();
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
            colCombo.DataSource = AssociatedSurfaces;
            colCombo.DisplayMember = "Name";
            colCombo.ValueMember = "This"; // needed to support binding column to complex object

            // Add all the FIS library items to the FIS combo box. Don't bind the combo
            // directly to the FIS library because we might need to add a temporary, project FIS
            // item to the combo if an error surface uses an FIS that is not in the library
            FISList.Clear();
            ProjectManager.FISLibrary.FISItems.ToList().ForEach(x => FISList.Add(x));
            cboFIS.DataSource = FISList;
            cboFIS.SelectedIndex = -1;

            cboFIS.SelectedIndexChanged += cboFIS_SelectedIndexChanged;
            rdoFIS.CheckedChanged += ErrorSurfaceTypeChanged;

            if (ErrSurfProperty.UniformValue.HasValue)
            {
                rdoUniform.Checked = true;
                valUniform.Value = ErrSurfProperty.UniformValue.Value;
            }

            if (ErrSurfProperty.FISRuleFile is FISLibraryItem)
            {
                rdoFIS.Checked = true;

                if (ErrSurfProperty.FISRuleFile.FISType == ErrorCalculation.FIS.FISLibrary.FISLibraryItemTypes.Project)
                {
                    // Existing error surface with project FIS. The combo will be locked so simply add and selected the project FIS
                    FISList.Add(ErrSurfProperty.FISRuleFile);
                    cboFIS.SelectedItem = ErrSurfProperty.FISRuleFile;
                }
                else
                {          
                    // New error surface select the system or custom FIS library item
                    for (int i = 0; i < cboFIS.Items.Count; i++)
                    {
                        if (string.Compare(((FISLibraryItem)cboFIS.Items[i]).FilePath.FullName, ErrSurfProperty.FISRuleFile.FilePath.FullName, true) == 0)
                        {
                            cboFIS.SelectedIndex = i;
                            break;
                        }
                    }
                }  
            }

            // Finally call the update to put the controls in the correct state
            ErrorSurfaceTypeChanged(sender, e);

            tTip.SetToolTip(rdoUniform, "A single floating point value is used for the entire error surface/region.");
            tTip.SetToolTip(valUniform, "the single floating point value that is used for the entire error surface/region.");
            tTip.SetToolTip(rdoFIS, "The error surface/region is calculated using a fuzzy inference system.");
            tTip.SetToolTip(cboFIS, "The FIS library entry that defines the FIS rule file to be used.");
            tTip.SetToolTip(grdFISInputs, "The list of inputs for the selected FIS rule file and the associated surface that corresponds to each input.");
        }

        private void ErrorSurfaceTypeChanged(object sender, EventArgs e)
        {
            rdoUniform.Enabled = rdoUniform.Checked ? true : Editable;
            valUniform.Enabled = Editable && rdoUniform.Checked;

            rdoFIS.Enabled = rdoFIS.Checked ? true : Editable && AssociatedSurfaces.Count() > 1;
            cboFIS.Enabled = rdoFIS.Checked && Editable;
            cmdFISProperties.Enabled = rdoFIS.Checked && cboFIS.SelectedItem is FISLibraryItem;
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

            FISLibraryItem selectedFIS = cboFIS.SelectedItem as FISLibraryItem;

            // Detect if this is already the identified FIS
            if (!(ErrSurfProperty.FISRuleFile is FISLibraryItem && string.Compare(ErrSurfProperty.FISRuleFile.FilePath.FullName, selectedFIS.FilePath.FullName, true) == 0))
            {
                // Load the inputs for the newly selected FIS rule file into the error properties
                ErrSurfProperty.FISRuleFile = selectedFIS;
                ErrSurfProperty.FISInputs.Clear();
                foreach (FISInputMeta input in selectedFIS.Inputs)
                    ErrSurfProperty.FISInputs.Add(new FISInput(input.Name));
            }

            grdFISInputs.DataSource = ErrSurfProperty.FISInputs;
            cmdFISProperties.Enabled = rdoFIS.Checked && cboFIS.SelectedItem is FISLibraryItem;

            // Select the inputs grid to speed up user input
            grdFISInputs.Select();
            if (grdFISInputs.Rows.Count > 0)
                grdFISInputs.Rows[0].Cells[1].Selected = true;
        }

        public bool ValidateForm()
        {
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

        private void cmdFISProperties_Click(object sender, EventArgs e)
        {
            if (cboFIS.SelectedItem is FISLibraryItem)
            {
                FISLibrary.frmFISProperties frm = new FISLibrary.frmFISProperties((FISLibraryItem) cboFIS.SelectedItem);
                frm.ShowDialog();
            }
        }
    }
}