using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using GCDConsoleLib.GCD;
using UnitsNet.Units;
using GCDConsoleTest.Helpers;
using GCDConsoleLib.ExtentAdjusters;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class ExtentAdjusterWithReferenceTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void ExtentAdjusterWithReference()
        {
            // reference extent
            ExtentRectangle dummyRef = new ExtentRectangle(0, 0, -1m, 1m, 1, 1);

            // second raster with different cell size and other properties
            ExtentRectangle dummySrc = new ExtentRectangle(1, 1, -2m, 2m, 100, 100);

            ExtentAdjusterWithReference ear = new ExtentAdjusterWithReference(dummySrc, dummyRef);

            Assert.IsTrue(ear.RefExtent == dummyRef, "Reference extent should be unchanged");

            // Output extent should match source but cell resolution should match reference
            Assert.AreEqual(ear.OutExtent.CellWidth, dummyRef.CellWidth);
            Assert.AreEqual(ear.OutExtent.CellHeight, dummyRef.CellHeight);
            Assert.AreEqual(ear.OutExtent.Left, dummySrc.Left);
            Assert.AreEqual(ear.OutExtent.Top, dummySrc.Top);
            Assert.AreEqual(ear.OutExtent.Right, dummySrc.Right);
            Assert.AreEqual(ear.OutExtent.Bottom, dummySrc.Bottom);
        }
    }
}