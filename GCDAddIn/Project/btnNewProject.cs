using System;

namespace GCDAddIn.Project
{
    class btnNewProject : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                GCDCore.UserInterface.Project.frmProjectProperties frm = new GCDCore.UserInterface.Project.frmProjectProperties(true);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    btnProjectExplorer.ShowProjectExplorer(true);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}