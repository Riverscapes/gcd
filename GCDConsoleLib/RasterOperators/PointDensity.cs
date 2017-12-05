using System;
using System.Collections.Generic;
using OSGeo.OGR;
using System.Diagnostics;

namespace GCDConsoleLib.Internal.Operators
{
    public class PointDensity : BaseOperator<double>
    {
        private static int NUMWINDOWS = 10;

        private Raster _routput;
        private double _fsize;
        private decimal _fsizedec;
        private RasterOperators.KernelShapes _kshape;
        private ExtentRectangle VectorChunkExtent;
        private Vector _vinput;
        private double area;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="newRect"></param>
        /// <param name="rOutputRaster"></param>
        public PointDensity(Raster rDEM, Vector vPointCloud, Raster OutputRaster, RasterOperators.KernelShapes eKernel, decimal fSize)
            : base(new List<Raster>() { rDEM }, OutputRaster)
        {
            _vinput = vPointCloud;
            _routput = OutputRaster;
            _kshape = eKernel;
            _fsize = (double)fSize;
            _fsizedec = fSize;

            // set the rows to be a certain multiple of fSize windows
            _oprows = (int)Math.Ceiling((NUMWINDOWS * fSize) / rDEM.Extent.CellHeight);

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);

            switch (eKernel)
            {
                case RasterOperators.KernelShapes.Circle:
                    area = Math.PI * Math.Pow((double)fSize, 2);
                    break;
                case RasterOperators.KernelShapes.Square:
                    area = Math.Pow((double)fSize * 2, 2);
                    break;
            }
        }

        public bool InsideRadius(Geometry pt, Geometry pt2)
        {
            // do distnce vertically and then horizontally.
            return pt.Distance(pt2) <= _fsize;
        }

        public bool InsideSquare(Geometry pt, Geometry pt2)
        {
            return Math.Abs(pt.GetX(0) - pt2.GetX(0)) <= _fsize && Math.Abs(pt.GetY(0) - pt2.GetY(0)) <= _fsize;
        }

        /// <summary>
        /// Put our points from this chunk into a bunch of bins
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public List<Geometry>[,] BinPoints(List<Geometry> pts)
        {
            int vbins = (int)(ChunkExtent.Rows / _fsize) + 2;
            int hbins = (int)(ChunkExtent.Cols / _fsize) + 2; // the extra 2 is a buffer on either side

            List<Geometry>[,] bins = new List<Geometry>[hbins, vbins];

            // Populate all the bins with empty lists
            for (int idx = 0; idx < hbins; idx++)
                for (int idy = 0; idy < vbins; idy++)
                    bins[idx, idy] = new List<Geometry>();

            foreach (Geometry pt in pts)
            {
                int[] bc = TranslateCIDToBin(pt);
                bins[bc[0], bc[1]].Add(pt);
            }

            return bins;
        }

        /// <summary>
        /// Given a point, what bina re we in?
        /// </summary>
        /// <param name="pt"></param>
        /// <returns>integet array [xbin, ybin] </returns>
        public int[] TranslateCIDToBin(Geometry pt)
        {
            int xbin = (int)Math.Floor((pt.GetX(0) - (double)VectorChunkExtent.Left) / _fsize);
            int ybin = (int)Math.Floor((pt.GetY(0) - (double)VectorChunkExtent.Top) / _fsize);
            return new int[2] { xbin, ybin };
        }

        /// <summary>
        /// Translate a cell id into up to 9 bins to check for valid points
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<int[]> GetRelevantBins(Geometry pt)
        {
            List<int[]> bins = new List<int[]>();
            // Get the bin we're in
            int[] bc = TranslateCIDToBin(pt);

            // add the bins we want to check into the array. We should get 9 back
            for (int idx = bc[0] - 1; idx <= idx + 1; idx++)
                for (int idy = bc[1] - 1; idy <= idy + 1; idy++)
                    bins.Add(new int[2] { idx, idy });

            return bins;
        }

        protected override void ChunkOp(List<double[]> data, double[] outChunk)
        {
            // Get all the points in this chunk
            VectorChunkExtent = ChunkExtent.Buffer(_fsizedec);
            List<Geometry>[,] bins = BinPoints(_vinput.PointsInExtent(VectorChunkExtent));

            for (int cid = 0; cid < outChunk.Length; cid++)
            {
                // Save us a metric Tonne of effort by giving up on nodatavals in the DEM
                if (data[0][cid] == _rasternodatavals[0])
                {
                    outChunk[cid] = OpNodataVal;
                    break;
                }
                int outval = 0;

                decimal[] cidxy = ChunkExtent.Id2XY(cid);
                Geometry pt2 = new Geometry(wkbGeometryType.wkbPoint);
                pt2.AddPoint((double)cidxy[0], (double)cidxy[1], 0);

                List<int[]> bins2test = GetRelevantBins(pt2);

                // create a circle and then buffer it. 
                foreach (int[] binids in bins2test)
                {
                    foreach (Geometry pt in bins[binids[0], binids[1]])
                    {
                        // If we're a circle we have to do a more expensive operation now
                        if (_kshape == RasterOperators.KernelShapes.Circle)
                        {
                            if (InsideRadius(pt, pt2)) outval++;
                        }
                        else if (InsideSquare(pt, pt2)) outval++;
                    }
                }
                outChunk[cid] = outval / area;
            }
            Debug.WriteLine("here");
        }

    }
}