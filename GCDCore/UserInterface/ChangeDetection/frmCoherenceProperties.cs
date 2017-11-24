using System;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class frmCoherenceProperties
    {
        public int FilterSize { get; set; }

        public int PercentLess
        {
            get { return Convert.ToInt32(numLess.Value); }
            set { numLess.Value = value; }
        }

        public int PercentGreater
        {
            get { return Convert.ToInt32(numGreater.Value); }
            set { numGreater.Value = value; }
        }


        private void frmCoherenceProperties_Load(System.Object sender, System.EventArgs e)
        {
            string sFilterSizeText = FilterSize.ToString() + " x " + FilterSize.ToString();
            cboFilterSize.SelectedItem = sFilterSizeText;
        }

        private void cboFilterSize_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int i = cboFilterSize.Text.IndexOf(" ");
            if (i > 0)
            {
                FilterSize = int.Parse(cboFilterSize.Text.Substring(0, i));
            }
        }

        public frmCoherenceProperties()
        {
            // This call is required by the designer.
            InitializeComponent();

            // Default filter size.
            FilterSize = 5;
        }
    }
}
