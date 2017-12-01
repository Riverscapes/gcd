using System;
using System.Collections.Generic;
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
        private delegate bool insideOp(Geometry pt1, Geometry pt2);
        private insideOp theOp;

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
                    theOp = new insideOp(InsideRadius);
                    area = Math.PI * Math.Pow((double)fSize, 2);
                    break;
                case RasterOperators.KernelShapes.Square:
                    theOp = new insideOp(InsideSquare);
                    area = Math.Pow((double)fSize, 2);
                    break;
            }
        }


        public bool InsideRadius(Geometry pt, Geometry pt2)
        {
            // do distnce vertically and then horizontally.
            return pt.Distance(pt2) <= (double)_fsize;
        }

        public bool InsideSquare(Geometry pt, Geometry pt2)
        {
            return Math.Abs(pt.GetX(0) - pt2.GetX(0)) <= (double)_fsize && Math.Abs(pt.GetX(0) - pt2.GetY(1)) <= (double)_fsize;
        }


        protected override void ChunkOp(List<double[]> data, double[] outChunk)
        {
            // Get all the points in this chunk
            List<Geometry> retval = _vinput.PointsInExtent(ChunkExtent.Buffer(_fsize));
            for (int cid = 0; cid < data[0].Length; cid++)
            {
                double outval = 0;
                bool bFoundCi = false;

                foreach (Geometry pt in retval)
                {
                    Geometry pt2 = new Geometry(wkbGeometryType.wkbPoint);
                    decimal[] cidxy = ChunkExtent.Id2XY(cid);
                    pt2.AddPoint((double)cidxy[0], (double)cidxy[1], 0);
                    if (InsideRadius(pt, pt2))
                    {
                        outval++;
                        bFoundCi = true;
                    }
                }
                if (bFoundCi) outChunk[cid] = outval / area;
                else outChunk[cid] = OpNodataVal;
            }
        }

    }
}