using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.FIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void addOutputMFSetTest()
        {
            // We don't need to test this. It's too simple.
        }

        [TestMethod()]
        public void addRuleTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void calculateTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void calculateTest1()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void setInputMFSetTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void validTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void getFuzzyValTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ImplicatorOpTest()
        {
            throw new NotImplementedException();
        }
    }
}