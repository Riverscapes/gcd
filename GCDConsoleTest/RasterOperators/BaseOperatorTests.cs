using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class BaseOperatorTests
    {
        /// <summary>
        /// In order to test the BaseOperator we need to implement it
        /// </summary>
        public class TestOp : BaseOperator
        {
            public TestOp(ref Raster rR1, ref Raster rR2, OpTypes theType, ExtentRectangle newRect) : base(ref rR1, ref rR2, theType, newRect) { }
            protected override double OpCell(ref List<double[]> data, int id) { return 0; }
            protected override void OpChunk(ref List<double[]> data, ref double[] outChunk) { }
            protected override double OpOverlap(ref List<double[]> data) { return 0; }
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

        [TestMethod()]
        public void RunCellTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RunChunkTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RunOverlapTest()
        {
            Assert.Fail();
        }
    }
}