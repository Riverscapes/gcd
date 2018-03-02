using GCDCore.Project;
using GCDCore.Visualization;
using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.ChangeDetection
{
    public partial class ucChangeBars
    {
        public readonly ElevationChangeBarViewer Viewer;
        private GCDConsoleLib.GCD.DoDStats m_chngStats;
        public GCDConsoleLib.GCD.DoDStats ChangeStats
        {
            get { return m_chngStats; }

            set
            {
                // If this is the first time specifying the change stats then need to also set the units
                // But set the units by setting the internal variable to avoid double call of RefreshBars()
                bool bUseChangeStatsUnits = m_chngStats == null;
                if (value != null)
                {
                    m_chngStats = value;
                    m_DisplayUnits = value.StatsUnits;

                    RefreshBars(null, null);
                }
            }
        }

        private GCDConsoleLib.GCD.UnitGroup m_DisplayUnits;
        public GCDConsoleLib.GCD.UnitGroup DisplayUnits
        {
            get { return m_DisplayUnits; }
            set
            {
                m_DisplayUnits = value;
                RefreshBars(null, null);
            }
        }

        public ContextMenuStrip ChartContextMenuStrip
        {
            get
            {
                return chtControl.ContextMenuStrip;
            }

            set
            {
                chtControl.ContextMenuStrip = value;
            }
        }

        public ucChangeBars()
        {
            // This call is required by the designer.
            InitializeComponent();
            // Add any initialization after the InitializeComponent() call.
            Viewer = new ElevationChangeBarViewer(chtControl);
        }

        private void ChangeBarsUC_Load(object sender, System.EventArgs e)
        {
            cboType.Items.Add(new naru.db.NamedObject((long)ElevationChangeBarViewer.BarTypes.Area, "Areal"));
            cboType.Items.Add(new naru.db.NamedObject((long)ElevationChangeBarViewer.BarTypes.Volume, "Volumetric"));
            cboType.Items.Add(new naru.db.NamedObject((long)ElevationChangeBarViewer.BarTypes.Vertical, "Vertical Averages"));

            // Add these handlers here so that everything is initialized properly before they fire
            rdoAbsolute.CheckedChanged += RefreshBars;
            cboType.SelectedIndexChanged += RefreshBars;
            cboType.SelectedIndex = 0;

            Viewer.SetFont(Properties.Settings.Default.ChartFont);
        }

        private void RefreshBars(object sender, EventArgs e)
        {
            if (!(cboType.SelectedItem is naru.db.NamedObject) || ProjectManager.Project == null)
            {
                return;
            }

            ElevationChangeBarViewer.BarTypes eType = (ElevationChangeBarViewer.BarTypes)Convert.ToInt32(((naru.db.NamedObject)cboType.SelectedItem).ID);

            UnitsNet.Area ca = ProjectManager.Project.CellArea;

            switch (eType)
            {
                case ElevationChangeBarViewer.BarTypes.Area:
                    Viewer.Refresh(m_chngStats.ErosionThr.GetArea(ca).As(DisplayUnits.ArUnit), m_chngStats.DepositionThr.GetArea(ca).As(DisplayUnits.ArUnit), UnitsNet.Area.GetAbbreviation(DisplayUnits.ArUnit), eType, rdoAbsolute.Checked);

                    break;
                case ElevationChangeBarViewer.BarTypes.Volume:
                    Viewer.Refresh(m_chngStats.ErosionThr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit), m_chngStats.DepositionThr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit), m_chngStats.NetVolumeOfDifference_Thresholded.As(DisplayUnits.VolUnit), m_chngStats.ErosionErr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit), m_chngStats.DepositionErr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit), m_chngStats.NetVolumeOfDifference_Error.As(m_chngStats.StatsUnits.VolUnit), UnitsNet.Volume.GetAbbreviation(m_chngStats.StatsUnits.VolUnit), eType, rdoAbsolute.Checked);

                    break;
                case ElevationChangeBarViewer.BarTypes.Vertical:
                    Viewer.Refresh(m_chngStats.AverageDepthErosion_Thresholded.As(DisplayUnits.VertUnit), m_chngStats.AverageDepthDeposition_Thresholded.As(DisplayUnits.VertUnit), m_chngStats.AverageThicknessOfDifferenceADC_Thresholded.As(DisplayUnits.VertUnit), m_chngStats.AverageThicknessOfDifferenceADC_Error.As(DisplayUnits.VertUnit), m_chngStats.AverageDepthErosion_Error.As(DisplayUnits.VertUnit), m_chngStats.AverageNetThicknessOfDifferenceADC_Error.As(DisplayUnits.VertUnit), UnitsNet.Length.GetAbbreviation(DisplayUnits.VertUnit), eType, rdoAbsolute.Checked);
                    break;
            }
        }
    }
}
