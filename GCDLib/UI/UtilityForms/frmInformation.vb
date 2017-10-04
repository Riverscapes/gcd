Namespace UI.UtilityForms
    Public Class frmInformation

        Private m_sFormTitle As String
        Private m_sMessage As String

        Public Sub InitializeSizeable(sTitle As String, sMessage As String, nMaxWidth As Integer, nMaxHeight As Integer)
            m_sFormTitle = sTitle
            m_sMessage = sMessage
            Me.MaximumSize = New System.Drawing.Size(nMaxWidth, nMaxHeight)
        End Sub

        Public Sub InitializeFixedDialog(sTitle As String, sMessage As String)
            m_sFormTitle = sTitle
            m_sMessage = sMessage
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        End Sub


        Private Sub InformationForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Me.Text = m_sFormTitle
            txtMessage.Text = m_sMessage
        End Sub
    End Class
End Namespace