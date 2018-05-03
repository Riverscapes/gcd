using System;
using System.IO;
using System.Collections.Generic;
using OSGeo.OGR;
using System.Linq;
using System.Text.RegularExpressions;

namespace GCDConsoleLib
{
    public class Vector : GISDataset
    {
        public static string CGDMASKFIELD = "GCDFID";

        private static Dictionary<string, string> _driverstrings = new Dictionary<string, string>
        {
            {@".*\.shp","ESRI Shapefile"},
            {@".*\.(json|geojson)","GeoJSON"}
        };

        private Driver _drv;
        internal DataSource _ds;
        public Dictionary<long, VectorFeature> Features;
        public Dictionary<string, VectorField> Fields;
        public string FIDColumn;
        public string LayerName { get; private set; }

        /// <summary>
        /// Construct a vector by opening up a file on disk and reading it.
        /// </summary>
        /// <param name="sFilename"></param>
        public Vector(FileInfo sFilename) : base(sFilename)
        {
            Features = new Dictionary<long, VectorFeature>();
            Fields = new Dictionary<string, VectorField>();

            _load();
        }

        private void _Init(string sProj)
        {
            Proj = new Projection(sProj);
        }

        /// <summary>
        /// Create a vector file.
        /// </summary>
        /// <param name="leaveopen"></param>
        public override void Create(bool leaveopen = true)
        {
            if (GISFileInfo.Exists)
                OSGeo.GDAL.Gdal.Unlink(GISFileInfo.FullName);

            List<string> creationOpts = new List<string>();

            _drv = GetDriver(GISFileInfo);

            if (!leaveopen)
                UnloadDS();
        }

        /// <summary>
        /// Because of inconsistencies with how ESRI and GDAL handle FID and because the GDAL rasterize method lacks
        /// the ability to raterize using its own FIDs we need to fake this column. This function will augment a 
        /// dataset with a GCDFID field. 
        /// 
        /// It's not a great solution so anyone reading this who has a better idea.... have at it.
        /// </summary>
        /// <param name="srcds">Dataset of the file you want to augment. Watch that it's not already open.</param>
        public void CreateGCDFID(DataSource srcds)
        {
            // Populate some important metadata
            Layer mLayer = srcds.GetLayerByIndex(0);

            // Only crete it if it isn't already there
            if (mLayer.FindFieldIndex(CGDMASKFIELD, 1) == -1)
                mLayer.CreateField(new FieldDefn(CGDMASKFIELD, FieldType.OFTInteger), 0);

            int GCDFIDID = mLayer.FindFieldIndex(CGDMASKFIELD, 1);

            // Get our FEATURE definitions and make sure to set the GCDMASK to the FID that GDAL
            // Creates for us.
            Feature mFeat = mLayer.GetNextFeature();
            while (mFeat != null)
            {
                int gdalGID = (int)mFeat.GetFID();
                mFeat.SetField(GCDFIDID, gdalGID);
                mLayer.SetFeature(mFeat);
                mFeat = mLayer.GetNextFeature();
            }
        }

        private GDalGeometryType _geometryType;
        public GDalGeometryType GeometryType
        {
            get
            {
                return _geometryType;
            }
            protected set { _geometryType = value; }
        }

        /// <summary>
        /// Object hygiene is super important with GDAL. 
        /// </summary>
        public override void UnloadDS()
        {
            if (_ds != null)
            {
                _ds.Dispose();
                _ds = null;
            }
            //if (_drv != null)
            //{
            //    _drv.Dispose();
            //    _drv = null;
            //}
        }

        /// <summary>
        /// Copy the vector somewhere else
        /// </summary>
        /// <param name="destPath"></param>
        public override void Copy(FileInfo destPath)
        {
            Open();
            DataSource _cpyDs = _drv.CopyDataSource(_ds, destPath.FullName, null);

            // Now we add our Unique GCDFID field
            CreateGCDFID(_cpyDs);


            _cpyDs.Dispose();
            UnloadDS();
        }

