using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.ExtentAdjusters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.ExtentAdjusters.Tests
{
    [TestClass()]
    public class ExtentAdjusterNoReferenceTests
    {
        [TestMethod()]
        public void ExtentAdjusterNoReferenceTest()
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Nice regular first DEM extent
            ExtentRectangle dummySrc1 = new ExtentRectangle(0, 0, 1m, 1m, 1, 1);
            ExtentAdjusterNoReference t1 = new ExtentAdjusterNoReference(dummySrc1);

            Assert.AreEqual(t1.Precision, 0);

            Assert.AreEqual(t1.SrcExtent.CellWidth, 1m);
            Assert.AreEqual(t1.SrcExtent.CellHeight, 1m);
            Assert.AreEqual(t1.SrcExtent.Top, 0);
            Assert.AreEqual(t1.SrcExtent.Left, 0);
            Assert.AreEqual(t1.SrcExtent.Rows, 1);
            Assert.AreEqual(t1.SrcExtent.Cols, 1);

            Assert.AreEqual(t1.OutExtent.CellWidth, 1m);
            Assert.AreEqual(t1.OutExtent.CellHeight, 1m);
            Assert.AreEqual(t1.OutExtent.Top, 0);
            Assert.AreEqual(t1.OutExtent.Left, 0);
            Assert.AreEqual(t1.OutExtent.Rows, 1);
            Assert.AreEqual(t1.OutExtent.Cols, 1);

            Assert.IsTrue(t1.OutExtent.IsDivisible());

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Irregular first DEM extent should yield a nice pretty divisible extent
            decimal cellWidth = 1.510000m;
            ExtentRectangle dummySrc2 = new ExtentRectangle(17.345m, 19.12345m, cellWidth, cellWidth, 2, 2);
            ExtentAdjusterNoReference t2 = new ExtentAdjusterNoReference(dummySrc2);

            Assert.AreEqual(t2.Precision, 2);

            Assert.AreEqual(t2.SrcExtent.CellWidth, 1.51m);
            Assert.AreEqual(t2.SrcExtent.CellHeight, 1.51m);
            Assert.AreEqual(t2.SrcExtent.Top, dummySrc2.Top);
            Assert.AreEqual(t2.SrcExtent.Left, dummySrc2.Left);
            Assert.AreEqual(t2.SrcExtent.Rows, dummySrc2.Rows);
            Assert.AreEqual(t2.SrcExtent.Cols, dummySrc2.Cols);

            Assert.AreEqual(t2.OutExtent.CellWidth, 1.51m);
            Assert.AreEqual(t2.OutExtent.CellHeight, 1.51m);
            Assert.AreEqual(t2.OutExtent.Top, 0);
            Assert.AreEqual(t2.OutExtent.Left, 0);
            Assert.AreEqual(t2.OutExtent.Rows, 2);
            Assert.AreEqual(t2.OutExtent.Cols, 2);

            Assert.IsTrue(t1.OutExtent.IsDivisible());

            Assert.Fail();
        }
    }
}