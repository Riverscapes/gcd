using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal;
using System;
using System.Collections.Generic;
using GCDConsoleLib.Tests.Utility;

namespace GCDConsoleLib.Internal.Tests
{
    /// <summary>
    /// In order to test this class we need to have a fake member of it.
    /// </summary>
    [TestClass()]
    class TestCBCOp<T> : CellByCellOperator<T>
    {
        public TestCBCOp(List<Raster> rRasters, ref Raster rOutput) : base(rRasters, rOutput)
        {
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }
        public TestCBCOp(List<Raster> rRasters, ref Raster rOutput, ExtentRectangle newExtent) : base(rRasters, rOutput)
        {
            SetOpExtent(newExtent);
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }

        protected override T CellOp(ref List<T[]> data, int id)
        {
            // Calculate the real 1D index of this cell 
            int idR0 = (int)((ChunkExtent.Top - OpExtent.Top) / OpExtent.CellHeight);
            int idR1 = (int)((ChunkExtent.Left - OpExtent.Left) / OpExtent.CellWidth);

            //int realid = (idR0 * dstSizeR1 + (idR1 + offsetR1);
            //for (int did = 0; did < data.Count; did++)
            //{
            //    Assert.AreEqual(_rasters[did]._innter, data[did][id]);
            //}
            //return realid;
            return data[0][id];
        }

    }


    [TestClass()]
    public class CellByCellOperatorTests
    {
        [TestMethod()]
        public void CellByCellOperatorTest()
        {
            FakeRaster<int> Raster1 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            FakeRaster<int> Raster2 = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 1, 0, 0, 1 }, { 0, 1, 1, 0 }, { 0, 2, 0, 0 } });
            int[,] rExpected = new int[,] { { 2, 2, 3, 5 }, { 5, 7, 8, 8 }, { 9, 12, 11, 12 } };
            Raster rOutput = new FakeRaster<int>(0, 0, -1, 1, new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } });
            TestCBCOp<int> theTest = new TestCBCOp<int>(new List<Raster> { Raster1, Raster2 }, ref rOutput);
            theTest.Run();
            //CollectionAssert.AreEqual(rOutput._outputGrid, rExpected);
        }
    }
}