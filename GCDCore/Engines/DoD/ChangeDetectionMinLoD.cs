using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public class ChangeDetectionEngineMinLoD : ChangeDetectionEngineBase
    {
        public decimal Threshold { get; internal set; }

        public ChangeDetectionEngineMinLoD(string name, DirectoryInfo folder, DEMSurvey NewDEM, DEMSurvey OldDEM, decimal fThreshold)
            : base(name, folder, NewDEM, OldDEM)
        {
            Threshold = fThreshold;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoDPath"></param>
        /// <returns></returns>
        /// <remarks>Let the base class build the pyramids for the thresholded raster</remarks>
        protected override Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath)
        {
            return RasterOperators.SetNull(rawDoD, RasterOperators.ThresholdOps.LessThan, Threshold, thrDoDPath);
        }

        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units)
        {
            return RasterOperators.GetStatsMinLoD(rawDoD, thrDoD, Threshold, cellArea, units);
        }

        protected override DoDBase GetDoDResult(DoDStats changeStats, Raster rawDoD, Raster thrDoD, HistogramPair histograms, FileInfo summaryXML)
        {
            return new DoDMinLoD(Name, AnalysisFolder, NewDEM, OldDEM, rawDoD, thrDoD, histograms, summaryXML, Threshold, changeStats);
        }
    }
}