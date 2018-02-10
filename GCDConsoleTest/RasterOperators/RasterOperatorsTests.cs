using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Common.Extensons;
using System.IO;
using UnitsNet.Units;
using GCDConsoleLib.GCD;
using System.Collections.Generic;
using GCDConsoleTest.Helpers;
using System.Diagnostics;
using System.Linq;
using System;

namespace GCDConsoleLib.Internal.Operators.Tests
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
        [TestCategory("Functional")]
        public void ExtendedCopyTest()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("Slopey950-980.tif")));
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
        [TestCategory("Functional")]
        public void MathTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const950.tif")));

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
        [TestCategory("Functional")]
        public void GetStatsMinLoDTest()
        {
            Raster rRaw = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rThresh = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, ug);
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void GetStatsMinLoDBudgetSegTest()
        {
            Raster rRaw = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rThresh = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\BudgetMasks\DoD_Geomorphic_Interpretation.shp")));

            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, rPolyMask, "Category", ug);
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void GetStatsPropagatedTest()
        {
            Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            // test the non-budget seg case
            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test1 = RasterOperators.GetStatsPropagated(rTemp2005, rTemp2006, ug);
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void GetStatsPropagatedBudgetSegTest()
        {
            Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\BudgetMasks\DoD_Geomorphic_Interpretation.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsPropagated(rTemp2005, rTemp2006, rPolyMask, "Category", ug);
        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void GetStatsProbalisticTest()
        {
            Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test = RasterOperators.GetStatsProbalistic(rTemp2005, rTemp2006, rTemp2005, ug);

        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void GetStatsProbalisticBudgetSegTest()
        {
            Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\BudgetMasks\DoD_Geomorphic_Interpretation.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsProbalistic(rTemp2005, rTemp2006, rTemp2005, rPolyMask, "Category", ug);

        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void BilinearResampleTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = new ExtentRectangle(rTempl.Extent);
                newExtReal.CellHeight = newExtReal.CellHeight * 2;
                newExtReal.CellWidth = newExtReal.CellWidth * 2;

                Raster rTemplateOutput = RasterOperators.BilinearResample(rTempl, new FileInfo(Path.Combine(tmp.Name, "BilinearResample.tif")), newExtReal);
            }

        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void SlopeHillshadeTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\SulpherCreek\2006Feb_DEM.img")));

                Raster rTemplateOutput1 = RasterOperators.SlopeDegrees(rTempl, new FileInfo(Path.Combine(tmp.Name, "SlopeDegrees.tif")));
                Raster rTemplateOutput2 = RasterOperators.SlopePercent(rTempl, new FileInfo(Path.Combine(tmp.Name, "SlopePercent.tif")));
                Raster rTemplateOutput3 = RasterOperators.Hillshade(rTempl, new FileInfo(Path.Combine(tmp.Name, "Hillshade.tif")));
            }
        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void UniformTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                Raster rTemplateOutput1 = RasterOperators.Uniform<int>(rTempl, new FileInfo(Path.Combine(tmp.Name, "UniformTest.tif")), 7);
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void MosaicTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                List<FileInfo> theList = new List<FileInfo>() {
                    new FileInfo(DirHelpers.GetTestRasterPath("const900.tif")),
                    new FileInfo(DirHelpers.GetTestRasterPath("const950.tif"))
                };
                Raster rTemplateOutput2 = RasterOperators.Mosaic(theList, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void MaskTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const950.tif")));
                Raster rTemplateOutput2 = RasterOperators.Mask(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
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

        [TestMethod()]
        [TestCategory("Functional")]
        public void RootSumSquaresTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const950.tif")));
                Raster rTemplateOutput2 = RasterOperators.RootSumSquares(rTempl, rTemp2, new FileInfo(Path.Combine(tmp.Name, "FISTest.tif")));
            }
        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void LinearExtractorTest()
        {
            Assert.Inconclusive();
            Raster rDetrended = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"ExtractorTest\Detrended.tif")));
            Raster rWSEDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"ExtractorTest\WSEDEM.tif")));

            Vector centerline = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"ExtractorTest\BCenterline.shp")));
            Vector xs = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"ExtractorTest\BCrossSections.shp")));

            using (ITempDir tmp = TempDir.Create())
            {
                FileInfo centerlinecsv = new FileInfo(Path.Combine(tmp.Name, "centerline.csv"));
                FileInfo xscsv = new FileInfo(Path.Combine(tmp.Name, "xs.csv"));

                //FileInfo centerlinecsv = new FileInfo(@"c:\dev\CSV\centerline.csv");
                //FileInfo xscsv = new FileInfo(@"c:\dev\CSV\xs.csv");

                RasterOperators.LinearExtractor(xs, new List<Raster> { rDetrended, rWSEDEM }, xscsv);
                RasterOperators.LinearExtractor(centerline, new List<Raster> { rDetrended, rWSEDEM }, centerlinecsv);

                FileInfo centerlinecsv1m = new FileInfo(Path.Combine(tmp.Name, "centerline1m.csv"));
                FileInfo xscsv1m = new FileInfo(Path.Combine(tmp.Name, "xs1m.csv"));

                RasterOperators.LinearExtractor(xs, new List<Raster> { rDetrended, rWSEDEM }, xscsv1m, 1.0m);
                RasterOperators.LinearExtractor(centerline, new List<Raster> { rDetrended, rWSEDEM }, centerlinecsv1m, 1.0m);

                Assert.Fail();
            }
        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void BinRasterTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "BinRasterTest.tif")), newExtReal);
                Histogram theHisto = RasterOperators.BinRaster(rTemplateOutput, 10);
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void SetNullTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const900.tif")));
                Raster rTemp2 = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const950.tif")));

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
        [TestCategory("Functional")]
        public void SetNullAbsoluteTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
                Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));
                Raster rDoD = RasterOperators.Subtract(rTemp2006, rTemp2005, new FileInfo(Path.Combine(tmp.Name, "rDoD.tif")));

                ErrorRasterProperties props02 = new ErrorRasterProperties(0.2m);
                ErrorRasterProperties props01 = new ErrorRasterProperties(0.1m);
                // 0.1 2006
                // 0.2 2005
                Raster r2005Error = RasterOperators.CreateErrorRaster(rTemp2005, props02, new FileInfo(Path.Combine(tmp.Name, "2005Dec_DEM_CONSTERR02.tif")));
                Raster r2006Error = RasterOperators.CreateErrorRaster(rTemp2006, props01, new FileInfo(Path.Combine(tmp.Name, "2006Feb_DEM_CONSTERR01.tif")));
                Raster propError = RasterOperators.RootSumSquares(r2006Error, r2005Error, new FileInfo(Path.Combine(tmp.Name, "properror.tif")));

                Raster rTemplateOutput5 = RasterOperators.AbsoluteSetNull(rDoD, RasterOperators.ThresholdOps.GreaterThan, propError, new FileInfo(Path.Combine(tmp.Name, "AbsRasterGreaterThan.tif")));
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void PosteriorProbabilityTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
                Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));
                Raster rDoD = RasterOperators.Subtract(rTemp2006, rTemp2005, new FileInfo(Path.Combine(tmp.Name, "rDoD.tif")));

                Raster GCDErosion1 = RasterOperators.NeighbourCount(rDoD, RasterOperators.GCDWindowType.Erosion, 1, new FileInfo(Path.Combine(tmp.Name, "Erosion1.tif")));
                Raster GCDDeposition1 = RasterOperators.NeighbourCount(rDoD, RasterOperators.GCDWindowType.Deposition, 1, new FileInfo(Path.Combine(tmp.Name, "Deposition1.tif")));
                Raster GCDAll1 = RasterOperators.NeighbourCount(rDoD, RasterOperators.GCDWindowType.All, 1, new FileInfo(Path.Combine(tmp.Name, "All1.tif")));

                ErrorRasterProperties props02 = new ErrorRasterProperties(0.2m);
                ErrorRasterProperties props01 = new ErrorRasterProperties(0.1m);
                // min 60% => 5 // max 100% => 9

                // 0.1 2006
                // 0.2 2005
                Raster r2005Error = RasterOperators.CreateErrorRaster(rTemp2005, props02, new FileInfo(Path.Combine(tmp.Name, "2005Dec_DEM_CONSTERR02.tif")));
                Raster r2006Error = RasterOperators.CreateErrorRaster(rTemp2006, props01, new FileInfo(Path.Combine(tmp.Name, "2006Feb_DEM_CONSTERR01.tif")));

                Raster propError = RasterOperators.RootSumSquares(r2006Error, r2005Error, new FileInfo(Path.Combine(tmp.Name, "properror.tif")));
                Raster postProb = RasterOperators.CreatePriorProbabilityRaster(rDoD, propError, new FileInfo(Path.Combine(tmp.Name, "priorprob.tif")));

                Raster PostProb = RasterOperators.PosteriorProbability(rDoD, postProb, GCDErosion1, GCDDeposition1, new FileInfo(Path.Combine(tmp.Name, "postprob.tif")), new FileInfo(Path.Combine(tmp.Name, "cond.tif")), 5, 9);
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void ThresholdProbabilityNoSCTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTemp2005 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
                Raster rTemp2006 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));
                Raster rDoD = RasterOperators.Subtract(rTemp2006, rTemp2005, new FileInfo(Path.Combine(tmp.Name, "rDoD.tif")));

                ErrorRasterProperties props02 = new ErrorRasterProperties(0.2m);
                ErrorRasterProperties props01 = new ErrorRasterProperties(0.1m);
                // min 60% => 5 // max 100% => 9

                // 0.1 2006
                // 0.2 2005
                Raster r2005Error = RasterOperators.CreateErrorRaster(rTemp2005, props02, new FileInfo(Path.Combine(tmp.Name, "2005Dec_DEM_CONSTERR02.tif")));
                Raster r2006Error = RasterOperators.CreateErrorRaster(rTemp2006, props01, new FileInfo(Path.Combine(tmp.Name, "2006Feb_DEM_CONSTERR01.tif")));

                Raster propError = RasterOperators.RootSumSquares(r2006Error, r2005Error, new FileInfo(Path.Combine(tmp.Name, "properror.tif")));
                Raster postProb = RasterOperators.CreatePriorProbabilityRaster(rDoD, propError, new FileInfo(Path.Combine(tmp.Name, "priorprob.tif")));

                Raster thrDoD = RasterOperators.ThresholdDoDProbability(rDoD, propError, new FileInfo(Path.Combine(tmp.Name, "tDoD.tif")), 0.95m);

            }
        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void BuildPyramidsInterfaceTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "PyramidTest.tif")), newExtReal);
                RasterOperators.BuildPyramids(new FileInfo(Path.Combine(tmp.Name, "PyramidTest.tif")));
            }
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void CreateErrorRasterTest()
        {
            Raster rRaw = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2005Dec_DEM\2005Dec_DEM.img")));
            Raster rThresh = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\2006Feb_DEM\2006Feb_DEM.img")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            DoDStats test = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, ug);

            // And now the budget seg case
            Vector rPolyMask = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\BudgetMasks\DoD_Geomorphic_Interpretation.shp")));
            Dictionary<string, DoDStats> testBudgetSeg = RasterOperators.GetStatsMinLoD(rRaw, rThresh, 73.0m, rPolyMask, "Category", ug);
        }

        /// <summary>
        /// Things we have to test here:
        /// 
        /// -- Does the rasterization work?
        /// -- Do all the functions that use it give good outputs?
        /// -- Do the results of the rasterization method match the inputs?
        /// 
        /// </summary>
        [TestMethod()]
        [TestCategory("Long")]
        public void RasterizedVectorTest()
        {
            Raster rOld = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\2005DecDEM.tif")));
            Raster rNew = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2006FebDEM\2006FebDEM.tif")));

            Raster rOldErr = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\ErrorSurfaces\Constant01\Constant01.tif")));
            Raster rNewErr = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2006FebDEM\ErrorSurfaces\Constant02\Constant02.tif")));

            Raster rDoD = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\Analyses\CD\GCD0001\raw.tif")));
            Raster rThresh = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\Analyses\CD\GCD0001\thresh.tif")));

            // And now the budget seg case
            Vector vPolyMaskSimple = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"SulphurGCDMASK\Sulphur_SimpleGCDMask.shp")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);

            List<string> times = new List<string> { };
            var watch = Stopwatch.StartNew();

            using (ITempDir tmp = TempDir.Create())
            {
                Raster rPropErr = RasterOperators.RootSumSquares(rOldErr, rNewErr, new FileInfo(Path.Combine(tmp.Name, "PropErr.tif")));

                watch.Restart();
                Dictionary<string, Histogram> binRasterV = RasterOperators.BinRaster(rDoD, 100, vPolyMaskSimple, "Method");
                times.Add(string.Format("BinRaster, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, Histogram> binRasterR = RasterOperators.BinRaster(rDoD, 100, vPolyMaskSimple, "Method", true);
                times.Add(string.Format("BinRaster, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                foreach (KeyValuePair<string, Histogram> kvp in binRasterV)
                    Assert.IsTrue(kvp.Value.Equals(binRasterR[kvp.Key]));

                watch.Restart();
                Dictionary<string, DoDStats> statsPropV = RasterOperators.GetStatsPropagated(rDoD, rPropErr, vPolyMaskSimple, "Method", ug);
                times.Add(string.Format("GetStatsPropagated, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> statsPropR = RasterOperators.GetStatsPropagated(rDoD, rPropErr, vPolyMaskSimple, "Method", ug, true);
                times.Add(string.Format("GetStatsPropagated, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                foreach (KeyValuePair<string, DoDStats> kvp in statsPropV)
                    Assert.IsTrue(kvp.Value.Equals(statsPropR[kvp.Key]));

                watch.Restart();
                Dictionary<string, DoDStats> minlodV = RasterOperators.GetStatsMinLoD(rDoD, rThresh, 0.2m, vPolyMaskSimple, "Method", ug);
                times.Add(string.Format("GetStatsMinLoD, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> minlodR = RasterOperators.GetStatsMinLoD(rDoD, rThresh, 0.2m, vPolyMaskSimple, "Method", ug, true);
                times.Add(string.Format("GetStatsMinLoD, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                foreach (KeyValuePair<string, DoDStats> kvp in minlodV)
                    Assert.IsTrue(kvp.Value.Equals(minlodR[kvp.Key]));

                watch.Restart();
                Dictionary<string, DoDStats> statsprobV = RasterOperators.GetStatsProbalistic(rDoD, rThresh, rPropErr, vPolyMaskSimple, "Method", ug);
                times.Add(string.Format("GetStatsProbalistic, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> statsprobR = RasterOperators.GetStatsProbalistic(rDoD, rThresh, rPropErr, vPolyMaskSimple, "Method", ug, true);
                times.Add(string.Format("GetStatsProbalistic, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                foreach (KeyValuePair<string, DoDStats> kvp in statsprobV)
                    Assert.IsTrue(kvp.Value.Equals(statsprobR[kvp.Key]));

                Dictionary<string, ErrorRasterProperties> props = new Dictionary<string, ErrorRasterProperties>
                {
                    {"LiDAR", new ErrorRasterProperties(0.1m) },
                    {"Total Station", new ErrorRasterProperties(0.2m) }
                };

                watch.Restart();
                Raster errorV = RasterOperators.CreateErrorRaster(rDoD, vPolyMaskSimple, "Method", props, new FileInfo(Path.Combine(tmp.Name, "MultiMethodErrorV.tif")));
                times.Add(string.Format("CreateErrorRaster, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Raster errorR = RasterOperators.CreateErrorRaster(rDoD, vPolyMaskSimple, "Method", props, new FileInfo(Path.Combine(tmp.Name, "MultiMethodErrorR.tif")), true);
                times.Add(string.Format("CreateErrorRaster, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                RasterTests.RasterCompare(errorV, errorR);

                foreach (string line in times)
                    Debug.WriteLine(line);
            }
        }

        /// <summary>
        /// Things we have to test here:
        /// 
        /// -- Does the rasterization work?
        /// -- Do all the functions that use it give good outputs?
        /// -- Do the results of the rasterization method match the inputs?
        /// 
        /// </summary>
        [TestMethod()]
        [TestCategory("Long")]
        public void RasterizedVectorComplexTest()
        {
            Raster rOld = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\2005DecDEM.tif")));
            Raster rNew = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2006FebDEM\2006FebDEM.tif")));

            Raster rOldErr = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\ErrorSurfaces\Constant01\Constant01.tif")));
            Raster rNewErr = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2006FebDEM\ErrorSurfaces\Constant02\Constant02.tif")));

            Raster rDoD = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\Analyses\CD\GCD0001\raw.tif")));
            Raster rThresh = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\Analyses\CD\GCD0001\thresh.tif")));

            // And now the budget seg case
            Vector vPolyMask = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"SulphurGCDMASK\Sulphur_ComplexGCDMask.shp")));

            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);

            List<string> times = new List<string> { };
            var watch = Stopwatch.StartNew();

            using (ITempDir tmp = TempDir.Create())
            {
                Raster rPropErr = RasterOperators.RootSumSquares(rOldErr, rNewErr, new FileInfo(Path.Combine(tmp.Name, "PropErr.tif")));

                watch.Restart();
                Dictionary<string, Histogram> binRasterV = RasterOperators.BinRaster(rDoD, 100, vPolyMask, "Desc_");
                times.Add(string.Format("BinRaster, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, Histogram> binRasterR = RasterOperators.BinRaster(rDoD, 100, vPolyMask, "Desc_", true);
                times.Add(string.Format("BinRaster, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> statsPropV = RasterOperators.GetStatsPropagated(rDoD, rPropErr, vPolyMask, "Desc_", ug);
                times.Add(string.Format("GetStatsPropagated, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> statsPropR = RasterOperators.GetStatsPropagated(rDoD, rPropErr, vPolyMask, "Desc_", ug, true);
                times.Add(string.Format("GetStatsPropagated, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> minlodV = RasterOperators.GetStatsMinLoD(rDoD, rThresh, 0.2m, vPolyMask, "Desc_", ug);
                times.Add(string.Format("GetStatsMinLoD, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> minlodR = RasterOperators.GetStatsMinLoD(rDoD, rThresh, 0.2m, vPolyMask, "Desc_", ug, true);
                times.Add(string.Format("GetStatsMinLoD, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> statsprobV = RasterOperators.GetStatsProbalistic(rDoD, rThresh, rPropErr, vPolyMask, "Desc_", ug);
                times.Add(string.Format("GetStatsProbalistic, Vector, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));

                watch.Restart();
                Dictionary<string, DoDStats> statsprobR = RasterOperators.GetStatsProbalistic(rDoD, rThresh, rPropErr, vPolyMask, "Desc_", ug, true);
                times.Add(string.Format("GetStatsProbalistic, Rasterized, {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));


                foreach (string line in times)
                    Debug.WriteLine(line);

            }
        }
    }
}