using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDStandalone
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = GCDLib.My.Resources.Resources.ApplicationNameLong;

            try
            {
                GCDLib.Core.WorkspaceManager.Initialize();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error initializing temporary workspace.");
            }

            try
            {
                GCDLib.Core.GCDProject.ProjectManagerUI.CopyDeployFolder();
                new GCDLib.Core.GCDProject.ProjectManagerUI(GCDConsoleLib.Raster.RasterDriver.GTiff, "true");
            }
            catch(Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error setting up application files.");
            }

            ucProjectExplorer1.ProjectTreeNodeSelectionChange += UpdateMenusAndToolstrips;
            UpdateMenusAndToolstrips(sender, e);
        }

        private void ProjectProperties_Click(object sender, EventArgs e)
        {
            try
            {
                bool bEditMode = string.Compare(((ToolStripItem)sender).Name, "projectPropertiesToolStripMenuItem", true) == 0 ||
                        string.Compare(((ToolStripItem)sender).Name, "tsiProjectProperties", true) == 0;

                GCDLib.UI.Project.frmProjectProperties frm = new GCDLib.UI.Project.frmProjectProperties(!bEditMode);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ucProjectExplorer1.cmdRefresh_Click(sender, e);
                }

                UpdateMenusAndToolstrips(sender, e);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void openGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.DefaultExt = "xml";
            f.Filter = "GCD Project Files (*.gcd)|*.gcd";
            f.Title = "Open Existing GCD Project";
            f.CheckFileExists = true;
            //
            // PGB 2 May 2011 - Use the last browsed folder for project files. Note that
            // this is stored in a user setting and does not rely on the FileDialog to 
            // remember this value because the FileDialog may have been used for other purposes.
            if (!string.IsNullOrEmpty(GCDLib.My.MySettings.Default.LastUsedProjectFolder) && System.IO.Directory.Exists(GCDLib.My.MySettings.Default.LastUsedProjectFolder))
            {
                f.InitialDirectory = GCDLib.My.MySettings.Default.LastUsedProjectFolder;

                // Try and find the last used project in the folder
                string[] fis = System.IO.Directory.GetFiles(GCDLib.My.MySettings.Default.LastUsedProjectFolder, "*.gcd", System.IO.SearchOption.TopDirectoryOnly);
                if (fis.Count<string>() > 0)
                {
                    f.FileName = System.IO.Path.GetFileName(fis[0]);
                }
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Set the project file path first (which will attempt to read the XML file and throw an error if anything goes wrong)
                    // Then set the settings if the read was successful.
                    GCDLib.Core.GCDProject.ProjectManagerBase.FilePath = f.FileName;
                    GCDLib.My.MySettings.Default.LastUsedProjectFolder = System.IO.Path.GetDirectoryName(f.FileName);
                    GCDLib.My.MySettings.Default.Save();

                    try
                    {
                        GCDLib.Core.GCDProject.ProjectManagerUI.Validate();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error validating GCD project", ex);
                    }

                    ucProjectExplorer1.cmdRefresh_Click(sender, e);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error reading the GCD project file '{0}'. Ensure that the file is a valid GCD project file with valid and complete XML contents.", f.FileName), GCDLib.My.Resources.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            UpdateMenusAndToolstrips(sender, e);
        }

        private void browseGCDProjectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GCDLib.Core.GCDProject.ProjectManagerBase.FilePath) && System.IO.File.Exists(GCDLib.Core.GCDProject.ProjectManagerBase.FilePath))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(GCDLib.Core.GCDProject.ProjectManagerBase.FilePath));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateMenusAndToolstrips(object sender, EventArgs e)
        {
            UpdateMenuItemStatus(toolStrip1.Items);
            UpdateMenuItemStatus(menuStrip1.Items);
        }

        private void UpdateMenuItemStatus(ToolStripItemCollection aMenu)
        {
            foreach (ToolStripItem subMenu in aMenu)
            {
                // Skip over separators etc
                if (!(subMenu is ToolStripMenuItem || subMenu is ToolStripButton))
                    continue;

                if ((subMenu is ToolStripMenuItem && ((ToolStripMenuItem)subMenu).HasDropDownItems)) // if subMenu has children
                {
                    switch (subMenu.Name)
                    {
                        // Skip top level menus
                        case "analysisToolStripMenuItem":
                        case "dataPreparationToolStripMenuItem":
                        case "customizeToolStripMenuItem":
                        case "helpToolStripMenuItem1":
                            return;

                        default:
                            UpdateMenuItemStatus(((ToolStripMenuItem)subMenu).DropDownItems); // Call recursive Method.
                            break;
                    }
                }
                else
                {
                    switch (subMenu.Name)
                    {
                        // Skip specific menu items here
                        case "newGCDProjectToolStripMenuItem":
                        case "openGCDProjectToolStripMenuItem":
                        case "exitToolStripMenuItem":
                        case "customizeToolStripMenuItem":
                            break; // do nothing. Always enabled.

                        // Skip specific tool strip items here
                        case "tsiNewProject":
                        case "tsiOpenProject":
                            break;

                        default:
                            subMenu.Enabled = !string.IsNullOrEmpty(GCDLib.Core.GCDProject.ProjectManagerBase.FilePath);
                            break;
                    }
                }
            }

            // Now update the tool status strip
            tssProjectPath.Text = GCDLib.Core.GCDProject.ProjectManagerBase.FilePath;
        }

        private void onlineGCDHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDLib.My.Resources.Resources.HelpBaseURL);
        }

        private void gCDWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDLib.My.Resources.Resources.GCDWebSiteURL);
        }

        private void aboutGCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.UI.About.frmAbout frm = new GCDLib.UI.About.frmAbout();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.UI.Options.frmOptions frm = new GCDLib.UI.Options.frmOptions();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void cleanRasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.UI.SurveyLibrary.frmImportRaster frm = new GCDLib.UI.SurveyLibrary.frmImportRaster();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void fISLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.UI.FISLibrary.frmFISLibrary frm = new GCDLib.UI.FISLibrary.frmFISLibrary();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private void closeGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.Core.GCDProject.ProjectManagerBase.FilePath = string.Empty;
                ucProjectExplorer1.cmdRefresh_Click(sender, e);
                UpdateMenusAndToolstrips(sender, e);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}