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

        public ChangeDetectionEnginePropProb(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, ErrorSurface newError, ErrorSurface oldError)
            : base(name, folder, newDEM, oldDEM)
        {
            NewError = newError;
            OldError = oldError;
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

        protected override DoDBase GetDoDResult(DoDStats changeStats, Raster rawDoD, Raster thrDoD, HistogramPair histograms, FileInfo summaryXML)
        {
            return new DoDPropagated(Name, AnalysisFolder, NewDEM, OldDEM, rawDoD, thrDoD, histograms, summaryXML, NewError, OldError, PropagatedErrRaster.GISFileInfo, changeStats);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Calculate the propograted error raster based on the two error surfaces. Then threshold the raw
        /// DoD removing any cells that have a value less than the propogated error.</remarks>
        protected Raster GeneratePropagatedErrorRaster()
        {
            FileInfo propErrPath = ProjectManager.OutputManager.PropagatedErrorPath(AnalysisFolder);
            Raster propErr = RasterOperators.RootSumSquares(NewError.Raster.Raster, OldError.Raster.Raster, propErrPath);

            // Build Pyramids
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError, propErrPath);

            return propErr;
        }
    }
}