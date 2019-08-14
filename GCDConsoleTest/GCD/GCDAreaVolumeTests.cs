using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.GCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace GCDConsoleLib.GCD.Tests
{
    [TestClass()]
    public class GCDAreaVolumeTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void GCDAreaVolumeTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void EqualsTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void AddToSumAndIncrementCounterTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void IncrementCountTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void AddToSumTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void GetAreaTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void SetAreaTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void GetAreaValueTest()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Ensure that areas and volumes stored in imperial units are read from GCD project file convert back to same values
        /// </summary>
        /// <remarks>
        /// Test written in response to GCD issue # 355
        /// https://github.com/Riverscapes/gcd/issues/355
        /// 
        /// Projects stored in non-metric units were reading volumes from disk and then - if the project was changed during a GCD session -
        /// writing different values back to the project file. Somewhere along the way volumes were getting warped by unit conversion.
        /// 
        /// Turns out that the GCDVolume class was storing the count of cells based on metric units but was assuming it was derived from
        /// the project units when reconstituting it back to a volume for retrieval.
        /// </remarks>
        [TestMethod()]
        [TestCategory("Unit")]
        public void GetVolumeTest()
        {
            // The code always worked when the project was in metric units. It was only broken for imperial units.
            // The following test performs the test on both metric and imperial to ensure that we haven't broken anything.
            UnitGroup metricUnits = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicMeter, UnitsNet.Units.AreaUnit.SquareMeter, UnitsNet.Units.LengthUnit.Meter, UnitsNet.Units.LengthUnit.Meter);
            UnitGroup imperiUnits = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicYard, UnitsNet.Units.AreaUnit.SquareFoot, UnitsNet.Units.LengthUnit.UsSurveyFoot, UnitsNet.Units.LengthUnit.UsSurveyFoot);

            foreach (UnitGroup testUnits in new List<UnitGroup>() { imperiUnits, metricUnits })
            {
                // Raw values that will be converted to a GCD area and volume and then back to double to ensure we get the same values
                double cellAreaVal = 2.2500099687832296;
                double areaOriginal = 41626748.17941805;
                double volOriginal = 91729.489667072165;

                // Deserialize from String
                Area cellArea = Area.From(cellAreaVal, testUnits.ArUnit);
                Area area = Area.From(areaOriginal, testUnits.ArUnit);
                Volume vol = Volume.From(volOriginal, testUnits.VolUnit);
                GCDAreaVolume areaVol = new GCDAreaVolume(area, vol, cellArea, testUnits);

                // Serialize back to string
                double areaResult = areaVol.GetArea(cellArea).As(testUnits.ArUnit);
                double volResult = areaVol.GetVolume(cellArea, testUnits).As(testUnits.VolUnit);

                Assert.AreEqual(areaOriginal, areaResult, 0.00001);
                Assert.AreEqual(volOriginal, volResult, 0.00001);
            }
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void SetVolumeTest()
        {
            Assert.Inconclusive();
        }
    }
}