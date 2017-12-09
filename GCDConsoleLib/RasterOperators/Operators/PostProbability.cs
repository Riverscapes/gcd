using System;
using System.Collections.Generic;
namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// We do Hillshade as a float on purpose since we need it to be fast and accuracy is less important
    /// </summary>
    class PosteriorProbability : WindowOverlapOperator<double>
    {
        // Just helpful statics for reference
        private static int rawDod = 0;
        private static int priorProb = 1;

        // HEre are our other output rasters
        private Raster _ConditionalRaster;
        private Raster _SpatialCoErosionRaster;
        private Raster _SpatialCoDepositionRaster;

        // Some parameters we store
        private int _infA;
        private int _infB;

        /// <summary>
        /// Pass-through constructor for Creating Prior Probability Rasters
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        /// 
        public PosteriorProbability(Raster rawDoD, Raster rPriorProb,
            Raster sPosteriorRaster, Raster sConditionalRaster, Raster sSpatialCoErosionRaster, Raster sSpatialCoDepositionRaster,
            int buffCells,
            int inflectionA,
            int inflectionB) :
            base(new List<Raster> { rawDoD, rPriorProb }, buffCells, sPosteriorRaster)
        {
            _ConditionalRaster = sConditionalRaster;
            _SpatialCoDepositionRaster = sSpatialCoDepositionRaster;
            _SpatialCoErosionRaster = sSpatialCoErosionRaster;
            _infA = inflectionA;
            _infB = inflectionB;
        }

        protected override double WindowOp(List<double[]> windowData)
        {
            // NEED TO FIGURE THESE OUT
            double xMax = -1;
            double xMin = -1;
            double depositionNoData = OpNodataVal;
            double erosionNoData = OpNodataVal;
            double[] depositionData = new double[1] { 1 };
            double[] conditionalData = new double[1] { 1 };
            double[] erosionData = new double[1] { 1 };
            double[] postData = new double[1] { 1 };


            double pAgEjDenom = xMax - xMin;
            double pA, pAgEj, pEj, nbrCnt;
            int i = BufferCenterID;

            if ((windowData[rawDod][i] != _rasternodatavals[rawDod]) && (windowData[priorProb][i] != _rasternodatavals[priorProb]))
            {
                if ((windowData[rawDod][i] > 0) && (depositionData[i] != depositionNoData))
                {
                    nbrCnt = depositionData[i];
                    if (nbrCnt <= xMin)
                    {
                        conditionalData[i] = 0;
                        postData[i] = 0;
                    }
                    else if (nbrCnt >= xMax)
                    {
                        conditionalData[i] = 1;
                        postData[i] = 1;
                    }
                    else
                    {
                        pEj = windowData[priorProb][i];
                        pAgEj = (nbrCnt - xMin) / pAgEjDenom;
                        conditionalData[i] = pAgEj;
                        pA = pAgEj * pEj + (1 - pAgEj) * (1 - pEj);
                        postData[i] = pAgEj * pEj / pA;
                    }
                }
                else if ((windowData[rawDod][i] < 0) && (erosionData[i] != erosionNoData))
                {
                    nbrCnt = erosionData[i];
                    if (nbrCnt <= xMin)
                    {
                        conditionalData[i] = 0;
                        postData[i] = 0;
                    }
                    else if (nbrCnt >= xMax)
                    {
                        conditionalData[i] = -1;
                        postData[i] = -1;
                    }
                    else
                    {
                        pEj = -windowData[priorProb][i];
                        pAgEj = (nbrCnt - xMin) / pAgEjDenom;
                        conditionalData[i] = -pAgEj;
                        pA = pAgEj * pEj + (1 - pAgEj) * (1 - pEj);
                        postData[i] = -pAgEj * pEj / pA;
                    }
                }
                else if (windowData[rawDod][i] == 0)
                {
                    conditionalData[i] = 0;
                    postData[i] = 0;
                }
                else
                {
                    conditionalData[i] = OpNodataVal;
                    postData[i] = OpNodataVal;
                }
            }
            else
            {
                conditionalData[i] = OpNodataVal;
                postData[i] = OpNodataVal;
            }
            return 0;
        }

    }
}