Imports GCDLib.Core
Imports System.Windows.Forms

Namespace UI.BudgetSegregation

    Public Class frmBudgetSegProperties

        Private m_nBSID As Integer
        Private m_bsOutputs As Core.BudgetSegregation.BudgetSegregationOutputsClass
        Private m_nDoDID As Integer

        Public ReadOnly Property BSID As Integer
            Get
                Return m_nBSID
            End Get
        End Property

        Public Sub New(rDoD As ProjectDS.DoDsRow)

            ' This call is required by the designer.
            InitializeComponent()
            m_nDoDID = rDoD.DoDID

            ucPolygon.Initialize("Budget Segregation Polygon Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon)
        End Sub

        Private Sub BudgetSegPropertiesForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim nSelIndex As Integer = 0
            For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.CurrentProject.GetDoDsRows
                Dim nIndex As Integer = cboDoD.Items.Add(New naru.db.NamedObject(rDoD.DoDID, rDoD.Name))
                If rDoD.DoDID = m_nDoDID Then
                    nSelIndex = nIndex
                End If
            Next

            If cboDoD.Items.Count > 0 Then
                If nSelIndex >= 0 Then
                    cboDoD.SelectedIndex = nSelIndex
                End If
            End If

        End Sub

        Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

            If Not ValidateForm() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            Try
                Cursor.Current = Cursors.WaitCursor

                Dim rDoD As ProjectDS.DoDsRow = Nothing
                For Each aDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.CurrentProject.GetDoDsRows
                    If aDoD.DoDID = DirectCast(cboDoD.SelectedItem, naru.db.NamedObject).ID Then
                        rDoD = aDoD
                        'New place to create budget segregation folder
                        Dim sBS_Folder As String = GCDProject.ProjectManagerBase.OutputManager.CreateBudgetSegFolder(rDoD.Name)
                        If IO.Directory.Exists(sBS_Folder) Then
                            IO.Directory.CreateDirectory(IO.Path.Combine(sBS_Folder, "Figs"))
                        End If
                        Exit For
                    End If
                Next

                If Not TypeOf rDoD Is ProjectDS.DoDsRow Then
                    Dim ex As New Exception("Unable to find DoD row in GCD project.")
                    ex.Data("DoDID") = DirectCast(cboDoD.SelectedItem, naru.db.NamedObject).ID
                    Throw ex
                End If

                Dim dodProps As Core.ChangeDetection.ChangeDetectionProperties = Core.ChangeDetection.ChangeDetectionProperties.CreateFromDoDRow(rDoD)
                Dim bs As New Core.BudgetSegregation.BudgetSegregationEngine(dodProps, New IO.DirectoryInfo(txtOutputFolder.Text), My.Settings.ChartHeight, My.Settings.ChartWidth)
                m_bsOutputs = bs.Calculate(ucPolygon.SelectedItem, cboField.Text, My.Settings.ChartWidth, My.Settings.ChartHeight, True)
                Cursor.Current = Cursors.WaitCursor

                Dim sRelativeFolder As String = GCDProject.ProjectManager.GetRelativePath(txtOutputFolder.Text)
                Dim sPCDepositionVolPie As String = GCDProject.ProjectManager.GetRelativePath(m_bsOutputs.PieCharts.PercentageTotalDepositionVolumePiePath)
                Dim sPCErosionVolPie As String = GCDProject.ProjectManager.GetRelativePath(m_bsOutputs.PieCharts.PercentageTotalErosionVolumePiePath)
                Dim sPCTotalVolPie As String = GCDProject.ProjectManager.GetRelativePath(m_bsOutputs.PieCharts.PercentageTotalVolumePiePath)
                Dim sClassLegend As String = GCDProject.ProjectManager.GetRelativePath(m_bsOutputs.ClassLegendPath)
                Dim sClassSummary As String = GCDProject.ProjectManager.GetRelativePath(m_bsOutputs.ClassSummaryPath)

                Dim bsRow As ProjectDS.BudgetSegregationsRow = GCDProject.ProjectManager.ds.BudgetSegregations.AddBudgetSegregationsRow(rDoD, txtName.Text, m_bsOutputs.PolygonMask, cboField.Text, sRelativeFolder,
                                                           sPCDepositionVolPie, sPCErosionVolPie, sPCTotalVolPie, sClassLegend, sClassSummary)

                For Each sMaskName As String In m_bsOutputs.MaskOutputs.Keys
                    Cursor.Current = Cursors.WaitCursor

                    Dim aMask As Core.BudgetSegregation.BudgetSegregationOutputsClass.MaskOutputClass = m_bsOutputs.MaskOutputs(sMaskName)

                    Dim sMaskCSV As String = GCDProject.ProjectManager.GetRelativePath(aMask.csvFilename)
                    Dim sSummaryFile As String = GCDProject.ProjectManager.GetRelativePath(aMask.SummaryPath)
                    Dim sAreaHistPath As String = GCDProject.ProjectManager.GetRelativePath(aMask.AreaChartPath)
                    Dim sVolHistPath As String = GCDProject.ProjectManager.GetRelativePath(aMask.VolumeChartPath)

                    GCDProject.ProjectManager.ds.BSMasks.AddBSMasksRow(bsRow, aMask.MaskValue, sMaskName,
                                                                       aMask.ChangeStats.AreaErosion_Raw, aMask.ChangeStats.AreaDeposition_Raw,
                                                                       aMask.ChangeStats.AreaErosion_Thresholded, aMask.ChangeStats.AreaDeposition_Thresholded,
                                                                       aMask.ChangeStats.VolumeErosion_Raw, aMask.ChangeStats.VolumeDeposition_Raw,
                                                                       aMask.ChangeStats.VolumeErosion_Thresholded, aMask.ChangeStats.VolumeDeposition_Thresholded,
                                                                       aMask.ChangeStats.VolumeErosion_Error, aMask.ChangeStats.VolumeDeposition_Error,
                                                                       sAreaHistPath, sVolHistPath, sSummaryFile, sMaskCSV)
                Next

                GCDProject.ProjectManager.save()

                m_nBSID = bsRow.BudgetID
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
                m_nBSID = 0
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End Sub

        Private Function ValidateForm() As Boolean

            If String.IsNullOrEmpty(txtName.Text) Then
                MsgBox("Please enter a name for the budget segregation analysis.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return Nothing
            Else
                For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.ds.DoDs
                    ' Budget Seg Names are unique to a DoD.
                    If rDoD.DoDID = m_nDoDID Then
                        For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows
                            If String.Compare(rBS.Name, txtName.Text, True) = 0 Then
                                MsgBox("Another budget segregation already uses the name '" & txtName.Text & "'. Please choose a unique name.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                                Return False
                            End If
                        Next
                    End If
                Next
            End If

            If TypeOf ucPolygon.SelectedItem Is GCDConsoleLib.Vector Then

                If ucPolygon.SelectedItem.Features.Count < 1 Then
                    MsgBox("The polygon mask feature class is empty and contains no features. You must choose a polygon feature class with at least one feature.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If

                If TypeOf cboDoD.SelectedItem Is naru.db.NamedObject Then
                    Dim nDoD As Integer = DirectCast(cboDoD.SelectedItem, naru.db.NamedObject).ID
                    Dim rDoD As ProjectDS.DoDsRow = Nothing
                    Dim bDoDExists As Boolean = False
                    For Each rItem As ProjectDS.DoDsRow In GCDProject.ProjectManager.ds.DoDs
                        If rItem.DoDID = nDoD Then
                            rDoD = rItem
                            bDoDExists = True
                            Exit For
                        End If
                    Next

                    If TypeOf rDoD Is ProjectDS.DoDsRow Then
                        Dim sDoDPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.RawDoDPath)
                        If GCDConsoleLib.GISDataset.FileExists(sDoDPath) Then
                            Dim gDoD As New GCDConsoleLib.Raster(sDoDPath)

                            ' Confirm that the polygon mask has a spatial reference.
                            Dim bMissingSpatialReference As Boolean = True
                            'If TypeOf ucPolygon.SelectedItem.SpatialReference Is ESRI.ArcGIS.Geometry.ISpatialReference Then
                            bMissingSpatialReference = ucPolygon.SelectedItem.Proj.PrettyWkt.ToLower.Contains("unknown")
                            'End If

                            If bMissingSpatialReference Then
                                MsgBox("The selected feature class appears to be missing a spatial reference. All GCD inputs must possess a spatial reference and it must be the same spatial reference for all datasets in a GCD project." &
                              " If the selected feature class exists in the same coordinate system, " & gDoD.Proj.PrettyWkt & ", but the coordinate system has not yet been defined for the feature class." &
                              " Use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected datasets by defining the coordinate system as:" & vbCrLf & vbCrLf & gDoD.Proj.PrettyWkt & vbCrLf & vbCrLf & "Then try using it with the GCD again.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                                Return False
                            Else
                                If Not gDoD.Proj.IsSame(ucPolygon.SelectedItem.Proj) Then

                                    MsgBox("The coordinate system of the selected feature class:" & vbCrLf & vbCrLf & ucPolygon.SelectedItem.Proj.PrettyWkt & vbCrLf & vbCrLf & "does not match that of the GCD project:" & vbCrLf & vbCrLf & gDoD.Proj.PrettyWkt & vbCrLf & vbCrLf &
                     "All datasets within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " &
                     "If you believe that the selected dataset does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " &
                     "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected dataset by defining the coordinate system as:" & vbCrLf & vbCrLf & gDoD.Proj.PrettyWkt & vbCrLf & vbCrLf & "Then try importing it into the GCD again.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                                    Return False
                                End If
                            End If
                        Else
                            MsgBox("The selected raw DoD raster cannot be found.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                        End If
                    Else
                        MsgBox("The selected DoD cannot be found in the GCD project.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    End If
                Else
                    MsgBox("Please choose a change detection analysis on which you want to base this budget segregation.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            Else
                MsgBox("Please choose a polygon mask on which you wish to base this budget segregation.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            If String.IsNullOrEmpty(cboField.Text) Then
                MsgBox("Please choose a polygon mask field. Or add a ""string"" field to the feature class if one does not exist.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            Return True

        End Function

        Private Sub cboDoD_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboDoD.SelectedIndexChanged

            If TypeOf cboDoD.SelectedItem Is naru.db.NamedObject Then
                Dim nDoDID As Integer = DirectCast(cboDoD.SelectedItem, naru.db.NamedObject).ID
                If nDoDID > 0 Then
                    For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.CurrentProject.GetDoDsRows
                        If rDoD.DoDID = nDoDID Then
                            txtNewDEM.Text = rDoD.NewSurveyName
                            txtOldDEM.Text = rDoD.OldSurveyName

                            If rDoD.TypeMinLOD Then
                                txtUncertaintyAnalysis.Text = "Minimum Level of Detection"
                                If Not rDoD.IsThresholdNull Then
                                    txtUncertaintyAnalysis.Text &= ", Threshold at " & rDoD.Threshold.ToString("#.00") & GCDProject.ProjectManager.CurrentProject.DisplayUnits
                                End If
                            ElseIf rDoD.TypePropagated Then
                                txtUncertaintyAnalysis.Text = "Propagated Error"
                            ElseIf rDoD.TypeProbabilistic Then
                                txtUncertaintyAnalysis.Text = "Probabilistic"
                                If Not rDoD.IsThresholdNull Then
                                    txtUncertaintyAnalysis.Text &= ", Confidence level at " & (100 * rDoD.Threshold) & "%"
                                End If
                            End If
                            UpdateOutputFolder(rDoD.Name)

                            Exit For
                        End If
                    Next
                End If
            End If
        End Sub

        Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles _
        txtName.TextChanged, cboField.SelectedIndexChanged

        End Sub

        Private Sub UpdateOutputFolder(ByVal sDoDName As String)

            txtOutputFolder.Text = GCDProject.ProjectManagerBase.OutputManager.GetBudgetSegreationDirectoryPath(sDoDName)

        End Sub

        Private Sub UpdateOutputFolder()

            txtOutputFolder.Text = String.Empty
            If Not String.IsNullOrEmpty(txtName.Text) Then

                If TypeOf ucPolygon.SelectedItem Is GCDConsoleLib.Vector Then
                    Dim sPolygonPath As String = ucPolygon.SelectedItem.FilePath

                    If Not String.IsNullOrEmpty(cboField.Text) Then
                        If TypeOf cboDoD.SelectedItem Is naru.db.NamedObject Then
                            Dim nDoDID As Integer = DirectCast(cboDoD.SelectedItem, naru.db.NamedObject).ID
                            If nDoDID > 0 Then
                                For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.CurrentProject.GetDoDsRows
                                    If nDoDID = rDoD.DoDID Then
                                        txtOutputFolder.Text = GCDProject.ProjectManagerBase.OutputManager.GetBudgetSegreationDirectoryPath(rDoD.Name)
                                        'txtOutputFolder.Text = GCD.GCDProject.ProjectManager.OutputManager.CreateBudgetSegFolder(rDoD.Name, sPolygonPath, cboField.Text)
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub PolygonChanged(ByVal sender As Object, ByVal e As naru.ui.PathEventArgs) Handles ucPolygon.PathChanged
            If TypeOf ucPolygon.SelectedItem Is GCDConsoleLib.Vector Then
                ' TODO: Figure solution to following line
                'ucPolygon.SelectedItem.FillComboWithFields(cboField, "Mask", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString, True)
            End If

            If cboField.Items.Count > 0 Then
                cboField.SelectedIndex = 0
            End If

        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/v-add-budget-segregation")
        End Sub
    End Class

End Namespace