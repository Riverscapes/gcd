Imports System.Windows.Forms
Imports System.Drawing
Imports GCDLib.UI.SurveyLibrary
Imports GCDLib.Core

Namespace UI.Project

    Public Class ucProjectExplorer

        Public Event ProjectTreeNodeSelectionChange(sender As Object, e As EventArgs)

        Private Const m_sGroupInputs As String = "Inputs"
        Private Const m_sAssocSurfaces As String = "Associated Surfaces"
        Private Const m_sErrorSurfaces As String = "Error Surfaces"
        Private Const m_sBudgetSegs As String = "Budget Segregations"
        Private Shared m_eSortBy As SortSurveyBy = SortSurveyBy.DEMSurveyID_Asc

        Public Enum GCDNodeTypes
            Project
            InputsGroup
            SurveysGroup
            DEMSurvey
            AssociatedSurfaceGroup
            AssociatedSurface
            ErrorSurfaceGroup
            ErrorSurface
            AOIGroup
            AOI
            AnalysesGroup
            ChangeDetectionGroup
            ChangeDetectionDEMPair
            DoD
            BudgetSegregationGroup
            BudgetSegregationMask
            BudgetSegregation
            ReservoirGroup
            Reservoir
        End Enum

        Public Enum SortSurveyBy
            NameAsc
            NameDsc
            SurveyDateAsc
            SurveyDateDsc
            DEMSurveyID_Asc
            DEMSurveyID_Dsc
        End Enum

        Private ReadOnly Property IsStandaloneMode As Boolean
            Get
                Return Not Reflection.Assembly.GetEntryAssembly().FullName.ToLower().Contains("arcmap")
            End Get
        End Property

        Private Sub ProjectExplorerUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            LoadTree()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSelectedNodeTag">If provided, the code will make this the selected node</param>
        ''' <remarks>Grouping nodes are added to the tree with the enumeration above as their key. i.e. Project node has key "1".
        ''' Items that have database IDs are added with the key as type_id. So DEM Survey with ID 4 would have key "3_4"</remarks>
        Private Sub LoadTree(Optional sSelectedNodeTag As String = "", Optional eSortSurveyBy As SortSurveyBy = SortSurveyBy.DEMSurveyID_Asc)

            treProject.Nodes.Clear()

            Dim ds As ProjectDS = GCDProject.ProjectManagerBase.ds
            If Not TypeOf ds Is ProjectDS Then
                Exit Sub
            End If

            Dim dr As ProjectDS.ProjectRow = GCDProject.ProjectManagerBase.CurrentProject
            If TypeOf dr Is ProjectDS.ProjectRow Then
                LoadTree(treProject, dr, False, sSelectedNodeTag, eSortSurveyBy)
            End If

        End Sub

        Public Shared Sub LoadTree(ByRef tre As TreeView,
                                   rProject As ProjectDS.ProjectRow,
                                   bCheckboxes As Boolean,
                                   Optional sSelectedNodeTag As String = "",
                                   Optional eSortSurveyBy As SortSurveyBy = SortSurveyBy.DEMSurveyID_Asc)

            Try


                Dim nodProject As TreeNode = tre.Nodes.Add(GCDNodeTypes.Project.ToString, rProject.Name, GCDNodeTypes.Project, GCDNodeTypes.Project)
                nodProject.Tag = GCDNodeTypes.Project.ToString
                nodProject.SelectedImageIndex = nodProject.ImageIndex
                If nodProject.Tag = sSelectedNodeTag Then tre.SelectedNode = nodProject

                Dim nodInputs As TreeNode = nodProject.Nodes.Add(GCDNodeTypes.InputsGroup.ToString, m_sGroupInputs, GCDNodeTypes.InputsGroup)
                nodInputs.Tag = GCDNodeTypes.InputsGroup.ToString
                nodInputs.SelectedImageIndex = nodInputs.ImageIndex
                If nodInputs.Tag = sSelectedNodeTag Then tre.SelectedNode = nodInputs

                Dim nodSurveysGroup As TreeNode = nodInputs.Nodes.Add(GCDNodeTypes.SurveysGroup.ToString, "DEM Surveys", GCDNodeTypes.SurveysGroup)
                nodSurveysGroup.Tag = GCDNodeTypes.SurveysGroup.ToString
                nodSurveysGroup.SelectedImageIndex = nodSurveysGroup.ImageIndex
                If nodSurveysGroup.Tag = sSelectedNodeTag Then tre.SelectedNode = nodSurveysGroup

                'Lambda Expression way
                Dim leSortBy As Func(Of ProjectDS.DEMSurveyRow, String, String) = Function(pKey, sField) pKey.Item(sField)
                Dim sSortBy As String = eSortSurveyBy.ToString()
                'For Each rSurveys As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows.OrderBy(leSortBy.Invoke(rSurveys, sSortBy))

                'Store DEM Survey Rows in an Ienumerable then loop over
                Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)


                ' DEM survyes
                'For Each rSurveys As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows.OrderBy(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Item(eSortSurveyBy.ToString))
                If sortedSurveys Is Nothing Then
                    Exit Sub
                End If

                For Each rSurveys As ProjectDS.DEMSurveyRow In sortedSurveys
                    Dim nodSurvey As TreeNode = nodSurveysGroup.Nodes.Add(GCDNodeTypes.DEMSurvey.ToString & "_" & rSurveys.DEMSurveyID.ToString, rSurveys.Name, GCDNodeTypes.DEMSurvey)
                    nodSurvey.Tag = GCDNodeTypes.DEMSurvey.ToString & "_" & rSurveys.DEMSurveyID.ToString
                    nodSurvey.SelectedImageIndex = nodSurvey.ImageIndex
                    If nodSurvey.Tag = sSelectedNodeTag Then tre.SelectedNode = nodSurvey
                    Dim bExpandSurveyNode As Boolean = False

                    ' Associated surfaces
                    Dim nodAssocGroup As TreeNode = nodSurvey.Nodes.Add(GCDNodeTypes.AssociatedSurfaceGroup.ToString, m_sAssocSurfaces, GCDNodeTypes.AssociatedSurfaceGroup)
                    nodAssocGroup.Tag = GCDNodeTypes.AssociatedSurfaceGroup.ToString
                    nodAssocGroup.SelectedImageIndex = nodAssocGroup.ImageIndex
                    If nodAssocGroup.Tag = sSelectedNodeTag Then tre.SelectedNode = nodAssocGroup

                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rSurveys.GetAssociatedSurfaceRows
                        Dim nodAssoc As TreeNode = nodAssocGroup.Nodes.Add(GCDNodeTypes.AssociatedSurface.ToString & "_" & rAssoc.AssociatedSurfaceID.ToString, rAssoc.Name, GCDNodeTypes.AssociatedSurface)
                        nodAssoc.Tag = GCDNodeTypes.AssociatedSurface.ToString & "_" & rAssoc.AssociatedSurfaceID.ToString
                        nodAssoc.SelectedImageIndex = nodAssoc.ImageIndex
                        If nodAssoc.Tag = sSelectedNodeTag Then tre.SelectedNode = nodAssoc
                        bExpandSurveyNode = True
                    Next

                    ' Error surfaces
                    Dim nodErrorGroup As TreeNode = nodSurvey.Nodes.Add(GCDNodeTypes.ErrorSurfaceGroup.ToString, m_sErrorSurfaces, GCDNodeTypes.ErrorSurfaceGroup)
                    nodErrorGroup.Tag = GCDNodeTypes.ErrorSurfaceGroup.ToString
                    nodErrorGroup.SelectedImageIndex = nodErrorGroup.ImageIndex
                    If nodErrorGroup.Tag = sSelectedNodeTag Then tre.SelectedNode = nodErrorGroup

                    For Each rError As ProjectDS.ErrorSurfaceRow In rSurveys.GetErrorSurfaceRows
                        Dim nodError As TreeNode = nodErrorGroup.Nodes.Add(GCDNodeTypes.ErrorSurface.ToString & "_" & rError.ErrorSurfaceID.ToString, rError.Name, GCDNodeTypes.ErrorSurface)
                        nodError.Tag = GCDNodeTypes.ErrorSurface.ToString & "_" & rError.ErrorSurfaceID.ToString
                        nodError.SelectedImageIndex = nodError.ImageIndex
                        If nodError.Tag = sSelectedNodeTag Then tre.SelectedNode = nodError
                        bExpandSurveyNode = True
                    Next

                    If bExpandSurveyNode Then nodSurvey.Expand()
                Next

                nodInputs.Expand()
                nodSurveysGroup.Expand()

                'Dim nodAOIGroup As TreeNode = nodInputs.Nodes.Add(GCDNodeTypes.AOIGroup.ToString, "Areas of Interest", GCDNodeTypes.AOIGroup)
                'nodAOIGroup.Tag = GCDNodeTypes.AOIGroup.ToString
                'nodAOIGroup.SelectedImageIndex = nodAOIGroup.ImageIndex
                'If nodAOIGroup.Tag = sSelectedNodeTag Then tre.SelectedNode = nodAOIGroup
                'nodAOIGroup.Expand()

                'Dim bAOI As Boolean = False
                'For Each rAOI As ProjectDS.AOIsRow In rProject.GetAOIsRows
                '    Dim nodAOI As TreeNode = nodAOIGroup.Nodes.Add(GCDNodeTypes.AOI.ToString, rAOI.Name, GCDNodeTypes.AOI)
                '    nodAOI.Tag = GCDNodeTypes.AOI.ToString & "_" & rAOI.AOIID.ToString
                '    nodAOI.SelectedImageIndex = nodAOI.ImageIndex
                '    If nodAOI.Tag = sSelectedNodeTag Then tre.SelectedNode = nodAOI
                '    bAOI = True
                'Next
                'nodAOIGroup.Expand()

                Dim AnalNode As TreeNode = nodProject.Nodes.Add(GCDNodeTypes.AnalysesGroup.ToString, "Analyses", GCDNodeTypes.AnalysesGroup)
                AnalNode.Tag = GCDNodeTypes.AnalysesGroup.ToString
                AnalNode.SelectedImageIndex = AnalNode.ImageIndex
                If AnalNode.Tag = sSelectedNodeTag Then tre.SelectedNode = AnalNode

                Dim CDNode As TreeNode = AnalNode.Nodes.Add(GCDNodeTypes.ChangeDetectionGroup.ToString, "Change Detection", GCDNodeTypes.ChangeDetectionGroup)
                CDNode.Tag = GCDNodeTypes.ChangeDetectionGroup.ToString
                CDNode.SelectedImageIndex = CDNode.ImageIndex
                If CDNode.Tag = sSelectedNodeTag Then tre.SelectedNode = CDNode
                Dim bExpandCDNode As Boolean = False

                Dim dDoD As New Dictionary(Of String, TreeNode)
                For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows
                    CDNode.Expand()

                    Dim sDEMPair As String = rDoD.NewSurveyName & " - " & rDoD.OldSurveyName
                    Dim theParent As TreeNode = Nothing
                    If dDoD.ContainsKey(sDEMPair) Then
                        ' This pair of DEMs already exists in the tree
                        theParent = dDoD(sDEMPair)
                    Else
                        ' Create a new parent of DEM surveys for this DoD

                        ' Find the two DEMs for this DEM and add their ID to the tag of the pair node.
                        Dim rNewDEM As ProjectDS.DEMSurveyRow = Nothing
                        Dim rOldDEM As ProjectDS.DEMSurveyRow = Nothing
                        For Each rDEM As ProjectDS.DEMSurveyRow In GCDProject.ProjectManagerBase.ds.DEMSurvey
                            If String.Compare(rDEM.Name, rDoD.NewSurveyName, True) = 0 Then
                                rNewDEM = rDEM
                            ElseIf String.Compare(rDEM.Name = rDoD.OldSurveyName, True) = 0 Then
                                rOldDEM = rDEM
                            End If
                        Next

                        theParent = CDNode.Nodes.Add(sDEMPair, sDEMPair, GCDNodeTypes.ChangeDetectionDEMPair)
                        theParent.SelectedImageIndex = theParent.ImageIndex
                        theParent.Tag = GCDNodeTypes.ChangeDetectionDEMPair.ToString

                        If TypeOf rNewDEM Is ProjectDS.DEMSurveyRow AndAlso TypeOf rOldDEM Is ProjectDS.DEMSurveyRow Then
                            theParent.Tag &= "_" & rNewDEM.DEMSurveyID.ToString & "_" & rOldDEM.DEMSurveyID.ToString
                        End If

                        dDoD.Add(sDEMPair, theParent)
                    End If

                    ' Now create the actual DoD node under the node for the pair of DEMs
                    Dim nodDoD As TreeNode = theParent.Nodes.Add(rDoD.Name, rDoD.Name, GCDNodeTypes.DoD)
                    nodDoD.SelectedImageIndex = nodDoD.ImageIndex
                    nodDoD.Tag = GCDNodeTypes.DoD.ToString & "_" & rDoD.DoDID.ToString
                    theParent.Expand()

                    ' DoD Summary XML node
                    'If Not rDoD.IsSummaryXMLPathNull Then
                    '    If IO.File.Exists(rDoD.SummaryXMLPath) Then
                    '        Dim nodSummary As TreeNode = nodDoD.Nodes.Add(GCDNodeTypes.SummaryXML.ToString & "_" & rDoD.DoDID.ToString, IO.Path.GetFileNameWithoutExtension(rDoD.SummaryXMLPath), GCDNodeTypes.SummaryXML)
                    '        nodSummary.Tag = GCDNodeTypes.SummaryXML.ToString & "_" & rDoD.DoDID.ToString
                    '        nodSummary.SelectedImageIndex = nodSummary.ImageIndex
                    '    End If
                    'End If

                    ' Budget Segregation Group Node
                    Dim nodBSGroup As TreeNode = Nothing
                    Dim sMaskDict As New Dictionary(Of String, String)
                    For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows
                        nodDoD.Expand()

                        ' Loop through and find all the unique polygon masks used
                        sMaskDict(rBS.PolygonMask) = IO.Path.GetFileNameWithoutExtension(rBS.PolygonMask)
                    Next

                    ' Now loop through all the BS and add them under the appropriate mask polygon node
                    For Each sPolygonMask As String In sMaskDict.Keys
                        Dim nodMask As TreeNode = Nothing

                        For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows
                            If String.Compare(sPolygonMask, rBS.PolygonMask, True) = 0 Then

                                If Not TypeOf nodBSGroup Is TreeNode Then
                                    nodBSGroup = nodDoD.Nodes.Add(GCDNodeTypes.BudgetSegregationGroup.ToString, m_sBudgetSegs, GCDNodeTypes.BudgetSegregationGroup)
                                    nodBSGroup.Tag = GCDNodeTypes.BudgetSegregationGroup.ToString
                                    nodBSGroup.SelectedImageIndex = nodBSGroup.ImageIndex
                                    nodBSGroup.Expand()
                                End If

                                If nodMask Is Nothing Then
                                    Dim sTag As String = GCDNodeTypes.BudgetSegregationMask.ToString & "_bs_" & naru.os.File.RemoveDangerousCharacters(rBS.PolygonMask) & "_dod_" & rDoD.DoDID
                                    nodMask = nodBSGroup.Nodes.Add(sTag, sMaskDict(sPolygonMask), GCDNodeTypes.BudgetSegregationMask)
                                    nodMask.SelectedImageIndex = nodMask.ImageIndex
                                    nodMask.Tag = sTag
                                End If

                                ' Budget Segregation
                                Dim nodBS As TreeNode = nodMask.Nodes.Add(GCDNodeTypes.BudgetSegregation.ToString & "_" & rBS.BudgetID.ToString, rBS.Name, GCDNodeTypes.BudgetSegregation)
                                nodBS.Tag = GCDNodeTypes.BudgetSegregation.ToString & "_" & rBS.BudgetID.ToString
                                nodBS.SelectedImageIndex = nodBS.ImageIndex
                            End If
                        Next
                    Next

                    bExpandCDNode = True
                    nodDoD.ExpandAll()
                Next

                AnalNode.Expand()
                CDNode.Expand()

                'nodProject.ExpandAll()
                nodProject.Expand()

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub treProject_DoubleClick(sender As Object, e As System.EventArgs) Handles treProject.DoubleClick

            Try

                Dim theNode As TreeNode = treProject.SelectedNode
                If TypeOf theNode Is TreeNode Then

                    Dim eType As GCDNodeTypes = GetNodeType(theNode)
                    Dim nID As Integer = GetNodeID(theNode)
                    Dim frm As Form = Nothing

                    Select Case eType
                        Case GCDNodeTypes.DEMSurvey
                            frm = New frmDEMSurveyProperties(nID)

                        Case GCDNodeTypes.AssociatedSurface
                            Dim nSurveyID As Integer = GetNodeID(theNode.Parent.Parent)
                            frm = New frmAssocSurfaceProperties(nSurveyID, nID)

                        Case GCDNodeTypes.ErrorSurface
                            Dim rError As ProjectDS.ErrorSurfaceRow = GCDProject.ProjectManagerBase.ds.ErrorSurface.FindByErrorSurfaceID(nID)
                            frm = New ErrorCalculation.frmErrorCalculation(rError)

                    End Select

                    If TypeOf frm Is Form Then
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(theNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Function GetNodeType(aNode As TreeNode) As GCDNodeTypes

            Dim sType As String = String.Empty
            If aNode.Tag.ToString.Contains("_") Then
                Dim nIndexOfSeparator As Integer = aNode.Tag.ToString.IndexOf("_")
                sType = aNode.Tag.ToString.Substring(0, nIndexOfSeparator)
            Else
                sType = aNode.Tag.ToString
            End If

            Dim eType As GCDNodeTypes
            Select Case sType
                Case GCDNodeTypes.Project.ToString : eType = GCDNodeTypes.Project
                Case GCDNodeTypes.InputsGroup.ToString : eType = GCDNodeTypes.InputsGroup
                Case GCDNodeTypes.SurveysGroup.ToString : eType = GCDNodeTypes.SurveysGroup
                Case GCDNodeTypes.DEMSurvey.ToString : eType = GCDNodeTypes.DEMSurvey
                Case GCDNodeTypes.AssociatedSurfaceGroup.ToString : eType = GCDNodeTypes.AssociatedSurfaceGroup
                Case GCDNodeTypes.AssociatedSurface.ToString : eType = GCDNodeTypes.AssociatedSurface
                Case GCDNodeTypes.ErrorSurfaceGroup.ToString : eType = GCDNodeTypes.ErrorSurfaceGroup
                Case GCDNodeTypes.ErrorSurface.ToString : eType = GCDNodeTypes.ErrorSurface
                Case GCDNodeTypes.AnalysesGroup.ToString : eType = GCDNodeTypes.AnalysesGroup
                Case GCDNodeTypes.ChangeDetectionGroup.ToString : eType = GCDNodeTypes.ChangeDetectionGroup
                Case GCDNodeTypes.ChangeDetectionDEMPair.ToString : eType = GCDNodeTypes.ChangeDetectionDEMPair
                Case GCDNodeTypes.DoD.ToString : eType = GCDNodeTypes.DoD
                'Case GCDNodeTypes.SummaryXML.ToString : eType = GCDNodeTypes.SummaryXML
                Case GCDNodeTypes.BudgetSegregationGroup.ToString : eType = GCDNodeTypes.BudgetSegregationGroup
                Case GCDNodeTypes.BudgetSegregationMask.ToString : eType = GCDNodeTypes.BudgetSegregationMask
                Case GCDNodeTypes.BudgetSegregation.ToString : eType = GCDNodeTypes.BudgetSegregation
                Case GCDNodeTypes.ReservoirGroup.ToString : eType = GCDNodeTypes.ReservoirGroup
                Case GCDNodeTypes.Reservoir.ToString : eType = GCDNodeTypes.Reservoir
                Case GCDNodeTypes.AOIGroup.ToString : eType = GCDNodeTypes.AOIGroup
                Case GCDNodeTypes.AOI.ToString : eType = GCDNodeTypes.AOI
                Case Else
                    Throw New Exception("Unhandled tree node type")
            End Select

            Return eType

        End Function

        Private Shared Function GetSortedSurveyRows(ByVal rProject As ProjectDS.ProjectRow, ByVal eSortSurveyBy As SortSurveyBy) As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow)

            'Store DEM Survey Rows in an Ienumerable then loop over
            Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = Nothing
            Select Case eSortSurveyBy

                Case SortSurveyBy.NameDsc
                    sortedSurveys = rProject.GetDEMSurveyRows.OrderByDescending(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Name)

                Case SortSurveyBy.NameAsc
                    sortedSurveys = rProject.GetDEMSurveyRows.OrderBy(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Name)

                Case SortSurveyBy.DEMSurveyID_Dsc
                    sortedSurveys = rProject.GetDEMSurveyRows.OrderByDescending(Function(pKey As ProjectDS.DEMSurveyRow) pKey.DEMSurveyID)

                Case SortSurveyBy.DEMSurveyID_Asc
                    sortedSurveys = rProject.GetDEMSurveyRows.OrderBy(Function(pKey As ProjectDS.DEMSurveyRow) pKey.DEMSurveyID)

                Case SortSurveyBy.SurveyDateAsc
                    sortedSurveys = rProject.GetDEMSurveyRows.OrderBy(Function(aKey As ProjectDS.DEMSurveyRow) If(Not aKey.IsSurveyYearNull(), aKey.SurveyYear, 0)).
                    ThenBy(Function(bKey As ProjectDS.DEMSurveyRow) If(Not bKey.IsSurveyMonthNull(), bKey.SurveyMonth, 0)).
                    ThenBy(Function(cKey As ProjectDS.DEMSurveyRow) If(Not cKey.IsSurveyDayNull, cKey.SurveyDay, 0)).
                    ThenBy(Function(dKey As ProjectDS.DEMSurveyRow) If(Not dKey.IsSurveyHourNull, dKey.SurveyHour, -1)).
                    ThenBy(Function(eKey As ProjectDS.DEMSurveyRow) If(Not eKey.IsSurveyMinNull, eKey.SurveyMin, -1))

                Case SortSurveyBy.SurveyDateDsc
                    sortedSurveys = rProject.GetDEMSurveyRows.OrderByDescending(Function(aKey As ProjectDS.DEMSurveyRow) If(Not aKey.IsSurveyYearNull(), aKey.SurveyYear, 0)).
                    ThenByDescending(Function(bKey As ProjectDS.DEMSurveyRow) If(Not bKey.IsSurveyMonthNull(), bKey.SurveyMonth, 0)).
                    ThenByDescending(Function(cKey As ProjectDS.DEMSurveyRow) If(Not cKey.IsSurveyDayNull, cKey.SurveyDay, 0)).
                    ThenByDescending(Function(dKey As ProjectDS.DEMSurveyRow) If(Not dKey.IsSurveyHourNull, dKey.SurveyHour, -1)).
                    ThenByDescending(Function(eKey As ProjectDS.DEMSurveyRow) If(Not eKey.IsSurveyMinNull, eKey.SurveyMin, -1))

            End Select

            Return sortedSurveys

        End Function

        Private Function GetNodeID(aNode As TreeNode) As Integer

            Dim nID As Integer = -1
            Dim nIndex As Integer = aNode.Tag.ToString.IndexOf("_")
            If nIndex > 0 Then
                Integer.TryParse(aNode.Tag.ToString.Substring(nIndex + 1), nID)
            End If

            Return nID

        End Function

        Private Sub treProject_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles treProject.MouseDown

            Try
                If e.Button = System.Windows.Forms.MouseButtons.Right Then
                    Dim theNode As TreeNode = treProject.GetNodeAt(e.X, e.Y)
                    If TypeOf theNode Is TreeNode Then
                        treProject.SelectedNode = theNode
                        Dim cms As ContextMenuStrip = Nothing
                        Select Case GetNodeType(theNode)

                            Case GCDNodeTypes.Project
                                cms = cmsProject

                            Case GCDNodeTypes.InputsGroup
                                cms = cmsInputsGroup

                            Case GCDNodeTypes.SurveysGroup
                                cms = cmsSurveysGroup

                            Case GCDNodeTypes.DEMSurvey
                                cms = cmsDEMSurvey

                            Case GCDNodeTypes.AssociatedSurfaceGroup
                                cms = cmsAssociatedSurfaceGroup

                            Case GCDNodeTypes.AssociatedSurface
                                cms = cmsAssociatedSurface

                            Case GCDNodeTypes.ErrorSurfaceGroup
                                cms = cmsErrorSurfacesGroup

                            Case GCDNodeTypes.ErrorSurface
                                cms = cmsErrorSurface

                            Case GCDNodeTypes.ChangeDetectionGroup
                                cms = cmsChangeDetectionGroup

                            Case GCDNodeTypes.DoD
                                cms = cmsChangeDetection

                            'Case GCDNodeTypes.SummaryXML
                            '    cms = cmsSummaryXML

                            Case GCDNodeTypes.AOIGroup : cms = cmsAOIGroup
                            Case GCDNodeTypes.AOI : cms = cmsAOI
                            Case GCDNodeTypes.ChangeDetectionDEMPair : cms = cmsDEMSurveyPair
                            Case GCDNodeTypes.BudgetSegregationGroup : cms = cmsBSGroup
                            Case GCDNodeTypes.BudgetSegregation : cms = cmsBS
                            Case GCDNodeTypes.BudgetSegregationMask : cms = cmsBSGroup

                        End Select

                        If TypeOf cms Is ContextMenuStrip Then

                            ' Hide any GIS related menu items in standalone mode
                            If IsStandaloneMode Then
                                For Each item As ToolStripItem In cms.Items
                                    item.Visible = Not item.Text.ToLower().Contains("map")
                                Next
                            End If

                            cms.Show(treProject, New Point(e.X, e.Y))
                        End If
                    End If

                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Public Function AddDEMSurvey() As Integer

            Dim nDEMSurveyID As Integer = 0
            Dim gReferenceRaster As GCDConsoleLib.Raster = Nothing

            If Core.GCDProject.ProjectManager.ds.DEMSurvey.Rows.Count > 0 Then
                Dim sRasterPath As String = Core.GCDProject.ProjectManager.GetAbsolutePath(Core.GCDProject.ProjectManager.ds.DEMSurvey.Item(0).Source)
                gReferenceRaster = New GCDConsoleLib.Raster(sRasterPath)
            End If
            Dim frmImport As New UI.SurveyLibrary.frmImportRaster(gReferenceRaster, Nothing, frmImportRaster.ImportRasterPurposes.DEMSurvey, "DEM Survey")
            If frmImport.ShowDialog = System.Windows.Forms.DialogResult.OK Then

                Dim gRaster As GCDConsoleLib.Raster = frmImport.ProcessRaster
                If TypeOf gRaster Is GCDConsoleLib.Raster Then

                    Dim sRelativeRasterPath As String = Core.GCDProject.ProjectManager.GetRelativePath(frmImport.txtRasterPath.Text)

                    Dim demRow As ProjectDS.DEMSurveyRow = Core.GCDProject.ProjectManager.ds.DEMSurvey.AddDEMSurveyRow(frmImport.txtName.Text, sRelativeRasterPath,
                    "[Undefined]", "", "", Core.GCDProject.ProjectManager.CurrentProject, True, False, "", frmImport.valCellSize.Value,
                    frmImport.valLeft.Value, frmImport.valBottom.Value, frmImport.valRight.Value, frmImport.valTop.Value,
                    frmImport.OriginalExtent.Left, frmImport.OriginalExtent.Bottom, frmImport.OriginalExtent.Right, frmImport.OriginalExtent.Top,
                    frmImport.ucRaster.SelectedItem.FilePath, System.Net.Dns.GetHostName, frmImport.valCellSize.Value, Nothing, Nothing, Nothing, Nothing, Nothing)
                    Core.GCDProject.ProjectManager.save()

                    nDEMSurveyID = demRow.DEMSurveyID

                    LoadTree(GCDNodeTypes.DEMSurvey.ToString & "_" & nDEMSurveyID.ToString)

                    If My.Settings.AddOutputLayersToMap Then
                        GCDProject.ProjectManagerUI.ArcMapManager.AddSurvey(demRow)
                    End If

                    Dim SurveyForm As New UI.SurveyLibrary.frmDEMSurveyProperties(demRow.DEMSurveyID)
                    SurveyForm.ShowDialog()

                    ' Load the tree again because the use may have added Assoc or error surfaces
                    ' while the form was open (and since the tree was last loaded)
                    LoadTree(GCDNodeTypes.DEMSurvey.ToString & "_" & nDEMSurveyID.ToString)

                End If

                LoadTree(GCDNodeTypes.DEMSurvey.ToString & "_" & nDEMSurveyID.ToString)
            End If

            Return nDEMSurveyID

        End Function

