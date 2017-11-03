using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib.Internal.Operators
{

    public class MinLodStats : CellByCellOperator<float>
    {

        private float fAreaErosionRaw, fAreaDepositonRaw, fAreaErosionThr, fAreaDepositionThr, 
            fVolErosionRaw, fVolDepositionRaw, fVolErosionThr, fVolDepositionThr, fVolErosionErr, 
            fVolDepositonErr, fDoDValue;
        private float _thresh;
        private int nRawErosionCount, nRawDepositionCount, nThrErosionCount, nThrDepositionCount;
        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public MinLodStats(ref Raster rInput1, ref Raster rInput2, Raster rOutputRaster, float thresh) :
            base(new List<Raster> { rInput1, rInput2 }, rOutputRaster)
        {
            fAreaErosionRaw = 0;
            fAreaDepositonRaw = 0;
            fAreaErosionThr = 0;
            fAreaDepositionThr = 0;
            fVolErosionRaw = 0;
            fVolDepositionRaw = 0;
            fVolErosionThr = 0;
            fVolDepositionThr = 0;
            fVolErosionErr = 0;
            fVolDepositonErr = 0;

            _thresh = thresh;

            nRawErosionCount = 0;
            nRawDepositionCount = 0;
            nThrErosionCount = 0;
            nThrDepositionCount = 0;
        }

        public Dictionary<string, float> ChangeStats(Area cellArea, LengthUnit vUnit, VolumeUnit volUnit)
        {
            Dictionary<string, float> retVal = new Dictionary<string, float>() {
                { "AreaErosion", 0 },
                { "AreaDeposition", 0 },
                { "VolumeErosion", 0 },
                { "VolumeDeposition", 0 } };
            return retVal;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(ref List<float[]> data, int id)
        {
            fDoDValue = data[0][id];
            if (fDoDValue != _rasternodatavals[0])
            {
                // Deposition
                if (fDoDValue > 0)
                {
                    // Raw Deposition
                    fVolDepositionRaw += fDoDValue;
                    nRawDepositionCount += 1;

                    if (fDoDValue > _thresh)
                    {
                        // Thresholded Deposition
                        fVolDepositionThr += fDoDValue;
                        nThrDepositionCount += 1;
                    }
                }

                // Erosion
                if (fDoDValue < 0)
                {
                    // Raw Erosion
                    fVolErosionRaw += fDoDValue * -1;
                    nRawErosionCount += 1;

                    if (fDoDValue < (_thresh * -1))
                    {
                        // Thresholded Erosion
                        fVolErosionThr += fDoDValue * -1;
                        nThrErosionCount += 1;
                    }
                }
            }
            // We need to return something
            return 0;
        }

    }
}
