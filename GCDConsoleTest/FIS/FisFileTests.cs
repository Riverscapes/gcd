using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib.Tests;
using GCDConsoleLib.FIS;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class FisFileTests
    {
        [TestMethod()]
        public void FisFileTest()
        {
            FileInfo fn = new FileInfo(TestHelpers.GetTestRootPath(@"FIS\FuzzyChinookSpawner_03.fis"));
            FisFile test = new FisFile(fn);

            Assert.AreEqual(test.ruleset.Rules.Count, 64);
            Assert.AreEqual(test.ruleset.Inputs.Count, 3);
            Assert.AreEqual(test.ruleset.Inputs["Velocity"]._mfs.Count, 4);

            CollectionAssert.AreEqual(test.ruleset.Inputs["Velocity"]._mfs[0].Coords,
                new List<KeyValuePair<double, double>>() {
                    new KeyValuePair<double,double>(0,0),
                    new KeyValuePair<double,double>(0,1),
                    new KeyValuePair<double,double>(0.08,1),
                    new KeyValuePair<double,double>(0.14,0)
                });
            Assert.AreEqual(test.ruleset.Output._mfs.Count, 4);
            Assert.AreEqual(test.ruleset.OutputName, "HabitatSuitablity");
        }

        [TestMethod()]
        public void RangeSquareBracketsTest()
        {
            List<double> expected1 = new List<double>() { 0, -1, 0.09, 0.17 };
            CollectionAssert.AreEqual(FisFile.RangeSquareBrackets("[0 -1 0.09 0.17]"), expected1);
        }
    }
}