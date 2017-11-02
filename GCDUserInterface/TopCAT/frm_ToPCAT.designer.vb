Namespace TopCAT

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frm_ToPCAT
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
            Me.btn_Run = New System.Windows.Forms.Button()
            Me.btn_Cancel = New System.Windows.Forms.Button()
            Me.btn_SelectOutputPath = New System.Windows.Forms.Button()
            Me.btn_OutputFileDescription = New System.Windows.Forms.Button()
            Me.txtBox_SelectOutputPath = New System.Windows.Forms.TextBox()
            Me.txtBox_SpatialReference = New System.Windows.Forms.TextBox()
            Me.btn_SpatialReference = New System.Windows.Forms.Button()
            Me.chkIncludeBinarySorted = New System.Windows.Forms.CheckBox()
            Me.chkIncludeBinaryZstats = New System.Windows.Forms.CheckBox()
            Me.chkIncludeTextVersions = New System.Windows.Forms.CheckBox()
            Me.chkCreateZunderPopulatedShp = New System.Windows.Forms.CheckBox()
            Me.chkCreateZstatShp = New System.Windows.Forms.CheckBox()
            Me.chkCreateZmaxShp = New System.Windows.Forms.CheckBox()
            Me.chkCreateZminShp = New System.Windows.Forms.CheckBox()
            Me.btn_Help = New System.Windows.Forms.Button()
            Me.grbOutputs = New System.Windows.Forms.GroupBox()
            Me.grbBinaryFiles = New System.Windows.Forms.GroupBox()
            Me.grbShapefiles = New System.Windows.Forms.GroupBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.grbTextFiles = New System.Windows.Forms.GroupBox()
            Me.lblSpatialRef = New System.Windows.Forms.Label()
            Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.ucInputsWindow = New TopCAT.ucToPCAT_Inputs()
            Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
            Me.grbOutputs.SuspendLayout()
            Me.grbBinaryFiles.SuspendLayout()
            Me.grbShapefiles.SuspendLayout()
            Me.grbTextFiles.SuspendLayout()
            Me.SuspendLayout()
            '
            'btn_Run
            '
            Me.btn_Run.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn_Run.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btn_Run.Location = New System.Drawing.Point(329, 420)
            Me.btn_Run.Name = "btn_Run"
            Me.btn_Run.Size = New System.Drawing.Size(75, 23)
            Me.btn_Run.TabIndex = 5
            Me.btn_Run.Text = "Run"
            Me.btn_Run.UseVisualStyleBackColor = True
            '
            'btn_Cancel
            '
            Me.btn_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btn_Cancel.Location = New System.Drawing.Point(410, 420)
            Me.btn_Cancel.Name = "btn_Cancel"
            Me.btn_Cancel.Size = New System.Drawing.Size(75, 23)
            Me.btn_Cancel.TabIndex = 6
            Me.btn_Cancel.Text = "Cancel"
            Me.btn_Cancel.UseVisualStyleBackColor = True
            '
            'btn_SelectOutputPath
            '
            Me.btn_SelectOutputPath.Image = Global.GCDAddIn.My.Resources.Resources.SaveGIS
            Me.btn_SelectOutputPath.Location = New System.Drawing.Point(439, 191)
            Me.btn_SelectOutputPath.Name = "btn_SelectOutputPath"
            Me.btn_SelectOutputPath.Size = New System.Drawing.Size(23, 23)
            Me.btn_SelectOutputPath.TabIndex = 3
            Me.btn_SelectOutputPath.UseVisualStyleBackColor = True
            '
            'btn_OutputFileDescription
            '
            Me.btn_OutputFileDescription.Image = Global.GCDAddIn.My.Resources.Resources.Help
            Me.btn_OutputFileDescription.Location = New System.Drawing.Point(439, 101)
            Me.btn_OutputFileDescription.Name = "btn_OutputFileDescription"
            Me.btn_OutputFileDescription.Size = New System.Drawing.Size(23, 23)
            Me.btn_OutputFileDescription.TabIndex = 10
            Me.btn_OutputFileDescription.UseVisualStyleBackColor = True
            '
            'txtBox_SelectOutputPath
            '
            Me.txtBox_SelectOutputPath.BackColor = System.Drawing.SystemColors.HighlightText
            Me.txtBox_SelectOutputPath.Cursor = System.Windows.Forms.Cursors.Arrow
            Me.txtBox_SelectOutputPath.Location = New System.Drawing.Point(112, 193)
            Me.txtBox_SelectOutputPath.Name = "txtBox_SelectOutputPath"
            Me.txtBox_SelectOutputPath.ReadOnly = True
            Me.txtBox_SelectOutputPath.Size = New System.Drawing.Size(321, 20)
            Me.txtBox_SelectOutputPath.TabIndex = 20
            '
            'txtBox_SpatialReference
            '
            Me.txtBox_SpatialReference.BackColor = System.Drawing.SystemColors.HighlightText
            Me.txtBox_SpatialReference.Cursor = System.Windows.Forms.Cursors.Arrow
            Me.txtBox_SpatialReference.Location = New System.Drawing.Point(112, 157)
            Me.txtBox_SpatialReference.Name = "txtBox_SpatialReference"
            Me.txtBox_SpatialReference.Size = New System.Drawing.Size(321, 20)
            Me.txtBox_SpatialReference.TabIndex = 19
            '
            'btn_SpatialReference
            '
            Me.btn_SpatialReference.Image = Global.GCDAddIn.My.Resources.Resources.BrowseFolder
            Me.btn_SpatialReference.Location = New System.Drawing.Point(439, 154)
            Me.btn_SpatialReference.Name = "btn_SpatialReference"
            Me.btn_SpatialReference.Size = New System.Drawing.Size(23, 23)
            Me.btn_SpatialReference.TabIndex = 18
            Me.btn_SpatialReference.UseVisualStyleBackColor = True
            '
            'chkIncludeBinarySorted
            '
            Me.chkIncludeBinarySorted.AutoSize = True
            Me.chkIncludeBinarySorted.Location = New System.Drawing.Point(6, 42)
            Me.chkIncludeBinarySorted.Name = "chkIncludeBinarySorted"
            Me.chkIncludeBinarySorted.Size = New System.Drawing.Size(112, 17)
            Me.chkIncludeBinarySorted.TabIndex = 17
            Me.chkIncludeBinarySorted.Text = "Sorted point cloud"
            Me.chkIncludeBinarySorted.UseVisualStyleBackColor = True
            '
            'chkIncludeBinaryZstats
            '
            Me.chkIncludeBinaryZstats.AutoSize = True
            Me.chkIncludeBinaryZstats.Location = New System.Drawing.Point(6, 19)
            Me.chkIncludeBinaryZstats.Name = "chkIncludeBinaryZstats"
            Me.chkIncludeBinaryZstats.Size = New System.Drawing.Size(53, 17)
            Me.chkIncludeBinaryZstats.TabIndex = 16
            Me.chkIncludeBinaryZstats.Text = "Z stat"
            Me.chkIncludeBinaryZstats.UseVisualStyleBackColor = True
            '
            'chkIncludeTextVersions
            '
            Me.chkIncludeTextVersions.AutoSize = True
            Me.chkIncludeTextVersions.Checked = True
            Me.chkIncludeTextVersions.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkIncludeTextVersions.Location = New System.Drawing.Point(6, 19)
            Me.chkIncludeTextVersions.Name = "chkIncludeTextVersions"
            Me.chkIncludeTextVersions.Size = New System.Drawing.Size(101, 17)
            Me.chkIncludeTextVersions.TabIndex = 15
            Me.chkIncludeTextVersions.Text = "include .txt files "
            Me.chkIncludeTextVersions.UseVisualStyleBackColor = True
            '
            'chkCreateZunderPopulatedShp
            '
            Me.chkCreateZunderPopulatedShp.AutoSize = True
            Me.chkCreateZunderPopulatedShp.Checked = True
            Me.chkCreateZunderPopulatedShp.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkCreateZunderPopulatedShp.Location = New System.Drawing.Point(6, 88)
            Me.chkCreateZunderPopulatedShp.Name = "chkCreateZunderPopulatedShp"
            Me.chkCreateZunderPopulatedShp.Size = New System.Drawing.Size(137, 17)
            Me.chkCreateZunderPopulatedShp.TabIndex = 14
            Me.chkCreateZunderPopulatedShp.Text = "Underpopulated Z stats"
            Me.chkCreateZunderPopulatedShp.UseVisualStyleBackColor = True
            '
            'chkCreateZstatShp
            '
            Me.chkCreateZstatShp.AutoSize = True
            Me.chkCreateZstatShp.Checked = True
            Me.chkCreateZstatShp.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkCreateZstatShp.Location = New System.Drawing.Point(6, 19)
            Me.chkCreateZstatShp.Name = "chkCreateZstatShp"
            Me.chkCreateZstatShp.Size = New System.Drawing.Size(53, 17)
            Me.chkCreateZstatShp.TabIndex = 13
            Me.chkCreateZstatShp.Text = "Z stat"
            Me.chkCreateZstatShp.UseVisualStyleBackColor = True
            '
            'chkCreateZmaxShp
            '
            Me.chkCreateZmaxShp.AutoSize = True
            Me.chkCreateZmaxShp.Checked = True
            Me.chkCreateZmaxShp.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkCreateZmaxShp.Location = New System.Drawing.Point(6, 65)
            Me.chkCreateZmaxShp.Name = "chkCreateZmaxShp"
            Me.chkCreateZmaxShp.Size = New System.Drawing.Size(55, 17)
            Me.chkCreateZmaxShp.TabIndex = 12
            Me.chkCreateZmaxShp.Text = "Z max"
            Me.chkCreateZmaxShp.UseVisualStyleBackColor = True
            '
            'chkCreateZminShp
            '
            Me.chkCreateZminShp.AutoSize = True
            Me.chkCreateZminShp.Checked = True
            Me.chkCreateZminShp.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkCreateZminShp.Location = New System.Drawing.Point(6, 42)
            Me.chkCreateZminShp.Name = "chkCreateZminShp"
            Me.chkCreateZminShp.Size = New System.Drawing.Size(52, 17)
            Me.chkCreateZminShp.TabIndex = 11
            Me.chkCreateZminShp.Text = "Z min"
            Me.chkCreateZminShp.UseVisualStyleBackColor = True
            '
            'btn_Help
            '
            Me.btn_Help.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btn_Help.Image = Global.GCDAddIn.My.Resources.Resources.Help
            Me.btn_Help.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btn_Help.Location = New System.Drawing.Point(11, 420)
            Me.btn_Help.Name = "btn_Help"
            Me.btn_Help.Size = New System.Drawing.Size(60, 23)
            Me.btn_Help.TabIndex = 35
            Me.btn_Help.Text = "Help"
            Me.btn_Help.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btn_Help.UseVisualStyleBackColor = True
            '
            'grbOutputs
            '
            Me.grbOutputs.Controls.Add(Me.grbBinaryFiles)
            Me.grbOutputs.Controls.Add(Me.grbShapefiles)
            Me.grbOutputs.Controls.Add(Me.Label3)
            Me.grbOutputs.Controls.Add(Me.grbTextFiles)
            Me.grbOutputs.Controls.Add(Me.lblSpatialRef)
            Me.grbOutputs.Controls.Add(Me.btn_OutputFileDescription)
            Me.grbOutputs.Controls.Add(Me.txtBox_SpatialReference)
            Me.grbOutputs.Controls.Add(Me.txtBox_SelectOutputPath)
            Me.grbOutputs.Controls.Add(Me.btn_SelectOutputPath)
            Me.grbOutputs.Controls.Add(Me.btn_SpatialReference)
            Me.grbOutputs.Location = New System.Drawing.Point(12, 184)
            Me.grbOutputs.Name = "grbOutputs"
            Me.grbOutputs.Size = New System.Drawing.Size(477, 221)
            Me.grbOutputs.TabIndex = 38
            Me.grbOutputs.TabStop = False
            Me.grbOutputs.Text = "Outputs"
            '
            'grbBinaryFiles
            '
            Me.grbBinaryFiles.Controls.Add(Me.chkIncludeBinaryZstats)
            Me.grbBinaryFiles.Controls.Add(Me.chkIncludeBinarySorted)
            Me.grbBinaryFiles.Location = New System.Drawing.Point(315, 16)
            Me.grbBinaryFiles.Name = "grbBinaryFiles"
            Me.grbBinaryFiles.Size = New System.Drawing.Size(147, 82)
            Me.grbBinaryFiles.TabIndex = 26
            Me.grbBinaryFiles.TabStop = False
            Me.grbBinaryFiles.Text = "Binary Files"
            '
            'grbShapefiles
            '
            Me.grbShapefiles.Controls.Add(Me.chkCreateZminShp)
            Me.grbShapefiles.Controls.Add(Me.chkCreateZmaxShp)
            Me.grbShapefiles.Controls.Add(Me.chkCreateZstatShp)
            Me.grbShapefiles.Controls.Add(Me.chkCreateZunderPopulatedShp)
            Me.grbShapefiles.Location = New System.Drawing.Point(9, 16)
            Me.grbShapefiles.Name = "grbShapefiles"
            Me.grbShapefiles.Size = New System.Drawing.Size(147, 109)
            Me.grbShapefiles.TabIndex = 24
            Me.grbShapefiles.TabStop = False
            Me.grbShapefiles.Text = "Shapefiles"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(6, 191)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(71, 13)
            Me.Label3.TabIndex = 22
            Me.Label3.Text = "Output folder:"
            '
            'grbTextFiles
            '
            Me.grbTextFiles.Controls.Add(Me.chkIncludeTextVersions)
            Me.grbTextFiles.Location = New System.Drawing.Point(162, 16)
            Me.grbTextFiles.Name = "grbTextFiles"
            Me.grbTextFiles.Size = New System.Drawing.Size(147, 109)
            Me.grbTextFiles.TabIndex = 25
            Me.grbTextFiles.TabStop = False
            Me.grbTextFiles.Text = "Text Files"
            '
            'lblSpatialRef
            '
            Me.lblSpatialRef.AutoSize = True
            Me.lblSpatialRef.Location = New System.Drawing.Point(6, 159)
            Me.lblSpatialRef.Name = "lblSpatialRef"
            Me.lblSpatialRef.Size = New System.Drawing.Size(90, 13)
            Me.lblSpatialRef.TabIndex = 21
            Me.lblSpatialRef.Text = "Spatial reference:"
            '
            'ucInputsWindow
            '
            Me.ucInputsWindow.Location = New System.Drawing.Point(11, 0)
            Me.ucInputsWindow.Name = "ucInputsWindow"
            Me.ucInputsWindow.Size = New System.Drawing.Size(478, 184)
            Me.ucInputsWindow.TabIndex = 39
            Me.ucInputsWindow.Units = "lblUnits"
            '
            'frm_ToPCAT
            '
            Me.AcceptButton = Me.btn_Run
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScroll = True
            Me.CancelButton = Me.btn_Cancel
            Me.ClientSize = New System.Drawing.Size(497, 455)
            Me.Controls.Add(Me.ucInputsWindow)
            Me.Controls.Add(Me.grbOutputs)
            Me.Controls.Add(Me.btn_Help)
            Me.Controls.Add(Me.btn_Cancel)
            Me.Controls.Add(Me.btn_Run)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frm_ToPCAT"
            Me.Text = "ToPCAT Decimation"
            Me.grbOutputs.ResumeLayout(False)
            Me.grbOutputs.PerformLayout()
            Me.grbBinaryFiles.ResumeLayout(False)
            Me.grbBinaryFiles.PerformLayout()
            Me.grbShapefiles.ResumeLayout(False)
            Me.grbShapefiles.PerformLayout()
            Me.grbTextFiles.ResumeLayout(False)
            Me.grbTextFiles.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btn_Run As System.Windows.Forms.Button
        Friend WithEvents btn_Cancel As System.Windows.Forms.Button
        Friend WithEvents btn_SelectOutputPath As System.Windows.Forms.Button
        Friend WithEvents btn_OutputFileDescription As System.Windows.Forms.Button
        Friend WithEvents chkCreateZstatShp As System.Windows.Forms.CheckBox
        Friend WithEvents chkCreateZmaxShp As System.Windows.Forms.CheckBox
        Friend WithEvents chkCreateZminShp As System.Windows.Forms.CheckBox
        Friend WithEvents chkIncludeBinarySorted As System.Windows.Forms.CheckBox
        Friend WithEvents chkIncludeBinaryZstats As System.Windows.Forms.CheckBox
        Friend WithEvents chkIncludeTextVersions As System.Windows.Forms.CheckBox
        Friend WithEvents chkCreateZunderPopulatedShp As System.Windows.Forms.CheckBox
        Friend WithEvents btn_SpatialReference As System.Windows.Forms.Button
        Friend WithEvents txtBox_SpatialReference As System.Windows.Forms.TextBox
        Friend WithEvents txtBox_SelectOutputPath As System.Windows.Forms.TextBox
        Friend WithEvents btn_Help As System.Windows.Forms.Button
        Friend WithEvents grbOutputs As System.Windows.Forms.GroupBox
        Friend WithEvents lblSpatialRef As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents tTip As System.Windows.Forms.ToolTip
        Friend WithEvents ucInputsWindow As ucToPCAT_Inputs
        Friend WithEvents grbShapefiles As System.Windows.Forms.GroupBox
        Friend WithEvents grbTextFiles As System.Windows.Forms.GroupBox
        Friend WithEvents grbBinaryFiles As System.Windows.Forms.GroupBox
        Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    End Class

End Namespace