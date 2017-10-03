Public Class frmMessageBoxWithReminder

    Public Sub New(sNewWorkspacePath As String) ', bReferenceUserName As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Only show the user name warning when setting to the default workspace.
        Dim sUserNameWarning As String = String.Empty
        'If bReferenceUserName Then
        '    sUserNameWarning = " This is typically because your user name contains spaces or periods."
        'End If

        lblMessage.Text = String.Format("The proposed temporary workspace path ({0}) contains characters that are not recommended.{1}" & _
                                          " It is strongly recommended that you choose an alternative path that is near the root of your C drive and that does not contain spaces or periods. Do you want to proceed and use the selected path anyway?", _
                                          sNewWorkspacePath, sUserNameWarning)
    End Sub

    Private Sub cmdYes_Click(sender As System.Object, e As System.EventArgs) Handles cmdYes.Click, cmdNo.Click

        If My.Settings.StartUpWorkspaceWarning <> chkRemember.Checked Then
            My.Settings.StartUpWorkspaceWarning = chkRemember.Checked
            My.Settings.Save()
        End If

    End Sub

End Class