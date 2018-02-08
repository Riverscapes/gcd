using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UnitsNet;
using UnitsNet.Units;
using GCDConsoleLib.GCD;
using System.IO;
using GCDConsoleTest.Helpers;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class HistogramTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void HistogramTest()
        {
            Histogram rTest1 = new Histogram(20, 1);
            UnitGroup ug = new UnitGroup(VolumeUnit.ImperialBeerBarrel, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);

            Assert.AreEqual(rTest1.FirstBinId, 0);
            Assert.AreEqual(rTest1.LastBinId, 19);
            Assert.AreEqual(rTest1.HistogramLower(ug).As(ug.VertUnit), -10.0);
            Assert.AreEqual(rTest1.HistogramUpper(ug).As(ug.VertUnit), 10.0);
            Assert.AreEqual(rTest1.Count, 20);

            // Now let's try one with uneven bins
            Histogram rTest2 = new Histogram(19, 1);
            Assert.AreEqual(rTest2.FirstBinId, 0);
            Assert.AreEqual(rTest2.LastBinId, 19);
            Assert.AreEqual(rTest2.HistogramLower(ug).As(ug.VertUnit), -10.0);
            Assert.AreEqual(rTest2.HistogramUpper(ug).As(ug.VertUnit), 10.0);
            Assert.AreEqual(rTest2.Count, 20);

            // Now let's try a zero width histogram
            Histogram rTest3 = new Histogram(1, decimal.MaxValue);
            Assert.AreEqual(rTest3.FirstBinId, 0);
            Assert.AreEqual(rTest3.LastBinId, 1);
            Assert.AreEqual(rTest3.HistogramLower(ug).As(ug.VertUnit), (double)decimal.MinValue);
            Assert.AreEqual(rTest3.HistogramUpper(ug).As(ug.VertUnit), (double)decimal.MaxValue);
            Assert.AreEqual(rTest3.Count, 2);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void BinIdTest()
        {
            Histogram rTest1 = new Histogram(20, 1);
            // Values fall to the right into bins: binLeft <= val < binRight
            Assert.AreEqual(rTest1.BinId(-10.1), -1);
            Assert.AreEqual(rTest1.BinId(-10), 0);
            Assert.AreEqual(rTest1.BinId(-9.999), 0);

            Assert.AreEqual(rTest1.BinId(-5.1), 4);
            Assert.AreEqual(rTest1.BinId(-5), 5);
            Assert.AreEqual(rTest1.BinId(-4.9), 5);

            Assert.AreEqual(rTest1.BinId(-0.1), 9);
            Assert.AreEqual(rTest1.BinId(0), 10);
            Assert.AreEqual(rTest1.BinId(0.1), 10);

            Assert.AreEqual(rTest1.BinId(10.1), -1);
            Assert.AreEqual(rTest1.BinId(10), 19);
            Assert.AreEqual(rTest1.BinId(9.999), 19);

            // Special case
            Histogram rTest2 = new Histogram(1, decimal.MaxValue);

            Assert.AreEqual(rTest2.BinId(10.1), 1);
            Assert.AreEqual(rTest2.BinId(100000.1), 1);
            Assert.AreEqual(rTest2.BinId(-10.1), 0);
            Assert.AreEqual(rTest2.BinId(-100000.1), 0);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void BinLowerTest()
        {
            UnitGroup ug = new UnitGroup(VolumeUnit.ImperialBeerBarrel, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);

            Histogram rTest1 = new Histogram(20, 1);
            Assert.AreEqual(rTest1.BinLower(0, ug).As(ug.VertUnit), -10);
            Assert.AreEqual(rTest1.BinLower(19, ug).As(ug.VertUnit), 9);

            Histogram rTest2 = new Histogram(1, decimal.MaxValue);
            Assert.AreEqual(rTest2.BinLower(0, ug).As(ug.VertUnit), (double)decimal.MinValue);
            Assert.AreEqual(rTest2.BinLower(1, ug).As(ug.VertUnit), 0);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void BinUpperTest()
        {
            UnitGroup ug = new UnitGroup(VolumeUnit.ImperialBeerBarrel, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);

            Histogram rTest1 = new Histogram(20, 1);
            Assert.AreEqual(rTest1.BinUpper(0, ug).As(ug.VertUnit), -9);
            Assert.AreEqual(rTest1.BinUpper(19, ug).As(ug.VertUnit), 10);

            Histogram rTest2 = new Histogram(1, decimal.MaxValue);
            Assert.AreEqual(rTest2.BinUpper(0, ug).As(ug.VertUnit), 0);
            Assert.AreEqual(rTest2.BinUpper(1, ug).As(ug.VertUnit), (double)decimal.MaxValue);
        }


        [TestMethod()]
        [TestCategory("Unit")]
        public void BinCentreTest()
        {
            UnitGroup ug = new UnitGroup(VolumeUnit.ImperialBeerBarrel, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);

            Histogram rTest1 = new Histogram(20, 1);
            Assert.AreEqual(rTest1.BinCentre(0, ug).As(ug.VertUnit), -9.5);
            Assert.AreEqual(rTest1.BinCentre(19, ug).As(ug.VertUnit), 9.5);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void BinValsTest()
        {
            Histogram rTest1 = new Histogram(20, 1);

            //Add some fake values into the mix
            for (int i = 0; i < 20; i++)
                rTest1.AddBinVal((double)i - 9.9);

            // Now test the values
            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(rTest1.BinCount(i), 1);
            }

            // The special case
            Histogram rTest2 = new Histogram(1, decimal.MaxValue);
            //Add some fake values into the mix
            rTest2.AddBinVal(-1);
            rTest2.AddBinVal(1);
            rTest2.AddBinVal(1000);

            Assert.AreEqual(rTest2.BinCount(0), 1);
            Assert.AreEqual(rTest2.BinCount(1), 2);

        }


        [TestMethod()]
        [TestCategory("Unit")]
        public void BinValsTest2()
        {
            Histogram rTest1 = new Histogram(4, 1);

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
        [TestCategory("Unit")]
        public void GetNearestFiveOrderWidthTest()
        {
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.1m), 0.1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.11m), 0.1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.2m), 0.1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.1000000001m), 0.1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.0900000000m), 0.1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.0800000000m), 0.1m);

            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.5m), 0.5m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.49999999m), 0.5m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.500000001m), 0.5m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.432342352352m), 0.5m);

            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(1.49m), 1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(1.0m), 1m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(1.234523451m), 1.0m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(0.756m), 1.0m);

            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(50.500234234m), 50m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(64.500234234m), 50m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(80), 100m);

            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(120), 100m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(124), 100m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(125), 100m);

            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(250), 100m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(300), 500m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(500), 500m);
            Assert.AreEqual(Histogram.GetNearestFiveOrderWidth(749), 500m);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void GetCleanBinsTest()
        {
            Tuple<int, decimal> t1 = Histogram.GetCleanBins(10, 1, -1);
            Assert.AreEqual(t1.Item1, 20);
            Assert.AreEqual(t1.Item2, 0.1m);

            Tuple<int, decimal> t2 = Histogram.GetCleanBins(10, 1.1m, -1);
            Assert.AreEqual(t2.Item1, 22);
            Assert.AreEqual(t2.Item2, 0.1m);

            Tuple<int, decimal> t3 = Histogram.GetCleanBins(9, 1.1m, -1);
            Assert.AreEqual(t3.Item1, 22);
            Assert.AreEqual(t3.Item2, 0.1m);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void BinVolumeTest()
        {
            Histogram rTest1 = new Histogram(20, 1);

            //Add some fake values into the mix
            for (int i = 0; i < 20; i++)
                rTest1.AddBinVal((double)i - 9.9);

            // Make sure our unit conversions are working
            decimal cH = -0.2m; //Foots
            decimal cW = 0.3m;  //Foots

            UnitGroup ug = new UnitGroup(VolumeUnit.ImperialBeerBarrel, AreaUnit.SquareInch, LengthUnit.Foot, LengthUnit.Meter);
            Area cellArea = Area.From((double)Math.Abs(cH * cW), AreaUnit.SquareFoot);

            for (int i = 0; i < 20; i++)
            {
                // Length(m) X Width(m) X Height(ft)
                double manualvolume = Volume.From(cellArea.SquareMeters * Length.FromFeet(i - 9.9).Meters, VolumeUnit.CubicMeter).ImperialBeerBarrels;
                Assert.AreEqual(rTest1.BinVolume(i, cellArea, ug).As(ug.VolUnit), manualvolume, 0.0000001);
            }

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void ReadWriteFileTest()
        {
            // Write to a file then read it to see if we get the same thing
            Histogram rTest1 = new Histogram(20, 1);

            //Add some fake values into the mix
            for (int i = 0; i < 20; i++)
                rTest1.AddBinVal(i - 9.9);

            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
                Area cellArea = Area.From(0.2 * 0.3, ug.ArUnit);
                FileInfo fPath = new FileInfo(Path.Combine(tmp.Name, "myHistogram.csv"));
                rTest1.WriteFile(fPath, cellArea, ug);
                Histogram rTestRead = new Histogram(fPath);

                // Make sure the two histograms have the same edges and width
                Assert.AreEqual(rTest1.Count, rTest1.Count);
                Assert.AreEqual(rTest1.HistogramLower(ug).Meters, rTest1.HistogramLower(ug).Meters);
                Assert.AreEqual(rTest1.HistogramUpper(ug).Meters, rTest1.HistogramUpper(ug).Meters);
                Assert.AreEqual(rTest1._binWidth, rTest1._binWidth);

                // Now go bin-by-bin to make sure we end up with the same numbers everywhere
                for (int bid = 0; bid < rTestRead.Count; bid++)
                {
                    Assert.AreEqual(rTest1.BinCounts[bid], rTestRead.BinCounts[bid]);
                    Assert.AreEqual(rTest1.BinArea(bid, cellArea).SquareMeters, rTestRead.BinArea(bid, cellArea).SquareMeters);
                    Assert.AreEqual(rTest1.BinLower(bid, ug).Meters, rTestRead.BinLower(bid, ug).Meters);
                }

            }
        }


        [TestMethod()]
        [TestCategory("Unit")]
        public void ReadWriteFileSpecialCaseTest()
        {
            // Write to a file then read it to see if we get the same thing
            Histogram rTest1 = new Histogram(1, decimal.MaxValue);

            //Add some fake values into the mix
            rTest1.AddBinVal(-5);
            rTest1.AddBinVal(7.123);

            // First try it with a real file
            using (ITempDir tmp = TempDir.Create())
            {
                UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
                Area cellArea = Area.From(0.2 * 0.3, ug.ArUnit);
                FileInfo fPath = new FileInfo(Path.Combine(tmp.Name, "myHistogram.csv"));
                rTest1.WriteFile(fPath, cellArea, ug);
                Histogram rTestRead = new Histogram(fPath);

                // Make sure the two histograms have the same edges and width
                Assert.AreEqual(rTest1.Count, rTest1.Count);
                Assert.AreEqual(rTest1.HistogramLower(ug).Meters, rTest1.HistogramLower(ug).Meters);
                Assert.AreEqual(rTest1.HistogramUpper(ug).Meters, rTest1.HistogramUpper(ug).Meters);
                Assert.AreEqual(rTest1._binWidth, rTest1._binWidth);

                // Now go bin-by-bin to make sure we end up with the same numbers everywhere
                for (int bid = 0; bid < rTestRead.Count; bid++)
                {
                    Assert.AreEqual(rTest1.BinCounts[bid], rTestRead.BinCounts[bid]);
                    Assert.AreEqual(rTest1.BinArea(bid, cellArea).SquareMeters, rTestRead.BinArea(bid, cellArea).SquareMeters);
                    Assert.AreEqual(rTest1.BinLower(bid, ug).Meters, rTestRead.BinLower(bid, ug).Meters);
                }

            }
        }
        [TestMethod()]
        [TestCategory("Unit")]
        public void Histogram_Uniform_Raster_Test()
        {
            Raster rTempl = new Raster(new FileInfo(DirHelpers.GetTestRasterPath("const950.tif")));
            Histogram theHisto = new Histogram(1, rTempl);
            theHisto.AddBinVal(1.0);
        }

    }

}