using System;

namespace GCDAddIn.DataPreparation
{
    class btnCleanRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCDUserInterface.SurveyLibrary.frmImportRaster frm = new GCDUserInterface.SurveyLibrary.frmImportRaster();

            frm.ucRaster.BrowseRaster += BrowseRaster;
            frm.ucRaster.SelectRasterFromArcMap += SelectRasterFromArcMap;

            try
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    GCDConsoleLib.Raster gOutput = frm.ProcessRaster();
                    if (gOutput is GCDConsoleLib.Raster)
                    {
                        if (GCDCore.Properties.Settings.Default.AddOutputLayersToMap)
                        {
                            ArcMapUtilities.AddToMap(new System.IO.FileInfo(gOutput.GISFileInfo.FullName));
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
            GCDConsoleLib.Raster selectedRaster = ArcMapBrowse.BrowseOpenRaster(e.FormTitle, ref diWorkspace, sDataset);
            if (!(selectedRaster == null))
            {
                ((System.Windows.Forms.TextBox)sender).Text = selectedRaster.GISFileInfo.FullName;
            }
        }

        private void SelectRasterFromArcMap(object sender, naru.ui.PathEventArgs e)
        {
            try
            {
                frmLayerSelector frm = new frmLayerSelector( ArcMapBrowse.BrowseGISTypes.Raster);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ((System.Windows.Forms.TextBox)sender).Text = frm.SelectedLayer.GISFileInfo.FullName;
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
