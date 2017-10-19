using System;
using System.IO;
using System.Collections.Generic;
using OSGeo.OGR;

namespace GCDConsoleLib
{
    public class Vector : GISDataset
    {
        private static string _driverstring = "ESRI Shapefile";
        private Driver _drv;
        private DataSource _ds;
        public Dictionary<long, Feature> Features;
        public Dictionary<string, FieldDefn> Fields;
        public string FIDColumn;
        public string LayerName { get; private set; }

        /// <summary>
        /// Construct a vector by opening up a file on disk and reading it.
        /// </summary>
        /// <param name="sFilename"></param>
        public Vector(string sFilename) : base(sFilename)
        {
            Features = new Dictionary<long, Feature>();
            Fields = new Dictionary<string, FieldDefn>();

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
            if (File.Exists(filepath))
                OSGeo.GDAL.Gdal.Unlink(filepath);

            List<string> creationOpts = new List<string>();

            Driver driverobj = Ogr.GetDriverByName(_driverstring);

            //dataset = driverobj.Create(filepath, Extent.cols, Extent.rows, 1, Datatype, creationOpts.ToArray());
            //dataset.SetProjection(projection);

            if (!leaveopen)
            {
                Dispose();
            }
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
            if (_drv != null)
            {
                _drv.Dispose();
                _drv = null;
            }
        }

        public override void Copy(string destPath)
        {
            Open();
            DataSource _cpyDs = _drv.CopyDataSource(_ds, destPath, null);
            _cpyDs.Dispose();
            Dispose();
        }

        public override void Open(bool write = false)
        {
            // Register GDal and get the driver objects
            GdalConfiguration.ConfigureOgr();
            if (_ds == null)
            {
                _drv = Ogr.GetDriverByName(_driverstring);
                _ds = _drv.Open(filepath, 0); // 0 => Read-only
            }
        }

        public override void Delete()
        {
            Dispose();
            // We need a separate copy of the driver so we can delete it from the outside.
            Driver drv = Ogr.GetDriverByName(_driverstring);
            drv.DeleteDataSource(filepath);
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
                Features.Add(mFeat.GetFID(), mFeat);
                mFeat = mLayer.GetNextFeature();
            }

            // Now get our FIELD definitions
            FeatureDefn mFeatDfn = mLayer.GetLayerDefn();
            int iFldCnt = mFeatDfn.GetFieldCount();
            for (int fldId = 0; fldId < iFldCnt; fldId++)
            {
                FieldDefn mFldDef = mFeatDfn.GetFieldDefn(fldId);
                Fields.Add(mFldDef.GetName(), mFldDef);
            }

            // Spatial ref is way harder than it needs to be:
            OSGeo.OSR.SpatialReference sRef = mLayer.GetSpatialRef();
            string sRefstring = "";
            sRef.ExportToWkt(out sRefstring);
            _Init(sRefstring);

            if (!leaveopen)
            {
                Dispose();
            }
        }

        public void AddField(string sName, GDalFieldType fType, int? precision)
        {
            Open();
            Layer mLayer = _ds.GetLayerByIndex(0);
            FieldDefn fDef = new FieldDefn(sName, fType._origType);
            if (precision != null)
            {
                fDef.SetPrecision((int)precision);
            }
            mLayer.CreateField(fDef, 0); // 0 => Approx ok
        }

    }
}
