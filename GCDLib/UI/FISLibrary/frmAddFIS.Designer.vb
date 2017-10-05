Namespace UI.FISLibrary
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmAddFIS
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddFIS))
            Me.txtFISFile = New System.Windows.Forms.TextBox()
            Me.FISTableBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.FISLibrary = New GCDLib.FISLibrary()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.btnHelp = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.btnBrowseFIS = New System.Windows.Forms.Button()
            CType(Me.FISTableBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FISLibrary, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtFISFile
            '
            Me.txtFISFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtFISFile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.FISTableBindingSource, "Path", True))
            Me.txtFISFile.Location = New System.Drawing.Point(60, 12)
            Me.txtFISFile.Name = "txtFISFile"
            Me.txtFISFile.ReadOnly = True
            Me.txtFISFile.Size = New System.Drawing.Size(441, 20)
            Me.txtFISFile.TabIndex = 1
            Me.txtFISFile.TabStop = False
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
            'txtName
            '
            Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtName.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.FISTableBindingSource, "Name", True))
            Me.txtName.Location = New System.Drawing.Point(60, 48)
            Me.txtName.Name = "txtName"
            Me.txtName.Size = New System.Drawing.Size(441, 20)
            Me.txtName.TabIndex = 4
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(377, 79)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 6
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnHelp
            '
            Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnHelp.Location = New System.Drawing.Point(12, 83)
            Me.btnHelp.Name = "btnHelp"
            Me.btnHelp.Size = New System.Drawing.Size(75, 23)
            Me.btnHelp.TabIndex = 5
            Me.btnHelp.Text = "Help"
            Me.btnHelp.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(458, 79)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 7
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(12, 17)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(42, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "FIS file:"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(12, 51)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(38, 13)
            Me.Label2.TabIndex = 3
            Me.Label2.Text = "Name:"
            '
            'btnBrowseFIS
            '
            Me.btnBrowseFIS.Anchor = System.Windows.Forms.AnchorStyles.Right
            Me.btnBrowseFIS.Image = CType(resources.GetObject("btnBrowseFIS.Image"), System.Drawing.Image)
            Me.btnBrowseFIS.Location = New System.Drawing.Point(506, 10)
            Me.btnBrowseFIS.Name = "btnBrowseFIS"
            Me.btnBrowseFIS.Size = New System.Drawing.Size(29, 23)
            Me.btnBrowseFIS.TabIndex = 2
            Me.btnBrowseFIS.UseVisualStyleBackColor = True
            '
            'AddFISForm
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(545, 114)
            Me.Controls.Add(Me.btnBrowseFIS)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnHelp)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.txtFISFile)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AddFISForm"
            Me.Text = "Add FIS Library File"
            CType(Me.FISTableBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FISLibrary, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtFISFile As System.Windows.Forms.TextBox
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents btnHelp As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents btnBrowseFIS As System.Windows.Forms.Button
        Friend WithEvents FISLibrary As GCDLib.FISLibrary
        Friend WithEvents FISTableBindingSource As System.Windows.Forms.BindingSource
    End Class
End Namespace