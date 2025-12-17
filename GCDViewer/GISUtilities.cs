using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Mapping;
using GCDViewer.ProjectTree;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using System.Windows.Controls;
using System.Xml.Linq;
using ArcGIS.Core.Geometry;
using System.Collections;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Core.Data.UtilityNetwork.Trace;
using System.Windows;
using ArcGIS.Desktop.Internal.Mapping.Voxel.Controls.Transparency;
using System.Reflection.PortableExecutable;
using Microsoft.VisualBasic;
using ArcGIS.Desktop.Core.Geoprocessing;
//using ArcGIS.Core.Internal.CIM;

namespace GCDViewer
{
    public class GISUtilities
    {
        public enum NodeInsertModes
        {
            Add,
            Insert
        };

        public async Task AddToMapAsync(TreeViewItemModel item, int index, Tuple<double, double> range = null)
        {
            await QueuedTask.Run(async () =>
               {
                   if (index < 0)
                       throw new ArgumentOutOfRangeException(nameof(index), "Layer index must be greater than or equal to zero");

                   // If we need to zoom to layer then set this variable to the layer and then zoom to it at the end
                   Layer zoomLayer = null;

                   // Check if there is an active map view
                   if (MapView.Active == null)
                   {
                       // Check if there are any maps in the project
                       var mapProjectItems = Project.Current.GetItems<MapProjectItem>();
                       Map map = null;

                       if (!mapProjectItems.Any())
                       {
                           // No maps exist, so create a new map
                           map = MapFactory.Instance.CreateMap("NewMap", MapType.Map, MapViewingMode.Map);
                           ProApp.Panes.CreateMapPaneAsync(map);
                       }
                       else
                       {
                           // Use the first available map
                           map = mapProjectItems.FirstOrDefault()?.GetMap();
                       }
                   }


                   // At this point, there should be at least one map, so you can proceed to add layers
                   Map activeMap = MapView.Active?.Map ?? Project.Current.GetItems<MapProjectItem>().FirstOrDefault()?.GetMap();


                   // Attempt to find existing layer and exit if its present
                   if (!string.IsNullOrEmpty(item.MapLayerUri))
                   {
                       Layer existingLayer = MapView.Active?.Map.FindLayer(item.MapLayerUri);
                       if (existingLayer != null)
                           return;
                   }

                   // Build a list of the parent groups
                   var parentItems = new List<TreeViewItemModel>();
                   var parentItem = item.Parent;
                   while (parentItem is not null)
                   {
                       parentItems.Add(parentItem);
                       parentItem = parentItem.Parent;
                   }

                   // Now try to find these groups, starting with the root, project entry
                   parentItems.Reverse();
                   ILayerContainerEdit parent = activeMap;
                   foreach (TreeViewItemModel groupItem in parentItems)
                   {
                       // Attempt to find the existing ToC group layer
                       if (!string.IsNullOrEmpty(groupItem.MapLayerUri))
                       {
                           var parentLayer = activeMap.FindLayer(groupItem.MapLayerUri);
                           if (parentLayer != null)
                           {
                               parent = parentLayer as ILayerContainerEdit;
                               continue;
                           }
                       }

                       // If this is the basemap group then find the count of project nodes
                       // and ensure that we insert this base map group node below any Riverscapes projects.
                       // For now we just add it to the bottom of the map ToC (even though this
                       // means that it will be below any ESRI added basemaps
                       int groupIndex = 0;


                       if (!(groupItem is ProjectTree.GCDProject) && groupItem.Parent != null)
                       {
                           //MapView.Active.Map.GetLayersAsFlattenedList();
                           int insert_index = GetInsertIndex(groupItem);
                           groupIndex = Math.Min(parent.Layers.Count, insert_index);

                           //groupIndex = Math.Min(groupItem.Parent.Children.IndexOf(groupItem), parent.Layers.Count);
                           // This is a group node somewhere in project hierarchy. Get its positional index
                           //groupIndex = groupItem.Parent.Children.IndexOf(groupItem);
                       }


                       // If got to here then the group layer doesn't exist
                       parent = LayerFactory.Instance.CreateGroupLayer(parent, groupIndex, groupItem.Name);
                       groupItem.MapLayerUri = ((ArcGIS.Desktop.Mapping.GroupLayer)parent).URI;
                       groupItem.MapLayer = parent;

                       // DO NOT ATTEMPT TO EXPAND GROUP LAYERS HERE.
                       // This was causing ghosting of groups in the Map ToC.

                       // If this is the project and we just added it then save it as the zoom layer
                       if (groupItem.Item is GCDProject)
                           zoomLayer = parent as Layer;
                   }

                   Layer layer = null;
                   if (item.Item is ProjectTree.Vector && (item.Item as ProjectTree.Vector).WorkspaceType == FileSystemDataset.GISDataStorageTypes.GeoPackage)
                   {
                       CIMStandardDataConnection conn = new CIMStandardDataConnection();
                       conn.WorkspaceConnectionString = string.Format("DATABASE={0}", (item.Item as GISDataset).WorkspacePath);
                       conn.WorkspaceFactory = WorkspaceFactory.Sql;
                       conn.DatasetType = esriDatasetType.esriDTFeatureClass;
                       conn.Dataset = (item.Item as GISDataset).GISUri.ToString().Split("main.")[1];

                       LayerCreationParams p = new LayerCreationParams(conn);
                       p.MapMemberIndex = GetInsertIndex(item);
                       p.Name = item.Name;
                       layer = LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.FeatureLayer>(p, parent as ILayerContainerEdit);
                   }
                   else
                   {
                       Uri uri = null;
                       if (item.Item is GISDataset dataset)
                       {
                           uri = dataset.GISUri;
                           if (!dataset.Exists)
                               throw new FileNotFoundException("The dataset workspace file does not exist.", dataset.Path.FullName);
                       }
                       //else if (item.Item is ProjectTree.WMSLayer wmsLayer)
                       //{
                       //    uri = wmsLayer.URL;
                       //}

                       System.Diagnostics.Debug.Print("Creating layer {0} with parent {1}. Uri: {2}", item.Name, parent, uri?.ToString());
                       index = GetInsertIndex(item);
                       layer = LayerFactory.Instance.CreateLayer(uri, parent as ILayerContainerEdit, index, item.Name);
                       item.MapLayer = layer;
                   }

                   // Force evaluation of the extent
                   if (layer is FeatureLayer featurelayer)
                   {
                       featurelayer.ClearSelection();
                       featurelayer.ClearDisplayCache();
                       featurelayer.SetDefinitionQuery("");
                       Envelope extent = featurelayer.QueryExtent();
                       System.Diagnostics.Debug.Print("Envelope: XMin={0}, YMin={1}, XMax={2}, YMax={3}", extent.XMin, extent.YMin, extent.XMax, extent.YMax);
                   }
                   else if (layer is RasterLayer rasterLayer)
                   {
                       Envelope extent = rasterLayer.GetRaster().GetExtent();
                       //SymbolizeRasterLayer(item.Item, rasterLayer);
                       //GIS.MapRenderers.ApplyNamedColorRampToRasterAsync(rasterLayer, "Partial Spectrum");

                       if (item.Item is DEMSurvey)
                       {
                           // Add the hillshade first
                           DEMSurvey dem = item.Item as DEMSurvey;
                           //Raster hillshade = dem.Hillshade;
                           //if (hillshade is Raster)
                           //{
                           //    int hillshadeIndex = index + 1;
                           //    LayerFactory.Instance.CreateLayer(hillshade.GISUri, parent as ILayerContainerEdit, hillshadeIndex, string.Format("{0} - Hillshade", item.Name));
                           //}

                           GIS.MapRenderers.ApplyStretchRenderer(rasterLayer, "Brown to Blue Green Diverging, Bright", range);
                       }
                       else if (item.Item is ErrorSurface)
                       {
                           GIS.MapRenderers.ApplyStretchRenderer(rasterLayer, "Partial Spectrum");
                       }
                       else if (item.Item is AssocSurface)
                       {
                           AssocSurface assoc = item.Item as AssocSurface;
                           switch (assoc.AssocSurfaceType)
                           {
                               case AssocSurface.AssociatedSurfaceTypes.InterpolationError:
                               case AssocSurface.AssociatedSurfaceTypes.SlopeDegree:
                               case AssocSurface.AssociatedSurfaceTypes.SlopePercent:
                                   GIS.MapRenderers.ApplyStretchRenderer(rasterLayer, "Slope");
                                   break;

                               case AssocSurface.AssociatedSurfaceTypes.Roughness:
                                   GIS.MapRenderers.ApplyStretchRenderer(rasterLayer, "Brown Light To Dark");
                                   break;

                               case AssocSurface.AssociatedSurfaceTypes.PointDensity:
                                   GIS.MapRenderers.ApplyStretchRenderer(rasterLayer, "Green to Blue");
                                   break;

                               case AssocSurface.AssociatedSurfaceTypes.PointQuality3D:
                                   GIS.MapRenderers.ApplyStretchRenderer(rasterLayer, "Precipitation");
                                   break;
                           }
                       }

                       //if (item.Item is DEMSurvey)
                       //{
                       //    DEMSurvey dem = item.Item as DEMSurvey;
                       //    Raster hillshade = dem.Hillshade;
                       //    if (hillshade is Raster)
                       //    {
                       //        int hillshadeIndex = index;
                       //        Layer hs_layer = LayerFactory.Instance.CreateLayer(hillshade.GISUri, parent as ILayerContainerEdit, index, string.Format("{0} - Hillshade", item.Name));
                       //    }
                       //}
                   }

                   // Store the ArcPro layer URI so that we can find this layer again
                   item.MapLayerUri = layer.URI;

                   // Apply feature filter
                   if (item.Item is ProjectTree.Vector vector && layer is FeatureLayer featureLayer)
                   {
                       if (!string.IsNullOrEmpty(vector.DefinitionQuery))
                           featureLayer.SetDefinitionQuery(vector.DefinitionQuery);
                   }

                   // Trace back up hierarchy and expand any group layers
                   parentItem = item.Parent;
                   while (parentItem is not null)
                   {
                       if (!string.IsNullOrEmpty(parentItem.MapLayerUri))
                       {
                           var parentLayer = activeMap.FindLayer(parentItem.MapLayerUri);
                           if (parentLayer is ArcGIS.Desktop.Mapping.GroupLayer)
                           {
                               var groupLayer = parentLayer as ArcGIS.Desktop.Mapping.GroupLayer;
                               if (parentItem.Item is ProjectTree.GroupLayer)
                               {
                                   var parentGroupItem = parentItem.Item as ProjectTree.GroupLayer;
                                   if (!groupLayer.IsExpanded && parentGroupItem.Expanded)
                                       groupLayer.SetExpanded(true);
                               }
                               else
                               {
                                   groupLayer.SetExpanded(true);
                               }
                           }

                       }
                       parentItem = parentItem.Parent;
                   }


                   if (layer == null)
                       throw new InvalidOperationException("Failed to create layer from the layer file.");

                   if (zoomLayer is Layer)
                   {
                       var layerExtent = GetLayerExtent(layer);
                       if (layerExtent != null)
                       {
                           //Map activeMap = MapView.Active?.Map ?? Project.Current.GetItems<MapProjectItem>().FirstOrDefault()?.GetMap();

                           //Project.Current.GetItems<MapProjectItem>().FirstOrDefault().

                           MapView view = MapView.Active;
                           view?.ZoomToAsync(layerExtent);
                       }
                   }
               });
        }

