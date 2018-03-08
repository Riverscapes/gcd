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
        private const string SurveysGroupLayer = "Surveys";
        private const string AssociatedSurfacesGroupLayer = "Associated Surfaces";
        private const string ErrorSurfacesGroupLayer = "Error Surfaces";
        private const string AnalysesGroupLayer = "Analyses";
        private const string MasksGroupLayer = "Masks";
        private const string RefSurfGroupLayer = "Reference Surfaces";
        private const string ProfileRouteGroupLayer = "Profile Routes";

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
            IGroupLayer pSurveysGrpLyr = ArcMapUtilities.GetGroupLayer(SurveysGroupLayer, pInputsGrpLyr);
            return ArcMapUtilities.GetGroupLayer(dem.Name, pSurveysGrpLyr);
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

        private IGroupLayer AddReferenceSurfaceGroupLayer(Surface surf)
        {
            IGroupLayer pInputsGrpLyr = AddInputsGroupLayer();
            IGroupLayer pRefSurfGrpLyr = ArcMapUtilities.GetGroupLayer(RefSurfGroupLayer, pInputsGrpLyr);
            return ArcMapUtilities.GetGroupLayer(surf.Name, pRefSurfGrpLyr);
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

            if (dem.Hillshade != null && dem.Hillshade.Raster.GISFileInfo.Exists)
            {
                AddRasterLayer(dem.Hillshade.Raster, null, dem.Name + " Hillshade", pSurveyLyr, "Aspect", -1, ExpandLegend: false);
                fDEMTransparency = DefaultTransparency;
            }

            IRasterRenderer demRenderer = RasterSymbolization.CreateDEMColorRamp(dem.Raster);
            AddRasterLayer(dem.Raster, demRenderer, dem.Name, pSurveyLyr, dem.LayerHeader, fDEMTransparency);
        }

        public void AddReferenceSurface(Surface surf)
        {
            short fDEMTransparency = (short)-1;
            IGroupLayer pSurveyLyr = AddReferenceSurfaceGroupLayer(surf);

            if (surf.Hillshade != null && surf.Hillshade.Raster.GISFileInfo.Exists)
            {
                AddRasterLayer(surf.Hillshade.Raster, null, surf.Name + " Hillshade", pSurveyLyr, "Aspect", -1, ExpandLegend: false);
                fDEMTransparency = DefaultTransparency;
            }

            IRasterRenderer demRenderer = RasterSymbolization.CreateDEMColorRamp(surf.Raster);
            AddRasterLayer(surf.Raster, demRenderer, surf.Name, pSurveyLyr, surf.LayerHeader, fDEMTransparency);
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

        public void AddMask(GCDCore.Project.Masks.AttributeFieldMask mask)
        {
            IGroupLayer pProjLyr = AddProjectGroupLayer();
            IGroupLayer pGrpLayer = ArcMapUtilities.GetGroupLayer(MasksGroupLayer, pProjLyr);

            IFeatureRenderer pRenderer = null;
            string queryFilter = string.Empty;
            string labelField = string.Empty;
            if (mask is GCDCore.Project.Masks.RegularMask)
            {
                GCDCore.Project.Masks.RegularMask rMask = mask as GCDCore.Project.Masks.RegularMask;

                pRenderer = VectorSymbolization.GetRegularMaskRenderer(rMask) as IFeatureRenderer;

                // Create a definition query if some features are not included

                if (rMask._Items.Any(x => !x.Include))
                {
                    queryFilter = string.Format("\"{0}\" IN ('{1}')", mask._Field, string.Join("','", rMask._Items.Where(x => x.Include).Select(y => y.FieldValue)));
                }
            }
            else if (mask is GCDCore.Project.Masks.DirectionalMask)
            {
                GCDCore.Project.Masks.DirectionalMask dirMask = mask as GCDCore.Project.Masks.DirectionalMask;
                // Directional mask. Black outline with labels
                pRenderer = VectorSymbolization.GetDirectionalMaskRenderer(dirMask) as IFeatureRenderer;

                labelField = string.IsNullOrEmpty(dirMask.LabelField) ? dirMask._Field : dirMask.LabelField;
            }

            VectorSymbolization.AddToMapVector(mask.Vector.GISFileInfo, mask.Name, pGrpLayer, mask._Field, pRenderer, queryFilter, labelField);
        }

        public void AddAOI(GCDCore.Project.Masks.AOIMask mask)
        {
            IGroupLayer pProjLyr = AddProjectGroupLayer();
            IGroupLayer pGrpLayer = ArcMapUtilities.GetGroupLayer(MasksGroupLayer, pProjLyr);

            IFeatureRenderer pRenderer = VectorSymbolization.GetAOIRenderer(mask) as IFeatureRenderer;

            VectorSymbolization.AddToMapVector(mask.Vector.GISFileInfo, mask.Name, pGrpLayer, string.Empty, pRenderer, string.Empty, string.Empty);
        }

        public void AddProfileRoute(GCDCore.Project.ProfileRoutes.ProfileRoute route)
        {
            IGroupLayer pProjLyr = AddProjectGroupLayer();
            IGroupLayer pGrpLayer = ArcMapUtilities.GetGroupLayer(ProfileRouteGroupLayer, pProjLyr);

            IFeatureRenderer pRenderer = null;// VectorSymbolization.GetAOIRenderer(mask) as IFeatureRenderer;

            VectorSymbolization.AddToMapVector(route.Vector.GISFileInfo, route.Name, pGrpLayer, string.Empty, pRenderer, string.Empty, string.Empty);
        }

        public void AddDoD(GCDProjectRasterItem dod)
        {
            Raster gDoDRaster = dod.Raster;

            IGroupLayer pAnalGrpLayer = AddAnalysesGroupLayer();
            short dTransparency = -1;
            if (GCDCore.Properties.Settings.Default.TransparencyAnalysesLayers)
            {
                dTransparency = GCDCore.Properties.Settings.Default.AutoTransparencyValue;
            }

            IRasterRenderer rasterRenderer = RasterSymbolization.CreateDoDClassifyRenderer(gDoDRaster, 20);
            string sHeader = string.Format("Elevation Difference ({0})", UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            AddRasterLayer(gDoDRaster, rasterRenderer, dod.Name, pAnalGrpLayer, sHeader, dTransparency);
        }

        private IGroupLayer AddAnalysesGroupLayer()
        {
            IGroupLayer pProjLyr = AddProjectGroupLayer();
            IGroupLayer pAnalGrpLyr = ArcMapUtilities.GetGroupLayer(AnalysesGroupLayer, pProjLyr);
            return pAnalGrpLyr;
        }

        public void AddErrSurface(ErrorSurface errRow)
        {
            IGroupLayer pErrGrpLyr = null;

            if (errRow.Surf is DEMSurvey)
            {
                pErrGrpLyr = AddErrorSurfacesGroupLayer(errRow.Surf as DEMSurvey);
            }
            else
            {
                pErrGrpLyr = AddReferenceErrorSurfacesGroupLayer(errRow.Surf);
            }

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

        private IGroupLayer AddReferenceErrorSurfacesGroupLayer(Surface surf)
        {
            IGroupLayer pSurveyGrpLyr = AddReferenceSurfaceGroupLayer(surf);
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
