using System;

namespace GCDAddIn.Help
{
    class btnTATWebSite : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                System.Diagnostics.Process.Start(GCDCore.Properties.Resources.TATWebSite);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }
    }
}
