using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class DefuzzifyTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
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
        [TestCategory("Unit")]
        public void FISDefuzzBisectTest()
        {
            // Test a symmetric shape
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



            // Test a lopsided shape
            MemberFunction inMf2 = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 3,1},
            });

            double result2 = Defuzzify.DefuzzBisect(inMf2);

            // REsult should be the balancing point along the x axis
            Assert.AreEqual(result2, 2);


            // Test a shape where the bisect happens beteen points
            MemberFunction inMf3 = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 4,1},
            });

            double result3 = Defuzzify.DefuzzBisect(inMf3);

            // REsult should be the balancing point along the x axis
            Assert.AreEqual(result3, 2.5);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void FISDefuzzMaxMethodsTest()
        {
            // Test a shape where the bisect happens beteen points
            MemberFunction inMf3 = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,0},
                new double[] { 1,1},
                new double[] { 2,2},
                new double[] { 4,2},
                new double[] { 5,0},
            });

            double LargeMax = Defuzzify.FISDefuzzLargeMax(inMf3);
            double MidMax = Defuzzify.FISDefuzzMidMax(inMf3);
            double SmallMax = Defuzzify.FISDefuzzSmallMax(inMf3);

            // REsult should be the balancing point along the x axis
            Assert.AreEqual(LargeMax, 4);
            Assert.AreEqual(MidMax, 3);
            Assert.AreEqual(SmallMax, 2);
        }


    }
}