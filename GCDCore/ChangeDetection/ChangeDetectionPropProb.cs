using GCDConsoleLib;
using System.IO;

namespace GCDCore.ChangeDetection
{
    public class ChangeDetectionEnginePropProb : ChangeDetectionEngineBase
    {
        protected Raster NewError;
        protected Raster OldError;
        public Raster PropagatedErrRaster;

        public ChangeDetectionEnginePropProb(DirectoryInfo folder, ref Raster gNewDEM, ref Raster gOldDEM, ref Raster gNewError, ref Raster gOldError)
            : base(folder, ref gNewDEM, ref gOldDEM)
        {
            NewError = gNewError;
            OldError = gOldError;
        }

        protected override Raster ThresholdRawDoD(ref Raster rawDoD, FileInfo thrDoDPath)
        {
            PropagatedErrRaster = GeneratePropagatedErrorRaster();
            Raster thrDoD = RasterOperators.SetNullLessThan(ref rawDoD, ref PropagatedErrRaster, thrDoDPath.FullName);

            return thrDoD;
        }

        protected override DoDStats CalculateChangeStats(ref Raster rawDoD, ref Raster thrDoD)
        {
            return RasterOperators.GetStatsPropagated(ref rawDoD, ref thrDoD, ref PropagatedErrRaster);
        }

        protected override DoDResult GetDoDResult(ref DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHist, FileInfo thrHist, UnitsNet.Units.LengthUnit eUnits)
        {
            return new DoDResultPropagated(ref changeStats, rawDoDPath, rawHist, thrDoDPath, thrHist, new FileInfo(PropagatedErrRaster.FilePath), eUnits);
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
            Raster propErr = RasterOperators.RootSumSquares(ref NewError, ref OldError, propErrPath.FullName);

            // Build Pyramids
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError, propErrPath);

            return propErr;
        }
    }
}