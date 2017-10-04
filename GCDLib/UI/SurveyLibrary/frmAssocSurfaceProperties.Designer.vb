Namespace UI.SurveyLibrary

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmAssocSurfaceProperties
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAssocSurfaceProperties))
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.cboType = New System.Windows.Forms.ComboBox()
            Me.btnBrowse = New System.Windows.Forms.Button()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.ttpTooltip = New System.Windows.Forms.ToolTip(Me.components)
            Me.txtOriginalRaster = New System.Windows.Forms.TextBox()
            Me.btnSlopePercent = New System.Windows.Forms.Button()
            Me.btnDensity = New System.Windows.Forms.Button()
            Me.txtProjectRaster = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.btnRoughness = New System.Windows.Forms.Button()
            Me.btnSlopeDegree = New System.Windows.Forms.Button()
            Me.ProjectDS = New Core.GCDProject.ProjectDS()
            Me.AssociatedSurfaceBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            CType(Me.ProjectDS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AssociatedSurfaceBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(12, 16)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Name:"
            '
            'txtName
            '
            Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtName.Location = New System.Drawing.Point(115, 12)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(483, 20)
            Me.txtName.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(12, 102)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(34, 13)
            Me.Label2.TabIndex = 11
            Me.Label2.Text = "Type:"
            '
            'cboType
            '
            Me.cboType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboType.FormattingEnabled = True
            Me.cboType.Items.AddRange(New Object() {"[Undefined]", "3D Point Quality", "Grain Size Statistic", "Interpolation Error", "Point Density", "Roughness", "Slope Percent Rise", "Slope Degrees"})
            Me.cboType.Location = New System.Drawing.Point(115, 98)
            Me.cboType.Name = "cboType"
            Me.cboType.Size = New System.Drawing.Size(483, 21)
            Me.cboType.TabIndex = 12
            '
            'btnBrowse
            '
            Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
            Me.btnBrowse.Location = New System.Drawing.Point(458, 39)
            Me.btnBrowse.Name = "btnBrowse"
            Me.btnBrowse.Size = New System.Drawing.Size(25, 22)
            Me.btnBrowse.TabIndex = 4
            Me.btnBrowse.UseVisualStyleBackColor = True
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(12, 44)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(80, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Original source:"
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Location = New System.Drawing.Point(15, 130)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(80, 22)
            Me.btnHelp.TabIndex = 15
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(432, 130)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(80, 22)
            Me.btnOK.TabIndex = 13
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(518, 130)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(80, 22)
            Me.btnCancel.TabIndex = 14
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'txtOriginalRaster
            '
            Me.txtOriginalRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtOriginalRaster.Location = New System.Drawing.Point(115, 40)
            Me.txtOriginalRaster.Name = "txtOriginalRaster"
            Me.txtOriginalRaster.ReadOnly = True
            Me.txtOriginalRaster.Size = New System.Drawing.Size(338, 20)
            Me.txtOriginalRaster.TabIndex = 3
            Me.txtOriginalRaster.TabStop = False
            '
            'btnSlopePercent
            '
            Me.btnSlopePercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSlopePercent.Image = Global.GCDAddIn.My.Resources.Resources.SlopePercent
            Me.btnSlopePercent.Location = New System.Drawing.Point(515, 39)
            Me.btnSlopePercent.Name = "btnSlopePercent"
            Me.btnSlopePercent.Size = New System.Drawing.Size(25, 22)
            Me.btnSlopePercent.TabIndex = 6
            Me.btnSlopePercent.UseVisualStyleBackColor = True
            '
            'btnDensity
            '
            Me.btnDensity.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDensity.Image = Global.GCDAddIn.My.Resources.Resources.PointDensity
            Me.btnDensity.Location = New System.Drawing.Point(544, 39)
            Me.btnDensity.Name = "btnDensity"
            Me.btnDensity.Size = New System.Drawing.Size(25, 22)
            Me.btnDensity.TabIndex = 7
            Me.btnDensity.UseVisualStyleBackColor = True
            '
            'txtProjectRaster
            '
            Me.txtProjectRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtProjectRaster.Location = New System.Drawing.Point(115, 69)
            Me.txtProjectRaster.Name = "txtProjectRaster"
            Me.txtProjectRaster.ReadOnly = True
            Me.txtProjectRaster.Size = New System.Drawing.Size(483, 20)
            Me.txtProjectRaster.TabIndex = 10
            Me.txtProjectRaster.TabStop = False
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(12, 73)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(96, 13)
            Me.Label4.TabIndex = 9
            Me.Label4.Text = "Project raster path:"
            '
            'btnRoughness
            '
            Me.btnRoughness.Image = Global.GCDAddIn.My.Resources.Resources.Roughness
            Me.btnRoughness.Location = New System.Drawing.Point(573, 39)
            Me.btnRoughness.Name = "btnRoughness"
            Me.btnRoughness.Size = New System.Drawing.Size(25, 22)
            Me.btnRoughness.TabIndex = 8
            Me.btnRoughness.UseVisualStyleBackColor = True
            '
            'btnSlopeDegree
            '
            Me.btnSlopeDegree.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSlopeDegree.Image = My.Resources.Resources.SlopeDegree
            Me.btnSlopeDegree.Location = New System.Drawing.Point(486, 39)
            Me.btnSlopeDegree.Name = "btnSlopeDegree"
            Me.btnSlopeDegree.Size = New System.Drawing.Size(25, 22)
            Me.btnSlopeDegree.TabIndex = 5
            Me.btnSlopeDegree.UseVisualStyleBackColor = True
            '
            'ProjectDS
            '
            Me.ProjectDS.DataSetName = "ProjectDS"
            Me.ProjectDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'AssociatedSurfaceBindingSource
            '
            Me.AssociatedSurfaceBindingSource.DataMember = "AssociatedSurface"
            Me.AssociatedSurfaceBindingSource.DataSource = Me.ProjectDS
            '
            'SurfacePropertiesForm
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(607, 162)
            Me.ControlBox = False
            Me.Controls.Add(Me.btnSlopeDegree)
            Me.Controls.Add(Me.btnRoughness)
            Me.Controls.Add(Me.txtProjectRaster)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.btnDensity)
            Me.Controls.Add(Me.btnSlopePercent)
            Me.Controls.Add(Me.txtOriginalRaster)
            Me.Controls.Add(Me.btnBrowse)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.cboType)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "SurfacePropertiesForm"
            Me.Text = "Associated Surface Properties"
            CType(Me.ProjectDS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AssociatedSurfaceBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents cboType As System.Windows.Forms.ComboBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents btnHelp As System.Windows.Forms.Button
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnBrowse As System.Windows.Forms.Button
        Friend WithEvents ttpTooltip As System.Windows.Forms.ToolTip
        Friend WithEvents txtOriginalRaster As System.Windows.Forms.TextBox
        Friend WithEvents btnSlopePercent As System.Windows.Forms.Button
        Friend WithEvents btnDensity As System.Windows.Forms.Button
        Friend WithEvents txtProjectRaster As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents btnRoughness As System.Windows.Forms.Button
        Friend WithEvents btnSlopeDegree As System.Windows.Forms.Button
        Friend WithEvents ProjectDS As Core.GCDProject.ProjectDS
        Friend WithEvents AssociatedSurfaceBindingSource As System.Windows.Forms.BindingSource
    End Class

End Namespace