using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.Internal.Tests
{
    class TestWOOOp<T> : WindowOverlapOperator<T>
    {
        public TestWOOOp(List<Raster> rRasters, ref Raster rOutput, int buff) : base(rRasters, ref rOutput,  buff)
        {
            Assert.AreEqual(rRasters.Count, _rasters.Count);
            Assert.IsFalse(OpDone);
        }
        public TestWOOOp(List<Raster> rRasters, ref Raster rOutput, ExtentRectangle newExtent, int buff) : base(rRasters, ref rOutput, buff)
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

            //int realid = (idR0 * dstSizeR1 + (idR1 + offsetR1);
            //for (int did = 0; did < data.Count; did++)
            //{
            //    Assert.AreEqual(_rasters[did]._innter, data[did][id]);
            //}
            return data[0][0];
        }

    }


    [TestClass()]
    public class WindowOverlapOperatorTests
    {
        [TestMethod()]
        public void WindowOverlapOperatorTest()
        {
            Assert.Fail();
        }
    }
}