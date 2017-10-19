
Namespace UI.SurveyLibrary

    Public Class frmPointDensity

        Private m_eLinearUnits As UnitsNet.Units.LengthUnit

        Public Sub New(eLinearUnits As UnitsNet.Units.LengthUnit)

            InitializeComponent()

            m_eLinearUnits = eLinearUnits

            ucPointCloud.Initialize("Point Cloud", Core.GISDataStructures.BrowseVectorTypes.Point)

        End Sub

        Private Sub PointDensityForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            ttpToolTip.SetToolTip(valSampleDistance, "Size of the rectangular sample window (in map units) over which point density is calculated")

            cboNeighbourhood.Items.Add("Circle")
            cboNeighbourhood.Items.Add("Rectangle")
            cboNeighbourhood.SelectedIndex = 1

            lblDistance.Text = lblDistance.Text.Substring(0, lblDistance.Text.Length - 1) & " " & m_eLinearUnits.GetUnitsAsString & ":"

        End Sub

        Public ReadOnly Property Neighborhood As String
            Get
                If cboNeighbourhood.SelectedIndex = 0 Then
                    ' Circle
                    Return cboNeighbourhood.SelectedItem.ToString & " " & valSampleDistance.Value.ToString & " MAP"
                Else
                    'rectangle
                    Return cboNeighbourhood.SelectedItem.ToString & " " & valSampleDistance.Value.ToString & " " & valSampleDistance.Value.ToString & " MAP"
                End If
            End Get
        End Property

        Private Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click

            If Not ucPointCloud.Validate Then
                Me.DialogResult = System.Windows.Forms.DialogResult.None
                Exit Sub
            End If

        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/3-deriving-point-density")
        End Sub
    End Class

End Namespace