#Region "Associated Surface Group Menu Items"

        Private Sub AddAssociatedSurfaceToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddAssociatedSurfaceToolStripMenuItem1.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AssociatedSurfaceGroup Then
                        Dim nParentID As Integer = GetNodeID(selNode.Parent)
                        Dim frm As New frmAssocSurfaceProperties(nParentID)

                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(selNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddAllAssociatedSurfacesToTheMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AssociatedSurfaceGroup Then
                        Dim nParentID As Integer = GetNodeID(selNode.Parent)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nParentID)
                        If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEMSurvey.GetAssociatedSurfaceRows
                                ' TODO 
                                Throw New Exception("not implemented")
                                '      GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
                            Next
                        End If
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub



#End Region

#Region "Associated Surface Menu Items"

        Private Sub EditPropertiesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditPropertiesToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AssociatedSurface Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim nParentID As Integer = GetNodeID(selNode.Parent.Parent)
                        If nID >= 0 Then
                            Dim frm As New frmAssocSurfaceProperties(nParentID, nID)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadTree(selNode.Tag)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem1.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AssociatedSurface Then
                        Dim nParentID As Integer = GetNodeID(selNode.Parent.Parent)
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nParentID)
                        If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEMSurvey.GetAssociatedSurfaceRows
                                If rAssoc.AssociatedSurfaceID = nID Then
                                    ' TODO 
                                    Throw New Exception("not implemented")
                                    '    GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#End Region

