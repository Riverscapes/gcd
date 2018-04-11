using System;
using System.IO;
using System.Collections.Generic;
using OSGeo.GDAL;
using UnitsNet;
using UnitsNet.Units;
using System.Linq;
using System.Diagnostics;

namespace GCDConsoleLib
{
    public class Raster : GISDataset, IDisposable
    {
        private bool _temporary;
        public enum RasterDriver : byte { GTiff, HFA }
        public LengthUnit VerticalUnits;
        private bool _bComputedStatistics { get; set; }

        private double? _origNodata;
        public double? origNodataVal
        {
            get
            {
                if (!IsLoaded && GISFileInfo.Exists)
                    _initfromfile();
                return _origNodata;
            }
            set { _origNodata = value; }
        }
        public bool HasNodata { get { return origNodataVal != null; } }
        public bool IsOpen { get { return _ds != null; } }
        public bool IsLoaded { get { return _extent != null; } }

        private GdalDataType _gdalDataType;
        public GdalDataType Datatype
        {
            get
            {
                if (!IsLoaded && GISFileInfo.Exists)
                    _initfromfile();
                return _gdalDataType;
            }
            protected set { _gdalDataType = value; }
        }


        private ExtentRectangle _extent;
        public ExtentRectangle Extent
        {
            get
            {
                if (!IsLoaded && GISFileInfo.Exists)
                    _initfromfile();
                return _extent;
            }
            set { _extent = value; }
        }

        private RasterDriver _driver;
        public RasterDriver driver
        {
            get
            {
                if (!IsLoaded && GISFileInfo.Exists)
                    _initfromfile();
                return _driver;
            }
            set { _driver = value; }
        }

        // We store the following in the guts since they are used there a lot
        protected Dataset _ds { get; set; }
        private bool _writepermission;

        /// <summary>
        /// Constructor for opening an existing Raster
        /// </summary>
        /// <param name="sFileInfo"></param>
        public Raster(FileInfo sFileInfo) : base(sFileInfo)
        { }

        /// <summary>
        /// Constructor to create a new raster file
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(Raster rTemplate, FileInfo sFilename, bool leaveopen = false) : base(sFilename)
        {
            _temporary = false;
            ExtentRectangle theExtent = new ExtentRectangle(rTemplate.Extent);
            _Init(RasterDriver.GTiff, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, theExtent, rTemplate.origNodataVal);
            if (!leaveopen)
                UnloadDS();
        }

        /// <summary>
        /// Sometimes we want to create a raster with all properties the same as a template except DataType (think Hillshade)
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(Raster rTemplate, FileInfo sFilename, GdalDataType dType, bool leaveopen = false) : base(sFilename)
        {
            _temporary = false;
            ExtentRectangle theExtent = new ExtentRectangle(rTemplate.Extent);
            _Init(RasterDriver.GTiff, rTemplate.VerticalUnits, rTemplate.Proj, dType, theExtent, rTemplate.origNodataVal);
            if (!leaveopen)
                UnloadDS();
        }

        // This is mainly for testing purposes
        public Raster(Raster rTemplate, bool leaveopen = false) : base()
        {
            _temporary = false;
            ExtentRectangle theExtent = new ExtentRectangle(rTemplate.Extent);
            _Init(RasterDriver.GTiff, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, theExtent, rTemplate.origNodataVal);
            if (!leaveopen)
                UnloadDS();
        }

        /// <summary>
        /// This is a create constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="rExtent"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(Raster rTemplate, ExtentRectangle rExtent, FileInfo sFilename, bool leaveopen = false) : base(sFilename)
        {
            _Init(rTemplate.driver, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, rExtent, rTemplate.origNodataVal);
            if (!leaveopen)
                UnloadDS();
        }

