Public Class BudgetSegResultsForm

    Private m_pArcMap As ESRI.ArcGIS.Framework.IApplication
    Private m_rBS As ProjectDS.BudgetSegregationsRow

    Public Sub New(pArcMap As ESRI.ArcGIS.Framework.IApplication, nBSID As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        m_pArcMap = pArcMap

        For Each rBS As ProjectDS.BudgetSegregationsRow In GCD.GCDProject.ProjectManager.ds.BudgetSegregations
            If rBS.BudgetID = nBSID Then
                m_rBS = rBS
                Exit Sub
            End If
        Next

        If TypeOf m_rBS Is ProjectDS.BudgetSegregationsRow Then

            Dim sOutputFolder As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(m_rBS.OutputFolder)

            Dim bsOutputs As New GCD.BudgetSegregation.BudgetSegregationOutputsClass(m_rBS)

        Else
            Dim ex As New Exception("Failed to find budget segregation.")
            ex.Data("Budget Seg ID") = nBSID
            Throw ex
        End If

    End Sub

    Private Sub BudgetSegResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        For Each rMask As ProjectDS.BSMasksRow In m_rBS.GetBSMasksRows
            cboSummaryClass.Items.Add(New ListItem(rMask.MaskID, rMask.MaskName))
            cboECDClass.Items.Add(New ListItem(rMask.MaskID, rMask.MaskName))
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

        If TypeOf cboSummaryClass.SelectedItem Is ListItem Then
            Dim nMaskID As Integer = DirectCast(cboSummaryClass.SelectedItem, ListItem).ID
            For Each rMask As ProjectDS.BSMasksRow In m_rBS.GetBSMasksRows
                If rMask.MaskID = nMaskID Then

                    Dim theStats As New GCD.ChangeDetection.ChangeStatsFromBSMaskRow(rMask)
                    Dim theDoDProps As GCD.ChangeDetection.ChangeDetectionProperties = GCD.ChangeDetection.ChangeDetectionProperties.CreateFromDoDRow(rMask.BudgetSegregationsRow.DoDsRow)
                    Dim theHistoStats As New GCD.ChangeDetection.DoDResultHistograms(GCD.GCDProject.ProjectManager.GetAbsolutePath(rMask.CSVFileName))
                    Dim theResultSet As New GCD.ChangeDetection.DoDResultSet(theStats, theHistoStats, theDoDProps)
                    ucSummary.DoDResultSet = theResultSet
                    Exit For
                End If
            Next

            ' synchronize the two dropdown lists
            cboECDClass.SelectedIndex = cboSummaryClass.SelectedIndex
        End If

    End Sub

    Private Sub cboECDClass_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboECDClass.SelectedIndexChanged

        If TypeOf cboECDClass.SelectedItem Is ListItem Then
            Dim lItem As ListItem = cboECDClass.SelectedItem
            For Each aMask As ProjectDS.BSMasksRow In m_rBS.GetBSMasksRows
                If aMask.MaskID = lItem.ID Then

                    Dim chngStats As New GCD.ChangeDetection.ChangeStatsFromBSMaskRow(aMask)

                    Dim cdProperties As GCD.ChangeDetection.ChangeDetectionProperties
                    Dim rDoD As ProjectDS.DoDsRow = aMask.BudgetSegregationsRow.DoDsRow
                    Dim sRawDod As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.RawDoDPath)
                    Dim sThrDoD As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshDoDPath)
                    Dim gRawDoD As New GISDataStructures.RasterDirect(sRawDod)
                    If rDoD.TypeMinLOD Then
                        cdProperties = New GCD.ChangeDetection.ChangeDetectionPropertiesMinLoD(sRawDod, sThrDoD, rDoD.Threshold, gRawDoD.CellSize, NumberFormatting.GetLinearUnitsFromESRI(gRawDoD.LinearUnits))
                    ElseIf rDoD.TypePropagated Then
                        Dim sPropErr As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.PropagatedErrorRasterPath)
                        cdProperties = New GCD.ChangeDetection.ChangeDetectionPropertiesPropagated(sRawDod, sThrDoD, sPropErr, gRawDoD.CellSize, NumberFormatting.GetLinearUnitsFromESRI(gRawDoD.LinearUnits))
                    Else

                        Dim sPropErr As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.PropagatedErrorRasterPath)

                        Dim sProbabilityRaster As String = String.Empty
                        If Not rDoD.IsProbabilityRasterNull Then
                            sProbabilityRaster = rDoD.ProbabilityRaster
                        End If

                        Dim sSpatialCoErosionRaster As String = String.Empty
                        If Not rDoD.IsSpatialCoErosionRasterNull Then
                            sSpatialCoErosionRaster = rDoD.SpatialCoErosionRaster
                        End If

                        Dim sSpatialCoDepositionRaster As String = String.Empty
                        If Not rDoD.IsSpatialCoDepositionRasterNull Then
                            sSpatialCoDepositionRaster = rDoD.SpatialCoDepositionRaster
                        End If

                        Dim sConditionalProbRaster As String = String.Empty
                        If Not rDoD.IsConditionalProbRasterNull Then
                            sConditionalProbRaster = rDoD.ConditionalProbRaster
                        End If

                        Dim sPosteriorRaster As String = String.Empty
                        If Not rDoD.IsPosteriorRasterNull Then
                            sPosteriorRaster = rDoD.PosteriorRaster
                        End If

                        cdProperties = New GCD.ChangeDetection.ChangeDetectionPropertiesProbabilistic(sRawDod, sThrDoD, sPropErr, sProbabilityRaster, sSpatialCoErosionRaster, sSpatialCoDepositionRaster, sConditionalProbRaster, sPosteriorRaster, rDoD.Threshold, rDoD.Filter, rDoD.Bayesian, gRawDoD.CellSize, NumberFormatting.GetLinearUnitsFromESRI(gRawDoD.LinearUnits))
                    End If

                    Dim sRawCSV As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshHistPath)
                    Dim sThrCSV As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(aMask.CSVFileName)
                    Dim histo As New GCD.ChangeDetection.DoDResultHistograms(sRawCSV, sThrCSV)

                    Dim dod As New GCD.ChangeDetection.DoDResultSet(chngStats, histo, cdProperties)

                    ucHistogram.DoDResultSet = dod
                    ucHistogram.RefreshHistogram()

                    ' Update the elevation change bar chart control
                    ucBars.Initialize(chngStats, cdProperties.Units.LinearUnit)

                    Exit For
                End If
            Next

            ' syncronize the two dropdown lits
            cboSummaryClass.SelectedIndex = cboECDClass.SelectedIndex

        End If

    End Sub

    Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

        Dim sFolder As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(m_rBS.OutputFolder)
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

        Dim myItem As Windows.Forms.ToolStripMenuItem = CType(sender, Windows.Forms.ToolStripMenuItem)
        Dim cms As Windows.Forms.ContextMenuStrip = CType(myItem.Owner, Windows.Forms.ContextMenuStrip)


        Dim sPath As String = cms.SourceControl.Text
        sPath = GCD.GCDProject.ProjectManager.GetAbsolutePath(sPath)
        If Not String.IsNullOrEmpty(sPath) Then
            If GISDataStructures.PolygonDataSource.Exists(sPath) Then

                Try
                    Dim gPolygon As GISDataStructures.PolygonDataSource = New GISDataStructures.PolygonDataSource(sPath)
                    GCD.GCDProject.ProjectManagerUI.ArcMapManager.AddBSMaskVector(gPolygon, m_rBS)

                Catch ex As Exception
                    'Pass
                End Try

            End If
        End If

    End Sub

End Class