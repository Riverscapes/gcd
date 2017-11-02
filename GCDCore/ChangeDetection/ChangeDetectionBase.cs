using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDCore.Visualization;

namespace GCDCore.ChangeDetection
{
    public abstract class ChangeDetectionEngineBase
    {
        private const int DEFAULTHISTOGRAMNUMBER = 100;

        public readonly DirectoryInfo AnalysisFolder;
        protected Raster NewDEM;
        protected Raster OldDEM;

        protected UnitsNet.Units.LengthUnit LinearUnits
        {
            get { return NewDEM.Proj.LinearUnit; }
        }

        private DirectoryInfo FiguresFolder
        {
            get { return new DirectoryInfo(Path.Combine(AnalysisFolder.FullName, "Figures")); }
        }

        public ChangeDetectionEngineBase(DirectoryInfo folder, ref Raster gNewDEM, ref Raster gOldDEM)
        {
            AnalysisFolder = folder;

            if (!gNewDEM.Extent.HasOverlap(ref gOldDEM.Extent))
            {
                Exception ex = new Exception("The two rasters do not overlap.");
                ex.Data["New DEM Path"] = gNewDEM.FilePath;
                ex.Data["New DEM Extent"] = gNewDEM.Extent.ToString();
                ex.Data["Old DEM Path"] = gOldDEM.FilePath;
                ex.Data["Old DEM Extent"] = gOldDEM.Extent.ToString();
            }

            NewDEM = gNewDEM;
            OldDEM = gOldDEM;
        }

        public DoDResult Calculate(bool bBuildPyramids)
        {
            FileInfo rawDoDPath = Project.ProjectManagerBase.OutputManager.RawDoDPath(AnalysisFolder);
            FileInfo thrDoDPath = Project.ProjectManagerBase.OutputManager.ThrDoDPath(AnalysisFolder);
            FileInfo rawHstPath = Project.ProjectManagerBase.OutputManager.RawHistPath(AnalysisFolder);
            FileInfo thrHstPath = Project.ProjectManagerBase.OutputManager.ThrHistPath(AnalysisFolder);
            FileInfo sumXMLPath = Project.ProjectManagerBase.OutputManager.SummaryXMLPath(AnalysisFolder);

            // Subtract the new and old rasters to produce the raw DoD
            Raster rawDoD = RasterOperators.Subtract(ref NewDEM, ref OldDEM, rawDoDPath);

            // Build pyraminds
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw, rawDoDPath);

            // Calculate the raw histogram
            Dictionary<double, HistogramBin> rawHisto = RasterOperators.BinRaster(ref rawDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the raw histogram
            HistogramBin.WriteHistogram(ref rawHisto, rawHstPath);

            // Call the polymorphic method to threshold the DoD depending on the thresholding method
            Raster thrDoD = ThresholdRawDoD(ref rawDoD, thrDoDPath);

            // Build pyraminds
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded, thrDoDPath);

            // Calculate the thresholded histogram
            Dictionary<double, HistogramBin> thrHisto = RasterOperators.BinRaster(ref thrDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the thresholded histogram
            HistogramBin.WriteHistogram(ref thrHisto, thrHstPath);

            // Calculate the change statistics and write the output files
            GCDConsoleLib.DoDStats changeStats = CalculateChangeStats(ref rawDoD, ref thrDoD);
            GenerateSummaryXML(ref changeStats, sumXMLPath, LinearUnits);
            GenerateChangeBarGraphicFiles(ref changeStats, 0, 0);
            GenerateHistogramGraphicFiles(ref rawHisto, ref thrHisto, 0, 0);

            return GetDoDResult(ref changeStats, rawDoDPath, thrDoDPath, rawHstPath, thrHstPath, LinearUnits);
        }

        protected abstract Raster ThresholdRawDoD(ref Raster rawDoD, FileInfo thrDoDPath);

        protected abstract DoDStats CalculateChangeStats(ref Raster rawDoD, ref Raster thrDoD);