        /// <summary>
        /// Explicit Constructor to create a new raster
        /// </summary>
        /// <param name="fTop"></param>
        /// <param name="fLeft"></param>
        /// <param name="dCellHeight"></param>
        /// <param name="dCellWidth"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        /// <param name="dNoData"></param>
        /// <param name="psDriver"></param>
        /// <param name="eDataType"></param>
        /// <param name="psProjection"></param>
        /// <param name="psUnit"></param>
        public Raster(decimal fTop, decimal fLeft, decimal dCellHeight, decimal dCellWidth, int nRows, int nCols,
               double? dNoData, RasterDriver psDriver, GdalDataType dType,
               string psProjection, string psUnit, bool leaveopen = false) : base()
        {
            _temporary = false;
            ExtentRectangle theExtent = new ExtentRectangle(fTop, fLeft, dCellHeight, dCellWidth, nRows, nCols);
            _Init(psDriver, UnitFromString(psUnit), new Projection(psProjection), dType, theExtent, dNoData);
            if (!leaveopen)
                UnloadDS();
        }

        /// <summary>
        /// Temporary file constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(Raster rTemplate) : base(TempRasterFileInfo())
        {
            _temporary = true;
            ExtentRectangle theExtent = new ExtentRectangle(rTemplate.Extent);
            _Init(RasterDriver.GTiff, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, theExtent, rTemplate.origNodataVal);
            Create();
        }

        public static FileInfo TempRasterFileInfo()
        {
            return new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString() + ".tif");
        }

        /// <summary>
        /// Load all relevant settings from a file
        /// </summary>
        protected override void _initfromfile()
        {
            bool leaveOpen = IsOpen;
            Open();

            Band rBand1 = _ds.GetRasterBand(1);
            GdalDataType dType = new GdalDataType(rBand1.DataType);

            string sDriver = _ds.GetDriver().ShortName;
            ExtentRectangle theExtent = new ExtentRectangle(_ds);

            _Init(_str2driver(sDriver),
                UnitFromString(rBand1.GetUnitType()),
                new Projection(_ds.GetProjection()),
                dType, theExtent, GetNodataVal());

            if (!leaveOpen)
                UnloadDS();
        }

        /// <summary>
        /// Initialization function
        /// </summary>
        protected void _Init(RasterDriver rdDriver, LengthUnit lUnits, Projection proj, GdalDataType dType,
            ExtentRectangle theExtent, double? ndv)
        {
            _writepermission = false;
            origNodataVal = ndv;
            Datatype = dType;
            Extent = theExtent;
            Proj = proj;
            VerticalUnits = lUnits;
            driver = rdDriver;
        }

        public void CreateDS(Raster.RasterDriver driver, FileInfo finfo, ExtentRectangle theExtent, Projection proj, GdalDataType theType)
        {
            List<string> creationOpts = new List<string>();
            switch (driver)
            {
                case Raster.RasterDriver.GTiff:
                    creationOpts.Add("COMPRESS=LZW");
                    break;
                case Raster.RasterDriver.HFA:
                    creationOpts.Add("COMPRESS=PACKBITS");
                    break;
            }
            Driver driverobj = Gdal.GetDriverByName(Enum.GetName(typeof(Raster.RasterDriver), driver));

            _ds = driverobj.Create(finfo.FullName, theExtent.Cols, theExtent.Rows, 1, theType._origType, creationOpts.ToArray());
            _ds.SetGeoTransform(theExtent.Transform);
            _ds.SetProjection(proj.OriginalString);

            SetNoData((double)origNodataVal);
        }

        internal void SetNoData(double newNodataVal)
        {
            Open(true);
            Band band = _ds.GetRasterBand(1);
            if (HasNodata)
                band.SetNoDataValue(newNodataVal);
            _origNodata = newNodataVal;
            UnloadDS();
        }

