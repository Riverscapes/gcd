Namespace UI.Options
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmOptions
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.btnClose = New System.Windows.Forms.Button()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.cmdDefault = New System.Windows.Forms.Button()
            Me.txtWorkspace = New System.Windows.Forms.TextBox()
            Me.btnBrowse = New System.Windows.Forms.Button()
            Me.btnExploreWorkspace = New System.Windows.Forms.Button()
            Me.btnClearWorkspace = New System.Windows.Forms.Button()
            Me.chkAutoLoadEtalFIS = New System.Windows.Forms.CheckBox()
            Me.chkBoxValidateProjectOnLoad = New System.Windows.Forms.CheckBox()
            Me.chkWarnAboutLongPaths = New System.Windows.Forms.CheckBox()
            Me.chkAddOutputLayersToMap = New System.Windows.Forms.CheckBox()
            Me.chkAddInputLayersToMap = New System.Windows.Forms.CheckBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.cboFormat = New System.Windows.Forms.ComboBox()
            Me.chkClearWorkspaceOnStartup = New System.Windows.Forms.CheckBox()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.nbrError = New System.Windows.Forms.NumericUpDown()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.txtSurveyType = New System.Windows.Forms.TextBox()
            Me.DataGridView1 = New System.Windows.Forms.DataGridView()
            Me.NameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ErrorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.TypeIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.SurveyTypesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.SurveyTypes = New SurveyTypes()
            Me.btnDeleteSurveyType = New System.Windows.Forms.Button()
            Me.btnSettingsSurveyType = New System.Windows.Forms.Button()
            Me.btnAddSurveyType = New System.Windows.Forms.Button()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.grbTransparencyLayer = New System.Windows.Forms.GroupBox()
            Me.chkErrorSurfacesTransparency = New System.Windows.Forms.CheckBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.nudTransparency = New System.Windows.Forms.NumericUpDown()
            Me.chkAnalysesTransparency = New System.Windows.Forms.CheckBox()
            Me.chkAssociatedSurfacesTransparency = New System.Windows.Forms.CheckBox()
            Me.chkAutoApplyTransparency = New System.Windows.Forms.CheckBox()
            Me.grbComparitiveLayers = New System.Windows.Forms.GroupBox()
            Me.chkPointDensityComparative = New System.Windows.Forms.CheckBox()
            Me.chkDoDComparative = New System.Windows.Forms.CheckBox()
            Me.chkFISErrorComparative = New System.Windows.Forms.CheckBox()
            Me.chkInterpolationErrorComparative = New System.Windows.Forms.CheckBox()
            Me.chk3DPointQualityComparative = New System.Windows.Forms.CheckBox()
            Me.chkComparativeSymbology = New System.Windows.Forms.CheckBox()
            Me.cboLayer = New System.Windows.Forms.ComboBox()
            Me.cboType = New System.Windows.Forms.ComboBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.btnSet = New System.Windows.Forms.Button()
            Me.btnSymReset = New System.Windows.Forms.Button()
            Me.TabPage4 = New System.Windows.Forms.TabPage()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.lblWidth = New System.Windows.Forms.Label()
            Me.numChartHeight = New System.Windows.Forms.NumericUpDown()
            Me.lblHeight = New System.Windows.Forms.Label()
            Me.numChartWidth = New System.Windows.Forms.NumericUpDown()
            Me.TabPage5 = New System.Windows.Forms.TabPage()
            Me.lstPyramids = New System.Windows.Forms.CheckedListBox()
            Me.lnkPyramidsHelp = New System.Windows.Forms.LinkLabel()
            Me.ttpTooltip = New System.Windows.Forms.ToolTip(Me.components)
            Me.chkTempWorkspaceWarning = New System.Windows.Forms.CheckBox()
            Me.TabControl1.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            CType(Me.nbrError, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.SurveyTypesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.SurveyTypes, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage3.SuspendLayout()
            Me.grbTransparencyLayer.SuspendLayout()
            CType(Me.nudTransparency, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grbComparitiveLayers.SuspendLayout()
            Me.TabPage4.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            CType(Me.numChartHeight, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numChartWidth, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage5.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Location = New System.Drawing.Point(13, 328)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(75, 23)
            Me.btnHelp.TabIndex = 2
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'btnClose
            '
            Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClose.Location = New System.Drawing.Point(446, 328)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(75, 23)
            Me.btnClose.TabIndex = 1
            Me.btnClose.Text = "Close"
            Me.btnClose.UseVisualStyleBackColor = True
            '
            'TabControl1
            '
            Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.TabControl1.Controls.Add(Me.TabPage1)
            Me.TabControl1.Controls.Add(Me.TabPage2)
            Me.TabControl1.Controls.Add(Me.TabPage3)
            Me.TabControl1.Controls.Add(Me.TabPage4)
            Me.TabControl1.Controls.Add(Me.TabPage5)
            Me.TabControl1.Location = New System.Drawing.Point(13, 13)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(508, 309)
            Me.TabControl1.TabIndex = 0
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.chkTempWorkspaceWarning)
            Me.TabPage1.Controls.Add(Me.GroupBox2)
            Me.TabPage1.Controls.Add(Me.chkAutoLoadEtalFIS)
            Me.TabPage1.Controls.Add(Me.chkBoxValidateProjectOnLoad)
            Me.TabPage1.Controls.Add(Me.chkWarnAboutLongPaths)
            Me.TabPage1.Controls.Add(Me.chkAddOutputLayersToMap)
            Me.TabPage1.Controls.Add(Me.chkAddInputLayersToMap)
            Me.TabPage1.Controls.Add(Me.Label5)
            Me.TabPage1.Controls.Add(Me.cboFormat)
            Me.TabPage1.Controls.Add(Me.chkClearWorkspaceOnStartup)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(500, 283)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Workspace"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.cmdDefault)
            Me.GroupBox2.Controls.Add(Me.txtWorkspace)
            Me.GroupBox2.Controls.Add(Me.btnBrowse)
            Me.GroupBox2.Controls.Add(Me.btnExploreWorkspace)
            Me.GroupBox2.Controls.Add(Me.btnClearWorkspace)
            Me.GroupBox2.Location = New System.Drawing.Point(11, 9)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(483, 80)
            Me.GroupBox2.TabIndex = 0
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Temporary Workspace Folder"
            '
            'cmdDefault
            '
            Me.cmdDefault.Location = New System.Drawing.Point(328, 45)
            Me.cmdDefault.Name = "cmdDefault"
            Me.cmdDefault.Size = New System.Drawing.Size(148, 23)
            Me.cmdDefault.TabIndex = 4
            Me.cmdDefault.Text = "Use Default Workspace"
            Me.cmdDefault.UseVisualStyleBackColor = True
            '
            'txtWorkspace
            '
            Me.txtWorkspace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtWorkspace.Location = New System.Drawing.Point(6, 19)
            Me.txtWorkspace.Name = "txtWorkspace"
            Me.txtWorkspace.ReadOnly = True
            Me.txtWorkspace.Size = New System.Drawing.Size(436, 20)
            Me.txtWorkspace.TabIndex = 0
            '
            'btnBrowse
            '
            Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
            Me.btnBrowse.Location = New System.Drawing.Point(448, 18)
            Me.btnBrowse.Name = "btnBrowse"
            Me.btnBrowse.Size = New System.Drawing.Size(29, 23)
            Me.btnBrowse.TabIndex = 1
            Me.btnBrowse.UseVisualStyleBackColor = True
            '
            'btnExploreWorkspace
            '
            Me.btnExploreWorkspace.Location = New System.Drawing.Point(6, 45)
            Me.btnExploreWorkspace.Name = "btnExploreWorkspace"
            Me.btnExploreWorkspace.Size = New System.Drawing.Size(148, 23)
            Me.btnExploreWorkspace.TabIndex = 2
            Me.btnExploreWorkspace.Text = "Open In Explorer"
            Me.btnExploreWorkspace.UseVisualStyleBackColor = True
            '
            'btnClearWorkspace
            '
            Me.btnClearWorkspace.Location = New System.Drawing.Point(167, 45)
            Me.btnClearWorkspace.Name = "btnClearWorkspace"
            Me.btnClearWorkspace.Size = New System.Drawing.Size(148, 23)
            Me.btnClearWorkspace.TabIndex = 3
            Me.btnClearWorkspace.Text = "Clear Workspace"
            Me.btnClearWorkspace.UseVisualStyleBackColor = True
            '
            'chkAutoLoadEtalFIS
            '
            Me.chkAutoLoadEtalFIS.AutoSize = True
            Me.chkAutoLoadEtalFIS.Location = New System.Drawing.Point(11, 213)
            Me.chkAutoLoadEtalFIS.Margin = New System.Windows.Forms.Padding(2)
            Me.chkAutoLoadEtalFIS.Name = "chkAutoLoadEtalFIS"
            Me.chkAutoLoadEtalFIS.Size = New System.Drawing.Size(194, 17)
            Me.chkAutoLoadEtalFIS.TabIndex = 5
            Me.chkAutoLoadEtalFIS.Text = "Autoload ET-AL provided FIS library"
            Me.chkAutoLoadEtalFIS.UseVisualStyleBackColor = True
            '
            'chkBoxValidateProjectOnLoad
            '
            Me.chkBoxValidateProjectOnLoad.AutoSize = True
            Me.chkBoxValidateProjectOnLoad.Location = New System.Drawing.Point(11, 184)
            Me.chkBoxValidateProjectOnLoad.Name = "chkBoxValidateProjectOnLoad"
            Me.chkBoxValidateProjectOnLoad.Size = New System.Drawing.Size(163, 17)
            Me.chkBoxValidateProjectOnLoad.TabIndex = 4
            Me.chkBoxValidateProjectOnLoad.Text = "Validate GCD project on load"
            Me.chkBoxValidateProjectOnLoad.UseVisualStyleBackColor = True
            '
            'chkWarnAboutLongPaths
            '
            Me.chkWarnAboutLongPaths.AutoSize = True
            Me.chkWarnAboutLongPaths.Location = New System.Drawing.Point(265, 97)
            Me.chkWarnAboutLongPaths.Name = "chkWarnAboutLongPaths"
            Me.chkWarnAboutLongPaths.Size = New System.Drawing.Size(193, 17)
            Me.chkWarnAboutLongPaths.TabIndex = 8
            Me.chkWarnAboutLongPaths.Text = "Warn about potential long file paths"
            Me.chkWarnAboutLongPaths.UseVisualStyleBackColor = True
            '
            'chkAddOutputLayersToMap
            '
            Me.chkAddOutputLayersToMap.AutoSize = True
            Me.chkAddOutputLayersToMap.Location = New System.Drawing.Point(11, 155)
            Me.chkAddOutputLayersToMap.Name = "chkAddOutputLayersToMap"
            Me.chkAddOutputLayersToMap.Size = New System.Drawing.Size(143, 17)
            Me.chkAddOutputLayersToMap.TabIndex = 3
            Me.chkAddOutputLayersToMap.Text = "Add output layers to map"
            Me.chkAddOutputLayersToMap.UseVisualStyleBackColor = True
            '
            'chkAddInputLayersToMap
            '
            Me.chkAddInputLayersToMap.AutoSize = True
            Me.chkAddInputLayersToMap.Checked = True
            Me.chkAddInputLayersToMap.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkAddInputLayersToMap.Location = New System.Drawing.Point(11, 126)
            Me.chkAddInputLayersToMap.Name = "chkAddInputLayersToMap"
            Me.chkAddInputLayersToMap.Size = New System.Drawing.Size(136, 17)
            Me.chkAddInputLayersToMap.TabIndex = 2
            Me.chkAddInputLayersToMap.Text = "Add input layers to map"
            Me.chkAddInputLayersToMap.UseVisualStyleBackColor = True
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(11, 246)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(138, 13)
            Me.Label5.TabIndex = 6
            Me.Label5.Text = "Default output raster format:"
            '
            'cboFormat
            '
            Me.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboFormat.FormattingEnabled = True
            Me.cboFormat.Items.AddRange(New Object() {"ESRI GeoTIFF"})
            Me.cboFormat.Location = New System.Drawing.Point(166, 242)
            Me.cboFormat.Name = "cboFormat"
            Me.cboFormat.Size = New System.Drawing.Size(170, 21)
            Me.cboFormat.TabIndex = 7
            '
            'chkClearWorkspaceOnStartup
            '
            Me.chkClearWorkspaceOnStartup.AutoSize = True
            Me.chkClearWorkspaceOnStartup.Checked = True
            Me.chkClearWorkspaceOnStartup.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkClearWorkspaceOnStartup.Location = New System.Drawing.Point(11, 97)
            Me.chkClearWorkspaceOnStartup.Name = "chkClearWorkspaceOnStartup"
            Me.chkClearWorkspaceOnStartup.Size = New System.Drawing.Size(155, 17)
            Me.chkClearWorkspaceOnStartup.TabIndex = 1
            Me.chkClearWorkspaceOnStartup.Text = "Clear workspace on startup"
            Me.chkClearWorkspaceOnStartup.UseVisualStyleBackColor = True
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.Label6)
            Me.TabPage2.Controls.Add(Me.nbrError)
            Me.TabPage2.Controls.Add(Me.Label3)
            Me.TabPage2.Controls.Add(Me.Label4)
            Me.TabPage2.Controls.Add(Me.txtSurveyType)
            Me.TabPage2.Controls.Add(Me.DataGridView1)
            Me.TabPage2.Controls.Add(Me.btnDeleteSurveyType)
            Me.TabPage2.Controls.Add(Me.btnSettingsSurveyType)
            Me.TabPage2.Controls.Add(Me.btnAddSurveyType)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage2.Size = New System.Drawing.Size(500, 283)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Survey Types"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(78, 115)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(58, 13)
            Me.Label6.TabIndex = 11
            Me.Label6.Text = "(map units)"
            '
            'nbrError
            '
            Me.nbrError.DecimalPlaces = 2
            Me.nbrError.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.nbrError.Location = New System.Drawing.Point(7, 109)
            Me.nbrError.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
            Me.nbrError.Name = "nbrError"
            Me.nbrError.Size = New System.Drawing.Size(64, 20)
            Me.nbrError.TabIndex = 9
            Me.nbrError.Value = New Decimal(New Integer() {5, 0, 0, 131072})
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(3, 41)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(66, 13)
            Me.Label3.TabIndex = 8
            Me.Label3.Text = "Survey type:"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(3, 84)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(86, 13)
            Me.Label4.TabIndex = 10
            Me.Label4.Text = "Associated error:"
            '
            'txtSurveyType
            '
            Me.txtSurveyType.Location = New System.Drawing.Point(6, 61)
            Me.txtSurveyType.Name = "txtSurveyType"
            Me.txtSurveyType.Size = New System.Drawing.Size(135, 20)
            Me.txtSurveyType.TabIndex = 6
            '
            'DataGridView1
            '
            Me.DataGridView1.AllowUserToAddRows = False
            Me.DataGridView1.AllowUserToDeleteRows = False
            Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.DataGridView1.AutoGenerateColumns = False
            Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Control
            Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NameDataGridViewTextBoxColumn, Me.ErrorDataGridViewTextBoxColumn, Me.TypeIDDataGridViewTextBoxColumn})
            Me.DataGridView1.DataSource = Me.SurveyTypesBindingSource
            Me.DataGridView1.Location = New System.Drawing.Point(147, 7)
            Me.DataGridView1.MultiSelect = False
            Me.DataGridView1.Name = "DataGridView1"
            Me.DataGridView1.ReadOnly = True
            Me.DataGridView1.RowHeadersVisible = False
            Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.DataGridView1.Size = New System.Drawing.Size(346, 268)
            Me.DataGridView1.TabIndex = 5
            '
            'NameDataGridViewTextBoxColumn
            '
            Me.NameDataGridViewTextBoxColumn.DataPropertyName = "Name"
            Me.NameDataGridViewTextBoxColumn.HeaderText = "Name"
            Me.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn"
            Me.NameDataGridViewTextBoxColumn.ReadOnly = True
            Me.NameDataGridViewTextBoxColumn.Width = 195
            '
            'ErrorDataGridViewTextBoxColumn
            '
            Me.ErrorDataGridViewTextBoxColumn.DataPropertyName = "Error"
            Me.ErrorDataGridViewTextBoxColumn.HeaderText = "Error"
            Me.ErrorDataGridViewTextBoxColumn.Name = "ErrorDataGridViewTextBoxColumn"
            Me.ErrorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'TypeIDDataGridViewTextBoxColumn
            '
            Me.TypeIDDataGridViewTextBoxColumn.DataPropertyName = "TypeID"
            Me.TypeIDDataGridViewTextBoxColumn.HeaderText = "TypeID"
            Me.TypeIDDataGridViewTextBoxColumn.Name = "TypeIDDataGridViewTextBoxColumn"
            Me.TypeIDDataGridViewTextBoxColumn.ReadOnly = True
            Me.TypeIDDataGridViewTextBoxColumn.Visible = False
            '
            'SurveyTypesBindingSource
            '
            Me.SurveyTypesBindingSource.AllowNew = True
            Me.SurveyTypesBindingSource.DataMember = "SurveyTypes"
            Me.SurveyTypesBindingSource.DataSource = Me.SurveyTypes
            '
            'SurveyTypes
            '
            Me.SurveyTypes.DataSetName = "SurveyTypes"
            Me.SurveyTypes.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'btnDeleteSurveyType
            '
            Me.btnDeleteSurveyType.Image = CType(resources.GetObject("btnDeleteSurveyType.Image"), System.Drawing.Image)
            Me.btnDeleteSurveyType.Location = New System.Drawing.Point(42, 6)
            Me.btnDeleteSurveyType.Name = "btnDeleteSurveyType"
            Me.btnDeleteSurveyType.Size = New System.Drawing.Size(29, 23)
            Me.btnDeleteSurveyType.TabIndex = 3
            Me.btnDeleteSurveyType.UseVisualStyleBackColor = True
            '
            'btnSettingsSurveyType
            '
            Me.btnSettingsSurveyType.Image = My.Resources.Resources.Settings
            Me.btnSettingsSurveyType.Location = New System.Drawing.Point(77, 7)
            Me.btnSettingsSurveyType.Name = "btnSettingsSurveyType"
            Me.btnSettingsSurveyType.Size = New System.Drawing.Size(29, 23)
            Me.btnSettingsSurveyType.TabIndex = 2
            Me.btnSettingsSurveyType.UseVisualStyleBackColor = True
            Me.btnSettingsSurveyType.Visible = False
            '
            'btnAddSurveyType
            '
            Me.btnAddSurveyType.Image = My.Resources.Resources.Add
            Me.btnAddSurveyType.Location = New System.Drawing.Point(6, 6)
            Me.btnAddSurveyType.Name = "btnAddSurveyType"
            Me.btnAddSurveyType.Size = New System.Drawing.Size(29, 23)
            Me.btnAddSurveyType.TabIndex = 1
            Me.btnAddSurveyType.UseVisualStyleBackColor = True
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.grbTransparencyLayer)
            Me.TabPage3.Controls.Add(Me.chkAutoApplyTransparency)
            Me.TabPage3.Controls.Add(Me.grbComparitiveLayers)
            Me.TabPage3.Controls.Add(Me.chkComparativeSymbology)
            Me.TabPage3.Controls.Add(Me.cboLayer)
            Me.TabPage3.Controls.Add(Me.cboType)
            Me.TabPage3.Controls.Add(Me.Label2)
            Me.TabPage3.Controls.Add(Me.Label1)
            Me.TabPage3.Controls.Add(Me.btnSet)
            Me.TabPage3.Controls.Add(Me.btnSymReset)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage3.Size = New System.Drawing.Size(500, 283)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Symbology"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'grbTransparencyLayer
            '
            Me.grbTransparencyLayer.Controls.Add(Me.chkErrorSurfacesTransparency)
            Me.grbTransparencyLayer.Controls.Add(Me.Label7)
            Me.grbTransparencyLayer.Controls.Add(Me.nudTransparency)
            Me.grbTransparencyLayer.Controls.Add(Me.chkAnalysesTransparency)
            Me.grbTransparencyLayer.Controls.Add(Me.chkAssociatedSurfacesTransparency)
            Me.grbTransparencyLayer.Location = New System.Drawing.Point(48, 133)
            Me.grbTransparencyLayer.Margin = New System.Windows.Forms.Padding(2)
            Me.grbTransparencyLayer.Name = "grbTransparencyLayer"
            Me.grbTransparencyLayer.Padding = New System.Windows.Forms.Padding(2)
            Me.grbTransparencyLayer.Size = New System.Drawing.Size(176, 119)
            Me.grbTransparencyLayer.TabIndex = 9
            Me.grbTransparencyLayer.TabStop = False
            Me.grbTransparencyLayer.Text = "Transparency Layer"
            '
            'chkErrorSurfacesTransparency
            '
            Me.chkErrorSurfacesTransparency.AutoSize = True
            Me.chkErrorSurfacesTransparency.Location = New System.Drawing.Point(19, 65)
            Me.chkErrorSurfacesTransparency.Margin = New System.Windows.Forms.Padding(2)
            Me.chkErrorSurfacesTransparency.Name = "chkErrorSurfacesTransparency"
            Me.chkErrorSurfacesTransparency.Size = New System.Drawing.Size(93, 17)
            Me.chkErrorSurfacesTransparency.TabIndex = 4
            Me.chkErrorSurfacesTransparency.Text = "Error Surfaces"
            Me.chkErrorSurfacesTransparency.UseVisualStyleBackColor = True
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(21, 23)
            Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(72, 13)
            Me.Label7.TabIndex = 3
            Me.Label7.Text = "Transparency"
            '
            'nudTransparency
            '
            Me.nudTransparency.Increment = New Decimal(New Integer() {5, 0, 0, 0})
            Me.nudTransparency.Location = New System.Drawing.Point(103, 23)
            Me.nudTransparency.Margin = New System.Windows.Forms.Padding(2)
            Me.nudTransparency.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
            Me.nudTransparency.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
            Me.nudTransparency.Name = "nudTransparency"
            Me.nudTransparency.Size = New System.Drawing.Size(44, 20)
            Me.nudTransparency.TabIndex = 2
            Me.nudTransparency.Value = New Decimal(New Integer() {40, 0, 0, 0})
            '
            'chkAnalysesTransparency
            '
            Me.chkAnalysesTransparency.AutoSize = True
            Me.chkAnalysesTransparency.Location = New System.Drawing.Point(19, 85)
            Me.chkAnalysesTransparency.Margin = New System.Windows.Forms.Padding(2)
            Me.chkAnalysesTransparency.Name = "chkAnalysesTransparency"
            Me.chkAnalysesTransparency.Size = New System.Drawing.Size(113, 17)
            Me.chkAnalysesTransparency.TabIndex = 1
            Me.chkAnalysesTransparency.Text = "Analyses Surfaces"
            Me.chkAnalysesTransparency.UseVisualStyleBackColor = True
            '
            'chkAssociatedSurfacesTransparency
            '
            Me.chkAssociatedSurfacesTransparency.AutoSize = True
            Me.chkAssociatedSurfacesTransparency.Location = New System.Drawing.Point(19, 46)
            Me.chkAssociatedSurfacesTransparency.Margin = New System.Windows.Forms.Padding(2)
            Me.chkAssociatedSurfacesTransparency.Name = "chkAssociatedSurfacesTransparency"
            Me.chkAssociatedSurfacesTransparency.Size = New System.Drawing.Size(123, 17)
            Me.chkAssociatedSurfacesTransparency.TabIndex = 0
            Me.chkAssociatedSurfacesTransparency.Text = "Associated Surfaces"
            Me.chkAssociatedSurfacesTransparency.UseVisualStyleBackColor = True
            '
            'chkAutoApplyTransparency
            '
            Me.chkAutoApplyTransparency.AutoSize = True
            Me.chkAutoApplyTransparency.Location = New System.Drawing.Point(48, 111)
            Me.chkAutoApplyTransparency.Margin = New System.Windows.Forms.Padding(2)
            Me.chkAutoApplyTransparency.Name = "chkAutoApplyTransparency"
            Me.chkAutoApplyTransparency.Size = New System.Drawing.Size(140, 17)
            Me.chkAutoApplyTransparency.TabIndex = 8
            Me.chkAutoApplyTransparency.Text = "Auto apply transparency"
            Me.chkAutoApplyTransparency.UseVisualStyleBackColor = True
            '
            'grbComparitiveLayers
            '
            Me.grbComparitiveLayers.Controls.Add(Me.chkPointDensityComparative)
            Me.grbComparitiveLayers.Controls.Add(Me.chkDoDComparative)
            Me.grbComparitiveLayers.Controls.Add(Me.chkFISErrorComparative)
            Me.grbComparitiveLayers.Controls.Add(Me.chkInterpolationErrorComparative)
            Me.grbComparitiveLayers.Controls.Add(Me.chk3DPointQualityComparative)
            Me.grbComparitiveLayers.Enabled = False
            Me.grbComparitiveLayers.Location = New System.Drawing.Point(236, 133)
            Me.grbComparitiveLayers.Margin = New System.Windows.Forms.Padding(2)
            Me.grbComparitiveLayers.Name = "grbComparitiveLayers"
            Me.grbComparitiveLayers.Padding = New System.Windows.Forms.Padding(2)
            Me.grbComparitiveLayers.Size = New System.Drawing.Size(256, 119)
            Me.grbComparitiveLayers.TabIndex = 7
            Me.grbComparitiveLayers.TabStop = False
            Me.grbComparitiveLayers.Text = "Comparitive Layers"
            '
            'chkPointDensityComparative
            '
            Me.chkPointDensityComparative.AutoSize = True
            Me.chkPointDensityComparative.Location = New System.Drawing.Point(19, 85)
            Me.chkPointDensityComparative.Margin = New System.Windows.Forms.Padding(2)
            Me.chkPointDensityComparative.Name = "chkPointDensityComparative"
            Me.chkPointDensityComparative.Size = New System.Drawing.Size(88, 17)
            Me.chkPointDensityComparative.TabIndex = 4
            Me.chkPointDensityComparative.Text = "Point Density"
            Me.chkPointDensityComparative.UseVisualStyleBackColor = True
            '
            'chkDoDComparative
            '
            Me.chkDoDComparative.AutoSize = True
            Me.chkDoDComparative.Location = New System.Drawing.Point(180, 17)
            Me.chkDoDComparative.Margin = New System.Windows.Forms.Padding(2)
            Me.chkDoDComparative.Name = "chkDoDComparative"
            Me.chkDoDComparative.Size = New System.Drawing.Size(48, 17)
            Me.chkDoDComparative.TabIndex = 3
            Me.chkDoDComparative.Text = "DoD"
            Me.chkDoDComparative.UseVisualStyleBackColor = True
            '
            'chkFISErrorComparative
            '
            Me.chkFISErrorComparative.AutoSize = True
            Me.chkFISErrorComparative.Location = New System.Drawing.Point(180, 53)
            Me.chkFISErrorComparative.Margin = New System.Windows.Forms.Padding(2)
            Me.chkFISErrorComparative.Name = "chkFISErrorComparative"
            Me.chkFISErrorComparative.Size = New System.Drawing.Size(67, 17)
            Me.chkFISErrorComparative.TabIndex = 2
            Me.chkFISErrorComparative.Text = "FIS Error"
            Me.chkFISErrorComparative.UseVisualStyleBackColor = True
            '
            'chkInterpolationErrorComparative
            '
            Me.chkInterpolationErrorComparative.AutoSize = True
            Me.chkInterpolationErrorComparative.Location = New System.Drawing.Point(19, 53)
            Me.chkInterpolationErrorComparative.Margin = New System.Windows.Forms.Padding(2)
            Me.chkInterpolationErrorComparative.Name = "chkInterpolationErrorComparative"
            Me.chkInterpolationErrorComparative.Size = New System.Drawing.Size(109, 17)
            Me.chkInterpolationErrorComparative.TabIndex = 1
            Me.chkInterpolationErrorComparative.Text = "Interpolation Error"
            Me.chkInterpolationErrorComparative.UseVisualStyleBackColor = True
            '
            'chk3DPointQualityComparative
            '
            Me.chk3DPointQualityComparative.AutoSize = True
            Me.chk3DPointQualityComparative.Location = New System.Drawing.Point(19, 20)
            Me.chk3DPointQualityComparative.Margin = New System.Windows.Forms.Padding(2)
            Me.chk3DPointQualityComparative.Name = "chk3DPointQualityComparative"
            Me.chk3DPointQualityComparative.Size = New System.Drawing.Size(102, 17)
            Me.chk3DPointQualityComparative.TabIndex = 0
            Me.chk3DPointQualityComparative.Text = "3D Point Quality"
            Me.chk3DPointQualityComparative.UseVisualStyleBackColor = True
            '
            'chkComparativeSymbology
            '
            Me.chkComparativeSymbology.AutoSize = True
            Me.chkComparativeSymbology.Enabled = False
            Me.chkComparativeSymbology.Location = New System.Drawing.Point(236, 111)
            Me.chkComparativeSymbology.Margin = New System.Windows.Forms.Padding(2)
            Me.chkComparativeSymbology.Name = "chkComparativeSymbology"
            Me.chkComparativeSymbology.Size = New System.Drawing.Size(165, 17)
            Me.chkComparativeSymbology.TabIndex = 6
            Me.chkComparativeSymbology.Text = "Apply comparative symbology"
            Me.chkComparativeSymbology.UseVisualStyleBackColor = True
            '
            'cboLayer
            '
            Me.cboLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboLayer.Enabled = False
            Me.cboLayer.FormattingEnabled = True
            Me.cboLayer.Location = New System.Drawing.Point(48, 49)
            Me.cboLayer.Name = "cboLayer"
            Me.cboLayer.Size = New System.Drawing.Size(446, 21)
            Me.cboLayer.TabIndex = 3
            '
            'cboType
            '
            Me.cboType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboType.Enabled = False
            Me.cboType.FormattingEnabled = True
            Me.cboType.Location = New System.Drawing.Point(48, 15)
            Me.cboType.Name = "cboType"
            Me.cboType.Size = New System.Drawing.Size(446, 21)
            Me.cboType.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Enabled = False
            Me.Label2.Location = New System.Drawing.Point(6, 52)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(36, 13)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Layer:"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Enabled = False
            Me.Label1.Location = New System.Drawing.Point(6, 18)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(34, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Type:"
            '
            'btnSet
            '
            Me.btnSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSet.Enabled = False
            Me.btnSet.Location = New System.Drawing.Point(416, 76)
            Me.btnSet.Name = "btnSet"
            Me.btnSet.Size = New System.Drawing.Size(78, 23)
            Me.btnSet.TabIndex = 5
            Me.btnSet.Text = "Set"
            Me.btnSet.UseVisualStyleBackColor = True
            '
            'btnSymReset
            '
            Me.btnSymReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSymReset.Enabled = False
            Me.btnSymReset.Location = New System.Drawing.Point(335, 76)
            Me.btnSymReset.Name = "btnSymReset"
            Me.btnSymReset.Size = New System.Drawing.Size(75, 23)
            Me.btnSymReset.TabIndex = 4
            Me.btnSymReset.Text = "Reset"
            Me.btnSymReset.UseVisualStyleBackColor = True
            '
            'TabPage4
            '
            Me.TabPage4.Controls.Add(Me.GroupBox1)
            Me.TabPage4.Location = New System.Drawing.Point(4, 22)
            Me.TabPage4.Name = "TabPage4"
            Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage4.Size = New System.Drawing.Size(500, 283)
            Me.TabPage4.TabIndex = 3
            Me.TabPage4.Text = "Graphs"
            Me.TabPage4.UseVisualStyleBackColor = True
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.lblWidth)
            Me.GroupBox1.Controls.Add(Me.numChartHeight)
            Me.GroupBox1.Controls.Add(Me.lblHeight)
            Me.GroupBox1.Controls.Add(Me.numChartWidth)
            Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(479, 117)
            Me.GroupBox1.TabIndex = 4
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Default Settings for Automatically Saved Graphs:"
            '
            'lblWidth
            '
            Me.lblWidth.AutoSize = True
            Me.lblWidth.Location = New System.Drawing.Point(15, 26)
            Me.lblWidth.Name = "lblWidth"
            Me.lblWidth.Size = New System.Drawing.Size(73, 13)
            Me.lblWidth.TabIndex = 0
            Me.lblWidth.Text = "Width (pixels):"
            '
            'numChartHeight
            '
            Me.numChartHeight.Location = New System.Drawing.Point(96, 50)
            Me.numChartHeight.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.numChartHeight.Name = "numChartHeight"
            Me.numChartHeight.Size = New System.Drawing.Size(120, 20)
            Me.numChartHeight.TabIndex = 3
            Me.numChartHeight.Value = New Decimal(New Integer() {1000, 0, 0, 0})
            '
            'lblHeight
            '
            Me.lblHeight.AutoSize = True
            Me.lblHeight.Location = New System.Drawing.Point(16, 52)
            Me.lblHeight.Name = "lblHeight"
            Me.lblHeight.Size = New System.Drawing.Size(76, 13)
            Me.lblHeight.TabIndex = 1
            Me.lblHeight.Text = "Height (pixels):"
            '
            'numChartWidth
            '
            Me.numChartWidth.Location = New System.Drawing.Point(96, 24)
            Me.numChartWidth.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.numChartWidth.Name = "numChartWidth"
            Me.numChartWidth.Size = New System.Drawing.Size(120, 20)
            Me.numChartWidth.TabIndex = 2
            Me.numChartWidth.Value = New Decimal(New Integer() {1000, 0, 0, 0})
            '
            'TabPage5
            '
            Me.TabPage5.Controls.Add(Me.lstPyramids)
            Me.TabPage5.Controls.Add(Me.lnkPyramidsHelp)
            Me.TabPage5.Location = New System.Drawing.Point(4, 22)
            Me.TabPage5.Name = "TabPage5"
            Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage5.Size = New System.Drawing.Size(500, 283)
            Me.TabPage5.TabIndex = 4
            Me.TabPage5.Text = "Pyramids"
            Me.TabPage5.UseVisualStyleBackColor = True
            '
            'lstPyramids
            '
            Me.lstPyramids.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lstPyramids.CheckOnClick = True
            Me.lstPyramids.FormattingEnabled = True
            Me.lstPyramids.Location = New System.Drawing.Point(15, 38)
            Me.lstPyramids.Name = "lstPyramids"
            Me.lstPyramids.Size = New System.Drawing.Size(467, 229)
            Me.lstPyramids.TabIndex = 1
            '
            'lnkPyramidsHelp
            '
            Me.lnkPyramidsHelp.AutoSize = True
            Me.lnkPyramidsHelp.LinkArea = New System.Windows.Forms.LinkArea(44, 8)
            Me.lnkPyramidsHelp.Location = New System.Drawing.Point(15, 13)
            Me.lnkPyramidsHelp.Name = "lnkPyramidsHelp"
            Me.lnkPyramidsHelp.Size = New System.Drawing.Size(414, 17)
            Me.lnkPyramidsHelp.TabIndex = 0
            Me.lnkPyramidsHelp.TabStop = True
            Me.lnkPyramidsHelp.Text = "Choose which GCD rasters automatically have pyramids built as they are created."
            Me.lnkPyramidsHelp.UseCompatibleTextRendering = True
            '
            'chkTempWorkspaceWarning
            '
            Me.chkTempWorkspaceWarning.AutoSize = True
            Me.chkTempWorkspaceWarning.Location = New System.Drawing.Point(265, 126)
            Me.chkTempWorkspaceWarning.Name = "chkTempWorkspaceWarning"
            Me.chkTempWorkspaceWarning.Size = New System.Drawing.Size(226, 17)
            Me.chkTempWorkspaceWarning.TabIndex = 9
            Me.chkTempWorkspaceWarning.Text = "Invalid temp workspace character warning"
            Me.chkTempWorkspaceWarning.UseVisualStyleBackColor = True
            '
            'OptionsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(533, 363)
            Me.Controls.Add(Me.TabControl1)
            Me.Controls.Add(Me.btnClose)
            Me.Controls.Add(Me.btnHelp)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(500, 299)
            Me.Name = "OptionsForm"
            Me.Text = "GCD Options"
            Me.TabControl1.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.TabPage2.ResumeLayout(False)
            Me.TabPage2.PerformLayout()
            CType(Me.nbrError, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.SurveyTypesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.SurveyTypes, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage3.ResumeLayout(False)
            Me.TabPage3.PerformLayout()
            Me.grbTransparencyLayer.ResumeLayout(False)
            Me.grbTransparencyLayer.PerformLayout()
            CType(Me.nudTransparency, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grbComparitiveLayers.ResumeLayout(False)
            Me.grbComparitiveLayers.PerformLayout()
            Me.TabPage4.ResumeLayout(False)
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.numChartHeight, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numChartWidth, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage5.ResumeLayout(False)
            Me.TabPage5.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnHelp As System.Windows.Forms.Button
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents txtWorkspace As System.Windows.Forms.TextBox
        Friend WithEvents chkClearWorkspaceOnStartup As System.Windows.Forms.CheckBox
        Friend WithEvents btnClearWorkspace As System.Windows.Forms.Button
        Friend WithEvents btnBrowse As System.Windows.Forms.Button
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents btnAddSurveyType As System.Windows.Forms.Button
        Friend WithEvents btnSettingsSurveyType As System.Windows.Forms.Button
        Friend WithEvents btnDeleteSurveyType As System.Windows.Forms.Button
        Friend WithEvents btnSet As System.Windows.Forms.Button
        Friend WithEvents btnSymReset As System.Windows.Forms.Button
        Friend WithEvents cboLayer As System.Windows.Forms.ComboBox
        Friend WithEvents cboType As System.Windows.Forms.ComboBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents ttpTooltip As System.Windows.Forms.ToolTip
        Friend WithEvents SurveyTypesBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents SurveyTypes As SurveyTypes
        Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
        Friend WithEvents txtSurveyType As System.Windows.Forms.TextBox
        Friend WithEvents nbrError As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents NameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ErrorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TypeIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents cboFormat As System.Windows.Forms.ComboBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents chkAddOutputLayersToMap As System.Windows.Forms.CheckBox
        Friend WithEvents chkAddInputLayersToMap As System.Windows.Forms.CheckBox
        Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
        Friend WithEvents lblHeight As System.Windows.Forms.Label
        Friend WithEvents lblWidth As System.Windows.Forms.Label
        Friend WithEvents numChartHeight As System.Windows.Forms.NumericUpDown
        Friend WithEvents numChartWidth As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnExploreWorkspace As System.Windows.Forms.Button
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents chkWarnAboutLongPaths As System.Windows.Forms.CheckBox
        Friend WithEvents chkBoxValidateProjectOnLoad As System.Windows.Forms.CheckBox
        Friend WithEvents chkAutoLoadEtalFIS As System.Windows.Forms.CheckBox
        Friend WithEvents grbComparitiveLayers As System.Windows.Forms.GroupBox
        Friend WithEvents chkPointDensityComparative As System.Windows.Forms.CheckBox
        Friend WithEvents chkDoDComparative As System.Windows.Forms.CheckBox
        Friend WithEvents chkFISErrorComparative As System.Windows.Forms.CheckBox
        Friend WithEvents chkInterpolationErrorComparative As System.Windows.Forms.CheckBox
        Friend WithEvents chk3DPointQualityComparative As System.Windows.Forms.CheckBox
        Friend WithEvents chkComparativeSymbology As System.Windows.Forms.CheckBox
        Friend WithEvents grbTransparencyLayer As System.Windows.Forms.GroupBox
        Friend WithEvents chkAssociatedSurfacesTransparency As System.Windows.Forms.CheckBox
        Friend WithEvents chkAutoApplyTransparency As System.Windows.Forms.CheckBox
        Friend WithEvents chkAnalysesTransparency As System.Windows.Forms.CheckBox
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents nudTransparency As System.Windows.Forms.NumericUpDown
        Friend WithEvents chkErrorSurfacesTransparency As System.Windows.Forms.CheckBox
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents cmdDefault As System.Windows.Forms.Button
        Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
        Friend WithEvents lnkPyramidsHelp As System.Windows.Forms.LinkLabel
        Friend WithEvents lstPyramids As System.Windows.Forms.CheckedListBox
        Friend WithEvents chkTempWorkspaceWarning As System.Windows.Forms.CheckBox
    End Class
End Namespace