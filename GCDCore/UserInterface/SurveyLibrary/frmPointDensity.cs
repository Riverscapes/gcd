using System.Diagnostics;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmPointDensity
    {
        private UnitsNet.Units.LengthUnit m_eLinearUnits;

        public frmPointDensity(UnitsNet.Units.LengthUnit eLinearUnits)
        {
            InitializeComponent();

            m_eLinearUnits = eLinearUnits;

            ucPointCloud.Initialize("Point Cloud", GCDConsoleLib.GDalGeometryType.SimpleTypes.Point);
        }

        private void frmPointDensity_Load(object sender, System.EventArgs e)
        {
            ttpToolTip.SetToolTip(valSampleDistance, "Size of the rectangular sample window (in map units) over which point density is calculated");

            cboNeighbourhood.Items.Add("Circle");
            cboNeighbourhood.Items.Add("Rectangle");
            cboNeighbourhood.SelectedIndex = 1;

            lblDistance.Text = string.Format("{0} {1}:", lblDistance.Text.Substring(0, lblDistance.Text.Length - 1), UnitsNet.Length.GetAbbreviation(m_eLinearUnits));

        }

        public string Neighborhood
        {
            get
            {
                if (cboNeighbourhood.SelectedIndex == 0)
                {
                    // Circle
                    return cboNeighbourhood.SelectedItem.ToString() + " " + valSampleDistance.Value.ToString() + " MAP";
                }
                else
                {
                    //rectangle
                    return cboNeighbourhood.SelectedItem.ToString() + " " + valSampleDistance.Value.ToString() + " " + valSampleDistance.Value.ToString() + " MAP";
                }
            }
        }


        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (!ucPointCloud.Validate())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/3-deriving-point-density");
        }
    }

}
