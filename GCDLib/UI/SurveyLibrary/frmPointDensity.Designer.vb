Namespace UI.SurveyLibrary

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmPointDensity
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
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblDistance = New System.Windows.Forms.Label()
            Me.valSampleDistance = New System.Windows.Forms.NumericUpDown()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.ttpToolTip = New System.Windows.Forms.ToolTip(Me.components)
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.cboNeighbourhood = New System.Windows.Forms.ComboBox()
            Me.ucPointCloud = New GCDAddIn.GISCode.UserInterface.VectorInputUC()
            CType(Me.valSampleDistance, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(34, 17)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(63, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Point cloud:"
            '
            'lblDistance
            '
            Me.lblDistance.AutoSize = True
            Me.lblDistance.Location = New System.Drawing.Point(17, 56)
            Me.lblDistance.Name = "lblDistance"
            Me.lblDistance.Size = New System.Drawing.Size(52, 13)
            Me.lblDistance.TabIndex = 2
            Me.lblDistance.Text = "Distance:"
            '
            'valSampleDistance
            '
            Me.valSampleDistance.DecimalPlaces = 1
            Me.valSampleDistance.Location = New System.Drawing.Point(95, 52)
            Me.valSampleDistance.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.valSampleDistance.Name = "valSampleDistance"
            Me.valSampleDistance.Size = New System.Drawing.Size(108, 20)
            Me.valSampleDistance.TabIndex = 3
            Me.valSampleDistance.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(397, 140)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOK
            '
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(316, 140)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 3
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnHelp
            '
            Me.btnHelp.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnHelp.Enabled = False
            Me.btnHelp.Location = New System.Drawing.Point(12, 140)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(75, 23)
            Me.btnHelp.TabIndex = 5
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Controls.Add(Me.cboNeighbourhood)
            Me.GroupBox1.Controls.Add(Me.valSampleDistance)
            Me.GroupBox1.Controls.Add(Me.lblDistance)
            Me.GroupBox1.Location = New System.Drawing.Point(15, 39)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(455, 90)
            Me.GroupBox1.TabIndex = 2
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Neighborhood:"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(17, 25)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(34, 13)
            Me.Label3.TabIndex = 0
            Me.Label3.Text = "Type:"
            '
            'cboNeighbourhood
            '
            Me.cboNeighbourhood.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboNeighbourhood.FormattingEnabled = True
            Me.cboNeighbourhood.Location = New System.Drawing.Point(95, 21)
            Me.cboNeighbourhood.Name = "cboNeighbourhood"
            Me.cboNeighbourhood.Size = New System.Drawing.Size(327, 21)
            Me.cboNeighbourhood.TabIndex = 1
            '
            'ucPointCloud
            '
            Me.ucPointCloud.ArcMap = Nothing
            Me.ucPointCloud.BrowseType = GCDAddIn.GISCode.GISDataStructures.BrowseVectorTypes.Point
            Me.ucPointCloud.Location = New System.Drawing.Point(110, 11)
            Me.ucPointCloud.Name = "ucPointCloud"
            Me.ucPointCloud.Noun = ""
            Me.ucPointCloud.Size = New System.Drawing.Size(360, 25)
            Me.ucPointCloud.TabIndex = 1
            Me.ucPointCloud.ToolTip = ""
            '
            'PointDensityForm
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(483, 175)
            Me.Controls.Add(Me.ucPointCloud)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "PointDensityForm"
            Me.ShowIcon = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Generate Point Density Raster"
            CType(Me.valSampleDistance, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblDistance As System.Windows.Forms.Label
        Friend WithEvents valSampleDistance As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents btnHelp As System.Windows.Forms.Button
        Friend WithEvents ttpToolTip As System.Windows.Forms.ToolTip
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents cboNeighbourhood As System.Windows.Forms.ComboBox
        Friend WithEvents ucPointCloud As GISCode.UserInterface.VectorInputUC
    End Class

End Namespace