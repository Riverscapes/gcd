using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.GCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.GCD.Tests
{
    [TestClass()]
    public class DoDStatsTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void DoDStatsTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void EqualsTest()
        {
            Area ar1 = Area.FromSquareMeters(1);
            Area ar2 = Area.FromSquareMeters(2000);
            UnitGroup ug1 = new UnitGroup(VolumeUnit.ImperialBeerBarrel, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);
            UnitGroup ug2 = new UnitGroup(VolumeUnit.CubicCentimeter, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);

            DoDStats tester1 = new DoDStats(ar1, ug1); // zeros
            DoDStats tester2 = new DoDStats(ar2, ug1); // zeros different area
            DoDStats tester3 = new DoDStats(
                Area.FromSquareMeters(1),
                Area.FromSquareMeters(2),
                Area.FromSquareMeters(3),
                Area.FromSquareMeters(4),
                Volume.FromCubicMeters(1),
                Volume.FromCubicMeters(2),
                Volume.FromCubicMeters(3),
                Volume.FromCubicMeters(4),
                Volume.FromCubicMeters(5),
                Volume.FromCubicMeters(6),
                ar1, ug1  ); // Values 1
            DoDStats tester4 = new DoDStats(tester3); // Copied DoDStats tester3
            DoDStats tester5 = new DoDStats(
                Area.FromSquareMeters(1),
                Area.FromSquareMeters(2),
                Area.FromSquareMeters(3),
                Area.FromSquareMeters(4),
                Volume.FromCubicMeters(1),
                Volume.FromCubicMeters(2),
                Volume.FromCubicMeters(3),
                Volume.FromCubicMeters(4),
                Volume.FromCubicMeters(5),
                Volume.FromCubicMeters(6),
                ar1, ug2); // Same as tester2 with different units

            Assert.IsTrue(tester1.Equals(tester1));
            Assert.IsFalse(tester1.Equals(tester2));
            Assert.IsFalse(tester1.Equals(tester3));
            Assert.IsTrue(tester1.Equals(tester4));
            Assert.IsFalse(tester1.Equals(tester5));

            Assert.IsFalse(tester2.Equals(tester1));
            Assert.IsTrue(tester2.Equals(tester2));
            Assert.IsFalse(tester2.Equals(tester3));
            Assert.IsFalse(tester2.Equals(tester4));
            Assert.IsFalse(tester2.Equals(tester5));

            Assert.IsFalse(tester3.Equals(tester1));
            Assert.IsFalse(tester3.Equals(tester2));
            Assert.IsTrue(tester3.Equals(tester3));
            Assert.IsFalse(tester3.Equals(tester4));
            Assert.IsFalse(tester3.Equals(tester5));

            Assert.IsTrue(tester4.Equals(tester1));
            Assert.IsFalse(tester4.Equals(tester2));
            Assert.IsFalse(tester4.Equals(tester3));
            Assert.IsTrue(tester4.Equals(tester4));
            Assert.IsFalse(tester4.Equals(tester5));

            Assert.IsFalse(tester5.Equals(tester1));
            Assert.IsFalse(tester5.Equals(tester2));
            Assert.IsFalse(tester5.Equals(tester3));
            Assert.IsFalse(tester5.Equals(tester4));
            Assert.IsTrue(tester5.Equals(tester5));
        }
    }
}