Imports System.Windows.Forms
Imports System.Drawing
Imports GCDUserInterface.SurveyLibrary

Namespace Project

    Public Class ucProjectExplorer

        Public Event ProjectTreeNodeSelectionChange(sender As Object, e As EventArgs)

        Private Const m_sGroupInputs As String = "Inputs"
        Private Const m_sAssocSurfaces As String = "Associated Surfaces"
        Private Const m_sErrorSurfaces As String = "Error Surfaces"
        Private Const m_sBudgetSegs As String = "Budget Segregations"
        Private Shared m_eSortBy As SortSurveyBy = SortSurveyBy.SurveyDateDsc

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
        End Enum

        Private Sub ProjectExplorerUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            LoadTree()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSelectedNodeTag">If provided, the code will make this the selected node</param>
        ''' <remarks>Grouping nodes are added to the tree with the enumeration above as their key. i.e. Project node has key "1".
        ''' Items that have database IDs are added with the key as type_id. So DEM Survey with ID 4 would have key "3_4"</remarks>
        Private Sub LoadTree(Optional sSelectedNodeTag As String = "", Optional eSortSurveyBy As SortSurveyBy = SortSurveyBy.SurveyDateDsc)

            treProject.Nodes.Clear()

            If TypeOf ProjectManager.Project Is GCDProject Then
                LoadTree(treProject, ProjectManager.Project, False, sSelectedNodeTag, eSortSurveyBy)
            End If

        End Sub

        Public Shared Sub LoadTree(ByRef tre As TreeView,
                                   rProject As GCDProject,
                                   bCheckboxes As Boolean,
                                   Optional sSelectedNodeTag As String = "",
                                   Optional eSortSurveyBy As SortSurveyBy = SortSurveyBy.SurveyDateDsc)

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

                Dim orderedSurveys As New List(Of DEMSurvey)
                Select Case eSortSurveyBy
                    Case SortSurveyBy.NameAsc : orderedSurveys = rProject.DEMsSortByName(True)
                    Case SortSurveyBy.NameDsc : orderedSurveys = rProject.DEMsSortByName(False)
                    Case SortSurveyBy.SurveyDateAsc : orderedSurveys = rProject.DEMsSortByDate(True)
                    Case SortSurveyBy.SurveyDateDsc : orderedSurveys = rProject.DEMsSortByName(False)
                End Select

                For Each dem As DEMSurvey In orderedSurveys

                    Dim nodSurvey As TreeNode = nodSurveysGroup.Nodes.Add(GCDNodeTypes.DEMSurvey & "_" & dem.Name, dem.Name, GCDNodeTypes.DEMSurvey)
                    nodSurvey.Tag = New ProjectTreeNode(GCDNodeTypes.DEMSurvey, dem)
                    nodSurvey.SelectedImageIndex = nodSurvey.ImageIndex
                    If nodSurvey.Tag = sSelectedNodeTag Then tre.SelectedNode = nodSurvey
                    Dim bExpandSurveyNode As Boolean = False

                    ' Associated surfaces
                    Dim nodAssocGroup As TreeNode = nodSurvey.Nodes.Add(GCDNodeTypes.AssociatedSurfaceGroup.ToString, m_sAssocSurfaces, GCDNodeTypes.AssociatedSurfaceGroup)
                    nodAssocGroup.Tag = GCDNodeTypes.AssociatedSurfaceGroup.ToString
                    nodAssocGroup.SelectedImageIndex = nodAssocGroup.ImageIndex
                    If nodAssocGroup.Tag = sSelectedNodeTag Then tre.SelectedNode = nodAssocGroup

                    For Each assoc As AssocSurface In dem.AssocSurfaces.Values
                        Dim nodAssoc As TreeNode = nodAssocGroup.Nodes.Add(GCDNodeTypes.AssociatedSurface.ToString & "_" & assoc.Name, assoc.Name, GCDNodeTypes.AssociatedSurface)
                        nodAssoc.Tag = New ProjectTreeNode(GCDNodeTypes.AssociatedSurface, assoc)
                        nodAssoc.SelectedImageIndex = nodAssoc.ImageIndex
                        If nodAssoc.Tag = sSelectedNodeTag Then tre.SelectedNode = nodAssoc
                        bExpandSurveyNode = True
                    Next

                    ' Error surfaces
                    Dim nodErrorGroup As TreeNode = nodSurvey.Nodes.Add(GCDNodeTypes.ErrorSurfaceGroup.ToString, m_sErrorSurfaces, GCDNodeTypes.ErrorSurfaceGroup)
                    nodErrorGroup.Tag = GCDNodeTypes.ErrorSurfaceGroup.ToString
                    nodErrorGroup.SelectedImageIndex = nodErrorGroup.ImageIndex
                    If nodErrorGroup.Tag = sSelectedNodeTag Then tre.SelectedNode = nodErrorGroup

                    For Each errSurf As ErrorSurface In dem.ErrorSurfaces.Values
                        Dim nodError As TreeNode = nodErrorGroup.Nodes.Add(GCDNodeTypes.ErrorSurface.ToString & "_" & errSurf.Name, errSurf.Name, GCDNodeTypes.ErrorSurface)
                        nodError.Tag = New ProjectTreeNode(GCDNodeTypes.ErrorSurface, errSurf)
                        nodError.SelectedImageIndex = nodError.ImageIndex
                        If nodError.Tag = sSelectedNodeTag Then tre.SelectedNode = nodError
                        bExpandSurveyNode = True
                    Next

                    If bExpandSurveyNode Then nodSurvey.Expand()
                Next

                nodInputs.Expand()
                nodSurveysGroup.Expand()

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
                For Each rDoD As DoDBase In rProject.DoDs.Values
                    CDNode.Expand()

                    Dim sDEMPair As String = rDoD.NewDEM.Name & " - " & rDoD.OldDEM.Name
                    Dim theParent As TreeNode = Nothing
                    If dDoD.ContainsKey(sDEMPair) Then
                        ' This pair of DEMs already exists in the tree
                        theParent = dDoD(sDEMPair)
                    Else
                        ' Create a new parent of DEM surveys for this DoD
                        theParent = CDNode.Nodes.Add(sDEMPair, sDEMPair, GCDNodeTypes.ChangeDetectionDEMPair)
                        theParent.SelectedImageIndex = theParent.ImageIndex
                        theParent.Tag = GCDNodeTypes.ChangeDetectionDEMPair.ToString
                        theParent.Tag &= "_" & rDoD.NewDEM.Name & "_" & rDoD.OldDEM.Name

                        dDoD.Add(sDEMPair, theParent)
                    End If

                    ' Now create the actual DoD node under the node for the pair of DEMs
                    Dim nodDoD As TreeNode = theParent.Nodes.Add(rDoD.Name, rDoD.Name, GCDNodeTypes.DoD)
                    nodDoD.SelectedImageIndex = nodDoD.ImageIndex
                    nodDoD.Tag = New ProjectTreeNode(GCDNodeTypes.DoD, rDoD)
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
                    For Each rBS As BudgetSegregation In rDoD.BudgetSegregations.Values
                        nodDoD.Expand()

                        ' Loop through and find all the unique polygon masks used
                        sMaskDict(rBS.PolygonMask.FullName) = IO.Path.GetFileNameWithoutExtension(rBS.PolygonMask.FullName)
                    Next

                    ' Now loop through all the BS and add them under the appropriate mask polygon node
                    For Each sPolygonMask As String In sMaskDict.Keys
                        Dim nodMask As TreeNode = Nothing

                        For Each rBS As BudgetSegregation In rDoD.BudgetSegregations.Values
                            If String.Compare(sPolygonMask, rBS.PolygonMask.FullName, True) = 0 Then

                                If Not TypeOf nodBSGroup Is TreeNode Then
                                    nodBSGroup = nodDoD.Nodes.Add(GCDNodeTypes.BudgetSegregationGroup.ToString, m_sBudgetSegs, GCDNodeTypes.BudgetSegregationGroup)
                                    nodBSGroup.Tag = GCDNodeTypes.BudgetSegregationGroup.ToString
                                    nodBSGroup.SelectedImageIndex = nodBSGroup.ImageIndex
                                    nodBSGroup.Expand()
                                End If

                                If nodMask Is Nothing Then
                                    Dim sTag As String = GCDNodeTypes.BudgetSegregationMask.ToString & "_bs_" & naru.os.File.RemoveDangerousCharacters(rBS.PolygonMask.FullName) & "_dod_" & rDoD.Name
                                    nodMask = nodBSGroup.Nodes.Add(sTag, sMaskDict(sPolygonMask), GCDNodeTypes.BudgetSegregationMask)
                                    nodMask.SelectedImageIndex = nodMask.ImageIndex
                                    nodMask.Tag = sTag
                                End If

                                ' Budget Segregation
                                Dim nodBS As TreeNode = nodMask.Nodes.Add(GCDNodeTypes.BudgetSegregation.ToString & "_" & rBS.Name.ToString, rBS.Name, GCDNodeTypes.BudgetSegregation)
                                nodBS.Tag = New ProjectTreeNode(GCDNodeTypes.BudgetSegregation, rBS)
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
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub treProject_DoubleClick(sender As Object, e As System.EventArgs) Handles treProject.DoubleClick

            If Not TypeOf treProject.SelectedNode Is TreeNode Then
                Return
            End If

            Try
                If TypeOf treProject.SelectedNode.Tag Is ProjectTreeNode Then
                    Dim tag As ProjectTreeNode = DirectCast(treProject.SelectedNode.Tag, ProjectTreeNode)
                    Dim frm As Form

                    Select Case tag.NodeType
                        Case GCDNodeTypes.DEMSurvey
                            frm = New frmDEMSurveyProperties(DirectCast(tag.Item, DEMSurvey))

                        Case GCDNodeTypes.AssociatedSurface
                            Dim assoc As AssocSurface = DirectCast(tag.Item, AssocSurface)
                            frm = New frmAssocSurfaceProperties(assoc.DEM, assoc)

                        Case GCDNodeTypes.ErrorSurface
                            'frm = New ErrorCalculation.frmErrorCalculation()

                    End Select

                    If TypeOf frm Is Form Then
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree()
                        End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                            If Not ProjectManager.IsArcMap Then
                                For Each item As ToolStripItem In cms.Items
                                    item.Visible = Not item.Text.ToLower().Contains("map")
                                Next
                            End If

                            cms.Show(treProject, New Point(e.X, e.Y))
                        End If
                    End If

                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Public Function AddDEMSurvey() As Integer

            Dim nDEMSurveyID As Integer = 0
            Dim gReferenceRaster As GCDConsoleLib.Raster = Nothing

            If ProjectManager.Project.DEMSurveys.Count > 0 Then
                gReferenceRaster = ProjectManager.Project.DEMSurveys.First().Value.Raster.Raster
            End If

            Dim frmImport As New SurveyLibrary.frmImportRaster(gReferenceRaster, Nothing, frmImportRaster.ImportRasterPurposes.DEMSurvey, "DEM Survey")
            If frmImport.ShowDialog = System.Windows.Forms.DialogResult.OK Then

                Dim gRaster As GCDConsoleLib.Raster = frmImport.ProcessRaster
                If TypeOf gRaster Is GCDConsoleLib.Raster Then

                    Dim dem As New DEMSurvey(frmImport.txtName.Text, Nothing, New IO.FileInfo(frmImport.txtRasterPath.Text))
                    ProjectManager.Project.DEMSurveys.Add(dem.Name, dem)
                    ProjectManager.Project.Save()

                    LoadTree(GCDNodeTypes.DEMSurvey.ToString & "_" & nDEMSurveyID.ToString)

                    Throw New NotImplementedException("Add newly created DEM to map")

                    Dim frm As New frmDEMSurveyProperties(dem)
                    frm.ShowDialog()

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
                        Dim dem As DEMSurvey = DirectCast(DirectCast(selNode.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                        Dim frm As New frmAssocSurfaceProperties(dem, Nothing)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(selNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub AddAllAssociatedSurfacesToTheMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AssociatedSurfaceGroup Then
                        Dim dem As DEMSurvey = DirectCast(DirectCast(selNode.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                        For Each assoc As AssocSurface In dem.AssocSurfaces.Values
                            Throw New Exception("add all associated surfaes to map ")
                        Next
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                        Dim assoc As AssocSurface = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, AssocSurface)
                        Dim frm As New frmAssocSurfaceProperties(assoc.DEM, assoc)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(selNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub AddToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem1.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.AssociatedSurface Then
                        Dim assoc As AssocSurface = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, AssocSurface)
                        Throw New Exception("Add associated surface to map not implemented")
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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

                    Dim nodDEM As TreeNode = selNode
                    If eType = GCDNodeTypes.ErrorSurfaceGroup Then
                        nodDEM = selNode.Parent
                    End If

                    Dim dem As DEMSurvey = DirectCast(DirectCast(nodDEM.Tag, ProjectTreeNode).Item, DEMSurvey)
                    Throw New NotImplementedException("derive error surface not implemeneted")
                    'Dim frm As New ErrorCalculation.frmErrorCalculation(dem)
                    'If frm.ShowDialog() = DialogResult.OK Then
                    '        LoadTree(selNode.Tag)
                    '    End If
                    'End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub SpecifyErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles _
        AddErrorSurfaceToolStripMenuItem1.Click,
            AddErrorSurfaceToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim nodDEM As TreeNode = selNode
                    If GetNodeType(selNode) = GCDNodeTypes.ErrorSurfaceGroup Then
                        nodDEM = selNode.Parent
                    End If

                    Dim dem As DEMSurvey = DirectCast(DirectCast(nodDEM.Tag, ProjectTreeNode).Item, DEMSurvey)
                    Dim errSurface As ErrorSurface = frmDEMSurveyProperties.SpecifyErrorSurface(dem)
                    If TypeOf errSurface Is ErrorSurface Then
                        Throw New Exception("add error surface to map not implemented")
                        LoadTree()
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub AddErrorSurfaceToMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ErrorSurfaceGroup Then
                        Dim dem As DEMSurvey = DirectCast(DirectCast(selNode.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                        For Each errSurf As ErrorSurface In dem.ErrorSurfaces.Values
                            Throw New Exception("Add error surface to map not implemented")
                        Next
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                        Dim errSurf As ErrorSurface = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, ErrorSurface)
                        Throw New NotImplementedException("Editing error surface not implemented")
                        'Dim frm As New ErrorCalculation.frmErrorCalculation(errSurf)
                        'If frm.ShowDialog() = DialogResult.OK Then
                        '    LoadTree(selNode.Tag)
                        'End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub AddErrorSurfaceToMapToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToMapToolStripMenuItem1.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ErrorSurface Then
                        Dim errSurf As ErrorSurface = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, ErrorSurface)
                        Throw New Exception("Adding error surface to map not implemented")
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                naru.error.ExceptionUI.HandleException(ex)
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
            '    naru.error.ExceptionUI.HandleException(ex)
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
                naru.error.ExceptionUI.HandleException(ex)
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
                        Dim dem As DEMSurvey = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DEMSurvey)
                        Dim frm As New frmDEMSurveyProperties(dem)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(selNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub AddToMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddToMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DEMSurvey Then
                        Dim dem As DEMSurvey = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DEMSurvey)
                        Throw New Exception("Adding DEM to map not implemented")
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub AddAssociatedSurfaceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAssociatedSurfaceToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DEMSurvey Then
                        Dim dem As DEMSurvey = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DEMSurvey)
                        Dim frm As New frmAssocSurfaceProperties(dem, Nothing)
                        If frm.ShowDialog() = DialogResult.OK Then
                            LoadTree(selNode.Tag)
                        End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        'Private Sub AddErrorSurfaceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddErrorSurfaceToolStripMenuItem.Click

        '    Try
        '        Dim selNode As TreeNode = treProject.SelectedNode
        '        If TypeOf selNode Is TreeNode Then
        '            Dim eType As GCDNodeTypes = GetNodeType(selNode)
        '            If eType = GCDNodeTypes.DEMSurvey Then
        '                Dim nID As Integer = GetNodeID(selNode)
        '                Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nID)
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
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub DoChangeDetection(ByRef frmDoDCalculation As ChangeDetection.frmDoDProperties)

            Try
                If frmDoDCalculation.ShowDialog() = DialogResult.OK Then
                    Dim sTag As String = String.Empty
                    If TypeOf frmDoDCalculation.DoD Is GCDCore.Project.DoDBase Then
                        LoadTree()

                        ' Now show the results form for this new DoD Calculation
                        Dim frmResults As New ChangeDetection.frmDoDResults(frmDoDCalculation.DoD)
                        frmResults.ShowDialog()
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.ChangeDetectionGroup Then
                        For Each rDoD As DoDBase In ProjectManager.Project.DoDs.Values
                            Throw New Exception("add all dods to the map not implemented")
                        Next
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                        Dim dod As DoDBase = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DoDBase)
                        Throw New Exception("Add thresholded DoD raster to map not implemented")
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub AddRawChangeDetectionToTheMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AddRawChangeDetectionToTheMapToolStripMenuItem.Click
            Try

                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DoD Then
                        Dim dod As DoDBase = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DoDBase)
                        Throw New Exception("Add raw DoD raster to map not implemented")
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub ViewChangeDetectionResultsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewChangeDetectionResultsToolStripMenuItem.Click

            Try
                Dim selNode As TreeNode = treProject.SelectedNode
                If TypeOf selNode Is TreeNode Then
                    Dim eType As GCDNodeTypes = GetNodeType(selNode)
                    If eType = GCDNodeTypes.DoD Then
                        Dim dod As DoDBase = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DoDBase)
                        Dim frm As New ChangeDetection.frmDoDResults(dod)
                        frm.ShowDialog()
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

