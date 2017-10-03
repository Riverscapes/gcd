<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FISLibraryForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FISLibraryForm))
        Me.btnAddFIS = New System.Windows.Forms.Button()
        Me.btnEditFIS = New System.Windows.Forms.Button()
        Me.btnDeleteFIS = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.NameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PathDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FISTableBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.FISLibrary = New GCDAddIn.FISLibrary()
        Me.ttpTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnFISRepo = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FISTableBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FISLibrary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnAddFIS
        '
        Me.btnAddFIS.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.btnAddFIS.Location = New System.Drawing.Point(12, 12)
        Me.btnAddFIS.Name = "btnAddFIS"
        Me.btnAddFIS.Size = New System.Drawing.Size(29, 23)
        Me.btnAddFIS.TabIndex = 1
        Me.btnAddFIS.UseVisualStyleBackColor = True
        '
        'btnEditFIS
        '
        Me.btnEditFIS.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.btnEditFIS.Location = New System.Drawing.Point(47, 12)
        Me.btnEditFIS.Name = "btnEditFIS"
        Me.btnEditFIS.Size = New System.Drawing.Size(29, 23)
        Me.btnEditFIS.TabIndex = 2
        Me.btnEditFIS.UseVisualStyleBackColor = True
        '
        'btnDeleteFIS
        '
        Me.btnDeleteFIS.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.btnDeleteFIS.Location = New System.Drawing.Point(82, 12)
        Me.btnDeleteFIS.Name = "btnDeleteFIS"
        Me.btnDeleteFIS.Size = New System.Drawing.Size(29, 23)
        Me.btnDeleteFIS.TabIndex = 3
        Me.btnDeleteFIS.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(12, 227)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 7
        Me.btnHelp.Text = "Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(647, 227)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NameDataGridViewTextBoxColumn, Me.PathDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.FISTableBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(12, 41)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(710, 180)
        Me.DataGridView1.TabIndex = 9
        '
        'NameDataGridViewTextBoxColumn
        '
        Me.NameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.NameDataGridViewTextBoxColumn.DataPropertyName = "Name"
        Me.NameDataGridViewTextBoxColumn.FillWeight = 30.0!
        Me.NameDataGridViewTextBoxColumn.HeaderText = "Name"
        Me.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn"
        Me.NameDataGridViewTextBoxColumn.ReadOnly = True
        '
        'PathDataGridViewTextBoxColumn
        '
        Me.PathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.PathDataGridViewTextBoxColumn.DataPropertyName = "Path"
        Me.PathDataGridViewTextBoxColumn.FillWeight = 70.0!
        Me.PathDataGridViewTextBoxColumn.HeaderText = "Path"
        Me.PathDataGridViewTextBoxColumn.Name = "PathDataGridViewTextBoxColumn"
        Me.PathDataGridViewTextBoxColumn.ReadOnly = True
        '
        'FISTableBindingSource
        '
        Me.FISTableBindingSource.DataMember = "FISTable"
        Me.FISTableBindingSource.DataSource = Me.FISLibrary
        '
        'FISLibrary
        '
        Me.FISLibrary.DataSetName = "FISLibrary"
        Me.FISLibrary.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnFISRepo
        '
        Me.btnFISRepo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFISRepo.Location = New System.Drawing.Point(572, 11)
        Me.btnFISRepo.Name = "btnFISRepo"
        Me.btnFISRepo.Size = New System.Drawing.Size(150, 23)
        Me.btnFISRepo.TabIndex = 10
        Me.btnFISRepo.Text = "Visit ET-AL FIS Repository"
        Me.btnFISRepo.UseVisualStyleBackColor = True
        '
        'FISLibraryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(734, 262)
        Me.Controls.Add(Me.btnFISRepo)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnDeleteFIS)
        Me.Controls.Add(Me.btnEditFIS)
        Me.Controls.Add(Me.btnAddFIS)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(200, 199)
        Me.Name = "FISLibraryForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Fuzzy Inference System Library"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FISTableBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FISLibrary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAddFIS As System.Windows.Forms.Button
    Friend WithEvents btnEditFIS As System.Windows.Forms.Button
    Friend WithEvents btnDeleteFIS As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents FISTableBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents FISLibrary As FISLibrary
    Friend WithEvents ttpTooltip As System.Windows.Forms.ToolTip
    Friend WithEvents NameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PathDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnFISRepo As System.Windows.Forms.Button
End Class
