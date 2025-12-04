using System;
using System.IO;
using Microsoft.Win32;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using System.Diagnostics;

namespace GCDViewer.Buttons
{
    internal class HelpButton : Button
    {
        protected override void OnClick()
        {
            try
            {
                Process.Start(new ProcessStartInfo(Properties.Resources.HelpUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Opening Project");
            }
        }
    }
}
