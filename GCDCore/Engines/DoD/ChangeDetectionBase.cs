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
        protected Project.Masks.AOIMask AOIMask;

        public ChangeDetectionEngineBase(Surface newSurface, Surface oldSurface, Project.Masks.AOIMask aoi)
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
            AOIMask = aoi;
        }

        public DoDBase Calculate(string dodName, DirectoryInfo analysisFolder, bool bBuildPyramids, UnitGroup units)
        {
            FileInfo rawDoDPath = BuildFilePath(analysisFolder, "raw", ProjectManager.RasterExtension);
            FileInfo thrDoDPath = BuildFilePath(analysisFolder, "thresh", ProjectManager.RasterExtension);
            FileInfo errDoDPath = BuildFilePath(analysisFolder, "threrr", ProjectManager.RasterExtension);
            FileInfo rawHstPath = BuildFilePath(analysisFolder, "raw", "csv"); ;
            FileInfo thrHstPath = BuildFilePath(analysisFolder, "thresh", "csv");
            FileInfo sumXMLPath = BuildFilePath(analysisFolder, "summary", "xml");

            analysisFolder.Create();

            // Subtract the new and old rasters to produce the raw DoD
            Raster rawDoD;
            if (AOIMask == null)
            {
                rawDoD = RasterOperators.Subtract(NewSurface.Raster, OldSurface.Raster, rawDoDPath, ProjectManager.OnProgressChange);
            }
            else
            {
                rawDoD = RasterOperators.SubtractWithMask(NewSurface.Raster, OldSurface.Raster, AOIMask.Vector, rawDoDPath, ProjectManager.OnProgressChange);
            }

            // Build pyraminds
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw, rawDoDPath);

            // Calculate the raw histogram
            Histogram rawHisto = RasterOperators.BinRaster(rawDoD, DEFAULTHISTOGRAMNUMBER, ProjectManager.OnProgressChange);

            // Write the raw histogram
            WriteHistogram(rawHisto, rawHstPath);

            // Call the polymorphic method to threshold the DoD depending on the thresholding method
            Raster thrDoD = ThresholdRawDoD(rawDoD, thrDoDPath);

            // Build pyraminds for the thresholded raster
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded, thrDoDPath);

            // Calculate the thresholded histogram
            Histogram thrHisto = RasterOperators.BinRaster(thrDoD, DEFAULTHISTOGRAMNUMBER, ProjectManager.OnProgressChange);

            // Write the thresholded histogram
            WriteHistogram(thrHisto, thrHstPath);

            // Calculate the change statistics and write the output files
            DoDStats changeStats = CalculateChangeStats(rawDoD, thrDoD, units);
            GenerateSummaryXML(changeStats, sumXMLPath);
            GenerateChangeBarGraphicFiles(analysisFolder, changeStats, 0, 0);
            GenerateHistogramGraphicFiles(analysisFolder, rawHisto, thrHisto, 1920, 1080);

            // Calculate the thresholded error raster
            Raster errDoD = GenerateErrorRaster(errDoDPath);

            return GetDoDResult(dodName, changeStats, rawDoD, thrDoD, errDoD, new HistogramPair(rawHisto, rawHstPath, thrHisto, thrHstPath), sumXMLPath);
        }

        protected abstract Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath);

        protected abstract Raster GenerateErrorRaster(FileInfo errDoDPath);

        protected abstract DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitGroup units);

        protected abstract DoDBase GetDoDResult(string dodName, DoDStats changeStats, Raster rawDoD, Raster thrDoD, Raster errDoDPath, HistogramPair histograms, FileInfo summaryXML);

        protected FileInfo BuildFilePath(DirectoryInfo folder, string RasterFileName, string extension)
        {
            string sPath = Path.Combine(folder.FullName, RasterFileName);
            sPath = Path.ChangeExtension(sPath, extension);
            return new FileInfo(sPath);
        }
    }
}