        /// <summary>
        /// Handy unit convertor
        /// </summary>
        /// <param name="sUnit"></param>
        /// <returns></returns>
        private LengthUnit UnitFromString(string sUnit)
        {
            LengthUnit retUnit = LengthUnit.Undefined;
            if (sUnit != "")
                retUnit = Utility.Conversion.ParseLengthUnit(sUnit);

            return retUnit;
        }

        /// <summary>
        /// Get the original double? nodatavalue from the rasterfile 
        /// </summary>
        /// <returns></returns>
        private double? GetNodataVal()
        {
            Open();
            int hasndval;
            double nodatval;
            _ds.GetRasterBand(1).GetNoDataValue(out nodatval, out hasndval);
            double? ndv = null;
            if (hasndval == 1)
                ndv = (double?)nodatval;
            return ndv;

        }

        /// <summary>
        /// Create a raster.
        /// </summary>
        /// <param name="leaveopen"></param>
        public override void Create(bool leaveopen = false)
        {
            if (GISFileInfo.Exists)
                Gdal.Unlink(GISFileInfo.FullName);

            CreateDS(driver, GISFileInfo, Extent, Proj, Datatype);
            RefreshFileInfo();
            if (!leaveopen)
                UnloadDS();
        }

        /// <summary>
        /// Opening a dataset is generic so we do it here.
        /// </summary>
        public override void Open(bool write = false)
        {
            // Check if we're already open and have the correct permissions
            if (IsOpen && _writepermission.Equals(write)) return;

            // if it is open and we don't have the right permissions then close it and re-open it.
            if (IsOpen)
                UnloadDS();

            Access permission = Access.GA_ReadOnly;
            if (write) permission = Access.GA_Update;
            _writepermission = write;

            if (write)
            {
                // File exists AND we can overwrite it
                if (GISFileInfo.Exists && Utility.FileHelpers.IsGDALFileLocked(GISFileInfo.FullName, permission))
                    throw new IOException(string.Format("File `{0}` was locked for `{1}` operation", GISFileInfo, Enum.GetName(typeof(Access), permission)));
                // File does not exist but there is no directory to put it in.
                else if (!GISFileInfo.Exists && !Directory.Exists(GISFileInfo.DirectoryName))
                    throw new IOException(string.Format("File `{0}` could not be created because the directory `{1}`is not present", GISFileInfo, GISFileInfo.DirectoryName));
                else if (!GISFileInfo.Exists)
                    Create();
            }
            else
            {
                if (!GISFileInfo.Exists)
                    throw new IOException(string.Format("File not found: `{0}`", GISFileInfo, Enum.GetName(typeof(Access), permission)));
            }
            
            GdalConfiguration.ConfigureGdal();
            if (GISFileInfo.Exists)
            {
                _ds = Gdal.Open(GISFileInfo.FullName, permission);
                if (_ds == null)
                    throw new ArgumentException("Can't open " + GISFileInfo);
            }
            else
                throw new FileNotFoundException("Could not find dataset to open", GISFileInfo.FullName);
        }

        /// <summary>
        /// Just a quick check to see if two rasters are equivalent in terms of meta data
        /// </summary>
        /// <param name="Raster2"></param>
        /// <returns></returns>
        public bool IsMetaSame(Raster Raster2)
        {
            Raster me = this;
            bool bIsSame = true;

            try { ValidateSameMeta(me, Raster2); }
            catch (ArgumentException) { bIsSame = false; }

            return bIsSame;
        }

        /// <summary>
        ///  We're just going to scream if any of our inputs are in the wrong format
        /// </summary>
        /// <param name="rR1"></param>
        /// <param name="rR2"></param>
        public static void ValidateSameMeta(Raster rR1, Raster rR2)
        {
            if (rR1.Extent.CellHeight != rR2.Extent.CellHeight)
                throw new ArgumentException("Cellheights do not match");
            if (rR1.Extent.CellWidth != rR2.Extent.CellWidth)
                throw new ArgumentException("Cellwidths do not match");
            if (!rR1.IsDivisible() || !rR2.Extent.IsDivisible())
                throw new ArgumentException("Both raster extents must be divisible");
            if (!rR1.Proj.IsSame(rR2.Proj))
                throw new ArgumentException("Raster Projections do not match match");
            if (rR1.VerticalUnits != rR2.VerticalUnits)
                throw new ArgumentException(string.Format("Both rasters must have the same vertical units: `{0}` vs. `{1}`", rR1.VerticalUnits, rR2.VerticalUnits));
        }

