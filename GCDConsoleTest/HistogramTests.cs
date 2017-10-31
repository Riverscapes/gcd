using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class HistogramTests
    {
        [TestMethod()]
        public void HistogramTest()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(20, 1, -0.1m, 0.1m, vUnit, hUnit);

            Assert.AreEqual(rTest1.BinArea.SquareMeters, (0.1 * 0.1));
            Assert.AreEqual(rTest1.FirstBinId, 0);
            Assert.AreEqual(rTest1.LastBinId, 19);
            Assert.AreEqual(rTest1.HistogramLower.Meters, Length.From(-10.0,LengthUnit.Foot).Meters);
            Assert.AreEqual(rTest1.HistogramUpper.Meters, Length.From(10.0, LengthUnit.Foot).Meters);
            Assert.AreEqual(rTest1.HorizontalUnit, hUnit);
            Assert.AreEqual(rTest1.VerticalUnit, vUnit);
            Assert.AreEqual(rTest1.Count, 20);

            // Now let's try one with uneven bins
            Histogram rTest2 = new Histogram(19, 1, -0.1m, 0.1m, LengthUnit.Foot, LengthUnit.Meter);
            Assert.AreEqual(rTest2.BinArea.SquareMeters, (0.1 * 0.1));
            Assert.AreEqual(rTest2.FirstBinId, 0);
            Assert.AreEqual(rTest2.LastBinId, 19);
            Assert.AreEqual(rTest2.HistogramLower.Meters, Length.From(-10.0, LengthUnit.Foot).Meters);
            Assert.AreEqual(rTest2.HistogramUpper.Meters, Length.From(10.0, LengthUnit.Foot).Meters);
            Assert.AreEqual(rTest2.HorizontalUnit, hUnit);
            Assert.AreEqual(rTest2.VerticalUnit, vUnit);
            Assert.AreEqual(rTest1.Count, 20);

        }

        [TestMethod()]
        public void BinIdTest()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(20, 1, -0.1m, 0.1m, vUnit, hUnit);

            Assert.AreEqual(rTest1.BinId(Length.From(-10.1,vUnit)), -1);
            Assert.AreEqual(rTest1.BinId(Length.From(-10, vUnit)), 0);
            Assert.AreEqual(rTest1.BinId(Length.From(-9.999, vUnit)), 0);

            Assert.AreEqual(rTest1.BinId(Length.From(-5.1, vUnit)), 4);
            Assert.AreEqual(rTest1.BinId(Length.From(-5, vUnit)), 4);
            Assert.AreEqual(rTest1.BinId(Length.From(-4.9, vUnit)), 5);

            Assert.AreEqual(rTest1.BinId(Length.From(-0.1, vUnit)), 9);
            Assert.AreEqual(rTest1.BinId(Length.From(0, vUnit)), 9);
            Assert.AreEqual(rTest1.BinId(Length.From(0.1, vUnit)), 10);

            Assert.AreEqual(rTest1.BinId(Length.From(10.1, vUnit)), -1);
            Assert.AreEqual(rTest1.BinId(Length.From(10, vUnit)), 19);
            Assert.AreEqual(rTest1.BinId(Length.From(9.999, vUnit)), 19);
        }

        [TestMethod()]
        public void BinLowerTest()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(20, 1, -0.1m, 0.1m, vUnit, hUnit);
            Assert.AreEqual(rTest1.BinLower(0).As(vUnit), -10);
            Assert.AreEqual(rTest1.BinLower(19).As(vUnit), 9);

        }

        [TestMethod()]
        public void BinUpperTest()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(20, 1, -0.1m, 0.1m, vUnit, hUnit);
            Assert.AreEqual(rTest1.BinUpper(0).As(vUnit), -9);
            Assert.AreEqual(rTest1.BinUpper(19).As(vUnit), 10);
        }


        [TestMethod()]
        public void BinCentreTest()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(20, 1, -0.1m, 0.1m, vUnit, hUnit);
            Assert.AreEqual(rTest1.BinCentre(0).As(vUnit), -9.5);
            Assert.AreEqual(rTest1.BinCentre(19).As(vUnit), 9.5);
        }

        [TestMethod()]
        public void BinValsTest()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(20, 1, -0.1m, 0.1m, vUnit, hUnit);

            //Add some fake values into the mix
            for (int i = 0; i < 20; i++)
                rTest1.AddBinVal((double)i);

            double CellAreaM2 = rTest1.BinArea.SquareMeters;
            // Now test the values
            for (int i = 0; i < 20; i++)
            {
                double iVolm = Length.From((double)i, LengthUnit.Foot).Meters;
                Assert.AreEqual(rTest1.BinVolume(i).CubicMeters, iVolm * CellAreaM2);
                Assert.AreEqual(rTest1.BinCount(i), 1);
            }

        }

        [TestMethod()]
        public void WriteFileTest()
        {
            Assert.Inconclusive();
        }
    }
}