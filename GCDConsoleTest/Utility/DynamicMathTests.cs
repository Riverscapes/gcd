using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Utility;
using System;
using System.Collections.Generic;
using UnitsNet;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Utility.Tests
{
    [TestClass()]
    public class DynamicMathTests
    {
        [TestMethod()]
        public void DynamicMathTest()
        {
            Assert.AreEqual(DynamicMath.Add(0.5,4), 4.5);
            Assert.AreEqual(DynamicMath.Add(0.5m, 4), 4.5m);
            Assert.AreEqual(DynamicMath.Add(0.5f, 4), 4.5f);
            Assert.AreEqual(DynamicMath.Add(2,3), (int)5);

            Assert.AreEqual(DynamicMath.Subtract(0.5, 4), -3.5);
            Assert.AreEqual(DynamicMath.Subtract(0.5m, 4), -3.5m);
            Assert.AreEqual(DynamicMath.Subtract(0.5f, 4), -3.5f);
            Assert.AreEqual(DynamicMath.Subtract(2, 3), (int)-1);

            Assert.AreEqual(DynamicMath.Multiply(0.5, 4), 2.0);
            Assert.AreEqual(DynamicMath.Multiply(0.5m, 4), 2.0m);
            Assert.AreEqual(DynamicMath.Multiply(0.5f, 4), 2.0f);
            Assert.AreEqual(DynamicMath.Multiply(2, 3), (int)6);

            Assert.AreEqual(DynamicMath.Divide(6.0, 4), 1.5);
            Assert.AreEqual(DynamicMath.Divide(6.0m, 4), 1.5m);
            Assert.AreEqual(DynamicMath.Divide(6.0f, 4), 1.5f);
            Assert.AreEqual(DynamicMath.Divide(6, 3), (int)2);
        }

        [TestMethod()]
        public void SafeDivisionTest()
        {
            decimal dec1 = 0.7m;
            decimal dec2 = 0.025m;

            Length l1 = Length.FromMeters((double)dec1);
            Length l2 = Length.FromMeters((double)dec2);

            Area a1 = Area.FromSquareMeters((double)dec1);
            Area a2 = Area.FromSquareMeters((double)dec2);

            Volume v1 = Volume.FromCubicMeters((double)dec1);
            Volume v2 = Volume.FromCubicMeters((double)dec2);

            Assert.AreEqual(DynamicMath.SafeDivision(l1, l2), 28.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(a1, a2), 28.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(v1, v2), 28.0m);

            Assert.AreEqual(DynamicMath.SafeDivision(v1, a2).Meters, 28.0);
            Assert.AreEqual(DynamicMath.SafeDivision(a1, l2).Meters, 28.0);
            Assert.AreEqual(DynamicMath.SafeDivision(v1, l2).SquareMeters, 28.0);

        }
    }
}