Imports System.Xml

Namespace GISCode.GCD.ChangeDetection

    Public Class StatsDataClass

#Region "Members"

        Private m_fAreaErosion_Raw As Double
        Private m_fAreaDeposition_Raw As Double
        Private m_fAreaErosion_Thresholded As Double
        Private m_fAreaDeposition_Thresholded As Double

        Private m_fVolumeErosion_Raw As Double
        Private m_fVolumeDeposition_Raw As Double
        Private m_fVolumeErosion_Thresholded As Double
        Private m_fVolumeDeposition_Thresholded As Double

        Private m_fVolumeErosion_Error As Double
        Private m_fVolumeDeposition_Error As Double

        'Private m_fVolumeOfDifference_Error As Double
        'Private m_fNetVolumeOfDifference_Error As Double

        'histograms data
        Public m_RawAreaHist = New Dictionary(Of Double, Double)
        Public m_RawVolumeHist = New Dictionary(Of Double, Double)
        Public m_ThresholdedAreaHist = New Dictionary(Of Double, Double)
        Public m_ThresholdedVolumeHist = New Dictionary(Of Double, Double)
        Public m_MinAreaHist = New Dictionary(Of Double, Double)
        Public m_MinVolumeHist = New Dictionary(Of Double, Double)

        'Private MinErosionVolume As Double = 0
        'Private MinDepositionVolume As Double = 0

        Private m_sPrecisionFormatString As String = "#,##0.00"

#End Region

#Region "Properties"

