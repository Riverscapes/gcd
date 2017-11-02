Namespace UI.BudgetSegregation
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmBudgetSegProperties
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBudgetSegProperties))
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.txtUncertaintyAnalysis = New System.Windows.Forms.TextBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.txtOldDEM = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.txtNewDEM = New System.Windows.Forms.TextBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.cboDoD = New System.Windows.Forms.ComboBox()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.ucPolygon = New UI.UtilityForms.ucVectorInput
            Me.cmdHelp = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.cmdOK = New System.Windows.Forms.Button()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.cboField = New System.Windows.Forms.ComboBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.txtOutputFolder = New System.Windows.Forms.TextBox()
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(13, 13)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Name:"
            '
            'txtName
            '
            Me.txtName.Location = New System.Drawing.Point(95, 9)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(411, 20)
            Me.txtName.TabIndex = 1
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.txtUncertaintyAnalysis)
            Me.GroupBox1.Controls.Add(Me.Label5)
            Me.GroupBox1.Controls.Add(Me.txtOldDEM)
            Me.GroupBox1.Controls.Add(Me.Label4)
            Me.GroupBox1.Controls.Add(Me.txtNewDEM)
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.cboDoD)
            Me.GroupBox1.Location = New System.Drawing.Point(13, 41)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(490, 150)
            Me.GroupBox1.TabIndex = 2
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Change Detection Analysis"
            '
            'txtUncertaintyAnalysis
            '
            Me.txtUncertaintyAnalysis.Location = New System.Drawing.Point(120, 118)
            Me.txtUncertaintyAnalysis.Name = "txtUncertaintyAnalysis"
            Me.txtUncertaintyAnalysis.ReadOnly = True
            Me.txtUncertaintyAnalysis.Size = New System.Drawing.Size(355, 20)
            Me.txtUncertaintyAnalysis.TabIndex = 7
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(10, 122)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(104, 13)
            Me.Label5.TabIndex = 6
            Me.Label5.Text = "Uncertainty analysis:"
            '
            'txtOldDEM
            '
            Me.txtOldDEM.Location = New System.Drawing.Point(120, 88)
            Me.txtOldDEM.Name = "txtOldDEM"
            Me.txtOldDEM.ReadOnly = True
            Me.txtOldDEM.Size = New System.Drawing.Size(355, 20)
            Me.txtOldDEM.TabIndex = 5
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(10, 92)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(53, 13)
            Me.Label4.TabIndex = 4
            Me.Label4.Text = "Old DEM:"
            '
            'txtNewDEM
            '
            Me.txtNewDEM.Location = New System.Drawing.Point(120, 58)
            Me.txtNewDEM.Name = "txtNewDEM"
            Me.txtNewDEM.ReadOnly = True
            Me.txtNewDEM.Size = New System.Drawing.Size(355, 20)
            Me.txtNewDEM.TabIndex = 3
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(10, 62)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(59, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "New DEM:"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(10, 31)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(38, 13)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Name:"
            '
            'cboDoD
            '
            Me.cboDoD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboDoD.FormattingEnabled = True
            Me.cboDoD.Location = New System.Drawing.Point(120, 27)
            Me.cboDoD.Name = "cboDoD"
            Me.cboDoD.Size = New System.Drawing.Size(355, 21)
            Me.cboDoD.TabIndex = 1
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(13, 27)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(74, 13)
            Me.Label6.TabIndex = 3
            Me.Label6.Text = "Feature Class:"
            '
            'ucPolygon
            '
            Me.ucPolygon.Location = New System.Drawing.Point(120, 21)
            Me.ucPolygon.Name = "ucPolygon"
            Me.ucPolygon.Size = New System.Drawing.Size(355, 25)
            Me.ucPolygon.TabIndex = 0
            '
            'cmdHelp
            '
            Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdHelp.Location = New System.Drawing.Point(12, 340)
            Me.cmdHelp.Name = "cmdHelp"
            Me.cmdHelp.Size = New System.Drawing.Size(75, 23)
            Me.cmdHelp.TabIndex = 0
            Me.cmdHelp.Text = "Help"
            Me.cmdHelp.UseVisualStyleBackColor = True
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(430, 340)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 8
            Me.cmdCancel.Text = "Cancel"
            Me.cmdCancel.UseVisualStyleBackColor = True
            '
            'cmdOK
            '
            Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOK.Location = New System.Drawing.Point(349, 340)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.Size = New System.Drawing.Size(75, 23)
            Me.cmdOK.TabIndex = 7
            Me.cmdOK.Text = "OK"
            Me.cmdOK.UseVisualStyleBackColor = True
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(13, 57)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(32, 13)
            Me.Label7.TabIndex = 1
            Me.Label7.Text = "Field:"
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.cboField)
            Me.GroupBox2.Controls.Add(Me.Label7)
            Me.GroupBox2.Controls.Add(Me.Label6)
            Me.GroupBox2.Controls.Add(Me.ucPolygon)
            Me.GroupBox2.Location = New System.Drawing.Point(13, 200)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(493, 88)
            Me.GroupBox2.TabIndex = 4
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Polygon Mask"
            '
            'cboField
            '
            Me.cboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboField.FormattingEnabled = True
            Me.cboField.Location = New System.Drawing.Point(120, 53)
            Me.cboField.Name = "cboField"
            Me.cboField.Size = New System.Drawing.Size(355, 21)
            Me.cboField.TabIndex = 2
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(13, 304)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(71, 13)
            Me.Label8.TabIndex = 5
            Me.Label8.Text = "Output folder:"
            '
            'txtOutputFolder
            '
            Me.txtOutputFolder.Location = New System.Drawing.Point(95, 300)
            Me.txtOutputFolder.Name = "txtOutputFolder"
            Me.txtOutputFolder.ReadOnly = True
            Me.txtOutputFolder.Size = New System.Drawing.Size(411, 20)
            Me.txtOutputFolder.TabIndex = 6
            '
            'BudgetSegPropertiesForm
            '
            Me.AcceptButton = Me.cmdOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdCancel
            Me.ClientSize = New System.Drawing.Size(517, 375)
            Me.Controls.Add(Me.txtOutputFolder)
            Me.Controls.Add(Me.Label8)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.cmdOK)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdHelp)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "BudgetSegPropertiesForm"
            Me.Text = "Budget Segregation Properties"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents txtUncertaintyAnalysis As System.Windows.Forms.TextBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtOldDEM As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtNewDEM As System.Windows.Forms.TextBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents cboDoD As System.Windows.Forms.ComboBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents ucPolygon As UI.UtilityForms.ucVectorInput
        Friend WithEvents cmdHelp As System.Windows.Forms.Button
        Friend WithEvents cmdCancel As System.Windows.Forms.Button
        Friend WithEvents cmdOK As System.Windows.Forms.Button
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents cboField As System.Windows.Forms.ComboBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents txtOutputFolder As System.Windows.Forms.TextBox
    End Class
End Namespace