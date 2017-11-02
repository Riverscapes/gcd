Imports System.Windows.Forms
Imports System.IO

Namespace FISLibrary

    Public Class frmEditFIS

        Private m_sPath As String


        Public Sub New(ByVal sPath As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            If String.IsNullOrEmpty(sPath) Then
                Throw New Exception("Null or empty FIS path.")
            End If

            m_sPath = sPath

        End Sub

        Private Sub EditFISForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

            Try
                If IO.File.Exists(m_sPath) Then
                    Dim s As New StreamReader(m_sPath)

                    Do While Not s.EndOfStream
                        txtEditor.Text &= s.ReadLine & vbNewLine
                    Loop
                    txtEditor.Select(0, 0)
                    s.Close()
                End If

                txtEditor.ReadOnly = True

            Catch ex As Exception
                Dim ex2 As New Exception("Error loading FIS file into text editor.", ex)
                ex2.Data.Add("FIS Path", m_sPath)
                Throw ex2
            End Try

            ' TOOLTIPS
            'ttpTooltip.SetToolTip(btnEdit, My.Resources.ttpEditFISFormBtnEdit)
            'ttpTooltip.SetToolTip(btnOK, My.Resources.ttpEditFISFormBtnOK)
            'ttpTooltip.SetToolTip(btnSave, My.Resources.ttpEditFISFormBtnSave)
            'ttpTooltip.SetToolTip(btnSaveAs, My.Resources.ttpEditFISFormBtnSaveAs)
        End Sub

        Private Sub btnEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnEdit.Click

            txtEditor.ReadOnly = False

        End Sub

        Private Sub btnSaveAs_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveAs.Click

            Dim myStream As Stream
            Dim txtFIS As String = txtEditor.Text

            Dim fileDialog As SaveFileDialog = New SaveFileDialog()
            fileDialog.Title = "Save FIS file"
            fileDialog.Filter = "GCD FIS Files (*.fis) | *.fis"
            fileDialog.RestoreDirectory = False

            If fileDialog.ShowDialog() = DialogResult.OK Then
                myStream = fileDialog.OpenFile()

                If Not txtFIS.Length < 1 Then
                    Using sw As StreamWriter = New StreamWriter(myStream)
                        sw.Write(txtEditor.Text)
                        sw.Close()
                        txtEditor.ReadOnly = True
                        MsgBox("FIS file saved successfully.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    End Using
                Else
                    MsgBox("The edited FIS is empty.")
                End If
            End If

        End Sub

        Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

            If IO.File.Exists(m_sPath) Then
                Dim s As New StreamWriter(m_sPath)
                s.Write(txtEditor.Text)
                s.Close()
            End If

            MsgBox("FIS file saved successfully.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)

            'Else - if file does not exist - would you like to remove from library?

        End Sub

        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/customize-menu/fis-library")
        End Sub

    End Class

End Namespace