using System;
using System.Collections.Generic;
using OSGeo.OGR;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GCDConsoleLib.Internal.Operators
{
    class LinearExtractor<T> : BaseOperator<T>
    {
        private static int CHUNKSIZE = 100;
        private FileInfo _csvfile;
        private Vector _vinput;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rDEM"></param>
        /// <param name="vPointCloud"></param>
        /// <param name="OutputRaster"></param>
        /// <param name="eKernel"></param>
        /// <param name="fSize"></param>
        public LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo outCSV)
            : base(rRasters, new List<Raster>())
        {
            _vinput = vLineShp;
            _csvfile = outCSV;
            chunkRows = CHUNKSIZE;

            List<string> csvline = new List<string>() { "FID", "X", "Y" };
            foreach (Raster r in rRasters)
                csvline.Add(r.GISFileInfo.Name);

            using (StreamWriter stream = new StreamWriter(_csvfile.FullName))
                stream.WriteLine(String.Join(",", csvline));

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);
        }

        protected override void ChunkOp(List<T[]> data, List<T[]> outBuffers)
        {
            // Get all the lines in this chunk
            List<VectorFeature> chunkFeats = _vinput.FeaturesIntersectExtent(ChunkExtent);
            List<string> csvRows = new List<string>();

            if (chunkFeats.Count == 0) return;

            // Loop over all the cells in this chunk
            for (int cid = 0; cid < data[0].Length; cid++)
            {
                // If all null then continue so we don't do the expensive stuff.
                if (data.Select((x, i) => x[cid].Equals(inNodataVals[i])).All(i => i)) continue;

                int[] rowcol = ChunkExtent.Id2RowCol(cid);
                Geometry cellRect = Vector.CellToRect(rowcol[0], rowcol[1], ChunkExtent);

                // create a circle and then buffer it.
                foreach (VectorFeature feat in chunkFeats)
                {
                    if (cellRect.Intersects(feat.Feat.GetGeometryRef()))
                    {
                        decimal[] xydec = ChunkExtent.Id2XY(cid);
                        List<string> csvline = new List<string>() {
                            feat.Feat.GetFID().ToString(),
                            (xydec[0] + ChunkExtent.CellWidth/2).ToString(),
                            (xydec[1] + ChunkExtent.CellHeight/2).ToString()
                        };
                        for (int did = 0; did < data.Count; did++)
                        {
                            string rVal = "";
                            if (!data[did][cid].Equals(inNodataVals[did]))
                                rVal = data[did][cid].ToString();
                            csvline.Add(rVal);

                        }
                        csvRows.Add(String.Join(",", csvline));
                    }
                }

            }

            // Now write any values to a CSV file.
            if (csvRows.Count > 0)
                using (StreamWriter stream = new StreamWriter(_csvfile.FullName, true))
                    foreach (string row in csvRows)
                        stream.WriteLine(row);

        }
    }
}
