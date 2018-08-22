using System;

namespace GCDAddIn.DataPreparation
{
    class btnCleanRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCDCore.UserInterface.SurveyLibrary.frmImportRaster frm = new GCDCore.UserInterface.SurveyLibrary.frmImportRaster(null, GCDCore.UserInterface.SurveyLibrary.ExtentImporter.Purposes.Standalone, string.Empty);

            //frm.ucRaster.BrowseRaster += BrowseRaster;
            frm.GISBrowseSaveRasterHandler += BrowseSaveRaster;

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
                GCDCore.GCDException.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        //private void BrowseRaster(object sender, naru.ui.PathEventArgs e)
        //{
        //    System.IO.DirectoryInfo diWorkspace = ArcMapUtilities.GetWorkspacePath(e.Path.FullName);
        //    string sDataset = System.IO.Path.GetFileNameWithoutExtension(e.Path.FullName);
        //    GCDConsoleLib.Raster selectedRaster = ArcMapBrowse.BrowseOpenRaster(e.FormTitle, diWorkspace, sDataset, e.hWndParent);
        //    if (!(selectedRaster == null))
        //    {
        //        ((System.Windows.Forms.TextBox)sender).Text = selectedRaster.GISFileInfo.FullName;
        //    }
        //}

        private void BrowseSaveRaster(System.Windows.Forms.TextBox txt, string formTitle, IntPtr hParentWindowHandle)
        {
            string result = ArcMapBrowse.BrowseSaveRaster(formTitle, hParentWindowHandle);
            if (!string.IsNullOrEmpty(result))
                txt.Text = result;
        }    

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
