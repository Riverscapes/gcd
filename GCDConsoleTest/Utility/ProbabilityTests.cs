using GCDConsoleLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GCDConsoleLib.Utility.Tests
{
    [TestClass()]
    public class ProbabilityTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void normalDistTest()
        {
            double accuracy = 0.0001;
            // I'm using this to test:
            // http://sphweb.bumc.bu.edu/otlt/mph-modules/bs/bs704_probability/bs704_probability9.html
            // I don't really know what's going on. I'm just trying to make the numbers
            // Match the website

            Assert.AreEqual(Probability.normalDist(double.PositiveInfinity), 1.0, accuracy);
            Assert.AreEqual(Probability.normalDist(double.NegativeInfinity), 0.0, accuracy);

            Assert.AreEqual(Probability.normalDist(0.0), 0.5, accuracy);
            Assert.AreEqual(Probability.normalDist(0.5), 0.691462, accuracy);
            Assert.AreEqual(Probability.normalDist(2.0), 0.977249, accuracy);
            Assert.AreEqual(Probability.normalDist(10.0), 1.0, accuracy);

            Assert.AreEqual(Probability.normalDist(-0.5), 0.30853, accuracy);
            Assert.AreEqual(Probability.normalDist(-2.0), 0.022750, accuracy);
            Assert.AreEqual(Probability.normalDist(-10.0), 7.61985E-24, accuracy);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void ltqnormTest()
        {
            double accuracy = 0.0001;
            // For now, just make sure it doesn't die on the edge cases
            Assert.AreEqual(Probability.ltqnorm(double.PositiveInfinity), 0, accuracy);
            Assert.AreEqual(Probability.ltqnorm(double.NegativeInfinity), 0, accuracy);
            Assert.AreEqual(Probability.ltqnorm(0.0), double.NegativeInfinity, accuracy);
            Assert.AreEqual(Probability.ltqnorm(-1), 0, accuracy);
            Assert.AreEqual(Probability.ltqnorm(1.1), 0, accuracy);

            // There are 3 regions to test: p<LOW, p > High and the middle region
            // Remember: 
            //        static double LOW = 0.02425;
            //        static double HIGH = 0.97575;


            /////////////////////////////////////////////////////////////////////////////
            // NOTE: THESE ARE NOT VERIFIED!!!!! WE'RE JUST MAKING SURE IT DOESN'T DIE!!!
            /////////////////////////////////////////////////////////////////////////////


            // p < LOW Case
            Assert.AreEqual(Probability.ltqnorm(0.01), -2.3263478743880279, accuracy);
            Assert.AreEqual(Probability.ltqnorm(0.02), -2.0537489090030348, accuracy);
            // p between LOW and HIGH
            Assert.AreEqual(Probability.ltqnorm(0.5), 0, accuracy);
            Assert.AreEqual(Probability.ltqnorm(0.6), 0.253347102, accuracy);
            Assert.AreEqual(Probability.ltqnorm(0.2), -0.84162123, accuracy);
            // p > HIGH
            Assert.AreEqual(Probability.ltqnorm(0.976), 1.9773684300754033, accuracy);
            Assert.AreEqual(Probability.ltqnorm(0.999), 3.0902323047094038, accuracy);

        }

    }

}