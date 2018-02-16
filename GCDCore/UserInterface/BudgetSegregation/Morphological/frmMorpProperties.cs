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

namespace GCDCore.UserInterface.BudgetSegregation.Morphological
{
    public partial class frmMorpProperties : Form
    {
        public GCDCore.Project.Morphological.MorphologicalAnalysis Analysis { get; internal set; }

        public frmMorpProperties(GCDCore.Project.BudgetSegregation bs)
        {
            InitializeComponent();

            // Load all budget segregations that are part of the same DoD 
            cboBS.DataSource = new BindingList<GCDCore.Project.BudgetSegregation>(bs.DoD.BudgetSegregations.Values.ToList<GCDCore.Project.BudgetSegregation>().Where(x=>x.IsMaskDirectional).ToList<GCDCore.Project.BudgetSegregation>());
            cboBS.SelectedItem = bs;
        }

        private void frmMorpProperties_Load(object sender, EventArgs e)
        {

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            Analysis = new GCDCore.Project.Morphological.MorphologicalAnalysis(txtName.Text, ProjectManager.Project.GetAbsoluteDir(txtPath.Text), cboBS.SelectedItem as GCDCore.Project.BudgetSegregation);
        }

        private bool ValidateForm()
        {
            // Sanity check to avoid duplicate name
            txtName.Text = txtName.Text.Trim();

            if (!ValidateName(txtName, (GCDCore.Project.BudgetSegregation)cboBS.SelectedItem, null))
                return false;

            return true;
        }

        private void cboBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            GCDCore.Project.BudgetSegregation bs = cboBS.SelectedItem as GCDCore.Project.BudgetSegregation;
            txtMask.Text = bs.Mask.Name;
            txtDoD.Text = bs.DoD.Name;
            txtUncertainty.Text = bs.DoD.UncertaintyAnalysisLabel;

            txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetMorphologicalDirectory(bs.Folder, false).FullName);
        }

        public static bool ValidateName(TextBox txtName, GCDCore.Project.BudgetSegregation bs, GCDCore.Project.Morphological.MorphologicalAnalysis ma)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("The morphological analysis name cannot be blank.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (!bs.IsMorphologicalAnalysisNameUnique(txtName.Text, ma))
            {
                MessageBox.Show(string.Format("The {0} budget segregation already possesses a morphological analysis called \"{1}\"." +
                    " Please enter a unique name for the analysis.", bs.Name, txtName.Text), "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            return true;
        }
    }
}
