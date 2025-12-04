using System;
using System.IO;
using Microsoft.Win32;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using System.Diagnostics;

namespace GCDViewer.Buttons
{
    internal class AboutButton : Button
    {
        protected override void OnClick()
        {
            try
            {
                var aboutWindow = new AboutWindow();
                aboutWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Opening About Dialog");
            }
        }
    }
}
