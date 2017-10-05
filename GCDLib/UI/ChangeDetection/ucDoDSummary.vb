Imports System.Windows.Forms
Imports GCD.GCDLib.Core.ChangeDetection
Imports naru.math

Namespace UI.ChangeDetection

    Public Class ucDoDSummary

        Private m_DoDResultSet As DoDResultSet

        ''' <summary>
        ''' Stores the status of what columns, rows and units to use.
        ''' </summary>
        ''' <remarks>This is passed to the pop-up form </remarks>
        Private m_Options As DoDSummaryDisplayOptions

        ''' <summary>
        ''' Specify the DoD Result Set that contains the GCD summary XML values
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Set a DoD result set after this user control is created
        ''' BUT BEFORE it is loaded into the UI.</remarks>
        Public Property DoDResultSet As DoDResultSet
            Get
                Return m_DoDResultSet
            End Get
            Set(value As DoDResultSet)
                m_DoDResultSet = value

                ' Needed for Visual Studio Designer to work when the object is not created yet.
                If m_Options Is Nothing AndAlso TypeOf m_DoDResultSet Is DoDResultSet Then
                    m_Options = New DoDSummaryDisplayOptions(m_DoDResultSet.DoDProperties.Units.LinearUnit)
                End If
                RefreshGridView()
            End Set
        End Property

        Public ReadOnly Property Options As DoDSummaryDisplayOptions
        Get
            Return m_Options
        End Get
    End Property

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

#If DEBUG Then
        cmdRefresh.Visible = True
#End If

            ' Needed for Visual Studio Designer to work when the object is not created yet.
            If TypeOf m_DoDResultSet Is DoDResultSet Then
                m_Options = New DoDSummaryDisplayOptions(m_DoDResultSet.DoDProperties.Units.LinearUnit)
                RefreshGridView()
            End If
        End Sub

    Private Sub RefreshGridView()

        If m_Options Is Nothing Then
            Exit Sub
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Build the string formatting based on the precision in the pop-up properties
        ' form.
        Dim sFormat As String = "#,##0"
        For i As Integer = 1 To m_Options.m_nPrecision
            If i = 1 Then
                sFormat &= "."
            End If
            sFormat &= "0"
        Next

        grdData.Rows.Clear()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Show/hide columns based on the properties pop-up
        grdData.Columns(1).Visible = m_Options.m_bColsRaw
        grdData.Columns(2).Visible = m_Options.m_bColsThresholded
        grdData.Columns(3).Visible = m_Options.m_bColsPMError
        grdData.Columns(4).Visible = m_Options.m_bColsPCError
        grdData.Columns(5).Visible = m_Options.m_bColsPCError

        Dim aRow As DataGridViewRow
        Dim nIndex As Integer
        Dim cell As DataGridViewCell

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Title Row
        nIndex = grdData.Rows.Add(1)
        aRow = grdData.Rows(nIndex)

        aRow.Cells(0).Value = "Attribute"
        aRow.Cells(0).Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
        'aRow.Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter

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


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Area of erosion header
        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsAreal) Then

            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
            aRow.Cells(0).Value = "AREAL:"
            cell = aRow.Cells(0)
            cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)

            'Area of erosion
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Erosion (" & NumberFormatting.GetUnitsAsString(m_Options.AreaUnits) & ")" 'the superscript 2 is ALT+0178
                aRow.Cells(0).ToolTipText = "The amount of area experiencing erosion"
                aRow.Cells(1).Value = NumberFormatting.Convert((m_DoDResultSet.DoDProperties.Units.AreaUnit, m_Options.AreaUnits, m_DoDResultSet.ChangeStats.AreaErosion_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert((m_DoDResultSet.DoDProperties.Units.AreaUnit, m_Options.AreaUnits, m_DoDResultSet.ChangeStats.AreaErosion_Thresholded).ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

            'Area of deposition
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Deposition (" & NumberFormatting.GetUnitsAsString(m_Options.AreaUnits) & ")"
                aRow.Cells(0).ToolTipText = "The amount of area experiencing deposition"
                aRow.Cells(1).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.AreaUnit, m_Options.AreaUnits, m_DoDResultSet.ChangeStats.AreaDeposition_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.AreaUnit, m_Options.AreaUnits, m_DoDResultSet.ChangeStats.AreaDeposition_Thresholded).ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

            'Area of detectable change
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Detectable Change (" & NumberFormatting.GetUnitsAsString(m_Options.AreaUnits) & ")"
                aRow.Cells(0).ToolTipText = "The sum of areas experiencing detectable erosion and deposition"
            aRow.Cells(1).Value = "NA"
                aRow.Cells(2).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.AreaUnit, m_Options.AreaUnits, m_DoDResultSet.ChangeStats.AreaDetectableChange_Thresholded).ToString(sFormat)
                aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

            'Area of interest 
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Area of Interest (" & NumberFormatting.GetUnitsAsString(m_Options.AreaUnits) & ")"
                aRow.Cells(0).ToolTipText = "The total amount of area under analysis (including detectable and undetectable)"
                aRow.Cells(1).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.AreaUnit, m_Options.AreaUnits, m_DoDResultSet.ChangeStats.AreaOfInterest_Raw).ToString(sFormat)
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
            aRow.Cells(2).Value = m_DoDResultSet.ChangeStats.AreaPercentAreaInterestWithDetectableChange.ToString(sFormat) & "%"
            aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray
        End If
        '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' VOLUMETRIC
        '
        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
         m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
         (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVolumetric) Then

            ' Volume of erosion header
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
            aRow.Cells(0).Value = "VOLUMETRIC:"
            cell = aRow.Cells(0)
            cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
        End If

        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVolumetric) Then

            'Volume of erosion
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Volume of Erosion (" & NumberFormatting.GetUnitsAsString(m_Options.VolumeUnits) & ")" 'the superscript 3 is ALT+0179
                aRow.Cells(0).ToolTipText = "On a cell-by-cell basis, the DoD erosion depth multiplied by cell area and summed"
                aRow.Cells(1).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeErosion_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeErosion_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeErosion_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.VolumeOfErosion_Percent.ToString(sFormat)

            'Volume of deposition
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Volume of Deposition (" & NumberFormatting.GetUnitsAsString(m_Options.VolumeUnits) & ")"
                aRow.Cells(0).ToolTipText = "On a cell-by-cell basis, the DoD deposition depth multiplied by cell area and summed"
                aRow.Cells(1).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeDeposition_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeDeposition_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeDeposition_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.VolumeOfDeposition_Percent.ToString(sFormat)

            'Total volume of difference
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Volume of Difference (" & NumberFormatting.GetUnitsAsString(m_Options.VolumeUnits) & ")"
                aRow.Cells(0).ToolTipText = "The sum of erosion and deposition volumes (a measure of total turnover)"
                aRow.Cells(1).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeOfDifference_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeOfDifference_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.VolumeOfDifference_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.VolumeOfDifference_Percent.ToString(sFormat)
        End If

        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
            (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVolumetric) Then

            'Total NET volume difference
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Total Net Volume Difference (" & NumberFormatting.GetUnitsAsString(m_Options.VolumeUnits) & ")"
                aRow.Cells(0).ToolTipText = "The net difference of erosion and depostion volumes (i.e. deposition minus erosion)"
                aRow.Cells(1).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.NetVolumeOfDifference_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.NetVolumeOfDifference_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(m_DoDResultSet.DoDProperties.Units.VolumeUnit, m_Options.VolumeUnits, m_DoDResultSet.ChangeStats.NetVolumeOfDifference_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.NetVolumeOfDifference_Percent.ToString(sFormat)
        End If
        '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' VERTICAL AVERAGES
        '
        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
            (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVerticalAverages) Then

            ' Vertical Averages header
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
            aRow.Cells(0).Value = "VERTICAL AVERAGES:"
            cell = aRow.Cells(0)
            cell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
        End If

        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVerticalAverages Then

            ' Average Depth of Erosion
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Depth of Erosion (" & NumberFormatting.GetUnitsAsString(m_Options.LinearUnits) & ")"
                aRow.Cells(0).ToolTipText = "The average depth of erosion (erosion volume dividied by erosion area)"
                aRow.Cells(1).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageDepthErosion_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageDepthErosion_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageDepthErosion_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.AverageDepthErosion_Percent.ToString(sFormat)

            ' Average Depth of Deposition
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Depth of Deposition (" & NumberFormatting.GetUnitsAsString(m_Options.LinearUnits) & ")"
                aRow.Cells(0).ToolTipText = "The average depth of deposition (deposition volume dividied by deposition area)"
                aRow.Cells(1).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageDepthDeposition_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageDepthDeposition_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageDepthDeposition_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.AverageDepthDeposition_Percent.ToString(sFormat)

            ' Average Total Thickness of Difference for AOI
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Total Thickness of Difference (" & NumberFormatting.GetUnitsAsString(m_Options.LinearUnits) & ") for Area of Interest"
                aRow.Cells(0).ToolTipText = "The total volume of difference divided by the area of interest (a measure of total turnover thickness in the analysis area)"
                aRow.Cells(1).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceAOI_Percent.ToString(sFormat)
            End If

            If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
             m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
             (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVerticalAverages) Then

                ' Average **NET** Thickness of Difference AOI
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Net Thickness of Difference (" & NumberFormatting.GetUnitsAsString(m_Options.LinearUnits) & ") for Area of Interest"
                aRow.Cells(0).ToolTipText = "The total net volume of difference dividied by the area of interest (a measure of resulting net change within the analysis area)"
                aRow.Cells(1).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageNetThicknessofDifferenceAOI_Raw).ToString(sFormat)
                aRow.Cells(2).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageNetThicknessOfDifferenceAOI_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageNetThicknessOfDifferenceAOI_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.AverageNetThicknessOfDifferenceAOI_Percent.ToString(sFormat)
            End If

            If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
         m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVerticalAverages Then

                ' Average Thickness of Difference ADC
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Total Thickness of Difference (" & NumberFormatting.GetUnitsAsString(m_Options.LinearUnits) & ") for Area with Detectable Change"
                aRow.Cells(0).ToolTipText = "The total volume of difference divided by the total area of detectable change (a measure of total turnover thickness where there was detectable change)"
                aRow.Cells(1).Value = "NA"
                aRow.Cells(1).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(2).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceADC_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceADC_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.AverageThicknessOfDifferenceADC_Percent.ToString(sFormat)
            End If

            If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
              m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized OrElse
              (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsVerticalAverages) Then

                ' Average **NET** Thickness of Difference ADC
                nIndex = grdData.Rows.Add(1)
                aRow = grdData.Rows(nIndex)
                aRow.Cells(0).Value = "Average Net Thickness of Difference (" & NumberFormatting.GetUnitsAsString(m_Options.LinearUnits) & ") for Area with Detectable Change"
                aRow.Cells(0).ToolTipText = "The total net volume of difference dividied by the total area of detectable change (a measure of resulting net change where the was detectable change)"
                aRow.Cells(1).Value = "NA"
                aRow.Cells(1).Style.BackColor = Drawing.Color.LightGray
                aRow.Cells(2).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageNetThicknessOfDifferenceADC_Thresholded).ToString(sFormat)
                aRow.Cells(3).Value = "±"
                aRow.Cells(4).Value = NumberFormatting.Convert(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options.LinearUnits, m_DoDResultSet.ChangeStats.AverageNetThicknessOfDifferenceADC_Error).ToString(sFormat)
                aRow.Cells(5).Value = m_DoDResultSet.ChangeStats.AverageNetThicknessOfDifferenceADC_Percent.ToString(sFormat)
        End If

        If m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll OrElse
            (m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups AndAlso m_Options.m_bRowsPercentages) Then
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
            aRow.Cells(1).Value = m_DoDResultSet.ChangeStats.PercentErosion_Raw.ToString(sFormat)
            aRow.Cells(2).Value = m_DoDResultSet.ChangeStats.PercentErosion_Thresholded.ToString(sFormat)
            aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

            ' Percent Deposition
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
            aRow.Cells(0).Value = "Percent Deposition"
            aRow.Cells(0).ToolTipText = "Percent of Total Volume of Difference that is depositional"
            aRow.Cells(1).Value = m_DoDResultSet.ChangeStats.PercentDeposition_Raw.ToString(sFormat)
            aRow.Cells(2).Value = m_DoDResultSet.ChangeStats.PercentDeposition_Thresholded.ToString(sFormat)
            aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

            ' Percent Imbalance
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
            aRow.Cells(0).Value = "Percent Imbalance (departure from equilibium)"
            aRow.Cells(0).ToolTipText = "The percent depature from a 50%-50% equilibirum erosion/deposition balance (an normalized indication of the magnitude of the net difference)"
            aRow.Cells(1).Value = m_DoDResultSet.ChangeStats.PercentImbalance_Raw.ToString(sFormat)
            aRow.Cells(2).Value = m_DoDResultSet.ChangeStats.PercentImbalance_Thresholded.ToString(sFormat)
            aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray

            ' Net to Total Volume Ratio
            nIndex = grdData.Rows.Add(1)
            aRow = grdData.Rows(nIndex)
            aRow.Cells(0).Value = "Net to Total Volume Ratio"
            aRow.Cells(0).ToolTipText = "The ratio of net volumetric change divided by total volume of change (a measure of how much the net trend explains of the total turnover)"
            aRow.Cells(1).Value = m_DoDResultSet.ChangeStats.NetToTotalVolumeRatio_Raw.ToString(sFormat)
            aRow.Cells(2).Value = m_DoDResultSet.ChangeStats.NetToTotalVolumeRatio_Thresholded.ToString(sFormat)
            aRow.Cells(3).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(4).Style.BackColor = Drawing.Color.LightGray
            aRow.Cells(5).Style.BackColor = Drawing.Color.LightGray
        End If

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As System.EventArgs) Handles cmdRefresh.Click
        RefreshGridView()
    End Sub

    Private Sub cmdProperties_Click(sender As System.Object, e As System.EventArgs) Handles cmdProperties.Click

            Dim frmOptions As New frmDoDSummaryProperties(DoDResultSet.DoDProperties.Units.LinearUnit, m_Options)
            If frmOptions.ShowDialog = DialogResult.OK Then
            m_Options = frmOptions.Options
            RefreshGridView()
        End If

    End Sub

End Class

End Namespace