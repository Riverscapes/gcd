using System;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesRaster;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace GCDAddIn
{
    public struct ArcMapUtilities
    {
        public enum eEsriLayerTypes
        {
            Esri_DataLayer, //{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}
            Esri_GeoFeatureLayer, //{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}
            Esri_GraphicsLayer, //{34B2EF81-F4AC-11D1-A245-080009B6F22B}
            Esri_FDOGraphicsLayer, //{5CEAE408-4C0A-437F-9DB3-054D83919850}
            Esri_CoverageAnnotationLayer, //{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E}
            Esri_GroupLayer, //{EDAD6644-1810-11D1-86AE-0000F8751720}
            Esri_AnyLayer
        }

        public static IRasterDataset GetRasterDataset(GCDConsoleLib.Raster raster)
        {
            IWorkspace pWorkspace = GetWorkspace(raster.GISFileInfo);
            IRasterDataset pRDS = ((IRasterWorkspace)pWorkspace).OpenRasterDataset(raster.GISFileInfo.Name);

            int refsLeft = 0;
            do
            {
                refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspace);
            }
            while (refsLeft > 0);

            return pRDS;
        }

        public static ILayer AddToMap(FileSystemInfo fiFullPath, string sLayerName = "", string sGroupLayer = "", FileInfo fiSymbologyLayerFile = null, bool bAddToMapIfPresent = false)
        {
            if (!fiFullPath.Exists)
                return null;

            // Only add if it doesn't exist already
            ILayer pResultLayer = GetLayerBySource(fiFullPath);
            if (pResultLayer is ILayer && !bAddToMapIfPresent)
                return pResultLayer;

            // Confirm that the symbology layer file exists
            if (fiSymbologyLayerFile != null && !fiSymbologyLayerFile.Exists)
            {
                Exception ex = new Exception("A symbology layer file was provided, but the file does not exist");
                ex.Data["Data Source"] = fiFullPath.FullName;
                ex.Data["Layer file"] = fiSymbologyLayerFile.FullName;
                throw ex;
            }

            ArcMapBrowse.GISDataStorageTypes eStorageType = GetWorkspaceType(fiFullPath.FullName);
            IWorkspace pWorkspace = GetWorkspace(fiFullPath);

            switch (eStorageType)
            {
                case ArcMapBrowse.GISDataStorageTypes.RasterFile:
                    IRasterDataset pRDS = ((IRasterWorkspace)pWorkspace).OpenRasterDataset(fiFullPath.Name);
                    IRasterLayer pRLResult = new RasterLayer();
                    pRLResult.CreateFromDataset(pRDS);
                    break;

                case ArcMapBrowse.GISDataStorageTypes.CAD:
                    string sFile = Path.GetFileName(Path.GetDirectoryName(fiFullPath.FullName));
                    string sFC = sFile + ":" + Path.GetFileName(fiFullPath.FullName);
                    IFeatureClass pFC = ((IFeatureWorkspace)pWorkspace).OpenFeatureClass(sFC);
                    pResultLayer = new FeatureLayer();
                    ((IFeatureLayer)pResultLayer).FeatureClass = pFC;
                    break;

                case ArcMapBrowse.GISDataStorageTypes.ShapeFile:
                    IFeatureClass pShapeFile = ((IFeatureWorkspace)pWorkspace).OpenFeatureClass(fiFullPath.FullName);
                    pResultLayer = new FeatureLayer();
                    ((IFeatureLayer)pResultLayer).FeatureClass = pShapeFile;
                    break;

                case ArcMapBrowse.GISDataStorageTypes.TIN:
                    ITin pTIN = ((ITinWorkspace)pWorkspace).OpenTin(fiFullPath.FullName);
                    pResultLayer = new TinLayer();
                    ((ITinLayer)pResultLayer).Dataset = pTIN;
                    pResultLayer.Name = fiFullPath.Name;
                    break;

                default:
                    Exception ex = new Exception("Unhandled GIS dataset type");
                    ex.Data["FullPath Path"] = fiFullPath.FullName;
                    ex.Data["Storage Type"] = eStorageType.ToString();
                    throw ex;
            }

            if (!string.IsNullOrEmpty(sLayerName))
            {
                pResultLayer.Name = sLayerName;
            }

            if (string.IsNullOrEmpty(sGroupLayer))
            {
                ((IMapLayers)ArcMap.Document.FocusMap).InsertLayer(pResultLayer, true, 0);
            }
            else
            {
                IGroupLayer pGrpLayer = GetGroupLayer(sGroupLayer);
                ((IMapLayers)ArcMap.Document.FocusMap).InsertLayerInGroup(pGrpLayer, pResultLayer, true, 0);
            }

            ArcMap.Document.UpdateContents();
            ArcMap.Document.ActiveView.Refresh();
            ArcMap.Document.CurrentContentsView.Refresh(null);

            return pResultLayer;
        }

        public static ILayer GetLayerBySource(FileSystemInfo fiFullPath)
        {
            if (!fiFullPath.Exists)
                return null;

            IMapLayers mapLayers = (IMapLayers)ArcMap.Document.FocusMap;
            UID pID = new UIDClass();
            pID.Value = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}"; // eEsriLayerTypes.Esri_DataLayer
            IEnumLayer pEnumLayer = ((IMapLayers)ArcMap.Document.FocusMap).Layers[pID, true];
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                if (pLayer is IGeoFeatureLayer)
                {
                    IGeoFeatureLayer pGFL = (IGeoFeatureLayer)pLayer;
                    string sPath = ((IDataset)pGFL).Workspace.PathName;
                    if (pGFL.FeatureClass.FeatureDataset is IFeatureDataset)
                    {
                        sPath = Path.Combine(sPath, pGFL.FeatureClass.FeatureDataset.Name);
                    }

                    sPath = Path.Combine(sPath, ((IDataset)pGFL.FeatureClass).Name);

                    if (((IDataset)pGFL.FeatureClass).Workspace.Type == esriWorkspaceType.esriFileSystemWorkspace)
                    {
                        sPath = Path.ChangeExtension(sPath, "shp");
                    }

                    if (string.Compare(fiFullPath.FullName, sPath, true) == 0)
                    {
                        return pLayer;
                    }
                }
                else if (pLayer is IRasterLayer)
                {
                    if (string.Compare(((IRasterLayer)pLayer).FilePath, fiFullPath.FullName, true) == 0)
                    {
                        return pLayer;
                    }
                }

                pLayer = pEnumLayer.Next();
            }

            return null;
        }

        /// <summary>
        /// Gets all layers from ArcMap ToC that possess the specified name (case insensitive) and optionally of specified type 
        /// </summary>
        /// <param name="sLayerName">Name of the layer</param>
        /// <param name="pArcMap">ArcMap</param>
        /// <param name="eType">Optional constraint to look for a layer of a certain type. Pass Nothing to look for any type.</param>
        /// <returns>ILayer if found, otherwise nothing</returns>
        /// <remarks>Code taken from EDN on Jul 10 2007. Retrieves all layers from the current focus map that
        /// have the type specified by eType. Note that this code was enhanced from the copy taken off
        /// the internet. The method pMap.Layers() throws an exception when there are no layers in the map.
        /// 
        /// PGB - 27 - Jul 2007 - For some reason, the Layers() call throws an exception when it is called for
        /// a group layer and there are feature layers in the legend, but not group layers. It is
        /// commented out for now.</remarks>
        public static ILayer GetLayerByName(string sLayerName, eEsriLayerTypes eType)
        {
            if (string.IsNullOrEmpty(sLayerName))
                return null;

            if (ArcMap.Document.FocusMap.LayerCount < 1)
                return null;

            UID pID = new UIDClass();
            switch (eType)
            {
                case eEsriLayerTypes.Esri_DataLayer:
                    pID.Value = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}";
                    break;
                case eEsriLayerTypes.Esri_GeoFeatureLayer:
                    pID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
                    break;
                case eEsriLayerTypes.Esri_GraphicsLayer:
                    pID.Value = "{34B2EF81-F4AC-11D1-A245-080009B6F22B}";
                    break;
                case eEsriLayerTypes.Esri_FDOGraphicsLayer:
                    pID.Value = "{5CEAE408-4C0A-437F-9DB3-054D83919850}";
                    break;
                case eEsriLayerTypes.Esri_CoverageAnnotationLayer:
                    pID.Value = "{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E}";
                    break;
                case eEsriLayerTypes.Esri_GroupLayer:
                    pID.Value = "{EDAD6644-1810-11D1-86AE-0000F8751720}";
                    break;
                default:
                    pID = null;
                    break;
            }

            IEnumLayer pEnumLayer = ArcMap.Document.FocusMap.Layers[pID, true];
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                if (string.Compare(pLayer.Name, sLayerName, true) == 0)
                    return pLayer;

                pLayer = pEnumLayer.Next();
            }

            return null;
        }

        /// <summary>
        /// Get the group layer from the ArcMap TOC with the specified name, creating it if needed.
        /// </summary>
        /// <param name="pArcMap">ArcMap</param>
        /// <param name="sName">The name of the group layer</param>
        /// <param name="bCreateIfNeeded">If true then the group layer will be created if it doesn't exist</param>
        /// <returns>The group layer </returns>
        /// <remarks>The default is to create the group layer if it doesn't exist</remarks>
        public static IGroupLayer GetGroupLayer(string sName, bool bCreateIfNeeded = true)
        {
            if (string.IsNullOrEmpty(sName))
            {
                // This route might be needed if the GCD calls this function without an open project.
                return null;
            }

            // Try and get the group layer with the name
            IGroupLayer pGrpLayer = (IGroupLayer)GetLayerByName(sName, eEsriLayerTypes.Esri_GroupLayer);
            if (!(pGrpLayer is IGroupLayer))
            {
                pGrpLayer = new GroupLayer();
                pGrpLayer.Name = sName;
                ((IMapLayers)ArcMap.Document.FocusMap).InsertLayer(pGrpLayer, true, 0);
            }

            return pGrpLayer;
        }

        public static IGroupLayer GetGroupLayer(string sName, IGroupLayer pParentGroupLayer, bool bCreateIfNeeded = true)
        {
            if (string.IsNullOrEmpty(sName))
            {
                // This route might be needed if the GCD calls this function without an open project.
                return null;
            }

            // Try and find the group layer already in the hierarchy
            ICompositeLayer pCompositeLayer = (ICompositeLayer)pParentGroupLayer;
            for (int i = 0; i <= pCompositeLayer.Count - 1; i++)
            {
                if (string.Compare(pCompositeLayer.Layer[i].Name, sName, true) == 0)
                {
                    return (IGroupLayer)pCompositeLayer.Layer[i];
                }
            }

            IGroupLayer pResultLayer = new GroupLayer();
            pResultLayer.Name = sName;
            ((IMapLayers)ArcMap.Document.FocusMap).InsertLayerInGroup(pParentGroupLayer, pResultLayer, true, 0);

            return pResultLayer;
        }

        public static FileSystemInfo GetPathFromLayer(ILayer pL)
        {
            if (pL == null || pL is ICompositeLayer)
            {
                return null;
            }

            FileSystemInfo siResult = null;
            if (pL is IFeatureLayer)
            {
                IFeatureLayer pFL = (IFeatureLayer)pL;

                //check if featureclass is nothing (this can happen if the underlying file has been deleted but the layer is still in the TOC - FP Sep 10 2014
                if (pFL.FeatureClass != null)
                {
                    string sPath = ((IDataset)pFL.FeatureClass).Workspace.PathName;
                    if (pFL.FeatureClass.FeatureDataset is IFeatureDataset)
                    {
                        sPath = Path.Combine(sPath, pFL.FeatureClass.FeatureDataset.Name);
                    }
                    sPath = Path.Combine(sPath, ((IDataset)pFL.FeatureClass).Name);

                    if (((IDataset)pFL.FeatureClass).Workspace.Type == esriWorkspaceType.esriFileSystemWorkspace)
                    {
                        sPath = Path.ChangeExtension(sPath, "shp");
                    }

                    siResult = new FileInfo(sPath);
                }
            }
            else if (pL is IRasterLayer)
            {
                siResult = new FileInfo(((IRasterLayer)pL).FilePath);
            }
            else if (pL is ITinLayer)
            {
                IDataset pDS = (IDataset)pL;
                siResult = new DirectoryInfo(System.IO.Path.Combine(pDS.Workspace.PathName, pDS.Name));
            }

            return siResult;
        }

        /// <summary>
        /// Create a singleton for a workspace factory
        /// </summary>
        /// <param name="eGISStorageType"></param>
        /// <returns>CALLER IS RESPONSIBLE FOR RELEASING RETURNED COM OBJECT</returns>
        /// <remarks>This is the only correct method for creating a workspace factory. Do not call "New" to create this singleton classes.
        /// http://forums.esri.com/Thread.asp?c=93&f=993&t=178686
        /// </remarks>
        private static IWorkspaceFactory GetWorkspaceFactory(ArcMapBrowse.GISDataStorageTypes eGISStorageType)
        {
            Type aType = null;
            IWorkspaceFactory pWSFact = null;

            try
            {
                switch (eGISStorageType)
                {
                    case ArcMapBrowse.GISDataStorageTypes.RasterFile:
                        aType = Type.GetTypeFromProgID("esriDataSourcesRaster.RasterWorkspaceFactory");
                        break;
                    case ArcMapBrowse.GISDataStorageTypes.ShapeFile:
                        aType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                        break;
                    case ArcMapBrowse.GISDataStorageTypes.FileGeodatase:
                        aType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                        break;
                    case ArcMapBrowse.GISDataStorageTypes.CAD:
                        aType = Type.GetTypeFromProgID("esriDataSourcesFile.CadWorkspaceFactory");
                        break;
                    case ArcMapBrowse.GISDataStorageTypes.PersonalGeodatabase:
                        aType = Type.GetTypeFromProgID("esriDataSourcesGDB.AccessWorkspaceFactory");
                        break;
                    case ArcMapBrowse.GISDataStorageTypes.TIN:
                        aType = Type.GetTypeFromProgID("esriDataSourcesFile.TinWorkspaceFactory");
                        break;
                    default:
                        throw new Exception("Unhandled GIS storage type");
                }

                pWSFact = (IWorkspaceFactory)Activator.CreateInstance(aType);
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error getting workspace factory", ex);
                ex2.Data["Workspace Type"] = eGISStorageType.ToString();
                throw ex2;
            }

            // CALLER IS RESPONSIBLE FOR RELEASING RETURNED COM OBJECT
            return pWSFact;
        }

        /// <summary>
        /// Open a file-based workspace (Raster or ShapeFile)
        /// </summary>
        /// <param name="fiFullPath">Workspace directory or file path</param>
        /// <returns>CALLER IS RESPONSIBLE FOR RELEASING RETURNED COM OBJECT</returns>
        public static IWorkspace GetWorkspace(FileSystemInfo fiFullPath)
        {
            ArcMapBrowse.GISDataStorageTypes eType = GetWorkspaceType(fiFullPath.FullName);
            IWorkspaceFactory pWSFact = GetWorkspaceFactory(eType);
            DirectoryInfo fiWorkspace = GetWorkspacePath(fiFullPath.FullName);
            IWorkspace pWS = pWSFact.OpenFromFile(fiWorkspace.FullName, ArcMap.Application.hWnd);

            // Must release the workspace factory object
            int refsLeft = 0;
            do
            {
                refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pWSFact);
            }
            while (refsLeft > 0);

            // CALLER IS RESPONSIBLE FOR RELEASING RETURNED COM OBJECT
            return pWS;
        }

        /// <summary>
        /// Derives the file system path of a workspace given any path
        /// </summary>
        /// <param name="sPath">Any path. Can be a folder (e.g. file geodatabase) or absolute path to a file.</param>
        /// <returns>The workspace path (ending with .gdb for file geodatabases) or the folder for file based data.</returns>
        /// <remarks>PGB 9 Sep 2011.</remarks>
        public static DirectoryInfo GetWorkspacePath(string sFullPath)
        {
            if (string.IsNullOrEmpty(sFullPath))
                return null;

            string sWorkspacePath = string.Empty;

            switch (GetWorkspaceType(sFullPath))
            {
                case ArcMapBrowse.GISDataStorageTypes.FileGeodatase:
                    int index = sFullPath.ToLower().LastIndexOf(".gdb");
                    sWorkspacePath = sFullPath.Substring(0, index + 4);
                    break;
                case ArcMapBrowse.GISDataStorageTypes.CAD:
                    index = sFullPath.ToLower().LastIndexOf(".dxf");
                    sWorkspacePath = Path.GetDirectoryName(sFullPath.Substring(0, index));
                    break;
                default:
                    sWorkspacePath = Path.GetDirectoryName(sFullPath);
                    break;
            }
            return new System.IO.DirectoryInfo(sWorkspacePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        /// <remarks>Note that the path that comes in may or may not have a dataset name on the end. So it
        /// may be the path to a directory, or end with .gdb if a file geodatabase or may have a slash and
        /// then the dataset name on the end.</remarks>
        public static ArcMapBrowse.GISDataStorageTypes GetWorkspaceType(string sFullPath)
        {
            if (sFullPath.ToLower().Contains(".gdb"))
            {
                return ArcMapBrowse.GISDataStorageTypes.FileGeodatase;
            }
            else
            {
                if (System.IO.Directory.Exists(sFullPath))
                {
                    return ArcMapBrowse.GISDataStorageTypes.RasterFile; // ESRI GRID (folder)
                }
                else
                {
                    if (sFullPath.ToLower().Contains(".dxf"))
                    {
                        return ArcMapBrowse.GISDataStorageTypes.CAD;
                    }
                    else if (sFullPath.ToLower().Contains(".tif"))
                    {
                        return ArcMapBrowse.GISDataStorageTypes.RasterFile;
                    }
                    else if (sFullPath.ToLower().Contains(".img"))
                    {
                        return ArcMapBrowse.GISDataStorageTypes.RasterFile;
                    }
                    else
                    {
                        return ArcMapBrowse.GISDataStorageTypes.ShapeFile;
                    }
                }
            }
        }

        public static void RemoveLayer(FileSystemInfo layerPath)
        {
            ILayer pLayer = GetLayerBySource(layerPath);

            while (pLayer is ILayer)
            {
                IGroupLayer pParent = GetParentGroupLayer(pLayer);

                if (pLayer is IDataLayer2)
                {
                    ((IDataLayer2)pLayer).Disconnect();
                }

                ArcMap.Document.FocusMap.DeleteLayer(pLayer);
                ArcMap.Document.UpdateContents();

                // Remove empty group layers from ToC
                while (pParent is IGroupLayer)
                {
                    ILayer pNextParent = GetParentGroupLayer(pParent);
                    ICompositeLayer pComp = (ICompositeLayer)pParent;
                    if (pComp.Count < 1)
                    {
                        ArcMap.Document.FocusMap.DeleteLayer(pParent);
                        ArcMap.Document.UpdateContents();
                    }

                    if (pNextParent is IGroupLayer)
                        pParent = (IGroupLayer)pNextParent;
                    else
                        pParent = null;
                }

                ArcMap.Document.ActiveView.Refresh();

                // Release all references to the layer to prevent locks on the underlying data source
                // http://edndoc.esri.com/arcobjects/9.2/net/fe9f7423-2100-4c70-8bd6-f4f16d5ce8c0.htm
                int refsLeft = 0;
                do
                {
                    refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayer);
                }
                while (refsLeft > 0);
                pLayer = null;
                GC.Collect();
                pLayer = GetLayerBySource(layerPath);
            }
        }


        private static IGroupLayer GetParentGroupLayer(ILayer pLayer)
        {
            //Loop over all group layers and see if the specified layer is inside
            IMap pMap = ArcMap.Document.FocusMap;
            UID pUID = new UID();
            pUID.Value = "{EDAD6644-1810-11D1-86AE-0000F8751720}";

            IEnumLayer pEnum = ArcMap.Document.FocusMap.Layers[pUID, true];
            ICompositeLayer pGroupLayer = (ICompositeLayer)pEnum.Next();
            while (pGroupLayer is ICompositeLayer)
            {
                for (int i = 0; i < pGroupLayer.Count; i++)
                {
                    if (pGroupLayer.Layer[i].Equals(pLayer))
                    {
                        return (IGroupLayer)pGroupLayer;
                    }
                }
                pGroupLayer = (ICompositeLayer)pEnum.Next();
            }

            return null;
        }

        //public void RemoveLayersfromTOC(string directory)
        //{
        //    //IMxDocument mxMap = (IMxDocument)application.Document;
        //    //IMap pMap = mxMap.FocusMap;
        //    //IMapLayers pMapLayers = pMap;

        //    for (int i = 0; i <= ArcMap.Document.FocusMap.LayerCount - 1; i++)
        //    {
        //        ILayer player = ArcMap.Document.FocusMap.Layer[i];
        //        if (player is IGroupLayer)
        //        {
        //            RemoveLayersfromGroupLayer((IGroupLayer)player, directory);
        //        }
        //        else
        //        {
        //            IDataset pDS = player;
        //            try
        //            {
        //                if (LCase(directory) == LCase(pDS.Workspace.PathName))
        //                {
        //                    pMap.DeleteLayer(player);
        //                }
        //            }
        //            }

        //        if (player != null)
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(player);
        //            player = null;
        //        }
        //    }

        //    mxMap.UpdateContents();
        //    mxMap.ActiveView.Refresh();
        //    ESRI.ArcGIS.ArcMapUI.IContentsView pContentsView = mxMap.CurrentContentsView;
        //    pContentsView.Refresh(null);
        //    if (mxMap != null)
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(mxMap);
        //        mxMap = null;
        //    }

        //    if (pContentsView != null)
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pContentsView);
        //        pContentsView = null;
        //    }
        //}

        //public void RemoveLayersfromGroupLayer(IGroupLayer pGroupLayer, string directory)
        //{
        //    ILayer pLayer;
        //    List<ILayer> LayersToDelete = new List<ILayer>();
        //    ICompositeLayer pCompositeLayer = (ICompositeLayer)pGroupLayer;
        //    for (int i = 1; i <= pCompositeLayer.Count; i++)
        //    {
        //        pLayer = pCompositeLayer.Layer[i - 1];
        //        if (pLayer is IGroupLayer)
        //        {
        //            RemoveLayersfromGroupLayer(pLayer, directory);
        //        }
        //        else
        //        {
        //            try
        //            {
        //                IDataset pDS = (IDataset)pLayer;
        //                string LayerDirectoryname = pDS.Workspace.PathName.ToLower();
        //                if (LayerDirectoryname.EndsWith(IO.Path.DirectorySeparatorChar))
        //                {
        //                    LayerDirectoryname = LayerDirectoryname.Substring(0, LayerDirectoryname.Length - 1);
        //                }

        //                if (LCase(directory) == LayerDirectoryname)
        //                {
        //                    LayersToDelete.Add(pLayer);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine(ex.Message);
        //            }
        //        }
        //    }

        //    foreach (ILayer pDeleteLayer in LayersToDelete)
        //    {
        //        pGroupLayer.Delete(pDeleteLayer);
        //        if (pDeleteLayer != null)
        //        {
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(pDeleteLayer);
        //            pDeleteLayer = null;
        //        }
        //    }

        //    if (pGroupLayer != null)
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pGroupLayer);
        //        pGroupLayer = null;
        //    }
        //}

        //public void RemoveGroupLayer(string sGroupLayerName)
        //{
        //    IMap pMap = ArcMap.Document.FocusMap;
        //    UID pUID = new UID();
        //    pUID.Value = "{EDAD6644-1810-11D1-86AE-0000F8751720}";

        //    IEnumLayer pEnum = ArcMap.Document.FocusMap.Layers[pUID, true];
        //    ILayer pL = pEnum.Next();
        //    while (pL is ILayer)
        //    {
        //        if (string.Compare(sGroupLayerName, pL.Name, true) == 0)
        //        {
        //            pMap.DeleteLayer(pL);
        //        }

        //        pL = pEnum.Next();
        //    }
        //}


        public static IRasterLayer IsRasterLayerInGroupLayer(System.IO.FileSystemInfo rasterPath, IGroupLayer pGrpLyr)
        {
            ICompositeLayer compositeLayer = pGrpLyr as ICompositeLayer;
            if (compositeLayer != null & compositeLayer.Count > 0)
            {
                for (int i = 0; i <= compositeLayer.Count - 1; i++)
                {
                    if (compositeLayer.Layer[i] is IRasterLayer)
                    {
                        IRasterLayer pLayer = (IRasterLayer)compositeLayer.Layer[i];
                        if (string.Compare(pLayer.FilePath, rasterPath.FullName, true) == 0)
                        {
                            return pLayer;
                        }
                    }
                }
            }

            return null;
        }
    }
}
