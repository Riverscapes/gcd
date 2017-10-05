Namespace UI.ChangeDetection
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucDoDHistogram
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
            Me.rdoArea = New System.Windows.Forms.RadioButton()
            Me.rdoVolume = New System.Windows.Forms.RadioButton()
            Me.cmdRefresh = New System.Windows.Forms.Button()
            Me.zGraph = New System.Windows.Forms.DataVisualization.Charting.Chart
            Me.SuspendLayout()
            '
            'rdoArea
            '
            Me.rdoArea.AutoSize = True
            Me.rdoArea.Checked = True
            Me.rdoArea.Location = New System.Drawing.Point(4, 4)
            Me.rdoArea.Name = "rdoArea"
            Me.rdoArea.Size = New System.Drawing.Size(47, 17)
            Me.rdoArea.TabIndex = 0
            Me.rdoArea.TabStop = True
            Me.rdoArea.Text = "Area"
            Me.rdoArea.UseVisualStyleBackColor = True
            '
            'rdoVolume
            '
            Me.rdoVolume.AutoSize = True
            Me.rdoVolume.Location = New System.Drawing.Point(57, 4)
            Me.rdoVolume.Name = "rdoVolume"
            Me.rdoVolume.Size = New System.Drawing.Size(60, 17)
            Me.rdoVolume.TabIndex = 1
            Me.rdoVolume.Text = "Volume"
            Me.rdoVolume.UseVisualStyleBackColor = True
            '
            'cmdRefresh
            '
            Me.cmdRefresh.Image = My.Resources.Resources.refresh
            Me.cmdRefresh.Location = New System.Drawing.Point(472, 3)
            Me.cmdRefresh.Name = "cmdRefresh"
            Me.cmdRefresh.Size = New System.Drawing.Size(23, 23)
            Me.cmdRefresh.TabIndex = 2
            Me.cmdRefresh.UseVisualStyleBackColor = True
            Me.cmdRefresh.Visible = False
            '
            'zGraph
            '
            Me.zGraph.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.zGraph.Location = New System.Drawing.Point(0, 32)
            Me.zGraph.Name = "zGraph"
            Me.zGraph.Size = New System.Drawing.Size(500, 365)
            Me.zGraph.TabIndex = 3
            '
            'DoDHistogramUC
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.Controls.Add(Me.zGraph)
            Me.Controls.Add(Me.rdoVolume)
            Me.Controls.Add(Me.rdoArea)
            Me.Controls.Add(Me.cmdRefresh)
            Me.Name = "DoDHistogramUC"
            Me.Size = New System.Drawing.Size(503, 400)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents rdoArea As System.Windows.Forms.RadioButton
        Friend WithEvents rdoVolume As System.Windows.Forms.RadioButton
        Friend WithEvents cmdRefresh As System.Windows.Forms.Button
        Friend WithEvents zGraph As System.Windows.Forms.DataVisualization.Charting.Chart

    End Class
End Namespace