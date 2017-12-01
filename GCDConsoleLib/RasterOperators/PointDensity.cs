using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;
using GCDConsoleLib.Utility;
using OSGeo.OGR;

namespace GCDConsoleLib.Internal.Operators
{
    class PointDensity : BaseOperator<double>
    {
        private Raster _routput;
        private decimal _fsize;
        private RasterOperators.KernelShapes _kshape;
        private Vector _vinput;
        private double area;
        private List<int[]> celloffsets;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="newRect"></param>
        /// <param name="rOutputRaster"></param>
        public PointDensity(Vector vPointCloud, Raster OutputRaster, RasterOperators.KernelShapes eKernel, decimal fSize)
            : base(new List<Raster> { OutputRaster }, OutputRaster)
        {
            _vinput = vPointCloud;
            _routput = OutputRaster;
            _kshape = eKernel;
            _fsize = fSize;

            switch (eKernel)
            {
                case RasterOperators.KernelShapes.Circle:
                    area = Math.PI * Math.Pow((double)fSize, 2);
                    break;
                case RasterOperators.KernelShapes.Square:
                    area = Math.Pow((double)fSize, 2);
                    break;
            }
        }


        public bool InsideSquare(Geometry pt, decimal[] cidxy)
        {
            // do distnce vertically and then horizontally.
            return true;
        }

        public bool InsideRadius(Geometry pt, decimal[] cidxy)
        {
            // just do euclidean distance
            return true;
        }

        protected override void ChunkOp(List<double[]> data, double[] outChunk)
        {
            // Get all the points in this chunk
            List<Geometry> retval = _vinput.PointsInExtent(ChunkExtent.Buffer(_fsize));
            double outval = 0;

            switch (_kshape)
            {
                case RasterOperators.KernelShapes.Circle:
                    bool bFoundCi = false;
                    for (int cid = 0; cid < data[0].Length; cid++)
                    {
                        foreach (Geometry pt in retval)
                        {

                            if (InsideRadius(pt, ChunkExtent.Id2XY(cid)))
                            {
                                outval++;
                                bFoundCi = true;
                            }
                        }
                        if (bFoundCi) outChunk[cid] = outval / area;
                        else outChunk[cid] = OpNodataVal;
                    }
                    break;
                case RasterOperators.KernelShapes.Square:
                    bool bFoundSq = false;
                    for (int cid = 0; cid < data[0].Length; cid++)
                    {
                        foreach (Geometry pt in retval)
                        {

                            if (InsideRadius(pt, ChunkExtent.Id2XY(cid)))
                            {
                                outval++;
                                bFoundSq = true;
                            }
                        }
                        if (bFoundSq) outChunk[cid] = outval / area;
                        else outChunk[cid] = OpNodataVal;
                    }
                    break;
            }
        }

    }
}