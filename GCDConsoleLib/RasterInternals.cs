using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using OSGeo.GDAL;

// https://stackoverflow.com/questions/1013604/store-generic-data-in-a-non-generic-class
namespace GCDConsoleLib.Internal
{
    /// <summary>
    /// This is an untyped interface
    /// </summary>
    public interface IRasterGuts
    {
        Dataset ds { get; }
        double? origNodataVal { get; set; }
        GdalDataType Datatype { get; }
        void CreateDS(Raster.RasterDriver driver, string filepath, ExtentRectangle theExtent, Projection proj, GdalDataType theType);

        void LoadGuts();
        bool IsOpen { get; }
        void OpenDS(bool write = false);
        void Dispose();
    }

    /// <summary>
    /// This is a typed interface that we use to get the nodataval
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IRasterGuts<TResult>
    {
        TResult NodataVal { get; }
    }

    public class RasterInternals<T> : IRasterGuts, IDisposable
    {
        public Dataset ds { get; private set; }
        public GdalDataType Datatype { get; private set; }
        private string FilePath;

        public double? origNodataVal { get; set; }
        public bool HasNodata { get { return origNodataVal == null; } }
        public bool IsOpen { get { return ds != null; } }

        public RasterInternals(string sFilepath)
        {
            FilePath = sFilepath;
            Datatype = new GdalDataType(typeof(T));
        }

        public RasterInternals(string sFilepath, double? Nodata)
        {
            FilePath = sFilepath;
            origNodataVal = Nodata;
        }

        public void OpenDS(bool write = false)
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
                ds = Gdal.Open(FilePath, permission);
                if (ds == null)
                    throw new ArgumentException("Can't open " + FilePath);
            }
            else
                throw new FileNotFoundException("Could not find dataset to open", FilePath);
        }

        public void LoadGuts()
        {
            OpenDS();
            int hasndval;
            double nodatval;
            Band rBand1 = ds.GetRasterBand(1);
            Datatype = new GdalDataType(rBand1.DataType);
            rBand1.GetNoDataValue(out nodatval, out hasndval);
            origNodataVal = nodatval;
        }

        public void CreateDS(Raster.RasterDriver driver, string filepath, ExtentRectangle theExtent, Projection proj, GdalDataType theType)
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

            ds = driverobj.Create(filepath, theExtent.cols, theExtent.rows, 1, theType._origType, creationOpts.ToArray());
            ds.SetGeoTransform(theExtent.Transform);
            ds.SetProjection(proj.OriginalString);
            Band band = ds.GetRasterBand(1);
            if (HasNodata)
                band.SetNoDataValue((double)origNodataVal);
        }

        public T NodataVal
        {
            get
            {
                T retval;
                if (origNodataVal != null)
                    retval = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(origNodataVal);
                else
                    retval = minValue();
                return retval;
            }
        }


        public void Read(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            if (typeof(T) != Datatype.CSType)
                throw new Exception("Internal Type does not match raster type. Read not possible");

            if (Datatype.CSType == typeof(double))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as double[], xSize, ySize, 0, 0);
            else if (Datatype.CSType == typeof(Single))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as Single[], xSize, ySize, 0, 0);
            else if (Datatype.CSType == typeof(int))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (Datatype.CSType == typeof(byte))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
        }

        public void Write(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            if (typeof(T) != Datatype.CSType)
                throw new Exception("Internal Type does not match raster type. Write not possible");

            if (Datatype.CSType == typeof(double))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as double[], xSize, ySize, 0, 0);
            else if (Datatype.CSType == typeof(Single))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as Single[], xSize, ySize, 0, 0);
            else if (Datatype.CSType == typeof(int))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (Datatype.CSType == typeof(byte))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
        }

        public static T ConvertValue<U>(U value) where U : IConvertible
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Return an appropriate min value
        /// </summary>
        /// <returns></returns>
        private T minValue()
        {
            if (typeof(T) != Datatype.CSType)
                throw new Exception("Internal Type does not match raster type. Minimum type is not possible.");

            T val;
            if (Datatype.CSType == typeof(int))
                val = (T)ConvertValue<int>(int.MinValue);
            else if (Datatype.CSType == typeof(double))
                // NOTE: SINGLE VALUE HERE IS NOT A TYPO.
                val = (T)ConvertValue<Single>(Single.MinValue);
            else if (Datatype.CSType == typeof(Single))
                val = (T)ConvertValue<Single>(Single.MinValue);
            else if (Datatype.CSType == typeof(byte))
                val = (T)ConvertValue<byte>(byte.MinValue);
            else
                throw new NotSupportedException("Type conversion problem");

            return val;
        }

        /// <summary>
        /// Object hygiene is super important with GDAL. 
        /// </summary>
        public void Dispose()
        {
            if (ds != null)
            {
                ds.Dispose();
                ds = null;
            }
        }

    }


}
