using System;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;

namespace GCDAddIn
{
    public struct ArcMapBrowse
    {
        public enum BrowseGISTypes
        {
            Point,
            Line,
            Polygon,
            Raster,
            TIN,
            Any
        };

        public enum GISDataStorageTypes
        {
            RasterFile,
            ShapeFile,
            FileGeodatase,
            CAD,
            PersonalGeodatabase,
            TIN
        };

        public static GCDConsoleLib.Raster BrowseOpenRaster(string sFormTitle, ref System.IO.DirectoryInfo diWorkspace, string sName = "")
        {
            IGxDialog pGxDialog = new GxDialog();
            IGxObjectFilter pRasterFilter = new GxFilterRasterDatasets();
            IGxObjectFilterCollection pFilterCol = (IGxObjectFilterCollection)pGxDialog;
            pFilterCol.AddFilter(pRasterFilter, true);
            pGxDialog.RememberLocation = true;
            pGxDialog.AllowMultiSelect = false;
            pGxDialog.Title = sFormTitle;

            IEnumGxObject pEnumGx = null;
            IGxObject pGxObject = null;

            if (diWorkspace is System.IO.DirectoryInfo && diWorkspace.Exists)
            {
                pGxDialog.set_StartingLocation(diWorkspace.FullName);
            }

            GCDConsoleLib.Raster rResult = null;
            try
            {
                if (pGxDialog.DoModalOpen(0, out pEnumGx))
                {
                    pGxObject = pEnumGx.Next();
                    System.IO.FileInfo sFile = new System.IO.FileInfo(pGxObject.FullName);
                    sName = pGxObject.Name;
                    diWorkspace = sFile.Directory;

                    rResult = new GCDConsoleLib.Raster(new System.IO.FileInfo(pGxObject.FullName));
                }
            }
            catch (Exception ex)
            {
                ex.Data["Title"] = sFormTitle;
                ex.Data["Name"] = sName;
                throw;
            }

            return rResult;
        }
    }
}
