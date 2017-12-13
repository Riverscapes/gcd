using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucThresholding : UserControl
    {
        public EventHandler OnThresholdingMethodChanged;

        public bool IsMindLoD { get { return rdoMinLOD.Checked; } }
        public decimal MinLoD { get { return valMinLodThreshold.Value; } }

        public bool IsPropagated { get { return rdoPropagated.Checked; } }

        public bool IsProbabilistic { get { return rdoProbabilistic.Checked; } }
        public decimal ConfidenceThreshold { get { return valConfidence.Value; } }
        public bool UsesSpatialCoherence { get { return chkBayesian.Checked; } }
        public readonly CoherenceProperties CoherenceProps;

        public ucThresholding()
        {
            InitializeComponent();

            // Initialize coherence properties in case they are needed.
            CoherenceProps = new CoherenceProperties();
        }

        private void ucThresholding_Load(object sender, EventArgs e)
        {
            UpdateControls(sender, e);

            lblMinLodThreshold.Text = string.Format("Threshold ({0})", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            valMinLodThreshold.Enabled = rdoMinLOD.Checked;
            lblMinLodThreshold.Enabled = rdoMinLOD.Checked;

            lblConfidence.Enabled = rdoProbabilistic.Checked;
            valConfidence.Enabled = rdoProbabilistic.Checked;
            chkBayesian.Enabled = rdoProbabilistic.Checked;
            cmdBayesianProperties.Enabled = rdoProbabilistic.Checked && chkBayesian.Checked;

            if (OnThresholdingMethodChanged != null)
                OnThresholdingMethodChanged(sender, e);
        }

        private void cmdBayesianProperties_Click(System.Object sender, System.EventArgs e)
        {
            frmCoherenceProperties frm = new frmCoherenceProperties(CoherenceProps);
            frm.ShowDialog();
        }
    }
}
