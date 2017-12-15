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

        // Spacing gets used only for the secondary method
        private decimal _spacing;

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

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);

            WriteCSVHeaders();
        }

        /// <summary>
        /// Secondary constructor for when we want points at regular intervals.
        /// </summary>
        /// <param name="vLineShp"></param>
        /// <param name="rRasters"></param>
        /// <param name="outCSV"></param>
        /// <param name="ptsspacing"></param>
        public LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo outCSV, decimal ptsspacing)
            : base(rRasters, new List<Raster>())
        {
            _vinput = vLineShp;
            _csvfile = outCSV;
            _spacing = ptsspacing;

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);

            WriteCSVHeaders();
        }

        /// <summary>
        /// Write the headers to the CSV file
        /// </summary>
        private void WriteCSVHeaders()
        {
            List<string> csvline = new List<string>() { "FID", "X", "Y" };
            foreach (Raster r in _inputRasters)
                csvline.Add(r.GISFileInfo.Name);

            using (StreamWriter stream = new StreamWriter(_csvfile.FullName))
                stream.WriteLine(String.Join(",", csvline));
        }

        /// <summary>
        /// Write rows to the CSV file if applicable
        /// </summary>
        /// <param name="csvRows"></param>
        private void WriteLinesToCSV(List<string> csvRows)
        {
            // Now write any values to a CSV file but only bother if there are lines to write
            if (csvRows.Count > 0)
                using (StreamWriter stream = new StreamWriter(_csvfile.FullName, true))
                    foreach (string row in csvRows)
                        stream.WriteLine(row);
        }

        /// <summary>
        /// Chunk op for the pixel intersection method
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outBuffers"></param>
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
                            (xydec[0]).ToString(),
                            (xydec[1]).ToString()
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
            WriteLinesToCSV(csvRows);
        }


        /// <summary>
        /// The second method works inside out from the other 
        /// by travelling along lines and seeing what's underneath them at specific intervals
        /// </summary>
        public void RunWithSpacing()
        {
            foreach (VectorFeature feat in _vinput.Features.Values)
            {
                Geometry line = feat.Feat.GetGeometryRef();
                double length = line.Length();
                List<string> csvRows = new List<string>();

                // Segmentize makes sure there is not segment that is longer than distance d
                line.Segmentize((double)_spacing);
                for (int pti = 0; pti < line.GetPointCount(); pti++)
                {
                    double[] pt = new double[2];
                    line.GetPoint(pti, pt);

                    int[] rowcol = OpExtent.Pt2RowCol(pt);

                    List<string> csvCols = new List<string>() {
                            feat.Feat.GetFID().ToString(),
                            pt[0].ToString(),
                            pt[1].ToString()
                        };
                    for (int did = 0; did < _inputRasters.Count; did++)
                    {
                        T[] _buffer = new T[1];
                        _inputRasters[did].Read(rowcol[1], rowcol[0], 1, 1, _buffer);
                        if (_buffer[0].Equals(inNodataVals[did]))
                            csvCols.Add("");
                        else
                            csvCols.Add(_buffer[0].ToString());
                    }

                    csvRows.Add(String.Join(",", csvCols));
                    WriteLinesToCSV(csvRows);
                }
            }
        }
    }
}
