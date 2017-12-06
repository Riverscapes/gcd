using System;
using System.Windows.Forms;

namespace GCDAddIn.Project
{
    class btnOpenProject : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
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

                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        GCDCore.Project.ProjectManager.OpenProject(new System.IO.FileInfo(f.FileName));
                        btnProjectExplorer.OpenProjectExplorer();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error reading the GCD project file '{0}'. Ensure that the file is a valid GCD project file with valid and complete XML contents.", f.FileName), GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}