#Region "Error Surface Group Menu Items"

        Private Sub DeriveErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles _
        DeriveErrorSurfaceToolStripMenuItem.Click _
        , DeriveErrorSurfaceToolStripMenuItem1.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    Dim nID As Integer = 0
                    If eType = GCDNodeTypes.DEMSurvey Then
                        nID = GetNodeID(selNode)
                    ElseIf eType = GCDNodeTypes.ErrorSurfaceGroup Then
                        nID = GetNodeID(selNode.Parent)
                    End If

                    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nID)
                    If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                        Dim frm As New ErrorCalculation.frmErrorCalculation(rDEMSurvey)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(selNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                Core.ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub SpecifyErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles _
        AddErrorSurfaceToolStripMenuItem1.Click,
            AddErrorSurfaceToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    Dim nID As Integer = 0
                    If eType = GCDNodeTypes.DEMSurvey Then
                        nID = GetNodeID(selNode)
                    ElseIf eType = GCDNodeTypes.ErrorSurfaceGroup Then
                        nID = GetNodeID(selNode.Parent)
                    End If

                    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = Core.GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nID)
                    If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                        Dim rError As ProjectDS.ErrorSurfaceRow = frmDEMSurveyProperties.SpecifyErrorSurface(rDEMSurvey)
                        If TypeOf rError Is ProjectDS.ErrorSurfaceRow Then
                            If My.Settings.AddOutputLayersToMap Then
                                ' TODO 
                                Throw New Exception("not implemented")
                                'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
                            End If
                            LoadTree(GCDNodeTypes.ErrorSurface.ToString & "_" & rError.ErrorSurfaceID.ToString)
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddErrorSurfaceToMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ErrorSurfaceGroup Then
                        Dim nParentID As Integer = GetNodeID(selNode.Parent)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nParentID)
                        If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                            For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
                                ' TODO 
                                Throw New Exception("not implemented")
                                ' GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#End Region

