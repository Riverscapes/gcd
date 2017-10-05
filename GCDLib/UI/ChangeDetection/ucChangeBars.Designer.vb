Namespace UI.ChangeDetection
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucChangeBars
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
            Me.chtControl = New System.Windows.Forms.DataVisualization.Charting.Chart()
            Me.cboType = New System.Windows.Forms.ComboBox()
            Me.rdoAbsolute = New System.Windows.Forms.RadioButton()
            Me.rdoRelative = New System.Windows.Forms.RadioButton()
            Me.SuspendLayout()
            '
            'zGraph
            '
            Me.chtControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.chtControl.Location = New System.Drawing.Point(0, 57)
            Me.chtControl.Name = "zGraph"
            Me.chtControl.Size = New System.Drawing.Size(200, 320)
            Me.chtControl.TabIndex = 4
            '
            'cboType
            '
            Me.cboType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboType.FormattingEnabled = True
            Me.cboType.Location = New System.Drawing.Point(3, 3)
            Me.cboType.Name = "cboType"
            Me.cboType.Size = New System.Drawing.Size(194, 21)
            Me.cboType.TabIndex = 5
            '
            'rdoAbsolute
            '
            Me.rdoAbsolute.AutoSize = True
            Me.rdoAbsolute.Checked = True
            Me.rdoAbsolute.Location = New System.Drawing.Point(3, 32)
            Me.rdoAbsolute.Name = "rdoAbsolute"
            Me.rdoAbsolute.Size = New System.Drawing.Size(66, 17)
            Me.rdoAbsolute.TabIndex = 6
            Me.rdoAbsolute.TabStop = True
            Me.rdoAbsolute.Text = "Absolute"
            Me.rdoAbsolute.UseVisualStyleBackColor = True
            '
            'rdoRelative
            '
            Me.rdoRelative.AutoSize = True
            Me.rdoRelative.Location = New System.Drawing.Point(75, 32)
            Me.rdoRelative.Name = "rdoRelative"
            Me.rdoRelative.Size = New System.Drawing.Size(64, 17)
            Me.rdoRelative.TabIndex = 7
            Me.rdoRelative.Text = "Relative"
            Me.rdoRelative.UseVisualStyleBackColor = True
            '
            'ChangeBarsUC
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.rdoRelative)
            Me.Controls.Add(Me.rdoAbsolute)
            Me.Controls.Add(Me.cboType)
            Me.Controls.Add(Me.chtControl)
            Me.Name = "ChangeBarsUC"
            Me.Size = New System.Drawing.Size(200, 381)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents chtControl As System.Windows.Forms.DataVisualization.Charting.Chart
        Friend WithEvents cboType As System.Windows.Forms.ComboBox
        Friend WithEvents rdoAbsolute As System.Windows.Forms.RadioButton
        Friend WithEvents rdoRelative As System.Windows.Forms.RadioButton

    End Class
End Namespace