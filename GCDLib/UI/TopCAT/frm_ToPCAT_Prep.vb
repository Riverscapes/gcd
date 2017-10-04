Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO

Namespace TopCAT


    Public Class frm_ToPCAT_Prep

        Private m_sFilter As String
        'Private m_RawPointCloud_FileDialog As New OpenFileDialog
        'Private m_OutputSpaceDelim_FileDialog As New SaveFileDialog

        Public ReadOnly Property Filter As String
            Get
                Return m_sFilter
            End Get
        End Property

        Private Sub btn_RawPointCloud_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RawPointCloud.Click

            WindowsFormAssistant.OpenFileDialog("Raw Point Cloud", txtBox_RawPointCloud)

            'txtBox_RawPointCloud.Text = Path.GetFileName(m_RawPointCloud_FileDialog.FileName)

        End Sub

        Private Sub txtBox_RawPointCloud_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBox_RawPointCloud.TextChanged

        End Sub

        Private Sub btn_OutputSpaceDelim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OutputSpaceDelim.Click

            WindowsFormAssistant.SaveFileDialog("ToPCAT Ready Point Cloud File", txtBox_OutputSpaceDelim)
            'txtBox_OutputSpaceDelim.Text = Path.GetFileName(m_OutputSpaceDelim_FileDialog.FileName)

        End Sub

        Private Sub txtBox_OutputSpaceDelim_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBox_OutputSpaceDelim.TextChanged

        End Sub

        Private Sub cmbBox_SelectSeparator_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBox_SelectSeparator.SelectedIndexChanged

            cmbBox_SelectSeparator.SelectedItem = cmbBox_SelectSeparator.SelectedItem

        End Sub

        Private Sub btnPreviewFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviewFile.Click

            If Not String.IsNullOrEmpty(txtBox_RawPointCloud.Text) Then
                If IO.File.Exists(txtBox_RawPointCloud.Text) Then
                    ToPCAT_Assistant.PreviewFirstLine(txtBox_RawPointCloud.Text)
                End If
            End If

        End Sub

        Private Sub btn_Run_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Run.Click

            If Not ValidateForm() Then
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Try
                Dim seperator As String = WindowsFormAssistant.GetSeperator(cmbBox_SelectSeparator.SelectedItem)
                Dim msgText = ToPCAT_Assistant.ToPCATPrep(txtBox_RawPointCloud.Text, seperator, txtBox_OutputSpaceDelim.Text)
                MsgBox(msgText, MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
            Catch ex As Exception

            Finally
                Windows.Forms.Cursor.Current = Cursors.Default
            End Try

        End Sub

        Private Function ValidateForm() As Boolean

            '
            ' TODO: Validate all the inputs are good.
            '
            If String.IsNullOrEmpty(txtBox_RawPointCloud.Text) Then
                MsgBox("Please enter a raw point cloud file.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            Else
                If Not IO.File.Exists(txtBox_RawPointCloud.Text) Then
                    MsgBox("The point cloud file provided does not exist.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            If String.IsNullOrEmpty(cmbBox_SelectSeparator.Text) Then
                MsgBox("Please select a point cloud separator.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            If String.IsNullOrEmpty(txtBox_OutputSpaceDelim.Text) Then
                MsgBox("Please provide an output path and filename for the ToPCAT Ready point cloud file.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            If String.Compare(txtBox_RawPointCloud.Text, txtBox_OutputSpaceDelim.Text) = 0 Then
                MsgBox("Input file and output file cannot be the same. Please change output filename.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            Return True
        End Function


        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click

            System.Diagnostics.Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/data-prep-menu/e-topcat-menu/i-topcat-preparation-tool")

        End Sub

        Private Sub frm_ToPCAT_Prep_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            tTip.SetToolTip(btn_RawPointCloud, "Press this button to open a file dialog and select a raw point cloud file.")
            tTip.SetToolTip(txtBox_RawPointCloud, "Displays the file name of the point cloud. Use the selection button to the right to populate this field.")
            tTip.SetToolTip(cmbBox_SelectSeparator, "Choose the column separator of the raw point cloud input file.")
            tTip.SetToolTip(btn_OutputSpaceDelim, "Press this button to open a file dialog and choose the name and location to save the output to.")
            tTip.SetToolTip(txtBox_OutputSpaceDelim, "Displays the file name of the output ToPCAT ready point cloud file. Use the selection button to the right to populate this field.")

            tTip.SetToolTip(btn_Run, "Click to run the analysis")
            tTip.SetToolTip(btnHelp, "Click to go to the tool documentation.")
            tTip.SetToolTip(btn_Cancel, "Cancel analysis and exit the tool window.")
        End Sub

    End Class


End Namespace