#Region "Error Surface Menu Items"

        Private Sub EditErrorSurfacePropertiesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditErrorSurfacePropertiesToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ErrorSurface Then
                        Dim nErrorID As Integer = GetNodeID(selNode)
                        Dim rError As ProjectDS.ErrorSurfaceRow = GCDProject.ProjectManager.ds.ErrorSurface.FindByErrorSurfaceID(nErrorID)
                        If TypeOf rError Is ProjectDS.ErrorSurfaceRow Then
                            Dim frm As New ErrorCalculation.frmErrorCalculation(rError)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadTree(selNode.Tag)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub AddErrorSurfaceToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToMapToolStripMenuItem1.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ErrorSurface Then
                        Dim nParentID As Integer = GetNodeID(selNode.Parent.Parent)
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nParentID)
                        If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                            For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
                                If rError.ErrorSurfaceID = nID Then
                                    ' TODO 
                                    Throw New Exception("not implemented")
                                    ' GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub DeleteErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeleteErrorSurfaceToolStripMenuItem.Click
            'see btnDelete_Click
        End Sub

#End Region

#Region "GCD Project Menu Items"

        Private Sub EditGCDProjectPropertiesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditGCDProjectPropertiesToolStripMenuItem.Click

            Dim frm As New frmProjectProperties(False)
            Try
                frm.ShowDialog()
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub ToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem2.Click

            'TODO entire function contents commented out
            Throw New Exception("not implemented")
            'Try
            '    Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManagerBase.CurrentProject

            '    'TODO: Insert the GetSortedSurveyRowsMethod
            '    If TypeOf rProject Is ProjectDS.ProjectRow Then

            '        'Store DEM Survey Rows in an Ienumerable then loop over
            '        Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)
            '        For Each rDEM As ProjectDS.DEMSurveyRow In sortedSurveys.Reverse()
            '            GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEM)

            '            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows
            '                GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
            '            Next

            '            For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
            '                GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            '            Next
            '        Next

            '        For Each rAOI As ProjectDS.AOIsRow In rProject.GetAOIsRows
            '            GCDProject.ProjectManagerUI.ArcMapManager.AddAOI(rAOI)
            '        Next

            '        For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows
            '            GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(rDoD)
            '        Next
            '    End If

            'Catch ex As Exception
            '    ExceptionHelper.HandleException(ex)
            'End Try

        End Sub

        Private Sub ToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles _
        ToolStripMenuItem1.Click,
        AddDEMSurveyToolStripMenuItem.Click

            Try
                Dim nDEMSurveyID As Integer = AddDEMSurvey()
                If nDEMSurveyID > 0 Then
                    Dim sNodeTag As String = GCDNodeTypes.DEMSurvey.ToString & "_" & nDEMSurveyID.ToString
                    LoadTree(sNodeTag)
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#End Region

#Region "DEM Survey Menu Items"

        Private Sub EditDEMSurveyProperatieToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditDEMSurveyProperatieToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DEMSurvey Then
                        Dim nID As Integer = GetNodeID(selNode)
                        If nID >= 0 Then
                            Dim frm As New frmDEMSurveyProperties(nID)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadTree(selNode.Tag)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddToMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DEMSurvey Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nID)
                        If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                            ' TODO 
                            Throw New Exception("not implemented")
                            '  GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEMSurvey)
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddAssociatedSurfaceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAssociatedSurfaceToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DEMSurvey Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nID)
                        If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
                            Dim frm As New frmAssocSurfaceProperties(nID)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadTree(selNode.Tag)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub


        'Private Sub AddErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToolStripMenuItem.Click

        '    Try
        '        Dim selNode As TreeNode = treProject.SelectedNode
        '        If TypeOf selNode Is TreeNode Then
        '            Dim eType As GCDNodeTypes = GetNodeType(selNode)
        '            If eType = GCDNodeTypes.DEMSurvey Then
        '                Dim nID As Integer = GetNodeID(selNode)
        '                Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nID)
        '                If TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
        '                    Dim frm As New ErrorCalculationForm(My.ThisApplication, rDEMSurvey)
        '                    If frm.ShowDialog() = DialogResult.OK Then
        '                        LoadTree(selNode.Tag)
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Catch ex As Exception
        '        ExceptionUI.HandleException(ex)
        '    End Try
        'End Sub

#End Region

#Region "Inputs Group Menu Items"

        'Private Sub AddDEMSurveyToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddDEMSurveyToolStripMenuItem.Click

        '    Dim nDEMSurveyID As Integer = AddDEMSurvey()
        '    If nDEMSurveyID > 0 Then
        '        Dim sNodeTag As String = GCDNodeTypes.DEMSurvey.ToString & "_" & nDEMSurveyID.ToString
        '        LoadTree(sNodeTag)
        '    End If
        'End Sub

        Private Sub AddAllDEMSurveysToTheMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddAllDEMSurveysToTheMapToolStripMenuItem.Click

            Try
                Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManager.CurrentProject
                If TypeOf rProject Is ProjectDS.ProjectRow Then

                    'Store DEM Survey Rows in an Ienumerable then loop over
                    Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)

                    For Each rDEMSurvey As ProjectDS.DEMSurveyRow In sortedSurveys.Reverse()
                        ' TODO 
                        Throw New Exception("not implemented")
                        '  GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEMSurvey)
                    Next

                    For Each rAOI As ProjectDS.AOIsRow In rProject.GetAOIsRows
                        ' TODO 
                        Throw New Exception("not implemented")
                        '  GCDProject.ProjectManagerUI.ArcMapManager.AddAOI(rAOI)
                    Next
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#End Region

