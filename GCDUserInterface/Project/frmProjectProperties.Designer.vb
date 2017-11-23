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
            Me.Label6 = New System.Windows.Forms.Label()
            Me.cboHorizontalUnits = New System.Windows.Forms.ComboBox()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.groGCDUnits = New System.Windows.Forms.GroupBox()
            Me.cboVolumeUnits = New System.Windows.Forms.ComboBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.cboAreaUnits = New System.Windows.Forms.ComboBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.grpRasterUnits = New System.Windows.Forms.GroupBox()
            Me.cboVerticalUnits = New System.Windows.Forms.ComboBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.grdMetaData = New System.Windows.Forms.DataGridView()
            Me.colKey = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.TabControl1.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.groGCDUnits.SuspendLayout()
            Me.grpRasterUnits.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            CType(Me.grdMetaData, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(58, 14)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(35, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Name"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(12, 44)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(81, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Parent directory"
            '
            'txtName
            '
            Me.txtName.Location = New System.Drawing.Point(98, 10)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(394, 20)
            Me.txtName.TabIndex = 1
            '
            'txtDescription
            '
            Me.txtDescription.AcceptsReturn = True
            Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtDescription.Location = New System.Drawing.Point(3, 3)
            Me.txtDescription.MaxLength = 1000
            Me.txtDescription.Multiline = True
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(466, 166)
            Me.txtDescription.TabIndex = 8
            '
            'btnBrowseOutput
            '
            Me.btnBrowseOutput.Image = CType(resources.GetObject("btnBrowseOutput.Image"), System.Drawing.Image)
            Me.btnBrowseOutput.Location = New System.Drawing.Point(469, 39)
            Me.btnBrowseOutput.Name = "btnBrowseOutput"
            Me.btnBrowseOutput.Size = New System.Drawing.Size(23, 23)
            Me.btnBrowseOutput.TabIndex = 4
            Me.btnBrowseOutput.UseVisualStyleBackColor = True
            '
            'txtDirectory
            '
            Me.txtDirectory.Location = New System.Drawing.Point(98, 40)
            Me.txtDirectory.Name = "txtDirectory"
            Me.txtDirectory.Size = New System.Drawing.Size(365, 20)
            Me.txtDirectory.TabIndex = 3
            Me.txtDirectory.TabStop = False
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(417, 317)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 9
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(338, 317)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 8
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Location = New System.Drawing.Point(16, 317)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(75, 23)
            Me.btnHelp.TabIndex = 10
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'txtGCDPath
            '
            Me.txtGCDPath.Location = New System.Drawing.Point(98, 72)
            Me.txtGCDPath.Name = "txtGCDPath"
            Me.txtGCDPath.ReadOnly = True
            Me.txtGCDPath.Size = New System.Drawing.Size(394, 20)
            Me.txtGCDPath.TabIndex = 6
            Me.txtGCDPath.TabStop = False
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(12, 76)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(81, 13)
            Me.Label4.TabIndex = 5
            Me.Label4.Text = "GCD project file"
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(6, 23)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(54, 13)
            Me.Label6.TabIndex = 0
            Me.Label6.Text = "Horizontal"
            '
            'cboHorizontalUnits
            '
            Me.cboHorizontalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboHorizontalUnits.FormattingEnabled = True
            Me.cboHorizontalUnits.Location = New System.Drawing.Point(65, 19)
            Me.cboHorizontalUnits.Name = "cboHorizontalUnits"
            Me.cboHorizontalUnits.Size = New System.Drawing.Size(372, 21)
            Me.cboHorizontalUnits.TabIndex = 1
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.TabPage1)
            Me.TabControl1.Controls.Add(Me.TabPage2)
            Me.TabControl1.Controls.Add(Me.TabPage3)
            Me.TabControl1.Location = New System.Drawing.Point(12, 108)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(480, 198)
            Me.TabControl1.TabIndex = 7
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.groGCDUnits)
            Me.TabPage1.Controls.Add(Me.grpRasterUnits)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(472, 172)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Units"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'groGCDUnits
            '
            Me.groGCDUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.groGCDUnits.Controls.Add(Me.cboVolumeUnits)
            Me.groGCDUnits.Controls.Add(Me.Label7)
            Me.groGCDUnits.Controls.Add(Me.cboAreaUnits)
            Me.groGCDUnits.Controls.Add(Me.Label8)
            Me.groGCDUnits.Location = New System.Drawing.Point(6, 88)
            Me.groGCDUnits.Name = "groGCDUnits"
            Me.groGCDUnits.Size = New System.Drawing.Size(460, 76)
            Me.groGCDUnits.TabIndex = 1
            Me.groGCDUnits.TabStop = False
            Me.groGCDUnits.Text = "GCD Display Units"
            '
            'cboVolumeUnits
            '
            Me.cboVolumeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboVolumeUnits.FormattingEnabled = True
            Me.cboVolumeUnits.Location = New System.Drawing.Point(65, 46)
            Me.cboVolumeUnits.Name = "cboVolumeUnits"
            Me.cboVolumeUnits.Size = New System.Drawing.Size(372, 21)
            Me.cboVolumeUnits.TabIndex = 3
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(18, 50)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(42, 13)
            Me.Label7.TabIndex = 2
            Me.Label7.Text = "Volume"
            '
            'cboAreaUnits
            '
            Me.cboAreaUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAreaUnits.FormattingEnabled = True
            Me.cboAreaUnits.Location = New System.Drawing.Point(65, 19)
            Me.cboAreaUnits.Name = "cboAreaUnits"
            Me.cboAreaUnits.Size = New System.Drawing.Size(372, 21)
            Me.cboAreaUnits.TabIndex = 1
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(31, 23)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(29, 13)
            Me.Label8.TabIndex = 0
            Me.Label8.Text = "Area"
            '
            'grpRasterUnits
            '
            Me.grpRasterUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grpRasterUnits.Controls.Add(Me.cboVerticalUnits)
            Me.grpRasterUnits.Controls.Add(Me.Label2)
            Me.grpRasterUnits.Controls.Add(Me.cboHorizontalUnits)
            Me.grpRasterUnits.Controls.Add(Me.Label6)
            Me.grpRasterUnits.Location = New System.Drawing.Point(6, 6)
            Me.grpRasterUnits.Name = "grpRasterUnits"
            Me.grpRasterUnits.Size = New System.Drawing.Size(460, 76)
            Me.grpRasterUnits.TabIndex = 0
            Me.grpRasterUnits.TabStop = False
            Me.grpRasterUnits.Text = "Raster Units"
            '
            'cboVerticalUnits
            '
            Me.cboVerticalUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboVerticalUnits.FormattingEnabled = True
            Me.cboVerticalUnits.Location = New System.Drawing.Point(65, 46)
            Me.cboVerticalUnits.Name = "cboVerticalUnits"
            Me.cboVerticalUnits.Size = New System.Drawing.Size(372, 21)
            Me.cboVerticalUnits.TabIndex = 3
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(18, 50)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(42, 13)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Vertical"
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.txtDescription)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage2.Size = New System.Drawing.Size(472, 172)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Description"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.grdMetaData)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage3.Size = New System.Drawing.Size(472, 172)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "MetaData"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'grdMetaData
            '
            Me.grdMetaData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdMetaData.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colKey, Me.colValue})
            Me.grdMetaData.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdMetaData.Location = New System.Drawing.Point(3, 3)
            Me.grdMetaData.Name = "grdMetaData"
            Me.grdMetaData.Size = New System.Drawing.Size(466, 166)
            Me.grdMetaData.TabIndex = 0
            '
            'colKey
            '
            Me.colKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colKey.DataPropertyName = "Key"
            Me.colKey.HeaderText = "Key"
            Me.colKey.MaxInputLength = 255
            Me.colKey.Name = "colKey"
            '
            'colValue
            '
            Me.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colValue.DataPropertyName = "Value"
            Me.colValue.HeaderText = "Value"
            Me.colValue.MaxInputLength = 255
            Me.colValue.Name = "colValue"
            '
            'frmProjectProperties
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(504, 346)
            Me.Controls.Add(Me.TabControl1)
            Me.Controls.Add(Me.txtGCDPath)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.btnBrowseOutput)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.txtDirectory)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(1024, 599)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(520, 99)
            Me.Name = "frmProjectProperties"
            Me.Text = "GCD Project"
            Me.TabControl1.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.groGCDUnits.ResumeLayout(False)
            Me.groGCDUnits.PerformLayout()
            Me.grpRasterUnits.ResumeLayout(False)
            Me.grpRasterUnits.PerformLayout()
            Me.TabPage2.ResumeLayout(False)
            Me.TabPage2.PerformLayout()
            Me.TabPage3.ResumeLayout(False)
            CType(Me.grdMetaData, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
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
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents cboHorizontalUnits As System.Windows.Forms.ComboBox
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents grpRasterUnits As System.Windows.Forms.GroupBox
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents groGCDUnits As System.Windows.Forms.GroupBox
        Friend WithEvents cboVolumeUnits As System.Windows.Forms.ComboBox
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents cboAreaUnits As System.Windows.Forms.ComboBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents cboVerticalUnits As System.Windows.Forms.ComboBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents grdMetaData As System.Windows.Forms.DataGridView
        Friend WithEvents colKey As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colValue As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace