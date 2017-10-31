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

            Assert.AreEqual(rTest1.BinArea.SquareMeters, (double)(0.1m * 0.1m));
            Assert.AreEqual(rTest1.FirstBinId, 0);
            Assert.AreEqual(rTest1.LastBinId, 19);
            Assert.AreEqual(rTest1.HistogramLower.Meters, Length.From(-10.0,LengthUnit.Foot).Meters);
            Assert.AreEqual(rTest1.HistogramUpper.Meters, Length.From(10.0, LengthUnit.Foot).Meters);
            Assert.AreEqual(rTest1.HorizontalUnit, hUnit);
            Assert.AreEqual(rTest1.VerticalUnit, vUnit);
            Assert.AreEqual(rTest1.Count, 20);

            // Now let's try one with uneven bins
            Histogram rTest2 = new Histogram(19, 1, -0.1m, 0.1m, LengthUnit.Foot, LengthUnit.Meter);
            Assert.AreEqual(rTest2.BinArea.SquareMeters, (double)(0.1m * 0.1m));
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
            // Values fall to the right into bins: binLeft <= val < binRight
            Assert.AreEqual(rTest1.BinId(Length.From(-10.1,vUnit)), -1);
            Assert.AreEqual(rTest1.BinId(Length.From(-10, vUnit)), 0);
            Assert.AreEqual(rTest1.BinId(Length.From(-9.999, vUnit)), 0);

            Assert.AreEqual(rTest1.BinId(Length.From(-5.1, vUnit)), 4);
            Assert.AreEqual(rTest1.BinId(Length.From(-5, vUnit)), 5);
            Assert.AreEqual(rTest1.BinId(Length.From(-4.9, vUnit)), 5);

            Assert.AreEqual(rTest1.BinId(Length.From(-0.1, vUnit)), 9);
            Assert.AreEqual(rTest1.BinId(Length.From(0, vUnit)), 10);
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
                rTest1.AddBinVal((double)i-9.9);

            decimal CellAreaM2 = (decimal)rTest1.BinArea.SquareMeters;
            // Now test the values
            for (int i = 0; i < 20; i++)
            {
                decimal iVolm = (decimal)Length.From((double)i-9.9, LengthUnit.Foot).Meters;
                Assert.AreEqual((decimal)rTest1.BinVolume(i).CubicMeters, iVolm * CellAreaM2);
                Assert.AreEqual(rTest1.BinCount(i), 1);
            }

        }


        [TestMethod()]
        public void BinValsTest2()
        {
            LengthUnit vUnit = LengthUnit.Foot;
            LengthUnit hUnit = LengthUnit.Meter;
            Histogram rTest1 = new Histogram(4, 1, -0.1m, 0.1m, vUnit, hUnit);

            // Let's get our integers out of the way
            rTest1.AddBinVal(-2);
            Assert.AreEqual(rTest1.BinCount(0), 1);
            rTest1.AddBinVal(-1);
            Assert.AreEqual(rTest1.BinCount(1), 1);
            rTest1.AddBinVal(0);
            Assert.AreEqual(rTest1.BinCount(2), 1);
            rTest1.AddBinVal(1);
            Assert.AreEqual(rTest1.BinCount(3), 1);
            // Make sure the last value falls backward
            rTest1.AddBinVal(2);
            Assert.AreEqual(rTest1.BinCount(3), 2);

            // Now make sure everything that is supposed to fail does.
            List<double> badVals = new List<double>() { 3, -2.0000001, 2.0000011 };
            foreach (double val in badVals)
            {
                try
                {
                    rTest1.AddBinVal(val);
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.IsInstanceOfType(e, typeof(ArgumentOutOfRangeException));
                }
            }

        }

        [TestMethod()]
        public void WriteFileTest()
        {
            Assert.Inconclusive();
        }
    }
}