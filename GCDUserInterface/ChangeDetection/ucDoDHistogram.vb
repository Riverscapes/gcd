Imports GCDCore.Visualization

Namespace ChangeDetection

    Public Class ucDoDHistogram

        Private m_HistogramViewer As DoDHistogramViewerClass

        Private Sub rdoVolume_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoVolume.CheckedChanged
            m_HistogramViewer.SetChartType(rdoArea.Checked)
        End Sub

        Public Sub SetHistogramUnits(displayUnits As GCDConsoleLib.GCD.UnitGroup)
            m_HistogramViewer.UpdateDisplay(rdoArea.Checked, displayUnits)
        End Sub

        Public Sub LoadHistograms(rawHistogram As GCDConsoleLib.Histogram, thrHistogram As GCDConsoleLib.Histogram)
            m_HistogramViewer = New DoDHistogramViewerClass(rawHistogram, thrHistogram, ProjectManagerBase.Units)
        End Sub

    End Class

End Namespace