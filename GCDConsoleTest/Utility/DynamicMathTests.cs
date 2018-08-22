using GCDConsoleLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using UnitsNet;

namespace GCDConsoleLib.Utility.Tests
{
    [TestClass()]
    public class DynamicMathTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void DynamicMathTest()
        {
            Assert.AreEqual(DynamicMath.Add(0.5, 4), 4.5);
            Assert.AreEqual(DynamicMath.Add(0.5m, 4), 4.5m);
            Assert.AreEqual(DynamicMath.Add(0.5f, 4), 4.5f);
            Assert.AreEqual(DynamicMath.Add(2, 3), (int)5);

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
        [TestCategory("Unit")]
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

            Assert.AreEqual(DynamicMath.SafeDivision(dec1, dec2), 28.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(l1, l2), 28.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(a1, a2), 28.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(v1, v2), 28.0m);

            Assert.AreEqual(DynamicMath.SafeDivision(v1, a2).Meters, 28.0);
            Assert.AreEqual(DynamicMath.SafeDivision(a1, l2).Meters, 28.0);
            Assert.AreEqual(DynamicMath.SafeDivision(v1, l2).SquareMeters, 28.0);

            // Edge Cases (Very big numbers)
            Assert.AreEqual(DynamicMath.SafeDivision(decimal.MaxValue, decimal.MinValue), -1.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(decimal.MaxValue, decimal.MaxValue), 1.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(decimal.MinValue, decimal.MinValue), 1.0m);

            Area za0 = Area.FromSquareMeters(1.0);
            Area za1 = Area.FromSquareMeters((double)decimal.MaxValue);
            Area za2 = Area.FromSquareMeters((double)decimal.MinValue);

            Assert.AreEqual(DynamicMath.SafeDivision(za0, za1), 0.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(za1, za1), 1.0m);
            Assert.AreEqual(DynamicMath.SafeDivision(za1, za2), -1.0m);

            // Now do very small numbers
            Area zsm1 = Area.FromSquareMeters((double)(1 / decimal.MaxValue));
            Area zsm2 = Area.FromSquareMeters((double)(1 / decimal.MinValue));

            Assert.AreEqual(DynamicMath.SafeDivision(za0, zsm1), decimal.MaxValue); // 1/0 case
            Assert.AreEqual(DynamicMath.SafeDivision(za0, zsm2), decimal.MaxValue); // 1/0 case
            Assert.AreEqual(DynamicMath.SafeDivision(zsm1, zsm1), 0); // triggers the 0/0 case
            Assert.AreEqual(DynamicMath.SafeDivision(zsm1, zsm2), 0);// triggers the 0/0 case

            // Now do zeros
            Area zero = Area.FromSquareMeters(0);
            Assert.AreEqual(DynamicMath.SafeDivision(zero, zero), 0);
            Assert.AreEqual(DynamicMath.SafeDivision(za0, zero), decimal.MaxValue);
            Assert.AreEqual(DynamicMath.SafeDivision(zero, za0), 0);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void NumDecimalsTest()
        {
            Assert.AreEqual(DynamicMath.NumDecimals(2.1231m), 4);
            Assert.AreEqual(DynamicMath.NumDecimals(52352342.1231m), 4);
            Assert.AreEqual(DynamicMath.NumDecimals(52352342.12310m), 4);
            Assert.AreEqual(DynamicMath.NumDecimals(10000), 0);
            Assert.AreEqual(DynamicMath.NumDecimals(3), 0);
            Assert.AreEqual(DynamicMath.NumDecimals(3.0m), 0);
            Assert.AreEqual(DynamicMath.NumDecimals(0.00000106103m), 11);
            Debug.WriteLine("done");
        }
    }
}