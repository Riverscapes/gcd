Namespace UtilityForms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmInformation
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInformation))
            Me.cmdOK = New System.Windows.Forms.Button()
            Me.txtMessage = New System.Windows.Forms.RichTextBox()
            Me.SuspendLayout()
            '
            'cmdOK
            '
            Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOK.Location = New System.Drawing.Point(197, 227)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.Size = New System.Drawing.Size(75, 23)
            Me.cmdOK.TabIndex = 0
            Me.cmdOK.Text = "OK"
            Me.cmdOK.UseVisualStyleBackColor = True
            '
            'txtMessage
            '
            Me.txtMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtMessage.Location = New System.Drawing.Point(12, 12)
            Me.txtMessage.Name = "txtMessage"
            Me.txtMessage.Size = New System.Drawing.Size(260, 209)
            Me.txtMessage.TabIndex = 1
            Me.txtMessage.Text = ""
            '
            'InformationForm
            '
            Me.AcceptButton = Me.cmdOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdOK
            Me.ClientSize = New System.Drawing.Size(284, 262)
            Me.Controls.Add(Me.txtMessage)
            Me.Controls.Add(Me.cmdOK)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "InformationForm"
            Me.Text = "InformationForm"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cmdOK As System.Windows.Forms.Button
        Friend WithEvents txtMessage As System.Windows.Forms.RichTextBox
    End Class
End Namespace