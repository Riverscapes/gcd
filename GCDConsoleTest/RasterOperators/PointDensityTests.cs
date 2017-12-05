using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GCDConsoleLib.Internal.Operators;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class PointDensityTests
    {
        class PointDensityTesterClass : PointDensity
        {
            //Dummy constructor
            public PointDensityTesterClass(Raster rDEM, Vector vPointCloud, Raster OutputRaster, RasterOperators.KernelShapes eKernel, decimal fSize)
            : base(rDEM, vPointCloud, OutputRaster, eKernel, fSize)
            {

            }

        }

        [TestMethod()]
        public void PointDensityTest()
        {
            Raster rDEM = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster rDEM2 = new Raster(new FileInfo(TestHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity2 = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"PointDensity\SulpherCreek\feb06_all_points.shp")));


            using (ITempDir tmp = TempDir.Create())
            {
                Raster circleOut = new Raster(rDEM, new FileInfo(Path.Combine(tmp.Name, "PointDensityCircleTest.tif")));
                PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Circle, 5.0m);
                circletest.RunWithOutput();

                //Raster sqOut = new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "PointDensitySquareTest.tif")));
                //PointDensity squaretest = new PointDensity(rPtDensity, rTemplate, RasterOperators.KernelShapes.Square, 5.0m);
                //squaretest.RunWithOutput();


                //Raster circleOut2 = new Raster(rTemplate2, new FileInfo(Path.Combine(tmp.Name, "PointDensityCircleTest2.tif")));
                //PointDensity circletest2 = new PointDensity(rPtDensity2, circleOut2, RasterOperators.KernelShapes.Circle, 5.0m);
                //circletest2.RunWithOutput();

                //Raster sqOut2 = new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "PointDensitySquareTest2.tif")));
                //PointDensity squaretest2 = new PointDensity(rPtDensity, rTemplate, RasterOperators.KernelShapes.Square, 5.0m);
                //squaretest2.RunWithOutput();


                Assert.Inconclusive();
            }




        }

        [TestMethod()]
        public void InsideRadiusTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void InsideSquareTest()
        {
            Assert.Inconclusive();
        }
    }
}