using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using GCDConsoleTest.Helpers;
using OSGeo.OGR;

namespace GCDConsoleLib.Internal.Operators.FuncTests
{
    [TestClass()]
    public class PointDensityTests
    {
        class PointDensityTesterClass : PointDensity
        {
            //Dummy constructor
            public PointDensityTesterClass(Raster rDEM, Vector vPointCloud, Raster OutputRaster, RasterOperators.KernelShapes eKernel, decimal fSize)
            : base(rDEM, vPointCloud, OutputRaster, eKernel, fSize)
            {  }

        }

        /// <summary>
        /// HElper Method
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Geometry pointMaker(double x, double y)
        {
            Geometry pt = new Geometry(wkbGeometryType.wkbPoint);
            pt.AddPoint(x, y, 0);
            return pt;
        }


        [TestMethod()]
        [TestCategory("Functional")]
        public void PointDensityTest()
        {
            Raster rDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R02_DEM_Meters_2004_05.img")));
            Vector rPtDensity = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster rDEM2 = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\SulpherCreek\2006Feb_DEM.img")));
            Vector rPtDensity2 = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\SulpherCreek\feb06_all_points.shp")));

            using (ITempDir tmp = TempDir.Create())
            {
                Raster circleOut = new Raster(rDEM, new FileInfo(Path.Combine(tmp.Name, "GCPointDensityCircleTest.tif")));
                PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Circle, 4.0m);
                circletest.RunWithOutput();

                Raster sqOut = new Raster(rDEM, new FileInfo(Path.Combine(tmp.Name, "GCPointDensitySquareTest.tif")));
                PointDensity squaretest = new PointDensity(rDEM, rPtDensity, sqOut, RasterOperators.KernelShapes.Square, 4.0m);
                squaretest.RunWithOutput();

                Raster circleOut2 = new Raster(rDEM2, new FileInfo(Path.Combine(tmp.Name, "SulpherPointDensityCircleTest2.tif")));
                PointDensity circletest2 = new PointDensity(rDEM2, rPtDensity2, circleOut2, RasterOperators.KernelShapes.Circle, 4.0m);
                circletest2.RunWithOutput();

                Raster sqOut2 = new Raster(rDEM2, new FileInfo(Path.Combine(tmp.Name, "SulpherPointDensitySquareTest2.tif")));
                PointDensity squaretest2 = new PointDensity(rDEM2, rPtDensity2, sqOut2, RasterOperators.KernelShapes.Square, 4.0m);
                squaretest2.RunWithOutput();

            }

        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void InsideRadiusTest()
        {
            Raster rDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster circleOut = new FakeRaster<double>(rDEM);
            PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Circle, 5.0m);

            Geometry origin = pointMaker(0, 0);
            Assert.IsTrue(circletest.InsideRadius(origin, origin));
            Assert.IsTrue(circletest.InsideRadius(origin, pointMaker(0, 2)));
            Assert.IsTrue(circletest.InsideRadius(origin, pointMaker(2, 0)));
            Assert.IsTrue(circletest.InsideRadius(origin, pointMaker(1, 1)));

            Assert.IsFalse(circletest.InsideRadius(origin, pointMaker(0, 5.1)));
            Assert.IsFalse(circletest.InsideRadius(origin, pointMaker(2, 6.0)));
            Assert.IsFalse(circletest.InsideRadius(origin, pointMaker(5, 5)));
            Assert.IsFalse(circletest.InsideRadius(origin, pointMaker(5.1, 5.1)));

        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void InsideSquareTest()
        {
            Raster rDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster circleOut = new FakeRaster<double>(rDEM);
            PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Square, 5.0m);

            Geometry origin = pointMaker(0, 0);
            Assert.IsTrue(circletest.InsideSquare(origin, origin));
            Assert.IsTrue(circletest.InsideSquare(origin, pointMaker(0, 2)));
            Assert.IsTrue(circletest.InsideSquare(origin, pointMaker(2, 0)));

            Assert.IsTrue(circletest.InsideSquare(origin, pointMaker(1, 1)));
            Assert.IsTrue(circletest.InsideSquare(origin, pointMaker(5, 5)));

            Assert.IsFalse(circletest.InsideSquare(origin, pointMaker(0, 5.1)));
            Assert.IsFalse(circletest.InsideSquare(origin, pointMaker(2, 6.0)));

