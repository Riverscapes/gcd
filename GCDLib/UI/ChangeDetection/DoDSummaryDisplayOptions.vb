Imports naru.math.NumberFormatting

Namespace UI.ChangeDetection

    Public Class DoDSummaryDisplayOptions

        Public Enum RowGroups
            ShowAll
            Normalized
            SpecificGroups
        End Enum

        Private m_eLinearUnits As LinearUnits
        Private m_eAreaUnits As AreaUnits
        Private m_eVolumeUnits As VolumeUnits
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

        Public Property LinearUnits As naru.math.NumberFormatting.LinearUnits
            Get
                Return m_eLinearUnits
            End Get
            Set(value As naru.math.NumberFormatting.LinearUnits)
                m_eLinearUnits = value
            End Set
        End Property

        Public Property AreaUnits As naru.math.NumberFormatting.AreaUnits
            Get
                Return naru.math.NumberFormatting.GetAreaUnitsRaw(m_eLinearUnits)
            End Get
            Set(value As naru.math.NumberFormatting.AreaUnits)
                m_eAreaUnits = value
            End Set

        End Property

        Public ReadOnly Property AreaUnitsAsString As naru.math.NumberFormatting.AreaUnits
            Get
                Return naru.math.NumberFormatting.GetUnitsAsString(AreaUnits)
            End Get
        End Property

        Public Property VolumeUnits As naru.math.NumberFormatting.VolumeUnits
            Get
                Return naru.math.NumberFormatting.GetVolumeUnitsRaw(m_eLinearUnits)
            End Get
            Set(value As naru.math.NumberFormatting.VolumeUnits)
                m_eVolumeUnits = value
            End Set
        End Property

        Public ReadOnly Property VolumeUnitsAsString As naru.math.NumberFormatting.AreaUnits
            Get
                Return naru.math.NumberFormatting.GetUnitsAsString(VolumeUnits)
            End Get
        End Property

        Public Sub New(eOriginalUnits As LinearUnits)

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