        /// <summary>
        /// Open the vector file
        /// </summary>
        /// <param name="write"></param>
        public override void Open(bool write = false)
        {
            // Register GDal and get the driver objects
            GdalConfiguration.ConfigureOgr();
            if (_ds == null)
            {
                _ds = Ogr.Open(GISFileInfo.FullName, 0); // 0 => Read-only
                _drv = _ds.GetDriver();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Driver GetDriver(FileInfo src)
        {
            GdalConfiguration.ConfigureOgr();
            Driver driverobj = null;
            bool found = false;
            foreach (KeyValuePair<string, string> kvp in _driverstrings)
            {
                Regex r = new Regex(kvp.Key, RegexOptions.IgnoreCase);

                if (r.IsMatch(src.Name))
                {
                    driverobj = Ogr.GetDriverByName(kvp.Value);
                    found = true;
                    break;
                }
            }
            if (!found) throw new FileLoadException(String.Format("No driver for file: {0}", src.FullName));

            return driverobj;
        }

        /// <summary>
        /// Is a point inside a feature?
        /// </summary>
        /// <returns></returns>
        public List<string> ShapesContainPoint(double x, double y, string fieldName, List<long> shapemask = null)
        {
            if (!Fields.ContainsKey(fieldName))
                throw new ArgumentException("Field Not found: " + fieldName);

            Open();
            List<string> retVal = new List<string>();

            Geometry pt = new Geometry(wkbGeometryType.wkbPoint);
            pt.AddPoint(x, y, 0);

            // If no mask provided test against everything
            if (shapemask == null)
                shapemask = Features.Keys.ToList();

            foreach (long fid in shapemask)
            {
                if (Features[fid].Feat.GetGeometryRef().Contains(pt))
                    retVal.Add(Features[fid].Feat.GetFieldAsString(fieldName));
            }

            return retVal;
        }

        /// <summary>
        /// Is a point inside a feature (return only FID)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fieldName"></param>
        /// <param name="shapemask"></param>
        /// <returns></returns>
        public List<long> ShapesContainPoint(double x, double y, List<long> shapemask = null)
        {
            Open();
            List<long> retVal = new List<long>();

            Geometry pt = new Geometry(wkbGeometryType.wkbPoint);
            pt.AddPoint(x, y, 0);

            // If no mask provided test against everything
            if (shapemask == null)
                shapemask = Features.Keys.ToList();

            foreach (long fid in shapemask)
            {
                if (Features[fid].Feat.GetGeometryRef().Contains(pt))
                    retVal.Add(fid);
            }

            return retVal;
        }

        /// <summary>
        /// This is a quicker operation and returns the first shape only
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="shapemask"></param>
        /// <returns></returns>
        public long? ShapeContainPoint(double x, double y, List<long> shapemask = null)
        {
            Open();
            long? retval = null;

            Geometry pt = new Geometry(wkbGeometryType.wkbPoint);
            pt.AddPoint_2D(x, y);

            // If no mask provided test against everything
            if (shapemask == null)
                shapemask = Features.Keys.ToList();

            foreach (long fid in shapemask)
            {
                Geometry geo = Features[fid].Feat.GetGeometryRef();
                // Distance is slightly better
                retval = fid;
                if (geo.Contains(pt))
                {
                    retval = fid;
                    break;
                }
            }

            return retval;
        }

        /// <summary>
        /// Quick calculation if a point is inside a rectangle
        /// </summary>
        /// <param name="feat"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool PointEnvelopeIntersection(Envelope env, double x, double y)
        {
            // Return as fast as possible
            return (y <= env.MaxY && y >= env.MinY && x <= env.MaxX && x >= env.MinX);
        }

        /// <summary>
        /// Quick calculation if two envolopes intersect
        /// </summary>
        /// <param name="env1"></param>
        /// <param name="env2"></param>
        /// <returns></returns>
        public static bool EnvelopeEnvelopeIntersection(Envelope env1, Envelope env2)
        {
            return (
                env1.MaxX < env2.MinX ||
                env1.MinX > env2.MaxX ||
                env1.MaxY < env2.MinY ||
                env1.MinY > env2.MaxY);
        }

        /// <summary>
        /// Is a extent intersecting a feature?
        /// </summary>
        /// <returns>returns a double array [x,y]</returns>
        public Dictionary<long, VectorFeature> FeaturesIntersectExtent(ExtentRectangle ext)
        {
            Open();
            Dictionary<long, VectorFeature> retVal = new Dictionary<long, VectorFeature>();

            Geometry ring = new Geometry(wkbGeometryType.wkbLinearRing);
            ring.AddPoint((double)ext.Left, (double)ext.Top, 0);
            ring.AddPoint((double)ext.Right, (double)ext.Top, 0);
            ring.AddPoint((double)ext.Right, (double)ext.Bottom, 0);
            ring.AddPoint((double)ext.Left, (double)ext.Bottom, 0);
            ring.AddPoint((double)ext.Left, (double)ext.Top, 0);

            Geometry extrect = new Geometry(wkbGeometryType.wkbPolygon);
            extrect.AddGeometry(ring);

            foreach (KeyValuePair<long, VectorFeature> kvp in Features)
            {
                Geometry feat = kvp.Value.Feat.GetGeometryRef();
                if (feat != null && extrect.Intersects(feat))
                    retVal.Add(kvp.Key, kvp.Value);
            }
            return retVal;
        }

        /// <summary>
        /// We just want a list of keys back
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public List<long> FIDIntersectExtent(ExtentRectangle ext)
        {
            Open();
            List<long> retVal = new List<long>();

            Geometry ring = new Geometry(wkbGeometryType.wkbLinearRing);
            ring.AddPoint((double)ext.Left, (double)ext.Top, 0);
            ring.AddPoint((double)ext.Right, (double)ext.Top, 0);
            ring.AddPoint((double)ext.Right, (double)ext.Bottom, 0);
            ring.AddPoint((double)ext.Left, (double)ext.Bottom, 0);
            ring.AddPoint((double)ext.Left, (double)ext.Top, 0);

            Geometry extrect = new Geometry(wkbGeometryType.wkbPolygon);
            extrect.AddGeometry(ring);

            foreach (KeyValuePair<long, VectorFeature> kvp in Features)
            {
                Geometry feat = kvp.Value.Feat.GetGeometryRef();
                if (feat != null && extrect.Intersects(feat))
                    retVal.Add(kvp.Key);
            }
            return retVal;
        }

        public List<Geometry> GeometriesIntersectExtent(ExtentRectangle ext)
        {
            List<VectorFeature> feats = FeaturesIntersectExtent(ext).Values.ToList();
            return feats.Select(x => x.Feat.GetGeometryRef()).ToList();
        }

        /// <summary>
        /// Transform a cell into a rectangle geometry
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static Geometry CellToRect(int row, int col, ExtentRectangle ext)
        {
            double l = (double)(ext.Left + ((col - 1) * ext.CellWidth));
            double r = l + (double)ext.CellWidth;
            double t = (double)(ext.Top + ((row - 1) * ext.CellHeight));
            double b = t + (double)ext.CellWidth;

            Geometry ring = new Geometry(wkbGeometryType.wkbLinearRing);
            ring.AddPoint(l, t, 0);
            ring.AddPoint(r, t, 0);
            ring.AddPoint(r, b, 0);
            ring.AddPoint(l, b, 0);
            ring.AddPoint(l, t, 0);

            Geometry extrect = new Geometry(wkbGeometryType.wkbPolygon);
            extrect.AddGeometry(ring);
            return extrect;
        }

        /// <summary>
        /// Deletion
        /// </summary>
        /// <param name="sFilepath"></param>
        public static void Delete(FileInfo sFilepath)
        {
            Vector toDelete = new Vector(sFilepath);
            toDelete.Delete();
        }

        /// <summary>
        /// Deletion methods for vectors
        /// </summary>
        public override void Delete()
        {
            // We need a separate copy of the driver so we can delete it from the outside.
            Driver drv = Ogr.GetDriverByName(_drv.GetName());

            UnloadDS();
            drv.DeleteDataSource(GISFileInfo.FullName);
            drv.Dispose();
        }

        private void _load(bool leaveopen = false)
        {
            Open();

            // Populate some important metadata
            Layer mLayer = _ds.GetLayerByIndex(0);
            FIDColumn = mLayer.GetFIDColumn();
            LayerName = mLayer.GetName();

            _geometryType = new GDalGeometryType(mLayer.GetGeomType());

            // Get our FEATURE definitions
            Feature mFeat = mLayer.GetNextFeature();
            while (mFeat != null)
            {
                Features.Add(mFeat.GetFID(), new VectorFeature(mFeat));
                mFeat = mLayer.GetNextFeature();
            }

            // Now get our FIELD definitions
            FeatureDefn mFeatDfn = mLayer.GetLayerDefn();
            int iFldCnt = mFeatDfn.GetFieldCount();
            for (int fldId = 0; fldId < iFldCnt; fldId++)
            {
                FieldDefn mFldDef = mFeatDfn.GetFieldDefn(fldId);
                Fields.Add(mFldDef.GetName(), new VectorField(mFldDef, fldId));
            }

            // Spatial is way harder than it needs to be:
            OSGeo.OSR.SpatialReference sRef = mLayer.GetSpatialRef();
            if (sRef == null)
            {
                Exception ex = new Exception("Feature class is missing spatial reference");
                ex.Data["Path"] = GISFileInfo.FullName;
                throw ex;
            }

            string sRefstring = "";
            sRef.ExportToWkt(out sRefstring);
            _Init(sRefstring);

            if (!leaveopen)
            {
                UnloadDS();
            }
        }


        protected override void _initfromfile()
        {
            throw new NotImplementedException();
        }
    }
}
