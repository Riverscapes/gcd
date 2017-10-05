<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BudgetClassPieChartUC
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.chtPie = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.chtPie, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chtPie
        '
        ChartArea1.Name = "ChartArea1"
        Me.chtPie.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chtPie.Legends.Add(Legend1)
        Me.chtPie.Location = New System.Drawing.Point(73, 94)
        Me.chtPie.Name = "chtPie"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chtPie.Series.Add(Series1)
        Me.chtPie.Size = New System.Drawing.Size(300, 300)
        Me.chtPie.TabIndex = 0
        Me.chtPie.Text = "chtPie"
        '
        'BudgetClassPieChartUC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.chtPie)
        Me.Name = "BudgetClassPieChartUC"
        Me.Size = New System.Drawing.Size(538, 576)
        CType(Me.chtPie, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chtPie As System.Windows.Forms.DataVisualization.Charting.Chart

End Class
