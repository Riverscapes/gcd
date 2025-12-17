using System;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using System.Diagnostics;
using ArcGIS.Desktop.Framework;

namespace GCDViewer.Buttons
{
    internal class DataExchangeButton : Button
    {
        protected override void OnClick()
        {
            try
            {
                ProjectExplorerDockpaneViewModel.VisitDataExchange();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Opening About Dialog");
            }
        }
    }
}
