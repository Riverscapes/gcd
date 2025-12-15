using System;
using System.IO;
using Microsoft.Win32;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;

namespace GCDViewer.Buttons
{
    internal class CloseProjectButton : Button
    {
        protected override void OnClick()
        {
            try
            {
                ProjectExplorerDockpaneViewModel.CloseAllProjects();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Opening Project");
            }
        }
    }
}
