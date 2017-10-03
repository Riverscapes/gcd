Public Class DoDSummaryDisplayOptions

    Public Enum RowGroups
        ShowAll
        Normalized
        SpecificGroups
    End Enum

    Public m_eLinearUnits As NumberFormatting.LinearUnits
    Public m_eAreaUnits As NumberFormatting.AreaUnits
    Public m_eVolumetricUnits As NumberFormatting.VolumeUnits
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

    Public Sub New(eOriginalUnits As NumberFormatting.LinearUnits)

        m_eLinearUnits = eOriginalUnits

        Select Case m_eLinearUnits

            Case NumberFormatting.LinearUnits.inch
                m_eAreaUnits = NumberFormatting.AreaUnits.sqin

            Case NumberFormatting.LinearUnits.ft
                m_eAreaUnits = NumberFormatting.AreaUnits.sqft

            Case NumberFormatting.LinearUnits.yard
                m_eAreaUnits = NumberFormatting.AreaUnits.sqyd

            Case NumberFormatting.LinearUnits.mile
                m_eAreaUnits = NumberFormatting.AreaUnits.sqmi

            Case NumberFormatting.LinearUnits.mm
                m_eAreaUnits = NumberFormatting.AreaUnits.sqmm

            Case NumberFormatting.LinearUnits.cm
                m_eAreaUnits = NumberFormatting.AreaUnits.sqcm

            Case NumberFormatting.LinearUnits.m
                m_eAreaUnits = NumberFormatting.AreaUnits.sqm

            Case NumberFormatting.LinearUnits.km
                m_eAreaUnits = NumberFormatting.AreaUnits.sqkm

            Case Else
                m_eAreaUnits = NumberFormatting.AreaUnits.sqm

        End Select

        Select Case m_eLinearUnits
            Case NumberFormatting.LinearUnits.inch
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.inch3

            Case NumberFormatting.LinearUnits.ft
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.feet3

            Case NumberFormatting.LinearUnits.yard
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.yard3

            Case NumberFormatting.LinearUnits.mile
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.mi3

            Case NumberFormatting.LinearUnits.mm
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.mm3

            Case NumberFormatting.LinearUnits.cm
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.cm3

            Case NumberFormatting.LinearUnits.m
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.m3

            Case NumberFormatting.LinearUnits.km
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.km3

            Case Else 'catch
                m_eVolumetricUnits = NumberFormatting.VolumeUnits.m3

        End Select

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
