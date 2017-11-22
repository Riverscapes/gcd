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
        protected DEMSurvey NewDEM;
        protected DEMSurvey OldDEM;

        public ChangeDetectionEngineBase(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM)
            : base(name, folder)
        {
            if (!newDEM.Raster.Raster.Extent.HasOverlap(oldDEM.Raster.Raster.Extent))
            {
                Exception ex = new Exception("The two rasters do not overlap.");
                ex.Data["New DEM Path"] = newDEM.Raster.RasterPath.ToString();
                ex.Data["New DEM Extent"] = newDEM.Raster.Raster.Extent.ToString();
                ex.Data["Old DEM Path"] = oldDEM.Raster.RasterPath.ToString();
                ex.Data["Old DEM Extent"] = oldDEM.Raster.Raster.Extent.ToString();
                throw ex;
            }

            NewDEM = newDEM;
            OldDEM = oldDEM;
        }

        public DoDBase Calculate(bool bBuildPyramids, UnitsNet.Area cellArea, UnitGroup units)
        {
            FileInfo rawDoDPath = ProjectManager.OutputManager.RawDoDPath(AnalysisFolder);
            FileInfo thrDoDPath = ProjectManager.OutputManager.ThrDoDPath(AnalysisFolder);
            FileInfo rawHstPath = ProjectManager.OutputManager.RawHistPath(AnalysisFolder);
            FileInfo thrHstPath = ProjectManager.OutputManager.ThrHistPath(AnalysisFolder);
            FileInfo sumXMLPath = ProjectManager.OutputManager.SummaryXMLPath(AnalysisFolder);

            AnalysisFolder.Create();

            // Subtract the new and old rasters to produce the raw DoD
            Raster rawDoD = RasterOperators.Subtract(NewDEM.Raster.Raster, OldDEM.Raster.Raster, rawDoDPath);

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
            DoDStats changeStats = CalculateChangeStats(rawDoD, thrDoD, cellArea, units);
            GenerateSummaryXML(changeStats, sumXMLPath);
            GenerateChangeBarGraphicFiles(changeStats, 0, 0);
            GenerateHistogramGraphicFiles(rawHisto, thrHisto, 0, 0);

            return GetDoDResult(changeStats, rawDoD, thrDoD, new HistogramPair(rawHisto, rawHstPath, thrHisto, thrHstPath));
        }

        protected abstract Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath);

        protected abstract DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units);

        protected abstract DoDBase GetDoDResult(DoDStats changeStats, Raster rawDoD, Raster thrDoD, HistogramPair histograms);
    }
}