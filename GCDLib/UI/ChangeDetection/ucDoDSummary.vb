Imports System.Windows.Forms
Imports GCDLib.Core.ChangeDetection

Namespace UI.ChangeDetection

    Public Class ucDoDSummary

        Private Sub DoDSummaryUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            grdData.AllowUserToAddRows = False
            grdData.RowHeadersVisible = False
            grdData.ColumnHeadersVisible = False
            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grdData.AllowUserToOrderColumns = False
            grdData.AllowUserToResizeRows = False

            grdData.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            grdData.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            grdData.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            grdData.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            grdData.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        End Sub

        Public Sub RefreshDisplay(ByRef dodResultSet As Core.ChangeDetection.DoDResult, ByRef options As UI.ChangeDetection.DoDSummaryDisplayOptions)

            ' Build the string formatting based on the precision in the pop-up properties form
            Dim sFormat As String = "#,##0"
            For i As Integer = 1 To options.m_nPrecision
                If i = 1 Then
                    sFormat &= "."
                End If
                sFormat &= "0"
            Next

            grdData.Rows.Clear()

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Show/hide columns based on the properties pop-up
            grdData.Columns(1).Visible = options.m_bColsRaw
            grdData.Columns(2).Visible = options.m_bColsThresholded
            grdData.Columns(3).Visible = options.m_bColsPMError
            grdData.Columns(4).Visible = options.m_bColsPCError
            grdData.Columns(5).Visible = options.m_bColsPCError

            Dim aRow As DataGridViewRow
            Dim nIndex As Integer
            Dim cell As DataGridViewCell

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Title Row
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)

            aRow.Cells(0).Value = "Attribute"
            aRow.Cells(0).Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)

            aRow.Cells(1).Value = "Raw"
            aRow.Cells(1).Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            aRow.Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            aRow.Cells(2).Value = "Thresholded"
            aRow.Cells(2).Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            aRow.Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            aRow.Cells(4).Value = "± Error Volume"
            aRow.Cells(4).Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            aRow.Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            aRow.Cells(5).Value = "% Error"
            aRow.Cells(5).Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            aRow.Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' TODO: Waiting to hear back from UnitsNet developer how to do this dimensionality change
            Dim eAreaUnitsNotImplemented As UnitsNet.Units.AreaUnit = UnitsNet.Units.AreaUnit.SquareMeter
            ' Throw New NotImplementedException("Waiting to hear back from UnitsNet developer how to do this dimensionality change")

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Area of erosion header
            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
                (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsAreal) Then

                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "AREAL:"
                cell = aRow.Cells(0)
                cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)

                'Area of erosion
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Erosion (" & UnitsNet.Area.GetAbbreviation(options.AreaUnits) & ")" 'the superscript 2 is ALT+0178
                aRow.Cells(0).ToolTipText = "The amount of area experiencing erosion"
                aRow.Cells(1).Value = UnitsNet.Area.From(dodResultSet.ChangeStats.AreaErosion_Raw, eAreaUnitsNotImplemented).As(options.AreaUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Area.From(dodResultSet.ChangeStats.AreaErosion_Thresholded, eAreaUnitsNotImplemented).As(options.AreaUnits).ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                'Area of deposition
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Deposition (" & UnitsNet.Area.GetAbbreviation(options.AreaUnits) & ")"
                aRow.Cells(0).ToolTipText = "The amount of area experiencing deposition"
                aRow.Cells(1).Value = UnitsNet.Area.From(dodResultSet.ChangeStats.AreaDeposition_Raw, eAreaUnitsNotImplemented).As(options.AreaUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Area.From(dodResultSet.ChangeStats.AreaDeposition_Thresholded, eAreaUnitsNotImplemented).As(options.AreaUnits).ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                'Area of detectable change
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Detectable Change (" & UnitsNet.Area.GetAbbreviation(options.AreaUnits) & ")"
                aRow.Cells(0).ToolTipText = "The sum of areas experiencing detectable erosion and deposition"
                aRow.Cells(1).Value = "NA"
                aRow.Cells(2).Value = UnitsNet.Area.From(dodResultSet.ChangeStats.AreaDetectableChange_Thresholded, eAreaUnitsNotImplemented).As(options.AreaUnits).ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                'Area of interest 
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Interest (" & UnitsNet.Area.GetAbbreviation(options.AreaUnits) & ")"
                aRow.Cells(0).ToolTipText = "The total amount of area under analysis (including detectable and undetectable)"
                aRow.Cells(1).Value = UnitsNet.Area.From(dodResultSet.ChangeStats.AreaOfInterest_Raw, eAreaUnitsNotImplemented).As(options.AreaUnits).ToString(sFormat)
                aRow.Cells(2).Value = "NA"
                aRow.Cells(2).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                ' Percent of area of interest with detectable change
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Percent of Area of Interest with Detectable Change"
                aRow.Cells(0).ToolTipText = "The percent of the total area of interest with detectable changes (i.e. either exceeding the minimum level of detection or with a proability greater then the confidence interval chosen by user)"
                aRow.Cells(1).Value = "NA"
                aRow.Cells(1).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(2).Value = dodResultSet.ChangeStats.AreaPercentAreaInterestWithDetectableChange.ToString(sFormat) & "%"
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray
            End If
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' VOLUMETRIC
            '
            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
             options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
             (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVolumetric) Then

                ' Volume of erosion header
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "VOLUMETRIC:"
                cell = aRow.Cells(0)
                cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            End If

            Dim eVolUnitsNotImplemented As UnitsNet.Units.VolumeUnit = UnitsNet.Units.VolumeUnit.CubicMeter
            ' TODO: Waiting to hear back from UnitsNet developer how to do this dimensionality change
            ' Throw New NotImplementedException("Waiting to hear back from UnitsNet developer how to do this dimensionality change")

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
                (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVolumetric) Then


                'Volume of erosion
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Volume of Erosion (" & UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) & ")" 'the superscript 3 is ALT+0179
                aRow.Cells(0).ToolTipText = "On a cell-by-cell basis, the DoD erosion depth multiplied by cell area and summed"
                aRow.Cells(1).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeErosion_Raw, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeErosion_Thresholded, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeErosion_Error, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.VolumeOfErosion_Percent.ToString(sFormat)

                'Volume of deposition
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Volume of Deposition (" & UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) & ")"
                aRow.Cells(0).ToolTipText = "On a cell-by-cell basis, the DoD deposition depth multiplied by cell area and summed"
                aRow.Cells(1).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeDeposition_Raw, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeDeposition_Thresholded, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeDeposition_Error, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.VolumeOfDeposition_Percent.ToString(sFormat)

                'Total volume of difference
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Volume of Difference (" & UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) & ")"
                aRow.Cells(0).ToolTipText = "The sum of erosion and deposition volumes (a measure of total turnover)"
                aRow.Cells(1).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeOfDifference_Raw, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeOfDifference_Thresholded, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.VolumeOfDifference_Error, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.VolumeOfDifference_Percent.ToString(sFormat)
            End If

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
            (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVolumetric) Then

                'Total NET volume difference
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Net Volume Difference (" & UnitsNet.Volume.GetAbbreviation(options.VolumeUnits) & ")"
                aRow.Cells(0).ToolTipText = "The net difference of erosion and depostion volumes (i.e. deposition minus erosion)"
                aRow.Cells(1).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.NetVolumeOfDifference_Raw, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.NetVolumeOfDifference_Thresholded, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Volume.From(dodResultSet.ChangeStats.NetVolumeOfDifference_Error, eVolUnitsNotImplemented).As(options.VolumeUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.NetVolumeOfDifference_Percent.ToString(sFormat)
            End If
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' VERTICAL AVERAGES
            '
            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
                options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
                (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVerticalAverages) Then

                ' Vertical Averages header
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "VERTICAL AVERAGES:"
                cell = aRow.Cells(0)
                cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            End If

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
                options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVerticalAverages Then

                ' Average Depth of Erosion
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Depth of Erosion (" & UnitsNet.Length.GetAbbreviation(options.LinearUnits) & ")"
                aRow.Cells(0).ToolTipText = "The average depth of erosion (erosion volume dividied by erosion area)"
                aRow.Cells(1).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageDepthErosion_Raw, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageDepthErosion_Thresholded, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageDepthErosion_Error, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.AverageDepthErosion_Percent.ToString(sFormat)

                ' Average Depth of Deposition
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Depth of Deposition (" & UnitsNet.Length.GetAbbreviation(options.LinearUnits) & ")"
                aRow.Cells(0).ToolTipText = "The average depth of deposition (deposition volume dividied by deposition area)"
                aRow.Cells(1).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageDepthDeposition_Raw, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageDepthDeposition_Thresholded, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageDepthDeposition_Error, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.AverageDepthDeposition_Percent.ToString(sFormat)

                ' Average Total Thickness of Difference for AOI
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Total Thickness of Difference (" & UnitsNet.Length.GetAbbreviation(options.LinearUnits) & ") for Area of Interest"
                aRow.Cells(0).ToolTipText = "The total volume of difference divided by the area of interest (a measure of total turnover thickness in the analysis area)"
                aRow.Cells(1).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Raw, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Thresholded, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Error, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Percent.ToString(sFormat)
            End If

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
             options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
             (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVerticalAverages) Then

                ' Average **NET** Thickness of Difference AOI
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Net Thickness of Difference (" & UnitsNet.Area.GetAbbreviation(options.LinearUnits) & ") for Area of Interest"
                aRow.Cells(0).ToolTipText = "The total net volume of difference dividied by the area of interest (a measure of resulting net change within the analysis area)"
                aRow.Cells(1).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageNetThicknessofDifferenceAOI_Raw, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(2).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageNetThicknessOfDifferenceAOI_Thresholded, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageNetThicknessOfDifferenceAOI_Error, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.AverageNetThicknessOfDifferenceAOI_Percent.ToString(sFormat)
            End If

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
         options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVerticalAverages Then

                ' Average Thickness of Difference ADC
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Total Thickness of Difference (" & UnitsNet.Length.GetAbbreviation(options.LinearUnits) & ") for Area with Detectable Change"
                aRow.Cells(0).ToolTipText = "The total volume of difference divided by the total area of detectable change (a measure of total turnover thickness where there was detectable change)"
                aRow.Cells(1).Value = "NA"
                aRow.Cells(1).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(2).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageThicknessOfDifferenceADC_Thresholded, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageThicknessOfDifferenceADC_Error, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.AverageThicknessOfDifferenceADC_Percent.ToString(sFormat)
            End If

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
              options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
              (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsVerticalAverages) Then

                ' Average **NET** Thickness of Difference ADC
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Net Thickness of Difference (" & UnitsNet.Length.GetAbbreviation(options.LinearUnits) & ") for Area with Detectable Change"
                aRow.Cells(0).ToolTipText = "The total net volume of difference dividied by the total area of detectable change (a measure of resulting net change where the was detectable change)"
                aRow.Cells(1).Value = "NA"
                aRow.Cells(1).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(2).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageNetThicknessOfDifferenceADC_Thresholded, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = UnitsNet.Length.From(dodResultSet.ChangeStats.AverageNetThicknessOfDifferenceADC_Error, dodResultSet.Units).As(options.LinearUnits).ToString(sFormat)
                aRow.Cells(5).Value = dodResultSet.ChangeStats.AverageNetThicknessOfDifferenceADC_Percent.ToString(sFormat)
            End If

            If options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
                (options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso options.m_bRowsPercentages) Then
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Percentages by Volume
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "PERCENTAGES (BY VOLUME):"
                cell = aRow.Cells(0)
                cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)

                ' Percent Erosion
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Percent Erosion"
                aRow.Cells(0).ToolTipText = "Percent of Total Volume of Difference that is erosional"
                aRow.Cells(1).Value = dodResultSet.ChangeStats.PercentErosion_Raw.ToString(sFormat)
                aRow.Cells(2).Value = dodResultSet.ChangeStats.PercentErosion_Thresholded.ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                ' Percent Deposition
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Percent Deposition"
                aRow.Cells(0).ToolTipText = "Percent of Total Volume of Difference that is depositional"
                aRow.Cells(1).Value = dodResultSet.ChangeStats.PercentDeposition_Raw.ToString(sFormat)
                aRow.Cells(2).Value = dodResultSet.ChangeStats.PercentDeposition_Thresholded.ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                ' Percent Imbalance
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Percent Imbalance (departure from equilibium)"
                aRow.Cells(0).ToolTipText = "The percent depature from a 50%-50% equilibirum erosion/deposition balance (an normalized indication of the magnitude of the net difference)"
                aRow.Cells(1).Value = dodResultSet.ChangeStats.PercentImbalance_Raw.ToString(sFormat)
                aRow.Cells(2).Value = dodResultSet.ChangeStats.PercentImbalance_Thresholded.ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

                ' Net to Total Volume Ratio
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Net to Total Volume Ratio"
                aRow.Cells(0).ToolTipText = "The ratio of net volumetric change divided by total volume of change (a measure of how much the net trend explains of the total turnover)"
                aRow.Cells(1).Value = dodResultSet.ChangeStats.NetToTotalVolumeRatio_Raw.ToString(sFormat)
                aRow.Cells(2).Value = dodResultSet.ChangeStats.NetToTotalVolumeRatio_Thresholded.ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray
            End If

        End Sub

    End Class

End Namespace