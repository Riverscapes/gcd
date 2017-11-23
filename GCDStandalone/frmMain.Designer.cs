namespace GCDStandalone
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGCDProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGCDProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeGCDProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseGCDProjectFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataPreparationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanRasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fISLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineGCDHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCDWebSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutGCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssProjectPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsiNewProject = new System.Windows.Forms.ToolStripButton();
            this.tsiOpenProject = new System.Windows.Forms.ToolStripButton();
            this.tsiProjectProperties = new System.Windows.Forms.ToolStripButton();
            this.tsiBrowseProjectFolder = new System.Windows.Forms.ToolStripButton();
            this.ucProjectExplorer1 = new GCDUserInterface.Project.ucProjectExplorer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.dataPreparationToolStripMenuItem,
            this.analysisToolStripMenuItem,
            this.customizeToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(907, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGCDProjectToolStripMenuItem,
            this.openGCDProjectToolStripMenuItem,
            this.closeGCDProjectToolStripMenuItem,
            this.projectPropertiesToolStripMenuItem,
            this.browseGCDProjectFolderToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // newGCDProjectToolStripMenuItem
            // 
            this.newGCDProjectToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.NewProject;
            this.newGCDProjectToolStripMenuItem.Name = "newGCDProjectToolStripMenuItem";
            this.newGCDProjectToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.newGCDProjectToolStripMenuItem.Text = "New GCD Project...";
            this.newGCDProjectToolStripMenuItem.Click += new System.EventHandler(this.ProjectProperties_Click);
            // 
            // openGCDProjectToolStripMenuItem
            // 
            this.openGCDProjectToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.OpenProject;
            this.openGCDProjectToolStripMenuItem.Name = "openGCDProjectToolStripMenuItem";
            this.openGCDProjectToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.openGCDProjectToolStripMenuItem.Text = "Open GCD Project...";
            this.openGCDProjectToolStripMenuItem.Click += new System.EventHandler(this.openGCDProjectToolStripMenuItem_Click);
            // 
            // closeGCDProjectToolStripMenuItem
            // 
            this.closeGCDProjectToolStripMenuItem.Name = "closeGCDProjectToolStripMenuItem";
            this.closeGCDProjectToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.closeGCDProjectToolStripMenuItem.Text = "Close GCD Project";
            this.closeGCDProjectToolStripMenuItem.Click += new System.EventHandler(this.closeGCDProjectToolStripMenuItem_Click);
            // 
            // projectPropertiesToolStripMenuItem
            // 
            this.projectPropertiesToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Settings;
            this.projectPropertiesToolStripMenuItem.Name = "projectPropertiesToolStripMenuItem";
            this.projectPropertiesToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.projectPropertiesToolStripMenuItem.Text = "Project Properties";
            this.projectPropertiesToolStripMenuItem.Click += new System.EventHandler(this.ProjectProperties_Click);
            // 
            // browseGCDProjectFolderToolStripMenuItem
            // 
            this.browseGCDProjectFolderToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.BrowseFolder;
            this.browseGCDProjectFolderToolStripMenuItem.Name = "browseGCDProjectFolderToolStripMenuItem";
            this.browseGCDProjectFolderToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.browseGCDProjectFolderToolStripMenuItem.Text = "Browse GCD Project Folder";
            this.browseGCDProjectFolderToolStripMenuItem.Click += new System.EventHandler(this.browseGCDProjectFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataPreparationToolStripMenuItem
            // 
            this.dataPreparationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cleanRasterToolStripMenuItem});
            this.dataPreparationToolStripMenuItem.Name = "dataPreparationToolStripMenuItem";
            this.dataPreparationToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.dataPreparationToolStripMenuItem.Text = "Data Preparation";
            // 
            // cleanRasterToolStripMenuItem
            // 
            this.cleanRasterToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.CleanRaster;
            this.cleanRasterToolStripMenuItem.Name = "cleanRasterToolStripMenuItem";
            this.cleanRasterToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.cleanRasterToolStripMenuItem.Text = "Clean Raster...";
            this.cleanRasterToolStripMenuItem.Click += new System.EventHandler(this.cleanRasterToolStripMenuItem_Click);
            // 
            // analysisToolStripMenuItem
            // 
            this.analysisToolStripMenuItem.Name = "analysisToolStripMenuItem";
            this.analysisToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.analysisToolStripMenuItem.Text = "Analysis";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.fISLibraryToolStripMenuItem});
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.customizeToolStripMenuItem.Text = "Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Settings;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // fISLibraryToolStripMenuItem
            // 
            this.fISLibraryToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.FISLibrary;
            this.fISLibraryToolStripMenuItem.Name = "fISLibraryToolStripMenuItem";
            this.fISLibraryToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.fISLibraryToolStripMenuItem.Text = "FIS Library";
            this.fISLibraryToolStripMenuItem.Click += new System.EventHandler(this.fISLibraryToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineGCDHelpToolStripMenuItem,
            this.gCDWebSiteToolStripMenuItem,
            this.aboutGCDToolStripMenuItem});
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // onlineGCDHelpToolStripMenuItem
            // 
            this.onlineGCDHelpToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Help;
            this.onlineGCDHelpToolStripMenuItem.Name = "onlineGCDHelpToolStripMenuItem";
            this.onlineGCDHelpToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.onlineGCDHelpToolStripMenuItem.Text = "Online GCD Help";
            this.onlineGCDHelpToolStripMenuItem.Click += new System.EventHandler(this.onlineGCDHelpToolStripMenuItem_Click);
            // 
            // gCDWebSiteToolStripMenuItem
            // 
            this.gCDWebSiteToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Help;
            this.gCDWebSiteToolStripMenuItem.Name = "gCDWebSiteToolStripMenuItem";
            this.gCDWebSiteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.gCDWebSiteToolStripMenuItem.Text = "GCD Web Site";
            this.gCDWebSiteToolStripMenuItem.Click += new System.EventHandler(this.gCDWebSiteToolStripMenuItem_Click);
            // 
            // aboutGCDToolStripMenuItem
            // 
            this.aboutGCDToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.GCD;
            this.aboutGCDToolStripMenuItem.Name = "aboutGCDToolStripMenuItem";
            this.aboutGCDToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.aboutGCDToolStripMenuItem.Text = "About GCD";
            this.aboutGCDToolStripMenuItem.Click += new System.EventHandler(this.aboutGCDToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssProjectPath});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(907, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssProjectPath
            // 
            this.tssProjectPath.DoubleClickEnabled = true;
            this.tssProjectPath.Name = "tssProjectPath";
            this.tssProjectPath.Size = new System.Drawing.Size(118, 17);
            this.tssProjectPath.Text = "toolStripStatusLabel1";
            this.tssProjectPath.DoubleClick += new System.EventHandler(this.browseGCDProjectFolderToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiNewProject,
            this.tsiOpenProject,
            this.tsiProjectProperties,
            this.tsiBrowseProjectFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(907, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsiNewProject
            // 
            this.tsiNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsiNewProject.Image = global::GCDStandalone.Properties.Resources.NewProject;
            this.tsiNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiNewProject.Name = "tsiNewProject";
            this.tsiNewProject.Size = new System.Drawing.Size(23, 22);
            this.tsiNewProject.Text = "tsiNewProject";
            this.tsiNewProject.Click += new System.EventHandler(this.ProjectProperties_Click);
            // 
            // tsiOpenProject
            // 
            this.tsiOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsiOpenProject.Image = global::GCDStandalone.Properties.Resources.OpenProject;
            this.tsiOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiOpenProject.Name = "tsiOpenProject";
            this.tsiOpenProject.Size = new System.Drawing.Size(23, 22);
            this.tsiOpenProject.Text = "toolStripButton2";
            this.tsiOpenProject.Click += new System.EventHandler(this.openGCDProjectToolStripMenuItem_Click);
            // 
            // tsiProjectProperties
            // 
            this.tsiProjectProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsiProjectProperties.Image = global::GCDStandalone.Properties.Resources.Settings;
            this.tsiProjectProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiProjectProperties.Name = "tsiProjectProperties";
            this.tsiProjectProperties.Size = new System.Drawing.Size(23, 22);
            this.tsiProjectProperties.Text = "toolStripButton2";
            this.tsiProjectProperties.Click += new System.EventHandler(this.ProjectProperties_Click);
            // 
            // tsiBrowseProjectFolder
            // 
            this.tsiBrowseProjectFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsiBrowseProjectFolder.Image = global::GCDStandalone.Properties.Resources.BrowseFolder;
            this.tsiBrowseProjectFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiBrowseProjectFolder.Name = "tsiBrowseProjectFolder";
            this.tsiBrowseProjectFolder.Size = new System.Drawing.Size(23, 22);
            this.tsiBrowseProjectFolder.Text = "toolStripButton2";
            this.tsiBrowseProjectFolder.Click += new System.EventHandler(this.browseGCDProjectFolderToolStripMenuItem_Click);
            // 
            // ucProjectExplorer1
            // 
            this.ucProjectExplorer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucProjectExplorer1.Location = new System.Drawing.Point(3, 3);
            this.ucProjectExplorer1.Name = "ucProjectExplorer1";
            this.ucProjectExplorer1.Size = new System.Drawing.Size(901, 411);
            this.ucProjectExplorer1.TabIndex = 3;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(907, 441);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(907, 488);
            this.toolStripContainer1.TabIndex = 5;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ucProjectExplorer1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 417F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(907, 392);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 488);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataPreparationToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private GCDUserInterface.Project.ucProjectExplorer ucProjectExplorer1;
        private System.Windows.Forms.ToolStripMenuItem newGCDProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGCDProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeGCDProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseGCDProjectFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem onlineGCDHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gCDWebSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutGCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fISLibraryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel tssProjectPath;
        private System.Windows.Forms.ToolStripMenuItem cleanRasterToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsiNewProject;
        private System.Windows.Forms.ToolStripButton tsiOpenProject;
        private System.Windows.Forms.ToolStripButton tsiProjectProperties;
        private System.Windows.Forms.ToolStripButton tsiBrowseProjectFolder;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

