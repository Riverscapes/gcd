Namespace ChangeDetection
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmDoDResults
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDoDResults))
            Me.cmdOK = New System.Windows.Forms.Button()
            Me.cmdHelp = New System.Windows.Forms.Button()
            Me.txtDoDName = New System.Windows.Forms.TextBox()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.cmdAddToMap = New System.Windows.Forms.Button()
            Me.cmdBrowse = New System.Windows.Forms.Button()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.ucProperties = New ChangeDetection.ucDoDProperties()
            Me.tbpElevationChangeDistribution = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.ucHistogram = New ChangeDetection.ucDoDHistogram()
            Me.ucBars = New ChangeDetection.ucChangeBars()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.ucSummary = New ChangeDetection.ucDoDSummary()
            Me.tabProperties = New System.Windows.Forms.TabControl()
            Me.cmdSettings = New System.Windows.Forms.Button()
            Me.TabPage3.SuspendLayout()
            Me.tbpElevationChangeDistribution.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.tabProperties.SuspendLayout()
            Me.SuspendLayout()
            '
            'cmdOK
            '
            Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.cmdOK.Location = New System.Drawing.Point(608, 495)
            Me.cmdOK.Name = "cmdOK"
            Me.cmdOK.Size = New System.Drawing.Size(75, 23)
            Me.cmdOK.TabIndex = 6
            Me.cmdOK.Text = "Close"
            Me.cmdOK.UseVisualStyleBackColor = True
            '
            'cmdHelp
            '
            Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdHelp.Location = New System.Drawing.Point(11, 495)
            Me.cmdHelp.Name = "cmdHelp"
            Me.cmdHelp.Size = New System.Drawing.Size(75, 23)
            Me.cmdHelp.TabIndex = 7
            Me.cmdHelp.Text = "Help"
            Me.cmdHelp.UseVisualStyleBackColor = True
            '
            'txtDoDName
            '
            Me.txtDoDName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtDoDName.Location = New System.Drawing.Point(48, 12)
            Me.txtDoDName.Name = "txtDoDName"
            Me.txtDoDName.ReadOnly = True
            Me.txtDoDName.Size = New System.Drawing.Size(556, 20)
            Me.txtDoDName.TabIndex = 1
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(7, 16)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(35, 13)
            Me.Label6.TabIndex = 0
            Me.Label6.Text = "Name"
            '
            'cmdAddToMap
            '
            Me.cmdAddToMap.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdAddToMap.Image = Global.GCDUserInterface.My.Resources.Resources.AddToMap
            Me.cmdAddToMap.Location = New System.Drawing.Point(610, 11)
            Me.cmdAddToMap.Name = "cmdAddToMap"
            Me.cmdAddToMap.Size = New System.Drawing.Size(23, 23)
            Me.cmdAddToMap.TabIndex = 2
            Me.cmdAddToMap.UseVisualStyleBackColor = True
            '
            'cmdBrowse
            '
            Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdBrowse.Image = Global.GCDUserInterface.My.Resources.Resources.BrowseFolder
            Me.cmdBrowse.Location = New System.Drawing.Point(665, 11)
            Me.cmdBrowse.Name = "cmdBrowse"
            Me.cmdBrowse.Size = New System.Drawing.Size(23, 23)
            Me.cmdBrowse.TabIndex = 4
            Me.cmdBrowse.UseVisualStyleBackColor = True
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.ucProperties)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage3.Size = New System.Drawing.Size(673, 411)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Analysis Inputs"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'ucProperties
            '
            Me.ucProperties.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ucProperties.Location = New System.Drawing.Point(3, 3)
            Me.ucProperties.Name = "ucProperties"
            Me.ucProperties.Size = New System.Drawing.Size(667, 405)
            Me.ucProperties.TabIndex = 0
            '
            'tbpElevationChangeDistribution
            '
            Me.tbpElevationChangeDistribution.Controls.Add(Me.TableLayoutPanel1)
            Me.tbpElevationChangeDistribution.Location = New System.Drawing.Point(4, 22)
            Me.tbpElevationChangeDistribution.Name = "tbpElevationChangeDistribution"
            Me.tbpElevationChangeDistribution.Padding = New System.Windows.Forms.Padding(3)
            Me.tbpElevationChangeDistribution.Size = New System.Drawing.Size(673, 411)
            Me.tbpElevationChangeDistribution.TabIndex = 1
            Me.tbpElevationChangeDistribution.Text = "Graphical Results"
            Me.tbpElevationChangeDistribution.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.51274!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.48726!))
            Me.TableLayoutPanel1.Controls.Add(Me.ucHistogram, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.ucBars, 1, 0)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 1
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(667, 405)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'ucHistogram
            '
            Me.ucHistogram.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.ucHistogram.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ucHistogram.Location = New System.Drawing.Point(3, 3)
            Me.ucHistogram.Name = "ucHistogram"
            Me.ucHistogram.Size = New System.Drawing.Size(490, 399)
            Me.ucHistogram.TabIndex = 0
            '
            'ucBars
            '
            Me.ucBars.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ucBars.Location = New System.Drawing.Point(499, 3)
            Me.ucBars.Name = "ucBars"
            Me.ucBars.Size = New System.Drawing.Size(165, 399)
            Me.ucBars.TabIndex = 1
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.ucSummary)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(673, 411)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Tabular Results"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'ucSummary
            '
            Me.ucSummary.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ucSummary.Location = New System.Drawing.Point(3, 3)
            Me.ucSummary.Name = "ucSummary"
            Me.ucSummary.Size = New System.Drawing.Size(667, 405)
            Me.ucSummary.TabIndex = 0
            '
            'tabProperties
            '
            Me.tabProperties.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabProperties.Controls.Add(Me.TabPage1)
            Me.tabProperties.Controls.Add(Me.tbpElevationChangeDistribution)
            Me.tabProperties.Controls.Add(Me.TabPage3)
            Me.tabProperties.Location = New System.Drawing.Point(7, 48)
            Me.tabProperties.Name = "tabProperties"
            Me.tabProperties.SelectedIndex = 0
            Me.tabProperties.Size = New System.Drawing.Size(681, 437)
            Me.tabProperties.TabIndex = 5
            '
            'cmdSettings
            '
            Me.cmdSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdSettings.Image = Global.GCDUserInterface.My.Resources.Resources.Settings
            Me.cmdSettings.Location = New System.Drawing.Point(637, 11)
            Me.cmdSettings.Name = "cmdSettings"
            Me.cmdSettings.Size = New System.Drawing.Size(23, 23)
            Me.cmdSettings.TabIndex = 3
            Me.cmdSettings.UseVisualStyleBackColor = True
            '
            'frmDoDResults
            '
            Me.AcceptButton = Me.cmdOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.cmdOK
            Me.ClientSize = New System.Drawing.Size(695, 530)
            Me.Controls.Add(Me.cmdSettings)
            Me.Controls.Add(Me.cmdBrowse)
            Me.Controls.Add(Me.cmdAddToMap)
            Me.Controls.Add(Me.txtDoDName)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.tabProperties)
            Me.Controls.Add(Me.cmdHelp)
            Me.Controls.Add(Me.cmdOK)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MinimumSize = New System.Drawing.Size(300, 200)
            Me.Name = "frmDoDResults"
            Me.Text = "Change Detection Results"
            Me.TabPage3.ResumeLayout(False)
            Me.tbpElevationChangeDistribution.ResumeLayout(False)
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.tabProperties.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents cmdOK As System.Windows.Forms.Button
        Friend WithEvents cmdHelp As System.Windows.Forms.Button
        Friend WithEvents txtDoDName As System.Windows.Forms.TextBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents cmdAddToMap As System.Windows.Forms.Button
        Friend WithEvents cmdBrowse As System.Windows.Forms.Button
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents ucProperties As ucDoDProperties
        Friend WithEvents tbpElevationChangeDistribution As System.Windows.Forms.TabPage
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ucHistogram As ucDoDHistogram
        Friend WithEvents ucBars As ucChangeBars
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents ucSummary As ucDoDSummary
        Friend WithEvents tabProperties As System.Windows.Forms.TabControl
        Friend WithEvents cmdSettings As System.Windows.Forms.Button
    End Class
End Namespace