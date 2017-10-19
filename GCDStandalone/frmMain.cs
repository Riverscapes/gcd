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
        }

        private void newGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDLib.UI.Project.frmProjectProperties frm = new GCDLib.UI.Project.frmProjectProperties(GCDLib.UI.Project.frmProjectProperties.DisplayModes.Create);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ucProjectExplorer1.cmdRefresh_Click(sender, e);
                }
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
        }

        private void projectPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void browseGCDProjectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //[System.Runtime.InteropServices.DllImport("RasterManagerGCD.dll")]
        //public static extern void RegisterGDAL();

        //[System.Runtime.InteropServices.DllImport("RasterManagerGCD.dll")]
        //public static extern int GetRasterProperties(string sFullPath, ref double fCellHeight, ref double fCellWidth, ref double fLeft, ref double fTop, ref int nRows, ref int nCols, ref double fNoData, ref int nHasNoData, ref int nDataType,
        //System.Text.StringBuilder sUnits, System.Text.StringBuilder sSpatialReference, System.Text.StringBuilder sError);
    }
}
