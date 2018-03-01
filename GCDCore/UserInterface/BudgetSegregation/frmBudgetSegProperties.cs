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

            // Add the event handling after data binding to reduce false firing
            cboDoD.SelectedIndexChanged += cboDoD_SelectedIndexChanged;
            cboDoD.DataSource = new BindingList<DoDBase>(ProjectManager.Project.DoDs.Values.ToList<DoDBase>());
            cboDoD.SelectedItem = InitialDoD;

            cboMasks.DataSource = new BindingList<GCDCore.Project.Masks.Mask>(ProjectManager.Project.Masks.Values.Where(x=>x is GCDCore.Project.Masks.AttributeFieldMask).ToList<GCDCore.Project.Masks.Mask>());
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

                DoDBase dod = cboDoD.SelectedItem as DoDBase;
                GCDCore.Project.Masks.AttributeFieldMask mask = cboMasks.SelectedItem as GCDCore.Project.Masks.AttributeFieldMask;

                System.IO.DirectoryInfo bsFolder = ProjectManager.Project.GetAbsoluteDir(txtOutputFolder.Text);
                Engines.BudgetSegregationEngine bsEngine = new Engines.BudgetSegregationEngine();
                BudgetSeg = bsEngine.Calculate(txtName.Text, bsFolder, dod, mask);
                dod.BudgetSegregations[BudgetSeg.Name] = BudgetSeg;

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

            if (!(cboDoD.SelectedItem is DoDBase))
            {
                MessageBox.Show("Please choose a change detection analysis on which you want to base this budget segregation.", "Missing Change Detection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboDoD.Select();
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the budget segregation analysis.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }
            else
            {
                if (!((DoDBase)cboDoD.SelectedItem).IsBudgetSegNameUnique(txtName.Text, null))
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

        private void cboDoD_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoDBase dod = (DoDBase)cboDoD.SelectedItem;

            txtNewDEM.Text = dod.NewSurface.Name;
            txtOldDEM.Text = dod.OldSurface.Name;

            if (dod is DoDMinLoD)
            {
                txtUncertaintyAnalysis.Text = string.Format("Minimum Level of Detection at {0:#0.00}{1}", ((DoDMinLoD)dod).Threshold, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            }
            else if (dod is DoDPropagated)
            {
                txtUncertaintyAnalysis.Text = "Propagated Error";
            }
            else
            {
                txtUncertaintyAnalysis.Text = string.Format("Probabilistic at {0}% confidence level", ((DoDProbabilistic)dod).ConfidenceLevel * 100);
            }

            txtOutputFolder.Text = ProjectManager.Project.GetRelativePath(dod.BudgetSegPath().FullName);
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
            DoDBase dod = cboDoD.SelectedItem as DoDBase;

            int index = 0;
            string result = string.Empty;

            do
            {
                result = string.Format("Budget Segregation via {0}", maskName);
                if (index > 0)
                    result = string.Format("{0} ({1})", result, index);

                index++;

            } while (dod.BudgetSegregations.ContainsKey(result));

            return result;
        }
    }
}
