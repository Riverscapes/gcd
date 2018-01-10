using ESRI.ArcGIS.Carto;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using GCDConsoleLib;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;

namespace GCDAddIn
{
    public class GCDArcMapManager
    {
        private readonly short DefaultTransparency;
        private readonly IMapDocument MapDocument;

        // These constants are the names for the group layers that should be greated
        // and the appropriate rasters created inside them
        private const string InputsGroupLayer = "Inputs";
        private const string AssociatedSurfacesGroupLayer = "Associated Surfaces";
        private const string ErrorSurfacesGroupLayer = "Error Surfaces";
        private const string AnalysesGroupLayer = "Analyses";

        public GCDArcMapManager(short fDefaultDEMTransparency = 40, IMapDocument pMapDocument = null)
        {
            DefaultTransparency = fDefaultDEMTransparency;

            if (pMapDocument == null)
                MapDocument = (IMapDocument)ArcMap.Document;
            else
                MapDocument = pMapDocument;
        }

        private IGroupLayer AddProjectGroupLayer()
        {
            return ArcMapUtilities.GetGroupLayer(ProjectManager.Project.Name);
        }

        public IGroupLayer AddProject()
        {
            IGroupLayer pProjectGrpLyr = AddProjectGroupLayer();
            foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
            {
                AddSurvey(dem);
            }

            foreach (DoDBase dod in ProjectManager.Project.DoDs.Values)
            {
                AddDoD(dod.RawDoD);
            }

            return pProjectGrpLyr;
        }

        private IGroupLayer AddInputsGroupLayer()
        {
            IGroupLayer pProjectGrpLyr = AddProjectGroupLayer();
            return ArcMapUtilities.GetGroupLayer(InputsGroupLayer, pProjectGrpLyr);
        }

        private IGroupLayer AddSurveyGroupLayer(DEMSurvey dem)
        {
            IGroupLayer pInputsGrpLyr = AddInputsGroupLayer();
            return ArcMapUtilities.GetGroupLayer(dem.Name, pInputsGrpLyr);
        }

        public IGroupLayer AddSurvey(DEMSurvey dem)
        {
            IGroupLayer pSurveyLyr = AddSurveyGroupLayer(dem);
            AddDEM(dem);

            foreach (AssocSurface assoc in dem.AssocSurfaces)
            {
                AddAssociatedSurface(assoc);
            }

            foreach (ErrorSurface err in dem.ErrorSurfaces)
            {
                AddErrSurface(err);
            }

            return pSurveyLyr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="demRow"></param>
        /// <returns></returns>
        /// <remarks>Note: Add the hillshade first so that it appear UNDER the DEM in the TOC</remarks>
        public void AddDEM(DEMSurvey dem)
        {
            short fDEMTransparency = (short)-1;
            IGroupLayer pSurveyLyr = AddSurveyGroupLayer(dem);

            FileInfo hillshade = ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(dem.Name);
            if (hillshade.Exists)
            {
                Raster gHillshade = new Raster(hillshade);
                AddRasterLayer(gHillshade, null, dem.Name + " Hillshade", pSurveyLyr, "Aspect", -1, ExpandLegend: false);
                fDEMTransparency = DefaultTransparency;
            }

            IRasterRenderer demRenderer = RasterSymbolization.CreateDEMColorRamp(dem.Raster);
            AddRasterLayer(dem.Raster, demRenderer, dem.Name, pSurveyLyr, dem.LayerHeader, fDEMTransparency);

            // Collapse the Hillshade legend in the TOC
            //if (pHSLayer is IRasterLayer)
            //{
            //    ((ILegendGroup)((ILegendInfo)pHSLayer).LegendGroup[0]).Visible = false;
            //}

            //if (pDEMLyr is IDataLayer2)
            //{
            //    ((IDataLayer2)pDEMLyr).Disconnect();
            //}

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pDEMLyr);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pHSLayer);
        }

        private IGroupLayer AddAssociatedSurfaceGroupLayer(DEMSurvey dem)
        {
            IGroupLayer pSurveyGrpLyr = AddSurveyGroupLayer(dem);
            return ArcMapUtilities.GetGroupLayer(AssociatedSurfacesGroupLayer, pSurveyGrpLyr);
        }

