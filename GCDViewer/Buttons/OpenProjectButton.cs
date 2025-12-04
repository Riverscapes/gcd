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
                string folder = System.IO.Path.Combine(Environment.GetEnvironmentVariable("APPDATA"));//, Properties.Resources.AppDataFolder);
                if (!Directory.Exists(folder))
                {
                    MessageBox.Show("There are no Riverscapes Viewer resources on this computer. Riverscapes Viewer requires symbology and business logic resources before projects can be opened." +
                        " Use the 'Update Resources' button on the Riverscapes Viewer Toolbar to download the latest resources and then try to open a project again.", "No Resources Found",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                OpenFileDialog f = new OpenFileDialog();
                f.DefaultExt = "xml";
                f.Filter = "Riverscapes Project Files (*.rs.xml)|*.rs.xml";
                f.Title = "Open Existing Riverscapes Project";
                f.CheckFileExists = true;

                //if (!string.IsNullOrEmpty(Properties.Settings.Default.LastUsedProjectFolder) && Directory.Exists(Properties.Settings.Default.LastUsedProjectFolder))
                //{
                //    f.InitialDirectory = Properties.Settings.Default.LastUsedProjectFolder;

                //    // Try and find the last used project in the folder
                //    string[] fis = Directory.GetFiles(Properties.Settings.Default.LastUsedProjectFolder, "*.rs.xml", System.IO.SearchOption.TopDirectoryOnly);
                //    if (fis.Length > 0)
                //    {
                //        f.FileName = System.IO.Path.GetFileName(fis[0]);
                //    }
                //}

                //if (f.ShowDialog() == true)
                //{
                //    try
                //    {
                //        ProjectExplorerDockpaneViewModel.LoadProject(f.FileName);

                //        Properties.Settings.Default.LastUsedProjectFolder = Path.GetDirectoryName(f.FileName);
                //        Properties.Settings.Default.Save();

                //        // This will cause the project tree to reload all open projects
                //        ProjectExplorerDockpaneViewModel.Show();
                //    }
                //    catch (FileLoadException exFile)
                //    {
                //        MessageBox.Show(exFile.Message, "Invalid Business Logic File", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                //    }
                //    catch (Exception ex)
                //    {
                //        if (ex.Data.Contains("ErrorCode") && ex.Data["ErrorCode"].ToString() == ProjectTree.RaveProject.MISSING_BL_ERR_CODE)
                //        {
                //            MessageBox.Show("No business logic file could be found. Use the Update Resources button on the Riverscapes Viewer toolbar ribbon to ensure that you have the latest business logic and symbology files.", Properties.Resources.ApplicationNameLong, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                //        }
                //        else
                //        {
                //            MessageBox.Show(string.Format("Error reading the project file '{0}'. Ensure that the file is a valid project file with valid and complete XML contents.\n\n{1}", f.FileName, ex.Message), Properties.Resources.ApplicationNameLong, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Opening Project");
            }
        }
    }
}
