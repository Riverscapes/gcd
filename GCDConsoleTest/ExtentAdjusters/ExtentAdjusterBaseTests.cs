using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.ExtentAdjusters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.ExtentAdjusters.Tests
{
    [TestClass()]
    public class ExtentAdjusterBaseTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void GetInitialNumDecimalsTest()
        {
            // Easy usual scenarios
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(2m));
            Assert.AreEqual(1, ExtentAdjusterBase.GetInitialNumDecimals(0.1m));
            Assert.AreEqual(2, ExtentAdjusterBase.GetInitialNumDecimals(0.01m));
            Assert.AreEqual(3, ExtentAdjusterBase.GetInitialNumDecimals(0.001m));
            Assert.AreEqual(4, ExtentAdjusterBase.GetInitialNumDecimals(0.0001m));
            Assert.AreEqual(5, ExtentAdjusterBase.GetInitialNumDecimals(0.00001m));
            Assert.AreEqual(6, ExtentAdjusterBase.GetInitialNumDecimals(0.000001m));
            Assert.AreEqual(7, ExtentAdjusterBase.GetInitialNumDecimals(0.0000001m));
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(10m));
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(100m));
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(1000m));
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(10000m));

            // More tricky...

            // So close to that we might as well just drop the decimals
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(10.0003m));
            Assert.AreEqual(0, ExtentAdjusterBase.GetInitialNumDecimals(2.000000000000040102m));
            Assert.AreEqual(1, ExtentAdjusterBase.GetInitialNumDecimals(2.500000000000040102m));
        }
    }
}