using System;
using System.Collections.Generic;
namespace GCDConsoleLib.Internal.Operators
{
    /// <summary>
    /// We do Hillshade as a float on purpose since we need it to be fast and accuracy is less important
    /// </summary>
    public class PosteriorProbability : CellByCellOperator<double>
    {
        // Just helpful statics for reference
        private static int rawDod = 0;
        private static int priorProb = 1;
        private static int spCoEro = 2;
        private static int spCoDep = 3;

        private static int postRaster = 0;
        private static int condRaster = 1;

        // Some parameters we store
        private int _infA; // min
        private int _infB; // Max

        /// <summary>
        /// Pass-through constructor for Creating Prior Probability Rasters
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        /// 
        public PosteriorProbability(Raster rawDoD, Raster rPriorProb,
            Raster rSpatialCoErosionRaster, Raster rSpatialCoDepositionRaster,
            Raster sPosteriorRaster, Raster sConditionalRaster, 
            int inflectionA,
            int inflectionB) :
            base(new List<Raster> { rawDoD, rPriorProb, rSpatialCoErosionRaster, rSpatialCoDepositionRaster }, new List<Raster>() { sPosteriorRaster, sConditionalRaster })
        {
            _infA = inflectionA;
            _infB = inflectionB;
        }


        protected override void CellOp(List<double[]> data, List<double[]> outputs, int id)
        {
            double pAgEjDenom = _infB - _infA;
            double pA, pAgEj, pEj, nbrCnt;

            // Just for safety set it nodata to begin with
            outputs[condRaster][id] = outNodataVals[condRaster];
            outputs[postRaster][id] = outNodataVals[postRaster];

            if ((data[rawDod][id] != inNodataVals[rawDod]) && (data[priorProb][id] != inNodataVals[priorProb]))
            {
                // Deposition Case
                if ((data[rawDod][id] > 0) && (data[spCoDep][id] != inNodataVals[spCoDep]))
                {
                    nbrCnt = data[spCoDep][id];
                    if (nbrCnt <= _infA)
                    {
                        outputs[condRaster][id] = 0;
                        outputs[postRaster][id] = 0;
                    }
                    else if (nbrCnt >= _infB)
                    {
                        outputs[condRaster][id] = 1;
                        outputs[postRaster][id] = 1;
                    }
                    else
                    {
                        pEj = data[priorProb][id];
                        // Rise over run
                        pAgEj = (nbrCnt - _infA) / pAgEjDenom;
                        outputs[condRaster][id] = pAgEj;
                        // Just a linear slope
                        pA = pAgEj * pEj + (1 - pAgEj) * (1 - pEj);
                        outputs[postRaster][id] = pAgEj * pEj / pA;
                    }
                }
                // Erosion Case
                else if ((data[rawDod][id] < 0) && (data[spCoEro][id] != inNodataVals[spCoEro]))
                {
                    nbrCnt = data[spCoEro][id];
                    if (nbrCnt <= _infA)
                    {
                        outputs[condRaster][id] = 0;
                        outputs[postRaster][id] = 0;
                    }
                    else if (nbrCnt >= _infB)
                    {
                        outputs[condRaster][id] = -1;
                        outputs[postRaster][id] = -1;
                    }
                    else
                    {
                        pEj = -data[priorProb][id];
                        // Rise over run
                        pAgEj = (nbrCnt - _infA) / pAgEjDenom;
                        outputs[condRaster][id] = -pAgEj;
                        // Just a linear slope
                        pA = pAgEj * pEj + (1 - pAgEj) * (1 - pEj);
                        outputs[postRaster][id] = -pAgEj * pEj / pA;
                    }
                }
                else if (data[rawDod][id] == 0)
                {
                    outputs[condRaster][id] = 0;
                    outputs[postRaster][id] = 0;
                }
            }
        }

    }
}