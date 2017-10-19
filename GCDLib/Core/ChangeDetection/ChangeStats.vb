Imports System.Xml
Imports GCDLib.Core.External.ExternalLibs

Namespace Core.ChangeDetection

    ''' <summary>
    ''' Class to hold change/DoD summary statistics.
    ''' </summary>
    ''' <remarks>Note that this base class holds the statistics and can load them
    ''' from file. But it cannot generate them. That is the role of the inherited
    ''' ChangeStatsCalculator classes (and it's child classes for each DoD type).
    ''' You need to inherit this class either using the calculator classes or the
    ''' class that can load these data from a DoD row in the project dataset.</remarks>
    Public MustInherit Class ChangeStats

        Private m_fCellArea As Double

        Private m_fAreaErosion_Raw As Double
        Private m_fAreaDeposition_Raw As Double
        Private m_fAreaErosion_Thresholded As Double
        Private m_fAreaDeposition_Thresholded As Double

        Private m_fVolumeErosion_Raw As Double
        Private m_fVolumeDeposition_Raw As Double
        Private m_fVolumeErosion_Thresholded As Double
        Private m_fVolumeDeposition_Thresholded As Double

        Protected m_fVolumeErosion_Error As Double
        Protected m_fVolumeDeposition_Error As Double

#Region "Properties"

        Public Property CellArea As Double
            Get
                Return m_fCellArea
            End Get
            Set(value As Double)
                If value <= 0 Then
                    Throw New ArgumentOutOfRangeException("value", "The cell area must be greater than zero.")
                Else
                    m_fCellArea = value
                End If
            End Set
        End Property

#Region "Areal Properties"
        Public Property AreaErosion_Raw As Double
            Get
                Return m_fAreaErosion_Raw
            End Get
            Set(value As Double)
                m_fAreaErosion_Raw = value
            End Set
        End Property

        Public Property AreaDeposition_Raw As Double
            Get
                Return m_fAreaDeposition_Raw
            End Get
            Set(value As Double)
                m_fAreaDeposition_Raw = value
            End Set
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
        Public Property VolumeErosion_Raw As Double
            Get
                Return m_fVolumeErosion_Raw
            End Get
            Set(value As Double)
                m_fVolumeErosion_Raw = value
            End Set
        End Property

        Public Property VolumeDeposition_Raw As Double
            Get
                Return m_fVolumeDeposition_Raw
            End Get
            Set(value As Double)
                m_fVolumeDeposition_Raw = value
            End Set
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
                Return SafeDivision(VolumeErosion_Raw, AreaErosion_Raw)
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
                Return SafeDivision(VolumeDeposition_Raw, AreaDeposition_Raw)
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

        Private Function SafeDivision(fNumerator As Double, fDenominator As Double) As Double
            Dim fResult As Double = 0
            If fNumerator <> 0 AndAlso fDenominator <> 0 Then
                fResult = fNumerator / fDenominator
            End If
            Return fResult
        End Function

#End Region

        Public Function GetSummaryStatsAsHTML(ByVal sFormat As String, ByVal eLinearUnit As UnitsNet.Units.LengthUnit) As String

            Dim sHTML As New Text.StringBuilder

            sHTML.Append("<div class=""DoDSummaryTable"">").AppendLine()
            sHTML.Append("<h2>DoD Summary Table</h2>").AppendLine()
            sHTML.Append("</div>").AppendLine()

            sHTML.AppendLine()
            sHTML.Append("<table class=""DoDSummaryTable"" cellspacing=""0"" cellpadding=""3"">").AppendLine()
            sHTML.Append("<tr class = """">").AppendLine()
            sHTML.Append("<td>Attribute</td>").AppendLine()
            sHTML.Append("<td>Raw</td>").AppendLine()
            sHTML.Append("<td>Thresholded DoD Estimate</td>").AppendLine()
            sHTML.Append("<td>&nbsp;</td>").AppendLine()
            sHTML.Append("<td>&#177 Error Volume</td>").AppendLine()
            sHTML.Append("<td>&#037 Error</td>").AppendLine()
            sHTML.Append("</tr>")

            'AREAL
            sHTML.Append("<tr><td class=""DoDRowGroupHeader"" colspan=""6"">AREAL:</td></tr>").AppendLine()
            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Area of Erosion (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)</td>").AppendLine()
            sHTML.Append("<td>" & Me.AreaErosion_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AreaErosion_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Area of Deposition (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)</td>").AppendLine()
            sHTML.Append("<td>" & Me.AreaDeposition_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AreaDeposition_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Area of Detectable Change (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)</td>").AppendLine()
            sHTML.Append("<td>NA</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AreaDetectableChange_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Area of Interest (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)</td>").AppendLine()
            sHTML.Append("<td>" & Me.AreaOfInterest_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>NA</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Percent of Area Of Interest with Detectable Change</td>").AppendLine()
            sHTML.Append("<td>NA</td>").AppendLine()
            sHTML.Append("<td align = right>" & Math.Round(Me.AreaPercentAreaInterestWithDetectableChange, 1) & "&#037</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            'VOLUMETRIC
            sHTML.Append("<tr><td class=""DoDRowGroupHeader"" colspan=""6"">VOLUMETRIC:</td></tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Volume of Erosion (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)</td>").AppendLine()
            sHTML.Append("<td>" & Me.VolumeErosion_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.VolumeErosion_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.VolumeErosion_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Volume of Deposition (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)</td>").AppendLine()
            sHTML.Append("<td>" & Me.VolumeDeposition_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.VolumeDeposition_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.VolumeDeposition_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Volume of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)</td>").AppendLine()
            sHTML.Append("<td>" & Me.VolumeOfDifference_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.VolumeOfDifference_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.VolumeOfDifference_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Total Net Volume of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)</td>").AppendLine()
            sHTML.Append("<td>" & Me.NetVolumeOfDifference_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.NetVolumeOfDifference_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.NetVolumeOfDifference_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine().AppendLine()


            'VERTICAL AVERAGES
            sHTML.Append("<tr><td class=""DoDRowGroupHeader"" colspan=""4"">VERTICAL AVERAGES:</td>").AppendLine()
            sHTML.Append("<td>&#177 Error Thickness</td>").AppendLine()
            sHTML.Append("<td>&#037 Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Average Depth of Erosion (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ")</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageDepthErosion_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AverageDepthErosion_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageDepthErosion_Error & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Average Depth of Deposition (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ")</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageDepthDeposition_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AverageDepthDeposition_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageDepthDeposition_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Average Total Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area of Interest</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageThicknessOfDifferenceAOI_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Average Net Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area of Interest</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageThicknessOfDifferenceAOI_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Average Total Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area with Detectable Change</td>").AppendLine()
            sHTML.Append("<td>NA</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AverageThicknessOfDifferenceADC_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageThicknessOfDifferenceADC_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Average Net Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area with Detectable Change</td>").AppendLine()
            sHTML.Append("<td>NA</td>").AppendLine()
            sHTML.Append("<td align = right>" & Me.AverageNetThicknessOfDifferenceADC_Thresholded.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>&#177</td>").AppendLine()
            sHTML.Append("<td>" & Me.AverageNetThicknessOfDifferenceADC_Error.ToString(sFormat) & "</td>").AppendLine()
            sHTML.Append("<td>Percent Error</td>").AppendLine()
            sHTML.Append("</tr>").AppendLine().AppendLine()

            'PERCENTAGES (BY VOLUME)
            sHTML.Append("<tr><td class=""DoDRowGroupHeader"" colspan=""6"">PERCENTAGES (BY VOLUME):</td>").AppendLine()
            sHTML.Append("</tr>")

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Percent Erosion</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.PercentErosion_Raw, 1) & "&#037</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.PercentErosion_Thresholded, 1) & "&#037</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Percent Deposition</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.PercentDeposition_Raw, 1) & "&#037</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.PercentDeposition_Thresholded, 1) & "&#037</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Percent Imbalance (departure from equilibrium)</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.PercentImbalance_Raw, 1) & "&#037</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.PercentImbalance_Thresholded, 1) & "&#037</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()

            sHTML.Append("<tr>").AppendLine()
            sHTML.Append("<td>Net to Total Volume Ratio</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.NetToTotalVolumeRatio_Raw, 1) & "&#037</td>").AppendLine()
            sHTML.Append("<td>" & Math.Round(Me.NetToTotalVolumeRatio_Thresholded, 1) & "&#037</td>").AppendLine()
            EmptyCol(sHTML, 3)
            sHTML.Append("</tr>").AppendLine()
            sHTML.Append("</table>").AppendLine().AppendLine()

            Dim finishedHTML_Table As String = sHTML.ToString
            sHTML.Length = 0
            sHTML.Capacity = 0

            Return finishedHTML_Table
        End Function

        Public Function GetSummaryStatsAsJSON(ByVal sFormat As String, ByVal eLinearUnit As UnitsNet.Units.LengthUnit) As String()()


            Dim jsonDoDTable As String()() = New String(23)() {}

            'AREAL
            jsonDoDTable(0) = New String(0) {"AREAL:"}

            jsonDoDTable(1) = New String(2) {"Total Area of Erosion (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)",
                                             Me.AreaErosion_Raw.ToString(sFormat),
                                             Me.AreaErosion_Thresholded.ToString(sFormat)}

            jsonDoDTable(2) = New String(2) {"Total Area of Deposition (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)",
                                             Me.AreaDeposition_Raw.ToString(sFormat),
                                             Me.AreaDeposition_Thresholded.ToString(sFormat)}

            jsonDoDTable(3) = New String(2) {"Total Area of Detectable Change (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)",
                                             "NA",
                                             Me.AreaDetectableChange_Thresholded.ToString(sFormat)}


            jsonDoDTable(4) = New String(2) {"Total Area of Interest (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#178)",
                                         Me.AreaOfInterest_Raw.ToString(sFormat),
                                         "NA"}

            jsonDoDTable(5) = New String(2) {"Percent of Area Of Interest with Detectable Change",
                                            "NA",
                                            Math.Round(Me.AreaPercentAreaInterestWithDetectableChange, 1) & "&#037"}

            'VOLUMETRIC
            jsonDoDTable(6) = New String(0) {"VOLUMETRIC:"}

            jsonDoDTable(7) = New String(5) {"Total Volume of Erosion (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)",
                                             Me.VolumeErosion_Raw.ToString(sFormat),
                                             Me.VolumeErosion_Thresholded.ToString(sFormat),
                                             "&#177",
                                             Me.VolumeErosion_Error.ToString(sFormat),
                                             "Percent Error"}

            jsonDoDTable(8) = New String(5) {"Total Volume of Deposition (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)",
                                             Me.VolumeDeposition_Raw.ToString(sFormat),
                                             Me.VolumeDeposition_Thresholded.ToString(sFormat),
                                             "&#177",
                                             Me.VolumeDeposition_Error.ToString(sFormat),
                                             "Percent Error"}

            jsonDoDTable(9) = New String(5) {"Total Volume of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)",
                                             Me.VolumeOfDifference_Raw.ToString(sFormat),
                                             Me.VolumeOfDifference_Thresholded.ToString(sFormat),
                                             "&#177",
                                             Me.VolumeOfDifference_Error.ToString(sFormat),
                                             "Percent Error"}

            jsonDoDTable(10) = New String(5) {"Total Net Volume of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)",
                                              Me.NetVolumeOfDifference_Raw.ToString(sFormat),
                                              Me.NetVolumeOfDifference_Thresholded.ToString(sFormat),
                                              "&#177",
                                              Me.NetVolumeOfDifference_Error.ToString(sFormat),
                                              "Percent Error"}

            jsonDoDTable(11) = New String(5) {"Total Net Volume of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & "&#179)",
                                              Me.NetVolumeOfDifference_Raw.ToString(sFormat),
                                              Me.NetVolumeOfDifference_Thresholded.ToString(sFormat),
                                              "&#177",
                                              Me.NetVolumeOfDifference_Error.ToString(sFormat),
                                              "Percent Error"}

            'VERTICAL AVERAGES
            jsonDoDTable(12) = New String(5) {"VERTICAL AVERAGES:",
                                              String.Empty,
                                              String.Empty,
                                              String.Empty,
                                              "&#177 Error Thickness",
                                              "&#037 Error"}

            jsonDoDTable(13) = New String(5) {"Average Depth of Erosion (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ")",
                                              Me.AverageDepthErosion_Raw.ToString(sFormat),
                                              Me.AverageDepthErosion_Raw.ToString(sFormat),
                                              "&#177",
                                              Me.AverageDepthErosion_Error.ToString(sFormat),
                                              "Percent Error"}

            jsonDoDTable(14) = New String(5) {"Average Depth of Deposition (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ")",
                                              Me.AverageDepthDeposition_Raw.ToString(sFormat),
                                              Me.AverageDepthDeposition_Raw.ToString(sFormat),
                                              "&#177",
                                              Me.AverageDepthDeposition_Error.ToString(sFormat),
                                              "Percent Error"}

            jsonDoDTable(15) = New String(5) {"Average Total Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area of Interest",
                                              Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat),
                                              Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat),
                                              "&#177",
                                              Me.AverageThicknessOfDifferenceAOI_Error.ToString(sFormat),
                                              "Percent Error"}

            jsonDoDTable(16) = New String(5) {"Average Net Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area of Interest",
                                              Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat),
                                              Me.AverageThicknessOfDifferenceAOI_Raw.ToString(sFormat),
                                              "&#177",
                                              Me.AverageThicknessOfDifferenceAOI_Error.ToString(sFormat),
                                              "Percent Error"}

            jsonDoDTable(17) = New String(5) {"Average Total Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area with Detectable Change",
                                              "NA",
                                              Me.AverageThicknessOfDifferenceADC_Thresholded.ToString(sFormat),
                                              "&#177",
                                              Me.AverageThicknessOfDifferenceADC_Error.ToString(sFormat),
                                              "Percent Error"}

            jsonDoDTable(18) = New String(5) {"Average Net Thickness Of Difference (" & UnitsNet.Length.GetAbbreviation(eLinearUnit) & ") for Area with Detectable Change",
                                              "NA",
                                              Me.AverageNetThicknessOfDifferenceADC_Thresholded.ToString(sFormat),
                                              "&#177",
                                              Me.AverageNetThicknessOfDifferenceADC_Error.ToString(sFormat),
                                              "Percent Error"}

            'PERCENTAGES (BY VOLUME)
            jsonDoDTable(19) = New String(0) {"PERCENTAGES (BY VOLUME):"}

            jsonDoDTable(20) = New String(2) {"Percent Erosion",
                                              Math.Round(Me.PercentErosion_Raw, 1) & "&#037",
                                              Math.Round(Me.PercentErosion_Thresholded, 1) & "&#037"}

            jsonDoDTable(21) = New String(2) {"Percent Deposition",
                                              Math.Round(Me.PercentDeposition_Raw, 1) & "&#037",
                                              Math.Round(Me.PercentDeposition_Thresholded, 1) & "&#037"}

            jsonDoDTable(22) = New String(2) {"Percent Imbalance (departure from equilibrium)",
                                              Math.Round(Me.PercentImbalance_Raw, 1) & "&#037",
                                              Math.Round(Me.PercentImbalance_Thresholded, 1) & "&#037"}

            jsonDoDTable(23) = New String(2) {"Net to Total Volume Ratio",
                                              Math.Round(Me.NetToTotalVolumeRatio_Raw, 1) & "&#037",
                                              Math.Round(Me.NetToTotalVolumeRatio_Thresholded, 1) & "&#037"}


            Return jsonDoDTable

        End Function

        Private Sub EmptyColumn(ByRef sHTML As Text.StringBuilder, ByVal width As String)
            sHTML.Append("<td>" & width & "</td>")
        End Sub

        Private Sub EmptyCol(ByRef sHTML As Text.StringBuilder, ByVal numColumns As Integer)

            For i As Integer = 0 To numColumns - 1
                sHTML.Append("<td>&nbsp;</td>")
            Next

        End Sub
    End Class

End Namespace
