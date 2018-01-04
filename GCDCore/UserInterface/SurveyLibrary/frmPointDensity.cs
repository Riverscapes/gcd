using System.Diagnostics;
using GCDConsoleLib;
using System;
using GCDCore.Project;

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

        private void cboNeighbourhood_SelectedIndexChanged(object sender, EventArgs e)
        {
            string label;
            if ((RasterOperators.KernelShapes)cboNeighbourhood.SelectedItem == RasterOperators.KernelShapes.Circle)
            {
                label = "Diameter";
                ttpToolTip.SetToolTip(valSampleDistance, string.Format("Diameter of the circular sample window (in {0}) over which point density is calculated", m_eLinearUnits));
            }
            else
            {
                label = "Length";
                ttpToolTip.SetToolTip(valSampleDistance, string.Format("Size of the square sample window (in {0}) over which point density is calculated", m_eLinearUnits));
            }

            lblDistance.Text = string.Format("{0} ({1})", label, UnitsNet.Length.GetAbbreviation(m_eLinearUnits));
        }

        private void frmPointDensity_Load(object sender, EventArgs e)
        {
            if (ProjectManager.IsArcMap)
            {
                ucPointCloud.BrowseVector += ProjectManager.OnBrowseVector;
                ucPointCloud.SelectVector += ProjectManager.OnSelectVector;
            }
        }
    }
}
