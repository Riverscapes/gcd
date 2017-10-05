Namespace UI.ChangeDetection
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmDoDProperties
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDoDProperties))
            Me.Label1 = New System.Windows.Forms.Label()
            Me.cboNewDEM = New System.Windows.Forms.ComboBox()
            Me.cboNewError = New System.Windows.Forms.ComboBox()
            Me.lblNewError = New System.Windows.Forms.Label()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.GroupBox3 = New System.Windows.Forms.GroupBox()
            Me.valMinLodThreshold = New System.Windows.Forms.NumericUpDown()
            Me.lblMinLodThreshold = New System.Windows.Forms.Label()
            Me.cmdBayesianProperties = New System.Windows.Forms.Button()
            Me.chkBayesian = New System.Windows.Forms.CheckBox()
            Me.valConfidence = New System.Windows.Forms.NumericUpDown()
            Me.lblConfidence = New System.Windows.Forms.Label()
            Me.rdoProbabilistic = New System.Windows.Forms.RadioButton()
            Me.rdoPropagated = New System.Windows.Forms.RadioButton()
            Me.rdoMinLOD = New System.Windows.Forms.RadioButton()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.GroupBox4 = New System.Windows.Forms.GroupBox()
            Me.cboOldDEM = New System.Windows.Forms.ComboBox()
            Me.cboOldError = New System.Windows.Forms.ComboBox()
            Me.lblOldError = New System.Windows.Forms.Label()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.cmdOK = New System.Windows.Forms.Button()
            Me.cmdHelp = New System.Windows.Forms.Button()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.lstAOI = New System.Windows.Forms.CheckedListBox()
            Me.txtOutputFolder = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox3.SuspendLayout()
            CType(Me.valMinLodThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.valConfidence, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox4.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(14, 30)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(34, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "DEM:"
            '
            'cboNewDEM
            '
            Me.cboNewDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboNewDEM.FormattingEnabled = True
            Me.cboNewDEM.Location = New System.Drawing.Point(54, 30)
            Me.cboNewDEM.Name = "cboNewDEM"
            Me.cboNewDEM.Size = New System.Drawing.Size(201, 21)
            Me.cboNewDEM.TabIndex = 1
            '
            'cboNewError
            '
            Me.cboNewError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboNewError.FormattingEnabled = True
            Me.cboNewError.Location = New System.Drawing.Point(54, 57)
            Me.cboNewError.Name = "cboNewError"
            Me.cboNewError.Size = New System.Drawing.Size(201, 21)
            Me.cboNewError.TabIndex = 3
            '
            'lblNewError
            '
            Me.lblNewError.AutoSize = True
            Me.lblNewError.Location = New System.Drawing.Point(16, 57)
            Me.lblNewError.Name = "lblNewError"
            Me.lblNewError.Size = New System.Drawing.Size(32, 13)
            Me.lblNewError.TabIndex = 2
            Me.lblNewError.Text = "Error:"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.cboNewDEM)
            Me.GroupBox1.Controls.Add(Me.cboNewError)
            Me.GroupBox1.Controls.Add(Me.lblNewError)
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Location = New System.Drawing.Point(12, 74)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(266, 93)
            Me.GroupBox1.TabIndex = 4
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "New Survey"
            '
            'GroupBox3
            '
            Me.GroupBox3.Controls.Add(Me.valMinLodThreshold)
            Me.GroupBox3.Controls.Add(Me.lblMinLodThreshold)
            Me.GroupBox3.Controls.Add(Me.cmdBayesianProperties)
            Me.GroupBox3.Controls.Add(Me.chkBayesian)
            Me.GroupBox3.Controls.Add(Me.valConfidence)
            Me.GroupBox3.Controls.Add(Me.lblConfidence)
            Me.GroupBox3.Controls.Add(Me.rdoProbabilistic)
            Me.GroupBox3.Controls.Add(Me.rdoPropagated)
            Me.GroupBox3.Controls.Add(Me.rdoMinLOD)
            Me.GroupBox3.Location = New System.Drawing.Point(12, 286)
            Me.GroupBox3.Name = "GroupBox3"
            Me.GroupBox3.Size = New System.Drawing.Size(537, 177)
            Me.GroupBox3.TabIndex = 7
            Me.GroupBox3.TabStop = False
            Me.GroupBox3.Text = "Uncertainty Analysis Method"
            '
            'valMinLodThreshold
            '
            Me.valMinLodThreshold.DecimalPlaces = 2
            Me.valMinLodThreshold.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.valMinLodThreshold.Location = New System.Drawing.Point(170, 43)
            Me.valMinLodThreshold.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.valMinLodThreshold.Name = "valMinLodThreshold"
            Me.valMinLodThreshold.Size = New System.Drawing.Size(66, 20)
            Me.valMinLodThreshold.TabIndex = 2
            Me.valMinLodThreshold.Value = New Decimal(New Integer() {2, 0, 0, 65536})
            '
            'lblMinLodThreshold
            '
            Me.lblMinLodThreshold.AutoSize = True
            Me.lblMinLodThreshold.Location = New System.Drawing.Point(51, 47)
            Me.lblMinLodThreshold.Name = "lblMinLodThreshold"
            Me.lblMinLodThreshold.Size = New System.Drawing.Size(66, 13)
            Me.lblMinLodThreshold.TabIndex = 1
            Me.lblMinLodThreshold.Text = "Threshold ():"
            '
            'cmdBayesianProperties
            '
            Me.cmdBayesianProperties.Image = Global.GCDAddIn.My.Resources.Resources.Settings
            Me.cmdBayesianProperties.Location = New System.Drawing.Point(189, 141)
            Me.cmdBayesianProperties.Name = "cmdBayesianProperties"
            Me.cmdBayesianProperties.Size = New System.Drawing.Size(23, 23)
            Me.cmdBayesianProperties.TabIndex = 8
            Me.cmdBayesianProperties.UseVisualStyleBackColor = True
            '
            'chkBayesian
            '
            Me.chkBayesian.AutoSize = True
            Me.chkBayesian.Location = New System.Drawing.Point(51, 144)
            Me.chkBayesian.Name = "chkBayesian"
            Me.chkBayesian.Size = New System.Drawing.Size(138, 17)
            Me.chkBayesian.TabIndex = 7
            Me.chkBayesian.Text = "Use Bayesian updating:"
            Me.chkBayesian.UseVisualStyleBackColor = True
            '
            'valConfidence
            '
            Me.valConfidence.DecimalPlaces = 2
            Me.valConfidence.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.valConfidence.Location = New System.Drawing.Point(170, 112)
            Me.valConfidence.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.valConfidence.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.valConfidence.Name = "valConfidence"
            Me.valConfidence.Size = New System.Drawing.Size(66, 20)
            Me.valConfidence.TabIndex = 6
            Me.valConfidence.Value = New Decimal(New Integer() {95, 0, 0, 131072})
            '
            'lblConfidence
            '
            Me.lblConfidence.AutoSize = True
            Me.lblConfidence.Location = New System.Drawing.Point(51, 116)
            Me.lblConfidence.Name = "lblConfidence"
            Me.lblConfidence.Size = New System.Drawing.Size(113, 13)
            Me.lblConfidence.TabIndex = 5
            Me.lblConfidence.Text = "Confidence level (0-1):"
            '
            'rdoProbabilistic
            '
            Me.rdoProbabilistic.AutoSize = True
            Me.rdoProbabilistic.Location = New System.Drawing.Point(17, 92)
            Me.rdoProbabilistic.Name = "rdoProbabilistic"
            Me.rdoProbabilistic.Size = New System.Drawing.Size(141, 17)
            Me.rdoProbabilistic.TabIndex = 4
            Me.rdoProbabilistic.Text = "Probabilistic thresholding"
            Me.rdoProbabilistic.UseVisualStyleBackColor = True
            '
            'rdoPropagated
            '
            Me.rdoPropagated.AutoSize = True
            Me.rdoPropagated.Location = New System.Drawing.Point(17, 69)
            Me.rdoPropagated.Name = "rdoPropagated"
            Me.rdoPropagated.Size = New System.Drawing.Size(109, 17)
            Me.rdoPropagated.TabIndex = 3
            Me.rdoPropagated.Text = "Propagated errors"
            Me.rdoPropagated.UseVisualStyleBackColor = True
            '
            'rdoMinLOD
            '
            Me.rdoMinLOD.AutoSize = True
            Me.rdoMinLOD.Checked = True
            Me.rdoMinLOD.Location = New System.Drawing.Point(17, 22)
            Me.rdoMinLOD.Name = "rdoMinLOD"
            Me.rdoMinLOD.Size = New System.Drawing.Size(183, 17)
            Me.rdoMinLOD.TabIndex = 0
            Me.rdoMinLOD.TabStop = True
            Me.rdoMinLOD.Text = "Simple minimum level of detection"
            Me.rdoMinLOD.UseVisualStyleBackColor = True
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(12, 22)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(77, 13)
            Me.Label5.TabIndex = 0
            Me.Label5.Text = "Analysis name:"
            '
            'txtName
            '
            Me.txtName.Location = New System.Drawing.Point(98, 18)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(451, 20)
            Me.txtName.TabIndex = 1
            '
            'GroupBox4
            '
            Me.GroupBox4.Controls.Add(Me.cboOldDEM)
            Me.GroupBox4.Controls.Add(Me.cboOldError)
            Me.GroupBox4.Controls.Add(Me.lblOldError)
            Me.GroupBox4.Controls.Add(Me.Label7)
            Me.GroupBox4.Location = New System.Drawing.Point(283, 74)
            Me.GroupBox4.Name = "GroupBox4"
            Me.GroupBox4.Size = New System.Drawing.Size(266, 93)
            Me.GroupBox4.TabIndex = 5
            Me.GroupBox4.TabStop = False
            Me.GroupBox4.Text = "Old Survey"
            '
            'cboOldDEM
            '
            Me.cboOldDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboOldDEM.FormattingEnabled = True
            Me.cboOldDEM.Location = New System.Drawing.Point(52, 30)
            Me.cboOldDEM.Name = "cboOldDEM"
            Me.cboOldDEM.Size = New System.Drawing.Size(201, 21)
            Me.cboOldDEM.TabIndex = 1
            '
            'cboOldError
            '
            Me.cboOldError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboOldError.FormattingEnabled = True
            Me.cboOldError.Location = New System.Drawing.Point(52, 57)
            Me.cboOldError.Name = "cboOldError"
            Me.cboOldError.Size = New System.Drawing.Size(201, 21)
            Me.cboOldError.TabIndex = 3
            '
            'lblOldError
            '
            Me.lblOldError.AutoSize = True
            Me.lblOldError.Location = New System.Drawing.Point(14, 57)
            Me.lblOldError.Name = "lblOldError"
            Me.lblOldError.Size = New System.Drawing.Size(32, 13)
            Me.lblOldError.TabIndex = 2
            Me.lblOldError.Text = "Error:"
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(12, 30)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(34, 13)
            Me.Label7.TabIndex = 0
            Me.Label7.Text = "DEM:"
            '
            'cmdCancel
            '
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(474, 475)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 9
            Me.cmdCancel.Text = "Cancel"
            Me.cmdCancel.UseVisualStyleBackColor = True
            '
            'cmdOK
            '
            Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOK.Location = New System.Drawing.Point(393, 475)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.Size = New System.Drawing.Size(75, 23)
            Me.cmdOK.TabIndex = 8
            Me.cmdOK.Text = "Calculate"
            Me.cmdOK.UseVisualStyleBackColor = True
            '
            'cmdHelp
            '
            Me.cmdHelp.Location = New System.Drawing.Point(12, 475)
            Me.cmdHelp.Name = "cmdHelp"
            Me.cmdHelp.Size = New System.Drawing.Size(75, 23)
            Me.cmdHelp.TabIndex = 10
            Me.cmdHelp.Text = "Help"
            Me.cmdHelp.UseVisualStyleBackColor = True
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.lstAOI)
            Me.GroupBox2.Location = New System.Drawing.Point(12, 175)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(537, 103)
            Me.GroupBox2.TabIndex = 6
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Spatial Extent of Analysis"
            '
            'lstAOI
            '
            Me.lstAOI.CheckOnClick = True
            Me.lstAOI.Enabled = False
            Me.lstAOI.FormattingEnabled = True
            Me.lstAOI.Location = New System.Drawing.Point(17, 25)
            Me.lstAOI.Name = "lstAOI"
            Me.lstAOI.Size = New System.Drawing.Size(507, 64)
            Me.lstAOI.TabIndex = 0
            '
            'txtOutputFolder
            '
            Me.txtOutputFolder.Location = New System.Drawing.Point(98, 46)
            Me.txtOutputFolder.Name = "txtOutputFolder"
            Me.txtOutputFolder.ReadOnly = True
            Me.txtOutputFolder.Size = New System.Drawing.Size(451, 20)
            Me.txtOutputFolder.TabIndex = 3
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(12, 50)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(71, 13)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Output folder:"
            '
            'DoDPropertiesForm
            '
            Me.AcceptButton = Me.cmdOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdCancel
            Me.ClientSize = New System.Drawing.Size(562, 512)
            Me.Controls.Add(Me.txtOutputFolder)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.cmdHelp)
            Me.Controls.Add(Me.cmdOK)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.GroupBox4)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.GroupBox3)
            Me.Controls.Add(Me.GroupBox1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "DoDPropertiesForm"
            Me.Text = "Change Detection Configuration"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.GroupBox3.ResumeLayout(False)
            Me.GroupBox3.PerformLayout()
            CType(Me.valMinLodThreshold, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.valConfidence, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox4.ResumeLayout(False)
            Me.GroupBox4.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents cboNewDEM As System.Windows.Forms.ComboBox
        Friend WithEvents cboNewError As System.Windows.Forms.ComboBox
        Friend WithEvents lblNewError As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
        Friend WithEvents cmdBayesianProperties As System.Windows.Forms.Button
        Friend WithEvents chkBayesian As System.Windows.Forms.CheckBox
        Friend WithEvents valConfidence As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblConfidence As System.Windows.Forms.Label
        Friend WithEvents rdoProbabilistic As System.Windows.Forms.RadioButton
        Friend WithEvents rdoPropagated As System.Windows.Forms.RadioButton
        Friend WithEvents rdoMinLOD As System.Windows.Forms.RadioButton
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
        Friend WithEvents cboOldDEM As System.Windows.Forms.ComboBox
        Friend WithEvents cboOldError As System.Windows.Forms.ComboBox
        Friend WithEvents lblOldError As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents cmdCancel As System.Windows.Forms.Button
        Friend WithEvents cmdOK As System.Windows.Forms.Button
        Friend WithEvents cmdHelp As System.Windows.Forms.Button
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents txtOutputFolder As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents valMinLodThreshold As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblMinLodThreshold As System.Windows.Forms.Label
        Friend WithEvents lstAOI As System.Windows.Forms.CheckedListBox
    End Class
End Namespace