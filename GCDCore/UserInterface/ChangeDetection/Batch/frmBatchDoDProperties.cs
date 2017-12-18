using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Engines.DoD;

namespace GCDCore.UserInterface.ChangeDetection.Batch
{
    public partial class frmBatchDoDProperties : Form
    {
        public readonly frmBatchDoD.ThresholdTypes ThresholdType;
        naru.ui.SortableBindingList<ThresholdProps> Thresholds;
        GCDCore.Project.CoherenceProperties CoherenceProps;

        public frmBatchDoDProperties(naru.ui.SortableBindingList<ThresholdProps> thresholds, string sType, frmBatchDoD.ThresholdTypes eType)
        {
            InitializeComponent();
            Thresholds = thresholds;
            ThresholdType = eType;
            Text = string.Format("Add {0} To Batch", sType);
        }

        private void frmBatchDoDProperties_Load(object sender, EventArgs e)
        {

            if (ThresholdType == frmBatchDoD.ThresholdTypes.MinLoDSingle ||
                ThresholdType == frmBatchDoD.ThresholdTypes.MinLoDMulti)
            {
                chkBayesian.Visible = false;
                cmdBayesian.Visible = false;
                Height -= cmdOK.Top - chkBayesian.Top;
            }

            if (ThresholdType == frmBatchDoD.ThresholdTypes.MinLoDSingle ||
                ThresholdType == frmBatchDoD.ThresholdTypes.ProbSingle)
            {
                lblMax.Visible = false;
                valMax.Visible = false;
                lblInterval.Visible = false;
                valInterval.Visible = false;

                Height -= chkBayesian.Top - valMax.Top;

                chkBayesian.Top = valMax.Top;
                cmdBayesian.Top = valMax.Top;
            }

            string vertUnits = UnitsNet.Length.GetAbbreviation(GCDCore.Project.ProjectManager.Project.Units.VertUnit);

            switch (ThresholdType)
            {
                case frmBatchDoD.ThresholdTypes.MinLoDSingle:
                    lblMin.Text = string.Format("Minimum level of detection ({0})", vertUnits);
                    break;

                case frmBatchDoD.ThresholdTypes.MinLoDMulti:
                    lblMin.Text = string.Format("Minimum MinLoD threshold ({0})", vertUnits);
                    lblMax.Text = string.Format("Maximum MinLoD threshold ({0})", vertUnits);
                    lblInterval.Text = string.Format("Interval ({0})", vertUnits);

                    valMin.Maximum = 1;
                    valMax.Maximum = 1;
                    valInterval.Maximum = 1;

                    chkBayesian.Visible = false;
                    cmdBayesian.Visible = false;
                    Height -= cmdOK.Top - chkBayesian.Top;
                    break;

                case frmBatchDoD.ThresholdTypes.ProbSingle:
                    lblMin.Text = "Confidence level (0-1)";
                    valMin.Maximum = 1;
                    break;

                case frmBatchDoD.ThresholdTypes.ProbMulti:
                    lblMin.Text = "Minimum confidence level (0-1)";
                    lblMax.Text = "Maximum confidence level (0-1)";
                    lblInterval.Text = "Interval (0-1)";
                    break;

                default:
                    MessageBox.Show("Not Implemented");
                    break;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            switch (ThresholdType)
            {
                case frmBatchDoD.ThresholdTypes.MinLoDSingle:
                    Thresholds.Add(new ThresholdProps(valMin.Value));
                    break;

                case frmBatchDoD.ThresholdTypes.MinLoDMulti:
                    for (decimal minlod = valMin.Value; minlod <= valMax.Value; minlod += valInterval.Value)
                        Thresholds.Add(new ThresholdProps(minlod));
                    break;

                case frmBatchDoD.ThresholdTypes.ProbSingle:
                    Thresholds.Add(new ThresholdProps(valMin.Value, CoherenceProps));
                    break;

                case frmBatchDoD.ThresholdTypes.ProbMulti:
                    for (decimal conf = valMin.Value; conf <= valMax.Value; conf += valInterval.Value)
                        Thresholds.Add(new ThresholdProps(conf, CoherenceProps));
                    break;

                default:
                    MessageBox.Show("Not Implemented");
                    break;
            }
        }

        private bool ValidateForm()
        {
            if (ThresholdType == frmBatchDoD.ThresholdTypes.MinLoDMulti ||
                ThresholdType == frmBatchDoD.ThresholdTypes.ProbMulti)
            {
                if (valMin.Value >= valMax.Value)
                {
                    if (ThresholdType == frmBatchDoD.ThresholdTypes.MinLoDMulti)
                    {
                        MessageBox.Show("The minimum threshold must be less than the maximum threshold.", "Invalid Thresholds", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("The minimum confidence level must be less than the maximum confidence level.", "Invalid Confidence Levels", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    valMin.Select();
                    return false;
                }
            }

            return true;
        }

        private void ConfigureProbNumericUpDown(NumericUpDown ctrl)
        {
            ctrl.Minimum = 0;
            ctrl.Maximum = 1;
        }

        private void valSingleMinLoD_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void chkSProb_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
                CoherenceProps = new GCDCore.Project.CoherenceProperties();
            else
                CoherenceProps = null;
        }

        private void cmdSProb_Click(object sender, EventArgs e)
        {
            bool bNewObject = false;
            if (CoherenceProps == null)
            {
                CoherenceProps = new GCDCore.Project.CoherenceProperties();
                bNewObject = true;
            }

            frmCoherenceProperties frm = new frmCoherenceProperties(CoherenceProps);
            if (frm.ShowDialog() != DialogResult.OK && bNewObject)
            {
                // User canceled form and there were no existing coherence properties.
                // Reset the coherence properties item.
                CoherenceProps = null;
            }
        }
    }
}
