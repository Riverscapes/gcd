Imports naru.math.NumberFormatting

Namespace UI.ChangeDetection

    Public Class DoDSummaryDisplayOptions

        Public Enum RowGroups
            ShowAll
            Normalized
            SpecificGroups
        End Enum

        Private m_eLinearUnits As UnitsNet.Units.LengthUnit
        Private m_eAreaUnits As UnitsNet.Units.AreaUnit
        Private m_eVolumeUnits As UnitsNet.Units.VolumeUnit
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

        Public Property LinearUnits As UnitsNet.Units.LengthUnit
            Get
                Return m_eLinearUnits
            End Get
            Set(value As UnitsNet.Units.LengthUnit)
                m_eLinearUnits = value
            End Set
        End Property

        Public Property AreaUnits As UnitsNet.Units.AreaUnit
            Get
                Throw New NotImplementedException("Waiting to hear back from UnitsNet author on how best to do this.")
                Return UnitsNet.Units.AreaUnit.SquareMeter
            End Get
            Set(value As UnitsNet.Units.AreaUnit)
                m_eAreaUnits = value
            End Set

        End Property

        Public Property VolumeUnits As UnitsNet.Units.VolumeUnit
            Get
                Throw New NotImplementedException("Waiting to hear back from UnitsNet author on how best to do this.")
                Return UnitsNet.Units.VolumeUnit.CubicMeter
            End Get
            Set(value As UnitsNet.Units.VolumeUnit)
                m_eVolumeUnits = value
            End Set
        End Property

        Public Sub New(eOriginalUnits As UnitsNet.Units.LengthUnit)

            m_eLinearUnits = eOriginalUnits

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