using System;
using UnitsNet;

namespace GCDCore.Project.Morphological
{
    public class MorphologicalUnit
    {
        public string Name { get; internal set; }
        public override string ToString() { return Name; }
        public readonly bool IsTotal;

        public Volume VolErosion { get; internal set; }
        public Volume VolErosionErr { get; internal set; }

        public Volume VolDeposition { get; internal set; }
        public Volume VolDepositionErr { get; internal set; }

        public Volume VolChange { get { return VolDeposition - VolErosion; } }
        public Volume VolChangeErr
        {
            get
            {
                double vol = Math.Sqrt(Math.Pow(VolDeposition.As(UnitsNet.Units.VolumeUnit.CubicMeter), 2)
                    + Math.Pow(VolErosionErr.As(UnitsNet.Units.VolumeUnit.CubicMeter), 2));

                return Volume.FromCubicMeters(vol);
            }
        }

        public Volume VolIn { get; set; }
        public Volume VolOut { get; set; }

        public decimal FluxVolume { get; set; }
        public decimal FluxMass { get; set; }

        public Volume CumulativeVolume { get; set; }

        public MorphologicalUnit(string name, bool isTotal = false)
        {
            Name = name;
            IsTotal = isTotal;
        }

        // Default constructor for binding to grid control
        public MorphologicalUnit()
        {

        }
    }
}
