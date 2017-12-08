using ESRI.ArcGIS.Carto;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDAddIn
{
    public class GCDArcMapManager
    {
        private readonly double DefaultTransparency;
        private readonly IMapDocument MapDocument;

        // These constants are the names for the group layers that should be greated
        // and the appropriate rasters created inside them
        private const string InputsGroupLayer = "Inputs";
        private const string AssociatedSurfacesGroupLayer = "Associated Surfaces";
        private const string ErrorSurfacesGroupLayer = "Error Surfaces";
        private const string AnalysesGroupLayer = "Analyses";

        public GCDArcMapManager(double fDefaultDEMTransparency = 40, IMapDocument pMapDocument = null)
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
                AddDoD(dod);
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
        public IRasterLayer AddDEM(DEMSurvey dem)
        {
            double fDEMTransparency = -1;
            IGroupLayer pSurveyLyr = AddSurveyGroupLayer(dem);

            IRasterLayer pHSLayer = null;
            FileInfo hillshade = ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(dem.Name);
            if (hillshade.Exists)
            {
                pHSLayer = ArcMapUtilities.AddToMapRaster(hillshade, dem.Name + " HillShade", pSurveyLyr);
                fDEMTransparency = DefaultTransparency;
            }

            IRasterLayer pDEMLyr = ArcMapUtilities.AddToMapRaster(dem.Raster.GISFileInfo, dem.Name, pSurveyLyr, fDEMTransparency);

            // Collapse the Hillshade legend in the TOC
            if (pHSLayer is IRasterLayer)
            {
                ((ILegendGroup)((ILegendInfo)pHSLayer).LegendGroup[0]).Visible = false;
            }

            return pDEMLyr;
        }

        private IGroupLayer AddAssociatedSurfaceGroupLayer(DEMSurvey dem)
        {
            IGroupLayer pSurveyGrpLyr = AddSurveyGroupLayer(dem);
            return ArcMapUtilities.GetGroupLayer(AssociatedSurfacesGroupLayer, pSurveyGrpLyr);
        }

        public IRasterLayer AddAssociatedSurface(AssocSurface assocRow)
        {
            IGroupLayer pAssocGrpLyr = AddAssociatedSurfaceGroupLayer(assocRow.DEM);
            IRasterLayer pAssocLyr = null;

            double dTransparency = GCDCore.Properties.Settings.Default.TransparencyAssociatedLayers ? GCDCore.Properties.Settings.Default.AutoTransparencyValue : -1;

            switch (assocRow.AssocSurfaceType)
            {
                case AssocSurface.AssociatedSurfaceTypes.InterpolationError:

                    if (!GCDCore.Properties.Settings.Default.ApplyComparativeSymbology)
                    {
                        IRasterRenderer rasterRenderer = RasterSymbolization.CreateClassifyRenderer(assocRow.Raster, 11, "Slope");
                        pAssocLyr = ArcMapUtilities.AddRasterLayer(assocRow.Raster, rasterRenderer, assocRow.Name, pAssocGrpLyr, assocRow.LayerHeader, dTransparency);
                    }

                    break;

                case AssocSurface.AssociatedSurfaceTypes.PointQuality3D:

                    if (!GCDCore.Properties.Settings.Default.ApplyComparativeSymbology)
                    {
                        IRasterRenderer rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Precipitation", true);
                        pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                    }
                    break;

                case AssocSurface.AssociatedSurfaceTypes.PointDensity:

                    if (!GCDCore.Properties.Settings.Default.ApplyComparativeSymbology)
                    {
                        if (gAssociatedRaster.Maximum <= 2 & gAssociatedRaster.Maximum > 0.25)
                        {
                            IRasterRenderer rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Green to Blue", 1.1, true);
                            pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                        }
                        else
                        {
                            IRasterRenderer rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Green to Blue", true);
                            pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                        }
                    }
                    break;

                case AssocSurface.AssociatedSurfaceTypes.GrainSizeStatic:

                    NumberFormatting.LinearUnits eUnits = NumberFormatting.GetLinearUnitsFromString(GCDCore.Project.ProjectManager.Project.Units.VertUnit.ToString());
                    IRasterRenderer rasterRenderer = RasterSymbolization.CreateGrainSizeStatisticColorRamp(gAssociatedRaster, eUnits);
                    ILayer pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                    return pAssocLyr;

                case AssocSurface.AssociatedSurfaceTypes.Roughness:

                    IRasterRenderer rasterRenderer = GCD.RasterSymbolization.CreateRoughnessColorRamp(gAssociatedRaster);
                    ILayer pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                    return pAssocLyr;

                case AssocSurface.AssociatedSurfaceTypes.SlopeDegree:

                    IRasterRenderer rasterRenderer = GCD.RasterSymbolization.CreateSlopeDegreesColorRamp(gAssociatedRaster);
                    ILayer pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                    return pAssocLyr;

                case AssocSurface.AssociatedSurfaceTypes.SlopePercent:

                    IRasterRenderer rasterRenderer = GCD.RasterSymbolization.CreateSlopePrecentRiseColorRamp(gAssociatedRaster);
                    ILayer pAssocLyr = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency);
                    return pAssocLyr;

                default:

                    string sSymbology = GISDataStructures.RasterGCD.GetSymbologyLayerFile(eType);
                    IRasterLayer pAssocLyr = AddToMapRaster(sRasterPath, assocRow.Name, pAssocGrpLyr, sSymbology, dTransparency, sHeader);
                    return pAssocLyr;
            }
        }
    }
}
