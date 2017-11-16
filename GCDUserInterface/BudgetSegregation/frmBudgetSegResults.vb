Imports System.Windows.Forms
Imports GCDCore

Namespace UI.BudgetSegregation

    Public Class frmBudgetSegResults

        Private ResultSet As GCDCore.BudgetSegregation.BSResultSet
        Private m_Options As ChangeDetection.DoDSummaryDisplayOptions

        Public Sub New(nBSID As Integer)

            ' This call is required by the designer.
            InitializeComponent()

            For Each rBS As ProjectDS.BudgetSegregationsRow In ProjectManagerBase.ds.BudgetSegregations
                If rBS.BudgetID = nBSID Then
                    ResultSet = New GCDCore.BudgetSegregation.BSResultSet(rBS)
                    ucProperties.Initialize(rBS.DoDsRow)
                    txtName.Text = rBS.Name
                    Exit For
                End If
            Next

            If ResultSet Is Nothing Then
                Dim ex As New Exception("Failed to find budget segregation.")
                ex.Data("Budget Seg ID") = nBSID
                Throw ex
            End If

        End Sub

        Private Sub BudgetSegResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            cboSummaryClass.DataSource = ResultSet.ClassResults.Keys
            cboECDClass.DataSource = ResultSet.ClassResults.Keys

            If cboSummaryClass.Items.Count > 0 Then
                cboSummaryClass.SelectedIndex = 0
                cboECDClass.SelectedIndex = 0
            End If

            txtPolygonMask.Text = ProjectManagerBase.GetRelativePath(ResultSet.PolygonMask.FullName)
            txtField.Text = ResultSet.FieldName

            'Hide Report tab for now
            tabMain.TabPages.Remove(TabPage4)

        End Sub

        Private Sub cboSummaryClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboSummaryClass.SelectedIndexChanged

            Dim classResult As GCDCore.BudgetSegregation.BSResult = ResultSet.ClassResults(cboSummaryClass.SelectedItem.ToString())
            ucSummary.RefreshDisplay(classResult.ChangeStats, m_Options)

            ' syncronize the two dropdown lits
            cboECDClass.SelectedIndex = cboSummaryClass.SelectedIndex

        End Sub

        Private Sub cboECDClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboECDClass.SelectedIndexChanged

            Dim classResult As GCDCore.BudgetSegregation.BSResult = ResultSet.ClassResults(cboECDClass.SelectedItem.ToString())
            ucHistogram.LoadHistograms(classResult.RawHistogram, classResult.ThrHistogram)

            ' Update the elevation change bar chart control
            ucBars.ChangeStats = classResult.ChangeStats

            ' syncronize the two dropdown lits
            cboSummaryClass.SelectedIndex = cboECDClass.SelectedIndex

        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

            If ResultSet.Folder.Exists Then
                Process.Start("explorer.exe", ResultSet.Folder.FullName)
            Else
                MsgBox("The budget segregation folder does not exist: " & ResultSet.Folder.FullName, MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            End If
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results")
        End Sub

        Private Sub AddToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem1.Click

            Dim myItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim cms As ContextMenuStrip = CType(myItem.Owner, ContextMenuStrip)

            Dim path As IO.FileInfo = ProjectManagerBase.GetAbsolutePath(cms.SourceControl.Text)
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