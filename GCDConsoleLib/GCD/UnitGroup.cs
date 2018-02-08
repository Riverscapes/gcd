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

        /// <summary>
        /// Constructor. These units are created together and passed around to make conversions easier
        /// </summary>
        /// <param name="volU"></param>
        /// <param name="arU"></param>
        /// <param name="vertU"></param>
        /// <param name="horU"></param>
        public UnitGroup(VolumeUnit volU, AreaUnit arU, LengthUnit vertU, LengthUnit horU)
        {
            VolUnit = volU;
            ArUnit = arU;
            VertUnit = vertU;
            HorizUnit = horU;
        }

        /// <summary>
        /// Mainly used for testing
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(UnitGroup other)
        {
            return VolUnit.Equals(other.VolUnit) &&
                ArUnit.Equals(other.ArUnit) &&
                VertUnit.Equals(other.VertUnit) &&
                HorizUnit.Equals(other.HorizUnit);
        }
    }
}
