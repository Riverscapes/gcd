using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Internal.Tests
{
    class TestWOOOp<T> : WindowOverlapOperator<T>
    {
        public TestWOOOp(List<Raster> rRasters, int buff, Raster rOutput) : base(rRasters, buff, new List<Raster>() { rOutput })
        {
            Assert.AreEqual(rRasters.Count, _inputRasters.Count);
            Assert.IsFalse(OpDone);
            testChunkExtent();
        }
        public TestWOOOp(List<Raster> rRasters, Raster rOutput, ExtentRectangle newExtent, int buff) : base(rRasters, buff, new List<Raster>() { rOutput })
        {
            SetOpExtent(newExtent);
            Assert.AreEqual(rRasters.Count, _inputRasters.Count);
            Assert.IsFalse(OpDone);
        }

        /// <summary>
        /// Let's test that changing the op extent works
        /// </summary>
        /// <param name="buff"></param>
        public void BuffOpExtent(int buff)
        {
            SetOpExtent(OpExtent.Buffer(buff));
            testChunkExtent();
        }

        private void testChunkExtent()
        {
            Assert.AreEqual(ChunkExtent.Left, OpExtent.Left);
            Assert.AreEqual(ChunkExtent.Top, OpExtent.Top);
            Assert.AreEqual(ChunkExtent.Right, OpExtent.Right);
            Assert.AreEqual(ChunkExtent.CellHeight, OpExtent.CellHeight);
            Assert.AreEqual(ChunkExtent.CellWidth, OpExtent.CellWidth);
            Assert.AreEqual(ChunkExtent.Cols, OpExtent.Cols);
            Assert.AreEqual(ChunkExtent.Rows, 1);
        }

        protected override void WindowOp(List<T[]> windowData, List<T[]> outputs, int id)
        {
            // We just return the middle cell for testing
            outputs[0][id] = windowData[0][BufferCenterID];
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
                { 6, 7, 8, -999 },
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 },
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 },
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 }
            });
            int[,] output = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

            Raster rOutput1 = new FakeRaster<int>(0, 0, -1, 1, output);
            TestWOOOp<int> theTest1 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 1, rOutput1);
            theTest1.RunWithOutput();
            CollectionAssert.AreEqual(Raster1._inputgrid, ((FakeRaster<int>)rOutput1)._outputGrid);

            Raster rOutput2 = new FakeRaster<int>(0, 0, -1, 1, output);
            TestWOOOp<int> theTest2 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 2,  rOutput2);
            theTest2.RunWithOutput();
            CollectionAssert.AreEqual(Raster1._inputgrid, ((FakeRaster<int>)rOutput2)._outputGrid);

            Raster rOutput3 = new FakeRaster<int>(0, 0, -1, 1, output);
            TestWOOOp<int> theTest3 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 3, rOutput3);
            theTest3.RunWithOutput();
            CollectionAssert.AreEqual(Raster1._inputgrid, ((FakeRaster<int>)rOutput3)._outputGrid);
        }


        [TestMethod()]
        public void SetOpExtentTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] {
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 },
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 },
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 },
                { 0, 1, 2, -999 },
                { 3, 4, 5, -999 },
                { 6, 7, 8, -999 }
            });
            int[,] output = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

            Raster rOutput3 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            TestWOOOp<int> theTest3 = new TestWOOOp<int>(new List<Raster> { Raster1 }, 3, rOutput3);
            theTest3.BuffOpExtent(5);
        }
    }
}