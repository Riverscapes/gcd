using System;

namespace GCDAddIn.Help
{
    public class btnWebSite : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                System.Diagnostics.Process.Start(GCDCore.Properties.Resources.GCDWebSiteURL);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }
    }
}
