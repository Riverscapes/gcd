using System;

namespace GCDCore.Project
{
    public class CoherenceProperties
    {
        // a 5x5 kernel has a KernelRadius of 2
        public  int KernelRadius { get; set; }

        public int InflectionA { get; set; }
        public int InflectionB { get; set; }

        public int XMin { get { return Convert.ToInt32(Math.Floor((double)(KernelRadius ^ 2) * (InflectionA / 100))); } }
        public int XMax { get { return Convert.ToInt32(Math.Floor((double)(KernelRadius ^ 2) * (InflectionB / 100))); } }

        public CoherenceProperties(int nMovingWindowDimensions, int nInflectionA, int nInflectionB)
        {
            if (nMovingWindowDimensions < 1)
            {
                throw new ArgumentOutOfRangeException("MovingwindowDimensions", nMovingWindowDimensions, "The moving window dimension must be greater than zero.");
            }
            KernelRadius = nMovingWindowDimensions;

            if (nInflectionA < 0 || nInflectionA > 100)
            {
                throw new ArgumentOutOfRangeException("InflectionA", nInflectionA, "The inflection A point must be greater than or equal to zero and less than or equal to 100.");
            }
            InflectionA = nInflectionA;

            if (nInflectionB < 0 || nInflectionB > 100)
            {
                throw new ArgumentOutOfRangeException("InflectionB", nInflectionB, "The inflection B point must be greater than or equal to zero and less than or equal to 100.");
            }
            InflectionB = nInflectionB;
        }

        public CoherenceProperties()
        {
            KernelRadius = 2;
            InflectionA = 60;
            InflectionB = 100;
        }
    }
}