using System;
using static GCD.GCDLib.UI.UtilityForms.InputUCSelectedItemChangedEventArgs;

namespace GCDAddIn.DataPreparation
{
    class btnCleanRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCD.GCDLib.UI.SurveyLibrary.frmImportRaster frm = new GCD.GCDLib.UI.SurveyLibrary.frmImportRaster();

            frm.ucRaster.BrowseRaster += BrowseRaster;

            try
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    GCD.GCDLib.Core.GISDataStructures.Raster gOutput = frm.ProcessRaster();
                    if (gOutput is GCD.GCDLib.Core.GISDataStructures.Raster)
                    {
                        if (GCD.GCDLib.My.MySettings.Default.AddOutputLayersToMap)
                        {
                            ArcMapUtilities.AddToMap(gOutput.FilePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        private void BrowseRaster(object sender, BrowseLayerEventArgs e)
        {
            System.IO.DirectoryInfo diWorkspace = ArcMapUtilities.GetWorkspacePath(e.ExistingPath);
            string sDataset = System.IO.Path.GetFileNameWithoutExtension(e.ExistingPath);
            GCD.GCDLib.Core.GISDataStructures.Raster selectedRaster = ArcMapBrowse.BrowseOpenRaster(e.FormTitle, ref diWorkspace, sDataset);
            if (!(selectedRaster == null))
            {
                ((System.Windows.Forms.TextBox)sender).Text = selectedRaster.FullPath;
            }
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
