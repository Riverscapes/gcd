using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleTest.Helpers;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Vector vPolyMaskComplex = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\BudgetMasks\DoD_Geomorphic_Interpretation.shp")));
            Vector vPolyMaskSimple = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\MethodMask_ForTesting.shp")));

            // Try with a numeric field
            watch.Restart();
            VectorRaster test12 = new VectorRaster(rTemplate, vPolyMaskComplex, "Category");
            string time1 = string.Format("MyRasterizationTest1, , {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);

            // Try the same shape file with a string field
            watch.Restart();
            VectorRaster test = new VectorRaster(rTemplate, vPolyMaskComplex, "Desc_");
            string time2 = string.Format("MyRasterizationTest1, , {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);

            // Now try a simpler shapefule with a string field
            watch.Restart();
            VectorRaster test3 = new VectorRaster(rTemplate, vPolyMaskSimple, "Method");
            string time3 = string.Format("MyRasterizationTest1, , {0}.{1}", watch.Elapsed.Seconds, watch.Elapsed.Milliseconds);

            Debug.WriteLine(time1);
            Debug.WriteLine(time2);
            Debug.WriteLine(time3);
            Assert.Fail();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void RasterizeTest()
        {
            Raster rTemplate = new Raster(new FileInfo(DirHelpers.GetTestRootPath(@"VerificationProject\inputs\2005DecDEM\2005DecDEM.tif")));
            Vector vPolyMaskComplex = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\BudgetMasks\DoD_Geomorphic_Interpretation.shp")));
            Vector vPolyMaskSimple = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"BudgetSeg\SulphurCreek\MethodMask_ForTesting.shp")));

            using (ITempDir tmp = TempDir.Create())
            {
                VectorRaster.Rasterize(vPolyMaskComplex, new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "Complex_Numerical.tif"))));
                VectorRaster.Rasterize(vPolyMaskComplex, new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "Complex_String.tif"))));
                VectorRaster.Rasterize(vPolyMaskSimple, new Raster(rTemplate, new FileInfo(Path.Combine(tmp.Name, "Simple_String.tif"))));

                Debug.WriteLine(tmp.Name);
                Assert.Fail();
            }

        }
    }
}