        protected abstract DoDResult GetDoDResult(ref DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHist, FileInfo thrHist, UnitsNet.Units.LengthUnit eUnits);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeStats"></param>
        /// <returns></returns>
        /// <remarks>This method is needed by budget segregation as well</remarks>
        public static void GenerateSummaryXML(ref DoDStats changeStats, FileInfo outputPath, UnitsNet.Units.LengthUnit linearUnits)
        {
            string templatePath = Path.Combine(Project.ProjectManagerBase.ExcelTemplatesFolder.FullName, "GCDSummary.xml");
            System.Text.StringBuilder outputText = default(System.Text.StringBuilder);

            try
            {
                using (System.IO.StreamReader objReader = new System.IO.StreamReader(templatePath))
                {
                    outputText = new System.Text.StringBuilder(objReader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error reading the GCD summary XML template file", ex);
                ex.Data["Excel Template Path"] = templatePath;
                throw ex2;
            }

            outputText.Replace("[LinearUnits]", UnitsNet.Length.GetAbbreviation(linearUnits));

            outputText.Replace("[TotalAreaOfErosionRaw]", changeStats.AreaErosion_Raw.ToString());
            outputText.Replace("[TotalAreaOfErosionThresholded]", changeStats.AreaErosion_Thresholded.ToString());

            outputText.Replace("[TotalAreaOfDepositionRaw]", changeStats.AreaDeposition_Raw.ToString());
            outputText.Replace("[TotalAreaOfDepositionThresholded]", changeStats.AreaDeposition_Thresholded.ToString());

            outputText.Replace("[TotalVolumeOfErosionRaw]", changeStats.VolumeErosion_Raw.ToString());
            outputText.Replace("[TotalVolumeOfErosionThresholded]", changeStats.VolumeErosion_Thresholded.ToString());
            outputText.Replace("[ErrorVolumeOfErosion]", changeStats.VolumeErosion_Error.ToString());

            outputText.Replace("[TotalVolumeOfDepositionRaw]", changeStats.VolumeDeposition_Raw.ToString());
            outputText.Replace("[TotalVolumeOfDepositionThresholded]", changeStats.VolumeDeposition_Thresholded.ToString());
            outputText.Replace("[ErrorVolumeOfDeposition]", changeStats.VolumeDeposition_Error.ToString());

            try
            {
                using (StreamWriter objWriter = new StreamWriter(outputPath.FullName))
                {
                    objWriter.Write(outputText);
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error writing the GCD summary XML template file", ex);
                ex.Data["Excel Template Path"] = templatePath;
                throw ex2;
            }
        }

        protected void GenerateHistogramGraphicFiles(ref Dictionary<double, HistogramBin> rawHisto, ref Dictionary<double, HistogramBin> thrHisto, int fChartWidth, int fChartHeight)
        {
            FiguresFolder.Create();

            string areaHistPath = Path.Combine(FiguresFolder.FullName, "Histogram_Area.png");
            string volhistPath = Path.Combine(FiguresFolder.FullName, "Histogram_Volume.png");

            DoDHistogramViewerClass ExportHistogramViewer = new DoDHistogramViewerClass(ref rawHisto, ref thrHisto, LinearUnits);
            ExportHistogramViewer.ExportCharts(areaHistPath, volhistPath, fChartWidth, fChartHeight);
        }

        protected void GenerateChangeBarGraphicFiles(ref GCDConsoleLib.DoDStats changeStats, int fChartWidth, int fChartHeight, string sFilePrefix = "")
        {
            Visualization.ElevationChangeBarViewer barViewer = new Visualization.ElevationChangeBarViewer(LinearUnits);

            if (!string.IsNullOrEmpty(sFilePrefix))
            {
                if (!sFilePrefix.EndsWith("_"))
                {
                    sFilePrefix += "_";
                }
            }

            FiguresFolder.Create();

            barViewer.Refresh(changeStats.AreaErosion_Thresholded, changeStats.AreaDeposition_Thresholded, LinearUnits, Visualization.ElevationChangeBarViewer.BarTypes.Area, true);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_AreaAbsolute.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(changeStats.AreaErosion_Thresholded, changeStats.AreaDeposition_Thresholded, LinearUnits, Visualization.ElevationChangeBarViewer.BarTypes.Area, false);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_AreaRelative.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(changeStats.VolumeErosion_Thresholded, changeStats.VolumeDeposition_Thresholded, changeStats.NetVolumeOfDifference_Thresholded, changeStats.VolumeErosion_Error, changeStats.VolumeDeposition_Error, changeStats.NetVolumeOfDifference_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Volume, true);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_VolumeAbsolute.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(changeStats.VolumeErosion_Thresholded, changeStats.VolumeDeposition_Thresholded, changeStats.NetVolumeOfDifference_Thresholded, changeStats.VolumeErosion_Error, changeStats.VolumeDeposition_Error, changeStats.NetVolumeOfDifference_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Volume, false);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_VolumeRelative.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(changeStats.AverageDepthErosion_Thresholded, changeStats.AverageDepthDeposition_Thresholded, changeStats.AverageNetThicknessOfDifferenceADC_Thresholded, changeStats.AverageDepthErosion_Error, changeStats.AverageDepthDeposition_Error, changeStats.AverageThicknessOfDifferenceADC_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Vertical, true);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_DepthAbsolute.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(changeStats.AverageDepthErosion_Thresholded, changeStats.AverageDepthDeposition_Thresholded, changeStats.AverageNetThicknessOfDifferenceADC_Thresholded, changeStats.AverageDepthErosion_Error, changeStats.AverageDepthDeposition_Error, changeStats.AverageThicknessOfDifferenceADC_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Vertical, false);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_DepthRelative.png"), fChartWidth, fChartHeight);
        }
    }
}