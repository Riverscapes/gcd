using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using GCDCore.Project;
using GCDCore.Project.Masks;

namespace GCDCore.UserInterface.BudgetSegregation
{
    public partial class frmBudgetSegProperties : Form, IProjectItemForm
    {
        public GCDCore.Project.BudgetSegregation BudgetSeg { get; internal set; }
        private DoDBase InitialDoD;

        public GCDProjectItem GCDProjectItem { get { return BudgetSeg; } }

        private AttributeFieldMask SelectedMask
        {
            get
            {
                if (rdoRegular.Checked)
                    return cboRegularMasks.SelectedItem as AttributeFieldMask;
                else
                    return cboDirMasks.SelectedItem as AttributeFieldMask;
            }
        }

        public frmBudgetSegProperties(DoDBase parentDoD)
        {
            // This call is required by the designer.
            InitializeComponent();
            InitialDoD = parentDoD;
        }

        private void frmBudgetSegProperties_Load(object sender, EventArgs e)
        {
            tTip.SetToolTip(txtName, "The name for the budget segregation. It cannot be empty and must be unique across all budget segregations for the parent change detection.");
            tTip.SetToolTip(txtOutputFolder, "The relative output folder within this GCD project where the budget segregation will get produced.");
            tTip.SetToolTip(rdoRegular, "Choose this option to generate a budget segregation from a regular (non-directional) mask.");
            tTip.SetToolTip(rdoDirectional, "Choose this option to generate a budget segregation from a directional mask.");
            tTip.SetToolTip(cboRegularMasks, "Choose a mask from the list of all the regular masks in this GCD project.");
            tTip.SetToolTip(cboDirMasks, "Choose a mask from the list of all the directional masks in this GCD project.");

            cmdOK.Text = Properties.Resources.CreateButtonText;
            txtOutputFolder.Text = ProjectManager.Project.GetRelativePath(InitialDoD.BudgetSegPath().FullName);

            cboRegularMasks.DataSource = new BindingList<Mask>(ProjectManager.Project.Masks.Where(x => x is RegularMask).ToList());
            if (cboRegularMasks.Items.Count > 0)
            {
                cboRegularMasks.SelectedIndex = 0;
                cboRegularMasks.Select();
            }
            else
                rdoRegular.Enabled = false;

            cboDirMasks.DataSource = new BindingList<Mask>(ProjectManager.Project.Masks.Where(x => x is DirectionalMask).ToList());
            if (cboDirMasks.Items.Count > 0)
            {
                cboDirMasks.SelectedIndex = 0;
                if (cboRegularMasks.Items.Count < 1)
                {
                    rdoDirectional.Checked = true;
                    cboDirMasks.Select();
                }
            }
            else
            {
                rdoDirectional.Enabled = false;
            }

            // Ensure the appropriate controls are checked
            MaskTypeChanged(sender, e);
        }

        private void MaskTypeChanged(object sender, EventArgs e)
        {
            cboRegularMasks.Enabled = rdoRegular.Checked;
            cboDirMasks.Enabled = rdoDirectional.Checked;
            cboMasks_SelectedIndexChanged(sender, e);
        }

        private void cmdOK_Click(Object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AttributeFieldMask mask = SelectedMask;

                System.IO.DirectoryInfo bsFolder = ProjectManager.Project.GetAbsoluteDir(txtOutputFolder.Text);
                Engines.BudgetSegregationEngine bsEngine = new Engines.BudgetSegregationEngine();
                BudgetSeg = bsEngine.Calculate(txtName.Text, bsFolder, InitialDoD, mask);
                InitialDoD.BudgetSegregations.Add(BudgetSeg);

                ProjectManager.Project.Save();
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ValidateForm()
        {
            // Sanity check to avoid names with only empty spaces
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the budget segregation analysis.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }
            else
            {
                if (!InitialDoD.IsBudgetSegNameUnique(txtName.Text, null))
                {
                    MessageBox.Show("Another budget segregation already uses the name '" + txtName.Text + "'. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Select();
                    return false;
                }
            }

            if (SelectedMask == null)
            {
                MessageBox.Show("Please choose a mask on which you wish to base this budget segregation.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboRegularMasks.Select();
                return false;
            }

            return true;
        }

        private void cmdHelp_Click(Object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        private void cboMasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maskName = string.Empty;
            AttributeFieldMask mask = SelectedMask;
            if (mask != null)
                maskName = mask.Name;
            
            txtName.Text = GetUniqueName(maskName);
        }

        private string GetUniqueName(string maskName)
        {
            int index = 0;
            string result = string.Empty;

            do
            {
                result = string.Format("Budget Segregation via {0}", maskName);
                if (index > 0)
                    result = string.Format("{0} ({1})", result, index);

                index++;

            } while (InitialDoD.BudgetSegregations.Any(x => string.Compare(x.Name, result, true) == 0));

            return result;
        }
    }
}
