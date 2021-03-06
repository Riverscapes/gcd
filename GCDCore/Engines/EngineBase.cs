﻿using System;
using System.IO;
using GCDConsoleLib.GCD;
using GCDCore.Visualization;
using System.Globalization;
using GCDConsoleLib;

namespace GCDCore.Engines
{
    public abstract class EngineBase
    {
        protected const int DEFAULTHISTOGRAMNUMBER = 100;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeStats"></param>
        /// <returns></returns>
        /// <remarks>This method is needed by budget segregation as well</remarks>
        protected void GenerateSummaryXML(DoDStats changeStats, FileInfo outputPath)
        {
            string templatePath = Path.Combine(Project.ProjectManager.ExcelTemplatesFolder.FullName, "GCDSummary.xml");
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

            UnitsNet.Area ca = Project.ProjectManager.Project.CellArea;
            UnitsNet.Units.LengthUnit lu = Project.ProjectManager.Project.Units.VertUnit;
            UnitsNet.Units.AreaUnit au = Project.ProjectManager.Project.Units.ArUnit;
            UnitsNet.Units.VolumeUnit vu = Project.ProjectManager.Project.Units.VolUnit;
            string abbr = UnitsNet.Length.GetAbbreviation(lu);

            outputText.Replace("[LinearUnits]", abbr);

            outputText.Replace("[TotalAreaOfErosionRaw]", changeStats.ErosionRaw.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalAreaOfErosionThresholded]", changeStats.ErosionThr.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));

            outputText.Replace("[TotalAreaOfDepositionRaw]", changeStats.DepositionRaw.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalAreaOfDepositionThresholded]", changeStats.DepositionThr.GetArea(ca).As(au).ToString(CultureInfo.InvariantCulture));

            outputText.Replace("[TotalVolumeOfErosionRaw]", changeStats.ErosionRaw.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalVolumeOfErosionThresholded]", changeStats.ErosionThr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[ErrorVolumeOfErosion]", changeStats.ErosionErr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu).ToString(CultureInfo.InvariantCulture));

            outputText.Replace("[TotalVolumeOfDepositionRaw]", changeStats.DepositionRaw.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[TotalVolumeOfDepositionThresholded]", changeStats.DepositionThr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu).ToString(CultureInfo.InvariantCulture));
            outputText.Replace("[ErrorVolumeOfDeposition]", changeStats.DepositionErr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu).ToString(CultureInfo.InvariantCulture));

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

        protected void GenerateChangeBarGraphicFiles(DirectoryInfo analysisFolder, DoDStats stats, int fChartWidth, int fChartHeight, string sFilePrefix = "")
        {
            ElevationChangeBarViewer barViewer = new ElevationChangeBarViewer();

            if (!string.IsNullOrEmpty(sFilePrefix))
            {
                if (!sFilePrefix.EndsWith("_"))
                {
                    sFilePrefix += "_";
                }
            }

            DirectoryInfo figuresFolder = Project.DoDBase.FiguresFolderPath(analysisFolder);
            figuresFolder.Create();

            UnitsNet.Area ca = GCDCore.Project.ProjectManager.Project.CellArea;
            UnitsNet.Units.LengthUnit lu = Project.ProjectManager.Project.Units.VertUnit;
            UnitsNet.Units.AreaUnit au = Project.ProjectManager.Project.Units.ArUnit;
            UnitsNet.Units.VolumeUnit vu = Project.ProjectManager.Project.Units.VolUnit;
            string abbr = UnitsNet.Length.GetAbbreviation(lu);

            barViewer.Refresh(
                stats.ErosionThr.GetArea(ca).As(au),
                stats.DepositionThr.GetArea(ca).As(au), abbr, ElevationChangeBarViewer.BarTypes.Area, true);
            barViewer.Save(new FileInfo(Path.Combine(figuresFolder.FullName, sFilePrefix + "ChangeBars_AreaAbsolute.png")), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.ErosionThr.GetArea(ca).As(au),
                stats.DepositionThr.GetArea(ca).As(au), abbr, ElevationChangeBarViewer.BarTypes.Area, false);
            barViewer.Save(new FileInfo(Path.Combine(figuresFolder.FullName, sFilePrefix + "ChangeBars_AreaRelative.png")), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.ErosionThr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.DepositionThr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.NetVolumeOfDifference_Thresholded.As(vu),
                stats.ErosionErr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.DepositionErr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.NetVolumeOfDifference_Error.As(vu), abbr, ElevationChangeBarViewer.BarTypes.Volume, true);
            barViewer.Save(new FileInfo(Path.Combine(figuresFolder.FullName, sFilePrefix + "ChangeBars_VolumeAbsolute.png")), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.ErosionThr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.DepositionThr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.NetVolumeOfDifference_Thresholded.As(vu),
                stats.ErosionErr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.DepositionErr.GetVolume(ca, Project.ProjectManager.Project.Units).As(vu),
                stats.NetVolumeOfDifference_Error.As(vu), abbr, ElevationChangeBarViewer.BarTypes.Volume, false);
            barViewer.Save(new FileInfo(Path.Combine(figuresFolder.FullName, sFilePrefix + "ChangeBars_VolumeRelative.png")), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.AverageDepthErosion_Thresholded.As(lu),
                stats.AverageDepthDeposition_Thresholded.As(lu),
                stats.AverageNetThicknessOfDifferenceADC_Thresholded.As(lu),
                stats.AverageDepthErosion_Error.As(lu),
                stats.AverageDepthDeposition_Error.As(lu),
                stats.AverageThicknessOfDifferenceADC_Error.As(lu), abbr, ElevationChangeBarViewer.BarTypes.Vertical, true);
            barViewer.Save(new FileInfo(Path.Combine(figuresFolder.FullName, sFilePrefix + "ChangeBars_DepthAbsolute.png")), fChartWidth, fChartHeight);

            barViewer.Refresh(
                stats.AverageDepthErosion_Thresholded.As(lu),
                stats.AverageDepthDeposition_Thresholded.As(lu),
                stats.AverageNetThicknessOfDifferenceADC_Thresholded.As(lu),
                stats.AverageDepthErosion_Error.As(lu),
                stats.AverageDepthDeposition_Error.As(lu),
                stats.AverageThicknessOfDifferenceADC_Error.As(lu), abbr, ElevationChangeBarViewer.BarTypes.Vertical, false);
            barViewer.Save(new FileInfo(Path.Combine(figuresFolder.FullName, sFilePrefix + "ChangeBars_DepthRelative.png")), fChartWidth, fChartHeight);
        }

        protected void WriteHistogram(Histogram histo, FileInfo outputFile)
        {
            histo.WriteFile(outputFile, Project.ProjectManager.Project.CellArea, Project.ProjectManager.Project.Units);
        }

        protected void GenerateHistogramGraphicFiles(DirectoryInfo analysisFolder, Histogram rawHisto, Histogram thrHisto, int fChartWidth, int fChartHeight)
        {
            DirectoryInfo figuresFolder = Project.DoDBase.FiguresFolderPath(analysisFolder);
            figuresFolder.Create();

            FileInfo areaHistPath = new FileInfo(Path.Combine(figuresFolder.FullName, "Histogram_Area.png"));
            FileInfo volhistPath = new FileInfo(Path.Combine(figuresFolder.FullName, "Histogram_Volume.png"));

            DoDHistogramViewer ExportHistogramViewer = new DoDHistogramViewer(rawHisto, thrHisto, Project.ProjectManager.Project.Units);
            ExportHistogramViewer.ExportCharts(areaHistPath, volhistPath, fChartWidth, fChartHeight);
        }
    }
}