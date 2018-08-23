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
        public frmBatchDoD.ThresholdTypes ThresholdType { get; set; }
        naru.ui.SortableBindingList<BatchProps> Thresholds;
        GCDCore.Project.CoherenceProperties CoherenceProps;

        private readonly int DesignHeight;
        private readonly int Spacing;
        private readonly int BayesianChkTop;
        private readonly int BayesianCmdTop;

        public frmBatchDoDProperties(naru.ui.SortableBindingList<BatchProps> thresholds)
        {
            InitializeComponent();
            Thresholds = thresholds;
            DesignHeight = Height;
            Spacing = Height - cmdBayesian.Bottom;
            BayesianChkTop = chkBayesian.Top;
            BayesianCmdTop = cmdBayesian.Top;
        }

        private void frmBatchDoDProperties_Load(object sender, EventArgs e)
        {
            ucDEMs.EnableErrorSurfaces(ThresholdType != frmBatchDoD.ThresholdTypes.MinLoDSingle && ThresholdType != frmBatchDoD.ThresholdTypes.MinLoDMulti);

            string vertUnits = UnitsNet.Length.GetAbbreviation(GCDCore.Project.ProjectManager.Project.Units.VertUnit);

            lblMin.Visible = ThresholdType != frmBatchDoD.ThresholdTypes.Propagated;
            valMin.Visible = ThresholdType != frmBatchDoD.ThresholdTypes.Propagated;

            valMin.Value = 0;
            valMax.Value = 0;
            valInterval.Value = 0;
            chkBayesian.Checked = false;
            CoherenceProps = null;

            bool bRange = ThresholdType == frmBatchDoD.ThresholdTypes.MinLoDMulti || ThresholdType == frmBatchDoD.ThresholdTypes.ProbMulti;
            lblMax.Visible = bRange;
            valMax.Visible = bRange;
            lblInterval.Visible = bRange;
            valInterval.Visible = bRange;

            chkBayesian.Top = BayesianChkTop;
            cmdBayesian.Top = BayesianCmdTop;
            bool bProb = ThresholdType == frmBatchDoD.ThresholdTypes.ProbSingle || ThresholdType == frmBatchDoD.ThresholdTypes.ProbMulti;
            chkBayesian.Visible = bProb;
            cmdBayesian.Visible = bProb;

            switch (ThresholdType)
            {
                case frmBatchDoD.ThresholdTypes.MinLoDSingle:
                    lblMin.Text = string.Format("Minimum level of detection ({0})", vertUnits);
                    Height = valMin.Bottom + Spacing;
                    break;

                case frmBatchDoD.ThresholdTypes.MinLoDMulti:
                    lblMin.Text = string.Format("Minimum MinLoD threshold ({0})", vertUnits);
                    lblMax.Text = string.Format("Maximum MinLoD threshold ({0})", vertUnits);
                    lblInterval.Text = string.Format("Interval ({0})", vertUnits);
                    Height = valMax.Bottom + Spacing;
                    break;

                case frmBatchDoD.ThresholdTypes.ProbSingle:
                    lblMin.Text = "Confidence level (0-1)";
                    valMin.Minimum = 0.01m;
                    valMin.Value = 0.8m;
                    valMin.Maximum = 1;
                    chkBayesian.Top = lblMax.Top;
                    cmdBayesian.Top = valMax.Top;

                    Height = valMax.Bottom + Spacing;
                    break;

                case frmBatchDoD.ThresholdTypes.ProbMulti:
                    lblMin.Text = "Minimum confidence level (0-1)";
                    lblMax.Text = "Maximum confidence level (0-1)";
                    lblInterval.Text = "Interval (0-1)";
                    valMin.Minimum = 0.01m;
                    valMin.Maximum = 1;
                    valMax.Minimum = 0.01m;
                    valMax.Maximum = 1;
                    valInterval.Maximum = 1;
                    Height = chkBayesian.Bottom + Spacing;
                    break;

                default:
                    Height = ucDEMs.Bottom + Spacing;
                    break;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult eResult = ValidateForm();
            if (eResult != DialogResult.OK)
            {
                DialogResult = eResult;
                return;
            }

            switch (ThresholdType)
            {
                case frmBatchDoD.ThresholdTypes.MinLoDSingle:
                    Thresholds.Add(new BatchProps(ucDEMs.NewSurface, null, ucDEMs.OldSurface, null, ucDEMs.AOIMask, new ThresholdProps(valMin.Value)));
                    break;

                case frmBatchDoD.ThresholdTypes.MinLoDMulti:
                    for (decimal minlod = valMin.Value; minlod <= valMax.Value; minlod += valInterval.Value)
                        Thresholds.Add(new BatchProps(ucDEMs.NewSurface, null, ucDEMs.OldSurface, null, ucDEMs.AOIMask, new ThresholdProps(minlod)));
                    break;

                case frmBatchDoD.ThresholdTypes.Propagated:
                    Thresholds.Add(new BatchProps(ucDEMs.NewSurface, ucDEMs.NewError, ucDEMs.OldSurface, ucDEMs.OldError, ucDEMs.AOIMask, new ThresholdProps()));
                    break;

                case frmBatchDoD.ThresholdTypes.ProbSingle:
                    Thresholds.Add(new BatchProps(ucDEMs.NewSurface, ucDEMs.NewError, ucDEMs.OldSurface, ucDEMs.OldError, ucDEMs.AOIMask, new ThresholdProps(valMin.Value, CoherenceProps)));
                    break;

                case frmBatchDoD.ThresholdTypes.ProbMulti:
                    for (decimal conf = valMin.Value; conf <= valMax.Value; conf += valInterval.Value)
                        Thresholds.Add(new BatchProps(ucDEMs.NewSurface, ucDEMs.NewError, ucDEMs.OldSurface, ucDEMs.OldError, ucDEMs.AOIMask, new ThresholdProps(conf, CoherenceProps)));
                    break;

                default:
                    MessageBox.Show("Not Implemented");
                    break;
            }
        }

        private DialogResult ValidateForm()
        {
            if (!ucDEMs.ValidateForm())
                return DialogResult.None;

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
                    return DialogResult.None;
                }

                if (valInterval.Value == 0)
                {
                    MessageBox.Show("The interval cannot be zero.", "Invalid Interval", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valInterval.Select();
                    return DialogResult.None;
                }

                long count = Convert.ToInt64((valMax.Value - valMin.Value) / valInterval.Value);
                if (count > 20)
                {
                    switch (MessageBox.Show(string.Format("This process is about to generate a large number ({0:n0}) of change detection analyses in this GCD project." +
                        " Are you sure you want to proceed with this operation?", count),
                         Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        case DialogResult.No:
                            valMax.Select();
                            return DialogResult.None;

                        case DialogResult.Cancel:
                            return DialogResult.Cancel;
                    }
                }
            }

            return DialogResult.OK;
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
