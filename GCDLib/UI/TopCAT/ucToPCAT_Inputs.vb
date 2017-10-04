
Namespace TopCAT

    Public Class ucToPCAT_Inputs

        Public Property Units As String
            Get
                Return lblUnits.Text
            End Get
            Set(value As String)
                lblUnits.Text = value
            End Set
        End Property

        Private Sub btn_RawPointCloud_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RawPointCloud.Click

            'Error_NotSpaceDelimited.Clear()
            WindowsFormAssistant.OpenFileDialog("Raw Point Cloud", txtBox_RawPointCloudFile)
            'ToPCAT_Assistant.CheckIfToPCAT_Ready(txtBox_RawPointCloudFile.Text, btn_Run, ucInputsWindow.btn_RawPointCloud)

        End Sub

        Private Sub chkDetrendingOptions_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDetrendingOptions.CheckedChanged
            If chkDetrendingOptions.Checked = False Then
                rdbZmean.Enabled = False
                rdbZmin.Enabled = False
                numStdevDetrendedOption.Enabled = False
            ElseIf chkDetrendingOptions.Checked = True Then
                rdbZmean.Enabled = True
                rdbZmin.Enabled = True
                numStdevDetrendedOption.Enabled = True
            End If
        End Sub

        Private Sub ucToPCAT_Inputs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            tTip.SetToolTip(btn_RawPointCloud, "Press this button to open a file dialog and select a raw point cloud file.")
            tTip.SetToolTip(txtBox_RawPointCloudFile, "Displays the file name of the point cloud. Use the selection button to the right to populate this field.")
            tTip.SetToolTip(numXresolution, "Select x dimension of the decimation sample window. In units of raw point cloud file.")
            tTip.SetToolTip(numYresolution, "Select y dimension of the decimation sample window. In units of raw point cloud file.")
            tTip.SetToolTip(numNtoCalculateStats, "Select the minimum number of points necessary to calculate sample window statistics. Can't be less than 2 points.")
            tTip.SetToolTip(chkDetrendingOptions, "Check this box to enable advanced detrending options.")
            tTip.SetToolTip(btn_DetrendedOptionsInfo, "Click here to go to the documentation on advanced detrending options.")
            tTip.SetToolTip(numStdevDetrendedOption, "Choose how many standard deviations to base locally detrending on.")
            tTip.SetToolTip(rdbZmean, "Calculate locally detrended standard deviation from the mean elevation of the sample window.")
            tTip.SetToolTip(rdbZmin, "Calculate locally detrended standard deviation from the minimum elevation of the sample window.")

            UpdateControls()

        End Sub

        Public Function ValidateUserControl() As Boolean

            If String.IsNullOrEmpty(txtBox_RawPointCloudFile.Text) Then
                MsgBox("Please select a point cloud to process.", MsgBoxStyle.OkOnly, "No Raw Point Cloud Selected")
                Return False
            ElseIf Not IO.File.Exists(txtBox_RawPointCloudFile.Text) Then
                MsgBox("The file you have chosen to process does not exists. Please select an existing file.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            ElseIf Not ToPCAT_Assistant.CheckIfToPCAT_Ready(txtBox_RawPointCloudFile.Text) Then
                MsgBox("This file does not appear to be space delimited and cannot be processed by ToPCAT." & vbCrLf & vbCrLf & _
                       "Use ToPCAT Prep Tool to make file ready to be processed by ToPCAT.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            Return True

        End Function

        Private Sub UpdateControls()

        End Sub

    End Class

End Namespace