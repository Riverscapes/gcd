﻿using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public class ChangeDetectionEngineMinLoD : ChangeDetectionEngineBase
    {
        public decimal Threshold { get; internal set; }

        public ChangeDetectionEngineMinLoD(Surface newSurface, Surface oldSurface, Project.Masks.AOIMask aoi, decimal fThreshold, bool isAsync = false)
            : base(newSurface, oldSurface, aoi, isAsync)
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
            return RasterOperators.SetNull(rawDoD, RasterOperators.ThresholdOps.GreaterThanOrEqual, -Threshold,
                RasterOperators.ThresholdOps.LessThanOrEqual,
                Threshold, thrDoDPath,
                OnProgressChangeDoD);
        }

        protected override Raster GenerateErrorRaster(FileInfo errDoDPath)
        {
            return RasterOperators.Uniform<float>(NewSurface.Raster, errDoDPath, (float)Threshold, OnProgressChangeDoD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoD">NEVER USED IN THIS CASE.</param>
        /// <param name="units"></param>
        /// <returns></returns>
        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitGroup units)
        {
            return RasterOperators.GetStatsMinLoD(rawDoD, Threshold, units, OnProgressChangeDoD);
        }


        protected override DoDBase GetDoDResult(string dodName, DoDStats changeStats, Raster rawDoD, Raster thrDoD, Raster errDoD, HistogramPair histograms, FileInfo summaryXML)
        {
            return new DoDMinLoD(dodName, rawDoD.GISFileInfo.Directory, NewSurface, OldSurface, AOIMask, rawDoD, thrDoD, errDoD, histograms, summaryXML, Threshold, changeStats);
        }
    }
}