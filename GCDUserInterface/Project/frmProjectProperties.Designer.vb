Namespace Project
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmProjectProperties
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProjectProperties))
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.txtDescription = New System.Windows.Forms.TextBox()
            Me.btnBrowseOutput = New System.Windows.Forms.Button()
            Me.txtDirectory = New System.Windows.Forms.TextBox()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.ttpTooltip = New System.Windows.Forms.ToolTip(Me.components)
            Me.txtGCDPath = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.valPrecision = New System.Windows.Forms.NumericUpDown()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.cboDisplayUnits = New System.Windows.Forms.ComboBox()
            Me.cmdHelpPrecision = New System.Windows.Forms.Button()
            CType(Me.valPrecision, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(79, 14)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Name:"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(53, 100)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(63, 13)
            Me.Label2.TabIndex = 7
            Me.Label2.Text = "Description:"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(31, 44)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(84, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Parent directory:"
            '
            'txtName
            '
            Me.txtName.Location = New System.Drawing.Point(124, 10)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(377, 20)
            Me.txtName.TabIndex = 1
            '
            'txtDescription
            '
            Me.txtDescription.AcceptsReturn = True
            Me.txtDescription.Location = New System.Drawing.Point(124, 103)
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(377, 73)
            Me.txtDescription.TabIndex = 8
            '
            'btnBrowseOutput
            '
            Me.btnBrowseOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnBrowseOutput.Image = CType(resources.GetObject("btnBrowseOutput.Image"), System.Drawing.Image)
            Me.btnBrowseOutput.Location = New System.Drawing.Point(478, 39)
            Me.btnBrowseOutput.Name = "btnBrowseOutput"
            Me.btnBrowseOutput.Size = New System.Drawing.Size(23, 23)
            Me.btnBrowseOutput.TabIndex = 4
            Me.btnBrowseOutput.UseVisualStyleBackColor = True
            '
            'txtDirectory
            '
            Me.txtDirectory.Location = New System.Drawing.Point(124, 40)
            Me.txtDirectory.Name = "txtDirectory"
            Me.txtDirectory.Size = New System.Drawing.Size(348, 20)
            Me.txtDirectory.TabIndex = 3
            Me.txtDirectory.TabStop = False
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(427, 254)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 14
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(348, 254)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 13
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Location = New System.Drawing.Point(16, 254)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(75, 23)
            Me.btnHelp.TabIndex = 15
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'txtGCDPath
            '
            Me.txtGCDPath.Location = New System.Drawing.Point(124, 72)
            Me.txtGCDPath.Name = "txtGCDPath"
            Me.txtGCDPath.ReadOnly = True
            Me.txtGCDPath.Size = New System.Drawing.Size(377, 20)
            Me.txtGCDPath.TabIndex = 6
            Me.txtGCDPath.TabStop = False
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(32, 76)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(84, 13)
            Me.Label4.TabIndex = 5
            Me.Label4.Text = "GCD project file:"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(12, 188)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(102, 13)
            Me.Label5.TabIndex = 11
            Me.Label5.Text = "Horizontal precision:"
            '
            'valPrecision
            '
            Me.valPrecision.Location = New System.Drawing.Point(123, 186)
            Me.valPrecision.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.valPrecision.Name = "valPrecision"
            Me.valPrecision.Size = New System.Drawing.Size(62, 20)
            Me.valPrecision.TabIndex = 12
            Me.valPrecision.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(46, 219)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(69, 13)
            Me.Label6.TabIndex = 9
            Me.Label6.Text = "Display units:"
            Me.Label6.Visible = False
            '
            'cboDisplayUnits
            '
            Me.cboDisplayUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboDisplayUnits.FormattingEnabled = True
            Me.cboDisplayUnits.Location = New System.Drawing.Point(123, 215)
            Me.cboDisplayUnits.Name = "cboDisplayUnits"
            Me.cboDisplayUnits.Size = New System.Drawing.Size(378, 21)
            Me.cboDisplayUnits.TabIndex = 10
            Me.cboDisplayUnits.Visible = False
            '
            'cmdHelpPrecision
            '
            Me.cmdHelpPrecision.FlatAppearance.BorderSize = 0
            Me.cmdHelpPrecision.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.cmdHelpPrecision.Image = My.Resources.Resources.Help
            Me.cmdHelpPrecision.Location = New System.Drawing.Point(189, 185)
            Me.cmdHelpPrecision.Name = "cmdHelpPrecision"
            Me.cmdHelpPrecision.Size = New System.Drawing.Size(23, 23)
            Me.cmdHelpPrecision.TabIndex = 27
            Me.cmdHelpPrecision.UseVisualStyleBackColor = True
            '
            'ProjectPropertiesForm
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(514, 283)
            Me.Controls.Add(Me.cmdHelpPrecision)
            Me.Controls.Add(Me.cboDisplayUnits)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.valPrecision)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.txtGCDPath)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.btnBrowseOutput)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.txtDirectory)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(1024, 599)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(520, 99)
            Me.Name = "ProjectPropertiesForm"
            Me.Text = "GCD Project"
            CType(Me.valPrecision, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents txtDescription As System.Windows.Forms.TextBox
        Friend WithEvents txtDirectory As System.Windows.Forms.TextBox
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents btnHelp As System.Windows.Forms.Button
        Friend WithEvents btnBrowseOutput As System.Windows.Forms.Button
        Friend WithEvents ProjectBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents ttpTooltip As System.Windows.Forms.ToolTip
        Friend WithEvents txtGCDPath As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents valPrecision As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents cboDisplayUnits As System.Windows.Forms.ComboBox
        Friend WithEvents cmdHelpPrecision As System.Windows.Forms.Button
    End Class
End Namespace