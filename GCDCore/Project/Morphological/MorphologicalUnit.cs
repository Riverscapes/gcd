using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace GCDCore.Project.Morphological
{
    public class MorphologicalUnit
    {
        public string Name { get; internal set; }
        public override string ToString() { return Name; }

        public readonly bool IsTotal;

        public UnitsNet.Volume VolErosion { get; internal set; }
        public UnitsNet.Volume VolErosionErr { get; internal set; }

        public UnitsNet.Volume VolDeposition { get; internal set; }
        public UnitsNet.Volume VolDepositionErr { get; internal set; }

        public UnitsNet.Volume VolChange { get { return VolDeposition - VolErosion; } }
        public UnitsNet.Volume VolChangeErr
        {
            get
            {
                double vol = Math.Sqrt(Math.Pow(VolDeposition.As(UnitsNet.Units.VolumeUnit.CubicMeter), 2)
                    + Math.Pow(VolErosionErr.As(UnitsNet.Units.VolumeUnit.CubicMeter), 2));

                return Volume.FromCubicMeters(vol);
            }
        }

        public UnitsNet.Volume VolIn { get; set; }
        public UnitsNet.Volume VolOut { get; set; }
        public decimal FluxVolume { get; set; }
        public decimal FluxMass { get; set; }
        public UnitsNet.Volume CumulativeVolume { get; set; }

        public MorphologicalUnit(string name, bool isTotal = false)
        {
            Name = name;
            IsTotal = isTotal;
        }

        // Default constructor for binding to grid control
        public MorphologicalUnit()
        {

        }

        //public void CalculateWork(decimal porosity, decimal duration, decimal pcCompetent)
        //{
        //    decimal volume = (1m - porosity) * (decimal)VolOut.As(UnitsNet.Units.VolumeUnit.CubicMeter);
        //    decimal effectiveDuration = duration * (pcCompetent / 100m);
        //    Work = volume / effectiveDuration;
        //}
    }
}
