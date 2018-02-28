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
            this.AddAOIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddAllAOIsToTheMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToMapToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.treProject.Size = new System.Drawing.Size(187, 151);
            this.treProject.TabIndex = 0;
            this.treProject.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treProject_NodeMouseClick);
            this.treProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treProject_MouseDown);
            // 
            // imgTreeImageList
            // 
            this.imgTreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTreeImageList.ImageStream")));
            this.imgTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTreeImageList.Images.SetKeyName(0, "BrowseFolder.png");
            this.imgTreeImageList.Images.SetKeyName(1, "Delta.png");
            this.imgTreeImageList.Images.SetKeyName(2, "DEMSurveys.png");
            this.imgTreeImageList.Images.SetKeyName(3, "AssociatedSurfaces.png");
            this.imgTreeImageList.Images.SetKeyName(4, "sigma.png");
            this.imgTreeImageList.Images.SetKeyName(5, "Reservoir.png");
            this.imgTreeImageList.Images.SetKeyName(6, "mask.png");
            this.imgTreeImageList.Images.SetKeyName(7, "About.png");
            this.imgTreeImageList.Images.SetKeyName(8, "ConcaveHull.png");
            this.imgTreeImageList.Images.SetKeyName(9, "BudgetSeg.png");
            this.imgTreeImageList.Images.SetKeyName(10, "comparison.png");
            this.imgTreeImageList.Images.SetKeyName(11, "bars.png");
            this.imgTreeImageList.Images.SetKeyName(12, "GCDFilled.png");
            this.imgTreeImageList.Images.SetKeyName(13, "mask_dir.png");
            this.imgTreeImageList.Images.SetKeyName(14, "AOI.png");
            this.imgTreeImageList.Images.SetKeyName(15, "profile_routes.png");
            this.imgTreeImageList.Images.SetKeyName(16, "linear_extraction.png");
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
            // ucProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treProject);
            this.Name = "ucProjectExplorer";
            this.Size = new System.Drawing.Size(187, 151);
            this.Load += new System.EventHandler(this.ProjectExplorerUC_Load);
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.ImageList imgTreeImageList;
        internal System.Windows.Forms.ToolStripMenuItem AddAOIToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddAllAOIsToTheMapToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem2;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        internal System.Windows.Forms.ToolTip tTip;
        internal System.Windows.Forms.TreeView treProject;
    }
}
