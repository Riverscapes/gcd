using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;
using GCDCore.Visualization;
using System.Globalization;

namespace GCDCore.ChangeDetection
{
    public abstract class ChangeDetectionEngineBase : Engine
    {
        protected Raster NewDEM;
        protected Raster OldDEM;

        public ChangeDetectionEngineBase(DirectoryInfo folder, Raster gNewDEM, Raster gOldDEM)
            : base(folder)
        {
            if (!gNewDEM.Extent.HasOverlap(gOldDEM.Extent))
            {
                Exception ex = new Exception("The two rasters do not overlap.");
                ex.Data["New DEM Path"] = gNewDEM.GISFileInfo;
                ex.Data["New DEM Extent"] = gNewDEM.Extent.ToString();
                ex.Data["Old DEM Path"] = gOldDEM.GISFileInfo;
                ex.Data["Old DEM Extent"] = gOldDEM.Extent.ToString();
                throw ex;
            }

            NewDEM = gNewDEM;
            OldDEM = gOldDEM;
        }

        public DoDResult Calculate(bool bBuildPyramids, UnitsNet.Area cellArea, UnitGroup units)
        {
            FileInfo rawDoDPath = Project.ProjectManagerBase.OutputManager.RawDoDPath(AnalysisFolder);
            FileInfo thrDoDPath = Project.ProjectManagerBase.OutputManager.ThrDoDPath(AnalysisFolder);
            FileInfo rawHstPath = Project.ProjectManagerBase.OutputManager.RawHistPath(AnalysisFolder);
            FileInfo thrHstPath = Project.ProjectManagerBase.OutputManager.ThrHistPath(AnalysisFolder);
            FileInfo sumXMLPath = Project.ProjectManagerBase.OutputManager.SummaryXMLPath(AnalysisFolder);

            AnalysisFolder.Create();

            // Subtract the new and old rasters to produce the raw DoD
            Raster rawDoD = RasterOperators.Subtract(NewDEM, OldDEM, rawDoDPath);

            // Build pyraminds
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw, rawDoDPath);

            // Calculate the raw histogram
            Histogram rawHisto = RasterOperators.BinRaster(rawDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the raw histogram
            WriteHistogram(rawHisto, rawHstPath);

            // Call the polymorphic method to threshold the DoD depending on the thresholding method
            Raster thrDoD = ThresholdRawDoD(rawDoD, thrDoDPath);

            // Build pyraminds
            //Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded, thrDoDPath);

            // Calculate the thresholded histogram
            Histogram thrHisto = RasterOperators.BinRaster(thrDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the thresholded histogram
            WriteHistogram(thrHisto, thrHstPath);

            // Calculate the change statistics and write the output files
            DoDStats changeStats = CalculateChangeStats(rawDoD, thrDoD, cellArea, units);
            GenerateSummaryXML(changeStats, sumXMLPath);
            GenerateChangeBarGraphicFiles(changeStats, 0, 0);
            GenerateHistogramGraphicFiles(rawHisto, thrHisto, 0, 0);

            return GetDoDResult(changeStats, rawDoDPath, thrDoDPath, rawHstPath, rawHisto, thrHstPath, thrHisto, units);
        }

        protected abstract Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath);

        protected abstract DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units);

        protected abstract DoDResult GetDoDResult(DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHistPath, Histogram rawHist, FileInfo thrHistPath, Histogram thrHist, UnitGroup units);
    }
}