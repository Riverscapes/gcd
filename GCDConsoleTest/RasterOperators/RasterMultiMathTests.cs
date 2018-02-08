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
    public class RasterMultiMathTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void MultiMath_AdditionTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 2, -1, -1}, // Raster 1
                new double[]{ 1, -2, 3, -2}, // Raster 2
                new double[]{ -3, 2, 3, -3}  // Raster 3
            };
            List<double> inputs1Nodata = new List<double> { -1, -2, -3 };
            Assert.AreEqual(RasterMultiMath.Addition(inputs1, 0, inputs1Nodata, -2.0), 2.0);
            Assert.AreEqual(RasterMultiMath.Addition(inputs1, 1, inputs1Nodata, -2.0), 4.0);
            Assert.AreEqual(RasterMultiMath.Addition(inputs1, 2, inputs1Nodata, -2.0), 6.0);
            Assert.AreEqual(RasterMultiMath.Addition(inputs1, 3, inputs1Nodata, -2.0), -2.0);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void MultiMath_MinimumTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 20, -1, -1}, // Raster 1
                new double[]{ 14, -2, 36, -2}, // Raster 2
                new double[]{ -3, 2, 3, -3}  // Raster 3
            };
            List<double> inputs1Nodata = new List<double> { -1, -2, -3 };
            Assert.AreEqual(RasterMultiMath.Minimum(inputs1, 0, inputs1Nodata, -2.0), 1.0);
            Assert.AreEqual(RasterMultiMath.Minimum(inputs1, 1, inputs1Nodata, -2.0), 2.0);
            Assert.AreEqual(RasterMultiMath.Minimum(inputs1, 2, inputs1Nodata, -2.0), 3.0);
            Assert.AreEqual(RasterMultiMath.Minimum(inputs1, 3, inputs1Nodata, -2.0), -2.0);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void MultiMath_MaximumTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 20, -1, -1}, // Raster 1
                new double[]{ 14, -2, 36, -2}, // Raster 2
                new double[]{ -3, 2, 3, -3}  // Raster 3
            };
            List<double> inputs1Nodata = new List<double> { -1, -2, -3 };
            Assert.AreEqual(RasterMultiMath.Maximum(inputs1, 0, inputs1Nodata, -2.0), 14.0);
            Assert.AreEqual(RasterMultiMath.Maximum(inputs1, 1, inputs1Nodata, -2.0), 20.0);
            Assert.AreEqual(RasterMultiMath.Maximum(inputs1, 2, inputs1Nodata, -2.0), 36.0);
            Assert.AreEqual(RasterMultiMath.Maximum(inputs1, 3, inputs1Nodata, -2.0), -2.0);
        }


        [TestMethod()]
        [TestCategory("Unit")]
        public void MultiMath_MeanTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 20, -1, -1}, // Raster 1
                new double[]{ 14, -2, 36, -2}, // Raster 2
                new double[]{ 3, 2, 3, -3},  // Raster 3
                new double[]{ -4, 2, 3, -4}  // Raster 4
            };
            List<double> inputs1Nodata = new List<double> { -1, -2, -3, -4 };
            Assert.AreEqual(RasterMultiMath.Mean(inputs1, 0, inputs1Nodata, -2.0), 6.0);
            Assert.AreEqual(RasterMultiMath.Mean(inputs1, 1, inputs1Nodata, -2.0), 8.0);
            Assert.AreEqual(RasterMultiMath.Mean(inputs1, 2, inputs1Nodata, -2.0), 14.0);
            Assert.AreEqual(RasterMultiMath.Mean(inputs1, 3, inputs1Nodata, -2.0), -2.0);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void MultiMath_StdDevTest()
        {
            List<double[]> inputs1 = new List<double[]> {
                new double[]{ 1, 4, -1, -1}, // Raster 1
                new double[]{ 14, -2, 40, -2}, // Raster 2
                new double[]{ 3, 3, 40, -3},  // Raster 3
                new double[]{ -4, 5, 40, -4}  // Raster 4
            };
            List<double> inputs1Nodata = new List<double> { -1, -2, -3, -4 };
            Assert.AreEqual(RasterMultiMath.StandardDeviation(inputs1, 0, inputs1Nodata, -2.0), 7.0);
            Assert.AreEqual(RasterMultiMath.StandardDeviation(inputs1, 1, inputs1Nodata, -2.0), 1.0);
            Assert.AreEqual(RasterMultiMath.StandardDeviation(inputs1, 2, inputs1Nodata, -2.0), 0.0);
            Assert.AreEqual(RasterMultiMath.StandardDeviation(inputs1, 3, inputs1Nodata, -2.0), -2.0);
        }
    }
}