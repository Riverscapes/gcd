using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.GCD;
using System;
using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.GCD.Tests
{
    [TestClass()]
    public class DoDStatsTests
    {
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

            Assert.AreEqual(DoDStats.SafeDivision(l1,l2), 28.0m);
            Assert.AreEqual(DoDStats.SafeDivision(a1, a2), 28.0m);
            Assert.AreEqual(DoDStats.SafeDivision(v1, v2), 28.0m);

            Assert.AreEqual(DoDStats.SafeDivision(v1, a2).Meters, 28.0);
            Assert.AreEqual(DoDStats.SafeDivision(a1, l2).Meters, 28.0);
            Assert.AreEqual(DoDStats.SafeDivision(v1, l2).SquareMeters, 28.0);

        }

    }
}