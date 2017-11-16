Namespace ChangeDetection

    Public Class DoDSummaryDisplayOptions

        Public Enum RowGroups
            ShowAll
            Normalized
            SpecificGroups
        End Enum

        Private m_Units As GCDConsoleLib.GCD.UnitGroup
        Public m_nPrecision As Integer

        Public m_eRowGroups As RowGroups
        Public m_bRowsAreal As Boolean
        Public m_bRowsVolumetric As Boolean
        Public m_bRowsVerticalAverages As Boolean
        Public m_bRowsPercentages As Boolean

        Public m_bColsRaw As Boolean
        Public m_bColsThresholded As Boolean
        Public m_bColsPMError As Boolean
        Public m_bColsPCError As Boolean

        Public ReadOnly Property Units As GCDConsoleLib.GCD.UnitGroup
            Get
                Return m_Units
            End Get
        End Property

        Public Property LinearUnits As UnitsNet.Units.LengthUnit
            Get
                Return m_Units.VertUnit
            End Get
            Set(value As UnitsNet.Units.LengthUnit)
                m_Units = New GCDConsoleLib.GCD.UnitGroup(m_Units.VolUnit, m_Units.ArUnit, value, m_Units.HorizUnit)
            End Set
        End Property

        Public Property AreaUnits As UnitsNet.Units.AreaUnit
            Get
                Return m_Units.ArUnit
            End Get
            Set(value As UnitsNet.Units.AreaUnit)
                m_Units = New GCDConsoleLib.GCD.UnitGroup(m_Units.VolUnit, value, m_Units.VertUnit, m_Units.HorizUnit)
            End Set

        End Property

        Public Property VolumeUnits As UnitsNet.Units.VolumeUnit
            Get
                Return m_Units.VolUnit
            End Get
            Set(value As UnitsNet.Units.VolumeUnit)
                m_Units = New GCDConsoleLib.GCD.UnitGroup(value, m_Units.ArUnit, m_Units.VertUnit, m_Units.HorizUnit)
            End Set
        End Property

        Public Sub New(dataUnits As GCDConsoleLib.GCD.UnitGroup)

            m_Units = dataUnits
            m_nPrecision = 2

            m_eRowGroups = RowGroups.ShowAll
            m_bRowsAreal = True
            m_bRowsVolumetric = True
            m_bRowsVerticalAverages = True
            m_bRowsPercentages = True

            m_bColsRaw = True
            m_bColsThresholded = True
            m_bColsPMError = True
            m_bColsPCError = True

        End Sub

    End Class

End Namespace