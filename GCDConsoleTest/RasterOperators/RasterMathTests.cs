using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.RasterOperators;
using System;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.RasterOperators.Tests
{
    [TestClass()]
    public class RasterMathTests
    {
        [TestMethod()]
        public void AddTest()
        {
            Raster Raster1 = new FakeRaster<int>(new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            Raster Raster2 = new FakeRaster<int>(new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            Raster rOutput = new FakeRaster<int>(new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });


            Raster Result = RasterOperators.RasterMath.Add(ref Raster1, 4, ref rOutput);

            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest1()
        {
            Assert.Fail();
        }
    }
}