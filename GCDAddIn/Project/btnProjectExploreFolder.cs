using System;

namespace GCDAddIn.Project
{
    class btnProjectExploreFolder : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                System.Diagnostics.Process.Start(GCDCore.Project.ProjectManager.Project.Folder.FullName);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        protected override void OnUpdate()
        {
            Enabled = GCDCore.Project.ProjectManager.Project != null;
        }
    }
}
