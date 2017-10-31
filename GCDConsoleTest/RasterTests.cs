using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using OSGeo.GDAL;
using GCDConsoleLib;
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
            Raster rTemplateRaster = new Raster(TestHelpers.GetTestRasterPath("Slopey980-950.tif"));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsTrue(rTemplateRaster.Datatype.Equals(FakeRaster<Single>.floatType));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            rTemplateRaster = null;
        }

        [TestMethod()]
        public void BasicRasterDSCopyTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                Raster rTemplaetRaster = new Raster(TestHelpers.GetTestRasterPath("SinWave950-980.tif"));
                rTemplaetRaster.Copy(Path.Combine(tmp.Name, "CopyRasterTest.tif"));

                // Make sure we're good.
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyRasterTest.tif")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyRasterTest.tif.aux.xml")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyRasterTest.tif.ovr")));
                rTemplaetRaster.Dispose();
                rTemplaetRaster = null;
            }
        }


        [TestMethod()]
        public void RasterDeleteTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                string sSourceRater = TestHelpers.GetTestRasterPath("const990.tif");
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
                rRaster.Dispose();
                rDeleteRaster.Dispose();
                rDeleteRaster = null;
                rRaster = null;
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