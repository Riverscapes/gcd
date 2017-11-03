using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib.Internal.Operators
{

    public class DodMinLodStats : CellByCellOperator<float>
    {

        private float fCountErosionRaw, fCountDepositonRaw, fCountErosionThr, fCountDepositionThr, 
            fSumErosionRaw, fSumDepositionRaw, fSumErosionThr, fSumDepositionThr, fSumErosionErr, 
            fSumDepositonErr, fDoDValue;
        private float _thresh;
        private int nRawErosionCount, nRawDepositionCount, nThrErosionCount, nThrDepositionCount;
        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public DodMinLodStats(ref Raster rInput1, ref Raster rInput2, Raster rOutputRaster, float thresh) :
            base(new List<Raster> { rInput1, rInput2 }, rOutputRaster)
        {
            fCountErosionRaw = 0;
            fCountDepositonRaw = 0;
            fCountErosionThr = 0;
            fCountDepositionThr = 0;
            fSumErosionRaw = 0;
            fSumDepositionRaw = 0;
            fSumErosionThr = 0;
            fSumDepositionThr = 0;
            fSumErosionErr = 0;
            fSumDepositonErr = 0;

            _thresh = thresh;

            nRawErosionCount = 0;
            nRawDepositionCount = 0;
            nThrErosionCount = 0;
            nThrDepositionCount = 0;
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
                    fSumDepositionRaw += fDoDValue;
                    nRawDepositionCount += 1;

                    if (fDoDValue > _thresh)
                    {
                        // Thresholded Deposition
                        fSumDepositionThr += fDoDValue;
                        nThrDepositionCount += 1;
                    }
                }

                // Erosion
                if (fDoDValue < 0)
                {
                    // Raw Erosion
                    fSumErosionRaw += fDoDValue * -1;
                    nRawErosionCount += 1;

                    if (fDoDValue < (_thresh * -1))
                    {
                        // Thresholded Erosion
                        fSumErosionThr += fDoDValue * -1;
                        nThrErosionCount += 1;
                    }
                }
            }
            // We need to return something
            return 0;
        }

    }
}
