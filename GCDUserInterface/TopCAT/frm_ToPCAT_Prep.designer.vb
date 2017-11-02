Namespace TopCAT

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frm_ToPCAT_Prep
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
            Me.btn_Cancel = New System.Windows.Forms.Button()
            Me.btn_Run = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtBox_RawPointCloud = New System.Windows.Forms.TextBox()
            Me.btn_RawPointCloud = New System.Windows.Forms.Button()
            Me.btn_OutputSpaceDelim = New System.Windows.Forms.Button()
            Me.txtBox_OutputSpaceDelim = New System.Windows.Forms.TextBox()
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.btnPreviewFile = New System.Windows.Forms.Button()
            Me.cmbBox_SelectSeparator = New System.Windows.Forms.ComboBox()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'btn_Cancel
            '
            Me.btn_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btn_Cancel.Location = New System.Drawing.Point(538, 190)
            Me.btn_Cancel.Name = "btn_Cancel"
            Me.btn_Cancel.Size = New System.Drawing.Size(75, 23)
            Me.btn_Cancel.TabIndex = 5
            Me.btn_Cancel.Text = "Cancel"
            Me.btn_Cancel.UseVisualStyleBackColor = True
            '
            'btn_Run
            '
            Me.btn_Run.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn_Run.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btn_Run.Location = New System.Drawing.Point(454, 190)
            Me.btn_Run.Name = "btn_Run"
            Me.btn_Run.Size = New System.Drawing.Size(75, 23)
            Me.btn_Run.TabIndex = 4
            Me.btn_Run.Text = "Run Tool"
            Me.btn_Run.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(11, 63)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(110, 13)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "Point cloud separator:"
            '
            'txtBox_RawPointCloud
            '
            Me.txtBox_RawPointCloud.BackColor = System.Drawing.SystemColors.HighlightText
            Me.txtBox_RawPointCloud.Cursor = System.Windows.Forms.Cursors.Arrow
            Me.txtBox_RawPointCloud.Location = New System.Drawing.Point(170, 35)
            Me.txtBox_RawPointCloud.Name = "txtBox_RawPointCloud"
            Me.txtBox_RawPointCloud.Size = New System.Drawing.Size(394, 20)
            Me.txtBox_RawPointCloud.TabIndex = 2
            '
            'btn_RawPointCloud
            '
            Me.btn_RawPointCloud.Image = Global.GCDAddIn.My.Resources.Resources.BrowseFolder
            Me.btn_RawPointCloud.Location = New System.Drawing.Point(557, 23)
            Me.btn_RawPointCloud.Name = "btn_RawPointCloud"
            Me.btn_RawPointCloud.Size = New System.Drawing.Size(29, 23)
            Me.btn_RawPointCloud.TabIndex = 0
            Me.btn_RawPointCloud.UseVisualStyleBackColor = True
            '
            'btn_OutputSpaceDelim
            '
            Me.btn_OutputSpaceDelim.Image = Global.GCDAddIn.My.Resources.Resources.SaveGIS
            Me.btn_OutputSpaceDelim.Location = New System.Drawing.Point(557, 21)
            Me.btn_OutputSpaceDelim.Name = "btn_OutputSpaceDelim"
            Me.btn_OutputSpaceDelim.Size = New System.Drawing.Size(29, 23)
            Me.btn_OutputSpaceDelim.TabIndex = 2
            Me.btn_OutputSpaceDelim.UseVisualStyleBackColor = True
            '
            'txtBox_OutputSpaceDelim
            '
            Me.txtBox_OutputSpaceDelim.BackColor = System.Drawing.SystemColors.HighlightText
            Me.txtBox_OutputSpaceDelim.Cursor = System.Windows.Forms.Cursors.Arrow
            Me.txtBox_OutputSpaceDelim.Location = New System.Drawing.Point(157, 22)
            Me.txtBox_OutputSpaceDelim.Name = "txtBox_OutputSpaceDelim"
            Me.txtBox_OutputSpaceDelim.Size = New System.Drawing.Size(394, 20)
            Me.txtBox_OutputSpaceDelim.TabIndex = 1
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Image = Global.GCDAddIn.My.Resources.Resources.Help
            Me.btnHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnHelp.Location = New System.Drawing.Point(13, 190)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(58, 23)
            Me.btnHelp.TabIndex = 0
            Me.btnHelp.Text = "Help"
            Me.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(11, 28)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(87, 13)
            Me.Label2.TabIndex = 50
            Me.Label2.Text = "Raw point cloud:"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(11, 26)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(140, 13)
            Me.Label3.TabIndex = 0
            Me.Label3.Text = "Space delimited point cloud:"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.btnPreviewFile)
            Me.GroupBox1.Controls.Add(Me.cmbBox_SelectSeparator)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Controls.Add(Me.btn_RawPointCloud)
            Me.GroupBox1.Location = New System.Drawing.Point(13, 11)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(600, 97)
            Me.GroupBox1.TabIndex = 1
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Inputs"
            '
            'btnPreviewFile
            '
            Me.btnPreviewFile.Location = New System.Drawing.Point(474, 58)
            Me.btnPreviewFile.Name = "btnPreviewFile"
            Me.btnPreviewFile.Size = New System.Drawing.Size(112, 23)
            Me.btnPreviewFile.TabIndex = 3
            Me.btnPreviewFile.Text = "Preview First Lines"
            Me.btnPreviewFile.UseVisualStyleBackColor = True
            '
            'cmbBox_SelectSeparator
            '
            Me.cmbBox_SelectSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbBox_SelectSeparator.FormattingEnabled = True
            Me.cmbBox_SelectSeparator.Items.AddRange(New Object() {"Comma", "Period", "Semi-Colon", "Colon", "Tab"})
            Me.cmbBox_SelectSeparator.Location = New System.Drawing.Point(157, 59)
            Me.cmbBox_SelectSeparator.Name = "cmbBox_SelectSeparator"
            Me.cmbBox_SelectSeparator.Size = New System.Drawing.Size(311, 21)
            Me.cmbBox_SelectSeparator.TabIndex = 51
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.Label3)
            Me.GroupBox2.Controls.Add(Me.btn_OutputSpaceDelim)
            Me.GroupBox2.Controls.Add(Me.txtBox_OutputSpaceDelim)
            Me.GroupBox2.Location = New System.Drawing.Point(13, 116)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(600, 59)
            Me.GroupBox2.TabIndex = 3
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Output"
            '
            'frm_ToPCAT_Prep
            '
            Me.AcceptButton = Me.btn_Run
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btn_Cancel
            Me.ClientSize = New System.Drawing.Size(625, 225)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.btn_Cancel)
            Me.Controls.Add(Me.btn_Run)
            Me.Controls.Add(Me.txtBox_RawPointCloud)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.GroupBox2)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frm_ToPCAT_Prep"
            Me.Text = "ToPCAT Prep"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btn_Cancel As System.Windows.Forms.Button
        Friend WithEvents btn_Run As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtBox_RawPointCloud As System.Windows.Forms.TextBox
        Friend WithEvents btn_RawPointCloud As System.Windows.Forms.Button
        Friend WithEvents btn_OutputSpaceDelim As System.Windows.Forms.Button
        Friend WithEvents txtBox_OutputSpaceDelim As System.Windows.Forms.TextBox
        Friend WithEvents btnHelp As System.Windows.Forms.Button
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents cmbBox_SelectSeparator As System.Windows.Forms.ComboBox
        Friend WithEvents btnPreviewFile As System.Windows.Forms.Button
        Friend WithEvents tTip As System.Windows.Forms.ToolTip
    End Class

End Namespace