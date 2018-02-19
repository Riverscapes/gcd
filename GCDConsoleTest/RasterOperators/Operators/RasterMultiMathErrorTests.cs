using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.Internal.Operators.Tests
{
    [TestClass()]
    public class RasterMultiMathErrorTests
    {

        [TestMethod()]
        [TestCategory("Unit")]
        public void MinimumTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 20, -1, -1}, // Raster 1
                new double[]{ 14, -2, 36, -2}, // Raster 2
                new double[]{ -3, 2, 3, -3},  // Raster 3

                new double[]{ 11, 11, 11, 11 }, // Raster 1 ERROR
                new double[]{ 22, 22, 22, 22 }, // Raster 2 ERROR
                new double[]{ 33, 33, 33, 33 }  // Raster 3 ERROR
            };
            List<int> inIds = new List<int> { 0, 1, 2 };
            List<int> errIds = new List<int> { 3, 4, 5 };

            List<double> inputs1Nodata = new List<double> { -1, -2, -3, 0, 0, 0 };
            Assert.AreEqual(RasterMultiMathError.Minimum(inputs1, 0, inputs1Nodata, inIds, errIds, -2.0), 11.0);
            Assert.AreEqual(RasterMultiMathError.Minimum(inputs1, 1, inputs1Nodata, inIds, errIds, -2.0), 33.0);
            Assert.AreEqual(RasterMultiMathError.Minimum(inputs1, 2, inputs1Nodata, inIds, errIds, -2.0), 33.0);
            Assert.AreEqual(RasterMultiMathError.Minimum(inputs1, 3, inputs1Nodata, inIds, errIds, -2.0), -2.0);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void MaximumTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 20, -1, -1}, // Raster 1
                new double[]{ 14, -2, 36, -2}, // Raster 2
                new double[]{ -3, 2, 3, -3},  // Raster 3

                new double[]{ 11, 11, 11, 11 }, // Raster 1 ERROR
                new double[]{ 22, 22, 22, 22 }, // Raster 2 ERROR
                new double[]{ 33, 33, 33, 33 }  // Raster 3 ERROR
            };
            List<int> inIds = new List<int> { 0, 1, 2 };
            List<int> errIds = new List<int> { 3, 4, 5 };


            List<double> inputs1Nodata = new List<double> { -1, -2, -3, 0, 0, 0 };
            Assert.AreEqual(RasterMultiMathError.Maximum(inputs1, 0, inputs1Nodata, inIds, errIds, -2.0), 22.0);
            Assert.AreEqual(RasterMultiMathError.Maximum(inputs1, 1, inputs1Nodata, inIds, errIds, -2.0), 11.0);
            Assert.AreEqual(RasterMultiMathError.Maximum(inputs1, 2, inputs1Nodata, inIds, errIds, -2.0), 22.0);
            Assert.AreEqual(RasterMultiMathError.Maximum(inputs1, 3, inputs1Nodata, inIds, errIds, -2.0), -2.0);

        }
    }
}