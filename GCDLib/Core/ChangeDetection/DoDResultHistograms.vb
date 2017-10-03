Namespace Core.ChangeDetection

    Public Class DoDResultHistograms

        'histograms data
        Public m_RawAreaHist = New Dictionary(Of Double, Double)
        Public m_RawVolumeHist = New Dictionary(Of Double, Double)
        Public m_ThresholdedAreaHist = New Dictionary(Of Double, Double)
        Public m_ThresholdedVolumeHist = New Dictionary(Of Double, Double)
        Public m_MinAreaHist = New Dictionary(Of Double, Double)
        Public m_MinVolumeHist = New Dictionary(Of Double, Double)

        ''' <summary>
        ''' Load a single histogram into the class
        ''' </summary>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sThresholdedHist As String)
            HistogramStats.LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)
        End Sub

        ''' <summary>
        ''' Load raw and thresholded histogram into the class
        ''' </summary>
        ''' <param name="sRawHist">Raw CSV Histogram</param>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sRawHist As String, ByVal sThresholdedHist As String)
            HistogramStats.LoadHistogram(sRawHist, m_RawAreaHist, m_RawVolumeHist)
            HistogramStats.LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sRawHist">Raw CSV Histogram</param>
        ''' <param name="sThresholdedHist">Thresholded CSV Histogram</param>
        ''' <param name="sMinHist"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sRawHist As String, ByVal sThresholdedHist As String, ByVal sMinHist As String)
            HistogramStats.LoadHistogram(sRawHist, m_RawAreaHist, m_RawVolumeHist)
            HistogramStats.LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)

            If IO.File.Exists(sMinHist) Then
                HistogramStats.LoadHistogram(sMinHist, m_MinAreaHist, m_MinVolumeHist)
            End If
        End Sub

    End Class

End Namespace