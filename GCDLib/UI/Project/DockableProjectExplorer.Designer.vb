<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DockableProjectExplorer
  Inherits System.Windows.Forms.UserControl

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.ProjectExplorerUC1 = New GCDAddIn.ProjectExplorerUC()
        Me.SuspendLayout()
        '
        'ProjectExplorerUC1
        '
        Me.ProjectExplorerUC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProjectExplorerUC1.Location = New System.Drawing.Point(0, 0)
        Me.ProjectExplorerUC1.Name = "ProjectExplorerUC1"
        Me.ProjectExplorerUC1.Size = New System.Drawing.Size(637, 646)
        Me.ProjectExplorerUC1.TabIndex = 0
        '
        'DockableProjectExplorer
        '
        Me.Controls.Add(Me.ProjectExplorerUC1)
        Me.Name = "DockableProjectExplorer"
        Me.Size = New System.Drawing.Size(637, 646)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ProjectExplorerUC1 As GCDAddIn.ProjectExplorerUC

End Class
