using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using GCDConsoleLib.Tests.Utility;
using GCDConsoleLib.Common.Extensons;
using System.IO;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.GCD;
using System.Collections.Generic;

namespace GCDConsoleLib.Tests
{
    /// <summary>
    /// NOTE: WE ARE ONLY TESTING THE INTERFACE HERE. 
    /// 
    /// DO NOT TEST ANY VALUES PRODUCED HERE!!!!!!!
    /// </summary>
    [TestClass()]
    public class RasterOperatorsTests
    {
        [TestMethod()]
        public void ExtendedCopyTest()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("Slopey950-980.tif")));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "ExtendedCopyRasterTestBuffer.tif")), newExtReal);

                ExtentRectangle newExtReal2 = rTempl.Extent.Buffer(5);
                newExtReal2.Rows = (int)newExtReal2.Rows / 2;
                newExtReal2.Cols = (int)newExtReal2.Cols / 3;
                Raster rTemplateOutput2 = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "ExtendedCopyRasterTestSlice.tif")), newExtReal2);
            }

            Raster Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            int[,] outgrid = new int[7, 8];
            outgrid.Fill(-999);
            Raster rOutput = new FakeRaster<int>(10, 20, -1, 1, outgrid);

            ExtentRectangle newExt = Raster1.Extent.Buffer(2);
            Internal.Operators.ExtendedCopy<int> copyOp = new Internal.Operators.ExtendedCopy<int>(Raster1, rOutput, new ExtentRectangle(newExt));
            copyOp.RunWithOutput();

        }

        [TestMethod()]
        public void MathTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));

                Raster rAdd1 = RasterOperators.Add(rTempl, 2.1m, new FileInfo(Path.Combine(tmp.Name, "RasterAddOperand.tif")));
                Raster rAdd2 = RasterOperators.Add(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterAddRaster.tif")));

                Raster rSub1 = RasterOperators.Subtract(rTempl, 2.1m, new FileInfo(Path.Combine(tmp.Name, "RasterSubtractOperand.tif")));
                Raster rSub2 = RasterOperators.Subtract(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterSubtractRaster.tif")));

                Raster rMult1 = RasterOperators.Subtract(rTempl, 2.1m, new FileInfo(Path.Combine(tmp.Name, "RasterMultiplyOperand.tif")));
                Raster rMult2 = RasterOperators.Multiply(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterMultiplyRaster.tif")));

                Raster rDiv1 = RasterOperators.Subtract(rTempl, 2.1m, new FileInfo(Path.Combine(tmp.Name, "RasterDivideOperand.tif")));
                Raster rDiv2 = RasterOperators.Divide(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterDivideRaster.tif")));
            }
        }


        [TestMethod()]
        public void GetStatsMinLoDTest()
        {
            Raster rRaw = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rThresh = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, ug);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\MethodMask_ForTesting.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, rPolyMask, "Method", ug);
        }

        [TestMethod()]
        public void GetStatsPropagatedTest()
        {
            Raster rRaw = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
            Raster rThresh = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));
            Raster rErr = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const980.tif")));

            // test the non-budget seg case
            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test1 = RasterOperators.GetStatsPropagated(rRaw, rThresh, rErr, ug);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\MethodMask_ForTesting.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsPropagated(rRaw, rThresh, rThresh, rPolyMask, "Method", ug);

        }


        [TestMethod()]
        public void GetStatsProbalisticTest()
        {
            Raster rRaw = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
            Raster rThresh = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));
            Raster rErr = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const980.tif")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test = RasterOperators.GetStatsProbalistic(rRaw, rThresh, rThresh, ug);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\MethodMask_ForTesting.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsProbalistic(rRaw, rThresh, rThresh, rPolyMask, "Method", ug);

        }

        [TestMethod()]
        public void BilinearResampleTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = new ExtentRectangle(rTempl.Extent);
                newExtReal.CellHeight = newExtReal.CellHeight * 2;
                newExtReal.CellWidth = newExtReal.CellWidth * 2;

                Raster rTemplateOutput = RasterOperators.BilinearResample(rTempl, new FileInfo(Path.Combine(tmp.Name, "BilinearResample.tif")), newExtReal);
            }

        }

        [TestMethod()]
        public void SlopeHillshadeTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"PointDensity\SulpherCreek\2006Feb_DEM.img")));

                Raster rTemplateOutput1 = RasterOperators.SlopeDegrees(rTempl, new FileInfo(Path.Combine(tmp.Name, "SlopeDegrees.tif")));
                Raster rTemplateOutput2 = RasterOperators.SlopePercent(rTempl, new FileInfo(Path.Combine(tmp.Name, "SlopePercent.tif")));
                Raster rTemplateOutput3 = RasterOperators.Hillshade(rTempl, new FileInfo(Path.Combine(tmp.Name, "Hillshade.tif")));
            }
        }


        [TestMethod()]
        public void UniformTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                Raster rTemplateOutput1 = RasterOperators.Uniform<int>(rTempl, new FileInfo(Path.Combine(tmp.Name, "UniformTest.tif")), 7);
            }
        }

        [TestMethod()]
        public void MosaicTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                List<FileInfo> theList = new List<FileInfo>() {
                    new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")),
                    new FileInfo(TestHelpers.GetTestRasterPath("const950.tif"))
                };
                Raster rTemplateOutput2 = RasterOperators.Mosaic(theList, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }

        [TestMethod()]
        public void MaskTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));
                Raster rTemplateOutput2 = RasterOperators.Mask(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }

        [TestMethod()]
        public void FISRasterTest()
        {
            Assert.Inconclusive();
            using (ITempDir tmp = TempDir.Create())
            {
                FileInfo fisFile = new FileInfo(@"C:\code\gcd\extlib\TestData\FIS\FuzzyChinookJuvenile_03.fis");
                Raster reference = new Raster(new FileInfo(@"C:\code\gcd\extlib\TestData\VISIT_3454\Habitat\S0000_1536\Simulations\FIS-ch_jv\PreparedInputs\Depth.tif"));

                Dictionary<string, FileInfo> inputDict = new Dictionary<string, FileInfo>()
                {
                    { "Depth", new FileInfo(@"C:\code\gcd\extlib\TestData\VISIT_3454\Habitat\S0000_1536\Simulations\FIS-ch_jv\PreparedInputs\Depth.tif") },
                    { "Velocity",  new FileInfo(@"C:\code\gcd\extlib\TestData\VISIT_3454\Habitat\S0000_1536\Simulations\FIS-ch_jv\PreparedInputs\Velocity.tif") },
                    { "GrainSize_mm",  new FileInfo(@"C:\code\gcd\extlib\TestData\VISIT_3454\Habitat\S0000_1536\Simulations\FIS-ch_jv\PreparedInputs\GrainSize_mm.tif") }
                };

                Raster rTemplateOutput2 = RasterOperators.FISRaster(inputDict, fisFile, reference, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }


        //[TestMethod()]
        //public void FISERRORSTEST()
        //{
        //    //Assert.Inconclusive();
        //    using (ITempDir tmp = TempDir.Create())
        //    {
        //        //"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\2006FebDEM.tif"
        //        FileInfo fisFile = new FileInfo(@"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\CHaMP_TS_ZError_SLPdeg_PD_2012.fis");
        //        Raster reference = new Raster(new FileInfo(@"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\2006FebDEM.tif"));

        //        Dictionary<string, FileInfo> inputDict = new Dictionary<string, FileInfo>()
        //        {
        //            { "Slope", new FileInfo(@"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\SlopeDegrees5.tif") },
        //            { "PointDensity",  new FileInfo(@"C:\code\gcd\extlib\TestData\ERRORS\FISInputs\PointDensity.tif") }
        //        };

        //        Raster rTemplateOutput2 = RasterOperators.FISRaster(inputDict, fisFile, reference, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
        //        Assert.Fail();
        //    }
        //}



        [TestMethod()]
        public void RootSumSquaresTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));
                Raster rTemplateOutput2 = RasterOperators.RootSumSquares(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }

        [TestMethod()]
        public void BinRasterTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "BinRasterTest.tif")), newExtReal);
                Histogram theHisto = RasterOperators.BinRaster(rTemplateOutput, 10);
            }
        }

        [TestMethod()]
        public void SetNullTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("const950.tif")));

                Raster rTemplateOutput1 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.GreaterThan, 4, new FileInfo(Path.Combine(tmp.Name, "GreaterThan.tif")));
                Raster rTemplateOutput2 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.LessThan, 4, new FileInfo(Path.Combine(tmp.Name, "LessThan.tif")));
                Raster rTemplateOutput3 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.GreaterThanOrEqual, 4, new FileInfo(Path.Combine(tmp.Name, "GreaterThanOrEqual.tif")));
                Raster rTemplateOutput4 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.LessThanOrEqual, 4, new FileInfo(Path.Combine(tmp.Name, "LessThanOrEqual.tif")));

                Raster rTemplateOutput5 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.GreaterThan, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterGreaterThan.tif")));
                Raster rTemplateOutput6 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.LessThan, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterLessThan.tif")));
                Raster rTemplateOutput7 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.GreaterThanOrEqual, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterGreaterThanOrEqual.tif")));
                Raster rTemplateOutput8 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.LessThanOrEqual, rTemp2, new FileInfo(Path.Combine(tmp.Name, "RasterLessThanOrEqual.tif")));


                Raster rTemplateOutput9 = RasterOperators.SetNull(rTempl, RasterOperators.ThresholdOps.GreaterThan, 4, RasterOperators.ThresholdOps.LessThanOrEqual, 10, new FileInfo(Path.Combine(tmp.Name, "DoubleOp.tif")));
            }
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
        public void BuildPyramidsInterfaceTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "PyramidTest.tif")), newExtReal);
                RasterOperators.BuildPyramids(new FileInfo(Path.Combine(tmp.Name, "PyramidTest.tif")));
            }
        }

        [TestMethod()]
        public void CreateErrorRasterTest()
        {
            Raster rRaw = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rThresh = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, ug);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\MethodMask_ForTesting.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, rPolyMask, "Method", ug);
        }

    }
}