Namespace SurveyLibrary
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmSurveyDateTime
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.cboYear = New System.Windows.Forms.ComboBox()
            Me.cboMonth = New System.Windows.Forms.ComboBox()
            Me.cboDay = New System.Windows.Forms.ComboBox()
            Me.cboMinute = New System.Windows.Forms.ComboBox()
            Me.cboHour = New System.Windows.Forms.ComboBox()
            Me.cmdSave = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(46, 20)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(64, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Survey date"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(15, 57)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(95, 13)
            Me.Label2.TabIndex = 4
            Me.Label2.Text = "Survey time (24 hr)"
            '
            'cboYear
            '
            Me.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboYear.FormattingEnabled = True
            Me.cboYear.Location = New System.Drawing.Point(115, 16)
            Me.cboYear.Name = "cboYear"
            Me.cboYear.Size = New System.Drawing.Size(67, 21)
            Me.cboYear.TabIndex = 1
            '
            'cboMonth
            '
            Me.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMonth.FormattingEnabled = True
            Me.cboMonth.Location = New System.Drawing.Point(192, 16)
            Me.cboMonth.Name = "cboMonth"
            Me.cboMonth.Size = New System.Drawing.Size(48, 21)
            Me.cboMonth.TabIndex = 2
            '
            'cboDay
            '
            Me.cboDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboDay.FormattingEnabled = True
            Me.cboDay.Location = New System.Drawing.Point(250, 16)
            Me.cboDay.Name = "cboDay"
            Me.cboDay.Size = New System.Drawing.Size(48, 21)
            Me.cboDay.TabIndex = 3
            '
            'cboMinute
            '
            Me.cboMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMinute.FormattingEnabled = True
            Me.cboMinute.Location = New System.Drawing.Point(170, 53)
            Me.cboMinute.Name = "cboMinute"
            Me.cboMinute.Size = New System.Drawing.Size(48, 21)
            Me.cboMinute.TabIndex = 6
            '
            'cboHour
            '
            Me.cboHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboHour.FormattingEnabled = True
            Me.cboHour.Location = New System.Drawing.Point(115, 53)
            Me.cboHour.Name = "cboHour"
            Me.cboHour.Size = New System.Drawing.Size(48, 21)
            Me.cboHour.TabIndex = 5
            '
            'cmdSave
            '
            Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdSave.Location = New System.Drawing.Point(144, 87)
            Me.cmdSave.Name = "cmdSave"
            Me.cmdSave.Size = New System.Drawing.Size(75, 23)
            Me.cmdSave.TabIndex = 7
            Me.cmdSave.Text = "Save"
            Me.cmdSave.UseVisualStyleBackColor = True
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(225, 87)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 8
            Me.cmdCancel.Text = "Cancel"
            Me.cmdCancel.UseVisualStyleBackColor = True
            '
            'frmSurveyDateTime
            '
            Me.AcceptButton = Me.cmdSave
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdCancel
            Me.ClientSize = New System.Drawing.Size(312, 122)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdSave)
            Me.Controls.Add(Me.cboMinute)
            Me.Controls.Add(Me.cboHour)
            Me.Controls.Add(Me.cboDay)
            Me.Controls.Add(Me.cboMonth)
            Me.Controls.Add(Me.cboYear)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmSurveyDateTime"
            Me.Text = " Survey Date And Time"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents cboYear As System.Windows.Forms.ComboBox
        Friend WithEvents cboMonth As System.Windows.Forms.ComboBox
        Friend WithEvents cboDay As System.Windows.Forms.ComboBox
        Friend WithEvents cboMinute As System.Windows.Forms.ComboBox
        Friend WithEvents cboHour As System.Windows.Forms.ComboBox
        Friend WithEvents cmdSave As System.Windows.Forms.Button
        Friend WithEvents cmdCancel As System.Windows.Forms.Button
        Friend WithEvents tTip As System.Windows.Forms.ToolTip
    End Class
End Namespace