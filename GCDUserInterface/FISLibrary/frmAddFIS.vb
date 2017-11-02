Imports GCDCore.ErrorCalculation

Namespace FISLibrary

    Public Class frmAddFIS

        Private Sub AddFISForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        End Sub

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

            Try
                If txtName.TextLength < 1 Then
                    MsgBox("Please enter a name for the FIS file.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                    Me.DialogResult = System.Windows.Forms.DialogResult.None
                    Exit Sub
                End If

                If txtFISFile.TextLength < 1 Then
                    MsgBox("Please select a FIS file.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                    Me.DialogResult = System.Windows.Forms.DialogResult.None
                    Exit Sub
                End If

                If Not System.IO.File.Exists(txtFISFile.Text) Then
                    System.Windows.Forms.MessageBox.Show("The FIS file does not exist.", GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
                    Me.DialogResult = System.Windows.Forms.DialogResult.None
                    Exit Sub
                End If

                Try
                    Dim theFile As New FIS.FISRuleFile(New System.IO.FileInfo(txtFISFile.Text))

                Catch ex As Exception
                    System.Windows.Forms.MessageBox.Show("The FIS file is invalid and/or badly formatted. Check that the formatting of the file contents match the MatLab fully inference toolbox specifications and try again.", GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
                    Me.DialogResult = System.Windows.Forms.DialogResult.None
                    Exit Sub
                End Try

                ' old binding source code removed here
                Throw New NotImplementedException()

            Catch ex As Exception
                If ex.Message.ToString.ToLower.Contains("name") Then
                    MsgBox("Please select a unique name for the selected FIS file.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                ElseIf ex.Message.ToString.ToLower.Contains("path") Then
                    MsgBox("A FIS file with the same name is already present in the FIS library.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                Else
                    MsgBox("An error occured while trying to save the information, " & vbNewLine & ex.Message)
                End If
                DialogResult = System.Windows.Forms.DialogResult.None
            End Try

        End Sub

        Private Sub btnBrowseFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseFIS.Click

            Dim fileDialog As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
            fileDialog.Title = "Select a FIS File"
            fileDialog.Filter = "GCD FIS Files (*.fis) | *.fis"
            fileDialog.InitialDirectory =
        fileDialog.RestoreDirectory = False

            If fileDialog.ShowDialog() = DialogResult.OK Then
                txtFISFile.Text = fileDialog.FileName

                If String.IsNullOrEmpty(txtName.Text) Then
                    txtName.Text = IO.Path.GetFileNameWithoutExtension(fileDialog.FileName)
                    txtName.SelectAll()
                    txtName.Focus()
                End If
            End If

        End Sub

        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/customize-menu/fis-library")
        End Sub
    End Class

End Namespace