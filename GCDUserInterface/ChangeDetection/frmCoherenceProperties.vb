Namespace ChangeDetection

    Public Class frmCoherenceProperties

        Private m_nFilterSize As Integer

        Public Property FilterSize As Integer
            Get
                Return m_nFilterSize
            End Get
            Set(ByVal value As Integer)
                m_nFilterSize = value
            End Set
        End Property

        Public Property PercentLess As Integer
            Get
                Return numLess.Value
            End Get
            Set(ByVal value As Integer)
                numLess.Value = value
            End Set
        End Property

        Public Property PercentGreater As Integer
            Get
                Return numGreater.Value
            End Get
            Set(ByVal value As Integer)
                numGreater.Value = value
            End Set
        End Property

        Private Sub CoherencePropertiesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim sFilterSizeText As String = m_nFilterSize.ToString & " x " & m_nFilterSize.ToString
            cboFilterSize.SelectedItem = sFilterSizeText
        End Sub

        Private Sub cboFilterSize_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboFilterSize.SelectedIndexChanged

            Dim i As Integer = cboFilterSize.Text.IndexOf(" ")
            If i > 0 Then
                m_nFilterSize = cboFilterSize.Text.Substring(0, i)
            End If
        End Sub

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Default filter size.
            m_nFilterSize = 5
        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            'Process.Start(My.Resources.HelpBaseURL & "")
        End Sub
    End Class

End Namespace
