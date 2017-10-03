<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessageBoxWithReminder
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
        Me.cmdNo = New System.Windows.Forms.Button()
        Me.cmdYes = New System.Windows.Forms.Button()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.chkRemember = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cmdNo
        '
        Me.cmdNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNo.DialogResult = System.Windows.Forms.DialogResult.No
        Me.cmdNo.Location = New System.Drawing.Point(362, 83)
        Me.cmdNo.Name = "cmdNo"
        Me.cmdNo.Size = New System.Drawing.Size(75, 23)
        Me.cmdNo.TabIndex = 0
        Me.cmdNo.Text = "No"
        Me.cmdNo.UseVisualStyleBackColor = True
        '
        'cmdYes
        '
        Me.cmdYes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdYes.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.cmdYes.Location = New System.Drawing.Point(281, 83)
        Me.cmdYes.Name = "cmdYes"
        Me.cmdYes.Size = New System.Drawing.Size(75, 23)
        Me.cmdYes.TabIndex = 1
        Me.cmdYes.Text = "Yes"
        Me.cmdYes.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.Location = New System.Drawing.Point(12, 9)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(425, 71)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "Text provided via constructor"
        '
        'chkRemember
        '
        Me.chkRemember.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkRemember.AutoSize = True
        Me.chkRemember.Checked = True
        Me.chkRemember.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRemember.Location = New System.Drawing.Point(13, 89)
        Me.chkRemember.Name = "chkRemember"
        Me.chkRemember.Size = New System.Drawing.Size(187, 17)
        Me.chkRemember.TabIndex = 3
        Me.chkRemember.Text = "Remind me about this issue again."
        Me.chkRemember.UseVisualStyleBackColor = True
        '
        'frmMessageBoxWithReminder
        '
        Me.AcceptButton = Me.cmdYes
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(449, 118)
        Me.Controls.Add(Me.chkRemember)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.cmdYes)
        Me.Controls.Add(Me.cmdNo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMessageBoxWithReminder"
        Me.Text = "Invalid GCD Temporary Workspace"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdNo As System.Windows.Forms.Button
    Friend WithEvents cmdYes As System.Windows.Forms.Button
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents chkRemember As System.Windows.Forms.CheckBox
End Class
