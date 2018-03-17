using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using GCDCore.Project;

namespace GCDCore.UserInterface.BudgetSegregation
{
    public partial class frmBudgetSegProperties : Form, IProjectItemForm
    {
        public GCDCore.Project.BudgetSegregation BudgetSeg { get; internal set; }
        private DoDBase InitialDoD;

        public GCDProjectItem GCDProjectItem { get { return BudgetSeg; } }

        public frmBudgetSegProperties(DoDBase parentDoD)
        {
            // This call is required by the designer.
            InitializeComponent();
            InitialDoD = parentDoD;
        }

        private void frmBudgetSegProperties_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.CreateButtonText;

            txtDoDName.Text = InitialDoD.Name;
            txtNewDEM.Text = InitialDoD.NewSurface.Name;
            txtOldDEM.Text = InitialDoD.OldSurface.Name;
            txtOutputFolder.Text = ProjectManager.Project.GetRelativePath(InitialDoD.BudgetSegPath().FullName);
            txtUncertaintyAnalysis.Text = InitialDoD.UncertaintyAnalysisLabel;

            cboMasks.DataSource = new BindingList<GCDCore.Project.Masks.Mask>(ProjectManager.Project.Masks.Values.Where(x => x is GCDCore.Project.Masks.AttributeFieldMask).ToList<GCDCore.Project.Masks.Mask>());
            if (cboMasks.Items.Count > 0)
                cboMasks.SelectedIndex = 0;

            // Default the focus to the mask.
            cboMasks.Select();
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
                GCDCore.Project.Masks.AttributeFieldMask mask = cboMasks.SelectedItem as GCDCore.Project.Masks.AttributeFieldMask;

                System.IO.DirectoryInfo bsFolder = ProjectManager.Project.GetAbsoluteDir(txtOutputFolder.Text);
                Engines.BudgetSegregationEngine bsEngine = new Engines.BudgetSegregationEngine();
                BudgetSeg = bsEngine.Calculate(txtName.Text, bsFolder, InitialDoD, mask);
                InitialDoD.BudgetSegregations[BudgetSeg.Name] = BudgetSeg;

                ProjectManager.Project.Save();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
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

            if (cboMasks.SelectedItem == null)
            {
                MessageBox.Show("Please choose a mask on which you wish to base this budget segregation.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMasks.Select();
                return false;
            }

            return true;
        }

        private void cmdHelp_Click(Object sender, EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/v-add-budget-segregation");
        }

        private void cboMasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtField.Text = ((GCDCore.Project.Masks.AttributeFieldMask)cboMasks.SelectedItem)._Field;
            txtName.Text = GetUniqueName(cboMasks.Text);
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

            } while (InitialDoD.BudgetSegregations.ContainsKey(result));

            return result;
        }
    }
}
