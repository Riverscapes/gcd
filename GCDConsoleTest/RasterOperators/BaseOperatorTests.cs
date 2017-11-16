using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;
using GCDConsoleLib.Utility;
using GCDConsoleLib.Common.Extensons;
using System.IO;
using GCDConsoleLib.Tests;

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
            public TestOp(List<Raster> rRasters, FakeRaster<T> rOutput) : base(rRasters, rOutput)
            {
                Assert.AreEqual(rRasters.Count, _rasters.Count);
                Assert.IsFalse(OpDone);
            }
            public TestOp(List<Raster> rRasters, FakeRaster<T> rOutput, ExtentRectangle newExtent) : base(rRasters, rOutput)
            {
                SetOpExtent(newExtent);
                Assert.AreEqual(rRasters.Count, _rasters.Count);
                Assert.IsFalse(OpDone);
            }

            protected override void ChunkOp(List<T[]> data, T[] outChunk)
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
            public void TestRunWithOutput() { RunWithOutput(); }
        }


        [TestMethod()]
        public void BaseInitTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> Raster2 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 0, 0, 1 }, { 0, 1, 1, 0 }, { 0, 2, 0, 0 } });

            int[,] rExpected = new int[,] { { 2, 2, 3, 5 }, { 5, 7, 8, 8 }, { 9, 12, 11, 12 } };
            FakeRaster<int> rOutput = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1, Raster2 }, rOutput);
            theTest.TestRunWithOutput();
            CollectionAssert.AreEqual(rOutput._outputGrid, rExpected);
        }

        [TestMethod()]
        public void BaseInitTestReal()
        {
            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                Raster rTemplateRaster = new Raster(new FileInfo(TestHelpers.GetTestRasterPath("AngledSlopey950-980E.tif")));
                FakeRaster<float> rOut = new FakeRaster<float>(rTemplateRaster);
                TestOp<float> theTest = new TestOp<float>(new List<Raster> { rTemplateRaster }, rOut);

                // before we set the new extent
                Assert.AreEqual(theTest.InExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest.OpExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest.ChunkExtent.ToString(), "-9979 -10020.0 -9969.0 -10019");
                Assert.IsFalse(theTest.OpDone);

                // Now get the first chunk
                theTest.nextChunk();
                Assert.AreEqual(theTest.InExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest.OpExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest.ChunkExtent.ToString(), "-9979 -10021.0 -9969.0 -10020");
                Assert.IsFalse(theTest.OpDone);

                // Get to the end somehow. This raster is only 100 rows tall so 100 nexts should be 
                // well past the end
                int counter = 0;
                while (!theTest.OpDone && counter < 100)
                {
                    counter++;
                    theTest.nextChunk();
                }
                Assert.AreEqual(theTest.ChunkExtent.ToString(), "-9979 -10029.0 -9969.0 -10029");
                Assert.IsTrue(theTest.OpDone);

                // Now let's try with a different extent
                TestOp<float> theTest2 = new TestOp<float>(new List<Raster> { rTemplateRaster }, rOut, rTemplateRaster.Extent.Buffer(100));
                Assert.AreEqual(theTest2.InExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest2.OpExtent.ToString(), "-9989 -10059.0 -9959.0 -10029");
                Assert.AreEqual(theTest2.ChunkExtent.ToString(), "-9989 -10030.0 -9959.0 -10029");
                Assert.IsFalse(theTest2.OpDone);

                // Now get the first chunk
                theTest2.nextChunk();
                Assert.AreEqual(theTest2.InExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest2.OpExtent.ToString(), "-9989 -10059.0 -9959.0 -10029");
                Assert.AreEqual(theTest2.ChunkExtent.ToString(), "-9989 -10031.0 -9959.0 -10030");
                Assert.IsFalse(theTest2.OpDone);

                // Get to the end somehow. This raster is only 200 rows tall so 100 nexts should be 
                // well past the end
                counter = 0;
                while (!theTest2.OpDone && counter < 100)
                {
                    counter++;
                    theTest2.nextChunk();
                }
                Assert.AreEqual(theTest2.ChunkExtent.ToString(), "-9989 -10059.0 -9959.0 -10059");
                Assert.IsTrue(theTest2.OpDone);

            }
        }


        [TestMethod()]
        public void nextChunkTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> rOutput = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1 }, rOutput);
            Assert.AreEqual(theTest.ChunkExtent.Top, 10);
            Assert.AreEqual(theTest.ChunkExtent.Left, 20);
            Assert.AreEqual(theTest.ChunkExtent.rows, 3);
            Assert.AreEqual(theTest.ChunkExtent.cols, 4);
            Assert.AreEqual(theTest.ChunkExtent.CellHeight, -1);
            Assert.AreEqual(theTest.ChunkExtent.CellWidth, 1);

            theTest.nextChunk();
            Assert.IsTrue(theTest.OpDone);
        }

        [TestMethod()]
        public void GetChunkTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> rOutput = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1 }, rOutput);

            List<int[]> data = new List<int[]>() { new int[theTest.ChunkExtent.cols * theTest.ChunkExtent.rows] };

            theTest.GetChunk(data);
            CollectionAssert.AreEqual(data[0], Raster1._inputgrid.Make1DArray<int>());
        }
    }
}