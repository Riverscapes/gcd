using System;
using System.IO;
using System.Collections.Generic;
using OSGeo.GDAL;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.Internal;
using System.ComponentModel;

namespace GCDConsoleLib
{
    public class Raster : GISDataset
    {
        public enum RasterDriver : byte { GTiff, HFA }
        public LengthUnit VerticalUnits;
        public bool bComputedStatistics { get; private set; }

        public double? origNodataVal { get; set; }
        public bool HasNodata { get { return origNodataVal != null; } }

        public ExtentRectangle Extent;
        public RasterDriver driver;

        // We store the following in the guts since they are used there a lot
        protected Dataset ds { get; private set; }
        public GdalDataType Datatype { get; private set; }
        public bool IsOpen { get { return ds != null; } }
        private bool _writepermission;


        /// <summary>
        /// Constructor for opening an existing Raster
        /// </summary>
        /// <param name="sfilepath"></param>
        public Raster(string sfilepath) : base(sfilepath)
        {
            _initfromfile();
            // Remember to clean things up afterwards
            Dispose();
        }

        /// <summary>
        /// Constructor for opening an existing Raster
        /// </summary>
        /// <param name="sfilepath"></param>
        public Raster(System.IO.FileInfo filepath) : base(filepath.FullName)
        {
            _initfromfile();
            // Remember to clean things up afterwards
            Dispose();
        }

        /// <summary>
        /// Constructor to create a new raster file
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(ref Raster rTemplate, string sFilename, bool leaveopen = false) : base(sFilename)
        {
            ExtentRectangle theExtent = new ExtentRectangle(ref rTemplate.Extent);
            _Init(RasterDriver.GTiff, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, theExtent, rTemplate.origNodataVal);
        }
        // This is mainly for testing purposes
        public Raster(ref Raster rTemplate) : base("")
        {
            ExtentRectangle theExtent = new ExtentRectangle(ref rTemplate.Extent);
            _Init(RasterDriver.GTiff, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, theExtent, rTemplate.origNodataVal);
        }

        /// <summary>
        /// This is a create constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="rExtent"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(ref Raster rTemplate, ExtentRectangle rExtent, string sFilename, bool leaveopen = false) : base(sFilename)
        {
            _Init(rTemplate.driver, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, rExtent, rTemplate.origNodataVal);
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
               string psProjection, string psUnit, bool leaveopen = false) : base("")
        {
            ExtentRectangle theExtent = new ExtentRectangle(fTop, fLeft, dCellHeight, dCellWidth, nRows, nCols);
            _Init(psDriver, UnitFromString(psUnit), new Projection(psProjection), dType, theExtent, dNoData);
        }


        /// <summary>
        /// Load all relevant settings from a file
        /// </summary>
        private void _initfromfile()
        {
            Open();
            Band rBand1 = ds.GetRasterBand(1);
            GdalDataType dType = new GdalDataType(rBand1.DataType);

            string sDriver = ds.GetDriver().ShortName;
            ExtentRectangle theExtent = new ExtentRectangle(ds);

            _Init(_str2driver(sDriver),
                UnitFromString(rBand1.GetUnitType()),
                new Projection(ds.GetProjection()),
                dType, theExtent, GetNodataVal());
        }

        /// <summary>
        /// Initialization function
        /// </summary>
        private void _Init(RasterDriver rdDriver, LengthUnit lUnits, Projection proj, GdalDataType dType,
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

            ds = driverobj.Create(finfo.FullName, theExtent.cols, theExtent.rows, 1, theType._origType, creationOpts.ToArray());
            ds.SetGeoTransform(theExtent.Transform);
            ds.SetProjection(proj.OriginalString);
            Band band = ds.GetRasterBand(1);
            if (HasNodata)
                band.SetNoDataValue((double)origNodataVal);
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
                retUnit = Length.ParseUnit(sUnit);
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
            ds.GetRasterBand(1).GetNoDataValue(out nodatval, out hasndval);
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
            GISFileInfo.Refresh();
            if (!leaveopen)
                Dispose();
        }

        /// <summary>
        /// Opening a dataset is generic so we do it here.
        /// </summary>
        public override void Open(bool write = false)
        {
            // Check if we're already open and have the correct permissions
            if (IsOpen && _writepermission.Equals(write)) return;

            // Close the file so we can re-open it with the correct permissions
            if (_writepermission == write) Dispose();

            Access permission = Access.GA_ReadOnly;
            if (write) permission = Access.GA_Update;
            _writepermission = write;

            if (write)
            {
                // File exists AND we can overwrite it
                if (GISFileInfo.Exists && Utility.FileHelpers.IsFileLocked(GISFileInfo.FullName, permission))
                    throw new IOException(String.Format("File `{0}` was locked for `{1}` operation", GISFileInfo, Enum.GetName(typeof(Access), permission)));
                // File does not exist but there is no directory to put it in.
                else if (!GISFileInfo.Exists && !Directory.Exists(GISFileInfo.DirectoryName))
                    throw new IOException(String.Format("File `{0}` could not be created because the directory `{1}`is not present", GISFileInfo, GISFileInfo.DirectoryName));
                else
                    Create();
            }
            else
            {
                if (!GISFileInfo.Exists || Utility.FileHelpers.IsFileLocked(GISFileInfo.FullName, permission))
                    throw new IOException(String.Format("File `{0}` was locked for `{1}` operation", GISFileInfo, Enum.GetName(typeof(Access), permission)));
            }
            if (Utility.FileHelpers.IsFileLocked(GISFileInfo.FullName, permission))
                throw new IOException(String.Format("File `{0}` was locked for `{}` operation", GISFileInfo, Enum.GetName(typeof(Access), permission)));

            GdalConfiguration.ConfigureGdal();
            if (GISFileInfo.Exists)
            {
                ds = Gdal.Open(GISFileInfo.FullName, permission);
                if (ds == null)
                    throw new ArgumentException("Can't open " + GISFileInfo);
            }
            else
                throw new FileNotFoundException("Could not find dataset to open", GISFileInfo.FullName);
        }

