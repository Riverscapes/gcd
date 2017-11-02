Namespace TopCAT
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ucMessageManager
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.grbResults = New System.Windows.Forms.GroupBox()
            Me.rtbResults = New System.Windows.Forms.RichTextBox()
            Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.grbResults.SuspendLayout()
            Me.SuspendLayout()
            '
            'grbResults
            '
            Me.grbResults.Controls.Add(Me.rtbResults)
            Me.grbResults.Location = New System.Drawing.Point(4, 4)
            Me.grbResults.Name = "grbResults"
            Me.grbResults.Size = New System.Drawing.Size(493, 305)
            Me.grbResults.TabIndex = 0
            Me.grbResults.TabStop = False
            Me.grbResults.Text = "Results"
            '
            'rtbResults
            '
            Me.rtbResults.Cursor = System.Windows.Forms.Cursors.Default
            Me.rtbResults.Location = New System.Drawing.Point(9, 19)
            Me.rtbResults.Name = "rtbResults"
            Me.rtbResults.ReadOnly = True
            Me.rtbResults.Size = New System.Drawing.Size(475, 280)
            Me.rtbResults.TabIndex = 0
            Me.rtbResults.Text = ""
            '
            'ProgressBar1
            '
            Me.ProgressBar1.Location = New System.Drawing.Point(13, 315)
            Me.ProgressBar1.Name = "ProgressBar1"
            Me.ProgressBar1.Size = New System.Drawing.Size(401, 23)
            Me.ProgressBar1.TabIndex = 1
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(421, 315)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(76, 23)
            Me.btnOk.TabIndex = 2
            Me.btnOk.Text = "Ok"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'ucMessageManager
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.ProgressBar1)
            Me.Controls.Add(Me.grbResults)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Me.Name = "ucMessageManager"
            Me.Size = New System.Drawing.Size(500, 347)
            Me.grbResults.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents grbResults As System.Windows.Forms.GroupBox
        Friend WithEvents rtbResults As System.Windows.Forms.RichTextBox
        Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
        Friend WithEvents btnOk As System.Windows.Forms.Button

    End Class
End Namespace