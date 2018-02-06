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
        private static Dictionary<string, string> _driverstrings = new Dictionary<string, string>
        {
            {@".*\.shp","ESRI Shapefile"},
            {@".*\.(json|geojson)","GeoJSON"}
        };
        private Driver _drv;
        private DataSource _ds;
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
                Dispose();
        }

        /// <summary>
        /// Object hygiene is super important with GDAL. 
        /// </summary>
        public override void Dispose()
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
            _cpyDs.Dispose();
            Dispose();
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
        public List<string> ShapesContainPoint(double x, double y, string fieldName)
        {
            if (!Fields.ContainsKey(fieldName))
                throw new ArgumentException("Field Not found: " + fieldName);

            Open();
            List<string> retVal = new List<string>();
            Layer mLayer = _ds.GetLayerByIndex(0);

            Geometry pt = new Geometry(wkbGeometryType.wkbPoint);
            pt.AddPoint(x, y, 0);

            foreach (KeyValuePair<long, VectorFeature> kvp in Features)
            {
                if (kvp.Value.Feat.GetGeometryRef().Contains(pt))
                    retVal.Add(kvp.Value.Feat.GetFieldAsString(fieldName));
            }

            return retVal;
        }

        /// <summary>
        /// Is a point inside a feature?
        /// </summary>
        /// <returns>returns a double array [x,y]</returns>
        public List<VectorFeature> FeaturesIntersectExtent(ExtentRectangle ext)
        {
            Open();
            List<VectorFeature> retVal = new List<VectorFeature>();
            Layer mLayer = _ds.GetLayerByIndex(0);

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
                    retVal.Add(kvp.Value);
            }
            return retVal;
        }

        public List<Geometry> GeometriesIntersectExtent(ExtentRectangle ext)
        {
            List<VectorFeature> feats = FeaturesIntersectExtent(ext);
            return feats.Select(x => x.Feat.GetGeometryRef()).ToList();
        }

        public static Geometry CellToRect(int row, int col, ExtentRectangle ext)
        {
            double l = (double)(ext.Left + ((col-1) * ext.CellWidth));
            double r = l + (double)ext.CellWidth;
            double t = (double)(ext.Top + ((row-1) * ext.CellHeight));
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

            Dispose();
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
                Fields.Add(mFldDef.GetName(), new VectorField(mFldDef));
            }

            // Spatial is way harder than it needs to be:
            OSGeo.OSR.SpatialReference sRef = mLayer.GetSpatialRef();
            string sRefstring = "";
            sRef.ExportToWkt(out sRefstring);
            _Init(sRefstring);

            if (!leaveopen)
            {
                Dispose();
            }
        }

        public int AddField(string sName, GDalFieldType fType, int? precision)
        {
            Open();
            Layer mLayer = _ds.GetLayerByIndex(0);
            FieldDefn fDef = new FieldDefn(sName, fType._origType);
            if (precision != null)
            {
                fDef.SetPrecision((int)precision);
            }

            Fields.Add(fDef.GetName(), new VectorField(fDef));

            return mLayer.CreateField(fDef, 0); // 0 => Approx ok
        }

        protected override void _initfromfile()
        {
            throw new NotImplementedException();
        }
    }
}
