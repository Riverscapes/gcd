using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using GCDConsoleLib.Tests.Utility;
using GCDConsoleLib.Common.Extensons;
using System.IO;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class RasterOperatorsTests
    {
        /// <summary>
        /// NOTE: WE ARE ONLY TESTING THE INTERFACE HERE. DO NOT TEST ANY VALUES PRODUCED HERE
        /// </summary>
        [TestMethod()]
        public void ExtendedCopyTest()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "ExtendedCopyRasterTestBuffer.tif")), newExtReal);

                ExtentRectangle newExtReal2 = rTempl.Extent.Buffer(5);
                newExtReal2.rows = (int)newExtReal2.rows / 2;
                newExtReal2.cols = (int)newExtReal2.cols / 3;
                Raster rTemplateOutput2 = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "ExtendedCopyRasterTestSlice.tif")), newExtReal2);
            }

            Raster Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            int[,] outgrid = new int[7, 8];
            outgrid.Fill(-999);
            Raster rOutput = new FakeRaster<int>(10, 20, -1, 1, outgrid);

            ExtentRectangle newExt = Raster1.Extent.Buffer(2);
            RasterOperators.ExtendedCopy(Raster1, rOutput, new ExtentRectangle(newExt));
        }

        [TestMethod()]
        public void MathTest()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));

                Raster rAdd1 = RasterOperators.Add(rTempl, 2.1, new FileInfo(Path.Combine(tmp.Name, "RasterAddOperand.tif")));
                Raster rAdd2 = RasterOperators.Add(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterAddRaster.tif")));

                Raster rSub1 = RasterOperators.Subtract(rTempl, 2.1, new FileInfo(Path.Combine(tmp.Name, "RasterSubtractOperand.tif")));
                Raster rSub2 = RasterOperators.Subtract(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterSubtractRaster.tif")));

                Raster rMult1 = RasterOperators.Subtract(rTempl, 2.1, new FileInfo(Path.Combine(tmp.Name, "RasterMultiplyOperand.tif")));
                Raster rMult2 = RasterOperators.Multiply(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterMultiplyRaster.tif")));

                Raster rDiv1 = RasterOperators.Subtract(rTempl, 2.1, new FileInfo(Path.Combine(tmp.Name, "RasterDivideOperand.tif")));
                Raster rDiv2 = RasterOperators.Divide(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterDivideRaster.tif")));
            }
        }

        [TestMethod()]
        public void ExtendedCopyTest1()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ExtendedCopyTest2()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ExtendedCopyTest3()
        {
            Assert.Inconclusive();
        }


        [TestMethod()]
        public void GetStatsMinLoDTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void GetStatsPropagatedTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void GetStatsProbalisticTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void BilinearResampleTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void HillshadeTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SlopePercentTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SlopeDegreesTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void PointDensityTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void UniformTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void MosaicTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void MaskTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void FISRasterTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SubtractTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void RootSumSquaresTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void BinRasterTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SetNullTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SetNullTest1()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void SetNullTest2()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void CreatePriorProbabilityRasterTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ThresholdDoDProbWithSpatialCoherenceTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ThresholdDoDProbabilityTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void BuildPyramidsTest()
        {
            Assert.Inconclusive();
        }
    }
}