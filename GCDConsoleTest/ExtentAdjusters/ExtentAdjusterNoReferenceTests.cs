using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            ExtentRectangle dummySrc1 = new ExtentRectangle(0, 0, -1m, 1m, 1, 1);
            ExtentAdjusterNoReference t1 = new ExtentAdjusterNoReference(dummySrc1);

            Assert.AreEqual(t1.Precision, 0);

            Assert.AreEqual(t1.SrcExtent.CellWidth, 1m);
            Assert.AreEqual(t1.SrcExtent.CellHeight, -1m);
            Assert.AreEqual(t1.SrcExtent.Top, 0);
            Assert.AreEqual(t1.SrcExtent.Left, 0);
            Assert.AreEqual(t1.SrcExtent.Rows, 1);
            Assert.AreEqual(t1.SrcExtent.Cols, 1);

            Assert.AreEqual(t1.OutExtent.CellWidth, 1m);
            Assert.AreEqual(t1.OutExtent.CellHeight, -1m);
            Assert.AreEqual(t1.OutExtent.Top, 0);
            Assert.AreEqual(t1.OutExtent.Left, 0);
            Assert.AreEqual(t1.OutExtent.Rows, 1);
            Assert.AreEqual(t1.OutExtent.Cols, 1);

            Assert.IsTrue(t1.OutExtent.IsDivisible());

            {
                // Irregular first DEM extent should yield a nice pretty divisible extent
                decimal cellWidth = 1.510000m;
                ExtentRectangle dummySrc2 = new ExtentRectangle(17.345m, 19.12345m, -cellWidth, cellWidth, 2, 2);
                ExtentAdjusterNoReference t2 = new ExtentAdjusterNoReference(dummySrc2);
                Assert.AreEqual(t2.Precision, 2);
                Assert.IsFalse(t2.SrcExtent.IsDivisible());
                Assert.AreEqual(t2.SrcExtent.CellWidth, 1.51m);
                Assert.IsTrue(t2.OutExtent.IsDivisible());
            }

            {
                // GCD user reported raster issue 2017/01/15 by P
                ExtentRectangle user1_src = new ExtentRectangle(114.58069247m, 122.938517551m, -0.003m, 0.003m, 4410, 11487);
                ExtentAdjusterNoReference user1_out = new ExtentAdjusterNoReference(user1_src);
                Assert.AreEqual(user1_out.Precision, 3m);
                Assert.IsFalse(user1_out.SrcExtent.IsDivisible());
                Assert.AreEqual(user1_out.OutExtent.Top, 114.582m);
                Assert.AreEqual(user1_out.OutExtent.Bottom, 101.349m);
                Assert.AreEqual(user1_out.OutExtent.Left, 122.937m);
                Assert.AreEqual(user1_out.OutExtent.Right, 157.401m);
                Assert.AreEqual(user1_out.OutExtent.Rows, 4411);
                Assert.AreEqual(user1_out.OutExtent.Cols, 11488);
                Assert.IsTrue(user1_out.OutExtent.Width >= user1_src.Width);
                Assert.IsTrue(user1_out.OutExtent.Height >= user1_src.Height);
                Assert.IsTrue(user1_out.OutExtent.Rows >= user1_src.Rows);
                Assert.IsTrue(user1_out.OutExtent.Cols >= user1_src.Cols);
                Assert.IsTrue(user1_out.OutExtent.IsDivisible());
            }

            {
                // GCD user reported raster issue 2017/11/15 by AT
                ExtentRectangle user2_src = new ExtentRectangle(5629639.30575m, 558111.051499m, -1m, 1m, 47, 46);
                ExtentAdjusterNoReference user2_out = new ExtentAdjusterNoReference(user2_src);
                Assert.AreEqual(user2_out.Precision, 0m);
                Assert.AreEqual(user2_out.OutExtent.CellWidth, 1m);
                Assert.IsFalse(user2_out.SrcExtent.IsDivisible());
                Assert.IsTrue(user2_out.OutExtent.IsDivisible());
            }

            {
                // GCD user reported raster issue 2018/09/04 by M
                ExtentRectangle user3_src = new ExtentRectangle(7111409.27802m, 445073.27318m, -0.05m, 0.05m, 11800, 10160);
                ExtentAdjusterNoReference user3_out = new ExtentAdjusterNoReference(user3_src);
                Assert.AreEqual(user3_out.Precision, 2m);
                Assert.AreEqual(user3_out.OutExtent.CellWidth, 0.05m);
                Assert.IsFalse(user3_out.SrcExtent.IsDivisible());
                Assert.IsTrue(user3_out.OutExtent.IsDivisible());
            }

            {
                // Sulphur Creek Sanity Check
                ExtentRectangle user4_src = new ExtentRectangle(592252.5m, 1958980.5m, -0.5m, 0.5m, 583, 413);
                ExtentAdjusterNoReference user4_out = new ExtentAdjusterNoReference(user4_src);
                Assert.AreEqual(user4_out.Precision, 1m);
                Assert.AreEqual(user4_out.OutExtent.Top, 592252.5m);
                Assert.AreEqual(user4_out.OutExtent.Bottom, 591961m);
                Assert.AreEqual(user4_out.OutExtent.Left, 1958980.5m);
                Assert.AreEqual(user4_out.OutExtent.Right, 1959187m);
                Assert.AreEqual(user4_out.OutExtent.Rows, 583);
                Assert.AreEqual(user4_out.OutExtent.Cols, 413);
                Assert.IsTrue(user4_out.OutExtent.Width >= user4_src.Width);
                Assert.IsTrue(user4_out.OutExtent.Height >= user4_src.Height);
                Assert.IsTrue(user4_out.OutExtent.Rows >= user4_src.Rows);
                Assert.IsTrue(user4_out.OutExtent.Cols >= user4_src.Cols);
                Assert.IsTrue(user4_out.OutExtent.IsDivisible());
            }
        }

        [TestMethod()]
        public void AdjustDimensions()
        {
            // Moving the top coordinate by two cells. Note other coordinates should not move.
            decimal cellWidth = 1.510000m;
            ExtentRectangle dummySrc2 = new ExtentRectangle(1600.345m, 19.12345m, -cellWidth, cellWidth, 2, 2);

            {
                ExtentAdjusterNoReference t2 = new ExtentAdjusterNoReference(dummySrc2);
                ExtentAdjusterBase t3 = t2.AdjustDimensions(t2.OutExtent.Top + (2m * 1.51m), t2.OutExtent.Right, t2.OutExtent.Bottom, t2.OutExtent.Left);
                Assert.IsTrue(t3.OutExtent.IsDivisible());
                Assert.AreEqual(t3.OutExtent.Top, 1603.62m); // 1600.61 + 3.02
                Assert.AreEqual(t3.OutExtent.Right, 22.65m); // unchanged
                Assert.AreEqual(t3.OutExtent.Bottom, 1596.07m); // unchanged
                Assert.AreEqual(t3.OutExtent.Left, 18.12m); // unchanged
            }

            {
                // Add irregular value to top and ensure output is divisible
                ExtentAdjusterNoReference t3 = new ExtentAdjusterNoReference(dummySrc2);
                Assert.IsTrue(t3.OutExtent.IsDivisible());
                ExtentAdjusterBase t4 = t3.AdjustDimensions(t3.OutExtent.Top + 0.0003456m, t3.OutExtent.Right, t3.OutExtent.Bottom, t3.OutExtent.Left);
                Assert.IsTrue(t4.OutExtent.IsDivisible());
            }

            {
                // Add irregular value to all dimensions
                ExtentAdjusterNoReference t3 = new ExtentAdjusterNoReference(dummySrc2);
                Assert.IsTrue(t3.OutExtent.IsDivisible());
                ExtentAdjusterBase t4 = t3.AdjustDimensions(t3.OutExtent.Top + 0.0003456m, t3.OutExtent.Right + 1.513334566m, t3.OutExtent.Bottom - 12.321m, t3.OutExtent.Left - 19.11111m);
                Assert.IsTrue(t4.OutExtent.IsDivisible());
            }
        }

        [TestMethod()]
        public void AdjustCellSize()
        {
            // Moving the top coordinate by two cells. Note other coordinates should not move.
            decimal cellWidth = 1.510000m;
            ExtentRectangle dummySrc2 = new ExtentRectangle(1600.345m, 19.12345m, -cellWidth, cellWidth, 2, 2);

            {
                ExtentAdjusterNoReference t2 = new ExtentAdjusterNoReference(dummySrc2);
                ExtentAdjusterBase t3 = t2.AdjustCellSize(2m);
                Assert.IsTrue(t3.OutExtent.IsDivisible());
                Assert.AreEqual(t3.OutExtent.Top, 1602m);
                Assert.AreEqual(t3.OutExtent.Right, 26m);
                Assert.AreEqual(t3.OutExtent.Bottom, 1594m);
                Assert.AreEqual(t3.OutExtent.Left, 18m);
            }
        }

        [TestMethod()]
        public void AdjustPrecision()
        {
            // Moving the top coordinate by two cells. Note other coordinates should not move.
            decimal cellWidth = 1.510000m;
            ExtentRectangle dummySrc2 = new ExtentRectangle(1600.345m, 19.12345m, -cellWidth, cellWidth, 2, 2);

            // Reduce precision to 1
            ExtentAdjusterNoReference t2 = new ExtentAdjusterNoReference(dummySrc2);
            ExtentAdjusterBase t3 = t2.AdjustPrecision(1);
            Assert.IsTrue(t3.OutExtent.IsDivisible());
            Assert.AreEqual(t3.OutExtent.CellWidth, 1.5m);
            Assert.AreEqual(t3.Precision, 1);

            // Now reduce it again to zero
            ExtentAdjusterBase t4 = ((ExtentAdjusterNoReference)t3).AdjustPrecision(0);
            Assert.IsTrue(t4.OutExtent.IsDivisible());
            Assert.AreEqual(t4.OutExtent.CellWidth, 2m);
            Assert.AreEqual(t4.Precision, 0);
        }
    }
}