#Region "Change Detection Group Menu Items"

        Private Sub AddChangeDetectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddChangeDetectionToolStripMenuItem.Click

            AddDoDChangeDetection()

            'Try
            '    Dim frmDoDCalculation As New DoDPropertiesForm(My.ThisApplication)
            '    DoChangeDetection(frmDoDCalculation)

            'Catch ex As Exception
            '    ExceptionUI.HandleException(ex)
            'End Try
        End Sub

        ''' <summary>
        ''' Allows for a change detection to be added 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddDoDChangeDetection()

            Try
                Dim frmDoDCalculation As New ChangeDetection.frmDoDProperties()
                DoChangeDetection(frmDoDCalculation)

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub DoChangeDetection(ByRef frmDoDCalculation As ChangeDetection.frmDoDProperties)

            Try
                If frmDoDCalculation.ShowDialog() = DialogResult.OK Then
                    Dim sTag As String = String.Empty
                    If TypeOf frmDoDCalculation.DoDRow Is ProjectDS.DoDsRow Then
                        sTag = GCDNodeTypes.DoD.ToString & "_" & frmDoDCalculation.DoDRow.DoDID.ToString
                        LoadTree(sTag)

                        ' Now show the results form for this new DoD Calculation
                        Dim frmResults As New ChangeDetection.frmDoDResults(frmDoDCalculation.DoDResults, frmDoDCalculation.DoDRow)
                        frmResults.ShowDialog()
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ChangeDetectionGroup Then
                        For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManagerBase.CurrentProject.GetDoDsRows
                            ' TODO 
                            Throw New Exception("not implemented")
                            '   GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(rDoD)
                        Next
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#End Region

#Region "Change Detection Menu Items"

        Private Sub AddChangeDetectionToTheMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddChangeDetectionToTheMapToolStripMenuItem.Click

            Try

                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DoD Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDod As ProjectDS.DoDsRow = GCDProject.ProjectManager.ds.DoDs.FindByDoDID(nID)
                        If TypeOf rDod Is ProjectDS.DoDsRow Then
                            ' TODO 
                            Throw New Exception("not implemented")
                            '  GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(rDod)
                        End If
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub AddRawChangeDetectionToTheMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddRawChangeDetectionToTheMapToolStripMenuItem.Click
            Try

                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DoD Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDod As ProjectDS.DoDsRow = GCDProject.ProjectManager.ds.DoDs.FindByDoDID(nID)
                        If TypeOf rDod Is ProjectDS.DoDsRow Then
                            ' TODO 
                            Throw New Exception("not implemented")
                            '  GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(rDod, False)
                        End If
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub ViewChangeDetectionResultsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewChangeDetectionResultsToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DoD Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDoD As ProjectDS.DoDsRow = GCDProject.ProjectManagerBase.ds.DoDs.FindByDoDID(nID)
                        If TypeOf rDoD Is ProjectDS.DoDsRow Then

                            Dim changeStats As New Core.ChangeDetection.ChangeStatsFromDoDRow(rDoD)
                            Dim sRawHistoStats As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.RawHistPath)
                            Dim sThrHistoStats As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshHistPath)

                            Dim changeHistos As New Core.ChangeDetection.DoDResultHistograms(sRawHistoStats, sThrHistoStats)
                            Dim dodProp As Core.ChangeDetection.ChangeDetectionProperties = Core.ChangeDetection.ChangeDetectionProperties.CreateFromDoDRow(rDoD)
                            Dim rResults As New Core.ChangeDetection.DoDResultSet(changeStats, changeHistos, dodProp)
                            Dim frm As New ChangeDetection.frmDoDResults(rResults, rDoD)
                            If frm.ShowDialog() = DialogResult.OK Then
                                ' Don't need to reload the tree here. Nothing changed.
                                ' LoadTree(selNode.Tag)
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub DeleteDoD(ByVal rDoD As ProjectDS.DoDsRow)

            'TODO entire contents commented out
            Throw New Exception("not implemented")

            'If Not TypeOf rDoD Is ProjectDS.DoDsRow Then
            '    Exit Sub
            'End If


            'Dim response As MsgBoxResult = MsgBox("Are you sure you want to remove the selected change detection?  Note: this will remove the change detection from the GCD Project (*.gcd) file and also delete the copy of the raster in the GCD project folder.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.ApplicationNameLong)
            'If response = MsgBoxResult.Yes Then

            '    Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = DirectCast(My.ThisApplication, ESRI.ArcGIS.Framework.IApplication).Document
            '    Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
            '    Dim projectName As String = rDoD.Name

            '    ' Both raw and thresholded DoD rasters can be in the map. Make a list and remove both.
            '    Dim lDoDSource As New List(Of String)
            '    lDoDSource.Add(GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.RawDoDPath))
            '    lDoDSource.Add(GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshDoDPath))

            '    For Each sDoDSource As String In lDoDSource
            '        'New code to remove project layer from map if in map
            '        Dim enumLayer As ESRI.ArcGIS.Carto.IEnumLayer = pMap.Layers
            '        Dim pLayer As ESRI.ArcGIS.Carto.ILayer = enumLayer.Next()

            '        While pLayer IsNot Nothing
            '            If TypeOf pLayer Is ESRI.ArcGIS.Carto.RasterLayer Then
            '                If String.Compare(sDoDSource, DirectCast(pLayer, ESRI.ArcGIS.Carto.RasterLayer).FilePath, True) = 0 Then
            '                    pMap.DeleteLayer(pLayer)
            '                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayer)
            '                    pLayer = Nothing
            '                    Exit While
            '                End If
            '            End If
            '            pLayer = enumLayer.Next()
            '        End While
            '    Next

            '    Try
            '        'DeleteSurveyFiles(IO.Path.GetDirectoryName(CurrentRow.Item("Source")))
            '        'IO.Directory.Delete(IO.Path.GetDirectoryName(CurrentRow.Item("Source")), True)
            '        DeleteSurveyFiles(IO.Path.GetDirectoryName(lDoDSource(0)))
            '        IO.Directory.Delete(IO.Path.GetDirectoryName(lDoDSource(0)), True)
            '    Catch ex As Exception
            '        ExceptionHelper.HandleException(ex, "The GCD survey files failed to delete. Some of the files associated with this survey still exist.")
            '        'Dim ex2 As New Exception("The GCD survey files failed to delete. Some of the files associated with this survey still exist.", ex)
            '        'ex2.Data.Add("Project Name: ", projectName)
            '        'Throw ex2
            '    Finally
            '        rDoD.Delete()
            '        GCDProject.ProjectManager.save()
            '    End Try

            'End If
        End Sub

#End Region

        Private Sub DeleteDEMSurvey(ByVal rDEMSurvey As ProjectDS.DEMSurveyRow)

            'TODO
            Throw New Exception("not implemented")

            'If Not TypeOf rDEMSurvey Is ProjectDS.DEMSurveyRow Then
            '    Exit Sub
            'End If


            'Dim response As MsgBoxResult = MsgBox("Are you sure you want to remove the selected survey? Note this removes ALL FILES associated with this survey from the map and the project: digital elevation models, hillshades, associated surfaces, and error surfaces.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.ApplicationNameLong)
            'If response = MsgBoxResult.Yes Then

            '    Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = DirectCast(My.ThisApplication, ESRI.ArcGIS.Framework.IApplication).Document
            '    Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
            '    Dim projectName As String = rDEMSurvey.Name
            '    Dim pSurveySource As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDEMSurvey.Source)

            '    'New code to remove project layer from map if in map
            '    Dim enumLayer As ESRI.ArcGIS.Carto.IEnumLayer = pMap.Layers
            '    Dim pLayer As ESRI.ArcGIS.Carto.ILayer = enumLayer.Next()

            '    While pLayer IsNot Nothing
            '        If String.Compare(projectName, pLayer.Name, True) = 0 Then
            '            pMap.DeleteLayer(pLayer)
            '            System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayer)
            '            pLayer = Nothing
            '            Exit While
            '        End If
            '        pLayer = enumLayer.Next()
            '    End While

            '    Try

            '        DeleteSurveyFiles(IO.Path.GetDirectoryName(pSurveySource))
            '        For Each sSubFolder As String In IO.Directory.GetDirectories(IO.Path.GetDirectoryName(pSurveySource))
            '            Try
            '                IO.Directory.Delete(sSubFolder, True)
            '            Catch ex As Exception
            '                'Do Nothing because if should just be a esri lock file if it can't be deleted
            '                Debug.Print(ex.Message)
            '            End Try
            '        Next

            '    Catch ex As Exception
            '        ExceptionHelper.HandleException(ex, "Some of the folders associated with this survey still exist because an ESRI lock file cannot be deleted while current ArcMap session is still running.")
            '    Finally
            '        rDEMSurvey.Delete()
            '        GCDProject.ProjectManagerBase.save()
            '    End Try

            'End If
        End Sub

        Private Sub DeleteSurveyFiles(ByVal sFolder As String)

            If String.IsNullOrEmpty(sFolder) Then
                Throw New ArgumentNullException(sFolder, "Null or empty project folder path")
            End If

            If Not IO.Directory.Exists(sFolder) Then
                Exit Sub
            End If

            'TODO loop below commented out
            Throw New Exception("not implemented")
            'Code written to replace the commented out code below that was not working - Hensleigh 12/10/2013
            'Dim pWorkspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.RasterFile)
            'Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IWorkspace = pWorkspaceFactory.OpenFromFile(sFolder, 0)
            'Dim pEnumerateDataset As ESRI.ArcGIS.Geodatabase.IEnumDataset = pWorkspace.Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
            'Dim pDataset As ESRI.ArcGIS.Geodatabase.IDataset = pEnumerateDataset.Next
            'Do While TypeOf pDataset Is ESRI.ArcGIS.Geodatabase.IDataset
            '    Try
            '        If pDataset.CanDelete() Then
            '            pDataset.Delete()
            '        End If
            '    Catch ex As Exception
            '        Debug.Print(ex.Message)
            '    Finally
            '        pDataset = pEnumerateDataset.Next
            '    End Try

            'Loop
            'pWorkspace = Nothing

            For Each sSubFolder As String In IO.Directory.GetDirectories(sFolder)
                DeleteSurveyFiles(sSubFolder)
            Next

        End Sub

        Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        DeleteAssociatedSurfaceToolStripMenuItem.Click,
        DeleteErrorSurfaceToolStripMenuItem.Click,
        DeleteDEMSurveyToolStripMenuItem.Click,
        DeleteChangeDetectionToolStripMenuItem.Click,
        DeleteDEMSurveyToolStripMenuItem.Click,
        DeleteChangeDetectionToolStripMenuItem.Click,
        DeleteAOIToolStripMenuItem.Click

            Dim nodSelected As TreeNode = treProject.SelectedNode
            If Not TypeOf nodSelected Is TreeNode Then
                Exit Sub
            End If

            Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
            Dim nID As Integer = GetNodeID(nodSelected)
            Select Case eType
                Case GCDNodeTypes.DEMSurvey
                    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nID)
                    DeleteDEMSurvey(rDEMSurvey)
                    LoadTree(nodSelected.Parent.Tag)

                Case GCDNodeTypes.AssociatedSurface
                    Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
                    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEMSurvey.GetAssociatedSurfaceRows
                        If rAssoc.AssociatedSurfaceID = nID Then
                            frmDEMSurveyProperties.DeleteAssociatedSurface(rAssoc)
                            LoadTree(nodSelected.Parent.Tag)
                        End If
                    Next

                Case GCDNodeTypes.ErrorSurface
                    Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
                    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
                    For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
                        If rError.ErrorSurfaceID = nID Then
                            frmDEMSurveyProperties.DeleteErrorSurface(rError)
                            LoadTree(nodSelected.Parent.Tag)
                        End If
                    Next

                Case GCDNodeTypes.DoD
                    Dim nDoDID As Integer = GetNodeID(nodSelected)
                    For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManagerBase.ds.DoDs
                        If rDoD.DoDID = nDoDID Then
                            DeleteDoD(rDoD)
                            LoadTree(nodSelected.Parent.Tag)
                            Exit For
                        End If
                    Next

                Case GCDNodeTypes.AOI
                    MsgBox("TODO")

            End Select

        End Sub

