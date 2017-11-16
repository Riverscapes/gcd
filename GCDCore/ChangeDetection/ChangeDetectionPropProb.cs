using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;

namespace GCDCore.ChangeDetection
{
    public class ChangeDetectionEnginePropProb : ChangeDetectionEngineBase
    {
        protected Raster NewError;
        protected Raster OldError;
        public Raster PropagatedErrRaster;

        public ChangeDetectionEnginePropProb(DirectoryInfo folder, Raster gNewDEM, Raster gOldDEM, Raster gNewError, Raster gOldError)
            : base(folder, gNewDEM, gOldDEM)
        {
            NewError = gNewError;
            OldError = gOldError;
        }

        protected override Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath)
        {
            PropagatedErrRaster = GeneratePropagatedErrorRaster();
            Raster thrDoD = RasterOperators.SetNull(rawDoD, RasterOperators.ThresholdOps.LessThan, PropagatedErrRaster, thrDoDPath);
            return thrDoD;
        }

        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units)
        {
            return RasterOperators.GetStatsPropagated(rawDoD, thrDoD, PropagatedErrRaster, cellArea, units);
        }

        protected override DoDResult GetDoDResult(DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHistPath, Histogram rawHist, FileInfo thrHistPath, Histogram thrHist, UnitGroup units)
        {
            return new DoDResultPropagated(changeStats, rawDoDPath, rawHistPath, thrDoDPath, thrHistPath, PropagatedErrRaster.GISFileInfo, units);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Calculate the propograted error raster based on the two error surfaces. Then threshold the raw
        /// DoD removing any cells that have a value less than the propogated error.</remarks>
        protected Raster GeneratePropagatedErrorRaster()
        {
            FileInfo propErrPath = Project.ProjectManagerBase.OutputManager.PropagatedErrorPath(AnalysisFolder);
            Raster propErr = RasterOperators.RootSumSquares(NewError, OldError, propErrPath);

            // Build Pyramids
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError, propErrPath);

            return propErr;
        }
    }
}