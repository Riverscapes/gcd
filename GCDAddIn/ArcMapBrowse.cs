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

        public static GCDConsoleLib.Raster BrowseOpenRaster(string sFormTitle, System.IO.DirectoryInfo diWorkspace, string sName, IntPtr hParentWindowHandle)
        {
            IGxDialog pGxDialog = new GxDialogClass();
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
                if (pGxDialog.DoModalOpen(hParentWindowHandle.ToInt32(), out pEnumGx))
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

        public static GCDConsoleLib.Vector BrowseOpenVector(string formTitle, System.IO.DirectoryInfo diWorkspace, string sFCName, BrowseGISTypes eType, IntPtr hParentWindowHandle)
        {
            IGxDialog pGxDialog = new GxDialogClass();
            IGxObjectFilterCollection pFilterCol = (IGxObjectFilterCollection)pGxDialog;
            switch (eType)
            {
                case BrowseGISTypes.Point: pFilterCol.AddFilter(new GxFilterPointFeatureClasses(), true); break;
                case BrowseGISTypes.Line: pFilterCol.AddFilter(new GxFilterPolylineFeatureClasses(), true); break;
                case BrowseGISTypes.Polygon: pFilterCol.AddFilter(new GxFilterPolygonFeatureClasses(), true); break;
                default: pFilterCol.AddFilter(new GxFilterFeatureClasses(), true); break;
            }

            IEnumGxObject pEnumGx = null;
            IGxObject pGxObject = null;
            pGxDialog.RememberLocation = true;
            pGxDialog.AllowMultiSelect = false;
            pGxDialog.Title = formTitle;
            pGxDialog.ButtonCaption = "Select";
            if (diWorkspace != null && diWorkspace.Exists)
            {
                object existingDirectory = diWorkspace.FullName;
                pGxDialog.set_StartingLocation(ref existingDirectory);
            }

            pGxDialog.Name = sFCName;
            GCDConsoleLib.Vector gResult = null;
            try
            {
                if (pGxDialog.DoModalOpen(hParentWindowHandle.ToInt32(), out pEnumGx))
                {
                    pGxObject = pEnumGx.Next();
                    sFCName = pGxObject.BaseName;
                    gResult = new GCDConsoleLib.Vector(new System.IO.FileInfo(pGxObject.FullName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error attempting to browse for vector GIS data source", ex);
            }

            return gResult;
        }
    }
}
