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
            Raster rTemplateRaster = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("Slopey980-950.tif")));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsTrue(rTemplateRaster.Datatype.Equals(FakeRaster<float>.floatType));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            rTemplateRaster = null;
        }

        [TestMethod()]
        public void BasicRasterDSCopyTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                Raster rTemplaetRaster = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("SinWave950-980.tif")));
                rTemplaetRaster.Copy(new FileInfo(Path.Combine(tmp.Name, "CopyRasterTest.tif")));

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
                FileInfo sSourceRater = new FileInfo(TestHelpers.GetTestRasterPath("const990.tif"));
                FileInfo sDeletePath = new FileInfo(Path.Combine(tmp.Name, "DeleteRasterTest.tif"));

                Raster rRaster = new Raster(sSourceRater);
                rRaster.Copy(sDeletePath);
                // Make sure our setup worked                
                Assert.IsTrue(sDeletePath.Exists);

                Raster rDeleteRaster = new Raster(sDeletePath);
                rDeleteRaster.Delete();

                // Make sure we're good.
                sDeletePath.Refresh();
                Assert.IsFalse(sDeletePath.Exists);
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
        public void WriteTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void BuildPyramidsTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "PyramidTest.tif")));

                rTemplateOutput.BuildPyramids("average");
                Assert.Fail();
            }
        }
    }
}