using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;

namespace GCDCore.ChangeDetection
{
    public class ChangeDetectionEngineMinLoD : ChangeDetectionEngineBase
    {
        public float Threshold { get; internal set; }

        public ChangeDetectionEngineMinLoD(DirectoryInfo folder, Raster gNewDEM, Raster gOldDEM, float fThreshold)
            : base(folder, gNewDEM, gOldDEM)
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

        protected override DoDResult GetDoDResult(DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHistoPath, Histogram rawHist, FileInfo thrHistoPath, Histogram thrHist, UnitGroup units)
        {
            return new DoDResultMinLoD(changeStats, rawDoDPath, thrDoDPath, rawHistoPath, thrHistoPath, Threshold, units);
        }
    }
}