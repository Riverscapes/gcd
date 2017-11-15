using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;
using GCDCore.Visualization;
using System.Globalization;

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
            get { return NewDEM.Proj.HorizontalUnit; }
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
            Raster rawDoD = RasterOperators.Subtract(ref NewDEM, ref OldDEM, rawDoDPath);

            // Build pyraminds
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw, rawDoDPath);

            // Calculate the raw histogram
            Histogram rawHisto = RasterOperators.BinRaster(ref rawDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the raw histogram
            WriteHistogram(ref rawHisto, rawHstPath);

            // Call the polymorphic method to threshold the DoD depending on the thresholding method
            Raster thrDoD = ThresholdRawDoD(ref rawDoD, thrDoDPath);

            // Build pyraminds
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded, thrDoDPath);

            // Calculate the thresholded histogram
            Histogram thrHisto = RasterOperators.BinRaster(ref thrDoD, DEFAULTHISTOGRAMNUMBER);

            // Write the thresholded histogram
            WriteHistogram(ref thrHisto, thrHstPath);

            // Calculate the change statistics and write the output files
            DoDStats changeStats = CalculateChangeStats(ref rawDoD, ref thrDoD, cellArea, units);
            GenerateSummaryXML(ref changeStats, sumXMLPath, LinearUnits);
            GenerateChangeBarGraphicFiles(ref changeStats, 0, 0);
            GenerateHistogramGraphicFiles(ref rawHisto, ref thrHisto, 0, 0);

            return GetDoDResult(ref changeStats, rawDoDPath, thrDoDPath, rawHstPath, thrHstPath, LinearUnits);
        }

        protected abstract Raster ThresholdRawDoD(ref Raster rawDoD, FileInfo thrDoDPath);

        protected abstract DoDStats CalculateChangeStats(ref Raster rawDoD, ref Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units);

        protected abstract DoDResult GetDoDResult(ref DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHist, FileInfo thrHist, UnitsNet.Units.LengthUnit eUnits);

        protected void WriteHistogram(ref Histogram histo, FileInfo outputFile)
        {
            histo.WriteFile(outputFile, Project.ProjectManagerBase.CellArea, Project.ProjectManagerBase.Units);
        }

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

            UnitsNet.Area ca = Project.ProjectManagerBase.CellArea;
            UnitsNet.Units.LengthUnit lu = Project.ProjectManagerBase.Units.VertUnit;
            UnitsNet.Units.AreaUnit au = Project.ProjectManagerBase.Units.ArUnit;
            UnitsNet.Units.VolumeUnit vu = Project.ProjectManagerBase.Units.VolUnit;


            outputText.Replace("[LinearUnits]", UnitsNet.Length.GetAbbreviation(linearUnits));

            outputText.Replace("[TotalAreaOfErosionRaw]", changeStats.ErosionRaw.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalAreaOfErosionThresholded]", changeStats.ErosionThr.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));

            outputText.Replace("[TotalAreaOfDepositionRaw]", changeStats.DepositionRaw.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalAreaOfDepositionThresholded]", changeStats.DepositionThr.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));

            outputText.Replace("[TotalVolumeOfErosionRaw]", changeStats.ErosionRaw.GetVolume(ca, lu).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalVolumeOfErosionThresholded]", changeStats.ErosionThr.GetVolume(ca, lu).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[ErrorVolumeOfErosion]", changeStats.ErosionErr.GetVolume(ca, lu).As(vu).ToString(CultureInfo.InvariantCulture));

            outputText.Replace("[TotalVolumeOfDepositionRaw]", changeStats.DepositionRaw.GetVolume(ca, lu).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalVolumeOfDepositionThresholded]", changeStats.DepositionThr.GetVolume(ca, lu).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[ErrorVolumeOfDeposition]", changeStats.DepositionErr.GetVolume(ca, lu).As(vu).ToString(CultureInfo.InvariantCulture));

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

        protected void GenerateHistogramGraphicFiles(ref Histogram rawHisto, ref Histogram thrHisto, int fChartWidth, int fChartHeight)
        {
            FiguresFolder.Create();

            string areaHistPath = Path.Combine(FiguresFolder.FullName, "Histogram_Area.png");
            string volhistPath = Path.Combine(FiguresFolder.FullName, "Histogram_Volume.png");

            //DoDHistogramViewerClass ExportHistogramViewer = new DoDHistogramViewerClass(ref rawHisto, ref thrHisto, LinearUnits);
            //ExportHistogramViewer.ExportCharts(areaHistPath, volhistPath, fChartWidth, fChartHeight);
        }

        protected void GenerateChangeBarGraphicFiles(ref DoDStats stats, int fChartWidth, int fChartHeight, string sFilePrefix = "")
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

            UnitsNet.Area ca = GCDCore.Project.ProjectManagerBase.CellArea;
            UnitsNet.Units.LengthUnit lu = Project.ProjectManagerBase.Units.VertUnit;
            UnitsNet.Units.AreaUnit au = Project.ProjectManagerBase.Units.ArUnit;
            UnitsNet.Units.VolumeUnit vu = Project.ProjectManagerBase.Units.VolUnit;

            barViewer.Refresh(
                stats.ErosionThr.GetArea(ca).As(au),
                stats.DepositionThr.GetArea(ca).As(au), LinearUnits, ElevationChangeBarViewer.BarTypes.Area, true);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_AreaAbsolute.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.ErosionThr.GetArea(ca).As(au),
                stats.DepositionThr.GetArea(ca).As(au), LinearUnits, ElevationChangeBarViewer.BarTypes.Area, false);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_AreaRelative.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.ErosionThr.GetVolume(ca, lu).As(vu),
                stats.DepositionThr.GetVolume(ca, lu).As(vu),
                stats.NetVolumeOfDifference_Thresholded.As(vu),
                stats.ErosionErr.GetVolume(ca, lu).As(vu),
                stats.DepositionErr.GetVolume(ca, lu).As(vu),
                stats.NetVolumeOfDifference_Error.As(vu), LinearUnits, ElevationChangeBarViewer.BarTypes.Volume, true);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_VolumeAbsolute.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.ErosionThr.GetVolume(ca, lu).As(vu),
                stats.DepositionThr.GetVolume(ca, lu).As(vu),
                stats.NetVolumeOfDifference_Thresholded.As(vu),
                stats.ErosionErr.GetVolume(ca, lu).As(vu),
                stats.DepositionErr.GetVolume(ca, lu).As(vu),
                stats.NetVolumeOfDifference_Error.As(vu), LinearUnits, ElevationChangeBarViewer.BarTypes.Volume, false);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_VolumeRelative.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.AverageDepthErosion_Thresholded.As(lu),
                stats.AverageDepthDeposition_Thresholded.As(lu),
                stats.AverageNetThicknessOfDifferenceADC_Thresholded.As(lu),
                stats.AverageDepthErosion_Error.As(lu),
                stats.AverageDepthDeposition_Error.As(lu),
                stats.AverageThicknessOfDifferenceADC_Error.As(lu), LinearUnits, ElevationChangeBarViewer.BarTypes.Vertical, true);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_DepthAbsolute.png"), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.AverageDepthErosion_Thresholded.As(lu),
                stats.AverageDepthDeposition_Thresholded.As(lu),
                stats.AverageNetThicknessOfDifferenceADC_Thresholded.As(lu),
                stats.AverageDepthErosion_Error.As(lu),
                stats.AverageDepthDeposition_Error.As(lu),
                stats.AverageThicknessOfDifferenceADC_Error.As(lu), LinearUnits, ElevationChangeBarViewer.BarTypes.Vertical, false);
            barViewer.Save(Path.Combine(FiguresFolder.FullName, sFilePrefix + "ChangeBars_DepthRelative.png"), fChartWidth, fChartHeight);
        }
    }
}