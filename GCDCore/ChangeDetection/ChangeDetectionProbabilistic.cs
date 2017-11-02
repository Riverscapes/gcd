using GCDConsoleLib;
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

        public ChangeDetectionEngineProbabilistic(DirectoryInfo folder, ref Raster gNewDEM, ref Raster gOldDEM, ref Raster gNewError, ref Raster gOldError,
            double fThreshold, CoherenceProperties spatCoherence = null)
        : base(folder, ref gNewDEM, ref gOldDEM, ref gNewError, ref gOldError)
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
        protected override Raster ThresholdRawDoD(ref Raster rawDoD, FileInfo thrDoDPath)
        {
            Raster propErrorRaster = GeneratePropagatedErrorRaster();
            Raster thrDoD = null;

            Raster newErr = NewError;
            Raster oldErr = OldError;

            // Create the prior probability raster
            m_PriorProbRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "priorprob"), Project.ProjectManagerBase.RasterExtension));
            RasterOperators.CreatePriorProbabilityRaster(ref rawDoD, ref newErr, ref oldErr, m_PriorProbRaster.FullName);

            // Build Pyramids
            Project.ProjectManagerUI.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PriorProbRaster);

            if (SpatialCoherence == null)
            {
                thrDoD = RasterOperators.ThresholdDoDProbability(ref rawDoD, thrDoDPath.FullName, ref newErr, ref oldErr, m_PriorProbRaster.FullName, Threshold);
            }
            else
            {
                m_PosteriorRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "postProb"), Project.ProjectManagerBase.RasterExtension));
                m_ConditionalRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "condProb"), Project.ProjectManagerBase.RasterExtension));
                m_SpatialCoErosionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "nbrErosion"), Project.ProjectManagerBase.RasterExtension));
                m_SpatialCoDepositionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "nbrDeposition"), Project.ProjectManagerBase.RasterExtension));

                thrDoD = RasterOperators.ThresholdDoDProbWithSpatialCoherence(ref rawDoD, thrDoDPath.FullName, ref newErr, ref oldErr, m_PriorProbRaster.FullName,
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

        protected override DoDStats CalculateChangeStats(ref Raster rawDoD, ref Raster thrDoD)
        {
            Raster propErr = PropagatedErrRaster;
            return RasterOperators.GetStatsProbalistic(ref rawDoD, ref thrDoD, ref propErr);
        }

        protected override DoDResult GetDoDResult(ref DoDStats changeStats, FileInfo rawDoDPath, FileInfo thrDoDPath, FileInfo rawHist, FileInfo thrHist, UnitsNet.Units.LengthUnit eUnits)
        {
            bool bBayesian = SpatialCoherence is CoherenceProperties;
            int nFilter = 0;
            if (SpatialCoherence is CoherenceProperties)
            {
                nFilter = SpatialCoherence.MovingWindowDimensions;
            }

            return new DoDResultProbabilisitic(ref changeStats, rawDoDPath, rawHist, thrDoDPath, thrHist, new FileInfo(PropagatedErrRaster.FilePath), m_PriorProbRaster, m_SpatialCoErosionRaster, m_SpatialCoDepositionRaster, m_ConditionalRaster,
            m_PosteriorRaster, Threshold, nFilter, bBayesian, eUnits);
        }
    }
}