            Assert.IsFalse(circletest.InsideSquare(origin, pointMaker(5.1, 5.1)));

        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void BinPointsTest()
        {
            Raster rDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster circleOut = new FakeRaster<double>(rDEM);
            PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Square, 5.0m);

            List<Geometry> pts = new List<Geometry>() {
                pointMaker((double)circletest.ChunkExtent.Left, (double)circletest.ChunkExtent.Top),
                pointMaker((double)circletest.ChunkExtent.Left + 5, (double)circletest.ChunkExtent.Top),
                pointMaker((double)circletest.ChunkExtent.Left + 10, (double)circletest.ChunkExtent.Top),

                pointMaker((double)circletest.ChunkExtent.Left, (double)circletest.ChunkExtent.Top - 5),
                pointMaker((double)circletest.ChunkExtent.Left, (double)circletest.ChunkExtent.Top - 10),
            };

            List<Geometry>[,] bins = circletest.BinPoints(pts);

            // Now make sure we still have our 5 points in the right boxes
            Assert.AreEqual(bins[1, 1].Count, 1);

            Assert.AreEqual(bins[1, 2].Count, 1);
            Assert.AreEqual(bins[1, 3].Count, 1);

            Assert.AreEqual(bins[2, 1].Count, 1);
            Assert.AreEqual(bins[3, 1].Count, 1);
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void TranslateCIDToBinIDTest()
        {
            Raster rDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster circleOut = new FakeRaster<double>(rDEM);
            PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Square, 5.0m);

            int[] topleft = circletest.TranslateCIDToBinID(pointMaker((double)circletest.ChunkExtent.Left, (double)circletest.ChunkExtent.Top));
            CollectionAssert.AreEqual(topleft, new int[2] { 1, 1 });

            int[] topleft1 = circletest.TranslateCIDToBinID(pointMaker((double)circletest.ChunkExtent.Left+5.0, (double)circletest.ChunkExtent.Top));
            CollectionAssert.AreEqual(topleft1, new int[2] { 2, 1 });

            int[] topright = circletest.TranslateCIDToBinID(pointMaker((double)circletest.ChunkExtent.Right, (double)circletest.ChunkExtent.Top));
            CollectionAssert.AreEqual(topright, new int[2] { 120, 1 });

            int[] bottomleft = circletest.TranslateCIDToBinID(pointMaker((double)circletest.ChunkExtent.Left, (double)circletest.ChunkExtent.Bottom));
            CollectionAssert.AreEqual(bottomleft, new int[2] { 1, 11 });

            int[] bottomright = circletest.TranslateCIDToBinID(pointMaker((double)circletest.ChunkExtent.Right, (double)circletest.ChunkExtent.Bottom));
            CollectionAssert.AreEqual(bottomright, new int[2] { 120, 11 });
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void GetRelevantBinsTest()
        {
            Raster rDEM = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\RASTER1m.tif")));
            Vector rPtDensity = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"PointDensity\GrandCanyon\R2_HybridData_2004_05.shp")));

            Raster circleOut = new FakeRaster<double>(rDEM);
            PointDensity circletest = new PointDensity(rDEM, rPtDensity, circleOut, RasterOperators.KernelShapes.Square, 5.0m);

            List<int[]> topleft = circletest.GetRelevantBins(pointMaker((double)circletest.ChunkExtent.Left, (double)circletest.ChunkExtent.Top));
            List<int[]> expectedTopLeft = new List<int[]> {
                new int[2] { 0,0}, new int[2] { 0,1}, new int[2] { 0,2},
                new int[2] { 1,0}, new int[2] { 1,1}, new int[2] { 1,2},
                new int[2] { 2,0}, new int[2] { 2,1}, new int[2] { 2,2},
            };
            for(int i = 0; i < topleft.Count; i++)
                CollectionAssert.AreEqual(topleft[i], expectedTopLeft[i]);

            List<int[]> bottomright = circletest.GetRelevantBins(pointMaker((double)circletest.ChunkExtent.Right, (double)circletest.ChunkExtent.Bottom));
            List<int[]> expectedBottomRight = new List<int[]> {
                new int[2] { 119,10}, new int[2] { 119,11}, new int[2] { 119,12},
                new int[2] { 120,10}, new int[2] { 120,11}, new int[2] { 120,12},
                new int[2] { 121,10}, new int[2] { 121,11}, new int[2] { 121,12},
            };
            for (int i = 0; i < bottomright.Count; i++)
                CollectionAssert.AreEqual(bottomright[i], expectedBottomRight[i]);
        }
    }
}