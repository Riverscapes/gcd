using System;

namespace GCDAddIn.Project
{
    public class btnCloseProject : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                GCDCore.Project.ProjectManager.CloseCurrentProject();

                btnProjectExplorer.ShowProjectExplorer(false);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }
        
        protected override void OnUpdate()
        {
            Enabled = GCDCore.Project.ProjectManager.Project != null;
        }
    }
}
