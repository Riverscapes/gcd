using System;
using System.Windows.Forms;

namespace GCDAddIn.Project
{
    class btnOpenProject : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            MessageBox.Show("Open Project 1", "Diagnostic Message");

            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.DefaultExt = "xml";
                f.Filter = "GCD Project Files (*.gcd)|*.gcd";
                f.Title = "Open Existing GCD Project";
                f.CheckFileExists = true;

                if (!string.IsNullOrEmpty(GCDCore.Properties.Settings.Default.LastUsedProjectFolder) && System.IO.Directory.Exists(GCDCore.Properties.Settings.Default.LastUsedProjectFolder))
                {
                    f.InitialDirectory = GCDCore.Properties.Settings.Default.LastUsedProjectFolder;

                    // Try and find the last used project in the folder
                    string[] fis = System.IO.Directory.GetFiles(GCDCore.Properties.Settings.Default.LastUsedProjectFolder, "*.gcd", System.IO.SearchOption.TopDirectoryOnly);
                    if (fis.Length > 0)
                    {
                        f.FileName = System.IO.Path.GetFileName(fis[0]);
                    }
                }

                MessageBox.Show("Open Project 2", "Diagnostic Message");


                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MessageBox.Show("Open Project 3", "Diagnostic Message");

                        GCDCore.Project.ProjectManager.OpenProject(new System.IO.FileInfo(f.FileName));
                        GCDCore.Properties.Settings.Default.LastUsedProjectFolder = System.IO.Path.GetDirectoryName(f.FileName);
                        GCDCore.Properties.Settings.Default.Save();

                        MessageBox.Show("Open Project 4", "Diagnostic Message");

                        btnProjectExplorer.ShowProjectExplorer(true);

                        MessageBox.Show("Open Project 5", "Diagnostic Message");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error reading the GCD project file '{0}'. Ensure that the file is a valid GCD project file with valid and complete XML contents.\n\n{1}", f.FileName, ex.Message), GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}