using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public class ChangeDetectionEnginePropProb : ChangeDetectionEngineBase
    {
        protected readonly ErrorSurface NewError;
        protected readonly ErrorSurface OldError;
        public Raster PropagatedErrRaster;

        public ChangeDetectionEnginePropProb(Surface newDEM, Surface oldDEM, ErrorSurface newError, ErrorSurface oldError, Project.Masks.AOIMask aoi)
            : base(newDEM, oldDEM, aoi)
        {
            NewError = newError;
            OldError = oldError;
        }

        protected override Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath)
        {
            GeneratePropagatedErrorRaster(thrDoDPath.Directory);
            Raster thrDoD = RasterOperators.AbsoluteSetNull(rawDoD, RasterOperators.ThresholdOps.GreaterThan, PropagatedErrRaster, thrDoDPath);
            return thrDoD;
        }

        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitGroup units)
        {
            return RasterOperators.GetStatsPropagated(rawDoD, PropagatedErrRaster, units);
        }

        protected override DoDBase GetDoDResult(string dodName, DoDStats changeStats, Raster rawDoD, Raster thrDoD, HistogramPair histograms, FileInfo summaryXML)
        {
            return new DoDPropagated(dodName, rawDoD.GISFileInfo.Directory, NewSurface, OldSurface, AOIMask, rawDoD, thrDoD, histograms, summaryXML, NewError, OldError, PropagatedErrRaster, changeStats);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Calculate the propograted error raster based on the two error surfaces. Then threshold the raw
        /// DoD removing any cells that have a value less than the propogated error.</remarks>
        protected void GeneratePropagatedErrorRaster(DirectoryInfo analysisFolder)
        {
            FileInfo propErrPath = ProjectManager.OutputManager.PropagatedErrorPath(analysisFolder);
            PropagatedErrRaster = RasterOperators.RootSumSquares(NewError.Raster, OldError.Raster, propErrPath);

            // Build Pyramids
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError, propErrPath);
        }
    }
}