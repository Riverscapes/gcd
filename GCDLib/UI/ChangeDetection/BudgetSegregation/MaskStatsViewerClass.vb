Imports System.Windows.Forms
Imports System.Drawing

Public Class MaskStatsViewerClass
    Private _grdDetails As DataGridView
    Private _MaskStats As GCD.BudgetSegregation.MaskStatsClass
    Private _MaskName As String
    Private _Units As String

    Public Property MaskName As String
        Get
            Return _MaskName
        End Get
        Set(value As String)
            _MaskName = value
        End Set
    End Property

    Public Sub New(ByVal grdDetails As DataGridView, ByVal MaskStats As GCD.BudgetSegregation.MaskStatsClass, ByVal units As String)
        _grdDetails = grdDetails
        _MaskStats = MaskStats
        _Units = units
    End Sub

    Public Sub refresh()
        _grdDetails.Rows.Clear()
        If _MaskName = "" Then
            Exit Sub
        End If

        Dim fValue As Double
        Dim aRow As DataGridViewRow
        Dim nIndex As Integer
        Dim cell As DataGridViewCell
        Dim stats As GISCode.GCD.ChangeDetection.StatsDataClass = _MaskStats.MaskStats(_MaskName)

        'Area of erosion header
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "AREAL:"
        cell = aRow.Cells(0)
        cell.Style.Font = New Font(_grdDetails.Font, FontStyle.Bold) 'text should be bold

        'Area of erosion
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Total Area of Erosion (" & _Units & "2)"
        aRow.Cells(1).Value = stats.AreaErosion_Thresholded

        fValue = Math.Round(stats.AreaErosion_Thresholded / _MaskStats.TotalStats.AreaErosion_Thresholded * 100)
        If Not (Double.IsNaN(fValue) OrElse Double.IsInfinity(fValue)) Then
            aRow.Cells(2).Value = fValue & "%"
        End If

        'Area of deposition
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Total Area of Deposition (" & _Units & "2)"
        aRow.Cells(1).Value = stats.AreaDeposition_Thresholded
        WritePercentValueRowToGrid(aRow, 2, stats.AreaDeposition_Thresholded, _MaskStats.TotalStats.AreaDeposition_Thresholded)

        'volume of erosion header
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "VOLUMETRIC:"
        cell = aRow.Cells(0)
        cell.Style.Font = New Font(_grdDetails.Font, FontStyle.Bold) 'text should be bold

        'Volume of erosion
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Total Volume of Erosion (" & _Units & "3)"
        aRow.Cells(1).Value = stats.VolumeErosion_Thresholded
        WritePercentValueRowToGrid(aRow, 2, stats.VolumeErosion_Thresholded, _MaskStats.TotalStats.VolumeErosion_Thresholded)

        'Volume of deposition
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Total Volume of Deposition (" & _Units & "3)"
        aRow.Cells(1).Value = stats.VolumeDeposition_Thresholded
        WritePercentValueRowToGrid(aRow, 2, stats.VolumeDeposition_Thresholded, _MaskStats.TotalStats.VolumeDeposition_Thresholded)

        'Total volume of difference
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Total Volume of Difference (" & _Units & "3)"
        aRow.Cells(1).Value = stats.VolumeOfDifference_Thresholded
        WritePercentValueRowToGrid(aRow, 2, stats.VolumeOfDifference_Thresholded, _MaskStats.TotalStats.VolumeOfDifference_Thresholded)

        'Total NET volume difference
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Total Net Volume Difference (" & _Units & "3)"
        aRow.Cells(1).Value = stats.NetVolumeOfDifference_Thresholded
        WritePercentValueRowToGrid(aRow, 2, stats.NetVolumeOfDifference_Thresholded, _MaskStats.TotalStats.NetVolumeOfDifference_Thresholded)

        'volume of erosion header
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "PERCENTAGES (BY VOLUME)"
        cell = aRow.Cells(0)
        cell.Style.Font = New Font(_grdDetails.Font, FontStyle.Bold) 'text should be bold

        'Percent Erosion
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Percent Erosion"
        aRow.Cells(1).Value = Math.Round(stats.PercentErosion_Thresholded) & "%"
        'aRow.Cells(2).Value = Math.Round(Stats.ThresholdedPercentErosion * 100) & "%"

        'Percent Deposition
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Percent Deposition"
        aRow.Cells(1).Value = Math.Round(stats.PercentDeposition_Thresholded) & "%"
        'aRow.Cells(2).Value = Math.Round(Stats.ThresholdedPercentDeposition * 100) & "%"

        'Percent Imbalance (departure from equilibrium)
        nIndex = _grdDetails.Rows.Add(1)
        aRow = _grdDetails.Rows(nIndex)
        aRow.Cells(0).Value = "Percent Imbalance (departure from equilibrium)"
        aRow.Cells(1).Value = Math.Round(stats.PercentImbalance_Thresholded) & "%"
        'aRow.Cells(2).Value = Math.Round(stats.ThresholdedPercentImbalance * 100) & "%"
    End Sub

    Private Sub WritePercentValueRowToGrid(ByRef aRow As DataGridViewRow, nColumn As Integer, fNumerator As Double, fDenominator As Double)

        If fNumerator <> 0 AndAlso fDenominator <> 0 Then
            aRow.Cells(nColumn).Value = Math.Round((fNumerator / fDenominator) * 100).ToString & "%"
        End If
    End Sub

  
End Class
