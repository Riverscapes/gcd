using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.FIS;
using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class RuleTests
    {
        [TestMethod()]
        public void RuleTest()
        {
            Rule testRule = new Rule();
            Assert.AreEqual(testRule.Inputs.Count, 0);
            Assert.AreEqual(testRule.MFSInd.Count, 0);
            Assert.AreEqual(testRule.MFSNot.Count, 0);
            Assert.AreEqual(testRule.Weight, 1.0);
        }

        [TestMethod()]
        public void addMfTest()
        {
            Rule testRule = new Rule();
            testRule.addMf(1, 2);
            testRule.addMf(2, -2);

            Assert.AreEqual(testRule.Inputs[0], 1);
            Assert.AreEqual(testRule.Inputs[1], 2);

            Assert.AreEqual(testRule.MFSInd[0], 1);
            Assert.AreEqual(testRule.MFSInd[1], 1);

            // The MFSNot is just a list with boolean flags in it
            Assert.AreEqual(testRule.MFSNot[0], false);
            Assert.AreEqual(testRule.MFSNot[1], true);

        }
    }
}