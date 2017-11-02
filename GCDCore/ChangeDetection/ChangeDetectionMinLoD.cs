using GCDConsoleLib;
using System.IO;

namespace GCDCore.ChangeDetection
{
    public class ChangeDetectionEngineMinLoD : ChangeDetectionEngineBase
    {
        public double Threshold { get; internal set; }

        public ChangeDetectionEngineMinLoD(DirectoryInfo folder, Raster gNewDEM, Raster gOldDEM, double fThreshold)
            : base(folder, ref gNewDEM, ref gOldDEM)
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
        protected override Raster ThresholdRawDoD(ref Raster rawDoD, FileInfo thrDoDPath)
        {
            return RasterOperators.SetNullLessThan(ref rawDoD, Threshold, thrDoDPath.FullName);
        }

        protected override DoDStats CalculateChangeStats(ref Raster rawDoD, ref Raster thrDoD)
        {
            return RasterOperators.GetStatsMinLoD(ref rawDoD, ref thrDoD, Threshold);
        }

        protected override DoDResult GetDoDResult(ref DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHistoPath, FileInfo thrHistoPath, UnitsNet.Units.LengthUnit eUnits)
        {
            return new DoDResultMinLoD(ref changeStats, rawDoDPath, thrDoDPath, rawHistoPath, thrHistoPath, Threshold, eUnits);
        }
    }
}