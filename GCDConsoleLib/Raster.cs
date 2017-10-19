using System;
using System.IO;
using System.Collections.Generic;
using OSGeo.GDAL;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib
{
    public class Raster : GISDataset
    {
        public enum RasterDriver : byte { GTiff, HFA }
        public RasterDriver driver;
        public Dataset dataset;
        public LengthUnit VerticalUnits;

        public bool IsOpen { get { return dataset != null; } }

        // This is the GDAL enumeration of DataType
        public GdalDataType Datatype { get; private set; }
        public ExtentRectangle Extent;
        private double? _nodataval;
        public double NodataVal
        {
            get
            {
                // WARNING: SOMETIMES WE NEED A NODATAVAL NOT SURE THIS IS THE BEST WAY TO DO IT
                double retval = -9999.0;
                if (_nodataval != null) retval = (double)_nodataval;
                return retval;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rTemplate"></param>
        /// <param name="sFilename"></param>
        /// <param name="leaveopen"></param>
        public Raster(ref Raster rTemplate, string sFilename, bool leaveopen = false) : base(sFilename)
        {
            Extent = new ExtentRectangle(ref rTemplate.Extent);
            _Init(RasterDriver.GTiff, null, "", rTemplate.Datatype, rTemplate._nodataval);
            Create(leaveopen);
        }

        public Raster(ref Raster rTemplate, ExtentRectangle rExtent, string sFilename, bool leaveopen = false) : base(sFilename)
        {
            Extent = new ExtentRectangle(ref rExtent);
            _Init(rTemplate.driver, rTemplate.VerticalUnits, rTemplate.Proj, rTemplate.Datatype, rTemplate._nodataval);
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
            Extent = new ExtentRectangle(fTop, fLeft, dCellHeight, dCellWidth, nRows, nCols);
            _Init(psDriver, psUnit, psProjection, eDataType, dNoData);
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
            Band rBand1 = dataset.GetRasterBand(1);
            string proj = dataset.GetProjection();

            int hasndval;
            double nodatval;
            rBand1.GetNoDataValue(out nodatval, out hasndval);
            string sDriver = dataset.GetDriver().ShortName;
            Extent = new ExtentRectangle(ref dataset);
            GdalDataType theDataType = new GdalDataType(rBand1.DataType);

            _Init(_str2driver(sDriver), rBand1.GetUnitType(), proj, theDataType, _nodataval);
            // Remember to clean things up afterwards
            Dispose();
        }

        public void ComputeStatistics()
        {
            Open();
            double min, max, mean, std;
            dataset.GetRasterBand(1).ComputeStatistics(false, out min, out max, out mean, out std, null, null);
        }

        /// <summary>
        /// Initialization function
        /// </summary>
        /// <param name="rdDriver"></param>
        /// <param name="sUnits"></param>
        /// <param name="sProjection"></param>
        /// <param name="dType"></param>
        /// <param name="dNodata"></param>
        protected void _Init(RasterDriver rdDriver, string sUnits, string sProjection, GdalDataType dType, double? dNodata)
        {
            Proj = new Projection(sProjection);
            driver = rdDriver;
            if (sUnits != "")
            {
                VerticalUnits = Length.ParseUnit(sUnits);
            }
            Datatype = new GdalDataType(dType);
            _nodataval = dNodata;
        }
        protected void _Init(RasterDriver rdDriver, LengthUnit lUnits, Projection proj, GdalDataType dType, double? dNodata)
        {
            Proj = proj;
            driver = rdDriver;
            VerticalUnits = lUnits;
            Datatype = new GdalDataType(dType);
            _nodataval = dNodata;
        }

        /// <summary>
        /// Create a raster.
        /// </summary>
        /// <param name="leaveopen"></param>
        public override void Create(bool leaveopen = false)
        {
            if (File.Exists(FilePath))
                Gdal.Unlink(FilePath);

            List<string> creationOpts = new List<string>();

            switch (driver)
            {
                case RasterDriver.GTiff:
                    creationOpts.Add("COMPRESS=LZW");
                    break;
                case RasterDriver.HFA:
                    creationOpts.Add("COMPRESS=PACKBITS");
                    break;
            }

            Driver driverobj = Gdal.GetDriverByName(Enum.GetName(typeof(RasterDriver), driver));

            dataset = driverobj.Create(FilePath, Extent.cols, Extent.rows, 1, Datatype._origType, creationOpts.ToArray());
            dataset.SetGeoTransform(Extent.Transform);
            dataset.SetProjection(Proj.OriginalString);
            Band band = dataset.GetRasterBand(1);
            if (_nodataval.HasValue)
            {
                band.SetNoDataValue((double)_nodataval);
            }
            if (!leaveopen)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Opening a dataset is generic so we do it here.
        /// </summary>
        public override void Open(bool write = false)
        {
            // Check if we're already open
            if (IsOpen) return;

            Access permission = Access.GA_ReadOnly;
            if (write) permission = Access.GA_Update;

            // Make sure we have permission to do what we want to do
            if (Utility.FileHelpers.IsFileLocked(FilePath, permission))
                throw new IOException(String.Format("File `{0}` was locked for `{}` operation", FilePath, Enum.GetName(typeof(Access), permission)));

            GdalConfiguration.ConfigureGdal();
            if (File.Exists(FilePath))
            {
                dataset = Gdal.Open(FilePath, permission);
                if (dataset == null)
                {
                    throw new ArgumentException("Can't open " + FilePath);
                }
            }
            else
            {
                throw new FileNotFoundException("Could not find dataset to open", FilePath);
            }
        }

        /// <summary>
        /// Object hygiene is super important with GDAL. 
        /// </summary>
        public override void Dispose()
        {
            if (dataset != null)
            {
                dataset.Dispose();
                dataset = null;
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
            dataset.GetDriver().CopyFiles(destPath, FilePath);
            //dataset.GetDriver().CreateCopy(destPath, dataset, 1, null, null, null);
            Dispose();
        }

        public override void Delete()
        {
            Open();
            Driver drv = dataset.GetDriver();
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
        /// We wrap the read and write so we can do better type-ing
        /// </summary>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public void Read(int xOff, int yOff, int xSize, int ySize, ref Byte[] buffer)
        {
            dataset.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }
        public void Read(int xOff, int yOff, int xSize, int ySize, ref Int32[] buffer)
        {
            dataset.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }
        public void Read(int xOff, int yOff, int xSize, int ySize, ref Single[] buffer)
        {
            dataset.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }
        public void Read(int xOff, int yOff, int xSize, int ySize, ref Double[] buffer)
        {
            dataset.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }

        /// <summary>
        /// Writing is the same as reading (basically
        /// </summary>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <param name="buffer"></param>
        public void Write(int xOff, int yOff, int xSize, int ySize, ref Byte[] buffer)
        {
            dataset.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }
        public void Write(int xOff, int yOff, int xSize, int ySize, ref Int32[] buffer)
        {
            dataset.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }
        public void Write(int xOff, int yOff, int xSize, int ySize, ref Single[] buffer)
        {
            dataset.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
        }
        public void Write(int xOff, int yOff, int xSize, int ySize, ref Double[] buffer)
        {
            dataset.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer, xSize, ySize, 0, 0);
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


