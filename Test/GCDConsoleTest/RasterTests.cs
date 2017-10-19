using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;

using System.IO;
using System;
using OSGeo.GDAL;
using GCDConsoleLib.Utility;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Tests
{

    [TestClass()]
    public class RasterTests
    {
        [TestMethod()]
        public void RasterInitTest()
        {
            Raster rTemplaetRaster = new Raster(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif"));
            Assert.IsFalse(rTemplaetRaster.IsOpen);
            Assert.IsTrue(rTemplaetRaster.Datatype.Equals(FakeRaster<Single>.floatType));
            Assert.IsNull(rTemplaetRaster.dataset);
        }

        [TestMethod()]
        public void RasterCopyTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                Raster rTemplaetRaster = new Raster(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif"));
                rTemplaetRaster.Copy(Path.Combine(tmp.Name, "CopyRasterTest.tif"));

                // Make sure we're good.
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyRasterTest.tif")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyRasterTest.tif.aux.xml")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyRasterTest.tif.ovr")));
            }
        }


        [TestMethod()]
        public void RasterDeleteTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                string sSourceRater = TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif");
                string sDeletePath = Path.Combine(tmp.Name, "DeleteRasterTest.tif");

                Raster rRaster = new Raster(sSourceRater);
                rRaster.Copy(sDeletePath);
                // Make sure our setup worked                
                Assert.IsTrue(File.Exists(sDeletePath));

                Raster rDeleteRaster = new Raster(sDeletePath);
                rDeleteRaster.Delete();

                // Make sure we're good.
                Assert.IsFalse(File.Exists(sDeletePath));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteRasterTest.tif.aux.xml")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteRasterTest.tif.ovr")));
            }
        }

        [TestMethod()]
        public void RasterExtentExpandTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void RasterUnDelimitTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ReadTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ReadTest1()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ReadTest2()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ReadTest3()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void WriteTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void WriteTest1()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void WriteTest2()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void WriteTest3()
        {
            Assert.Inconclusive();
        }

    }
}