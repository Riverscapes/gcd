using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GCDCore
{
    public class GCDUnits
    {
        public class FormattedUnit<T>
        {
            public readonly T Unit;

            public override string ToString()
            {
                return Regex.Replace(Unit.ToString(), "(\\B[A-Z])", " $1");
            }

            public FormattedUnit(T unit)
            {
                Unit = unit;
            }
        }

        private static void AddUnit(Dictionary<string, UnitsNet.Units.LengthUnit> dict, UnitsNet.Units.LengthUnit unit)
        {
            dict[Regex.Replace(unit.ToString(), "(\\B[A-Z])", " $1")] = unit;
        }

        public static Dictionary<string, UnitsNet.Units.LengthUnit> GetLinearUnits2()
        {
            Dictionary<string, UnitsNet.Units.LengthUnit> result = new Dictionary<string, UnitsNet.Units.LengthUnit>();

            AddUnit(result, UnitsNet.Units.LengthUnit.Centimeter);
            AddUnit(result, UnitsNet.Units.LengthUnit.Decimeter);
            AddUnit(result, UnitsNet.Units.LengthUnit.Fathom);
            AddUnit(result, UnitsNet.Units.LengthUnit.Foot);
            AddUnit(result, UnitsNet.Units.LengthUnit.Inch);
            AddUnit(result, UnitsNet.Units.LengthUnit.Kilometer);
            AddUnit(result, UnitsNet.Units.LengthUnit.Meter);
            AddUnit(result, UnitsNet.Units.LengthUnit.Micrometer);
            AddUnit(result, UnitsNet.Units.LengthUnit.Mile);
            AddUnit(result, UnitsNet.Units.LengthUnit.Millimeter);
            AddUnit(result, UnitsNet.Units.LengthUnit.Nanometer);
            AddUnit(result, UnitsNet.Units.LengthUnit.NauticalMile);
            AddUnit(result, UnitsNet.Units.LengthUnit.UsSurveyFoot);
            AddUnit(result, UnitsNet.Units.LengthUnit.Yard);

            return result;
        }

        public static void SelectUnit(System.Windows.Forms.ComboBox cbo, UnitsNet.Units.LengthUnit unit)
        {
            foreach(FormattedUnit<UnitsNet.Units.LengthUnit> cboUnit in cbo.Items)
            {
                if (cboUnit.Unit == unit)
                {
                    cbo.SelectedItem = cboUnit;
                    return;
                }
            }
        }

        public static List<FormattedUnit<UnitsNet.Units.LengthUnit>> GCDLinearUnits()
        {
            List<FormattedUnit<UnitsNet.Units.LengthUnit>> units = new List<FormattedUnit<UnitsNet.Units.LengthUnit>> {
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Centimeter),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Decimeter),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Fathom),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Foot),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Inch),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Kilometer),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Meter),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Micrometer),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Mile),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Millimeter),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Nanometer),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.NauticalMile),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.UsSurveyFoot),
                new FormattedUnit<UnitsNet.Units.LengthUnit>(UnitsNet.Units.LengthUnit.Yard)
            };

            return units;
        }

        public static void SelectUnit(System.Windows.Forms.ComboBox cbo, UnitsNet.Units.AreaUnit unit)
        {
            foreach (FormattedUnit<UnitsNet.Units.AreaUnit> cboUnit in cbo.Items)
            {
                if (cboUnit.Unit == unit)
                {
                    cbo.SelectedItem = cboUnit;
                    return;
                }
            }
        }

        public static List<FormattedUnit<UnitsNet.Units.AreaUnit>> GCDAreaUnits()
        {
            List<FormattedUnit<UnitsNet.Units.AreaUnit>> units = new List<FormattedUnit<UnitsNet.Units.AreaUnit>> {
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.Acre),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.Hectare),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareCentimeter),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareDecimeter),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareFoot),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareInch),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareKilometer),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareMeter),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareMicrometer),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareMile),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareMillimeter),
                new FormattedUnit<UnitsNet.Units.AreaUnit>(UnitsNet.Units.AreaUnit.SquareYard)
            };

            return units;
        }


        public static void SelectUnit(System.Windows.Forms.ComboBox cbo, UnitsNet.Units.VolumeUnit unit)
        {
            foreach (FormattedUnit<UnitsNet.Units.VolumeUnit> cboUnit in cbo.Items)
            {
                if (cboUnit.Unit == unit)
                {
                    cbo.SelectedItem = cboUnit;
                    return;
                }
            }
        }

        public static List<FormattedUnit<UnitsNet.Units.VolumeUnit>> GCDVolumeUnits()
        {
            List<FormattedUnit<UnitsNet.Units.VolumeUnit>> units = new List<FormattedUnit<UnitsNet.Units.VolumeUnit>> {
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.AuTablespoon),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicCentimeter),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicDecimeter),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicFoot),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicInch),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicKilometer),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicMeter),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicMicrometer),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicMile),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicMillimeter),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.CubicYard),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.ImperialBeerBarrel),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.HectocubicFoot),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.HectocubicMeter),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.KilocubicFoot),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.KilocubicMeter),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.MegacubicFoot),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.OilBarrel),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.UsCustomaryCup),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.UsGallon),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.UsLegalCup),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.UsOunce),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.UsTablespoon),
                new FormattedUnit<UnitsNet.Units.VolumeUnit>(UnitsNet.Units.VolumeUnit.UsTeaspoon)
            };

            return units;
        }

        public static void SelectUnit(System.Windows.Forms.ComboBox cbo, UnitsNet.Units.DurationUnit unit)
        {
            foreach (FormattedUnit<UnitsNet.Units.DurationUnit> cboUnit in cbo.Items)
            {
                if (cboUnit.Unit == unit)
                {
                    cbo.SelectedItem = cboUnit;
                    return;
                }
            }
        }

        public static List<FormattedUnit<UnitsNet.Units.DurationUnit>> GCDDurationUnits()
        {
            List<FormattedUnit<UnitsNet.Units.DurationUnit>> units = new List<FormattedUnit<UnitsNet.Units.DurationUnit>> {
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Day),
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Hour),
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Minute),
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Month),
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Second),
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Week),
                new FormattedUnit<UnitsNet.Units.DurationUnit>(UnitsNet.Units.DurationUnit.Year)
            };

            return units;
        }

        public static void SelectUnit(System.Windows.Forms.ComboBox cbo, UnitsNet.Units.MassUnit unit)
        {
            foreach (FormattedUnit<UnitsNet.Units.MassUnit> cboUnit in cbo.Items)
            {
                if (cboUnit.Unit == unit)
                {
                    cbo.SelectedItem = cboUnit;
                    return;
                }
            }
        }

        public static List<FormattedUnit<UnitsNet.Units.MassUnit>> GCDMassUnits()
        {
            List<FormattedUnit<UnitsNet.Units.MassUnit>> units = new List<FormattedUnit<UnitsNet.Units.MassUnit>> {
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.Gram),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.Kilogram),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.Kilotonne),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.LongTon),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.Pound),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.Tonne),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.ShortTon),
                new FormattedUnit<UnitsNet.Units.MassUnit>(UnitsNet.Units.MassUnit.Milligram)
            };

            return units;
        }

        //public static List<string> GCDVolumeUnitsAsString()
        //{
        //    return GCDVolumeUnits().Select((UnitsNet.Units.VolumeUnit p) => p.ToString()).ToList(),
        //}
    }
}