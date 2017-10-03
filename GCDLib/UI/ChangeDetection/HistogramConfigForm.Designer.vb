<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HistogramConfigForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HistogramConfigForm))
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.valNumBins = New System.Windows.Forms.NumericUpDown()
        Me.valMinBin = New System.Windows.Forms.NumericUpDown()
        Me.valBinSize = New System.Windows.Forms.NumericUpDown()
        Me.valIncrement = New System.Windows.Forms.NumericUpDown()
        CType(Me.valNumBins, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.valMinBin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.valBinSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.valIncrement, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(162, 159)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOK.Location = New System.Drawing.Point(81, 159)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Number of bins:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Minimum bin:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Bin size:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 117)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Bin increment:"
        '
        'valNumBins
        '
        Me.valNumBins.Location = New System.Drawing.Point(116, 11)
        Me.valNumBins.Name = "valNumBins"
        Me.valNumBins.Size = New System.Drawing.Size(113, 20)
        Me.valNumBins.TabIndex = 1
        Me.valNumBins.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'valMinBin
        '
        Me.valMinBin.DecimalPlaces = 3
        Me.valMinBin.Location = New System.Drawing.Point(116, 45)
        Me.valMinBin.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.valMinBin.Name = "valMinBin"
        Me.valMinBin.Size = New System.Drawing.Size(113, 20)
        Me.valMinBin.TabIndex = 3
        Me.valMinBin.Value = New Decimal(New Integer() {1, 0, 0, -2147221504})
        '
        'valBinSize
        '
        Me.valBinSize.DecimalPlaces = 3
        Me.valBinSize.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.valBinSize.Location = New System.Drawing.Point(116, 79)
        Me.valBinSize.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.valBinSize.Name = "valBinSize"
        Me.valBinSize.Size = New System.Drawing.Size(113, 20)
        Me.valBinSize.TabIndex = 5
        Me.valBinSize.Value = New Decimal(New Integer() {2, 0, 0, 65536})
        '
        'valIncrement
        '
        Me.valIncrement.DecimalPlaces = 3
        Me.valIncrement.Location = New System.Drawing.Point(116, 113)
        Me.valIncrement.Minimum = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.valIncrement.Name = "valIncrement"
        Me.valIncrement.Size = New System.Drawing.Size(113, 20)
        Me.valIncrement.TabIndex = 7
        Me.valIncrement.Value = New Decimal(New Integer() {20, 0, 0, 131072})
        '
        'HistogramConfigForm
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(249, 194)
        Me.Controls.Add(Me.valIncrement)
        Me.Controls.Add(Me.valBinSize)
        Me.Controls.Add(Me.valMinBin)
        Me.Controls.Add(Me.valNumBins)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "HistogramConfigForm"
        Me.Text = "Histogram Properties"
        CType(Me.valNumBins, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.valMinBin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.valBinSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.valIncrement, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents valNumBins As System.Windows.Forms.NumericUpDown
    Friend WithEvents valMinBin As System.Windows.Forms.NumericUpDown
    Friend WithEvents valBinSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents valIncrement As System.Windows.Forms.NumericUpDown
End Class
