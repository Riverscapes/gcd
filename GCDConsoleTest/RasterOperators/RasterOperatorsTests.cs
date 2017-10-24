using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;
using System.IO;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class RasterOperatorsTests
    {
        [TestMethod()]
        public void ExtendedCopyTest()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTemplateRaster = new Raster(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif"));
                ExtentRectangle newExtReal = rTemplateRaster.Extent.Buffer(5);
                RasterOperators.ExtendedCopy(ref rTemplateRaster, Path.Combine(tmp.Name, "ExtendedCopyRasterTest.tif"), newExtReal);
            }

            Raster Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            Raster rOutput = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            ExtentRectangle newExt = Raster1.Extent.Buffer(2);
            RasterOperators.ExtendedCopy(ref Raster1, ref rOutput, new ExtentRectangle(ref newExt));

            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.Fail();
        }

    }
}