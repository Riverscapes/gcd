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
using GCDCore.Project.Morphological;

namespace GCDCore.UserInterface.BudgetSegregation.Morphological
{
    public partial class frmMorpProperties : Form, IProjectItemForm
    {
        public MorphologicalAnalysis Analysis { get; internal set; }

        public GCDProjectItem GCDProjectItem { get { return Analysis as GCDProjectItem; } }

        public frmMorpProperties(GCDCore.Project.BudgetSegregation bs)
        {
            InitializeComponent();

            // Load all budget segregations that are part of the same DoD 
            cboBS.DataSource = new BindingList<GCDCore.Project.BudgetSegregation>(bs.DoD.BudgetSegregations.Where(x => x.IsMaskDirectional).ToList<GCDCore.Project.BudgetSegregation>());
            cboBS.SelectedItem = bs;
        }

        private void frmMorpProperties_Load(object sender, EventArgs e)
        {
            cmdOK.Text = Properties.Resources.CreateButtonText;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                // Create the morphological analysis
                Analysis = new MorphologicalAnalysis(txtName.Text, ProjectManager.Project.GetAbsoluteDir(txtPath.Text), cboBS.SelectedItem as GCDCore.Project.BudgetSegregation);

                try
                {
                    // Save the morphological spreadsheet to file
                    Analysis.OutputFolder.Create();
                    Analysis.SaveExcelSpreadsheet();
                }
                catch (Exception ex)
                {
                    // We can live without the spreadsheet
                    Console.Write("Morphological analysis spreadsheet error saving to " + Analysis.Spreadsheet.FullName);
                }

                GCDCore.Project.BudgetSegregation bs = cboBS.SelectedItem as GCDCore.Project.BudgetSegregation;
                bs.MorphologicalAnalyses.Add(Analysis);
                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                GCDException.HandleException(ex, "Error generating morphological analysis.");
            }
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

            txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.GetIndexedSubDirectory(bs.MorphologicalFolder, "MA").FullName);
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

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
