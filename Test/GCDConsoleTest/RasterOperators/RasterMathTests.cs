using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.RasterOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Raster Result = RasterOperators.RasterMath.Add(ref Raster1, 2, @"c:\FAKEPATH.tif");

            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest1()
        {
            Assert.Fail();
        }
    }
}