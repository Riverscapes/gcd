<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CoherencePropertiesForm
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
        Me.components = New System.ComponentModel.Container()
        Dim xDimensionsLabel As System.Windows.Forms.Label
        Dim xLowPercentSuffixLabel As System.Windows.Forms.Label
        Dim xLowPercentPrefixLabel As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CoherencePropertiesForm))
        Me.fisPictureBox = New System.Windows.Forms.PictureBox()
        Me.xHighPercentSuffixLabel = New System.Windows.Forms.Label()
        Me.xHighPercentPrefixLabel = New System.Windows.Forms.Label()
        Me.dimensionsLabel = New System.Windows.Forms.Label()
        Me.cboFilterSize = New System.Windows.Forms.ComboBox()
        Me.toolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cancelButton = New System.Windows.Forms.Button()
        Me.okButton = New System.Windows.Forms.Button()
        Me.grpInput = New System.Windows.Forms.GroupBox()
        Me.numGreater = New System.Windows.Forms.NumericUpDown()
        Me.numLess = New System.Windows.Forms.NumericUpDown()
        Me.btnHelp = New System.Windows.Forms.Button()
        xDimensionsLabel = New System.Windows.Forms.Label()
        xLowPercentSuffixLabel = New System.Windows.Forms.Label()
        xLowPercentPrefixLabel = New System.Windows.Forms.Label()
        CType(Me.fisPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpInput.SuspendLayout()
        CType(Me.numGreater, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numLess, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'xDimensionsLabel
        '
        xDimensionsLabel.AutoSize = True
        xDimensionsLabel.Location = New System.Drawing.Point(6, 30)
        xDimensionsLabel.Name = "xDimensionsLabel"
        xDimensionsLabel.Size = New System.Drawing.Size(139, 13)
        xDimensionsLabel.TabIndex = 12
        xDimensionsLabel.Text = "Moving window dimensions:"
        '
        'xLowPercentSuffixLabel
        '
        xLowPercentSuffixLabel.AutoSize = True
        xLowPercentSuffixLabel.Location = New System.Drawing.Point(188, 69)
        xLowPercentSuffixLabel.Name = "xLowPercentSuffixLabel"
        xLowPercentSuffixLabel.Size = New System.Drawing.Size(110, 13)
        xLowPercentSuffixLabel.TabIndex = 17
        xLowPercentSuffixLabel.Text = "%, then probability = 0"
        '
        'xLowPercentPrefixLabel
        '
        xLowPercentPrefixLabel.AutoSize = True
        xLowPercentPrefixLabel.Location = New System.Drawing.Point(6, 69)
        xLowPercentPrefixLabel.Name = "xLowPercentPrefixLabel"
        xLowPercentPrefixLabel.Size = New System.Drawing.Size(126, 13)
        xLowPercentPrefixLabel.TabIndex = 15
        xLowPercentPrefixLabel.Text = "A) If percent of cells is <="
        '
        'fisPictureBox
        '
        Me.fisPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fisPictureBox.Image = CType(resources.GetObject("fisPictureBox.Image"), System.Drawing.Image)
        Me.fisPictureBox.Location = New System.Drawing.Point(9, 9)
        Me.fisPictureBox.Margin = New System.Windows.Forms.Padding(0)
        Me.fisPictureBox.Name = "fisPictureBox"
        Me.fisPictureBox.Size = New System.Drawing.Size(298, 193)
        Me.fisPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.fisPictureBox.TabIndex = 23
        Me.fisPictureBox.TabStop = False
        Me.toolTip1.SetToolTip(Me.fisPictureBox, resources.GetString("fisPictureBox.ToolTip"))
        '
        'xHighPercentSuffixLabel
        '
        Me.xHighPercentSuffixLabel.AutoSize = True
        Me.xHighPercentSuffixLabel.Location = New System.Drawing.Point(188, 95)
        Me.xHighPercentSuffixLabel.Name = "xHighPercentSuffixLabel"
        Me.xHighPercentSuffixLabel.Size = New System.Drawing.Size(110, 13)
        Me.xHighPercentSuffixLabel.TabIndex = 20
        Me.xHighPercentSuffixLabel.Text = "%, then probability = 1"
        '
        'xHighPercentPrefixLabel
        '
        Me.xHighPercentPrefixLabel.AutoSize = True
        Me.xHighPercentPrefixLabel.Location = New System.Drawing.Point(6, 95)
        Me.xHighPercentPrefixLabel.Name = "xHighPercentPrefixLabel"
        Me.xHighPercentPrefixLabel.Size = New System.Drawing.Size(126, 13)
        Me.xHighPercentPrefixLabel.TabIndex = 18
        Me.xHighPercentPrefixLabel.Text = "B) If percent of cells is >="
        '
        'dimensionsLabel
        '
        Me.dimensionsLabel.AutoSize = True
        Me.dimensionsLabel.Location = New System.Drawing.Point(228, 30)
        Me.dimensionsLabel.Name = "dimensionsLabel"
        Me.dimensionsLabel.Size = New System.Drawing.Size(28, 13)
        Me.dimensionsLabel.TabIndex = 14
        Me.dimensionsLabel.Text = "cells"
        '
        'cboFilterSize
        '
        Me.cboFilterSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboFilterSize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboFilterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilterSize.FormattingEnabled = True
        Me.cboFilterSize.Items.AddRange(New Object() {"3 x 3", "5 x 5", "7 x 7", "9 x 9", "11 x 11", "13 x 13", "15 x 15"})
        Me.cboFilterSize.Location = New System.Drawing.Point(151, 27)
        Me.cboFilterSize.Name = "cboFilterSize"
        Me.cboFilterSize.Size = New System.Drawing.Size(71, 21)
        Me.cboFilterSize.TabIndex = 13
        Me.toolTip1.SetToolTip(Me.cboFilterSize, "Choose the size of your moving window.")
        '
        'cancelButton
        '
        Me.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelButton.Location = New System.Drawing.Point(565, 208)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(75, 23)
        Me.cancelButton.TabIndex = 22
        Me.cancelButton.Text = "Cancel"
        Me.cancelButton.UseVisualStyleBackColor = True
        '
        'okButton
        '
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Location = New System.Drawing.Point(484, 208)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 21
        Me.okButton.Text = "OK"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'grpInput
        '
        Me.grpInput.Controls.Add(Me.numGreater)
        Me.grpInput.Controls.Add(Me.numLess)
        Me.grpInput.Controls.Add(xDimensionsLabel)
        Me.grpInput.Controls.Add(xLowPercentPrefixLabel)
        Me.grpInput.Controls.Add(Me.xHighPercentSuffixLabel)
        Me.grpInput.Controls.Add(Me.cboFilterSize)
        Me.grpInput.Controls.Add(xLowPercentSuffixLabel)
        Me.grpInput.Controls.Add(Me.xHighPercentPrefixLabel)
        Me.grpInput.Controls.Add(Me.dimensionsLabel)
        Me.grpInput.Location = New System.Drawing.Point(321, 9)
        Me.grpInput.Name = "grpInput"
        Me.grpInput.Size = New System.Drawing.Size(319, 193)
        Me.grpInput.TabIndex = 24
        Me.grpInput.TabStop = False
        Me.grpInput.Text = "Inputs"
        '
        'numGreater
        '
        Me.numGreater.Location = New System.Drawing.Point(138, 93)
        Me.numGreater.Name = "numGreater"
        Me.numGreater.Size = New System.Drawing.Size(44, 20)
        Me.numGreater.TabIndex = 22
        Me.numGreater.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'numLess
        '
        Me.numLess.Location = New System.Drawing.Point(138, 67)
        Me.numLess.Name = "numLess"
        Me.numLess.Size = New System.Drawing.Size(44, 20)
        Me.numLess.TabIndex = 21
        Me.numLess.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'btnHelp
        '
        Me.btnHelp.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnHelp.Enabled = False
        Me.btnHelp.Location = New System.Drawing.Point(9, 208)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 25
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'CoherencePropertiesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(657, 239)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.grpInput)
        Me.Controls.Add(Me.fisPictureBox)
        Me.Controls.Add(Me.cancelButton)
        Me.Controls.Add(Me.okButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CoherencePropertiesForm"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Transform Function Parameter Editor"
        CType(Me.fisPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpInput.ResumeLayout(False)
        Me.grpInput.PerformLayout()
        CType(Me.numGreater, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numLess, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents fisPictureBox As System.Windows.Forms.PictureBox
    Private WithEvents toolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents xHighPercentSuffixLabel As System.Windows.Forms.Label
    Private WithEvents xHighPercentPrefixLabel As System.Windows.Forms.Label
    Private WithEvents dimensionsLabel As System.Windows.Forms.Label
    Private WithEvents cboFilterSize As System.Windows.Forms.ComboBox
    Private Shadows cancelButton As System.Windows.Forms.Button
    Private WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents grpInput As System.Windows.Forms.GroupBox
    Private WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents numGreater As System.Windows.Forms.NumericUpDown
    Friend WithEvents numLess As System.Windows.Forms.NumericUpDown
End Class
