using System;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class frmCoherenceProperties
    {
        public readonly CoherenceProperties CoherenceProps;

        public frmCoherenceProperties(CoherenceProperties props)
        {
            // This call is required by the designer.
            InitializeComponent();

            CoherenceProps = props;
        }

        private void frmCoherenceProperties_Load(System.Object sender, System.EventArgs e)
        {
            cboFilterSize.SelectedItem = string.Format("{0} x {0}", CoherenceProps.MovingWindowDimensions);
            numLess.Value = CoherenceProps.InflectionA;
            numGreater.Value = CoherenceProps.InflectionB;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (numLess.Value >= numGreater.Value)
            {
                MessageBox.Show("The value for inflection point A must be less than the value for inflection point B.", "Invalid Spatial Coherence Inflection Points", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.None;
                return; 
            }

            CoherenceProps.MovingWindowDimensions = int.Parse(cboFilterSize.Text.Substring(0, cboFilterSize.Text.IndexOf(" ")));
            CoherenceProps.InflectionA = Convert.ToInt32(numLess.Value);
            CoherenceProps.InflectionB = Convert.ToInt32(numGreater.Value);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }
    }
}
