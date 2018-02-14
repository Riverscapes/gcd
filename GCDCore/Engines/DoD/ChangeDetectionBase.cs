using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public abstract class ChangeDetectionEngineBase : EngineBase
    {
        protected Surface NewSurface;
        protected Surface OldSurface;

        public ChangeDetectionEngineBase(Surface newSurface, Surface oldSurface)
            : base()
        {
            if (!newSurface.Raster.Extent.HasOverlap(oldSurface.Raster.Extent))
            {
                Exception ex = new Exception("The two rasters do not overlap.");
                ex.Data["New DEM Path"] = newSurface.Raster.GISFileInfo.ToString();
                ex.Data["New DEM Extent"] = newSurface.Raster.Extent.ToString();
                ex.Data["Old DEM Path"] = oldSurface.Raster.GISFileInfo.ToString();
                ex.Data["Old DEM Extent"] = oldSurface.Raster.Extent.ToString();
                throw ex;
            }

            NewSurface = newSurface;
            OldSurface = oldSurface;
        }

        public DoDBase Calculate(string dodName, DirectoryInfo analysisFolder, bool bBuildPyramids, UnitGroup units)
        {
            FileInfo rawDoDPath = ProjectManager.OutputManager.RawDoDPath(analysisFolder);
            FileInfo thrDoDPath = ProjectManager.OutputManager.ThrDoDPath(analysisFolder);
            FileInfo rawHstPath = ProjectManager.OutputManager.RawHistPath(analysisFolder);
            FileInfo thrHstPath = ProjectManager.OutputManager.ThrHistPath(analysisFolder);
            FileInfo sumXMLPath = ProjectManager.OutputManager.SummaryXMLPath(analysisFolder);

            analysisFolder.Create();

            // Subtract the new and old rasters to produce the raw DoD
            Raster rawDoD = RasterOperators.Subtract(NewSurface.Raster, OldSurface.Raster, rawDoDPath);

            // Build pyraminds
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw, rawDoDPath);

            // Calculate the raw histogram
            Histogram rawHisto = RasterOperators.BinRaster(rawDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the raw histogram
            WriteHistogram(rawHisto, rawHstPath);

            // Call the polymorphic method to threshold the DoD depending on the thresholding method
            Raster thrDoD = ThresholdRawDoD(rawDoD, thrDoDPath);

            // Build pyraminds
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded, thrDoDPath);

            // Calculate the thresholded histogram
            Histogram thrHisto = RasterOperators.BinRaster(thrDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the thresholded histogram
            WriteHistogram(thrHisto, thrHstPath);

            // Calculate the change statistics and write the output files
            DoDStats changeStats = CalculateChangeStats(rawDoD, thrDoD, units);
            GenerateSummaryXML(changeStats, sumXMLPath);
            GenerateChangeBarGraphicFiles(analysisFolder, changeStats, 0, 0);
            GenerateHistogramGraphicFiles(analysisFolder, rawHisto, thrHisto, 1920, 1080);

            return GetDoDResult(dodName, changeStats, rawDoD, thrDoD, new HistogramPair(rawHisto, rawHstPath, thrHisto, thrHstPath), sumXMLPath);
        }

        protected abstract Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath);

        protected abstract DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitGroup units);

        protected abstract DoDBase GetDoDResult(string dodName, DoDStats changeStats, Raster rawDoD, Raster thrDoD, HistogramPair histograms, FileInfo summaryXML);
    }
}