        public void AddAssociatedSurface(AssocSurface assocRow)
        {
            IGroupLayer pAssocGrpLyr = AddAssociatedSurfaceGroupLayer(assocRow.DEM);

            short dTransparency = GCDCore.Properties.Settings.Default.TransparencyAssociatedLayers ? GCDCore.Properties.Settings.Default.AutoTransparencyValue : (short)-1;

            IRasterRenderer rasterRenderer = null;
            switch (assocRow.AssocSurfaceType)
            {
                case AssocSurface.AssociatedSurfaceTypes.InterpolationError:

                    if (!GCDCore.Properties.Settings.Default.ApplyComparativeSymbology)
                    {
                        rasterRenderer = RasterSymbolization.CreateClassifyRenderer(assocRow.Raster, 11, "Slope");
                    }
                    break;

                case AssocSurface.AssociatedSurfaceTypes.PointQuality3D:

                    if (!GCDCore.Properties.Settings.Default.ApplyComparativeSymbology)
                    {
                        rasterRenderer = RasterSymbolization.CreateClassifyRenderer(assocRow.Raster, 11, "Precipitation", true);
                    }
                    break;

                case AssocSurface.AssociatedSurfaceTypes.PointDensity:
                    assocRow.Raster.ComputeStatistics();
                    decimal rasterMax = assocRow.Raster.GetStatistics()["max"];

                    if (rasterMax <= 2 & rasterMax > 0.25m)
                    {
                        rasterRenderer = RasterSymbolization.CreateClassifyRenderer(assocRow.Raster, 11, "Green to Blue", 1.1, true);
                   }
                    else
                    {
                        rasterRenderer = RasterSymbolization.CreateClassifyRenderer(assocRow.Raster, 11, "Green to Blue", true);
                    }
                    break;

                case AssocSurface.AssociatedSurfaceTypes.GrainSizeStatic:

                    rasterRenderer = RasterSymbolization.CreateGrainSizeStatisticColorRamp(assocRow.Raster, ProjectManager.Project.Units.VertUnit);
                    break;

                case AssocSurface.AssociatedSurfaceTypes.Roughness:
                    rasterRenderer = RasterSymbolization.CreateRoughnessColorRamp(assocRow.Raster);
                    break;

                case AssocSurface.AssociatedSurfaceTypes.SlopeDegree:
                    rasterRenderer = RasterSymbolization.CreateSlopeDegreesColorRamp(assocRow.Raster);
                    break;

                case AssocSurface.AssociatedSurfaceTypes.SlopePercent:
                    rasterRenderer = RasterSymbolization.CreateSlopePrecentRiseColorRamp(assocRow.Raster);
                    break;
            }

            AddRasterLayer(assocRow.Raster, rasterRenderer, assocRow.Name, pAssocGrpLyr, assocRow.LayerHeader, dTransparency);
        }

