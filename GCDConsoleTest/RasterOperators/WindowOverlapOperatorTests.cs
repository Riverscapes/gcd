using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Internal.Tests
{
    class TestWOOOp<T> : WindowOverlapOperator<T>
    {
        public TestWOOOp(List<Raster> rRasters, int buff, Raster rOutput) : base(rRasters, buff, rOutput)
        {
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }
        public TestWOOOp(List<Raster> rRasters, Raster rOutput, ExtentRectangle newExtent, int buff) : base(rRasters, buff, rOutput)
        {
            SetOpExtent(newExtent);
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }
        protected override T WindowOp(List<T[]> windowData)
        {
            // We just return the middle cell for testing
            return windowData[0][BufferCenterID];
        }

    }


    [TestClass()]
    public class WindowOverlapOperatorTests
    {
        [TestMethod()]
        public void WindowOverlapOperatorTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] {
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 }
            });

            Raster rOutput1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            TestWOOOp<int> theTest1 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 1, rOutput1);
            theTest1.RunWithOutput();
            CollectionAssert.AreEqual(Raster1._inputgrid, ((FakeRaster<int>)rOutput1)._outputGrid);

            Raster rOutput2 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            TestWOOOp<int> theTest2 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 2,  rOutput2);
            theTest2.RunWithOutput();
            CollectionAssert.AreEqual(Raster1._inputgrid, ((FakeRaster<int>)rOutput2)._outputGrid);

            Raster rOutput3 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            TestWOOOp<int> theTest3 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 3, rOutput3);
            theTest3.RunWithOutput();
            CollectionAssert.AreEqual(Raster1._inputgrid, ((FakeRaster<int>)rOutput3)._outputGrid);
        }
    }
}