        public async Task AddToMapScaledAsync(TreeViewItemModel item, int index)
        {
            await QueuedTask.Run(async () =>
            {

                var dem = item.Item as DEMSurvey;
                if (dem is null)
                    return;

                // Get the min and Max of all DEMs in the project
                double? minElevation = new Nullable<double>();
                double? maxElevation = new Nullable<double>();
                foreach (DEMSurvey raster in dem.Project.DEMSurveys)
                {
                    var parametersMin = Geoprocessing.MakeValueArray(raster.GISPath, "MINIMUM");
                    var parametersMax = Geoprocessing.MakeValueArray(raster.GISPath, "MAXIMUM");
                    var minRes = await Geoprocessing.ExecuteToolAsync("GetRasterProperties_management", parametersMin);
                    var maxRes = await Geoprocessing.ExecuteToolAsync("GetRasterProperties_management", parametersMax);

                    double min = Convert.ToDouble(minRes.Values[0]);
                    double max = Convert.ToDouble(maxRes.Values[0]);

                    if (!minElevation.HasValue || min < minElevation.Value)
                        minElevation = min;

                    if (!maxElevation.HasValue || max > maxElevation.Value)
                        maxElevation = max;
                }

                Tuple<double, double> range = new Tuple<double, double>(minElevation.Value, maxElevation.Value);

                await AddToMapAsync(item, index, range);
            });
        }

