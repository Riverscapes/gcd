namespace GCDCore.UserInterface.ChangeDetection
{
    public class DoDSummaryDisplayOptions
    {
        public enum RowGroups
        {
            ShowAll,
            Normalized,
            SpecificGroups
        }

        private GCDConsoleLib.GCD.UnitGroup m_Units;

        public int m_nPrecision;
        public RowGroups m_eRowGroups;
        public bool m_bRowsAreal;
        public bool m_bRowsVolumetric;
        public bool m_bRowsVerticalAverages;

        public bool m_bRowsPercentages;
        public bool m_bColsRaw;
        public bool m_bColsThresholded;
        public bool m_bColsPMError;
        public bool m_bColsPCError;

        public System.Drawing.Font Font { get; set; }

        public bool AutomatedYAxisScale { get; set; }

        public GCDConsoleLib.GCD.UnitGroup Units { get { return m_Units; } }

        public UnitsNet.Units.LengthUnit LinearUnits
        {
            get { return m_Units.VertUnit; }
            set { m_Units = new GCDConsoleLib.GCD.UnitGroup(m_Units.VolUnit, m_Units.ArUnit, value, m_Units.HorizUnit); }
        }

        public UnitsNet.Units.AreaUnit AreaUnits
        {
            get { return m_Units.ArUnit; }
            set { m_Units = new GCDConsoleLib.GCD.UnitGroup(m_Units.VolUnit, value, m_Units.VertUnit, m_Units.HorizUnit); }
        }


        public UnitsNet.Units.VolumeUnit VolumeUnits
        {
            get { return m_Units.VolUnit; }
            set { m_Units = new GCDConsoleLib.GCD.UnitGroup(value, m_Units.ArUnit, m_Units.VertUnit, m_Units.HorizUnit); }
        }

        public DoDSummaryDisplayOptions(GCDConsoleLib.GCD.UnitGroup dataUnits)
        {
            m_Units = dataUnits;
            m_nPrecision = 2;

            m_eRowGroups = RowGroups.ShowAll;
            m_bRowsAreal = true;
            m_bRowsVolumetric = true;
            m_bRowsVerticalAverages = true;
            m_bRowsPercentages = true;

            m_bColsRaw = true;
            m_bColsThresholded = true;
            m_bColsPMError = true;
            m_bColsPCError = true;

            Font = Properties.Settings.Default.ChartFont;

            AutomatedYAxisScale = true;
        }
    }
}