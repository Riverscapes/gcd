using System;
using System.Collections.Generic;

namespace GCDCore.Visualization
{
    public class ElevationChangeDataPoint
    {
        private double m_fElevation;
        private double m_fAreaChange;

        private double m_fVolumeChange;
        public double Elevation
        {
            get { return m_fElevation; }
        }

        public ElevationChangeDataPoint(double fElevation, double fArea, double fVolume)
        {
            m_fElevation = fElevation;
            m_fAreaChange = fArea;
            m_fVolumeChange = fVolume;
        }

        public double AreaChange(UnitsNet.Units.AreaUnit dataUnits, UnitsNet.Units.AreaUnit displayUnits)
        {
            return UnitsNet.Area.From(m_fAreaChange, dataUnits).As(displayUnits);
        }

        public double VolumeChange(UnitsNet.Units.VolumeUnit dataUnits, UnitsNet.Units.VolumeUnit displayUnits)
        {
            return UnitsNet.Volume.From(m_fVolumeChange, dataUnits).As(displayUnits);
        }

        public double GetElevation(UnitsNet.Units.LengthUnit dataUnits, UnitsNet.Units.LengthUnit displayUnits)
        {
            return UnitsNet.Length.From(m_fElevation, dataUnits).As(displayUnits);
        }

        public double AreaDeposition(UnitsNet.Units.AreaUnit dataUnits, UnitsNet.Units.AreaUnit displayUnits)
        {
            if (m_fElevation > 0)
            {
                return UnitsNet.Area.From(m_fAreaChange, dataUnits).As(displayUnits);
            }
            else
            {
                return 0;
            }
        }

        public double AreaErosion(UnitsNet.Units.AreaUnit dataUnits, UnitsNet.Units.AreaUnit displayUnits)
        {
            if (m_fElevation < 0)
            {
                return UnitsNet.Area.From(m_fAreaChange, dataUnits).As(displayUnits);
            }
            else
            {
                return 0;
            }
        }

        public double VolumeErosion(UnitsNet.Units.VolumeUnit dataUnits, UnitsNet.Units.VolumeUnit displayUnits)
        {
            if (m_fElevation < 0)
            {
                return UnitsNet.Volume.From(m_fVolumeChange, dataUnits).As(displayUnits);
            }
            else
            {
                return 0;
            }
        }

        public double VolumeDeposition(UnitsNet.Units.VolumeUnit dataUnits, UnitsNet.Units.VolumeUnit displayUnits)
        {
            if (m_fElevation > 0)
            {
                return UnitsNet.Volume.From(m_fVolumeChange, dataUnits).As(displayUnits);
            }
            else
            {
                return 0;
            }
        }
    }
}