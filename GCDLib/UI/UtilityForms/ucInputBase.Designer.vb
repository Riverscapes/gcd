Namespace UI.UtilityForms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucInputBase
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
            Me.cboInput = New System.Windows.Forms.ComboBox()
            Me.cmdBrowse = New System.Windows.Forms.Button()
            Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.SuspendLayout()
            '
            'cboInput
            '
            Me.cboInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboInput.FormattingEnabled = True
            Me.cboInput.Location = New System.Drawing.Point(1, 2)
            Me.cboInput.Name = "cboInput"
            Me.cboInput.Size = New System.Drawing.Size(254, 21)
            Me.cboInput.TabIndex = 0
            '
            'cmdBrowse
            '
            Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdBrowse.Image = My.Resources.BrowseFolder ' Global.RBT.My.Resources.Resources.openfolder
            Me.cmdBrowse.Location = New System.Drawing.Point(261, 1)
            Me.cmdBrowse.Name = "cmdBrowse"
            Me.cmdBrowse.Size = New System.Drawing.Size(23, 23)
            Me.cmdBrowse.TabIndex = 1
            Me.cmdBrowse.UseVisualStyleBackColor = True
            '
            'VectorInputUC
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.cmdBrowse)
            Me.Controls.Add(Me.cboInput)
            Me.Name = "VectorInputUC"
            Me.Size = New System.Drawing.Size(286, 25)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cboInput As System.Windows.Forms.ComboBox
        Friend WithEvents cmdBrowse As System.Windows.Forms.Button
        Friend WithEvents tTip As System.Windows.Forms.ToolTip

    End Class

End Namespace