        public void AddDoD(GCDProjectRasterItem dod, bool bThresholded = true)
        {
            Raster gDoDRaster;
            string sLayerName = dod.Name;

            if (bThresholded)
            {
                gDoDRaster = dod.Raster;
                sLayerName += " (Thresholded)";
            }
            else
            {
                gDoDRaster = dod.Raster;
                sLayerName += " (Raw)";
            }

            IGroupLayer pAnalGrpLayer = AddAnalysesGroupLayer();
            short dTransparency = -1;
            if (GCDCore.Properties.Settings.Default.TransparencyAnalysesLayers)
            {
                dTransparency = GCDCore.Properties.Settings.Default.AutoTransparencyValue;
            }

            IRasterRenderer rasterRenderer = RasterSymbolization.CreateDoDClassifyRenderer(gDoDRaster, 20);
            string sHeader = string.Format("Elevation Difference ({0})", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            AddRasterLayer(gDoDRaster, rasterRenderer, sLayerName, pAnalGrpLayer, sHeader, dTransparency);
        }

        private IGroupLayer AddAnalysesGroupLayer()
        {
            IGroupLayer pProjLyr = AddProjectGroupLayer();
            IGroupLayer pAnalGrpLyr = ArcMapUtilities.GetGroupLayer(AnalysesGroupLayer, pProjLyr);
            return pAnalGrpLyr;
        }

        public void AddErrSurface(ErrorSurface errRow)
        {
            IGroupLayer pErrGrpLyr = AddErrorSurfacesGroupLayer(errRow.DEM);
            string sHeader = string.Format("Error ({0})", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));

            short dTransparency = -1;
            if (GCDCore.Properties.Settings.Default.TransparencyErrorLayers)
            {
                dTransparency = GCDCore.Properties.Settings.Default.AutoTransparencyValue;
            }

            errRow.Raster.ComputeStatistics();
            Dictionary<string, decimal> stats = errRow.Raster.GetStatistics();
            double rMin = (double)stats["min"];
            double rMax = (double)stats["max"];

            if (rMin == rMax)
            {
                IRasterRenderer rasterRenderer = RasterSymbolization.CreateESRIDefinedContinuousRenderer(errRow.Raster, 1, "Partial Spectrum");
                AddRasterLayer(errRow.Raster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency);
            }
            else if (rMax <= 1 & rMax > 0.25)
            {
                IRasterRenderer rasterRenderer = RasterSymbolization.CreateClassifyRenderer(errRow.Raster, 11, "Partial Spectrum", 1.1);
                AddRasterLayer(errRow.Raster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency);
            }
            else
            {
                IRasterRenderer rasterRenderer = RasterSymbolization.CreateClassifyRenderer(errRow.Raster, 11, "Partial Spectrum");
                AddRasterLayer(errRow.Raster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency);
            }
        }

        private IGroupLayer AddErrorSurfacesGroupLayer(DEMSurvey dem)
        {
            IGroupLayer pSurveyGrpLyr = AddSurveyGroupLayer(dem);
            IGroupLayer pErrGrpLyr = ArcMapUtilities.GetGroupLayer(ErrorSurfacesGroupLayer, pSurveyGrpLyr);
            return pErrGrpLyr;
        }

        private static void AddRasterLayer(Raster gRaster, IRasterRenderer rasterRenderer, string sRasterName, IGroupLayer pGrpLyr, string sHeader = null, short fTransparency = -1, bool ExpandLegend = true)
        {
            if (pGrpLyr != null)
            {
                IRasterLayer pResultLayer = ArcMapUtilities.IsRasterLayerInGroupLayer(gRaster.GISFileInfo, pGrpLyr);
                if (pResultLayer is ILayer)
                {
                    return;
                }
            }

            IRasterLayer rasterLayer = new RasterLayer();
            IRasterDataset pRDS = ArcMapUtilities.GetRasterDataset(gRaster);
            rasterLayer.CreateFromDataset(pRDS);
            if (rasterRenderer != null)
            {
                rasterLayer.Renderer = rasterRenderer;
            }

            if (rasterLayer != null)
            {
                IMapLayers pMapLayers = (IMapLayers)ArcMap.Document.FocusMap;
                if (!string.IsNullOrEmpty(sRasterName))
                {
                    rasterLayer.Name = sRasterName;
                }

                if (!string.IsNullOrEmpty(sHeader))
                {
                    ESRI.ArcGIS.Carto.ILegendInfo pLegend = (ESRI.ArcGIS.Carto.ILegendInfo)rasterLayer;
                    pLegend.LegendGroup[0].Heading = sHeader;
                }

                if (fTransparency >= 0)
                {
                    ILayerEffects pLayerEffects = (ILayerEffects)rasterLayer;
                    pLayerEffects.Transparency = fTransparency;
                }

                if (pGrpLyr == null)
                {
                    pMapLayers.InsertLayer(rasterLayer, false, 0);
                }
                else
                {
                    pMapLayers.InsertLayerInGroup(pGrpLyr, rasterLayer, false, 0);
                }

                // Collapse or expand the legend in the ToC (e.g. Hillshade should be collapsed)
                ((ILegendGroup)((ILegendInfo)rasterLayer).LegendGroup[0]).Visible = ExpandLegend;
            }

            int refsLeft = 0;
            do
            {
                refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pRDS);
            }
            while (refsLeft > 0);

            refsLeft = 0;
            do
            {
                refsLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(rasterLayer);
            }
            while (refsLeft > 0);
        }
    }
}
