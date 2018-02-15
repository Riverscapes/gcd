using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using UnitsNet;

namespace GCDCore.UserInterface.BudgetSegregation.Morphological
{
    public partial class frmMorphResults : Form
    {
        public readonly GCDCore.Project.Morphological.MorphologicalAnalysis Analysis;

        public frmMorphResults(GCDCore.Project.Morphological.MorphologicalAnalysis ma)
        {
            InitializeComponent();

            Analysis = ma;
        }

        private void frmMorphResults_Load(object sender, EventArgs e)
        {
            foreach (UnitsNet.Units.DurationUnit val in Enum.GetValues(typeof(UnitsNet.Units.DurationUnit)))
                cboDuration.Items.Add(val);

            cboDuration.SelectedItem = Analysis.DurationDisplayUnits;
            valDuration.Value = (decimal)Analysis.Duration.As(Analysis.DurationDisplayUnits);
            valPorosity.Value = Analysis.Porosity;
            valDensity.Value = Analysis.Density;

            txtName.Text = Analysis.Name;
            txtPath.Text = ProjectManager.Project.GetRelativePath(Analysis.OutputFolder.FullName);

            grdData.AutoGenerateColumns = false;
            grdData.DataSource = Analysis.Units;
        }

        private void cboBS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Analysis == null)
            //{
            //    GCDCore.Project.BudgetSegregation bs = cboBS.SelectedItem as GCDCore.Project.BudgetSegregation;
            //    txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetMorphologicalDirectory(bs.Folder, false).FullName);
            //}
            //else
            //{
            //    txtPath.Text = ProjectManager.Project.GetRelativePath(Analysis.OutputFolder);
            //}
        }

        private void cboDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal newValue = (decimal)UnitsNet.Duration.From((double)valDuration.Value, Analysis.DurationDisplayUnits).As((UnitsNet.Units.DurationUnit)cboDuration.SelectedItem);
            valDuration.Maximum = Math.Max(valDuration.Maximum, newValue);
            valDuration.Value = newValue;

            Analysis.DurationDisplayUnits = (UnitsNet.Units.DurationUnit)cboDuration.SelectedItem;

            UpdateCriticalDuration();
        }

        private void valDuration_ValueChanged(object sender, EventArgs e)
        {
            Analysis.Duration = UnitsNet.Duration.From((double)valDuration.Value, (UnitsNet.Units.DurationUnit)cboDuration.SelectedItem);
            UpdateCriticalDuration();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            UpdateCriticalDuration();
        }

        private void UpdateCriticalDuration()
        {
            decimal value = 0;
            if (valPercentCompetent.Value > 0)
                value = valDuration.Value * valPercentCompetent.Value;

            txtCriticalDuration.Text = string.Format("{0:F} {1}{2}", value, cboDuration.SelectedItem.ToString(), value > 0 ? "s" : "");
        }
    }
}
