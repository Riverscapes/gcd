<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DoDSummaryUC
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
        Me.grdData = New System.Windows.Forms.DataGridView()
        Me.colAttribute = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRaw = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colThresholded = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SymbolCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colError = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colErrorPC = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.cmdProperties = New System.Windows.Forms.Button()
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdData
        '
        Me.grdData.AllowUserToAddRows = False
        Me.grdData.AllowUserToDeleteRows = False
        Me.grdData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colAttribute, Me.colRaw, Me.colThresholded, Me.SymbolCol, Me.colError, Me.colErrorPC})
        Me.grdData.Location = New System.Drawing.Point(0, 30)
        Me.grdData.Name = "grdData"
        Me.grdData.ReadOnly = True
        Me.grdData.Size = New System.Drawing.Size(500, 370)
        Me.grdData.TabIndex = 3
        '
        'colAttribute
        '
        Me.colAttribute.Frozen = True
        Me.colAttribute.HeaderText = "Attribute"
        Me.colAttribute.Name = "colAttribute"
        Me.colAttribute.ReadOnly = True
        Me.colAttribute.Width = 300
        '
        'colRaw
        '
        Me.colRaw.HeaderText = "Raw"
        Me.colRaw.Name = "colRaw"
        Me.colRaw.ReadOnly = True
        Me.colRaw.Width = 70
        '
        'colThresholded
        '
        Me.colThresholded.HeaderText = "Thresholded"
        Me.colThresholded.Name = "colThresholded"
        Me.colThresholded.ReadOnly = True
        Me.colThresholded.Width = 70
        '
        'SymbolCol
        '
        Me.SymbolCol.HeaderText = "Symbol"
        Me.SymbolCol.Name = "SymbolCol"
        Me.SymbolCol.ReadOnly = True
        Me.SymbolCol.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.SymbolCol.Width = 15
        '
        'colError
        '
        Me.colError.HeaderText = "Error Volume"
        Me.colError.Name = "colError"
        Me.colError.ReadOnly = True
        Me.colError.Width = 70
        '
        'colErrorPC
        '
        Me.colErrorPC.HeaderText = "Error PC"
        Me.colErrorPC.Name = "colErrorPC"
        Me.colErrorPC.ReadOnly = True
        Me.colErrorPC.Width = 70
        '
        'Button1
        '
        Me.Button1.Image = Global.GCDAddIn.My.Resources.Resources.Excel
        Me.Button1.Location = New System.Drawing.Point(89, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(23, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Image = Global.GCDAddIn.My.Resources.Resources.refresh
        Me.cmdRefresh.Location = New System.Drawing.Point(60, 4)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(23, 23)
        Me.cmdRefresh.TabIndex = 1
        Me.cmdRefresh.UseVisualStyleBackColor = True
        Me.cmdRefresh.Visible = False
        '
        'cmdProperties
        '
        Me.cmdProperties.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.cmdProperties.Location = New System.Drawing.Point(0, 4)
        Me.cmdProperties.Name = "cmdProperties"
        Me.cmdProperties.Size = New System.Drawing.Size(23, 23)
        Me.cmdProperties.TabIndex = 0
        Me.cmdProperties.UseVisualStyleBackColor = True
        '
        'DoDSummaryUC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cmdProperties)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.grdData)
        Me.Name = "DoDSummaryUC"
        Me.Size = New System.Drawing.Size(500, 400)
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdData As System.Windows.Forms.DataGridView
    Friend WithEvents colAttribute As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRaw As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colThresholded As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SymbolCol As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colError As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colErrorPC As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
    Friend WithEvents cmdProperties As System.Windows.Forms.Button

End Class
