using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Internal.Tests
{
    [TestClass()]
    public class BaseOperatorTests
    {
        /// <summary>
        /// In order to test this class we need to have a fake member of it.
        /// </summary>
        [TestClass()]
        class TestOp<T> : BaseOperator<T>
        {
            public TestOp(List<Raster> rRasters, ref Raster rOutput) : base(rRasters, rOutput)
            {
                Assert.AreEqual(rRasters.Count, _rasters.Count);
                Assert.IsFalse(bDone);
            }
            protected override void ChunkOp(ref List<T[]> data, ref T[] outChunk)
            {
                throw new NotImplementedException();
            }
            public void TestRun() { Run(); }
        }


        [TestMethod()]
        public void BaseInitTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> Raster2 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            Raster rOutput = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1, Raster2 }, ref rOutput);
            theTest.TestRun();
        }


        [TestMethod()]
        public void nextChunkTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetChunkTest()
        {
            Assert.Fail();
        }
    }
}