#End Region

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

            Try
                Throw New NotImplementedException("Delete click")
                LoadTree()
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

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
                        Dim dem As DEMSurvey = DirectCast(DirectCast(nodSelected.Tag, ProjectTreeNode).Item, DEMSurvey)
                        frm = New frmDEMSurveyProperties(dem)

                    Case GCDNodeTypes.AssociatedSurface
                        Dim assoc As AssocSurface = DirectCast(DirectCast(nodSelected.Tag, ProjectTreeNode).Item, AssocSurface)
                        frm = New frmAssocSurfaceProperties(assoc.DEM, assoc)

                    Case GCDNodeTypes.ErrorSurface
                        Throw New NotImplementedException("error surface properties")
                        '    Dim nDEMSurveyID As Integer = GetNodeID(nodSelected.Parent.Parent)
                        '    Dim rDEMSurvey As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(nDEMSurveyID)
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
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

#End Region

        Public Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            'Dim sSortBy As String = sender.ToString

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
            '    Dim rProject As ProjectDS.ProjectRow = GCDProject.ProjectManagerBase.CurrentProject
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
            '    naru.error.ExceptionUI.HandleException(ex)
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
        '                    Dim rDod As ProjectDS.DoDsRow = GCD.GCDProject.ProjectManagerBase.ds.DoDs.FindByDoDID(nID)
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
                        Dim dod As DoDBase = DirectCast(DirectCast(selNode.Tag, ProjectTreeNode).Item, DoDBase)
                        If dod.Folder.Exists Then
                            Process.Start("explorer.exe", dod.Folder.FullName)
                        End If
                    End If
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub ExploreGCDProjectFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExploreGCDProjectFolderToolStripMenuItem.Click
            Try
                Process.Start("explorer.exe", IO.Path.GetDirectoryName(ProjectManager.Project.ProjectFile.FullName))
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub AddChangeDetectionToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddChangeDetectionToolStripMenuItem1.Click

            Try
                Throw New NotImplementedException("add dod with same DEMs not implemented")
                'Dim nodSelected As TreeNode = treProject.SelectedNode

                'If TypeOf nodSelected Is TreeNode Then
                '    If GetNodeType(nodSelected) = GCDNodeTypes.ChangeDetectionDEMPair Then
                '        Dim n1stUnderscore As Integer = nodSelected.Tag.ToString.IndexOf("_")
                '        Dim n2ndUnderscore As Integer = nodSelected.Tag.ToString.LastIndexOf("_")

                '        'Handles if the selected there are no dem in the selection tree because they have been deleted
                '        If n1stUnderscore = -1 OrElse n2ndUnderscore = -1 Then
                '            MsgBox("One or more of the selected DEM have been deleted and cannot be used in a change detection calculation.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                '            Exit Sub
                '        End If

                '        Dim nNewDEMID, nOldDEMID As Integer

                '        If Integer.TryParse(nodSelected.Tag.ToString.Substring(n1stUnderscore + 1, n2ndUnderscore - n1stUnderscore - 1), nNewDEMID) Then
                '            If Integer.TryParse(nodSelected.Tag.ToString.Substring(n2ndUnderscore + 1, nodSelected.Tag.ToString.Length - n2ndUnderscore - 1), nOldDEMID) Then
                '                Dim frmDoDCalculation As New ChangeDetection.frmDoDProperties(nNewDEMID, nOldDEMID)
                '                DoChangeDetection(frmDoDCalculation)
                '            End If
                '        End If

                '    End If
                'End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub AddAllChangeDetectionsToTheMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllChangeDetectionsToTheMapToolStripMenuItem.Click

            Try
                Throw New NotImplementedException("Add all DoDs to the map")

                'Dim nodSelected As TreeNode = treProject.SelectedNode
                'If TypeOf nodSelected Is TreeNode Then
                '    If GetNodeType(nodSelected) = GCDNodeTypes.ChangeDetectionDEMPair Then
                '        Dim n1stUnderscore As Integer = nodSelected.Tag.ToString.IndexOf("_")
                '        Dim n2ndUnderscore As Integer = nodSelected.Tag.ToString.LastIndexOf("_")
                '        Dim nNewDEMID, nOldDEMID As Integer
                '        If Integer.TryParse(nodSelected.Tag.ToString.Substring(n1stUnderscore + 1, n2ndUnderscore - n1stUnderscore - 1), nNewDEMID) Then
                '            If Integer.TryParse(nodSelected.Tag.ToString.Substring(n2ndUnderscore + 1, nodSelected.Tag.ToString.Length - n2ndUnderscore - 1), nOldDEMID) Then
                '                Dim sNewDEMName As String = String.Empty
                '                Dim sOldDEMName As String = String.Empty
                '                For Each aDEMRow As ProjectDS.DEMSurveyRow In ProjectManager.ds.DEMSurvey
                '                    If aDEMRow.DEMSurveyID = nNewDEMID Then
                '                        sNewDEMName = aDEMRow.Name
                '                    ElseIf aDEMRow.DEMSurveyID = nOldDEMID Then
                '                        sOldDEMName = aDEMRow.Name
                '                    End If
                '                Next

                '                If Not String.IsNullOrEmpty(sNewDEMName) Then
                '                    If Not String.IsNullOrEmpty(sOldDEMName) Then
                '                        For Each aDoDRow As ProjectDS.DoDsRow In ProjectManager.ds.DoDs
                '                            If String.Compare(aDoDRow.NewSurveyName, sNewDEMName, True) = 0 Then
                '                                If String.Compare(aDoDRow.OldSurveyName, sOldDEMName, True) = 0 Then
                '                                    ' TODO 
                '                                    Throw New Exception("not implemented")
                '                                    '  GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(aDoDRow)
                '                                End If
                '                            End If
                '                        Next
                '                    End If
                '                End If
                '            End If
                '        End If
                '    End If
                'End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Try
                Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer")
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub AddAllDEMSurveysToTheMapToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAllDEMSurveysToTheMapToolStripMenuItem1.Click

            For Each dem As DEMSurvey In ProjectManager.Project.DEMSurveys.Values
                Throw New Exception("Add all DEMs to the map")
            Next

        End Sub

        Private Sub AddDEMSurveyToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddDEMSurveyToolStripMenuItem1.Click
            Try
                AddDEMSurvey()
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                        'Dim frm As New BudgetSegregation.frmBudgetSegResults(nID)
                        'frm.ShowDialog()
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub BrowseBudgetSegregationFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseBudgetSegregationFolderToolStripMenuItem.Click
            Try
                If TypeOf treProject.SelectedNode Is TreeNode Then
                    If TypeOf treProject.SelectedNode.Tag Is ProjectTreeNode Then
                        Dim tag As ProjectTreeNode = DirectCast(treProject.SelectedNode.Tag, ProjectTreeNode)
                        If TypeOf tag.Item Is GCDCore.Project.BudgetSegregation Then
                            Process.Start("explorer.exe", DirectCast(tag.Item, GCDCore.Project.BudgetSegregation).Folder.FullName)
                        End If
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
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
                            Dim DEM As DEMSurvey = DirectCast(DirectCast(nodSelected.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                            frm = New frmAssocSurfaceProperties(DEM, Nothing)

                        Case GCDNodeTypes.AssociatedSurface
                            Dim DEM As DEMSurvey = DirectCast(DirectCast(nodSelected.Parent.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                            frm = New frmAssocSurfaceProperties(DEM, Nothing)

                        Case GCDNodeTypes.ErrorSurfaceGroup
                            Dim DEM As DEMSurvey = DirectCast(DirectCast(nodSelected.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                            'frm = New ErrorCalculation.frmErrorCalculation(rDEM)

                        Case GCDNodeTypes.ErrorSurface
                            Dim DEM As DEMSurvey = DirectCast(DirectCast(nodSelected.Parent.Parent.Tag, ProjectTreeNode).Item, DEMSurvey)
                            'frm = New ErrorCalculation.frmErrorCalculation(rDEM)

                        Case GCDNodeTypes.BudgetSegregationGroup, GCDNodeTypes.BudgetSegregation
                            Dim DoD As DoDBase = DirectCast(DirectCast(nodSelected.Parent.Parent.Tag, ProjectTreeNode).Item, DoDBase)
                            frm = New UI.BudgetSegregation.frmBudgetSegProperties(DoD)

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
                naru.error.ExceptionUI.HandleException(ex)
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

                    Dim treDod As ProjectTreeNode = DirectCast(nodDoD.Tag, ProjectTreeNode)

                    Dim frm As New UI.BudgetSegregation.frmBudgetSegProperties(DirectCast(treDod.Item, DoDBase))
                    If frm.ShowDialog() = DialogResult.OK Then
                        LoadTree()
                        Dim frmResults As New UI.BudgetSegregation.frmBudgetSegResults(frm.BudgetSeg)
                        frmResults.ShowDialog()
                    End If
                End If
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub treProject_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles treProject.AfterSelect
            RaiseEvent ProjectTreeNodeSelectionChange(sender, e)
        End Sub
    End Class

End Namespace