        /// <summary>
        /// Just a quick check to see if two rasters are equivalent in terms of meta data
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool IsMetaSame(ref Raster Raster2)
        {
            Raster me = this;
            bool bIsSame = true;

            try { ValidateSameMeta(ref me, ref Raster2); }
            catch (ArgumentException) { bIsSame = false; }

            return bIsSame;
        }

        /// <summary>
        /// We're just going to scream if any of our inputs are in the wrong format
        /// </summary>
        public static void ValidateSameMeta(ref Raster rR1, ref Raster rR2)
        {
            if (rR1.Extent.CellHeight != rR2.Extent.CellHeight)
                throw new ArgumentException("Cellheights do not match");
            if (rR1.Extent.CellWidth != rR2.Extent.CellWidth)
                throw new ArgumentException("Cellwidths do not match");
            if (!rR1.IsDivisible() || !rR2.Extent.IsDivisible())
                throw new ArgumentException("Both raster extents must be divisible");
            if (!rR1.Proj.IsSame(ref rR2.Proj))
                throw new ArgumentException("Raster Projections do not match match");
            if (rR1.VerticalUnits != rR2.VerticalUnits)
                throw new ArgumentException(String.Format("Both rasters must have the same vertical units: `{0}` vs. `{1}`", rR1.VerticalUnits, rR2.VerticalUnits));
        }

        /// <summary>
        /// Object hygiene is super important with GDAL. 
        /// </summary>
        public override void Dispose()
        {
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
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
        public override void Copy(string destPath)
        {
            Open();
            ds.GetDriver().CopyFiles(destPath, GISFileInfo.FullName);
            //dataset.GetDriver().CreateCopy(destPath, dataset, 1, null, null, null);
            Dispose();
        }

        /// <summary>
        /// Deletion
        /// </summary>
        /// <param name="sFilepath"></param>
        public static void Delete(string sFilepath)
        {
            Raster toDelete = new Raster(sFilepath);
            toDelete.Delete();
        }

        /// <summary>
        /// Delete a Raster
        /// </summary>
        public override void Delete()
        {
            Open();
            Driver drv = ds.GetDriver();
            Dispose();
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
            {
                newExtent.Union(ref raster.Extent);
            }
            return newExtent;
        }

        /// <summary>
        /// Compute the statistics of this raster (if we haven't already)
        /// </summary>
        public virtual void ComputeStatistics(bool bForce = false)
        {
            if (bForce || !bComputedStatistics)
            {
                Open(true);
                double min, max, mean, std;
                ds.GetRasterBand(1).ComputeStatistics(false, out min, out max, out mean, out std, null, null);
                bComputedStatistics = true;
            }
        }

        /// <summary>
        /// Get Statistics. Does not calculate them first.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, decimal> GetStatistics()
        {
            Open();
            double dMin, dMax, dMean, dStdDev;
            ds.GetRasterBand(1).GetStatistics(0, 1, out dMin, out dMax, out dMean, out dStdDev);
            Dictionary<string, decimal> output = new Dictionary<string, decimal>()  {
                {"min", (decimal)dMin},
                {"max", (decimal)dMax},
                {"mean", (decimal)dMean},
                {"stddev", (decimal)dStdDev}
            };
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRastersString"></param>
        /// <param name="bCheckExist"></param>
        /// <param name="bCheckOrthogonal"></param>
        /// <param name="bCheckConcurrent"></param>
        /// <returns></returns>
        public static List<string> RasterUnDelimit(string sRastersString, bool bCheckExist, bool bCheckOrthogonal, bool bCheckConcurrent)
        {
            throw new NotImplementedException("Coming soon(if I need it)");

            //List<string> sRasters = new List<string>();
            //string[] sRawSplit = sRastersString.Split(';');

            //foreach (string teststring in sRawSplit)
            //{
            //    //if (System.)
            //}
            //return sRasters;
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
                throw new InvalidDataException(String.Format("You are trying operate on a '{0}' raster using '{1}'. This is not allowed.", Datatype.TypeName, dType.TypeName));
            }
        }

        /// <summary>
        /// Just some helper convenience methods:
        /// </summary>
        public bool IsConcurrent(ref Raster otherRaster)
        {
            return Extent.IsConcurrent(ref otherRaster.Extent);
        }
        public bool IsDivisible()
        {
            return Extent.IsDivisible();
        }
        public bool IsOrthogonal(ref Raster otherRaster)
        {
            return Extent.IsOrthogonal(ref otherRaster.Extent);
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
                retval = (T)Convert.ChangeType(origNodataVal, typeof(T));
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
        public virtual void Write<T>(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            Open(true);
            if (typeof(T) == typeof(Single))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as Single[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(Double))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as Double[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(int))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(byte))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
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
        public virtual void Read<T>(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            Open();
            if (typeof(T) == typeof(Single))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as Single[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(Double))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as Double[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(int))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (typeof(T) == typeof(byte))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
            else
                throw new Exception("Unsupported type");
        }
    }

}


