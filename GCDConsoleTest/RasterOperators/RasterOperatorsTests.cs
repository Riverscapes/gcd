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
        [TestMethod()]
        public void ExtendedCopyTest()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTempl = new Raster(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif"));
                ExtentRectangle newExtReal = rTempl.Extent.Buffer(15);
                Raster rTemplateOutput = RasterOperators.ExtendedCopy(ref rTempl, Path.Combine(tmp.Name, "ExtendedCopyRasterTestBuffer.tif"), newExtReal);

                ExtentRectangle newExtReal2 = rTempl.Extent.Buffer(5);
                newExtReal2.rows = (int)newExtReal2.rows / 2;
                newExtReal2.cols = (int)newExtReal2.cols / 3;
                Raster rTemplateOutput2 = RasterOperators.ExtendedCopy(ref rTempl, Path.Combine(tmp.Name, "ExtendedCopyRasterTestSlice.tif"), newExtReal2);
            }

            Raster Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            int[,] outgrid = new int[7, 8];
            outgrid.Fill(-999);
            Raster rOutput = new FakeRaster<int>(10, 20, -1, 1, outgrid);

            ExtentRectangle newExt = Raster1.Extent.Buffer(2);
            RasterOperators.ExtendedCopy(ref Raster1, ref rOutput, new ExtentRectangle(ref newExt));
        }

        [TestMethod()]
        public void AddTest()
        {
            Assert.Inconclusive();
        }

    }
}