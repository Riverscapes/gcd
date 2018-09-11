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
            System.Windows.Forms.MessageBox.Show("GCD Extension Start Up", "Diagnostic Message");

            try
            {
                GCDCore.Project.ProjectManager.Init(GCDCore.Properties.Settings.Default.AutomaticPyramids);
                GCDCore.Project.ProjectManager.OnProgressChange += OnProgressChange;
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex, "Error setting up application files.");
            }

            System.Windows.Forms.MessageBox.Show("GCD Extension Start Up Has Completed", "Diagnostic Message");
        }

        protected override void OnShutdown()
        {

        }

        private void OnProgressChange(object sender, GCDConsoleLib.OpStatus opStatus)
        {
            switch (opStatus.State)
            {
                case GCDConsoleLib.OpStatus.States.Initialized:
                    ArcMap.Application.StatusBar.ShowProgressBar(opStatus.Message, 0, 100, 1, true);
                    break;

                case GCDConsoleLib.OpStatus.States.Started:
                    ArcMap.Application.StatusBar.ProgressBar.Position = opStatus.Progress;
                    break;

                default:
                    ArcMap.Application.StatusBar.HideProgressBar();
                    break;
            }
        }
    }
}
