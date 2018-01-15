using System;

namespace GCDAddIn.Customize
{
    public class btnFISLibrary : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCDCore.UserInterface.FISLibrary.frmFISLibrary frm = new GCDCore.UserInterface.FISLibrary.frmFISLibrary();
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }
    }
}
