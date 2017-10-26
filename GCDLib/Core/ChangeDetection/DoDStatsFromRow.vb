Namespace Core.ChangeDetection

    ''' <summary>
    ''' Loads change detection statistics from the project dataset
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DoDStatsFromRow
        Inherits GCDConsoleLib.DoDStats

        ''' <summary>
        ''' Create GCD statistics from a GCD ProjectDS dataset row in the DoD table
        ''' </summary>
        ''' <param name="rDod">DoD row from GCD ProjectDS dataset</param>
        ''' <remarks>Only set the primary values. All the other statistics are derived
        ''' from these.</remarks>
        Public Sub New(rDoDRow As ProjectDS.DoDsRow)
            MyBase.New(rDoDRow.CellArea, rDoDRow.AreaErosionRaw, rDoDRow.AreaDepositonRaw,
            rDoDRow.AreaErosionThresholded, rDoDRow.AreaDepositionThresholded, rDoDRow.VolumeErosionRaw, rDoDRow.VolumeDepositionRaw,
            rDoDRow.VolumeErosionThresholded, rDoDRow.VolumeDepositionThresholded,
            rDoDRow.VolumeErosionError, rDoDRow.VolumeDepositionError)

        End Sub

        Public Sub New(rBSMaskRow As ProjectDS.BSMasksRow)
            MyBase.New(rBSMaskRow.BudgetSegregationsRow.DoDsRow.CellArea, rBSMaskRow.AreaErosionRaw, rBSMaskRow.AreaDepositionRaw,
            rBSMaskRow.AreaErosionThresholded, rBSMaskRow.AreaDepositionThresholded, rBSMaskRow.VolumeErosionRaw, rBSMaskRow.VolumeDepositionRaw,
            rBSMaskRow.VolumeErosionThresholded, rBSMaskRow.VolumeDepositionThresholded,
            rBSMaskRow.VolumeErosionError, rBSMaskRow.VolumeDepositionError)
        End Sub

    End Class

End Namespace