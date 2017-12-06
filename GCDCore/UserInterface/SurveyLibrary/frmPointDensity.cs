using System.Diagnostics;
using GCDConsoleLib;
using System;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmPointDensity
    {
        private UnitsNet.Units.LengthUnit m_eLinearUnits;

        public RasterOperators.KernelShapes KernelShape { get { return (RasterOperators.KernelShapes)cboNeighbourhood.SelectedItem; } }
        public decimal KernerlSize { get { return valSampleDistance.Value; } }
        public Vector PointCloud { get { return ucPointCloud.SelectedItem; } }

        public frmPointDensity(UnitsNet.Units.LengthUnit eLinearUnits, RasterOperators.KernelShapes kernelShape, decimal kernelSize)
        {
            InitializeComponent();

            m_eLinearUnits = eLinearUnits;
            valSampleDistance.Value = kernelSize;

            foreach (RasterOperators.KernelShapes shape in Enum.GetValues(typeof(RasterOperators.KernelShapes)))
            {
                int index = cboNeighbourhood.Items.Add(shape);
                if (shape == kernelShape)
                    cboNeighbourhood.SelectedIndex = index;
            }

            ucPointCloud.Initialize("Point Cloud", GCDConsoleLib.GDalGeometryType.SimpleTypes.Point);
        }

        private void frmPointDensity_Load(object sender, System.EventArgs e)
        {
            ttpToolTip.SetToolTip(valSampleDistance, "Size of the rectangular sample window (in map units) over which point density is calculated");
            lblDistance.Text = string.Format("{0} {1}:", lblDistance.Text.Substring(0, lblDistance.Text.Length - 1), UnitsNet.Length.GetAbbreviation(m_eLinearUnits));
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
