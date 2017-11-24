using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.FIS;
using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class RuleSetTests
    {
        [TestMethod()]
        public void RuleSetTest()
        {
            RuleSet rs1 = new RuleSet();
            Assert.IsFalse(rs1.Valid);
            Assert.AreEqual(rs1.Inputs.Count, 0);
            Assert.AreEqual(rs1.InputLookupMap.Count, 0);
            Assert.AreEqual(rs1.Outputs.Count, 0);
            Assert.AreEqual(rs1.Rules.Count, 0);
            Assert.AreEqual(rs1.OutputName, "");

            RuleSet rs2 = new RuleSet(
                FISOperatorAnd.FISOpAnd_Min,
                FISOperatorOr.FISOpOr_Max,
                FISImplicator.FISImp_Product, 
                FISAggregator.FISAgg_Probor,
                FISDefuzzifier.FISDefuzz_MidMax);

            Assert.AreEqual(rs2.Inputs.Count, 0);
            Assert.IsFalse(rs1.Valid);
            Assert.AreEqual(rs2.InputLookupMap.Count, 0);
            Assert.AreEqual(rs2.Outputs.Count, 0);
            Assert.AreEqual(rs2.Rules.Count, 0);
            Assert.AreEqual(rs2.OutputName, "");
        }


        [TestMethod()]
        public void addInputMFSetTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void addOutputMFSetTest()
        {
            // We don't need to test this. It's too simple.
        }

        [TestMethod()]
        public void addRuleTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void calculateTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void calculateTest1()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void setInputMFSetTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void validTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void getFuzzyValTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ImplicatorOpTest()
        {
            Assert.Inconclusive();
        }
    }
}