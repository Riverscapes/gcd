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
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
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
			this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
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
			this.cmsAOIGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.AddAOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddAllAOIsToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsAOI = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.AddAOIToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.EditAOIPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddToMapToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.DeleteAOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsDEMSurveyPair = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.AddChangeDetectionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
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
			this.cmsAOIGroup.SuspendLayout();
			this.cmsAOI.SuspendLayout();
			this.cmsDEMSurveyPair.SuspendLayout();
			this.cmsBSGroup.SuspendLayout();
			this.cmsBS.SuspendLayout();
			this.SuspendLayout();
			//
			//treProject
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
			//imgTreeImageList
			//
			this.imgTreeImageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgTreeImageList.ImageStream");
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
			//cmsProject
			//
			this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.EditGCDProjectPropertiesToolStripMenuItem,
				this.ToolStripMenuItem2,
				this.ExploreGCDProjectFolderToolStripMenuItem,
				this.ToolStripSeparator2,
				this.ToolStripMenuItem1
			});
			this.cmsProject.Name = "cmsProject";
			this.cmsProject.Size = new System.Drawing.Size(231, 98);
			//
			//EditGCDProjectPropertiesToolStripMenuItem
			//
			this.EditGCDProjectPropertiesToolStripMenuItem.Image = Properties.Resources.Settings;
			this.EditGCDProjectPropertiesToolStripMenuItem.Name = "EditGCDProjectPropertiesToolStripMenuItem";
			this.EditGCDProjectPropertiesToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.EditGCDProjectPropertiesToolStripMenuItem.Text = "Edit GCD Project Properties";
			//
			//ToolStripMenuItem2
			//
			this.ToolStripMenuItem2.Image = Properties.Resources.AddToMap;
			this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
			this.ToolStripMenuItem2.Size = new System.Drawing.Size(230, 22);
			this.ToolStripMenuItem2.Text = "Add Entire Project to the Map";
			//
			//ExploreGCDProjectFolderToolStripMenuItem
			//
			this.ExploreGCDProjectFolderToolStripMenuItem.Image = Properties.Resources.BrowseFolder;
			this.ExploreGCDProjectFolderToolStripMenuItem.Name = "ExploreGCDProjectFolderToolStripMenuItem";
			this.ExploreGCDProjectFolderToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.ExploreGCDProjectFolderToolStripMenuItem.Text = "Explore GCD Project Folder";
			//
			//ToolStripSeparator2
			//
			this.ToolStripSeparator2.Name = "ToolStripSeparator2";
			this.ToolStripSeparator2.Size = new System.Drawing.Size(227, 6);
			//
			//ToolStripMenuItem1
			//
			this.ToolStripMenuItem1.Image = Properties.Resources.Add;
			this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
			this.ToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
			this.ToolStripMenuItem1.Text = "Specify DEM Survey";
			//
			//cmsDEMSurvey
			//
			this.cmsDEMSurvey.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.EditDEMSurveyProperatieToolStripMenuItem,
				this.AddToMapToolStripMenuItem,
				this.DeleteDEMSurveyToolStripMenuItem,
				this.ToolStripSeparator1,
				this.AddAssociatedSurfaceToolStripMenuItem,
				this.AddErrorSurfaceToolStripMenuItem,
				this.DeriveErrorSurfaceToolStripMenuItem1
			});
			this.cmsDEMSurvey.Name = "cmsDEMSurvey";
			this.cmsDEMSurvey.Size = new System.Drawing.Size(217, 142);
			//
			//EditDEMSurveyProperatieToolStripMenuItem
			//
			this.EditDEMSurveyProperatieToolStripMenuItem.Image = Properties.Resources.Settings;
			this.EditDEMSurveyProperatieToolStripMenuItem.Name = "EditDEMSurveyProperatieToolStripMenuItem";
			this.EditDEMSurveyProperatieToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.EditDEMSurveyProperatieToolStripMenuItem.Text = "Edit DEM Survey Properties";
			//
			//AddToMapToolStripMenuItem
			//
			this.AddToMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddToMapToolStripMenuItem.Name = "AddToMapToolStripMenuItem";
			this.AddToMapToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.AddToMapToolStripMenuItem.Text = "Add to Map";
			//
			//DeleteDEMSurveyToolStripMenuItem
			//
			this.DeleteDEMSurveyToolStripMenuItem.Image = Properties.Resources.Delete;
			this.DeleteDEMSurveyToolStripMenuItem.Name = "DeleteDEMSurveyToolStripMenuItem";
			this.DeleteDEMSurveyToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.DeleteDEMSurveyToolStripMenuItem.Text = "Delete DEM Survey";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(213, 6);
			//
			//AddAssociatedSurfaceToolStripMenuItem
			//
			this.AddAssociatedSurfaceToolStripMenuItem.Image = Properties.Resources.Add;
			this.AddAssociatedSurfaceToolStripMenuItem.Name = "AddAssociatedSurfaceToolStripMenuItem";
			this.AddAssociatedSurfaceToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.AddAssociatedSurfaceToolStripMenuItem.Text = "Add Associated Surface";
			//
			//AddErrorSurfaceToolStripMenuItem
			//
			this.AddErrorSurfaceToolStripMenuItem.Image = Properties.Resources.Add;
			this.AddErrorSurfaceToolStripMenuItem.Name = "AddErrorSurfaceToolStripMenuItem";
			this.AddErrorSurfaceToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.AddErrorSurfaceToolStripMenuItem.Text = "Specify Error Surface";
			//
			//DeriveErrorSurfaceToolStripMenuItem1
			//
			this.DeriveErrorSurfaceToolStripMenuItem1.Image = Properties.Resources.sigma;
			this.DeriveErrorSurfaceToolStripMenuItem1.Name = "DeriveErrorSurfaceToolStripMenuItem1";
			this.DeriveErrorSurfaceToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
			this.DeriveErrorSurfaceToolStripMenuItem1.Text = "Derive Error Surface";
			//
			//cmsAssociatedSurface
			//
			this.cmsAssociatedSurface.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.EditPropertiesToolStripMenuItem,
				this.AddToMapToolStripMenuItem1,
				this.DeleteAssociatedSurfaceToolStripMenuItem
			});
			this.cmsAssociatedSurface.Name = "cmsAssociatedSurface";
			this.cmsAssociatedSurface.Size = new System.Drawing.Size(253, 70);
			//
			//EditPropertiesToolStripMenuItem
			//
			this.EditPropertiesToolStripMenuItem.Image = Properties.Resources.Settings;
			this.EditPropertiesToolStripMenuItem.Name = "EditPropertiesToolStripMenuItem";
			this.EditPropertiesToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
			this.EditPropertiesToolStripMenuItem.Text = "Edit Associated Surface Properties";
			//
			//AddToMapToolStripMenuItem1
			//
			this.AddToMapToolStripMenuItem1.Image = Properties.Resources.AddToMap;
			this.AddToMapToolStripMenuItem1.Name = "AddToMapToolStripMenuItem1";
			this.AddToMapToolStripMenuItem1.Size = new System.Drawing.Size(252, 22);
			this.AddToMapToolStripMenuItem1.Text = "Add Associated Surface to Map";
			//
			//DeleteAssociatedSurfaceToolStripMenuItem
			//
			this.DeleteAssociatedSurfaceToolStripMenuItem.Image = Properties.Resources.Delete;
			this.DeleteAssociatedSurfaceToolStripMenuItem.Name = "DeleteAssociatedSurfaceToolStripMenuItem";
			this.DeleteAssociatedSurfaceToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
			this.DeleteAssociatedSurfaceToolStripMenuItem.Text = "Delete Associated Surface";
			//
			//cmsAssociatedSurfaceGroup
			//
			this.cmsAssociatedSurfaceGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddAssociatedSurfaceToolStripMenuItem1,
				this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem
			});
			this.cmsAssociatedSurfaceGroup.Name = "cmsAssociatedSurfaceGroup";
			this.cmsAssociatedSurfaceGroup.Size = new System.Drawing.Size(282, 48);
			//
			//AddAssociatedSurfaceToolStripMenuItem1
			//
			this.AddAssociatedSurfaceToolStripMenuItem1.Image = Properties.Resources.Add;
			this.AddAssociatedSurfaceToolStripMenuItem1.Name = "AddAssociatedSurfaceToolStripMenuItem1";
			this.AddAssociatedSurfaceToolStripMenuItem1.Size = new System.Drawing.Size(281, 22);
			this.AddAssociatedSurfaceToolStripMenuItem1.Text = "Add Associated Surface";
			//
			//AddAllAssociatedSurfacesToTheMapToolStripMenuItem
			//
			this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Name = "AddAllAssociatedSurfacesToTheMapToolStripMenuItem";
			this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
			this.AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Text = "Add All Associated Surfaces to the Map";
			//
			//cmsInputsGroup
			//
			this.cmsInputsGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddDEMSurveyToolStripMenuItem,
				this.AddAllDEMSurveysToTheMapToolStripMenuItem
			});
			this.cmsInputsGroup.Name = "cmsInputsGroup";
			this.cmsInputsGroup.Size = new System.Drawing.Size(297, 48);
			//
			//AddDEMSurveyToolStripMenuItem
			//
			this.AddDEMSurveyToolStripMenuItem.Image = Properties.Resources.Add;
			this.AddDEMSurveyToolStripMenuItem.Name = "AddDEMSurveyToolStripMenuItem";
			this.AddDEMSurveyToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.AddDEMSurveyToolStripMenuItem.Text = "Specify DEM Survey";
			//
			//AddAllDEMSurveysToTheMapToolStripMenuItem
			//
			this.AddAllDEMSurveysToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddAllDEMSurveysToTheMapToolStripMenuItem.Name = "AddAllDEMSurveysToTheMapToolStripMenuItem";
			this.AddAllDEMSurveysToTheMapToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
			this.AddAllDEMSurveysToTheMapToolStripMenuItem.Text = "Add All DEM Surveys and AOIs to the Map";
			//
			//cmsErrorSurfacesGroup
			//
			this.cmsErrorSurfacesGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddErrorSurfaceToolStripMenuItem1,
				this.DeriveErrorSurfaceToolStripMenuItem,
				this.AddErrorSurfaceToMapToolStripMenuItem
			});
			this.cmsErrorSurfacesGroup.Name = "cmsErrorSurfacesGroup";
			this.cmsErrorSurfacesGroup.Size = new System.Drawing.Size(250, 70);
			//
			//AddErrorSurfaceToolStripMenuItem1
			//
			this.AddErrorSurfaceToolStripMenuItem1.Image = Properties.Resources.Add;
			this.AddErrorSurfaceToolStripMenuItem1.Name = "AddErrorSurfaceToolStripMenuItem1";
			this.AddErrorSurfaceToolStripMenuItem1.Size = new System.Drawing.Size(249, 22);
			this.AddErrorSurfaceToolStripMenuItem1.Text = "Specify Error Surface";
			//
			//DeriveErrorSurfaceToolStripMenuItem
			//
			this.DeriveErrorSurfaceToolStripMenuItem.Image = Properties.Resources.sigma;
			this.DeriveErrorSurfaceToolStripMenuItem.Name = "DeriveErrorSurfaceToolStripMenuItem";
			this.DeriveErrorSurfaceToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.DeriveErrorSurfaceToolStripMenuItem.Text = "Derive Error Surface";
			//
			//AddErrorSurfaceToMapToolStripMenuItem
			//
			this.AddErrorSurfaceToMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddErrorSurfaceToMapToolStripMenuItem.Name = "AddErrorSurfaceToMapToolStripMenuItem";
			this.AddErrorSurfaceToMapToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
			this.AddErrorSurfaceToMapToolStripMenuItem.Text = "Add All Error Surfaces to the Map";
			//
			//cmsErrorSurface
			//
			this.cmsErrorSurface.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.EditErrorSurfacePropertiesToolStripMenuItem,
				this.AddErrorSurfaceToMapToolStripMenuItem1,
				this.DeleteErrorSurfaceToolStripMenuItem,
				this.ToolStripSeparator3
			});
			this.cmsErrorSurface.Name = "cmsErrorSurface";
			this.cmsErrorSurface.Size = new System.Drawing.Size(221, 76);
			//
			//EditErrorSurfacePropertiesToolStripMenuItem
			//
			this.EditErrorSurfacePropertiesToolStripMenuItem.Image = Properties.Resources.Settings;
			this.EditErrorSurfacePropertiesToolStripMenuItem.Name = "EditErrorSurfacePropertiesToolStripMenuItem";
			this.EditErrorSurfacePropertiesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.EditErrorSurfacePropertiesToolStripMenuItem.Text = "Edit Error Surface Properties";
			//
			//AddErrorSurfaceToMapToolStripMenuItem1
			//
			this.AddErrorSurfaceToMapToolStripMenuItem1.Image = Properties.Resources.AddToMap;
			this.AddErrorSurfaceToMapToolStripMenuItem1.Name = "AddErrorSurfaceToMapToolStripMenuItem1";
			this.AddErrorSurfaceToMapToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
			this.AddErrorSurfaceToMapToolStripMenuItem1.Text = "Add Error Surface to Map";
			//
			//DeleteErrorSurfaceToolStripMenuItem
			//
			this.DeleteErrorSurfaceToolStripMenuItem.Image = Properties.Resources.Delete;
			this.DeleteErrorSurfaceToolStripMenuItem.Name = "DeleteErrorSurfaceToolStripMenuItem";
			this.DeleteErrorSurfaceToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.DeleteErrorSurfaceToolStripMenuItem.Text = "Delete Error Surface";
			//
			//ToolStripSeparator3
			//
			this.ToolStripSeparator3.Name = "ToolStripSeparator3";
			this.ToolStripSeparator3.Size = new System.Drawing.Size(217, 6);
			//
			//cmsChangeDetectionGroup
			//
			this.cmsChangeDetectionGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddChangeDetectionToolStripMenuItem,
				this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem
			});
			this.cmsChangeDetectionGroup.Name = "cmsChangeDetectionGroup";
			this.cmsChangeDetectionGroup.Size = new System.Drawing.Size(322, 48);
			//
			//AddChangeDetectionToolStripMenuItem
			//
			this.AddChangeDetectionToolStripMenuItem.Image = Properties.Resources.Add;
			this.AddChangeDetectionToolStripMenuItem.Name = "AddChangeDetectionToolStripMenuItem";
			this.AddChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(321, 22);
			this.AddChangeDetectionToolStripMenuItem.Text = "Add Change Detection";
			//
			//AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem
			//
			this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Name = "AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem";
			this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Size = new System.Drawing.Size(321, 22);
			this.AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Text = "Add All Change Detection Analyses to the Map";
			//
			//cmsChangeDetection
			//
			this.cmsChangeDetection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.ViewChangeDetectionResultsToolStripMenuItem,
				this.AddChangeDetectionToTheMapToolStripMenuItem,
				this.AddRawChangeDetectionToTheMapToolStripMenuItem,
				this.ExploreChangeDetectionFolderToolStripMenuItem,
				this.DeleteChangeDetectionToolStripMenuItem,
				this.ToolStripSeparator5,
				this.AddBudgetSegregationToolStripMenuItem
			});
			this.cmsChangeDetection.Name = "cmsChangeDetection";
			this.cmsChangeDetection.Size = new System.Drawing.Size(325, 142);
			//
			//ViewChangeDetectionResultsToolStripMenuItem
			//
			this.ViewChangeDetectionResultsToolStripMenuItem.Image = Properties.Resources.GCD;
			this.ViewChangeDetectionResultsToolStripMenuItem.Name = "ViewChangeDetectionResultsToolStripMenuItem";
			this.ViewChangeDetectionResultsToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
			this.ViewChangeDetectionResultsToolStripMenuItem.Text = "View Change Detection Results";
			//
			//AddChangeDetectionToTheMapToolStripMenuItem
			//
			this.AddChangeDetectionToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddChangeDetectionToTheMapToolStripMenuItem.Name = "AddChangeDetectionToTheMapToolStripMenuItem";
			this.AddChangeDetectionToTheMapToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
			this.AddChangeDetectionToTheMapToolStripMenuItem.Text = "Add Thresholded Change Detection to the Map";
			//
			//AddRawChangeDetectionToTheMapToolStripMenuItem
			//
			this.AddRawChangeDetectionToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddRawChangeDetectionToTheMapToolStripMenuItem.Name = "AddRawChangeDetectionToTheMapToolStripMenuItem";
			this.AddRawChangeDetectionToTheMapToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
			this.AddRawChangeDetectionToTheMapToolStripMenuItem.Text = "Add Raw Change Detection to the Map";
			//
			//ExploreChangeDetectionFolderToolStripMenuItem
			//
			this.ExploreChangeDetectionFolderToolStripMenuItem.Image = Properties.Resources.BrowseFolder;
			this.ExploreChangeDetectionFolderToolStripMenuItem.Name = "ExploreChangeDetectionFolderToolStripMenuItem";
			this.ExploreChangeDetectionFolderToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
			this.ExploreChangeDetectionFolderToolStripMenuItem.Text = "Explore Change Detection Folder";
			//
			//DeleteChangeDetectionToolStripMenuItem
			//
			this.DeleteChangeDetectionToolStripMenuItem.Image = Properties.Resources.Delete;
			this.DeleteChangeDetectionToolStripMenuItem.Name = "DeleteChangeDetectionToolStripMenuItem";
			this.DeleteChangeDetectionToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
			this.DeleteChangeDetectionToolStripMenuItem.Text = "Delete Change Detection";
			//
			//ToolStripSeparator5
			//
			this.ToolStripSeparator5.Name = "ToolStripSeparator5";
			this.ToolStripSeparator5.Size = new System.Drawing.Size(321, 6);
			//
			//AddBudgetSegregationToolStripMenuItem
			//
			this.AddBudgetSegregationToolStripMenuItem.Image = Properties.Resources.Add;
			this.AddBudgetSegregationToolStripMenuItem.Name = "AddBudgetSegregationToolStripMenuItem";
			this.AddBudgetSegregationToolStripMenuItem.Size = new System.Drawing.Size(324, 22);
			this.AddBudgetSegregationToolStripMenuItem.Text = "Add Budget Segregation";
			//
			//cmsSurveysGroup
			//
			this.cmsSurveysGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddDEMSurveyToolStripMenuItem1,
				this.AddAllDEMSurveysToTheMapToolStripMenuItem1,
				this.SortByToolStripMenuItem
			});
			this.cmsSurveysGroup.Name = "cmsSurveysGroup";
			this.cmsSurveysGroup.Size = new System.Drawing.Size(246, 70);
			//
			//AddDEMSurveyToolStripMenuItem1
			//
			this.AddDEMSurveyToolStripMenuItem1.Image = Properties.Resources.Add;
			this.AddDEMSurveyToolStripMenuItem1.Name = "AddDEMSurveyToolStripMenuItem1";
			this.AddDEMSurveyToolStripMenuItem1.Size = new System.Drawing.Size(245, 22);
			this.AddDEMSurveyToolStripMenuItem1.Text = "Add DEM Survey";
			//
			//AddAllDEMSurveysToTheMapToolStripMenuItem1
			//
			this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Image = Properties.Resources.AddToMap;
			this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Name = "AddAllDEMSurveysToTheMapToolStripMenuItem1";
			this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Size = new System.Drawing.Size(245, 22);
			this.AddAllDEMSurveysToTheMapToolStripMenuItem1.Text = "Add All DEM Surveys to the Map";
			//
			//SortByToolStripMenuItem
			//
			this.SortByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.NameToolStripMenuItem,
				this.SurveyDateToolStripMenuItem,
				this.DateAddedToolStripMenuItem
			});
			this.SortByToolStripMenuItem.Image = Properties.Resources.alphabetical;
			this.SortByToolStripMenuItem.Name = "SortByToolStripMenuItem";
			this.SortByToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
			this.SortByToolStripMenuItem.Text = "Sort by:";
			//
			//NameToolStripMenuItem
			//
			this.NameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.NameAscendingToolStripMenuItem,
				this.NameDescendingToolStripMenuItem
			});
			this.NameToolStripMenuItem.Name = "NameToolStripMenuItem";
			this.NameToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.NameToolStripMenuItem.Text = "Name";
			//
			//NameAscendingToolStripMenuItem
			//
			this.NameAscendingToolStripMenuItem.Name = "NameAscendingToolStripMenuItem";
			this.NameAscendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.NameAscendingToolStripMenuItem.Text = "Ascending";
			//
			//NameDescendingToolStripMenuItem
			//
			this.NameDescendingToolStripMenuItem.Name = "NameDescendingToolStripMenuItem";
			this.NameDescendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.NameDescendingToolStripMenuItem.Text = "Descending";
			//
			//SurveyDateToolStripMenuItem
			//
			this.SurveyDateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.SurveyDateAscendingToolStripMenuItem,
				this.SurveyDateDescendingToolStripMenuItem
			});
			this.SurveyDateToolStripMenuItem.Name = "SurveyDateToolStripMenuItem";
			this.SurveyDateToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.SurveyDateToolStripMenuItem.Text = "Survey date";
			//
			//SurveyDateAscendingToolStripMenuItem
			//
			this.SurveyDateAscendingToolStripMenuItem.Name = "SurveyDateAscendingToolStripMenuItem";
			this.SurveyDateAscendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.SurveyDateAscendingToolStripMenuItem.Text = "Ascending";
			//
			//SurveyDateDescendingToolStripMenuItem
			//
			this.SurveyDateDescendingToolStripMenuItem.Name = "SurveyDateDescendingToolStripMenuItem";
			this.SurveyDateDescendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.SurveyDateDescendingToolStripMenuItem.Text = "Descending";
			//
			//DateAddedToolStripMenuItem
			//
			this.DateAddedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.DateAddedAscendingToolStripMenuItem,
				this.DateAddedDescendingToolStripMenuItem
			});
			this.DateAddedToolStripMenuItem.Image = Properties.Resources.Check;
			this.DateAddedToolStripMenuItem.Name = "DateAddedToolStripMenuItem";
			this.DateAddedToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.DateAddedToolStripMenuItem.Text = "Date added";
			//
			//DateAddedAscendingToolStripMenuItem
			//
			this.DateAddedAscendingToolStripMenuItem.Name = "DateAddedAscendingToolStripMenuItem";
			this.DateAddedAscendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.DateAddedAscendingToolStripMenuItem.Text = "Ascending";
			//
			//DateAddedDescendingToolStripMenuItem
			//
			this.DateAddedDescendingToolStripMenuItem.Name = "DateAddedDescendingToolStripMenuItem";
			this.DateAddedDescendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.DateAddedDescendingToolStripMenuItem.Text = "Descending";
			//
			//cmsAOIGroup
			//
			this.cmsAOIGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddAOIToolStripMenuItem,
				this.AddAllAOIsToTheMapToolStripMenuItem
			});
			this.cmsAOIGroup.Name = "cmsAOIGroup";
			this.cmsAOIGroup.Size = new System.Drawing.Size(203, 48);
			//
			//AddAOIToolStripMenuItem
			//
			this.AddAOIToolStripMenuItem.Image = Properties.Resources.Add;
			this.AddAOIToolStripMenuItem.Name = "AddAOIToolStripMenuItem";
			this.AddAOIToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.AddAOIToolStripMenuItem.Text = "Add AOI";
			//
			//AddAllAOIsToTheMapToolStripMenuItem
			//
			this.AddAllAOIsToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddAllAOIsToTheMapToolStripMenuItem.Name = "AddAllAOIsToTheMapToolStripMenuItem";
			this.AddAllAOIsToTheMapToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.AddAllAOIsToTheMapToolStripMenuItem.Text = "Add All AOIs to the Map";
			//
			//cmsAOI
			//
			this.cmsAOI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddAOIToolStripMenuItem1,
				this.EditAOIPropertiesToolStripMenuItem,
				this.AddToMapToolStripMenuItem2,
				this.ToolStripSeparator4,
				this.DeleteAOIToolStripMenuItem
			});
			this.cmsAOI.Name = "cmsAOI";
			this.cmsAOI.Size = new System.Drawing.Size(174, 98);
			//
			//AddAOIToolStripMenuItem1
			//
			this.AddAOIToolStripMenuItem1.Image = Properties.Resources.Add;
			this.AddAOIToolStripMenuItem1.Name = "AddAOIToolStripMenuItem1";
			this.AddAOIToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
			this.AddAOIToolStripMenuItem1.Text = "Add AOI";
			//
			//EditAOIPropertiesToolStripMenuItem
			//
			this.EditAOIPropertiesToolStripMenuItem.Image = Properties.Resources.Settings;
			this.EditAOIPropertiesToolStripMenuItem.Name = "EditAOIPropertiesToolStripMenuItem";
			this.EditAOIPropertiesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			this.EditAOIPropertiesToolStripMenuItem.Text = "Edit AOI Properties";
			//
			//AddToMapToolStripMenuItem2
			//
			this.AddToMapToolStripMenuItem2.Image = Properties.Resources.AddToMap;
			this.AddToMapToolStripMenuItem2.Name = "AddToMapToolStripMenuItem2";
			this.AddToMapToolStripMenuItem2.Size = new System.Drawing.Size(173, 22);
			this.AddToMapToolStripMenuItem2.Text = "Add To Map";
			//
			//ToolStripSeparator4
			//
			this.ToolStripSeparator4.Name = "ToolStripSeparator4";
			this.ToolStripSeparator4.Size = new System.Drawing.Size(170, 6);
			//
			//DeleteAOIToolStripMenuItem
			//
			this.DeleteAOIToolStripMenuItem.Image = Properties.Resources.Delete;
			this.DeleteAOIToolStripMenuItem.Name = "DeleteAOIToolStripMenuItem";
			this.DeleteAOIToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			this.DeleteAOIToolStripMenuItem.Text = "Delete AOI";
			//
			//cmsDEMSurveyPair
			//
			this.cmsDEMSurveyPair.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.AddChangeDetectionToolStripMenuItem1,
				this.AddAllChangeDetectionsToTheMapToolStripMenuItem
			});
			this.cmsDEMSurveyPair.Name = "cmsDEMSurveyPair";
			this.cmsDEMSurveyPair.Size = new System.Drawing.Size(424, 48);
			//
			//AddChangeDetectionToolStripMenuItem1
			//
			this.AddChangeDetectionToolStripMenuItem1.Image = Properties.Resources.Add;
			this.AddChangeDetectionToolStripMenuItem1.Name = "AddChangeDetectionToolStripMenuItem1";
			this.AddChangeDetectionToolStripMenuItem1.Size = new System.Drawing.Size(423, 22);
			this.AddChangeDetectionToolStripMenuItem1.Text = "Add Change Detection (With These DEM Surveys)";
			//
			//AddAllChangeDetectionsToTheMapToolStripMenuItem
			//
			this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Image = Properties.Resources.AddToMap;
			this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Name = "AddAllChangeDetectionsToTheMapToolStripMenuItem";
			this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Size = new System.Drawing.Size(423, 22);
			this.AddAllChangeDetectionsToTheMapToolStripMenuItem.Text = "Add All Change Detections (With These DEM Surveys) To The Map";
			//
			//cmsBSGroup
			//
			this.cmsBSGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.AddBudgetSegregationToolStripMenuItem1 });
			this.cmsBSGroup.Name = "cmsBSGroup";
			this.cmsBSGroup.Size = new System.Drawing.Size(204, 26);
			//
			//AddBudgetSegregationToolStripMenuItem1
			//
			this.AddBudgetSegregationToolStripMenuItem1.Image = Properties.Resources.Add;
			this.AddBudgetSegregationToolStripMenuItem1.Name = "AddBudgetSegregationToolStripMenuItem1";
			this.AddBudgetSegregationToolStripMenuItem1.Size = new System.Drawing.Size(203, 22);
			this.AddBudgetSegregationToolStripMenuItem1.Text = "Add Budget Segregation";
			//
			//cmsBS
			//
			this.cmsBS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.BudgetSegregationPropertiesToolStripMenuItem,
				this.BrowseBudgetSegregationFolderToolStripMenuItem,
				this.ToolStripSeparator6,
				this.AddBudgetSegregationToolStripMenuItem2,
				this.DeleteBudgetSegregationToolStripMenuItem
			});
			this.cmsBS.Name = "cmsBS";
			this.cmsBS.Size = new System.Drawing.Size(256, 98);
			//
			//BudgetSegregationPropertiesToolStripMenuItem
			//
			this.BudgetSegregationPropertiesToolStripMenuItem.Image = Properties.Resources.BudgetSeg;
			this.BudgetSegregationPropertiesToolStripMenuItem.Name = "BudgetSegregationPropertiesToolStripMenuItem";
			this.BudgetSegregationPropertiesToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.BudgetSegregationPropertiesToolStripMenuItem.Text = "View Budget Segregation Results";
			//
			//BrowseBudgetSegregationFolderToolStripMenuItem
			//
			this.BrowseBudgetSegregationFolderToolStripMenuItem.Image = Properties.Resources.BrowseFolder;
			this.BrowseBudgetSegregationFolderToolStripMenuItem.Name = "BrowseBudgetSegregationFolderToolStripMenuItem";
			this.BrowseBudgetSegregationFolderToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.BrowseBudgetSegregationFolderToolStripMenuItem.Text = "Browse Budget Segregation Folder";
			//
			//ToolStripSeparator6
			//
			this.ToolStripSeparator6.Name = "ToolStripSeparator6";
			this.ToolStripSeparator6.Size = new System.Drawing.Size(252, 6);
			//
			//AddBudgetSegregationToolStripMenuItem2
			//
			this.AddBudgetSegregationToolStripMenuItem2.Image = Properties.Resources.Add;
			this.AddBudgetSegregationToolStripMenuItem2.Name = "AddBudgetSegregationToolStripMenuItem2";
			this.AddBudgetSegregationToolStripMenuItem2.Size = new System.Drawing.Size(255, 22);
			this.AddBudgetSegregationToolStripMenuItem2.Text = "Add Budget Segregation";
			//
			//DeleteBudgetSegregationToolStripMenuItem
			//
			this.DeleteBudgetSegregationToolStripMenuItem.Image = Properties.Resources.Delete;
			this.DeleteBudgetSegregationToolStripMenuItem.Name = "DeleteBudgetSegregationToolStripMenuItem";
			this.DeleteBudgetSegregationToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
			this.DeleteBudgetSegregationToolStripMenuItem.Text = "Delete Budget Segregation";
			//
			//ucProjectExplorer
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
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
			this.cmsAOIGroup.ResumeLayout(false);
			this.cmsAOI.ResumeLayout(false);
			this.cmsDEMSurveyPair.ResumeLayout(false);
			this.cmsBSGroup.ResumeLayout(false);
			this.cmsBS.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		internal System.Windows.Forms.ImageList imgTreeImageList;
		internal System.Windows.Forms.ContextMenuStrip cmsProject;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1 {
			get { return withEventsField_ToolStripMenuItem1; }
			set {
				if (withEventsField_ToolStripMenuItem1 != null) {
					withEventsField_ToolStripMenuItem1.Click -= ToolStripMenuItem1_Click;
				}
				withEventsField_ToolStripMenuItem1 = value;
				if (withEventsField_ToolStripMenuItem1 != null) {
					withEventsField_ToolStripMenuItem1.Click += ToolStripMenuItem1_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_ToolStripMenuItem2;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2 {
			get { return withEventsField_ToolStripMenuItem2; }
			set {
				if (withEventsField_ToolStripMenuItem2 != null) {
					withEventsField_ToolStripMenuItem2.Click -= ToolStripMenuItem2_Click;
				}
				withEventsField_ToolStripMenuItem2 = value;
				if (withEventsField_ToolStripMenuItem2 != null) {
					withEventsField_ToolStripMenuItem2.Click += ToolStripMenuItem2_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsDEMSurvey;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddAssociatedSurfaceToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddAssociatedSurfaceToolStripMenuItem {
			get { return withEventsField_AddAssociatedSurfaceToolStripMenuItem; }
			set {
				if (withEventsField_AddAssociatedSurfaceToolStripMenuItem != null) {
					withEventsField_AddAssociatedSurfaceToolStripMenuItem.Click -= AddAssociatedSurfaceToolStripMenuItem_Click;
				}
				withEventsField_AddAssociatedSurfaceToolStripMenuItem = value;
				if (withEventsField_AddAssociatedSurfaceToolStripMenuItem != null) {
					withEventsField_AddAssociatedSurfaceToolStripMenuItem.Click += AddAssociatedSurfaceToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddErrorSurfaceToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToolStripMenuItem {
			get { return withEventsField_AddErrorSurfaceToolStripMenuItem; }
			set {
				if (withEventsField_AddErrorSurfaceToolStripMenuItem != null) {
					withEventsField_AddErrorSurfaceToolStripMenuItem.Click -= SpecifyErrorSurfaceToolStripMenuItem_Click;
				}
				withEventsField_AddErrorSurfaceToolStripMenuItem = value;
				if (withEventsField_AddErrorSurfaceToolStripMenuItem != null) {
					withEventsField_AddErrorSurfaceToolStripMenuItem.Click += SpecifyErrorSurfaceToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddToMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem {
			get { return withEventsField_AddToMapToolStripMenuItem; }
			set {
				if (withEventsField_AddToMapToolStripMenuItem != null) {
					withEventsField_AddToMapToolStripMenuItem.Click -= AddToMapToolStripMenuItem_Click;
				}
				withEventsField_AddToMapToolStripMenuItem = value;
				if (withEventsField_AddToMapToolStripMenuItem != null) {
					withEventsField_AddToMapToolStripMenuItem.Click += AddToMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeleteDEMSurveyToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteDEMSurveyToolStripMenuItem {
			get { return withEventsField_DeleteDEMSurveyToolStripMenuItem; }
			set {
				if (withEventsField_DeleteDEMSurveyToolStripMenuItem != null) {
					withEventsField_DeleteDEMSurveyToolStripMenuItem.Click -= btnDelete_Click;
					withEventsField_DeleteDEMSurveyToolStripMenuItem.Click -= btnDelete_Click;
				}
				withEventsField_DeleteDEMSurveyToolStripMenuItem = value;
				if (withEventsField_DeleteDEMSurveyToolStripMenuItem != null) {
					withEventsField_DeleteDEMSurveyToolStripMenuItem.Click += btnDelete_Click;
					withEventsField_DeleteDEMSurveyToolStripMenuItem.Click += btnDelete_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_EditGCDProjectPropertiesToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem EditGCDProjectPropertiesToolStripMenuItem {
			get { return withEventsField_EditGCDProjectPropertiesToolStripMenuItem; }
			set {
				if (withEventsField_EditGCDProjectPropertiesToolStripMenuItem != null) {
					withEventsField_EditGCDProjectPropertiesToolStripMenuItem.Click -= EditGCDProjectPropertiesToolStripMenuItem_Click;
				}
				withEventsField_EditGCDProjectPropertiesToolStripMenuItem = value;
				if (withEventsField_EditGCDProjectPropertiesToolStripMenuItem != null) {
					withEventsField_EditGCDProjectPropertiesToolStripMenuItem.Click += EditGCDProjectPropertiesToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_EditDEMSurveyProperatieToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem EditDEMSurveyProperatieToolStripMenuItem {
			get { return withEventsField_EditDEMSurveyProperatieToolStripMenuItem; }
			set {
				if (withEventsField_EditDEMSurveyProperatieToolStripMenuItem != null) {
					withEventsField_EditDEMSurveyProperatieToolStripMenuItem.Click -= EditDEMSurveyProperatieToolStripMenuItem_Click;
				}
				withEventsField_EditDEMSurveyProperatieToolStripMenuItem = value;
				if (withEventsField_EditDEMSurveyProperatieToolStripMenuItem != null) {
					withEventsField_EditDEMSurveyProperatieToolStripMenuItem.Click += EditDEMSurveyProperatieToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsAssociatedSurface;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_EditPropertiesToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem EditPropertiesToolStripMenuItem {
			get { return withEventsField_EditPropertiesToolStripMenuItem; }
			set {
				if (withEventsField_EditPropertiesToolStripMenuItem != null) {
					withEventsField_EditPropertiesToolStripMenuItem.Click -= EditPropertiesToolStripMenuItem_Click;
				}
				withEventsField_EditPropertiesToolStripMenuItem = value;
				if (withEventsField_EditPropertiesToolStripMenuItem != null) {
					withEventsField_EditPropertiesToolStripMenuItem.Click += EditPropertiesToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddToMapToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem1 {
			get { return withEventsField_AddToMapToolStripMenuItem1; }
			set {
				if (withEventsField_AddToMapToolStripMenuItem1 != null) {
					withEventsField_AddToMapToolStripMenuItem1.Click -= AddToMapToolStripMenuItem1_Click;
				}
				withEventsField_AddToMapToolStripMenuItem1 = value;
				if (withEventsField_AddToMapToolStripMenuItem1 != null) {
					withEventsField_AddToMapToolStripMenuItem1.Click += AddToMapToolStripMenuItem1_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsAssociatedSurfaceGroup;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddAssociatedSurfaceToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddAssociatedSurfaceToolStripMenuItem1 {
			get { return withEventsField_AddAssociatedSurfaceToolStripMenuItem1; }
			set {
				if (withEventsField_AddAssociatedSurfaceToolStripMenuItem1 != null) {
					withEventsField_AddAssociatedSurfaceToolStripMenuItem1.Click -= AddAssociatedSurfaceToolStripMenuItem1_Click;
				}
				withEventsField_AddAssociatedSurfaceToolStripMenuItem1 = value;
				if (withEventsField_AddAssociatedSurfaceToolStripMenuItem1 != null) {
					withEventsField_AddAssociatedSurfaceToolStripMenuItem1.Click += AddAssociatedSurfaceToolStripMenuItem1_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddAllAssociatedSurfacesToTheMapToolStripMenuItem {
			get { return withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem; }
			set {
				if (withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem != null) {
					withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Click -= AddAllAssociatedSurfacesToTheMapToolStripMenuItem_Click;
				}
				withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem = value;
				if (withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem != null) {
					withEventsField_AddAllAssociatedSurfacesToTheMapToolStripMenuItem.Click += AddAllAssociatedSurfacesToTheMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsInputsGroup;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddDEMSurveyToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddDEMSurveyToolStripMenuItem {
			get { return withEventsField_AddDEMSurveyToolStripMenuItem; }
			set {
				if (withEventsField_AddDEMSurveyToolStripMenuItem != null) {
					withEventsField_AddDEMSurveyToolStripMenuItem.Click -= ToolStripMenuItem1_Click;
				}
				withEventsField_AddDEMSurveyToolStripMenuItem = value;
				if (withEventsField_AddDEMSurveyToolStripMenuItem != null) {
					withEventsField_AddDEMSurveyToolStripMenuItem.Click += ToolStripMenuItem1_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem AddAllDEMSurveysToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ContextMenuStrip cmsErrorSurfacesGroup;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddErrorSurfaceToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToolStripMenuItem1 {
			get { return withEventsField_AddErrorSurfaceToolStripMenuItem1; }
			set {
				if (withEventsField_AddErrorSurfaceToolStripMenuItem1 != null) {
					withEventsField_AddErrorSurfaceToolStripMenuItem1.Click -= SpecifyErrorSurfaceToolStripMenuItem_Click;
				}
				withEventsField_AddErrorSurfaceToolStripMenuItem1 = value;
				if (withEventsField_AddErrorSurfaceToolStripMenuItem1 != null) {
					withEventsField_AddErrorSurfaceToolStripMenuItem1.Click += SpecifyErrorSurfaceToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddErrorSurfaceToMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToMapToolStripMenuItem {
			get { return withEventsField_AddErrorSurfaceToMapToolStripMenuItem; }
			set {
				if (withEventsField_AddErrorSurfaceToMapToolStripMenuItem != null) {
					withEventsField_AddErrorSurfaceToMapToolStripMenuItem.Click -= AddErrorSurfaceToMapToolStripMenuItem_Click;
				}
				withEventsField_AddErrorSurfaceToMapToolStripMenuItem = value;
				if (withEventsField_AddErrorSurfaceToMapToolStripMenuItem != null) {
					withEventsField_AddErrorSurfaceToMapToolStripMenuItem.Click += AddErrorSurfaceToMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsErrorSurface;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_EditErrorSurfacePropertiesToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem EditErrorSurfacePropertiesToolStripMenuItem {
			get { return withEventsField_EditErrorSurfacePropertiesToolStripMenuItem; }
			set {
				if (withEventsField_EditErrorSurfacePropertiesToolStripMenuItem != null) {
					withEventsField_EditErrorSurfacePropertiesToolStripMenuItem.Click -= EditErrorSurfacePropertiesToolStripMenuItem_Click;
				}
				withEventsField_EditErrorSurfacePropertiesToolStripMenuItem = value;
				if (withEventsField_EditErrorSurfacePropertiesToolStripMenuItem != null) {
					withEventsField_EditErrorSurfacePropertiesToolStripMenuItem.Click += EditErrorSurfacePropertiesToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddErrorSurfaceToMapToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddErrorSurfaceToMapToolStripMenuItem1 {
			get { return withEventsField_AddErrorSurfaceToMapToolStripMenuItem1; }
			set {
				if (withEventsField_AddErrorSurfaceToMapToolStripMenuItem1 != null) {
					withEventsField_AddErrorSurfaceToMapToolStripMenuItem1.Click -= AddErrorSurfaceToMapToolStripMenuItem1_Click;
				}
				withEventsField_AddErrorSurfaceToMapToolStripMenuItem1 = value;
				if (withEventsField_AddErrorSurfaceToMapToolStripMenuItem1 != null) {
					withEventsField_AddErrorSurfaceToMapToolStripMenuItem1.Click += AddErrorSurfaceToMapToolStripMenuItem1_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeleteErrorSurfaceToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteErrorSurfaceToolStripMenuItem {
			get { return withEventsField_DeleteErrorSurfaceToolStripMenuItem; }
			set {
				if (withEventsField_DeleteErrorSurfaceToolStripMenuItem != null) {
					withEventsField_DeleteErrorSurfaceToolStripMenuItem.Click -= DeleteErrorSurfaceToolStripMenuItem_Click;
					withEventsField_DeleteErrorSurfaceToolStripMenuItem.Click -= btnDelete_Click;
				}
				withEventsField_DeleteErrorSurfaceToolStripMenuItem = value;
				if (withEventsField_DeleteErrorSurfaceToolStripMenuItem != null) {
					withEventsField_DeleteErrorSurfaceToolStripMenuItem.Click += DeleteErrorSurfaceToolStripMenuItem_Click;
					withEventsField_DeleteErrorSurfaceToolStripMenuItem.Click += btnDelete_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeleteAssociatedSurfaceToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteAssociatedSurfaceToolStripMenuItem {
			get { return withEventsField_DeleteAssociatedSurfaceToolStripMenuItem; }
			set {
				if (withEventsField_DeleteAssociatedSurfaceToolStripMenuItem != null) {
					withEventsField_DeleteAssociatedSurfaceToolStripMenuItem.Click -= btnDelete_Click;
				}
				withEventsField_DeleteAssociatedSurfaceToolStripMenuItem = value;
				if (withEventsField_DeleteAssociatedSurfaceToolStripMenuItem != null) {
					withEventsField_DeleteAssociatedSurfaceToolStripMenuItem.Click += btnDelete_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsChangeDetectionGroup;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddChangeDetectionToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddChangeDetectionToolStripMenuItem {
			get { return withEventsField_AddChangeDetectionToolStripMenuItem; }
			set {
				if (withEventsField_AddChangeDetectionToolStripMenuItem != null) {
					withEventsField_AddChangeDetectionToolStripMenuItem.Click -= AddChangeDetectionToolStripMenuItem_Click;
				}
				withEventsField_AddChangeDetectionToolStripMenuItem = value;
				if (withEventsField_AddChangeDetectionToolStripMenuItem != null) {
					withEventsField_AddChangeDetectionToolStripMenuItem.Click += AddChangeDetectionToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem {
			get { return withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem; }
			set {
				if (withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem != null) {
					withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Click -= AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem_Click;
				}
				withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem = value;
				if (withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem != null) {
					withEventsField_AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem.Click += AddAllChangeDetectionAnalysesToTheMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsChangeDetection;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_ViewChangeDetectionResultsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ViewChangeDetectionResultsToolStripMenuItem {
			get { return withEventsField_ViewChangeDetectionResultsToolStripMenuItem; }
			set {
				if (withEventsField_ViewChangeDetectionResultsToolStripMenuItem != null) {
					withEventsField_ViewChangeDetectionResultsToolStripMenuItem.Click -= ViewChangeDetectionResultsToolStripMenuItem_Click;
				}
				withEventsField_ViewChangeDetectionResultsToolStripMenuItem = value;
				if (withEventsField_ViewChangeDetectionResultsToolStripMenuItem != null) {
					withEventsField_ViewChangeDetectionResultsToolStripMenuItem.Click += ViewChangeDetectionResultsToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddChangeDetectionToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddChangeDetectionToTheMapToolStripMenuItem {
			get { return withEventsField_AddChangeDetectionToTheMapToolStripMenuItem; }
			set {
				if (withEventsField_AddChangeDetectionToTheMapToolStripMenuItem != null) {
					withEventsField_AddChangeDetectionToTheMapToolStripMenuItem.Click -= AddChangeDetectionToTheMapToolStripMenuItem_Click;
				}
				withEventsField_AddChangeDetectionToTheMapToolStripMenuItem = value;
				if (withEventsField_AddChangeDetectionToTheMapToolStripMenuItem != null) {
					withEventsField_AddChangeDetectionToTheMapToolStripMenuItem.Click += AddChangeDetectionToTheMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsSurveysGroup;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddDEMSurveyToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddDEMSurveyToolStripMenuItem1 {
			get { return withEventsField_AddDEMSurveyToolStripMenuItem1; }
			set {
				if (withEventsField_AddDEMSurveyToolStripMenuItem1 != null) {
					withEventsField_AddDEMSurveyToolStripMenuItem1.Click -= AddDEMSurveyToolStripMenuItem1_Click;
				}
				withEventsField_AddDEMSurveyToolStripMenuItem1 = value;
				if (withEventsField_AddDEMSurveyToolStripMenuItem1 != null) {
					withEventsField_AddDEMSurveyToolStripMenuItem1.Click += AddDEMSurveyToolStripMenuItem1_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddAllDEMSurveysToTheMapToolStripMenuItem1 {
			get { return withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1; }
			set {
				if (withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1 != null) {
					withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1.Click -= AddAllDEMSurveysToTheMapToolStripMenuItem1_Click;
				}
				withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1 = value;
				if (withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1 != null) {
					withEventsField_AddAllDEMSurveysToTheMapToolStripMenuItem1.Click += AddAllDEMSurveysToTheMapToolStripMenuItem1_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_ExploreChangeDetectionFolderToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ExploreChangeDetectionFolderToolStripMenuItem {
			get { return withEventsField_ExploreChangeDetectionFolderToolStripMenuItem; }
			set {
				if (withEventsField_ExploreChangeDetectionFolderToolStripMenuItem != null) {
					withEventsField_ExploreChangeDetectionFolderToolStripMenuItem.Click -= ExploreChangeDetectionFolderToolStripMenuItem_Click;
				}
				withEventsField_ExploreChangeDetectionFolderToolStripMenuItem = value;
				if (withEventsField_ExploreChangeDetectionFolderToolStripMenuItem != null) {
					withEventsField_ExploreChangeDetectionFolderToolStripMenuItem.Click += ExploreChangeDetectionFolderToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_ExploreGCDProjectFolderToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ExploreGCDProjectFolderToolStripMenuItem {
			get { return withEventsField_ExploreGCDProjectFolderToolStripMenuItem; }
			set {
				if (withEventsField_ExploreGCDProjectFolderToolStripMenuItem != null) {
					withEventsField_ExploreGCDProjectFolderToolStripMenuItem.Click -= ExploreGCDProjectFolderToolStripMenuItem_Click;
				}
				withEventsField_ExploreGCDProjectFolderToolStripMenuItem = value;
				if (withEventsField_ExploreGCDProjectFolderToolStripMenuItem != null) {
					withEventsField_ExploreGCDProjectFolderToolStripMenuItem.Click += ExploreGCDProjectFolderToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsAOIGroup;
		internal System.Windows.Forms.ToolStripMenuItem AddAOIToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddAllAOIsToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ContextMenuStrip cmsAOI;
		internal System.Windows.Forms.ToolStripMenuItem EditAOIPropertiesToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem2;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeleteAOIToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteAOIToolStripMenuItem {
			get { return withEventsField_DeleteAOIToolStripMenuItem; }
			set {
				if (withEventsField_DeleteAOIToolStripMenuItem != null) {
					withEventsField_DeleteAOIToolStripMenuItem.Click -= btnDelete_Click;
				}
				withEventsField_DeleteAOIToolStripMenuItem = value;
				if (withEventsField_DeleteAOIToolStripMenuItem != null) {
					withEventsField_DeleteAOIToolStripMenuItem.Click += btnDelete_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem AddAOIToolStripMenuItem1;
		internal System.Windows.Forms.ContextMenuStrip cmsDEMSurveyPair;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddChangeDetectionToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddChangeDetectionToolStripMenuItem1 {
			get { return withEventsField_AddChangeDetectionToolStripMenuItem1; }
			set {
				if (withEventsField_AddChangeDetectionToolStripMenuItem1 != null) {
					withEventsField_AddChangeDetectionToolStripMenuItem1.Click -= AddChangeDetectionToolStripMenuItem1_Click;
				}
				withEventsField_AddChangeDetectionToolStripMenuItem1 = value;
				if (withEventsField_AddChangeDetectionToolStripMenuItem1 != null) {
					withEventsField_AddChangeDetectionToolStripMenuItem1.Click += AddChangeDetectionToolStripMenuItem1_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddAllChangeDetectionsToTheMapToolStripMenuItem {
			get { return withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem; }
			set {
				if (withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem != null) {
					withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem.Click -= AddAllChangeDetectionsToTheMapToolStripMenuItem_Click;
				}
				withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem = value;
				if (withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem != null) {
					withEventsField_AddAllChangeDetectionsToTheMapToolStripMenuItem.Click += AddAllChangeDetectionsToTheMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolTip tTip;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddBudgetSegregationToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddBudgetSegregationToolStripMenuItem {
			get { return withEventsField_AddBudgetSegregationToolStripMenuItem; }
			set {
				if (withEventsField_AddBudgetSegregationToolStripMenuItem != null) {
					withEventsField_AddBudgetSegregationToolStripMenuItem.Click -= AddBudgetSegregationToolStripMenuItem1_Click;
				}
				withEventsField_AddBudgetSegregationToolStripMenuItem = value;
				if (withEventsField_AddBudgetSegregationToolStripMenuItem != null) {
					withEventsField_AddBudgetSegregationToolStripMenuItem.Click += AddBudgetSegregationToolStripMenuItem1_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsBSGroup;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddBudgetSegregationToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddBudgetSegregationToolStripMenuItem1 {
			get { return withEventsField_AddBudgetSegregationToolStripMenuItem1; }
			set {
				if (withEventsField_AddBudgetSegregationToolStripMenuItem1 != null) {
					withEventsField_AddBudgetSegregationToolStripMenuItem1.Click -= AddBudgetSegregationToolStripMenuItem1_Click;
				}
				withEventsField_AddBudgetSegregationToolStripMenuItem1 = value;
				if (withEventsField_AddBudgetSegregationToolStripMenuItem1 != null) {
					withEventsField_AddBudgetSegregationToolStripMenuItem1.Click += AddBudgetSegregationToolStripMenuItem1_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsBS;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_BudgetSegregationPropertiesToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem BudgetSegregationPropertiesToolStripMenuItem {
			get { return withEventsField_BudgetSegregationPropertiesToolStripMenuItem; }
			set {
				if (withEventsField_BudgetSegregationPropertiesToolStripMenuItem != null) {
					withEventsField_BudgetSegregationPropertiesToolStripMenuItem.Click -= BudgetSegregationPropertiesToolStripMenuItem_Click;
				}
				withEventsField_BudgetSegregationPropertiesToolStripMenuItem = value;
				if (withEventsField_BudgetSegregationPropertiesToolStripMenuItem != null) {
					withEventsField_BudgetSegregationPropertiesToolStripMenuItem.Click += BudgetSegregationPropertiesToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem BrowseBudgetSegregationFolderToolStripMenuItem {
			get { return withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem; }
			set {
				if (withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem != null) {
					withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem.Click -= BrowseBudgetSegregationFolderToolStripMenuItem_Click;
				}
				withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem = value;
				if (withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem != null) {
					withEventsField_BrowseBudgetSegregationFolderToolStripMenuItem.Click += BrowseBudgetSegregationFolderToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddBudgetSegregationToolStripMenuItem2;
		internal System.Windows.Forms.ToolStripMenuItem AddBudgetSegregationToolStripMenuItem2 {
			get { return withEventsField_AddBudgetSegregationToolStripMenuItem2; }
			set {
				if (withEventsField_AddBudgetSegregationToolStripMenuItem2 != null) {
					withEventsField_AddBudgetSegregationToolStripMenuItem2.Click -= AddBudgetSegregationToolStripMenuItem1_Click;
				}
				withEventsField_AddBudgetSegregationToolStripMenuItem2 = value;
				if (withEventsField_AddBudgetSegregationToolStripMenuItem2 != null) {
					withEventsField_AddBudgetSegregationToolStripMenuItem2.Click += AddBudgetSegregationToolStripMenuItem1_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem DeleteBudgetSegregationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeleteChangeDetectionToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeleteChangeDetectionToolStripMenuItem {
			get { return withEventsField_DeleteChangeDetectionToolStripMenuItem; }
			set {
				if (withEventsField_DeleteChangeDetectionToolStripMenuItem != null) {
					withEventsField_DeleteChangeDetectionToolStripMenuItem.Click -= btnDelete_Click;
					withEventsField_DeleteChangeDetectionToolStripMenuItem.Click -= btnDelete_Click;
				}
				withEventsField_DeleteChangeDetectionToolStripMenuItem = value;
				if (withEventsField_DeleteChangeDetectionToolStripMenuItem != null) {
					withEventsField_DeleteChangeDetectionToolStripMenuItem.Click += btnDelete_Click;
					withEventsField_DeleteChangeDetectionToolStripMenuItem.Click += btnDelete_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeriveErrorSurfaceToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DeriveErrorSurfaceToolStripMenuItem {
			get { return withEventsField_DeriveErrorSurfaceToolStripMenuItem; }
			set {
				if (withEventsField_DeriveErrorSurfaceToolStripMenuItem != null) {
					withEventsField_DeriveErrorSurfaceToolStripMenuItem.Click -= DeriveErrorSurfaceToolStripMenuItem_Click;
				}
				withEventsField_DeriveErrorSurfaceToolStripMenuItem = value;
				if (withEventsField_DeriveErrorSurfaceToolStripMenuItem != null) {
					withEventsField_DeriveErrorSurfaceToolStripMenuItem.Click += DeriveErrorSurfaceToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DeriveErrorSurfaceToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem DeriveErrorSurfaceToolStripMenuItem1 {
			get { return withEventsField_DeriveErrorSurfaceToolStripMenuItem1; }
			set {
				if (withEventsField_DeriveErrorSurfaceToolStripMenuItem1 != null) {
					withEventsField_DeriveErrorSurfaceToolStripMenuItem1.Click -= DeriveErrorSurfaceToolStripMenuItem_Click;
				}
				withEventsField_DeriveErrorSurfaceToolStripMenuItem1 = value;
				if (withEventsField_DeriveErrorSurfaceToolStripMenuItem1 != null) {
					withEventsField_DeriveErrorSurfaceToolStripMenuItem1.Click += DeriveErrorSurfaceToolStripMenuItem_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AddRawChangeDetectionToTheMapToolStripMenuItem {
			get { return withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem; }
			set {
				if (withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem != null) {
					withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem.Click -= AddRawChangeDetectionToTheMapToolStripMenuItem_Click;
				}
				withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem = value;
				if (withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem != null) {
					withEventsField_AddRawChangeDetectionToTheMapToolStripMenuItem.Click += AddRawChangeDetectionToTheMapToolStripMenuItem_Click;
				}
			}
		}
		internal System.Windows.Forms.ToolStripMenuItem SortByToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem NameToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SurveyDateToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DateAddedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_NameAscendingToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem NameAscendingToolStripMenuItem {
			get { return withEventsField_NameAscendingToolStripMenuItem; }
			set {
				if (withEventsField_NameAscendingToolStripMenuItem != null) {
					withEventsField_NameAscendingToolStripMenuItem.Click -= SortTOC_Click;
				}
				withEventsField_NameAscendingToolStripMenuItem = value;
				if (withEventsField_NameAscendingToolStripMenuItem != null) {
					withEventsField_NameAscendingToolStripMenuItem.Click += SortTOC_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_NameDescendingToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem NameDescendingToolStripMenuItem {
			get { return withEventsField_NameDescendingToolStripMenuItem; }
			set {
				if (withEventsField_NameDescendingToolStripMenuItem != null) {
					withEventsField_NameDescendingToolStripMenuItem.Click -= SortTOC_Click;
				}
				withEventsField_NameDescendingToolStripMenuItem = value;
				if (withEventsField_NameDescendingToolStripMenuItem != null) {
					withEventsField_NameDescendingToolStripMenuItem.Click += SortTOC_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_SurveyDateAscendingToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SurveyDateAscendingToolStripMenuItem {
			get { return withEventsField_SurveyDateAscendingToolStripMenuItem; }
			set {
				if (withEventsField_SurveyDateAscendingToolStripMenuItem != null) {
					withEventsField_SurveyDateAscendingToolStripMenuItem.Click -= SortTOC_Click;
				}
				withEventsField_SurveyDateAscendingToolStripMenuItem = value;
				if (withEventsField_SurveyDateAscendingToolStripMenuItem != null) {
					withEventsField_SurveyDateAscendingToolStripMenuItem.Click += SortTOC_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_SurveyDateDescendingToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SurveyDateDescendingToolStripMenuItem {
			get { return withEventsField_SurveyDateDescendingToolStripMenuItem; }
			set {
				if (withEventsField_SurveyDateDescendingToolStripMenuItem != null) {
					withEventsField_SurveyDateDescendingToolStripMenuItem.Click -= SortTOC_Click;
				}
				withEventsField_SurveyDateDescendingToolStripMenuItem = value;
				if (withEventsField_SurveyDateDescendingToolStripMenuItem != null) {
					withEventsField_SurveyDateDescendingToolStripMenuItem.Click += SortTOC_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DateAddedAscendingToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DateAddedAscendingToolStripMenuItem {
			get { return withEventsField_DateAddedAscendingToolStripMenuItem; }
			set {
				if (withEventsField_DateAddedAscendingToolStripMenuItem != null) {
					withEventsField_DateAddedAscendingToolStripMenuItem.Click -= SortTOC_Click;
				}
				withEventsField_DateAddedAscendingToolStripMenuItem = value;
				if (withEventsField_DateAddedAscendingToolStripMenuItem != null) {
					withEventsField_DateAddedAscendingToolStripMenuItem.Click += SortTOC_Click;
				}
			}
		}
		private System.Windows.Forms.ToolStripMenuItem withEventsField_DateAddedDescendingToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem DateAddedDescendingToolStripMenuItem {
			get { return withEventsField_DateAddedDescendingToolStripMenuItem; }
			set {
				if (withEventsField_DateAddedDescendingToolStripMenuItem != null) {
					withEventsField_DateAddedDescendingToolStripMenuItem.Click -= SortTOC_Click;
				}
				withEventsField_DateAddedDescendingToolStripMenuItem = value;
				if (withEventsField_DateAddedDescendingToolStripMenuItem != null) {
					withEventsField_DateAddedDescendingToolStripMenuItem.Click += SortTOC_Click;
				}
			}
		}
		private System.Windows.Forms.TreeView withEventsField_treProject;
		internal System.Windows.Forms.TreeView treProject {
			get { return withEventsField_treProject; }
			set {
				if (withEventsField_treProject != null) {
					withEventsField_treProject.DoubleClick -= treProject_DoubleClick;
					withEventsField_treProject.MouseDown -= treProject_MouseDown;
					withEventsField_treProject.AfterSelect -= treProject_AfterSelect;
				}
				withEventsField_treProject = value;
				if (withEventsField_treProject != null) {
					withEventsField_treProject.DoubleClick += treProject_DoubleClick;
					withEventsField_treProject.MouseDown += treProject_MouseDown;
					withEventsField_treProject.AfterSelect += treProject_AfterSelect;
				}
			}
		}
	}
}
