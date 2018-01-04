using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace GCDCore.UserInterface.Project
{
    partial class ucProjectExplorer : System.Windows.Forms.UserControl
    {

        //UserControl overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucProjectExplorer));
            this.treProject = new System.Windows.Forms.TreeView();
            this.imgTreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.cmsProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditGCDProjectPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAddProjectToMap = new System.Windows.Forms.ToolStripMenuItem();
            this.ExploreGCDProjectFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDEMSurvey = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditDEMSurveyProperatieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteDEMSurveyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddAssociatedSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddErrorSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeriveErrorSurfaceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssociatedSurface = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToMapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteAssociatedSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAssociatedSurfaceGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddAssociatedSurfaceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsInputsGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddDEMSurveyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllDEMSurveysToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsErrorSurfacesGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddErrorSurfaceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DeriveErrorSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddErrorSurfaceToMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsErrorSurface = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditErrorSurfacePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddErrorSurfaceToMapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteErrorSurfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsChangeDetectionGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddChangeDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchChangeDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiEpochChangeDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsChangeDetection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ViewChangeDetectionResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddChangeDetectionToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddRawChangeDetectionToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExploreChangeDetectionFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteChangeDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.AddBudgetSegregationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSurveysGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddDEMSurveyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SortByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NameAscendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NameDescendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SurveyDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SurveyDateAscendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SurveyDateDescendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DateAddedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DateAddedAscendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DateAddedDescendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllAOIsToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToMapToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsDEMSurveyPair = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddChangeDetectionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmsBSGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddBudgetSegregationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsBS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BudgetSegregationPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrowseBudgetSegregationFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.AddBudgetSegregationToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteBudgetSegregationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProject.SuspendLayout();
            this.cmsDEMSurvey.SuspendLayout();
            this.cmsAssociatedSurface.SuspendLayout();
            this.cmsAssociatedSurfaceGroup.SuspendLayout();
            this.cmsInputsGroup.SuspendLayout();
            this.cmsErrorSurfacesGroup.SuspendLayout();
            this.cmsErrorSurface.SuspendLayout();
            this.cmsChangeDetectionGroup.SuspendLayout();
            this.cmsChangeDetection.SuspendLayout();
            this.cmsSurveysGroup.SuspendLayout();
            this.cmsDEMSurveyPair.SuspendLayout();
            this.cmsBSGroup.SuspendLayout();
            this.cmsBS.SuspendLayout();
            this.SuspendLayout();
            // 
            // treProject
            // 
            this.treProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treProject.ImageIndex = 0;
            this.treProject.ImageList = this.imgTreeImageList;
            this.treProject.Location = new System.Drawing.Point(0, 0);
            this.treProject.Name = "treProject";
            this.treProject.SelectedImageIndex = 0;
            this.treProject.ShowNodeToolTips = true;
            this.treProject.ShowRootLines = false;
            this.treProject.Size = new System.Drawing.Size(696, 663);
            this.treProject.TabIndex = 0;
            // 
            // imgTreeImageList
            // 
            this.imgTreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTreeImageList.ImageStream")));
            this.imgTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTreeImageList.Images.SetKeyName(0, "Delta.png");
            this.imgTreeImageList.Images.SetKeyName(1, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(2, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(3, "DEMSurveys.png");
            this.imgTreeImageList.Images.SetKeyName(4, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(5, "AssociatedSurfaces.png");
            this.imgTreeImageList.Images.SetKeyName(6, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(7, "sigma.png");
            this.imgTreeImageList.Images.SetKeyName(8, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(9, "AOI.png");
            this.imgTreeImageList.Images.SetKeyName(10, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(11, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(12, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(13, "About.png");
            this.imgTreeImageList.Images.SetKeyName(14, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(15, "ConcaveHull.png");
            this.imgTreeImageList.Images.SetKeyName(16, "BudgetSeg.png");
            // 
            // cmsProject
            // 
            this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditGCDProjectPropertiesToolStripMenuItem,
            this.cmsAddProjectToMap,
            this.ExploreGCDProjectFolderToolStripMenuItem,
            this.ToolStripSeparator2,
            this.ToolStripMenuItem1});
            this.cmsProject.Name = "cmsProject";
            this.cmsProject.Size = new System.Drawing.Size(231, 98);
            // 
            // EditGCDProjectPropertiesToolStripMenuItem
            // 
            this.EditGCDProjectPropertiesToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Settings;
            this.EditGCDProjectPropertiesToolStripMenuItem.Name = "EditGCDProjectPropertiesToolStripMenuItem";
            this.EditGCDProjectPropertiesToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.EditGCDProjectPropertiesToolStripMenuItem.Text = "Edit GCD Project Properties";
            // 
            // cmsAddProjectToMap
            // 
            this.cmsAddProjectToMap.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.cmsAddProjectToMap.Name = "cmsAddProjectToMap";
            this.cmsAddProjectToMap.Size = new System.Drawing.Size(230, 22);
            this.cmsAddProjectToMap.Text = "Add Entire Project to the Map";
            // 
            // ExploreGCDProjectFolderToolStripMenuItem
            // 
            this.ExploreGCDProjectFolderToolStripMenuItem.Image = global::GCDCore.Properties.Resources.BrowseFolder;
            this.ExploreGCDProjectFolderToolStripMenuItem.Name = "ExploreGCDProjectFolderToolStripMenuItem";
            this.ExploreGCDProjectFolderToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.ExploreGCDProjectFolderToolStripMenuItem.Text = "Explore GCD Project Folder";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.Add;
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.ToolStripMenuItem1.Text = "Specify DEM Survey";
            // 
            // cmsDEMSurvey
            // 
            this.cmsDEMSurvey.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditDEMSurveyProperatieToolStripMenuItem,
            this.AddToMapToolStripMenuItem,
            this.DeleteDEMSurveyToolStripMenuItem,
            this.ToolStripSeparator1,
            this.AddAssociatedSurfaceToolStripMenuItem,
            this.AddErrorSurfaceToolStripMenuItem,
            this.DeriveErrorSurfaceToolStripMenuItem1});
            this.cmsDEMSurvey.Name = "cmsDEMSurvey";
            this.cmsDEMSurvey.Size = new System.Drawing.Size(217, 142);
            // 
            // EditDEMSurveyProperatieToolStripMenuItem
            // 
            this.EditDEMSurveyProperatieToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Settings;
            this.EditDEMSurveyProperatieToolStripMenuItem.Name = "EditDEMSurveyProperatieToolStripMenuItem";
            this.EditDEMSurveyProperatieToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.EditDEMSurveyProperatieToolStripMenuItem.Text = "Edit DEM Survey Properties";
            // 
            // AddToMapToolStripMenuItem
            // 
            this.AddToMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddToMapToolStripMenuItem.Name = "AddToMapToolStripMenuItem";
            this.AddToMapToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.AddToMapToolStripMenuItem.Text = "Add to Map";
            // 
            // DeleteDEMSurveyToolStripMenuItem
            // 
            this.DeleteDEMSurveyToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Delete;
            this.DeleteDEMSurveyToolStripMenuItem.Name = "DeleteDEMSurveyToolStripMenuItem";
            this.DeleteDEMSurveyToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.DeleteDEMSurveyToolStripMenuItem.Text = "Delete DEM Survey";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // AddAssociatedSurfaceToolStripMenuItem
            // 
            this.AddAssociatedSurfaceToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.AddAssociatedSurfaceToolStripMenuItem.Name = "AddAssociatedSurfaceToolStripMenuItem";
            this.AddAssociatedSurfaceToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.AddAssociatedSurfaceToolStripMenuItem.Text = "Add Associated Surface";
            // 
            // AddErrorSurfaceToolStripMenuItem
            // 
            this.AddErrorSurfaceToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.AddErrorSurfaceToolStripMenuItem.Name = "AddErrorSurfaceToolStripMenuItem";
            this.AddErrorSurfaceToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.AddErrorSurfaceToolStripMenuItem.Text = "Specify Error Surface";
            // 
            // DeriveErrorSurfaceToolStripMenuItem1
            // 
            this.DeriveErrorSurfaceToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.sigma;
            this.DeriveErrorSurfaceToolStripMenuItem1.Name = "DeriveErrorSurfaceToolStripMenuItem1";
            this.DeriveErrorSurfaceToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            this.DeriveErrorSurfaceToolStripMenuItem1.Text = "Derive Error Surface";
            // 
            // cmsAssociatedSurface
            // 
            this.cmsAssociatedSurface.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditPropertiesToolStripMenuItem,
            this.AddToMapToolStripMenuItem1,
            this.DeleteAssociatedSurfaceToolStripMenuItem});
            this.cmsAssociatedSurface.Name = "cmsAssociatedSurface";
            this.cmsAssociatedSurface.Size = new System.Drawing.Size(253, 70);
            // 
            // EditPropertiesToolStripMenuItem
            // 
            this.EditPropertiesToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Settings;
            this.EditPropertiesToolStripMenuItem.Name = "EditPropertiesToolStripMenuItem";
            this.EditPropertiesToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.EditPropertiesToolStripMenuItem.Text = "Edit Associated Surface Properties";
            // 
            // AddToMapToolStripMenuItem1
            // 
            this.AddToMapToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddToMapToolStripMenuItem1.Name = "AddToMapToolStripMenuItem1";
            this.AddToMapToolStripMenuItem1.Size = new System.Drawing.Size(252, 22);
            this.AddToMapToolStripMenuItem1.Text = "Add Associated Surface to Map";
            // 
            // DeleteAssociatedSurfaceToolStripMenuItem
            // 
            this.DeleteAssociatedSurfaceToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Delete;
            this.DeleteAssociatedSurfaceToolStripMenuItem.Name = "DeleteAssociatedSurfaceToolStripMenuItem";
            this.DeleteAssociatedSurfaceToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.DeleteAssociatedSurfaceToolStripMenuItem.Text = "Delete Associated Surface";
            // 
            // cmsAssociatedSurfaceGroup
            // 
            this.cmsAssociatedSurfaceGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddAssociatedSurfaceToolStripMenuItem1,
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem});
            this.cmsAssociatedSurfaceGroup.Name = "cmsAssociatedSurfaceGroup";
            this.cmsAssociatedSurfaceGroup.Size = new System.Drawing.Size(282, 48);
            // 
            // AddAssociatedSurfaceToolStripMenuItem1
            // 
            this.AddAssociatedSurfaceToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.Add;
            this.AddAssociatedSurfaceToolStripMenuItem1.Name = "AddAssociatedSurfaceToolStripMenuItem1";
            this.AddAssociatedSurfaceToolStripMenuItem1.Size = new System.Drawing.Size(281, 22);
            this.AddAssociatedSurfaceToolStripMenuItem1.Text = "Add Associated Surface";
            // 
            // AddAllAssociatedSurfacesToTheMapToolStripMenuItem
            // 
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Name = "AddAllAssociatedSurfacesToTheMapToolStripMenuItem";
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
            this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Text = "Add All Associated Surfaces to the Map";
            // 
            // cmsInputsGroup
            // 
            this.cmsInputsGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddDEMSurveyToolStripMenuItem,
            this.AddAllDEMSurveysToTheMapToolStripMenuItem});
            this.cmsInputsGroup.Name = "cmsInputsGroup";
            this.cmsInputsGroup.Size = new System.Drawing.Size(246, 70);
            // 
            // AddDEMSurveyToolStripMenuItem
            // 
            this.AddDEMSurveyToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.AddDEMSurveyToolStripMenuItem.Name = "AddDEMSurveyToolStripMenuItem";
            this.AddDEMSurveyToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.AddDEMSurveyToolStripMenuItem.Text = "Specify DEM Survey";
            // 
            // AddAllDEMSurveysToTheMapToolStripMenuItem
            // 
            this.AddAllDEMSurveysToTheMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddAllDEMSurveysToTheMapToolStripMenuItem.Name = "AddAllDEMSurveysToTheMapToolStripMenuItem";
            this.AddAllDEMSurveysToTheMapToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.AddAllDEMSurveysToTheMapToolStripMenuItem.Text = "Add All DEM Surveys to the Map";
            // 
            // cmsErrorSurfacesGroup
            // 
            this.cmsErrorSurfacesGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddErrorSurfaceToolStripMenuItem1,
            this.DeriveErrorSurfaceToolStripMenuItem,
            this.AddErrorSurfaceToMapToolStripMenuItem});
            this.cmsErrorSurfacesGroup.Name = "cmsErrorSurfacesGroup";
            this.cmsErrorSurfacesGroup.Size = new System.Drawing.Size(250, 70);
            // 
            // AddErrorSurfaceToolStripMenuItem1
            // 
            this.AddErrorSurfaceToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.Add;
            this.AddErrorSurfaceToolStripMenuItem1.Name = "AddErrorSurfaceToolStripMenuItem1";
            this.AddErrorSurfaceToolStripMenuItem1.Size = new System.Drawing.Size(249, 22);
            this.AddErrorSurfaceToolStripMenuItem1.Text = "Specify Error Surface";
            // 
            // DeriveErrorSurfaceToolStripMenuItem
            // 
            this.DeriveErrorSurfaceToolStripMenuItem.Image = global::GCDCore.Properties.Resources.sigma;
            this.DeriveErrorSurfaceToolStripMenuItem.Name = "DeriveErrorSurfaceToolStripMenuItem";
            this.DeriveErrorSurfaceToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.DeriveErrorSurfaceToolStripMenuItem.Text = "Derive Error Surface";
            // 
            // AddErrorSurfaceToMapToolStripMenuItem
            // 
            this.AddErrorSurfaceToMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddErrorSurfaceToMapToolStripMenuItem.Name = "AddErrorSurfaceToMapToolStripMenuItem";
            this.AddErrorSurfaceToMapToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.AddErrorSurfaceToMapToolStripMenuItem.Text = "Add All Error Surfaces to the Map";
            // 
            // cmsErrorSurface
            // 
            this.cmsErrorSurface.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditErrorSurfacePropertiesToolStripMenuItem,
            this.AddErrorSurfaceToMapToolStripMenuItem1,
            this.DeleteErrorSurfaceToolStripMenuItem,
            this.ToolStripSeparator3});
            this.cmsErrorSurface.Name = "cmsErrorSurface";
            this.cmsErrorSurface.Size = new System.Drawing.Size(221, 76);
            // 
            // EditErrorSurfacePropertiesToolStripMenuItem
            // 
            this.EditErrorSurfacePropertiesToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Settings;
            this.EditErrorSurfacePropertiesToolStripMenuItem.Name = "EditErrorSurfacePropertiesToolStripMenuItem";
            this.EditErrorSurfacePropertiesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.EditErrorSurfacePropertiesToolStripMenuItem.Text = "Edit Error Surface Properties";
            // 
            // AddErrorSurfaceToMapToolStripMenuItem1
            // 
            this.AddErrorSurfaceToMapToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddErrorSurfaceToMapToolStripMenuItem1.Name = "AddErrorSurfaceToMapToolStripMenuItem1";
            this.AddErrorSurfaceToMapToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.AddErrorSurfaceToMapToolStripMenuItem1.Text = "Add Error Surface to Map";
            // 
            // DeleteErrorSurfaceToolStripMenuItem
            // 
            this.DeleteErrorSurfaceToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Delete;
            this.DeleteErrorSurfaceToolStripMenuItem.Name = "DeleteErrorSurfaceToolStripMenuItem";
            this.DeleteErrorSurfaceToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.DeleteErrorSurfaceToolStripMenuItem.Text = "Delete Error Surface";
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(217, 6);
            // 
            // cmsChangeDetectionGroup
            // 
            this.cmsChangeDetectionGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddChangeDetectionToolStripMenuItem,
            this.batchChangeDetectionToolStripMenuItem,
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem});
            this.cmsChangeDetectionGroup.Name = "cmsChangeDetectionGroup";
            this.cmsChangeDetectionGroup.Size = new System.Drawing.Size(322, 70);
            // 
            // AddChangeDetectionToolStripMenuItem
            // 
            this.AddChangeDetectionToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.AddChangeDetectionToolStripMenuItem.Name = "AddChangeDetectionToolStripMenuItem";
            this.AddChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(321, 22);
            this.AddChangeDetectionToolStripMenuItem.Text = "Add Change Detection";
            // 
            // batchChangeDetectionToolStripMenuItem
            // 
            this.batchChangeDetectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiEpochChangeDetectionToolStripMenuItem,
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem});
            this.batchChangeDetectionToolStripMenuItem.Name = "batchChangeDetectionToolStripMenuItem";
            this.batchChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(321, 22);
            this.batchChangeDetectionToolStripMenuItem.Text = "Batch Change Detection";
            // 
            // multiEpochChangeDetectionToolStripMenuItem
            // 
            this.multiEpochChangeDetectionToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.multiEpochChangeDetectionToolStripMenuItem.Name = "multiEpochChangeDetectionToolStripMenuItem";
            this.multiEpochChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.multiEpochChangeDetectionToolStripMenuItem.Text = "Multiple Epoch";
            this.multiEpochChangeDetectionToolStripMenuItem.Click += new System.EventHandler(this.multiEpochChangeDetectionToolStripMenuItem_Click);
            // 
            // multiUncertaintyAnalysisChangeDetectionToolStripMenuItem
            // 
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem.Name = "multiUncertaintyAnalysisChangeDetectionToolStripMenuItem";
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem.Text = "Multiple Uncertainty Analysis";
            this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem.Click += new System.EventHandler(this.multiUncertaintyAnalysisChangeDetectionToolStripMenuItem_Click);
            // 
            // AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem
            // 
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Name = "AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem";
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Size = new System.Drawing.Size(321, 22);
            this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Text = "Add All Change Detection Analyses to the Map";
            // 
            // cmsChangeDetection
            // 
            this.cmsChangeDetection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewChangeDetectionResultsToolStripMenuItem,
            this.AddChangeDetectionToTheMapToolStripMenuItem,
            this.AddRawChangeDetectionToTheMapToolStripMenuItem,
            this.ExploreChangeDetectionFolderToolStripMenuItem,
            this.DeleteChangeDetectionToolStripMenuItem,
            this.ToolStripSeparator5,
            this.AddBudgetSegregationToolStripMenuItem});
            this.cmsChangeDetection.Name = "cmsChangeDetection";
            this.cmsChangeDetection.Size = new System.Drawing.Size(325, 142);
            // 
            // ViewChangeDetectionResultsToolStripMenuItem
            // 
            this.ViewChangeDetectionResultsToolStripMenuItem.Image = global::GCDCore.Properties.Resources.GCD;
            this.ViewChangeDetectionResultsToolStripMenuItem.Name = "ViewChangeDetectionResultsToolStripMenuItem";
            this.ViewChangeDetectionResultsToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
            this.ViewChangeDetectionResultsToolStripMenuItem.Text = "View Change Detection Results";
            // 
            // AddChangeDetectionToTheMapToolStripMenuItem
            // 
            this.AddChangeDetectionToTheMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddChangeDetectionToTheMapToolStripMenuItem.Name = "AddChangeDetectionToTheMapToolStripMenuItem";
            this.AddChangeDetectionToTheMapToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
            this.AddChangeDetectionToTheMapToolStripMenuItem.Text = "Add Thresholded Change Detection to the Map";
            // 
            // AddRawChangeDetectionToTheMapToolStripMenuItem
            // 
            this.AddRawChangeDetectionToTheMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddRawChangeDetectionToTheMapToolStripMenuItem.Name = "AddRawChangeDetectionToTheMapToolStripMenuItem";
            this.AddRawChangeDetectionToTheMapToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
            this.AddRawChangeDetectionToTheMapToolStripMenuItem.Text = "Add Raw Change Detection to the Map";
            // 
            // ExploreChangeDetectionFolderToolStripMenuItem
            // 
            this.ExploreChangeDetectionFolderToolStripMenuItem.Image = global::GCDCore.Properties.Resources.BrowseFolder;
            this.ExploreChangeDetectionFolderToolStripMenuItem.Name = "ExploreChangeDetectionFolderToolStripMenuItem";
            this.ExploreChangeDetectionFolderToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
            this.ExploreChangeDetectionFolderToolStripMenuItem.Text = "Explore Change Detection Folder";
            // 
            // DeleteChangeDetectionToolStripMenuItem
            // 
            this.DeleteChangeDetectionToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Delete;
            this.DeleteChangeDetectionToolStripMenuItem.Name = "DeleteChangeDetectionToolStripMenuItem";
            this.DeleteChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
            this.DeleteChangeDetectionToolStripMenuItem.Text = "Delete Change Detection";
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(321, 6);
            // 
            // AddBudgetSegregationToolStripMenuItem
            // 
            this.AddBudgetSegregationToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.AddBudgetSegregationToolStripMenuItem.Name = "AddBudgetSegregationToolStripMenuItem";
            this.AddBudgetSegregationToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
            this.AddBudgetSegregationToolStripMenuItem.Text = "Add Budget Segregation";
            // 
            // cmsSurveysGroup
            // 
            this.cmsSurveysGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddDEMSurveyToolStripMenuItem1,
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1,
            this.SortByToolStripMenuItem});
            this.cmsSurveysGroup.Name = "cmsSurveysGroup";
            this.cmsSurveysGroup.Size = new System.Drawing.Size(246, 70);
            // 
            // AddDEMSurveyToolStripMenuItem1
            // 
            this.AddDEMSurveyToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.Add;
            this.AddDEMSurveyToolStripMenuItem1.Name = "AddDEMSurveyToolStripMenuItem1";
            this.AddDEMSurveyToolStripMenuItem1.Size = new System.Drawing.Size(245, 22);
            this.AddDEMSurveyToolStripMenuItem1.Text = "Specify DEM Survey";
            // 
            // AddAllDEMSurveysToTheMapToolStripMenuItem1
            // 
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Name = "AddAllDEMSurveysToTheMapToolStripMenuItem1";
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Size = new System.Drawing.Size(245, 22);
            this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Text = "Add All DEM Surveys to the Map";
            // 
            // SortByToolStripMenuItem
            // 
            this.SortByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NameToolStripMenuItem,
            this.SurveyDateToolStripMenuItem,
            this.DateAddedToolStripMenuItem});
            this.SortByToolStripMenuItem.Image = global::GCDCore.Properties.Resources.alphabetical;
            this.SortByToolStripMenuItem.Name = "SortByToolStripMenuItem";
            this.SortByToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.SortByToolStripMenuItem.Text = "Sort by:";
            // 
            // NameToolStripMenuItem
            // 
            this.NameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NameAscendingToolStripMenuItem,
            this.NameDescendingToolStripMenuItem});
            this.NameToolStripMenuItem.Name = "NameToolStripMenuItem";
            this.NameToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.NameToolStripMenuItem.Text = "Name";
            // 
            // NameAscendingToolStripMenuItem
            // 
            this.NameAscendingToolStripMenuItem.Name = "NameAscendingToolStripMenuItem";
            this.NameAscendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.NameAscendingToolStripMenuItem.Text = "Ascending";
            // 
            // NameDescendingToolStripMenuItem
            // 
            this.NameDescendingToolStripMenuItem.Name = "NameDescendingToolStripMenuItem";
            this.NameDescendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.NameDescendingToolStripMenuItem.Text = "Descending";
            // 
            // SurveyDateToolStripMenuItem
            // 
            this.SurveyDateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SurveyDateAscendingToolStripMenuItem,
            this.SurveyDateDescendingToolStripMenuItem});
            this.SurveyDateToolStripMenuItem.Name = "SurveyDateToolStripMenuItem";
            this.SurveyDateToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.SurveyDateToolStripMenuItem.Text = "Survey date";
            // 
            // SurveyDateAscendingToolStripMenuItem
            // 
            this.SurveyDateAscendingToolStripMenuItem.Name = "SurveyDateAscendingToolStripMenuItem";
            this.SurveyDateAscendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SurveyDateAscendingToolStripMenuItem.Text = "Ascending";
            // 
            // SurveyDateDescendingToolStripMenuItem
            // 
            this.SurveyDateDescendingToolStripMenuItem.Name = "SurveyDateDescendingToolStripMenuItem";
            this.SurveyDateDescendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SurveyDateDescendingToolStripMenuItem.Text = "Descending";
            // 
            // DateAddedToolStripMenuItem
            // 
            this.DateAddedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DateAddedAscendingToolStripMenuItem,
            this.DateAddedDescendingToolStripMenuItem});
            this.DateAddedToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Check;
            this.DateAddedToolStripMenuItem.Name = "DateAddedToolStripMenuItem";
            this.DateAddedToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.DateAddedToolStripMenuItem.Text = "Date added";
            // 
            // DateAddedAscendingToolStripMenuItem
            // 
            this.DateAddedAscendingToolStripMenuItem.Name = "DateAddedAscendingToolStripMenuItem";
            this.DateAddedAscendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.DateAddedAscendingToolStripMenuItem.Text = "Ascending";
            // 
            // DateAddedDescendingToolStripMenuItem
            // 
            this.DateAddedDescendingToolStripMenuItem.Name = "DateAddedDescendingToolStripMenuItem";
            this.DateAddedDescendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.DateAddedDescendingToolStripMenuItem.Text = "Descending";
            // 
            // AddAOIToolStripMenuItem
            // 
            this.AddAOIToolStripMenuItem.Name = "AddAOIToolStripMenuItem";
            this.AddAOIToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // AddAllAOIsToTheMapToolStripMenuItem
            // 
            this.AddAllAOIsToTheMapToolStripMenuItem.Name = "AddAllAOIsToTheMapToolStripMenuItem";
            this.AddAllAOIsToTheMapToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // AddToMapToolStripMenuItem2
            // 
            this.AddToMapToolStripMenuItem2.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddToMapToolStripMenuItem2.Name = "AddToMapToolStripMenuItem2";
            this.AddToMapToolStripMenuItem2.Size = new System.Drawing.Size(173, 22);
            this.AddToMapToolStripMenuItem2.Text = "Add To Map";
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(170, 6);
            // 
            // cmsDEMSurveyPair
            // 
            this.cmsDEMSurveyPair.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddChangeDetectionToolStripMenuItem1,
            this.addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem,
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem});
            this.cmsDEMSurveyPair.Name = "cmsDEMSurveyPair";
            this.cmsDEMSurveyPair.Size = new System.Drawing.Size(424, 70);
            // 
            // AddChangeDetectionToolStripMenuItem1
            // 
            this.AddChangeDetectionToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.Add;
            this.AddChangeDetectionToolStripMenuItem1.Name = "AddChangeDetectionToolStripMenuItem1";
            this.AddChangeDetectionToolStripMenuItem1.Size = new System.Drawing.Size(423, 22);
            this.AddChangeDetectionToolStripMenuItem1.Text = "Add Change Detection (With These DEM Surveys)";
            // 
            // addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem
            // 
            this.addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Add;
            this.addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem.Name = "addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem";
            this.addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem.Size = new System.Drawing.Size(423, 22);
            this.addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem.Text = "Add Batch Change Detection (With These DEM Surveys)";
            // 
            // AddAllChangeDetectionsToTheMapToolStripMenuItem
            // 
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Name = "AddAllChangeDetectionsToTheMapToolStripMenuItem";
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Size = new System.Drawing.Size(423, 22);
            this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Text = "Add All Change Detections (With These DEM Surveys) To The Map";
            // 
            // cmsBSGroup
            // 
            this.cmsBSGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBudgetSegregationToolStripMenuItem1});
            this.cmsBSGroup.Name = "cmsBSGroup";
            this.cmsBSGroup.Size = new System.Drawing.Size(204, 26);
            // 
            // AddBudgetSegregationToolStripMenuItem1
            // 
            this.AddBudgetSegregationToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.Add;
            this.AddBudgetSegregationToolStripMenuItem1.Name = "AddBudgetSegregationToolStripMenuItem1";
            this.AddBudgetSegregationToolStripMenuItem1.Size = new System.Drawing.Size(203, 22);
            this.AddBudgetSegregationToolStripMenuItem1.Text = "Add Budget Segregation";
            // 
            // cmsBS
            // 
            this.cmsBS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BudgetSegregationPropertiesToolStripMenuItem,
            this.BrowseBudgetSegregationFolderToolStripMenuItem,
            this.ToolStripSeparator6,
            this.AddBudgetSegregationToolStripMenuItem2,
            this.DeleteBudgetSegregationToolStripMenuItem});
            this.cmsBS.Name = "cmsBS";
            this.cmsBS.Size = new System.Drawing.Size(256, 98);
            // 
            // BudgetSegregationPropertiesToolStripMenuItem
            // 
            this.BudgetSegregationPropertiesToolStripMenuItem.Image = global::GCDCore.Properties.Resources.BudgetSeg;
            this.BudgetSegregationPropertiesToolStripMenuItem.Name = "BudgetSegregationPropertiesToolStripMenuItem";
            this.BudgetSegregationPropertiesToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.BudgetSegregationPropertiesToolStripMenuItem.Text = "View Budget Segregation Results";
            // 
            // BrowseBudgetSegregationFolderToolStripMenuItem
            // 
            this.BrowseBudgetSegregationFolderToolStripMenuItem.Image = global::GCDCore.Properties.Resources.BrowseFolder;
            this.BrowseBudgetSegregationFolderToolStripMenuItem.Name = "BrowseBudgetSegregationFolderToolStripMenuItem";
            this.BrowseBudgetSegregationFolderToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.BrowseBudgetSegregationFolderToolStripMenuItem.Text = "Browse Budget Segregation Folder";
            // 
            // ToolStripSeparator6
            // 
            this.ToolStripSeparator6.Name = "ToolStripSeparator6";
            this.ToolStripSeparator6.Size = new System.Drawing.Size(252, 6);
            // 
            // AddBudgetSegregationToolStripMenuItem2
            // 
            this.AddBudgetSegregationToolStripMenuItem2.Image = global::GCDCore.Properties.Resources.Add;
            this.AddBudgetSegregationToolStripMenuItem2.Name = "AddBudgetSegregationToolStripMenuItem2";
            this.AddBudgetSegregationToolStripMenuItem2.Size = new System.Drawing.Size(255, 22);
            this.AddBudgetSegregationToolStripMenuItem2.Text = "Add Budget Segregation";
            // 
            // DeleteBudgetSegregationToolStripMenuItem
            // 
            this.DeleteBudgetSegregationToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Delete;
            this.DeleteBudgetSegregationToolStripMenuItem.Name = "DeleteBudgetSegregationToolStripMenuItem";
            this.DeleteBudgetSegregationToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.DeleteBudgetSegregationToolStripMenuItem.Text = "Delete Budget Segregation";
            // 
            // ucProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treProject);
            this.Name = "ucProjectExplorer";
            this.Size = new System.Drawing.Size(696, 663);
            this.cmsProject.ResumeLayout(false);
            this.cmsDEMSurvey.ResumeLayout(false);
            this.cmsAssociatedSurface.ResumeLayout(false);
            this.cmsAssociatedSurfaceGroup.ResumeLayout(false);
            this.cmsInputsGroup.ResumeLayout(false);
            this.cmsErrorSurfacesGroup.ResumeLayout(false);
            this.cmsErrorSurface.ResumeLayout(false);
            this.cmsChangeDetectionGroup.ResumeLayout(false);
            this.cmsChangeDetection.ResumeLayout(false);
            this.cmsSurveysGroup.ResumeLayout(false);
            this.cmsDEMSurveyPair.ResumeLayout(false);
            this.cmsBSGroup.ResumeLayout(false);
            this.cmsBS.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.ImageList imgTreeImageList;
        internal System.Windows.Forms.ContextMenuStrip cmsProject;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem cmsAddProjectToMap;
        internal System.Windows.Forms.ContextMenuStrip cmsDEMSurvey;
        internal System.Windows.Forms.ToolStripMenuItem AddAssociatedSurfaceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripMenuItem DeleteDEMSurveyToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem EditGCDProjectPropertiesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem EditDEMSurveyProperatieToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsAssociatedSurface;
        internal System.Windows.Forms.ToolStripMenuItem EditPropertiesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem1;
        internal System.Windows.Forms.ContextMenuStrip cmsAssociatedSurfaceGroup;
        internal System.Windows.Forms.ToolStripMenuItem AddAssociatedSurfaceToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem AddAllAssociatedSurfacesToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsInputsGroup;
        internal System.Windows.Forms.ToolStripMenuItem AddDEMSurveyToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddAllDEMSurveysToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsErrorSurfacesGroup;
        internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToMapToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsErrorSurface;
        internal System.Windows.Forms.ToolStripMenuItem EditErrorSurfacePropertiesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToMapToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem DeleteErrorSurfaceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem DeleteAssociatedSurfaceToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsChangeDetectionGroup;
        internal System.Windows.Forms.ToolStripMenuItem AddChangeDetectionToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsChangeDetection;
        internal System.Windows.Forms.ToolStripMenuItem ViewChangeDetectionResultsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddChangeDetectionToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsSurveysGroup;
        internal System.Windows.Forms.ToolStripMenuItem AddDEMSurveyToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem AddAllDEMSurveysToTheMapToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem ExploreChangeDetectionFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ExploreGCDProjectFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddAOIToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddAllAOIsToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem EditAOIPropertiesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem2;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        internal System.Windows.Forms.ContextMenuStrip cmsDEMSurveyPair;
        internal System.Windows.Forms.ToolStripMenuItem AddChangeDetectionToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem AddAllChangeDetectionsToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ToolTip tTip;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.ToolStripMenuItem AddBudgetSegregationToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip cmsBSGroup;
        internal System.Windows.Forms.ToolStripMenuItem AddBudgetSegregationToolStripMenuItem1;
        internal System.Windows.Forms.ContextMenuStrip cmsBS;
        internal System.Windows.Forms.ToolStripMenuItem BudgetSegregationPropertiesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem BrowseBudgetSegregationFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator6;
        internal System.Windows.Forms.ToolStripMenuItem AddBudgetSegregationToolStripMenuItem2;
        internal System.Windows.Forms.ToolStripMenuItem DeleteBudgetSegregationToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DeleteChangeDetectionToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DeriveErrorSurfaceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DeriveErrorSurfaceToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem AddRawChangeDetectionToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SortByToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem NameToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SurveyDateToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DateAddedToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem NameAscendingToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem NameDescendingToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SurveyDateAscendingToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SurveyDateDescendingToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DateAddedAscendingToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DateAddedDescendingToolStripMenuItem;
        internal System.Windows.Forms.TreeView treProject;
        private System.Windows.Forms.ToolStripMenuItem addBatchChangeDetectionWithTheseDEMSurveysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchChangeDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiEpochChangeDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiUncertaintyAnalysisChangeDetectionToolStripMenuItem;
    }
}