#Region "Properties Button"

        Private Sub btnProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode
                If Not TypeOf nodSelected Is TreeNode Then
                    Exit Sub
                End If

                Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
                Dim nID As Integer = GetNodeID(nodSelected)
                Dim frm As Form = Nothing
                Select Case eType

                    Case GCDNodeTypes.Project
                        frm = New frmProjectProperties(True)

                    Case GCDNodeTypes.DEMSurvey
                        frm = New frmDEMSurveyProperties(nID)

                    Case GCDNodeTypes.AssociatedSurface
                        Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
                        Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
                        For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEMSurvey.GetAssociatedSurfaceRows
                            If rAssoc.AssociatedSurfaceID = nID Then
                                frm = New frmAssocSurfaceProperties(nID, nDEMSurveyID)
                            End If
                        Next

                        'Case GCDNodeTypes.ErrorSurface
                        '    Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
                        '    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
                        '    For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
                        '        If rError.ErrorSurfaceID = nID Then
                        '            Dim frm As Newe
                        '        End If
                        '    Next

                End Select

                If TypeOf frm Is Form Then
                    If frm.ShowDialog = DialogResult.OK Then
                        LoadTree()
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#End Region

        Private Sub btnAddToMap_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            'TODO: entire function contents commented out
            Throw New Exception("not implemented")

            'Try
            '    Dim nodSelected As TreeNode = treProject.SelectedNode
            '    If Not TypeOf nodSelected Is TreeNode Then
            '        Exit Sub
            '    End If

            '    Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
            '    Dim nID As Integer = GetNodeID(nodSelected)

            '    Select Case eType
            '        Case GCDNodeTypes.InputsGroup
            '            For Each rDEMSurvey As ProjectDS.DEMSurveyRow In GCDProject.ProjectManager.CurrentProject.GetDEMSurveyRows
            '                GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEMSurvey)
            '                For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEMSurvey.GetAssociatedSurfaceRows
            '                    GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
            '                Next

            '                For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
            '                    GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            '                Next
            '            Next

            '        Case GCDNodeTypes.DEMSurvey
            '            Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nID)
            '            GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEMSurvey)

            '        Case GCDNodeTypes.AssociatedSurface, GCDNodeTypes.AssociatedSurfaceGroup
            '            Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
            '            Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
            '            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEMSurvey.GetAssociatedSurfaceRows
            '                If nID = -1 OrElse rAssoc.AssociatedSurfaceID = nID Then
            '                    GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
            '                End If
            '            Next

            '        Case GCDNodeTypes.ErrorSurface, GCDNodeTypes.ErrorSurfaceGroup
            '            Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
            '            Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
            '            For Each rError As ProjectDS.ErrorSurfaceRow In rDEMSurvey.GetErrorSurfaceRows
            '                If nID = -1 OrElse rError.ErrorSurfaceID = nID Then
            '                    GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            '                End If
            '            Next

            '        Case GCDNodeTypes.DoD
            '            For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.ds.DoDs.Rows
            '            Next
            '    End Select

            'Catch ex As Exception
            '    ExceptionHelper.HandleException(ex)
            'End Try

        End Sub

        Public Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim sSortBy As String = sender.ToString

            'If String.Compare(sSortBy, "Name") = 0 Then
            '    m_eSortBy = SortSurveyBy.Name
            'ElseIf String.Compare(sSortBy, "Survey date") = 0 Then
            '    m_eSortBy = SortSurveyBy.SurveyDate
            'Else
            '    m_eSortBy = SortSurveyBy.DEMSurveyID
            'End If

            LoadTree(Nothing, m_eSortBy)

        End Sub

        Private Sub SortTOC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        SurveyDateDescendingToolStripMenuItem.Click,
        SurveyDateAscendingToolStripMenuItem.Click,
        NameDescendingToolStripMenuItem.Click,
        NameAscendingToolStripMenuItem.Click,
        DateAddedDescendingToolStripMenuItem.Click,
        DateAddedAscendingToolStripMenuItem.Click

            'TODO entire function contents commented out
            Throw New Exception("not implemented")

            ''Get the name of the menu item that was clicked
            'Dim pMenuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            'Dim pParentMenuItem As ToolStripMenuItem = CType(pMenuItem.OwnerItem, ToolStripMenuItem)


            ''Change the image of the selected tag to a Check mark
            'pMenuItem.Image = My.Resources.Check
            'pParentMenuItem.Image = My.Resources.Check

            ''Set other menu item images to nothing
            'For Each pTempMenuItem As ToolStripMenuItem In pMenuItem.Owner.Items
            '    If Not String.Compare(pTempMenuItem.Text, pMenuItem.Text) = 0 Then
            '        pTempMenuItem.Image = Nothing
            '    End If
            'Next

            'RefreshMenuStripImages(pParentMenuItem)


            ''Assign the proper enumeration value to the member sort by member variable
            'If String.Compare(pParentMenuItem.Text, "Name") = 0 Then
            '    If String.Compare(pMenuItem.Text, "Ascending") = 0 Then
            '        m_eSortBy = SortSurveyBy.NameAsc
            '    ElseIf String.Compare(pMenuItem.Text, "Descending") = 0 Then
            '        m_eSortBy = SortSurveyBy.NameDsc
            '    End If
            'ElseIf String.Compare(pParentMenuItem.Text, "Survey date") = 0 Then
            '    If String.Compare(pMenuItem.Text, "Ascending") = 0 Then
            '        m_eSortBy = SortSurveyBy.SurveyDateAsc
            '    ElseIf String.Compare(pMenuItem.Text, "Descending") = 0 Then
            '        m_eSortBy = SortSurveyBy.SurveyDateDsc
            '    End If
            'ElseIf String.Compare(pParentMenuItem.Text, "Date added") = 0 Then
            '    If String.Compare(pMenuItem.Text, "Ascending") = 0 Then
            '        m_eSortBy = SortSurveyBy.DEMSurveyID_Asc
            '    ElseIf String.Compare(pMenuItem.Text, "Descending") = 0 Then
            '        m_eSortBy = SortSurveyBy.DEMSurveyID_Dsc
            '    End If
            'Else
            '    Throw New Exception("Unsupported sorting order selected.")
            'End If

            ''Load the tree using the sort by variable
            'LoadTree(Nothing, m_eSortBy)

            'Try
            '    Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManager.CurrentProject
            '    If TypeOf rProject Is ProjectDS.ProjectRow Then

            '        'Loop over the survey rows in the order provided by the m_eSortBy
            '        'For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows.OrderByDescending(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Item(m_eSortBy.ToString()))
            '        'Store DEM Survey Rows in an Ienumerable then loop over
            '        Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)


            '        ' DEM survyes
            '        'For Each rSurveys As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows.OrderBy(Function(pKey As ProjectDS.DEMSurveyRow) pKey.Item(eSortSurveyBy.ToString))
            '        If sortedSurveys Is Nothing Then
            '            Exit Sub
            '        End If

            '        For Each rDEM In sortedSurveys.Reverse()

            '            'Test to see if the group layer for the survey is in the map
            '            Dim pTest As ESRI.ArcGIS.Carto.ILayer = ArcMap.GetLayerByName(rDEM.Name, My.ThisApplication, ArcMap.eEsriLayerType.Esri_GroupLayer)

            '            'If it is in map then we will apply methods to add it in the order that the ProjectExplorerUC is now ordered in
            '            If pTest IsNot Nothing Then

            '                'The survey group layer is in the map so get the group variable
            '                Dim pSurveyGroupLayer As ESRI.ArcGIS.Carto.IGroupLayer = ArcMap.GetGroupLayer(My.ThisApplication, rDEM.Name, False)

            '                'Check to see if associated surfaces and/or error surfaces are in the map for that survey
            '                Dim bAddAssociatedSurfaces = ArcMap.SubGroupLayerExists(My.ThisApplication, "Associated Surfaces", pSurveyGroupLayer)
            '                Dim bAddErrorSurfaces = ArcMap.SubGroupLayerExists(My.ThisApplication, "Error Surfaces", pSurveyGroupLayer)

            '                'Create empty layer variable to be used and empty list of associated surface rows
            '                Dim pLayer As ESRI.ArcGIS.Carto.ILayer = Nothing
            '                Dim lAssocLayers As List(Of ProjectDS.AssociatedSurfaceRow) = New List(Of ProjectDS.AssociatedSurfaceRow)

            '                'Associated surfaces were in the map for this survey
            '                If bAddAssociatedSurfaces Then
            '                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows

            '                        'Get path of associated surface source path, get group layer, and check for presence of associated surface in associated surface sub layer of survey layer
            '                        Dim sPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rAssoc.Source)
            '                        Dim pAssocGroupLayer As ESRI.ArcGIS.Carto.IGroupLayer = ArcMap.GetGroupLayer(My.ThisApplication, "Associated Surfaces", pSurveyGroupLayer)
            '                        pLayer = GCDProject.ProjectManagerUI.ArcMapManager.IsLayerInGroupLayer(sPath, pAssocGroupLayer)
            '                        If pLayer IsNot Nothing Then
            '                            'Add associated surface row to list to be used later
            '                            lAssocLayers.Add(rAssoc)
            '                        End If
            '                    Next
            '                End If

            '                'Create  empty list of error surface rows
            '                Dim lErrorLayers As List(Of ProjectDS.ErrorSurfaceRow) = New List(Of ProjectDS.ErrorSurfaceRow)

            '                'Error surfaces were in the map for this survey
            '                If bAddErrorSurfaces Then
            '                    For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows

            '                        'Get path of error surface source path, get group layer, and check for presence of error surface in error surface sub layer of survey layer
            '                        Dim sPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rError.Source)
            '                        Dim pErrorGroupLayer As ESRI.ArcGIS.Carto.IGroupLayer = ArcMap.GetGroupLayer(My.ThisApplication, "Error Surfaces", pSurveyGroupLayer)
            '                        pLayer = GCDProject.ProjectManagerUI.ArcMapManager.IsLayerInGroupLayer(sPath, pErrorGroupLayer)
            '                        If pLayer IsNot Nothing Then
            '                            'Add error surface row to list to be used later
            '                            lErrorLayers.Add(rError)
            '                        End If
            '                    Next
            '                End If

            '                'Remove group layer as it will be placed in new sort order
            '                ArcMap.RemoveGroupLayer(My.ThisApplication, pSurveyGroupLayer.Name)

            '                'Add DEM in the order new order that is created by loop with OrderByDescending
            '                GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEM)

            '                'Add all associated surfaces to group layer that were previously in map
            '                If lAssocLayers.Count > 0 Then
            '                    For Each rAssoc As ProjectDS.AssociatedSurfaceRow In lAssocLayers
            '                        GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(rAssoc)
            '                    Next
            '                End If

            '                'Add all error surfaces to group layer that were previously in map
            '                If lErrorLayers.Count > 0 Then
            '                    For Each rError As ProjectDS.ErrorSurfaceRow In lErrorLayers
            '                        GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            '                    Next
            '                End If
            '            End If
            '        Next

            '    End If
            'Catch ex As Exception
            '    ExceptionHelper.HandleException(ex)
            'End Try

        End Sub

        Private Sub RefreshMenuStripImages(ByRef pParentMenuItem As ToolStripMenuItem)

            'Set other parent menu items images to nothing
            For Each pTempMenuItem As ToolStripMenuItem In pParentMenuItem.Owner.Items
                If Not String.Compare(pTempMenuItem.Text, pParentMenuItem.Text) = 0 Then
                    pTempMenuItem.Image = Nothing
                    If pTempMenuItem.HasDropDownItems() Then
                        For Each pItem As ToolStripMenuItem In pTempMenuItem.DropDownItems
                            pItem.Image = Nothing
                        Next
                    End If
                End If
            Next

        End Sub

        '#Region "Summary XML Node"

        '    Private Sub OpenInExcelToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

        '        Try
        '            Dim selNode As TreeNode = treProject.SelectedNode
        '            If TypeOf selNode Is TreeNode Then
        '                Dim eType As GCDNodeTypes = GetNodeType(selNode)
        '                If eType = GCDNodeTypes.SummaryXML Then
        '                    Dim nID As Integer = GetNodeID(selNode)
        '                    Dim rDod As ProjectDS.DoDsRow = GCD.GCDProject.ProjectManager.ds.DoDs.FindByDoDID(nID)
        '                    If TypeOf rDod Is ProjectDS.DoDsRow Then
        '                        If Not rDod.IsSummaryXMLPathNull Then
        '                            Dim sPath As String = rDod.SummaryXMLPath
        '                            If sPath.Contains(" ") Then
        '                                sPath = """" & sPath & """"
        '                            End If
        '                            Process.Start("notepad.exe", sPath)
        '                        End If
        '                    End If
        '                End If
        '            End If

        '        Catch ex As Exception
        '            ExceptionUI.HandleException(ex)
        '        End Try

        '    End Sub

        '#End Region

        Private Sub ExploreChangeDetectionFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExploreChangeDetectionFolderToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DoD Then
                        Dim nID As Integer = GetNodeID(selNode)
                        Dim rDod As ProjectDS.DoDsRow = GCDProject.ProjectManager.ds.DoDs.FindByDoDID(nID)
                        If TypeOf rDod Is ProjectDS.DoDsRow Then
                            Dim sFolder As String = GCDProject.ProjectManagerBase.OutputManager.GetDoDOutputFolder(rDod.Name)
                            If IO.Directory.Exists(sFolder) Then
                                If sFolder.Contains(" ") Then
                                    sFolder = """" & sFolder & """"
                                End If
                                Process.Start("explorer.exe", sFolder)
                            End If

                        End If
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub ExploreGCDProjectFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExploreGCDProjectFolderToolStripMenuItem.Click
            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.Project Then
                        Process.Start("explorer.exe", IO.Path.GetDirectoryName(GCDProject.ProjectManager.FilePath))
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

