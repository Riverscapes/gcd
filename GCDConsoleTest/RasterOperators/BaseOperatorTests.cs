using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;
using GCDConsoleLib.Utility;
using GCDConsoleLib.Common.Extensons;

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
            public TestOp(List<Raster> rRasters, ref FakeRaster<T> rOutput) : base(rRasters, rOutput)
            {
                Assert.AreEqual(rRasters.Count, _rasters.Count);
                Assert.IsFalse(OpDone);
            }

            protected override void ChunkOp(ref List<T[]> data, ref T[] outChunk)
            {
                for (int idx = 0; idx < data[0].GetLength(0); idx++)
                {
                    T val = (T)Convert.ChangeType(0, typeof(T));
                    for (int idd = 0; idd < data.Count; idd++)
                    {
                        if (val.Equals(OpNodataVal) || data[idd][idx].Equals(OpNodataVal))
                            val = OpNodataVal;
                        else
                            val = Dynamics.Add(val, data[idd][idx]);
                    }
                    outChunk[idx] = val;
                }
            }
            public void TestRun() { Run(); }
        }


        [TestMethod()]
        public void BaseInitTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> Raster2 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 0, 0, 1 }, { 0, 1, 1, 0 }, { 0, 2, 0, 0 } });

            int[,] rExpected = new int[,] { { 2, 2, 3, 5 }, { 5, 7, 8, 8 }, { 9, 12, 11, 12 } };
            FakeRaster<int> rOutput = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1, Raster2 }, ref rOutput);
            theTest.TestRun();
            CollectionAssert.AreEqual(rOutput._outputGrid, rExpected);
        }


        [TestMethod()]
        public void nextChunkTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> rOutput = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1 }, ref rOutput);
            Assert.AreEqual(theTest.ChunkWindow.Top, 10);
            Assert.AreEqual(theTest.ChunkWindow.Left, 20);
            Assert.AreEqual(theTest.ChunkWindow.rows, 3);
            Assert.AreEqual(theTest.ChunkWindow.cols, 4);
            Assert.AreEqual(theTest.ChunkWindow.CellHeight, -1);
            Assert.AreEqual(theTest.ChunkWindow.CellWidth, 1);

            theTest.nextChunk();
            Assert.IsTrue(theTest.OpDone);
        }

        [TestMethod()]
        public void GetChunkTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> rOutput = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1 }, ref rOutput);

            List<int[]> data = new List<int[]> { };
            theTest.GetChunk(ref data);
            CollectionAssert.AreEqual(data[0], Raster1._inputgrid.Make1DArray<int>());
        }
    }
}