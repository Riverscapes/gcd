using System;
using OSGeo.GDAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Common.Extensons;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib.Tests.Utility
{
    public class FakeRaster<U> : Raster
    {
        public static string fakeproj = "GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIU[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]]";
        public static string fakeunit = "m";
        public static GdalDataType floatType = new GdalDataType(DataType.GDT_Float32);

        public U[,] _inputgrid;
        public U[,] _outputGrid;
        public U FakeNodataVal;

        public FakeRaster() : base(0, 0, -0.1m, 0.1m, 100, 100, Conversion.MinValueDouble<U>(), 
            Raster.RasterDriver.GTiff, floatType, fakeproj, fakeunit)
        {
            _inputgrid = new U[100, 100];
            _outputGrid = new U[100, 100];

            _outputGrid.Fill<U>(NodataValue<U>());
        }

        public FakeRaster(U[,] grid) : base(0, 0, -0.1m, 0.1m, grid.GetLength(1), grid.GetLength(0),
           Conversion.MinValueDouble<U>(), Raster.RasterDriver.GTiff, floatType, fakeproj, fakeunit)
        {
            _inputgrid = grid;
            _outputGrid = new U[grid.GetLength(0), grid.GetLength(1)];
            _outputGrid.Fill<U>(NodataValue<U>());
        }

        // This special case is for taking the outputs of a real raster into a fake one.
        public FakeRaster(ref Raster rRef) : base(ref rRef)
        {
            _inputgrid = new U[rRef.Extent.rows, rRef.Extent.cols];
            _outputGrid = new U[rRef.Extent.rows, rRef.Extent.cols];
        }

        public FakeRaster(int Top, int Left, decimal cellHeight, decimal cellWidth, U[,] grid) : 
            base(0, 0, -0.1m, 0.1m, grid.GetLength(1), grid.GetLength(0),
             Conversion.MinValueDouble<U>(), Raster.RasterDriver.GTiff, floatType, fakeproj, fakeunit)
        {
            _inputgrid = grid;
            Extent = new ExtentRectangle(Top, Left, cellHeight, cellWidth, grid.GetLength(0), grid.GetLength(1));
            _outputGrid = new U[grid.GetLength(0), grid.GetLength(1)];
            _outputGrid.Fill<U>(NodataValue<U>());
        }

        public FakeRaster(decimal fTop, decimal fLeft, decimal dCellHeight, decimal dCellWidth, int rows, int cols, 
            GdalDataType eDataType) : base(fTop, fLeft, dCellHeight, dCellWidth, rows, cols, Single.MinValue,
                RasterDriver.GTiff, eDataType, "My Fake Projection", "m")
        {
            _inputgrid = new U[cols, rows];
            _outputGrid = new U[cols, rows];
            _inputgrid.Fill<U>(NodataValue<U>());
            _outputGrid.Fill<U>(NodataValue<U>());
        }

        public FakeRaster(decimal fTop, decimal fLeft, decimal dCellHeight, decimal dCellWidth, double fNoData,
               Raster.RasterDriver psDriver, GdalDataType eDataType, string psProjection, string psUnit,
               U[,] grid) : base(fTop, fLeft, dCellHeight, dCellWidth, grid.GetLength(1), grid.GetLength(0),
                   fNoData, psDriver, eDataType, psProjection, psUnit)
        {
            _inputgrid = grid;
            _outputGrid = new U[grid.GetLength(0), grid.GetLength(1)];
            _outputGrid.Fill<U>(NodataValue<U>());
        }

        /// <summary>
        /// Fakey Fake fake. do nothing
        /// </summary>
        public override void Create(bool leavopen = false) { return; }

        /// <summary>
        /// Bypass file writing and fake a purely in-memory interface
        /// </summary>
        public override void Read<T>(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            for (int r0 = 0; r0 < ySize; r0++)
            {
                for (int r1 = 0; r1 < xSize; r1++)
                {
                    buffer[r1 + (r0 * xSize)] = (T)Convert.ChangeType(_inputgrid[r0 + yOff, r1 + xOff], typeof(T));
                }
            }
        }

        /// <summary>
        /// Bypass file writing and fake a purely in-memory interface
        /// </summary>
        public override void Write<T>(int xOff, int yOff, int xSize, int ySize, ref T[] buffer)
        {
            for (int r0 = 0; r0 < ySize; r0++)
            {
                for (int r1 = 0; r1 < xSize; r1++)
                {
                    _outputGrid[r0 + yOff,r1 + xOff] = (U)Convert.ChangeType(buffer[r1 + (r0 * xSize)], typeof(U));
                }
            }
        }

        public override void ComputeStatistics()
        {

        }

        /// <summary>
        /// Opening a dataset is generic so we do it here.
        /// </summary>
        public override void Open(bool write = false)
        {
            // It's always open here
        }
    }

    [TestClass()]
    public class FakeRasterTest
    {

        [TestMethod()]
        public void InitTest()
        {
            int[,] intArr = new int[5, 4] {
                { -1, 0, 1, 2 },
                { 3, 4, 5, 6},
                { 7, 8, 9, 10},
                { 11, 12, 13, 14},
                { 15, 16, 17, 18} };

            double[,] dblArr = new double[5, 4] {
                { -1.1, 0.0, 1.0, 2.0 },
                { 3, 4, 5, 6 },
                { 7, 8, 9, 10 },
                { 11, 12, 13, 14 },
                { 15, 16, 17, 18 } };

            Single[,] singlArr = new Single[5, 4] {
                { -1.1f, 0.0f, 1.0f, 2.0f },
                { 3, 4, 5, 6 },
                { 7, 8, 9, 10 },
                { 11, 12, 13, 14 },
                { 15, 16, 17, 18 } };

            // Basic Initialization
            FakeRaster<double> frInit1 = new FakeRaster<double>();
            Assert.AreEqual(frInit1.Extent.rows, 100);
            Assert.AreEqual(frInit1.Extent.cols, 100);
            Assert.AreEqual(frInit1.Extent.CellHeight, -0.1m);
            Assert.AreEqual(frInit1.Extent.CellWidth, 0.1m);
            Assert.AreEqual(frInit1.Extent.Top, 0);
            Assert.AreEqual(frInit1.Extent.Bottom, -10);
            Assert.AreEqual(frInit1.Extent.Left, 0);
            Assert.AreEqual(frInit1.Extent.Right, 10);
            Assert.AreEqual(frInit1.NodataValue<Single>(), Single.MinValue);
            Assert.AreEqual(frInit1.driver, Raster.RasterDriver.GTiff);
            Assert.AreEqual(frInit1.Proj.OriginalString, FakeRaster<double>.fakeproj);
            Assert.AreEqual(UnitsNet.Length.GetAbbreviation(frInit1.VerticalUnits), FakeRaster<double>.fakeunit);

            
            FakeRaster<double> frInit2 = new FakeRaster<double>(dblArr);
            Assert.AreEqual(frInit2.Extent.rows, 4);
            Assert.AreEqual(frInit2.Extent.cols, 5);
            Assert.AreEqual(frInit2.Extent.CellHeight, -0.1m);
            Assert.AreEqual(frInit2.Extent.CellWidth, 0.1m);
            Assert.AreEqual(frInit2.Extent.Top, 0);
            Assert.AreEqual(frInit2.Extent.Bottom, -0.4m);
            Assert.AreEqual(frInit2.Extent.Left, 0);
            Assert.AreEqual(frInit2.Extent.Right, 0.5m);
            Assert.AreEqual(frInit2.NodataValue<Single>(), Single.MinValue);
            Assert.AreEqual(frInit2.driver, Raster.RasterDriver.GTiff);
            Assert.AreEqual(frInit2.Proj.OriginalString, FakeRaster<double>.fakeproj);
            Assert.AreEqual(UnitsNet.Length.GetAbbreviation(frInit2.VerticalUnits), FakeRaster<double>.fakeunit);

            string myFakeProj = "My Fake Proj";
            string myFakeUnit = "ft";
            FakeRaster<Single> frInit3 = new FakeRaster<Single>(10.3m, 11.5m, -0.2m, 0.3m, -999.9, Raster.RasterDriver.HFA, FakeRaster<Single>.floatType, myFakeProj, myFakeUnit, singlArr);
            Assert.AreEqual(frInit3.Extent.rows, 4);
            Assert.AreEqual(frInit3.Extent.cols, 5);
            Assert.AreEqual(frInit3.Extent.CellHeight, -0.2m);
            Assert.AreEqual(frInit3.Extent.CellWidth, 0.3m);
            Assert.AreEqual(frInit3.Extent.Top, 10.3m);
            Assert.AreEqual(frInit3.Extent.Bottom, 9.5m);
            Assert.AreEqual(frInit3.Extent.Left, 11.5m);
            Assert.AreEqual(frInit3.Extent.Right, 13);
            Assert.AreEqual(frInit2.NodataValue<Single>(), Single.MinValue);
            Assert.AreEqual(frInit3.driver, Raster.RasterDriver.HFA);
            Assert.AreEqual(frInit3.Proj.OriginalString, myFakeProj);
            Assert.AreEqual(UnitsNet.Length.GetAbbreviation(frInit3.VerticalUnits), myFakeUnit);

            // Differen Types of rasters. Make sure the fills are working properly.
            FakeRaster<int> frInt = new FakeRaster<int>(intArr);
            Assert.IsInstanceOfType(frInt._inputgrid, typeof(int[,]));
            CollectionAssert.AreEqual(frInt._inputgrid, intArr);
            Assert.AreEqual(frInt._outputGrid[0, 0], int.MinValue);

            FakeRaster<double> frDouble = new FakeRaster<double>(dblArr);
            Assert.IsInstanceOfType(frDouble._inputgrid, typeof(Double[,]));
            CollectionAssert.AreEqual(frDouble._inputgrid, dblArr);
            Assert.AreEqual(frDouble._outputGrid[0, 0], Single.MinValue);

            FakeRaster<Single> frSingle = new FakeRaster<Single>(singlArr);
            Assert.IsInstanceOfType(frSingle._inputgrid, typeof(Single[,]));
            CollectionAssert.AreEqual(frSingle._inputgrid, singlArr);
            Assert.AreEqual(frSingle._outputGrid[0, 0], Single.MinValue);

        }
    }
}
