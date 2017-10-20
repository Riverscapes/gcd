using System;
using System.IO;
using System.Collections.Generic;
using OSGeo.GDAL;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.Internal;
namespace GCDConsoleLib
{
    public class Raster : GISDataset
    {
        public enum RasterDriver : byte { GTiff, HFA }
        public LengthUnit VerticalUnits;

        public ExtentRectangle Extent;

        public IRasterGuts RasterGuts;
        public RasterDriver driver;
        // We store the following in the guts since they are used there a lot
        public GdalDataType Datatype { get { return RasterGuts.Datatype; } }
        public double? origNodataVal { get { return RasterGuts.origNodataVal; } }
        public bool IsOpen { get { return RasterGuts.IsOpen; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(ref Raster rTemplate, string sFilename, bool leaveopen = false) : base(sFilename)
        {
            ExtentRectangle theExtent = new ExtentRectangle(ref rTemplate.Extent);
            _Init(RasterDriver.GTiff, null, "", rTemplate.Datatype, rTemplate.origNodataVal, theExtent);
            Create(leaveopen);
        }

        public Raster(ref Raster rTemplate, ExtentRectangle rExtent, string sFilename, bool leaveopen = false) : base(sFilename)
        {
            _Init(rTemplate.driver, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, rTemplate.origNodataVal, rExtent);
        }

        /// <summary>
        /// Explicit Constructore
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
               double dNoData, RasterDriver psDriver, GdalDataType eDataType,
               string psProjection, string psUnit) : base("")
        {
            ExtentRectangle theExtent = new ExtentRectangle(fTop, fLeft, dCellHeight, dCellWidth, nRows, nCols);
            _Init(psDriver, psUnit, psProjection, eDataType, dNoData, theExtent);
        }

        /// <summary>
        /// Constructor for opening an existing Raster
        /// </summary>
        /// <param name="sfilepath"></param>
        public Raster(string sfilepath) : base(sfilepath)
        {
            _load();
        }

        /// <summary>
        /// Load the Raster properties from a file
        /// </summary>
        private void _load()
        {
            Open();
            Band rBand1 = RasterGuts.ds.GetRasterBand(1);
            string proj = RasterGuts.ds.GetProjection();

            int hasndval;
            double nodatval;
            rBand1.GetNoDataValue(out nodatval, out hasndval);
            string sDriver = RasterGuts.ds.GetDriver().ShortName;
            ExtentRectangle theExtent = new ExtentRectangle(RasterGuts.ds);
            GdalDataType theDataType = new GdalDataType(rBand1.DataType);

            _Init(_str2driver(sDriver), rBand1.GetUnitType(), proj, theDataType, origNodataVal, theExtent);
            // Remember to clean things up afterwards
            Dispose();
        }

        public void ComputeStatistics()
        {
            Open();
            double min, max, mean, std;
            RasterGuts.ds.GetRasterBand(1).ComputeStatistics(false, out min, out max, out mean, out std, null, null);
        }

        /// <summary>
        /// Initialization function
        /// </summary>
        /// <param name="rdDriver"></param>
        /// <param name="sUnits"></param>
        /// <param name="sProjection"></param>
        /// <param name="dType"></param>
        /// <param name="dNodata"></param>
        protected void _Init(RasterDriver rdDriver, string sUnits, string sProjection,
            GdalDataType dType, double? dNodata, ExtentRectangle theExtent)
        {
            Proj = new Projection(sProjection);
            if (sUnits != "")
            {
                VerticalUnits = Length.ParseUnit(sUnits);
            }
            _Init(rdDriver, VerticalUnits, Proj, dType, dNodata, theExtent);
        }
        protected void _Init(RasterDriver rdDriver, LengthUnit lUnits, Projection proj,
            GdalDataType dType, double? dNodata, ExtentRectangle theExtent)
        {
            Proj = proj;
            VerticalUnits = lUnits;
            driver = rdDriver;
            // This is regrettable but it's the only way to get 
            if (Datatype.CSType == typeof(int)) { RasterGuts = new RasterInternals<int>(dNodata, new GdalDataType(dType)); }
            else if (Datatype.CSType == typeof(double)) { RasterGuts = new RasterInternals<double>(dNodata, new GdalDataType(dType)); }
            else if (Datatype.CSType == typeof(Single)) { RasterGuts = new RasterInternals<Single>(dNodata, new GdalDataType(dType)); }
            else if (Datatype.CSType == typeof(byte)) { RasterGuts = new RasterInternals<byte>(dNodata, new GdalDataType(dType)); }
        }

        /// <summary>
        /// Create a raster.
        /// </summary>
        /// <param name="leaveopen"></param>
        public override void Create(bool leaveopen = false)
        {
            if (File.Exists(FilePath))
                Gdal.Unlink(FilePath);

            RasterGuts.CreateDS(driver, FilePath, Extent, Proj, Datatype);

            if (!leaveopen)
                Dispose();
        }

        /// <summary>
        /// Opening a dataset is generic so we do it here.
        /// </summary>
        public override void Open(bool write = false)
        {
            RasterGuts.OpenDS(FilePath, write);
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
            RasterGuts.Dispose();
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
            RasterGuts.ds.GetDriver().CopyFiles(destPath, FilePath);
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
            Driver drv = RasterGuts.ds.GetDriver();
            Dispose();
            drv.Delete(FilePath);
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

    }

}


