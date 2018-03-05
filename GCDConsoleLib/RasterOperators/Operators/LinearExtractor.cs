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
        // define all the col names as static strings
        private static string FIELD_FID = "fid";
        private static string FIELD_X = "x";
        private static string FIELD_Y = "y";
        private static string FIELD_DISTANCE = "distance";
        private static string FIELD_STATION = "station";

        // and also a useful enum 
        private enum ecols { FID, X, Y, DISTANCE, STATION }

        private static int CHUNKSIZE = 100;
        private FileInfo _csvfile;
        string sFieldName;
        private Vector _vinput;
        private StreamWriter _stream;

        // Just a useful enum to use
        // Define all the non-variable column names as static
        private static Dictionary<ecols, string> cols = new Dictionary<ecols, string>(){
           {ecols.FID, "FID"},
           {ecols.X, "X"},
           {ecols.Y, "Y"},
           {ecols.DISTANCE, "Distance"}, // This one will take on the field name later
           {ecols.STATION, "Station"}
        };
        // The final header list goes into this list
        private List<ecols> header;

        // We keep a list of the XY coords of each feature so we don't have to calculate it every time
        private Dictionary<long, double[]> fid0xy;

        // Spacing gets used only for the secondary method
        private double _spacing;

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
            sFieldName = "";

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);

            header = new List<ecols>() { ecols.FID, ecols.STATION, ecols.X, ecols.Y };

            _stream = new StreamWriter(_csvfile.FullName, true);
            WriteCSVHeaders();
            GetFID0XY();

        }

        /// <summary>
        /// The second constructor still uses intersection method but adds a field for an extracted distance field
        /// </summary>
        /// <param name="vLineShp"></param>
        /// <param name="rRasters"></param>
        /// <param name="outCSV"></param>
        public LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo outCSV, string FieldName)
            : base(rRasters, new List<Raster>())
        {
            _vinput = vLineShp;
            _csvfile = outCSV;
            chunkRows = CHUNKSIZE;

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);

            sFieldName = FieldName;
            header = new List<ecols>() { ecols.FID, ecols.STATION, ecols.DISTANCE, ecols.X, ecols.Y };

            _stream = new StreamWriter(_csvfile.FullName, true);
            WriteCSVHeaders();
            GetFID0XY();

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
            _spacing = (double)ptsspacing;

            header = new List<ecols>() { ecols.FID, ecols.STATION, ecols.X, ecols.Y };

            sFieldName = "";

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);
            _stream = new StreamWriter(_csvfile.FullName, true);
            WriteCSVHeaders();
            GetFID0XY();
        }

        /// <summary>
        /// Secondary constructor for when we want points at regular intervals.
        /// </summary>
        /// <param name="vLineShp"></param>
        /// <param name="rRasters"></param>
        /// <param name="outCSV"></param>
        /// <param name="ptsspacing"></param>
        public LinearExtractor(Vector vLineShp, List<Raster> rRasters, FileInfo outCSV, decimal ptsspacing, string FieldName)
            : base(rRasters, new List<Raster>())
        {
            _vinput = vLineShp;
            _csvfile = outCSV;
            _spacing = (double)ptsspacing;

            header = new List<ecols>() { ecols.FID, ecols.DISTANCE, ecols.STATION, ecols.X, ecols.Y };

            sFieldName = FieldName;

            // Calling this again after setting the rows will give us a nicer chunk size
            SetOpExtent(OpExtent);
            _stream = new StreamWriter(_csvfile.FullName, true);
            WriteCSVHeaders();
            GetFID0XY();
        }

        /// <summary>
        /// Clean up and close the stream
        /// </summary>
        ~LinearExtractor()
        {

        }

        /// <summary>
        /// We pre-calculate the first point in each feature so it makes the station 
        /// calculation easier later
        /// </summary>
        private void GetFID0XY()
        {
            fid0xy = new Dictionary<long, double[]>();
            foreach (KeyValuePair<long, VectorFeature> kvp in _vinput.Features)
            {
                double[] output = new double[2];
                kvp.Value.Feat.GetGeometryRef().GetPoint_2D(0, output);

                fid0xy.Add(kvp.Key, (double[])output.Clone());
            }

        }

        /// <summary>
        /// Write the headers to the CSV file
        /// </summary>
        private void WriteCSVHeaders()
        {
            List<string> headerline = new List<string>();
            foreach (ecols item in header)
            {
                switch (item)
                {
                    case (ecols.FID): headerline.Add(FIELD_FID); break;
                    case (ecols.X): headerline.Add(FIELD_X); break;
                    case (ecols.Y): headerline.Add(FIELD_Y); break;
                    case (ecols.DISTANCE): headerline.Add(sFieldName); break;
                    case (ecols.STATION): headerline.Add(FIELD_STATION); break;
                }
            }

            foreach (Raster r in _inputRasters)
                headerline.Add(r.GISFileInfo.Name);

            _stream.WriteLine(String.Join(",", headerline));
        }

        /// <summary>
        /// Write rows to the CSV file if applicable
        /// </summary>
        /// <param name="csvRows"></param>
        private void WriteLinesToCSV(List<string> csvRows)
        {
            // Now write any values to a CSV file but only bother if there are lines to write
            if (csvRows.Count > 0)
                foreach (string row in csvRows)
                    _stream.WriteLine(row);
        }

        /// <summary>
        /// Chunk op for the pixel intersection method
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outBuffers"></param>
        protected override void ChunkOp(List<T[]> data, List<T[]> outBuffers)
        {
            throw new NotImplementedException("This operation has problems and was depprecated. Please use the other constructor");
            // Get all the lines in this chunk
            Dictionary<long, VectorFeature> chunkFeats = _vinput.FeaturesIntersectExtent(ChunkExtent);
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
                foreach (KeyValuePair<long, VectorFeature> feat in chunkFeats)
                {
                    if (cellRect.Intersects(feat.Value.Feat.GetGeometryRef()))
                    {
                        decimal[] xydec = ChunkExtent.Id2XY(cid);
                        int fid = (int)feat.Value.Feat.GetFID();

                        double station = ptDist(fid0xy[feat.Key], new double[] { (double)xydec[0], (double)xydec[1] });

                        List<string> csvline = new List<string>();
                        foreach (ecols item in header)
                        {
                            switch (item)
                            {
                                case (ecols.FID): csvline.Add(fid.ToString()); break;
                                case (ecols.X): csvline.Add((xydec[0]).ToString()); break;
                                case (ecols.Y): csvline.Add((xydec[1]).ToString()); break;
                                case (ecols.DISTANCE): csvline.Add((0).ToString()); break;
                                case (ecols.STATION): csvline.Add(station.ToString()); break;
                            }
                        }

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
        /// The Second method samples at every vertex along hte line
        /// </summary>
        //public void RunWithMaxSegLength()
        //{
        //    foreach (VectorFeature feat in _vinput.Features.Values)
        //    {
        //        Geometry line = feat.Feat.GetGeometryRef();
        //        double length = line.Length();
        //        List<string> csvRows = new List<string>();

        //        // Segmentize makes sure there is not segment that is longer than distance d
        //        line.Segmentize(_spacing);

        //        double lenAcc = 0; // The length accumulator

        //        double[] pt = new double[2];
        //        line.GetPoint(0, pt);

        //        for (int pti = 0; pti < line.GetPointCount(); pti++)
        //        {
        //            double[] newpt = new double[2];
        //            line.GetPoint(pti, newpt);

        //            int[] rowcol = OpExtent.Pt2RowCol(pt);

        //            int fid = (int)feat.Feat.GetFID();

        //            lenAcc += ptDist(pt, newpt);

        //            List<string> csvcols = new List<string>();
        //            foreach (ecols item in header)
        //            {
        //                switch (item)
        //                {
        //                    case (ecols.FID): csvcols.Add(fid.ToString()); break;
        //                    case (ecols.X): csvcols.Add((pt[0]).ToString()); break;
        //                    case (ecols.Y): csvcols.Add((pt[1]).ToString()); break;
        //                    case (ecols.DISTANCE): csvcols.Add((0).ToString()); break;
        //                    case (ecols.STATION): csvcols.Add(lenAcc.ToString()); break;
        //                }
        //            }

        //            for (int did = 0; did < _inputRasters.Count; did++)
        //            {
        //                T[] _buffer = new T[1];
        //                _inputRasters[did].Read(rowcol[1], rowcol[0], 1, 1, _buffer);
        //                if (_buffer[0].Equals(inNodataVals[did]))
        //                    csvcols.Add("");
        //                else
        //                    csvcols.Add(_buffer[0].ToString());
        //            }

        //            csvRows.Add(String.Join(",", csvcols));
        //            WriteLinesToCSV(csvRows);
        //        }
        //    }
        //}

        /// <summary>
        /// Distance between two points
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double ptDist(double[] pt1, double[] pt2)
        {
            return Math.Sqrt(Math.Pow(pt2[1] - pt1[1], 2) + Math.Pow(pt2[0] - pt1[0], 2));
        }


        public void lineWrite(VectorFeature feat, double[] fractionalpt, double distance)
        {
            // Get the row and col of the cell we're looking for
            int[] rowcol = OpExtent.Pt2RowCol(fractionalpt);

            int fid = (int)feat.Feat.GetFID();

            List<string> csvRows = new List<string>();

            // Now we write a line to the file
            List<string> csvcols = new List<string>();
            foreach (ecols item in header)
            {
                switch (item)
                {
                    case (ecols.FID): csvcols.Add(fid.ToString()); break;
                    case (ecols.X): csvcols.Add((fractionalpt[0]).ToString()); break;
                    case (ecols.Y): csvcols.Add((fractionalpt[1]).ToString()); break;
                    case (ecols.DISTANCE): csvcols.Add(feat.GetFieldAsDouble(sFieldName).ToString()); break;
                    case (ecols.STATION): csvcols.Add((distance).ToString()); break;
                }
            }
            for (int did = 0; did < _inputRasters.Count; did++)
            {
                if (rowcol[1] > 0 && rowcol[1] < _inputRasters[did].Extent.Cols &&
                    rowcol[0] > 0 && rowcol[0] < _inputRasters[did].Extent.Rows)
                {
                    T[] _buffer = new T[1];
                    _inputRasters[did].Read(rowcol[1], rowcol[0], 1, 1, _buffer);
                    if (_buffer[0].Equals(inNodataVals[did]))
                        csvcols.Add("");
                    else
                        csvcols.Add(_buffer[0].ToString());

                }
                else
                {
                    csvcols.Add("");
                }
            }
            csvRows.Add(String.Join(",", csvcols));
            WriteLinesToCSV(csvRows);
        }

        /// <summary>
        /// This version does hard-coded regulat intervals
        /// </summary>
        public void RunWithSpacing()
        {
            foreach (VectorFeature feat in _vinput.Features.Values)
            {
                Geometry line = feat.Feat.GetGeometryRef();
                double length = line.Length();
                double totalDist = 0;
                double remaining = _spacing;


                double[] pt = new double[2];
                line.GetPoint_2D(0, pt);
                int ptCoint = line.GetPointCount();

                // Write the zero point
                lineWrite(feat, new double[2] { pt[0], pt[1] }, 0);

                for (int pti = 0; pti < ptCoint; pti++)
                {
                    double[] newpt = new double[2];
                    line.GetPoint_2D(pti, newpt);

                    double seglength = ptDist(pt, newpt);

                    // Only if our desired spacing overshoots a point do we really care
                    if (seglength >= remaining || pti == 0 || pti == ptCoint)
                    {
                        // Keep in mind newpt is the point that overshoots
                        double partialdist = remaining;
                        while (partialdist < seglength)
                        {
                            double fractionAlongSeg = (partialdist / seglength);
                            double[] fractionalpt = new double[2]{
                                pt[0] + ((newpt[0]-pt[0]) * fractionAlongSeg),
                                pt[1] + ((newpt[1]-pt[1]) * fractionAlongSeg)
                            };

                            lineWrite(feat, fractionalpt, partialdist + totalDist);

                            // Do this first so we can mutate partialdist
                            remaining = _spacing - (seglength - partialdist);
                            // This doesn't last if the while loop fails
                            partialdist += _spacing;
                        }
                    }
                    else
                    {
                        remaining -= seglength;
                    }

                    // The total distance along the line accumulator needs incrementing
                    totalDist += seglength;
                    pt = newpt;
                }

                // Write the last point
                lineWrite(feat, new double[2] { pt[0], pt[1] }, 0);
            }
            _stream.Close();
        }
    }
}
