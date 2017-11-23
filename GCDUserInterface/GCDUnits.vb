Public Class GCDUnits

    Public Shared Function GCDLinearUnits() As List(Of UnitsNet.Units.LengthUnit)

        Dim units As New List(Of UnitsNet.Units.LengthUnit)

        units.Add(UnitsNet.Units.LengthUnit.Centimeter)
        units.Add(UnitsNet.Units.LengthUnit.Decimeter)
        units.Add(UnitsNet.Units.LengthUnit.Fathom)
        units.Add(UnitsNet.Units.LengthUnit.Foot)
        units.Add(UnitsNet.Units.LengthUnit.Inch)
        units.Add(UnitsNet.Units.LengthUnit.Kilometer)
        units.Add(UnitsNet.Units.LengthUnit.Meter)
        units.Add(UnitsNet.Units.LengthUnit.Micrometer)
        units.Add(UnitsNet.Units.LengthUnit.Mile)
        units.Add(UnitsNet.Units.LengthUnit.Millimeter)
        units.Add(UnitsNet.Units.LengthUnit.Nanometer)
        units.Add(UnitsNet.Units.LengthUnit.NauticalMile)
        units.Add(UnitsNet.Units.LengthUnit.UsSurveyFoot)
        units.Add(UnitsNet.Units.LengthUnit.Yard)

        Return units

    End Function

    Public Shared Function GCDLinearUnitsAsString() As List(Of String)
        Return GCDLinearUnits().Select(Of String)(Function(p As UnitsNet.Units.LengthUnit) p.ToString()).ToList()
    End Function

    Public Shared Function GCDAreaUnits() As List(Of UnitsNet.Units.AreaUnit)

        Dim units As New List(Of UnitsNet.Units.AreaUnit)

        units.Add(UnitsNet.Units.AreaUnit.Acre)
        units.Add(UnitsNet.Units.AreaUnit.Hectare)
        units.Add(UnitsNet.Units.AreaUnit.SquareCentimeter)
        units.Add(UnitsNet.Units.AreaUnit.SquareDecimeter)
        units.Add(UnitsNet.Units.AreaUnit.SquareFoot)
        units.Add(UnitsNet.Units.AreaUnit.SquareInch)
        units.Add(UnitsNet.Units.AreaUnit.SquareKilometer)
        units.Add(UnitsNet.Units.AreaUnit.SquareMeter)
        units.Add(UnitsNet.Units.AreaUnit.SquareMicrometer)
        units.Add(UnitsNet.Units.AreaUnit.SquareMile)
        units.Add(UnitsNet.Units.AreaUnit.SquareMillimeter)
        units.Add(UnitsNet.Units.AreaUnit.SquareYard)

        Return units

    End Function

    Public Shared Function GCDAreaUnitsAsString() As List(Of String)
        Return GCDAreaUnits().Select(Of String)(Function(p As UnitsNet.Units.AreaUnit) p.ToString()).ToList()
    End Function

    Public Shared Function GCDVolumeUnits() As List(Of UnitsNet.Units.VolumeUnit)

        Dim units As New List(Of UnitsNet.Units.VolumeUnit)

        units.Add(UnitsNet.Units.VolumeUnit.AuTablespoon)
        units.Add(UnitsNet.Units.VolumeUnit.CubicCentimeter)
        units.Add(UnitsNet.Units.VolumeUnit.CubicDecimeter)
        units.Add(UnitsNet.Units.VolumeUnit.CubicFoot)
        units.Add(UnitsNet.Units.VolumeUnit.CubicInch)
        units.Add(UnitsNet.Units.VolumeUnit.CubicKilometer)
        units.Add(UnitsNet.Units.VolumeUnit.CubicMeter)
        units.Add(UnitsNet.Units.VolumeUnit.CubicMicrometer)
        units.Add(UnitsNet.Units.VolumeUnit.CubicMile)
        units.Add(UnitsNet.Units.VolumeUnit.CubicMillimeter)
        units.Add(UnitsNet.Units.VolumeUnit.CubicYard)
        units.Add(UnitsNet.Units.VolumeUnit.ImperialBeerBarrel)
        units.Add(UnitsNet.Units.VolumeUnit.HectocubicFoot)
        units.Add(UnitsNet.Units.VolumeUnit.HectocubicMeter)
        units.Add(UnitsNet.Units.VolumeUnit.KilocubicFoot)
        units.Add(UnitsNet.Units.VolumeUnit.KilocubicMeter)
        units.Add(UnitsNet.Units.VolumeUnit.MegacubicFoot)
        units.Add(UnitsNet.Units.VolumeUnit.OilBarrel)
        units.Add(UnitsNet.Units.VolumeUnit.UsCustomaryCup)
        units.Add(UnitsNet.Units.VolumeUnit.UsGallon)
        units.Add(UnitsNet.Units.VolumeUnit.UsLegalCup)
        units.Add(UnitsNet.Units.VolumeUnit.UsOunce)
        units.Add(UnitsNet.Units.VolumeUnit.UsTablespoon)
        units.Add(UnitsNet.Units.VolumeUnit.UsTeaspoon)

        Return units

    End Function

    Public Shared Function GCDVolumeUnitsAsString() As List(Of String)
        Return GCDAreaUnits().Select(Of String)(Function(p As UnitsNet.Units.VolumeUnit) p.ToString()).ToList()
    End Function

End Class
