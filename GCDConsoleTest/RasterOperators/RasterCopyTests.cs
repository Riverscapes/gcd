using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.RasterOperators;
using System;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.RasterOperators.Tests
{
    [TestClass()]
    public class RasterCopyTests
    {
        [TestMethod()]
        public void ExtendedCopyTest()
        {
            Raster Raster1 = new FakeRaster<int>(new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            Raster rOutput = new FakeRaster<int>(new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            Raster Result = RasterOperators.RasterCopy.ExtendedCopy(ref Raster1, ref rOutput, Raster1.Extent, Raster1.Proj, Raster1.VerticalUnits);
            Assert.Fail();
        }

    }
}