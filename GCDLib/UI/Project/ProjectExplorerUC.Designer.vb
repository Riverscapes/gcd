<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProjectExplorerUC
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProjectExplorerUC))
        Me.treProject = New System.Windows.Forms.TreeView()
        Me.imgTreeImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.btnAddToMap = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnProperties = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.cmsProject = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditGCDProjectPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExploreGCDProjectFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsDEMSurvey = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditDEMSurveyProperatieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddToMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteDEMSurveyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddAssociatedSurfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddErrorSurfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeriveErrorSurfaceToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsAssociatedSurface = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddToMapToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteAssociatedSurfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsAssociatedSurfaceGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddAssociatedSurfaceToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllAssociatedSurfacesToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsInputsGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddDEMSurveyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsErrorSurfacesGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddErrorSurfaceToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeriveErrorSurfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddErrorSurfaceToMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsErrorSurface = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditErrorSurfacePropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddErrorSurfaceToMapToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteErrorSurfaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmsChangeDetectionGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddChangeDetectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsChangeDetection = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewChangeDetectionResultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddChangeDetectionToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddRawChangeDetectionToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExploreChangeDetectionFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteChangeDetectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddBudgetSegregationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsSurveysGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddDEMSurveyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SortByToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NameAscendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NameDescendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SurveyDateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SurveyDateAscendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SurveyDateDescendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DateAddedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DateAddedAscendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DateAddedDescendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.cmsAOIGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddAOIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllAOIsToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsAOI = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddAOIToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditAOIPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddToMapToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteAOIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsDEMSurveyPair = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddChangeDetectionToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllChangeDetectionsToTheMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmsBSGroup = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddBudgetSegregationToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsBS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BudgetSegregationPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BrowseBudgetSegregationFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddBudgetSegregationToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteBudgetSegregationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsProject.SuspendLayout()
        Me.cmsDEMSurvey.SuspendLayout()
        Me.cmsAssociatedSurface.SuspendLayout()
        Me.cmsAssociatedSurfaceGroup.SuspendLayout()
        Me.cmsInputsGroup.SuspendLayout()
        Me.cmsErrorSurfacesGroup.SuspendLayout()
        Me.cmsErrorSurface.SuspendLayout()
        Me.cmsChangeDetectionGroup.SuspendLayout()
        Me.cmsChangeDetection.SuspendLayout()
        Me.cmsSurveysGroup.SuspendLayout()
        Me.cmsAOIGroup.SuspendLayout()
        Me.cmsAOI.SuspendLayout()
        Me.cmsDEMSurveyPair.SuspendLayout()
        Me.cmsBSGroup.SuspendLayout()
        Me.cmsBS.SuspendLayout()
        Me.SuspendLayout()
        '
        'treProject
        '
        Me.treProject.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treProject.ImageIndex = 0
        Me.treProject.ImageList = Me.imgTreeImageList
        Me.treProject.Location = New System.Drawing.Point(3, 32)
        Me.treProject.Name = "treProject"
        Me.treProject.SelectedImageIndex = 0
        Me.treProject.ShowNodeToolTips = True
        Me.treProject.ShowRootLines = False
        Me.treProject.Size = New System.Drawing.Size(690, 628)
        Me.treProject.TabIndex = 0
        '
        'imgTreeImageList
        '
        Me.imgTreeImageList.ImageStream = CType(resources.GetObject("imgTreeImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgTreeImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.imgTreeImageList.Images.SetKeyName(0, "Delta.png")
        Me.imgTreeImageList.Images.SetKeyName(1, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(2, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(3, "DEMSurveys.png")
        Me.imgTreeImageList.Images.SetKeyName(4, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(5, "AssociatedSurfaces.png")
        Me.imgTreeImageList.Images.SetKeyName(6, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(7, "sigma.png")
        Me.imgTreeImageList.Images.SetKeyName(8, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(9, "AOI.png")
        Me.imgTreeImageList.Images.SetKeyName(10, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(11, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(12, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(13, "About.png")
        Me.imgTreeImageList.Images.SetKeyName(14, "BrowseFolder.png")
        Me.imgTreeImageList.Images.SetKeyName(15, "ConcaveHull.png")
        Me.imgTreeImageList.Images.SetKeyName(16, "BudgetSeg.png")
        '
        'btnCopy
        '
        Me.btnCopy.Enabled = False
        Me.btnCopy.Image = Global.GCDAddIn.My.Resources.Resources.Copy
        Me.btnCopy.Location = New System.Drawing.Point(143, 3)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(29, 23)
        Me.btnCopy.TabIndex = 8
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'btnAddToMap
        '
        Me.btnAddToMap.Enabled = False
        Me.btnAddToMap.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.btnAddToMap.Location = New System.Drawing.Point(108, 3)
        Me.btnAddToMap.Name = "btnAddToMap"
        Me.btnAddToMap.Size = New System.Drawing.Size(29, 23)
        Me.btnAddToMap.TabIndex = 9
        Me.btnAddToMap.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Enabled = False
        Me.btnDelete.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.btnDelete.Location = New System.Drawing.Point(73, 3)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(29, 23)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnProperties
        '
        Me.btnProperties.Enabled = False
        Me.btnProperties.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.btnProperties.Location = New System.Drawing.Point(38, 3)
        Me.btnProperties.Name = "btnProperties"
        Me.btnProperties.Size = New System.Drawing.Size(29, 23)
        Me.btnProperties.TabIndex = 6
        Me.btnProperties.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.btnAdd.Location = New System.Drawing.Point(3, 3)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(29, 23)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'cmsProject
        '
        Me.cmsProject.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditGCDProjectPropertiesToolStripMenuItem, Me.ToolStripMenuItem2, Me.ExploreGCDProjectFolderToolStripMenuItem, Me.ToolStripSeparator2, Me.ToolStripMenuItem1})
        Me.cmsProject.Name = "cmsProject"
        Me.cmsProject.Size = New System.Drawing.Size(231, 98)
        '
        'EditGCDProjectPropertiesToolStripMenuItem
        '
        Me.EditGCDProjectPropertiesToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.EditGCDProjectPropertiesToolStripMenuItem.Name = "EditGCDProjectPropertiesToolStripMenuItem"
        Me.EditGCDProjectPropertiesToolStripMenuItem.Size = New System.Drawing.Size(230, 22)
        Me.EditGCDProjectPropertiesToolStripMenuItem.Text = "Edit GCD Project Properties"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(230, 22)
        Me.ToolStripMenuItem2.Text = "Add Entire Project to the Map"
        '
        'ExploreGCDProjectFolderToolStripMenuItem
        '
        Me.ExploreGCDProjectFolderToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.BrowseFolder
        Me.ExploreGCDProjectFolderToolStripMenuItem.Name = "ExploreGCDProjectFolderToolStripMenuItem"
        Me.ExploreGCDProjectFolderToolStripMenuItem.Size = New System.Drawing.Size(230, 22)
        Me.ExploreGCDProjectFolderToolStripMenuItem.Text = "Explore GCD Project Folder"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(230, 22)
        Me.ToolStripMenuItem1.Text = "Add DEM Survey"
        '
        'cmsDEMSurvey
        '
        Me.cmsDEMSurvey.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditDEMSurveyProperatieToolStripMenuItem, Me.AddToMapToolStripMenuItem, Me.DeleteDEMSurveyToolStripMenuItem, Me.ToolStripSeparator1, Me.AddAssociatedSurfaceToolStripMenuItem, Me.AddErrorSurfaceToolStripMenuItem, Me.DeriveErrorSurfaceToolStripMenuItem1})
        Me.cmsDEMSurvey.Name = "cmsDEMSurvey"
        Me.cmsDEMSurvey.Size = New System.Drawing.Size(217, 142)
        '
        'EditDEMSurveyProperatieToolStripMenuItem
        '
        Me.EditDEMSurveyProperatieToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.EditDEMSurveyProperatieToolStripMenuItem.Name = "EditDEMSurveyProperatieToolStripMenuItem"
        Me.EditDEMSurveyProperatieToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.EditDEMSurveyProperatieToolStripMenuItem.Text = "Edit DEM Survey Properties"
        '
        'AddToMapToolStripMenuItem
        '
        Me.AddToMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddToMapToolStripMenuItem.Name = "AddToMapToolStripMenuItem"
        Me.AddToMapToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.AddToMapToolStripMenuItem.Text = "Add to Map"
        '
        'DeleteDEMSurveyToolStripMenuItem
        '
        Me.DeleteDEMSurveyToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.DeleteDEMSurveyToolStripMenuItem.Name = "DeleteDEMSurveyToolStripMenuItem"
        Me.DeleteDEMSurveyToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.DeleteDEMSurveyToolStripMenuItem.Text = "Delete DEM Survey"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(213, 6)
        '
        'AddAssociatedSurfaceToolStripMenuItem
        '
        Me.AddAssociatedSurfaceToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddAssociatedSurfaceToolStripMenuItem.Name = "AddAssociatedSurfaceToolStripMenuItem"
        Me.AddAssociatedSurfaceToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.AddAssociatedSurfaceToolStripMenuItem.Text = "Add Associated Surface"
        '
        'AddErrorSurfaceToolStripMenuItem
        '
        Me.AddErrorSurfaceToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddErrorSurfaceToolStripMenuItem.Name = "AddErrorSurfaceToolStripMenuItem"
        Me.AddErrorSurfaceToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.AddErrorSurfaceToolStripMenuItem.Text = "Specify Error Surface"
        '
        'DeriveErrorSurfaceToolStripMenuItem1
        '
        Me.DeriveErrorSurfaceToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.sigma
        Me.DeriveErrorSurfaceToolStripMenuItem1.Name = "DeriveErrorSurfaceToolStripMenuItem1"
        Me.DeriveErrorSurfaceToolStripMenuItem1.Size = New System.Drawing.Size(216, 22)
        Me.DeriveErrorSurfaceToolStripMenuItem1.Text = "Derive Error Surface"
        '
        'cmsAssociatedSurface
        '
        Me.cmsAssociatedSurface.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditPropertiesToolStripMenuItem, Me.AddToMapToolStripMenuItem1, Me.DeleteAssociatedSurfaceToolStripMenuItem})
        Me.cmsAssociatedSurface.Name = "cmsAssociatedSurface"
        Me.cmsAssociatedSurface.Size = New System.Drawing.Size(253, 70)
        '
        'EditPropertiesToolStripMenuItem
        '
        Me.EditPropertiesToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.EditPropertiesToolStripMenuItem.Name = "EditPropertiesToolStripMenuItem"
        Me.EditPropertiesToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
        Me.EditPropertiesToolStripMenuItem.Text = "Edit Associated Surface Properties"
        '
        'AddToMapToolStripMenuItem1
        '
        Me.AddToMapToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddToMapToolStripMenuItem1.Name = "AddToMapToolStripMenuItem1"
        Me.AddToMapToolStripMenuItem1.Size = New System.Drawing.Size(252, 22)
        Me.AddToMapToolStripMenuItem1.Text = "Add Associated Surface to Map"
        '
        'DeleteAssociatedSurfaceToolStripMenuItem
        '
        Me.DeleteAssociatedSurfaceToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.DeleteAssociatedSurfaceToolStripMenuItem.Name = "DeleteAssociatedSurfaceToolStripMenuItem"
        Me.DeleteAssociatedSurfaceToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
        Me.DeleteAssociatedSurfaceToolStripMenuItem.Text = "Delete Associated Surface"
        '
        'cmsAssociatedSurfaceGroup
        '
        Me.cmsAssociatedSurfaceGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddAssociatedSurfaceToolStripMenuItem1, Me.AddAllAssociatedSurfacesToTheMapToolStripMenuItem})
        Me.cmsAssociatedSurfaceGroup.Name = "cmsAssociatedSurfaceGroup"
        Me.cmsAssociatedSurfaceGroup.Size = New System.Drawing.Size(282, 48)
        '
        'AddAssociatedSurfaceToolStripMenuItem1
        '
        Me.AddAssociatedSurfaceToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddAssociatedSurfaceToolStripMenuItem1.Name = "AddAssociatedSurfaceToolStripMenuItem1"
        Me.AddAssociatedSurfaceToolStripMenuItem1.Size = New System.Drawing.Size(281, 22)
        Me.AddAssociatedSurfaceToolStripMenuItem1.Text = "Add Associated Surface"
        '
        'AddAllAssociatedSurfacesToTheMapToolStripMenuItem
        '
        Me.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Name = "AddAllAssociatedSurfacesToTheMapToolStripMenuItem"
        Me.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Size = New System.Drawing.Size(281, 22)
        Me.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Text = "Add All Associated Surfaces to the Map"
        '
        'cmsInputsGroup
        '
        Me.cmsInputsGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDEMSurveyToolStripMenuItem, Me.AddAllDEMSurveysToTheMapToolStripMenuItem})
        Me.cmsInputsGroup.Name = "cmsInputsGroup"
        Me.cmsInputsGroup.Size = New System.Drawing.Size(297, 48)
        '
        'AddDEMSurveyToolStripMenuItem
        '
        Me.AddDEMSurveyToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddDEMSurveyToolStripMenuItem.Name = "AddDEMSurveyToolStripMenuItem"
        Me.AddDEMSurveyToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.AddDEMSurveyToolStripMenuItem.Text = "Add DEM Survey"
        '
        'AddAllDEMSurveysToTheMapToolStripMenuItem
        '
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem.Name = "AddAllDEMSurveysToTheMapToolStripMenuItem"
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem.Size = New System.Drawing.Size(296, 22)
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem.Text = "Add All DEM Surveys and AOIs to the Map"
        '
        'cmsErrorSurfacesGroup
        '
        Me.cmsErrorSurfacesGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddErrorSurfaceToolStripMenuItem1, Me.DeriveErrorSurfaceToolStripMenuItem, Me.AddErrorSurfaceToMapToolStripMenuItem})
        Me.cmsErrorSurfacesGroup.Name = "cmsErrorSurfacesGroup"
        Me.cmsErrorSurfacesGroup.Size = New System.Drawing.Size(250, 70)
        '
        'AddErrorSurfaceToolStripMenuItem1
        '
        Me.AddErrorSurfaceToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddErrorSurfaceToolStripMenuItem1.Name = "AddErrorSurfaceToolStripMenuItem1"
        Me.AddErrorSurfaceToolStripMenuItem1.Size = New System.Drawing.Size(249, 22)
        Me.AddErrorSurfaceToolStripMenuItem1.Text = "Specify Error Surface"
        '
        'DeriveErrorSurfaceToolStripMenuItem
        '
        Me.DeriveErrorSurfaceToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.sigma
        Me.DeriveErrorSurfaceToolStripMenuItem.Name = "DeriveErrorSurfaceToolStripMenuItem"
        Me.DeriveErrorSurfaceToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.DeriveErrorSurfaceToolStripMenuItem.Text = "Derive Error Surface"
        '
        'AddErrorSurfaceToMapToolStripMenuItem
        '
        Me.AddErrorSurfaceToMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddErrorSurfaceToMapToolStripMenuItem.Name = "AddErrorSurfaceToMapToolStripMenuItem"
        Me.AddErrorSurfaceToMapToolStripMenuItem.Size = New System.Drawing.Size(249, 22)
        Me.AddErrorSurfaceToMapToolStripMenuItem.Text = "Add All Error Surfaces to the Map"
        '
        'cmsErrorSurface
        '
        Me.cmsErrorSurface.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditErrorSurfacePropertiesToolStripMenuItem, Me.AddErrorSurfaceToMapToolStripMenuItem1, Me.DeleteErrorSurfaceToolStripMenuItem, Me.ToolStripSeparator3})
        Me.cmsErrorSurface.Name = "cmsErrorSurface"
        Me.cmsErrorSurface.Size = New System.Drawing.Size(221, 98)
        '
        'EditErrorSurfacePropertiesToolStripMenuItem
        '
        Me.EditErrorSurfacePropertiesToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.EditErrorSurfacePropertiesToolStripMenuItem.Name = "EditErrorSurfacePropertiesToolStripMenuItem"
        Me.EditErrorSurfacePropertiesToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.EditErrorSurfacePropertiesToolStripMenuItem.Text = "Edit Error Surface Properties"
        '
        'AddErrorSurfaceToMapToolStripMenuItem1
        '
        Me.AddErrorSurfaceToMapToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddErrorSurfaceToMapToolStripMenuItem1.Name = "AddErrorSurfaceToMapToolStripMenuItem1"
        Me.AddErrorSurfaceToMapToolStripMenuItem1.Size = New System.Drawing.Size(220, 22)
        Me.AddErrorSurfaceToMapToolStripMenuItem1.Text = "Add Error Surface to Map"
        '
        'DeleteErrorSurfaceToolStripMenuItem
        '
        Me.DeleteErrorSurfaceToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.DeleteErrorSurfaceToolStripMenuItem.Name = "DeleteErrorSurfaceToolStripMenuItem"
        Me.DeleteErrorSurfaceToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.DeleteErrorSurfaceToolStripMenuItem.Text = "Delete Error Surface"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(217, 6)
        '
        'cmsChangeDetectionGroup
        '
        Me.cmsChangeDetectionGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddChangeDetectionToolStripMenuItem, Me.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem})
        Me.cmsChangeDetectionGroup.Name = "cmsChangeDetectionGroup"
        Me.cmsChangeDetectionGroup.Size = New System.Drawing.Size(322, 48)
        '
        'AddChangeDetectionToolStripMenuItem
        '
        Me.AddChangeDetectionToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddChangeDetectionToolStripMenuItem.Name = "AddChangeDetectionToolStripMenuItem"
        Me.AddChangeDetectionToolStripMenuItem.Size = New System.Drawing.Size(321, 22)
        Me.AddChangeDetectionToolStripMenuItem.Text = "Add Change Detection"
        '
        'AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem
        '
        Me.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Name = "AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem"
        Me.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Size = New System.Drawing.Size(321, 22)
        Me.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Text = "Add All Change Detection Analyses to the Map"
        '
        'cmsChangeDetection
        '
        Me.cmsChangeDetection.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewChangeDetectionResultsToolStripMenuItem, Me.AddChangeDetectionToTheMapToolStripMenuItem, Me.AddRawChangeDetectionToTheMapToolStripMenuItem, Me.ExploreChangeDetectionFolderToolStripMenuItem, Me.DeleteChangeDetectionToolStripMenuItem, Me.ToolStripSeparator5, Me.AddBudgetSegregationToolStripMenuItem})
        Me.cmsChangeDetection.Name = "cmsChangeDetection"
        Me.cmsChangeDetection.Size = New System.Drawing.Size(325, 142)
        '
        'ViewChangeDetectionResultsToolStripMenuItem
        '
        Me.ViewChangeDetectionResultsToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.GCD
        Me.ViewChangeDetectionResultsToolStripMenuItem.Name = "ViewChangeDetectionResultsToolStripMenuItem"
        Me.ViewChangeDetectionResultsToolStripMenuItem.Size = New System.Drawing.Size(324, 22)
        Me.ViewChangeDetectionResultsToolStripMenuItem.Text = "View Change Detection Results"
        '
        'AddChangeDetectionToTheMapToolStripMenuItem
        '
        Me.AddChangeDetectionToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddChangeDetectionToTheMapToolStripMenuItem.Name = "AddChangeDetectionToTheMapToolStripMenuItem"
        Me.AddChangeDetectionToTheMapToolStripMenuItem.Size = New System.Drawing.Size(324, 22)
        Me.AddChangeDetectionToTheMapToolStripMenuItem.Text = "Add Thresholded Change Detection to the Map"
        '
        'AddRawChangeDetectionToTheMapToolStripMenuItem
        '
        Me.AddRawChangeDetectionToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddRawChangeDetectionToTheMapToolStripMenuItem.Name = "AddRawChangeDetectionToTheMapToolStripMenuItem"
        Me.AddRawChangeDetectionToTheMapToolStripMenuItem.Size = New System.Drawing.Size(324, 22)
        Me.AddRawChangeDetectionToTheMapToolStripMenuItem.Text = "Add Raw Change Detection to the Map"
        '
        'ExploreChangeDetectionFolderToolStripMenuItem
        '
        Me.ExploreChangeDetectionFolderToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.BrowseFolder
        Me.ExploreChangeDetectionFolderToolStripMenuItem.Name = "ExploreChangeDetectionFolderToolStripMenuItem"
        Me.ExploreChangeDetectionFolderToolStripMenuItem.Size = New System.Drawing.Size(324, 22)
        Me.ExploreChangeDetectionFolderToolStripMenuItem.Text = "Explore Change Detection Folder"
        '
        'DeleteChangeDetectionToolStripMenuItem
        '
        Me.DeleteChangeDetectionToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.DeleteChangeDetectionToolStripMenuItem.Name = "DeleteChangeDetectionToolStripMenuItem"
        Me.DeleteChangeDetectionToolStripMenuItem.Size = New System.Drawing.Size(324, 22)
        Me.DeleteChangeDetectionToolStripMenuItem.Text = "Delete Change Detection"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(321, 6)
        '
        'AddBudgetSegregationToolStripMenuItem
        '
        Me.AddBudgetSegregationToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddBudgetSegregationToolStripMenuItem.Name = "AddBudgetSegregationToolStripMenuItem"
        Me.AddBudgetSegregationToolStripMenuItem.Size = New System.Drawing.Size(324, 22)
        Me.AddBudgetSegregationToolStripMenuItem.Text = "Add Budget Segregation"
        '
        'cmsSurveysGroup
        '
        Me.cmsSurveysGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDEMSurveyToolStripMenuItem1, Me.AddAllDEMSurveysToTheMapToolStripMenuItem1, Me.SortByToolStripMenuItem})
        Me.cmsSurveysGroup.Name = "cmsSurveysGroup"
        Me.cmsSurveysGroup.Size = New System.Drawing.Size(246, 70)
        '
        'AddDEMSurveyToolStripMenuItem1
        '
        Me.AddDEMSurveyToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddDEMSurveyToolStripMenuItem1.Name = "AddDEMSurveyToolStripMenuItem1"
        Me.AddDEMSurveyToolStripMenuItem1.Size = New System.Drawing.Size(245, 22)
        Me.AddDEMSurveyToolStripMenuItem1.Text = "Add DEM Survey"
        '
        'AddAllDEMSurveysToTheMapToolStripMenuItem1
        '
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem1.Name = "AddAllDEMSurveysToTheMapToolStripMenuItem1"
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem1.Size = New System.Drawing.Size(245, 22)
        Me.AddAllDEMSurveysToTheMapToolStripMenuItem1.Text = "Add All DEM Surveys to the Map"
        '
        'SortByToolStripMenuItem
        '
        Me.SortByToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NameToolStripMenuItem, Me.SurveyDateToolStripMenuItem, Me.DateAddedToolStripMenuItem})
        Me.SortByToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.alphabetical
        Me.SortByToolStripMenuItem.Name = "SortByToolStripMenuItem"
        Me.SortByToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.SortByToolStripMenuItem.Text = "Sort by:"
        '
        'NameToolStripMenuItem
        '
        Me.NameToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NameAscendingToolStripMenuItem, Me.NameDescendingToolStripMenuItem})
        Me.NameToolStripMenuItem.Name = "NameToolStripMenuItem"
        Me.NameToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.NameToolStripMenuItem.Text = "Name"
        '
        'NameAscendingToolStripMenuItem
        '
        Me.NameAscendingToolStripMenuItem.Name = "NameAscendingToolStripMenuItem"
        Me.NameAscendingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NameAscendingToolStripMenuItem.Text = "Ascending"
        '
        'NameDescendingToolStripMenuItem
        '
        Me.NameDescendingToolStripMenuItem.Name = "NameDescendingToolStripMenuItem"
        Me.NameDescendingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NameDescendingToolStripMenuItem.Text = "Descending"
        '
        'SurveyDateToolStripMenuItem
        '
        Me.SurveyDateToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SurveyDateAscendingToolStripMenuItem, Me.SurveyDateDescendingToolStripMenuItem})
        Me.SurveyDateToolStripMenuItem.Name = "SurveyDateToolStripMenuItem"
        Me.SurveyDateToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.SurveyDateToolStripMenuItem.Text = "Survey date"
        '
        'SurveyDateAscendingToolStripMenuItem
        '
        Me.SurveyDateAscendingToolStripMenuItem.Name = "SurveyDateAscendingToolStripMenuItem"
        Me.SurveyDateAscendingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SurveyDateAscendingToolStripMenuItem.Text = "Ascending"
        '
        'SurveyDateDescendingToolStripMenuItem
        '
        Me.SurveyDateDescendingToolStripMenuItem.Name = "SurveyDateDescendingToolStripMenuItem"
        Me.SurveyDateDescendingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SurveyDateDescendingToolStripMenuItem.Text = "Descending"
        '
        'DateAddedToolStripMenuItem
        '
        Me.DateAddedToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DateAddedAscendingToolStripMenuItem, Me.DateAddedDescendingToolStripMenuItem})
        Me.DateAddedToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Check
        Me.DateAddedToolStripMenuItem.Name = "DateAddedToolStripMenuItem"
        Me.DateAddedToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.DateAddedToolStripMenuItem.Text = "Date added"
        '
        'DateAddedAscendingToolStripMenuItem
        '
        Me.DateAddedAscendingToolStripMenuItem.Name = "DateAddedAscendingToolStripMenuItem"
        Me.DateAddedAscendingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DateAddedAscendingToolStripMenuItem.Text = "Ascending"
        '
        'DateAddedDescendingToolStripMenuItem
        '
        Me.DateAddedDescendingToolStripMenuItem.Name = "DateAddedDescendingToolStripMenuItem"
        Me.DateAddedDescendingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DateAddedDescendingToolStripMenuItem.Text = "Descending"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Image = Global.GCDAddIn.My.Resources.Resources.refresh
        Me.cmdRefresh.Location = New System.Drawing.Point(213, 3)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(29, 23)
        Me.cmdRefresh.TabIndex = 10
        Me.cmdRefresh.UseVisualStyleBackColor = True
        Me.cmdRefresh.Visible = False
        '
        'cmsAOIGroup
        '
        Me.cmsAOIGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddAOIToolStripMenuItem, Me.AddAllAOIsToTheMapToolStripMenuItem})
        Me.cmsAOIGroup.Name = "cmsAOIGroup"
        Me.cmsAOIGroup.Size = New System.Drawing.Size(203, 48)
        '
        'AddAOIToolStripMenuItem
        '
        Me.AddAOIToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddAOIToolStripMenuItem.Name = "AddAOIToolStripMenuItem"
        Me.AddAOIToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.AddAOIToolStripMenuItem.Text = "Add AOI"
        '
        'AddAllAOIsToTheMapToolStripMenuItem
        '
        Me.AddAllAOIsToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddAllAOIsToTheMapToolStripMenuItem.Name = "AddAllAOIsToTheMapToolStripMenuItem"
        Me.AddAllAOIsToTheMapToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.AddAllAOIsToTheMapToolStripMenuItem.Text = "Add All AOIs to the Map"
        '
        'cmsAOI
        '
        Me.cmsAOI.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddAOIToolStripMenuItem1, Me.EditAOIPropertiesToolStripMenuItem, Me.AddToMapToolStripMenuItem2, Me.ToolStripSeparator4, Me.DeleteAOIToolStripMenuItem})
        Me.cmsAOI.Name = "cmsAOI"
        Me.cmsAOI.Size = New System.Drawing.Size(174, 98)
        '
        'AddAOIToolStripMenuItem1
        '
        Me.AddAOIToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddAOIToolStripMenuItem1.Name = "AddAOIToolStripMenuItem1"
        Me.AddAOIToolStripMenuItem1.Size = New System.Drawing.Size(173, 22)
        Me.AddAOIToolStripMenuItem1.Text = "Add AOI"
        '
        'EditAOIPropertiesToolStripMenuItem
        '
        Me.EditAOIPropertiesToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Settings
        Me.EditAOIPropertiesToolStripMenuItem.Name = "EditAOIPropertiesToolStripMenuItem"
        Me.EditAOIPropertiesToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.EditAOIPropertiesToolStripMenuItem.Text = "Edit AOI Properties"
        '
        'AddToMapToolStripMenuItem2
        '
        Me.AddToMapToolStripMenuItem2.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddToMapToolStripMenuItem2.Name = "AddToMapToolStripMenuItem2"
        Me.AddToMapToolStripMenuItem2.Size = New System.Drawing.Size(173, 22)
        Me.AddToMapToolStripMenuItem2.Text = "Add To Map"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(170, 6)
        '
        'DeleteAOIToolStripMenuItem
        '
        Me.DeleteAOIToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.DeleteAOIToolStripMenuItem.Name = "DeleteAOIToolStripMenuItem"
        Me.DeleteAOIToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.DeleteAOIToolStripMenuItem.Text = "Delete AOI"
        '
        'cmsDEMSurveyPair
        '
        Me.cmsDEMSurveyPair.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddChangeDetectionToolStripMenuItem1, Me.AddAllChangeDetectionsToTheMapToolStripMenuItem})
        Me.cmsDEMSurveyPair.Name = "cmsDEMSurveyPair"
        Me.cmsDEMSurveyPair.Size = New System.Drawing.Size(425, 48)
        '
        'AddChangeDetectionToolStripMenuItem1
        '
        Me.AddChangeDetectionToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddChangeDetectionToolStripMenuItem1.Name = "AddChangeDetectionToolStripMenuItem1"
        Me.AddChangeDetectionToolStripMenuItem1.Size = New System.Drawing.Size(424, 22)
        Me.AddChangeDetectionToolStripMenuItem1.Text = "Add Change Detection (With These DEM Surveys)"
        '
        'AddAllChangeDetectionsToTheMapToolStripMenuItem
        '
        Me.AddAllChangeDetectionsToTheMapToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.AddToMap
        Me.AddAllChangeDetectionsToTheMapToolStripMenuItem.Name = "AddAllChangeDetectionsToTheMapToolStripMenuItem"
        Me.AddAllChangeDetectionsToTheMapToolStripMenuItem.Size = New System.Drawing.Size(424, 22)
        Me.AddAllChangeDetectionsToTheMapToolStripMenuItem.Text = "Add All Change Detections (With These DEM Surveys) To The Map"
        '
        'btnHelp
        '
        Me.btnHelp.Image = Global.GCDAddIn.My.Resources.Resources.Help
        Me.btnHelp.Location = New System.Drawing.Point(178, 3)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(29, 23)
        Me.btnHelp.TabIndex = 14
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'cmsBSGroup
        '
        Me.cmsBSGroup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddBudgetSegregationToolStripMenuItem1})
        Me.cmsBSGroup.Name = "cmsBSGroup"
        Me.cmsBSGroup.Size = New System.Drawing.Size(204, 26)
        '
        'AddBudgetSegregationToolStripMenuItem1
        '
        Me.AddBudgetSegregationToolStripMenuItem1.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddBudgetSegregationToolStripMenuItem1.Name = "AddBudgetSegregationToolStripMenuItem1"
        Me.AddBudgetSegregationToolStripMenuItem1.Size = New System.Drawing.Size(203, 22)
        Me.AddBudgetSegregationToolStripMenuItem1.Text = "Add Budget Segregation"
        '
        'cmsBS
        '
        Me.cmsBS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BudgetSegregationPropertiesToolStripMenuItem, Me.BrowseBudgetSegregationFolderToolStripMenuItem, Me.ToolStripSeparator6, Me.AddBudgetSegregationToolStripMenuItem2, Me.DeleteBudgetSegregationToolStripMenuItem})
        Me.cmsBS.Name = "cmsBS"
        Me.cmsBS.Size = New System.Drawing.Size(256, 98)
        '
        'BudgetSegregationPropertiesToolStripMenuItem
        '
        Me.BudgetSegregationPropertiesToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.BudgetSeg
        Me.BudgetSegregationPropertiesToolStripMenuItem.Name = "BudgetSegregationPropertiesToolStripMenuItem"
        Me.BudgetSegregationPropertiesToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.BudgetSegregationPropertiesToolStripMenuItem.Text = "View Budget Segregation Results"
        '
        'BrowseBudgetSegregationFolderToolStripMenuItem
        '
        Me.BrowseBudgetSegregationFolderToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.BrowseFolder
        Me.BrowseBudgetSegregationFolderToolStripMenuItem.Name = "BrowseBudgetSegregationFolderToolStripMenuItem"
        Me.BrowseBudgetSegregationFolderToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.BrowseBudgetSegregationFolderToolStripMenuItem.Text = "Browse Budget Segregation Folder"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(252, 6)
        '
        'AddBudgetSegregationToolStripMenuItem2
        '
        Me.AddBudgetSegregationToolStripMenuItem2.Image = Global.GCDAddIn.My.Resources.Resources.Add
        Me.AddBudgetSegregationToolStripMenuItem2.Name = "AddBudgetSegregationToolStripMenuItem2"
        Me.AddBudgetSegregationToolStripMenuItem2.Size = New System.Drawing.Size(255, 22)
        Me.AddBudgetSegregationToolStripMenuItem2.Text = "Add Budget Segregation"
        '
        'DeleteBudgetSegregationToolStripMenuItem
        '
        Me.DeleteBudgetSegregationToolStripMenuItem.Image = Global.GCDAddIn.My.Resources.Resources.Delete
        Me.DeleteBudgetSegregationToolStripMenuItem.Name = "DeleteBudgetSegregationToolStripMenuItem"
        Me.DeleteBudgetSegregationToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.DeleteBudgetSegregationToolStripMenuItem.Text = "Delete Budget Segregation"
        '
        'ProjectExplorerUC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnAddToMap)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnProperties)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.treProject)
        Me.Name = "ProjectExplorerUC"
        Me.Size = New System.Drawing.Size(696, 663)
        Me.cmsProject.ResumeLayout(False)
        Me.cmsDEMSurvey.ResumeLayout(False)
        Me.cmsAssociatedSurface.ResumeLayout(False)
        Me.cmsAssociatedSurfaceGroup.ResumeLayout(False)
        Me.cmsInputsGroup.ResumeLayout(False)
        Me.cmsErrorSurfacesGroup.ResumeLayout(False)
        Me.cmsErrorSurface.ResumeLayout(False)
        Me.cmsChangeDetectionGroup.ResumeLayout(False)
        Me.cmsChangeDetection.ResumeLayout(False)
        Me.cmsSurveysGroup.ResumeLayout(False)
        Me.cmsAOIGroup.ResumeLayout(False)
        Me.cmsAOI.ResumeLayout(False)
        Me.cmsDEMSurveyPair.ResumeLayout(False)
        Me.cmsBSGroup.ResumeLayout(False)
        Me.cmsBS.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents treProject As System.Windows.Forms.TreeView
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnAddToMap As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnProperties As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents imgTreeImageList As System.Windows.Forms.ImageList
    Friend WithEvents cmsProject As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsDEMSurvey As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddAssociatedSurfaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddErrorSurfaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddToMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteDEMSurveyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditGCDProjectPropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EditDEMSurveyProperatieToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsAssociatedSurface As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditPropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddToMapToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsAssociatedSurfaceGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddAssociatedSurfaceToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAllAssociatedSurfacesToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsInputsGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddDEMSurveyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAllDEMSurveysToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsErrorSurfacesGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddErrorSurfaceToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddErrorSurfaceToMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsErrorSurface As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditErrorSurfacePropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddErrorSurfaceToMapToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteErrorSurfaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteAssociatedSurfaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsChangeDetectionGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddChangeDetectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsChangeDetection As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewChangeDetectionResultsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddChangeDetectionToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsSurveysGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddDEMSurveyToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAllDEMSurveysToTheMapToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
    Friend WithEvents ExploreChangeDetectionFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExploreGCDProjectFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsAOIGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddAOIToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAllAOIsToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsAOI As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditAOIPropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddToMapToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteAOIToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAOIToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsDEMSurveyPair As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddChangeDetectionToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAllChangeDetectionsToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents tTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddBudgetSegregationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsBSGroup As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddBudgetSegregationToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsBS As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents BudgetSegregationPropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BrowseBudgetSegregationFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddBudgetSegregationToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteBudgetSegregationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteChangeDetectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeriveErrorSurfaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeriveErrorSurfaceToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddRawChangeDetectionToTheMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SortByToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SurveyDateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DateAddedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NameAscendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NameDescendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SurveyDateAscendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SurveyDateDescendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DateAddedAscendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DateAddedDescendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
