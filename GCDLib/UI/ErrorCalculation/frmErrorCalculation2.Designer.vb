<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmErrorCalculation2
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
        Me.txtRasterPath = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.grdErrorProperties = New System.Windows.Forms.DataGridView()
        Me.Method = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ErrorType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cboAssociated = New System.Windows.Forms.ComboBox()
        Me.rdoAssociated = New System.Windows.Forms.RadioButton()
        Me.grdFISInputs = New System.Windows.Forms.DataGridView()
        Me.FISInput = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AssociatedSurface = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.cboFIS = New System.Windows.Forms.ComboBox()
        Me.rdoFIS = New System.Windows.Forms.RadioButton()
        Me.valUniform = New System.Windows.Forms.NumericUpDown()
        Me.rdoUniform = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.grdErrorProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.grdFISInputs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.valUniform, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtRasterPath
        '
        Me.txtRasterPath.Location = New System.Drawing.Point(95, 38)
        Me.txtRasterPath.MaxLength = 255
        Me.txtRasterPath.Name = "txtRasterPath"
        Me.txtRasterPath.ReadOnly = True
        Me.txtRasterPath.Size = New System.Drawing.Size(297, 20)
        Me.txtRasterPath.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 41)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Project raster:"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(95, 12)
        Me.txtName.MaxLength = 255
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(297, 20)
        Me.txtName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(53, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(787, 328)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(662, 328)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(119, 23)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "Derive Error Surface"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Enabled = False
        Me.btnHelp.Location = New System.Drawing.Point(20, 328)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 8
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'grdErrorProperties
        '
        Me.grdErrorProperties.AllowUserToAddRows = False
        Me.grdErrorProperties.AllowUserToDeleteRows = False
        Me.grdErrorProperties.AllowUserToResizeRows = False
        Me.grdErrorProperties.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grdErrorProperties.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.grdErrorProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdErrorProperties.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Method, Me.ErrorType})
        Me.grdErrorProperties.Location = New System.Drawing.Point(6, 19)
        Me.grdErrorProperties.Name = "grdErrorProperties"
        Me.grdErrorProperties.RowHeadersVisible = False
        Me.grdErrorProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdErrorProperties.Size = New System.Drawing.Size(355, 218)
        Me.grdErrorProperties.TabIndex = 0
        '
        'Method
        '
        Me.Method.HeaderText = "Survey Method"
        Me.Method.Name = "Method"
        Me.Method.ReadOnly = True
        Me.Method.Width = 175
        '
        'ErrorType
        '
        Me.ErrorType.DataPropertyName = "ErrorType"
        Me.ErrorType.HeaderText = "Error Type"
        Me.ErrorType.Name = "ErrorType"
        Me.ErrorType.ReadOnly = True
        Me.ErrorType.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ErrorType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ErrorType.Width = 175
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboAssociated)
        Me.GroupBox1.Controls.Add(Me.rdoAssociated)
        Me.GroupBox1.Controls.Add(Me.grdFISInputs)
        Me.GroupBox1.Controls.Add(Me.cboFIS)
        Me.GroupBox1.Controls.Add(Me.rdoFIS)
        Me.GroupBox1.Controls.Add(Me.valUniform)
        Me.GroupBox1.Controls.Add(Me.rdoUniform)
        Me.GroupBox1.Location = New System.Drawing.Point(409, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(453, 302)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Error Calculation Definition For Selected Survey Method"
        '
        'cboAssociated
        '
        Me.cboAssociated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAssociated.FormattingEnabled = True
        Me.cboAssociated.Location = New System.Drawing.Point(161, 54)
        Me.cboAssociated.Name = "cboAssociated"
        Me.cboAssociated.Size = New System.Drawing.Size(282, 21)
        Me.cboAssociated.TabIndex = 3
        '
        'rdoAssociated
        '
        Me.rdoAssociated.AutoSize = True
        Me.rdoAssociated.Location = New System.Drawing.Point(16, 56)
        Me.rdoAssociated.Name = "rdoAssociated"
        Me.rdoAssociated.Size = New System.Drawing.Size(139, 17)
        Me.rdoAssociated.TabIndex = 2
        Me.rdoAssociated.TabStop = True
        Me.rdoAssociated.Text = "Associated error surface"
        Me.rdoAssociated.UseVisualStyleBackColor = True
        '
        'grdFISInputs
        '
        Me.grdFISInputs.AllowUserToAddRows = False
        Me.grdFISInputs.AllowUserToDeleteRows = False
        Me.grdFISInputs.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grdFISInputs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.grdFISInputs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FISInput, Me.AssociatedSurface})
        Me.grdFISInputs.Location = New System.Drawing.Point(34, 116)
        Me.grdFISInputs.Name = "grdFISInputs"
        Me.grdFISInputs.RowHeadersVisible = False
        Me.grdFISInputs.Size = New System.Drawing.Size(409, 177)
        Me.grdFISInputs.TabIndex = 6
        '
        'FISInput
        '
        Me.FISInput.HeaderText = "FIS Input"
        Me.FISInput.Name = "FISInput"
        Me.FISInput.ReadOnly = True
        Me.FISInput.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.FISInput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.FISInput.Width = 200
        '
        'AssociatedSurface
        '
        Me.AssociatedSurface.HeaderText = "Associated Surface"
        Me.AssociatedSurface.Name = "AssociatedSurface"
        Me.AssociatedSurface.Width = 200
        '
        'cboFIS
        '
        Me.cboFIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFIS.FormattingEnabled = True
        Me.cboFIS.Location = New System.Drawing.Point(161, 84)
        Me.cboFIS.Name = "cboFIS"
        Me.cboFIS.Size = New System.Drawing.Size(282, 21)
        Me.cboFIS.TabIndex = 5
        '
        'rdoFIS
        '
        Me.rdoFIS.AutoSize = True
        Me.rdoFIS.Location = New System.Drawing.Point(16, 86)
        Me.rdoFIS.Name = "rdoFIS"
        Me.rdoFIS.Size = New System.Drawing.Size(96, 17)
        Me.rdoFIS.TabIndex = 4
        Me.rdoFIS.TabStop = True
        Me.rdoFIS.Text = "FIS error model"
        Me.rdoFIS.UseVisualStyleBackColor = True
        '
        'valUniform
        '
        Me.valUniform.DecimalPlaces = 2
        Me.valUniform.Location = New System.Drawing.Point(161, 25)
        Me.valUniform.Name = "valUniform"
        Me.valUniform.Size = New System.Drawing.Size(85, 20)
        Me.valUniform.TabIndex = 1
        '
        'rdoUniform
        '
        Me.rdoUniform.AutoSize = True
        Me.rdoUniform.Checked = True
        Me.rdoUniform.Location = New System.Drawing.Point(16, 27)
        Me.rdoUniform.Name = "rdoUniform"
        Me.rdoUniform.Size = New System.Drawing.Size(114, 17)
        Me.rdoUniform.TabIndex = 0
        Me.rdoUniform.TabStop = True
        Me.rdoUniform.Text = "Uniform error value"
        Me.rdoUniform.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.grdErrorProperties)
        Me.GroupBox2.Location = New System.Drawing.Point(20, 71)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(372, 243)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Error Calculation Methods"
        '
        'frmErrorCalculation2
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(874, 363)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.txtRasterPath)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmErrorCalculation2"
        Me.Text = "Error Calculation Properties"
        CType(Me.grdErrorProperties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.grdFISInputs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.valUniform, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtRasterPath As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents grdErrorProperties As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboFIS As System.Windows.Forms.ComboBox
    Friend WithEvents rdoFIS As System.Windows.Forms.RadioButton
    Friend WithEvents valUniform As System.Windows.Forms.NumericUpDown
    Friend WithEvents rdoUniform As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents grdFISInputs As System.Windows.Forms.DataGridView
    Friend WithEvents FISInput As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AssociatedSurface As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents tTip As System.Windows.Forms.ToolTip
    Friend WithEvents Method As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ErrorType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cboAssociated As System.Windows.Forms.ComboBox
    Friend WithEvents rdoAssociated As System.Windows.Forms.RadioButton
End Class
