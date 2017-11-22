namespace GCDCore.Visualization
{
    public class HistogramDisplayData
    {
        private double m_elevation;
        private double m_erosion;
        private double m_deposition;

        private double m_raw;
        public double Elevation
        {
            get { return m_elevation; }
        }

        public double Deposition
        {
            get { return m_deposition; }
            set { m_deposition = value; }
        }

        public double Erosion
        {
            get { return m_erosion; }
            set { m_erosion = value; }
        }

        public double Raw
        {
            get { return m_raw; }
            set { m_raw = value; }
        }

        public HistogramDisplayData(double fElevation)
        {
            m_elevation = fElevation;
            m_erosion = 0;
            m_deposition = 0;
        }
    }
}