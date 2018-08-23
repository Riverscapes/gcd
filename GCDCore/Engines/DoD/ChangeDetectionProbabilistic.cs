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

        public ChangeDetectionEngineProbabilistic(Surface newDEM, Surface oldDEM, Project.Masks.AOIMask aoi, ErrorSurface newError, ErrorSurface oldError,
            decimal fThreshold, CoherenceProperties spatCoherence = null, bool isAsync = false)
        : base(newDEM, oldDEM, newError, oldError, aoi, isAsync)
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
            GeneratePropagatedErrorRaster(thrDoDPath.Directory);
            Raster thrDoD = null;

            // Create the prior probability raster
            m_PriorProbRaster = new FileInfo(Path.ChangeExtension(Path.Combine(thrDoDPath.DirectoryName, "priorprob"), ProjectManager.RasterExtension));
            Raster priorPRob = RasterOperators.CreatePriorProbabilityRaster(rawDoD, PropagatedErrRaster, m_PriorProbRaster, OnProgressChangeDoD);

            // Build Pyramids
            ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PriorProbRaster);

            if (SpatialCoherence == null)
            {
                thrDoD = RasterOperators.ThresholdDoDProbability(rawDoD, PropagatedErrRaster, thrDoDPath, Threshold, OnProgressChangeDoD);
            }
            else
            {
                m_PosteriorRaster = new FileInfo(Path.ChangeExtension(Path.Combine(thrDoDPath.DirectoryName, "postProb"), ProjectManager.RasterExtension));
                m_ConditionalRaster = new FileInfo(Path.ChangeExtension(Path.Combine(thrDoDPath.DirectoryName, "condProb"), ProjectManager.RasterExtension));
                m_SpatialCoErosionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(thrDoDPath.DirectoryName, "nbrErosion"), ProjectManager.RasterExtension));
                m_SpatialCoDepositionRaster = new FileInfo(Path.ChangeExtension(Path.Combine(thrDoDPath.DirectoryName, "nbrDeposition"), ProjectManager.RasterExtension));

                // Count erosion and Deposition in a window around each cell
                Raster rSpatialCoErosion = RasterOperators.NeighbourCount(rawDoD, RasterOperators.GCDWindowType.Erosion, SpatialCoherence.BufferSize, m_SpatialCoErosionRaster,
                    OnProgressChangeDoD);
                Raster rSpatialCoDeposition = RasterOperators.NeighbourCount(rawDoD, RasterOperators.GCDWindowType.Deposition, SpatialCoherence.BufferSize, m_SpatialCoDepositionRaster,
                    OnProgressChangeDoD);

                Raster PostProb = RasterOperators.PosteriorProbability(rawDoD, priorPRob,
                    rSpatialCoErosion, rSpatialCoDeposition,
                    m_PosteriorRaster, m_ConditionalRaster,
                    SpatialCoherence.XMin, SpatialCoherence.XMax,
                    OnProgressChangeDoD);

                thrDoD = RasterOperators.ThresholdDoDProbability(rawDoD, PostProb, new FileInfo(thrDoDPath.FullName), Threshold, OnProgressChangeDoD);

                // Build Pyramids
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_SpatialCoErosionRaster);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_SpatialCoDepositionRaster);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_ConditionalRaster);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, m_PosteriorRaster);
            }

            return thrDoD;
        }

        protected override Raster GenerateErrorRaster(FileInfo thrErrorPath)
        {
            double zvalDbl = GCDConsoleLib.Utility.Probability.ltqnorm((double)Threshold);

            decimal zval = decimal.MinValue;
            if (!(double.IsNegativeInfinity(zvalDbl) || double.IsPositiveInfinity(zvalDbl)))
                zval = (decimal) zvalDbl;

            return RasterOperators.Multiply(PropagatedErrRaster, zval, thrErrorPath, OnProgressChangeDoD);
        }

        protected override DoDStats CalculateChangeStats(Raster rawDoD, Raster thrDoD, UnitGroup units)
        {
            return RasterOperators.GetStatsProbalistic(rawDoD, thrDoD, PropagatedErrRaster, units, OnProgressChangeDoD);
        }

        protected override DoDBase GetDoDResult(string dodName, DoDStats changeStats, Raster rawDoD, Raster thrDoD, Raster thrErr, HistogramPair histograms, FileInfo summaryXML)
        {
            bool bBayesian = SpatialCoherence is CoherenceProperties;
            int nFilter = 0;
            if (SpatialCoherence is CoherenceProperties)
            {
                nFilter = SpatialCoherence.BufferSize;
            }

            return new DoDProbabilistic(dodName, rawDoD.GISFileInfo.Directory, NewSurface, OldSurface, AOIMask, histograms, summaryXML, rawDoD, thrDoD, thrErr, NewError, OldError,
                PropagatedErrRaster, m_PriorProbRaster, m_PosteriorRaster, m_ConditionalRaster, m_SpatialCoErosionRaster, m_SpatialCoDepositionRaster,
                SpatialCoherence, Threshold, changeStats);
        }
    }
}
