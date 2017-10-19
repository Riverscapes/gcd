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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeGCDProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataPreparationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ucProjectExplorer1 = new GCDLib.UI.Project.ucProjectExplorer();
            this.analysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newGCDProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGCDProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseGCDProjectFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineGCDHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCDWebSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutGCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fISLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.dataPreparationToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.analysisToolStripMenuItem,
            this.customizeToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(541, 24);
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
            this.browseGCDProjectFolderToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // closeGCDProjectToolStripMenuItem
            // 
            this.closeGCDProjectToolStripMenuItem.Name = "closeGCDProjectToolStripMenuItem";
            this.closeGCDProjectToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.closeGCDProjectToolStripMenuItem.Text = "Close GCD Project";
            // 
            // dataPreparationToolStripMenuItem
            // 
            this.dataPreparationToolStripMenuItem.Name = "dataPreparationToolStripMenuItem";
            this.dataPreparationToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.dataPreparationToolStripMenuItem.Text = "Data Preparation";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(541, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ucProjectExplorer1
            // 
            this.ucProjectExplorer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucProjectExplorer1.Location = new System.Drawing.Point(0, 24);
            this.ucProjectExplorer1.Name = "ucProjectExplorer1";
            this.ucProjectExplorer1.Size = new System.Drawing.Size(541, 215);
            this.ucProjectExplorer1.TabIndex = 3;
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
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "Options";
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
            // newGCDProjectToolStripMenuItem
            // 
            this.newGCDProjectToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.NewProject;
            this.newGCDProjectToolStripMenuItem.Name = "newGCDProjectToolStripMenuItem";
            this.newGCDProjectToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.newGCDProjectToolStripMenuItem.Text = "New GCD Project...";
            this.newGCDProjectToolStripMenuItem.Click += new System.EventHandler(this.newGCDProjectToolStripMenuItem_Click);
            // 
            // openGCDProjectToolStripMenuItem
            // 
            this.openGCDProjectToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.OpenProject;
            this.openGCDProjectToolStripMenuItem.Name = "openGCDProjectToolStripMenuItem";
            this.openGCDProjectToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.openGCDProjectToolStripMenuItem.Text = "Open GCD Project...";
            this.openGCDProjectToolStripMenuItem.Click += new System.EventHandler(this.openGCDProjectToolStripMenuItem_Click);
            // 
            // projectPropertiesToolStripMenuItem
            // 
            this.projectPropertiesToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Settings;
            this.projectPropertiesToolStripMenuItem.Name = "projectPropertiesToolStripMenuItem";
            this.projectPropertiesToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.projectPropertiesToolStripMenuItem.Text = "Project Properties";
            this.projectPropertiesToolStripMenuItem.Click += new System.EventHandler(this.projectPropertiesToolStripMenuItem_Click);
            // 
            // browseGCDProjectFolderToolStripMenuItem
            // 
            this.browseGCDProjectFolderToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.BrowseFolder;
            this.browseGCDProjectFolderToolStripMenuItem.Name = "browseGCDProjectFolderToolStripMenuItem";
            this.browseGCDProjectFolderToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.browseGCDProjectFolderToolStripMenuItem.Text = "Browse GCD Project Folder";
            this.browseGCDProjectFolderToolStripMenuItem.Click += new System.EventHandler(this.browseGCDProjectFolderToolStripMenuItem_Click);
            // 
            // onlineGCDHelpToolStripMenuItem
            // 
            this.onlineGCDHelpToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Help;
            this.onlineGCDHelpToolStripMenuItem.Name = "onlineGCDHelpToolStripMenuItem";
            this.onlineGCDHelpToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.onlineGCDHelpToolStripMenuItem.Text = "Online GCD Help";
            // 
            // gCDWebSiteToolStripMenuItem
            // 
            this.gCDWebSiteToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.Help;
            this.gCDWebSiteToolStripMenuItem.Name = "gCDWebSiteToolStripMenuItem";
            this.gCDWebSiteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.gCDWebSiteToolStripMenuItem.Text = "GCD Web Site";
            // 
            // aboutGCDToolStripMenuItem
            // 
            this.aboutGCDToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.GCD;
            this.aboutGCDToolStripMenuItem.Name = "aboutGCDToolStripMenuItem";
            this.aboutGCDToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.aboutGCDToolStripMenuItem.Text = "About GCD";
            // 
            // fISLibraryToolStripMenuItem
            // 
            this.fISLibraryToolStripMenuItem.Image = global::GCDStandalone.Properties.Resources.FISLibrary;
            this.fISLibraryToolStripMenuItem.Name = "fISLibraryToolStripMenuItem";
            this.fISLibraryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fISLibraryToolStripMenuItem.Text = "FIS Library";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 261);
            this.Controls.Add(this.ucProjectExplorer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataPreparationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private GCDLib.UI.Project.ucProjectExplorer ucProjectExplorer1;
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
    }
}

