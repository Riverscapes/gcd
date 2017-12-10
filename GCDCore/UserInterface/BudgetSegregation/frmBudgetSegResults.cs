using GCDCore.Project;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using GCDCore.UserInterface.ChangeDetection;

namespace GCDCore.UserInterface.BudgetSegregation
{
    public partial class frmBudgetSegResults
    {
        private GCDCore.Project.BudgetSegregation BudgetSeg;

        private ChangeDetection.DoDSummaryDisplayOptions m_Options;

        //BindingList<BudgetSegregationClass> Classes;

        public frmBudgetSegResults(GCDCore.Project.BudgetSegregation BS)
        {
            // This call is required by the designer.
            InitializeComponent();

            BudgetSeg = BS;
            ucProperties.Initialize(BudgetSeg.DoD);
            m_Options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);
        }

        private void BudgetSegResultsForm_Load(object sender, System.EventArgs e)
        {
            txtName.Text = BudgetSeg.Name;
            cboBudgetClass.DataSource = new BindingList<BudgetSegregationClass>(BudgetSeg.Classes.Values.ToList());

            txtPolygonMask.Text = ProjectManager.Project.GetRelativePath(BudgetSeg.PolygonMask.FullName);
            txtField.Text = BudgetSeg.MaskField;

            //Hide Report tab for now
            tabMain.TabPages.Remove(TabPage4);
        }

        private void cboBudgetClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BudgetSegregationClass classResult = (BudgetSegregationClass) cboBudgetClass.SelectedItem;
            ucSummary.RefreshDisplay(classResult.Statistics, m_Options);
            ucHistogram.LoadHistograms(classResult.Histograms.Raw.Data, classResult.Histograms.Thr.Data);
            ucBars.ChangeStats = classResult.Statistics;
        }

        private void cmdBrowse_Click(System.Object sender, System.EventArgs e)
        {
            if (BudgetSeg.Folder.Exists)
            {
                Process.Start("explorer.exe", BudgetSeg.Folder.FullName);
            }
            else
            {
                MessageBox.Show("The budget segregation folder does not exist: " + BudgetSeg.Folder.FullName, Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results");
        }

        private void AddToMapToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            ToolStripMenuItem myItem = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)myItem.Owner;

            System.IO.FileInfo path = ProjectManager.Project.GetAbsolutePath(cms.SourceControl.Text);
            if (path.Exists)
            {
                try
                {
                    GCDConsoleLib.Vector gPolygon = new GCDConsoleLib.Vector(path);
                    throw new NotImplementedException("not implemented");
                    //GCDProject.ProjectManagerUI.ArcMapManager.AddBSMaskVector(gPolygon, m_rBS)
                }
                catch (Exception ex)
                {
                    //Pass
                }
            }
        }
    }
}
