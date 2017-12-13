using System;
using System.Collections.Generic;
namespace GCDConsoleLib.Internal.Operators
{
    public class GCDNeighbourCount : WindowOverlapOperator<double>
    {
        RasterOperators.GCDWindowType wType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rDoD"></param>
        /// <param name="rOutput"></param>
        /// <param name="buffCells"></param>
        /// <param name="wType"></param>
        public GCDNeighbourCount(Raster rDoD, Raster rOutput, int buffCells, RasterOperators.GCDWindowType wType) :
            base(new List<Raster> { rDoD }, buffCells, new List<Raster>() { rOutput })
        {
            SetOpExtent(rDoD.Extent.Buffer(buffCells));
        }

        /// <summary>
        /// Window operation for neighbour counting
        /// </summary>
        /// <param name="windowData"></param>
        /// <returns></returns>
        protected override void WindowOp(List<double[]> windowData, List<double[]> outbuffers, int id)
        {
            double count = 0;

            if (windowData[0][BufferCenterID] == inNodataVals[0])
                outbuffers[0][id] = outNodataVals[0];
            else
            {
                // Loop over the window and calculate the erosion and deposition
                for (int i = 0; i < windowData[0].Length; i++)
                {
                    // Don't ever count nodatavalues or 
                    if (i == BufferCenterID || windowData[0][i] == inNodataVals[0])
                        continue;

                    switch (wType)
                    {
                        case RasterOperators.GCDWindowType.Erosion:
                            if (windowData[0][i] < 0)
                                count++;
                            break;
                        case RasterOperators.GCDWindowType.Deposition:
                            if (windowData[0][i] < 0)
                                count++;
                            break;
                        case RasterOperators.GCDWindowType.All:
                            count++;
                            break;
                    }
                }
                outbuffers[0][id] = count;
            }
        }
    }
}
