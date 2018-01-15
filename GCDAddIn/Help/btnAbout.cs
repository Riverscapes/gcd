using System;

namespace GCDAddIn.Help
{
    public class btnAbout : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                GCDCore.UserInterface.About.frmAbout frm = new GCDCore.UserInterface.About.frmAbout();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }
    }
}