        private async void SymbolizeRasterLayer(ITreeItem item, RasterLayer rasterLayer)
        {
            if (item is DEMSurvey)
            {
                await GIS.MapRenderers.ApplyDEMColorRampAsync(rasterLayer);
                rasterLayer.SetTransparency(40);
            }
            else if (item is AssocSurface)
            {
                AssocSurface assoc = item as AssocSurface;
                switch (assoc.AssocSurfaceType)
                {
                    case AssocSurface.AssociatedSurfaceTypes.Roughness:
                        await GIS.MapRenderers.ApplyRoughnessColorRampAsync(rasterLayer);
                        break;

                    case AssocSurface.AssociatedSurfaceTypes.SlopePercent:
                    case AssocSurface.AssociatedSurfaceTypes.SlopeDegree:
                        await GIS.MapRenderers.ApplyNamedColorRampToRasterAsync(rasterLayer, "Slope");
                        break;

                        //TODO: point density
                }
            }
            else if (item is DoDBase)
            {
                DoDBase dod = item as DoDBase as DoDBase;
                await GIS.MapRenderers.ApplyDoDClassifyColorRampAsync(rasterLayer, 10);
            }
            else if (item is ErrorSurface)
            {
                //await GIS.MapRenderers.ApplyNamedColorRampToRasterAsync(rasterLayer, "Magma");
                await GIS.MapRenderers.ApplyNamedColorRampToRasterAsync(rasterLayer, "Partial Spectrum");

                //errRow.Raster.ComputeStatistics();
                //Dictionary<string, decimal> stats = errRow.Raster.GetStatistics();
                //double rMin = (double)stats["min"];
                //double rMax = (double)stats["max"];

                //if (rMin == rMax)
                //{
                //    IRasterRenderer rasterRenderer = RasterSymbolization.CreateESRIDefinedContinuousRenderer(errRow.Raster, 1, "Partial Spectrum");
                //    AddRasterLayer(errRow.Raster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency);
                //}
                //else if (rMax <= 1 & rMax > 0.25)
                //{
                //    IRasterRenderer rasterRenderer = RasterSymbolization.CreateClassifyRenderer(errRow.Raster, 11, "Partial Spectrum", 1.1);
                //    AddRasterLayer(errRow.Raster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency);
                //}
                //else
                //{
                //    IRasterRenderer rasterRenderer = RasterSymbolization.CreateClassifyRenderer(errRow.Raster, 11, "Partial Spectrum");
                //    AddRasterLayer(errRow.Raster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency);
                //}
            }

        }

