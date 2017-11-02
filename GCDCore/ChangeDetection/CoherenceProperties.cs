using System;

namespace GCDCore.ChangeDetection
{
    public class CoherenceProperties
    {
        public readonly int MovingWindowDimensions;
        public readonly int InflectionA;
        public int InflectionB;

        public int XMin { get { return Convert.ToInt32(Math.Floor((double)(MovingWindowDimensions ^ 2) * (InflectionA / 100))); } }
        public int XMax { get { return Convert.ToInt32(Math.Floor((double)(MovingWindowDimensions ^ 2) * (InflectionB / 100))); } }

        public CoherenceProperties(int nMovingWindowDimensions, int nInflectionA, int nInflectionB)
        {
            if (nMovingWindowDimensions < 1)
            {
                throw new ArgumentOutOfRangeException("MovingwindowDimensions", nMovingWindowDimensions, "The moving window dimension must be greater than zero.");
            }
            MovingWindowDimensions = nMovingWindowDimensions;

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
            MovingWindowDimensions = 5;
            InflectionA = 60;
            InflectionB = 100;
        }
    }
}