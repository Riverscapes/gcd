Imports System.Windows.Forms
Imports GCDCore.Project

Namespace UI.BudgetSegregation

    Public Class frmBudgetSegResults

        Private BudgetSeg As GCDCore.Project.BudgetSegregation
        Private m_Options As ChangeDetection.DoDSummaryDisplayOptions

        Public Sub New(BS As GCDCore.Project.BudgetSegregation)

            ' This call is required by the designer.
            InitializeComponent()

            BudgetSeg = BS
            ucProperties.Initialize(BudgetSeg.DoD)

        End Sub

        Private Sub BudgetSegResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            txtName.Text = BudgetSeg.Name
            cboSummaryClass.DataSource = BudgetSeg.Classes.Keys
            cboECDClass.DataSource = BudgetSeg.Classes.Keys

            If cboSummaryClass.Items.Count > 0 Then
                cboSummaryClass.SelectedIndex = 0
                cboECDClass.SelectedIndex = 0
            End If

            txtPolygonMask.Text = ProjectManager.Project.GetRelativePath(BudgetSeg.PolygonMask.FullName)
            txtField.Text = BudgetSeg.MaskField

            'Hide Report tab for now
            tabMain.TabPages.Remove(TabPage4)

        End Sub

        Private Sub cboSummaryClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboSummaryClass.SelectedIndexChanged

            Dim classResult As BudgetSegregationClass = BudgetSeg.Classes(cboSummaryClass.SelectedItem.ToString())
            ucSummary.RefreshDisplay(classResult.Statistics, m_Options)

            ' syncronize the two dropdown lits
            cboECDClass.SelectedIndex = cboSummaryClass.SelectedIndex

        End Sub

        Private Sub cboECDClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboECDClass.SelectedIndexChanged

            Dim classResult As BudgetSegregationClass = BudgetSeg.Classes(cboECDClass.SelectedItem.ToString())
            ucHistogram.LoadHistograms(classResult.Histograms.Raw.Data, classResult.Histograms.Thr.Data)

            ' Update the elevation change bar chart control
            ucBars.ChangeStats = classResult.Statistics

            ' syncronize the two dropdown lits
            cboSummaryClass.SelectedIndex = cboECDClass.SelectedIndex

        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

            If BudgetSeg.Folder.Exists Then
                Process.Start("explorer.exe", BudgetSeg.Folder.FullName)
            Else
                MsgBox("The budget segregation folder does not exist: " & BudgetSeg.Folder.FullName, MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            End If
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results")
        End Sub

        Private Sub AddToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem1.Click

            Dim myItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim cms As ContextMenuStrip = CType(myItem.Owner, ContextMenuStrip)

            Dim path As IO.FileInfo = ProjectManager.Project.GetAbsolutePath(cms.SourceControl.Text)
            If path.Exists Then
                Try
                    Dim gPolygon As GCDConsoleLib.Vector = New GCDConsoleLib.Vector(path)
                    Throw New NotImplementedException("not implemented")
                    'GCDProject.ProjectManagerUI.ArcMapManager.AddBSMaskVector(gPolygon, m_rBS)
                Catch ex As Exception
                    'Pass
                End Try
            End If

        End Sub

    End Class

End Namespace