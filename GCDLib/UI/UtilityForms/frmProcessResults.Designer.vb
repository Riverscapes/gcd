Namespace UI.UtilityForms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmProcessResults
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
            Me.lstMessages = New System.Windows.Forms.ListBox()
            Me.cmdOK = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lstMessages
            '
            Me.lstMessages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lstMessages.FormattingEnabled = True
            Me.lstMessages.Location = New System.Drawing.Point(12, 12)
            Me.lstMessages.Name = "lstMessages"
            Me.lstMessages.Size = New System.Drawing.Size(655, 511)
            Me.lstMessages.TabIndex = 0
            '
            'cmdOK
            '
            Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOK.Location = New System.Drawing.Point(592, 529)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.Size = New System.Drawing.Size(75, 23)
            Me.cmdOK.TabIndex = 1
            Me.cmdOK.Text = "OK"
            Me.cmdOK.UseVisualStyleBackColor = True
            '
            'frmProcessResults
            '
            Me.AcceptButton = Me.cmdOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdOK
            Me.ClientSize = New System.Drawing.Size(679, 561)
            Me.Controls.Add(Me.cmdOK)
            Me.Controls.Add(Me.lstMessages)
            Me.MinimumSize = New System.Drawing.Size(100, 200)
            Me.Name = "frmProcessResults"
            Me.ShowIcon = False
            Me.Text = "Process Results"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lstMessages As System.Windows.Forms.ListBox
        Friend WithEvents cmdOK As System.Windows.Forms.Button
    End Class
End Namespace