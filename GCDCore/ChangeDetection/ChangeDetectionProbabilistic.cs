using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;

namespace GCDCore.ChangeDetection
{
    public class ChangeDetectionEngineProbabilistic : ChangeDetectionEnginePropProb
    {
        public readonly double Threshold;
        public readonly CoherenceProperties SpatialCoherence;
        private FileInfo m_PriorProbRaster;
        private FileInfo m_PosteriorRaster;
        private FileInfo m_ConditionalRaster;
        private FileInfo m_SpatialCoErosionRaster;

        private FileInfo m_SpatialCoDepositionRaster;

        public ChangeDetectionEngineProbabilistic(DirectoryInfo folder, Raster gNewDEM, Raster gOldDEM, Raster gNewError, Raster gOldError,
            double fThreshold, CoherenceProperties spatCoherence = null)
        : base(folder, gNewDEM, gOldDEM, gNewError, gOldError)
        {
            Threshold = fThreshold;
            SpatialCoherence = spatCoherence;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDoD"></param>
        /// <param name="thrDoDPath"></param>
        /// <returns></returns>
        /// <remarks>Let the base class build pyramids for the thresholded raster</remarks>
        protected override Raster ThresholdRawDoD(Raster rawDoD, FileInfo thrDoDPath)
        {
            Raster propErrorRaster = GeneratePropagatedErrorRaster();
            Raster thrDoD = null;

            Raster newErr = NewError;
            Raster oldErr = OldError;

            // Create the prior probability raster
            m_PriorProbRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "priorprob"), Project.ProjectManagerBase.RasterExtension));
            RasterOperators.CreatePriorProbabilityRaster(rawDoD, newErr, oldErr, m_PriorProbRaster.FullName);

            // Build Pyramids
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PriorProbRaster);

            if (SpatialCoherence == null)
            {
                thrDoD = RasterOperators.ThresholdDoDProbability(rawDoD, thrDoDPath.FullName, newErr, oldErr, m_PriorProbRaster.FullName, Threshold);
            }
            else
            {
                m_PosteriorRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "postProb"), Project.ProjectManagerBase.RasterExtension));
                m_ConditionalRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "condProb"), Project.ProjectManagerBase.RasterExtension));
                m_SpatialCoErosionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "nbrErosion"), Project.ProjectManagerBase.RasterExtension));
                m_SpatialCoDepositionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "nbrDeposition"), Project.ProjectManagerBase.RasterExtension));

                thrDoD = RasterOperators.ThresholdDoDProbWithSpatialCoherence(rawDoD, thrDoDPath.FullName, newErr, oldErr, m_PriorProbRaster.FullName,
                    m_PosteriorRaster.FullName, m_ConditionalRaster.FullName, m_SpatialCoErosionRaster.FullName, m_SpatialCoDepositionRaster.FullName,
                    SpatialCoherence.MovingWindowDimensions, SpatialCoherence.MovingWindowDimensions, Threshold);

                // Build Pyramids
                Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_SpatialCoErosionRaster);
                Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_SpatialCoDepositionRaster);
                Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_ConditionalRaster);
                Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PosteriorRaster);
            }

            return thrDoD;
        }

        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units)
        {
            Raster propErr = PropagatedErrRaster;
            return RasterOperators.GetStatsProbalistic(rawDoD, thrDoD, propErr, cellArea, units);
        }

        protected override DoDResult GetDoDResult(DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHistPath, Histogram rawHist, FileInfo thrHistPath, Histogram thrHist, UnitGroup units)
        {
            bool bBayesian = SpatialCoherence is CoherenceProperties;
            int nFilter = 0;
            if (SpatialCoherence is CoherenceProperties)
            {
                nFilter = SpatialCoherence.MovingWindowDimensions;
            }

            return new DoDResultProbabilisitic(ref changeStats, rawDoDPath, rawHistPath, thrDoDPath, thrHistPath, PropagatedErrRaster.GISFileInfo, m_PriorProbRaster, m_SpatialCoErosionRaster, m_SpatialCoDepositionRaster, m_ConditionalRaster,
            m_PosteriorRaster, Threshold, nFilter, bBayesian, units);
        }
    }
}
