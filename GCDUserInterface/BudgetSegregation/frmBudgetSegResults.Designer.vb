Namespace UI.BudgetSegregation
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmBudgetSegResults
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBudgetSegResults))
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.cmdHelp = New System.Windows.Forms.Button()
            Me.tabMain = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.ucSummary = New ChangeDetection.ucDoDSummary()
            Me.cboSummaryClass = New System.Windows.Forms.ComboBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.ucBars = New ChangeDetection.ucChangeBars()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.cboECDClass = New System.Windows.Forms.ComboBox()
            Me.ucHistogram = New ChangeDetection.ucDoDHistogram()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.grpBudgetSeg = New System.Windows.Forms.GroupBox()
            Me.txtField = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.txtPolygonMask = New System.Windows.Forms.TextBox()
            Me.cmsBasicRaster = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.AddToMapToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.ucProperties = New ChangeDetection.ucDoDProperties()
            Me.TabPage4 = New System.Windows.Forms.TabPage()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtName = New System.Windows.Forms.TextBox()
            Me.cmdBrowse = New System.Windows.Forms.Button()
            Me.tabMain.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            Me.grpBudgetSeg.SuspendLayout()
            Me.cmsBasicRaster.SuspendLayout()
            Me.SuspendLayout()
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cmdCancel.Location = New System.Drawing.Point(591, 475)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 0
            Me.cmdCancel.Text = "Close"
            Me.cmdCancel.UseVisualStyleBackColor = True
            '
            'cmdHelp
            '
            Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdHelp.Location = New System.Drawing.Point(12, 475)
            Me.cmdHelp.Name = "cmdHelp"
            Me.cmdHelp.Size = New System.Drawing.Size(75, 23)
            Me.cmdHelp.TabIndex = 2
            Me.cmdHelp.Text = "Help"
            Me.cmdHelp.UseVisualStyleBackColor = True
            '
            'tabMain
            '
            Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabMain.Controls.Add(Me.TabPage1)
            Me.tabMain.Controls.Add(Me.TabPage2)
            Me.tabMain.Controls.Add(Me.TabPage3)
            Me.tabMain.Controls.Add(Me.TabPage4)
            Me.tabMain.Location = New System.Drawing.Point(4, 36)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New System.Drawing.Size(662, 433)
            Me.tabMain.TabIndex = 3
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.ucSummary)
            Me.TabPage1.Controls.Add(Me.cboSummaryClass)
            Me.TabPage1.Controls.Add(Me.Label2)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(654, 407)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Tabular Results By Category"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'ucSummary
            '
            Me.ucSummary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ucSummary.Location = New System.Drawing.Point(8, 39)
            Me.ucSummary.Name = "ucSummary"
            Me.ucSummary.Size = New System.Drawing.Size(640, 362)
            Me.ucSummary.TabIndex = 2
            '
            'cboSummaryClass
            '
            Me.cboSummaryClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboSummaryClass.FormattingEnabled = True
            Me.cboSummaryClass.Location = New System.Drawing.Point(64, 12)
            Me.cboSummaryClass.Name = "cboSummaryClass"
            Me.cboSummaryClass.Size = New System.Drawing.Size(293, 21)
            Me.cboSummaryClass.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(10, 16)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(52, 13)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Category:"
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.TableLayoutPanel1)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage2.Size = New System.Drawing.Size(654, 407)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Graphical Results By Category"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 3
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.ucBars, 2, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.cboECDClass, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.ucHistogram, 0, 1)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(648, 401)
            Me.TableLayoutPanel1.TabIndex = 6
            '
            'ucBars
            '
            Me.ucBars.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ucBars.Location = New System.Drawing.Point(504, 28)
            Me.ucBars.Name = "ucBars"
            Me.ucBars.Size = New System.Drawing.Size(141, 370)
            Me.ucBars.TabIndex = 5
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Label5.Location = New System.Drawing.Point(3, 0)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(56, 25)
            Me.Label5.TabIndex = 2
            Me.Label5.Text = "Category:"
            '
            'cboECDClass
            '
            Me.cboECDClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboECDClass.FormattingEnabled = True
            Me.cboECDClass.Location = New System.Drawing.Point(65, 3)
            Me.cboECDClass.Name = "cboECDClass"
            Me.cboECDClass.Size = New System.Drawing.Size(293, 21)
            Me.cboECDClass.TabIndex = 3
            '
            'ucHistogram
            '
            Me.ucHistogram.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.TableLayoutPanel1.SetColumnSpan(Me.ucHistogram, 2)
            Me.ucHistogram.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ucHistogram.Location = New System.Drawing.Point(3, 28)
            Me.ucHistogram.Name = "ucHistogram"
            Me.ucHistogram.Size = New System.Drawing.Size(495, 370)
            Me.ucHistogram.TabIndex = 4
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.grpBudgetSeg)
            Me.TabPage3.Controls.Add(Me.ucProperties)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage3.Size = New System.Drawing.Size(654, 407)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Analysis Inputs"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'grpBudgetSeg
            '
            Me.grpBudgetSeg.Controls.Add(Me.txtField)
            Me.grpBudgetSeg.Controls.Add(Me.Label4)
            Me.grpBudgetSeg.Controls.Add(Me.txtPolygonMask)
            Me.grpBudgetSeg.Controls.Add(Me.Label3)
            Me.grpBudgetSeg.Location = New System.Drawing.Point(8, 200)
            Me.grpBudgetSeg.Name = "grpBudgetSeg"
            Me.grpBudgetSeg.Size = New System.Drawing.Size(640, 100)
            Me.grpBudgetSeg.TabIndex = 1
            Me.grpBudgetSeg.TabStop = False
            Me.grpBudgetSeg.Text = "Budget Segregation"
            '
            'txtField
            '
            Me.txtField.Location = New System.Drawing.Point(99, 59)
            Me.txtField.Name = "txtField"
            Me.txtField.ReadOnly = True
            Me.txtField.Size = New System.Drawing.Size(204, 20)
            Me.txtField.TabIndex = 3
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(10, 59)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(32, 13)
            Me.Label4.TabIndex = 2
            Me.Label4.Text = "Field:"
            '
            'txtPolygonMask
            '
            Me.txtPolygonMask.ContextMenuStrip = Me.cmsBasicRaster
            Me.txtPolygonMask.Location = New System.Drawing.Point(99, 24)
            Me.txtPolygonMask.Name = "txtPolygonMask"
            Me.txtPolygonMask.ReadOnly = True
            Me.txtPolygonMask.Size = New System.Drawing.Size(535, 20)
            Me.txtPolygonMask.TabIndex = 1
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
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(10, 24)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(76, 13)
            Me.Label3.TabIndex = 0
            Me.Label3.Text = "Polygon mask:"
            '
            'ucProperties
            '
            Me.ucProperties.DoDRow = Nothing
            Me.ucProperties.Location = New System.Drawing.Point(6, 6)
            Me.ucProperties.Name = "ucProperties"
            Me.ucProperties.Size = New System.Drawing.Size(642, 206)
            Me.ucProperties.TabIndex = 0
            '
            'TabPage4
            '
            Me.TabPage4.Location = New System.Drawing.Point(4, 22)
            Me.TabPage4.Name = "TabPage4"
            Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage4.Size = New System.Drawing.Size(654, 407)
            Me.TabPage4.TabIndex = 3
            Me.TabPage4.Text = "Report"
            Me.TabPage4.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(13, 13)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 4
            Me.Label1.Text = "Name:"
            '
            'txtName
            '
            Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtName.Location = New System.Drawing.Point(78, 10)
            Me.txtName.Name = "txtName"
            Me.txtName.ReadOnly = True
            Me.txtName.Size = New System.Drawing.Size(559, 20)
            Me.txtName.TabIndex = 5
            '
            'cmdBrowse
            '
            Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmdBrowse.Image = My.Resources.Resources.BrowseFolder
            Me.cmdBrowse.Location = New System.Drawing.Point(643, 9)
            Me.cmdBrowse.Name = "cmdBrowse"
            Me.cmdBrowse.Size = New System.Drawing.Size(23, 23)
            Me.cmdBrowse.TabIndex = 6
            Me.cmdBrowse.UseVisualStyleBackColor = True
            '
            'BudgetSegResultsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(678, 510)
            Me.Controls.Add(Me.cmdBrowse)
            Me.Controls.Add(Me.txtName)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.tabMain)
            Me.Controls.Add(Me.cmdHelp)
            Me.Controls.Add(Me.cmdCancel)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "BudgetSegResultsForm"
            Me.Text = "Budget Segregation Results"
            Me.tabMain.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            Me.TabPage2.ResumeLayout(False)
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.TabPage3.ResumeLayout(False)
            Me.grpBudgetSeg.ResumeLayout(False)
            Me.grpBudgetSeg.PerformLayout()
            Me.cmsBasicRaster.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents cmdCancel As System.Windows.Forms.Button
        Friend WithEvents cmdHelp As System.Windows.Forms.Button
        Friend WithEvents tabMain As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtName As System.Windows.Forms.TextBox
        Friend WithEvents ucSummary As ChangeDetection.ucDoDSummary
        Friend WithEvents cboSummaryClass As System.Windows.Forms.ComboBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
        Friend WithEvents grpBudgetSeg As System.Windows.Forms.GroupBox
        Friend WithEvents txtField As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtPolygonMask As System.Windows.Forms.TextBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents ucProperties As ChangeDetection.ucDoDProperties
        Friend WithEvents cboECDClass As System.Windows.Forms.ComboBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents ucHistogram As ChangeDetection.ucDoDHistogram
        Friend WithEvents ucBars As ChangeDetection.ucChangeBars
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents cmdBrowse As System.Windows.Forms.Button
        Friend WithEvents cmsBasicRaster As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents AddToMapToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    End Class
End Namespace