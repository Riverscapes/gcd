Imports System.Windows.Forms
Imports GCDLib.Core

Namespace UI.BudgetSegregation

    Public Class frmBudgetSegResults

        Private m_rBS As ProjectDS.BudgetSegregationsRow
        Private m_Options As UI.ChangeDetection.DoDSummaryDisplayOptions

        Public Sub New(nBSID As Integer)

            ' This call is required by the designer.
            InitializeComponent()

            For Each rBS As ProjectDS.BudgetSegregationsRow In GCDProject.ProjectManagerBase.ds.BudgetSegregations
                If rBS.BudgetID = nBSID Then
                    m_rBS = rBS
                    Exit Sub
                End If
            Next

            If TypeOf m_rBS Is ProjectDS.BudgetSegregationsRow Then

                Dim sOutputFolder As String = GCDProject.ProjectManagerBase.GetAbsolutePath(m_rBS.OutputFolder)

                Dim bsOutputs As New Core.BudgetSegregation.BudgetSegregationOutputsClass(m_rBS)

            Else
                Dim ex As New Exception("Failed to find budget segregation.")
                ex.Data("Budget Seg ID") = nBSID
                Throw ex
            End If

        End Sub

        Private Sub BudgetSegResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            For Each rMask As ProjectDS.BSMasksRow In m_rBS.GetBSMasksRows
                cboSummaryClass.Items.Add(New naru.db.NamedObject(rMask.MaskID, rMask.MaskName))
                cboECDClass.Items.Add(New naru.db.NamedObject(rMask.MaskID, rMask.MaskName))
            Next

            If cboSummaryClass.Items.Count > 0 Then
                cboSummaryClass.SelectedIndex = 0
                cboECDClass.SelectedIndex = 0
            End If

            ucProperties.DoDRow = m_rBS.DoDsRow
            txtName.Text = m_rBS.Name

            txtPolygonMask.Text = m_rBS.PolygonMask
            txtField.Text = m_rBS.Field

            'Hide Report tab for now
            tabMain.TabPages.Remove(TabPage4)

        End Sub

        Private Sub cboSummaryClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboSummaryClass.SelectedIndexChanged

            If TypeOf cboSummaryClass.SelectedItem Is naru.db.NamedObject Then
                Dim nMaskID As Integer = DirectCast(cboSummaryClass.SelectedItem, naru.db.NamedObject).ID
                For Each rMask As ProjectDS.BSMasksRow In m_rBS.GetBSMasksRows
                    If rMask.MaskID = nMaskID Then

                        Dim dodResult As Core.ChangeDetection.DoDResult = Core.ChangeDetection.DoDResult.CreateFromDoDRow(rMask.BudgetSegregationsRow.DoDsRow)

                        Throw New NotImplementedException("Need a way to get the stats from a budget class without masked rasters on disk")
                        'Dim theResultSet As New Core.ChangeDetection.DoDResultSet(theStats, theDoDProps, GCDProject.ProjectManagerBase.GetAbsolutePath(rMask.BudgetSegregationsRow.DoDsRow.RawHistPath), GCDProject.ProjectManagerBase.GetAbsolutePath(rMask.CSVFileName))
                        'ucSummary.RefreshDisplay(theResultSet, m_Options)
                        Exit For
                    End If
                Next

                ' synchronize the two dropdown lists
                cboECDClass.SelectedIndex = cboSummaryClass.SelectedIndex
            End If

        End Sub

        Private Sub cboECDClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboECDClass.SelectedIndexChanged

            If TypeOf cboECDClass.SelectedItem Is naru.db.NamedObject Then
                Dim lItem As naru.db.NamedObject = cboECDClass.SelectedItem
                For Each aMask As ProjectDS.BSMasksRow In m_rBS.GetBSMasksRows
                    If aMask.MaskID = lItem.ID Then
                        Dim dodResult As Core.ChangeDetection.DoDResult = Core.ChangeDetection.DoDResult.CreateFromDoDRow(aMask.BudgetSegregationsRow.DoDsRow)
                        ucHistogram.LoadHistograms(dodResult.RawHistogram, dodResult.ThresholdedHistogram, dodResult.Units)

                        ' Update the elevation change bar chart control
                        ucBars.Initialize(dodResult.ChangeStats, dodResult.Units)
                        Exit For
                    End If
                Next

                ' syncronize the two dropdown lits
                cboSummaryClass.SelectedIndex = cboECDClass.SelectedIndex

            End If

        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

            Dim sFolder As String = GCDProject.ProjectManagerBase.GetAbsolutePath(m_rBS.OutputFolder)
            If IO.Directory.Exists(sFolder) Then
                Process.Start("explorer.exe", sFolder)
            Else
                MsgBox("The budget segregation folder does not exist: " & sFolder.ToString, MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
            End If
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results")
        End Sub

        Private Sub AddToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem1.Click

            Dim myItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim cms As ContextMenuStrip = CType(myItem.Owner, ContextMenuStrip)


            Dim sPath As String = cms.SourceControl.Text
            sPath = GCDProject.ProjectManagerBase.GetAbsolutePath(sPath)
            If Not String.IsNullOrEmpty(sPath) Then
                If GCDConsoleLib.GISDataset.FileExists(sPath) Then

                    Try
                        Dim gPolygon As GCDConsoleLib.Vector = New GCDConsoleLib.Vector(sPath)
                        ' TODO 
                        Throw New Exception("not implemented")
                        'GCDProject.ProjectManagerUI.ArcMapManager.AddBSMaskVector(gPolygon, m_rBS)

                    Catch ex As Exception
                        'Pass
                    End Try

                End If
            End If

        End Sub

    End Class

End Namespace