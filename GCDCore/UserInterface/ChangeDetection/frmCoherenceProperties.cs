using System;
using System.Collections.Generic;
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
            // The filter sizes display as the diameter of the kernel but store the radius
            foreach (long kernelSize in new List<long> { 3, 5, 7, 9, 11, 13, 15 })
            {
                long bufferSize = (kernelSize - 1) / 2;
                int index = cboFilterSize.Items.Add(new naru.db.NamedObject(bufferSize, string.Format("{0} x {0}", kernelSize)));
                if (bufferSize == CoherenceProps.BufferSize)
                {
                    cboFilterSize.SelectedIndex = index;
                }
            }

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

            CoherenceProps.BufferSize = Convert.ToInt32(((naru.db.NamedObject)cboFilterSize.SelectedItem).ID);
            CoherenceProps.InflectionA = Convert.ToInt32(numLess.Value);
            CoherenceProps.InflectionB = Convert.ToInt32(numGreater.Value);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }
    }
}
