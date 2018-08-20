using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.esriSystem;

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
                GCDCore.Project.ProjectManager.OnProgressChange += OnProgressChange;
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex, "Error setting up application files.");
            }
        }

        protected override void OnShutdown()
        {

        }

        private void OnProgressChange(object sender, int prog)
        {
            ArcMap.Application.StatusBar.ShowProgressBar("Point Density", 0, 100, 1, true);
            ArcMap.Application.StatusBar.ProgressBar.Position = prog;
        }
    }
}