#Region "AOI Group Menu Items"

        Private Sub AddAllAOIsToTheMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllAOIsToTheMapToolStripMenuItem.Click

            Try
                Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManager.CurrentProject
                If TypeOf rProject Is ProjectDS.ProjectRow Then
                    For Each rAOI As ProjectDS.AOIsRow In rProject.GetAOIsRows
                        ' TODO 
                        Throw New Exception("not implemented")
                        '     GCDProject.ProjectManagerUI.ArcMapManager.AddAOI(rAOI)
                    Next
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddAOIToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        AddAOIToolStripMenuItem.Click,
        AddAOIToolStripMenuItem1.Click

            'Try
            '    Dim frm As New AOIPropertiesForm(My.ThisApplication)
            '    If frm.ShowDialog = DialogResult.OK Then
            '        Dim nID As Integer = frm.AOIID
            '        If nID > 0 Then
            '            Dim sTag As String = GCDNodeTypes.AOI.ToString & "_" & nID.ToString
            '            LoadTree(sTag)
            '        End If
            '    End If
            'Catch ex As Exception
            '    ExceptionUI.HandleException(ex)
            'End Try
        End Sub

#End Region

#Region "AOI Menu ITems"

        Private Sub EditAOIPropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditAOIPropertiesToolStripMenuItem.Click

            'Try
            '    Dim selNode As TreeNode = treProject.SelectedNode
            '    If TypeOf selNode Is TreeNode Then
            '        Dim eType As GCDNodeTypes = GetNodeType(selNode)
            '        If eType = GCDNodeTypes.AOI Then
            '            Dim nID As Integer = GetNodeID(selNode)
            '            If nID > 0 Then
            '                Dim frm As New AOIPropertiesForm(My.ThisApplication, nID)
            '                If frm.ShowDialog = DialogResult.OK Then
            '                    LoadTree(selNode.Tag)
            '                End If
            '            End If
            '        End If
            '    End If

            'Catch ex As Exception
            '    ExceptionUI.HandleException(ex)
            'End Try

        End Sub

        Private Sub AddToMapToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToMapToolStripMenuItem2.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AOI Then
                        Dim nID As Integer = GetNodeID(selNode)
                        For Each rAOI As ProjectDS.AOIsRow In GCDProject.ProjectManager.CurrentProject.GetAOIsRows
                            If rAOI.AOIID = nID Then
                                ' TODO 
                                Throw New Exception("not implemented")
                                '  GCDProject.ProjectManagerUI.ArcMapManager.AddAOI(rAOI)
                                Exit Sub
                            End If
                        Next
                    End If
                End If

            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

