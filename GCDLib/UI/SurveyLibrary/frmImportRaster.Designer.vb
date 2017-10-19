Namespace UI.SurveyLibrary
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmImportRaster
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
            Me.lblName = New System.Windows.Forms.Label()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.lblRasterPath = New System.Windows.Forms.Label()
            Me.txtRasterPath = New System.Windows.Forms.TextBox()
            Me.grpOriginalRaster = New System.Windows.Forms.GroupBox()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.txtOrigCellSize = New System.Windows.Forms.TextBox()
            Me.txtOrigRows = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.txtBottom = New System.Windows.Forms.TextBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.txtOrigCols = New System.Windows.Forms.TextBox()
            Me.txtLeft = New System.Windows.Forms.TextBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.txtOrigWidth = New System.Windows.Forms.TextBox()
            Me.txtRight = New System.Windows.Forms.TextBox()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.txtOrigHeight = New System.Windows.Forms.TextBox()
            Me.txtTop = New System.Windows.Forms.TextBox()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.ucRaster = New GCD.GCDLib.UI.UtilityForms.ucRasterInput()
            Me.grpProjectRaaster = New System.Windows.Forms.GroupBox()
            Me.cmdHelpPrecision = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.cboMethod = New System.Windows.Forms.ComboBox()
            Me.cmdSave = New System.Windows.Forms.Button()
            Me.valPrecision = New System.Windows.Forms.NumericUpDown()
            Me.valCellSize = New System.Windows.Forms.NumericUpDown()
            Me.valBottom = New System.Windows.Forms.NumericUpDown()
            Me.valRight = New System.Windows.Forms.NumericUpDown()
            Me.valLeft = New System.Windows.Forms.NumericUpDown()
            Me.valTop = New System.Windows.Forms.NumericUpDown()
            Me.lblPrecision = New System.Windows.Forms.Label()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.Label18 = New System.Windows.Forms.Label()
            Me.lblCellResolution = New System.Windows.Forms.Label()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.txtProjRows = New System.Windows.Forms.TextBox()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.txtProjCols = New System.Windows.Forms.TextBox()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.txtProjHeight = New System.Windows.Forms.TextBox()
            Me.Label16 = New System.Windows.Forms.Label()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.txtProjWidth = New System.Windows.Forms.TextBox()
            Me.cmdHelp = New System.Windows.Forms.Button()
            Me.cmdOK = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.grpOriginalRaster.SuspendLayout()
            Me.grpProjectRaaster.SuspendLayout()
            CType(Me.valPrecision, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.valCellSize, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.valBottom, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.valRight, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.valLeft, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.valTop, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblName
            '
            Me.lblName.AutoSize = True
            Me.lblName.Location = New System.Drawing.Point(71, 13)
            Me.lblName.Name = "lblName"
            Me.lblName.Size = New System.Drawing.Size(35, 13)
            Me.lblName.TabIndex = 0
            Me.lblName.Text = "Name"
            '
            'txtName
            '
            Me.txtName.Location = New System.Drawing.Point(113, 9)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(491, 20)
            Me.txtName.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(24, 25)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(71, 13)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Original raster"
            '
            'lblRasterPath
            '
            Me.lblRasterPath.AutoSize = True
            Me.lblRasterPath.Location = New System.Drawing.Point(8, 25)
            Me.lblRasterPath.Name = "lblRasterPath"
            Me.lblRasterPath.Size = New System.Drawing.Size(93, 13)
            Me.lblRasterPath.TabIndex = 0
            Me.lblRasterPath.Text = "Project raster path"
            '
            'txtRasterPath
            '
            Me.txtRasterPath.Location = New System.Drawing.Point(107, 21)
            Me.txtRasterPath.Name = "txtRasterPath"
            Me.txtRasterPath.ReadOnly = True
            Me.txtRasterPath.Size = New System.Drawing.Size(456, 20)
            Me.txtRasterPath.TabIndex = 1
            Me.txtRasterPath.TabStop = False
            '
            'grpOriginalRaster
            '
            Me.grpOriginalRaster.Controls.Add(Me.Label12)
            Me.grpOriginalRaster.Controls.Add(Me.txtOrigCellSize)
            Me.grpOriginalRaster.Controls.Add(Me.txtOrigRows)
            Me.grpOriginalRaster.Controls.Add(Me.Label4)
            Me.grpOriginalRaster.Controls.Add(Me.txtBottom)
            Me.grpOriginalRaster.Controls.Add(Me.Label5)
            Me.grpOriginalRaster.Controls.Add(Me.Label8)
            Me.grpOriginalRaster.Controls.Add(Me.txtOrigCols)
            Me.grpOriginalRaster.Controls.Add(Me.txtLeft)
            Me.grpOriginalRaster.Controls.Add(Me.Label7)
            Me.grpOriginalRaster.Controls.Add(Me.Label9)
            Me.grpOriginalRaster.Controls.Add(Me.txtOrigWidth)
            Me.grpOriginalRaster.Controls.Add(Me.txtRight)
            Me.grpOriginalRaster.Controls.Add(Me.Label6)
            Me.grpOriginalRaster.Controls.Add(Me.Label10)
            Me.grpOriginalRaster.Controls.Add(Me.txtOrigHeight)
            Me.grpOriginalRaster.Controls.Add(Me.txtTop)
            Me.grpOriginalRaster.Controls.Add(Me.Label11)
            Me.grpOriginalRaster.Controls.Add(Me.Label2)
            Me.grpOriginalRaster.Controls.Add(Me.ucRaster)
            Me.grpOriginalRaster.Location = New System.Drawing.Point(12, 37)
            Me.grpOriginalRaster.Name = "grpOriginalRaster"
            Me.grpOriginalRaster.Size = New System.Drawing.Size(606, 156)
            Me.grpOriginalRaster.TabIndex = 2
            Me.grpOriginalRaster.TabStop = False
            Me.grpOriginalRaster.Text = "Original Raster"
            '
            'Label12
            '
            Me.Label12.AutoSize = True
            Me.Label12.Location = New System.Drawing.Point(434, 113)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(72, 13)
            Me.Label12.TabIndex = 18
            Me.Label12.Text = "Cell resolution"
            '
            'txtOrigCellSize
            '
            Me.txtOrigCellSize.Location = New System.Drawing.Point(512, 109)
            Me.txtOrigCellSize.Name = "txtOrigCellSize"
            Me.txtOrigCellSize.ReadOnly = True
            Me.txtOrigCellSize.Size = New System.Drawing.Size(80, 20)
            Me.txtOrigCellSize.TabIndex = 19
            '
            'txtOrigRows
            '
            Me.txtOrigRows.Location = New System.Drawing.Point(372, 56)
            Me.txtOrigRows.Name = "txtOrigRows"
            Me.txtOrigRows.ReadOnly = True
            Me.txtOrigRows.Size = New System.Drawing.Size(80, 20)
            Me.txtOrigRows.TabIndex = 11
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(332, 60)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(34, 13)
            Me.Label4.TabIndex = 10
            Me.Label4.Text = "Rows"
            '
            'txtBottom
            '
            Me.txtBottom.Location = New System.Drawing.Point(107, 126)
            Me.txtBottom.Name = "txtBottom"
            Me.txtBottom.Size = New System.Drawing.Size(100, 20)
            Me.txtBottom.TabIndex = 9
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(459, 60)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(47, 13)
            Me.Label5.TabIndex = 14
            Me.Label5.Text = "Columns"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(61, 130)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(40, 13)
            Me.Label8.TabIndex = 8
            Me.Label8.Text = "Bottom"
            '
            'txtOrigCols
            '
            Me.txtOrigCols.Location = New System.Drawing.Point(512, 56)
            Me.txtOrigCols.Name = "txtOrigCols"
            Me.txtOrigCols.ReadOnly = True
            Me.txtOrigCols.Size = New System.Drawing.Size(80, 20)
            Me.txtOrigCols.TabIndex = 15
            '
            'txtLeft
            '
            Me.txtLeft.Location = New System.Drawing.Point(39, 91)
            Me.txtLeft.Name = "txtLeft"
            Me.txtLeft.Size = New System.Drawing.Size(100, 20)
            Me.txtLeft.TabIndex = 3
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(331, 87)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(35, 13)
            Me.Label7.TabIndex = 12
            Me.Label7.Text = "Width"
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(75, 60)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(26, 13)
            Me.Label9.TabIndex = 4
            Me.Label9.Text = "Top"
            '
            'txtOrigWidth
            '
            Me.txtOrigWidth.Location = New System.Drawing.Point(372, 83)
            Me.txtOrigWidth.Name = "txtOrigWidth"
            Me.txtOrigWidth.ReadOnly = True
            Me.txtOrigWidth.Size = New System.Drawing.Size(80, 20)
            Me.txtOrigWidth.TabIndex = 13
            '
            'txtRight
            '
            Me.txtRight.Location = New System.Drawing.Point(200, 91)
            Me.txtRight.Name = "txtRight"
            Me.txtRight.Size = New System.Drawing.Size(100, 20)
            Me.txtRight.TabIndex = 7
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(468, 87)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(38, 13)
            Me.Label6.TabIndex = 16
            Me.Label6.Text = "Height"
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(163, 95)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(32, 13)
            Me.Label10.TabIndex = 6
            Me.Label10.Text = "Right"
            '
            'txtOrigHeight
            '
            Me.txtOrigHeight.Location = New System.Drawing.Point(512, 83)
            Me.txtOrigHeight.Name = "txtOrigHeight"
            Me.txtOrigHeight.ReadOnly = True
            Me.txtOrigHeight.Size = New System.Drawing.Size(80, 20)
            Me.txtOrigHeight.TabIndex = 17
            '
            'txtTop
            '
            Me.txtTop.Location = New System.Drawing.Point(107, 56)
            Me.txtTop.Name = "txtTop"
            Me.txtTop.Size = New System.Drawing.Size(100, 20)
            Me.txtTop.TabIndex = 5
            '
            'Label11
            '
            Me.Label11.AutoSize = True
            Me.Label11.Location = New System.Drawing.Point(8, 95)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(25, 13)
            Me.Label11.TabIndex = 2
            Me.Label11.Text = "Left"
            '
            'ucRaster
            '
            Me.ucRaster.Location = New System.Drawing.Point(101, 19)
            Me.ucRaster.Name = "ucRaster"
            Me.ucRaster.Size = New System.Drawing.Size(491, 25)
            Me.ucRaster.TabIndex = 1
            '
            'grpProjectRaaster
            '
            Me.grpProjectRaaster.Controls.Add(Me.cmdHelpPrecision)
            Me.grpProjectRaaster.Controls.Add(Me.Label1)
            Me.grpProjectRaaster.Controls.Add(Me.cboMethod)
            Me.grpProjectRaaster.Controls.Add(Me.cmdSave)
            Me.grpProjectRaaster.Controls.Add(Me.valPrecision)
            Me.grpProjectRaaster.Controls.Add(Me.valCellSize)
            Me.grpProjectRaaster.Controls.Add(Me.valBottom)
            Me.grpProjectRaaster.Controls.Add(Me.valRight)
            Me.grpProjectRaaster.Controls.Add(Me.valLeft)
            Me.grpProjectRaaster.Controls.Add(Me.valTop)
            Me.grpProjectRaaster.Controls.Add(Me.lblPrecision)
            Me.grpProjectRaaster.Controls.Add(Me.Label17)
            Me.grpProjectRaaster.Controls.Add(Me.Label18)
            Me.grpProjectRaaster.Controls.Add(Me.lblCellResolution)
            Me.grpProjectRaaster.Controls.Add(Me.txtRasterPath)
            Me.grpProjectRaaster.Controls.Add(Me.Label19)
            Me.grpProjectRaaster.Controls.Add(Me.lblRasterPath)
            Me.grpProjectRaaster.Controls.Add(Me.Label20)
            Me.grpProjectRaaster.Controls.Add(Me.txtProjRows)
            Me.grpProjectRaaster.Controls.Add(Me.Label21)
            Me.grpProjectRaaster.Controls.Add(Me.txtProjCols)
            Me.grpProjectRaaster.Controls.Add(Me.Label14)
            Me.grpProjectRaaster.Controls.Add(Me.txtProjHeight)
            Me.grpProjectRaaster.Controls.Add(Me.Label16)
            Me.grpProjectRaaster.Controls.Add(Me.Label15)
            Me.grpProjectRaaster.Controls.Add(Me.txtProjWidth)
            Me.grpProjectRaaster.Location = New System.Drawing.Point(12, 199)
            Me.grpProjectRaaster.Name = "grpProjectRaaster"
            Me.grpProjectRaaster.Size = New System.Drawing.Size(606, 196)
            Me.grpProjectRaaster.TabIndex = 3
            Me.grpProjectRaaster.TabStop = False
            Me.grpProjectRaaster.Text = "Project Raster"
            '
            'cmdHelpPrecision
            '
            Me.cmdHelpPrecision.FlatAppearance.BorderSize = 0
            Me.cmdHelpPrecision.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.cmdHelpPrecision.Image = Global.GCD.GCDLib.My.Resources.Resources.Help
            Me.cmdHelpPrecision.Location = New System.Drawing.Point(343, 129)
            Me.cmdHelpPrecision.Name = "cmdHelpPrecision"
            Me.cmdHelpPrecision.Size = New System.Drawing.Size(23, 23)
            Me.cmdHelpPrecision.TabIndex = 26
            Me.cmdHelpPrecision.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(260, 161)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(103, 13)
            Me.Label1.TabIndex = 25
            Me.Label1.Text = "Interpolation method"
            '
            'cboMethod
            '
            Me.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMethod.FormattingEnabled = True
            Me.cboMethod.Location = New System.Drawing.Point(372, 157)
            Me.cboMethod.Name = "cboMethod"
            Me.cboMethod.Size = New System.Drawing.Size(220, 21)
            Me.cboMethod.TabIndex = 24
            '
            'cmdSave
            '
            Me.cmdSave.Image = Global.GCD.GCDLib.My.Resources.Resources.SaveGIS
            Me.cmdSave.Location = New System.Drawing.Point(569, 20)
            Me.cmdSave.Name = "cmdSave"
            Me.cmdSave.Size = New System.Drawing.Size(23, 23)
            Me.cmdSave.TabIndex = 23
            Me.cmdSave.UseVisualStyleBackColor = True
            '
            'valPrecision
            '
            Me.valPrecision.Location = New System.Drawing.Point(512, 130)
            Me.valPrecision.Name = "valPrecision"
            Me.valPrecision.Size = New System.Drawing.Size(80, 20)
            Me.valPrecision.TabIndex = 22
            '
            'valCellSize
            '
            Me.valCellSize.Location = New System.Drawing.Point(512, 104)
            Me.valCellSize.Name = "valCellSize"
            Me.valCellSize.Size = New System.Drawing.Size(80, 20)
            Me.valCellSize.TabIndex = 19
            '
            'valBottom
            '
            Me.valBottom.Location = New System.Drawing.Point(107, 121)
            Me.valBottom.Name = "valBottom"
            Me.valBottom.Size = New System.Drawing.Size(100, 20)
            Me.valBottom.TabIndex = 9
            '
            'valRight
            '
            Me.valRight.Location = New System.Drawing.Point(200, 86)
            Me.valRight.Name = "valRight"
            Me.valRight.Size = New System.Drawing.Size(100, 20)
            Me.valRight.TabIndex = 7
            '
            'valLeft
            '
            Me.valLeft.Location = New System.Drawing.Point(39, 86)
            Me.valLeft.Name = "valLeft"
            Me.valLeft.Size = New System.Drawing.Size(100, 20)
            Me.valLeft.TabIndex = 3
            '
            'valTop
            '
            Me.valTop.Location = New System.Drawing.Point(107, 51)
            Me.valTop.Name = "valTop"
            Me.valTop.Size = New System.Drawing.Size(100, 20)
            Me.valTop.TabIndex = 5
            '
            'lblPrecision
            '
            Me.lblPrecision.AutoSize = True
            Me.lblPrecision.Location = New System.Drawing.Point(368, 134)
            Me.lblPrecision.Name = "lblPrecision"
            Me.lblPrecision.Size = New System.Drawing.Size(138, 13)
            Me.lblPrecision.TabIndex = 20
            Me.lblPrecision.Text = "Horizontal decimal precision"
            '
            'Label17
            '
            Me.Label17.AutoSize = True
            Me.Label17.Location = New System.Drawing.Point(459, 55)
            Me.Label17.Name = "Label17"
            Me.Label17.Size = New System.Drawing.Size(47, 13)
            Me.Label17.TabIndex = 14
            Me.Label17.Text = "Columns"
            '
            'Label18
            '
            Me.Label18.AutoSize = True
            Me.Label18.Location = New System.Drawing.Point(61, 125)
            Me.Label18.Name = "Label18"
            Me.Label18.Size = New System.Drawing.Size(40, 13)
            Me.Label18.TabIndex = 8
            Me.Label18.Text = "Bottom"
            '
            'lblCellResolution
            '
            Me.lblCellResolution.AutoSize = True
            Me.lblCellResolution.Location = New System.Drawing.Point(434, 108)
            Me.lblCellResolution.Name = "lblCellResolution"
            Me.lblCellResolution.Size = New System.Drawing.Size(72, 13)
            Me.lblCellResolution.TabIndex = 18
            Me.lblCellResolution.Text = "Cell resolution"
            '
            'Label19
            '
            Me.Label19.AutoSize = True
            Me.Label19.Location = New System.Drawing.Point(75, 55)
            Me.Label19.Name = "Label19"
            Me.Label19.Size = New System.Drawing.Size(26, 13)
            Me.Label19.TabIndex = 4
            Me.Label19.Text = "Top"
            '
            'Label20
            '
            Me.Label20.AutoSize = True
            Me.Label20.Location = New System.Drawing.Point(163, 90)
            Me.Label20.Name = "Label20"
            Me.Label20.Size = New System.Drawing.Size(32, 13)
            Me.Label20.TabIndex = 6
            Me.Label20.Text = "Right"
            '
            'txtProjRows
            '
            Me.txtProjRows.Location = New System.Drawing.Point(372, 51)
            Me.txtProjRows.Name = "txtProjRows"
            Me.txtProjRows.ReadOnly = True
            Me.txtProjRows.Size = New System.Drawing.Size(80, 20)
            Me.txtProjRows.TabIndex = 11
            '
            'Label21
            '
            Me.Label21.AutoSize = True
            Me.Label21.Location = New System.Drawing.Point(8, 90)
            Me.Label21.Name = "Label21"
            Me.Label21.Size = New System.Drawing.Size(25, 13)
            Me.Label21.TabIndex = 2
            Me.Label21.Text = "Left"
            '
            'txtProjCols
            '
            Me.txtProjCols.Location = New System.Drawing.Point(512, 51)
            Me.txtProjCols.Name = "txtProjCols"
            Me.txtProjCols.ReadOnly = True
            Me.txtProjCols.Size = New System.Drawing.Size(80, 20)
            Me.txtProjCols.TabIndex = 15
            '
            'Label14
            '
            Me.Label14.AutoSize = True
            Me.Label14.Location = New System.Drawing.Point(332, 55)
            Me.Label14.Name = "Label14"
            Me.Label14.Size = New System.Drawing.Size(34, 13)
            Me.Label14.TabIndex = 10
            Me.Label14.Text = "Rows"
            '
            'txtProjHeight
            '
            Me.txtProjHeight.Location = New System.Drawing.Point(512, 78)
            Me.txtProjHeight.Name = "txtProjHeight"
            Me.txtProjHeight.ReadOnly = True
            Me.txtProjHeight.Size = New System.Drawing.Size(80, 20)
            Me.txtProjHeight.TabIndex = 17
            '
            'Label16
            '
            Me.Label16.AutoSize = True
            Me.Label16.Location = New System.Drawing.Point(468, 82)
            Me.Label16.Name = "Label16"
            Me.Label16.Size = New System.Drawing.Size(38, 13)
            Me.Label16.TabIndex = 16
            Me.Label16.Text = "Height"
            '
            'Label15
            '
            Me.Label15.AutoSize = True
            Me.Label15.Location = New System.Drawing.Point(331, 82)
            Me.Label15.Name = "Label15"
            Me.Label15.Size = New System.Drawing.Size(35, 13)
            Me.Label15.TabIndex = 12
            Me.Label15.Text = "Width"
            '
            'txtProjWidth
            '
            Me.txtProjWidth.Location = New System.Drawing.Point(372, 78)
            Me.txtProjWidth.Name = "txtProjWidth"
            Me.txtProjWidth.ReadOnly = True
            Me.txtProjWidth.Size = New System.Drawing.Size(80, 20)
            Me.txtProjWidth.TabIndex = 13
            '
            'cmdHelp
            '
            Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdHelp.Location = New System.Drawing.Point(11, 408)
            Me.cmdHelp.Name = "cmdHelp"
            Me.cmdHelp.Size = New System.Drawing.Size(75, 23)
            Me.cmdHelp.TabIndex = 6
            Me.cmdHelp.Text = "Help"
            Me.cmdHelp.UseVisualStyleBackColor = True
            '
            'cmdOK
            '
            Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOK.Location = New System.Drawing.Point(419, 408)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.Size = New System.Drawing.Size(119, 23)
            Me.cmdOK.TabIndex = 4
            Me.cmdOK.Text = "Import"
            Me.cmdOK.UseVisualStyleBackColor = True
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(544, 408)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 5
            Me.cmdCancel.Text = "Cancel"
            Me.cmdCancel.UseVisualStyleBackColor = True
            '
            'frmImportRaster
            '
            Me.AcceptButton = Me.cmdOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdCancel
            Me.ClientSize = New System.Drawing.Size(631, 443)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOK)
            Me.Controls.Add(Me.cmdHelp)
            Me.Controls.Add(Me.grpProjectRaaster)
            Me.Controls.Add(Me.grpOriginalRaster)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.lblName)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmImportRaster"
            Me.ShowIcon = False
            Me.Text = "Import Raster"
            Me.grpOriginalRaster.ResumeLayout(False)
            Me.grpOriginalRaster.PerformLayout()
            Me.grpProjectRaaster.ResumeLayout(False)
            Me.grpProjectRaaster.PerformLayout()
            CType(Me.valPrecision, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.valCellSize, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.valBottom, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.valRight, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.valLeft, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.valTop, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblName As System.Windows.Forms.Label
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblRasterPath As System.Windows.Forms.Label
        Friend WithEvents txtRasterPath As System.Windows.Forms.TextBox
        Friend WithEvents grpOriginalRaster As System.Windows.Forms.GroupBox
        Friend WithEvents grpProjectRaaster As System.Windows.Forms.GroupBox
        Friend WithEvents cmdHelp As System.Windows.Forms.Button
        Friend WithEvents cmdOK As System.Windows.Forms.Button
        Friend WithEvents cmdCancel As System.Windows.Forms.Button
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents txtOrigCellSize As System.Windows.Forms.TextBox
        Friend WithEvents txtOrigRows As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtBottom As System.Windows.Forms.TextBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents txtOrigCols As System.Windows.Forms.TextBox
        Friend WithEvents txtLeft As System.Windows.Forms.TextBox
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents txtOrigWidth As System.Windows.Forms.TextBox
        Friend WithEvents txtRight As System.Windows.Forms.TextBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents txtOrigHeight As System.Windows.Forms.TextBox
        Friend WithEvents txtTop As System.Windows.Forms.TextBox
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents valCellSize As System.Windows.Forms.NumericUpDown
        Friend WithEvents valBottom As System.Windows.Forms.NumericUpDown
        Friend WithEvents valRight As System.Windows.Forms.NumericUpDown
        Friend WithEvents valLeft As System.Windows.Forms.NumericUpDown
        Friend WithEvents valTop As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblPrecision As System.Windows.Forms.Label
        Friend WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents lblCellResolution As System.Windows.Forms.Label
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents txtProjRows As System.Windows.Forms.TextBox
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents txtProjCols As System.Windows.Forms.TextBox
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents txtProjHeight As System.Windows.Forms.TextBox
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents txtProjWidth As System.Windows.Forms.TextBox
        Friend WithEvents valPrecision As System.Windows.Forms.NumericUpDown
        Friend WithEvents cmdSave As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents cboMethod As System.Windows.Forms.ComboBox
        Friend WithEvents cmdHelpPrecision As System.Windows.Forms.Button
        Public WithEvents ucRaster As UtilityForms.ucRasterInput
    End Class

End Namespace