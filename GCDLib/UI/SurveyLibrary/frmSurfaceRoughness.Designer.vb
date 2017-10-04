Namespace UI.SurveyLibrary
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmSurfaceRoughness
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
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.ucToPCAT_Inputs = New GCDAddIn.TopCAT.ucToPCAT_Inputs()
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(485, 189)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 16
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(404, 189)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 15
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Enabled = False
            Me.btnHelp.Location = New System.Drawing.Point(13, 189)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(75, 23)
            Me.btnHelp.TabIndex = 17
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'ucToPCAT_Inputs
            '
            Me.ucToPCAT_Inputs.Location = New System.Drawing.Point(12, 3)
            Me.ucToPCAT_Inputs.Name = "ucToPCAT_Inputs"
            Me.ucToPCAT_Inputs.Size = New System.Drawing.Size(548, 180)
            Me.ucToPCAT_Inputs.TabIndex = 0
            Me.ucToPCAT_Inputs.Units = ""
            '
            'SurfaceRoughnessForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(572, 224)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.ucToPCAT_Inputs)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "SurfaceRoughnessForm"
            Me.Text = "Generate Surface Roughness Raster"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ucToPCAT_Inputs As TopCAT.ucToPCAT_Inputs
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents btnHelp As System.Windows.Forms.Button
    End Class
End Namespace