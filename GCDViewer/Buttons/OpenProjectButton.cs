using System;
using System.IO;
using Microsoft.Win32;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;

namespace GCDViewer.Buttons
{
    internal class OpenProjectButton : Button
    {
        protected override void OnClick()
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.DefaultExt = "gcd";
                f.Filter = "GCD Project Files (*.gcd)|*.gcd";
                f.Title = "Open Existing GCD Project";
                f.CheckFileExists = true;

                if (!string.IsNullOrEmpty(GCDViewer.Properties.Settings.Default.LastUsedProjectFolder) && Directory.Exists(Properties.Settings.Default.LastUsedProjectFolder))
                {
                    f.InitialDirectory = Properties.Settings.Default.LastUsedProjectFolder;

                    // Try and find the last used project in the folder
                    string[] fis = Directory.GetFiles(Properties.Settings.Default.LastUsedProjectFolder, "*.rs.xml", System.IO.SearchOption.TopDirectoryOnly);
                    if (fis.Length > 0)
                    {
                        f.FileName = System.IO.Path.GetFileName(fis[0]);
                    }
                }

                if (f.ShowDialog() == true)
                {
                    try
                    {
                        ProjectExplorerDockpaneViewModel.LoadProject(f.FileName);

                        Properties.Settings.Default.LastUsedProjectFolder = Path.GetDirectoryName(f.FileName);
                        Properties.Settings.Default.Save();

                        // This will cause the project tree to reload all open projects
                        ProjectExplorerDockpaneViewModel.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error reading the project file '{0}'. Ensure that the file is a valid project file with valid and complete XML contents.\n\n{1}", f.FileName, ex.Message), Properties.Resources.ApplicationNameLong, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Opening Project");
            }
        }
    }
}
