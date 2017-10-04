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
            Me.cmdBrowse = New System.Windows.Forms.Button()
            Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.txtPath = New System.Windows.Forms.TextBox()
            Me.cmdGISBrowse = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'cmdBrowse
            '
            Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdBrowse.Image = Global.GCD.GCDLib.My.Resources.Resources.BrowseFolder
            Me.cmdBrowse.Location = New System.Drawing.Point(271, 0)
            Me.cmdBrowse.Name = "cmdBrowse"
            Me.cmdBrowse.Size = New System.Drawing.Size(23, 23)
            Me.cmdBrowse.TabIndex = 1
            Me.cmdBrowse.UseVisualStyleBackColor = True
            '
            'txtPath
            '
            Me.txtPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtPath.Location = New System.Drawing.Point(0, 1)
            Me.txtPath.Name = "txtPath"
            Me.txtPath.ReadOnly = True
            Me.txtPath.Size = New System.Drawing.Size(268, 20)
            Me.txtPath.TabIndex = 2
            '
            'cmdGISBrowse
            '
            Me.cmdGISBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdGISBrowse.Image = Global.GCD.GCDLib.My.Resources.Resources.AddToMap
            Me.cmdGISBrowse.Location = New System.Drawing.Point(297, 0)
            Me.cmdGISBrowse.Name = "cmdGISBrowse"
            Me.cmdGISBrowse.Size = New System.Drawing.Size(23, 23)
            Me.cmdGISBrowse.TabIndex = 3
            Me.cmdGISBrowse.UseVisualStyleBackColor = True
            '
            'ucInputBase
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.cmdGISBrowse)
            Me.Controls.Add(Me.txtPath)
            Me.Controls.Add(Me.cmdBrowse)
            Me.Name = "ucInputBase"
            Me.Size = New System.Drawing.Size(323, 23)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents cmdBrowse As System.Windows.Forms.Button
        Friend WithEvents tTip As System.Windows.Forms.ToolTip
        Friend WithEvents txtPath As System.Windows.Forms.TextBox
        Friend WithEvents cmdGISBrowse As System.Windows.Forms.Button
    End Class

End Namespace