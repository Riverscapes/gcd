using System;
using ArcGIS.Desktop.Framework.Contracts;


namespace GCDViewer.Buttons
{
    internal class ReloadProjectButton : Button
    {
        protected override void OnClick()
        {
            try
            {

                ProjectExplorerDockpaneViewModel.LoadProject();
            }
            catch (Exception ex)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(ex.Message, "Error reloading GCD Project");
            }
        }
    }
}
