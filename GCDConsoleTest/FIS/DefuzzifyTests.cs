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
    public class DefuzzifyTests
    {
        [TestMethod()]
        public void DefuzzCentroidTest()
        {
            // Set our input
            // https://www.mathworks.com/help/fuzzy/examples/defuzzification-methods.html
            MemberFunction inMf = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 3,0},
            });

            double result = Defuzzify.DefuzzCentroid(inMf);

            // Should be the point along the x axis that divides the shape
            // into two equal pieces
            Assert.AreEqual(result, 1.5);
        }

        [TestMethod()]
        public void FISDefuzzBisectTest()
        {
            // Set our input
            MemberFunction inMf = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 3,0},
            });

            double result = Defuzzify.DefuzzBisect(inMf);

            // REsult should be the balancing point along the x axis
            Assert.AreEqual(result, 1.5);
        }

        [TestMethod()]
        public void FISDefuzzMidMaxTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void FISDefuzzLargeMaxTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void FISDefuzzSmallMaxTest()
        {
            Assert.Inconclusive();
        }
    }
}