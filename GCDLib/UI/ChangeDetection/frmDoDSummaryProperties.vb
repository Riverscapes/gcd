Imports naru.math

Namespace UI.ChangeDetection

    Public Class frmDoDSummaryProperties

        Private m_eOriginalUnits As UnitsNet.Units.LengthUnit
        Private m_Options As DoDSummaryDisplayOptions

        Public ReadOnly Property Options As DoDSummaryDisplayOptions
            Get
                Return m_Options
            End Get
        End Property

        Public Sub New(eOriginalUnits As UnitsNet.Units.LengthUnit, theOptions As DoDSummaryDisplayOptions)

            ' This call is required by the designer
            InitializeComponent()

            m_eOriginalUnits = eOriginalUnits
            m_Options = theOptions

        End Sub

        Private Sub DoDSummaryPropertiesForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            txtUnitsOriginal.Text = UnitsNet.Length.GetAbbreviation(m_eOriginalUnits)

            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Millimeter)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Centimeter)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Meter)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Kilometer)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Inch)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Foot)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Yard)
            AddUnitsToCombo(UnitsNet.Units.LengthUnit.Mile)

            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareMillimeter)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareCentimeter)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareMeter)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareKilometer)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareInch)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareFoot)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareYard)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.SquareMile)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.Hectare)
            AddUnitsToCombo(UnitsNet.Units.AreaUnit.Acre)

            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicMillimeter)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicCentimeter)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.MetricCup)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.Liter)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicMeter)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicInch)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicFoot)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.UsGallon)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicYard)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.acrefeet)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicMile)
            AddUnitsToCombo(UnitsNet.Units.VolumeUnit.CubicKilometer)

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

        Private Sub AddUnitsToCombo(eUnit As UnitsNet.Units.LengthUnit)
            Dim i As Integer = cboLinear.Items.Add(New LinearComboItem(eUnit.ToString(), eUnit))
            If eUnit = m_Options.LinearUnits Then
                cboLinear.SelectedIndex = i
            End If
        End Sub

        Private Sub AddUnitsToCombo(eUnit As UnitsNet.Units.AreaUnit)

            Dim i As Integer = cboArea.Items.Add(New AreaComboItem(eUnit.ToString, eUnit))
            If eUnit = m_Options.AreaUnits Then
                cboArea.SelectedIndex = i
            End If
        End Sub

        Private Sub AddUnitsToCombo(eUnit As UnitsNet.Units.VolumeUnit)

            Dim i As Integer = cboVolume.Items.Add(New VolumeComboItem(eUnit.ToString, eUnit))
            If eUnit = m_Options.VolumeUnits Then
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

            Private m_eUnit As UnitsNet.Units.LengthUnit

            Public ReadOnly Property Units As UnitsNet.Units.LengthUnit
                Get
                    Return m_eUnit
                End Get
            End Property

            Public Sub New(sName As String, eUnit As UnitsNet.Units.LengthUnit)
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

            Private m_eUnit As UnitsNet.Units.AreaUnit

            Public ReadOnly Property Units As UnitsNet.Units.AreaUnit
                Get
                    Return m_eUnit
                End Get
            End Property

            Public Sub New(sName As String, eUnit As UnitsNet.Units.AreaUnit)
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

            Private m_eUnit As UnitsNet.Units.VolumeUnit

            Public ReadOnly Property Units As UnitsNet.Units.VolumeUnit
                Get
                    Return m_eUnit
                End Get
            End Property

            Public Sub New(sName As String, eUnit As UnitsNet.Units.VolumeUnit)
                MyBase.New(sName)

                m_eUnit = eUnit
            End Sub
        End Class

        Private Sub cmdOK_Click(sender As Object, e As System.EventArgs) Handles cmdOK.Click

            m_Options.LinearUnits = DirectCast(cboLinear.SelectedItem, LinearComboItem).Units
            m_Options.AreaUnits = DirectCast(cboArea.SelectedItem, AreaComboItem).Units
            m_Options.VolumeUnits = DirectCast(cboVolume.SelectedItem, VolumeComboItem).Units

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

End Namespace