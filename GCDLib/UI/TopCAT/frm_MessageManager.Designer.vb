Namespace TopCAT
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frm_MessageManager
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
            Me.ucResultsWindow = New GCDAddIn.TopCAT.ucMessageManager()
            Me.SuspendLayout()
            '
            'ucResultsWindow
            '
            Me.ucResultsWindow.Cursor = System.Windows.Forms.Cursors.Default
            Me.ucResultsWindow.Location = New System.Drawing.Point(3, 12)
            Me.ucResultsWindow.Name = "ucResultsWindow"
            Me.ucResultsWindow.Size = New System.Drawing.Size(500, 347)
            Me.ucResultsWindow.TabIndex = 0
            '
            'frm_MessageManager
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(506, 359)
            Me.Controls.Add(Me.ucResultsWindow)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frm_MessageManager"
            Me.Text = "ToPCAT Decimation"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ucResultsWindow As GCDAddIn.TopCAT.ucMessageManager
    End Class
End Namespace