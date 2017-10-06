using System;

namespace GCDAddIn.DataPreparation
{
    class btnCleanRaster : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            ImportRasterForm frm = new ImportRasterForm(My.ArcMap.Application, null, null, ImportRasterForm.ImportRasterPurposes.StandaloneTool, "Raster");
            try
            {
                if (frm.ShowDialog() == Windows.Forms.DialogResult.OK)
                {
                    GISDataStructures.RasterDirect gOutput = frm.ProcessRaster;
                    if (gOutput is GISDataStructures.RasterDirect)
                    {
                        if (My.Settings.AddOutputLayersToMap)
                        {
                            gOutput.AddToMap(My.ThisApplication);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUI.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
