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
        // These unit collections are named so the first suffix describes the linear units (vertical and horizontal) and the second suffix describes the area and volume units
        private UnitGroup units_m_m = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicMeter, UnitsNet.Units.AreaUnit.SquareMeter, UnitsNet.Units.LengthUnit.Meter, UnitsNet.Units.LengthUnit.Meter);
        private UnitGroup units_m_ft = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicFoot, UnitsNet.Units.AreaUnit.SquareFoot, UnitsNet.Units.LengthUnit.Meter, UnitsNet.Units.LengthUnit.Meter);
        private UnitGroup units_m_yd = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicYard, UnitsNet.Units.AreaUnit.SquareYard, UnitsNet.Units.LengthUnit.Meter, UnitsNet.Units.LengthUnit.Meter);
        private UnitGroup units_ft_m = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicMeter, UnitsNet.Units.AreaUnit.SquareMeter, UnitsNet.Units.LengthUnit.Foot, UnitsNet.Units.LengthUnit.Foot);
        private UnitGroup units_ft_ft = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicFoot, UnitsNet.Units.AreaUnit.SquareFoot, UnitsNet.Units.LengthUnit.Foot, UnitsNet.Units.LengthUnit.Foot);
        private UnitGroup units_ft_yd = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicYard, UnitsNet.Units.AreaUnit.SquareYard, UnitsNet.Units.LengthUnit.Foot, UnitsNet.Units.LengthUnit.Foot);
        private UnitGroup units_usft_m = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicMeter, UnitsNet.Units.AreaUnit.SquareMeter, UnitsNet.Units.LengthUnit.UsSurveyFoot, UnitsNet.Units.LengthUnit.UsSurveyFoot);
        private UnitGroup units_usft_ft = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicFoot, UnitsNet.Units.AreaUnit.SquareFoot, UnitsNet.Units.LengthUnit.UsSurveyFoot, UnitsNet.Units.LengthUnit.UsSurveyFoot);
        private UnitGroup units_usft_yd = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicYard, UnitsNet.Units.AreaUnit.SquareYard, UnitsNet.Units.LengthUnit.UsSurveyFoot, UnitsNet.Units.LengthUnit.UsSurveyFoot);
        private UnitGroup units_yd_m = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicMeter, UnitsNet.Units.AreaUnit.SquareMeter, UnitsNet.Units.LengthUnit.Yard, UnitsNet.Units.LengthUnit.Yard);
        private UnitGroup units_yd_ft = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicFoot, UnitsNet.Units.AreaUnit.SquareFoot, UnitsNet.Units.LengthUnit.Yard, UnitsNet.Units.LengthUnit.Yard);
        private UnitGroup units_yd_yd = new UnitGroup(UnitsNet.Units.VolumeUnit.CubicYard, UnitsNet.Units.AreaUnit.SquareYard, UnitsNet.Units.LengthUnit.Yard, UnitsNet.Units.LengthUnit.Yard);

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
        /// 
        /// The code always worked when the project was in metric units. It was only broken for imperial units.
        /// The following test performs the test on both metric and imperial to ensure that we haven't broken anything.
        /// </remarks>
        [TestMethod()]
        [TestCategory("Unit")]
        public void GetVolumeTest()
        {
            foreach (UnitGroup testUnits in new List<UnitGroup>() { units_ft_yd, units_m_m })
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
        public void GetVolumeMisMatchUnitsTest()
        {
            // All tests assume the same 4m² cell resolution (43 ft², 4.78 yd²)
            Area cellArea = new Area(4);
            
            // All the tests use the same 1000 elevation, but interpret it differently depending on the units used.
            GCDAreaVolume areaVol = new GCDAreaVolume();
            for (int i = 0; i < 10; i++)
                areaVol.AddToSumAndIncrementCounter(100d);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Source elevation in metres

            // 1000m elevation by 4m² is 4000 m³
            Assert.AreEqual(4000, areaVol.GetVolume(cellArea, units_m_m).As(units_m_m.VolUnit));

            // 1000m elevation by 4m² is 141,258 ft³
            Assert.AreEqual(141258, areaVol.GetVolume(cellArea, units_m_ft).As(units_m_ft.VolUnit), 1);

            // 1000m elevation by 4m² is 5,231.80 yd³
            Assert.AreEqual(5231, areaVol.GetVolume(cellArea, units_m_yd).As(units_m_yd.VolUnit), 1);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Source elevation in feet

            // 1000ft elevation by 43ft² is 43,055 ft³
            Assert.AreEqual(43055, areaVol.GetVolume(cellArea, units_ft_ft).As(units_ft_ft.VolUnit), 1);

            // 1000ft elevation by 43ft² is 1,219 m³
            Assert.AreEqual(1219, areaVol.GetVolume(cellArea, units_ft_m).As(units_ft_m.VolUnit), 1);

            // 1000ft elevation by 43ft² is 1,594 ft³
            Assert.AreEqual(1594, areaVol.GetVolume(cellArea, units_ft_yd).As(units_ft_yd.VolUnit), 1);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Source elevation in yards

            // 1000yd elevation by 4.78 yd² is 4,783 yd³
            Assert.AreEqual(4783, areaVol.GetVolume(cellArea, units_yd_yd).As(units_yd_yd.VolUnit), 1);

            // 1000yd elevation by 4.78 yd² is 129,166 ft³
            Assert.AreEqual(129167, areaVol.GetVolume(cellArea, units_yd_ft).As(units_yd_ft.VolUnit), 1);

            // 1000yd elevation by 4.78 yd² is 3,657 m³
            Assert.AreEqual(3657, areaVol.GetVolume(cellArea, units_yd_m).As(units_yd_m.VolUnit), 1);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Source elevation in US Survey Feet

            // 1000 USft elevation by 43ft² is 43056 ft³
            Assert.AreEqual(43055, areaVol.GetVolume(cellArea, units_usft_ft).As(units_usft_ft.VolUnit), 1);

            // 1000 USft elevation by 43ft² is 1,219 m³
            Assert.AreEqual(1219, areaVol.GetVolume(cellArea, units_usft_m).As(units_usft_m.VolUnit), 1);

            // 1000 USft elevation by 43ft² is 1,595 yd³
            Assert.AreEqual(1595, areaVol.GetVolume(cellArea, units_usft_yd).As(units_usft_yd.VolUnit), 1);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void SetVolumeTest()
        {
            Assert.Inconclusive();
        }
    }
}