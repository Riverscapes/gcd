Namespace UI.UtilityForms
    Public Class frmProcessResults

        Private m_lMessages As List(Of GISException)

        Private Sub frmProcessResults_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

            For Each ex As GISException In m_lMessages
                lstMessages.Items.Add(ex.Type.ToString & ": " & ex.Message)
            Next

        End Sub

        Public Sub New(ByRef lMessages As List(Of GISException))

            ' This call is required by the designer.
            InitializeComponent()
            m_lMessages = lMessages
            ' Add any initialization after the InitializeComponent() call.

        End Sub
    End Class
End Namespace