#End Region

        Private Sub AddChangeDetectionToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddChangeDetectionToolStripMenuItem1.Click

            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode

                If TypeOf nodSelected Is TreeNode Then
                    If GetNodeType(nodSelected) = GCDNodeTypes.ChangeDetectionDEMPair Then
                        Dim n1stUnderscore As Integer = nodSelected.Tag.ToString.IndexOf("_")
                        Dim n2ndUnderscore As Integer = nodSelected.Tag.ToString.LastIndexOf("_")

                        'Handles if the selected there are no dem in the selection tree because they have been deleted
                        If n1stUnderscore = -1 OrElse n2ndUnderscore = -1 Then
                            MsgBox("One or more of the selected DEM have been deleted and cannot be used in a change detection calculation.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                            Exit Sub
                        End If

                        Dim nNewDEMID, nOldDEMID As Integer

                        If Integer.TryParse(nodSelected.Tag.ToString.Substring(n1stUnderscore + 1, n2ndUnderscore - n1stUnderscore - 1), nNewDEMID) Then
                            If Integer.TryParse(nodSelected.Tag.ToString.Substring(n2ndUnderscore + 1, nodSelected.Tag.ToString.Length - n2ndUnderscore - 1), nOldDEMID) Then
                                Dim frmDoDCalculation As New ChangeDetection.frmDoDProperties(nNewDEMID, nOldDEMID)
                                DoChangeDetection(frmDoDCalculation)
                            End If
                        End If

                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub AddAllChangeDetectionsToTheMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllChangeDetectionsToTheMapToolStripMenuItem.Click

            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode
                If TypeOf nodSelected Is TreeNode Then
                    If GetNodeType(nodSelected) = GCDNodeTypes.ChangeDetectionDEMPair Then
                        Dim n1stUnderscore As Integer = nodSelected.Tag.ToString.IndexOf("_")
                        Dim n2ndUnderscore As Integer = nodSelected.Tag.ToString.LastIndexOf("_")
                        Dim nNewDEMID, nOldDEMID As Integer
                        If Integer.TryParse(nodSelected.Tag.ToString.Substring(n1stUnderscore + 1, n2ndUnderscore - n1stUnderscore - 1), nNewDEMID) Then
                            If Integer.TryParse(nodSelected.Tag.ToString.Substring(n2ndUnderscore + 1, nodSelected.Tag.ToString.Length - n2ndUnderscore - 1), nOldDEMID) Then
                                Dim sNewDEMName As String = String.Empty
                                Dim sOldDEMName As String = String.Empty
                                For Each aDEMRow As ProjectDS.DEMSurveyRow In GCDProject.ProjectManagerBase.ds.DEMSurvey
                                    If aDEMRow.DEMSurveyID = nNewDEMID Then
                                        sNewDEMName = aDEMRow.Name
                                    ElseIf aDEMRow.DEMSurveyID = nOldDEMID Then
                                        sOldDEMName = aDEMRow.Name
                                    End If
                                Next

                                If Not String.IsNullOrEmpty(sNewDEMName) Then
                                    If Not String.IsNullOrEmpty(sOldDEMName) Then
                                        For Each aDoDRow As ProjectDS.DoDsRow In GCDProject.ProjectManager.ds.DoDs
                                            If String.Compare(aDoDRow.NewSurveyName, sNewDEMName, True) = 0 Then
                                                If String.Compare(aDoDRow.OldSurveyName, sOldDEMName, True) = 0 Then
                                                    ' TODO 
                                                    Throw New Exception("not implemented")
                                                    '  GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(aDoDRow)
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Try
                Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer")
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub AddAllDEMSurveysToTheMapToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllDEMSurveysToTheMapToolStripMenuItem1.Click

            Try

                Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManager.CurrentProject

                'TODO: Insert the GetSortedSurveyRowsMethod
                'Store DEM Survey Rows in an Ienumerable then loop over
                Dim sortedSurveys As System.Linq.IOrderedEnumerable(Of ProjectDS.DEMSurveyRow) = GetSortedSurveyRows(rProject, m_eSortBy)

                If TypeOf rProject Is ProjectDS.ProjectRow Then
                    For Each rDEM As ProjectDS.DEMSurveyRow In sortedSurveys.Reverse()
                        'For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows
                        ' TODO 
                        Throw New Exception("not implemented")
                        ' GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(rDEM)
                    Next
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try


        End Sub

        Private Sub AddDEMSurveyToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddDEMSurveyToolStripMenuItem1.Click
            Try
                AddDEMSurvey()
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub BudgetSegregationPropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        BudgetSegregationPropertiesToolStripMenuItem.Click ', _
            'AddBudgetSegregationToolStripMenuItem2.Click

            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode
                If TypeOf nodSelected Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
                    Dim nID As Integer = GetNodeID(nodSelected)
                    If eType = GCDNodeTypes.BudgetSegregation AndAlso nID > 0 Then
                        Dim frm As New BudgetSegregation.frmBudgetSegResults(nID)
                        frm.ShowDialog()
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub BrowseBudgetSegregationFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseBudgetSegregationFolderToolStripMenuItem.Click
            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode
                If TypeOf nodSelected Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
                    Dim nID As Integer = GetNodeID(nodSelected)

                    For Each rBS As ProjectDS.BudgetSegregationsRow In GCDProject.ProjectManager.ds.BudgetSegregations
                        If rBS.BudgetID = nID Then
                            Dim sPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rBS.OutputFolder)
                            If IO.Directory.Exists(sPath) Then
                                Process.Start("explorer.exe", sPath)
                                Exit For
                            Else
                                Dim ex As New Exception("The budget segregation folder path does not exist.")
                                ex.Data("BS Folder Path") = sPath
                                Throw ex
                            End If
                        End If
                    Next

                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode
                If TypeOf nodSelected Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
                    Dim nID As Integer = GetNodeID(nodSelected)
                    Dim frm As Form = Nothing

                    Select Case eType
                        Case GCDNodeTypes.InputsGroup, GCDNodeTypes.DEMSurvey
                            AddDEMSurvey()
                        'frm = New frmSurveyProperties(My.ThisApplication, 0)

                        Case GCDNodeTypes.SurveysGroup
                            AddDEMSurvey()

                        Case GCDNodeTypes.AssociatedSurfaceGroup
                            nID = GetNodeID(nodSelected.Parent)
                            frm = New frmAssocSurfaceProperties(nID, 0)

                        Case GCDNodeTypes.AssociatedSurface
                            nID = GetNodeID(nodSelected.Parent.Parent)
                            frm = New frmAssocSurfaceProperties(nID, 0)

                        Case GCDNodeTypes.ErrorSurfaceGroup
                            nID = GetNodeID(nodSelected.Parent)
                            For Each rDEM As ProjectDS.DEMSurveyRow In GCDProject.ProjectManagerBase.CurrentProject.GetDEMSurveyRows
                                If nID = rDEM.DEMSurveyID Then
                                    frm = New ErrorCalculation.frmErrorCalculation(rDEM)
                                    Exit For
                                End If
                            Next

                        Case GCDNodeTypes.ErrorSurface
                            nID = GetNodeID(nodSelected.Parent.Parent)
                            For Each rDEM As ProjectDS.DEMSurveyRow In GCDProject.ProjectManagerBase.CurrentProject.GetDEMSurveyRows
                                If nID = rDEM.DEMSurveyID Then
                                    frm = New ErrorCalculation.frmErrorCalculation(rDEM)
                                    Exit For
                                End If
                            Next

                        Case GCDNodeTypes.AOIGroup, GCDNodeTypes.AOI
                            'frm = New AOIPropertiesForm(My.ThisApplication)

                        Case GCDNodeTypes.BudgetSegregationGroup, GCDNodeTypes.BudgetSegregation
                            nID = GetNodeID(nodSelected.Parent.Parent)
                            For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManagerBase.ds.DoDs
                                If rDoD.DoDID = nID Then
                                    frm = New BudgetSegregation.frmBudgetSegProperties(rDoD)
                                    Exit For
                                End If
                            Next

                        Case GCDNodeTypes.DoD, GCDNodeTypes.ChangeDetectionGroup, GCDNodeTypes.ChangeDetectionDEMPair, GCDNodeTypes.AnalysesGroup
                            frm = New ChangeDetection.frmDoDProperties()

                    End Select

                    If TypeOf frm Is Form Then
                        If frm.ShowDialog = DialogResult.OK Then
                            LoadTree(nodSelected.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub AddBudgetSegregationToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles _
            AddBudgetSegregationToolStripMenuItem1.Click,
            AddBudgetSegregationToolStripMenuItem2.Click,
            AddBudgetSegregationToolStripMenuItem.Click

            Try
                Dim nodSelected As TreeNode = treProject.SelectedNode
                If TypeOf nodSelected Is TreeNode Then
                    Dim nID As Integer = GetNodeID(nodSelected)
                    Dim eType As GCDNodeTypes = GetNodeType(nodSelected)
                    Dim nodDoD As TreeNode = Nothing
                    Select Case eType
                        Case GCDNodeTypes.DoD
                            nodDoD = nodSelected

                        Case GCDNodeTypes.BudgetSegregationGroup
                            nodDoD = nodSelected.Parent

                        Case GCDNodeTypes.BudgetSegregationMask
                            nodDoD = nodSelected.Parent.Parent

                        Case GCDNodeTypes.BudgetSegregation
                            nodDoD = nodSelected.Parent.Parent.Parent
                        Case Else
                            Throw New Exception("Unhandled Node Type")
                    End Select
                    Dim nDoDID As Integer = GetNodeID(nodDoD)

                    For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManagerBase.ds.DoDs
                        If rDoD.DoDID = nDoDID Then
                            Dim frm As New BudgetSegregation.frmBudgetSegProperties(rDoD)
                            If frm.ShowDialog() = DialogResult.OK Then
                                LoadTree(frm.BSID)
                                Dim frmResults As New BudgetSegregation.frmBudgetSegResults(frm.BSID)
                                frmResults.ShowDialog()
                            End If
                            Exit For
                        End If
                    Next
                End If
            Catch ex As Exception
                ExceptionHelper.HandleException(ex)
            End Try
        End Sub

        Private Sub treProject_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles treProject.AfterSelect
            RaiseEvent ProjectTreeNodeSelectionChange(sender, e)
        End Sub
    End Class

End Namespace