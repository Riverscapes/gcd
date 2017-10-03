Public Class DoDSummaryPropertiesForm

    Private m_eOriginalUnits As GISCode.NumberFormatting.LinearUnits
    Private m_Options As DoDSummaryDisplayOptions

    Public ReadOnly Property Options As DoDSummaryDisplayOptions
        Get
            Return m_Options
        End Get
    End Property

    Public Sub New(eOriginalUnits As GISCode.NumberFormatting.LinearUnits, theOptions As DoDSummaryDisplayOptions)

        ' This call is required by the designer.
        InitializeComponent()

        m_eOriginalUnits = eOriginalUnits
        m_Options = theOptions

    End Sub

    Private Sub DoDSummaryPropertiesForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        txtUnitsOriginal.Text = GISCode.NumberFormatting.GetUnitsAsString(m_eOriginalUnits)

        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.mm)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.cm)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.m)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.km)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.inch)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.ft)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.yard)
        AddUnitsToCombo(GISCode.NumberFormatting.LinearUnits.mile)

        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqmm)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqcm)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqm)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqkm)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqin)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqft)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqyd)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.sqmi)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.hectare)
        AddUnitsToCombo(GISCode.NumberFormatting.AreaUnits.acre)

        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.mm3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.cm3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.cupsUS)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.litres)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.m3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.inch3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.feet3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.gallons)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.yard3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.acrefeet)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.mi3)
        AddUnitsToCombo(GISCode.NumberFormatting.VolumeUnits.km3)

        NumericUpDown1.Value = m_Options.m_nPrecision

        ' Do the row check boxes first with the specifc box checked so
        ' that they are enabled.
        rdoRowsSpecific.Checked = True
        chkRowsAreal.Checked = m_Options.m_bRowsAreal
        chkVolumetric.Checked = m_Options.m_bRowsVolumetric
        chkVertical.Checked = m_Options.m_bRowsVerticalAverages
        chkPercentages.Checked = m_Options.m_bRowsPercentages

        rdoRowsAll.Checked = m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll
        rdoRowsNormalized.Checked = m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized
        rdoRowsSpecific.Checked = m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups

        UpdateControls()

        chkColsRaw.Checked = m_Options.m_bColsRaw
        chkColsThresholded.Checked = m_Options.m_bColsThresholded
        chkColsError.Checked = m_Options.m_bColsPMError
        chkColsPercentage.Checked = m_Options.m_bColsPCError

    End Sub

    Private Sub rdoRows_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles _
        rdoRowsSpecific.CheckedChanged

        UpdateControls()
    End Sub

    Private Sub UpdateControls()
        chkRowsAreal.Enabled = rdoRowsSpecific.Checked
        chkVolumetric.Enabled = rdoRowsSpecific.Checked
        chkVertical.Enabled = rdoRowsSpecific.Checked
        chkPercentages.Enabled = rdoRowsSpecific.Checked

        If Not chkColsThresholded.Checked Then
            chkColsError.Checked = False
            chkColsPercentage.Checked = False
        End If

        chkColsError.Enabled = chkColsThresholded.Checked
        chkColsPercentage.Enabled = chkColsThresholded.Checked

    End Sub

    Private Sub AddUnitsToCombo(eUnit As GISCode.NumberFormatting.LinearUnits)
        Dim i As Integer = cboLinear.Items.Add(New LinearComboItem(GISCode.NumberFormatting.GetUnitsAsString(eUnit), eUnit))
        If eUnit = m_Options.m_eLinearUnits Then
            cboLinear.SelectedIndex = i
        End If
    End Sub

    Private Sub AddUnitsToCombo(eUnit As GISCode.NumberFormatting.AreaUnits)

        Dim i As Integer = cboArea.Items.Add(New AreaComboItem(GISCode.NumberFormatting.GetUnitsAsString(eUnit), eUnit))
        If eUnit = m_Options.m_eAreaUnits Then
            cboArea.SelectedIndex = i
        End If
    End Sub

    Private Sub AddUnitsToCombo(eUnit As GISCode.NumberFormatting.VolumeUnits)

        Dim i As Integer = cboVolume.Items.Add(New VolumeComboItem(GISCode.NumberFormatting.GetUnitsAsString(eUnit), eUnit))
        If eUnit = m_Options.m_eVolumetricUnits Then
            cboVolume.SelectedIndex = i
        End If
    End Sub

    ''' <summary>
    ''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks>
    Private Class UnitComboItem
        Private m_sName As String

        Public Overrides Function ToString() As String
            Return m_sName
        End Function

        Public Sub New(sName As String)
            m_sName = sName
        End Sub
    End Class

    ''' <summary>
    ''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks> 
    Private Class LinearComboItem
        Inherits UnitComboItem

        Private m_eUnit As GISCode.NumberFormatting.LinearUnits

        Public ReadOnly Property Units As GISCode.NumberFormatting.LinearUnits
            Get
                Return m_eUnit
            End Get
        End Property

        Public Sub New(sName As String, eUnit As GISCode.NumberFormatting.LinearUnits)
            MyBase.New(sName)

            m_eUnit = eUnit
        End Sub
    End Class

    ''' <summary>
    ''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks>
    Private Class AreaComboItem
        Inherits UnitComboItem

        Private m_eUnit As GISCode.NumberFormatting.AreaUnits

        Public ReadOnly Property Units As GISCode.NumberFormatting.AreaUnits
            Get
                Return m_eUnit
            End Get
        End Property

        Public Sub New(sName As String, eUnit As GISCode.NumberFormatting.AreaUnits)
            MyBase.New(sName)

            m_eUnit = eUnit
        End Sub
    End Class

    ''' <summary>
    ''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks>
    Private Class VolumeComboItem
        Inherits UnitComboItem

        Private m_eUnit As GISCode.NumberFormatting.VolumeUnits

        Public ReadOnly Property Units As GISCode.NumberFormatting.VolumeUnits
            Get
                Return m_eUnit
            End Get
        End Property

        Public Sub New(sName As String, eUnit As GISCode.NumberFormatting.VolumeUnits)
            MyBase.New(sName)

            m_eUnit = eUnit
        End Sub
    End Class

    Private Sub cmdOK_Click(sender As Object, e As System.EventArgs) Handles cmdOK.Click

        m_Options.m_eLinearUnits = DirectCast(cboLinear.SelectedItem, LinearComboItem).Units
        m_Options.m_eAreaUnits = DirectCast(cboArea.SelectedItem, AreaComboItem).Units
        m_Options.m_eVolumetricUnits = DirectCast(cboVolume.SelectedItem, VolumeComboItem).Units

        m_Options.m_nPrecision = CInt(NumericUpDown1.Value)

        If rdoRowsAll.Checked Then
            m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.ShowAll
        ElseIf rdoRowsNormalized.Checked Then
            m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.Normalized
        Else
            m_Options.m_eRowGroups = DoDSummaryDisplayOptions.RowGroups.SpecificGroups
        End If

        m_Options.m_bRowsAreal = chkRowsAreal.Checked
        m_Options.m_bRowsVolumetric = chkVolumetric.Checked
        m_Options.m_bRowsVerticalAverages = chkVertical.Checked
        m_Options.m_bRowsPercentages = chkPercentages.Checked

        m_Options.m_bColsRaw = chkColsRaw.Checked
        m_Options.m_bColsThresholded = chkColsThresholded.Checked
        m_Options.m_bColsPMError = chkColsError.Checked
        m_Options.m_bColsPCError = chkColsPercentage.Checked

    End Sub

    Private Sub chkColsThresholded_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkColsThresholded.CheckedChanged
        UpdateControls()
    End Sub

    Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
        'Process.Start(My.Resources.HelpBaseURL & "")
    End Sub
End Class