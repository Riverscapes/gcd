using System;

namespace GCDAddIn.DataPreparation
{
    class btnCleanRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            GCD.GCDLib.UI.SurveyLibrary.frmImportRaster frm = new GCD.GCDLib.UI.SurveyLibrary.frmImportRaster(null, null, GCD.GCDLib.UI.SurveyLibrary.frmImportRaster.ImportRasterPurposes.StandaloneTool, "Raster");
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

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
