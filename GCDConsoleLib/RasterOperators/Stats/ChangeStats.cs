using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib.Internal.Operators
{

    public class ChangeStats : CellByCellOperator<float>
    {

        private float fCountErosion, fCountDeposition, fSumErosion, fSumDeposition, fDoDValue;
        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public ChangeStats(ref Raster rInput1, ref Raster rInput2, Raster rOutputRaster) :
            base(new List<Raster> { rInput1, rInput2 }, rOutputRaster)
        {
            fCountErosion = 0;
            fCountDeposition = 0;
            fSumErosion = 0;
            fSumDeposition = 0;
        }

        public Dictionary<string, float> GetChangeStats(Area cellArea, LengthUnit vUnit, VolumeUnit volUnit)
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
                    fSumDeposition += fDoDValue;
                    fCountDeposition++;
                }

                // Erosion
                if (fDoDValue < 0)
                {
                    fSumErosion += (fDoDValue * -1);
                    fCountErosion++;
                }
            }
            // We need to return something
            return 0;
        }

    }
}
