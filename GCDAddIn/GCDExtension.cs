using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GCDAddIn
{
    public class GCDExtension : ESRI.ArcGIS.Desktop.AddIns.Extension
    {
        public GCDExtension()
        {
        }

        protected override void OnStartup()
        {
            try
            {
                GCDCore.Project.ProjectManager.Init(GCDCore.Properties.Settings.Default.AutomaticPyramids);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex, "Error setting up application files.");
            }
        }

        protected override void OnShutdown()
        {

        }
    }
}
