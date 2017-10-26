Imports GCDLib.Core.Visualization

Namespace UI.ChangeDetection

    Public Class ucDoDHistogram

        Private m_HistogramViewer As DoDHistogramViewerClass

        Private Sub rdoVolume_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoVolume.CheckedChanged
            m_HistogramViewer.SetChartType(rdoArea.Checked)
        End Sub

        Public Sub SetHistogramUnits(ByRef options As DoDSummaryDisplayOptions)
            m_HistogramViewer.RefreshDisplay(rdoArea.Checked, options.LinearUnits, options.AreaUnits, options.VolumeUnits)
        End Sub

        Public Sub LoadHistograms(rawHistogram As IO.FileInfo, thrHistogram As IO.FileInfo, linearDataUnits As UnitsNet.Units.LengthUnit)
            m_HistogramViewer = New DoDHistogramViewerClass(rawHistogram.FullName, thrHistogram.FullName, linearDataUnits)
        End Sub

    End Class

End Namespace