using System;
using UnitsNet.Units;
using UnitsNet;

namespace GCDConsoleLib.GCD
{
    public class UnitGroup
    {
        public readonly VolumeUnit VolUnit;
        public readonly AreaUnit ArUnit;
        public readonly LengthUnit VertUnit;
        public readonly LengthUnit HorizUnit;

        public UnitGroup(VolumeUnit volU, AreaUnit arU, LengthUnit vertU, LengthUnit horU)
        {
            VolUnit = volU;
            ArUnit = arU;
            VertUnit = vertU;
            HorizUnit = horU;
        }
    }
}
