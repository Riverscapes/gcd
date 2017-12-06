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
            public TestOp(List<Raster> rRasters) : base(rRasters)
            {
                Assert.AreEqual(rRasters.Count, _rasters.Count);
                Assert.IsFalse(OpDone);
            }
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

            public TestOp(List<Raster> rRasters, Raster outRaster) : base(rRasters, outRaster)
            {
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
                            val = DynamicMath.Add(val, data[idd][idx]);
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


        delegate void Del(object sender, int e);

        [TestMethod]
        public void TestEventRaised()
        {
            // This is a test object to collect all the progress events
            List<int> receivedEvents = new List<int>();

            // Now we define an action to run when the progress event fires. Do tjis in whatever function
            // You're calling Interface.cs from 
            EventHandler<int> receivedEventsHandler = delegate(object sender, int e)  { receivedEvents.Add(e); };

            ////////////////////////////////// <Interface.cs> ////////////////////////////////////////////

            // Now we fake what's happening inside Interface.cs"
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1 });

            // Here's where the delegate is assigned to the 
            theTest.ProgressEvent += receivedEventsHandler;

            theTest.Run();
            ////////////////////////////////// </Interface.cs> ////////////////////////////////////////////

            // Now make sure our list contains the events (doesn't have to be a list. You could update a UI control instead)
            CollectionAssert.Contains(receivedEvents, 0);
            CollectionAssert.Contains(receivedEvents, 100);
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
                int buffer = 100;
                TestOp<float> theTest2 = new TestOp<float>(new List<Raster> { rTemplateRaster }, rOut, rTemplateRaster.Extent.Buffer(buffer));
                Assert.AreEqual(theTest2.InExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest2.OpExtent.ToString(), "-9989 -10039.0 -9959.0 -10009");
                Assert.AreEqual(theTest2.ChunkExtent.ToString(), "-9989 -10010.0 -9959.0 -10009");
                Assert.IsFalse(theTest2.OpDone);

                // Now get the first chunk
                theTest2.nextChunk();
                Assert.AreEqual(theTest2.InExtent.ToString(), "-9979 -10029.0 -9969.0 -10019");
                Assert.AreEqual(theTest2.OpExtent.ToString(), "-9989 -10039.0 -9959.0 -10009");
                Assert.AreEqual(theTest2.ChunkExtent.ToString(), "-9989 -10011.0 -9959.0 -10010");
                Assert.IsFalse(theTest2.OpDone);

                // Get to the end somehow. This raster is only 200 rows tall so 100 nexts should be 
                // well past the end
                counter = 0;
                while (!theTest2.OpDone && counter < 100)
                {
                    counter++;
                    theTest2.nextChunk();
                }
                Assert.AreEqual(theTest2.ChunkExtent.ToString(), "-9989 -10039.0 -9959.0 -10039");
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
            Assert.AreEqual(theTest.ChunkExtent.Rows, 3);
            Assert.AreEqual(theTest.ChunkExtent.Cols, 4);
            Assert.AreEqual(theTest.ChunkExtent.CellHeight, -1);
            Assert.AreEqual(theTest.ChunkExtent.CellWidth, 1);

            theTest.nextChunk();
            Assert.IsTrue(theTest.OpDone);
        }


        [TestMethod()]
        public void NodataTests()
        {
            Raster intRaster = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1} });
            Raster floatRaster = new FakeRaster<float>(10, 20, -1, 1, new float[,] { { 1.0f } });
            Raster doubleRaster = new FakeRaster<double>(10, 20, -1, 1, new double[,] { { 1.0 } });
            Raster byteRaster = new FakeRaster<byte>(10, 20, -1, 1, new byte[,] { { 0x20 } });

            intRaster.origNodataVal = int.MinValue;
            floatRaster.origNodataVal = float.MinValue;
            doubleRaster.origNodataVal = double.MinValue;
            byteRaster.origNodataVal = byte.MinValue;

            Raster OUTfloatRaster1 = new FakeRaster<float>(10, 20, -1, 1, new float[,] { { 0.0f } });
            OUTfloatRaster1.origNodataVal = 0; // Set Purposely wrong
            TestOp<float> theTest1 = new TestOp<float>(new List<Raster> { floatRaster }, OUTfloatRaster1);
            float expected = floatRaster.NodataValue<float>();
            Assert.AreEqual(theTest1.OpNodataVal, expected);
            Assert.AreEqual(OUTfloatRaster1.NodataValue<float>(), expected);

            Raster OUTdoubleRaster1 = new FakeRaster<double>(10, 20, -1, 1, new double[,] { { 0.0 } });
            OUTdoubleRaster1.origNodataVal = 0; // Set Purposely wrong
            TestOp<double> theTest2 = new TestOp<double>(new List<Raster> { floatRaster }, OUTdoubleRaster1);
            double expected2 = floatRaster.NodataValue<double>();
            Assert.AreEqual(theTest2.OpNodataVal, expected2);
            Assert.AreEqual(OUTdoubleRaster1.NodataValue<double>(), floatRaster.NodataValue<double>());

            Raster OUTfloatRaster2 = new FakeRaster<float>(10, 20, -1, 1, new float[,] { { 0.0f } });
            OUTfloatRaster2.origNodataVal = 0; // Set Purposely wrong
            TestOp<double> theTest3 = new TestOp<double>(new List<Raster> { doubleRaster }, OUTfloatRaster2);
            double expected3 = floatRaster.NodataValue<double>();
            Assert.AreEqual(theTest3.OpNodataVal, expected3);
            Assert.AreEqual(OUTfloatRaster2.NodataValue<double>(), expected3);

            Raster OUTfloatRaster3 = new FakeRaster<float>(10, 20, -1, 1, new float[,] { { 0.0f } });
            OUTfloatRaster3.origNodataVal = 0; // Set Purposely wrong
            TestOp<float> theTest4 = new TestOp<float>(new List<Raster> { intRaster }, OUTfloatRaster3);
            float expected4 = intRaster.NodataValue<float>();
            Assert.AreEqual(theTest4.OpNodataVal, expected4);
            Assert.AreEqual(OUTfloatRaster3.NodataValue<double>(), expected4);

            Raster OUTintRaster = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0 } });
            OUTintRaster.origNodataVal = 0; // Set Purposely wrong
            TestOp<float> theTest5 = new TestOp<float>(new List<Raster> { doubleRaster }, OUTintRaster);
            float expected5 = floatRaster.NodataValue<float>();
            Assert.AreEqual(theTest5.OpNodataVal, expected5);
            Assert.AreEqual(OUTintRaster.NodataValue<float>(), expected5);
        }

        [TestMethod()]
        public void GetChunkTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> rOutput = new FakeRaster<int>(10, 20, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestOp<int> theTest = new TestOp<int>(new List<Raster> { Raster1 }, rOutput);

            List<int[]> data = new List<int[]>() { new int[theTest.ChunkExtent.Cols * theTest.ChunkExtent.Rows] };

            theTest.GetChunk(data);
            CollectionAssert.AreEqual(data[0], Raster1._inputgrid.Make1DArray<int>());
        }
    }
}