#Region "Areal Properties"
        Public ReadOnly Property AreaErosion_Raw As Double
            Get
                Return m_fAreaErosion_Raw
            End Get
        End Property

        Public ReadOnly Property AreaDeposition_Raw As Double
            Get
                Return m_fAreaDeposition_Raw
            End Get
        End Property

        Public Property AreaErosion_Thresholded As Double
            Get
                Return m_fAreaErosion_Thresholded
            End Get
            Set(value As Double)
                m_fAreaErosion_Thresholded = value
            End Set
        End Property

        Public Property AreaDeposition_Thresholded As Double
            Get
                Return m_fAreaDeposition_Thresholded
            End Get
            Set(value As Double)
                m_fAreaDeposition_Thresholded = value
            End Set
        End Property

        Public ReadOnly Property AreaDetectableChange_Thresholded As Double
            Get
                Return (AreaErosion_Thresholded + AreaDeposition_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AreaOfInterest_Raw As Double
            Get
                Return (AreaErosion_Raw + AreaDeposition_Raw)
            End Get
        End Property

        Public ReadOnly Property AreaPercentAreaInterestWithDetectableChange As Double
            Get
                Return 100 * (AreaDetectableChange_Thresholded / AreaOfInterest_Raw)
            End Get
        End Property
#End Region

#Region "Volumetric Properties"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public ReadOnly Property VolumeErosion_Raw As Double
            Get
                Return m_fVolumeErosion_Raw
            End Get
        End Property

        Public ReadOnly Property VolumeDeposition_Raw As Double
            Get
                Return m_fVolumeDeposition_Raw
            End Get
        End Property

        Public Property VolumeErosion_Thresholded As Double
            Get
                Return m_fVolumeErosion_Thresholded
            End Get
            Set(value As Double)
                m_fVolumeErosion_Thresholded = value
            End Set
        End Property

        Public Property VolumeDeposition_Thresholded As Double
            Get
                Return m_fVolumeDeposition_Thresholded
            End Get
            Set(value As Double)
                m_fVolumeDeposition_Thresholded = value
            End Set
        End Property

        Public Property VolumeErosion_Error As Double
            Get
                Return m_fVolumeErosion_Error
            End Get
            Set(value As Double)
                m_fVolumeErosion_Error = value
            End Set
        End Property

        Public Property VolumeDeposition_Error As Double
            Get
                Return m_fVolumeDeposition_Error
            End Get
            Set(value As Double)
                m_fVolumeDeposition_Error = value
            End Set
        End Property

        Public ReadOnly Property VolumeOfDifference_Raw As Double
            Get
                Return VolumeErosion_Raw + VolumeDeposition_Raw
            End Get
        End Property

        Public ReadOnly Property VolumeOfDifference_Thresholded As Double
            Get
                Return VolumeErosion_Thresholded + VolumeDeposition_Thresholded
            End Get
        End Property

        Public ReadOnly Property VolumeOfDifference_Error As Double
            Get
                Return (VolumeDeposition_Error + VolumeErosion_Error)
            End Get
        End Property

        Public ReadOnly Property NetVolumeOfDifference_Raw As Double
            Get
                Return (VolumeDeposition_Raw - VolumeErosion_Raw)
            End Get
        End Property

        Public ReadOnly Property NetVolumeOfDifference_Thresholded As Double
            Get
                Return (VolumeDeposition_Thresholded - VolumeErosion_Thresholded)
            End Get
        End Property

        Public ReadOnly Property NetVolumeOfDifference_Error As Double
            Get
                Return Math.Sqrt(Math.Pow(VolumeDeposition_Error, 2) + Math.Pow(VolumeErosion_Error, 2))
            End Get
        End Property

        Public ReadOnly Property VolumeOfErosion_Percent As Double
            Get
                Dim fResult As Double = 0
                If VolumeErosion_Error <> 0 AndAlso VolumeErosion_Thresholded <> 0 Then
                    fResult = 100 * (VolumeErosion_Error / VolumeErosion_Thresholded)
                End If
                Return fResult
            End Get
        End Property

        Public ReadOnly Property VolumeOfDeposition_Percent As Double
            Get
                Dim fResult As Double = 0
                If VolumeDeposition_Error <> 0 AndAlso VolumeDeposition_Thresholded <> 0 Then
                    fResult = 100 * (VolumeDeposition_Error / VolumeDeposition_Thresholded)
                End If
                Return fResult
            End Get
        End Property

        Public ReadOnly Property VolumeOfDifference_Percent As Double
            Get
                Dim fResult As Double = 0
                If VolumeOfDifference_Error <> 0 AndAlso VolumeOfDifference_Thresholded <> 0 Then
                    fResult = 100 * (VolumeOfDifference_Error / VolumeOfDifference_Thresholded)
                End If
                Return fResult
            End Get
        End Property

        Public ReadOnly Property NetVolumeOfDifference_Percent As Double
            Get
                Dim fResult As Double = 0
                If NetVolumeOfDifference_Error <> 0 AndAlso NetVolumeOfDifference_Thresholded <> 0 Then
                    fResult = 100 * (NetVolumeOfDifference_Error / NetVolumeOfDifference_Thresholded)
                End If
                Return fResult
            End Get
        End Property

#End Region

#Region "Vertical Averages Erosion"

        Public ReadOnly Property AverageDepthErosion_Raw As Double
            Get
                Return SafeDivision(AreaErosion_Raw, VolumeErosion_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageDepthErosion_Thresholded As Double
            Get
                Return SafeDivision(VolumeErosion_Thresholded, AreaErosion_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageDepthErosion_Error As Double
            Get
                Return SafeDivision(VolumeErosion_Error, AreaErosion_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageDepthErosion_Percent As Double
            Get
                Return 100 * SafeDivision(AverageDepthErosion_Error, AverageDepthErosion_Thresholded)
            End Get
        End Property

#End Region

#Region "Vertical Averages Deposition"

        Public ReadOnly Property AverageDepthDeposition_Raw As Double
            Get
                Return SafeDivision(AreaDeposition_Raw, VolumeDeposition_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageDepthDeposition_Thresholded As Double
            Get
                Return SafeDivision(VolumeDeposition_Thresholded, AreaDeposition_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageDepthDeposition_Error As Double
            Get
                Return SafeDivision(VolumeDeposition_Error, AreaDeposition_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageDepthDeposition_Percent As Double
            Get
                Return 100 * SafeDivision(AverageDepthDeposition_Error, AverageDepthDeposition_Thresholded)
            End Get
        End Property

#End Region


#Region "Vertical Averages Total Thickness of Difference for AOI"

        Public ReadOnly Property AverageThicknessOfDifferenceAOI_Raw As Double
            Get
                Return SafeDivision(VolumeOfDifference_Raw, AreaOfInterest_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageThicknessOfDifferenceAOI_Thresholded As Double
            Get
                Return SafeDivision(VolumeOfDifference_Thresholded, AreaOfInterest_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageThicknessOfDifferenceAOI_Error As Double
            Get
                Return SafeDivision(VolumeOfDifference_Error, AreaOfInterest_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageThicknessOfDifferenceAOI_Percent As Double
            Get
                Return 100 * SafeDivision(AverageThicknessOfDifferenceAOI_Error, AverageThicknessOfDifferenceAOI_Thresholded)
            End Get
        End Property

#End Region

#Region "Vertical Averages **NET** Thickness of Difference for Area of Interest (AOI)"

        Public ReadOnly Property AverageNetThicknessofDifferenceAOI_Raw As Double
            Get
                Return SafeDivision(NetVolumeOfDifference_Raw, AreaOfInterest_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageNetThicknessOfDifferenceAOI_Thresholded As Double
            Get
                Return SafeDivision(NetVolumeOfDifference_Thresholded, AreaOfInterest_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageNetThicknessOfDifferenceAOI_Error As Double
            Get
                Return SafeDivision(NetVolumeOfDifference_Error, AreaOfInterest_Raw)
            End Get
        End Property

        Public ReadOnly Property AverageNetThicknessOfDifferenceAOI_Percent As Double
            Get
                Return 100 * SafeDivision(AverageNetThicknessOfDifferenceAOI_Error, AverageNetThicknessOfDifferenceAOI_Thresholded)
            End Get
        End Property

#End Region

#Region "Vertical Averages **Total** Thickness of Difference for Area of Detectable Change (ADC)"

        Public ReadOnly Property AverageThicknessOfDifferenceADC_Thresholded As Double
            Get
                Return SafeDivision(VolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageThicknessOfDifferenceADC_Error As Double
            Get
                Return SafeDivision(VolumeOfDifference_Error, AreaDetectableChange_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageThicknessOfDifferenceADC_Percent As Double
            Get
                Return 100 * SafeDivision(AverageThicknessOfDifferenceADC_Error, AverageThicknessOfDifferenceADC_Thresholded)
            End Get
        End Property

#End Region

#Region "Vertical Averages **NET** Thickness of Difference for Area of Detecktable Change (ADC)"

        Public ReadOnly Property AverageNetThicknessOfDifferenceADC_Thresholded As Double
            Get
                Return SafeDivision(NetVolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageNetThicknessOfDifferenceADC_Error As Double
            Get
                Return SafeDivision(NetVolumeOfDifference_Error, AreaDetectableChange_Thresholded)
            End Get
        End Property

        Public ReadOnly Property AverageNetThicknessOfDifferenceADC_Percent As Double
            Get
                Return 100 * SafeDivision(AverageNetThicknessOfDifferenceADC_Error, AverageNetThicknessOfDifferenceADC_Thresholded)
            End Get
        End Property

#End Region

#Region "Percentages By Volume"

        Public ReadOnly Property PercentErosion_Raw As Double
            Get
                Return 100 * SafeDivision(VolumeErosion_Raw, VolumeOfDifference_Raw)
            End Get
        End Property

        Public ReadOnly Property PercentErosion_Thresholded As Double
            Get
                Return 100 * SafeDivision(VolumeErosion_Thresholded, VolumeOfDifference_Thresholded)
            End Get
        End Property

        Public ReadOnly Property PercentDeposition_Raw As Double
            Get
                Return 100 * SafeDivision(VolumeDeposition_Raw, VolumeOfDifference_Raw)
            End Get
        End Property

        Public ReadOnly Property PercentDeposition_Thresholded As Double
            Get
                Return 100 * SafeDivision(VolumeDeposition_Thresholded, VolumeOfDifference_Thresholded)
            End Get
        End Property

        Public ReadOnly Property PercentImbalance_Raw As Double
            Get
                Return 100 * SafeDivision(NetVolumeOfDifference_Raw, (2 * VolumeOfDifference_Raw))
            End Get
        End Property

        Public ReadOnly Property PercentImbalance_Thresholded As Double
            Get
                Return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, (2 * VolumeOfDifference_Thresholded))
            End Get
        End Property

        Public ReadOnly Property NetToTotalVolumeRatio_Raw As Double
            Get
                Return 100 * SafeDivision(NetVolumeOfDifference_Raw, VolumeOfDifference_Raw)
            End Get
        End Property

        Public ReadOnly Property NetToTotalVolumeRatio_Thresholded As Double
            Get
                Return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, VolumeOfDifference_Thresholded)
            End Get
        End Property

#End Region

#End Region

        Private Function SafeDivision(fNumerator As Double, fDenominator As Double) As Double
            Dim fResult As Double = 0
            If fNumerator <> 0 AndAlso fDenominator <> 0 Then
                fResult = fNumerator / fDenominator
            End If
            Return fResult
        End Function

        Public Sub New()

        End Sub

        Public Sub New(ByVal sThresholdedHist As String)
            HistogramStats.LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)
            CalculateStatistics()
        End Sub

        Public Sub New(ByVal sRawHist As String, ByVal sThresholdedHist As String)
            HistogramStats.LoadHistogram(sRawHist, m_RawAreaHist, m_RawVolumeHist)
            HistogramStats.LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)
            CalculateStatistics()
        End Sub

        Public Sub New(ByVal sRawHist As String, ByVal sThresholdedHist As String, ByVal sMinHist As String, sRawDoD As String, fThreshold As Double, sThresholdDod As String, fCellSize As Double, Optional sPropagatedError As String = "")
            HistogramStats.LoadHistogram(sRawHist, m_RawAreaHist, m_RawVolumeHist)
            HistogramStats.LoadHistogram(sThresholdedHist, m_ThresholdedAreaHist, m_ThresholdedVolumeHist)

            If IO.File.Exists(sMinHist) Then
                HistogramStats.LoadHistogram(sMinHist, m_MinAreaHist, m_MinVolumeHist)
            End If
            CalculateStatisticsWithPlusMinusError(sRawDoD, fThreshold, sThresholdDod, fCellSize, sPropagatedError)
        End Sub

        Public Sub CalculateStatistics()

            m_fAreaErosion_Raw = 0
            m_fAreaDeposition_Raw = 0
            m_fAreaErosion_Thresholded = 0
            m_fAreaDeposition_Thresholded = 0

            m_fVolumeErosion_Raw = 0
            m_fVolumeDeposition_Raw = 0
            m_fVolumeErosion_Thresholded = 0
            m_fVolumeDeposition_Thresholded = 0

            'TotalRawVolumeOfDifference = 0
            'TotalRawNetVolumeDifference = 0
            'TotalThresholdedVolumeOfDifference = 0
            'TotalThresholdedNetVolumeDifference = 0

            Dim elevation As Double
            Dim area As Double
            Dim volume As Double

            'Calculate raw total area
            For Each kvp As KeyValuePair(Of Double, Double) In m_RawAreaHist
                elevation = kvp.Key
                area = kvp.Value
                If elevation < 0 Then
                    m_fAreaErosion_Raw += area
                Else
                    m_fAreaDeposition_Raw += area
                End If

            Next

            'Calculate thresholded total area
            For Each kvp As KeyValuePair(Of Double, Double) In m_ThresholdedAreaHist
                elevation = kvp.Key
                area = kvp.Value
                If elevation < 0 Then
                    m_fAreaErosion_Thresholded += area
                Else
                    m_fAreaDeposition_Thresholded += area
                End If

            Next

            'Calculate raw total volume
            For Each kvp As KeyValuePair(Of Double, Double) In m_RawVolumeHist
                elevation = kvp.Key
                volume = kvp.Value
                If elevation < 0 Then
                    m_fVolumeErosion_Raw += volume
                Else
                    m_fVolumeDeposition_Raw += volume
                End If
            Next

            'Calculate thresholded total volume
            For Each kvp As KeyValuePair(Of Double, Double) In m_ThresholdedVolumeHist
                elevation = kvp.Key
                volume = kvp.Value
                If elevation < 0 Then
                    m_fVolumeErosion_Thresholded += volume
                Else
                    m_fVolumeDeposition_Thresholded += volume
                End If
            Next



            'Calculate thresholded total volume
            'For Each kvp As KeyValuePair(Of Double, Double) In m_MinVolumeHist
            '    elevation = kvp.Key
            '    volume = kvp.Value
            '    If elevation < 0 Then
            '        MinErosionVolume += volume
            '    Else
            '        MinDepositionVolume += volume
            '    End If
            'Next

        End Sub

        Private Sub CalculateStatisticsWithPlusMinusError(sRawDoD As String, fThreshold As Double, sThresholdDod As String, fCellSize As Double, Optional sPropagatedError As String = "")

            CalculateStatistics()

            Try
                If String.IsNullOrEmpty(sPropagatedError) Then
                    ' Simple MinLOD uses the threshold to CON the DoD to find the volumetric plus minus error
                    m_fVolumeErosion_Error = CalculatePlusMinusVolumetricError_SimpleMinLOD(sRawDoD, fThreshold, fCellSize, True)
                    m_fVolumeDeposition_Error = CalculatePlusMinusVolumetricError_SimpleMinLOD(sRawDoD, fThreshold, fCellSize, False)
                Else
                    ' Propagated and probalistic use the propagated error raster to find the plus minus error
                    CalculatePlusMinusVolumetricError_Propagated(sThresholdDod, sPropagatedError, fCellSize)
                End If
            Catch ex As Exception
                ex.Data("Raw DoD") = sRawDoD
                ex.Data("Thresholded DoD") = sThresholdDod
                ex.Data("Cell Size") = fCellSize.ToString
                Throw
            End Try

        End Sub

        ''' <summary>
        ''' Output the statistics to an XML text writer.
        ''' </summary>
        ''' <param name="xmlResults">XML text writer</param>
        ''' <remarks>This method will only output the actual value XML elements. It's assumed that other code
        ''' is responsible for surrounding these methods with the appropriate tags.</remarks>
        Public Sub OutputCHaMPResults(xmlResults As XmlTextWriter)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' AREAS
            xmlResults.WriteElementString("raw_area_erosion", AreaErosion_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("raw_area_deposition", AreaDeposition_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_area_erosion", AreaErosion_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_area_deposition", AreaDeposition_Thresholded.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("area_detectable_change", AreaDetectableChange_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("area_of_interest_raw", AreaOfInterest_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("percent_area_of_interest_detectable_change", AreaPercentAreaInterestWithDetectableChange.ToString(m_sPrecisionFormatString))

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' VOLUMES
            xmlResults.WriteElementString("raw_volume_erosion", VolumeErosion_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_volume_erosion", VolumeErosion_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("error_volume_erosion", VolumeErosion_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_percent_erosion", VolumeOfErosion_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("raw_volume_deposition", VolumeDeposition_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_volume_deposition", VolumeDeposition_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("error_volume_deposition", VolumeDeposition_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_percent_deposition", VolumeOfDeposition_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("raw_volume_difference", VolumeOfDifference_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("thresholded_volume_difference", VolumeOfDifference_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("error_volume_difference", VolumeOfDifference_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("volume_difference_percent", VolumeOfDifference_Percent.ToString(m_sPrecisionFormatString))

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' VERTICAL AVERAGES
            xmlResults.WriteElementString("average_depth_erosion_raw", AverageDepthErosion_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_depth_erosion_thresholded", AverageDepthErosion_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_depth_erosion_error", AverageDepthErosion_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_depth_erosion_percent", AverageDepthErosion_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("average_depth_deposition_raw", AverageDepthDeposition_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_depth_deposition_thresholded", AverageDepthDeposition_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_depth_deposition_error", AverageDepthDeposition_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_depth_deposition_percent", AverageDepthDeposition_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("average_thickness_difference_aoi_raw", AverageThicknessOfDifferenceAOI_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_thickness_difference_aoi_thresholded", AverageThicknessOfDifferenceAOI_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_thickness_difference_aoi_error", AverageThicknessOfDifferenceAOI_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_thickness_difference_aoi_percent", AverageThicknessOfDifferenceAOI_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("average_net_thickness_difference_aoi_raw", AverageNetThicknessofDifferenceAOI_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_net_thickness_difference_aoi_thresholded", AverageNetThicknessOfDifferenceAOI_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_net_thickness_difference_aoi_error", AverageNetThicknessOfDifferenceAOI_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_net_thickness_difference_aoi_percent", AverageNetThicknessOfDifferenceAOI_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("average_thickness_difference_adc_thresholded", AverageThicknessOfDifferenceADC_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_thickness_difference_adc_error", AverageThicknessOfDifferenceADC_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_thickness_difference_adc_percent", AverageThicknessOfDifferenceADC_Percent.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("average_net_thickness_difference_adc_thresholded", AverageNetThicknessOfDifferenceADC_Thresholded.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_net_thickness_difference_adc_error", AverageNetThicknessOfDifferenceADC_Error.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("average_net_thickness_difference_adc_percent", AverageNetThicknessOfDifferenceADC_Percent.ToString(m_sPrecisionFormatString))

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' PERCENTAGES
            xmlResults.WriteElementString("percent_erosion_raw", PercentErosion_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("percent_erosion_thresholded", PercentErosion_Thresholded.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("percent_deposition_raw", PercentDeposition_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("percent_deposition_thresholded", PercentDeposition_Thresholded.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("percent_imbalance_raw", PercentImbalance_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("percent_imbalance_thresholded", PercentImbalance_Thresholded.ToString(m_sPrecisionFormatString))

            xmlResults.WriteElementString("percent_net_volume_ratio_raw", NetToTotalVolumeRatio_Raw.ToString(m_sPrecisionFormatString))
            xmlResults.WriteElementString("percent_net_volume_ratio_thresholded", NetToTotalVolumeRatio_Thresholded.ToString(m_sPrecisionFormatString))

        End Sub

        ''' <summary>
        ''' Write the statistics to a new copy of the GCDSummary.xml spreadsheet XML file.
        ''' </summary>
        ''' <param name="sDisplayUnits">Units of measurement for the statistics. e.g. "ft"</param>
        ''' <param name="sOutputPath">Full path to the output XML path where the GCD Summary stats XML file should be generated</param>
        ''' <remarks></remarks>
        Public Sub ExportSummary(sExcelTemplateFolder As String, ByVal sDisplayUnits As String, ByVal sOutputPath As String)

            ' Get the template file
            '            Dim sRootFolder As String
            '#If DEBUG Then
            '            sRootFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            '#Else
            '            sRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            '            sRootFolder = IO.Path.Combine(sRootFolder, "GCD")
            '#End If
            'Dim ExcelTemplateFolder As String = GCDProject.ProjectManager.ExcelTemplatesFolder.FullName ' IO.Path.Combine(sRootFolder, "GISCode\GCD\ExcelTemplates")
            Dim TemplateFile As String = IO.Path.Combine(sExcelTemplateFolder, "GCDSummary.xml")
            If Not IO.File.Exists(TemplateFile) Then
                Throw New Exception("Could not find required template '" & TemplateFile & "'")
            End If

            ' Read the template file
            Dim objReader As System.IO.StreamReader
            Try
                objReader = New System.IO.StreamReader(TemplateFile)
            Catch ex As System.IO.IOException
                'need to limit the length of the file to display properly
                Dim TrimmedFilename As String = GISCode.FileSystem.TrimFilename(TemplateFile, 80)
                ex.Data("UIMessage") = "Could not access the file '" & TrimmedFilename & "' because it is being used by another program."
                Throw
            End Try

            Dim OutputText As Text.StringBuilder = New Text.StringBuilder(objReader.ReadToEnd())
            objReader.Close()
            '
            ' PGB 3 Mar 2012
            ' Need to dynamically put the units in the summary XML.
            '
            OutputText.Replace("[LinearUnits]", sDisplayUnits)

            OutputText.Replace("[TotalAreaOfErosionRaw]", AreaErosion_Raw)
            OutputText.Replace("[TotalAreaOfErosionThresholded]", AreaErosion_Thresholded)

            OutputText.Replace("[TotalAreaOfDepositionRaw]", AreaDeposition_Raw)
            OutputText.Replace("[TotalAreaOfDepositionThresholded]", AreaDeposition_Thresholded)

            OutputText.Replace("[TotalVolumeOfErosionRaw]", VolumeErosion_Raw)
            OutputText.Replace("[TotalVolumeOfErosionThresholded]", m_fVolumeErosion_Thresholded)
            OutputText.Replace("[ErrorVolumeOfErosion]", VolumeErosion_Error)

            OutputText.Replace("[TotalVolumeOfDepositionRaw]", VolumeDeposition_Raw)
            OutputText.Replace("[TotalVolumeOfDepositionThresholded]", m_fVolumeDeposition_Thresholded)
            OutputText.Replace("[ErrorVolumeOfDeposition]", VolumeDeposition_Error)

            'OutputText.Replace("[TotalRawVolumeOfDifference]", VolumeOfDifference_Raw)
            'OutputText.Replace("[TotalThresholdedVolumeOfDifference]", VolumeOfDifference_Thresholded)

            'OutputText.Replace("[TotalRawNetVolumeDifference]", NetVolumeOfDifference_Raw)
            'OutputText.Replace("[TotalThresholdedNetVolumeDifference]", NetVolumeOfDifference_Thresholded)

            'OutputText.Replace("[RawPercentErosion]", Math.Round(PercentErosion_Raw))
            'OutputText.Replace("[ThresholdedPercentErosion]", Math.Round(PercentErosion_Thresholded))

            'OutputText.Replace("[RawPercentDeposition]", Math.Round(PercentDeposition_Raw))
            'OutputText.Replace("[ThresholdedPercentDeposition]", Math.Round(PercentDeposition_Thresholded))

            'OutputText.Replace("[RawPercentImbalance]", Math.Round(PercentImbalance_Raw))
            'OutputText.Replace("[ThresholdedPercentImbalance]", Math.Round(PercentImbalance_Thresholded))

            'OutputText.Replace("[ErrorVolumeOfDifference]", VolumeOfDifference_Error)
            'OutputText.Replace("[ErrorNetVolumeOfDifference]", NetVolumeOfDifference_Error)

            ' Write the template file
            Dim objWriter As New System.IO.StreamWriter(sOutputPath)
            objWriter.Write(OutputText)
            objWriter.Close()

        End Sub

        ''' <summary>
        ''' Temporary method.
        ''' </summary>
        ''' <param name="sRawDoD"></param>
        ''' <param name="fThreshold"></param>
        ''' <param name="fCellResolution"></param>
        ''' <param name="bErosion"></param>
        ''' <returns></returns>
        ''' <remarks>PGB 30 Apr 2012. Temporary hack to calculate plus minus values for simple DoD. Written to fix Frank's, uncommented code.</remarks>
        Private Function CalculatePlusMinusVolumetricError_SimpleMinLOD(sRawDoD As String, fThreshold As Double, fCellResolution As Double, bErosion As Boolean) As Double

            Dim fResult As Double = 0

            Try
                '
                ' Calculate the erosion 
                Dim sMask As String = WorkspaceManager.GetTempRaster("Mask")
                Dim sDirection As String
                Dim nSign As Integer = 1

                If bErosion Then
                    sDirection = "<"
                    nSign = -1
                Else
                    sDirection = ">"
                End If

                GP.SpatialAnalyst.Con(sRawDoD, "1", "", sMask, "Value " & sDirection & " " & (fThreshold * nSign))
                ' deposition threshold: Dod VALUE > Threshold
                ' erosion threshold: Dod VALUE < Threshold * -1

                '
                ' Run zonal statistics on this mask to get the sum of all pixel values in the masked area.
                '
                Dim pWFS As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.ShapeFile)
                Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IWorkspace = pWFS.OpenFromFile(WorkspaceManager.WorkspacePath, Nothing)
                Dim sErosionTable As String = GISCode.Table.GetSafeName(pWorkspace, "MaskVals")
                GISCode.GP.SpatialAnalyst.ZonalStatisticsAsTable(sMask, "Value", sRawDoD, sErosionTable, "SUM")

                Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.Table.OpenDBFTable(sErosionTable)
                Dim nCountField As Integer = pTable.FindField("COUNT")

                Dim nCount As Integer
                Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor = pTable.Search(Nothing, True)
                Dim aRow As ESRI.ArcGIS.Geodatabase.IRow = pCursor.NextRow
                If TypeOf aRow Is ESRI.ArcGIS.Geodatabase.IRow Then
                    If Not IsDBNull(aRow.Value(nCountField)) Then
                        nCount = aRow.Value(nCountField)
                        fResult = (fCellResolution ^ 2) * CType(nCount, Double) * Math.Abs(fThreshold)
                    End If
                End If
                Runtime.InteropServices.Marshal.ReleaseComObject(pCursor)

            Catch ex As Exception
                ex.Data("Raw DoD") = sRawDoD
                ex.Data("Threshold") = fThreshold.ToString
                ex.Data("Cell Resolution") = fCellResolution.ToString
                ex.Data("Erosion or Deposition") = bErosion.ToString
                Throw
            End Try

            Return fResult
        End Function

        Private Sub CalculatePlusMinusVolumetricError_Propagated(sThresholdedDoD As String, sPropagatedError As String, fCellSize As Double)

            Try
                If Not GISDataStructures.RasterDirect.Exists(sThresholdedDoD) Then
                    Dim ex As New Exception("The thresholded DoD raster does not exist")
                    Throw ex
                End If

                If Not GISDataStructures.RasterDirect.Exists(sPropagatedError) Then
                    Dim ex As New Exception("The propagated error raster does not exist")
                    Throw ex
                End If

                Dim sPropErrorErosion As String = WorkspaceManager.GetTempRaster("PropErrErosion")
                GP.SpatialAnalyst.Con(sThresholdedDoD, "1", "0", sPropErrorErosion, """Value"" < 0")

                Dim sPropErrorDeposition As String = WorkspaceManager.GetTempRaster("PropErrDeposition")
                GP.SpatialAnalyst.Con(sThresholdedDoD, "2", "0", sPropErrorDeposition, """Value"" > 0")

                Dim sMask1 As String = WorkspaceManager.GetTempRaster("ErosionDepositionMask1")
                GP.SpatialAnalyst.Raster_Calculator("(""" & sPropErrorErosion & """) + (""" & sPropErrorDeposition & """)", sMask1)

                Dim sMask2 As String = WorkspaceManager.GetTempRaster("ErosionDepositionMask2")
                GP.SpatialAnalyst.SetNull(sMask1, sMask1, sMask2, """Value"" = 0")

                Dim gMask2 As New GISDataStructures.Raster(sMask2)

                Dim pWSF As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory
                Dim pWS As ESRI.ArcGIS.Geodatabase.IWorkspace = pWSF.OpenFromFile(WorkspaceManager.WorkspacePath, Nothing)

                Dim sStatistics As String = GISCode.Table.GetSafeName(pWS, "Stats")
                GP.SpatialAnalyst.ZonalStatisticsAsTable(sMask2, "Value", sPropagatedError, sStatistics, "ALL")
                Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.OpenDBFTable(sStatistics)

                Dim dicSum As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "SUM")
                Dim dicArea As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "AREA")

                If dicSum.ContainsKey(1) AndAlso dicArea.ContainsKey(1) Then
                    m_fVolumeErosion_Error = dicSum(1) * (fCellSize ^ 2) ' dicArea(1)
                End If

                If dicSum.ContainsKey(2) AndAlso dicArea.ContainsKey(2) Then
                    m_fVolumeDeposition_Error = dicSum(2) * (fCellSize ^ 2)  ' dicArea(2)
                End If

            Catch ex As Exception
                ex.Data("Thresholded DoD") = sThresholdedDoD
                ex.Data("Propagated Error") = sPropagatedError
                Throw
            End Try
        End Sub

    End Class
End Namespace