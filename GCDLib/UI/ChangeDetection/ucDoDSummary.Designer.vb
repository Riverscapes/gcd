Namespace UI.ChangeDetection
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucDoDSummary
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
            Me.grdData = New System.Windows.Forms.DataGridView()
            Me.colAttribute = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colRaw = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colThresholded = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.SymbolCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colError = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colErrorPC = New System.Windows.Forms.DataGridViewTextBoxColumn()
            CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grdData
            '
            Me.grdData.AllowUserToAddRows = False
            Me.grdData.AllowUserToDeleteRows = False
            Me.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colAttribute, Me.colRaw, Me.colThresholded, Me.SymbolCol, Me.colError, Me.colErrorPC})
            Me.grdData.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdData.Location = New System.Drawing.Point(0, 0)
            Me.grdData.Name = "grdData"
            Me.grdData.ReadOnly = True
            Me.grdData.Size = New System.Drawing.Size(500, 400)
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
            'ucDoDSummary
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.grdData)
            Me.Name = "ucDoDSummary"
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

    End Class
End Namespace