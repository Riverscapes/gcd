using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection.Batch
{
    public partial class frmBatchDoDProperties : Form
    {
        naru.ui.SortableBindingList<ThresholdProps> Thresholds;
        GCDCore.Project.CoherenceProperties CoherenceProps;

        public frmBatchDoDProperties(naru.ui.SortableBindingList<ThresholdProps> thresholds)
        {
            InitializeComponent();
            Thresholds = thresholds;
            rdoSimpleMinLoD.Checked = true;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            if (rdoSimpleMinLoD.Checked)
            {
                Thresholds.Add(new ThresholdProps(valSingleMinLoD.Value));
            }
            else if (rdoMultipleMinLoD.Checked)
            {
                for (decimal minlod = valMMinLoDMin.Value; minlod <= valMMinLoDMax.Value; minlod += valMMinLoDInterval.Value)
                    Thresholds.Add(new ThresholdProps(minlod));
            }
            else if (rdoPropagated.Checked)
                Thresholds.Add(new ThresholdProps());
            else if (rdoSingleProbabilistic.Checked)
            {
                Thresholds.Add(new ThresholdProps(valSProb.Value, CoherenceProps));
            }
            else if (rdoMultipleProbabilistic.Checked)
            {
                for (decimal conf = valMProbMin.Value; conf <= valMProbMax.Value; conf += valMProbInterval.Value)
                    Thresholds.Add(new ThresholdProps(conf, CoherenceProps));
            }
            else if (rdoPrescribed.Checked)
            {
                MessageBox.Show("Not Implemented");
            }
        }

        private bool ValidateForm()
        {
            if (rdoMultipleMinLoD.Checked)
            {
                if (valMMinLoDMin.Value >= valMMinLoDMax.Value)
                {
                    MessageBox.Show("The minimum threshold must be less than the maximum threshold.", "Invalid Thresholds", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valMMinLoDMin.Select();
                    return false;
                }
            }

            if (rdoMultipleProbabilistic.Checked)
            {
                if (valMProbMin.Value >= valMProbMax.Value)
                {
                    MessageBox.Show("The minimum confidence level must be less than the maximum confidence level.", "Invalid Confidence Levels", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valMProbMin.Select();
                    return false;
                }
            }

            return true;
        }

        private void frmBatchDoDProperties_Load(object sender, EventArgs e)
        {
            ConfigureProbNumericUpDown(valSProb);
            ConfigureProbNumericUpDown(valMProbMin);
            ConfigureProbNumericUpDown(valMProbMax);
            ConfigureProbNumericUpDown(valMProbInterval);

            rdoSimpleMinLoD.Checked = true;
        }

        private void ConfigureProbNumericUpDown(NumericUpDown ctrl)
        {
            ctrl.Minimum = 0;
            ctrl.Maximum = 1;
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            lblSingleMinLoD.Enabled = rdoSimpleMinLoD.Checked;
            valSingleMinLoD.Enabled = rdoSimpleMinLoD.Checked;

            lblMMinLoDMin.Enabled = rdoMultipleMinLoD.Checked;
            lblMMinLoDMax.Enabled = rdoMultipleMinLoD.Checked;
            lblMMinLoDInterval.Enabled = rdoMultipleMinLoD.Checked;
            valMMinLoDMin.Enabled = rdoMultipleMinLoD.Checked;
            valMMinLoDMax.Enabled = rdoMultipleMinLoD.Checked;
            valMMinLoDInterval.Enabled = rdoMultipleMinLoD.Checked;

            lblSProb.Enabled = rdoSingleProbabilistic.Checked;
            valSProb.Enabled = rdoSingleProbabilistic.Checked;
            chkSProb.Enabled = rdoSingleProbabilistic.Checked;
            cmdSProb.Enabled = rdoSingleProbabilistic.Checked;

            lblMProbMin.Enabled = rdoMultipleProbabilistic.Checked;
            lblMProbMax.Enabled = rdoMultipleProbabilistic.Checked;
            lblMProbInterval.Enabled = rdoMultipleProbabilistic.Checked;
            valMProbMin.Enabled = rdoMultipleProbabilistic.Checked;
            valMProbMax.Enabled = rdoMultipleProbabilistic.Checked;
            valMProbInterval.Enabled = rdoMultipleProbabilistic.Checked;
            chkMProb.Enabled = rdoMultipleProbabilistic.Checked;
            cmdMProb.Enabled = rdoMultipleProbabilistic.Checked;

            lblPProb.Enabled = rdoPrescribed.Checked;
            chkPProb.Enabled = rdoPrescribed.Checked;
            cmdPProb.Enabled = rdoPrescribed.Checked;
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
