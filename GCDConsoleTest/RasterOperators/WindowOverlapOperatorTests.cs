using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Internal.Tests
{
    class TestWOOOp<T> : WindowOverlapOperator<T>
    {
        public TestWOOOp(List<Raster> rRasters, int buff,  ref Raster rOutput) : base(rRasters, buff, rOutput)
        {
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }
        public TestWOOOp(List<Raster> rRasters, ref Raster rOutput, ExtentRectangle newExtent, int buff) : base(rRasters, buff, rOutput)
        {
            SetOpExtent(newExtent);
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }
        protected override T WindowOp(ref List<T[]> data)
        {
            // Calculate the real 1D index of this cell 
            int idR0 = (int)((ChunkWindow.Top - OpExtent.Top) / OpExtent.CellHeight);
            int idR1 = (int)((ChunkWindow.Left - OpExtent.Left) / OpExtent.CellWidth);

            for (int did = 0; did < data.Count; did++)
            {
            
            }
            return data[0][0];
        }

    }


    [TestClass()]
    public class WindowOverlapOperatorTests
    {
        [TestMethod()]
        public void WindowOverlapOperatorTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 1, 2, -999 }, { 3, 4, 5, -999 }, { 6, 7, 8, -999 } });
            Raster rOutput = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });

            TestWOOOp<int> theTest = new TestWOOOp<int>(new List<Raster> { Raster1 }, 1, ref rOutput);
            theTest.Run();
            Assert.Inconclusive();
        }
    }
}