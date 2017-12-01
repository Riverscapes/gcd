using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.Internal.Operators.Tests
{
    [TestClass()]
    public class PointDensityTests
    {
        class PointDensityTesterClass : PointDensity
        {
            //Dummy constructor
            public PointDensityTesterClass(Vector vPointCloud, Raster OutputRaster, RasterOperators.KernelShapes eKernel, decimal fSize)
            : base(vPointCloud, OutputRaster, eKernel, fSize)
            {

            }

        }

        [TestMethod()]
        public void PointDensityTest()
        {
            Assert.Inconclusive();
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