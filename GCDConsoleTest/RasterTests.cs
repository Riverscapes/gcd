using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
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
        }

        [TestMethod()]
        public void RasterInitLazyTest()
        {
            Raster rTemplateRaster = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("Slopey980-950.tif")));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsFalse(rTemplateRaster.IsLoaded);

            // Now do somethign with the extent
            double nodataval = (double)rTemplateRaster.origNodataVal;
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsTrue(rTemplateRaster.IsLoaded);

            // Reset and try again
            rTemplateRaster = null;

            rTemplateRaster = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("Slopey980-950.tif")));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsFalse(rTemplateRaster.IsLoaded);

            // Now do somethign with the extent
            ExtentRectangle extrect = rTemplateRaster.Extent;
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsTrue(rTemplateRaster.IsLoaded);

            // Reset and try again
            rTemplateRaster = null;

            rTemplateRaster = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("Slopey980-950.tif")));
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsFalse(rTemplateRaster.IsLoaded);

            // Now do somethign with the extent
            Projection proj = rTemplateRaster.Proj;
            Assert.IsFalse(rTemplateRaster.IsOpen);
            Assert.IsTrue(rTemplateRaster.IsLoaded);
        }

        [TestMethod()]
        public void BasicRasterDSCopyTest()
        {
            using (ITempDir tmp = TempDir.Create())
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
            using (ITempDir tmp = TempDir.Create())
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
            // This is kind of too simple to test.
        }


        [TestMethod()]
        public void ReadTest()
        {
            Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
            rTempl.Open();

            float[] fBuff = new float[1];
            rTempl.Read(0, 0, 1, 1, fBuff);
            Assert.AreEqual(fBuff[0], 964.85f);

            double[] dBuff = new double[1];
            rTempl.Read(0, 0, 1, 1, dBuff);
            Assert.AreEqual(dBuff[0], 964.8499755859375);

            int[] iBuff = new int[1];
            rTempl.Read(0, 0, 1, 1, iBuff);
            Assert.AreEqual(iBuff[0], 965);
        }

        [TestMethod()]
        public void WriteTest()
        {
            Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("SquareValley950-980.tif")));
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rOutput = new Raster(rTempl, new FileInfo(Path.Combine(tmp.Name, "ExtendedCopyRasterTestBuffer.tif")));
                rTempl.Dispose();
                rTempl = null;

                rOutput.Open(true);

                rOutput.Write(0, 0, 1, 1, new double[1] { 0.55 });
                rOutput.Write(0, 1, 1, 1, new float[1] { 30.135f });
                rOutput.Write(0, 2, 1, 1, new int[1] { 3 });
                rOutput.Dispose();
                rOutput = null;

                Raster rTest = new Raster(new FileInfo(Path.Combine(tmp.Name, "ExtendedCopyRasterTestBuffer.tif")));

                double[] dBuff = new double[1];
                rTest.Read(0, 0, 1, 1, dBuff);
                Assert.AreEqual(dBuff[0], 0.55, 0.000001);

                float[] fBuff = new float[1];
                rTest.Read(0, 1, 1, 1, fBuff);
                Assert.AreEqual(fBuff[0], 30.135f);

                int[] iBuff = new int[1];
                rTest.Read(0, 2, 1, 1, iBuff);
                Assert.AreEqual(iBuff[0], 3);

                rTest.Dispose();
                rTest = null;
            }
        }

        [TestMethod()]
        public void BuildPyramidsTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("SquareValley950-980.tif")));
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(rTempl, new FileInfo(Path.Combine(tmp.Name, "PyramidTest.tif")));

                rTemplateOutput.BuildPyramids("average");
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "PyramidTest.tif.ovr")));
            }
        }
    }
}