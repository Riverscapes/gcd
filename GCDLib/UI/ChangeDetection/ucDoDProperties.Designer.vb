Namespace UI.ChangeDetection
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucDoDProperties
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.txtNewError = New System.Windows.Forms.TextBox()
            Me.cmsRaster = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddToMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.txtNewDEM = New System.Windows.Forms.TextBox()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.txtOldError = New System.Windows.Forms.TextBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtOldDEM = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.GroupBox3 = New System.Windows.Forms.GroupBox()
            Me.txtThreshold = New System.Windows.Forms.TextBox()
            Me.lblThreshold = New System.Windows.Forms.Label()
            Me.txtType = New System.Windows.Forms.TextBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.Button1 = New System.Windows.Forms.Button()
            Me.grpProbabilistic = New System.Windows.Forms.GroupBox()
            Me.txtDepositionSpatialCoherenceRaster = New System.Windows.Forms.TextBox()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.txtPosteriorRaster = New System.Windows.Forms.TextBox()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.txtConditionalRaster = New System.Windows.Forms.TextBox()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.txtErosionalSpatialCoherenceRaster = New System.Windows.Forms.TextBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.txtBayesian = New System.Windows.Forms.TextBox()
            Me.txtConfidence = New System.Windows.Forms.TextBox()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.lblConfidence = New System.Windows.Forms.Label()
            Me.txtProbabilityRaster = New System.Windows.Forms.TextBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.grpPropagated = New System.Windows.Forms.GroupBox()
            Me.txtPropErr = New System.Windows.Forms.TextBox()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.cmsBasicRaster = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.AddToMapToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.GroupBox1.SuspendLayout()
            Me.cmsRaster.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.GroupBox3.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.grpProbabilistic.SuspendLayout()
            Me.grpPropagated.SuspendLayout()
            Me.cmsBasicRaster.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(9, 24)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(34, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "DEM:"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.txtNewError)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.txtNewDEM)
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.GroupBox1.Location = New System.Drawing.Point(3, 33)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(244, 74)
            Me.GroupBox1.TabIndex = 1
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "New Survey"
            '
            'txtNewError
            '
            Me.txtNewError.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNewError.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtNewError.Location = New System.Drawing.Point(71, 46)
            Me.txtNewError.Name = "txtNewError"
            Me.txtNewError.ReadOnly = True
            Me.txtNewError.Size = New System.Drawing.Size(163, 20)
            Me.txtNewError.TabIndex = 3
            '
            'cmsRaster
            '
            Me.cmsRaster.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertiesToolStripMenuItem, Me.AddToMapToolStripMenuItem})
            Me.cmsRaster.Name = "cmsRaster"
            Me.cmsRaster.Size = New System.Drawing.Size(138, 48)
            '
            'PropertiesToolStripMenuItem
            '
            Me.PropertiesToolStripMenuItem.Image = My.Resources.Resources.Settings
            Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
            Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
            Me.PropertiesToolStripMenuItem.Text = "Properties"
            '
            'AddToMapToolStripMenuItem
            '
            Me.AddToMapToolStripMenuItem.Image = My.Resources.Resources.AddToMap
            Me.AddToMapToolStripMenuItem.Name = "AddToMapToolStripMenuItem"
            Me.AddToMapToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
            Me.AddToMapToolStripMenuItem.Text = "Add to Map"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(9, 50)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(32, 13)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Error:"
            '
            'txtNewDEM
            '
            Me.txtNewDEM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtNewDEM.ContextMenuStrip = Me.cmsRaster
            Me.txtNewDEM.Location = New System.Drawing.Point(72, 20)
            Me.txtNewDEM.Name = "txtNewDEM"
            Me.txtNewDEM.ReadOnly = True
            Me.txtNewDEM.Size = New System.Drawing.Size(163, 20)
            Me.txtNewDEM.TabIndex = 1
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.txtOldError)
            Me.GroupBox2.Controls.Add(Me.Label3)
            Me.GroupBox2.Controls.Add(Me.txtOldDEM)
            Me.GroupBox2.Controls.Add(Me.Label4)
            Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.GroupBox2.Location = New System.Drawing.Point(253, 33)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(244, 74)
            Me.GroupBox2.TabIndex = 2
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Old Survey"
            '
            'txtOldError
            '
            Me.txtOldError.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtOldError.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtOldError.Location = New System.Drawing.Point(71, 46)
            Me.txtOldError.Name = "txtOldError"
            Me.txtOldError.ReadOnly = True
            Me.txtOldError.Size = New System.Drawing.Size(163, 20)
            Me.txtOldError.TabIndex = 3
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(9, 50)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(32, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Error:"
            '
            'txtOldDEM
            '
            Me.txtOldDEM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtOldDEM.ContextMenuStrip = Me.cmsRaster
            Me.txtOldDEM.Location = New System.Drawing.Point(71, 20)
            Me.txtOldDEM.Name = "txtOldDEM"
            Me.txtOldDEM.ReadOnly = True
            Me.txtOldDEM.Size = New System.Drawing.Size(163, 20)
            Me.txtOldDEM.TabIndex = 1
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(9, 24)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(34, 13)
            Me.Label4.TabIndex = 0
            Me.Label4.Text = "DEM:"
            '
            'GroupBox3
            '
            Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.GroupBox3.Controls.Add(Me.txtThreshold)
            Me.GroupBox3.Controls.Add(Me.lblThreshold)
            Me.GroupBox3.Controls.Add(Me.txtType)
            Me.GroupBox3.Controls.Add(Me.Label5)
            Me.GroupBox3.Location = New System.Drawing.Point(3, 113)
            Me.GroupBox3.Name = "GroupBox3"
            Me.GroupBox3.Size = New System.Drawing.Size(244, 74)
            Me.GroupBox3.TabIndex = 3
            Me.GroupBox3.TabStop = False
            Me.GroupBox3.Text = "Uncertainty Analysis Method"
            '
            'txtThreshold
            '
            Me.txtThreshold.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtThreshold.Location = New System.Drawing.Point(72, 46)
            Me.txtThreshold.Name = "txtThreshold"
            Me.txtThreshold.ReadOnly = True
            Me.txtThreshold.Size = New System.Drawing.Size(162, 20)
            Me.txtThreshold.TabIndex = 3
            '
            'lblThreshold
            '
            Me.lblThreshold.AutoSize = True
            Me.lblThreshold.Location = New System.Drawing.Point(9, 50)
            Me.lblThreshold.Name = "lblThreshold"
            Me.lblThreshold.Size = New System.Drawing.Size(57, 13)
            Me.lblThreshold.TabIndex = 2
            Me.lblThreshold.Text = "Threshold:"
            '
            'txtType
            '
            Me.txtType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtType.Location = New System.Drawing.Point(72, 20)
            Me.txtType.Name = "txtType"
            Me.txtType.ReadOnly = True
            Me.txtType.Size = New System.Drawing.Size(162, 20)
            Me.txtType.TabIndex = 1
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(9, 24)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(34, 13)
            Me.Label5.TabIndex = 0
            Me.Label5.Text = "Type:"
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.GroupBox2, 1, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.GroupBox3, 0, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.Button1, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.grpProbabilistic, 0, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.grpPropagated, 1, 2)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 4
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(500, 472)
            Me.TableLayoutPanel1.TabIndex = 5
            '
            'Button1
            '
            Me.Button1.Image = My.Resources.Resources.Copy
            Me.Button1.Location = New System.Drawing.Point(3, 3)
            Me.Button1.Name = "Button1"
            Me.Button1.Size = New System.Drawing.Size(23, 23)
            Me.Button1.TabIndex = 0
            Me.Button1.UseVisualStyleBackColor = True
            '
            'grpProbabilistic
            '
            Me.TableLayoutPanel1.SetColumnSpan(Me.grpProbabilistic, 2)
            Me.grpProbabilistic.Controls.Add(Me.txtDepositionSpatialCoherenceRaster)
            Me.grpProbabilistic.Controls.Add(Me.Label12)
            Me.grpProbabilistic.Controls.Add(Me.txtPosteriorRaster)
            Me.grpProbabilistic.Controls.Add(Me.Label10)
            Me.grpProbabilistic.Controls.Add(Me.txtConditionalRaster)
            Me.grpProbabilistic.Controls.Add(Me.Label9)
            Me.grpProbabilistic.Controls.Add(Me.txtErosionalSpatialCoherenceRaster)
            Me.grpProbabilistic.Controls.Add(Me.Label8)
            Me.grpProbabilistic.Controls.Add(Me.txtBayesian)
            Me.grpProbabilistic.Controls.Add(Me.txtConfidence)
            Me.grpProbabilistic.Controls.Add(Me.Label6)
            Me.grpProbabilistic.Controls.Add(Me.lblConfidence)
            Me.grpProbabilistic.Controls.Add(Me.txtProbabilityRaster)
            Me.grpProbabilistic.Controls.Add(Me.Label7)
            Me.grpProbabilistic.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpProbabilistic.Location = New System.Drawing.Point(3, 193)
            Me.grpProbabilistic.Name = "grpProbabilistic"
            Me.grpProbabilistic.Size = New System.Drawing.Size(494, 276)
            Me.grpProbabilistic.TabIndex = 4
            Me.grpProbabilistic.TabStop = False
            Me.grpProbabilistic.Text = "Probabilistic Uncertainty Properties"
            Me.grpProbabilistic.Visible = False
            '
            'txtDepositionSpatialCoherenceRaster
            '
            Me.txtDepositionSpatialCoherenceRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtDepositionSpatialCoherenceRaster.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtDepositionSpatialCoherenceRaster.Location = New System.Drawing.Point(106, 154)
            Me.txtDepositionSpatialCoherenceRaster.Name = "txtDepositionSpatialCoherenceRaster"
            Me.txtDepositionSpatialCoherenceRaster.ReadOnly = True
            Me.txtDepositionSpatialCoherenceRaster.Size = New System.Drawing.Size(375, 20)
            Me.txtDepositionSpatialCoherenceRaster.TabIndex = 9
            '
            'Label12
            '
            Me.Label12.Location = New System.Drawing.Point(9, 147)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(94, 35)
            Me.Label12.TabIndex = 8
            Me.Label12.Text = "Deposition spatial coherence raster:"
            '
            'txtPosteriorRaster
            '
            Me.txtPosteriorRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtPosteriorRaster.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtPosteriorRaster.Location = New System.Drawing.Point(106, 244)
            Me.txtPosteriorRaster.Name = "txtPosteriorRaster"
            Me.txtPosteriorRaster.ReadOnly = True
            Me.txtPosteriorRaster.Size = New System.Drawing.Size(375, 20)
            Me.txtPosteriorRaster.TabIndex = 13
            '
            'Label10
            '
            Me.Label10.Location = New System.Drawing.Point(9, 237)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(94, 35)
            Me.Label10.TabIndex = 12
            Me.Label10.Text = "Posterior probability raster:"
            '
            'txtConditionalRaster
            '
            Me.txtConditionalRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtConditionalRaster.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtConditionalRaster.Location = New System.Drawing.Point(106, 199)
            Me.txtConditionalRaster.Name = "txtConditionalRaster"
            Me.txtConditionalRaster.ReadOnly = True
            Me.txtConditionalRaster.Size = New System.Drawing.Size(375, 20)
            Me.txtConditionalRaster.TabIndex = 11
            '
            'Label9
            '
            Me.Label9.Location = New System.Drawing.Point(9, 192)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(94, 35)
            Me.Label9.TabIndex = 10
            Me.Label9.Text = "Conditional probability raster:"
            '
            'txtErosionalSpatialCoherenceRaster
            '
            Me.txtErosionalSpatialCoherenceRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtErosionalSpatialCoherenceRaster.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtErosionalSpatialCoherenceRaster.Location = New System.Drawing.Point(106, 109)
            Me.txtErosionalSpatialCoherenceRaster.Name = "txtErosionalSpatialCoherenceRaster"
            Me.txtErosionalSpatialCoherenceRaster.ReadOnly = True
            Me.txtErosionalSpatialCoherenceRaster.Size = New System.Drawing.Size(375, 20)
            Me.txtErosionalSpatialCoherenceRaster.TabIndex = 7
            '
            'Label8
            '
            Me.Label8.Location = New System.Drawing.Point(9, 102)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(94, 35)
            Me.Label8.TabIndex = 6
            Me.Label8.Text = "Erosion spatial coherence raster:"
            '
            'txtBayesian
            '
            Me.txtBayesian.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtBayesian.Location = New System.Drawing.Point(106, 52)
            Me.txtBayesian.Name = "txtBayesian"
            Me.txtBayesian.ReadOnly = True
            Me.txtBayesian.Size = New System.Drawing.Size(376, 20)
            Me.txtBayesian.TabIndex = 3
            '
            'txtConfidence
            '
            Me.txtConfidence.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtConfidence.Location = New System.Drawing.Point(106, 29)
            Me.txtConfidence.Name = "txtConfidence"
            Me.txtConfidence.ReadOnly = True
            Me.txtConfidence.Size = New System.Drawing.Size(376, 20)
            Me.txtConfidence.TabIndex = 1
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(9, 56)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(53, 13)
            Me.Label6.TabIndex = 2
            Me.Label6.Text = "Bayesian:"
            '
            'lblConfidence
            '
            Me.lblConfidence.AutoSize = True
            Me.lblConfidence.Location = New System.Drawing.Point(9, 33)
            Me.lblConfidence.Name = "lblConfidence"
            Me.lblConfidence.Size = New System.Drawing.Size(89, 13)
            Me.lblConfidence.TabIndex = 0
            Me.lblConfidence.Text = "Confidence level:"
            '
            'txtProbabilityRaster
            '
            Me.txtProbabilityRaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtProbabilityRaster.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtProbabilityRaster.Location = New System.Drawing.Point(106, 75)
            Me.txtProbabilityRaster.Name = "txtProbabilityRaster"
            Me.txtProbabilityRaster.ReadOnly = True
            Me.txtProbabilityRaster.Size = New System.Drawing.Size(375, 20)
            Me.txtProbabilityRaster.TabIndex = 5
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(9, 79)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(87, 13)
            Me.Label7.TabIndex = 4
            Me.Label7.Text = "Probability raster:"
            '
            'grpPropagated
            '
            Me.grpPropagated.Controls.Add(Me.txtPropErr)
            Me.grpPropagated.Controls.Add(Me.Label11)
            Me.grpPropagated.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpPropagated.Location = New System.Drawing.Point(253, 113)
            Me.grpPropagated.Name = "grpPropagated"
            Me.grpPropagated.Size = New System.Drawing.Size(244, 74)
            Me.grpPropagated.TabIndex = 5
            Me.grpPropagated.TabStop = False
            Me.grpPropagated.Text = "Propagated Error"
            Me.grpPropagated.Visible = False
            '
            'txtPropErr
            '
            Me.txtPropErr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtPropErr.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtPropErr.Location = New System.Drawing.Point(82, 25)
            Me.txtPropErr.Name = "txtPropErr"
            Me.txtPropErr.ReadOnly = True
            Me.txtPropErr.Size = New System.Drawing.Size(152, 20)
            Me.txtPropErr.TabIndex = 5
            '
            'Label11
            '
            Me.Label11.Location = New System.Drawing.Point(9, 24)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(72, 40)
            Me.Label11.TabIndex = 4
            Me.Label11.Text = "Propagated error:"
            '
            'cmsBasicRaster
            '
            Me.cmsBasicRaster.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToMapToolStripMenuItem1})
            Me.cmsBasicRaster.Name = "cmsBasicRaster"
            Me.cmsBasicRaster.Size = New System.Drawing.Size(138, 26)
            '
            'AddToMapToolStripMenuItem1
            '
            Me.AddToMapToolStripMenuItem1.Image = My.Resources.Resources.AddToMap
            Me.AddToMapToolStripMenuItem1.Name = "AddToMapToolStripMenuItem1"
            Me.AddToMapToolStripMenuItem1.Size = New System.Drawing.Size(137, 22)
            Me.AddToMapToolStripMenuItem1.Text = "Add to Map"
            '
            'DodPropertiesUC
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "DodPropertiesUC"
            Me.Size = New System.Drawing.Size(500, 472)
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.cmsRaster.ResumeLayout(False)
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.GroupBox3.ResumeLayout(False)
            Me.GroupBox3.PerformLayout()
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.grpProbabilistic.ResumeLayout(False)
            Me.grpProbabilistic.PerformLayout()
            Me.grpPropagated.ResumeLayout(False)
            Me.grpPropagated.PerformLayout()
            Me.cmsBasicRaster.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents txtNewError As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtNewDEM As System.Windows.Forms.TextBox
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents txtOldError As System.Windows.Forms.TextBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents txtOldDEM As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtThreshold As System.Windows.Forms.TextBox
        Friend WithEvents lblThreshold As System.Windows.Forms.Label
        Friend WithEvents txtType As System.Windows.Forms.TextBox
        Friend WithEvents cmsRaster As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents AddToMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Button1 As System.Windows.Forms.Button
        Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents grpProbabilistic As System.Windows.Forms.GroupBox
        Friend WithEvents txtPosteriorRaster As System.Windows.Forms.TextBox
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents txtConditionalRaster As System.Windows.Forms.TextBox
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents txtErosionalSpatialCoherenceRaster As System.Windows.Forms.TextBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents txtBayesian As System.Windows.Forms.TextBox
        Friend WithEvents txtConfidence As System.Windows.Forms.TextBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents lblConfidence As System.Windows.Forms.Label
        Friend WithEvents txtProbabilityRaster As System.Windows.Forms.TextBox
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents grpPropagated As System.Windows.Forms.GroupBox
        Friend WithEvents txtPropErr As System.Windows.Forms.TextBox
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents txtDepositionSpatialCoherenceRaster As System.Windows.Forms.TextBox
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents cmsBasicRaster As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents AddToMapToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem

    End Class
End Namespace