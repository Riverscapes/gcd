using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleTest.Helpers;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class VectorRasterTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void VectorRasterTest()
        {
            List<string> times = new List<string> { };
            var watch = Stopwatch.StartNew();

            Raster rTemplate = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\2005DecDEM.tif")));
            Vector vPolyMaskSimple = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"SulphurGCDMASK\Sulphur_SimpleGCDMask.shp")));
            Vector vPolyMaskComplex = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"SulphurGCDMASK\Sulphur_ComplexGCDMask.shp")));

            // Try with a numeric field
            watch.Restart();
            VectorRaster test1 = new VectorRaster(rTemplate, vPolyMaskComplex, "Category");
            string time1 = string.Format("MyRasterizationTest1, , {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);

            // Try the same shape file with a string field
            watch.Restart();
            VectorRaster test2 = new VectorRaster(rTemplate, vPolyMaskComplex, "Desc_");
            string time2 = string.Format("MyRasterizationTest1, , {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);

            // Now try a simpler shapefule with a string field
            watch.Restart();
            VectorRaster test3 = new VectorRaster(rTemplate, vPolyMaskSimple, "Method");
            string time3 = string.Format("MyRasterizationTest1, , {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);

            Debug.WriteLine("");
            Debug.WriteLine(time1);
            Debug.WriteLine(time2);
            Debug.WriteLine(time3);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void RasterizeTest()
        {
            Raster rTemplate = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\2005DecDEM.tif")));
            Vector vPolyMaskSimple = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"SulphurGCDMASK\Sulphur_SimpleGCDMask.shp")));
            Vector vPolyMaskComplex = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"SulphurGCDMASK\Sulphur_ComplexGCDMask.shp")));

            using (ITempDir tmp = TempDir.Create())
            {
                VectorRaster.Rasterize(vPolyMaskComplex, new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "Complex.tif"))));
                VectorRaster.Rasterize(vPolyMaskSimple, new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "Simple.tif"))));

                Debug.WriteLine(tmp.Name);
            }

        }
    }
}