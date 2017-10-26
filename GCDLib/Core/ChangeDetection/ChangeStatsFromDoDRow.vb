Namespace Core.ChangeDetection

    ''' <summary>
    ''' Loads change detection statistics from the project dataset
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChangeStatsFromDoDRow
        Inherits ChangeStats

        Private m_rDoD As ProjectDS.DoDsRow

        ''' <summary>
        ''' Create GCD statistics from a GCD ProjectDS dataset row in the DoD table
        ''' </summary>
        ''' <param name="rDod">DoD row from GCD ProjectDS dataset</param>
        ''' <remarks>Only set the primary values. All the other statistics are derived
        ''' from these.</remarks>
        Public Sub New(rDod As ProjectDS.DoDsRow)

            m_rDoD = rDod
            CellArea = rDod.CellArea

            AreaErosion_Raw = rDod.AreaErosionRaw
            AreaDeposition_Raw = rDod.AreaDepositonRaw
            AreaErosion_Thresholded = rDod.AreaErosionThresholded
            AreaDeposition_Thresholded = rDod.AreaDepositionThresholded

            VolumeErosion_Raw = rDod.VolumeErosionRaw
            VolumeDeposition_Raw = rDod.VolumeDepositionRaw
            VolumeErosion_Thresholded = rDod.VolumeErosionThresholded
            VolumeDeposition_Thresholded = rDod.VolumeDepositionThresholded

            VolumeErosion_Error = rDod.VolumeErosionError
            VolumeDeposition_Error = rDod.VolumeDepositionError

        End Sub

    End Class

    Public Class ChangeStatsFromBSMaskRow
        Inherits ChangeStats

        Private m_rMaskRow As ProjectDS.BSMasksRow

        Public ReadOnly Property BSMaskRow As ProjectDS.BSMasksRow
            Get
                Return m_rMaskRow
            End Get
        End Property

        Public Sub New(rBSMaskRow As ProjectDS.BSMasksRow)
            m_rMaskRow = rBSMaskRow
            CellArea = m_rMaskRow.BudgetSegregationsRow.DoDsRow.CellArea

            AreaErosion_Raw = m_rMaskRow.AreaErosionRaw
            AreaDeposition_Raw = m_rMaskRow.AreaDepositionRaw
            AreaErosion_Thresholded = m_rMaskRow.AreaErosionThresholded
            AreaDeposition_Thresholded = m_rMaskRow.AreaDepositionThresholded

            VolumeErosion_Raw = m_rMaskRow.VolumeErosionRaw
            VolumeDeposition_Raw = m_rMaskRow.VolumeDepositionRaw
            VolumeErosion_Thresholded = m_rMaskRow.VolumeErosionThresholded
            VolumeDeposition_Thresholded = m_rMaskRow.VolumeDepositionThresholded

            VolumeErosion_Error = m_rMaskRow.VolumeErosionError
            VolumeDeposition_Error = m_rMaskRow.VolumeDepositionError
        End Sub

    End Class

End Namespace