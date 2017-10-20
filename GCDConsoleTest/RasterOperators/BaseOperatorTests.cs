using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class BaseOperatorTests
    {

        public class TestOp : BaseOperator
        {
            public TestOp(List<Raster> rRasters, ref Raster rOutputRaster) :  base(rRasters, ref rOutputRaster)  { }
            protected override void ChunkOp(ref List<double[]> data, ref double[] outChunk)
            {
            }
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