        private int GetInsertIndex(TreeViewItemModel newItem)
        {
            // This is the position of the item in the Business Logic Tree
            //int business_logic_index = newItem.Parent.Children.IndexOf(newItem);

            var map_layers = MapView.Active?.Map.GetLayersAsFlattenedList();

            int insert_index = 0;
            foreach (TreeViewItemModel child in newItem.Parent.Children)
            {
                if (child == newItem)
                    return insert_index;

                if (child.MapLayer is Layer && map_layers != null)
                {
                    if (map_layers.Contains(child.MapLayer))
                        insert_index++;
                }
            }

            return insert_index;
        }

        private static Envelope GetLayerExtent(Layer layer)
        {
            if (layer is FeatureLayer featureLayer)
            {
                return featureLayer.QueryExtent();
            }
            else if (layer is RasterLayer rasterLayer)
            {
                return rasterLayer.GetRaster().GetExtent();
            }
            else if (layer is ArcGIS.Desktop.Mapping.GroupLayer groupLayer)
            {
                Envelope combinedExtent = null;
                foreach (var subLayer in groupLayer.Layers)
                {
                    var subLayerExtent = GetLayerExtent(subLayer);
                    if (subLayerExtent != null)
                    {
                        combinedExtent = combinedExtent == null ? subLayerExtent : combinedExtent.Union(subLayerExtent);
                    }
                }

                return combinedExtent;
            }

            return null;
        }



        /// <summary>
        /// Determine the location of the layer file for this GIS item
        /// </summary>
        /// <remarks>
        /// The following locations will be searched in order for a 
        /// file with the name SYMBOLOGY_KEY.lyr
        /// 
        /// 1. ProjectFolder
        /// 2. %APPDATA%\RAVE\Symbology\esri\MODEL
        /// 3. %APPDATA%\RAVE\Symbology\esrsi\Shared
        /// 4. SOFTWARE_DEPLOYMENT\Symbology\esri\MODEL
        /// 5. SOFTWARE_DEPLOYMENT\Symbology\esri\Shared
        /// 
        /// </remarks>
        //public FileInfo GetSymbologyFile(GISDataset layer)
        //{
        //    if (layer is null || string.IsNullOrEmpty(layer.SymbologyKey))
        //        return null;

        //    string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Resources.AppDataFolder);
        //    string symbologyFolder = Path.Combine(appDataFolder, Properties.Resources.AppDataSymbologyFolder);

        //    List<string> SearchFolders = new List<string>()
        //    {
        //        layer.Project.Folder.FullName,
        //        Path.Combine(symbologyFolder, layer.Project.ProjectType),
        //        Path.Combine(symbologyFolder, Properties.Resources.AppDataSymbologySharedFolder)
        //    };

        //    foreach (string folder in SearchFolders)
        //    {
        //        MessageBox.Show(folder, "Symbology Search Folder", MessageBoxButton.OK, MessageBoxImage.Information);
        //        if (Directory.Exists(folder))
        //        {
        //            string path = Path.ChangeExtension(Path.Combine(folder, layer.SymbologyKey), "lyrx");
        //            if (File.Exists(path))
        //            {
        //                return new FileInfo(path);
        //            }
        //        }
        //    }

        //    return null;
        //}

        public async Task RemoveGroupLayer(TreeViewItemModel item, ILayerContainer parent)
        {
            await QueuedTask.Run(async () =>
           {
               if (!string.IsNullOrEmpty(item.MapLayerUri))
               {
                   Layer layer = MapView.Active.Map.FindLayer(item.MapLayerUri);
                   if (layer != null)
                       MapView.Active.Map.RemoveLayer(layer);
               }
           });
        }
    }
}