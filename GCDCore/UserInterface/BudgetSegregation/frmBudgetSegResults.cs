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
    public partial class frmBudgetSegResults : Form, IProjectItemForm
    {
        public enum RawRepresents
        {
            RawDoDAreaOfInterest,
            ThrDoDAreaDetectableChange,
            RawClassAreaOfInterest,
            ThrClassAreaDetectableChange
        }

        private GCDCore.Project.BudgetSegregation BudgetSeg;
        private DoDSummaryDisplayOptions m_Options;
        private Visualization.BudgetSegPieChartViewer PieChartViewer;

        public GCDProjectItem GCDProjectItem { get { return BudgetSeg; } }


        private RawRepresents SelectedRawRepresents { get { return (RawRepresents)((naru.db.NamedObject)cboRaw.SelectedItem).ID; } }

        public frmBudgetSegResults(GCDCore.Project.BudgetSegregation BS)
        {
            // This call is required by the designer.
            InitializeComponent();

            BudgetSeg = BS;
            ucProperties.Initialize(BudgetSeg.DoD);
            m_Options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);

            PieChartViewer = new Visualization.BudgetSegPieChartViewer(BudgetSeg.Classes.Values.ToList<BudgetSegregationClass>(), chtPieCharts);
        }

        private void BudgetSegResultsForm_Load(object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            txtName.Text = BudgetSeg.Name;
            cboBudgetClass.DataSource = BudgetSeg.FilteredClasses;

            cboRaw.Items.Add(new naru.db.NamedObject((long)RawRepresents.RawDoDAreaOfInterest, "Raw DoD Area of Intereset"));
            cboRaw.Items.Add(new naru.db.NamedObject((long)RawRepresents.ThrDoDAreaDetectableChange, "Thresholded DoD Area of Detectable Change"));
            cboRaw.Items.Add(new naru.db.NamedObject((long)RawRepresents.RawClassAreaOfInterest, "Raw DoD within Budget Class"));
            cboRaw.Items.Add(new naru.db.NamedObject((long)RawRepresents.ThrClassAreaDetectableChange, "Thresholded DoD within Budget Class"));

            // Only hook up events once both combo controls are full of content
            cboBudgetClass.SelectedIndexChanged += cboBudgetClass_SelectedIndexChanged;
            cboRaw.SelectedIndexChanged += cboBudgetClass_SelectedIndexChanged;
            cboRaw.SelectedIndex = 0;

            ucProperties.AddDoDProperty("Mask", BudgetSeg.Mask.Name);
            ucProperties.AddDoDProperty("Mask Field", BudgetSeg.Mask._Field);
            ucProperties.AddDoDProperty("Mask Type", BudgetSeg.Mask.Noun);

            //Hide Report tab for now
            tabMain.TabPages.Remove(TabPage4);

            PieChartViewer.RefreshPieCharts(m_Options.Units);
            Cursor = Cursors.Default;
        }

        private void cboBudgetClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBudgetClass.SelectedItem == null)
                return;

            BudgetSegregationClass classResult = (BudgetSegregationClass)cboBudgetClass.SelectedItem;
            List<GCDConsoleLib.GCD.DoDStats> allStats = BudgetSeg.Classes.Values.Select(x => x.Statistics).ToList<GCDConsoleLib.GCD.DoDStats>();

            ucSummary.RefreshDisplay(classResult.Statistics, allStats, SelectedRawRepresents, m_Options);
            ucBars.ChangeStats = classResult.Statistics;

            // The raw histogram is either that for the whole DoD or just the budget seg class
            GCDConsoleLib.Histogram rawHist;
            switch (SelectedRawRepresents)
            {
                case RawRepresents.RawDoDAreaOfInterest: rawHist = BudgetSeg.DoD.Histograms.Raw.Data; break;
                case RawRepresents.ThrDoDAreaDetectableChange: rawHist = BudgetSeg.DoD.Histograms.Thr.Data; break;
                case RawRepresents.RawClassAreaOfInterest: rawHist = classResult.Histograms.Raw.Data; break;
                case RawRepresents.ThrClassAreaDetectableChange: rawHist = classResult.Histograms.Thr.Data; break;
                default:
                    throw new Exception("Unhandled raw represents type");
            }
            ucHistogram.LoadHistograms(rawHist, classResult.Histograms.Thr.Data);
        }

        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results");
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            if (BudgetSeg.Folder.Exists)
            {
                try
                {
                    Process.Start(BudgetSeg.Folder.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error attempting to browse to budget segregation folder at {0}\n\n", BudgetSeg.Folder, ex.Message));
                }
            }
        }
    }
}
