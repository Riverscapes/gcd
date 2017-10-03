<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DoDSummaryPropertiesForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkPercentages = New System.Windows.Forms.CheckBox()
        Me.chkVertical = New System.Windows.Forms.CheckBox()
        Me.chkVolumetric = New System.Windows.Forms.CheckBox()
        Me.chkRowsAreal = New System.Windows.Forms.CheckBox()
        Me.rdoRowsSpecific = New System.Windows.Forms.RadioButton()
        Me.rdoRowsNormalized = New System.Windows.Forms.RadioButton()
        Me.rdoRowsAll = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkColsPercentage = New System.Windows.Forms.CheckBox()
        Me.chkColsError = New System.Windows.Forms.CheckBox()
        Me.chkColsThresholded = New System.Windows.Forms.CheckBox()
        Me.chkColsRaw = New System.Windows.Forms.CheckBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUnitsOriginal = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboVolume = New System.Windows.Forms.ComboBox()
        Me.cboArea = New System.Windows.Forms.ComboBox()
        Me.cboLinear = New System.Windows.Forms.ComboBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkPercentages)
        Me.GroupBox1.Controls.Add(Me.chkVertical)
        Me.GroupBox1.Controls.Add(Me.chkVolumetric)
        Me.GroupBox1.Controls.Add(Me.chkRowsAreal)
        Me.GroupBox1.Controls.Add(Me.rdoRowsSpecific)
        Me.GroupBox1.Controls.Add(Me.rdoRowsNormalized)
        Me.GroupBox1.Controls.Add(Me.rdoRowsAll)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 190)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(205, 190)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Row Groups"
        '
        'chkPercentages
        '
        Me.chkPercentages.AutoSize = True
        Me.chkPercentages.Checked = True
        Me.chkPercentages.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPercentages.Location = New System.Drawing.Point(53, 164)
        Me.chkPercentages.Name = "chkPercentages"
        Me.chkPercentages.Size = New System.Drawing.Size(86, 17)
        Me.chkPercentages.TabIndex = 6
        Me.chkPercentages.Text = "Percentages"
        Me.chkPercentages.UseVisualStyleBackColor = True
        '
        'chkVertical
        '
        Me.chkVertical.AutoSize = True
        Me.chkVertical.Checked = True
        Me.chkVertical.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkVertical.Location = New System.Drawing.Point(53, 141)
        Me.chkVertical.Name = "chkVertical"
        Me.chkVertical.Size = New System.Drawing.Size(108, 17)
        Me.chkVertical.TabIndex = 5
        Me.chkVertical.Text = "Vertical averages"
        Me.chkVertical.UseVisualStyleBackColor = True
        '
        'chkVolumetric
        '
        Me.chkVolumetric.AutoSize = True
        Me.chkVolumetric.Checked = True
        Me.chkVolumetric.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkVolumetric.Location = New System.Drawing.Point(53, 118)
        Me.chkVolumetric.Name = "chkVolumetric"
        Me.chkVolumetric.Size = New System.Drawing.Size(75, 17)
        Me.chkVolumetric.TabIndex = 4
        Me.chkVolumetric.Text = "Volumetric"
        Me.chkVolumetric.UseVisualStyleBackColor = True
        '
        'chkRowsAreal
        '
        Me.chkRowsAreal.AutoSize = True
        Me.chkRowsAreal.Checked = True
        Me.chkRowsAreal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRowsAreal.Location = New System.Drawing.Point(53, 95)
        Me.chkRowsAreal.Name = "chkRowsAreal"
        Me.chkRowsAreal.Size = New System.Drawing.Size(50, 17)
        Me.chkRowsAreal.TabIndex = 3
        Me.chkRowsAreal.Text = "Areal"
        Me.chkRowsAreal.UseVisualStyleBackColor = True
        '
        'rdoRowsSpecific
        '
        Me.rdoRowsSpecific.AutoSize = True
        Me.rdoRowsSpecific.Location = New System.Drawing.Point(17, 71)
        Me.rdoRowsSpecific.Name = "rdoRowsSpecific"
        Me.rdoRowsSpecific.Size = New System.Drawing.Size(100, 17)
        Me.rdoRowsSpecific.TabIndex = 2
        Me.rdoRowsSpecific.Text = "Specific Groups"
        Me.rdoRowsSpecific.UseVisualStyleBackColor = True
        '
        'rdoRowsNormalized
        '
        Me.rdoRowsNormalized.AutoSize = True
        Me.rdoRowsNormalized.Checked = True
        Me.rdoRowsNormalized.Location = New System.Drawing.Point(17, 47)
        Me.rdoRowsNormalized.Name = "rdoRowsNormalized"
        Me.rdoRowsNormalized.Size = New System.Drawing.Size(99, 17)
        Me.rdoRowsNormalized.TabIndex = 1
        Me.rdoRowsNormalized.TabStop = True
        Me.rdoRowsNormalized.Text = "Normalized only"
        Me.rdoRowsNormalized.UseVisualStyleBackColor = True
        '
        'rdoRowsAll
        '
        Me.rdoRowsAll.AutoSize = True
        Me.rdoRowsAll.Location = New System.Drawing.Point(17, 23)
        Me.rdoRowsAll.Name = "rdoRowsAll"
        Me.rdoRowsAll.Size = New System.Drawing.Size(65, 17)
        Me.rdoRowsAll.TabIndex = 0
        Me.rdoRowsAll.Text = "Show all"
        Me.rdoRowsAll.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkColsPercentage)
        Me.GroupBox2.Controls.Add(Me.chkColsError)
        Me.GroupBox2.Controls.Add(Me.chkColsThresholded)
        Me.GroupBox2.Controls.Add(Me.chkColsRaw)
        Me.GroupBox2.Location = New System.Drawing.Point(221, 190)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(211, 190)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Column Groups"
        '
        'chkColsPercentage
        '
        Me.chkColsPercentage.AutoSize = True
        Me.chkColsPercentage.Checked = True
        Me.chkColsPercentage.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkColsPercentage.Location = New System.Drawing.Point(38, 92)
        Me.chkColsPercentage.Name = "chkColsPercentage"
        Me.chkColsPercentage.Size = New System.Drawing.Size(59, 17)
        Me.chkColsPercentage.TabIndex = 3
        Me.chkColsPercentage.Text = "% Error"
        Me.chkColsPercentage.UseVisualStyleBackColor = True
        '
        'chkColsError
        '
        Me.chkColsError.AutoSize = True
        Me.chkColsError.Checked = True
        Me.chkColsError.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkColsError.Location = New System.Drawing.Point(38, 69)
        Me.chkColsError.Name = "chkColsError"
        Me.chkColsError.Size = New System.Drawing.Size(57, 17)
        Me.chkColsError.TabIndex = 2
        Me.chkColsError.Text = "± Error"
        Me.chkColsError.UseVisualStyleBackColor = True
        '
        'chkColsThresholded
        '
        Me.chkColsThresholded.AutoSize = True
        Me.chkColsThresholded.Checked = True
        Me.chkColsThresholded.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkColsThresholded.Location = New System.Drawing.Point(15, 46)
        Me.chkColsThresholded.Name = "chkColsThresholded"
        Me.chkColsThresholded.Size = New System.Drawing.Size(85, 17)
        Me.chkColsThresholded.TabIndex = 1
        Me.chkColsThresholded.Text = "Thresholded"
        Me.chkColsThresholded.UseVisualStyleBackColor = True
        '
        'chkColsRaw
        '
        Me.chkColsRaw.AutoSize = True
        Me.chkColsRaw.Checked = True
        Me.chkColsRaw.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkColsRaw.Location = New System.Drawing.Point(15, 23)
        Me.chkColsRaw.Name = "chkColsRaw"
        Me.chkColsRaw.Size = New System.Drawing.Size(48, 17)
        Me.chkColsRaw.TabIndex = 0
        Me.chkColsRaw.Text = "Raw"
        Me.chkColsRaw.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOK.Location = New System.Drawing.Point(276, 394)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(357, 394)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Location = New System.Drawing.Point(12, 394)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(75, 23)
        Me.cmdHelp.TabIndex = 5
        Me.cmdHelp.Text = "Help"
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Original linear units:"
        '
        'txtUnitsOriginal
        '
        Me.txtUnitsOriginal.Location = New System.Drawing.Point(139, 15)
        Me.txtUnitsOriginal.Name = "txtUnitsOriginal"
        Me.txtUnitsOriginal.ReadOnly = True
        Me.txtUnitsOriginal.Size = New System.Drawing.Size(268, 20)
        Me.txtUnitsOriginal.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Display linear units:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Display areal units:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 111)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Display volumetric units:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.NumericUpDown1)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.cboVolume)
        Me.GroupBox3.Controls.Add(Me.cboArea)
        Me.GroupBox3.Controls.Add(Me.cboLinear)
        Me.GroupBox3.Controls.Add(Me.txtUnitsOriginal)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Location = New System.Drawing.Point(10, 10)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(422, 172)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Units"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(139, 138)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(53, 20)
        Me.NumericUpDown1.TabIndex = 9
        Me.NumericUpDown1.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 142)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Precision:"
        '
        'cboVolume
        '
        Me.cboVolume.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboVolume.FormattingEnabled = True
        Me.cboVolume.Location = New System.Drawing.Point(139, 107)
        Me.cboVolume.Name = "cboVolume"
        Me.cboVolume.Size = New System.Drawing.Size(268, 21)
        Me.cboVolume.TabIndex = 7
        '
        'cboArea
        '
        Me.cboArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboArea.FormattingEnabled = True
        Me.cboArea.Location = New System.Drawing.Point(139, 76)
        Me.cboArea.Name = "cboArea"
        Me.cboArea.Size = New System.Drawing.Size(268, 21)
        Me.cboArea.TabIndex = 5
        '
        'cboLinear
        '
        Me.cboLinear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLinear.FormattingEnabled = True
        Me.cboLinear.Location = New System.Drawing.Point(139, 45)
        Me.cboLinear.Name = "cboLinear"
        Me.cboLinear.Size = New System.Drawing.Size(268, 21)
        Me.cboLinear.TabIndex = 3
        '
        'DoDSummaryPropertiesForm
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(444, 429)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DoDSummaryPropertiesForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "DoD Summary Properties"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkPercentages As System.Windows.Forms.CheckBox
    Friend WithEvents chkVertical As System.Windows.Forms.CheckBox
    Friend WithEvents chkVolumetric As System.Windows.Forms.CheckBox
    Friend WithEvents chkRowsAreal As System.Windows.Forms.CheckBox
    Friend WithEvents rdoRowsSpecific As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRowsNormalized As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRowsAll As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkColsPercentage As System.Windows.Forms.CheckBox
    Friend WithEvents chkColsError As System.Windows.Forms.CheckBox
    Friend WithEvents chkColsThresholded As System.Windows.Forms.CheckBox
    Friend WithEvents chkColsRaw As System.Windows.Forms.CheckBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUnitsOriginal As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cboVolume As System.Windows.Forms.ComboBox
    Friend WithEvents cboArea As System.Windows.Forms.ComboBox
    Friend WithEvents cboLinear As System.Windows.Forms.ComboBox
End Class
