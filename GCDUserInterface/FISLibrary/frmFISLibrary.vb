Namespace FISLibrary

    Public Class frmFISLibrary

        Private Sub btnAddFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFIS.Click

            Dim AddFISForm As New frmAddFIS
            AddFISForm.ShowDialog()

        End Sub

        Private Sub btnDeleteFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteFIS.Click

            Dim CurrentRow As DataRowView
            CurrentRow = FISTableBindingSource.Current

            If Not CurrentRow Is Nothing Then

                Dim response As MsgBoxResult
                response = MsgBox("Are you sure you want to remove the selected FIS file from the GCD Software? Note that this will not delete the associated *.fis file.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong)
                If response = MsgBoxResult.Yes Then
                    If Not CurrentRow Is Nothing Then
                        'Delete the selected item from the dataset and write this new information to the XML file at the specified location
                        Throw New NotImplementedException()
                    End If
                End If
            End If

        End Sub

        Private Sub FISLibraryForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ttpTooltip.SetToolTip(btnAddFIS, "Add a FIS file to the GCD FIS Library.")
            ttpTooltip.SetToolTip(btnEditFIS, "Edit the selected FIS file.")
            ttpTooltip.SetToolTip(btnDeleteFIS, "Delete the selected FIS file.")

            If DataGridView1.Columns.Count = 2 Then
                DataGridView1.Columns(1).Width = DataGridView1.Width - DataGridView1.Columns(0).Width - 5
            End If

            'XMLHandling.XMLReadFIS(Me.FISLibrary)

        End Sub

        Private Sub btnEditFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditFIS.Click

            'Dim CurrentRow As DataRowView = FISTableBindingSource.Current
            'If Not CurrentRow Is Nothing Then
            '    If TypeOf CurrentRow.Row Is GCDLib.FISLibrary.FISTableRow Then
            '        Dim fisRow As GCDLib.FISLibrary.FISTableRow = CurrentRow.Row
            '        If IO.File.Exists(fisRow.Path) Then
            '            Try
            '                Dim frm As New frmEditFIS(fisRow.Path)
            '                frm.ShowDialog()
            '            Catch ex As Exception
            '                Dim ex2 As New Exception("Error showing FIS form.", ex)
            '                ex2.Data.Add("FIS Path", fisRow.Path)
            '                Throw ex2
            '            End Try
            '        Else
            '            MsgBox("The specified FIS file does not exist.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
            '        End If
            '    End If
            'End If
        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/customize-menu/fis-library")
        End Sub

        Private Sub btnFISRepo_Click(sender As System.Object, e As System.EventArgs) Handles btnFISRepo.Click
            Process.Start(GCDCore.Properties.Resources.FISRepositoryWebsite)
        End Sub
    End Class
End Namespace