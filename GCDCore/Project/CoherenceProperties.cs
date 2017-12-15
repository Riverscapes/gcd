using System;

namespace GCDCore.Project
{
    public class CoherenceProperties
    {
        // a 5x5 kernel has a KernelRadius of 2
        public int BufferSize { get; set; }
        public int KernelSize { get { return BufferSize * 2 + 1; } }

        public int InflectionA { get; set; }
        public int InflectionB { get; set; }

        public int XMin { get { return Convert.ToInt32(Math.Floor((Math.Pow((double) KernelSize, 2) * ((double) InflectionA / 100)))); } }
        public int XMax { get { return Convert.ToInt32(Math.Floor((Math.Pow((double) KernelSize, 2) * ((double) InflectionB / 100)))); } }

        public CoherenceProperties(int bufferSize, int nInflectionA, int nInflectionB)
        {
            if (bufferSize < 1)
            {
                throw new ArgumentOutOfRangeException("MovingwindowDimensions", bufferSize, "The moving window dimension must be greater than zero.");
            }
            BufferSize = bufferSize;

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
            BufferSize = 2;
            InflectionA = 60;
            InflectionB = 100;
        }
    }
}