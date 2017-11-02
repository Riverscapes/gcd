Namespace UI.TopCAT
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucToPCAT_Inputs
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.txtBox_RawPointCloudFile = New System.Windows.Forms.TextBox()
            Me.btn_RawPointCloud = New System.Windows.Forms.Button()
            Me.tbcDecimationDetrending = New System.Windows.Forms.TabControl()
            Me.tbpDecimation = New System.Windows.Forms.TabPage()
            Me.lblUnits = New System.Windows.Forms.Label()
            Me.numYresolution = New System.Windows.Forms.NumericUpDown()
            Me.numXresolution = New System.Windows.Forms.NumericUpDown()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.numNtoCalculateStats = New System.Windows.Forms.NumericUpDown()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.tbpDetrending = New System.Windows.Forms.TabPage()
            Me.btn_DetrendedOptionsInfo = New System.Windows.Forms.Button()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.numStdevDetrendedOption = New System.Windows.Forms.NumericUpDown()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.rdbZmean = New System.Windows.Forms.RadioButton()
            Me.chkDetrendingOptions = New System.Windows.Forms.CheckBox()
            Me.rdbZmin = New System.Windows.Forms.RadioButton()
            Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.GroupBox1.SuspendLayout()
            Me.tbcDecimationDetrending.SuspendLayout()
            Me.tbpDecimation.SuspendLayout()
            CType(Me.numYresolution, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numXresolution, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numNtoCalculateStats, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tbpDetrending.SuspendLayout()
            CType(Me.numStdevDetrendedOption, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'GroupBox1
            '
            Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.GroupBox1.Controls.Add(Me.Label13)
            Me.GroupBox1.Controls.Add(Me.txtBox_RawPointCloudFile)
            Me.GroupBox1.Controls.Add(Me.btn_RawPointCloud)
            Me.GroupBox1.Controls.Add(Me.tbcDecimationDetrending)
            Me.GroupBox1.Location = New System.Drawing.Point(4, 3)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(539, 175)
            Me.GroupBox1.TabIndex = 0
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Inputs"
            '
            'Label13
            '
            Me.Label13.AutoSize = True
            Me.Label13.Location = New System.Drawing.Point(8, 26)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New System.Drawing.Size(87, 13)
            Me.Label13.TabIndex = 0
            Me.Label13.Text = "Raw point cloud:"
            '
            'txtBox_RawPointCloudFile
            '
            Me.txtBox_RawPointCloudFile.AllowDrop = True
            Me.txtBox_RawPointCloudFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtBox_RawPointCloudFile.BackColor = System.Drawing.SystemColors.HighlightText
            Me.txtBox_RawPointCloudFile.Cursor = System.Windows.Forms.Cursors.Arrow
            Me.txtBox_RawPointCloudFile.Location = New System.Drawing.Point(103, 23)
            Me.txtBox_RawPointCloudFile.Name = "txtBox_RawPointCloudFile"
            Me.txtBox_RawPointCloudFile.Size = New System.Drawing.Size(401, 20)
            Me.txtBox_RawPointCloudFile.TabIndex = 1
            '
            'btn_RawPointCloud
            '
            Me.btn_RawPointCloud.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn_RawPointCloud.Image = My.Resources.Resources.BrowseFolder
            Me.btn_RawPointCloud.Location = New System.Drawing.Point(510, 22)
            Me.btn_RawPointCloud.Name = "btn_RawPointCloud"
            Me.btn_RawPointCloud.Size = New System.Drawing.Size(23, 23)
            Me.btn_RawPointCloud.TabIndex = 2
            Me.btn_RawPointCloud.UseVisualStyleBackColor = True
            '
            'tbcDecimationDetrending
            '
            Me.tbcDecimationDetrending.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbcDecimationDetrending.Controls.Add(Me.tbpDecimation)
            Me.tbcDecimationDetrending.Controls.Add(Me.tbpDetrending)
            Me.tbcDecimationDetrending.Location = New System.Drawing.Point(7, 52)
            Me.tbcDecimationDetrending.Name = "tbcDecimationDetrending"
            Me.tbcDecimationDetrending.SelectedIndex = 0
            Me.tbcDecimationDetrending.Size = New System.Drawing.Size(526, 116)
            Me.tbcDecimationDetrending.TabIndex = 3
            '
            'tbpDecimation
            '
            Me.tbpDecimation.BackColor = System.Drawing.Color.Transparent
            Me.tbpDecimation.Controls.Add(Me.lblUnits)
            Me.tbpDecimation.Controls.Add(Me.numYresolution)
            Me.tbpDecimation.Controls.Add(Me.numXresolution)
            Me.tbpDecimation.Controls.Add(Me.Label6)
            Me.tbpDecimation.Controls.Add(Me.Label5)
            Me.tbpDecimation.Controls.Add(Me.Label4)
            Me.tbpDecimation.Controls.Add(Me.numNtoCalculateStats)
            Me.tbpDecimation.Controls.Add(Me.Label7)
            Me.tbpDecimation.Location = New System.Drawing.Point(4, 22)
            Me.tbpDecimation.Name = "tbpDecimation"
            Me.tbpDecimation.Padding = New System.Windows.Forms.Padding(3)
            Me.tbpDecimation.Size = New System.Drawing.Size(518, 90)
            Me.tbpDecimation.TabIndex = 0
            Me.tbpDecimation.Text = "Decimation Options"
            Me.tbpDecimation.UseVisualStyleBackColor = True
            '
            'lblUnits
            '
            Me.lblUnits.AutoSize = True
            Me.lblUnits.Location = New System.Drawing.Point(321, 14)
            Me.lblUnits.Name = "lblUnits"
            Me.lblUnits.Size = New System.Drawing.Size(0, 13)
            Me.lblUnits.TabIndex = 7
            '
            'numYresolution
            '
            Me.numYresolution.DecimalPlaces = 4
            Me.numYresolution.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
            Me.numYresolution.Location = New System.Drawing.Point(258, 10)
            Me.numYresolution.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.numYresolution.Minimum = New Decimal(New Integer() {1, 0, 0, 262144})
            Me.numYresolution.Name = "numYresolution"
            Me.numYresolution.Size = New System.Drawing.Size(57, 20)
            Me.numYresolution.TabIndex = 4
            Me.numYresolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.numYresolution.ThousandsSeparator = True
            Me.numYresolution.Value = New Decimal(New Integer() {2000, 0, 0, 196608})
            '
            'numXresolution
            '
            Me.numXresolution.DecimalPlaces = 4
            Me.numXresolution.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
            Me.numXresolution.Location = New System.Drawing.Point(160, 10)
            Me.numXresolution.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.numXresolution.Minimum = New Decimal(New Integer() {1, 0, 0, 262144})
            Me.numXresolution.Name = "numXresolution"
            Me.numXresolution.Size = New System.Drawing.Size(57, 20)
            Me.numXresolution.TabIndex = 2
            Me.numXresolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.numXresolution.ThousandsSeparator = True
            Me.numXresolution.Value = New Decimal(New Integer() {2000, 0, 0, 196608})
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(236, 14)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(14, 13)
            Me.Label6.TabIndex = 3
            Me.Label6.Text = "Y"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(143, 14)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(14, 13)
            Me.Label5.TabIndex = 1
            Me.Label5.Text = "X"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(11, 14)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(105, 13)
            Me.Label4.TabIndex = 0
            Me.Label4.Text = "Sample window size:"
            '
            'numNtoCalculateStats
            '
            Me.numNtoCalculateStats.Location = New System.Drawing.Point(258, 44)
            Me.numNtoCalculateStats.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numNtoCalculateStats.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.numNtoCalculateStats.Name = "numNtoCalculateStats"
            Me.numNtoCalculateStats.Size = New System.Drawing.Size(57, 20)
            Me.numNtoCalculateStats.TabIndex = 6
            Me.numNtoCalculateStats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.numNtoCalculateStats.Value = New Decimal(New Integer() {4, 0, 0, 0})
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(11, 48)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(233, 13)
            Me.Label7.TabIndex = 5
            Me.Label7.Text = "Minimum number of points to calculate statistics:"
            '
            'tbpDetrending
            '
            Me.tbpDetrending.BackColor = System.Drawing.Color.Transparent
            Me.tbpDetrending.Controls.Add(Me.btn_DetrendedOptionsInfo)
            Me.tbpDetrending.Controls.Add(Me.Label10)
            Me.tbpDetrending.Controls.Add(Me.numStdevDetrendedOption)
            Me.tbpDetrending.Controls.Add(Me.Label9)
            Me.tbpDetrending.Controls.Add(Me.rdbZmean)
            Me.tbpDetrending.Controls.Add(Me.chkDetrendingOptions)
            Me.tbpDetrending.Controls.Add(Me.rdbZmin)
            Me.tbpDetrending.Location = New System.Drawing.Point(4, 22)
            Me.tbpDetrending.Name = "tbpDetrending"
            Me.tbpDetrending.Padding = New System.Windows.Forms.Padding(3)
            Me.tbpDetrending.Size = New System.Drawing.Size(518, 90)
            Me.tbpDetrending.TabIndex = 1
            Me.tbpDetrending.Text = "Detrending Options (Advanced)"
            Me.tbpDetrending.UseVisualStyleBackColor = True
            '
            'btn_DetrendedOptionsInfo
            '
            Me.btn_DetrendedOptionsInfo.Image = My.Resources.Resources.Help
            Me.btn_DetrendedOptionsInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btn_DetrendedOptionsInfo.Location = New System.Drawing.Point(166, 10)
            Me.btn_DetrendedOptionsInfo.Name = "btn_DetrendedOptionsInfo"
            Me.btn_DetrendedOptionsInfo.Size = New System.Drawing.Size(23, 23)
            Me.btn_DetrendedOptionsInfo.TabIndex = 22
            Me.btn_DetrendedOptionsInfo.UseVisualStyleBackColor = True
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(131, 37)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(103, 13)
            Me.Label10.TabIndex = 21
            Me.Label10.Text = "Standard Deviations"
            '
            'numStdevDetrendedOption
            '
            Me.numStdevDetrendedOption.Enabled = False
            Me.numStdevDetrendedOption.Location = New System.Drawing.Point(89, 34)
            Me.numStdevDetrendedOption.Name = "numStdevDetrendedOption"
            Me.numStdevDetrendedOption.Size = New System.Drawing.Size(36, 20)
            Me.numStdevDetrendedOption.TabIndex = 20
            Me.numStdevDetrendedOption.Tag = ""
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(26, 15)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(134, 13)
            Me.Label9.TabIndex = 16
            Me.Label9.Text = "Enable Detrending Options"
            '
            'rdbZmean
            '
            Me.rdbZmean.AutoSize = True
            Me.rdbZmean.Checked = True
            Me.rdbZmean.Enabled = False
            Me.rdbZmean.Location = New System.Drawing.Point(5, 35)
            Me.rdbZmean.Name = "rdbZmean"
            Me.rdbZmean.Size = New System.Drawing.Size(78, 17)
            Me.rdbZmean.TabIndex = 19
            Me.rdbZmean.TabStop = True
            Me.rdbZmean.Text = "Use zmean"
            Me.rdbZmean.UseVisualStyleBackColor = True
            '
            'chkDetrendingOptions
            '
            Me.chkDetrendingOptions.AutoSize = True
            Me.chkDetrendingOptions.Location = New System.Drawing.Point(5, 15)
            Me.chkDetrendingOptions.Name = "chkDetrendingOptions"
            Me.chkDetrendingOptions.Size = New System.Drawing.Size(15, 14)
            Me.chkDetrendingOptions.TabIndex = 17
            Me.chkDetrendingOptions.UseVisualStyleBackColor = True
            '
            'rdbZmin
            '
            Me.rdbZmin.AutoSize = True
            Me.rdbZmin.Enabled = False
            Me.rdbZmin.Location = New System.Drawing.Point(5, 58)
            Me.rdbZmin.Name = "rdbZmin"
            Me.rdbZmin.Size = New System.Drawing.Size(68, 17)
            Me.rdbZmin.TabIndex = 18
            Me.rdbZmin.Text = "Use zmin"
            Me.rdbZmin.UseVisualStyleBackColor = True
            '
            'ucToPCAT_Inputs
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.GroupBox1)
            Me.Name = "ucToPCAT_Inputs"
            Me.Size = New System.Drawing.Size(546, 180)
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.tbcDecimationDetrending.ResumeLayout(False)
            Me.tbpDecimation.ResumeLayout(False)
            Me.tbpDecimation.PerformLayout()
            CType(Me.numYresolution, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numXresolution, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numNtoCalculateStats, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tbpDetrending.ResumeLayout(False)
            Me.tbpDetrending.PerformLayout()
            CType(Me.numStdevDetrendedOption, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents tbcDecimationDetrending As System.Windows.Forms.TabControl
        Friend WithEvents tbpDecimation As System.Windows.Forms.TabPage
        Friend WithEvents tbpDetrending As System.Windows.Forms.TabPage
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents txtBox_RawPointCloudFile As System.Windows.Forms.TextBox
        Friend WithEvents btn_RawPointCloud As System.Windows.Forms.Button
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents numYresolution As System.Windows.Forms.NumericUpDown
        Friend WithEvents numXresolution As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents numNtoCalculateStats As System.Windows.Forms.NumericUpDown
        Friend WithEvents btn_DetrendedOptionsInfo As System.Windows.Forms.Button
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents numStdevDetrendedOption As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents rdbZmean As System.Windows.Forms.RadioButton
        Friend WithEvents chkDetrendingOptions As System.Windows.Forms.CheckBox
        Friend WithEvents rdbZmin As System.Windows.Forms.RadioButton
        Friend WithEvents tTip As System.Windows.Forms.ToolTip
        Friend WithEvents lblUnits As System.Windows.Forms.Label

    End Class

End Namespace