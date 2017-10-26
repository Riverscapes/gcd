Imports GCDLib.Core.ChangeDetection
Imports GCDLib.Core.Visualization
Imports GCDConsoleLib.Utility.Conversion

Namespace UI.ChangeDetection

    Public Class ucDoDHistogram

        Private m_DoDResultSet As DoDResultSet
        Private m_HistogramViewer As DoDHistogramViewerClass

        Private Sub rdoVolume_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoVolume.CheckedChanged
            m_HistogramViewer.Refresh(rdoArea.Checked)
        End Sub

        Public Sub SetHistogramUnits(ByRef options As DoDSummaryDisplayOptions)
            m_HistogramViewer.SetHistogramUnits(rdoArea.Checked, options.LinearUnits, options.AreaUnits, options.VolumeUnits)
        End Sub

        Public Sub LoadHistograms(sRawHistogram As String, sThresholdedHistogram As String, linearDataUnits As UnitsNet.Units.LengthUnit)
            m_HistogramViewer = New DoDHistogramViewerClass(sRawHistogram, sThresholdedHistogram, linearDataUnits)
        End Sub

    End Class

End Namespace