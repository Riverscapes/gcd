using System;

namespace GCDAddIn.Customize
{
    public class btnOptions : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                GCDCore.UserInterface.Options.frmOptions frm = new GCDCore.UserInterface.Options.frmOptions();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }
    }
}
