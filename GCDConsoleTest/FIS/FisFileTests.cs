﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using GCDConsoleTest.Helpers;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class FisFileTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void FisFileTest()
        {
            FileInfo fn = new FileInfo(DirHelpers.GetTestRootPath(@"FIS\FuzzyChinookSpawner_03.fis"));
            FisFile test = new FisFile(fn);

            Assert.AreEqual(test.ruleset.Rules.Count, 64);
            Assert.AreEqual(test.ruleset.Inputs.Count, 3);
            Assert.AreEqual(test.ruleset.Inputs["Velocity"].MFunctions.Count, 4);

            List<double[]> expected = new List<double[]>
                {
                    new double[] { 0, 0 },
                    new double[] { 0, 1 },
                    new double[] {0.08, 1 },
                    new double[] { 0.14, 0 }
                };

            // Test the values
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(test.ruleset.Inputs["Velocity"].MFunctions[0].Coords[i][0], expected[i][0]);
                Assert.AreEqual(test.ruleset.Inputs["Velocity"].MFunctions[0].Coords[i][1], expected[i][1]);
            }

            Assert.AreEqual(test.ruleset.Outputs.MFunctions.Count, 4);
            Assert.AreEqual(test.ruleset.OutputName, "HabitatSuitablity");
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void RangeSquareBracketsTest()
        {
            List<double> expected1 = new List<double>() { 0, -1, 0.09, 0.17 };
            CollectionAssert.AreEqual(FisFile.RangeSquareBrackets("[0 -1 0.09 0.17]"), expected1);
        }
    }
}