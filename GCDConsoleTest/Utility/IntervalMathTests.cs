using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Utility;
using System;

namespace GCDConsoleLib.Utility.Tests
{
    [TestClass()]
    public class IntervalMathTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void GetNearestFiveOrderWidthTest()
        {
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.1m), 0.1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.11m), 0.1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.2m), 0.1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.1000000001m), 0.1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.0900000000m), 0.1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.0800000000m), 0.1m);

            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.5m), 0.5m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.49999999m), 0.5m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.500000001m), 0.5m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.432342352352m), 0.5m);

            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(1.49m), 1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(1.0m), 1m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(1.234523451m), 1.0m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(0.756m), 1.0m);

            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(50.500234234m), 50m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(64.500234234m), 50m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(80), 100m);

            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(120), 100m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(124), 100m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(125), 100m);

            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(250), 100m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(300), 500m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(500), 500m);
            Assert.AreEqual(IntervalMath.GetNearestFiveOrderWidth(749), 500m);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void GetRegularizedMaxMinTest()
        {
            // Test some bad behaviour first"
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(0, 0).Equals(new Tuple<decimal, decimal>(0, 0)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(0, 10).Equals(new Tuple<decimal, decimal>(0, 10)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(10, 0).Equals(new Tuple<decimal, decimal>(10, 0)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(decimal.MaxValue, decimal.MinValue).Equals(new Tuple<decimal, decimal>(decimal.MaxValue, decimal.MinValue)));

            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(20, 10).Equals(new Tuple<decimal, decimal>(20, 10)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(-20, 10).Equals(new Tuple<decimal, decimal>(-20, 10)));

            // Now some regular interval stuff
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(100, -100).Equals(new Tuple<decimal, decimal>(100, -100)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(101, -100).Equals(new Tuple<decimal, decimal>(150, -100)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(101, -101).Equals(new Tuple<decimal, decimal>(150, -150)));

            // Now some irregular interval stuff
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(2756, -75.123123123m).Equals(new Tuple<decimal, decimal>(3000, -500)));
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(2756.2342342m, -75.123123123m).Equals(new Tuple<decimal, decimal>(3000, -500)));

            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(10, -734234235.123123123m).Equals(new Tuple<decimal, decimal>(50000000, -750000000)));

            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(0.0001m, -0.0001m).Equals(new Tuple<decimal, decimal>(0.0001m, -0.0001m)));

            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(0.00015234m, -0.0001332m).Equals(new Tuple<decimal, decimal>(0.00020m, -0.00015m)));


            // Now with buffering
            Assert.IsTrue(IntervalMath.GetRegularizedMaxMin(100, -100, 0.1m).Equals(new Tuple<decimal, decimal>(150, -150)));

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void SensibleChartIntervalTest()
        {
            Assert.AreEqual(IntervalMath.GetSensibleChartInterval(100, -100, 20), 10);
            Assert.AreEqual(IntervalMath.GetSensibleChartInterval(100, 0, 20), 5);
            Assert.AreEqual(IntervalMath.GetSensibleChartInterval(20, -10, 31), 1);

        }
    }
}