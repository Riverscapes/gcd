using System;

namespace GCDAddIn.DataPreparation
{
    class btnCleanRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCD.GCDLib.UI.SurveyLibrary.frmImportRaster frm = new GCD.GCDLib.UI.SurveyLibrary.frmImportRaster();

            frm.ucRaster.BrowseRaster += BrowseRaster;
            frm.ucRaster.SelectRasterFromArcMap += SelectRasterFromArcMap;

            try
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                   GCD. GCD.GCDLib.GCDConsoleLib.Raster gOutput = frm.ProcessRaster();
                    if (gOutput is GCD.GCDLib.GCDConsoleLib.Raster)
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

        private void BrowseRaster(object sender, naru.ui.PathEventArgs e)
        {
            System.IO.DirectoryInfo diWorkspace = ArcMapUtilities.GetWorkspacePath(e.Path.FullName);
            string sDataset = System.IO.Path.GetFileNameWithoutExtension(e.Path.FullName);
          gcd  GCDConsoleLib.Raster selectedRaster = ArcMapBrowse.BrowseOpenRaster(e.FormTitle, ref diWorkspace, sDataset);
            if (!(selectedRaster == null))
            {
                ((System.Windows.Forms.TextBox)sender).Text = selectedRaster.FullPath;
            }
        }

        private void SelectRasterFromArcMap(object sender, naru.ui.PathEventArgs e)
        {
            try
            {
                frmLayerSelector frm = new frmLayerSelector(GCD.GCDLib.Core.GISDataStructures.BrowseGISTypes.Raster);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ((System.Windows.Forms.TextBox)sender).Text = frm.SelectedLayer.FullPath;
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
