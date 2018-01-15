using System;

namespace GCDAddIn.Project
{
    public class btnProjectProperties : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCDCore.UserInterface.Project.frmProjectProperties frm = new GCDCore.UserInterface.Project.frmProjectProperties(false);
            try
            {
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        protected override void OnUpdate()
        {
            Enabled = GCDCore.Project.ProjectManager.Project != null;
        }
    }
}
