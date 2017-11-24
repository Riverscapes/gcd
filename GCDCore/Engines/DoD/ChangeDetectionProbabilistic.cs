using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.IO;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public class ChangeDetectionEngineProbabilistic : ChangeDetectionEnginePropProb
    {
        public readonly decimal Threshold;
        public readonly CoherenceProperties SpatialCoherence;
        private FileInfo m_PriorProbRaster;
        private FileInfo m_PosteriorRaster;
        private FileInfo m_ConditionalRaster;
        private FileInfo m_SpatialCoErosionRaster;

        private FileInfo m_SpatialCoDepositionRaster;

        public ChangeDetectionEngineProbabilistic(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, ErrorSurface newError, ErrorSurface oldError,
            decimal fThreshold, CoherenceProperties spatCoherence = null)
        : base(name, folder, newDEM, oldDEM, newError, oldError)
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

            // Create the prior probability raster
            m_PriorProbRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "priorprob"), OutputManager.RasterExtension));
            RasterOperators.CreatePriorProbabilityRaster(rawDoD, NewError.Raster.Raster, OldError.Raster.Raster, m_PriorProbRaster.FullName);

            // Build Pyramids
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PriorProbRaster);

            if (SpatialCoherence == null)
            {
                thrDoD = RasterOperators.ThresholdDoDProbability(rawDoD, thrDoDPath.FullName, NewError.Raster.Raster, OldError.Raster.Raster, m_PriorProbRaster.FullName, Threshold);
            }
            else
            {
                m_PosteriorRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "postProb"), OutputManager.RasterExtension));
                m_ConditionalRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "condProb"), OutputManager.RasterExtension));
                m_SpatialCoErosionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "nbrErosion"), OutputManager.RasterExtension));
                m_SpatialCoDepositionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(AnalysisFolder.FullName, "nbrDeposition"), OutputManager.RasterExtension));

                thrDoD = RasterOperators.ThresholdDoDProbWithSpatialCoherence(rawDoD, thrDoDPath.FullName, NewError.Raster.Raster, OldError.Raster.Raster, m_PriorProbRaster.FullName,
                    m_PosteriorRaster.FullName, m_ConditionalRaster.FullName, m_SpatialCoErosionRaster.FullName, m_SpatialCoDepositionRaster.FullName,
                    SpatialCoherence.MovingWindowDimensions, SpatialCoherence.MovingWindowDimensions, Threshold);

                // Build Pyramids
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_SpatialCoErosionRaster);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_SpatialCoDepositionRaster);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_ConditionalRaster);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PosteriorRaster);
            }

            return thrDoD;
        }

        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitsNet.Area cellArea, UnitGroup units)
        {
            Raster propErr = PropagatedErrRaster;
            return RasterOperators.GetStatsProbalistic(rawDoD, thrDoD, propErr, cellArea, units);
        }

        protected override DoDBase GetDoDResult(DoDStats changeStats, Raster rawDoD, Raster thrDoD, HistogramPair histograms)
        {
            bool bBayesian = SpatialCoherence is CoherenceProperties;
            int nFilter = 0;
            if (SpatialCoherence is CoherenceProperties)
            {
                nFilter = SpatialCoherence.MovingWindowDimensions;
            }

            return new DoDProbabilistic(Name, AnalysisFolder, NewDEM, OldDEM, histograms, rawDoD, thrDoD, NewError, OldError,
                PropagatedErrRaster.GISFileInfo, m_PriorProbRaster, m_PosteriorRaster, m_ConditionalRaster, m_SpatialCoErosionRaster, m_SpatialCoDepositionRaster,
                SpatialCoherence, Threshold, changeStats);
        }
    }
}
