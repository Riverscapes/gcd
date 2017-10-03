#Region "Code Comments"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'       Author: Philip Bailey, Nick Ochoski, & Frank Poulsen
'               ESSA Software Ltd.
'               1765 W 8th Avenue
'               Vancouver, BC, Canada V6J 5C6
'     
'     Copyright: (C) 2011 by ESSA technologies Ltd.
'                This software is subject to copyright protection under the       
'                laws of Canada and other countries.
'
'  Date Created: 14 January 2011
'
'   Description: 
'
#End Region

#Region " Imports "
Imports System.Windows.Forms

#End Region

Public Class AddFISForm

    Private Sub AddFISForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        FISTableBindingSource.DataSource = ProjectManager.fisds
        FISTableBindingSource.AddNew()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Try
            If txtName.TextLength < 1 Then
                MsgBox("Please enter a name for the FIS file.", MsgBoxStyle.Exclamation, My.Resources.ApplicationNameLong)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If

            If txtFISFile.TextLength < 1 Then
                MsgBox("Please select a FIS file.", MsgBoxStyle.Exclamation, My.Resources.ApplicationNameLong)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If

            If Not System.IO.File.Exists(txtFISFile.Text) Then
                System.Windows.Forms.MessageBox.Show("The FIS file does not exist.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If

            Try
                Dim theFile As New GISCode.GCD.ErrorCalculation.FISRuleFile(txtFISFile.Text)
               
            Catch ex As Exception
                MessageBox.Show("The FIS file is invalid and/or badly formatted. Check that the formatting of the file contents match the MatLab fully inference toolbox specifications and try again.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End Try

            Dim CurrentRow As DataRowView = FISTableBindingSource.Current

            CurrentRow("Name") = txtName.Text
            CurrentRow("Path") = txtFISFile.Text

            FISTableBindingSource.EndEdit()
            ProjectManager.saveFIS()

        Catch ex As Exception
            If ex.Message.ToString.ToLower.Contains("name") Then
                MsgBox("Please select a unique name for the selected FIS file.", MsgBoxStyle.Exclamation, My.Resources.ApplicationNameLong)
            ElseIf ex.Message.ToString.ToLower.Contains("path") Then
                MsgBox("A FIS file with the same name is already present in the FIS library.", MsgBoxStyle.Exclamation, My.Resources.ApplicationNameLong)
            Else
                MsgBox("An error occured while trying to save the information, " & vbNewLine & ex.Message)
            End If
            Me.DialogResult = Windows.Forms.DialogResult.None
        End Try

    End Sub

    Private Sub btnBrowseFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseFIS.Click

        Dim fileDialog As OpenFileDialog = New OpenFileDialog()
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
        Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/customize-menu/fis-library")
    End Sub
End Class