        /// <summary>
        /// Here's our destructor. It's Pretty simple.
        /// </summary>
        ~Raster() {
            Dispose(false);
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
                RefreshFileInfo();
            }
        }

        public new void Dispose() { Dispose(true); }

        /// <summary>
        /// Dispose of this temporary raster
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // Make sure we've freed up all the handles on the file
            UnloadDS();

            // Don't clean up this file unless it's marked temporary
            if (!_temporary) return;

            // Don't call the destructor if we're explicitly disposing of this.
            if (disposing)
                GC.SuppressFinalize(this);

            // Make sure to refresh and check
            GISFileInfo.Refresh();
            if (GISFileInfo.Exists)
            {
                try { Delete(); }
                catch(Exception e) {
                    Debug.WriteLine(e.ToString());
                } // best effort only
                GISFileInfo.Refresh();
            }
        }

        /// <summary>
        /// Turn a string into a driver enumeration
        /// </summary>
        /// <param name="drvstring"></param>
        /// <returns></returns>
        private RasterDriver _str2driver(string drvstring)
        {
            RasterDriver val;
            try
            {
                val = (RasterDriver)Enum.Parse(typeof(RasterDriver), drvstring);
            }
            catch (ArgumentException)
            {
                throw new NotSupportedException("Driver not supported: " + drvstring);
            }
            return val;
        }

        /// <summary>
        /// Really simple copy functionality
        /// </summary>
        /// <param name="destPath"></param>
        public override void Copy(FileInfo destPath)
        {
            Open();
            _ds.GetDriver().CopyFiles(destPath.FullName, GISFileInfo.FullName);
            UnloadDS();
            RefreshFileInfo();
            destPath.Refresh();
        }

        /// <summary>
        /// Deletion
        /// </summary>
        /// <param name="sFilepath"></param>
        public static void Delete(FileInfo sFilepath)
        {
            Raster toDelete = new Raster(sFilepath);
            toDelete.Delete();
            toDelete.RefreshFileInfo();
        }

        /// <summary>
        /// Delete a Raster
        /// </summary>
        public override void Delete()
        {
            Open();
            Driver drv = _ds.GetDriver();
            UnloadDS();
            drv.Delete(GISFileInfo.FullName);
            drv.Dispose();
        }

        /// <summary>
        /// Pass in a list of rasters to extend the extent of this raster
        /// </summary>
        /// <param name="rasters"></param>
        /// <returns></returns>
        public static ExtentRectangle RasterExtentExpand(List<Raster> rasters)
        {
            ExtentRectangle newExtent = rasters[0].Extent;

            foreach (Raster raster in rasters)
                newExtent.Union(raster.Extent);

            return newExtent;
        }

        /// <summary>
        /// Compute the statistics of this raster (if we haven't already)
        /// </summary>
        public virtual void ComputeStatistics(bool bForce = false)
        {
            if (bForce || !_bComputedStatistics)
            {
                try
                {
                    Open(true);
                    double min, max, mean, std;
                    _ds.GetRasterBand(1).ComputeStatistics(false, out min, out max, out mean, out std, null, null);
                }
                catch { }
                _bComputedStatistics = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method">"nearest", "gauss", "cubic", "average", "mode", "average_magphase" or "none" </param>
        public void BuildPyramids(string method)
        {
            // ArcGIS does not use Pyramids when the raster has less than 1024 rows or columns
            // http://desktop.arcgis.com/en/arcmap/10.3/tools/data-management-toolbox/build-pyramids.htm
            if (Extent.Rows < 1024 || Extent.Cols < 1024)
                return;

            Open();
            int iPixelNum = Extent.Rows * Extent.Rows;
            int iTopNum = 4096;
            int iCurNum = iPixelNum / 4;

            int[] anLevels = new int[1024];
            int nLevelCount = 0;

            do
            {
                anLevels[nLevelCount] = Convert.ToInt32(Math.Pow(2.0, nLevelCount + 2));
                nLevelCount++;
                iCurNum /= 4;
            } while (iCurNum > iTopNum);

            int[] levels = new int[nLevelCount];

            for (int a = 0; a < nLevelCount; a++)
                levels[a] = anLevels[a];

            if (_ds.BuildOverviews(method, levels, null, null) == (int)CPLErr.CE_Failure)
                throw new InvalidDataException("Pyramids could not be built for this dataset");

            UnloadDS();
        }


        /// <summary>
        /// Get Statistics. Does not calculate them first.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, decimal> GetStatistics()
        {
            Open();
            double dMin, dMax, dMean, dStdDev;
            _ds.GetRasterBand(1).GetStatistics(0, 1, out dMin, out dMax, out dMean, out dStdDev);
            Dictionary<string, decimal> output = new Dictionary<string, decimal>()  {
                {"min", (decimal)dMin},
                {"max", (decimal)dMax},
                {"mean", (decimal)dMean},
                {"stddev", (decimal)dStdDev}
            };
            return output;
        }

        /// <summary>
        /// Checking Data Types to make sure they are compatible
        /// </summary>
        /// <param name="dType"></param>
        private void checkType(GdalDataType dType)
        {
            // We need to make two conversions to make sure this is the right thing
            if (!dType.Equivalent(Datatype))
            {
                throw new InvalidDataException(string.Format("You are trying operate on a '{0}' raster using '{1}'. This is not allowed.", Datatype.TypeName, dType.TypeName));
            }
        }

        /// <summary>
        /// Just some helper convenience methods:
        /// </summary>
        public bool IsConcurrent(Raster otherRaster)
        {
            return Extent.IsConcurrent(otherRaster.Extent);
        }
        public bool IsDivisible()
        {
            return Extent.IsDivisible();
        }
        public bool IsOrthogonal(Raster otherRaster)
        {
            return Extent.IsOrthogonal(otherRaster.Extent);
        }
        public int VerticalPrecision
        {
            get { return 0; }
        }
        public int HorizontalPrecision
        {
            get { return 0; }
        }

        /**
         * 
         *   EVERYTHING BELOW HERE USES GENERICS. PROCEED WITH CAUTION
         * 
         */

        public T NodataValue<T>()
        {
            T retval;
            if (origNodataVal != null)
                try
                {
                    retval = (T)Convert.ChangeType(origNodataVal, typeof(T));
                }
                catch (Exception)
                {
                    retval = Utility.Conversion.minValue<T>();
                }
            else
            {
                retval = Utility.Conversion.minValue<T>();
            }
            return retval;
        }

        /// <summary>
        /// GDaL Write operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <param name="buffer"></param>
        public virtual void Write<T>(int xOff, int yOff, int xSize, int ySize, T[] buffer)
        {
            Open(true);
            if (typeof(T) == typeof(float))
                _ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as float[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(double))
                _ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as double[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(int))
                _ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(byte))
                _ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
            else
                throw new Exception("Unsupported type");
        }

        /// <summary>
        /// GDaL Read Operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <param name="buffer"></param>
        public virtual void Read<T>(int xOff, int yOff, int xSize, int ySize, T[] buffer)
        {
            Open();
            if (typeof(T) == typeof(float))
                _ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as float[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(double))
                _ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as double[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(int))
                _ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(byte))
                _ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
            else
                throw new Exception("Unsupported type");
        }
    }

}


