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
        double? origNodataVal { get; }
        GdalDataType Datatype { get; }
        void CreateDS(Raster.RasterDriver driver, string filepath, ExtentRectangle theExtent, Projection proj, GdalDataType theType);

        bool IsOpen { get; }
        void OpenDS(string filepath, bool write = false);
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
        Type internalType;
        public Dataset ds { get { return ds; } private set { ds = value; } }
        public GdalDataType Datatype { get; private set; }

        public double? origNodataVal { get; private set; }
        public bool HasNodata { get { return origNodataVal == null; } }
        public bool IsOpen { get { return ds != null; } }

        public void OpenDS(string filepath, bool write = false)
        {
            // Check if we're already open
            if (IsOpen) return;

            Access permission = Access.GA_ReadOnly;
            if (write) permission = Access.GA_Update;

            // Make sure we have permission to do what we want to do
            if (Utility.FileHelpers.IsFileLocked(filepath, permission))
                throw new IOException(String.Format("File `{0}` was locked for `{}` operation", filepath, Enum.GetName(typeof(Access), permission)));

            GdalConfiguration.ConfigureGdal();
            if (File.Exists(filepath))
            {
                ds = Gdal.Open(filepath, permission);
                if (ds == null)
                    throw new ArgumentException("Can't open " + filepath);
            }
            else
                throw new FileNotFoundException("Could not find dataset to open", filepath);

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

        public RasterInternals(double? nodataval, GdalDataType rType)
        {
            origNodataVal = nodataval;
            internalType = typeof(T);
            Datatype = rType;
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
            if (internalType == typeof(double))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as double[], xSize, ySize, 0, 0);
            else if (internalType == typeof(Single))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as Single[], xSize, ySize, 0, 0);
            else if (internalType == typeof(int))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (internalType == typeof(byte))
                ds.GetRasterBand(1).ReadRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
        }

        public void Write(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            if (internalType == typeof(double))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as double[], xSize, ySize, 0, 0);
            else if (internalType == typeof(Single))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as Single[], xSize, ySize, 0, 0);
            else if (internalType == typeof(int))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as int[], xSize, ySize, 0, 0);
            else if (internalType == typeof(byte))
                ds.GetRasterBand(1).WriteRaster(xOff, yOff, xSize, ySize, buffer as byte[], xSize, ySize, 0, 0);
        }


        /// <summary>
        /// Return an appropriate min value
        /// </summary>
        /// <returns></returns>
        private T minValue()
        {
            T val;
            if (internalType == typeof(int))
                val = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(int.MinValue);
            else if (internalType == typeof(double))
                // NOTE: SINGLE VALUE HERE IS NOT A TYPO. ------------------v
                val = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(Single.MinValue);
            else if (internalType == typeof(Single))
                val = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(Single.MinValue);
            else if (internalType == typeof(byte))
                val = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(byte.MinValue);
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
